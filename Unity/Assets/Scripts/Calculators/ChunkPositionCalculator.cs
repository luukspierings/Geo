using Assets.Scripts.Models;

namespace Assets.Scripts.Calculators
{
	public static class ChunkPositionCalculator
	{

		public static Position CalculatePosition(int tileX, int tileY)
		{
			if (MapManager.MapConfiguration == null)
				return null;

			var tilesPerChunkDimension = MapManager.MapConfiguration.TilesPerChunkDimension;

			var x = (int)((double)tileX / (double)tilesPerChunkDimension);
			var y = (int)((double)tileY / (double)tilesPerChunkDimension);

			return new Position()
			{
				X = x,
				Y = y,
			};
		}

	}
}
