// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class Analytics : SingletonMono<Analytics>
{
	public static float KilabytesSentInTheLastSecond
	{
		get
		{
			return Analytics.rpcsOut.KilabytesInTheLastSecond;
		}
	}

	public static float KilabytesRecievedInTheLastSecond
	{
		get
		{
			return Analytics.rpcsIn.KilabytesInTheLastSecond;
		}
	}

	public static void Clear()
	{
		Analytics.rpcsOut = new BandwidthMonitor();
		Analytics.rpcsIn = new BandwidthMonitor();
	}

	public static void AddRpcOut(RPCObject rpc)
	{
		int byteCount;
		if (rpc is StaticRPCObject)
		{
			byteCount = TypeSerializer.ObjectToByteArray<StaticRPCObject>((StaticRPCObject)rpc).Length;
		}
		else
		{
			byteCount = TypeSerializer.ObjectToByteArray<NonStaticRPCObject>((NonStaticRPCObject)rpc).Length;
		}
		Analytics.rpcsOut.AddRPC(rpc, byteCount);
	}

	public static void AddRpcIn(RPCObject rpc)
	{
		int byteCount;
		if (rpc is StaticRPCObject)
		{
			byteCount = TypeSerializer.ObjectToByteArray<StaticRPCObject>((StaticRPCObject)rpc).Length;
		}
		else
		{
			byteCount = TypeSerializer.ObjectToByteArray<NonStaticRPCObject>((NonStaticRPCObject)rpc).Length;
		}
		Analytics.rpcsIn.AddRPC(rpc, byteCount);
	}

	private void Update()
	{
		Analytics.rpcsIn.Update();
		Analytics.rpcsOut.Update();
		this.timer -= NetworkTimeSync.RealDeltaTime;
		this.printTimer -= NetworkTimeSync.RealDeltaTime;
		this.timer = Mathf.Max(0f, this.timer);
		this.printTimer = Mathf.Max(0f, this.printTimer);
		if (this.timer <= 0f)
		{
			this.timer += 1f;
			Analytics.UpdateKbpsLog();
		}
		if (this.printTimer <= 0f)
		{
			this.printTimer += 60f;
			Analytics.PrintKbpsLog();
		}
	}

	public static void UpdateKbpsLog()
	{
		if (BNetwork.iNeteworkFacade != null)
		{
			Analytics.kbpsInForTheLastTenSecs.Add((float)BNetwork.iNeteworkFacade.InboundBytesPerSecond * 0.000976562f);
			Analytics.kbpsOutForTheLastTenSecs.Add((float)BNetwork.iNeteworkFacade.OutboundBytesPerSecond * 0.000976562f);
			if (Analytics.kbpsInForTheLastTenSecs.Count > 60)
			{
				Analytics.kbpsInForTheLastTenSecs.RemoveAt(0);
			}
			if (Analytics.kbpsOutForTheLastTenSecs.Count > 60)
			{
				Analytics.kbpsOutForTheLastTenSecs.RemoveAt(0);
			}
		}
	}

	public static void PrintKbpsLog()
	{
	}

	public void Draw()
	{
		if (GUILayout.Button("Clear", new GUILayoutOption[0]))
		{
			Analytics.rpcsIn = new BandwidthMonitor();
			Analytics.rpcsOut = new BandwidthMonitor();
		}
		GUIContent[] content = new GUIContent[]
		{
			new GUIContent(string.Concat(new object[]
			{
				"Inbound ",
				Useful.Round(Analytics.rpcsIn.KilabytesInTheLastSecond),
				"   ",
				Useful.Round(Analytics.rpcsIn.KilabytesInTotal)
			})),
			new GUIContent(string.Concat(new object[]
			{
				"OutBound ",
				Useful.Round(Analytics.rpcsOut.KilabytesInTheLastSecond),
				"   ",
				Useful.Round(Analytics.rpcsOut.KilabytesInTotal)
			}))
		};
		this.InOrOut = GUILayout.SelectionGrid(this.InOrOut, content, 2, new GUILayoutOption[0]);
		if (this.InOrOut == 0)
		{
			Analytics.rpcsIn.Draw();
		}
		else
		{
			Analytics.rpcsOut.Draw();
		}
	}

	public const float byteToKB = 0.000976562f;

	private static BandwidthMonitor rpcsOut = new BandwidthMonitor();

	private static BandwidthMonitor rpcsIn = new BandwidthMonitor();

	private static List<float> kbpsInForTheLastTenSecs = new List<float>();

	private static List<float> kbpsOutForTheLastTenSecs = new List<float>();

	private float timer;

	private float printTimer;

	private int InOrOut;
}
