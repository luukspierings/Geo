using GeoKingdom.Models;
using GeoKingdom.Models.Primary;
using System.Collections.Generic;

namespace GeoKingdom.Application.Models
{
	public class Map
	{
		public Position CenterPosition { get; set; }
		public IEnumerable<Tile> Tiles { get; set; }
	}
}
