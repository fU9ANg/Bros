// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ShowMouseController : MonoBehaviour
{
	public static bool ShowMouse
	{
		get
		{
			return ShowMouseController.showMouse;
		}
		set
		{
			ShowMouseController.showMouse = (value || ShowMouseController.showMouse);
		}
	}

	public static bool ForceMouse
	{
		get
		{
			return ShowMouseController.forceMouse;
		}
		set
		{
			Cursor.visible = value;
			ShowMouseController.forceMouse = value;
		}
	}

	private void Update()
	{
		Cursor.visible = (ShowMouseController.showMouse || ShowMouseController.forceMouse);
		ShowMouseController.showMouse = false;
	}

	private static bool showMouse;

	private static bool forceMouse;
}
