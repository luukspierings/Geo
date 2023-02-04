using GeoKingdom.Application.Extensions;
using GeoKingdom.Application.Helpers;
using GeoKingdom.Base.Configuration;
using GeoKingdom.Models;
using GeoKingdom.Models.Map;
using GeoKingdom.Models.Primary;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoKingdom.Application.Generators
{
	public class ChunkGenerator
	{
		private readonly MapConfiguration _mapConfiguration;
		private readonly ResourceGenerator _resourceGenerator;
		private readonly BiomeGenerator _biomeGenerator;

		public ChunkGenerator(
			IOptionsSnapshot<MapConfiguration> mapConfiguration,
			ResourceGenerator resourceGenerator,
			BiomeGenerator biomeGenerator)
		{
			_mapConfiguration = mapConfiguration.Value;
			_resourceGenerator = resourceGenerator;
			_biomeGenerator = biomeGenerator;
		}

		public async Task<Chunk> GenerateChunk(
			Position position)
		{
			var tilesPerChunkDimension = _mapConfiguration.TilesPerChunkDimension;

			var minX = position.X * tilesPerChunkDimension;
			var maxX = minX + tilesPerChunkDimension - 1;
			var minY = position.Y * tilesPerChunkDimension;
			var maxY = minY + tilesPerChunkDimension - 1;

			var tiles = await PositionHelper.GetPositionsInArea(minX, maxX, minY, maxY)
				.Select(async p => new Tile()
				{
					X = p.X,
					Y = p.Y,
					Resource = await _resourceGenerator.Generate(p.X, p.Y),
					Biome = await _biomeGenerator.Generate()
				})
				.ToArrayAsync();

			return new Chunk()
			{
				X = position.X,
				Y = position.Y,
				MinX = minX,
				MaxX = maxX,
				MinY = minY,
				MaxY = maxY,
				Tiles = tiles
			};
		}

	}
}
