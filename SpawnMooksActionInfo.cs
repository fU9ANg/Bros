// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SpawnMooksActionInfo : TriggerActionInfo
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
		GUILayout.Label("Mooks Count:", new GUILayoutOption[0]);
		int.TryParse(GUILayout.TextField(this.mooksCount.ToString(), new GUILayoutOption[0]), out this.mooksCount);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Suicide Mooks Count:", new GUILayoutOption[0]);
		int.TryParse(GUILayout.TextField(this.mooksSuicideCount.ToString(), new GUILayoutOption[0]), out this.mooksSuicideCount);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("X Scatter Range:", new GUILayoutOption[0]);
		int.TryParse(GUILayout.TextField(this.xRange.ToString(), new GUILayoutOption[0]), out this.xRange);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Y Scatter Range:", new GUILayoutOption[0]);
		int.TryParse(GUILayout.TextField(this.yRange.ToString(), new GUILayoutOption[0]), out this.yRange);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		this.tumble = GUILayout.Toggle(this.tumble, "Mooks Will Tumble:", new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		this.parachute = GUILayout.Toggle(this.parachute, "Mooks Will Parachute:", new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		this.isAlert = GUILayout.Toggle(this.isAlert, "Mooks Start Alert:", new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		this.isOnFire = GUILayout.Toggle(this.isOnFire, "Mooks Are On Fire:", new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		this.spawnTruck = GUILayout.Toggle(this.spawnTruck, "Spawn Truck:", new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
	}

	public int mooksCount = 1;

	public int mooksSuicideCount;

	public bool tumble;

	public bool parachute;

	public bool isAlert;

	public bool isOnFire;

	public int xRange = 16;

	public int yRange;

	public bool spawnTruck;

	public GridPoint targetPoint = new GridPoint(0, 0);
}
