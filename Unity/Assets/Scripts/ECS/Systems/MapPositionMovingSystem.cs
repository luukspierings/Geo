using Unity.Entities;
using UnityEngine;

public class MapPositionMovingSystem : SystemBase
{

	protected override void OnUpdate()
	{
		var leftKeyDown = Input.GetKey(KeyCode.LeftArrow);
		var upKeyDown = Input.GetKey(KeyCode.UpArrow);
		var rightKeyDown = Input.GetKey(KeyCode.RightArrow);
		var downKeyDown = Input.GetKey(KeyCode.DownArrow);

		var deltaTime = Time.DeltaTime;
		var speed = 0.0000005d;

		Entities
			.ForEach((ref MapPositionComponent mapPositionComponent) =>
			{
				if (leftKeyDown) mapPositionComponent.Longitude -= speed;
				if (rightKeyDown) mapPositionComponent.Longitude += speed;
				if (upKeyDown) mapPositionComponent.Latitude += speed;
				if (downKeyDown) mapPositionComponent.Latitude -= speed;
			})
			.WithoutBurst()
			.Schedule();
	}
}
