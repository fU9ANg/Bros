// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using Badumna.Autoreplication.Serialization;
using Badumna.Match;
using UnityEngine;

public class BadumnaLayer : ConnectionLayer
{
	public BadumnaLayer()
	{
		BadumnaLayer.Instance = this;
	}

	public override bool IsOffline
	{
		get
		{
			return BadumnaLayer.Instance == null || this.Match == null;
		}
	}

	public override bool IsHost
	{
		get
		{
			return SingletonMono<BadumnaController>.Instance.IsHosting;
		}
	}

	public override bool IsDC(PID pid)
	{
		MemberIdentity memberID = MatchController.GetMemberID(pid);
		if (memberID == (MemberIdentity)null)
		{
			UnityEngine.Debug.Log(pid + "Is null");
			return true;
		}
		List<MemberIdentity> list = new List<MemberIdentity>(this.Match.Members);
		if (!list.Contains(memberID))
		{
			UnityEngine.Debug.Log(pid + "Is not contained");
			return true;
		}
		return false;
	}

	public override bool IsLoggedIn
	{
		get
		{
			return BNetwork.iNeteworkFacade != null && BNetwork.iNeteworkFacade.IsLoggedIn;
		}
	}

	public override void JoinMatch(GameInfo gameRoom)
	{
		BadumnaGameInfo badumnaGameInfo = gameRoom as BadumnaGameInfo;
		this.JoinMatch(badumnaGameInfo.room);
	}

	public override void FindMatch()
	{
		MatchmakingCriteria criteria = new MatchmakingCriteria
		{
			PlayerGroup = 0
		};
		ConnectionLayer.matchQueryHandled = false;
		DebugLobby.state = GameState.FindingMatch;
		BNetwork.iNeteworkFacade.Match.FindMatches(criteria, new MatchmakingResultHandler(this.RecieveLobbyListingCallback));
	}

	public override void SendData(PID pid, byte[] bytes)
	{
		if (BadumnaLayer.Instance == null || this.Match == null || !BadumnaLayer.MatchJoined)
		{
			return;
		}
		if (this.Match != null && this.Match.Controller != null)
		{
			MemberIdentity memberID = MatchController.GetMemberID(pid);
			if (memberID != (MemberIdentity)null)
			{
				this.Match.CallMethodOnMember<byte[]>(memberID, new Badumna.Autoreplication.Serialization.RpcSignature<byte[]>(this.Match.Controller.Recieve), bytes);
			}
			else
			{
				UnityEngine.Debug.LogWarning("MemberID is null " + pid);
			}
		}
	}

	private void RecieveLobbyListingCallback(MatchmakingQueryResult result)
	{
		ConnectionLayer.matchList.Clear();
		foreach (MatchmakingResult room in result.Results)
		{
			ConnectionLayer.matchList.Add(new BadumnaGameInfo(room));
		}
		ConnectionLayer.matchQueryHandled = true;
		if (result.Error != MatchError.None)
		{
			UnityEngine.Debug.LogError("Error: " + result.Error.ToString());
		}
		if (SingletonMono<LobbyMenu>.Instance != null)
		{
			SingletonMono<LobbyMenu>.Instance.RefreshGameList(true);
		}
	}

