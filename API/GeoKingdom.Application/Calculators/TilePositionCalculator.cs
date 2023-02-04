using GeoKingdom.Base.Configuration;
using GeoKingdom.Models.Primary;
using Microsoft.Extensions.Options;
using System;

namespace GeoKingdom.Application.Calculators
{
	public class TilePositionCalculator
	{
		private readonly MapConfiguration _mapConfiguration;

		private const int _earthCircumference = 40000; // km

		public TilePositionCalculator(
			IOptionsSnapshot<MapConfiguration> mapConfiguration)
		{
			_mapConfiguration = mapConfiguration.Value;
		}

		public Position CalculatePosition(double longitude, double latitude)
		{
			var tilesOnLat = TilesOnY();
			var divisionFactor = 180 / (double)tilesOnLat;

			var tilesOnLon = TilesOnX(latitude);

			var y = latitude / divisionFactor;
			var x = tilesOnLon * longitude / 360;

			return new Position()
			{
				X = (int)x,
				Y = (int)y
			};
		}

		public (double lon, double lat) CalculateCoordinates(int x, int y)
		{
			var tilesOnLat = TilesOnY();
			var lat = (double)y * 180 / (double)tilesOnLat;

			var tilesOnLon = TilesOnX(lat);
			var lon = (double)x / tilesOnLon * 360;

			return (lon, lat);
		}

		private int TilesOnX(double latitude)
		{
			var metersPerTile = (double)_mapConfiguration.MetersPerTile;

			var longitudeLength = _earthCircumference * Math.Cos(ConvertToRadians(latitude));
			var tilesOnLon = longitudeLength * 1000 / metersPerTile;

			return (int)tilesOnLon;
		}

		private int TilesOnY()
			=> (int)(_earthCircumference * 1000 / (double)_mapConfiguration.MetersPerTile);

		private static double ConvertToRadians(double angle)
			=> Math.PI / 180 * angle;

	}
}
