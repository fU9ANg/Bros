// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class PNewsletter
{
	public void Subscribe(PNewsletterOptions options, Action<PResponse> callback)
	{
		Playtomic.API.StartCoroutine(this.SendRequest("newsletter", "subscribe", callback, options));
	}

	private IEnumerator SendRequest(string section, string action, Action<PResponse> callback, PNewsletterOptions options)
	{
		WWW www = PRequest.Prepare(section, action, options);
		yield return www;
		PResponse response = PRequest.Process(www);
		callback(response);
		yield break;
	}

	private const string SECTION = "newsletter";

	private const string SUBSCRIBE = "subscribe";
}
