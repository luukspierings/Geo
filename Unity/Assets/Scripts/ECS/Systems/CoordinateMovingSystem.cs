using Assets.Scripts.Calculators;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class CoordinateMovingSystem : SystemBase
{

	protected override void OnUpdate()
	{
		Entities
			.ForEach((ref Translation translation, ref MapPositionComponent mapPosition) =>
			{
				var position = CoordinateCalculator.CalculatePosition(mapPosition.Longitude, mapPosition.Latitude);
				if (position == null)
					return;

				var x = ((position.X - mapPosition.OriginX) * 20) - 10;
				var y = ((position.Y - mapPosition.OriginY) * 20) - 10;

				translation.Value = new float3((float)x, translation.Value.y, (float)y);

				var newTileX = (int)position.X;
				var newTileY = (int)position.Y;

				mapPosition.ChangedPosition = newTileX != mapPosition.TileX || newTileY != mapPosition.TileY;

				mapPosition.TileX = newTileX;
				mapPosition.TileY = newTileY;

				var chunkPosition = ChunkPositionCalculator.CalculatePosition(newTileX, newTileY);

				mapPosition.ChunkX = chunkPosition.X;
				mapPosition.ChunkY = chunkPosition.Y;

			})
			.WithoutBurst()
			.Schedule();
	}
}

