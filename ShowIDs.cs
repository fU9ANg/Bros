// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ShowIDs : SingletonMono<ShowIDs>
{
	private void Start()
	{
	}

	public static void Refresh()
	{
		if (SingletonMono<ShowIDs>.Instance == null)
		{
			return;
		}
		SingletonMono<ShowIDs>.Instance.units = (UnityEngine.Object.FindObjectsOfType(typeof(NetworkedUnit)) as NetworkedUnit[]);
	}

	public void DrawUnits()
	{
		foreach (NetworkedUnit networkedUnit in this.units)
		{
			Vector3 position = networkedUnit.transform.position;
			position = Camera.main.WorldToScreenPoint(position);
			string text = string.Empty + networkedUnit.Owner;
			if (networkedUnit.IsMine)
			{
				text = "IsMine";
			}
			GUI.Label(new Rect(position.x - 12f, (float)Screen.height - position.y - 100f, 100f, 30f), text);
		}
	}

	public static Vector2 GetScreenPos(Vector3 worldPos)
	{
		Vector2 result = Camera.main.WorldToScreenPoint(worldPos);
		result.y = (float)Screen.height - result.y;
		return result;
	}

	private NetworkedUnit[] units = new NetworkedUnit[0];
}
