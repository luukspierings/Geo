using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public class GameobjectLifecycleSystem : SystemBase
{
	private EntityCommandBufferSystem _commandBufferSystem;

	protected override void OnCreate()
	{
		_commandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
	}

	protected override void OnUpdate()
	{
		var commandBuffer = _commandBufferSystem.CreateCommandBuffer();

		Entities
			.WithAll<GameobjectComponent, Translation, LocalToWorld>()
			.ForEach((
				Entity entity,
				ref GameobjectComponent gameobjectComponent,
				in Translation translation,
				in LocalToWorld localToWorld) =>
			{
				if (gameobjectComponent.Spawned)
					return;

				var id = GameobjectManager.Instance.Queue(new GameobjectQueueItem()
				{
					Type = gameobjectComponent.Type,
					Position = localToWorld.Position,
					Rotation = localToWorld.Rotation
				});

				commandBuffer.AddComponent(entity, new GameobjectStateComponent()
				{
					Id = id
				});

				gameobjectComponent.Spawned = true;
			})
			.WithoutBurst()
			.Schedule();


		Entities
			.WithAll<GameobjectStateComponent>()
			.WithNone<GameobjectComponent>()
			.ForEach((Entity entity, in GameobjectStateComponent gameobjectStateComponent) =>
			{
				GameobjectManager.Instance.Destroy(gameobjectStateComponent.Id);

				commandBuffer.RemoveComponent<GameobjectStateComponent>(entity);
			})
			.WithoutBurst()
			.Schedule();

		_commandBufferSystem.AddJobHandleForProducer(Dependency);
	}

}
