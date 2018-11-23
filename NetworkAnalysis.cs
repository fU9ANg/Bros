// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NetworkAnalysis : MonoBehaviour
{
	private void Start()
	{
		NetworkAnalysis.instance = this;
	}

	private void Update()
	{
		this.PerSecTimer += Time.deltaTime;
		if (this.PerSecTimer > 1f)
		{
			this.PerSecTimer -= 1f;
			this.incomingBandWidthPerSec.Add(NetworkAnalysis.BitsPerSecRecieved);
			this.outgoingBandWidthPerSec.Add(NetworkAnalysis.BitsPerSecSent);
			this.rpcCallsPerSecThisMap.Add(this.rpcCallsThisSec);
			this.rpcCallsInTheLastSec = this.rpcCallsThisSec;
			this.rpcCallsThisSec = new Dictionary<string, RPCdata>();
			foreach (KeyValuePair<string, RPCdata> keyValuePair in this.rpcCallsInTheLastSec)
			{
				if (!this.worstCasePerRPC.ContainsKey(keyValuePair.Key))
				{
					this.worstCasePerRPC.Add(keyValuePair.Key, new RPCdata(keyValuePair.Key));
				}
				if (this.worstCasePerRPC[keyValuePair.Key].kilobits < keyValuePair.Value.kilobits)
				{
					this.worstCasePerRPC[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			if (NetworkAnalysis.GetTotalRpcKilobits(this.rpcCallsInTheLastSec) > NetworkAnalysis.GetTotalRpcKilobits(this.worstSecondOfRPCs))
			{
				this.worstSecondOfRPCs = this.rpcCallsInTheLastSec;
			}
		}
	}

	public static float BitsPerSecSent
	{
		get
		{
			return (float)NetworkAnalysis.instance.bytesSentInTheLastSecond * 0.0078125f;
		}
	}

	private static float BytesToBits(float bytes)
	{
		return bytes * 0.0078125f;
	}

	public static float BitsPerSecRecieved
	{
		get
		{
			return (float)NetworkAnalysis.instance.bytesRecievedInTheLastSecond * 0.0078125f;
		}
	}

	public static int GetTotalRpcCount(Dictionary<string, RPCdata> rpcCounts)
	{
		int num = 0;
		foreach (RPCdata rpcdata in rpcCounts.Values)
		{
			num += rpcdata.calls;
		}
		return num;
	}

	public static float GetTotalRpcKilobits(Dictionary<string, RPCdata> rpcCounts)
	{
		float num = 0f;
		foreach (RPCdata rpcdata in rpcCounts.Values)
		{
			num += rpcdata.kilobits;
		}
		return num;
	}

	private static bool AnalysisDisabled
	{
		get
		{
			return !Info.IsDevBuild || !Info.IsRichardsPC || Application.isWebPlayer;
		}
	}

	public static void AddRecievedRPC(string methodName, int sizeInBytes)
	{
		if (NetworkAnalysis.instance != null)
		{
			if (!NetworkAnalysis.instance.rpcCallsThisSec.ContainsKey(methodName))
			{
				NetworkAnalysis.instance.rpcCallsThisSec.Add(methodName, new RPCdata(methodName));
			}
			NetworkAnalysis.instance.rpcCallsThisSec[methodName].kilobits += NetworkAnalysis.BytesToBits((float)sizeInBytes);
			NetworkAnalysis.instance.rpcCallsThisSec[methodName].calls++;
			if (!NetworkAnalysis.instance.rpcCallsThisMap.ContainsKey(methodName))
			{
				NetworkAnalysis.instance.rpcCallsThisMap.Add(methodName, new RPCdata(methodName));
			}
			NetworkAnalysis.instance.rpcCallsThisMap[methodName].kilobits += NetworkAnalysis.BytesToBits((float)sizeInBytes);
			NetworkAnalysis.instance.rpcCallsThisMap[methodName].calls++;
		}
	}

	private void OnDestroy()
	{
	}

	private void WriteToFile()
	{
		if (NetworkAnalysis.AnalysisDisabled)
		{
			return;
		}
		UnityEngine.Debug.Log("Attempt Write");
		StreamWriter streamWriter = null;
		if (!this.writtenMapName)
		{
			streamWriter.WriteLine("Map: " + Map.LevelFileName);
			this.writtenMapName = true;
		}
		if (this.incomingBandWidthPerSec.Count == 0)
		{
			streamWriter.WriteLine("No Samples");
		}
		else
		{
			List<string> rpcheadings = this.GetRPCHeadings();
			string text = " ,Outgoing kb/s,Incoming kb/s, TotalRPC";
			foreach (string str in rpcheadings)
			{
				text = text + "," + str;
			}
			text += ",------ ,Totals kilobits";
			foreach (string str2 in rpcheadings)
			{
				text = text + "," + str2;
			}
			streamWriter.WriteLine(text);
			for (int i = 0; i < this.incomingBandWidthPerSec.Count; i++)
			{
				string text2 = ",";
				string text3 = text2;
				text2 = string.Concat(new object[]
				{
					text3,
					this.outgoingBandWidthPerSec[i],
					",",
					this.incomingBandWidthPerSec[i]
				});
				Dictionary<string, RPCdata> dictionary = this.rpcCallsPerSecThisMap[i];
				text2 = text2 + "," + NetworkAnalysis.GetTotalRpcCount(dictionary);
				foreach (string key in rpcheadings)
				{
					text2 += ",";
					if (dictionary.ContainsKey(key))
					{
						text2 += dictionary[key].calls;
					}
					else
					{
						text2 += 0;
					}
				}
				text2 += ",";
				text2 = text2 + "," + System.Math.Round((double)NetworkAnalysis.GetTotalRpcKilobits(dictionary), 1);
				foreach (string key2 in rpcheadings)
				{
					text2 += ",";
					if (dictionary.ContainsKey(key2))
					{
						text2 += System.Math.Round((double)dictionary[key2].kilobits, 1);
					}
					else
					{
						text2 += 0;
					}
				}
				streamWriter.WriteLine(text2);
			}
		}
		streamWriter.WriteLine(string.Empty);
		streamWriter.Flush();
		streamWriter.Close();
		this.incomingBandWidthPerSec.Clear();
		this.outgoingBandWidthPerSec.Clear();
		this.rpcCallsPerSecThisMap.Clear();
		UnityEngine.Debug.Log("Write successful");
	}

	private void OnApplicationQuit()
	{
	}

	private List<string> GetRPCHeadings()
	{
		List<string> list = new List<string>();
		foreach (Dictionary<string, RPCdata> dictionary in this.rpcCallsPerSecThisMap)
		{
			foreach (string item in dictionary.Keys)
			{
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		return list;
	}

	private bool writtenMapName;

	public static NetworkAnalysis instance;

	public float PerSecTimer;

	public long bytesRecievedPrev;

	public long bytesSentPrev;

	public int bytesRecievedInTheLastSecond;

	public int bytesSentInTheLastSecond;

	public Dictionary<string, RPCdata> rpcCallsThisSec = new Dictionary<string, RPCdata>();

	public Dictionary<string, RPCdata> rpcCallsInTheLastSec = new Dictionary<string, RPCdata>();

	public Dictionary<string, RPCdata> rpcCallsThisMap = new Dictionary<string, RPCdata>();

	public Dictionary<string, RPCdata> worstCasePerRPC = new Dictionary<string, RPCdata>();

	public Dictionary<string, RPCdata> worstSecondOfRPCs = new Dictionary<string, RPCdata>();

	public List<Dictionary<string, RPCdata>> rpcCallsPerSecThisMap = new List<Dictionary<string, RPCdata>>();

	public List<Dictionary<string, long>> rpcBytesPerSec = new List<Dictionary<string, long>>();

	public List<float> incomingBandWidthPerSec = new List<float>();

	public List<float> outgoingBandWidthPerSec = new List<float>();
}
