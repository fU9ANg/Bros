// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SpawnResourceActionInfo : TriggerActionInfo
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
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Resource Name:", new GUILayoutOption[0]);
		this.resourceName = GUILayout.TextField(this.resourceName ?? string.Empty, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Call Method On Object:", new GUILayoutOption[0]);
		this.callMethod = GUILayout.TextField(this.callMethod ?? string.Empty, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Tag new object as:", new GUILayoutOption[0]);
		this.newObjectTag = GUILayout.TextField(this.newObjectTag ?? string.Empty, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
	}

	public string resourceName;

	public GridPoint targetPoint = new GridPoint(0, 0);

	public string callMethod = string.Empty;

	public string newObjectTag;
}
