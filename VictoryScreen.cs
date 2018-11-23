// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class VictoryScreen : MonoBehaviour
{
	private void Start()
	{
		this.startTime = Time.time;
	}

	public static void HasQuit()
	{
		VictoryScreen.nextScene = "Quit";
	}

	private void Update()
	{
	}

	protected float startTime;

	protected static string nextScene = LevelSelectionController.MainMenuScene;
}
