using Unity.Entities;

[GenerateAuthoringComponent]
public struct ObjectSpawningComponent : IComponentData
{
	public Entity Plane;

	public int MaximumCollisionTries;
	public int MaximumSpawns;

	public bool Spawned;
}
