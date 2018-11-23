// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[RequireComponent(typeof(SyncController))]
[RequireComponent(typeof(InstantiationController))]
[RequireComponent(typeof(RPCController))]
[RequireComponent(typeof(NetworkTimeSync))]
[RequireComponent(typeof(Registry))]
[RequireComponent(typeof(PingController))]
public class Connect : SingletonMono<Connect>
{
	public static string PlayerName
	{
		get
		{
			string text = "Brononymous";
			try
			{
				if (PlayerOptions.Instance != null)
				{
					text = PlayerOptions.Instance.PlayerName;
				}
				if (text.Length > 12)
				{
					text = text.Substring(0, 12);
				}
			}
			catch (Exception message)
			{
				text = "Brononymous";
				UnityEngine.Debug.Log(message);
			}
			return text;
		}
		set
		{
			try
			{
				if (PlayerOptions.Instance != null)
				{
					PlayerOptions.Instance.playerName = value;
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.Log(message);
			}
		}
	}

	public static SID SessionID
	{
		get
		{
			return Connect.sessionID;
		}
	}

	public static float AddedLatency
	{
		get
		{
			return Connect.addedLatency;
		}
	}

	public static float SimulatedLatency
	{
		get
		{
			return Connect.simulatedLatency;
		}
	}

	public static bool BypassNetworkLayer
	{
		get
		{
			return SingletonMono<Connect>.Instance.bypassNetworkLayer && Application.isEditor;
		}
	}

	public static List<PID> playerIDList
	{
		get
		{
			return Connect.Layer.playerIDList;
		}
	}

	public static ConnectionLayer Layer
	{
		get
		{
			if (Connect.layer == null)
			{
				if (SingletonMono<Connect>.Instance.connectivityLayer == Connect.ConnectivityLayer.Steam)
				{
					Connect.layer = new SteamLayer();
				}
				else
				{
					Connect.layer = new BadumnaLayer();
				}
			}
			return Connect.layer;
		}
	}

	public static bool IsOffline
	{
		get
		{
			return Connect.Layer.IsOffline;
		}
	}

	public static bool IsHost
	{
		get
		{
			return Connect.Layer.IsHost;
		}
	}

	public static void SetAddedLatency(float latency)
	{
		Connect.addedLatency = Mathf.Max(0f, latency);
	}

	public static SID GetIncrementedSessionID()
	{
		SID sid = new SID(Connect.sessionID.AsByte);
		sid.Increment();
		return sid;
	}

	public static bool IsRichardsPC
	{
		get
		{
			return Connect.isRichardsPC;
		}
	}

	private void Awake()
	{
		try
		{
			if (Application.isEditor && Environment.UserName == "HAL 9000")
			{
				Connect.isRichardsPC = true;
			}
		}
		catch (Exception ex)
		{
		}
		if (Connect.addedLatency > 3f)
		{
			UnityEngine.Debug.LogWarning("Very high simulated latency " + Connect.addedLatency);
		}
		if (Connect.addedLatency > 0f)
		{
			UnityEngine.Debug.LogError(">>>>>> RUNNING WTH ADDED LATENCY " + Connect.addedLatency);
		}
		if (SingletonMono<Connect>.Instance != null && SingletonMono<Connect>.Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			MonoBehaviour.print("Destroying duplicate");
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Connect.GameName = "GAME " + NonDeterministicRandom.Range(0, 99);
		try
		{
			string locationInfo = Connect.GetLocationInfo();
			Connect.Country = locationInfo.Split(new char[]
			{
				'"'
			})[3];
			Connect.City = locationInfo.Split(new char[]
			{
				'"'
			})[11];
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception, this);
		}
	}

	public static void SendKick(PID playerBeingKicked)
	{
		UnityEngine.Debug.Log(" --- SendKick --- " + playerBeingKicked);
		Networking.RPC<PID>(playerBeingKicked, new RpcSignature<PID>(SingletonMono<Connect>.Instance.NotifyPlayerThatHeHasBeenKicked), PID.MyID, true);
		Networking.RPC<PID>(PID.TargetAll, new RpcSignature<PID>(SingletonMono<Connect>.Instance.NotifyOthersThatPlayerHasBeenKicked), playerBeingKicked, true);
	}

	private void NotifyPlayerThatHeHasBeenKicked(PID TheAssholeResponsible)
	{
		KickedNotification.Show(TheAssholeResponsible.PlayerName);
		Connect.Disconnect();
		UnityEngine.Debug.Log("> ---  YOU WERE KICKED FROM THE GAME BY --- " + TheAssholeResponsible.PlayerName);
	}

	private void NotifyOthersThatPlayerHasBeenKicked(PID kickedPlayer)
	{
		UnityEngine.Debug.Log("> NotifyOthersThatPlayerHasBeenKicked " + kickedPlayer.PlayerName);
		if (!kickedPlayer.IsMine)
		{
			this.DeregisterPlayer(kickedPlayer);
		}
	}

	public static string GetLocationInfo()
	{
		return new WebClient().DownloadString("http://api.hostip.info/get_json.php");
	}

	private void FixedUpdate()
	{
		this.SimulatedLatencyUpdateTimer -= UnityEngine.Time.fixedDeltaTime;
		if (this.SimulatedLatencyUpdateTimer < 0f)
		{
			Connect.simulatedLatency = Mathf.Max(0f, Connect.addedLatency + Connect.simulatedVariance * NonDeterministicRandom.Range(-1f, 1f));
			if (NonDeterministicRandom.value < Connect.spikeProbablity)
			{
				if (NonDeterministicRandom.value < 0.7f)
				{
					Connect.simulatedLatency *= 2f;
				}
				else
				{
					Connect.simulatedLatency /= 2f;
				}
			}
			this.SimulatedLatencyUpdateTimer += 0.5f;
		}
	}

	public static void SetApproximateTime(double serverTime)
	{
		UnityEngine.Debug.Log(">> [4] SetApproximateTime");
		NetworkTimeSync.InitializeDeltaTime(serverTime);
	}

	public static void RequestSyncedServerData(PID requestee)
	{
		Networking.RPC<double>(requestee, true, true, new RpcSignature<double>(Connect.SetApproximateTime), Connect.Time);
		UnityEngine.Debug.Log("> [6] RefreshSyncedServerData");
		Networking.RPC<int, int>(requestee, true, true, new RpcSignature<int, int>(Connect.SyncWithServer), (int)PID.ServerID, (int)Connect.SessionID);
		string campaignName = Connect.GetCampaignName();
		if (SingletonMono<MapController>.Instance != null)
		{
			Networking.RPC<int, string, int, string, int, int, bool, GridPos>(requestee, true, true, new RpcSignature<int, string, int, string, int, int, bool, GridPos>(Connect.SyncCampaign), Networking.RandomSeed, campaignName, LevelSelectionController.CurrentLevelNum, Application.loadedLevelName, Map.lastXLoadOffset, Map.lastYLoadOffset, Map.startFromSuperCheckPoint, Map.superCheckpointStartPos);
		}
		else
		{
			Networking.RPC<int, string, string, int>(requestee, true, true, new RpcSignature<int, string, string, int>(Connect.SyncJoinOrMissionScreen), Networking.RandomSeed, Application.loadedLevelName, campaignName, LevelSelectionController.CurrentLevelNum);
		}
	}

	public static void IDsAreSetup()
	{
		MonoBehaviour.print("> IDs are now set up " + PID.ServerID);
		Networking.RPC<PID>(PID.TargetServer, true, true, new RpcSignature<PID>(Connect.RequestSyncedServerData), PID.MyID);
	}

	public static void FullReset()
	{
		MonoBehaviour.print("> FullReset");
		List<PID> playerIDList = Connect.playerIDList;
		foreach (PID pid in playerIDList)
		{
			SingletonMono<Connect>.Instance.DeregisterPlayer(pid);
		}
		InstantiationController.DeregisterPlayer(PID.TargetServer);
		InstantiationController.Clear();
		Ack.Clear();
		PID.Reset();
		Connect.playerIDList.Clear();
		Connect.playerNameList.Clear();
		Connect.SetRandomSessionID();
		RPCBatcher.Reset();
		HeroController.ResetPlayersPlaying();
		SingletonMono<NetworkTimeSync>.Instance.Reset();
		SingletonMono<PingController>.Instance.Reset();
		RPCController.Instance.Reset();
		SingletonMono<PingController>.Instance.Reset();
		Connect.layer.Reset();
	}

	public static void RemoveAllOtherPlayers()
	{
		MonoBehaviour.print("> Connect Reset");
		List<PID> playerIDList = Connect.playerIDList;
		foreach (PID pid in playerIDList)
		{
			if (!pid.IsMine)
			{
				SingletonMono<Connect>.Instance.DeregisterPlayer(pid);
			}
		}
		PID.Reset();
	}

	public static string GetCampaignName()
	{
		string text = LevelSelectionController.campaignToLoad;
		if (LevelSelectionController.currentCampaign != null)
		{
			text = LevelSelectionController.currentCampaign.name;
		}
		if (text == string.Empty)
		{
			UnityEngine.Debug.LogError("Dont know what campaign to tell the player to load");
		}
		return text;
	}

	public static void SyncCampaign(int randomSeed, string currentCampaign, int CurrentLevelNum, string sceneToLoad, int lastXLoadOffset, int lastYLoadOffset, bool startFromSuperCheckPoint, GridPos superCheckpointStartPos)
	{
		MonoBehaviour.print(string.Concat(new object[]
		{
			"> [8] SyncCampaign - Host: ",
			Connect.IsHost,
			" - currentCampaign: ",
			currentCampaign,
			" - CurrentLevelNum: ",
			CurrentLevelNum,
			" - randomSeed: ",
			randomSeed
		}));
		if (!Connect.IsHost)
		{
			if (Networking.RandomSeed == randomSeed)
			{
				MonoBehaviour.print("Random randomSeed already equals!");
			}
			LevelSelectionController.ResetLevelAndGameModeToDefault();
			LevelSelectionController.loadCustomCampaign = false;
			LevelSelectionController.loadMode = MapLoadMode.Campaign;
			LevelSelectionController.campaignToLoad = currentCampaign;
			GameModeController.GameMode = GameMode.Campaign;
			LevelSelectionController.CurrentLevelNum = CurrentLevelNum;
			Map.nextLevelToLoad = CurrentLevelNum;
			Map.nextXLoadOffset = lastXLoadOffset;
			Map.nextYLoadOffset = lastYLoadOffset;
			Map.startFromSuperCheckPoint = startFromSuperCheckPoint;
			Map.superCheckpointStartPos = superCheckpointStartPos;
			Networking.SetSeed(randomSeed);
			if (!Connect.IsHost)
			{
				Networking.StreamIsPaused = true;
				Application.LoadLevel(sceneToLoad);
			}
		}
	}

	public void OnMatchConnectionClosed()
	{
		UnityEngine.Debug.Log("> On connection closed ");
		LevelTitle.ShowText("Disconnected", 0f);
		ChatSystem.AddMessage("Disconnected", PID.NoID, Connect.Timef);
	}

	public static void OnHostMigration(PID newServer, bool announce)
	{
		PID.SetServerID(newServer);
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"> Host has migrated  new host: ",
			PID.ServerID,
			" ",
			PID.IamServer()
		}));
		Ack.ResendServerRequests();
		if (announce)
		{
			ChatSystem.AddMessage(newServer.PlayerName + " is Host", PID.NoID, Connect.Timef);
		}
	}

	public static void SyncWithServer(int _serverID, int sessionIDAsInt)
	{
		MonoBehaviour.print(string.Concat(new object[]
		{
			"> [7] SyncWithServer ",
			_serverID,
			"  sessionIDAsInt: ",
			sessionIDAsInt
		}));
		Connect.SetSessionID((byte)sessionIDAsInt);
	}

	public static void SyncJoinOrMissionScreen(int randomSeed, string sceneToLoad, string CampaignToLoad, int currentlevelNumber)
	{
		MonoBehaviour.print(string.Concat(new object[]
		{
			"> [8] SyncJoinOrMissionScreen - Is Host: ",
			Connect.IsHost,
			" - sceneToLoad: ",
			sceneToLoad,
			" - CampaignToLoad: ",
			CampaignToLoad,
			" - currentlevelNumber: ",
			currentlevelNumber
		}));
		if (!Connect.IsHost)
		{
			MonoBehaviour.print("[8] CampaignToLoad " + CampaignToLoad + "  -  " + sceneToLoad);
			LevelSelectionController.campaignToLoad = CampaignToLoad;
			LevelSelectionController.CurrentLevelNum = currentlevelNumber;
			Networking.SetSeed(randomSeed);
			if (sceneToLoad == "Connection Test")
			{
				MonoBehaviour.print("Connection Test");
				Connect.AllDeterminsiticObjectsHaveBeenRegistered();
			}
			else if (!Connect.IsHost)
			{
				Networking.StreamIsPaused = true;
				Application.LoadLevel(sceneToLoad);
			}
		}
	}

	private void Update()
	{
		Connect.Layer.Update();
	}

	public void Test(PID sender)
	{
		this.messagesRecieved.Add("Message receieved from " + sender);
	}

	public static void SetSessionID(byte newSessionID)
	{
		Connect.sessionID.Set(newSessionID);
		Registry.RecacheDictionaries();
	}

	public static void SetRandomSessionID()
	{
		Connect.SetSessionID((byte)UnityEngine.Random.Range(1, 200));
	}

	public static void ClearDCPlayers()
	{
		List<PID> list = new List<PID>();
		foreach (PID pid in Connect.playerIDList)
		{
			if (Connect.Layer.IsDC(pid))
			{
				MonoBehaviour.print(pid + " is DC");
				list.Add(pid);
			}
		}
		foreach (PID pid2 in list)
		{
			SingletonMono<Connect>.Instance.DeregisterPlayer(pid2);
		}
	}

	public static void Disconnect()
	{
		try
		{
			RPCBatcher.FlushQueue();
			UnityEngine.Debug.Log("> [14] Disconnect");
			Connect.layer.LeaveMatch();
			BNetwork.Shutdown();
			Connect.RemoveAllOtherPlayers();
			Connect.playerIDList.Clear();
			Connect.playerNameList.Clear();
			Connect.layer = null;
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	public static void AllDeterminsiticObjectsHaveBeenRegistered()
	{
		Networking.StreamIsPaused = false;
		MonoBehaviour.print("> [9] AllDeterminsiticObjectsHaveBeenRegistered " + Connect.playerIDList.Count);
		if (Connect.playerIDList.Count > 1)
		{
			Networking.RPC<PID>(PID.TargetOthers, true, false, new RpcSignature<PID>(Registry.RequestNetworkObjectSync), PID.MyID);
		}
		else
		{
			Connect.NotifyThatSceneIsfullySynced();
		}
	}

	public static string GetPlayerName(PID pid)
	{
		if (Connect.playerNameList.ContainsKey(pid))
		{
			return Connect.playerNameList[pid];
		}
		return Connect.PlayerName;
	}

	public static void NotifyThatSceneIsfullySynced()
	{
		MonoBehaviour.print("> [11] Scene is fully synced ----");
		SingletonMono<Connect>.Instance.BroadcastMessageToAllMonoBehaviours("OnSceneSyncronised");
	}

	[RPC]
	public void DeregisterPlayer(PID pid)
	{
		MonoBehaviour.print("> DeregisterPlayer " + pid);
		int playerNum = HeroController.GetPlayerNum(pid);
		HeroController.Dropout(playerNum, false);
		InstantiationController.DeregisterPlayer(pid);
		Connect.Layer.RemovePlayer(pid);
	}

	[RPC]
	private void UpdateNextPlayerID(int AllocatedPlayerIDs)
	{
		PID.allocatedIDs = (byte)AllocatedPlayerIDs;
	}

	public static double Time
	{
		get
		{
			return NetworkTimeSync.SyncedTime;
		}
	}

	public static float Timef
	{
		get
		{
			return (float)Connect.Time;
		}
	}

	public static double GetInterpTime(PID playerID)
	{
		if (playerID == PID.TargetServer)
		{
			playerID = PID.ServerID;
		}
		double num = (double)PingController.GetInterpolationOffset(playerID);
		return Connect.Time - num;
	}

	private void OnApplicationQuit()
	{
		if (!Connect.IsOffline)
		{
			Connect.Disconnect();
		}
	}

	public static string Password = string.Empty;

	public static string GameName = string.Empty;

	private static SID sessionID = new SID(byte.MaxValue);

	private static float addedLatency = 0f;

	private static float simulatedVariance = 0f;

	private static float spikeProbablity = 0f;

	private static float simulatedLatency = 0f;

	private float SimulatedLatencyUpdateTimer;

	private bool bypassNetworkLayer;

	public static Dictionary<PID, string> playerNameList = new Dictionary<PID, string>();

	public static string Country = string.Empty;

	public static string City = string.Empty;

	public Connect.ConnectivityLayer connectivityLayer;

	private static ConnectionLayer layer = null;

	private static bool isRichardsPC = false;

	private List<string> messagesRecieved = new List<string>();

	public enum ConnectivityLayer
	{
		Badumna,
		Steam
	}
}
