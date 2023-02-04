using Assets.Scripts.Calculators;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class CameraTransitionSystem : SystemBase
{
	private EntityQuery _tileComponentQuery;
	private EntityQuery _cameraFocusQuery;
	private EntityCommandBufferSystem _commandBufferSystem;

	protected override void OnCreate()
	{
		_tileComponentQuery = GetEntityQuery(typeof(TileComponent), typeof(Translation));
		_cameraFocusQuery = GetEntityQuery(typeof(CameraFocusComponent), typeof(Translation));
		_commandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
	}

	protected override void OnUpdate()
	{
		var shouldTransition = EventManager.Instance.EventIsActive(Assets.Scripts.Configuration.EventType.Build);
		if (!shouldTransition)
			return;
		
		var commandBuffer = _commandBufferSystem.CreateCommandBuffer();
		var tileComponents = _tileComponentQuery.ToComponentDataArray<TileComponent>(Allocator.TempJob);
		var tileTranslations = _tileComponentQuery.ToComponentDataArray<Translation>(Allocator.TempJob);
		var cameraFocuses = _cameraFocusQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

		Entities
			.ForEach((
				Entity e,
				ref CameraComponent cameraComponent,
				in Translation translation,
				in Rotation rotation) =>
			{
				var newCameraMode = cameraComponent.CameraModeType switch
				{
					CameraModeType.Following => CameraModeType.BirdsEye,
					CameraModeType.BirdsEye => CameraModeType.Following,
					_ => CameraModeType.Following
				};

				(quaternion toRotation, float3 toPosition) = newCameraMode switch
				{
					CameraModeType.Following => CalculateFollowing(
							cameraComponent,
							cameraFocuses[0]
						),
					CameraModeType.BirdsEye => CalculateBirdsEye(
							ref tileComponents,
							ref tileTranslations,
							ref cameraComponent
						),
					_ => (quaternion.identity, float3.zero)
				};

				commandBuffer.AddComponent(e, new TransitioningComponent()
				{
					FromPosition = translation.Value,
					ToPosition = toPosition,
					FromRotation = rotation.Value,
					ToRotation = toRotation,
					TransitionSeconds = 1
				});
				cameraComponent.CameraModeType = newCameraMode;
			})
			.WithDisposeOnCompletion(tileComponents)
			.WithDisposeOnCompletion(tileTranslations)
			.WithDisposeOnCompletion(cameraFocuses)
			.WithoutBurst()
			.Schedule();

		_commandBufferSystem.AddJobHandleForProducer(Dependency);
	}

	private static (quaternion, float3) CalculateFollowing(
		CameraComponent cameraComponent,
		Translation focus)
	{
		return ViewingCalculator.CalculateLookat(
			cameraComponent.XRotation,
			cameraComponent.YRotation,
			focus.Value,
			cameraComponent.Radius
		);
	}

	private static (quaternion, float3) CalculateBirdsEye(
		ref NativeArray<TileComponent> tileComponents,
		ref NativeArray<Translation> tileTranslations,
		ref CameraComponent cameraComponent)
	{
		float3 centerPosition = default;

		for (int i = 0; i < tileComponents.Length; i++)
		{
			var tileComponent = tileComponents[i];
			if (tileComponent.LocalX != 0 || tileComponent.LocalY != 0)
				continue;

			Debug.Log($"Found center: {tileComponent.X}, {tileComponent.Y}");

			centerPosition = tileTranslations[i].Value;
		}

		var lookat = ViewingCalculator.CalculateLookat(
			cameraComponent.BirdsEyeRotation,
			0f,
			centerPosition,
			cameraComponent.MaximumRadius
		);

		lookat.Item2 -= new float3(0, 0, 20);

		return lookat;
	}



}
