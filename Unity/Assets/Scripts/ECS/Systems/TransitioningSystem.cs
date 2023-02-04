using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class TransitioningSystem : SystemBase
{
	private EntityCommandBufferSystem _commandBufferSystem;

	protected override void OnCreate()
	{
		_commandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
	}

	protected override void OnUpdate()
	{
		var commandBuffer = _commandBufferSystem.CreateCommandBuffer();
		var deltaTime = Time.DeltaTime;

		Entities
			.ForEach((
				Entity e,
				ref Translation translation, 
				ref Rotation rotation, 
				ref TransitioningComponent transitioningComponent) =>
			{
				if (transitioningComponent.TransitionPercentage > 1f)
				{
					commandBuffer.RemoveComponent<TransitioningComponent>(e);
					return;
				}

				transitioningComponent.TransitionPercentage += deltaTime / transitioningComponent.TransitionSeconds;

				var interpolatedPosition = math.lerp(
					transitioningComponent.FromPosition, 
					transitioningComponent.ToPosition,
					 math.smoothstep(0.0f, 1.0f, transitioningComponent.TransitionPercentage)
				);
				var interpolatedRotation = math.lerp(
					transitioningComponent.FromRotation.value,
					transitioningComponent.ToRotation.value,
					 math.smoothstep(0.0f, 1.0f, transitioningComponent.TransitionPercentage)
				);

				translation.Value = interpolatedPosition;
				rotation.Value = interpolatedRotation;
			})
			.WithoutBurst()
			.Schedule();

		_commandBufferSystem.AddJobHandleForProducer(Dependency);
	}
}
