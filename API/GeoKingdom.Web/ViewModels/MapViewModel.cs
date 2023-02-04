using GeoKingdom.Models.Primary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoKingdom.Web.ViewModels
{
	public class MapViewModel
	{
		public Position CenterPosition { get; set; }
		public IEnumerable<TileViewModel> Tiles { get; set; }
	}
}
