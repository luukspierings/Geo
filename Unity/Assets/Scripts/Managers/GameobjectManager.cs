using Assets.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameobjectManager : Singleton<GameobjectManager>
{
	protected override GameobjectManager This => this;

	private int _counter;
	private Queue<GameobjectQueueItem> _instantiateQueue;
	private Queue<int> _destroyQueue;
	private Dictionary<int, GameObject> _objects;

	public List<GameobjectRegistrationItem> Registrations;


	private void Start()
	{
		_objects = new Dictionary<int, GameObject>();
		_instantiateQueue = new Queue<GameobjectQueueItem>();
		_destroyQueue = new Queue<int>();
		_counter = 0;
	}

	private void Update()
	{
		while (_instantiateQueue.Count != 0)
		{
			var item = _instantiateQueue.Dequeue();
			var registration = Registrations.FirstOrDefault(r => r.Type == item.Type);
			if (registration == null)
				continue;

			var obj = Instantiate(registration.Prefab, item.Position, item.Rotation);
			_objects.Add(item.Id, obj);
		}

		while (_destroyQueue.Count != 0)
		{
			var id = _destroyQueue.Dequeue();

			if (!_objects.TryGetValue(id, out GameObject obj))
				continue;

			Destroy(obj);
			_objects.Remove(id);
		}
	}

	public int Queue(GameobjectQueueItem gameobjectQueueItem)
	{
		var id = _counter++;
		gameobjectQueueItem.Id = id;
		_instantiateQueue.Enqueue(gameobjectQueueItem);
		return id;
	}

	public void Destroy(int id)
	{
		_destroyQueue.Enqueue(id);
	}

}

public struct GameobjectQueueItem
{
	public int Id { get; set; }
	public GameobjectType Type { get; set; }
	public Vector3 Position { get; set; }
	public Quaternion Rotation { get; set; }
}

public enum GameobjectType
{
	Unknown,
	Grass,
	Tree


}

[Serializable]
public class GameobjectRegistrationItem
{
	[SerializeField]
	public GameobjectType Type;

	[SerializeField]
	public GameObject Prefab;
}
