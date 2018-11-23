// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WorldMapProgress
{
	public WorldMapProgress()
	{
		this.territoryProgress = new List<TerritoryProgress>();
	}

	public TerritoryProgress GetTerritoryProgress(string name)
	{
		for (int i = 0; i < this.territoryProgress.Count; i++)
		{
			if (this.territoryProgress[i].name.ToLower() == name.ToLower())
			{
				return this.territoryProgress[i];
			}
		}
		return null;
	}

	public TerritoryProgress AddTerritory(string name, TerritoryState state, int terrorLevel, int infestationLevel, int populationLevel, int fails)
	{
		if (this.territoryProgress == null)
		{
			UnityEngine.Debug.LogError("Territory Progress is null");
			this.territoryProgress = new List<TerritoryProgress>();
		}
		TerritoryProgress territoryProgress = new TerritoryProgress(name, state, terrorLevel, infestationLevel, populationLevel, fails);
		this.territoryProgress.Add(territoryProgress);
		return territoryProgress;
	}

	public WorldMapProgress Copy()
	{
		WorldMapProgress worldMapProgress = new WorldMapProgress();
		worldMapProgress.territoryProgress = new List<TerritoryProgress>();
		for (int i = 0; i < this.territoryProgress.Count; i++)
		{
			TerritoryProgress item = new TerritoryProgress(this.territoryProgress[i].name, this.territoryProgress[i].state, this.territoryProgress[i].terrorLevel, this.territoryProgress[i].infestationLevel, this.territoryProgress[i].populationLevel, this.territoryProgress[i].failCount);
			worldMapProgress.territoryProgress.Add(item);
		}
		worldMapProgress.worldLiberty = this.worldLiberty;
		worldMapProgress.worldTerror = this.worldTerror;
		worldMapProgress.turnCount = this.turnCount;
		return worldMapProgress;
	}

	public List<TerritoryProgress> territoryProgress;

	public int worldLiberty;

	public int worldTerror;

	public int turnCount;

	public string currentTerritoryName;

	public string lastSafeTerritory;
}
