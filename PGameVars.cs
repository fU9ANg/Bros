// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGameVars
{
	public void Load(Action<Dictionary<string, GameVar>, PResponse> callback)
	{
		this.Load<GameVar>(callback);
	}

	public void Load(string name, Action<GameVar, PResponse> callback)
	{
		this.Load<GameVar>(name, callback);
	}

	public void Load<T>(Action<Dictionary<string, T>, PResponse> callback) where T : GameVar, new()
	{
		Playtomic.API.StartCoroutine(this.SendRequest<T>("gamevars", "load", callback));
	}

	public void Load<T>(string name, Action<T, PResponse> callback) where T : GameVar, new()
	{
		Dictionary<string, object> postdata = new Dictionary<string, object>
		{
			{
				"name",
				name
			}
		};
		Playtomic.API.StartCoroutine(this.SendRequest<T>(name, "gamevars", "single", callback, postdata));
	}

	internal IEnumerator SendRequest<T>(string section, string action, Action<Dictionary<string, T>, PResponse> callback) where T : GameVar, new()
	{
		WWW www = PRequest.Prepare(section, action, null);
		yield return www;
		PResponse response = PRequest.Process(www);
		Dictionary<string, object> data = (!response.success) ? null : response.json;
		Dictionary<string, T> gameVars = new Dictionary<string, T>();
		if (data != null && data is IDictionary)
		{
			foreach (string key in data.Keys)
			{
				if (data[key] is IDictionary)
				{
					gameVars.Add(key, (T)((object)Activator.CreateInstance(typeof(T), new object[]
					{
						data[key]
					})));
				}
			}
		}
		callback(gameVars, response);
		yield break;
	}

	internal IEnumerator SendRequest<T>(string name, string section, string action, Action<T, PResponse> callback, Dictionary<string, object> postdata = null) where T : GameVar, new()
	{
		WWW www = PRequest.Prepare(section, action, postdata);
		yield return www;
		PResponse response = PRequest.Process(www);
		Dictionary<string, object> data = (!response.success) ? null : response.json;
		T gameVar = Activator.CreateInstance<T>();
		if (data != null && data is IDictionary && data.ContainsKey(name))
		{
			gameVar = (T)((object)Activator.CreateInstance(typeof(T), new object[]
			{
				data[name]
			}));
		}
		callback(gameVar, response);
		yield break;
	}

	private const string SECTION = "gamevars";

	private const string LOAD = "load";

	private const string LOADSINGLE = "single";
}
