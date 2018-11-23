// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomCampaignMenu : Menu
{
	protected override void Awake()
	{
		base.Awake();
		if (PlayerOptions.Instance.PlayerName != null && this.playerNameText != null)
		{
			this.playerNameText.text = "player name: " + PlayerOptions.Instance.PlayerName.ToLower();
		}
	}

	protected override void SetupItems()
	{
		base.SetupItems();
		List<MenuBarItem> list = new List<MenuBarItem>(this.itemNames);
		foreach (string name in FileIO.FindPublishedCampaignFiles())
		{
			MenuBarItem menuBarItem = new MenuBarItem();
			menuBarItem.color = Color.white;
			menuBarItem.size = list[0].size;
			menuBarItem.name = name;
			menuBarItem.invokeMethod = "StartCampaign";
			list.Insert(list.Count - 1, menuBarItem);
		}
		list.Insert(0, new MenuBarItem
		{
			color = Color.yellow,
			size = list[0].size * 1.25f,
			name = "OFFLINE LEVELS:",
			invokeMethod = string.Empty
		});
		MenuBarItem menuBarItem2 = new MenuBarItem();
		menuBarItem2.color = Color.yellow;
		menuBarItem2.size = list[0].size;
		menuBarItem2.name = "UNPUBLISHED CAMPAIGNS:";
		menuBarItem2.invokeMethod = string.Empty;
		this.unpublishedHeaderIndex = list.Count;
		list.Insert(list.Count - 1, menuBarItem2);
		foreach (string name2 in FileIO.FindCampaignFiles())
		{
			MenuBarItem menuBarItem3 = new MenuBarItem();
			menuBarItem3.color = Color.white;
			menuBarItem3.size = list[0].size;
			menuBarItem3.name = name2;
			menuBarItem3.invokeMethod = "StartCampaign";
			list.Insert(list.Count - 1, menuBarItem3);
		}
		if (PlaytomicController.isOnlineBuild)
		{
			list.Insert(0, new MenuBarItem
			{
				color = Color.white,
				size = 16f,
				name = "NEW ONLINE LEVELS",
				invokeMethod = "GoToOnlineCustomCampaignsNew"
			});
		}
		if (PlaytomicController.isOnlineBuild)
		{
			list.Insert(1, new MenuBarItem
			{
				color = Color.white,
				size = 16f,
				name = "MOST PLAYED ONLINE LEVELS",
				invokeMethod = "GoToOnlineCustomCampaignsPopular"
			});
		}
		if (PlaytomicController.isOnlineBuild && PlayerProgress.Instance.lastOnlineLevelId != null)
		{
			list.Insert(0, new MenuBarItem
			{
				color = Color.white,
				size = 16f,
				name = "CONTINUE LAST ONLINE CAMPAIGN",
				invokeMethod = "ContinueOnlineCampaign"
			});
		}
		this.itemNames = list.ToArray();
	}

	private void GoToOnlineCustomCampaignsNew()
	{
		PlaytomicController.listNewLevels = true;
		this.onlineLevelsMenu.hasSetup = false;
		this.onlineLevelsMenu.pageNo = 1;
		this.onlineLevelsMenu.MenuActive = true;
		this.onlineLevelsMenu.TransitionIn();
		this.MenuActive = false;
	}

	private void ContinueOnlineCampaign()
	{
		LevelSelectionController.ResetLevelAndGameModeToDefault();
		LevelSelectionController.CurrentLevelNum = PlayerProgress.Instance.lastOnlineLevelProgress;
		this.onlineLevelsMenu.LaunchLevel(PlayerProgress.Instance.lastOnlineLevelId);
		this.onlineLevelsMenu.MenuActive = true;
		this.onlineLevelsMenu.TransitionIn();
		this.MenuActive = false;
	}

	private void GoToOnlineCustomCampaignsPopular()
	{
		PlaytomicController.listNewLevels = false;
		this.onlineLevelsMenu.hasSetup = false;
		this.onlineLevelsMenu.pageNo = 1;
		this.onlineLevelsMenu.MenuActive = true;
		this.onlineLevelsMenu.TransitionIn();
		this.MenuActive = false;
	}

	private void StartCampaign()
	{
		LevelSelectionController.ResetLevelAndGameModeToDefault();
		string name = this.itemNames[this.highlightIndex].name;
		GameModeController.publishRun = false;
		if (this.campHeader != null)
		{
			GameModeController.GameMode = this.campHeader.gameMode;
		}
		else
		{
			GameModeController.GameMode = GameMode.Campaign;
		}
		LevelSelectionController.campaignToLoad = name;
		LevelSelectionController.loadCustomCampaign = true;
		LevelSelectionController.loadPublishedCampaign = (this.highlightIndex <= this.unpublishedHeaderIndex);
		LevelSelectionController.CurrentLevelNum = 0;
		LevelSelectionController.loadMode = MapLoadMode.Campaign;
		StatisticsController.ResetScore();
		LevelEditorGUI.levelEditorActive = false;
		Application.LoadLevel(LevelSelectionController.JoinScene);
	}

	public override void InstantiateItems()
	{
		base.InstantiateItems();
		for (int i = 0; i < this.items.Length; i++)
		{
			if (this.items[i].text.Equals("UNPUBLISHED CAMPAIGNS:") || this.items[i].text.Equals("OFFLINE LEVELS:"))
			{
				this.itemEnabled[i] = false;
			}
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.MenuActive)
		{
			if (this.highlightIndex != this.lastHighlightIndex)
			{
				this.lastHighlightIndex = this.highlightIndex;
				if (this.highlightIndex != this.itemNames.Length - 1)
				{
					try
					{
						if (this.highlightIndex < this.unpublishedHeaderIndex)
						{
							this.campHeader = FileIO.ReadCampaignHeader(this.itemNames[this.highlightIndex].name, ".bfg");
						}
						else
						{
							this.campHeader = FileIO.ReadCampaignHeader(this.itemNames[this.highlightIndex].name, ".bfc");
						}
						if (this.campHeader != null && this.campHeader.name != null)
						{
							string text = this.campHeader.name;
							if (text.Length > 35)
							{
								text = text.Substring(0, 32) + "...";
							}
							this.nameText.text = text;
							this.authorText.text = string.Concat(new object[]
							{
								"AUTHOR: ",
								this.campHeader.author,
								"   LENGTH: ",
								this.campHeader.length,
								"   MODE: ",
								this.campHeader.gameMode
							});
							this.descriptionText.text = this.campHeader.description;
						}
						else
						{
							this.ClearHeaderStuff();
						}
					}
					catch (Exception)
					{
						this.ClearHeaderStuff();
					}
				}
				else
				{
					this.ClearHeaderStuff();
				}
			}
			if (Input.GetKeyDown(KeyCode.Escape) || InputReader.GetControllerPressingSpecial() > -1)
			{
				this.GoBackToMainMenu();
			}
		}
		else
		{
			this.ClearHeaderStuff();
		}
	}

	private void ClearHeaderStuff()
	{
		TextMesh textMesh = this.nameText;
		string text = string.Empty;
		this.gameModeText.text = text;
		text = text;
		this.descriptionText.text = text;
		text = text;
		this.lengthText.text = text;
		text = text;
		this.authorText.text = text;
		textMesh.text = text;
	}

	public override void TransitionIn()
	{
		base.TransitionIn();
	}

	private void GoBackToMainMenu()
	{
		Application.LoadLevel("MainMenu");
	}

	public Menu mainMenu;

	public GameObject logo;

	public TextMesh nameText;

	public TextMesh authorText;

	public TextMesh lengthText;

	public TextMesh descriptionText;

	public TextMesh gameModeText;

	public OnlineCustomCampaignBrowser onlineLevelsMenu;

	private int unpublishedHeaderIndex = -1;

	public TextMesh playerNameText;

	private int lastHighlightIndex = -1;

	private CampaignHeader campHeader;
}
