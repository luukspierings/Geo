using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoKingdom.Models.Map
{
	public class Building
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public int Price { get; set; }


		/// <summary>
		/// The specific biomes the building can be built on.
		/// </summary>
		public IEnumerable<BuildingBiome> AcceptableBiomes { get; set; }

		/// <summary>
		/// The specific resource where the building can be built on.
		/// </summary>
		public int? AcceptableResourceId { get; set; }
		public Resource AccepableResource { get; set; }




	}
}
