using Unity.Entities;

[GenerateAuthoringComponent]
public struct GameobjectComponent : IComponentData
{
	public GameobjectType Type;

	public bool Spawned;
	public int Id;
}
