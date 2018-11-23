// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class Ack
{
	public Ack()
	{
		this.AckID = Ack.AckCounter++;
	}

	public Ack(int id, bool isResponse)
	{
		this.AckID = id;
		this.IsResponse = isResponse;
	}

	public static Ack Allocate()
	{
		return new Ack();
	}

	public static void Clear()
	{
		Ack.RPCsAwaitingReplies = new Dictionary<int, RPCObject>();
	}

	public static void ResendServerRequests()
	{
		List<RPCObject> list = new List<RPCObject>(Ack.RPCsAwaitingReplies.Values);
		UnityEngine.Debug.Log("> ResendServerRequests  " + list.Count);
		foreach (RPCObject rpcobject in list)
		{
			UnityEngine.Debug.Log("--" + rpcobject.methodInfo);
			RPCController.Instance.RouteRPCObject(rpcobject, true);
		}
	}

	public static Dictionary<int, RPCObject> RPCsAwaitingReplies = new Dictionary<int, RPCObject>();

	private static int AckCounter = 0;

	public int AckID;

	public bool IsResponse;
}
