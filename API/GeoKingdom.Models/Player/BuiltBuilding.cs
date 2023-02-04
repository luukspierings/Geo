using System;

namespace GeoKingdom.Models.Player
{
	public class BuiltBuilding
	{
		public int Id { get; set; }

		public int PlayerId { get; set; }
		public int BuildingId { get; set; }
		public int TileId { get; set; }


		public DateTime? ResourcesGathered { get; set; }
	}
}
