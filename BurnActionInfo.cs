// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BurnActionInfo : TriggerActionInfo
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
	}

	public GridPoint targetPoint = new GridPoint(0, 0);

	public int damage;
}
