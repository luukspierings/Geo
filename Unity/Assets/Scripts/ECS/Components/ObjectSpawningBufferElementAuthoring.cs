using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct ObjectSpawningBufferElement : IBufferElementData
{
	public Entity Prefab;
	public float Percentage;
	public float PrefabSize;
}

[Serializable] 
public struct ObjectSpawningBufferElementValue
{
	[SerializeField]
	public GameObject Prefab;

	[SerializeField]
	[Range(0f, 100f)]
	public float Percentage;

	[SerializeField]
	public float PrefabSize;
}

public class ObjectSpawningBufferElementAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
	[SerializeField]
	public ObjectSpawningBufferElementValue[] Elements;

	public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
	{
		foreach (var item in Elements)
		{
			referencedPrefabs.Add(item.Prefab);
		}
	}

	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
		var dynamicBuffer = dstManager.AddBuffer<ObjectSpawningBufferElement>(entity);
		foreach (var item in Elements)
		{
			dynamicBuffer.Add(new ObjectSpawningBufferElement()
			{
				Prefab = conversionSystem.GetPrimaryEntity(item.Prefab),
				Percentage = item.Percentage,
				PrefabSize = item.PrefabSize
			});
		}
	}
}
