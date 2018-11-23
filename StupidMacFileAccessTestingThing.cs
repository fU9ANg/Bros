// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class StupidMacFileAccessTestingThing : MonoBehaviour
{
	private void OnGUI()
	{
		GUILayout.BeginArea(new Rect((float)Screen.width / 3f, (float)Screen.height / 3f, (float)Screen.width / 3f, (float)Screen.height / 3f), this.skin.box);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (GUILayout.Button("Method 1", new GUILayoutOption[0]))
		{
			this.TryMode(1);
		}
		if (GUILayout.Button("Method 2", new GUILayoutOption[0]))
		{
			this.TryMode(2);
		}
		if (GUILayout.Button("Method 3", new GUILayoutOption[0]))
		{
			this.TryMode(3);
		}
		GUILayout.EndHorizontal();
		if (this.files != null)
		{
			GUILayout.Label("Files Found:", new GUILayoutOption[0]);
			foreach (string text in this.files)
			{
				GUILayout.Label(text, new GUILayoutOption[0]);
			}
		}
		if (!string.IsNullOrEmpty(this.error))
		{
			GUILayout.Label("Error", new GUILayoutOption[0]);
			GUILayout.TextArea(this.error, new GUILayoutOption[0]);
		}
		GUILayout.EndArea();
	}

	private void TryMode(int mode)
	{
		this.files = null;
		this.error = null;
		try
		{
			switch (mode)
			{
			case 1:
				this.files = FileIO.FindCampaignFilesTest1();
				break;
			case 2:
				this.files = FileIO.FindCampaignFilesTest2();
				break;
			case 3:
				this.files = FileIO.FindCampaignFilesTest3();
				break;
			}
		}
		catch (Exception)
		{
		}
	}

	private string[] files;

	private string error;

	public GUISkin skin;
}
