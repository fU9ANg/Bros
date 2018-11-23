// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class PlayerOptions
{
	private PlayerOptions()
	{
	}

	[XmlIgnore]
	public string PlayerName
	{
		get
		{
			try
			{
				if (SteamController.IsSteamEnabled())
				{
					return SteamController.PlayerNick;
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
			return this.playerName;
		}
		set
		{
			try
			{
				if (SteamController.IsSteamEnabled())
				{
					UnityEngine.Debug.LogError("Can not set name if steam is enabled!");
				}
				else
				{
					this.playerName = value;
				}
				if (OptionsMenu.instance != null)
				{
					OptionsMenu.instance.SetPlayerNameText();
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
	}

	public static PlayerOptions Instance
	{
		get
		{
			if (PlayerOptions.instance == null)
			{
				PlayerOptions.InitializeOptions();
			}
			return PlayerOptions.instance;
		}
	}

	public static void InitializeOptions()
	{
		if (PlayerOptions.instance == null)
		{
			try
			{
				PlayerOptions.instance = FileIO.LoadOptions();
				if (PlayerOptions.instance.keyPlayer1Jump == KeyCode.None)
				{
					PlayerOptions.instance.keyPlayer1Jump = PlayerOptions.instance.keyPlayer1Up;
				}
				if (PlayerOptions.instance.keyPlayer2Jump == KeyCode.None)
				{
					PlayerOptions.instance.keyPlayer2Jump = PlayerOptions.instance.keyPlayer2Up;
				}
				cInput.Init();
				if (!string.IsNullOrEmpty(PlayerOptions.instance.cInputSetting))
				{
					cInput.LoadExternal(PlayerOptions.instance.cInputSetting);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Could not load player options, resetting to default:\n" + ex.Message + "\n" + ex.StackTrace);
				PlayerOptions.instance = new PlayerOptions();
				PlayerOptions.instance.ResetToDefault();
				PlayerOptions.Instance.Persist();
			}
			UnityEngine.Debug.Log(PlayerOptions.instance.playerName);
		}
	}

	public void ResetToDefault()
	{
		this.keyPlayer1Fire = KeyCode.Z;
		this.keyPlayer2Fire = KeyCode.T;
		this.keyPlayer1Special = KeyCode.X;
		this.keyPlayer2Special = KeyCode.Y;
		this.keyPlayer1Up = KeyCode.UpArrow; this.keyPlayer1Jump = (this.keyPlayer1Up );
		this.keyPlayer2Up = KeyCode.W; this.keyPlayer2Jump = (this.keyPlayer2Up );
		this.keyPlayer1Down = KeyCode.DownArrow;
		this.keyPlayer2Down = KeyCode.S;
		this.keyPlayer1Left = KeyCode.LeftArrow;
		this.keyPlayer2Left = KeyCode.A;
		this.keyPlayer1Right = KeyCode.RightArrow;
		this.keyPlayer2Right = KeyCode.D;
		this.keyPlayer1HighFive = KeyCode.C;
		this.keyPlayer2HighFive = KeyCode.U;
		cInput.Clear();
		for (int i = 1; i <= 4; i++)
		{
			cInput.SetKey("Fire_" + i, "Joystick" + i + "Button2", "Joy" + i + " Axis 3-");
			cInput.SetKey("Jump_" + i, "Joystick" + i + "Button0");
			cInput.SetKey("Special_" + i, "Joystick" + i + "Button1");
			cInput.SetKey("Melee_" + i, "Joystick" + i + "Button3");
			cInput.SetKey("Left_" + i, "Joy" + i + " Axis 1-", "Joy" + i + " Axis 6-");
			cInput.SetKey("Right_" + i, "Joy" + i + " Axis 1+", "Joy" + i + " Axis 6+");
			cInput.SetKey("Up_" + i, "Joy" + i + " Axis 2-", "Joy" + i + " Axis 7+");
			cInput.SetKey("Down_" + i, "Joy" + i + " Axis 2+", "Joy" + i + " Axis 7-");
			cInput.SetKey("Dash_" + i, "Joystick" + i + "Button4");
			cInput.SetKey("Start_" + i, "Joystick" + i + "Button7");
		}
		cInput.SetKey("MWheelUp", "Mouse Wheel Up");
		cInput.SetKey("MWheelDown", "Mouse Wheel Down");
		cInput.SetAxis("MWheel", "MWheelUp", "MWheelDown");
		this.musicVolume = 0.24f;
		this.cameraDistortionEnabled = true;
	}

	internal void Persist()
	{
		UnityEngine.Debug.Log("Persist " + this.playerName);
		this.cInputSetting = cInput.externalInputs;
		FileIO.SaveOptions(this);
	}

	public string playerName = "Brononymous";

	public string cInputSetting;

	public string LastCustomLevel;

	public float musicVolume;

	public int resolutionW;

	public int resolutionH;

	public bool fullscreen;

	public bool hardMode;

	public bool cameraDistortionEnabled;

	public string lastRatedCampaign;

	public KeyCode keyPlayer1Fire;

	public KeyCode keyPlayer2Fire;

	public KeyCode keyPlayer1Special;

	public KeyCode keyPlayer2Special;

	public KeyCode keyPlayer1Jump;

	public KeyCode keyPlayer2Jump;

	public KeyCode keyPlayer1Up;

	public KeyCode keyPlayer2Up;

	public KeyCode keyPlayer1Down;

	public KeyCode keyPlayer2Down;

	public KeyCode keyPlayer1Left;

	public KeyCode keyPlayer2Left;

	public KeyCode keyPlayer1Right;

	public KeyCode keyPlayer2Right;

	public KeyCode keyPlayer1HighFive;

	public KeyCode keyPlayer2HighFive;

	private static PlayerOptions instance;
}
