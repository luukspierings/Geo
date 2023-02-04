using Assets.Scripts.Calculators;
using Unity.Entities;
using UnityEngine;

public class MapChunkLoadingSystem : SystemBase
{
	private static int _lastLoadX = 0;
	private static int _lastLoadY = 0;
	private static readonly int _minimumDistanceFromLastLoad = 1;

	protected override void OnUpdate()
	{
		Entities
			.ForEach((ref MapPositionComponent mapPositionComponent) =>
			{
				if (PositionHelper.IsWithinOrEqualToRadius(_lastLoadX, mapPositionComponent.ChunkX, _minimumDistanceFromLastLoad)
					&& PositionHelper.IsWithinOrEqualToRadius(_lastLoadY, mapPositionComponent.ChunkY, _minimumDistanceFromLastLoad))
					return;

				Debug.Log($"Loading new map: {mapPositionComponent.Longitude}, {mapPositionComponent.Latitude}");

				MapManager.Instance.GetMap(mapPositionComponent.Longitude, mapPositionComponent.Latitude);

				_lastLoadX = mapPositionComponent.ChunkX;
				_lastLoadY = mapPositionComponent.ChunkY;
			})
			.WithoutBurst()
			.Schedule();
	}
}
