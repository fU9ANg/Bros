// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MissionScreenController : NetworkObject
{
	private void Awake()
	{
		MissionScreenController.instance = this;
		this.startLeaveCounter = this.leaveCounter;
	}

	public static WeatherType GetWeatherType()
	{
		if (MissionScreenController.weatherType != WeatherType.None)
		{
			return MissionScreenController.weatherType;
		}
		if (MissionScreenController.instance != null)
		{
			return MissionScreenController.instance.testWeatherType;
		}
		return WeatherType.None;
	}

	public static RainType GetRainType()
	{
		if (MissionScreenController.rainType != RainType.None)
		{
			return MissionScreenController.rainType;
		}
		if (MissionScreenController.instance != null)
		{
			return MissionScreenController.instance.testRainType;
		}
		return RainType.None;
	}

	public static void SetVariables(string operation, WeatherType wT, RainType rT)
	{
		MissionScreenController.operationName = operation;
		MissionScreenController.weatherType = wT;
		MissionScreenController.rainType = rT;
	}

	protected void ResetWeastherVariables()
	{
		MissionScreenController.weatherType = WeatherType.None;
		MissionScreenController.rainType = RainType.None;
	}

	protected void ResetOperationName()
	{
		MissionScreenController.operationName = string.Empty;
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.03334f);
		if (this.leaveCounter > 0f)
		{
			this.leaveCounter -= num;
			if (this.leaveCounter <= 0f)
			{
				this.helicopter.Leave();
			}
		}
		if (!this.fading && ((this.startLeaveCounter - this.leaveCounter > 0.3f && InputReader.GetControllerPressingFire() >= 0 && !this.showMenuOnFinish) || (this.leaveCounter <= 0f && this.helicopter.transform.position.x > Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width * 0.8f, 0f, 10f)).x)))
		{
			this.fading = true;
			if (this.showMenuOnFinish)
			{
				Fader.FadeSolid(2f, false);
				this.menuToShow.MenuActive = true;
			}
			else
			{
				Fader.FadeHorizontalSwipe(LevelSelectionController.CampaignScene);
			}
			this.ResetWeastherVariables();
		}
		if (this.titleCounter > 0f)
		{
			this.titleCounter -= Time.deltaTime;
			if (this.titleCounter <= 0f)
			{
				if (MissionScreenController.operationName.Length <= 0 && this.testOperationName.Length > 0)
				{
					MissionScreenController.operationName = this.testOperationName;
				}
				if (MissionScreenController.operationName.Length > 0)
				{
					LevelTitle.ShowText(MissionScreenController.operationName, 0f);
				}
				this.ResetOperationName();
			}
		}
	}

	public MissionScreenHelicopter helicopter;

	public Camera helicopterCamera;

	public float leaveCounter = 2f;

	protected float startLeaveCounter = 2f;

	public float titleCounter = 0.5f;

	protected bool fading;

	public bool showMenuOnFinish;

	public Menu menuToShow;

	protected static string operationName = "OPERATION BROFORCE!";

	public string testOperationName = "OPERATION BROFORCE!";

	public WeatherType testWeatherType = WeatherType.NoChange;

	public RainType testRainType = RainType.NoChange;

	protected static WeatherType weatherType;

	protected static RainType rainType;

	public static MissionScreenController instance;
}
