// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ManagedSteam;
using ManagedSteam.CallbackStructures;
using ManagedSteam.SteamTypes;
using UnityEngine;

public class SteamNetworking : MonoBehaviour
{
	public static SteamID MyID
	{
		get
		{
			if (SteamController.SteamInterface == null)
			{
				return SteamID.Invalid;
			}
			return SteamController.SteamInterface.User.GetSteamID();
		}
	}

	public static IMatchmaking matchMaking
	{
		get
		{
			if (SteamController.SteamInterface == null)
			{
				return null;
			}
			return SteamController.SteamInterface.Matchmaking;
		}
	}

	public static INetworking networking
	{
		get
		{
			if (SteamController.SteamInterface == null)
			{
				return null;
			}
			return SteamController.SteamInterface.Networking;
		}
	}

	private void Start()
	{
		this.lobbyName = "Lobby " + UnityEngine.Random.Range(0, 99);
		SteamNetworking.matchMaking.LobbyMatchListResult += this.RecieveLobbyList_Callback;
		SteamNetworking.matchMaking.LobbyCreatedResult += this.LobbyCreated_Callback;
		SteamNetworking.matchMaking.LobbyEnterResult += this.LobbyJoined_Callback;
		SteamNetworking.matchMaking.LobbyChatUpdate += this.LobbyChatUpdate_Callback;
		SteamNetworking.matchMaking.LobbyChatMsg += this.LobbyChatMsg_Callback;
		SteamNetworking.matchMaking.LobbyKicked += this.KickedFromLobby_Callback;
		SteamNetworking.networking.P2PSessionRequest += this.P2PSessionRequest_Callback;
		SteamNetworking.matchMaking.RequestLobbyList();
		MonoBehaviour.print("Invalid ID " + SteamID.Invalid);
	}

	private void OnGUI()
	{
		this.DrawLobbies();
		this.DrawChatGUI();
	}

