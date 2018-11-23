// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class ScreenDebug : MonoBehaviour
{
	public static List<ScreenDebug.Message> Messages
	{
		get
		{
			return ScreenDebug.messages;
		}
	}

	private void Awake()
	{
		if (this.logToFileImmediate && ScreenDebug.logStream == null)
		{
			ScreenDebug.logStream = new LogStream();
			MonoBehaviour.print("Awake");
		}
	}

	private void OnDisable()
	{
		Application.RegisterLogCallbackThreaded(null);
	}

	private bool CheckAgainstFilters(string text)
	{
		foreach (ScreenDebug.Message message in this.filters)
		{
			if (message.text == text)
			{
				return false;
			}
		}
		return true;
	}

	private void AddFilter(ScreenDebug.Message msg)
	{
		this.filters.Add(msg);
		for (int i = ScreenDebug.messages.Count - 1; i >= 0; i--)
		{
			if (ScreenDebug.messages[i].text == msg.text)
			{
				ScreenDebug.messages.RemoveAt(i);
			}
		}
	}

	private void Update()
	{
		ShowMouseController.ShowMouse = (this.state == ScreenDebug.State.Full);
		bool flag = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
		if (Input.GetKeyDown(KeyCode.BackQuote) && flag)
		{
			int num = (int)this.state;
			num++;
			num %= 3;
			this.state = (ScreenDebug.State)num;
		}
	}

	private void OnGUI()
	{
		bool flag = this.page == this.numberOfPages - 1;
		this.numberOfPages = Mathf.CeilToInt((float)ScreenDebug.messages.Count / (float)this.entriesPerPage);
		if (flag)
		{
			this.page = this.numberOfPages - 1;
		}
		string str = string.Concat(new object[]
		{
			"Page: ",
			this.page + 1,
			"/",
			this.numberOfPages
		});
		this.WindowRect.width = (float)Screen.width;
		this.WindowRect.height = (float)Screen.height;
		if (this.state == ScreenDebug.State.Full)
		{
			this.WindowRect = GUILayout.Window(this.WindowId, this.WindowRect, new GUI.WindowFunction(this.windowWindow), str + " (ctrl + ~)", new GUILayoutOption[0]);
		}
		else if (this.state == ScreenDebug.State.Overlay)
		{
			this.DrawOverLay();
		}
	}

	private void windowWindow(int windowId)
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (GUILayout.Button("◀◀ First Page", new GUILayoutOption[]
		{
			GUILayout.Height(50f)
		}))
		{
			this.page = 0;
		}
		if (GUILayout.Button("◀ Previous Page", new GUILayoutOption[]
		{
			GUILayout.Height(50f)
		}))
		{
			this.page--;
		}
		GUILayout.Space(10f);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		if (this.stopLogging)
		{
			if (GUILayout.Button("Start Logging", new GUILayoutOption[0]))
			{
				this.stopLogging = false;
			}
		}
		else if (GUILayout.Button("Stop Logging", new GUILayoutOption[0]))
		{
			this.stopLogging = true;
		}
		if (GUILayout.Button("Clear", new GUILayoutOption[0]))
		{
			ScreenDebug.messages.Clear();
		}
		GUILayout.EndVertical();
		GUILayout.Space(10f);
		if (GUILayout.Button("Next Page ▶", new GUILayoutOption[]
		{
			GUILayout.Height(50f)
		}))
		{
			this.page++;
		}
		if (GUILayout.Button("Latest Page ▶▶", new GUILayoutOption[]
		{
			GUILayout.Height(50f)
		}))
		{
			this.page = this.numberOfPages - 1;
		}
		this.page = Mathf.Clamp(this.page, 0, this.numberOfPages - 1);
		GUILayout.EndHorizontal();
		GUILayout.Space(20f);
		Color color = GUI.color;
		this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, new GUILayoutOption[0]);
		for (int i = 0; i < this.entriesPerPage; i++)
		{
			int num = this.page * this.entriesPerPage + i;
			if (num >= ScreenDebug.messages.Count)
			{
				break;
			}
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUI.color = ScreenDebug.messages[num].color;
			if (GUILayout.Button(ScreenDebug.messages[num].text, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(true)
			}))
			{
				ScreenDebug.messages[num].ShowStack = !ScreenDebug.messages[num].ShowStack;
			}
			if (GUILayout.Button("-", new GUILayoutOption[]
			{
				GUILayout.Width(50f)
			}))
			{
				this.AddFilter(ScreenDebug.messages[num]);
			}
			GUILayout.EndHorizontal();
			if (ScreenDebug.messages[num].ShowStack)
			{
				GUILayout.TextArea(ScreenDebug.messages[num].stackTrace, new GUILayoutOption[]
				{
					GUILayout.Width((float)Screen.width)
				});
			}
			if (i > 50)
			{
				break;
			}
		}
		GUILayout.EndScrollView();
		GUI.DragWindow();
		GUI.color = color;
	}

	private void DrawOverLay()
	{
		GUILayout.BeginArea(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
		for (int i = 0; i < ScreenDebug.messages.Count; i++)
		{
			int num = ScreenDebug.messages.Count - 1 - i;
			GUI.color = ScreenDebug.messages[num].color;
			GUILayout.Label(string.Concat(new object[]
			{
				"  ",
				num,
				")  ",
				ScreenDebug.messages[num].text
			}), new GUILayoutOption[0]);
			if (i > 50)
			{
				break;
			}
		}
		GUILayout.EndArea();
	}

	public static void DrawTextToScreen(Vector3 pos, string str)
	{
		Vector3 vector = Camera.main.WorldToScreenPoint(pos);
		vector.y = (float)Screen.height - vector.y;
		GUI.Label(new Rect(vector.x, vector.y, 50f, 50f), new GUIContent(str));
	}

	private const int logLimit = 5000;

	private static List<ScreenDebug.Message> writeToFileMessages = new List<ScreenDebug.Message>();

	private static List<ScreenDebug.Message> messages = new List<ScreenDebug.Message>();

	private int WindowId = 109;

	private Vector2 scrollPos = Vector2.zero;

	private Rect WindowRect = new Rect(0f, 0f, 160f, 100f);

	private bool reverseOrder;

	private bool stopLogging;

	private static LogStream logStream;

	private bool logToFileImmediate = true;

	public ScreenDebug.State state;

	private List<ScreenDebug.Message> filters = new List<ScreenDebug.Message>();

	private int page;

	private int entriesPerPage = 30;

	private int numberOfPages = 1;

	public class Message
	{
		public string text;

		public string stackTrace = string.Empty;

		public bool ShowStack;

		public Color color;
	}

	public enum State
	{
		Hidden,
		Full,
		Overlay
	}
}