	public Match<MatchController> Match
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
			return BadumnaLayer.Instance.matchJoined;
		}
	}

	public override IDWrapper MyNetworkLayerID
	{
		get
		{
			return new BadumnaIDWrapper(BadumnaLayer.Instance.match.MemberIdentity);
		}
	}

	private void OnMatchStatusChange(Match match, MatchStatusEventArgs e)
	{
		UnityEngine.Debug.Log("> OnMatchStatusChange " + e.Status.ToString());
		Analytics.PrintKbpsLog();
		if (e.Status == MatchStatus.Closed)
		{
			SingletonMono<Connect>.Instance.BroadcastMessageToAllMonoBehaviours("OnMatchConnectionClosed");
			if (this.match != null)
			{
				this.match.Controller.MigrateHostRPC(PID.MyID, false);
			}
			this.match = null;
			DebugLobby.state = GameState.Lobby;
			ConnectionLayer.connectionState = ConnectionState.Disconnected;
			SingletonMono<BadumnaController>.Instance.IsHosting = true;
		}
		else if (e.Status == MatchStatus.Hosting || e.Status == MatchStatus.Connected)
		{
			this.reconnectionAttempts = 0;
			this.matchJoined = true;
			SingletonMono<BadumnaController>.Instance.IsHosting = (e.Status != MatchStatus.Connected);
			DebugLobby.state = GameState.JoinedMatch;
			UnityEngine.Debug.Log("> BadumnaController.Instance.IsHosting " + SingletonMono<BadumnaController>.Instance.IsHosting);
		}
		if (e.Status == MatchStatus.Hosting)
		{
			ConnectionLayer.connectionState = ConnectionState.Hosting;
			this.match.Controller.MigrateHostRPC(PID.MyID, true);
			this.match.CallMethodOnMembers<PID, bool>(new Badumna.Autoreplication.Serialization.RpcSignature<PID, bool>(this.match.Controller.MigrateHostRPC), PID.MyID, true);
		}
		if (e.Status == MatchStatus.Connected)
		{
			ConnectionLayer.connectionState = ConnectionState.Connected;
		}
	}

	public override void ProcessNetworkState()
	{
		if (BNetwork.iNeteworkFacade == null || !BNetwork.iNeteworkFacade.IsLoggedIn)
		{
			return;
		}
		BNetwork.iNeteworkFacade.ProcessNetworkState();
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
				Connect.Layer.JoinMatch(LobbyMenu.matchToJoin);
			}
		}
		yield break;
	}

	public override void CreateMatch()
	{
		DebugLobby.state = GameState.CreatingMatch;
		MatchmakingCriteria criteria = new MatchmakingCriteria
		{
			PlayerGroup = 0,
			MatchName = GameInfo.EncodeGameInfo()
		};
		this.controller = new MatchController();
		this.match = BNetwork.iNeteworkFacade.Match.CreateMatch<MatchController>(this.controller, criteria, 4, Connect.PlayerName, null, null);
		this.match.StateChanged += this.OnMatchStatusChange;
		this.match.MemberAdded += MatchController.AddMember;
		this.match.MemberRemoved += this.controller.RemoveMember;
		SingletonMono<BadumnaController>.Instance.IsHosting = true;
		UnityEngine.Debug.Log("> Create Match " + Connect.GameName);
	}

	public void JoinMatch(MatchmakingResult matchToJoin)
	{
		Connect.FullReset();
		Connect.GameName = new BadumnaGameInfo(matchToJoin).HostName;
		UnityEngine.Debug.Log("> Join Match " + Connect.GameName);
		SingletonMono<BadumnaController>.Instance.IsHosting = false;
		this.matchJoined = false;
		DebugLobby.state = GameState.JoiningMatch;
		this.controller = new MatchController();
		this.match = BNetwork.iNeteworkFacade.Match.JoinMatch<MatchController>(this.controller, matchToJoin, Connect.PlayerName, null, null);
		this.match.StateChanged += this.OnMatchStatusChange;
		this.match.MemberAdded += MatchController.AddMember;
		this.match.MemberRemoved += this.controller.RemoveMember;
	}

	public override void LeaveMatch()
	{
		this.matchJoined = false;
		if (this.match != null)
		{
			this.match.Leave();
			this.match = null;
		}
		DebugLobby.state = GameState.Lobby;
		SingletonMono<BadumnaController>.Instance.IsHosting = true;
	}

	public static BadumnaLayer Instance;

	private Match<MatchController> match;

	private bool matchJoined;

	private MatchController controller;

	private int reconnectionAttempts;
}
