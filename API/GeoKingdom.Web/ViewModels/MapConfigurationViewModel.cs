using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoKingdom.Web.ViewModels
{
	public class MapConfigurationViewModel
	{
		public int MetersPerTile { get; set; }
		public int ChunkRenderDistance { get; set; }
		public int TilesPerChunkDimension { get; set; }
	}
}
