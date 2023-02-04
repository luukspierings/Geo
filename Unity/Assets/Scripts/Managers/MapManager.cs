using Assets.Scripts.Configuration;
using Assets.Scripts.Models;
using Assets.Scripts.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
	protected override MapManager This => this;

	public static MapConfiguration MapConfiguration;

	private Dictionary<(int, int), Tile> _tiles;

	private void Start()
	{
		_tiles = new Dictionary<(int, int), Tile>();
		GetConfiguration();
	}

	public void GetMap(double lon, double lat)
	{
		var serializedLon = JsonConvert.SerializeObject(lon);
		var serializedLat = JsonConvert.SerializeObject(lat);

		NetworkManager.Instance.EnqueueGetRequest($"map?lon={serializedLon}&lat={serializedLat}", body =>
		{
			Debug.Log(body);

			var map = JsonConvert.DeserializeObject<Map>(body);

			foreach (var tile in map.Tiles)
			{
				_tiles[(tile.X, tile.Y)] = tile;
			}
		});
	}

	private void GetConfiguration()
	{
		NetworkManager.Instance.EnqueueGetRequest("map/configuration", body =>
		{
			Debug.Log(body);

			MapConfiguration = JsonConvert.DeserializeObject<MapConfiguration>(body);
		});
	}

	public Tile GetTile(int x, int y)
	{
		return _tiles.TryGetValue((x, y), out Tile tile)
			? tile
			: null;
	}

}
