using Unity.Entities;

[GenerateAuthoringComponent]
public struct BuildingSelectionComponent : IComponentData
{
	public int X;
	public int Y;
}
