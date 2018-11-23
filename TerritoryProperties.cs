// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[Serializable]
public class TerritoryProperties
{
	public string territoryName = "VIETNAME";

	public string campaignName = "NUL";

	public string campaignDescription = string.Empty;

	public TerritoryState state;

	public LevelTheme theme;

	public WeatherType weatherType = WeatherType.NoChange;

	public RainType rainType = RainType.NoChange;

	public int terroristLevel;

	public int infestationLevel;

	public int populationLevel = 1;

	public int threatLevel;

	public Color threatColor = Color.white;

	public string threatName = "SALMON";

	public bool isCity;

	public int failCount;
}
