using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

[Serializable]
public enum PrefabIdentity
{
    Unknown,
    Tile_Grass,
    Tile_Forest,
    Border_Cloud
}

[Serializable]
public class PrefabRegistrationItem
{
    [SerializeField]
    public PrefabIdentity Identity;

    [SerializeField]
    public GameObject Prefab;
}

public class PrefabRegistration : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    public List<PrefabRegistrationItem> Registrations;
    private static Dictionary<PrefabIdentity, Entity> _registrationDictionary;

	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
        _registrationDictionary = new Dictionary<PrefabIdentity, Entity>();
		foreach (var registration in Registrations)
		{
            var prefabEntity = conversionSystem.GetPrimaryEntity(registration.Prefab);
            _registrationDictionary[registration.Identity] = prefabEntity;
		}
	}

	public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
	{
        var prefabs = Registrations
            .Select(r => r.Prefab)
            .ToArray();
        referencedPrefabs.AddRange(prefabs);
	}

    public static Entity GetEntity(PrefabIdentity prefabIdentity)
	{
        return _registrationDictionary[prefabIdentity];
	}
}
