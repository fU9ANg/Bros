// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SpawnBlockActionInfo : TriggerActionInfo
{
	public override void ShowGUI(LevelEditorGUI gui)
	{
		if (GUILayout.Button(string.Concat(new object[]
		{
			"Set Target (currently C ",
			this.targetPoint.collumn,
			" R ",
			this.targetPoint.row,
			")"
		}), new GUILayoutOption[0]))
		{
			gui.settingWaypoint = true;
			gui.waypointToSet = this.targetPoint;
		}
		if (GUILayout.Toggle(this.groundType == 26, "Spawn Barrel", new GUILayoutOption[0]))
		{
			this.groundType = 26;
		}
		if (GUILayout.Toggle(this.groundType == 40, "Spawn Steel", new GUILayoutOption[0]))
		{
			this.groundType = 40;
		}
		if (GUILayout.Toggle(this.disturbed, "Disturb Block On Spawn", new GUILayoutOption[0]))
		{
			this.disturbed = true;
		}
		else
		{
			this.disturbed = false;
		}
	}

	public GridPoint targetPoint = new GridPoint(0, 0);

	public int groundType = 26;

	public bool disturbed;
}
