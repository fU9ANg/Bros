// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class VictoryMenu : Menu
{
	private void GoToGreenlight()
	{
		if (!this.hasGoneToWebsite)
		{
			this.hasGoneToWebsite = true;
			Application.OpenURL("http://www.freelives.net/BROFORCE/PREORDER/PreOrders.html");
			PlaytomicController.LogPreorderClick();
		}
	}

	private void GoToMainMenu()
	{
		Application.LoadLevel(LevelSelectionController.MainMenuScene);
	}

	private void SupportTerror()
	{
		Application.LoadLevel(LevelSelectionController.MainMenuScene);
	}

	private bool hasGoneToWebsite;
}
