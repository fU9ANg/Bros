// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PAchievements
{
	public void List(PAchievementOptions options, Action<List<PlayerAchievement>, PResponse> callback)
	{
		this.List<PlayerAchievement>(options, callback);
	}

	public void List<T>(PAchievementOptions options, Action<List<T>, PResponse> callback) where T : PlayerAchievement, new()
	{
		Playtomic.API.StartCoroutine(this.SendListRequest<T>(PAchievements.SECTION, PAchievements.LIST, callback, options));
	}

	internal IEnumerator SendListRequest<T>(string section, string action, Action<List<T>, PResponse> callback, Dictionary<string, object> postdata = null) where T : PlayerAchievement
	{
		WWW www = PRequest.Prepare(section, action, postdata);
		yield return www;
		PResponse response = PRequest.Process(www);
		Dictionary<string, object> data = (!response.success) ? null : response.json;
		List<T> achievements = new List<T>();
		if (response.success)
		{
			foreach (object obj in ((IList)data["achievements"]))
			{
				IDictionary achievment = (IDictionary)obj;
				achievements.Add((T)((object)Activator.CreateInstance(typeof(T), new object[]
				{
					achievment
				})));
			}
		}
		callback(achievements, response);
		yield break;
	}

	public void Stream(PAchievementStreamOptions options, Action<List<PlayerAward>, int, PResponse> callback)
	{
		Playtomic.API.StartCoroutine(this.SendStreamRequest(PAchievements.SECTION, PAchievements.STREAM, callback, options));
	}

	internal IEnumerator SendStreamRequest(string section, string action, Action<List<PlayerAward>, int, PResponse> callback, Dictionary<string, object> postdata = null)
	{
		WWW www = PRequest.Prepare(section, action, postdata);
		yield return www;
		PResponse response = PRequest.Process(www);
		Dictionary<string, object> data = (!response.success) ? null : response.json;
		int numachievements = 0;
		int.TryParse(data["numachievements"].ToString(), out numachievements);
		List<PlayerAward> achievements = new List<PlayerAward>();
		if (response.success)
		{
			List<object> acharray = (List<object>)data["achievements"];
			achievements.AddRange(from object t in acharray
			select new PlayerAward((Dictionary<string, object>)t));
		}
		callback(achievements, numachievements, response);
		yield break;
	}

	public void Save(Dictionary<string, object> achievement, Action<PResponse> callback)
	{
		Playtomic.API.StartCoroutine(this.SendSaveRequest(PAchievements.SECTION, PAchievements.SAVE, callback, achievement));
	}

	internal IEnumerator SendSaveRequest(string section, string action, Action<PResponse> callback, Dictionary<string, object> postdata = null)
	{
		WWW www = PRequest.Prepare(section, action, postdata);
		yield return www;
		PResponse response = PRequest.Process(www);
		callback(response);
		yield break;
	}

	private static string SECTION = "achievements";

	private static string LIST = "list";

	private static string STREAM = "stream";

	private static string SAVE = "save";
}
