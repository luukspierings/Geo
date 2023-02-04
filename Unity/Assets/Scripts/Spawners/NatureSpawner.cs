using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable] 
public class ExtraObjectSetting : MainObjectSetting
{ 
	[SerializeField]
	[Range(0f, 100f)]
	public float Percentage;
}

[Serializable]
public class MainObjectSetting
{
	[SerializeField]
	public GameObject Prefab;
}

[ExecuteInEditMode]
public class NatureSpawner : MonoBehaviour
{
	public MainObjectSetting MainObject;
	public List<ExtraObjectSetting> ExtraObjects;

	public GameObject Plane;
	public GameObject Mask;

	public int MaximumCollisionTries = 20;
	public float MinimumCollisionDistance = 0.5f;
	public int MaximumSpawns = 100;

	public void Generate()
	{
		if (MainObject == null || Plane == null)
			return;

		Clear();

		var placedPoints = new List<Vector3>();
		var placeTry = 0;

		foreach (var point in GeneratePoints())
		{
			if (placedPoints.Count > MaximumSpawns || placeTry >= MaximumCollisionTries)
				break;

			if(!placedPoints.All(p => Vector3.Distance(p, point) > MinimumCollisionDistance))
			{
				placeTry++;
				continue;
			}

			Instantiate(SelectRandomPrefab(), point, Quaternion.identity, transform);
			placedPoints.Add(point);
			placeTry = 0;
		}
	}

	public void Clear()
	{
		var settingNames = new[] { MainObject }
			.Concat(ExtraObjects)
			.Select(s => s.Prefab.name + "(Clone)")
			.ToArray();

		for (int i = transform.childCount - 1; i >= 0; i--)
		{
			var child = transform.GetChild(i);
			if (!settingNames.Contains(child.name))
				continue;

			if (Application.isEditor)
				DestroyImmediate(child.gameObject);
			else
				Destroy(child.gameObject);
		}
	}

	private GameObject SelectRandomPrefab()
	{
		if (ExtraObjects.Count < 1)
			return MainObject.Prefab;

		var extraPercentage = ExtraObjects.Sum(s => s.Percentage);
		var totalPercentage = extraPercentage >= 100f
			? extraPercentage
			: 100f;

		var randomValue = UnityEngine.Random.Range(0f, totalPercentage);
		var lastValue = 0f;

		foreach (var obj in ExtraObjects)
		{
			if (lastValue + obj.Percentage >= randomValue)
				return obj.Prefab;

			lastValue += obj.Percentage;
		}

		return MainObject.Prefab;
	}

	private IEnumerable<Vector3> GeneratePoints()
	{
		var planeRenderer = Plane.GetComponent<Renderer>();
		var moveAreaX = planeRenderer.bounds.size.x / 2;
		var moveAreaZ = planeRenderer.bounds.size.z / 2;
		var center = planeRenderer.bounds.center;

		var minX = -moveAreaX;
		var maxX = moveAreaX;
		var minZ = -moveAreaZ;
		var maxZ = moveAreaZ;

		float randomX;
		float randomZ;

		while(true)
		{
			randomX = center.x + UnityEngine.Random.Range(minX, maxX);
			randomZ = center.z + UnityEngine.Random.Range(minZ, maxZ);

			if (IsWithinMask(randomX, randomZ))
				continue;

			yield return new Vector3(randomX, Plane.transform.position.y, randomZ);
		}
	}

	private bool IsWithinMask(float x, float z)
	{
		if (Mask == null)
			return false;

		var planeRenderer = Mask.GetComponent<Renderer>();
		var moveAreaX = planeRenderer.bounds.size.x / 2;
		var moveAreaZ = planeRenderer.bounds.size.z / 2;
		var center = planeRenderer.bounds.center;

		var minX = center.x - moveAreaX;
		var maxX = center.x + moveAreaX;
		var minZ = center.z - moveAreaZ;
		var maxZ = center.z + moveAreaZ;

		return IsWithin(x, minX, maxX) && IsWithin(z, minZ, maxZ);
	}

	private bool IsWithin(float pos, float minPos, float maxPos)
		=> pos > minPos && pos < maxPos;

}
