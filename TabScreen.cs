// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TabScreen : MonoBehaviour
{
	private void Awake()
	{
		TabScreen.instance = this;
	}

	private void Update()
	{
		this.WindowRect.x = Mathf.Abs(((float)Screen.width - this.WindowRect.width) / 2f);
		this.WindowRect.y = Mathf.Abs(((float)Screen.height - this.WindowRect.height) / 2f);
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			this.show = !this.show;
		}
	}

	private void OnGUI()
	{
		GUISkin skin = GUI.skin;
		GUI.skin = this.pixelSkin;
		GUI.skin = skin;
	}

	private void windowWindow(int windowId)
	{
		GUIStyle guistyle = new GUIStyle();
		guistyle.fontStyle = FontStyle.Bold;
		guistyle.normal.textColor = this.color;
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Connection State:", guistyle, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("ping to Server:", guistyle, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Space(20f);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("#", guistyle, new GUILayoutOption[0]);
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.playersPlaying[i])
			{
				GUILayout.Label(string.Empty + i, new GUILayoutOption[0]);
			}
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("Player", guistyle, new GUILayoutOption[0]);
		for (int j = 0; j < 4; j++)
		{
			if (!HeroController.playersPlaying[j] || HeroController.players[j] != null)
			{
			}
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("Ping", guistyle, new GUILayoutOption[0]);
		for (int k = 0; k < 4; k++)
		{
			if (HeroController.playersPlaying[k])
			{
			}
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("Lives", guistyle, new GUILayoutOption[0]);
		for (int l = 0; l < 4; l++)
		{
			if (HeroController.playersPlaying[l])
			{
				if (HeroController.players[l] != null)
				{
					GUILayout.Label(string.Empty + HeroController.players[l].Lives, new GUILayoutOption[0]);
				}
				else
				{
					GUILayout.Label("-", new GUILayoutOption[0]);
				}
			}
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("Character", guistyle, new GUILayoutOption[0]);
		for (int m = 0; m < 4; m++)
		{
			if (HeroController.playersPlaying[m])
			{
				if (HeroController.players[m] != null)
				{
					string text = string.Empty;
					if (HeroController.players[m].pendingRespawn)
					{
						GUILayout.Label("Waitin On Host", new GUILayoutOption[0]);
					}
					else if (HeroController.players[m].character != null)
					{
						text = HeroController.players[m].character.heroType.ToString();
						if (HeroController.players[m].character.actionState == ActionState.Dead)
						{
							text += " (Dead)";
						}
					}
					else
					{
						text = "Gibbed";
					}
					GUILayout.Label(text, new GUILayoutOption[0]);
				}
				else
				{
					GUILayout.Label("Joining", new GUILayoutOption[0]);
				}
			}
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.Space(160f);
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label(this.warningMessage, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	private Rect WindowRect = new Rect(0f, 0f, 600f, 450f);

	private bool show;

	public Color color;

	public string warningMessage = "Loading Map (this may take a while)";

	public static TabScreen instance;

	public GUISkin pixelSkin;
}
