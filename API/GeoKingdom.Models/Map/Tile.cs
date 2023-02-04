namespace GeoKingdom.Models
{
	public class Tile
	{
		public int Id { get; set; }
		public int ChunkId { get; set; }

		public int X { get; set; }
		public int Y { get; set; }

		public int BiomeId { get; set; }
		public Biome Biome { get; set; }

		public int? ResourceId { get; set; }
		public Resource Resource { get; set; }


	}
}
