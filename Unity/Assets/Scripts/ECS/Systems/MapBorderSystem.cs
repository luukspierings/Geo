using Assets.Scripts.Calculators;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MapBorderSystem : SystemBase
{
	private EntityQuery _entityQuery;

	protected override void OnCreate()
	{
		_entityQuery = GetEntityQuery(typeof(MapPositionComponent));
	}

	protected override void OnStartRunning()
	{
		var entity = PrefabRegistration.GetEntity(PrefabIdentity.Border_Cloud);

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if (x == 0 && y == 0)
					continue;

				var cloud = EntityManager.Instantiate(entity);
				EntityManager.AddComponentData(cloud, new BorderComponent()
				{
					X = x,
					Y = y
				});
			}
		}
	}

	protected override void OnUpdate()
	{
		using var mapPositionComponents = _entityQuery
			.ToComponentDataArray<MapPositionComponent>(Allocator.TempJob);

		var mapPositionComponent = mapPositionComponents[0];

		if (!mapPositionComponent.ChangedPosition)
			return;

		var magnitude = mapPositionComponent.TileSize * (mapPositionComponent.RenderDistance * 2 + 1);
		var xOffset = (mapPositionComponent.TileX - mapPositionComponent.OriginX) * mapPositionComponent.TileSize;
		var yOffset = (mapPositionComponent.TileY - mapPositionComponent.OriginY) * mapPositionComponent.TileSize;

		Entities
			.ForEach((ref Translation translation, in BorderComponent borderComponent) =>
			{
				translation.Value = new float3(borderComponent.X * magnitude + xOffset, 0, borderComponent.Y * magnitude + yOffset);
			})
			.WithoutBurst()
			.Schedule();
	}
}
