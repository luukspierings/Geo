using Unity.Entities;

[GenerateAuthoringComponent]
public struct BorderComponent : IComponentData
{
	public int X;
	public int Y;
}
