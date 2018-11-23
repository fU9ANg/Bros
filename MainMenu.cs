// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu
{
	public override bool MenuActive
	{
		get
		{
			return base.MenuActive;
		}
		set
		{
			base.MenuActive = value;
		}
	}

	private void OnLevelWasLoaded()
	{
		UnityEngine.Debug.Log("> Return To Main Mnu");
	}

	protected override void Awake()
	{
		if (SingletonMono<Connect>.Instance != null)
		{
			MonoBehaviour.print("> ------------------Destroying existing Connect instance");
			UnityEngine.Object.DestroyImmediate(SingletonMono<Connect>.Instance.gameObject);
		}
		UnityEngine.Object.Instantiate(this.connectPrefab.gameObject);
		CutsceneController.holdPlayersStill = false;
		Connect.Disconnect();
		Connect.FullReset();
		Map.MapData = null;
		MainMenu.wasShown = true;
		base.Awake();
		Time.timeScale = 1f;
		Map.ClearSuperCheckpointStatus();
		if (Application.isWebPlayer && MainMenu.firstStart)
		{
			this.MenuActive = false;
			MainMenu.firstStart = false;
		}
		else if (this.clickToStartMesh != null)
		{
			this.clickToStartMesh.gameObject.SetActive(false);
		}
		MainMenu.instance = this;
		HeroController.ResetPlayersPlaying();
		Cursor.visible = false;
	}

	protected void Start()
	{
		Announcer.AnnounceBroforce();
	}

	protected override void RunInput()
	{
		if (this.transitioning || this.transitioningToExplanation)
		{
			this.down = false; this.up = (this.down );
		}
		base.RunInput();
	}

	private void GoToOptions()
	{
		this.MenuActive = false;
		this.optionsMenu.MenuActive = true;
		this.optionsMenu.TransitionIn();
	}

	private void CustomCampaign()
	{
		this.MenuActive = false;
		Application.LoadLevel("OnlineCustomCampaignBrowser");
	}

	private void ClearUnlocks()
	{
		HeroUnlockController.ClearUnlocks();
		foreach (TextMesh textMesh in this.items)
		{
			if (textMesh.text.Contains("RESET"))
			{
				textMesh.color = new Color(0.06f, 0.06f, 0.06f, 1f);
			}
		}
	}

	private void GoToLobby()
	{
		this.MenuActive = false;
		PlayerOptions.Instance.hardMode = false;
		this.lobbyGUI.GetComponent<LobbyMenu>().Open();
	}

	public static void GoBackToMenu()
	{
		MainMenu.instance.MenuActive = true;
		SingletonMono<LobbyMenu>.Instance.Close();
	}

	protected override void SetupItems()
	{
		List<MenuBarItem> list = new List<MenuBarItem>(this.itemNames);
		if (!PlaytomicController.isExpendabrosBuild)
		{
			list.Insert(2, new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "DEATHMATCH",
				invokeMethod = "StartDeathMatch",
				isBetaAccess = true,
				alphaBetaTextXOffset = 83f
			});
		}
		if (!PlaytomicController.isExpendabrosBuild)
		{
			list.Insert(2, new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "CUSTOM CAMPAIGN",
				invokeMethod = "CustomCampaign"
			});
		}
		if (!PlaytomicController.isExhibitionBuild && !PlaytomicController.isExpendabrosBuild)
		{
			MenuBarItem menuBarItem = new MenuBarItem();
			menuBarItem.color = list[0].color;
			menuBarItem.size = list[0].size;
			menuBarItem.name = "EXPLOSION RUN";
			menuBarItem.invokeMethod = "StartExplosionRun";
			menuBarItem.isAlphaAccess = true;
			menuBarItem.alphaBetaTextXOffset = 99f;
			if (!PlaytomicController.isExhibitionBuild)
			{
				list.Insert(2, menuBarItem);
			}
			list.Insert(2, new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "RACE MODE",
				invokeMethod = "StartRace",
				isAlphaAccess = true,
				alphaBetaTextXOffset = 99f
			});
			list.Insert(2, new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "LEVEL EDITOR",
				invokeMethod = "LevelEditor",
				isAlphaAccess = true,
				alphaBetaTextXOffset = 91f
			});
			if (PlaytomicController.isOnlineBuild)
			{
				list.Insert(2, new MenuBarItem
				{
					color = list[0].color,
					size = list[0].size,
					name = "Play Online",
					invokeMethod = "GoToLobby",
					isAlphaAccess = true,
					alphaBetaTextXOffset = 91f
				});
			}
			if (!PlaytomicController.isExpendabrosBuild)
			{
			}
			if (Application.isEditor)
			{
				list.Insert(2, new MenuBarItem
				{
					color = list[0].color,
					size = list[0].size,
					name = "SUICIDE HORDE",
					invokeMethod = "StartSuicideHorde",
					isAlphaAccess = true,
					alphaBetaTextXOffset = 100f
				});
			}
			list.Insert(0, new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = ((WorldMapProgressController.GetTurn() != 0) ? "RESTART CAMPAIGN" : "START CAMPAIGN"),
				invokeMethod = "StartWorldMap",
				isAlphaAccess = true,
				alphaBetaTextXOffset = (float)((WorldMapProgressController.GetTurn() != 0) ? 128 : 120)
			});
			if (WorldMapProgressController.GetTurn() > 0)
			{
				list.Insert(0, new MenuBarItem
				{
					color = list[0].color,
					size = list[0].size,
					name = "CONTINUE CAMPAIGN",
					invokeMethod = "ContinueWorldMap",
					isAlphaAccess = true,
					alphaBetaTextXOffset = 135f
				});
			}
		}
		else if (HeroUnlockController.IsUnlocked(HeroType.BroHard))
		{
			list.Insert(3, new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "RESET UNLOCKS",
				invokeMethod = "ClearUnlocks"
			});
		}
		list.RemoveAll((MenuBarItem i) => i.name.ToUpper().Contains("PREORDER"));
		if (Application.isWebPlayer || Application.isEditor)
		{
			list.RemoveAll((MenuBarItem i) => i.name.ToUpper().Equals("EXIT GAME"));
		}
		if (PlayerProgress.Instance.lastFinishedLevelOffline <= 0)
		{
			list.RemoveAll((MenuBarItem i) => i.name.ToUpper().Equals("CONTINUE ARCADE CAMPAIGN"));
		}
		this.itemNames = list.ToArray();
	}

	protected override void Update()
	{
		base.Update();
		if (Application.isEditor && Input.GetKeyDown(KeyCode.Backspace))
		{
			PlayerPrefs.DeleteAll();
		}
		if (!this.MenuActive && this.clickToStartMesh != null && this.clickToStartMesh.gameObject.activeInHierarchy && this.clickToStartMesh.GetComponent<Renderer>().material.HasProperty("_TintColor"))
		{
			this.clickToStartMesh.GetComponent<Renderer>().material.SetColor("_TintColor", Color.Lerp(Color.black, Color.white, 0.6f + Mathf.Sin(Time.time * 3f) * 0.3f));
		}
		if (Input.GetKeyDown(KeyCode.Backspace) && Input.GetKey(KeyCode.LeftControl))
		{
			PlayerPrefs.DeleteAll();
		}
		if (Input.GetMouseButtonDown(0) && !this.transitioning && this.clickToStartMesh.gameObject.activeInHierarchy)
		{
			this.MenuActive = true;
			this.clickToStartMesh.gameObject.SetActive(false);
		}
		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			foreach (TextMesh textMesh in this.items)
			{
				textMesh.gameObject.SetActive(!textMesh.gameObject.activeSelf);
			}
		}
		this.CheckCheats();
		if (Input.GetKeyDown(KeyCode.F1))
		{
			if (Time.timeScale > 0.9f)
			{
				Time.timeScale = 0.1f;
			}
			else
			{
				Time.timeScale = 1f;
			}
		}
	}

	protected void CheckCheats()
	{
		for (KeyCode keyCode = KeyCode.A; keyCode <= KeyCode.Z; keyCode++)
		{
			if (Input.GetKeyDown(keyCode))
			{
				this.ProcessCharacter(keyCode.ToString().ToCharArray()[0]);
			}
		}
	}

	private void Feedback()
	{
		Application.OpenURL("http://www.broforcegame.com/forums");
	}

	private void ProcessCharacter(char c)
	{
		this.currentInput += c;
		if (this.CheckCheatString("alaskanpipeline"))
		{
			UnityEngine.Debug.Log("ALASKANPIPELINE!");
			HeroUnlockController.UnlockAllBros();
		}
	}

	protected bool CheckCheatString(string cheatString)
	{
		return this.currentInput.Length >= cheatString.Length && this.currentInput.Substring(this.currentInput.Length - cheatString.Length).ToLower() == cheatString.ToLower();
	}

	public void StartGame()
	{
		if (!this.transitioning && !this.transitioningToExplanation && PlaytomicController.isExhibitionBuild)
		{
			HeroUnlockController.ClearUnlocks();
			UnityEngine.Debug.Log("Start Game! " + LevelSelectionController.exhibitionCount);
			LevelSelectionController.exhibitionCount++;
		}
		if (!this.transitioning)
		{
			LevelSelectionController.ResetLevelAndGameModeToDefault();
			LevelSelectionController.campaignToLoad = LevelSelectionController.DefaultCampaign;
			LevelSelectionController.loadCustomCampaign = false;
			LevelSelectionController.loadMode = MapLoadMode.Campaign;
			GameModeController.GameMode = GameMode.Campaign;
			LevelEditorGUI.levelEditorActive = false;
			HeroUnlockController.Initialize();
			WorldMapProgressController.startNewWorldMapGame = true;
			LevelSelectionController.returnToWorldMap = false;
			if (PlaytomicController.isExpendabrosBuild)
			{
				Fader.nextScene = LevelSelectionController.JoinScene;
				Fader.FadeSolid();
			}
			else
			{
				this.Transition();
			}
		}
	}

	public void StartWorldMap()
	{
		if (!this.transitioning)
		{
			LevelSelectionController.ResetLevelAndGameModeToDefault();
			if (PlaytomicController.isExhibitionBuild)
			{
				HeroUnlockController.ClearUnlocks();
			}
			LevelSelectionController.returnToWorldMap = true;
			GameModeController.GameMode = GameMode.Campaign;
			LevelEditorGUI.levelEditorActive = false;
			WorldMapProgressController.startNewWorldMapGame = true;
			HeroUnlockController.Initialize();
			this.Transition();
		}
	}

	public void ContinueWorldMap()
	{
		if (!this.transitioning)
		{
			LevelSelectionController.ResetLevelAndGameModeToDefault();
			if (PlaytomicController.isExhibitionBuild)
			{
				HeroUnlockController.ClearUnlocks();
			}
			LevelSelectionController.returnToWorldMap = true;
			GameModeController.GameMode = GameMode.Campaign;
			LevelEditorGUI.levelEditorActive = false;
			HeroUnlockController.Initialize();
			this.Transition();
		}
	}

	public void StartOnline(bool Continue, GameInfo matchToJoin = null)
	{
		LevelSelectionController.ResetLevelAndGameModeToDefault();
		LevelSelectionController.campaignToLoad = LevelSelectionController.OnlineCampaign;
		LevelSelectionController.loadCustomCampaign = false;
		LevelSelectionController.loadMode = MapLoadMode.Campaign;
		GameModeController.GameMode = GameMode.Campaign;
		if (Continue)
		{
			Map.ContinueLevel();
		}
		LevelEditorGUI.levelEditorActive = false;
		HeroUnlockController.Initialize();
		if (matchToJoin == null)
		{
			Connect.Layer.CreateMatch();
			Application.LoadLevel(LevelSelectionController.JoinScene);
		}
		else
		{
			Connect.Layer.JoinMatch(matchToJoin);
		}
	}

	public void ContinueGame()
	{
		LevelSelectionController.ResetLevelAndGameModeToDefault();
		GameModeController.GameMode = GameMode.Campaign;
		GameModeController.publishRun = false;
		LevelSelectionController.campaignToLoad = LevelSelectionController.DefaultCampaign;
		LevelSelectionController.loadCustomCampaign = false;
		LevelSelectionController.loadMode = MapLoadMode.Campaign;
		LevelSelectionController.returnToWorldMap = false;
		Map.ContinueLevel();
		LevelEditorGUI.levelEditorActive = false;
		if (PlaytomicController.isExpendabrosBuild)
		{
			Fader.nextScene = LevelSelectionController.JoinScene;
			Fader.FadeSolid();
		}
		else
		{
			this.Transition();
		}
	}

	private void ExitGame()
	{
		Application.Quit();
	}

	protected void Transition()
	{
		foreach (TextMesh textMesh in this.items)
		{
			textMesh.gameObject.SetActive(false);
		}
		if (!this.transitioningToExplanation)
		{
			this.TransitionToExplanation();
		}
		else if (Time.time - this.explanationTime > 1.5f)
		{
			this.TransitionToGame();
		}
		this.menuHighlight.gameObject.SetActive(false);
	}

	protected void TransitionToExplanation()
	{
		if (!this.transitioningToExplanation)
		{
			PanningStuff.PanVertical(300f);
			this.transitioningToExplanation = true;
			this.explanationTime = Time.time;
		}
	}

	protected void TransitionToGame()
	{
		if (!this.transitioning)
		{
			if (this.networkedBuild)
			{
				Fader.nextScene = LevelSelectionController.CampaignScene;
			}
			else
			{
				Fader.nextScene = LevelSelectionController.JoinScene;
			}
			Fader.FadeSolid();
			this.transitioning = true;
		}
	}

	private void GoToSteamPage()
	{
		if (!this.hasOpenedWebPage)
		{
			this.hasOpenedWebPage = true;
			Application.OpenURL("http://store.steampowered.com/app/274190/");
		}
	}

	public void StartDeathMatch()
	{
		if (!this.transitioning)
		{
			LevelSelectionController.ResetLevelAndGameModeToDefault();
			GameModeController.ResetPlayerRoundWins();
			GameModeController.GameMode = GameMode.DeathMatch;
			LevelSelectionController.campaignToLoad = "DefaultDeathmatch";
			LevelSelectionController.loadCustomCampaign = false;
			LevelSelectionController.loadMode = MapLoadMode.Campaign;
			LevelEditorGUI.levelEditorActive = false;
		}
		this.Transition();
		this.gameExplanation.text = "This is a BETA feature, we are still working on it.\n\nPlease send us your feedback,\nand together we can make this a better game.";
		this.thankYouBetaBackerText.gameObject.SetActive(true);
	}

	public void StartExplosionRun()
	{
		if (!this.transitioning)
		{
			LevelSelectionController.ResetLevelAndGameModeToDefault();
			GameModeController.ResetPlayerRoundWins();
			GameModeController.GameMode = GameMode.ExplosionRun;
			LevelSelectionController.campaignToLoad = "DefaultExplosionRun";
			LevelSelectionController.loadCustomCampaign = false;
			LevelSelectionController.loadMode = MapLoadMode.Campaign;
			LevelEditorGUI.levelEditorActive = false;
			LevelSelectionController.returnToWorldMap = false;
		}
		this.Transition();
		this.gameExplanation.text = "This is a BETA feature, we are still working on it.\n\nPlease send us your feedback,\nand together we can make this a better game.";
		this.thankYouBetaBackerText.gameObject.SetActive(true);
	}

	public void StartRace()
	{
		if (!this.transitioning)
		{
			LevelSelectionController.ResetLevelAndGameModeToDefault();
			GameModeController.ResetPlayerRoundWins();
			GameModeController.GameMode = GameMode.Race;
			LevelSelectionController.campaignToLoad = "DefaultRace";
			LevelSelectionController.loadCustomCampaign = false;
			LevelSelectionController.loadMode = MapLoadMode.Campaign;
			LevelEditorGUI.levelEditorActive = false;
			LevelSelectionController.returnToWorldMap = false;
		}
		this.Transition();
		this.gameExplanation.text = "This is a BETA feature, we are still working on it.\n\nPlease send us your feedback,\nand together we can make this a better game.";
		this.thankYouBetaBackerText.gameObject.SetActive(true);
	}

	public void StartSuicideHorde()
	{
		if (!this.transitioning)
		{
			LevelSelectionController.ResetLevelAndGameModeToDefault();
			GameModeController.ResetPlayerRoundWins();
			GameModeController.GameMode = GameMode.SuicideHorde;
			LevelSelectionController.campaignToLoad = "SuibroTest";
			LevelSelectionController.loadCustomCampaign = false;
			LevelSelectionController.loadMode = MapLoadMode.Campaign;
			LevelEditorGUI.levelEditorActive = false;
			LevelSelectionController.returnToWorldMap = false;
		}
		this.Transition();
		this.gameExplanation.text = "This is a BETA feature, we are still working on it.\n\nPlease send us your feedback,\nand together we can make this a better game.";
		this.thankYouBetaBackerText.gameObject.SetActive(true);
	}

	public void LevelEditor()
	{
		LevelSelectionController.ResetLevelAndGameModeToDefault();
		GameModeController.GameMode = GameMode.Campaign;
		LevelEditorGUI.levelEditorActive = true;
		LevelSelectionController.loadCustomCampaign = true;
		LevelSelectionController.loadMode = MapLoadMode.LoadFromFile;
		LevelSelectionController.levelFileNameToLoad = PlayerOptions.Instance.LastCustomLevel;
		LevelSelectionController.returnToWorldMap = false;
		Application.LoadLevel(LevelSelectionController.CampaignScene);
	}

	public void FilmMode()
	{
		GameModeController.GameMode = GameMode.Campaign;
		LevelEditorGUI.levelEditorActive = true;
		LevelSelectionController.loadCustomCampaign = true;
		LevelSelectionController.loadMode = MapLoadMode.LoadFromFile;
		LevelSelectionController.levelFileNameToLoad = PlayerOptions.Instance.LastCustomLevel;
		Application.LoadLevel("FilmScene");
	}

	public TextMesh clickToStartMesh;

	protected bool transitioning;

	protected bool transitioningToExplanation;

	protected float explanationTime = -1.5f;

	public static bool wasShown = true;

	private static bool firstStart = true;

	public bool networkedBuild;

	public Menu optionsMenu;

	public Menu customCampaignMenu;

	public GameObject lobbyGUI;

	public static MainMenu instance;

	public GameObject logo;

	public TextMesh gameExplanation;

	public TextMesh thankYouBetaBackerText;

	public Connect connectPrefab;

	private string currentInput = string.Empty;

	private bool hasOpenedWebPage;
}
