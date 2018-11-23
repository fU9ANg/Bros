// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ManagedSteam;
using ManagedSteam.CallbackStructures;
using ManagedSteam.SteamTypes;
using UnityEngine;

public class SteamLayer : ConnectionLayer
{
	public SteamLayer()
	{
		SteamLayer.Instance = this;
		SteamLayer.matchMaking.LobbyMatchListResult += this.RecieveLobbyList_Callback;
		SteamLayer.matchMaking.LobbyCreatedResult += this.LobbyCreated_Callback;
		SteamLayer.matchMaking.LobbyEnterResult += this.LobbyJoined_Callback;
		SteamLayer.matchMaking.LobbyChatUpdate += this.LobbyChatUpdate_Callback;
		SteamLayer.matchMaking.LobbyChatMsg += this.LobbyChatMsg_Callback;
		SteamLayer.matchMaking.LobbyKicked += this.KickedFromLobby_Callback;
		SteamLayer.matchMaking.LobbyGameCreated += this.LobbyGameCreated_Callback;
		SteamLayer.matchMaking.LobbyDataUpdate += this.LobbyDataUpdate_Callback;
		SteamLayer.networking.P2PSessionRequest += this.P2PSessionRequest_Callback;
		SteamLayer.matchMaking.RequestLobbyList();
		UnityEngine.Debug.Log("Invalid ID " + SteamID.Invalid);
	}

