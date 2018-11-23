// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ExpendabrosVictoryMenu : Menu
{
	private void BroforceOnSteam()
	{
		if (Time.time - this.lastGoTime > 3f)
		{
			Application.OpenURL("http://store.steampowered.com/app/274190/");
			this.lastGoTime = Time.time;
		}
	}

	private void Expendables()
	{
		if (Time.time - this.lastGoTime > 3f)
		{
			Application.OpenURL("http://www.theexpendables3film.com/");
			this.lastGoTime = Time.time;
		}
	}

	private void GoToMainMenu()
	{
		Application.LoadLevel(LevelSelectionController.MainMenuScene);
	}

	private void QuitGame()
	{
		Application.Quit();
	}

	private float lastGoTime;
}
