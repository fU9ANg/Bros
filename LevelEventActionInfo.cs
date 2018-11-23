// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventActionInfo : TriggerActionInfo
{
	public override void ShowGUI(LevelEditorGUI gui)
	{
		if (LevelEventActionInfo.types == null)
		{
			LevelEventActionInfo.types = new List<LevelEventActionType>();
			foreach (object obj in Enum.GetValues(typeof(LevelEventActionType)))
			{
				LevelEventActionInfo.types.Add((LevelEventActionType)((int)obj));
			}
		}
		this.levelActionType = (LevelEventActionType)((int)LevelEditorGUI.SelectList(LevelEventActionInfo.types, this.levelActionType, gui.skin.label, gui.skin.button));
		if (this.levelActionType == LevelEventActionType.Cutscene)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Cutscene No:", new GUILayoutOption[0]);
			int.TryParse(GUILayout.TextField(this.cutsceneNumber.ToString(), new GUILayoutOption[0]), out this.cutsceneNumber);
			GUILayout.EndHorizontal();
		}
		if (this.levelActionType == LevelEventActionType.GoToLevelSilent)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Level Number:", new GUILayoutOption[0]);
			int.TryParse(GUILayout.TextField(this.cutsceneNumber.ToString(), new GUILayoutOption[0]), out this.cutsceneNumber);
			GUILayout.EndHorizontal();
		}
		if (this.levelActionType == LevelEventActionType.ActivateTrigger || this.levelActionType == LevelEventActionType.DeactivateTrigger)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Trigger name:", new GUILayoutOption[0]);
			this.triggerName = GUILayout.TextField(this.triggerName ?? string.Empty, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
		}
		if (this.levelActionType == LevelEventActionType.CallHelicopter)
		{
			if (this.pos == null)
			{
				this.pos = new GridPoint();
			}
			gui.MarkTargetPoint(this.pos);
			if (GUILayout.Button("Set Target (Currently " + this.pos.ToString() + ")", new GUILayoutOption[0]))
			{
				gui.settingWaypoint = true;
				gui.waypointToSet = this.pos;
			}
		}
		if (this.levelActionType == LevelEventActionType.TimeSlowdown)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Timescale:", new GUILayoutOption[0]);
			float.TryParse(GUILayout.TextField(this.value1.ToString("0.00"), new GUILayoutOption[0]), out this.value1);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Duration (REAL time):", new GUILayoutOption[0]);
			float.TryParse(GUILayout.TextField(this.value2.ToString("0.00"), new GUILayoutOption[0]), out this.value2);
			GUILayout.EndHorizontal();
		}
	}

	public LevelEventActionType levelActionType;

	public int cutsceneNumber;

	public string triggerName;

	public GridPoint pos;

	public float value1;

	public float value2;

	private static List<LevelEventActionType> types;
}
