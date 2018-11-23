// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

internal class PRequest
{
	public static void Initialise(string publickey, string privatekey, string apiurl)
	{
		if (!apiurl.EndsWith("/"))
		{
			apiurl += "/";
		}
		PRequest.APIURL = apiurl + "v1?publickey=" + publickey;
		PRequest.PRIVATEKEY = privatekey;
		PRequest.PUBLICKEY = publickey;
	}

	public static WWW Prepare(string section, string action, Dictionary<string, object> postdata = null)
	{
		if (postdata == null)
		{
			postdata = new Dictionary<string, object>();
		}
		else
		{
			postdata.Remove("publickey");
			postdata.Remove("section");
			postdata.Remove("action");
		}
		postdata.Add("publickey", PRequest.PUBLICKEY);
		postdata.Add("section", section);
		postdata.Add("action", action);
		string text = PJSON.JsonEncode(postdata);
		WWWForm wwwform = new WWWForm();
		wwwform.AddField("data", PEncode.Base64(text));
		wwwform.AddField("hash", PEncode.MD5(text + PRequest.PRIVATEKEY));
		return new WWW(PRequest.APIURL, wwwform);
	}

	public static PResponse Process(WWW www)
	{
		if (www == null)
		{
			return PResponse.GeneralError(1);
		}
		if (www.error != null)
		{
			return PResponse.GeneralError(www.error);
		}
		if (string.IsNullOrEmpty(www.text))
		{
			return PResponse.Error(1);
		}
		Dictionary<string, object> dictionary = (Dictionary<string, object>)PJSON.JsonDecode(www.text);
		if (!dictionary.ContainsKey("success") || !dictionary.ContainsKey("errorcode"))
		{
			return PResponse.GeneralError(1);
		}
		return new PResponse
		{
			success = (bool)dictionary["success"],
			errorcode = (int)((double)dictionary["errorcode"]),
			json = dictionary
		};
	}

	public static Dictionary<string, PResponse> Requests = new Dictionary<string, PResponse>();

	private static string APIURL;

	private static string PUBLICKEY;

	private static string PRIVATEKEY;
}
