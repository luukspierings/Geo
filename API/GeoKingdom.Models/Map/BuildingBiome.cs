using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoKingdom.Models.Map
{
	public class BuildingBiome
	{
		public int BuildingId { get; set; }
		public Building Building { get; set; }

		public int BiomeId { get; set; }
		public Biome Biome { get; set; }
	}
}
