// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class RPCBatcher : MonoBehaviour
{
	public static void Reset()
	{
		RPCBatcher.batches = new Dictionary<PID, List<RPCObject>>();
	}

	public static void Send(RPCObject rpc)
	{
		if (Connect.IsOffline)
		{
			return;
		}
		PID destination = rpc.messageInfo.Destination;
		if (destination == PID.TargetAll)
		{
			foreach (PID pid in Connect.playerIDList)
			{
				if (pid != PID.MyID)
				{
					RPCBatcher.QueueSend(pid, rpc);
				}
			}
		}
		else if (destination == PID.TargetOthers)
		{
			foreach (PID pid2 in Connect.playerIDList)
			{
				if (pid2 != PID.MyID)
				{
					RPCBatcher.QueueSend(pid2, rpc);
				}
			}
		}
		else if (destination == PID.TargetServer)
		{
			if (!PID.IamServer())
			{
				RPCBatcher.QueueSend(PID.ServerID, rpc);
			}
		}
		else if (destination != PID.MyID)
		{
			RPCBatcher.QueueSend(destination, rpc);
		}
	}

	private static void QueueSend(PID pid, RPCObject rpc)
	{
		if (!RPCBatcher.batches.ContainsKey(pid))
		{
			RPCBatcher.batches[pid] = new List<RPCObject>();
		}
		RPCBatcher.batches[pid].Add(rpc);
	}

	private void OnlevelWasLoaded()
	{
		RPCBatcher.maxBatched = 0;
	}

	public static void FlushQueue()
	{
		foreach (KeyValuePair<PID, List<RPCObject>> keyValuePair in RPCBatcher.batches)
		{
			RPCBatcher.maxBatched = Mathf.Max(RPCBatcher.maxBatched, keyValuePair.Value.Count);
			if (keyValuePair.Value.Count > 0)
			{
				if (RPCController.DebugMode)
				{
					MonoBehaviour.print("Flushing pair.Value" + keyValuePair.Value);
				}
				UnityStream unityStream = new UnityStream();
				foreach (RPCObject rpcobject in keyValuePair.Value)
				{
					if (RPCController.DebugMode)
					{
						MonoBehaviour.print("Flushing " + rpcobject.methodInfo);
					}
					if (rpcobject is StaticRPCObject)
					{
						unityStream.Serialize<StaticRPCObject>((StaticRPCObject)rpcobject);
					}
					else
					{
						unityStream.Serialize<NonStaticRPCObject>((NonStaticRPCObject)rpcobject);
					}
					Analytics.AddRpcOut(rpcobject);
				}
				byte[] byteArray = unityStream.ByteArray;
				Connect.Layer.SendData(keyValuePair.Key, byteArray);
			}
			keyValuePair.Value.Clear();
		}
		Connect.Layer.ProcessNetworkState();
	}

	private void Update()
	{
		float num = 0.05f;
		this.timer -= NetworkTimeSync.RealDeltaTime;
		this.timer = Mathf.Max(0f, this.timer);
		if (this.timer <= 0f)
		{
			this.timer += num;
			RPCBatcher.FlushQueue();
		}
	}

	private static Dictionary<PID, List<RPCObject>> batches = new Dictionary<PID, List<RPCObject>>();

	public static int maxBatched = 0;

	private float timer;
}
