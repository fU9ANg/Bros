// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ExecuteFunctionActionInfo : TriggerActionInfo
{
	public override void ShowGUI(LevelEditorGUI gui)
	{
		this.callFunctionOnBros = GUILayout.Toggle(this.callFunctionOnBros, "Call function on bros", new GUILayoutOption[0]);
		if (!this.callFunctionOnBros)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Entity: ", new GUILayoutOption[0]);
			this.entity = GUILayout.TextField(this.entity ?? string.Empty, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
		}
		else
		{
			this.entity = string.Empty;
		}
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Function: ", new GUILayoutOption[0]);
		this.function = GUILayout.TextField(this.function ?? string.Empty, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Toggle(this.useScriptedVariable, "Use Scripted Value As Parameter", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (this.useScriptedVariable)
		{
			GUILayout.Label("Variable: ", new GUILayoutOption[0]);
			this.variableName = GUILayout.TextField(this.variableName ?? string.Empty, new GUILayoutOption[0]);
		}
		else
		{
			GUILayout.Label("Parameter: ", new GUILayoutOption[0]);
			this.parameter = GUILayout.TextField(this.parameter ?? string.Empty, new GUILayoutOption[0]);
		}
		GUILayout.EndHorizontal();
	}

	public string entity;

	public string function;

	public string variableName;

	public string parameter;

	public bool useScriptedVariable;

	public bool callFunctionOnBros;
}
