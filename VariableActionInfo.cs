// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class VariableActionInfo : TriggerActionInfo
{
	public override void ShowGUI(LevelEditorGUI gui)
	{
		GUILayout.Label("Variable Identifier:", new GUILayoutOption[0]);
		this.variableName = GUILayout.TextField(this.variableName, 24, new GUILayoutOption[0]);
		GUILayout.Label("Increment By:", new GUILayoutOption[0]);
		float.TryParse(GUILayout.TextField(this.alterByValue.ToString(), new GUILayoutOption[0]), out this.alterByValue);
	}

	public string variableName = "Variable1";

	public float alterByValue = 1f;
}
