using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoKingdom.Models.Player
{
	public class ClaimedTile
	{
		public int Id { get; set; }

		public int PlayerId { get; set; }
		public int TileId { get; set; }

	}
}
