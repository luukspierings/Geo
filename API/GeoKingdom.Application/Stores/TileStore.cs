using GeoKingdom.Application.DataAccess;
using GeoKingdom.Models;
using GeoKingdom.Models.Primary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoKingdom.Application.Stores
{
	public class TileStore
	{
		private readonly ApplicationDbContext _applicationDbContext;

		public TileStore(
			ApplicationDbContext applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
		}

		public Tile GetTile(Position position)
		{
			return _applicationDbContext.Tiles
				.FirstOrDefault(t => TileWithPosition(t, position));
		}

		private static bool TileWithPosition(Tile tile, Position position)
			=> tile.X == position.X && tile.Y == position.Y;

	}
}
