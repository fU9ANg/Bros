// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkLog : SingletonMono<NetworkLog>
{
	public static void TryAddMessage(string str)
	{
		if (str.Length > 1 && str[0] == '>')
		{
			NetworkLog.messages.Add(str.Trim());
		}
	}

	private void Start()
	{
	}

	private void FixedUpdate()
	{
	}

	public void DrawLog()
	{
		GUI.color = Color.white * 0.9f + Color.black * 0.1f;
		GUILayout.Label("CONNECTION LOG:", new GUILayoutOption[0]);
		if (GUILayout.Button("Clear", new GUILayoutOption[0]))
		{
			NetworkLog.messages.Clear();
		}
		int num = 0;
		foreach (string arg in NetworkLog.messages)
		{
			num++;
			GUILayout.Label(num + ")  " + arg, new GUILayoutOption[0]);
		}
	}

	public static List<string> messages = new List<string>();

	private static Vector2 pos = Vector2.zero;
}
