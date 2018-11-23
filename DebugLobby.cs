// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLobby : SingletonMono<DebugLobby>
{
	private IEnumerator ConnectionFeedBack()
	{
		int counter = 0;
		int max = 10;
		for (;;)
		{
			if (counter == max)
			{
				yield return new WaitForSeconds(0.5f);
			}
			counter++;
			counter %= max + 1;
			this.FeedBack = string.Empty;
			for (int i = 0; i < max; i++)
			{
				if (i == counter)
				{
					this.FeedBack += "0";
				}
				else
				{
					this.FeedBack += "1";
				}
			}
			yield return new WaitForSeconds(0.08f);
		}
		yield break;
	}

	private void Start()
	{
		base.StartCoroutine(this.ConnectionFeedBack());
		DebugLobby.leveToSkipTo = LevelSelectionController.CurrentLevelNum + string.Empty;
	}

	public void DrawLobbyInfo()
	{
		GUILayout.Label(BNetwork.status, new GUILayoutOption[0]);
		if (!Connect.Layer.IsLoggedIn)
		{
			Connect.PlayerName = GUILayout.TextField(Connect.PlayerName, new GUILayoutOption[0]);
			if (GUILayout.Button("Try Login as " + Connect.PlayerName, new GUILayoutOption[0]))
			{
				BNetwork.TryLogin();
			}
		}
		else
		{
			this.LobbyWindow();
		}
		List<PID> playerIDList = Connect.playerIDList;
		foreach (PID pid in playerIDList)
		{
			if (!pid.IsMine && GUILayout.Button("Kick " + pid.PlayerName, new GUILayoutOption[0]))
			{
				Connect.SendKick(pid);
			}
		}
		GUILayout.Label(string.Empty, new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		DebugLobby.leveToSkipTo = GUILayout.TextField(DebugLobby.leveToSkipTo, new GUILayoutOption[]
		{
			GUILayout.Width(30f)
		});
		this.levelSkipPassword = GUILayout.TextField(this.levelSkipPassword, new GUILayoutOption[]
		{
			GUILayout.Width(100f)
		});
		if (GUILayout.Button("GO", new GUILayoutOption[]
		{
			GUILayout.Width(50f)
		}))
		{
			if (this.levelSkipPassword.ToLower() == "fucking werewolf")
			{
				int currentLevelNum = int.Parse(DebugLobby.leveToSkipTo);
				LevelSelectionController.CurrentLevelNum = currentLevelNum;
				GameModeController.RestartLevel();
			}
			GUI.SetNextControlName("FOCUS_OUT_UID");
			GUI.Label(new Rect(-100f, -100f, 1f, 1f), string.Empty);
			GUI.FocusControl("FOCUS_OUT_UID");
		}
		GUILayout.EndHorizontal();
	}

	public void LobbyWindow()
	{
		switch (DebugLobby.state)
		{
		case GameState.Lobby:
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUI.enabled = Connect.Layer.IsLoggedIn;
			GUILayout.Label("Match name:", new GUILayoutOption[0]);
			Connect.GameName = GUILayout.TextField(Connect.GameName, new GUILayoutOption[0]);
			if (GUILayout.Button("Create match", new GUILayoutOption[0]))
			{
				Connect.Layer.CreateMatch();
			}
			if (GUILayout.Button("Find match", new GUILayoutOption[0]))
			{
				Connect.Layer.FindMatch();
			}
			GUI.enabled = true;
			GUILayout.EndVertical();
			break;
		case GameState.CreatingMatch:
			GUILayout.Label("Creating match ...", new GUILayoutOption[0]);
			NetworkDebugger.Hide();
			break;
		case GameState.FindingMatch:
			if (ConnectionLayer.matchQueryHandled)
			{
				if (ConnectionLayer.matchList.Count == 0)
				{
					GUILayout.Label("No match found.", new GUILayoutOption[0]);
				}
				else
				{
					foreach (GameInfo gameInfo in ConnectionLayer.matchList)
					{
						if (gameInfo.Version == VersionNumber.version)
						{
							GUILayout.BeginHorizontal(new GUILayoutOption[0]);
							GUILayout.Label(string.Concat(new object[]
							{
								gameInfo.HostName,
								"   ",
								gameInfo.GameName,
								"   v",
								gameInfo.Version,
								"   ",
								gameInfo.Capacity
							}), new GUILayoutOption[0]);
							if (GUILayout.Button("Join", new GUILayoutOption[0]) && gameInfo.Version == VersionNumber.version)
							{
								Connect.Layer.JoinMatch(gameInfo);
								NetworkDebugger.Hide();
							}
							GUILayout.EndHorizontal();
						}
					}
				}
				GUILayout.Space(20f);
				if (GUILayout.Button("Back to lobby", new GUILayoutOption[0]))
				{
					DebugLobby.state = GameState.Lobby;
				}
			}
			else
			{
				GUILayout.Label("Searching for match ...", new GUILayoutOption[0]);
			}
			break;
		case GameState.JoiningMatch:
			GUILayout.Label("Joining match ...", new GUILayoutOption[0]);
			break;
		case GameState.JoinedMatch:
			if (GUILayout.Button("Leave Match", new GUILayoutOption[0]))
			{
				Connect.Disconnect();
			}
			break;
		}
	}

	private void RecieveMsg(string s)
	{
		MonoBehaviour.print(s);
		this.Message = s;
	}

	public static GameState state;

	private string FeedBack = "_________________";

	private string Message = string.Empty;

	public static string leveToSkipTo = string.Empty;

	private string levelSkipPassword = string.Empty;
}
