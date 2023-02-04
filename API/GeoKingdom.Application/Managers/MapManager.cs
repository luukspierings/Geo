using GeoKingdom.Application.Calculators;
using GeoKingdom.Application.Extensions;
using GeoKingdom.Application.Helpers;
using GeoKingdom.Application.Models;
using GeoKingdom.Base.Configuration;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace GeoKingdom.Application.Managers
{
	public class MapManager
	{
		private readonly TilePositionCalculator _coordinateCalculator;
		private readonly MapConfiguration _mapConfiguration;
		private readonly ChunkManager _chunkManager;
		private readonly ChunkPositionCalculator _chunkPositionCalculator;

		public MapManager(
			TilePositionCalculator coordinateCalculator,
			IOptionsSnapshot<MapConfiguration> mapConfiguration,
			ChunkManager chunkManager,
			ChunkPositionCalculator chunkPositionCalculator)
		{
			_coordinateCalculator = coordinateCalculator;
			_chunkManager = chunkManager;
			_chunkPositionCalculator = chunkPositionCalculator;
			_mapConfiguration = mapConfiguration.Value;
		}

		public async Task<Map> GetMap(double lon, double lat)
		{
			var tilePosition = _coordinateCalculator.CalculatePosition(lon, lat);
			var chunkPosition = _chunkPositionCalculator.CalculatePosition(tilePosition);

			var chunks = await PositionHelper.GetPositionsInArea(
					chunkPosition.X,
					chunkPosition.Y,
					_mapConfiguration.ChunkRenderDistance
				)
				.Select(p => _chunkManager.GetChunk(p))
				.ToArrayAsync();

			var tiles = chunks
				.SelectMany(c => c.Tiles)
				.ToArray();

			return new Map()
			{
				CenterPosition = tilePosition,
				Tiles = tiles
			};
		}









	}
}
