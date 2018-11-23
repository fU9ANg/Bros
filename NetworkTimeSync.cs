// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTimeSync : SingletonMono<NetworkTimeSync>
{
	public static double SyncedTime
	{
		get
		{
			return NetworkTimeSync.RealTime + NetworkTimeSync.deltaTime;
		}
	}

	private static double RealTime
	{
		get
		{
			return (double)Time.realtimeSinceStartup;
		}
	}

	public static float RealDeltaTime
	{
		get
		{
			return NetworkTimeSync.realDeltaTime;
		}
	}

	public bool TimeHasBeenSet
	{
		get
		{
			return this.timeHasBeenSet;
		}
	}

	public void Reset()
	{
		NetworkTimeSync.deltaTime = 0.0;
		this.updateTimer = 0f;
		this.ignnoreSessionCounter = 0;
		this.timeHasBeenSet = false;
		this.deltaTimeSamples.Clear();
	}

	public static void InitializeDeltaTime(double serverTime)
	{
		NetworkTimeSync.deltaTime = serverTime - (double)Time.realtimeSinceStartup;
		MonoBehaviour.print("SyncedTime: " + NetworkTimeSync.SyncedTime);
	}

	private void Awake()
	{
		this.PrevUpdateTime = Connect.Timef;
		NetworkTimeSync.realDeltaTime = 0f;
	}

	private void Update()
	{
		NetworkTimeSync.realDeltaTime = Mathf.Max(0f, Connect.Timef - this.PrevUpdateTime);
		this.PrevUpdateTime = Connect.Timef;
		if (!Connect.IsHost && !PID.IamServer())
		{
			this.updateTimer -= NetworkTimeSync.RealDeltaTime;
			this.updateTimer = Mathf.Max(this.updateTimer, 0f);
			if (this.updateTimer <= 0f)
			{
				bool flag = false;
				if (this.ignnoreSessionCounter < 0)
				{
					flag = true;
					this.ignnoreSessionCounter = 10;
				}
				Networking.RPC<PID, double, bool>(PID.TargetServer, true, flag, false, new RpcSignature<PID, double, bool>(this.Request), PID.MyID, NetworkTimeSync.RealTime, flag);
				this.updateTimer += this.updateInterval;
				this.updateInterval += 0.2f;
				this.updateInterval = Mathf.Min(this.updateInterval, this.updateIntervalMax);
			}
		}
	}

	private void Request(PID requestee, double timeStamp, bool ignoreSessionID)
	{
		Networking.RPC<double, double>(requestee, true, ignoreSessionID, new RpcSignature<double, double>(this.Response), NetworkTimeSync.SyncedTime, timeStamp);
	}

	private void Response(double serverTime, double requestTimeStamp)
	{
		double num = (NetworkTimeSync.RealTime - requestTimeStamp) / 2.0;
		double num2 = serverTime + num;
		double item = num2 - NetworkTimeSync.RealTime;
		if (this.deltaTimeSamples.Count > 500)
		{
			this.deltaTimeSamples.RemoveAt(0);
		}
		this.deltaTimeSamples.Add(item);
		string text = string.Empty;
		double num3 = 0.0;
		for (int i = 0; i < this.deltaTimeSamples.Count; i++)
		{
			num3 += this.deltaTimeSamples[i];
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[",
				System.Math.Round(this.deltaTimeSamples[i], 2),
				"]   "
			});
		}
		num3 /= (double)this.deltaTimeSamples.Count;
		float num4 = Mathf.Abs((float)(NetworkTimeSync.deltaTime - num3));
		if (num4 > 0.01f)
		{
			NetworkTimeSync.deltaTime = num3;
		}
		if (!SingletonMono<NetworkTimeSync>.Instance.timeHasBeenSet)
		{
			MonoBehaviour.print("--- Time Is Set ---");
			SingletonMono<NetworkTimeSync>.Instance.timeHasBeenSet = true;
		}
	}

	private static double deltaTime;

	public List<double> deltaTimeSamples = new List<double>();

	private float updateTimer;

	private float updateInterval = 0.2f;

	private float updateIntervalMax = 2f;

	private float PrevUpdateTime;

	private static float realDeltaTime;

	private int ignnoreSessionCounter;

	private bool timeHasBeenSet;
}
