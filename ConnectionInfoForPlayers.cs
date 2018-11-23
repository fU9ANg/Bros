// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ConnectionInfoForPlayers : MonoBehaviour
{
	private void OnLevelWasLoaded()
	{
		this.show = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			this.show = !this.show;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this.show = false;
		}
	}

	private void OnGUI()
	{
		if (!this.show)
		{
			return;
		}
		if (Connect.IsOffline || !LevelSelectionController.IsCampaignScene)
		{
			return;
		}
		GUI.skin = this.skin;
		GUILayout.Window(12, new Rect(0f, 0f, 1920f, 1080f), new GUI.WindowFunction(this.Window), string.Empty, new GUILayoutOption[0]);
	}

	private void Window(int id)
	{
		float d = (float)Screen.width / 1920f;
		GUIUtility.ScaleAroundPivot(Vector3.one * d, new Vector2(0f, 0f) / 2f);
		float num = 100f;
		GUILayout.Space(200f);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("#", new GUILayoutOption[0]);
		for (int i = 0; i < 4; i++)
		{
			this.SetColor(i);
			GUILayout.Label(string.Empty + i, new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
		GUILayout.Space(num);
		this.SetColor(-1);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("NAME", new GUILayoutOption[0]);
		for (int j = 0; j < 4; j++)
		{
			this.SetColor(j);
			string playerName = HeroController.PIDS[j].PlayerName;
			GUILayout.Label(playerName, new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
		GUILayout.Space(num);
		this.SetColor(-1);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("PING", new GUILayoutOption[0]);
		for (int k = 0; k < 4; k++)
		{
			string text = "--";
			this.SetColor(k);
			if (HeroController.PIDS[k] != PID.NoID)
			{
				if (HeroController.PIDS[k].IsMine)
				{
					text = "(local)";
				}
				else
				{
					int num2 = Mathf.RoundToInt(HeroController.PIDS[k].RawPing * 1000f);
					int num3 = Mathf.RoundToInt(HeroController.PIDS[k].Ping * 1000f);
					int num4 = Mathf.RoundToInt(PingController.GetInterpolationOffset(HeroController.PIDS[k]) * 1000f);
					text = string.Concat(new object[]
					{
						num3,
						"  (",
						num2,
						":",
						num4,
						")"
					});
				}
			}
			GUILayout.Label(text, new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
		GUILayout.Space(num);
		this.SetColor(-1);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("STATUS", new GUILayoutOption[0]);
		for (int l = 0; l < 4; l++)
		{
			this.SetColor(l);
			if (HeroController.playersPlaying[l])
			{
				string text2 = "LOADING MAP";
				if (HeroController.players[l] != null)
				{
					text2 = "NO HERO";
					if (HeroController.players[l].character != null)
					{
						text2 = string.Empty + HeroController.GetHeroName(HeroController.players[l].heroType).ToUpper();
						if (!HeroController.players[l].IsAlive())
						{
							text2 += "(DEAD)";
						}
					}
				}
				GUILayout.Label(text2, new GUILayoutOption[0]);
			}
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.Space(num);
		GUILayout.Space(num);
		this.SetColor(-1);
		GUI.skin = this.skinSmall;
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("GAME: ", new GUILayoutOption[0]);
		GUILayout.Label("LEVEL: ", new GUILayoutOption[0]);
		GUILayout.Label("SESSION: ", new GUILayoutOption[0]);
		GUILayout.Label("SEED: ", new GUILayoutOption[0]);
		GUILayout.EndVertical();
		GUILayout.Space(num / 2f);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label(string.Empty + Connect.GameName, new GUILayoutOption[0]);
		GUILayout.Label(string.Concat(new object[]
		{
			string.Empty,
			Connect.GetCampaignName(),
			"(",
			LevelSelectionController.CurrentLevelNum,
			")"
		}).ToUpper(), new GUILayoutOption[0]);
		GUILayout.Label(string.Empty + Connect.SessionID, new GUILayoutOption[0]);
		GUILayout.Label(string.Empty + Networking.RandomSeed, new GUILayoutOption[0]);
		GUILayout.EndVertical();
		GUILayout.Space(num * 3f);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("OFFLINE: ", new GUILayoutOption[0]);
		GUILayout.Label("STATE: ", new GUILayoutOption[0]);
		GUILayout.Label("KB/S IN: ", new GUILayoutOption[0]);
		GUILayout.Label("KB/S OUT: ", new GUILayoutOption[0]);
		GUILayout.Label("PACKET LOSS: ", new GUILayoutOption[0]);
		GUILayout.EndVertical();
		GUILayout.Space(num / 2f);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label((string.Empty + Connect.IsOffline).ToUpper(), new GUILayoutOption[0]);
		GUILayout.Label((string.Empty + ConnectionLayer.connectionState).ToUpper(), new GUILayoutOption[0]);
		GUILayout.Label(string.Empty + BNetwork.KbReceived, new GUILayoutOption[0]);
		GUILayout.Label(string.Empty + BNetwork.KbSent, new GUILayoutOption[0]);
		if (BNetwork.iNeteworkFacade != null)
		{
			GUILayout.Label(string.Empty + BNetwork.iNeteworkFacade.AveragePacketLossRate, new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	private void SetColor(int playerNum)
	{
		if (playerNum >= 0 && playerNum < 4)
		{
			GUI.color = HeroController.GetHeroColor(playerNum);
		}
		else
		{
			GUI.color = Color.white;
		}
	}

	public GUISkin skin;

	public GUISkin skinSmall;

	private bool show;
}
