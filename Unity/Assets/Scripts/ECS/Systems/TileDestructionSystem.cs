using Assets.Scripts.Calculators;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class TileDestructionSystem : SystemBase
{
	private EntityQuery _entityQuery;
	private EntityCommandBufferSystem _commandBufferSystem;

	protected override void OnCreate()
	{
		_entityQuery = GetEntityQuery(typeof(TileComponent));
		_commandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
	}

	protected override void OnUpdate()
	{
		var tileEntities = _entityQuery
			.ToEntityArray(Allocator.TempJob);

		var commandBuffer = _commandBufferSystem.CreateCommandBuffer();

		Entities
			.ForEach((ref MapPositionComponent mapPosition) =>
			{
				if (!mapPosition.ChangedPosition)
					return;

				var currentX = mapPosition.TileX;
				var currentY = mapPosition.TileY;

				var renderSize = mapPosition.RenderDistance;

				var minX = currentX - renderSize;
				var maxX = currentX + renderSize;
				var minY = currentY - renderSize;
				var maxY = currentY + renderSize;

				for (int i = 0; i < tileEntities.Length; i++)
				{
					var tileComponent = GetComponent<TileComponent>(tileEntities[i]);

					if (PositionHelper.IsWithinOrEqualTo(tileComponent.X, minX, maxX)
						&& PositionHelper.IsWithinOrEqualTo(tileComponent.Y, minY, maxY))
						continue;

					//Debug.Log($"Destroying tile: {tileComponent.X}, {tileComponent.Y} ({minX}, {maxX}, {minY}, {maxY})");

					var childBuffer = GetBuffer<Child>(tileEntities[i]);
					foreach (var child in childBuffer)
						commandBuffer.DestroyEntity(child.Value);

					commandBuffer.DestroyEntity(tileEntities[i]);

					TileConstructionSystem.TileDestroyed(tileComponent.Id);
				}
			})
			.WithDisposeOnCompletion(tileEntities)
			.WithoutBurst()
			.Schedule();

		_commandBufferSystem.AddJobHandleForProducer(Dependency);
	}


}
