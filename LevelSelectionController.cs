// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public static class LevelSelectionController
{
	public static string ExhibitionCampaign
	{
		get
		{
			if (Application.loadedLevelName.ToUpper().Equals("Victory"))
			{
				return "Victory";
			}
			switch (LevelSelectionController.exhibitionCount % 4)
			{
			case 0:
				return "VIETNAM_EXHIBITION_A";
			case 1:
				return "VIETNAM_EXHIBITION_B";
			case 2:
				return "VIETNAM_EXHIBITION_C";
			default:
				return "VIETNAM_EXHIBITION_D";
			}
		}
	}

	public static string MainMenuScene
	{
		get
		{
			if (PlaytomicController.isExpendabrosBuild)
			{
				return "MainMenuExpendabros";
			}
			if (PlaytomicController.isExhibitionBuild)
			{
				return "MainMenuExhibition";
			}
			return "MainMenu";
		}
	}

	public static void RestartCampaignScene()
	{
		Fader.FadeSolid(2f, false);
	}

	public static void GotoNextCampaignScene(bool instant)
	{
		UnityEngine.Debug.Log("GotoNextCampaignScene");
		if (LevelSelectionController.isOnlineCampaign)
		{
			if (instant)
			{
				Application.LoadLevel(LevelSelectionController.CampaignScene);
			}
			else
			{
				Fader.nextScene = LevelSelectionController.CampaignScene;
				Fader.FadeSolid();
				UnityEngine.Debug.Log("Fader Next Scene   " + Fader.nextScene);
			}
		}
		else if (instant)
		{
			if (LevelSelectionController.currentLevelNum <= 0 && !LevelSelectionController.shownHelicopterIntro && !LevelSelectionController.IsCustomCampaign())
			{
				LevelSelectionController.shownHelicopterIntro = true;
				UnityEngine.Debug.Log("MissionScreenVietnam");
				Application.LoadLevel("MissionScreenVietnam");
				MissionScreenController.SetVariables(string.Empty, WeatherType.Day, RainType.NoChange);
				Fader.nextScene = LevelSelectionController.CampaignScene;
				UnityEngine.Debug.Log("Fader Next Scene   " + Fader.nextScene);
			}
			else
			{
				Application.LoadLevel(LevelSelectionController.CampaignScene);
			}
		}
		else
		{
			Fader.nextScene = LevelSelectionController.CampaignScene;
			Fader.FadeSolid();
			UnityEngine.Debug.Log("Fader Next Scene   " + Fader.nextScene);
		}
	}

	public static void ResetLevelAndGameModeToDefault()
	{
		UnityEngine.Debug.Log("Reset level and game mode to default");
		LevelSelectionController.CurrentLevelNum = 0;
		GameModeController.campaignLevelFailCount = 0;
		LevelEditorGUI.publishRunSuccessful = false;
		GameModeController.publishRun = false;
		LevelSelectionController.loadMode = MapLoadMode.NotSet;
		LevelSelectionController.campaignToLoad = null;
		LevelSelectionController.loadCustomCampaign = false;
		LevelSelectionController.isOnlineCampaign = false;
		LevelEditorGUI.levelEditorActive = false;
		GameModeController.ResetPlayerRoundWins();
		StatisticsController.ResetScore();
		GameModeController.GameMode = GameMode.NotSet;
		if (PlaytomicController.isExpendabrosBuild)
		{
			LevelSelectionController.DefaultCampaign = LevelSelectionController.ExpendabrosCampaign;
		}
		else if (!PlaytomicController.isExhibitionBuild)
		{
			LevelSelectionController.DefaultCampaign = LevelSelectionController.OfflineCampaign;
		}
		else
		{
			LevelSelectionController.DefaultCampaign = LevelSelectionController.ExhibitionCampaign;
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"LevelSelectionController Exhibition Campaign ",
				LevelSelectionController.DefaultCampaign,
				" Exhibition Count ",
				LevelSelectionController.exhibitionCount
			}));
		}
		LevelSelectionController.returnToWorldMap = false;
		LevelSelectionController.currentCampaign = null;
	}

	public static bool IsCustomCampaign()
	{
		return LevelSelectionController.loadCustomCampaign || LevelSelectionController.isOnlineCampaign;
	}

	public static string CurrentGameModeScene
	{
		get
		{
			switch (GameModeController.GameMode)
			{
			case GameMode.ExplosionRun:
				return LevelSelectionController.ExplosionRunSceneName;
			case GameMode.DeathMatch:
			case GameMode.TeamDeathMatch:
				return LevelSelectionController.DeathmatchSceneName;
			case GameMode.SuicideHorde:
				return LevelSelectionController.SuicideHordeSceneName;
			case GameMode.Race:
				return LevelSelectionController.RaceRunSceneName;
			}
			return LevelSelectionController.CampaignScene;
		}
	}

	public static bool IsCampaignScene
	{
		get
		{
			return Application.loadedLevelName == LevelSelectionController.CampaignScene;
		}
	}

	public static int GetCurrentCampaignLength()
	{
		GameMode gameMode = GameModeController.GameMode;
		if (gameMode != GameMode.Campaign)
		{
			if (gameMode != GameMode.Cutscene)
			{
				return LevelSelectionController.currentCampaign.Length;
			}
			return 1;
		}
		else
		{
			if (LevelSelectionController.currentCampaign != null)
			{
				return LevelSelectionController.currentCampaign.Length;
			}
			return 0;
		}
	}

	public static string GetLevelFileName(int no, GameMode gameMode)
	{
		switch (gameMode)
		{
		case GameMode.Campaign:
			return LevelSelectionController.currentCampaign.levels[no].levelDescription;
		default:
			switch (gameMode)
			{
			case GameMode.Cutscene:
				return "Victory";
			case GameMode.TeamDeathMatch:
				goto IL_3F;
			}
			UnityEngine.Debug.LogError("Could not find level for gametype " + gameMode);
			return "Level1a";
		case GameMode.DeathMatch:
			break;
		}
		IL_3F:
		return LevelSelectionController.deathmatchLevels[no % LevelSelectionController.deathmatchLevels.Length];
	}

	public static string GetLevelFileName(int no)
	{
		return LevelSelectionController.GetLevelFileName(no, GameModeController.GameMode);
	}

	public static MapData GetCurrentMap()
	{
		if (string.IsNullOrEmpty(LevelSelectionController.campaignToLoad))
		{
			UnityEngine.Debug.Log("Empty Campaign To Load String");
		}
		MapData mapData = null;
		if (LevelSelectionController.loadMode == MapLoadMode.NotSet)
		{
			LevelSelectionController.loadMode = MapLoadMode.Campaign;
			if (!Map.Instance.forceTestLevel)
			{
				LevelSelectionController.campaignToLoad = LevelSelectionController.DefaultCampaign;
				PlayerOptions.Instance.LastCustomLevel = LevelSelectionController.campaignToLoad;
			}
			else if (!string.IsNullOrEmpty(Map.Instance.testLevelFileName))
			{
				LevelSelectionController.campaignToLoad = Map.Instance.testLevelFileName;
				LevelSelectionController.loadMode = MapLoadMode.LoadFromFile;
			}
			else if (!string.IsNullOrEmpty(PlayerOptions.Instance.LastCustomLevel))
			{
				LevelSelectionController.campaignToLoad = PlayerOptions.Instance.LastCustomLevel;
				LevelSelectionController.loadMode = MapLoadMode.LoadFromFile;
			}
			else
			{
				LevelSelectionController.campaignToLoad = LevelSelectionController.DefaultCampaign;
				PlayerOptions.Instance.LastCustomLevel = LevelSelectionController.campaignToLoad;
			}
		}
		if (LevelSelectionController.loadMode == MapLoadMode.LoadFromMapdata)
		{
			UnityEngine.Debug.Log("GetCurrentMap loadMode is load from map data");
			mapData = LevelSelectionController.MapDataToLoad;
			if (mapData == null)
			{
				UnityEngine.Debug.LogError("Map Load Mode set to MapData but Map's MapdataToLoad is null!");
			}
			else
			{
				UnityEngine.Debug.Log("MapDataToLoad Not Null");
			}
		}
		else if (LevelSelectionController.loadMode == MapLoadMode.LoadFromFile)
		{
			UnityEngine.Debug.Log("LOAD FROM FILE");
			if (string.IsNullOrEmpty(LevelSelectionController.levelFileNameToLoad))
			{
				LevelSelectionController.levelFileNameToLoad = Map.Instance.testLevelFileName;
				if (string.IsNullOrEmpty(LevelSelectionController.levelFileNameToLoad))
				{
					LevelSelectionController.levelFileNameToLoad = PlayerOptions.Instance.LastCustomLevel;
				}
				else
				{
					PlayerOptions.Instance.LastCustomLevel = LevelSelectionController.campaignToLoad;
				}
			}
			try
			{
				LevelEditorGUI.campaign = (LevelSelectionController.currentCampaign = FileIO.LoadCampaignFromDisk(Map.Instance.testLevelFileName));
				mapData = LevelSelectionController.currentCampaign.levels[Map.nextLevelToLoad % LevelSelectionController.currentCampaign.Length];
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
				LevelSelectionController.MapDataToLoad = FileIO.LoadLevelFromDisk(Map.Instance.testLevelFileName);
				mapData = LevelSelectionController.MapDataToLoad;
			}
		}
		else if (LevelSelectionController.loadMode == MapLoadMode.Campaign)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Load Campaign ",
				string.IsNullOrEmpty(LevelSelectionController.campaignToLoad),
				" name ",
				LevelSelectionController.campaignToLoad,
				"  CurrentLevelNum ",
				LevelSelectionController.CurrentLevelNum
			}));
			if (!string.IsNullOrEmpty(LevelSelectionController.campaignToLoad))
			{
				if (LevelSelectionController.loadCustomCampaign)
				{
					UnityEngine.Debug.Log("Load Custom Campaign " + LevelSelectionController.campaignToLoad);
					if (LevelSelectionController.loadPublishedCampaign)
					{
						LevelSelectionController.currentCampaign = FileIO.LoadPublishedCampaignFromDisk(LevelSelectionController.campaignToLoad);
					}
					else
					{
						LevelSelectionController.currentCampaign = FileIO.LoadCampaignFromDisk(LevelSelectionController.campaignToLoad);
					}
				}
				else if (LevelSelectionController.currentCampaign == null || !LevelSelectionController.currentCampaign.name.ToUpper().Equals(LevelSelectionController.campaignToLoad.ToUpper()))
				{
					LevelSelectionController.currentCampaign = FileIO.LoadCampaignFromResources(LevelSelectionController.campaignToLoad);
				}
				LevelSelectionController.campaignToLoad = null;
			}
			if (GameModeController.GameMode == GameMode.BroDown)
			{
				UnityEngine.Debug.Log("PREPARE FOR BRODOWN!!");
				int brodownBroCount = GameModeController.GetBrodownBroCount();
				int num = brodownBroCount - 2;
				if (num >= 0 && num < LevelSelectionController.GetCurrentCampaignLength())
				{
					mapData = LevelSelectionController.currentCampaign.levels[num];
					UnityEngine.Debug.Log("Set Map Date " + LevelSelectionController.currentCampaign.name + " mapdata " + mapData.levelDescription);
				}
				else
				{
					UnityEngine.Debug.LogError("Bad Brodown player count?");
					num %= LevelSelectionController.GetCurrentCampaignLength();
					mapData = LevelSelectionController.currentCampaign.levels[num];
				}
			}
			else if (GameModeController.GameMode != GameMode.Campaign && GameModeController.GameMode != GameMode.Cutscene)
			{
				UnityEngine.Debug.Log("Other load!");
				int num2 = LevelSelectionController.CurrentLevelNum;
				if (num2 >= LevelSelectionController.GetCurrentCampaignLength())
				{
					num2 %= LevelSelectionController.GetCurrentCampaignLength();
				}
				mapData = LevelSelectionController.currentCampaign.levels[num2];
			}
			else
			{
				UnityEngine.Debug.Log("Other load 2! " + LevelSelectionController.CurrentLevelNum);
				UnityEngine.Debug.Log("Other load 2! " + LevelSelectionController.campaignToLoad);
				UnityEngine.Debug.Log("GameModeController.GameMode! " + GameModeController.GameMode);
				UnityEngine.Debug.Log("GetCurrentCampaignLength! " + LevelSelectionController.GetCurrentCampaignLength());
				if (LevelSelectionController.CurrentLevelNum >= LevelSelectionController.GetCurrentCampaignLength())
				{
					UnityEngine.Debug.Log("Loading Victory");
					mapData = FileIO.LoadLevelFromResources("Victory");
				}
				else
				{
					mapData = LevelSelectionController.currentCampaign.levels[LevelSelectionController.CurrentLevelNum];
				}
			}
		}
		return mapData;
	}

	public static int CurrentLevelNum
	{
		get
		{
			if (LevelSelectionController.currentLevelNum == -1 && SingletonMono<MapController>.Instance != null)
			{
				UnityEngine.Debug.Log("Set Start Level " + MapController.StartLevel);
				LevelSelectionController.currentLevelNum = MapController.StartLevel;
			}
			return LevelSelectionController.currentLevelNum;
		}
		set
		{
			LevelSelectionController.currentLevelNum = value;
		}
	}

	public static void GotoNextLevel()
	{
		LevelSelectionController.currentLevelNum++;
		UnityEngine.Debug.Log("currentLevelNum " + LevelSelectionController.currentLevelNum);
		LevelSelectionController.Save();
		if (LevelSelectionController.currentLevelNum < LevelSelectionController.currentCampaign.Length)
		{
			if (!Fader.FadeSolid())
			{
				UnityEngine.Debug.Log("Fader");
				Application.LoadLevel(Application.loadedLevel);
			}
			else
			{
				Fader.nextScene = Application.loadedLevelName;
				UnityEngine.Debug.Log("Fader Next Scene   " + Fader.nextScene);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Not Implemented Cutscene");
			LevelSelectionController.currentLevelNum = 0;
			LevelSelectionController.Save();
		}
	}

	private static void Save()
	{
		if (LevelSelectionController.currentLevelNum >= 0)
		{
			PlayerPrefs.SetInt("Level_Save_Current_LevelNum", LevelSelectionController.currentLevelNum);
		}
	}

	public static void Load()
	{
		if (!LevelSelectionController.hasLoaded)
		{
			LevelSelectionController.currentLevelNum = PlayerPrefs.GetInt("Level_Save_MostRecentCompletedLevelNum", -1) + 1;
			UnityEngine.Debug.Log(" currentLevelNum " + LevelSelectionController.currentLevelNum);
			if (LevelSelectionController.currentLevelNum >= 0)
			{
			}
			if (LevelSelectionController.currentLevelNum < 0)
			{
				LevelSelectionController.currentLevelNum = 0;
			}
			LevelSelectionController.hasLoaded = true;
		}
		else
		{
			UnityEngine.Debug.LogError("Already Loaded Current Level");
		}
	}

	public static bool LoadLevelStatus(string campaignName, int levelNum)
	{
		return PlayerPrefs.GetInt(string.Concat(new object[]
		{
			"Level_Save_",
			campaignName,
			"_",
			levelNum
		}), -1) > 0;
	}

	public static void CompleteCurrentLevel()
	{
		LevelSelectionController.currentLevelNum++;
		LevelSelectionController.shownHelicopterIntro = false;
		if (LevelSelectionController.isOnlineCampaign)
		{
			PlayerProgress.Instance.lastOnlineLevelProgress = LevelSelectionController.currentLevelNum;
			PlayerProgress.Save();
		}
	}

	private const string _MainMenuScene = "MainMenu";

	public const string MainMenuSceneExhibition = "MainMenuExhibition";

	public const string HeroSelectScreen = "HeroSelect";

	public const string MainMenuSceneExpendabros = "MainMenuExpendabros";

	public static bool hasLoaded = false;

	private static int currentLevelNum = -1;

	public static MapLoadMode loadMode = MapLoadMode.NotSet;

	public static string levelFileNameToLoad;

	public static string campaignToLoad;

	public static bool loadCustomCampaign;

	public static MapData MapDataToLoad;

	public static string JoinScene = "Expendabros Join";

	public static string DeathmatchSceneName = "Test Deathmatch";

	public static string ExplosionRunSceneName = "Test Evan";

	public static string RaceRunSceneName = "Race Scene";

	public static string BrodownSceneName = "Test Deathmatch";

	public static string SuicideHordeSceneName = "Test Evan";

	public static string CampaignScene = "Test Evan2";

	public static string CampaignSceneDefault = "Test Evan2";

	public static string CustomCampaignVictoryScene = "VictoryCustomCampaign";

	public static string VictoryScene = "ExpendabrosVictory";

	public static string OfflineCampaign = "Expendabros_Campaign";

	public static string DefaultCampaign = LevelSelectionController.OfflineCampaign;

	public static string OnlineCampaign = LevelSelectionController.OfflineCampaign;

	public static int exhibitionCount = -1;

	public static string ExpendabrosCampaign = "Expendabros_Campaign";

	public static bool returnToWorldMap = false;

	public static bool shownHelicopterIntro = false;

	public static string[] deathmatchLevels = new string[]
	{
		"Melee5",
		"MeleeCity4",
		"Melee4",
		"MeleeCity8",
		"MeleeCity2"
	};

	public static Campaign currentCampaign;

	public static bool loadPublishedCampaign;

	public static bool isOnlineCampaign;
}
