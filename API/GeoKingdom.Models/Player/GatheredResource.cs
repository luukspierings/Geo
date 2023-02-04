namespace GeoKingdom.Models.Player
{
	public class GatheredResource
	{
		public int Id { get; set; }

		public int PlayerId { get; set; }
		public int ResourceId { get; set; }

		public int Amount { get; set; }

	}
}
