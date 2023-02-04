using GeoKingdom.Models.Map;

namespace GeoKingdom.Models
{
	public class Biome
	{
		public int Id { get; set; }
		public BiomeCodeType Code { get; set; }
		public string Name { get; set; }
		public BiomeType Type { get; set; }

	}
}
