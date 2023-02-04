using Assets.Scripts.Calculators;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class CameraSystem : SystemBase
{
	private EntityQuery _cameraFocusQuery;

	protected override void OnCreate()
	{
		_cameraFocusQuery = GetEntityQuery(typeof(CameraFocusComponent), typeof(Translation));
	}

	protected override void OnUpdate()
	{
		var touchDeltaPosition = InputHelper.GetDeltaPosition();
		var deltaZoom = InputHelper.GetDeltaZoom();

		var cameraFocuses = _cameraFocusQuery
			.ToComponentDataArray<Translation>(Allocator.TempJob);

		Entities
			.WithNone<TransitioningComponent>()
			.ForEach((
				ref Translation translation,
				ref Rotation rotation,
				ref CameraComponent cameraComponent) =>
			{
				if (cameraComponent.CameraModeType == CameraModeType.Unknown)
					return;
			
				(quaternion newRotation, float3 newPosition) = cameraComponent.CameraModeType switch
				{
					CameraModeType.Following => CalculateFollowing(
							touchDeltaPosition,
							deltaZoom,
							cameraFocuses[0],
							ref cameraComponent
						),
					CameraModeType.BirdsEye => CalculateBirdsEye(
							touchDeltaPosition,
							deltaZoom,
							rotation,
							translation,
							ref cameraComponent
						),
					_ => (quaternion.identity, float3.zero)
				};

				rotation.Value = newRotation;
				translation.Value = newPosition;
			})
			.WithDisposeOnCompletion(cameraFocuses)
			.WithoutBurst()
			.Schedule();
	}

	private static (quaternion, float3) CalculateFollowing(
		Vector2 touchDeltaPosition,
		float deltaZoom,
		Translation focus,
		ref CameraComponent cameraComponent)
	{
		cameraComponent.Radius = math.clamp(
			cameraComponent.Radius - deltaZoom,
			cameraComponent.MinimumRadius,
			cameraComponent.MaximumRadius
		);

		cameraComponent.YRotation += (touchDeltaPosition.x * (cameraComponent.MovementSpeed * 0.01f));
		cameraComponent.XRotation = math.clamp(
			cameraComponent.XRotation - (touchDeltaPosition.y * (cameraComponent.MovementSpeed * 0.01f)),
			cameraComponent.MinimumXRotation,
			cameraComponent.MaximumXRotation
		);

		return ViewingCalculator.CalculateLookat(
			cameraComponent.XRotation,
			cameraComponent.YRotation,
			focus.Value,
			cameraComponent.Radius
		);
	}

	private static (quaternion, float3) CalculateBirdsEye(
		Vector2 touchDeltaPosition,
		float deltaZoom,
		Rotation rotation,
		Translation translation,
		ref CameraComponent cameraComponent)
	{
		var magnitude = cameraComponent.MovementSpeed * 0.01f;
		var newTranslation = translation.Value - new float3(touchDeltaPosition.x * magnitude, 0, touchDeltaPosition.y * magnitude);

		var lookDirection = math.mul(rotation.Value, new float3(0, 0, 1));

		newTranslation += lookDirection * deltaZoom * cameraComponent.ZoomSpeed * 0.1f;

		return (rotation.Value, newTranslation);
	}


}
