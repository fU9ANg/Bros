// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLeaderboards
{
	public void Save(PlayerScore score, Action<PResponse> callback)
	{
		this.Save<PlayerScore>(score, callback);
	}

	public void Save<T>(T score, Action<PResponse> callback) where T : PlayerScore
	{
		Playtomic.API.StartCoroutine(this.SendSaveRequest("leaderboards", "save", score, callback));
	}

	public void SaveAndList(PlayerScore score, Action<List<PlayerScore>, int, PResponse> callback)
	{
		this.SaveAndList<PlayerScore>(score, callback);
	}

	public void SaveAndList<T>(T score, Action<List<T>, int, PResponse> callback) where T : PlayerScore
	{
		Playtomic.API.StartCoroutine(this.SendListRequest<T>("leaderboards", "saveandlist", score, callback));
	}

	public void List(PLeaderboardOptions options, Action<List<PlayerScore>, int, PResponse> callback)
	{
		this.List<PlayerScore>(options, callback);
	}

	public void List<T>(PLeaderboardOptions options, Action<List<T>, int, PResponse> callback) where T : PlayerScore
	{
		Playtomic.API.StartCoroutine(this.SendListRequest<T>("leaderboards", "list", options, callback));
	}

	private IEnumerator SendSaveRequest(string section, string action, Dictionary<string, object> postdata, Action<PResponse> callback)
	{
		WWW www = PRequest.Prepare(section, action, postdata);
		yield return www;
		PResponse response = PRequest.Process(www);
		callback(response);
		yield break;
	}

	private IEnumerator SendListRequest<T>(string section, string action, Dictionary<string, object> postdata, Action<List<T>, int, PResponse> callback) where T : PlayerScore
	{
		WWW www = PRequest.Prepare(section, action, postdata);
		yield return www;
		PResponse response = PRequest.Process(www);
		Dictionary<string, object> data = response.json;
		List<T> scores = new List<T>();
		int numscores = 0;
		if (response.success)
		{
			if (data.ContainsKey("numscores"))
			{
				int.TryParse(data["numscores"].ToString(), out numscores);
			}
			if (data.ContainsKey("scores") && data["scores"] is IList)
			{
				foreach (object obj in ((IList)data["scores"]))
				{
					IDictionary score = (IDictionary)obj;
					scores.Add((T)((object)Activator.CreateInstance(typeof(T), new object[]
					{
						score
					})));
				}
			}
		}
		callback(scores, numscores, response);
		yield break;
	}

	private const string SECTION = "leaderboards";

	private const string SAVEANDLIST = "saveandlist";

	private const string SAVE = "save";

	private const string LIST = "list";
}
