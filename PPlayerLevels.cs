// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPlayerLevels
{
	public void Save(PlayerLevel level, Action<PlayerLevel, PResponse> callback)
	{
		Playtomic.API.StartCoroutine(this.SendSaveLoadRequest("playerlevels", "save", level, callback));
	}

	public void Load(string levelid, Action<PlayerLevel, PResponse> callback)
	{
		Dictionary<string, object> postdata = new Dictionary<string, object>
		{
			{
				"levelid",
				levelid
			}
		};
		Playtomic.API.StartCoroutine(this.SendSaveLoadRequest("playerlevels", "load", postdata, callback));
	}

	private IEnumerator SendSaveLoadRequest(string section, string action, Dictionary<string, object> postdata, Action<PlayerLevel, PResponse> callback)
	{
		WWW www = PRequest.Prepare(section, action, postdata);
		yield return www;
		PResponse response = PRequest.Process(www);
		PlayerLevel level = null;
		if (response.success)
		{
			level = new PlayerLevel((Dictionary<string, object>)response.json["level"]);
		}
		callback(level, response);
		yield break;
	}

	public void List(PPlayerLevelOptions options, Action<List<PlayerLevel>, int, PResponse> callback)
	{
		Playtomic.API.StartCoroutine(this.SendListRequest("playerlevels", "list", options, callback));
	}

	private IEnumerator SendListRequest(string section, string action, Dictionary<string, object> postdata, Action<List<PlayerLevel>, int, PResponse> callback)
	{
		WWW www = PRequest.Prepare("playerlevels", "list", postdata);
		yield return www;
		PResponse response = PRequest.Process(www);
		List<PlayerLevel> levels = null;
		int numlevels = 0;
		if (response.success)
		{
			Dictionary<string, object> data = response.json;
			levels = new List<PlayerLevel>();
			numlevels = (int)((double)data["numlevels"]);
			List<object> levelarr = (List<object>)data["levels"];
			for (int i = 0; i < levelarr.Count; i++)
			{
				levels.Add(new PlayerLevel((Dictionary<string, object>)levelarr[i]));
			}
		}
		callback(levels, numlevels, response);
		yield break;
	}

	public void Rate(string levelid, int rating, Action<PResponse> callback)
	{
		if (rating < 1 || rating > 10)
		{
			callback(PResponse.Error(401));
			return;
		}
		Dictionary<string, object> postdata = new Dictionary<string, object>
		{
			{
				"levelid",
				levelid
			},
			{
				"rating",
				rating
			}
		};
		Playtomic.API.StartCoroutine(this.SendRateRequest("playerlevels", "rate", postdata, callback));
	}

	private IEnumerator SendRateRequest(string section, string action, Dictionary<string, object> postdata, Action<PResponse> callback)
	{
		WWW www = PRequest.Prepare("playerlevels", "rate", postdata);
		yield return www;
		PResponse response = PRequest.Process(www);
		callback(response);
		yield break;
	}

	private const string SECTION = "playerlevels";

	private const string SAVE = "save";

	private const string LIST = "list";

	private const string LOAD = "load";

	private const string RATE = "rate";
}
