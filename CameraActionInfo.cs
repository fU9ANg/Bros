// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraActionInfo : TriggerActionInfo
{
	public override void ShowGUI(LevelEditorGUI gui)
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		this.holdPlayersInCutscene = GUILayout.Toggle(this.holdPlayersInCutscene, "Hold Players in Cutscene:", new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		this.killOffscreenPlayers = GUILayout.Toggle(this.killOffscreenPlayers, "Kill Offscreen Players:", new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		this.smootherCamMovement = GUILayout.Toggle(this.smootherCamMovement, "Smoother Camera Movement:", new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		this.letterbox = GUILayout.Toggle(this.letterbox, "ShowLetterBox:", new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Letter Box Amount (zero for away):", new GUILayoutOption[0]);
		float.TryParse(GUILayout.TextField(this.letterboxAmount.ToString(), new GUILayoutOption[0]), out this.letterboxAmount);
		GUILayout.EndHorizontal();
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
		gui.MarkTargetPoint(this.targetPoint, Color.green);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Zoom:", new GUILayoutOption[0]);
		float.TryParse(GUILayout.TextField(this.zoom.ToString("#.00"), new GUILayoutOption[0]), out this.zoom);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Time:", new GUILayoutOption[0]);
		float.TryParse(GUILayout.TextField(this.time.ToString("#.00"), new GUILayoutOption[0]), out this.time);
		GUILayout.EndHorizontal();
		if (this.posList != null && this.posList.Count > 0)
		{
			for (int i = 0; i < this.posList.Count; i++)
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.Label("Position " + (i + 2).ToString(), new GUILayoutOption[0]);
				if (GUILayout.Button("Delete", new GUILayoutOption[0]))
				{
					this.posList.RemoveAt(i);
					this.zooms.RemoveAt(i);
					this.times.RemoveAt(i);
					return;
				}
				GUILayout.EndHorizontal();
				if (GUILayout.Button(string.Concat(new object[]
				{
					"Set Target (currently C ",
					this.posList[i].collumn,
					" R ",
					this.posList[i].row,
					")"
				}), new GUILayoutOption[0]))
				{
					gui.settingWaypoint = true;
					gui.waypointToSet = this.posList[i];
				}
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.Label("Zoom:", new GUILayoutOption[0]);
				float value = this.zooms[i];
				float.TryParse(GUILayout.TextField(this.zooms[i].ToString("#.00"), new GUILayoutOption[0]), out value);
				this.zooms[i] = value;
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.Label("Time:", new GUILayoutOption[0]);
				float value2 = this.times[i];
				float.TryParse(GUILayout.TextField(this.times[i].ToString("#.00"), new GUILayoutOption[0]), out value2);
				this.times[i] = value2;
				GUILayout.EndHorizontal();
				gui.MarkTargetPoint(this.posList[i], (i != this.posList.Count - 1) ? Color.Lerp(Color.red, Color.yellow, 0.5f) : Color.red);
			}
		}
		if (GUILayout.Button("Add Position", new GUILayoutOption[0]))
		{
			if (this.posList == null)
			{
				this.posList = new List<GridPoint>();
				this.times = new List<float>();
				this.zooms = new List<float>();
			}
			this.posList.Add(new GridPoint());
			this.times.Add(1f);
			this.zooms.Add(1f);
		}
	}

	public GridPoint targetPoint = new GridPoint(0, 0);

	public float zoom = 1f;

	public float time = 1f;

	public List<GridPoint> posList;

	public List<float> times;

	public List<float> zooms;

	public bool letterbox;

	public float letterboxAmount;

	public bool holdPlayersInCutscene;

	public bool killOffscreenPlayers;

	public bool smootherCamMovement;
}
