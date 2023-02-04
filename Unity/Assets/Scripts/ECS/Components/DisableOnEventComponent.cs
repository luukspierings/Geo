using Assets.Scripts.Configuration;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct DisableOnEventComponent : IComponentData
{
	public EventType EnableEvent;
	public EventType DisableEvent;
}
