using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[DisableAutoCreation]
public class ArrowInputCameraSystem : SystemBase
{
	private EntityQuery _entityQuery;
	protected override void OnCreate()
	{
		_entityQuery = GetEntityQuery(typeof(CameraFocusComponent), typeof(Translation));
	}
	protected override void OnUpdate()
	{
		var leftKeyDown = Input.GetKey(KeyCode.A);
		var upKeyDown = Input.GetKey(KeyCode.W);
		var rightKeyDown = Input.GetKey(KeyCode.D);
		var downKeyDown = Input.GetKey(KeyCode.S);

		var deltaTime = Time.DeltaTime;

		var cameraFocuses = _entityQuery
			.ToComponentDataArray<Translation>(Allocator.TempJob);

		Entities
			.ForEach((ref Translation translation, ref Rotation rotation, ref CameraComponent cameraComponent) =>
			{
				var focus = cameraFocuses[0];

				if (leftKeyDown) cameraComponent.YRotation += deltaTime;
				if (rightKeyDown) cameraComponent.YRotation -= deltaTime;
				if (upKeyDown) cameraComponent.XRotation += deltaTime;
				if (downKeyDown) cameraComponent.XRotation -= deltaTime;

				var lookRotation = quaternion.Euler(new float3(cameraComponent.XRotation, cameraComponent.YRotation, 0));
				var lookDirection = math.mul(lookRotation, new float3(0, 0, 1));
				var lookPosition = focus.Value - (lookDirection * cameraComponent.Radius);

				rotation.Value = lookRotation;
				translation.Value = lookPosition;
			})
			.WithDisposeOnCompletion(cameraFocuses)
			.WithoutBurst()
			.Schedule();
	}
}
