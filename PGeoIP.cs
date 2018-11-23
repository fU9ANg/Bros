// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGeoIP
{
	public void Lookup(Action<PlayerCountry, PResponse> callback)
	{
		Playtomic.API.StartCoroutine(this.SendRequest("geoip", "lookup", callback));
	}

	private IEnumerator SendRequest(string section, string action, Action<PlayerCountry, PResponse> callback)
	{
		WWW www = PRequest.Prepare(section, action, null);
		yield return www;
		PResponse response = PRequest.Process(www);
		Dictionary<string, object> data = (!response.success) ? null : response.json;
		callback(new PlayerCountry(data), response);
		yield break;
	}

	private const string SECTION = "geoip";

	private const string LOOKUP = "lookup";
}
