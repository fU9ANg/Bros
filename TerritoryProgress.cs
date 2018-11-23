// dnSpy decompiler from Assembly-CSharp.dll
using System;

[Serializable]
public class TerritoryProgress
{
	public TerritoryProgress(string n, TerritoryState s)
	{
		this.name = n;
		this.state = s;
	}

	public TerritoryProgress(string n, TerritoryState s, int terror, int infest, int population, int fails)
	{
		this.name = n;
		this.state = s;
		this.terrorLevel = terror;
		this.infestationLevel = infest;
		this.populationLevel = population;
		this.failCount = fails;
	}

	public string name = "NULL";

	public TerritoryState state;

	public int terrorLevel;

	public int infestationLevel;

	public int populationLevel;

	public int failCount;
}
