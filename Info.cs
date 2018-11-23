// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public static class Info
{
	public static bool IsDevBuild
	{
		get
		{
			if (!Info.initialized)
			{
				Info.initialized = true;
				Info.isDevBuild = (!Application.absoluteURL.Contains("www.freelives.net") && !Application.absoluteURL.ToLower().Contains("http"));
			}
			return Info.isDevBuild;
		}
	}

	public static bool IsRichardsPC
	{
		get
		{
			return !Application.isWebPlayer && Environment.UserName.Contains("Richard");
		}
	}

	private static bool initialized;

	private static bool isDevBuild;
}
