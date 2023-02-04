using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class PlaneObjectSpawnerSystem : SystemBase
{
	private static Unity.Mathematics.Random Random;

	protected override void OnCreate()
	{
		var r = (uint)UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		Random = new Unity.Mathematics.Random(r == 0 ? r + 1 : r);

		base.OnCreate();
	}

	protected override void OnUpdate()
	{
		var commandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
		var commandBuffer = commandBufferSystem.CreateCommandBuffer();

		Entities
			.ForEach((Entity e, ref ObjectSpawningComponent objectSpawningComponent) =>
			{
				if (objectSpawningComponent.Spawned)
					return;

				var objectElements = GetBuffer<ObjectSpawningBufferElement>(e);
				var renderBounds = GetComponent<RenderBounds>(objectSpawningComponent.Plane);

				if(objectElements.IsEmpty)
				{
					objectSpawningComponent.Spawned = true;
					return;
				}


				var parentLinkedEntityGroup = commandBuffer.AddBuffer<LinkedEntityGroup>(e);
				parentLinkedEntityGroup.Add(new LinkedEntityGroup()
				{
					Value = e
				});

				var placedPoints = new List<(float3 pos, float size)>();
				var placeTry = 0;

				foreach (var point in GeneratePoints(ref objectSpawningComponent, renderBounds))
				{
					if (placedPoints.Count() > objectSpawningComponent.MaximumSpawns || placeTry >= objectSpawningComponent.MaximumCollisionTries)
						break;

					(Entity entity, float size) randomEntity = SelectRandomPrefab(objectElements);

					var collisionPoints = placedPoints.Where(p => math.distance(p.pos, point) < p.size + randomEntity.size);
					if (collisionPoints.Any())
					{
						placeTry++;
						continue;
					}
					 
					placedPoints.Add((point, randomEntity.size));


					var entity = commandBuffer.Instantiate(randomEntity.entity);

					commandBuffer.AddComponent(entity, new LocalToParent()
					{
						Value = new float4x4(quaternion.identity, point)
					});
					commandBuffer.AddComponent(entity, new Translation()
					{
						Value = point
					});
					commandBuffer.AddComponent(entity, new Parent()
					{
						Value = e
					});
					commandBuffer.AddComponent(entity, new Rotation()
					{
						Value = quaternion.RotateY(Random.NextFloat(0, 360))
					});
					commandBuffer.AppendToBuffer(e, new LinkedEntityGroup()
					{
						Value = entity
					});

					placeTry = 0;
				}

				objectSpawningComponent.Spawned = true;
			})
			.WithoutBurst()
			.Schedule();

		commandBufferSystem.AddJobHandleForProducer(Dependency);
	}

	private static (Entity entity, float size) SelectRandomPrefab(
		in DynamicBuffer<ObjectSpawningBufferElement> objectSpawningElements)
	{
		var totalPercentage = 0f;
		for (int i = 0; i < objectSpawningElements.Length; i++)
			totalPercentage += objectSpawningElements[i].Percentage;

		var randomValue = Random.NextFloat(0f, totalPercentage);
		var lastValue = 0f;

		for (int i = 0; i < objectSpawningElements.Length; i++)
		{
			var obj = objectSpawningElements[i];
			if (lastValue + obj.Percentage >= randomValue)
				return (obj.Prefab, obj.PrefabSize);

			lastValue += obj.Percentage;
		}

		throw new System.Exception("Something went wrong while selecting random prefab.");
	}

	private static List<float3> GeneratePoints(ref ObjectSpawningComponent osc, RenderBounds rb)
	{
		var list = new List<float3>();
		var size = (rb.Value.Max - rb.Value.Min);

		for (int i = 0; i < osc.MaximumSpawns; i++)
		{
			var randomX = Random.NextFloat(-1f, 1f) * size.x;
			var randomZ = Random.NextFloat(-1f, 1f) * size.z;

			list.Add(new float3(randomX, rb.Value.Center.y, randomZ));
		}

		return list;
	}

}
