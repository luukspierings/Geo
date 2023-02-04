using Unity.Entities;

[GenerateAuthoringComponent]
public struct TileComponent : IComponentData
{
	public int Id;

	public int X;
	public int Y;

	public int LocalX;
	public int LocalY;

	public PrefabIdentity PrefabIdentity;
}