	private void DrawLobbies()
	{
		GUILayout.BeginArea(new Rect(0f, 0f, (float)(Screen.width / 3), (float)Screen.height));
		GUILayout.Label("Steam ID: " + SteamNetworking.MyID, new GUILayoutOption[0]);
		GUILayout.Label("Current Lobby: " + this.currentLobby.ToString(), new GUILayoutOption[0]);
		this.lobbyName = GUILayout.TextField(this.lobbyName, new GUILayoutOption[0]);
		if (GUILayout.Button("Create Lobby", new GUILayoutOption[0]))
		{
			SteamNetworking.matchMaking.CreateLobby(LobbyType.Public, 4);
		}
		if (GUILayout.Button("Refresh", new GUILayoutOption[0]))
		{
			SteamNetworking.matchMaking.RequestLobbyList();
		}
		if (this.lobbies.Count == 0)
		{
			GUILayout.Label("No Lobbies available", new GUILayoutOption[0]);
		}
		else
		{
			for (int i = 0; i < this.lobbies.Count; i++)
			{
				SteamID steamID = this.lobbies[i];
				string lobbyData = SteamNetworking.matchMaking.GetLobbyData(steamID, "lobbyName");
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.Label(string.Concat(new object[]
				{
					i,
					") ",
					lobbyData,
					" ",
					steamID
				}), new GUILayoutOption[0]);
				if (GUILayout.Button("Join", new GUILayoutOption[0]))
				{
					if (this.currentLobby != SteamID.Invalid)
					{
						SteamNetworking.matchMaking.LeaveLobby(this.currentLobby);
					}
					SteamNetworking.matchMaking.JoinLobby(steamID);
				}
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.EndArea();
	}

	private void DrawChatGUI()
	{
		GUILayout.BeginArea(new Rect((float)(Screen.width * 2 / 3), 0f, (float)(Screen.width / 3), (float)Screen.height));
		this.chatMessage = GUILayout.TextArea(this.chatMessage, new GUILayoutOption[]
		{
			GUILayout.Height(100f)
		});
		if (this.chatMessage.Length > 100)
		{
			this.chatMessage = this.chatMessage.Substring(0, 100);
		}
		if (GUILayout.Button("Send Message", new GUILayoutOption[0]) && this.chatMessage.Length > 0)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(this.chatMessage);
			SteamNetworking.matchMaking.SendLobbyChatMsg(this.currentLobby, bytes);
			this.chatMessage = string.Empty;
		}
		if (GUILayout.Button("Send Packet", new GUILayoutOption[0]))
		{
			this.SendPacket();
		}
		foreach (string text in this.chatMessages)
		{
			GUILayout.Label(text, new GUILayoutOption[0]);
		}
		GUILayout.EndArea();
	}

	private void SendPacket()
	{
		if (this.currentLobby == SteamID.Invalid)
		{
			return;
		}
		int numLobbyMembers = SteamNetworking.matchMaking.GetNumLobbyMembers(this.currentLobby);
		for (int i = 0; i < numLobbyMembers; i++)
		{
			SteamID lobbyMemberByIndex = SteamNetworking.matchMaking.GetLobbyMemberByIndex(this.currentLobby, i);
			string obj = "Rise, our Father Lucifer. From " + SteamNetworking.MyID;
			byte[] array = TypeSerializer.ObjectToByteArray<string>(obj);
			int dataSize = array.Length;
			MonoBehaviour.print(string.Concat(new object[]
			{
				"Send to ",
				lobbyMemberByIndex,
				" ",
				array.Length
			}));
			IntPtr data = GCHandle.Alloc(array, GCHandleType.Pinned).AddrOfPinnedObject();
			SteamNetworking.networking.SendP2PPacket(lobbyMemberByIndex, data, (uint)dataSize, P2PSend.Reliable, 0);
		}
	}

	private void LobbyJoined_Callback(LobbyEnter value, bool ioFailure)
	{
		MonoBehaviour.print(string.Concat(new object[]
		{
			"LobbyJoinedResult ",
			ioFailure,
			" ",
			value
		}));
		this.currentLobby = value.SteamIDLobby;
	}

	private void LobbyCreated_Callback(LobbyCreated value, bool ioFailure)
	{
		MonoBehaviour.print("Matchmaking_LobbyCreatedResult " + ioFailure);
		SteamNetworking.matchMaking.SetLobbyData(value.SteamIDLobby, "lobbyName", this.lobbyName);
		SteamNetworking.matchMaking.RequestLobbyList();
		this.currentLobby = value.SteamIDLobby;
	}

	private void RecieveLobbyList_Callback(LobbyMatchList resultEvent, bool failure)
	{
		this.lobbies.Clear();
		MonoBehaviour.print("RecieveLobbyList " + resultEvent.LobbiesMatching);
		int num = 0;
		while ((long)num < (long)((ulong)resultEvent.LobbiesMatching))
		{
			SteamID lobbyByIndex = SteamNetworking.matchMaking.GetLobbyByIndex(num);
			this.lobbies.Add(lobbyByIndex);
			num++;
		}
	}

	private void KickedFromLobby_Callback(LobbyKicked value)
	{
		MonoBehaviour.print("KickedFromLobby_Callback  " + value);
	}

	private void LobbyChatMsg_Callback(LobbyChatMsg value)
	{
		MonoBehaviour.print("LobbyChatMsg_Callback  " + value);
		byte[] array = new byte[100];
		MonoBehaviour.print(SteamNetworking.matchMaking.GetLobbyChatEntry(this.currentLobby, (int)value.ChatID, array).Result);
		string @string = Encoding.ASCII.GetString(array);
		MonoBehaviour.print(@string);
		this.chatMessages.Add(@string);
	}

	private void LobbyChatUpdate_Callback(LobbyChatUpdate value)
	{
		MonoBehaviour.print(string.Concat(new object[]
		{
			"LobbyChatUpdate_Callback  ",
			value.ChatMemberStateChange,
			" ",
			value.ChatMemberStateChange
		}));
	}

	private void P2PSessionRequest_Callback(P2PSessionRequest value)
	{
		MonoBehaviour.print("> P2PSessionRequest_Callback from " + value.SteamIDRemote);
		SteamNetworking.networking.AcceptP2PSessionWithUser(value.SteamIDRemote);
	}

	public List<SteamID> lobbies = new List<SteamID>();

	private SteamID currentLobby = SteamID.Invalid;

	private string lobbyName = string.Empty;

	private List<string> chatMessages = new List<string>();

	private string chatMessage = string.Empty;
}
