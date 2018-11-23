// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Demonstration
{
	public static void NextDemoLevel()
	{
		Demonstration.levelNum++;
	}

	public static void PreviousDemoLevel()
	{
		Demonstration.levelNum--;
	}

	protected static void GotoDemoLevel(int num)
	{
		switch (num)
		{
		case 0:
			Fader.nextScene = "DemoLevel_" + num;
			Fader.FadeSolid();
			break;
		case 1:
			Fader.nextScene = "DemoLevel_" + num;
			Fader.FadeSolid();
			break;
		case 2:
			Fader.nextScene = "DemoLevel_" + num;
			Fader.FadeSolid();
			break;
		case 3:
			Fader.nextScene = "DemoLevel_" + num;
			Fader.FadeSolid();
			break;
		case 4:
			Fader.nextScene = "DemoLevel_" + num;
			Fader.FadeSolid();
			break;
		case 5:
			Fader.nextScene = "DemoLevel_" + num;
			Fader.FadeSolid();
			break;
		case 6:
			Fader.nextScene = "DemoLevel_" + num;
			Fader.FadeSolid();
			break;
		case 7:
			Fader.nextScene = "DemoLevel_" + num;
			Fader.FadeSolid();
			break;
		case 8:
			Fader.nextScene = "DemoLevel_" + num;
			Fader.FadeSolid();
			break;
		case 9:
			Fader.nextScene = "DemoLevel_" + num;
			Fader.FadeSolid();
			break;
		case 10:
			Fader.nextScene = "DemoLevel_" + num;
			Fader.FadeSolid();
			break;
		case 11:
			Fader.nextScene = "DemoLevel_" + num;
			Fader.FadeSolid();
			break;
		case 12:
			Fader.nextScene = "DemoLevel_" + num;
			Fader.FadeSolid();
			break;
		default:
			UnityEngine.Debug.LogError("Don't Know");
			Application.Quit();
			break;
		}
	}

	public static bool doesLoadLevel = true;

	public static bool projectilesHitWalls = true;

	public static bool mooksStartUnknowing = true;

	public static bool bulletsHurtWalls = true;

	public static bool bulletsAreFast = true;

	public static bool enemiesSeeThroughWalls;

	public static bool herosClimbWalls = true;

	public static bool enemiesHaveDelayOnAlert = true;

	public static bool canPushBlocks = true;

	public static bool enemiesSetOnFire = true;

	public static bool enemiesSpreadFire = true;

	public static bool enemiesWanderFar = true;

	public static bool enemiesAlreadyAware;

	public static bool infiniteLives;

	public static int levelNum;
}
