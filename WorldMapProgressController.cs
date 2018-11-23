// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapProgressController : MonoBehaviour
{
	private void Awake()
	{
		WorldMapProgressController.instance = this;
		this.FindAllTerritoryObjects();
		if (WorldMapProgressController.startNewWorldMapGame || (!WorldMapProgressController.hasAlreadyStartedFromScratch && this.startFromScratch && Application.isEditor))
		{
			UnityEngine.Debug.Log("Start From Scratch");
			this.StartFromScratch();
		}
		this.LoadProgress();
		this.CheckProgressDiscrepancies();
	}

	private void Start()
	{
		if (this.CurrentTurnIsGreaterThanPreviousTurn())
		{
		}
		this.RestoreProgressEquilibrium();
	}

	protected bool CurrentTurnIsGreaterThanPreviousTurn()
	{
		return PlayerPrefs.GetInt("PreviousProgress_Turncount", 0) < PlayerPrefs.GetInt("CurrentProgress_Turncount", 0);
	}

	public static bool CanBeTerroristBaseIfLiberatedNeighbour(WorldTerritory3D[] territoryList)
	{
		if (territoryList == null || territoryList.Length == 0)
		{
			return true;
		}
		foreach (WorldTerritory3D worldTerritory3D in territoryList)
		{
			TerritoryProgress territoryProgress = WorldMapProgressController.currentProgress.GetTerritoryProgress(worldTerritory3D.name);
			if (territoryProgress != null && territoryProgress.state == TerritoryState.Liberated)
			{
				return true;
			}
		}
		return false;
	}

	protected void ReenforceRandomTerritory(WorldTerritory3D originTerritory, ref List<WorldTerritory3D> possibleReinforceTerritories)
	{
		int index = UnityEngine.Random.Range(0, possibleReinforceTerritories.Count);
		UnityEngine.Debug.Log("ReenforceTerritory : " + possibleReinforceTerritories[index].name + " originTerritory : " + ((!(originTerritory != null)) ? "Null" : originTerritory.name));
		WorldEventController.AddCurrentEvent(new WorldMapEvent(WorldMapEventType.ReenforceTerritory, possibleReinforceTerritories[index], originTerritory, 0.5f));
		TerritoryProgress territoryProgress = WorldMapProgressController.currentProgress.GetTerritoryProgress(possibleReinforceTerritories[index].name);
		territoryProgress.terrorLevel++;
		if (territoryProgress.state == TerritoryState.Empty)
		{
			territoryProgress.state = TerritoryState.TerroristBase; possibleReinforceTerritories[index].properties.state = (territoryProgress.state );
			possibleReinforceTerritories[index].properties.terroristLevel++;
		}
		possibleReinforceTerritories.RemoveAt(index);
	}

	protected void CheckProgressDiscrepancies()
	{
		if (WorldMapProgressController.currentProgress == null)
		{
			UnityEngine.Debug.LogError("Current progress null!");
		}
		if (WorldMapProgressController.previousProgress == null)
		{
			UnityEngine.Debug.LogError("Previous progress null!");
		}
		if (WorldMapProgressController.currentProgress.worldLiberty != WorldMapProgressController.previousProgress.worldLiberty)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Found World Liberty Change! current : ",
				WorldMapProgressController.currentProgress.worldLiberty,
				" previuos : ",
				WorldMapProgressController.previousProgress.worldLiberty
			}));
		}
		foreach (TerritoryProgress territoryProgress in WorldMapProgressController.previousProgress.territoryProgress)
		{
			TerritoryProgress territoryProgress2 = WorldMapProgressController.currentProgress.GetTerritoryProgress(territoryProgress.name);
			if (territoryProgress2 != null && territoryProgress2.state != territoryProgress.state && territoryProgress2.state == TerritoryState.Liberated)
			{
				UnityEngine.Debug.Log("ADD EVENT LIBERATE ");
				WorldEventController.AddCurrentEvent(new WorldMapEvent(WorldMapEventType.Liberate, this.GetTerritory(territoryProgress2.name), null, 1f));
				this.GetTerritory(territoryProgress2.name).DropFlag();
			}
		}
	}

	protected void RestoreProgressEquilibrium()
	{
		UnityEngine.Debug.Log("MAKE PREVIOUS PROGRESS AND CURRENT PROGRESS EQUAL ");
		WorldMapProgressController.previousProgress = WorldMapProgressController.currentProgress.Copy();
	}

	protected void FindAllTerritoryObjects()
	{
		this.territoryObjects = (UnityEngine.Object.FindObjectsOfType(typeof(WorldTerritory3D)) as WorldTerritory3D[]);
	}

	public static void SetCurrentTerritoryName(string territoryName)
	{
		if (WorldMapProgressController.currentProgress == null)
		{
			UnityEngine.Debug.LogError("No current progress!!");
			return;
		}
		WorldMapProgressController.currentProgress.currentTerritoryName = territoryName;
	}

	public static void SavePreviousProgress()
	{
		WorldMapProgressController.previousProgress = WorldMapProgressController.currentProgress.Copy();
	}

	public static void SaveProgress()
	{
		if (WorldMapProgressController.currentProgress == null)
		{
			UnityEngine.Debug.LogError("No progress to save!!");
			return;
		}
		foreach (TerritoryProgress territoryProgress in WorldMapProgressController.currentProgress.territoryProgress)
		{
			PlayerPrefs.SetInt("CurrentProgress_" + territoryProgress.name + "_Exists", 1);
			PlayerPrefs.SetString("CurrentProgress_" + territoryProgress.name + "_State", territoryProgress.state.ToString());
			PlayerPrefs.SetInt("CurrentProgress_" + territoryProgress.name + "_Terror", territoryProgress.terrorLevel);
			PlayerPrefs.SetInt("CurrentProgress_" + territoryProgress.name + "_Infestation", territoryProgress.infestationLevel);
		}
		foreach (TerritoryProgress territoryProgress2 in WorldMapProgressController.previousProgress.territoryProgress)
		{
			PlayerPrefs.SetInt("PreviousProgress_" + territoryProgress2.name + "_Exists", 1);
			PlayerPrefs.SetString("PreviousProgress_" + territoryProgress2.name + "_State", territoryProgress2.state.ToString());
			PlayerPrefs.SetInt("PreviousProgress_" + territoryProgress2.name + "_Terror", territoryProgress2.terrorLevel);
			PlayerPrefs.SetInt("PreviousProgress_" + territoryProgress2.name + "_Infestation", territoryProgress2.infestationLevel);
		}
		PlayerPrefs.SetInt("CurrentProgress_WorldLiberty", WorldMapProgressController.currentProgress.worldLiberty);
		PlayerPrefs.SetInt("CurrentProgress_WorldTerror", WorldMapProgressController.currentProgress.worldTerror);
		PlayerPrefs.SetInt("CurrentProgress_TurnCount", WorldMapProgressController.currentProgress.turnCount);
		PlayerPrefs.SetInt("PreviousProgress_TurnCount", WorldMapProgressController.previousProgress.turnCount);
		if (!string.IsNullOrEmpty(WorldMapProgressController.currentProgress.currentTerritoryName))
		{
			PlayerPrefs.SetString("CurrentProgress_TerritoryName", WorldMapProgressController.currentProgress.currentTerritoryName);
		}
		else
		{
			PlayerPrefs.SetString("CurrentProgress_TerritoryName", string.Empty);
		}
		if (!string.IsNullOrEmpty(WorldMapProgressController.currentProgress.currentTerritoryName))
		{
			PlayerPrefs.SetString("CurrentProgress_LastSafeTerritory", WorldMapProgressController.currentProgress.lastSafeTerritory);
		}
		else
		{
			PlayerPrefs.SetString("CurrentProgress_LastSafeTerritory", string.Empty);
		}
	}

	public static void SaveTerritoryProgress(WorldTerritory3D territory)
	{
		PlayerPrefs.SetInt("CurrentProgress_" + territory.name + "_Exists", 1);
		PlayerPrefs.SetString("CurrentProgress_" + territory.name + "_State", territory.properties.state.ToString());
		PlayerPrefs.SetInt("CurrentProgress_" + territory.name + "_Terror", territory.properties.terroristLevel);
		PlayerPrefs.SetInt("CurrentProgress_" + territory.name + "_Infestation", 0);
	}

	public void StartFromScratch()
	{
		WorldMapProgressController.startNewWorldMapGame = false;
		WorldMapProgressController.hasAlreadyStartedFromScratch = true;
		WorldMapProgressController.currentProgress = new WorldMapProgress();
		foreach (WorldTerritory3D worldTerritory3D in this.territoryObjects)
		{
			WorldMapProgressController.SaveTerritoryProgress(worldTerritory3D);
			WorldMapProgressController.currentProgress.AddTerritory(worldTerritory3D.name, (!(worldTerritory3D.name.ToLower() == this.defaultLastSafeTerritory.ToLower())) ? worldTerritory3D.properties.state : TerritoryState.ForwardBase, worldTerritory3D.properties.terroristLevel, worldTerritory3D.properties.infestationLevel, worldTerritory3D.properties.populationLevel, worldTerritory3D.properties.failCount);
		}
		WorldMapProgressController.currentProgress.turnCount = 0;
		WorldMapProgressController.currentProgress.worldLiberty = 0;
		WorldMapProgressController.currentProgress.worldTerror = 0;
		WorldMapProgressController.currentProgress.lastSafeTerritory = this.defaultLastSafeTerritory;
		WorldMapProgressController.currentProgress.currentTerritoryName = this.defaultLastSafeTerritory;
		WorldMapProgressController.previousProgress = WorldMapProgressController.currentProgress.Copy();
		WorldMapProgressController.SaveProgress();
	}

	public static void SetTerritoryState(string territoryNae, TerritoryState state)
	{
		foreach (TerritoryProgress territoryProgress in WorldMapProgressController.currentProgress.territoryProgress)
		{
			if (territoryProgress.name.ToLower() == territoryNae.ToLower())
			{
				territoryProgress.state = state;
			}
		}
	}

	public static void SetCurrentTerritory(WorldTerritory3D territory)
	{
		WorldMapProgressController.currentProgress.lastSafeTerritory = territory.name;
		WorldMapProgressController.currentProgress.currentTerritoryName = territory.name;
		WorldMapProgressController.SaveProgress();
	}

	public void LoadProgress()
	{
		UnityEngine.Debug.LogError("LOAD PROGRESS INCOMPLETE ??");
		if (WorldMapProgressController.currentProgress == null)
		{
			WorldMapProgressController.currentProgress = new WorldMapProgress();
			UnityEngine.Debug.Log("NEW PROGRESS");
		}
		if (WorldMapProgressController.previousProgress == null)
		{
			WorldMapProgressController.previousProgress = new WorldMapProgress();
		}
		UnityEngine.Debug.Log("Territory Objects Length " + this.territoryObjects.Length);
		foreach (WorldTerritory3D worldTerritory3D in this.territoryObjects)
		{
			if (PlayerPrefs.GetInt("CurrentProgress_" + worldTerritory3D.name + "_Exists", -1) <= 0)
			{
				UnityEngine.Debug.LogWarning(worldTerritory3D.name + "Does not exist in saves! Create new territory save! ");
				WorldMapProgressController.SaveTerritoryProgress(worldTerritory3D);
				WorldMapProgressController.currentProgress.AddTerritory(worldTerritory3D.name, worldTerritory3D.properties.state, worldTerritory3D.properties.terroristLevel, worldTerritory3D.properties.infestationLevel, worldTerritory3D.properties.populationLevel, worldTerritory3D.properties.failCount);
			}
			else
			{
				TerritoryProgress territoryProgress = WorldMapProgressController.currentProgress.GetTerritoryProgress(worldTerritory3D.name);
				TerritoryProgress territoryProgress2 = WorldMapProgressController.previousProgress.GetTerritoryProgress(worldTerritory3D.name);
				if (territoryProgress == null)
				{
					territoryProgress = WorldMapProgressController.currentProgress.AddTerritory(worldTerritory3D.name, worldTerritory3D.properties.state, worldTerritory3D.properties.terroristLevel, worldTerritory3D.properties.infestationLevel, worldTerritory3D.properties.populationLevel, worldTerritory3D.properties.failCount);
				}
				if (territoryProgress2 == null)
				{
					territoryProgress2 = WorldMapProgressController.previousProgress.AddTerritory(worldTerritory3D.name, worldTerritory3D.properties.state, worldTerritory3D.properties.terroristLevel, worldTerritory3D.properties.infestationLevel, worldTerritory3D.properties.populationLevel, worldTerritory3D.properties.failCount);
				}
				territoryProgress.state = (TerritoryState)((int)Enum.Parse(typeof(TerritoryState), PlayerPrefs.GetString("CurrentProgress_" + territoryProgress.name + "_State")));
				territoryProgress.terrorLevel = PlayerPrefs.GetInt("CurrentProgress_" + territoryProgress.name + "_Terror", 1);
				territoryProgress.infestationLevel = PlayerPrefs.GetInt("CurrentProgress_" + territoryProgress.name + "_Infestation", 1);
				territoryProgress2.state = (TerritoryState)((int)Enum.Parse(typeof(TerritoryState), PlayerPrefs.GetString("PreviousProgress_" + territoryProgress.name + "_State")));
				territoryProgress2.terrorLevel = PlayerPrefs.GetInt("PreviousProgress_" + territoryProgress.name + "_Terror", 1);
				territoryProgress2.infestationLevel = PlayerPrefs.GetInt("PreviousProgress_" + territoryProgress.name + "_Infestation", 1);
				WorldMapProgressController.LoadProgress(worldTerritory3D, territoryProgress, territoryProgress2);
			}
		}
		WorldMapProgressController.currentProgress.worldLiberty = PlayerPrefs.GetInt("CurrentProgress_WorldLiberty", 0);
		WorldMapProgressController.currentProgress.worldTerror = PlayerPrefs.GetInt("CurrentProgress_WorldTerror", 0);
		WorldMapProgressController.currentProgress.currentTerritoryName = PlayerPrefs.GetString("CurrentProgress_TerritoryName");
		WorldMapProgressController.currentProgress.lastSafeTerritory = PlayerPrefs.GetString("CurrentProgress_LastSafeTerritory");
		WorldMapProgressController.currentProgress.turnCount = PlayerPrefs.GetInt("CurrentProgress_TurnCount", 0);
		if (WorldMapProgressController.previousProgress == null)
		{
			WorldMapProgressController.previousProgress = WorldMapProgressController.currentProgress.Copy();
			UnityEngine.Debug.Log("First Time Start ! ... liberty = " + WorldMapProgressController.previousProgress.worldLiberty);
		}
	}

	protected static void LoadProgress(WorldTerritory3D territory, TerritoryProgress progress, TerritoryProgress previousProgress)
	{
		territory.properties.state = progress.state;
		territory.properties.terroristLevel = progress.terrorLevel;
		territory.properties.infestationLevel = progress.infestationLevel;
		territory.properties.populationLevel = progress.populationLevel;
		territory.properties.failCount = progress.failCount;
		if (previousProgress != null)
		{
			territory.previousProperties.state = previousProgress.state;
			territory.previousProperties.terroristLevel = previousProgress.terrorLevel;
			territory.previousProperties.infestationLevel = previousProgress.infestationLevel;
			territory.previousProperties.populationLevel = previousProgress.populationLevel;
			territory.previousProperties.failCount = previousProgress.failCount;
		}
		else
		{
			territory.previousProperties.state = TerritoryState.Empty;
		}
	}

	protected static bool CurrentProgressContains(string territoryName)
	{
		foreach (TerritoryProgress territoryProgress in WorldMapProgressController.currentProgress.territoryProgress)
		{
			if (territoryProgress.name.ToLower() == territoryName.ToLower())
			{
				return true;
			}
		}
		return false;
	}

	public static WorldTerritory3D GetStartingTerritory()
	{
		if (WorldMapProgressController.currentProgress == null || WorldMapProgressController.instance == null || WorldMapProgressController.instance.territoryObjects == null)
		{
			UnityEngine.Debug.LogError("No progress to start from!!");
			return null;
		}
		if (WorldMapProgressController.currentProgress.lastSafeTerritory == null || WorldMapProgressController.currentProgress.lastSafeTerritory.Length <= 0)
		{
			return null;
		}
		return WorldMapProgressController.instance.GetTerritory(WorldMapProgressController.currentProgress.lastSafeTerritory);
	}

	protected WorldTerritory3D GetTerritory(string territoryName)
	{
		foreach (WorldTerritory3D worldTerritory3D in this.territoryObjects)
		{
			if (worldTerritory3D.name == territoryName)
			{
				return worldTerritory3D;
			}
		}
		return null;
	}

	public static void FailedCampaign()
	{
		UnityEngine.Debug.Log("Failed Campaign");
		if (WorldMapProgressController.currentProgress == null)
		{
			UnityEngine.Debug.LogError("No progress to fail from!!");
			return;
		}
		if (WorldMapProgressController.currentProgress.currentTerritoryName == null || WorldMapProgressController.currentProgress.currentTerritoryName.Length == 0)
		{
			UnityEngine.Debug.LogError("No current territory name to save to");
		}
		TerritoryProgress currentTerritoryProgress = WorldMapProgressController.GetCurrentTerritoryProgress(WorldMapProgressController.currentProgress.currentTerritoryName);
		if (currentTerritoryProgress != null)
		{
			currentTerritoryProgress.failCount++;
		}
		else
		{
			UnityEngine.Debug.LogError("No territory in current progress called : " + WorldMapProgressController.currentProgress.currentTerritoryName);
		}
		WorldMapProgressController.SaveProgress();
	}

	public static void FinishCampaign()
	{
		UnityEngine.Debug.Log("Finish Campaign");
		if (WorldMapProgressController.currentProgress == null)
		{
			UnityEngine.Debug.LogError("No progress to be victorious of!!");
			return;
		}
		if (WorldMapProgressController.currentProgress.currentTerritoryName == null || WorldMapProgressController.currentProgress.currentTerritoryName.Length == 0)
		{
			UnityEngine.Debug.LogError("No current territory name to save to");
		}
		if (WorldMapProgressController.currentProgress == null)
		{
			UnityEngine.Debug.LogError("No current progress to save to");
			return;
		}
		TerritoryProgress currentTerritoryProgress = WorldMapProgressController.GetCurrentTerritoryProgress(WorldMapProgressController.currentProgress.currentTerritoryName);
		if (currentTerritoryProgress != null)
		{
			UnityEngine.Debug.Log("Finish Campaign name = " + WorldMapProgressController.currentProgress.currentTerritoryName);
			WorldMapProgressController.currentProgress.lastSafeTerritory = WorldMapProgressController.currentProgress.currentTerritoryName;
			if (currentTerritoryProgress.terrorLevel < 6 && currentTerritoryProgress.infestationLevel < 3)
			{
				WorldMapProgressController.currentProgress.worldLiberty += 500 + currentTerritoryProgress.populationLevel * 500;
				currentTerritoryProgress.terrorLevel = 0;
				currentTerritoryProgress.infestationLevel = 0;
				currentTerritoryProgress.state = TerritoryState.Liberated;
			}
			else
			{
				currentTerritoryProgress.terrorLevel = 0;
				currentTerritoryProgress.infestationLevel = 0;
				currentTerritoryProgress.state = TerritoryState.Obliterated;
			}
		}
		else
		{
			UnityEngine.Debug.LogError("No territory in current progress called : " + WorldMapProgressController.currentProgress.currentTerritoryName);
		}
		WorldMapProgressController.currentProgress.turnCount++;
		WorldMapProgressController.SaveProgress();
	}

	public static int GetTurn()
	{
		return PlayerPrefs.GetInt("CurrentProgress_TurnCount", 0);
	}

	public static TerritoryProgress GetCurrentTerritoryProgress(string territoryName)
	{
		if (WorldMapProgressController.currentProgress == null)
		{
			UnityEngine.Debug.LogError("currentProgress is null!");
			return null;
		}
		foreach (TerritoryProgress territoryProgress in WorldMapProgressController.currentProgress.territoryProgress)
		{
			if (territoryProgress.name.ToLower() == territoryName.ToLower())
			{
				return territoryProgress;
			}
		}
		return null;
	}

	public static TerritoryProgress GetInitialTerritoryProgress(string territoryName)
	{
		if (WorldMapProgressController.previousProgress == null)
		{
			UnityEngine.Debug.LogError("Get Progress called before progress has been set up !");
			return null;
		}
		foreach (TerritoryProgress territoryProgress in WorldMapProgressController.previousProgress.territoryProgress)
		{
			if (territoryProgress.name.ToLower() == territoryName.ToLower())
			{
				return territoryProgress;
			}
		}
		return null;
	}

	public static WorldMapProgressController instance;

	public bool forceTestProgress;

	public WorldMapProgress testProgress;

	public static WorldMapProgress currentProgress;

	public static WorldMapProgress previousProgress;

	protected WorldTerritory3D[] territoryObjects;

	public string defaultLastSafeTerritory = "TerritoryUSA";

	public bool reinforceTerrorEveryTurn = true;

	protected static int turnCount;

	public bool startFromScratch = true;

	public static bool startNewWorldMapGame;

	protected static bool hasAlreadyStartedFromScratch;
}
