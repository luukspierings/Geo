using Assets.Scripts.Models;
using Assets.Scripts.Utility;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : Singleton<NetworkManager>
{
	protected override NetworkManager This => this;

	private Queue<(string path, Action<string> callback)> _requestQueue;
	private string _baseUrl;

	public NetworkManager()
	{
		_requestQueue = new Queue<(string, Action<string>)>();
		_baseUrl = "https://localhost:5001/";
	}

	private void Update()
	{
		while (_requestQueue.Count != 0)
		{
			var (path, callback) = _requestQueue.Dequeue();

			StartCoroutine(GetRequest(path, callback));
		}
	}

	public void EnqueueGetRequest(string path, Action<string> callback)
	{
		_requestQueue.Enqueue((path, callback));
	}

	IEnumerator GetRequest(string path, Action<string> callback)
	{
		using var webRequest = UnityWebRequest.Get(_baseUrl + path);

		// Request and wait for the desired page.
		yield return webRequest.SendWebRequest();

		if (webRequest.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError(webRequest.error);
			yield break;
		}
		callback(webRequest.downloadHandler.text);
	}



}
