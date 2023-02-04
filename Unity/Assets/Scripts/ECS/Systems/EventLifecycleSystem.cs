using Unity.Entities;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public class EventLifecycleSystem : SystemBase
{
	protected override void OnUpdate()
	{
		EventManager.Instance.StartNewCycle();
	}
}
