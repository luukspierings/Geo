using GeoKingdom.Models;
using GeoKingdom.Models.Map;
using System.Collections.Generic;

namespace GeoKingdom.Base.Configuration
{
	public class MapConfiguration
	{
		/// <summary>
		/// Amounts of meters each tile is.
		/// </summary>
		public int MetersPerTile { get; set; }

		/// <summary>
		/// The amount of chunks side of the center tile should be rendered.
		/// </summary>
		public int ChunkRenderDistance { get; set; }

		/// <summary>
		/// The with of each chunk.
		/// </summary>
		public int TilesPerChunkDimension { get; set; }

		/// <summary>
		/// The distribution of resources
		/// </summary>
		public Dictionary<ResourceCodeType, DistributionConfiguration> ResourceDistribution { get; set; }

	}
}
