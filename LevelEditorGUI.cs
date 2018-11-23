// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class LevelEditorGUI : MonoBehaviour
{
	public static GUISkin GetGuiSkin()
	{
		return LevelEditorGUI.instance.skin;
	}

	public static string DebugText { get; set; }

	public static bool IsActive
	{
		get
		{
			return LevelEditorGUI.instance != null && LevelEditorGUI.instance.gameObject.activeSelf;
		}
	}

	private static LevelEditorGUI instance
	{
		get
		{
			if (LevelEditorGUI.Instance == null)
			{
				LevelEditorGUI[] array = Resources.FindObjectsOfTypeAll(typeof(LevelEditorGUI)) as LevelEditorGUI[];
				if (array.Length > 0)
				{
					LevelEditorGUI.Instance = array[0];
				}
			}
			return LevelEditorGUI.Instance;
		}
	}

	private void Awake()
	{
		LevelEditorGUI.levelEditorActive = true;
	}

	private void Start()
	{
		ShowMouseController.ForceMouse = true;
		this.targetSelectionIndicator = (UnityEngine.Object.Instantiate(this.SelectionIndicatorPrefab) as SelectionIndicator);
		this.targetSelectionIndicator.enabled = false;
		this.targetSelectionIndicator.name = "TargetSelectionIndicator";
		this.targetSelectionIndicatorOthers = new SelectionIndicator[16];
		for (int i = 0; i < this.targetSelectionIndicatorOthers.Length; i++)
		{
			this.targetSelectionIndicatorOthers[i] = (UnityEngine.Object.Instantiate(this.SelectionIndicatorPrefab) as SelectionIndicator);
			this.targetSelectionIndicatorOthers[i].enabled = false;
		}
		this.terrainTypes = (from TerrainType e in Enum.GetValues(typeof(TerrainType))
		orderby e.ToString()
		select e).ToList<TerrainType>();
		if (!Application.isEditor)
		{
			foreach (TerrainType item in Map.Instance.activeTheme.HiddenTerrain)
			{
				this.terrainTypes.Remove(item);
			}
		}
		this.doodadTypes = (from DoodadType e in Enum.GetValues(typeof(DoodadType))
		orderby e.ToString()
		select e).ToList<DoodadType>();
		if (!Application.isEditor)
		{
			foreach (DoodadType item2 in Map.Instance.activeTheme.HiddenDoodads)
			{
				this.doodadTypes.Remove(item2);
			}
		}
		this.triggerTypes = new List<TriggerType>();
		foreach (object obj in Enum.GetValues(typeof(TriggerType)))
		{
			this.triggerTypes.Add((TriggerType)((int)obj));
		}
		this.actionTypes = new List<TriggerActionType>();
		foreach (object obj2 in Enum.GetValues(typeof(TriggerActionType)))
		{
			this.actionTypes.Add((TriggerActionType)((int)obj2));
		}
		this.characterCommandTypes = new List<CharacterCommandType>();
		foreach (object obj3 in Enum.GetValues(typeof(CharacterCommandType)))
		{
			this.characterCommandTypes.Add((CharacterCommandType)((int)obj3));
		}
		this.enemyActionTypes = new List<EnemyActionType>();
		foreach (object obj4 in Enum.GetValues(typeof(EnemyActionType)))
		{
			this.enemyActionTypes.Add((EnemyActionType)((int)obj4));
		}
		this.gameModes = new List<GameMode>();
		this.gameModes.Add(GameMode.Campaign);
		this.gameModes.Add(GameMode.DeathMatch);
		this.gameModes.Add(GameMode.ExplosionRun);
		this.gameModes.Add(GameMode.Race);
		this.gameModes.Add(GameMode.SuicideHorde);
		this.levelThemes = new List<LevelTheme>();
		foreach (object obj5 in Enum.GetValues(typeof(LevelTheme)))
		{
			this.levelThemes.Add((LevelTheme)((int)obj5));
		}
		this.heroSpawnModes = new List<HeroSpawnMode>();
		foreach (object obj6 in Enum.GetValues(typeof(HeroSpawnMode)))
		{
			this.heroSpawnModes.Add((HeroSpawnMode)((int)obj6));
		}
		this.heroTypes = (from HeroType e in Enum.GetValues(typeof(HeroType))
		orderby e.ToString()
		select e).ToList<HeroType>();
		this.heroTypes.Remove(HeroType.None);
		this.heroTypes.Remove(HeroType.Final);
		this.heroTypes.Remove(HeroType.Random);
		this.heroTypes.Remove(HeroType.MadMaxBrotansky);
		this.heroTypes.Remove(HeroType.SuicideBro);
		this.musicTypes = (from MusicType e in Enum.GetValues(typeof(MusicType))
		orderby e.ToString()
		select e).ToList<MusicType>();
		this.musicTypes.Remove(MusicType.IntensityTest);
		this.weatherTypes = new List<WeatherType>();
		this.weatherTypes.Add(WeatherType.NoChange);
		this.weatherTypes.Add(WeatherType.Day);
		this.weatherTypes.Add(WeatherType.Overcast);
		this.weatherTypes.Add(WeatherType.Burning);
		this.weatherTypes.Add(WeatherType.Night);
		this.cameraFollowModes = new List<CameraFollowMode>();
		this.cameraFollowModes.Add(CameraFollowMode.ForcedHorizontal);
		this.cameraFollowModes.Add(CameraFollowMode.ForcedVertical);
		this.cameraFollowModes.Add(CameraFollowMode.Horizontal);
		this.cameraFollowModes.Add(CameraFollowMode.Vertical);
		this.raceCameraFollowModes = new List<CameraFollowMode>();
		this.raceCameraFollowModes.Add(CameraFollowMode.Horizontal);
		this.raceCameraFollowModes.Add(CameraFollowMode.Vertical);
		this.activeSelectionIndicator = (UnityEngine.Object.Instantiate(this.SelectionIndicatorPrefab) as SelectionIndicator);
		this.activeSelectionIndicator.enabled = false;
		this.mousePosSelectionIndicator = (UnityEngine.Object.Instantiate(this.SelectionIndicatorPrefab) as SelectionIndicator);
		this.RefreshFiles();
		this.mapData = Map.MapData;
		this.fileName = PlayerOptions.Instance.LastCustomLevel;
		Map.isEditing = true;
		this.newMapWidth = this.mapData.Width;
		this.newMapHeight = this.mapData.Height;
		this.gridClicked = new bool[this.mapData.Width, this.mapData.Height];
	}

	private void OnGUI()
	{
		if (Application.isEditor)
		{
			LevelEditorGUI.hasShownDisclaimer = true;
		}
		if (!LevelEditorGUI.hasShownDisclaimer)
		{
			GUILayout.BeginArea(new Rect((float)(Screen.width / 4), (float)(Screen.height / 4), (float)(Screen.width / 2), (float)(Screen.height / 2)));
			GUILayout.TextArea("This is the Broforce Level Editor (alpha)!\n\nThis is the tool we use to build levels in Broforce and we're quite excited to see what you guys can come up with.\n\nRight now the editor has a few bugs and is not as user-friendly as it should be. If you encounter any bugs or have any suggestions or feedback you can email me at ruan@freelives.net \n\nPlease note that a lot of the things in the level editor represent unfinished work, if you see something that we haven't put in our levels chances are it won't work properly.  \n\nSee our forums for a guide on how to use it: www.broforcegame.com/forums\n", new GUILayoutOption[0]);
			if (GUILayout.Button("OK!", new GUILayoutOption[0]) || Application.isEditor)
			{
				LevelEditorGUI.hasShownDisclaimer = true;
			}
			if (GUILayout.Button("VISIT THE BROFORCE LEVEL EDITOR FORUMS", new GUILayoutOption[0]))
			{
				Application.OpenURL("http://www.broforcegame.com/forums/forum/main-category/level-editor");
			}
			GUILayout.EndArea();
			return;
		}
		if (LevelEditorGUI.publishRunSuccessful)
		{
			LevelEditorGUI.publishRunSuccessful = false;
			this.showPublishRunSuccessful = true;
			PlaytomicController.levelUploadStatus = RemoteOperationStatus.Idle;
		}
		if (this.showPublishRunSuccessful)
		{
			GUILayout.BeginArea(new Rect((float)(Screen.width / 4), (float)(Screen.height / 4), (float)(Screen.width / 2), (float)(Screen.height / 2)), this.skin.box);
			if (PlaytomicController.levelUploadStatus == RemoteOperationStatus.Idle)
			{
				GUILayout.Label("You've completed your campaign and it is now ready to publish! Click OK below to save a shareable .bfg of your campaign, or CANCEL to just go back to editing and publish at a later time (you'll have to complete the campaign again though!).\n\n DISCLAIMER: AT SOME STAGE IN THE FUTURE WE MAY HAVE TO CLEAR OUR DATABASE OF ALL USER CREATED CONTENT. WE WILL DO WHAT WE CAN TO AVOID THIS, BUT PLEASE KEEP A LOCAL COPY OF ALL YOUR CAMPAIGNS THAT YOU DO NOT WISH TO LOSE.", new GUILayoutOption[0]);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (GUILayout.Button("Upload!", new GUILayoutOption[0]))
				{
					FileIO.PublishCampaign(LevelEditorGUI.campaign, this.fileName);
					PlaytomicController.UploadLevel(this.fileName, LevelEditorGUI.campaign.header);
				}
				if (GUILayout.Button("OK!", new GUILayoutOption[0]))
				{
					this.showPublishRunSuccessful = false;
					FileIO.PublishCampaign(LevelEditorGUI.campaign, this.fileName);
				}
				if (GUILayout.Button("Cancel", new GUILayoutOption[0]))
				{
					this.showPublishRunSuccessful = false;
				}
				GUILayout.EndHorizontal();
			}
			else if (PlaytomicController.levelUploadStatus == RemoteOperationStatus.Busy)
			{
				if (Time.time % 0.5f < 0.25f)
				{
					GUILayout.Label("UPLOADING....", new GUILayoutOption[0]);
				}
			}
			else if (PlaytomicController.levelUploadStatus == RemoteOperationStatus.Success)
			{
				GUILayout.Label("Upload Successful!", new GUILayoutOption[0]);
				if (GUILayout.Button("Woohoo!", new GUILayoutOption[0]))
				{
					this.showPublishRunSuccessful = false;
				}
			}
			else if (PlaytomicController.levelUploadStatus == RemoteOperationStatus.Fail)
			{
				GUILayout.Label("Upload FAILED!", new GUILayoutOption[0]);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (GUILayout.Button("Oh No!", new GUILayoutOption[0]))
				{
					this.showPublishRunSuccessful = false;
				}
				if (GUILayout.Button("Retry", new GUILayoutOption[0]))
				{
					FileIO.PublishCampaign(LevelEditorGUI.campaign, this.fileName);
					PlaytomicController.UploadLevel(this.fileName, LevelEditorGUI.campaign.header);
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();
			return;
		}
		if (Event.current.keyCode == KeyCode.Tab || Event.current.character == '\t')
		{
			Event.current.Use();
			Event.current = null;
		}
		if (!this.showGUI)
		{
			return;
		}
		GUILayout.BeginArea(new Rect((float)Screen.width * (1f - LevelEditorGUI.controlWidth), 0f, (float)Screen.width * LevelEditorGUI.controlWidth, (float)Screen.height), this.skin.box);
		if (GUILayout.Button((!Map.isEditing) ? "Edit (F3)" : "Play (F3)", new GUILayoutOption[0]))
		{
			this.TogglePlayMode();
		}
		if (!Map.isEditing)
		{
			if (LevelEditorGUI.DebugText != null)
			{
				GUILayout.Label(LevelEditorGUI.DebugText, new GUILayoutOption[0]);
			}
			this.ShowDebugMenu();
			GUILayout.EndArea();
			this.DrawDebugInfo();
			return;
		}
		if (Map.MapData != null)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (GUILayout.Button("Terrain", new GUILayoutOption[0]))
			{
				this.placingSquare = false;
				LevelEditorGUI.mode = EditorMode.Terrain;
				LevelEditorGUI.scrollPos = Vector2.zero;
			}
			if (GUILayout.Button("Doodads", new GUILayoutOption[0]))
			{
				this.placingSquare = false;
				LevelEditorGUI.mode = EditorMode.Doodads;
				LevelEditorGUI.scrollPos = Vector2.zero;
			}
			if (GUILayout.Button("Doodad Properties", new GUILayoutOption[0]))
			{
				this.placingSquare = false;
				if (this.hoverDoodadInfo != null && this.hoverDoodadInfo.entity != null)
				{
					UnityEngine.Object.Destroy(this.hoverDoodadInfo.entity);
				}
				LevelEditorGUI.mode = EditorMode.Tagging;
				LevelEditorGUI.scrollPos = Vector2.zero;
			}
			if (GUILayout.Button("Fluid", new GUILayoutOption[0]))
			{
				this.placingSquare = false;
				LevelEditorGUI.mode = EditorMode.Fluid;
				LevelEditorGUI.scrollPos = Vector2.zero;
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (Application.isEditor && GUILayout.Button("Debug", new GUILayoutOption[0]))
			{
				this.placingSquare = false;
				if (this.hoverDoodadInfo != null && this.hoverDoodadInfo.entity != null)
				{
					UnityEngine.Object.Destroy(this.hoverDoodadInfo.entity);
				}
				LevelEditorGUI.mode = EditorMode.Debug;
				LevelEditorGUI.scrollPos = Vector2.zero;
			}
			if (GUILayout.Button("Triggers", new GUILayoutOption[0]))
			{
				this.placingSquare = false;
				if (this.hoverDoodadInfo != null && this.hoverDoodadInfo.entity != null)
				{
					UnityEngine.Object.Destroy(this.hoverDoodadInfo.entity);
				}
				LevelEditorGUI.mode = EditorMode.Triggers;
				LevelEditorGUI.scrollPos = Vector2.zero;
			}
			if (GUILayout.Button("Meta", new GUILayoutOption[0]))
			{
				this.placingSquare = false;
				if (this.hoverDoodadInfo != null && this.hoverDoodadInfo.entity != null)
				{
					UnityEngine.Object.Destroy(this.hoverDoodadInfo.entity);
				}
				LevelEditorGUI.mode = EditorMode.Meta;
				LevelEditorGUI.scrollPos = Vector2.zero;
			}
			if (GUILayout.Button("Campaign", new GUILayoutOption[0]))
			{
				this.placingSquare = false;
				if (this.hoverDoodadInfo != null && this.hoverDoodadInfo.entity != null)
				{
					UnityEngine.Object.Destroy(this.hoverDoodadInfo.entity);
				}
				LevelEditorGUI.mode = EditorMode.Campaign;
				LevelEditorGUI.scrollPos = Vector2.zero;
			}
			if (GUILayout.Button("File", new GUILayoutOption[0]))
			{
				this.placingSquare = false;
				if (this.hoverDoodadInfo != null && this.hoverDoodadInfo.entity != null)
				{
					UnityEngine.Object.Destroy(this.hoverDoodadInfo.entity);
				}
				LevelEditorGUI.mode = EditorMode.File;
				LevelEditorGUI.scrollPos = Vector2.zero;
			}
			GUILayout.EndHorizontal();
		}
		else
		{
			LevelEditorGUI.mode = EditorMode.File;
		}
		if (this.terrainTypes != null && LevelEditorGUI.mode == EditorMode.Terrain)
		{
			LevelEditorGUI.scrollPos = GUILayout.BeginScrollView(LevelEditorGUI.scrollPos, LevelEditorGUI.GetGuiSkin().scrollView);
			this.curTerrainType = (TerrainType)((int)LevelEditorGUI.SelectList(this.terrainTypes, this.curTerrainType, this.skin.label, this.skin.button));
			GUILayout.EndScrollView();
		}
		else if (this.doodadTypes != null && LevelEditorGUI.mode == EditorMode.Doodads)
		{
			this.ShowDoodadMenu();
		}
		else if (LevelEditorGUI.mode == EditorMode.Fluid)
		{
			this.ShowFluidMenu();
		}
		else if (LevelEditorGUI.mode == EditorMode.File)
		{
			this.ShowFileMenu();
		}
		else if (LevelEditorGUI.mode == EditorMode.Debug || LevelEditorGUI.mode == EditorMode.Play)
		{
			this.ShowDebugMenu();
		}
		else if (LevelEditorGUI.mode == EditorMode.Triggers)
		{
			this.ShowTriggerMenu();
		}
		else if (LevelEditorGUI.mode == EditorMode.Meta)
		{
			this.ShowMetaMenu();
		}
		else if (LevelEditorGUI.mode == EditorMode.Campaign)
		{
			this.ShowCampaignMenu();
		}
		else if (LevelEditorGUI.mode == EditorMode.Tagging)
		{
			this.ShowTaggingMenu();
		}
		if (LevelEditorGUI.DebugText != null)
		{
			GUILayout.Label(LevelEditorGUI.DebugText, new GUILayoutOption[0]);
		}
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (GUILayout.RepeatButton("+", new GUILayoutOption[0]))
		{
			LevelEditorGUI.controlWidth = Mathf.Clamp(LevelEditorGUI.controlWidth + 0.1f * Time.deltaTime, 0.1f, 1f);
		}
		if (GUILayout.RepeatButton("-", new GUILayoutOption[0]))
		{
			LevelEditorGUI.controlWidth = Mathf.Clamp(LevelEditorGUI.controlWidth - 0.1f * Time.deltaTime, 0.1f, 1f);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		if (LevelEditorGUI.mode == EditorMode.Doodads || LevelEditorGUI.mode == EditorMode.Tagging)
		{
			this.DrawDoodadUIInfo();
		}
		this.DrawDebugInfo();
	}

	private void ShowTaggingMenu()
	{
		LevelEditorGUI.scrollPos = GUILayout.BeginScrollView(LevelEditorGUI.scrollPos, this.skin.scrollView);
		if (this.taggedClick.row >= 0 && this.taggedClick.collumn >= 0)
		{
			for (int i = 0; i < this.mapData.DoodadList.Count; i++)
			{
				DoodadInfo doodadInfo = this.mapData.DoodadList[i];
				if (doodadInfo.position.c == this.taggedClick.collumn && doodadInfo.position.r == this.taggedClick.row)
				{
					GUILayout.Label("Doodad: " + doodadInfo.type, new GUILayoutOption[0]);
					GUILayout.Label(string.Concat(new object[]
					{
						"Position: ",
						doodadInfo.position.c,
						", ",
						doodadInfo.position.r
					}), new GUILayoutOption[0]);
					GUILayout.Label("Variation: " + doodadInfo.variation, new GUILayoutOption[0]);
					if (doodadInfo.type == DoodadType.Zipline)
					{
						if (!this.settingWaypoint)
						{
							if (GUILayout.Button("Set Other Point " + (string.IsNullOrEmpty(doodadInfo.tag) ? string.Empty : doodadInfo.tag), new GUILayoutOption[0]))
							{
								this.settingWaypoint = true;
							}
							if (GUILayout.Button("Clear other point", new GUILayoutOption[0]))
							{
								doodadInfo.tag = string.Empty;
								Map.Instance.ResetZiplines();
							}
						}
						else
						{
							GUILayout.Label("Setting other point...", new GUILayoutOption[0]);
							if (Input.GetMouseButtonUp(0) && this.settingWaypoint)
							{
								this.settingWaypoint = false;
								GridPoint gridPoint = new GridPoint();
								gridPoint.collumn = Mathf.Clamp(this.c, 0, this.mapData.Width - 1);
								gridPoint.row = Mathf.Clamp(this.r, 0, this.mapData.Height - 1);
								if (this.mapData.GetDoodadsAt(gridPoint.collumn, gridPoint.row).FirstOrDefault((DoodadInfo d) => d.type == DoodadType.Zipline) != null)
								{
									doodadInfo.tag = gridPoint.ToString();
								}
								Map.Instance.ResetZiplines();
							}
						}
					}
					else
					{
						GUILayout.BeginHorizontal(new GUILayoutOption[0]);
						GUILayout.Label("Tag: ", new GUILayoutOption[0]);
						doodadInfo.tag = GUILayout.TextField(doodadInfo.tag ?? string.Empty, new GUILayoutOption[0]);
						GUILayout.EndHorizontal();
					}
					this.ShowDoodadSpecificProperties(doodadInfo);
				}
			}
		}
		else
		{
			GUILayout.Label("Click on a doodad to edit its properties", new GUILayoutOption[0]);
		}
		GUILayout.EndScrollView();
	}

	private void ShowDoodadSpecificProperties(DoodadInfo dood)
	{
		if (dood.type != DoodadType.Miniboss || dood.variation == 1)
		{
		}
	}

	private void ShowFluidMenu()
	{
		GUILayout.Label("Left click to pour water", new GUILayoutOption[0]);
		string text = "Pause Simulation";
		if (FluidController.Paused)
		{
			text = "Unpause Simulation";
		}
		if (GUILayout.Button(text, new GUILayoutOption[0]))
		{
			if (FluidController.Paused)
			{
				FluidController.Unpause();
			}
			else
			{
				FluidController.Pause();
			}
		}
		if (GUILayout.Button("Clear All Fluid", new GUILayoutOption[0]))
		{
			FluidController.ClearAllFluid();
		}
	}

	private void ShowDoodadMenu()
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (this.hoverDoodadInfo != null)
		{
			if (GUILayout.Button("R", new GUILayoutOption[0]) || Input.GetKeyDown(KeyCode.R))
			{
				this.hoverDoodadInfo.variation = -1;
				this.curDoodadVariation = -1;
				this.PlaceHoverDoodad();
			}
			for (int i = 0; i < Map.Instance.GetDoodadVariationAmount(this.hoverDoodadInfo.type); i++)
			{
				if (GUILayout.Button(i.ToString(), new GUILayoutOption[0]) || Input.GetKeyDown(KeyCode.Alpha0 + i))
				{
					this.hoverDoodadInfo.variation = i;
					this.curDoodadVariation = i;
					this.PlaceHoverDoodad();
				}
			}
			if (InputReader.GetControllerPressingFire() > -1)
			{
				this.curDoodadVariation = (this.hoverDoodadInfo.variation = (this.hoverDoodadInfo.variation + 1) % Map.Instance.GetDoodadVariationAmount(this.hoverDoodadInfo.type));
				this.PlaceHoverDoodad();
			}
			if (InputReader.GetControllerPressingSpecial() > -1)
			{
				this.curDoodadVariation = (this.hoverDoodadInfo.variation = (this.hoverDoodadInfo.variation + Map.Instance.GetDoodadVariationAmount(this.hoverDoodadInfo.type) - 1) % Map.Instance.GetDoodadVariationAmount(this.hoverDoodadInfo.type));
				this.PlaceHoverDoodad();
			}
		}
		GUILayout.EndHorizontal();
		LevelEditorGUI.scrollPos = GUILayout.BeginScrollView(LevelEditorGUI.scrollPos, this.skin.scrollView);
		this.curDoodadType = (DoodadType)((int)LevelEditorGUI.SelectList(this.doodadTypes, this.curDoodadType, this.skin.label, this.skin.button));
		LevelEditorGUI.showAllDoodads = GUILayout.Toggle(LevelEditorGUI.showAllDoodads, "Show All Doodad Names", new GUILayoutOption[0]);
		GUILayout.EndScrollView();
	}

	public void ShowFileMenu()
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Filename:", new GUILayoutOption[0]);
		this.fileName = GUILayout.TextField(this.fileName ?? string.Empty, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		LevelEditorGUI.scrollPos = GUILayout.BeginScrollView(LevelEditorGUI.scrollPos, this.skin.scrollView);
		if (this.publishPrompt)
		{
			if (string.IsNullOrEmpty(LevelEditorGUI.campaign.header.name) || string.IsNullOrEmpty(LevelEditorGUI.campaign.header.author))
			{
				GUILayout.Label("You have to enter a level name and author before you can publish!", new GUILayoutOption[0]);
				if (GUILayout.Button("OK", new GUILayoutOption[0]))
				{
					this.publishPrompt = false;
				}
			}
			else if (LevelEditorGUI.campaign.header.gameMode != GameMode.Campaign)
			{
				GUILayout.Label("You can only publish normal campaigns (no explosion run/deathmatch) at the moment.", new GUILayoutOption[0]);
				if (GUILayout.Button("OK", new GUILayoutOption[0]))
				{
					this.publishPrompt = false;
				}
			}
			else if (PlaytomicController.IsThisBuildOutOfDate())
			{
				GUILayout.Label("Your version of Broforce is out of date - please update before publishing!", new GUILayoutOption[0]);
				if (GUILayout.Button("OK", new GUILayoutOption[0]))
				{
					this.publishPrompt = false;
				}
			}
			else
			{
				GUILayout.Label("Publishing a campaign means that it is saved in a format that isn't editable by the level editor anymore, so you can share the campaign freely without someone copying your work. Make sure to save a editable version of your campaign before publishing.\n\nEach published custom campaign also has its own leaderboard so players can compete on your level.To ensure the campaign is actually completeable, you will have to finish it once yourself before being able to publish it. Click OK to start your playthrough or CANCEL to go back to editing.\n\nPublished campaigns have the file extension .bfg, unpublished campaigns have .bfc", new GUILayoutOption[0]);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (GUILayout.Button("OK", new GUILayoutOption[0]))
				{
					this.StartPublishRun();
				}
				if (GUILayout.Button("Cancel", new GUILayoutOption[0]))
				{
					this.publishPrompt = false;
				}
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (Application.isEditor)
		{
			if (GUILayout.Button("Save Level", new GUILayoutOption[0]))
			{
				this.SaveLevel();
			}
			if (GUILayout.Button("Save Camp", new GUILayoutOption[0]))
			{
				PlayerOptions.Instance.LastCustomLevel = this.fileName;
				LevelEditorGUI.campaign.name = this.fileName;
				FileIO.SaveCampaign(LevelEditorGUI.campaign, this.fileName);
			}
		}
		else if (GUILayout.Button("Save", new GUILayoutOption[0]))
		{
			PlayerOptions.Instance.LastCustomLevel = this.fileName;
			LevelEditorGUI.campaign.name = this.fileName;
			FileIO.SaveCampaign(LevelEditorGUI.campaign, this.fileName);
		}
		if (GUILayout.Button("Publish", new GUILayoutOption[0]))
		{
			this.publishPrompt = true;
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (GUILayout.Button("Load", new GUILayoutOption[0]))
		{
			this.LoadCampaign();
		}
		if (GUILayout.Button("Additive Load", new GUILayoutOption[0]))
		{
			this.AdditiveLoadCampaign();
		}
		if (GUILayout.Button("New", new GUILayoutOption[0]))
		{
			MapData mapData = new MapData(this.newMapWidth, this.newMapHeight);
			this.SetBottomRowsToEarth(mapData);
			Campaign campaign = new Campaign();
			campaign.levels = new MapData[1];
			campaign.levels[0] = mapData;
			LevelEditorGUI.campaign = campaign;
			this.fileName = "Untitled";
			PlayerOptions.Instance.LastCustomLevel = this.fileName;
			LevelSelectionController.MapDataToLoad = mapData;
			LevelSelectionController.loadMode = MapLoadMode.LoadFromMapdata;
			Application.LoadLevel(Application.loadedLevel);
		}
		GUILayout.EndHorizontal();
		foreach (string text in this.files)
		{
			if (GUILayout.Button(text, new GUILayoutOption[0]))
			{
				this.fileName = text;
			}
		}
		GUILayout.EndScrollView();
	}

	private void ShowCampaignMenu()
	{
		if (LevelEditorGUI.campaign == null)
		{
			LevelEditorGUI.campaign = new Campaign();
		}
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Name: ", new GUILayoutOption[0]);
		LevelEditorGUI.campaign.name = (LevelEditorGUI.campaign.header.name = GUILayout.TextField(LevelEditorGUI.campaign.header.name ?? string.Empty, new GUILayoutOption[0]));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Author: ", new GUILayoutOption[0]);
		LevelEditorGUI.campaign.header.author = GUILayout.TextField(LevelEditorGUI.campaign.header.author ?? string.Empty, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Description: ", new GUILayoutOption[0]);
		LevelEditorGUI.campaign.header.description = GUILayout.TextField(LevelEditorGUI.campaign.header.description ?? string.Empty, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Label("Game Mode:", new GUILayoutOption[0]);
		LevelEditorGUI.campaign.header.gameMode = (GameMode)((int)LevelEditorGUI.SelectList(this.gameModes, LevelEditorGUI.campaign.header.gameMode, this.skin.label, this.skin.button));
		LevelEditorGUI.campaign.header.hasBrotalityScoreboard = GUILayout.Toggle(LevelEditorGUI.campaign.header.hasBrotalityScoreboard, "Enable Brotality Score", new GUILayoutOption[0]);
		LevelEditorGUI.campaign.header.hasTimeScoreBoard = GUILayout.Toggle(LevelEditorGUI.campaign.header.hasTimeScoreBoard, "Enable Time Score", new GUILayoutOption[0]);
		LevelEditorGUI.scrollPos = GUILayout.BeginScrollView(LevelEditorGUI.scrollPos, this.skin.scrollView);
		for (int i = 0; i < LevelEditorGUI.campaign.Length; i++)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label(i + ") ", new GUILayoutOption[]
			{
				GUILayout.Width(20f)
			});
			LevelEditorGUI.campaign.levels[i].levelDescription = GUILayout.TextField(LevelEditorGUI.campaign.levels[i].levelDescription, new GUILayoutOption[0]);
			if (GUILayout.Button("▲", new GUILayoutOption[0]) && i - 1 >= 0)
			{
				MapData mapData = LevelEditorGUI.campaign.levels[i];
				LevelEditorGUI.campaign.levels[i] = LevelEditorGUI.campaign.levels[i - 1];
				LevelEditorGUI.campaign.levels[i - 1] = mapData;
			}
			if (GUILayout.Button("▼", new GUILayoutOption[0]) && i + 1 < LevelEditorGUI.campaign.Length)
			{
				MapData mapData2 = LevelEditorGUI.campaign.levels[i];
				LevelEditorGUI.campaign.levels[i] = LevelEditorGUI.campaign.levels[i + 1];
				LevelEditorGUI.campaign.levels[i + 1] = mapData2;
			}
			if (this.mapData == LevelEditorGUI.campaign.levels[i])
			{
				GUILayout.Label("(CURRENTLY EDITING)", new GUILayoutOption[0]);
			}
			else
			{
				if (GUILayout.Button("EDIT", new GUILayoutOption[0]))
				{
					LevelSelectionController.MapDataToLoad = LevelEditorGUI.campaign.levels[i];
					LevelSelectionController.loadMode = MapLoadMode.LoadFromMapdata;
					Application.LoadLevel(Application.loadedLevel);
				}
				if (GUILayout.Button("DEL", new GUILayoutOption[0]))
				{
					List<MapData> list = new List<MapData>(LevelEditorGUI.campaign.levels);
					list.RemoveAt(i);
					LevelEditorGUI.campaign.levels = list.ToArray();
				}
			}
			GUILayout.EndHorizontal();
		}
		if (this.showNewLevelBox)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (GUILayout.Button("CREATE!", new GUILayoutOption[0]))
			{
				MapData[] array = new MapData[LevelEditorGUI.campaign.Length + 1];
				for (int j = 0; j < LevelEditorGUI.campaign.Length; j++)
				{
					array[j] = LevelEditorGUI.campaign.levels[j];
				}
				MapData mapData3 = array[LevelEditorGUI.campaign.Length] = new MapData(this.newMapWidth, this.newMapHeight);
				LevelEditorGUI.campaign.levels = array;
				mapData3.theme = this.newMapTheme;
				mapData3.levelDescription = this.newMapName;
				this.SetBottomRowsToEarth(mapData3);
				LevelSelectionController.loadMode = MapLoadMode.LoadFromMapdata;
				LevelSelectionController.MapDataToLoad = array[LevelEditorGUI.campaign.Length - 1];
				Application.LoadLevel(Application.loadedLevel);
			}
			if (GUILayout.Button("Cancel", new GUILayoutOption[0]))
			{
				this.showNewLevelBox = false;
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Name:", new GUILayoutOption[0]);
			this.newMapName = GUILayout.TextField(this.newMapName ?? string.Empty, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			this.newMapTheme = (LevelTheme)((int)LevelEditorGUI.SelectList(this.levelThemes, this.newMapTheme, this.skin.label, this.skin.button));
		}
		else
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (GUILayout.Button("Add New Level", new GUILayoutOption[0]))
			{
				this.showNewLevelBox = true;
			}
			if (GUILayout.Button("Resize Current Level", new GUILayoutOption[0]))
			{
				this.ResizeMap(this.newMapWidth, this.newMapHeight);
				LevelSelectionController.loadMode = MapLoadMode.LoadFromMapdata;
				LevelSelectionController.MapDataToLoad = this.mapData;
				Application.LoadLevel(Application.loadedLevel);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Width " + this.newMapWidth, new GUILayoutOption[0]);
		this.newMapWidth = (int)GUILayout.HorizontalSlider((float)this.newMapWidth, 32f, 256f, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Height " + this.newMapHeight, new GUILayoutOption[0]);
		this.newMapHeight = (int)GUILayout.HorizontalSlider((float)this.newMapHeight, 32f, 256f, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		if (Application.isEditor && GUILayout.Button("Add Current Level (only freelives sees this)", new GUILayoutOption[0]))
		{
			MapData[] array2 = new MapData[LevelEditorGUI.campaign.Length + 1];
			for (int k = 0; k < LevelEditorGUI.campaign.Length; k++)
			{
				array2[k] = LevelEditorGUI.campaign.levels[k];
			}
			array2[LevelEditorGUI.campaign.Length] = Map.MapData;
			LevelEditorGUI.campaign.levels = array2;
		}
		if (GUILayout.Button("Duplicate Current", new GUILayoutOption[0]))
		{
			Stream stream = new MemoryStream();
			using (stream)
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(MapData));
				MemoryStream memoryStream = new MemoryStream();
				StreamWriter streamWriter = new StreamWriter(memoryStream);
				xmlSerializer.Serialize(streamWriter, Map.MapData);
				streamWriter.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				XmlTextReader xmlReader = new XmlTextReader(memoryStream);
				MapData mapData4 = (MapData)xmlSerializer.Deserialize(xmlReader);
				MapData[] array3 = new MapData[LevelEditorGUI.campaign.Length + 1];
				for (int l = 0; l < LevelEditorGUI.campaign.Length; l++)
				{
					array3[l] = LevelEditorGUI.campaign.levels[l];
				}
				array3[LevelEditorGUI.campaign.Length] = mapData4;
				LevelEditorGUI.campaign.levels = array3;
				LevelSelectionController.loadMode = MapLoadMode.LoadFromMapdata;
				LevelSelectionController.MapDataToLoad = array3[LevelEditorGUI.campaign.Length - 1];
			}
		}
		GUILayout.EndScrollView();
	}

	public void ShowDebugMenu()
	{
		if (GUILayout.Button("Show Lightmap", new GUILayoutOption[0]))
		{
			this.showLightMap = !this.showLightMap;
		}
		if (GUILayout.Button("Show Terrain", new GUILayoutOption[0]))
		{
			this.showTerrain = !this.showTerrain;
		}
		if (GUILayout.Button("Show Grid", new GUILayoutOption[0]))
		{
			this.showGrid = !this.showGrid;
		}
		if (GUILayout.Button(((!this.showPathfinding) ? "Enable " : "Disable ") + " Show Path", new GUILayoutOption[0]))
		{
			this.showPathfinding = !this.showPathfinding;
		}
		if (GUILayout.Button("score player number: " + this.pcount, new GUILayoutOption[0]))
		{
			this.pcount++;
			if (this.pcount > 4)
			{
				this.pcount = 1;
			}
		}
		if (GUILayout.Button("Show Scores", new GUILayoutOption[0]))
		{
		}
	}

	private void ShowMetaMenu()
	{
		LevelEditorGUI.scrollPos = GUILayout.BeginScrollView(LevelEditorGUI.scrollPos, this.skin.scrollView);
		GUILayout.Label("Theme:", new GUILayoutOption[0]);
		this.mapData.theme = (LevelTheme)((int)LevelEditorGUI.SelectList(this.levelThemes, this.mapData.theme, this.skin.label, this.skin.button));
		GUILayout.Label("Spawn Mode:", new GUILayoutOption[0]);
		this.mapData.heroSpawnMode = (HeroSpawnMode)((int)LevelEditorGUI.SelectList(this.heroSpawnModes, this.mapData.heroSpawnMode, this.skin.label, this.skin.button));
		this.mapData.suppressAnnouncer = GUILayout.Toggle(this.mapData.suppressAnnouncer, "Suppress Intro Announcer", new GUILayoutOption[0]);
		if (LevelEditorGUI.campaign != null && LevelEditorGUI.campaign.header != null)
		{
			switch (LevelEditorGUI.campaign.header.gameMode)
			{
			case GameMode.Campaign:
				this.mapData.cameraFollowMode = CameraFollowMode.Normal;
				break;
			case GameMode.ExplosionRun:
			case GameMode.SuicideHorde:
				GUILayout.Label("Camera follow mode:", new GUILayoutOption[0]);
				this.mapData.cameraFollowMode = (CameraFollowMode)((int)LevelEditorGUI.SelectList(this.cameraFollowModes, this.mapData.cameraFollowMode, this.skin.label, this.skin.button));
				GUILayout.Label("Camera Speed: " + this.mapData.cameraSpeed, new GUILayoutOption[0]);
				this.mapData.cameraSpeed = GUILayout.HorizontalSlider(this.mapData.cameraSpeed, 10f, 50f, new GUILayoutOption[0]);
				break;
			case GameMode.DeathMatch:
			case GameMode.BroDown:
			case GameMode.TeamDeathMatch:
				this.mapData.cameraFollowMode = CameraFollowMode.SingleScreen;
				break;
			case GameMode.Race:
				GUILayout.Label("Camera follow mode:", new GUILayoutOption[0]);
				this.mapData.cameraFollowMode = (CameraFollowMode)((int)LevelEditorGUI.SelectList(this.raceCameraFollowModes, this.mapData.cameraFollowMode, this.skin.label, this.skin.button));
				break;
			}
		}
		GUILayout.Label("Level Title: (Leave blank to not show)", new GUILayoutOption[0]);
		this.mapData.levelDescription = GUILayout.TextField(this.mapData.levelDescription ?? string.Empty, new GUILayoutOption[0]);
		float regualrMookSpawnProbability = this.mapData.regualrMookSpawnProbability;
		float suicideMookSpawnProbability = this.mapData.suicideMookSpawnProbability;
		float riotShieldMookSpawnProbability = this.mapData.riotShieldMookSpawnProbability;
		float bigMookSpawnProbability = this.mapData.bigMookSpawnProbability;
		GUILayout.Label("Regular Mook Probability: " + (int)(this.mapData.regualrMookSpawnProbability * 100f), new GUILayoutOption[0]);
		this.mapData.regualrMookSpawnProbability = GUILayout.HorizontalSlider(this.mapData.regualrMookSpawnProbability, 0f, 1f, new GUILayoutOption[0]);
		GUILayout.Label("Suicide Mook Probability: " + (int)(this.mapData.suicideMookSpawnProbability * 100f), new GUILayoutOption[0]);
		this.mapData.suicideMookSpawnProbability = GUILayout.HorizontalSlider(this.mapData.suicideMookSpawnProbability, 0f, 1f, new GUILayoutOption[0]);
		GUILayout.Label("Riot Shield Mook Probability: " + (int)(this.mapData.riotShieldMookSpawnProbability * 100f), new GUILayoutOption[0]);
		this.mapData.riotShieldMookSpawnProbability = GUILayout.HorizontalSlider(this.mapData.riotShieldMookSpawnProbability, 0f, 1f, new GUILayoutOption[0]);
		GUILayout.Label("Big Mook Probability: " + (int)(this.mapData.bigMookSpawnProbability * 100f), new GUILayoutOption[0]);
		this.mapData.bigMookSpawnProbability = GUILayout.HorizontalSlider(this.mapData.bigMookSpawnProbability, 0f, 1f, new GUILayoutOption[0]);
		if (suicideMookSpawnProbability != this.mapData.suicideMookSpawnProbability && this.mapData.suicideMookSpawnProbability == 1f)
		{
			this.mapData.regualrMookSpawnProbability = 0f; this.mapData.bigMookSpawnProbability = (this.mapData.riotShieldMookSpawnProbability = (this.mapData.regualrMookSpawnProbability ));
		}
		if (regualrMookSpawnProbability != this.mapData.regualrMookSpawnProbability && this.mapData.regualrMookSpawnProbability == 1f)
		{
			this.mapData.suicideMookSpawnProbability = 0f; this.mapData.bigMookSpawnProbability = (this.mapData.riotShieldMookSpawnProbability = (this.mapData.suicideMookSpawnProbability ));
		}
		if (bigMookSpawnProbability != this.mapData.bigMookSpawnProbability && this.mapData.bigMookSpawnProbability == 1f)
		{
			this.mapData.suicideMookSpawnProbability = 0f; this.mapData.regualrMookSpawnProbability = (this.mapData.riotShieldMookSpawnProbability = (this.mapData.suicideMookSpawnProbability ));
		}
		if (riotShieldMookSpawnProbability != this.mapData.riotShieldMookSpawnProbability && this.mapData.riotShieldMookSpawnProbability == 1f)
		{
			this.mapData.suicideMookSpawnProbability = 0f; this.mapData.regualrMookSpawnProbability = (this.mapData.bigMookSpawnProbability = (this.mapData.suicideMookSpawnProbability ));
		}
		float num = this.mapData.suicideMookSpawnProbability + this.mapData.riotShieldMookSpawnProbability + this.mapData.bigMookSpawnProbability + this.mapData.regualrMookSpawnProbability;
		if (num > 0f)
		{
			if (!LevelEditorGUI.OnlyFirstVariableGreaterThanZeroDistribute(ref this.mapData.regualrMookSpawnProbability, ref this.mapData.suicideMookSpawnProbability, ref this.mapData.riotShieldMookSpawnProbability, ref this.mapData.bigMookSpawnProbability))
			{
				if (!LevelEditorGUI.OnlyFirstVariableGreaterThanZeroDistribute(ref this.mapData.suicideMookSpawnProbability, ref this.mapData.regualrMookSpawnProbability, ref this.mapData.riotShieldMookSpawnProbability, ref this.mapData.bigMookSpawnProbability))
				{
					if (!LevelEditorGUI.OnlyFirstVariableGreaterThanZeroDistribute(ref this.mapData.riotShieldMookSpawnProbability, ref this.mapData.regualrMookSpawnProbability, ref this.mapData.suicideMookSpawnProbability, ref this.mapData.bigMookSpawnProbability))
					{
						if (!LevelEditorGUI.OnlyFirstVariableGreaterThanZeroDistribute(ref this.mapData.bigMookSpawnProbability, ref this.mapData.regualrMookSpawnProbability, ref this.mapData.suicideMookSpawnProbability, ref this.mapData.riotShieldMookSpawnProbability))
						{
							float num2 = 1f / num;
							this.mapData.regualrMookSpawnProbability *= num2;
							this.mapData.suicideMookSpawnProbability *= num2;
							this.mapData.riotShieldMookSpawnProbability *= num2;
							this.mapData.bigMookSpawnProbability *= num2;
						}
					}
				}
			}
		}
		GUILayout.Label("Oil Barrel to Red Barrel Ratio: " + (int)(this.mapData.oilBarrelSpawnProbability * 100f), new GUILayoutOption[0]);
		this.mapData.oilBarrelSpawnProbability = GUILayout.HorizontalSlider(this.mapData.oilBarrelSpawnProbability, 0f, 1f, new GUILayoutOption[0]);
		GUILayout.Label("Propane Tank Probability: " + (int)(this.mapData.propaneTankSpawnProbability * 100f), new GUILayoutOption[0]);
		this.mapData.propaneTankSpawnProbability = GUILayout.HorizontalSlider(this.mapData.propaneTankSpawnProbability, 0f, 1f, new GUILayoutOption[0]);
		GUILayout.Label("Mine Field Probability: " + (int)(this.mapData.mineFieldSpawnProbability * 100f), new GUILayoutOption[0]);
		this.mapData.mineFieldSpawnProbability = GUILayout.HorizontalSlider(this.mapData.mineFieldSpawnProbability, 0f, 1f, new GUILayoutOption[0]);
		GUILayout.Label("Spike Trap Probability: " + (int)(this.mapData.spikeTrapSpawnProbability * 100f), new GUILayoutOption[0]);
		this.mapData.spikeTrapSpawnProbability = GUILayout.HorizontalSlider(this.mapData.spikeTrapSpawnProbability, 0f, 1f, new GUILayoutOption[0]);
		GUILayout.Label("Coconut Probability: " + (int)(this.mapData.coconutProbability * 100f), new GUILayoutOption[0]);
		this.mapData.coconutProbability = GUILayout.HorizontalSlider(this.mapData.coconutProbability, 0f, 1f, new GUILayoutOption[0]);
		this.mapData.spawnAmmoCrates = GUILayout.Toggle(this.mapData.spawnAmmoCrates, "Ammo Crates will Spawn: ", new GUILayoutOption[0]);
		if (LevelEditorGUI.campaign == null || LevelEditorGUI.campaign.header == null || LevelEditorGUI.campaign.header.gameMode == GameMode.Campaign)
		{
			if (this.mapData.forcedBros == null)
			{
				if (GUILayout.Toggle(false, "Force Specific Bros", new GUILayoutOption[0]))
				{
					this.mapData.forcedBros = new List<HeroType>();
				}
			}
			else if (!GUILayout.Toggle(true, "Force Specific Bros", new GUILayoutOption[0]))
			{
				this.mapData.forcedBros = null;
			}
			if (this.mapData.forcedBros != null)
			{
				foreach (HeroType heroType in this.heroTypes)
				{
					if (this.mapData.forcedBros.Contains(heroType))
					{
						if (!GUILayout.Toggle(true, HeroController.GetHeroName(heroType), new GUILayoutOption[0]))
						{
							this.mapData.forcedBros.Remove(heroType);
						}
					}
					else if (GUILayout.Toggle(false, HeroController.GetHeroName(heroType), new GUILayoutOption[0]))
					{
						this.mapData.forcedBros.Add(heroType);
					}
				}
			}
		}
		else
		{
			this.mapData.forcedBro = HeroType.Random;
		}
		if (LevelEditorGUI.campaign != null && LevelEditorGUI.campaign.header != null)
		{
			GUILayout.Label("Music Type:", new GUILayoutOption[0]);
			this.mapData.musicType = (MusicType)((int)LevelEditorGUI.SelectList(this.musicTypes, this.mapData.musicType, this.skin.label, this.skin.button));
		}
		GUILayout.Label("Initial Weather:", new GUILayoutOption[0]);
		this.mapData.weatherType = (WeatherType)((int)LevelEditorGUI.SelectList(this.weatherTypes, this.mapData.weatherType, this.skin.label, this.skin.button));
		GUILayout.EndScrollView();
	}

	protected static bool OnlyFirstVariableGreaterThanZeroDistribute(ref float firstProb, ref float secondProb, ref float thirdProb, ref float fourthProb)
	{
		float num = firstProb + secondProb + thirdProb + fourthProb;
		if (num < 1f && firstProb > 0f && secondProb <= 0f && thirdProb <= 0f && fourthProb <= 0f)
		{
			float num2 = 1f - firstProb;
			secondProb += num2 / 3f;
			thirdProb += num2 / 3f;
			fourthProb += num2 / 3f;
			return true;
		}
		return false;
	}

	private void DrawDebugInfo()
	{
		if (this.showTerrain)
		{
			for (int i = 0; i < Map.Width; i++)
			{
				for (int j = 0; j < Map.Height; j++)
				{
					float blocksX = Map.GetBlocksX(i);
					float blocksY = Map.GetBlocksY(j);
					Vector3 vector = Camera.main.WorldToScreenPoint(new Vector3(blocksX, blocksY, 0f));
					if (vector.x > 0f && vector.x < (float)Screen.width * 5f / 6f && vector.y > 0f && vector.y < (float)Screen.height)
					{
						GUI.Label(new Rect(vector.x, (float)Screen.height - vector.y - 50f, 50f, 50f), (!(Map.blocks[i, j] == null)) ? Map.blocks[i, j].groundType.ToString() : "Empty");
					}
				}
			}
		}
		if (this.showGrid)
		{
			for (int k = 0; k < Map.Width; k++)
			{
				for (int l = 0; l < Map.Height; l++)
				{
					float blocksX2 = Map.GetBlocksX(k);
					float blocksY2 = Map.GetBlocksY(l);
					Vector3 vector2 = Camera.main.WorldToScreenPoint(new Vector3(blocksX2, blocksY2, 0f));
					if (vector2.x > 0f && vector2.x < (float)Screen.width * 5f / 6f && vector2.y > 0f && vector2.y < (float)Screen.height)
					{
						GUI.Label(new Rect(vector2.x, (float)Screen.height - vector2.y - 50f, 50f, 50f), k.ToString() + ", " + l.ToString());
					}
				}
			}
		}
		if (this.showPathfinding)
		{
			if ((this.pathUpdateDelay -= Time.deltaTime) < 0f)
			{
			}
			if (LevelEditorGUI.displayPath != null)
			{
				int num = 1;
				foreach (PathPoint pathPoint in LevelEditorGUI.displayPath.points)
				{
					float blocksX3 = Map.GetBlocksX(pathPoint.collumn);
					float blocksY3 = Map.GetBlocksY(pathPoint.row);
					Vector3 vector3 = Camera.main.WorldToScreenPoint(new Vector3(blocksX3, blocksY3, 0f));
					GUI.Label(new Rect(vector3.x, (float)Screen.height - vector3.y - 50f, 50f, 50f), string.Concat(new object[]
					{
						"p ",
						num,
						" ",
						pathPoint.moveMode.ToString().Substring(0, 1),
						"(",
						pathPoint.fScore,
						")"
					}));
					num++;
				}
			}
		}
	}

	public static object SelectList(ICollection list, object selected, GUIStyle defaultStyle, GUIStyle selectedStyle)
	{
		foreach (object obj in list)
		{
			if (GUILayout.Button(obj.ToString(), ((int)selected != (int)obj) ? defaultStyle : selectedStyle, new GUILayoutOption[0]))
			{
				if (selected == obj)
				{
					selected = null;
				}
				else
				{
					selected = obj;
				}
			}
		}
		return selected;
	}

	private bool OnCheckboxItemGUI(object item, bool selected, ICollection list)
	{
		return GUILayout.Toggle(selected, item.ToString(), new GUILayoutOption[0]);
	}

	public static object SelectList(ICollection list, object selected, LevelEditorGUI.OnListItemGUI itemHandler)
	{
		ArrayList arrayList = new ArrayList(list);
		foreach (object obj in arrayList)
		{
			if (itemHandler(obj, obj == selected, list))
			{
				selected = obj;
			}
			else if (selected == obj)
			{
				selected = null;
			}
		}
		return selected;
	}

	private void OnApplicationFocus(bool focus)
	{
		this.altDown = false;
	}

	public void Update()
	{
		this.CheckKeyboard();
		if (Map.blocks == null)
		{
			return;
		}
		if (!Map.isEditing)
		{
			return;
		}
		if (Application.isEditor && !MainMenu.wasShown)
		{
			LevelEditorGUI.levelEditorActive = true;
		}
		if (Input.GetKeyDown(KeyCode.LeftAlt))
		{
			this.altDown = true;
		}
		else if (Input.GetKeyUp(KeyCode.LeftAlt))
		{
			this.altDown = false;
		}
		LevelEditorGUI.DebugText = string.Concat(new object[]
		{
			DateTime.Now.ToShortTimeString(),
			" -  (",
			this.c,
			", ",
			this.r,
			")"
		});
		if (Input.GetMouseButtonDown(2))
		{
			CameraController.OrthographicSize = 128f;
		}
		if (Input.mousePosition.x > (float)Screen.width * (1f - LevelEditorGUI.controlWidth))
		{
			if (LevelEditorGUI.mode == EditorMode.Triggers && this.selectedTrigger != null)
			{
				this.DrawTriggerAreas();
			}
			if (this.hoverDoodadInfo != null && this.hoverDoodadInfo.entity != null)
			{
				UnityEngine.Object.Destroy(this.hoverDoodadInfo.entity);
			}
			return;
		}
		CameraController.OrthographicSize = Mathf.Clamp(CameraController.OrthographicSize + cInput.GetAxisRaw("MWheel") * 15f, 5f, 1000f);
		Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftShift)) && Input.GetKeyDown(KeyCode.S))
		{
			if (this.fileName == null || this.fileName.ToUpper().Equals("Untitled"))
			{
				LevelEditorGUI.mode = EditorMode.File;
				LevelTitle.ShowText("ENTER LEVEL NAME", 0f);
			}
			else
			{
				this.SaveLevel();
			}
		}
		vector.z = 0f;
		this.c = Map.GetCollumn(vector.x);
		this.r = Map.GetRow(vector.y);
		this.mousePosSelectionIndicator.HighlightSquare(this.r, this.r, this.c, this.c, ((double)Time.time % 0.5 >= 0.25) ? (Color.gray / 2f) : Color.gray, true);
		if (LevelEditorGUI.mode == EditorMode.Tagging)
		{
			this.HandleTagging();
		}
		if (LevelEditorGUI.mode == EditorMode.Doodads)
		{
			this.HandleEditDoodads();
		}
		if (LevelEditorGUI.mode == EditorMode.Triggers)
		{
			this.HandleEditTriggers();
		}
		if (LevelEditorGUI.mode == EditorMode.Terrain)
		{
			if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButtonDown(2))
			{
				this.HandleTerrainEditClick();
			}
			else if (LevelEditorGUI.mode == EditorMode.Terrain && this.placingSquare)
			{
				this.placingSquare = false;
				this.PlaceSquare();
			}
			else if (LevelEditorGUI.mode == EditorMode.Terrain && this.deletingSquare)
			{
				this.deletingSquare = false;
				this.ClearSquare();
			}
			else if (this.hasSelectedSquare)
			{
				if (Time.timeSinceLevelLoad % 0.6f < 0.3f)
				{
					this.DrawSquare();
				}
				if (this.draggingSelection)
				{
					int num = this.c - this.dragStartPoint.collumn;
					int num2 = this.r - this.dragStartPoint.row;
					this.MoveSelection(num, num2);
					this.squareOrigin.collumn = Mathf.Clamp(this.squareOrigin.collumn + num, 0, Map.Width - 1);
					this.squareOrigin.row = Mathf.Clamp(this.squareOrigin.row + num2, 0, Map.Height - 1);
					this.squareEnd.collumn = Mathf.Clamp(this.squareEnd.collumn + num, 0, Map.Width - 1);
					this.squareEnd.row = Mathf.Clamp(this.squareEnd.row + num2, 0, Map.Height - 1);
					this.draggingSelection = false;
				}
			}
			FluidController.RefreshFluidStatus(this.c, this.r);
		}
		else if (LevelEditorGUI.mode == EditorMode.Fluid)
		{
			this.HandleFluidEditClick();
		}
	}

	private void HandleTagging()
	{
		if (!this.settingWaypoint && Input.GetMouseButtonDown(0))
		{
			this.taggedClick.collumn = this.c;
			this.taggedClick.row = this.r;
		}
	}

	private void StartSquare()
	{
		this.placingSquare = true;
		this.squareOrigin = new GridPoint(this.c, this.r);
		this.squareEnd = new GridPoint(this.c, this.r);
	}

	private void UpdateSquare()
	{
		this.squareEnd.collumn = Mathf.Clamp(this.c, 0, Map.Width - 1);
		this.squareEnd.row = Mathf.Clamp(this.r, 0, Map.Height - 1);
		this.squareOrigin.collumn = Mathf.Clamp(this.squareOrigin.collumn, 0, Map.Width - 1);
		this.squareOrigin.row = Mathf.Clamp(this.squareOrigin.row, 0, Map.Height - 1);
		this.DrawSquare();
	}

	private void HandleFluidEditClick()
	{
		if (Input.GetMouseButton(0))
		{
			FluidController.Instance.myJob.waterSimulator.AddFluid(this.c, this.r);
		}
		else if (Input.GetMouseButton(1))
		{
			FluidController.Instance.myJob.waterSimulator.RemoveFluid(this.c, this.r);
		}
	}

	private void HandleTerrainEditClick()
	{
		if (Input.GetMouseButtonDown(2))
		{
			if (Map.MapData.foregroundBlocks[this.c, this.r] != TerrainType.Empty)
			{
				this.curTerrainType = Map.MapData.foregroundBlocks[this.c, this.r];
			}
			else
			{
				this.curTerrainType = Map.MapData.backgroundBlocks[this.c, this.r];
			}
		}
		if (this.hasSelectedSquare && !this.altDown)
		{
			if (Input.GetMouseButtonDown(0))
			{
				this.dragStartPoint = new GridPoint(this.c, this.r);
			}
			else if (Input.GetMouseButton(0))
			{
				int colOffset = this.c - this.dragStartPoint.collumn;
				int rowOffset = this.r - this.dragStartPoint.row;
				this.DrawSquare(colOffset, rowOffset);
				this.draggingSelection = true;
			}
			else if (Input.GetMouseButtonDown(1))
			{
				this.hasSelectedSquare = false;
				this.draggingSelection = false;
			}
		}
		else if (Input.GetMouseButton(0))
		{
			if (this.altDown)
			{
				if (Input.GetMouseButtonDown(0))
				{
					if (Map.MapData.foregroundBlocks[this.c, this.r] != TerrainType.Empty)
					{
						this.curTerrainType = Map.MapData.foregroundBlocks[this.c, this.r];
					}
					else
					{
						this.curTerrainType = Map.MapData.backgroundBlocks[this.c, this.r];
					}
					this.squareOrigin = new GridPoint(this.c, this.r);
					this.squareEnd = new GridPoint(this.c, this.r);
					return;
				}
				if (this.c == this.squareOrigin.collumn && this.r == this.squareOrigin.row)
				{
					return;
				}
				this.placingSquare = true;
			}
			if (Input.GetMouseButtonDown(0) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftControl) || this.altDown))
			{
				this.StartSquare();
				return;
			}
			if (this.placingSquare)
			{
				this.UpdateSquare();
				return;
			}
			LevelEditorGUI.PlaceGround(this.c, this.r, this.curTerrainType);
			this.RefreshBlock(this.c, this.r);
		}
		else if (Input.GetMouseButton(1))
		{
			if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift))
			{
				this.deletingSquare = true;
				this.squareOrigin = new GridPoint(this.c, this.r);
				this.squareEnd = new GridPoint(this.c, this.r);
				return;
			}
			if (this.deletingSquare)
			{
				this.UpdateSquare();
				return;
			}
			if (Input.GetMouseButtonDown(1))
			{
				for (int i = 0; i < this.gridClicked.GetLength(0); i++)
				{
					for (int j = 0; j < this.gridClicked.GetLength(1); j++)
					{
						this.gridClicked[i, j] = false;
					}
				}
				this.destroyingForegroundThisClick = (Map.blocks[this.c, this.r] != null);
			}
			if (!this.gridClicked[this.c, this.r])
			{
				this.gridClicked[this.c, this.r] = true;
				if (Map.blocks[this.c, this.r] != null && this.destroyingForegroundThisClick)
				{
					Map.MapData.foregroundBlocks[this.c, this.r] = TerrainType.Empty;
					if (Map.MapData.backgroundBlocks[this.c, this.r] == TerrainType.Empty)
					{
					}
					Map.ClearForegroundBlock(this.c, this.r);
					this.RefreshBlock(this.c, this.r);
				}
				else if (!this.destroyingForegroundThisClick)
				{
					Map.ClearBackgroundBlock(this.c, this.r);
					Map.MapData.backgroundBlocks[this.c, this.r] = TerrainType.Empty;
					this.RefreshBlock(this.c, this.r);
				}
			}
		}
	}

	private void RefreshBlock(int c, int r)
	{
		for (int i = c - 1; i <= c + 1; i++)
		{
			for (int j = r - 1; j <= r + 1; j++)
			{
				if (i >= 0 && i < Map.Width && j >= 0 && j < Map.Height)
				{
					if (Map.MapData.foregroundBlocks[i, j] != TerrainType.Empty)
					{
						Map.ClearForegroundBlock(i, j);
						Map.Instance.PlaceGround(Map.MapData.GetGroundType(Map.MapData.foregroundBlocks, i, j), i, j, ref Map.blocks);
					}
					if (Map.MapData.backgroundBlocks[i, j] != TerrainType.Empty)
					{
						Map.ClearBackgroundBlock(i, j);
						Map.Instance.PlaceGround(Map.MapData.GetGroundType(Map.MapData.backgroundBlocks, i, j), i, j, ref Map.backGroundBlocks);
					}
				}
			}
		}
	}

	public static void PlaceGround(int c, int r, TerrainType terrainType)
	{
		if (terrainType == TerrainType.Empty)
		{
			if (Map.IsBlockSolid(c, r))
			{
			}
			Map.ClearForegroundBlock(c, r);
			Map.ClearBackgroundBlock(c, r);
			Map.MapData.foregroundBlocks[c, r] = terrainType;
			Map.MapData.backgroundBlocks[c, r] = terrainType;
		}
		else if (terrainType == TerrainType.PropaneBarrel && r < Map.MapData.foregroundBlocks.GetUpperBound(1))
		{
			Map.MapData.foregroundBlocks[c, r] = terrainType;
			Map.MapData.foregroundBlocks[c, r + 1] = TerrainType.Empty;
			Map.ClearForegroundBlock(c, r);
			Map.ClearForegroundBlock(c, r + 1);
			Map.Instance.PlaceGround(Map.MapData.GetGroundType(Map.MapData.foregroundBlocks, c, r), c, r, ref Map.blocks);
		}
		else if (MapData.IsForegroundType(terrainType))
		{
			Map.MapData.foregroundBlocks[c, r] = terrainType;
			Map.ClearForegroundBlock(c, r);
			Map.Instance.PlaceGround(Map.MapData.GetGroundType(Map.MapData.foregroundBlocks, c, r), c, r, ref Map.blocks);
		}
		else
		{
			Map.MapData.backgroundBlocks[c, r] = terrainType;
			if (Map.MapData.foregroundBlocks[c, r] != TerrainType.Ladder && Map.MapData.foregroundBlocks[c, r] != TerrainType.Barrel && Map.MapData.foregroundBlocks[c, r] != TerrainType.PropaneBarrel && Map.MapData.foregroundBlocks[c, r] != TerrainType.BuriedRocket)
			{
				Map.MapData.foregroundBlocks[c, r] = TerrainType.Empty;
				Map.ClearForegroundBlock(c, r);
			}
			Map.ClearBackgroundBlock(c, r);
			Map.Instance.PlaceGround(Map.MapData.GetGroundType(Map.MapData.backgroundBlocks, c, r), c, r, ref Map.backGroundBlocks);
		}
	}

	private void HandleEditDoodads()
	{
		DoodadInfo doodadInfo = Map.MapData.DoodadList.FirstOrDefault((DoodadInfo doodinfo) => doodinfo.position.c == this.c && doodinfo.position.r == this.r);
		if ((Input.GetMouseButtonDown(2) || (Input.GetMouseButtonDown(0) && this.altDown)) && doodadInfo != null)
		{
			this.curDoodadType = doodadInfo.type;
			this.curDoodadVariation = doodadInfo.variation;
			return;
		}
		if (this.curDoodadType != DoodadType.Empty && (this.lastMouseHoverPoint.collumn != this.c || this.lastMouseHoverPoint.row != this.r))
		{
			this.PlaceHoverDoodad();
		}
		if (Input.GetMouseButtonDown(0) && this.hoverDoodadInfo != null && this.hoverDoodadInfo.type != DoodadType.Empty)
		{
			if (this.curDoodadVariation < Map.Instance.GetDoodadVariationAmount(this.curDoodadType))
			{
				Map.MapData.DoodadList.Add(this.hoverDoodadInfo);
				this.hoverDoodadInfo = new DoodadInfo(new GridPos(this.c, this.r), this.curDoodadType, this.curDoodadVariation);
			}
			else
			{
				this.invalidVariationObject.SetActive(true);
				this.invalidVariationObject.transform.position = Map.GetBlocksXYPosition(this.c, this.r);
			}
		}
		else if (Input.GetMouseButtonDown(1) && doodadInfo != null)
		{
			UnityEngine.Object.Destroy(doodadInfo.entity);
			this.mapData.DoodadList.Remove(doodadInfo);
		}
	}

	private void PlaceHoverDoodad()
	{
		this.lastMouseHoverPoint.row = this.r;
		this.lastMouseHoverPoint.collumn = this.c;
		if (this.curDoodadVariation < Map.Instance.GetDoodadVariationAmount(this.curDoodadType))
		{
			this.invalidVariationObject.SetActive(false);
			if (this.hoverDoodadInfo == null)
			{
				this.hoverDoodadInfo = new DoodadInfo(new GridPos(this.c, this.r), this.curDoodadType, this.curDoodadVariation);
			}
			if (this.hoverDoodadInfo.entity != null)
			{
				this.hoverDoodadInfo.entity.SendMessage("HoverDoodadBeingDestroyed", SendMessageOptions.DontRequireReceiver);
				UnityEngine.Object.Destroy(this.hoverDoodadInfo.entity);
			}
			this.hoverDoodadInfo.position.r = this.r;
			this.hoverDoodadInfo.position.c = this.c;
			this.hoverDoodadInfo.type = this.curDoodadType;
			Map.Instance.PlaceDoodad(this.hoverDoodadInfo);
			if (this.hoverDoodadInfo.type == DoodadType.Zipline)
			{
				Map.Instance.Invoke("ResetZiplines", 0.01f);
			}
		}
		else
		{
			if (this.hoverDoodadInfo != null && this.hoverDoodadInfo.entity != null)
			{
				UnityEngine.Object.Destroy(this.hoverDoodadInfo.entity);
			}
			this.invalidVariationObject.transform.position = Map.GetBlocksXYPosition(this.c, this.r);
			this.invalidVariationObject.SetActive(true);
		}
	}

	public void SaveLevel()
	{
		PlayerOptions.Instance.LastCustomLevel = this.fileName;
		FileIO.SaveToBFLCompressed(Map.MapData, this.fileName);
		LevelTitle.ShowText("LEVEL SAVED", 0f);
	}

	private void DrawSquare()
	{
		this.DrawSquare(0, 0);
	}

	private void DrawSquare(int colOffset, int rowOffset)
	{
		if (!this.activeSelectionIndicator.enabled)
		{
			this.activeSelectionIndicator.enabled = true;
		}
		int num = Mathf.Min(this.squareOrigin.collumn, this.squareEnd.collumn) + colOffset;
		int num2 = Mathf.Max(this.squareOrigin.collumn, this.squareEnd.collumn) + colOffset;
		int num3 = Mathf.Max(this.squareOrigin.row, this.squareEnd.row) + rowOffset;
		int num4 = Mathf.Min(this.squareOrigin.row, this.squareEnd.row) + rowOffset;
		if (num < 0)
		{
			num = 0;
		}
		if (num2 >= Map.Width - 1)
		{
			num2 = Map.Width - 2;
		}
		if (num3 >= Map.Height)
		{
			num3 = Map.Height - 1;
		}
		if (num4 <= 0)
		{
			num4 = 0;
		}
		this.activeSelectionIndicator.HighlightSquare(num3, num4, num, num2, Color.white, true);
	}

	private void PlaceSquare()
	{
		int num = Mathf.Min(this.squareOrigin.collumn, this.squareEnd.collumn);
		int num2 = Mathf.Max(this.squareOrigin.collumn, this.squareEnd.collumn);
		int num3 = Mathf.Max(this.squareOrigin.row, this.squareEnd.row);
		int num4 = Mathf.Min(this.squareOrigin.row, this.squareEnd.row);
		if (num < 0)
		{
			num = 0;
		}
		if (num2 >= Map.Width - 1)
		{
			num2 = Map.Width - 2;
		}
		if (num3 >= Map.Height)
		{
			num3 = Map.Height - 1;
		}
		if (num4 <= 0)
		{
			num4 = 0;
		}
		if (this.curTerrainType == TerrainType.BigBlock)
		{
			return;
		}
		if (Input.GetKey(KeyCode.LeftShift))
		{
			for (int i = num; i <= num2; i++)
			{
				for (int j = num4; j <= num3; j++)
				{
					LevelEditorGUI.PlaceGround(i, j, this.curTerrainType);
				}
			}
		}
		else if (Input.GetKey(KeyCode.LeftControl))
		{
			for (int k = num; k <= num2; k++)
			{
				for (int l = num4; l <= num3; l++)
				{
					if (k == num || k == num2 || l == num4 || l == num3)
					{
						LevelEditorGUI.PlaceGround(k, l, this.curTerrainType);
					}
					else
					{
						LevelEditorGUI.PlaceGround(k, l, TerrainType.Empty);
					}
				}
			}
		}
		else if (this.altDown)
		{
			this.hasSelectedSquare = true;
		}
	}

	private void ClearSquare()
	{
		int num = Mathf.Min(this.squareOrigin.collumn, this.squareEnd.collumn);
		int num2 = Mathf.Max(this.squareOrigin.collumn, this.squareEnd.collumn);
		int num3 = Mathf.Max(this.squareOrigin.row, this.squareEnd.row);
		int num4 = Mathf.Min(this.squareOrigin.row, this.squareEnd.row);
		foreach (DoodadInfo doodadInfo in Map.MapData.DoodadList.ToArray())
		{
			if (doodadInfo.position.c >= num && doodadInfo.position.c <= num2 && doodadInfo.position.r >= num4 && doodadInfo.position.r <= num3)
			{
				Map.MapData.DoodadList.Remove(doodadInfo);
				if (doodadInfo.entity != null)
				{
					UnityEngine.Object.Destroy(doodadInfo.entity);
				}
			}
		}
		if (Input.GetKey(KeyCode.LeftShift))
		{
			for (int j = num; j <= num2; j++)
			{
				for (int k = num4; k <= num3; k++)
				{
					Map.ClearForegroundBlock(j, k);
					Map.ClearBackgroundBlock(j, k);
					Map.MapData.foregroundBlocks[j, k] = TerrainType.Empty;
					Map.MapData.backgroundBlocks[j, k] = TerrainType.Empty;
					if (j == num || j == num2 || k == num4 || k == num3)
					{
						this.RefreshBlock(j, k);
					}
				}
			}
		}
	}

	public void LoadCampaign()
	{
		PlayerOptions.Instance.LastCustomLevel = this.fileName;
		try
		{
			LevelEditorGUI.campaign = FileIO.LoadCampaignFromDisk(this.fileName);
			LevelSelectionController.loadMode = MapLoadMode.LoadFromMapdata;
			LevelSelectionController.MapDataToLoad = LevelEditorGUI.campaign.levels[0];
			MonoBehaviour.print("Level Editor LoadCampaign  " + LevelSelectionController.loadMode);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
			LevelSelectionController.MapDataToLoad = FileIO.LoadLevelFromDisk(this.fileName);
			LevelSelectionController.loadMode = MapLoadMode.LoadFromMapdata;
			MonoBehaviour.print("Level Editor LoadCampaign 2  " + LevelSelectionController.loadMode);
		}
		Application.LoadLevel(Application.loadedLevel);
	}

	public void AdditiveLoadCampaign()
	{
		PlayerOptions.Instance.LastCustomLevel = this.fileName;
		try
		{
			Campaign campaign = FileIO.LoadCampaignFromDisk(this.fileName);
			List<MapData> list = new List<MapData>();
			list.AddRange(LevelEditorGUI.campaign.levels);
			list.AddRange(campaign.levels);
			LevelEditorGUI.campaign.levels = list.ToArray();
			LevelTitle.ShowText("LEVELS ADDED TO CURRENT CAMPAIGN", 0f);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	private void ResizeMap(int newWidth, int newHeight)
	{
		TerrainType[,] array = new TerrainType[this.newMapWidth, newHeight];
		TerrainType[,] array2 = new TerrainType[this.newMapWidth, this.newMapHeight];
		int num = 0;
		while (num < this.newMapWidth && num < this.mapData.Width)
		{
			int num2 = 0;
			while (num2 < this.newMapHeight && num2 < this.mapData.Height)
			{
				array[num, num2] = this.mapData.foregroundBlocks[num, num2];
				array2[num, num2] = this.mapData.backgroundBlocks[num, num2];
				num2++;
			}
			num++;
		}
		foreach (DoodadInfo doodadInfo in this.mapData.DoodadList.ToArray())
		{
			if (doodadInfo.position.c >= this.newMapWidth || doodadInfo.position.r >= this.newMapHeight)
			{
				this.mapData.DoodadList.Remove(doodadInfo);
			}
		}
		this.mapData.Width = this.newMapWidth;
		this.mapData.Height = this.newMapHeight;
		this.mapData.foregroundBlocks = array;
		this.mapData.backgroundBlocks = array2;
	}

	private void SetBottomRowsToEarth(MapData mapData)
	{
		for (int i = 0; i < mapData.Width; i++)
		{
			for (int j = 0; j < mapData.Height; j++)
			{
				if (j <= 3)
				{
					mapData.foregroundBlocks[i, j] = TerrainType.Earth;
				}
			}
		}
	}

	private void RefreshFiles()
	{
		if (this.files == null)
		{
			this.files = new List<string>();
		}
		this.files.Clear();
		this.files.AddRange(FileIO.FindCampaignFiles());
	}

	private void DrawDoodadUIInfo()
	{
		foreach (DoodadInfo doodadInfo in this.mapData.DoodadList)
		{
			float blocksX = Map.GetBlocksX(doodadInfo.position.c);
			float blocksY = Map.GetBlocksY(doodadInfo.position.r);
			Vector3 vector = Camera.main.WorldToScreenPoint(new Vector3(blocksX, blocksY, 0f));
			if ((LevelEditorGUI.showAllDoodads && vector.x > 0f && vector.x < (float)Screen.width * 5f / 6f && vector.y > 0f && vector.y < (float)Screen.height) || Mathf.Abs(doodadInfo.position.c - this.c) + Mathf.Abs(doodadInfo.position.r - this.r) < 5)
			{
				GUI.Label(new Rect(vector.x, (float)Screen.height - vector.y - 50f, 50f, 50f), string.Concat(new string[]
				{
					doodadInfo.type.ToString(),
					"\n(",
					(doodadInfo.variation != -1) ? doodadInfo.variation.ToString() : "R",
					")",
					(!string.IsNullOrEmpty(doodadInfo.tag)) ? ("\n(" + doodadInfo.tag + ")") : string.Empty
				}));
			}
		}
	}

	protected void TogglePlayMode()
	{
		if (Map.isEditing)
		{
			this.showGUI = false;
			Map.isEditing = !Map.isEditing;
			if (this.hoverDoodadInfo != null && this.hoverDoodadInfo.entity != null)
			{
				UnityEngine.Object.Destroy(this.hoverDoodadInfo.entity);
			}
			TriggerManager.LoadTriggers(this.mapData.TriggerList);
			foreach (Switch @switch in Map.switches)
			{
				@switch.ResetTrigger();
			}
			ShowMouseController.ForceMouse = false;
			Brotalitometer.Reset();
		}
		else
		{
			Map.isEditing = !Map.isEditing;
			LevelSelectionController.loadMode = MapLoadMode.LoadFromMapdata;
			LevelSelectionController.MapDataToLoad = Map.MapData;
			LevelEditorGUI.lastCameraPos = SortOfFollow.instance.transform.position;
			ShowMouseController.ForceMouse = true;
			MonoBehaviour.print("Sitrs s  sr to edit");
			Brotalitometer.Reset();
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	protected void CheckKeyboard()
	{
		if (Input.GetKeyDown(KeyCode.F2))
		{
			this.showGUI = !this.showGUI;
		}
		if (Input.GetKeyDown(KeyCode.F3))
		{
			this.TogglePlayMode();
		}
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			LevelEditorGUI.NoClip = !LevelEditorGUI.NoClip;
		}
	}

	private void MoveSelection(int cols, int rows)
	{
		int num = Mathf.Min(this.squareOrigin.collumn, this.squareEnd.collumn);
		int num2 = Mathf.Max(this.squareOrigin.collumn, this.squareEnd.collumn);
		int num3 = Mathf.Max(this.squareOrigin.row, this.squareEnd.row);
		int num4 = Mathf.Min(this.squareOrigin.row, this.squareEnd.row);
		TerrainType[,] array = new TerrainType[num2 - num + 1, num3 - num4 + 1];
		TerrainType[,] array2 = new TerrainType[num2 - num + 1, num3 - num4 + 1];
		for (int i = num; i <= num2; i++)
		{
			for (int j = num4; j <= num3; j++)
			{
				array[i - num, j - num4] = Map.MapData.foregroundBlocks[i, j];
				array2[i - num, j - num4] = Map.MapData.backgroundBlocks[i, j];
				if (!Input.GetKey(KeyCode.LeftShift))
				{
					Map.MapData.backgroundBlocks[i, j] = TerrainType.Empty;
					Map.MapData.foregroundBlocks[i, j] = TerrainType.Empty;
					Map.ClearForegroundBlock(i, j);
					Map.ClearBackgroundBlock(i, j);
				}
			}
		}
		for (int k = num; k <= num2; k++)
		{
			for (int l = num4; l <= num3; l++)
			{
				if (k + cols >= 0 && k + cols < Map.Width && l + rows >= 0 && l + rows < Map.Height)
				{
					Map.MapData.foregroundBlocks[k + cols, l + rows] = array[k - num, l - num4];
					Map.MapData.backgroundBlocks[k + cols, l + rows] = array2[k - num, l - num4];
					this.RefreshBlock(k + cols, l + rows);
				}
			}
		}
		foreach (DoodadInfo doodadInfo in Map.MapData.DoodadList.ToArray())
		{
			if (doodadInfo.position.c >= num && doodadInfo.position.c <= num2 && doodadInfo.position.r >= num4 && doodadInfo.position.r <= num3)
			{
				if (!Input.GetKey(KeyCode.LeftShift))
				{
					UnityEngine.Object.Destroy(doodadInfo.entity);
					if (Map.IsWithinBounds(doodadInfo.position.c + cols, doodadInfo.position.r + rows))
					{
						DoodadInfo doodadInfo2 = doodadInfo;
						doodadInfo2.position.c = doodadInfo2.position.c + cols;
						DoodadInfo doodadInfo3 = doodadInfo;
						doodadInfo3.position.r = doodadInfo3.position.r + rows;
						Map.Instance.PlaceDoodad(doodadInfo);
					}
					else
					{
						Map.MapData.DoodadList.Remove(doodadInfo);
					}
				}
				else if (Map.IsWithinBounds(doodadInfo.position.c + cols, doodadInfo.position.r + rows))
				{
					DoodadInfo doodadInfo4 = new DoodadInfo(new GridPos(doodadInfo.position.c + cols, doodadInfo.position.r + rows), doodadInfo.type, doodadInfo.variation);
					Map.Instance.PlaceDoodad(doodadInfo4);
					Map.MapData.DoodadList.Add(doodadInfo4);
				}
			}
		}
		Map.Instance.ResetZiplines();
	}

	private void StartPublishRun()
	{
		GameModeController.publishRun = true;
		LevelEditorGUI.levelEditorActive = false;
		LevelSelectionController.currentCampaign = LevelEditorGUI.campaign;
		LevelSelectionController.CurrentLevelNum = 0;
		LevelSelectionController.loadMode = MapLoadMode.Campaign;
		Application.LoadLevel(Application.loadedLevel);
	}

	protected void HandleEditTriggers()
	{
		if (this.selectedTrigger != null)
		{
			if (this.selectedTrigger.type == TriggerType.Area || this.selectedTrigger.type == TriggerType.CheckTerrain || this.selectedTrigger.type == TriggerType.OnScreen)
			{
				if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
				{
					this.StartSquare();
				}
				else if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift))
				{
					this.UpdateSquare();
				}
				else if (this.placingSquare && Input.GetKey(KeyCode.LeftShift))
				{
					this.SetTriggerArea();
				}
			}
			if (Input.GetMouseButtonDown(0) && this.settingWaypoint)
			{
				this.settingWaypoint = false;
				this.waypointToSet.collumn = Mathf.Clamp(this.c, 0, this.mapData.Width - 1);
				this.waypointToSet.row = Mathf.Clamp(this.r, 0, this.mapData.Height - 1);
			}
		}
		this.DrawTriggerAreas();
	}

	private void SetTriggerArea()
	{
		if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
		{
			return;
		}
		int num = Mathf.Min(this.squareOrigin.collumn, this.squareEnd.collumn);
		int num2 = Mathf.Max(this.squareOrigin.collumn, this.squareEnd.collumn);
		int num3 = Mathf.Max(this.squareOrigin.row, this.squareEnd.row);
		int num4 = Mathf.Min(this.squareOrigin.row, this.squareEnd.row);
		if (num < 0)
		{
			num = 0;
		}
		if (num2 >= Map.Width - 1)
		{
			num2 = Map.Width - 2;
		}
		if (num3 >= Map.Height)
		{
			num3 = Map.Height - 1;
		}
		if (num4 <= 0)
		{
			num4 = 0;
		}
		this.selectedTrigger.bottomLeft.collumn = num;
		this.selectedTrigger.bottomLeft.row = num4;
		this.selectedTrigger.upperRight.collumn = num2;
		this.selectedTrigger.upperRight.row = num3;
	}

	private void DrawTriggerAreas()
	{
		TriggerInfo trig;
		foreach (TriggerInfo trig2 in this.mapData.TriggerList)
		{
			trig = trig2;
			if (trig.type == TriggerType.Area || trig.type == TriggerType.CheckTerrain || trig.type == TriggerType.OnScreen)
			{
				SelectionIndicator selectionIndicator = this.selectionIndicators.FirstOrDefault((SelectionIndicator si) => si.associatedObject == trig);
				if (selectionIndicator == null)
				{
					selectionIndicator = this.selectionIndicators.FirstOrDefault((SelectionIndicator si) => !si.enabled);
					if (selectionIndicator == null)
					{
						selectionIndicator = (UnityEngine.Object.Instantiate(this.SelectionIndicatorPrefab) as SelectionIndicator);
						this.selectionIndicators.Add(selectionIndicator);
					}
				}
				Color color = (trig != this.selectedTrigger) ? Color.grey : Color.yellow;
				selectionIndicator.associatedObject = trig;
				selectionIndicator.HighlightSquare(trig.upperRight.row, trig.bottomLeft.row, trig.bottomLeft.collumn, trig.upperRight.collumn, color, true);
			}
		}
	}

	protected void ShowTriggerMenu()
	{
		LevelEditorGUI.scrollPos = GUILayout.BeginScrollView(LevelEditorGUI.scrollPos, LevelEditorGUI.GetGuiSkin().scrollView);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (GUILayout.Button("New Trigger", new GUILayoutOption[0]))
		{
			this.selectedTrigger = new TriggerInfo();
			this.mapData.TriggerList.Add(this.selectedTrigger);
		}
		if (this.selectedTrigger != null)
		{
			if (GUILayout.Button("Deselect Trigger", new GUILayoutOption[0]))
			{
				this.selectedTrigger = null;
				this.selectedAction = null;
			}
			if (GUILayout.Button("Delete", new GUILayoutOption[0]))
			{
				this.mapData.TriggerList.Remove(this.selectedTrigger);
				this.selectedTrigger = null;
			}
		}
		GUILayout.EndHorizontal();
		if (this.selectedTrigger == null)
		{
			foreach (TriggerInfo triggerInfo in this.mapData.TriggerList)
			{
				if (GUILayout.Button((triggerInfo.name ?? "Unnamed") + " (" + triggerInfo.type.ToString() + ")", new GUILayoutOption[0]))
				{
					this.selectedTrigger = triggerInfo;
				}
			}
			GUILayout.Label("Entity Triggers:", new GUILayoutOption[0]);
			foreach (TriggerInfo triggerInfo2 in this.mapData.entityTriggers)
			{
				if (GUILayout.Button((triggerInfo2.name ?? "Unnamed") + " (" + triggerInfo2.type.ToString() + ")", new GUILayoutOption[0]))
				{
					this.selectedTrigger = triggerInfo2;
				}
			}
		}
		else
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Editing Trigger:", new GUILayoutOption[0]);
			this.selectedTrigger.name = GUILayout.TextField(this.selectedTrigger.name ?? string.Empty, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			this.selectedTrigger.startEnabled = GUILayout.Toggle(this.selectedTrigger.startEnabled, "Start Enabled", new GUILayoutOption[0]);
			if (this.selectedAction == null)
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (GUILayout.Button("Deselect", new GUILayoutOption[0]))
				{
					this.selectedTrigger = null;
					this.selectedAction = null;
					return;
				}
				if (GUILayout.Button("Delete", new GUILayoutOption[0]))
				{
					this.mapData.TriggerList.Remove(this.selectedTrigger);
					this.selectedTrigger = null;
					return;
				}
				GUILayout.EndHorizontal();
				GUILayout.Label("Type:", new GUILayoutOption[0]);
				this.selectedTrigger.type = (TriggerType)((int)LevelEditorGUI.SelectList(this.triggerTypes, this.selectedTrigger.type, this.skin.label, this.skin.button));
				if (this.selectedTrigger.type == TriggerType.Area || this.selectedTrigger.type == TriggerType.CheckTerrain || this.selectedTrigger.type == TriggerType.OnScreen)
				{
					if (this.selectedTrigger.bottomLeft == null)
					{
						this.selectedTrigger.bottomLeft = new GridPoint(0, 0);
					}
					if (this.selectedTrigger.upperRight == null)
					{
						this.selectedTrigger.upperRight = new GridPoint(0, 0);
					}
					GUILayout.Label(string.Concat(new object[]
					{
						"Area: ",
						this.selectedTrigger.bottomLeft.collumn,
						", ",
						this.selectedTrigger.bottomLeft.row,
						" -> ",
						this.selectedTrigger.upperRight.collumn,
						", ",
						this.selectedTrigger.upperRight.row
					}), new GUILayoutOption[0]);
					GUILayout.Label("Hold LEFT SHIFT and drag to select area", new GUILayoutOption[0]);
				}
				else if (this.selectedTrigger.type == TriggerType.Variable)
				{
					GUILayout.Label("Variable Identifier:", new GUILayoutOption[0]);
					this.selectedTrigger.variableName = GUILayout.TextField(this.selectedTrigger.variableName, 24, new GUILayoutOption[0]);
					GUILayout.Label("Must Be Equal To Or Greater Than:", new GUILayoutOption[0]);
					float.TryParse(GUILayout.TextField(this.selectedTrigger.evaluateAgainstValue.ToString(), new GUILayoutOption[0]), out this.selectedTrigger.evaluateAgainstValue);
				}
				else if (this.selectedTrigger.type == TriggerType.Entity)
				{
					GUILayout.Label("This trigger will be activated when ANY of the doodads tagged with this same tag are killed/destroyed", new GUILayoutOption[0]);
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					this.selectedTrigger.tag = GUILayout.TextField(this.selectedTrigger.tag ?? string.Empty, new GUILayoutOption[0]);
					GUILayout.EndHorizontal();
				}
				else if (this.selectedTrigger.type == TriggerType.EnemyDeath)
				{
					GUILayout.Label("Enemy Death Frequency (how many deaths before triggering):", new GUILayoutOption[0]);
					int.TryParse(GUILayout.TextField(this.selectedTrigger.enemyDeathFrequency.ToString(), new GUILayoutOption[0]), out this.selectedTrigger.enemyDeathFrequency);
				}
				bool flag = GUILayout.Toggle(this.selectedTrigger.useDefaultBrotality, "Use Default Brotality to Limit Trigger: ", new GUILayoutOption[0]);
				bool flag2 = GUILayout.Toggle(this.selectedTrigger.useCustomBrotality, "Use Custom Brotality to Limit Trigger: ", new GUILayoutOption[0]);
				if (flag2 && !this.selectedTrigger.useCustomBrotality)
				{
					this.selectedTrigger.useDefaultBrotality = false;
					this.selectedTrigger.useCustomBrotality = true;
				}
				else if (flag && !this.selectedTrigger.useDefaultBrotality)
				{
					this.selectedTrigger.useDefaultBrotality = true;
					this.selectedTrigger.useCustomBrotality = false;
				}
				if (this.selectedTrigger.useCustomBrotality)
				{
					GUILayout.Label("Brotality must be at least at level: (0 for always) to trigger", new GUILayoutOption[0]);
					int.TryParse(GUILayout.TextField(this.selectedTrigger.minBrotalityLevel.ToString(), new GUILayoutOption[0]), out this.selectedTrigger.minBrotalityLevel);
				}
			}
			for (int i = 0; i < this.targetSelectionIndicatorOthers.Length; i++)
			{
				if (this.selectedTrigger.actions.Count > i)
				{
					BombardmentActionInfo bombardmentActionInfo = this.selectedTrigger.actions[i] as BombardmentActionInfo;
					if (bombardmentActionInfo != null && bombardmentActionInfo.targetPoint.row > 0 && bombardmentActionInfo.targetPoint.collumn > 0)
					{
						this.targetSelectionIndicatorOthers[i].HighlightSquare(bombardmentActionInfo.targetPoint.row, bombardmentActionInfo.targetPoint.row, bombardmentActionInfo.targetPoint.collumn, bombardmentActionInfo.targetPoint.collumn, (this.selectedAction != this.selectedTrigger.actions[i]) ? Color.blue : Color.red, false);
					}
				}
				else
				{
					this.targetSelectionIndicatorOthers[i].UnHighlightSquare();
				}
			}
			if (this.selectedAction == null)
			{
				if (GUILayout.Button("Add New Camera Action", new GUILayoutOption[0]))
				{
					CameraActionInfo cameraActionInfo = new CameraActionInfo();
					cameraActionInfo.type = TriggerActionType.CameraMove;
					this.selectedTrigger.actions.Add(cameraActionInfo);
					this.selectedAction = cameraActionInfo;
				}
				if (GUILayout.Button("Add New Splosion Action", new GUILayoutOption[0]))
				{
					ExplosionActionInfo explosionActionInfo = new ExplosionActionInfo();
					explosionActionInfo.type = TriggerActionType.Explosion;
					this.selectedTrigger.actions.Add(explosionActionInfo);
					this.selectedAction = explosionActionInfo;
				}
				if (GUILayout.Button("Add New Collapse Action", new GUILayoutOption[0]))
				{
					CollapseActionInfo collapseActionInfo = new CollapseActionInfo();
					collapseActionInfo.type = TriggerActionType.Collapse;
					this.selectedTrigger.actions.Add(collapseActionInfo);
					this.selectedAction = collapseActionInfo;
				}
				if (GUILayout.Button("Add New Burn Action", new GUILayoutOption[0]))
				{
					BurnActionInfo burnActionInfo = new BurnActionInfo();
					burnActionInfo.type = TriggerActionType.Burn;
					this.selectedTrigger.actions.Add(burnActionInfo);
					this.selectedAction = burnActionInfo;
				}
				if (GUILayout.Button("Add New Spawn Resource Action", new GUILayoutOption[0]))
				{
					SpawnResourceActionInfo spawnResourceActionInfo = new SpawnResourceActionInfo();
					spawnResourceActionInfo.type = TriggerActionType.SpawnResource;
					this.selectedTrigger.actions.Add(spawnResourceActionInfo);
					this.selectedAction = spawnResourceActionInfo;
				}
				if (GUILayout.Button("Add New Spawn Mooks Action", new GUILayoutOption[0]))
				{
					SpawnMooksActionInfo spawnMooksActionInfo = new SpawnMooksActionInfo();
					spawnMooksActionInfo.type = TriggerActionType.SpawnMooks;
					this.selectedTrigger.actions.Add(spawnMooksActionInfo);
					this.selectedAction = spawnMooksActionInfo;
				}
				if (GUILayout.Button("Add New Bombardment Action", new GUILayoutOption[0]))
				{
					BombardmentActionInfo bombardmentActionInfo2 = new BombardmentActionInfo();
					bombardmentActionInfo2.type = TriggerActionType.Bombardment;
					this.selectedTrigger.actions.Add(bombardmentActionInfo2);
					this.selectedAction = bombardmentActionInfo2;
				}
				if (GUILayout.Button("Add New Level Event Action", new GUILayoutOption[0]))
				{
					LevelEventActionInfo levelEventActionInfo = new LevelEventActionInfo();
					levelEventActionInfo.type = TriggerActionType.LevelEvent;
					this.selectedTrigger.actions.Add(levelEventActionInfo);
					this.selectedAction = levelEventActionInfo;
				}
				if (GUILayout.Button("Add New Spawn Block Action", new GUILayoutOption[0]))
				{
					SpawnBlockActionInfo spawnBlockActionInfo = new SpawnBlockActionInfo();
					spawnBlockActionInfo.type = TriggerActionType.SpawnBlock;
					this.selectedTrigger.actions.Add(spawnBlockActionInfo);
					this.selectedAction = spawnBlockActionInfo;
				}
				if (GUILayout.Button("Add New Variable Action", new GUILayoutOption[0]))
				{
					VariableActionInfo variableActionInfo = new VariableActionInfo();
					variableActionInfo.type = TriggerActionType.Variable;
					this.selectedTrigger.actions.Add(variableActionInfo);
					this.selectedAction = variableActionInfo;
				}
				if (GUILayout.Button("Add New Weather Action", new GUILayoutOption[0]))
				{
					WeatherActionInfo weatherActionInfo = new WeatherActionInfo();
					weatherActionInfo.type = TriggerActionType.Weather;
					this.selectedTrigger.actions.Add(weatherActionInfo);
					this.selectedAction = weatherActionInfo;
				}
				if (GUILayout.Button("Add New Character Command Action", new GUILayoutOption[0]))
				{
					CharacterActionInfo characterActionInfo = new CharacterActionInfo();
					characterActionInfo.type = TriggerActionType.Character;
					this.selectedTrigger.actions.Add(characterActionInfo);
					this.selectedAction = characterActionInfo;
				}
				if (Application.isEditor && GUILayout.Button("Add New Execute Function Action", new GUILayoutOption[0]))
				{
					ExecuteFunctionActionInfo executeFunctionActionInfo = new ExecuteFunctionActionInfo();
					executeFunctionActionInfo.type = TriggerActionType.ExecuteFunction;
					this.selectedTrigger.actions.Add(executeFunctionActionInfo);
					this.selectedAction = executeFunctionActionInfo;
				}
				GUILayout.Label("Current Actions:", new GUILayoutOption[0]);
				int num = 1;
				foreach (TriggerActionInfo triggerActionInfo in this.selectedTrigger.actions)
				{
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					string text = num.ToString() + ":";
					if (triggerActionInfo.name != null)
					{
						string text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							triggerActionInfo.name,
							" (",
							triggerActionInfo.type.ToString(),
							")"
						});
					}
					else
					{
						text += triggerActionInfo.type.ToString();
					}
					GUILayout.Label(text, new GUILayoutOption[0]);
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					GUILayout.Label("Delay:", new GUILayoutOption[0]);
					float.TryParse(GUILayout.TextField(triggerActionInfo.timeOffset.ToString(), new GUILayoutOption[0]), out triggerActionInfo.timeOffset);
					if (GUILayout.Button("Edit", new GUILayoutOption[0]))
					{
						this.selectedAction = triggerActionInfo;
					}
					GUILayout.EndHorizontal();
				}
				num++;
			}
			else
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.Label("Editing Action: ", new GUILayoutOption[0]);
				this.selectedAction.name = GUILayout.TextField(this.selectedAction.name ?? string.Empty, new GUILayoutOption[0]);
				GUILayout.EndHorizontal();
				GUILayout.Label("Editing " + this.selectedAction.type + " action", new GUILayoutOption[0]);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (GUILayout.Button("Deselect", new GUILayoutOption[0]))
				{
					this.selectedAction = null;
				}
				if (GUILayout.Button("Delete", new GUILayoutOption[0]))
				{
					this.selectedTrigger.actions.Remove(this.selectedAction);
					this.selectedAction = null;
				}
				GUILayout.EndHorizontal();
				if (this.selectedAction != null)
				{
					this.selectedAction.ShowGUI(this);
				}
			}
		}
		GUILayout.EndScrollView();
	}

	internal void MarkTargetPoint(GridPoint targetPoint)
	{
		this.MarkTargetPoint(targetPoint, Color.white);
	}

	internal void MarkTargetPoint(GridPoint targetPoint, Color color)
	{
		color /= 2f;
		if (this.targetPointIndicators == null)
		{
			this.targetPointIndicators = new Dictionary<GridPos, AnimatedIcon>();
			this.targetPointIndicatorsTimeLeft = new Dictionary<GridPos, float>();
		}
		GridPos key = new GridPos(targetPoint.collumn, targetPoint.row);
		if (this.targetPointIndicators.ContainsKey(key))
		{
			this.targetPointIndicatorsTimeLeft[key] = 0.05f;
			if (this.targetPointIndicators[key] == null)
			{
				this.targetPointIndicators[key] = (UnityEngine.Object.Instantiate(this.TargetPointIndicatorPrefab, Map.GetBlockCenter(targetPoint.collumn, targetPoint.row), Quaternion.identity) as AnimatedIcon);
			}
			this.targetPointIndicators[key].stayOnRestFrame = true;
			if (color != this.targetPointIndicators[key].GetComponent<Renderer>().material.GetColor("_TintColor"))
			{
				this.targetPointIndicators[key].GetComponent<Renderer>().material.SetColor("_TintColor", Color.Lerp(this.targetPointIndicators[key].GetComponent<Renderer>().material.GetColor("_TintColor"), color, 0.5f));
			}
		}
		else
		{
			AnimatedIcon animatedIcon = UnityEngine.Object.Instantiate(this.TargetPointIndicatorPrefab, Map.GetBlockCenter(targetPoint.collumn, targetPoint.row), Quaternion.identity) as AnimatedIcon;
			this.targetPointIndicators.Add(key, animatedIcon);
			this.targetPointIndicatorsTimeLeft.Add(key, 0.1f);
			animatedIcon.GetComponent<Renderer>().material.SetColor("_TintColor", color);
		}
	}

	private void UpdateTargetPoints()
	{
		if (this.targetPointIndicatorsTimeLeft != null)
		{
			foreach (GridPos gridPos in this.targetPointIndicatorsTimeLeft.Keys.ToArray<GridPos>())
			{
				Dictionary<GridPos, float> dictionary2;
				Dictionary<GridPos, float> dictionary = dictionary2 = this.targetPointIndicatorsTimeLeft;
				GridPos key2;
				GridPos key = key2 = gridPos;
				float num = dictionary2[key2];
				if ((dictionary[key] = num - Time.deltaTime) < 0f)
				{
					UnityEngine.Object.Destroy(this.targetPointIndicators[gridPos]);
					this.targetPointIndicators.Remove(gridPos);
					this.targetPointIndicatorsTimeLeft.Remove(gridPos);
				}
			}
		}
	}

	private const string disclaimer = "This is the Broforce Level Editor (alpha)!\n\nThis is the tool we use to build levels in Broforce and we're quite excited to see what you guys can come up with.\n\nRight now the editor has a few bugs and is not as user-friendly as it should be. If you encounter any bugs or have any suggestions or feedback you can email me at ruan@freelives.net \n\nPlease note that a lot of the things in the level editor represent unfinished work, if you see something that we haven't put in our levels chances are it won't work properly.  \n\nSee our forums for a guide on how to use it: www.broforcegame.com/forums\n";

	private const string publishPromptText = "Publishing a campaign means that it is saved in a format that isn't editable by the level editor anymore, so you can share the campaign freely without someone copying your work. Make sure to save a editable version of your campaign before publishing.\n\nEach published custom campaign also has its own leaderboard so players can compete on your level.To ensure the campaign is actually completeable, you will have to finish it once yourself before being able to publish it. Click OK to start your playthrough or CANCEL to go back to editing.\n\nPublished campaigns have the file extension .bfg, unpublished campaigns have .bfc";

	private const string publishRunSuccessfulText = "You've completed your campaign and it is now ready to publish! Click OK below to save a shareable .bfg of your campaign, or CANCEL to just go back to editing and publish at a later time (you'll have to complete the campaign again though!).\n\n DISCLAIMER: AT SOME STAGE IN THE FUTURE WE MAY HAVE TO CLEAR OUR DATABASE OF ALL USER CREATED CONTENT. WE WILL DO WHAT WE CAN TO AVOID THIS, BUT PLEASE KEEP A LOCAL COPY OF ALL YOUR CAMPAIGNS THAT YOU DO NOT WISH TO LOSE.";

	public static Campaign campaign;

	private MapData mapData;

	public static bool NoClip = false;

	private static bool hasShownDisclaimer = false;

	private bool showPublishRunSuccessful;

	public static bool publishRunSuccessful = false;

	private static Vector2 scrollPos = Vector2.zero;

	public GUISkin skin;

	private SpriteSM[,] foregroundTiles;

	private SpriteSM[,] backgroundTiles;

	public SelectionIndicator SelectionIndicatorPrefab;

	public AnimatedIcon TargetPointIndicatorPrefab;

	private Dictionary<GridPos, AnimatedIcon> targetPointIndicators;

	private Dictionary<GridPos, float> targetPointIndicatorsTimeLeft;

	private SelectionIndicator activeSelectionIndicator;

	private SelectionIndicator mousePosSelectionIndicator;

	private SelectionIndicator targetSelectionIndicator;

	private SelectionIndicator[] targetSelectionIndicatorOthers;

	private List<SelectionIndicator> selectionIndicators = new List<SelectionIndicator>();

	private static float controlWidth = 0.25f;

	private List<TerrainType> terrainTypes;

	public List<EnemyActionType> enemyActionTypes;

	public List<CharacterCommandType> characterCommandTypes;

	private List<DoodadType> doodadTypes;

	private List<HeroSpawnMode> heroSpawnModes;

	private List<TriggerType> triggerTypes;

	private List<TriggerActionType> actionTypes;

	private List<GameMode> gameModes;

	private List<LevelTheme> levelThemes;

	private List<CameraFollowMode> cameraFollowModes;

	private List<CameraFollowMode> raceCameraFollowModes;

	private List<HeroType> heroTypes;

	private List<MusicType> musicTypes;

	private List<WeatherType> weatherTypes;

	private static EditorMode mode = EditorMode.Terrain;

	private TerrainType curTerrainType;

	private DoodadType curDoodadType;

	private int curDoodadVariation;

	private string fileName;

	private List<string> files;

	private bool showGUI = true;

	private int newMapWidth = 32;

	private int newMapHeight = 32;

	public bool settingWaypoint;

	public GridPoint waypointToSet;

	private static bool showAllDoodads = true;

	private bool showLightMap;

	private bool showTerrain;

	private bool showGrid;

	private bool showPathfinding;

	private bool publishPrompt;

	public static bool levelEditorActive = false;

	public static Vector3 lastCameraPos = -Vector3.one;

	public static LevelEditorGUI Instance;

	private bool showNewLevelBox;

	private string newMapName = string.Empty;

	private LevelTheme newMapTheme;

	private int pcount = 1;

	private float pathUpdateDelay;

	public static NavPath displayPath;

	private int c;

	private int r;

	private bool[,] gridClicked;

	private bool altDown;

	private float lastrealtime;

	private GridPoint taggedClick = new GridPoint(-1, -1);

	private bool destroyingForegroundThisClick;

	private bool destroyingMidgroundThisClick;

	private bool placingSquare;

	private bool deletingSquare;

	private GridPoint squareOrigin;

	private GridPoint squareEnd;

	private GridPoint dragStartPoint;

	private bool draggingSelection;

	private GridPoint lastMouseHoverPoint = new GridPoint(-1, -1);

	private DoodadInfo hoverDoodadInfo;

	public GameObject invalidVariationObject;

	private bool hasSelectedSquare;

	protected TriggerInfo selectedTrigger;

	protected TriggerActionInfo selectedAction;

	public delegate bool OnListItemGUI(object item, bool selected, ICollection list);
}
