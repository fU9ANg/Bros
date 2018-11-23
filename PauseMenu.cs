// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class PauseMenu : Menu
{
	protected override void Awake()
	{
		PauseMenu.instance = this;
		base.Awake();
		base.GetComponent<PauseComponent>().gamePausedChangedEvent += this.HandlegamePausedChangedEvent;
	}

	protected override void SetupItems()
	{
		base.SetupItems();
		for (int i = 0; i < this.itemNames.Length; i++)
		{
			if (this.itemNames[i].name.ToUpper().Equals("RESTART LEVEL") && GameModeController.publishRun)
			{
				this.itemNames[i].name = "Return to Editing";
			}
		}
	}

	public override void InstantiateItems()
	{
		base.InstantiateItems();
		if (Connect.IsOffline)
		{
		}
	}

	private void HandlegamePausedChangedEvent(bool paused)
	{
		this.MenuActive = paused;
		this.lastInputTime = Time.time;
		if (paused)
		{
			this.controlledByControllerID = PauseController.pausedByController;
			if (HeroController.GetPlayerUsingController(this.controlledByControllerID) == null)
			{
				base.SetMenuItemActive("DROP OUT", false);
			}
			else
			{
				base.SetMenuItemActive("DROP OUT", true);
			}
			if (!Connect.IsOffline && !Connect.IsHost)
			{
				base.SetMenuItemActive("RESTART LEVEL", false);
				base.SetMenuItemActive("KICK PLAYER", false);
			}
			else
			{
				base.SetMenuItemActive("RESTART LEVEL", true);
				if (!Connect.IsOffline)
				{
					base.SetMenuItemActive("KICK PLAYER", true);
				}
			}
		}
	}

	private void OnEnable()
	{
		this.HandlegamePausedChangedEvent(true);
	}

	private void RestartLevel()
	{
		Map.ClearSuperCheckpointStatus();
		if (GameModeController.publishRun)
		{
			GameModeController.publishRun = false;
			LevelEditorGUI.levelEditorActive = true;
		}
		GameModeController.RestartLevel();
	}

	private void ResumeGame()
	{
		this.controlledByControllerID = -1;
		PauseController.SetPause(PauseStatus.UnPaused);
	}

	private void OpenKickPlayerMenu()
	{
		base.gameObject.SetActive(false);
		KickPlayerMenu.OpenMenu(this.controlledByControllerID);
	}

	private void DropOut()
	{
		if (HeroController.GetPlayerUsingController(this.controlledByControllerID) != null)
		{
			HeroController.Dropout(HeroController.GetPlayerUsingController(this.controlledByControllerID).playerNum, true);
		}
		this.ResumeGame();
	}

	private void ReturnToMenu()
	{
		PauseController.SetPause(PauseStatus.UnPaused);
		if (GameModeController.GameMode == GameMode.Campaign && LevelSelectionController.isOnlineCampaign)
		{
			Application.LoadLevel(LevelSelectionController.CustomCampaignVictoryScene);
			PlaytomicController.GetCampaignScores();
		}
		else
		{
			Application.LoadLevel(LevelSelectionController.MainMenuScene);
		}
	}

	public static PauseMenu instance;
}
