// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class BandwidthMonitor
{
	public void AddRPC(RPCObject rpc, int byteCount)
	{
		string text = rpc.methodInfo.DeclaringType + "::" + rpc.methodInfo.Name;
		if (!this.rpcCounters.ContainsKey(text))
		{
			this.rpcCounters.Add(text, new BandwidthMonitor.RPCCounter(text));
		}
		this.rpcCounters[text].RegisterRPC(byteCount);
		this.totalRPCBytes += byteCount;
		this.rpcBytesThisSecond += byteCount;
	}

	public void Update()
	{
		this.timer += NetworkTimeSync.RealDeltaTime;
		if (this.timer >= 1f)
		{
			this.timer = 0f;
			foreach (BandwidthMonitor.RPCCounter rpccounter in this.rpcCounters.Values)
			{
				rpccounter.RefreshLastSecond();
			}
			this.rpcBytesInTheLastSecond = this.rpcBytesThisSecond;
			this.rpcBytesThisSecond = 0;
			float num = 0f;
			foreach (BandwidthMonitor.RPCCounter rpccounter2 in this.rpcCounters.Values)
			{
				num += rpccounter2.kilabytesLastSecondSecond;
			}
			if (num > this.WorstSecond)
			{
				this.WorstSecond = num;
				foreach (BandwidthMonitor.RPCCounter rpccounter3 in this.rpcCounters.Values)
				{
					rpccounter3.UpdateOverallWorst();
				}
			}
		}
	}

	public float KilabytesInTotal
	{
		get
		{
			return (float)this.totalRPCBytes * 0.000976562f;
		}
	}

	public float KilabytesInTheLastSecond
	{
		get
		{
			return (float)this.rpcBytesInTheLastSecond * 0.000976562f;
		}
	}

	public void Draw()
	{
		int num = 120;
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label(string.Empty, new GUILayoutOption[0]);
		GUIContent[] array = new GUIContent[]
		{
			new GUIContent("average Per Call"),
			new GUIContent("KB/S"),
			new GUIContent("Total KB"),
			new GUIContent("Call/sec"),
			new GUIContent("Total Calls"),
			new GUIContent("Worst Per RPC"),
			new GUIContent("Worst Second " + System.Math.Round((double)this.WorstSecond, 1))
		};
		this.sortBy = GUILayout.SelectionGrid(this.sortBy, array, array.Length, new GUILayoutOption[]
		{
			GUILayout.Width((float)((num + 5) * array.Length))
		});
		GUILayout.EndHorizontal();
		this.scroll = GUILayout.BeginScrollView(this.scroll, new GUILayoutOption[0]);
		List<BandwidthMonitor.RPCCounter> list = new List<BandwidthMonitor.RPCCounter>(this.rpcCounters.Values);
		if (this.sortBy == 0)
		{
			list.Sort(new Comparison<BandwidthMonitor.RPCCounter>(this.SortByAveragePerCall));
		}
		if (this.sortBy == 1)
		{
			list.Sort(new Comparison<BandwidthMonitor.RPCCounter>(this.SortByKBlastSecond));
		}
		if (this.sortBy == 2)
		{
			list.Sort(new Comparison<BandwidthMonitor.RPCCounter>(this.SortByTotalKB));
		}
		if (this.sortBy == 3)
		{
			list.Sort(new Comparison<BandwidthMonitor.RPCCounter>(this.SortByCallsLastSecond));
		}
		if (this.sortBy == 4)
		{
			list.Sort(new Comparison<BandwidthMonitor.RPCCounter>(this.SortByTotal));
		}
		if (this.sortBy == 5)
		{
			list.Sort(new Comparison<BandwidthMonitor.RPCCounter>(this.SortByPersonalWorst));
		}
		if (this.sortBy == 6)
		{
			list.Sort(new Comparison<BandwidthMonitor.RPCCounter>(this.SortByOverallWorst));
		}
		foreach (BandwidthMonitor.RPCCounter rpccounter in list)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label(rpccounter.Method, new GUILayoutOption[0]);
			GUILayout.Label(System.Math.Round((double)rpccounter.AverageKBperCall, 2) + string.Empty, new GUILayoutOption[]
			{
				GUILayout.Width((float)num)
			});
			GUILayout.Label(System.Math.Round((double)rpccounter.kilabytesLastSecondSecond, 2) + string.Empty, new GUILayoutOption[]
			{
				GUILayout.Width((float)num)
			});
			GUILayout.Label(System.Math.Round((double)rpccounter.totalKbps, 2) + string.Empty, new GUILayoutOption[]
			{
				GUILayout.Width((float)num)
			});
			GUILayout.Label(rpccounter.CallsLastSecondSecond + string.Empty, new GUILayoutOption[]
			{
				GUILayout.Width((float)num)
			});
			GUILayout.Label(rpccounter.totalCalls + string.Empty, new GUILayoutOption[]
			{
				GUILayout.Width((float)num)
			});
			GUILayout.Label(System.Math.Round((double)rpccounter.personalWorst, 2) + string.Empty, new GUILayoutOption[]
			{
				GUILayout.Width((float)num)
			});
			GUILayout.Label(System.Math.Round((double)rpccounter.overallWorst, 2) + string.Empty, new GUILayoutOption[]
			{
				GUILayout.Width((float)num)
			});
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
	}

	private int SortByTotal(BandwidthMonitor.RPCCounter A, BandwidthMonitor.RPCCounter B)
	{
		if (A.totalCalls < B.totalCalls)
		{
			return 1;
		}
		if (A.totalCalls > B.totalCalls)
		{
			return -1;
		}
		return 0;
	}

	private int SortByCallsLastSecond(BandwidthMonitor.RPCCounter A, BandwidthMonitor.RPCCounter B)
	{
		if (A.CallsLastSecondSecond < B.CallsLastSecondSecond)
		{
			return 1;
		}
		if (A.CallsLastSecondSecond > B.CallsLastSecondSecond)
		{
			return -1;
		}
		return 0;
	}

	private int SortByTotalKB(BandwidthMonitor.RPCCounter A, BandwidthMonitor.RPCCounter B)
	{
		if (A.totalKbps < B.totalKbps)
		{
			return 1;
		}
		if (A.totalKbps > B.totalKbps)
		{
			return -1;
		}
		return 0;
	}

	private int SortByKBlastSecond(BandwidthMonitor.RPCCounter A, BandwidthMonitor.RPCCounter B)
	{
		if (A.kilabytesLastSecondSecond < B.kilabytesLastSecondSecond)
		{
			return 1;
		}
		if (A.kilabytesLastSecondSecond > B.kilabytesLastSecondSecond)
		{
			return -1;
		}
		return 0;
	}

	private int SortByAveragePerCall(BandwidthMonitor.RPCCounter A, BandwidthMonitor.RPCCounter B)
	{
		if (A.AverageKBperCall < B.AverageKBperCall)
		{
			return 1;
		}
		if (A.AverageKBperCall > B.AverageKBperCall)
		{
			return -1;
		}
		return 0;
	}

	private int SortByPersonalWorst(BandwidthMonitor.RPCCounter A, BandwidthMonitor.RPCCounter B)
	{
		if (A.personalWorst < B.personalWorst)
		{
			return 1;
		}
		if (A.personalWorst > B.personalWorst)
		{
			return -1;
		}
		return 0;
	}

	private int SortByOverallWorst(BandwidthMonitor.RPCCounter A, BandwidthMonitor.RPCCounter B)
	{
		if (A.overallWorst < B.overallWorst)
		{
			return 1;
		}
		if (A.overallWorst > B.overallWorst)
		{
			return -1;
		}
		return 0;
	}

	private Dictionary<string, BandwidthMonitor.RPCCounter> rpcCounters = new Dictionary<string, BandwidthMonitor.RPCCounter>();

	private int totalRPCBytes;

	private int rpcBytesThisSecond;

	private int rpcBytesInTheLastSecond;

	private float timer;

	private float WorstSecond;

	private Dictionary<string, float> RPCsInWorstSecond = new Dictionary<string, float>();

	private int sortBy;

	private Vector2 scroll = default(Vector2);

	private class RPCCounter
	{
		public RPCCounter(string method)
		{
			this.Method = method;
		}

		public void RegisterRPC(int byteCount)
		{
			this.totalCalls++;
			this.CallsThisSecond++;
			float num = (float)byteCount * 0.000976562f;
			this.totalKbps += num;
			this.kilabytesThisSecond += num;
		}

		public void RefreshLastSecond()
		{
			this.CallsLastSecondSecond = this.CallsThisSecond;
			this.CallsThisSecond = 0;
			this.kilabytesLastSecondSecond = this.kilabytesThisSecond;
			this.kilabytesThisSecond = 0f;
			this.personalWorst = Mathf.Max(this.personalWorst, this.kilabytesLastSecondSecond);
		}

		public void UpdateOverallWorst()
		{
			this.overallWorst = this.kilabytesLastSecondSecond;
		}

		public float AverageKBperCall
		{
			get
			{
				return this.totalKbps / (float)this.totalCalls;
			}
		}

		public int totalCalls;

		public float totalKbps;

		public float MostKB_in_a_second;

		public string Method;

		private int CallsThisSecond;

		public int CallsLastSecondSecond;

		private float kilabytesThisSecond;

		public float kilabytesLastSecondSecond;

		public float personalWorst;

		public float overallWorst;
	}
}
