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

		public Dictionary<ResourceType> MyProperty { get; set; }

	}
}
