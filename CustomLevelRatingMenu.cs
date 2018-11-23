// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CustomLevelRatingMenu : Menu
{
	private void Start()
	{
		global::Math.SetupLookupTables();
		if (LevelSelectionController.currentCampaign.header.md5.Equals(PlayerOptions.Instance.lastRatedCampaign))
		{
			this.ReturnToMenu();
		}
		else if (!LevelSelectionController.isOnlineCampaign || PlaytomicController.loadedLevel == null)
		{
			this.ReturnToMenu();
		}
		PlayerOptions.Instance.lastRatedCampaign = LevelSelectionController.currentCampaign.header.md5;
	}

	private void Save()
	{
		FileIO.PublishCampaign(LevelSelectionController.currentCampaign, LevelSelectionController.currentCampaign.name);
	}

	private void Rate(int rating)
	{
		PlaytomicController.RateLevel(rating);
		this.ReturnToMenu();
	}

	private void Rate1()
	{
		this.Rate(1);
	}

	private void Rate2()
	{
		this.Rate(2);
	}

	private void Rate3()
	{
		this.Rate(3);
	}

	private void Rate4()
	{
		this.Rate(4);
	}

	private void Rate5()
	{
		this.Rate(5);
	}

	private void ReturnToMenu()
	{
		this.MenuActive = false;
		this.victoryMenu.MenuActive = true;
		this.victoryMenu.TransitionIn();
		this.promptText.gameObject.SetActive(false);
		this.ratingTitle.SetActive(false);
	}

	public TextMesh promptText;

	public Menu victoryMenu;

	public GameObject ratingTitle;
}
