using GeoKingdom.Base.Configuration;
using GeoKingdom.Models.Primary;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoKingdom.Application.Calculators
{
	public class ChunkPositionCalculator
	{
		private readonly MapConfiguration _mapConfiguration;

		public ChunkPositionCalculator(
			IOptionsSnapshot<MapConfiguration> mapConfiguration)
		{
			_mapConfiguration = mapConfiguration.Value;
		}

		public Position CalculatePosition(Position tilePosition)
		{
			var tilesPerChunkDimension = _mapConfiguration.TilesPerChunkDimension;

			var x = (int)((double)tilePosition.X / (double)tilesPerChunkDimension);
			var y = (int)((double)tilePosition.Y / (double)tilesPerChunkDimension);

			return new Position()
			{
				X = x,
				Y = y,
			};
		}

	}
}
