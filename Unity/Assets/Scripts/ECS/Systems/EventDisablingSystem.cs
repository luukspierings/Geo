using Unity.Entities;
using UnityEngine;

public class EventDisablingSystem : SystemBase
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
			.WithNone<Disabled>()
			.ForEach((
				Entity e,
				in DisableOnEventComponent disableOnEventComponent) =>
			{
				if (EventManager.Instance.EventIsActive(disableOnEventComponent.DisableEvent))
				{
					commandBuffer.AddComponent(e, new Disabled());
				}
			})
			.WithoutBurst()
			.Schedule();

		Entities
			.WithAll<Disabled, DisableOnEventComponent>()
			.ForEach((
				Entity e,
				in DisableOnEventComponent disableOnEventComponent) =>
			{
				if (EventManager.Instance.EventIsActive(disableOnEventComponent.EnableEvent))
				{
					commandBuffer.RemoveComponent<Disabled>(e);
				}
			})
			.WithoutBurst()
			.Schedule();

		_commandBufferSystem.AddJobHandleForProducer(Dependency);
	}
}
