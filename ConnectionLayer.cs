// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConnectionLayer
{
	public List<PID> playerIDList
	{
		get
		{
			return new List<PID>(this.playerIdPairs.Keys);
		}
	}

	protected bool ContainsID(IDWrapper id)
	{
		UnityEngine.Debug.Log("------ contains " + id);
		foreach (IDWrapper idwrapper in this.playerIdPairs.Values)
		{
			UnityEngine.Debug.Log("item " + idwrapper);
			if (idwrapper.Equals(id))
			{
				return true;
			}
		}
		UnityEngine.Debug.Log("false ");
		return false;
	}

	public virtual bool IsOffline
	{
		get
		{
			return true;
		}
	}

	public virtual bool IsHost
	{
		get
		{
			return true;
		}
	}

	public virtual bool IsLoggedIn
	{
		get
		{
			return true;
		}
	}

	public void RemovePlayer(PID pid)
	{
		if (this.playerIdPairs.ContainsKey(pid))
		{
			this.playerIdPairs.Remove(pid);
		}
	}

	public abstract bool IsDC(PID pid);

	public abstract void CreateMatch();

	public abstract void LeaveMatch();

	public abstract void JoinMatch(GameInfo gameRoom);

	public abstract void FindMatch();

	public abstract void SendData(PID pid, byte[] bytes);

	public virtual void Update()
	{
	}

	public abstract void ProcessNetworkState();

	public virtual IDWrapper MyNetworkLayerID
	{
		get
		{
			return null;
		}
	}

	protected void GeneratePlayerID(IDWrapper requesteeID)
	{
		UnityEngine.Debug.Log(Connect.IsHost);
		if (!Connect.IsHost)
		{
			UnityEngine.Debug.Log("> Server should not recieve GeneratePlayerID");
			return;
		}
		PID pid = PID.NoID;
		foreach (KeyValuePair<PID, IDWrapper> keyValuePair in Connect.Layer.playerIdPairs)
		{
			if (keyValuePair.Value == requesteeID)
			{
				UnityEngine.Debug.Log("> [4] Member already contained " + requesteeID);
				pid = keyValuePair.Key;
			}
		}
		if (pid.AsByte == PID.NoID.AsByte)
		{
			pid = PID.Allocate();
		}
		UnityEngine.Debug.Log("> [4] ReQuest Player ID " + pid);
		this.BroadcastPlayerID(pid, requesteeID);
		Networking.RPC<PID>(pid, true, true, new RpcSignature<PID>(PID.SetServerID), PID.ServerID);
	}

	public static void PlayerHasJoinedMatch(IDWrapper playerID)
	{
		UnityEngine.Debug.Log("> [2] AddMember  matchMaking " + playerID);
		if (!Connect.Layer.ContainsID(playerID))
		{
			if (Connect.IsHost)
			{
				Connect.Layer.GeneratePlayerID(playerID);
			}
		}
		else
		{
			UnityEngine.Debug.Log("> " + playerID + " already added!!!!!!");
		}
		if (LevelSelectionController.IsCampaignScene)
		{
			UnityEngine.Debug.Log("> " + playerID.ToString() + " has Joined");
			ChatSystem.AddMessage(playerID.ToString() + " has Joined", PID.NoID, Connect.Timef);
		}
	}

	public static void RegisterPlayerID(byte allocatedIDByte, byte AllocatedPlayerIDs, string MemIDasString, string name)
	{
		IDWrapper idwrapper = new IDWrapper(MemIDasString, name);
		UnityEngine.Debug.Log("> [4] Register Player ID " + allocatedIDByte + "---------------");
		PID pid = (PID)allocatedIDByte;
		if (PID.allocatedIDs < AllocatedPlayerIDs)
		{
			PID.allocatedIDs = AllocatedPlayerIDs;
		}
		UnityEngine.Debug.Log("allocatedIDByte  " + allocatedIDByte);
		Connect.Layer.playerIdPairs[pid] = idwrapper;
		Connect.playerNameList[pid] = idwrapper.Name;
		UnityEngine.Debug.Log("memID " + idwrapper.ToString());
		UnityEngine.Debug.Log("Connect.Layer.MyNetworkLayerID.Equals(memID) " + Connect.Layer.MyNetworkLayerID);
		UnityEngine.Debug.Log("Connect.Layer.MyNetworkLayerID.Equals(memID) " + Connect.Layer.MyNetworkLayerID.Equals(idwrapper));
		if (Connect.Layer.MyNetworkLayerID.Equals(idwrapper) && !Connect.IsOffline)
		{
			PID.SetMyID(pid.AsByte);
		}
	}

	protected void BroadcastPlayerID(PID newID, IDWrapper requesteeID)
	{
		UnityEngine.Debug.Log("BroadcastPlayerID " + newID);
		ConnectionLayer.RegisterPlayerID(newID.AsByte, PID.allocatedIDs, requesteeID.UnderlyingID, requesteeID.Name);
		Networking.RPC<byte, byte, string, string>(PID.TargetOthers, true, true, new RpcSignature<byte, byte, string, string>(ConnectionLayer.RegisterPlayerID), newID.AsByte, PID.allocatedIDs, requesteeID.UnderlyingID, requesteeID.Name);
		foreach (KeyValuePair<PID, IDWrapper> keyValuePair in Connect.Layer.playerIdPairs)
		{
			if (!requesteeID.Equals(keyValuePair.Value))
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"send to others ",
					requesteeID,
					" ",
					keyValuePair.Value
				}));
				Networking.RPC<byte, byte, string, string>(newID, true, true, new RpcSignature<byte, byte, string, string>(ConnectionLayer.RegisterPlayerID), keyValuePair.Key.AsByte, PID.allocatedIDs, keyValuePair.Value.UnderlyingID, keyValuePair.Value.Name);
			}
		}
	}

	public void Reset()
	{
		ConnectionLayer.rpcsRecieved.Clear();
	}

	public static void RecieveBytes(byte[] bytes)
	{
		UnityStream unityStream = new UnityStream(bytes);
		for (int i = 0; i < bytes.Length; i++)
		{
			if (unityStream.Finished)
			{
				break;
			}
			RPCObject item = (RPCObject)unityStream.DeserializeNext();
			ConnectionLayer.rpcsRecieved.Enqueue(item);
		}
	}

	public static RPCObject RecieveNext()
	{
		if (ConnectionLayer.rpcsRecieved.Count > 0)
		{
			return ConnectionLayer.rpcsRecieved.Dequeue();
		}
		return null;
	}

	public static bool matchQueryHandled = false;

	public static ConnectionState connectionState = ConnectionState.Disconnected;

	public Dictionary<PID, IDWrapper> playerIdPairs = new Dictionary<PID, IDWrapper>();

	public static List<GameInfo> matchList = new List<GameInfo>();

	public static Queue<RPCObject> rpcsRecieved = new Queue<RPCObject>();
}
