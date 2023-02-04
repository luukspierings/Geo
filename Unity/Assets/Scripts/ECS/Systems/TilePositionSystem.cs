using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class TilePositionSystem : SystemBase
{
	private EntityQuery _entityQuery;

	protected override void OnCreate()
	{
		_entityQuery = GetEntityQuery(typeof(MapPositionComponent));
	}

	protected override void OnUpdate()
	{
		var mapPositionComponents = _entityQuery
			.ToComponentDataArray<MapPositionComponent>(Allocator.TempJob);

		Entities
			.ForEach((
				ref TileComponent tileComponent) =>
			{
				var mapPosition = mapPositionComponents[0];

				tileComponent.LocalX = tileComponent.X - mapPosition.TileX;
				tileComponent.LocalY = tileComponent.Y - mapPosition.TileY;
			})
			.WithDisposeOnCompletion(mapPositionComponents)
			.WithoutBurst()
			.Schedule();
	}
}
