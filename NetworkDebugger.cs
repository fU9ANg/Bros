// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class NetworkDebugger : MonoBehaviour
{
	public static void Hide()
	{
		NetworkDebugger.current = NetworkDebugger.Windows.None;
	}

	private void Update()
	{
		ShowMouseController.ShowMouse = (NetworkDebugger.current != NetworkDebugger.Windows.None);
		NetworkObject.ShowInterpolatedPosiontMarkers = (NetworkDebugger.current == NetworkDebugger.Windows.Info);
		bool flag = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
		if (Input.GetKeyDown(KeyCode.RightBracket) && flag)
		{
			int num = (int)(NetworkDebugger.current + 1);
			num %= 8;
			NetworkDebugger.current = (NetworkDebugger.Windows)num;
		}
		if (Input.GetKeyDown(KeyCode.LeftBracket) && flag)
		{
			int num2 = (int)(NetworkDebugger.current + 7);
			num2 %= 8;
			NetworkDebugger.current = (NetworkDebugger.Windows)num2;
		}
		this.WindowRect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
	}

	private void OnGUI()
	{
		if (NetworkDebugger.current != NetworkDebugger.Windows.None)
		{
			this.WindowRect = GUILayout.Window(this.windowID, this.WindowRect, new GUI.WindowFunction(this.WindowGUI), NetworkDebugger.current + "  (Toggle: ])", new GUILayoutOption[]
			{
				GUILayout.ExpandHeight(true)
			});
		}
	}

	private void WindowGUI(int windowId)
	{
		NetworkDebugger.width = Mathf.Clamp(NetworkDebugger.width, NetworkDebugger.widthIncrements, (float)Screen.width);
		this.scroll = GUILayout.BeginScrollView(this.scroll, new GUILayoutOption[0]);
		switch (NetworkDebugger.current)
		{
		case NetworkDebugger.Windows.Lobby:
			if (SingletonMono<DebugLobby>.Instance != null)
			{
				SingletonMono<DebugLobby>.Instance.DrawLobbyInfo();
			}
			if (SingletonMono<NetworkLog>.Instance != null)
			{
				SingletonMono<NetworkLog>.Instance.DrawLog();
			}
			break;
		case NetworkDebugger.Windows.Info:
			if (SingletonMono<ConnectInfo>.Instance != null)
			{
				SingletonMono<ConnectInfo>.Instance.DrawConnectInfo();
			}
			break;
		case NetworkDebugger.Windows.HeroController:
			if (HeroController.Instance != null)
			{
				HeroController.Instance.DebugDraw();
			}
			break;
		case NetworkDebugger.Windows.Bandwidth:
			if (SingletonMono<Analytics>.Instance != null)
			{
				SingletonMono<Analytics>.Instance.Draw();
			}
			break;
		case NetworkDebugger.Windows.DrawRegistry:
			if (SingletonMono<Registry>.Instance != null)
			{
				SingletonMono<Registry>.Instance.DrawRegistry();
			}
			break;
		case NetworkDebugger.Windows.DrawRegistrationOrder:
			if (SingletonMono<Registry>.Instance != null)
			{
				SingletonMono<Registry>.Instance.DrawRegistrationOrder();
			}
			break;
		}
		GUILayout.EndScrollView();
		GUI.DragWindow();
	}

	private static float widthIncrements = 200f;

	private static float width = NetworkDebugger.widthIncrements * 3f;

	private static NetworkDebugger.Windows current = NetworkDebugger.Windows.None;

	private int windowID = 1111;

	private Rect WindowRect = new Rect((float)Screen.width - NetworkDebugger.width, 0f, NetworkDebugger.width, (float)Screen.height);

	private Vector2 scroll = Vector2.zero;

	private enum Windows
	{
		Lobby,
		Info,
		HeroController,
		Bandwidth,
		UnitPIDs,
		DrawRegistry,
		DrawRegistrationOrder,
		None
	}
}
