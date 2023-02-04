using System.Collections.Generic;

namespace GeoKingdom.Models.Player
{
	public class Kingdom
	{
		public int Id { get; set; }
		public int PlayerId { get; set; }

		public IEnumerable<ClaimedTile> Claims { get; set; }
		public IEnumerable<BuiltBuilding> Buildings { get; set; }
		public IEnumerable<GatheredResource> GatheredResources { get; set; }


	}
}
