using System.Collections.Generic;

namespace GeoKingdom.Models.Map
{
	public class Chunk
	{
		public int Id { get; set; }

		public int X { get; set; }
		public int Y { get; set; }

		public int MinX { get; set; }
		public int MaxX { get; set; }
		public int MinY { get; set; }
		public int MaxY { get; set; }

		public IEnumerable<Tile> Tiles { get; set; }
	}
}