	public static SteamID steamID
	{
		get
		{
			return Steam.Instance.User.GetSteamID();
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

	public override bool IsOffline
	{
		get
		{
			return SteamLayer.Instance == null || false || this.Match == SteamID.Invalid;
		}
	}

	public override bool IsHost
	{
		get
		{
			return this.match == SteamID.Invalid || Steam.Instance.User.GetSteamID() == SteamLayer.matchMaking.GetLobbyOwner(this.match);
		}
	}

	public override bool IsDC(PID pid)
	{
		SteamID steamID = SteamLayer.GetSteamID(pid);
		List<SteamID> memeberList = this.GetMemeberList();
		if (!memeberList.Contains(steamID))
		{
			UnityEngine.Debug.Log(pid + "Is not contained");
			return true;
		}
		return false;
	}

	private List<SteamID> GetMemeberList()
	{
		List<SteamID> list = new List<SteamID>();
		int numLobbyMembers = SteamLayer.matchMaking.GetNumLobbyMembers(this.match);
		for (int i = 0; i < numLobbyMembers; i++)
		{
			SteamID lobbyMemberByIndex = SteamLayer.matchMaking.GetLobbyMemberByIndex(this.match, i);
			list.Add(lobbyMemberByIndex);
		}
		return list;
	}

	public override void JoinMatch(GameInfo gameRoom)
	{
		if (this.match != SteamID.Invalid)
		{
			SteamLayer.matchMaking.LeaveLobby(this.match);
		}
		SteamGameInfo steamGameInfo = gameRoom as SteamGameInfo;
		SteamLayer.matchMaking.JoinLobby(steamGameInfo.Match);
		DebugLobby.state = GameState.JoiningMatch;
	}

	public override void FindMatch()
	{
		SteamLayer.matchMaking.RequestLobbyList();
		DebugLobby.state = GameState.FindingMatch;
		UnityEngine.Debug.Log("Find match");
	}

	private static SteamID GetSteamID(PID pid)
	{
		if (Connect.Layer.playerIdPairs.ContainsKey(pid))
		{
			ulong value = ulong.Parse(Connect.Layer.playerIdPairs[pid].UnderlyingID);
			return new SteamID(value);
		}
		return SteamID.Invalid;
	}

	public override void SendData(PID pid, byte[] bytes)
	{
		if (this.Match == SteamID.Invalid)
		{
			return;
		}
		SteamID steamID = SteamLayer.GetSteamID(pid);
		int dataSize = bytes.Length;
		IntPtr data = GCHandle.Alloc(bytes, GCHandleType.Pinned).AddrOfPinnedObject();
		SteamLayer.networking.SendP2PPacket(steamID, data, (uint)dataSize, P2PSend.Reliable, 0);
	}

	public SteamID Match
	{
		get
		{
			return this.match;
		}
	}

	public static bool MatchJoined
	{
		get
		{
			return SteamLayer.Instance.matchJoined;
		}
	}

	public override IDWrapper MyNetworkLayerID
	{
		get
		{
			return new SteamIDWrapper(Steam.Instance.User.GetSteamID(), string.Empty);
		}
	}

	private IEnumerator ReconnectInASecond()
	{
		if (this.reconnectionAttempts < 2)
		{
			yield return new WaitForSeconds(2f);
			ChatSystem.AddMessage("Attempting reconnection", PID.NoID, Connect.Timef);
			if (LevelSelectionController.IsCampaignScene)
			{
				UnityEngine.Debug.Log("> Attempting Reconnection");
				this.reconnectionAttempts++;
				Connect.FullReset();
			}
		}
		yield break;
	}

	public override void CreateMatch()
	{
		SteamLayer.matchMaking.CreateLobby(LobbyType.Public, 4);
	}

	public override void LeaveMatch()
	{
		UnityEngine.Debug.Log("> Leave match " + this.match);
		if (this.match != SteamID.Invalid)
		{
			SteamLayer.matchMaking.LeaveLobby(this.match);
		}
		this.match = SteamID.Invalid;
		this.matchJoined = false;
		DebugLobby.state = GameState.Lobby;
		ConnectionLayer.connectionState = ConnectionState.Disconnected;
	}

	private void FirstContact()
	{
		int numLobbyMembers = SteamLayer.matchMaking.GetNumLobbyMembers(this.match);
		for (int i = 0; i < numLobbyMembers; i++)
		{
			SteamID lobbyMemberByIndex = SteamLayer.matchMaking.GetLobbyMemberByIndex(this.match, i);
			if (lobbyMemberByIndex != SteamLayer.steamID)
			{
				byte[] array = new byte[2];
				array[0] = byte.MaxValue;
				byte[] array2 = array;
				int dataSize = array2.Length;
				IntPtr data = GCHandle.Alloc(array2, GCHandleType.Pinned).AddrOfPinnedObject();
				SteamLayer.networking.SendP2PPacket(lobbyMemberByIndex, data, (uint)dataSize, P2PSend.Reliable, 0);
			}
		}
		SteamLayer.matchMaking.SendLobbyChatMsg(this.match, new byte[1]);
	}

	private void LobbyJoined_Callback(LobbyEnter value, bool ioFailure)
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"LobbyJoinedResult ",
			ioFailure,
			" ",
			value
		}));
		if (ioFailure)
		{
			UnityEngine.Debug.Log("> FAILURE " + value);
		}
		this.match = value.SteamIDLobby;
		UnityEngine.Debug.Log(" matchMaking.GetLobbyOwner(match)  " + SteamLayer.matchMaking.GetLobbyOwner(this.match));
		DebugLobby.state = GameState.JoinedMatch;
		ConnectionLayer.connectionState = ConnectionState.Connected;
		ConnectionLayer.PlayerHasJoinedMatch(new SteamIDWrapper(SteamLayer.steamID, "not set"));
		this.FirstContact();
	}

	private void LobbyDataUpdate_Callback(LobbyDataUpdate value)
	{
		UnityEngine.Debug.Log("LobbyDataUpdate " + value);
		UnityEngine.Debug.Log("SteamIDMember " + value.SteamIDMember);
		UnityEngine.Debug.Log("SteamIDLobby " + value.SteamIDLobby);
		UnityEngine.Debug.Log("Success " + value.Success);
		Connect.ClearDCPlayers();
	}

	private void LobbyGameCreated_Callback(LobbyGameCreated value)
	{
		UnityEngine.Debug.Log("LobbyCreated " + value.SteamIDGameServer);
	}

	private void LobbyCreated_Callback(LobbyCreated value, bool ioFailure)
	{
		if (ioFailure)
		{
			UnityEngine.Debug.Log("> FAILURE " + value);
		}
		this.match = value.SteamIDLobby;
		UnityEngine.Debug.Log(" matchMaking.GetLobbyOwner(match)  " + SteamLayer.matchMaking.GetLobbyOwner(this.match));
		UnityEngine.Debug.Log("Matchmaking_LobbyCreatedResult " + ioFailure);
		SteamLayer.matchMaking.SetLobbyData(value.SteamIDLobby, "EncodedData", GameInfo.EncodeGameInfo());
		SteamLayer.matchMaking.RequestLobbyList();
		DebugLobby.state = GameState.JoinedMatch;
		ConnectionLayer.connectionState = ConnectionState.Hosting;
		ConnectionLayer.PlayerHasJoinedMatch(new SteamIDWrapper(SteamLayer.steamID, "not set"));
	}

	private void RecieveLobbyList_Callback(LobbyMatchList resultEvent, bool failure)
	{
		if (failure)
		{
			UnityEngine.Debug.Log("> FAILURE " + resultEvent);
		}
		ConnectionLayer.matchList.Clear();
		int num = 0;
		while ((long)num < (long)((ulong)resultEvent.LobbiesMatching))
		{
			SteamID lobbyByIndex = SteamLayer.matchMaking.GetLobbyByIndex(num);
			ConnectionLayer.matchList.Add(new SteamGameInfo(lobbyByIndex));
			num++;
		}
		ConnectionLayer.matchQueryHandled = true;
		if (SingletonMono<LobbyMenu>.Instance != null)
		{
			SingletonMono<LobbyMenu>.Instance.RefreshGameList(true);
		}
	}

	private void KickedFromLobby_Callback(LobbyKicked value)
	{
		UnityEngine.Debug.Log("KickedFromLobby_Callback  " + value);
		this.match = SteamID.Invalid;
		DebugLobby.state = GameState.Lobby;
		ConnectionLayer.connectionState = ConnectionState.Disconnected;
	}

	private void LobbyChatMsg_Callback(LobbyChatMsg value)
	{
		UnityEngine.Debug.Log("LobbyChatMsg_Callback  " + value);
		byte[] array = new byte[100];
		UnityEngine.Debug.Log(SteamLayer.matchMaking.GetLobbyChatEntry(this.match, (int)value.ChatID, array).Result);
		string @string = Encoding.ASCII.GetString(array);
		UnityEngine.Debug.Log(@string);
	}

	private void LobbyChatUpdate_Callback(LobbyChatUpdate value)
	{
		Connect.ClearDCPlayers();
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"LobbyChatUpdate_Callback  --------------------------------------",
			value.SteamIDUserChanged,
			" ",
			value.ChatMemberStateChange
		}));
	}

	private void P2PSessionRequest_Callback(P2PSessionRequest value)
	{
		UnityEngine.Debug.Log("P2PSessionRequest_Callback from " + value.SteamIDRemote);
		SteamLayer.networking.AcceptP2PSessionWithUser(value.SteamIDRemote);
		this.FirstContact();
	}

	public override void ProcessNetworkState()
	{
	}

	public override void Update()
	{
		base.Update();
		for (int i = 0; i < 200; i++)
		{
			try
			{
				if (!SteamLayer.networking.IsP2PPacketAvailable(0).Result)
				{
					break;
				}
				byte[] array = new byte[SteamLayer.networking.IsP2PPacketAvailable(0).MsgSize];
				SteamID steamIDRemote = SteamLayer.networking.ReadP2PPacket(array, 0).SteamIDRemote;
				IDWrapper idwrapper = new SteamIDWrapper(steamIDRemote, string.Empty);
				if (!this.playerIdPairs.ContainsValue(idwrapper))
				{
					UnityEngine.Debug.Log("Player not registered " + idwrapper);
					ConnectionLayer.PlayerHasJoinedMatch(idwrapper);
				}
				ConnectionLayer.RecieveBytes(array);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
	}

	public List<SteamID> lobbies = new List<SteamID>();

	public static SteamLayer Instance;

	private SteamID match = SteamID.Invalid;

	private bool matchJoined;

	private MatchController controller;

	private int reconnectionAttempts;
}
