using Unity.Entities;

[GenerateAuthoringComponent]
public struct MapPositionComponent : IComponentData
{
	public int TileX;
	public int TileY;
	public int ChunkX;
	public int ChunkY;

	public double Longitude;
	public double Latitude;

	public int OriginX;
	public int OriginY;

	public int RenderDistance;
	public int TileSize;

	public bool ChangedPosition;
	public bool WaitingForMap;
}
