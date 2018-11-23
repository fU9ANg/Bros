// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.IO;
using UnityEngine;

public class TakeScreenshot : MonoBehaviour
{
	private void Awake()
	{
		TakeScreenshot.screenshotCount = PlayerPrefs.GetInt("ScreenShotCount", 0);
	}

	private void FixedUpdate()
	{
		if (Application.platform == RuntimePlatform.WindowsPlayer && this.autoTakeScreenShot && Time.time > this.lastScreenShotTime + this.screenShotRate)
		{
			this.lastScreenShotTime = Time.time;
			this.TakeScreenShotNow();
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F9) || Input.GetKeyDown(KeyCode.Print))
		{
			this.TakeScreenShotNow();
		}
	}

	protected void TakeScreenShotNow()
	{
		string text;
		do
		{
			TakeScreenshot.screenshotCount++;
			text = "screenshot" + TakeScreenshot.screenshotCount + ".png";
		}
		while (File.Exists(Application.dataPath + text) || File.Exists(Application.dataPath + "/" + text));
		PlayerPrefs.SetInt("ScreenShotCount", TakeScreenshot.screenshotCount);
		Application.CaptureScreenshot(text);
	}

	private static int screenshotCount;

	public bool autoTakeScreenShot;

	protected float lastScreenShotTime;

	public float screenShotRate = 0.5f;
}
