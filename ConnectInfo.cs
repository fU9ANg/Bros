// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class ConnectInfo : SingletonMono<ConnectInfo>
{
	public void DrawConnectInfo()
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("----------------------------------- Time ------------------------------------------", new GUILayoutOption[0]);
		GUILayout.Label("SyncedTime: " + NetworkTimeSync.SyncedTime, new GUILayoutOption[0]);
		GUILayout.Label("Sync Samples: " + SingletonMono<NetworkTimeSync>.Instance.deltaTimeSamples.Count, new GUILayoutOption[0]);
		foreach (KeyValuePair<PID, IDWrapper> keyValuePair in Connect.Layer.playerIdPairs)
		{
			string text = "false";
			if (SingletonMono<Registry>.Instance != null && SingletonMono<Registry>.Instance.playersThatIHaveSyncedWith.Contains(keyValuePair.Key))
			{
				text = "true";
			}
			GUILayout.Label(string.Concat(new object[]
			{
				keyValuePair.Key,
				"  ",
				keyValuePair.Value,
				"  [raw ping ",
				System.Math.Round((double)PingController.GetRawPing(keyValuePair.Key), 2),
				"]    [ping ",
				System.Math.Round((double)PingController.GetPing(keyValuePair.Key), 2),
				"]   [interp: ",
				PingController.GetInterpolationOffset(keyValuePair.Key),
				"]  Synced:",
				text
			}), new GUILayoutOption[0]);
		}
		GUILayout.Label(string.Empty, new GUILayoutOption[0]);
		GUILayout.Label("---------------------------------- Connection State -------------------------------", new GUILayoutOption[0]);
		GUILayout.Label("Offline: " + Connect.IsOffline, new GUILayoutOption[0]);
		GUILayout.Label("Verson ID: " + VersionNumber.version, new GUILayoutOption[0]);
		GUILayout.Label("Game Name: " + Connect.GameName, new GUILayoutOption[0]);
		GUILayout.Label("Player Name: " + Connect.PlayerName, new GUILayoutOption[0]);
		GUILayout.Label("Is Server: " + Connect.IsHost, new GUILayoutOption[0]);
		GUILayout.Label("Session ID: " + Connect.SessionID, new GUILayoutOption[0]);
		GUILayout.Label("Seed: " + Networking.RandomSeed, new GUILayoutOption[0]);
		GUILayout.Label(string.Concat(new object[]
		{
			"My ID: ",
			PID.MyID,
			" | Server ID: ",
			PID.ServerID
		}), new GUILayoutOption[0]);
		GUILayout.Label("Stream Paused: " + Networking.StreamIsPaused, new GUILayoutOption[0]);
		if (BNetwork.iNeteworkFacade != null)
		{
			GUILayout.Label("Match : " + BNetwork.iNeteworkFacade.Match, new GUILayoutOption[0]);
			GUILayout.Label("IsTunnelled : " + BNetwork.iNeteworkFacade.IsTunnelled, new GUILayoutOption[0]);
			GUILayout.Label("InitializationProgress : " + BNetwork.iNeteworkFacade.InitializationProgress, new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("---------------------------------- Level Info -------------------------------------", new GUILayoutOption[0]);
		GUILayout.Label("DefaultCampaign: " + LevelSelectionController.DefaultCampaign, new GUILayoutOption[0]);
		GUILayout.Label("campaignToLoad: " + LevelSelectionController.campaignToLoad, new GUILayoutOption[0]);
		if (LevelSelectionController.currentCampaign == null)
		{
			GUILayout.Label("currentCampaign: None", new GUILayoutOption[0]);
		}
		else
		{
			GUILayout.Label("currentCampaign: " + LevelSelectionController.currentCampaign.name, new GUILayoutOption[0]);
		}
		GUILayout.Label("CurrentLevelNum: " + LevelSelectionController.CurrentLevelNum, new GUILayoutOption[0]);
		GUILayout.Label(string.Concat(new object[]
		{
			"nextLoadOffset: ",
			Map.nextXLoadOffset,
			"  ",
			Map.nextYLoadOffset
		}), new GUILayoutOption[0]);
		GUILayout.Label(string.Concat(new object[]
		{
			"lastLoadOffset: ",
			Map.lastXLoadOffset,
			"  ",
			Map.lastYLoadOffset
		}), new GUILayoutOption[0]);
		GUILayout.Label("lastFinishedLevelOffline: " + PlayerProgress.Instance.lastFinishedLevelOffline, new GUILayoutOption[0]);
		GUILayout.Label("lastFinishedLevelOnline: " + PlayerProgress.Instance.lastFinishedLevelOnline, new GUILayoutOption[0]);
		GUILayout.Label(string.Empty, new GUILayoutOption[0]);
		GUILayout.Label("-----------------------------------Latency ----------------------------------------", new GUILayoutOption[0]);
		GUILayout.Label("Max batched:     " + RPCBatcher.maxBatched, new GUILayoutOption[0]);
		GUILayout.Label("Kb/s Sent:     " + Analytics.KilabytesSentInTheLastSecond, new GUILayoutOption[0]);
		GUILayout.Label("Kb/s Recieved: " + Analytics.KilabytesRecievedInTheLastSecond, new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (GUILayout.Button("-50 ms", new GUILayoutOption[0]))
		{
			Networking.RPC<float>(PID.TargetAll, new RpcSignature<float>(Connect.SetAddedLatency), Connect.AddedLatency - 0.05f, false);
		}
		GUILayout.Label("Simulated Latency: " + Connect.AddedLatency, new GUILayoutOption[0]);
		if (GUILayout.Button("+50 ms", new GUILayoutOption[0]))
		{
			Networking.RPC<float>(PID.TargetAll, new RpcSignature<float>(Connect.SetAddedLatency), Connect.AddedLatency + 0.05f, false);
		}
		GUILayout.EndHorizontal();
		if (BNetwork.iNeteworkFacade != null)
		{
			GUILayout.Label("Kb/s in: " + BNetwork.iNeteworkFacade.InboundBytesPerSecond * 0.00097656197613105178, new GUILayoutOption[0]);
			GUILayout.Label("Kb/s out: " + BNetwork.iNeteworkFacade.OutboundBytesPerSecond * 0.00097656197613105178, new GUILayoutOption[0]);
			GUILayout.Label("SendLimitKilabytesPerSecond: " + Useful.Round((float)BNetwork.iNeteworkFacade.TotalSendLimitBytesPerSecond * 0.000976562f), new GUILayoutOption[0]);
			GUILayout.Label("MaximumSendLimitKBPerSecond: " + Useful.Round(BNetwork.iNeteworkFacade.MaximumSendLimitBytesPerSecond * 0.00097656197613105178), new GUILayoutOption[0]);
			GUILayout.Label("AveragePacketLossRate: " + Useful.Round(BNetwork.iNeteworkFacade.AveragePacketLossRate), new GUILayoutOption[0]);
			GUILayout.Label("MaximumPacketLossRate: " + Useful.Round(BNetwork.iNeteworkFacade.MaximumPacketLossRate), new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("---------------------------------- Follow Cam info -------------------------------------", new GUILayoutOption[0]);
		string str = "None";
		if (Map.newestHelicopter != null)
		{
			str = string.Empty + Map.newestHelicopter.Nid;
		}
		GUILayout.Label("Heli: " + str, new GUILayoutOption[0]);
		if (GameModeController.Instance != null)
		{
			GUILayout.Label("LevelFinished: " + GameModeController.LevelFinished, new GUILayoutOption[0]);
		}
		GUILayout.Label("GetPlayersOnHelicopterAmount: " + HeroController.GetPlayersOnHelicopterAmount(), new GUILayoutOption[0]);
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}
}
