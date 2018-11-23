// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ExplosionActionInfo : TriggerActionInfo
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
			gui.MarkTargetPoint(this.targetPoint);
		}
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Damage:", new GUILayoutOption[0]);
		int.TryParse(GUILayout.TextField(this.damage.ToString(), new GUILayoutOption[0]), out this.damage);
		GUILayout.EndHorizontal();
	}

	public GridPoint targetPoint = new GridPoint(0, 0);

	public int damage;
}
