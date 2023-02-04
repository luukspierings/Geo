using GeoKingdom.Application.Calculators;
using GeoKingdom.Base.Configuration;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Diagnostics;
using Xunit;

namespace GeoKingdom.Tests
{
	public class CoordinateCalculatorTests
	{
		private readonly TilePositionCalculator _coordinateCalculator;

		public CoordinateCalculatorTests()
		{
			var options = Substitute.For<IOptionsSnapshot<MapConfiguration>>();
			options.Value.Returns(new MapConfiguration()
			{
				MetersPerTile = 5
			});

			_coordinateCalculator = new TilePositionCalculator(options);
		}

		[Fact]
		public void Calculate_zero_coordinates()
		{
			var pos = _coordinateCalculator.CalculatePosition(0, 0);

			Assert.Equal(0, pos.X);
			Assert.Equal(0, pos.Y);
		}

		[Fact]
		public void Calculate_max_coordinates()
		{
			var maxPos = _coordinateCalculator.CalculatePosition(180, 90);
			var minPos = _coordinateCalculator.CalculatePosition(-180, -90);

			// x
			// xxx
			// xxxxx
			// xxx
			// x

			Assert.Equal(0, maxPos.X); // The poles only have 1 square, and that is 0
			Assert.Equal(4000000, maxPos.Y);

			Assert.Equal(0, minPos.X);
			Assert.Equal(-4000000, minPos.Y);
		}

		[Fact]
		public void Calculate_half_max_coordinates()
		{
			var lon = 90;
			var lat = 45;
			var pos = _coordinateCalculator.CalculatePosition(lon, lat);

			Assert.Equal(1414214, pos.X);
			Assert.Equal(2000000, pos.Y);

			var coords = _coordinateCalculator.CalculateCoordinates(pos.X, pos.Y);
			Assert.Equal(lon, (int)coords.lon);
			Assert.Equal(lat, (int)coords.lat);
		}


	}
}
