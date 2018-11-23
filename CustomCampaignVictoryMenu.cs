// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CustomCampaignVictoryMenu : Menu
{
	private void Start()
	{
		global::Math.SetupLookupTables();
	}

	public override void TransitionIn()
	{
		this.scoreDisplay.SetActive(true);
	}

	private void RestartCampaign()
	{
		StatisticsController.ResetScore();
		Map.ClearSuperCheckpointStatus();
		LevelSelectionController.CurrentLevelNum = 0;
		Application.LoadLevel(LevelSelectionController.CampaignScene);
		GameModeController.GameMode = GameMode.Campaign;
	}

	private void ReturnToLevelSelect()
	{
		Application.LoadLevel("OnlineCustomCampaignBrowser");
	}

	private void ReturnToMenu()
	{
		Application.LoadLevel("MainMenu");
	}

	public GameObject scoreDisplay;
}
