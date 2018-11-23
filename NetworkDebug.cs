// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkDebug : MonoBehaviour
{
	private void Awake()
	{
		NetworkDebug.instance = this;
	}

	private void Update()
	{
		NetworkDebug.DebugMessages.Clear();
		if (Input.GetKeyDown(KeyCode.N))
		{
			this.displayWindow = !this.displayWindow;
		}
		this.UpdateDebugText();
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.showBlockIds = !this.showBlockIds;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.showEnemyIds = !this.showEnemyIds;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			this.showDoodadIds = !this.showDoodadIds;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			this.ShowDebugInfo = !this.ShowDebugInfo;
		}
		if (!Info.IsDevBuild)
		{
			this.showBlockIds = false;
			this.showEnemyIds = false;
			this.showDoodadIds = false;
			this.ShowDebugInfo = false;
			this.displayWindow = false;
		}
	}

	private void OnGUI()
	{
	}

	private void WindowGUI(int windowId)
	{
	}

	private void ShowRPCCallCounts()
	{
		GUILayout.Label("Show RPC Break Down" + this.showRPCBreakDown, new GUILayoutOption[0]);
		GUIContent[] array = new GUIContent[]
		{
			new GUIContent("Avrg RPC size"),
			new GUIContent("Per Second"),
			new GUIContent("Worst Second"),
			new GUIContent("Worst per RPC"),
			new GUIContent("Total")
		};
		this.showRPCBreakDown = GUILayout.SelectionGrid(this.showRPCBreakDown, array, array.Length, new GUILayoutOption[0]);
		GUILayout.Label("rpcCallsThisMap " + NetworkAnalysis.instance.rpcCallsThisMap.Count, new GUILayoutOption[0]);
		GUILayout.Label("rpcCallsInTheLastSec " + NetworkAnalysis.instance.rpcCallsInTheLastSec.Count, new GUILayoutOption[0]);
		GUILayout.Label("worstSecondOfRPCs " + NetworkAnalysis.instance.worstSecondOfRPCs.Count, new GUILayoutOption[0]);
		GUILayout.Label("worstCasePerRPC " + NetworkAnalysis.instance.worstCasePerRPC.Count, new GUILayoutOption[0]);
		GUILayout.Label("rpcCallsThisMap " + NetworkAnalysis.instance.rpcCallsThisMap.Count, new GUILayoutOption[0]);
		if (this.showRPCBreakDown == 0)
		{
			this.DrawAverageRPCSizes(NetworkAnalysis.instance.rpcCallsThisMap);
		}
		else if (this.showRPCBreakDown == 1)
		{
			this.DrawRPCBreakDown(NetworkAnalysis.instance.rpcCallsInTheLastSec);
		}
		else if (this.showRPCBreakDown == 2)
		{
			this.DrawRPCBreakDown(NetworkAnalysis.instance.worstSecondOfRPCs);
		}
		else if (this.showRPCBreakDown == 3)
		{
			this.DrawRPCBreakDown(NetworkAnalysis.instance.worstCasePerRPC);
		}
		else if (this.showRPCBreakDown == 4)
		{
			this.DrawRPCBreakDown(NetworkAnalysis.instance.rpcCallsThisMap);
		}
	}

	private float Round(float f)
	{
		return (float)System.Math.Round((double)f, 1);
	}

	private void DrawRPCBreakDown(Dictionary<string, RPCdata> rpcCounts)
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Totals:", new GUILayoutOption[]
		{
			GUILayout.Width(250f)
		});
		GUILayout.FlexibleSpace();
		GUILayout.Label(NetworkAnalysis.GetTotalRpcCount(rpcCounts) + string.Empty, new GUILayoutOption[]
		{
			GUILayout.Width(50f)
		});
		GUILayout.Label(this.Round(NetworkAnalysis.GetTotalRpcKilobits(rpcCounts)) + string.Empty, new GUILayoutOption[]
		{
			GUILayout.Width(50f)
		});
		GUILayout.Label("100%", new GUILayoutOption[]
		{
			GUILayout.Width(60f)
		});
		GUILayout.EndHorizontal();
		float totalRpcKilobits = NetworkAnalysis.GetTotalRpcKilobits(rpcCounts);
		this.rpcScroll = GUILayout.BeginScrollView(this.rpcScroll, new GUILayoutOption[]
		{
			GUILayout.MinHeight(150f),
			GUILayout.MaxHeight(200f)
		});
		foreach (KeyValuePair<string, RPCdata> keyValuePair in this.GetSortedKeyPairs(rpcCounts))
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label(keyValuePair.Key + ":", new GUILayoutOption[]
			{
				GUILayout.Width(250f)
			});
			GUILayout.FlexibleSpace();
			GUILayout.Label(keyValuePair.Value.calls + string.Empty, new GUILayoutOption[]
			{
				GUILayout.Width(50f)
			});
			GUILayout.Label(System.Math.Round((double)keyValuePair.Value.kilobits, 1) + " kb", new GUILayoutOption[]
			{
				GUILayout.Width(50f)
			});
			GUILayout.Label(System.Math.Round((double)(keyValuePair.Value.kilobits * 100f / totalRpcKilobits), 1) + "%", new GUILayoutOption[]
			{
				GUILayout.Width(60f)
			});
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
	}

	private void DrawAverageRPCSizes(Dictionary<string, RPCdata> rpcCounts)
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Average RPC Sizes:", new GUILayoutOption[]
		{
			GUILayout.Width(250f)
		});
		GUILayout.FlexibleSpace();
		GUILayout.Label("Calls", new GUILayoutOption[]
		{
			GUILayout.Width(50f)
		});
		GUILayout.Label("Total Kbs", new GUILayoutOption[]
		{
			GUILayout.Width(50f)
		});
		GUILayout.Label("Ave Kbs", new GUILayoutOption[]
		{
			GUILayout.Width(50f)
		});
		GUILayout.Label(string.Empty, new GUILayoutOption[]
		{
			GUILayout.Width(20f)
		});
		GUILayout.EndHorizontal();
		this.rpcScroll = GUILayout.BeginScrollView(this.rpcScroll, new GUILayoutOption[]
		{
			GUILayout.MinHeight(150f),
			GUILayout.MaxHeight(200f)
		});
		foreach (KeyValuePair<string, RPCdata> keyValuePair in this.GetSortedKeyPairs(rpcCounts))
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label(keyValuePair.Key + ":", new GUILayoutOption[]
			{
				GUILayout.Width(250f)
			});
			GUILayout.FlexibleSpace();
			GUILayout.Label(keyValuePair.Value.calls + string.Empty, new GUILayoutOption[]
			{
				GUILayout.Width(50f)
			});
			GUILayout.Label(System.Math.Round((double)keyValuePair.Value.kilobits, 1) + string.Empty, new GUILayoutOption[]
			{
				GUILayout.Width(50f)
			});
			GUILayout.Label(System.Math.Round((double)(keyValuePair.Value.kilobits / (float)keyValuePair.Value.calls), 1) + "kb/rpc", new GUILayoutOption[]
			{
				GUILayout.Width(60f)
			});
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
	}

	private List<KeyValuePair<string, RPCdata>> GetSortedKeyPairs(Dictionary<string, RPCdata> rpcCounts)
	{
		List<KeyValuePair<string, RPCdata>> list = new List<KeyValuePair<string, RPCdata>>();
		foreach (KeyValuePair<string, RPCdata> item in rpcCounts)
		{
			list.Add(item);
		}
		list.Sort((KeyValuePair<string, RPCdata> firstPair, KeyValuePair<string, RPCdata> nextPair) => nextPair.Value.kilobits.CompareTo(firstPair.Value.kilobits));
		return list;
	}

	private void DrawDebugTexts()
	{
		for (int i = this.debugTexts.Count - 1; i >= 0; i--)
		{
			int fontSize = GUI.skin.label.fontSize;
			Vector3 vector = Camera.main.WorldToScreenPoint(this.debugTexts[i].worldPos);
			Color color = GUI.color;
			GUI.color = Color.white;
			GUI.Label(new Rect(vector.x, (float)Screen.height - vector.y, 30f, 30f), this.debugTexts[i].text);
			GUI.color = color;
			GUI.skin.label.fontSize = fontSize;
		}
	}

	private void UpdateDebugText()
	{
		for (int i = this.debugTexts.Count - 1; i >= 0; i--)
		{
			if (this.debugTexts[i].duration < 0f)
			{
				this.debugTexts.RemoveAt(i);
			}
			else
			{
				this.debugTexts[i].duration -= Time.deltaTime;
			}
		}
	}

	public static void AddDebugText(string text, Vector3 worldPos, float duration)
	{
		if (!Info.IsDevBuild)
		{
			return;
		}
		NetworkDebug.DebugText debugText = new NetworkDebug.DebugText();
		debugText.text = text;
		debugText.worldPos = worldPos;
		debugText.duration = duration;
		NetworkDebug.instance.debugTexts.Add(debugText);
	}

	public static NetworkDebug instance;

	public bool ShowDebugInfo;

	public bool showBlockIds;

	public bool showEnemyIds;

	public bool showDoodadIds;

	public bool displayWindow = true;

	public static List<string> DebugMessages = new List<string>();

	private Vector2 rpcScroll = Vector2.zero;

	private int showRPCBreakDown;

	private List<NetworkDebug.DebugText> debugTexts = new List<NetworkDebug.DebugText>();

	private class DebugText
	{
		public string text = "not set";

		public float duration;

		public Vector3 worldPos;
	}
}
