// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldMapController : MonoBehaviour
{
	private void Awake()
	{
		if (WorldMapProgressController.previousProgress != null)
		{
			UnityEngine.Debug.Log("USE previous  liberty " + WorldMapProgressController.previousProgress.worldLiberty);
		}
		WorldMapController.instance = this;
		WorldTerritory3D startingTerritory = WorldMapProgressController.GetStartingTerritory();
		WorldMapController.territories3D = (UnityEngine.Object.FindObjectsOfType(typeof(WorldTerritory3D)) as WorldTerritory3D[]);
	}

	private void Start()
	{
		WorldMapController.ClearMissionDetailsText();
		WorldMapCutsceneController.DeactivateCutscenes();
	}

	public static WorldTerritory3D GetTerritory(string territoryName)
	{
		for (int i = 0; i < WorldMapController.territories3D.Length; i++)
		{
			if (WorldMapController.territories3D[i].name.ToLower() == territoryName.ToLower())
			{
				return WorldMapController.territories3D[i];
			}
		}
		return null;
	}

	public static void CameraPanTo(Transform location, float zoomLevel, float time)
	{
		WorldMapController.instance.worldCamera.GotoLocation(location, zoomLevel, time);
	}

	public static void TransportGoTo(WorldLocation location)
	{
		WorldMapController.instance.transport.GoTo(location);
	}

	public static void TransportArriveAt(WorldTerritory3D territory)
	{
		if (territory.properties.state == TerritoryState.Brofort || territory.properties.state == TerritoryState.ForwardBase || (territory.properties.state == TerritoryState.Liberated && territory.HasHospital()))
		{
			WorldMapController.RestTransport(territory);
		}
		else if (territory.properties.state == TerritoryState.Liberated || territory.properties.state == TerritoryState.Empty || territory.properties.state == TerritoryState.Obliterated)
		{
			WorldTerritorySelector.SetCurrentTerritory(territory);
			WorldMapProgressController.SetCurrentTerritory(territory);
		}
		else
		{
			WorldMapProgressController.SetCurrentTerritoryName(territory.name);
			WorldMapProgressController.SaveProgress();
			WorldMapProgressController.SavePreviousProgress();
			WorldMapController.EnterMission(territory.properties.campaignName, territory.properties.campaignDescription, territory.properties);
		}
	}

	public static void EnterMission(string campaignName, string campaignDescription, TerritoryProperties territoryProperties)
	{
		LevelSelectionController.ResetLevelAndGameModeToDefault();
		LevelSelectionController.campaignToLoad = campaignName;
		LevelSelectionController.loadCustomCampaign = false;
		LevelSelectionController.loadMode = MapLoadMode.Campaign;
		GameModeController.GameMode = GameMode.Campaign;
		LevelEditorGUI.levelEditorActive = false;
		HeroUnlockController.Initialize();
		LevelSelectionController.returnToWorldMap = true;
		Fader.nextScene = LevelSelectionController.CampaignScene; Fader.nextNextScene = (Fader.nextScene );
		UnityEngine.Debug.Log("Fader Next Scene   " + Fader.nextScene);
		MissionScreenController.SetVariables(string.Empty, territoryProperties.weatherType, territoryProperties.rainType);
		if (WorldMapController.finishingMapsAutomatically)
		{
			UnityEngine.Debug.LogError("CHEATER !");
			WorldMapProgressController.FinishCampaign();
			Application.LoadLevel(Application.loadedLevel);
		}
		else
		{
			switch (territoryProperties.theme)
			{
			case LevelTheme.City:
				Application.LoadLevel("MissionScreenCity");
				return;
			case LevelTheme.Forest:
				Application.LoadLevel("MissionScreenForest");
				return;
			}
			Application.LoadLevel("MissionScreenVietnam");
		}
	}

	public static void LoadCampaign(string campaignName, LevelTheme themeType)
	{
		if (string.IsNullOrEmpty(campaignName))
		{
			UnityEngine.Debug.LogError("Empty Campaign Name");
			return;
		}
		LevelSelectionController.ResetLevelAndGameModeToDefault();
		LevelSelectionController.campaignToLoad = campaignName;
		LevelSelectionController.loadCustomCampaign = false;
		LevelSelectionController.loadMode = MapLoadMode.Campaign;
		LevelSelectionController.returnToWorldMap = true;
		GameModeController.GameMode = GameMode.Campaign;
		LevelEditorGUI.levelEditorActive = false;
		HeroUnlockController.Initialize();
		Fader.FadeSolid(1f, true);
		Application.LoadLevel("MissionScreenVietnam");
		MissionScreenController.SetVariables(string.Empty, WeatherType.Day, RainType.NoChange);
		Fader.nextScene = LevelSelectionController.CampaignScene; Fader.nextNextScene = (Fader.nextScene );
		UnityEngine.Debug.Log("Fader Next Scene   " + Fader.nextScene);
	}

	public static void TransportAriveAt(WorldLocation location)
	{
		WorldMapTerritoriesController.SaveTerritories();
		Application.LoadLevel("MissionAttack");
	}

	public static void SetMissionDetailsText(string text)
	{
		WorldMapController.instance.missionDetailsText.text = text;
	}

	public static void ClearMissionDetailsText()
	{
		WorldMapController.instance.missionDetailsText.text = string.Empty;
	}

	public static void RestTransport(WorldTerritory3D territory)
	{
		UnityEngine.Debug.Log("REST ************************************************************************");
		WorldTerritorySelector.SetCurrentTerritory(territory);
		WorldMapProgressController.SetCurrentTerritory(territory);
		WorldMapProgressController.SaveProgress();
	}

	public static int GetBroUnlockFrequency(int unlockedBroCount)
	{
		return 1 + unlockedBroCount;
	}

	public static bool IsNotBusy()
	{
		return WorldMapController.instance == null || WorldMapController.instance.gameObject.activeSelf;
	}

	public static void Activate()
	{
		WorldMapController.instance.gameObject.SetActive(true);
	}

	public static void Deactivate()
	{
		WorldMapController.instance.gameObject.SetActive(false);
	}

	public static HelicopterUpgradeType GetHelicopterUpgrade()
	{
		return HelicopterUpgradeType.Airwolf;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F11) && Input.GetKey(KeyCode.LeftControl))
		{
			UnityEngine.Debug.Log("Cheating enabled ");
			WorldMapController.finishingMapsAutomatically = true;
		}
	}

	public WorldTransport transport;

	public WorldCamera worldCamera;

	public GUIText terroristAlertText;

	public GUIText missionDetailsText;

	public WorldLocation startMissionBase;

	protected static MissionOutcomeType lastMissionOutcome;

	protected static WorldTerritory3D[] territories3D;

	protected static WorldMapController instance;

	public static bool finishingMapsAutomatically;
}
