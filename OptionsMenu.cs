// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class OptionsMenu : Menu
{
	protected override void Awake()
	{
		OptionsMenu.instance = this;
		base.Awake();
		this.SetMusicText();
		this.SetScreenDistortionText();
		this.SetDifficultyText();
		this.SeKeyboardLayoutText();
		if (!PlaytomicController.isExhibitionBuild)
		{
			this.SetPlayerNameText();
		}
	}

	private void ClearUnlocks()
	{
		HeroUnlockController.ClearUnlocks();
		foreach (TextMesh textMesh in this.items)
		{
			textMesh.transform.position = textMesh.transform.position + UnityEngine.Random.insideUnitSphere * 30f;
		}
	}

	private void ToggleMusic()
	{
		if (PlayerOptions.Instance.musicVolume > 0.1f)
		{
			PlayerOptions.Instance.musicVolume = 0f;
			Sound.GetInstance().SetMusicVolume(0f);
		}
		else
		{
			PlayerOptions.Instance.musicVolume = 0.24f;
			Sound.GetInstance().SetMusicVolume(0.24f);
		}
		this.SetMusicText();
	}

	private void ToggleGermanKeyLayout()
	{
		ChatSystem.UseGermanLayout = !ChatSystem.UseGermanLayout;
		this.SeKeyboardLayoutText();
	}

	private void ToggleDifficulty()
	{
		PlayerOptions.Instance.hardMode = !PlayerOptions.Instance.hardMode;
		this.SetDifficultyText();
	}

	private void ToggleScreenDistortion()
	{
		CameraController.ScreenDistortionEnabled = !CameraController.ScreenDistortionEnabled;
		this.SetScreenDistortionText();
	}

	private void SetMusicText()
	{
		TextMesh textMesh = this.backDrops[0];
		string text = "MUSIC: " + ((PlayerOptions.Instance.musicVolume <= 0.1f) ? "OFF" : "ON");
		this.items[0].text = text;
		textMesh.text = text;
	}

	private void SetDifficultyText()
	{
		TextMesh textMesh = this.backDrops[2];
		string text = "DIFFICULTY: " + ((!PlayerOptions.Instance.hardMode) ? "NORMAL" : "HARD");
		this.items[2].text = text;
		textMesh.text = text;
	}

	private void SeKeyboardLayoutText()
	{
		TextMesh textMesh = this.backDrops[6];
		string text = "GERMAN KEYBOARD FOR CHAT : " + ((!ChatSystem.UseGermanLayout) ? "NO" : "YES");
		this.items[6].text = text;
		textMesh.text = text;
	}

	private void SetScreenDistortionText()
	{
		TextMesh textMesh = this.backDrops[1];
		string text = "SCREEN DISTORTION: " + ((!CameraController.ScreenDistortionEnabled) ? "OFF" : "ON");
		this.items[1].text = text;
		textMesh.text = text;
	}

	public void SetPlayerNameText()
	{
		this.playerName = PlayerOptions.Instance.PlayerName;
		if (this.settingPlayerName)
		{
			TextMesh textMesh = this.backDrops[5];
			string text = this.playerName;
			this.items[5].text = text;
			textMesh.text = text;
		}
		else
		{
			TextMesh textMesh2 = this.backDrops[5];
			string text = "NAME ON LEADERBOARDS: " + this.playerName;
			this.items[5].text = text;
			textMesh2.text = text;
		}
		MonoBehaviour.print("SetPlayerNameText " + this.playerName);
	}

	protected override void Update()
	{
		if (this.settingPlayerName)
		{
			foreach (char c in Input.inputString.ToCharArray())
			{
				if (c == "\n"[0] || c == "\r"[0])
				{
					this.settingPlayerName = false;
					PlayerOptions.Instance.PlayerName = this.playerName;
				}
				else if (c == "\b"[0])
				{
					this.playerName = this.playerName.Substring(0, Mathf.Clamp(this.playerName.Length - 1, 0, 999999));
				}
				else if (this.playerName.Length < 15)
				{
					this.playerName += Input.inputString;
				}
			}
			this.SetPlayerNameText();
		}
		else
		{
			base.Update();
		}
	}

	protected void StartSettingUpKeys()
	{
		this.MenuActive = false;
		this.controlsMenu.MenuActive = true;
		this.controlsMenu.TransitionIn();
	}

	private void SetPlayerName()
	{
		this.settingPlayerName = true;
	}

	private void ToggleFullscreen()
	{
		if (!Screen.fullScreen)
		{
			this.windowHeight = Screen.height;
			this.windowWidth = Screen.width;
			Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
			PlayerOptions.Instance.fullscreen = true;
		}
		else
		{
			if (this.windowWidth * this.windowHeight > 0)
			{
				Screen.SetResolution(this.windowWidth, this.windowHeight, false);
			}
			else
			{
				Screen.fullScreen = false;
			}
			PlayerOptions.Instance.fullscreen = false;
		}
	}

	protected void ShowMenu()
	{
		this.logo.gameObject.SetActive(true);
		this.MenuActive = true;
	}

	protected void HideMenu()
	{
		this.logo.gameObject.SetActive(false);
		this.MenuActive = false;
	}

	private void GoToResolutionMenu()
	{
		this.MenuActive = false;
		this.resolutionMenu.MenuActive = true;
		this.resolutionMenu.TransitionIn();
	}

	private void GoBackToMainMenu()
	{
		PlayerOptions.Instance.Persist();
		this.MenuActive = false;
		this.mainMenu.MenuActive = true;
		this.mainMenu.TransitionIn();
	}

	private void ResetOptions()
	{
		PlayerOptions.Instance.ResetToDefault();
	}

	public Menu mainMenu;

	public Menu controlsMenu;

	public Menu resolutionMenu;

	public GameObject logo;

	private string playerName = string.Empty;

	private int windowHeight;

	private int windowWidth;

	public static OptionsMenu instance;

	public GameObject keySetupHolder;

	protected float keyPressDelay = 0.1f;

	private bool settingPlayerName;
}
