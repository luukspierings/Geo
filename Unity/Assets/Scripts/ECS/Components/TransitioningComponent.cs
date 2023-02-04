using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct TransitioningComponent : IComponentData
{
	public float TransitionPercentage;
	public float TransitionSeconds;

	public float3 FromPosition;
	public float3 ToPosition;

	public quaternion FromRotation;
	public quaternion ToRotation;

}