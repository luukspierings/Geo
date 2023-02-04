using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class TileConstructionSystem : SystemBase
{
	private static Dictionary<int, bool> _tilesRendered;
	private EntityCommandBufferSystem _entityCommandBufferSystem;

	public TileConstructionSystem()
	{
		_tilesRendered = new Dictionary<int, bool>();
	}
	

	protected override void OnCreate()
	{
		_entityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
	}

	protected override void OnUpdate()
	{
		var commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

		Entities
			.ForEach((ref MapPositionComponent mapPosition) =>
			{
				if (!mapPosition.ChangedPosition && !mapPosition.WaitingForMap)
					return;

				var currentX = mapPosition.TileX;
				var currentY = mapPosition.TileY;

				var renderSize = mapPosition.RenderDistance;
				var tileSize = mapPosition.TileSize;

				if (mapPosition.OriginX == 0)
					mapPosition.OriginX = currentX;
				if (mapPosition.OriginY == 0)
					mapPosition.OriginY = currentY;

				var minX = currentX - renderSize;
				var maxX = currentX + renderSize;
				var minY = currentY - renderSize;
				var maxY = currentY + renderSize;

				var waitingForMap = false;

				for (int x = minX; x <= maxX; x++)
				{
					for (int y = minY; y <= maxY; y++)
					{
						var tile = MapManager.Instance.GetTile(x, y);
						if (tile == null)
						{
							waitingForMap = true;
							continue;
						}

						var renderedTile = _tilesRendered.TryGetValue(tile.Id, out bool rendered) && rendered;
						if (renderedTile)
							continue;

						var xOffset = x - mapPosition.OriginX;
						var zOffset = y - mapPosition.OriginY;

						var xPosition = xOffset * tileSize;
						var zPosition = zOffset * tileSize;

						var prefabIdentity = tile.ResourceId.HasValue
							? PrefabIdentity.Tile_Forest
							: PrefabIdentity.Tile_Grass;
						var entity = PrefabRegistration.GetEntity(prefabIdentity);

						commandBuffer.AddComponent(entity, new Translation()
						{
							Value = new float3(xPosition, 0, zPosition)
						});
						commandBuffer.SetComponent(entity, new TileComponent()
						{
							Id = tile.Id,
							X = x,
							Y = y,
							PrefabIdentity = prefabIdentity
						});
						commandBuffer.Instantiate(entity);

						_tilesRendered[tile.Id] = true;
					}
				}

				mapPosition.WaitingForMap = waitingForMap;
			})
			.WithoutBurst()
			.Schedule();

		_entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
	}

	public static void TileDestroyed(int tileId)
	{
		_tilesRendered[tileId] = false;
	}

}
