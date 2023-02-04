using Assets.Scripts.Models;
using System;
using UnityEngine;

namespace Assets.Scripts.Calculators
{
	public static class CoordinateCalculator
	{
		private const int _earthCircumference = 40000;

		public static DoublePosition CalculatePosition(double longitude, double latitude)
		{
			if (MapManager.MapConfiguration == null)
				return null;

			var tilesOnLat = TilesOnY();
			var divisionFactor = 180 / (double)tilesOnLat;

			var y = latitude / divisionFactor;

			var tilesOnLon = TilesOnX(latitude);

			var x = tilesOnLon * longitude / 360;

			return new DoublePosition()
			{
				X = x,
				Y = y
			};
		}

		private static int TilesOnX(double latitude)
		{
			var metersPerTile = (double)MapManager.MapConfiguration.MetersPerTile;

			var longitudeLength = _earthCircumference * Math.Cos(ConvertToRadians(latitude));
			var tilesOnLon = longitudeLength * 1000 / metersPerTile;

			return (int)tilesOnLon;
		}

		private static int TilesOnY()
			=> (int)(_earthCircumference * 1000 / (double)MapManager.MapConfiguration.MetersPerTile);

		private static double ConvertToRadians(double angle)
			=> Mathf.PI / 180 * angle;



	}
}
