// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeatherActionInfo : TriggerActionInfo
{
	public override void ShowGUI(LevelEditorGUI gui)
	{
		GUILayout.Label("Change Weather to:", new GUILayoutOption[0]);
		this.weatherType = (WeatherType)((int)LevelEditorGUI.SelectList(new List<WeatherType>
		{
			WeatherType.NoChange,
			WeatherType.Day,
			WeatherType.Overcast,
			WeatherType.Burning,
			WeatherType.Night
		}, this.weatherType, LevelEditorGUI.GetGuiSkin().label, LevelEditorGUI.GetGuiSkin().button));
		GUILayout.Label("Change Rain to:", new GUILayoutOption[0]);
		this.rainType = (RainType)((int)LevelEditorGUI.SelectList(new List<RainType>
		{
			RainType.NoChange,
			RainType.Clear,
			RainType.Drizzle,
			RainType.Raining
		}, this.rainType, LevelEditorGUI.GetGuiSkin().label, LevelEditorGUI.GetGuiSkin().button));
	}

	public WeatherType weatherType;

	public RainType rainType;
}
