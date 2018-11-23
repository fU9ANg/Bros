// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

[Serializable]
public class TriggerInfo
{
	public TriggerType type;

	public GridPoint bottomLeft = new GridPoint(0, 0);

	public GridPoint upperRight = new GridPoint(0, 0);

	public bool startEnabled = true;

	public string name;

	public string variableName = string.Empty;

	public float evaluateAgainstValue = 1f;

	public bool isTerrainEmpty = true;

	public bool useDefaultBrotality;

	public bool useCustomBrotality;

	public int minBrotalityLevel;

	public int enemyDeathFrequency;

	public List<TriggerActionInfo> actions = new List<TriggerActionInfo>();

	public string tag;
}
