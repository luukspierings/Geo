using Unity.Entities;

[GenerateAuthoringComponent]
public struct CameraComponent : IComponentData
{
	public float MinimumRadius;
	public float MaximumRadius;
	public float Radius;

	public int MovementSpeed;
	public int ZoomSpeed;

	public float XRotation;
	public float YRotation;
	public float MinimumXRotation;
	public float MaximumXRotation;

	public float BirdsEyeRotation;


	public CameraModeType CameraModeType;
}

public enum CameraModeType
{
	Unknown,
	Following,
	BirdsEye
}
