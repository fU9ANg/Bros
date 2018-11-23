// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public static class InputReader
{
	private static bool IsBlocked
	{
		get
		{
			return ChatSystem.IsFocused;
		}
	}

	public static void GetInput(int controllerNum, ref bool up, ref bool down, ref bool left, ref bool right, ref bool fire, ref bool buttonJump, ref bool special, ref bool highFive)
	{
		highFive = false; up = (down = (left = (right = (fire = (buttonJump = (special = (highFive )))))));
		if (!CutsceneController.PlayersCanMove())
		{
			return;
		}
		if (InputReader.IsBlocked || CutsceneController.isInCutscene)
		{
			return;
		}
		InputReader.GetInputIgnoreBlock(controllerNum, ref up, ref down, ref left, ref right, ref fire, ref buttonJump, ref special, ref highFive);
	}

	public static void GetCombinedInput(ref bool up, ref bool down, ref bool left, ref bool right, ref bool fire, ref bool buttonJump, ref bool special, ref bool highFive)
	{
		if (InputReader.IsBlocked)
		{
			return;
		}
		highFive = false; up = (down = (left = (right = (fire = (buttonJump = (special = (highFive )))))));
		bool flag8;
		bool flag7;
		bool flag6;
		bool flag5;
		bool flag4;
		bool flag3;
		bool flag2;
		flag8 = false; bool flag = flag2 = (flag3 = (flag4 = (flag5 = (flag6 = (flag7 = (flag8 ))))));
		for (int i = 0; i < 6; i++)
		{
			InputReader.GetInput(i, ref flag2, ref flag, ref flag3, ref flag4, ref flag5, ref flag6, ref flag7, ref flag8);
			up = (up || flag2);
			down = (down || flag);
			left = (left || flag3);
			right = (right || flag4);
			fire = (fire || flag5);
			buttonJump = (buttonJump || flag6);
			special = (special || flag7);
			highFive = (highFive || flag8);
		}
	}

	public static bool GetMenuInputStandardKeys(ref bool up, ref bool down, ref bool left, ref bool right, ref bool accept, ref bool decline)
	{
		if (InputReader.IsBlocked)
		{
			return false;
		}
		up = Input.GetKeyDown(KeyCode.UpArrow);
		down = Input.GetKeyDown(KeyCode.DownArrow);
		left = Input.GetKeyDown(KeyCode.LeftArrow);
		right = Input.GetKeyDown(KeyCode.RightArrow);
		accept = Input.GetKeyDown(KeyCode.Return);
		decline = Input.GetKeyDown(KeyCode.Escape);
		return up | down | left | right | accept | decline;
	}

	public static bool GetMenuInputCombined(ref bool up, ref bool down, ref bool left, ref bool right, ref bool accept, ref bool decline)
	{
		if (InputReader.IsBlocked)
		{
			return false;
		}
		bool flag8;
		bool flag7;
		bool flag6;
		bool flag5;
		bool flag4;
		bool flag3;
		bool flag2;
		flag8 = false; bool flag = flag2 = (flag3 = (flag4 = (flag5 = (flag6 = (flag7 = (flag8 ))))));
		for (int i = 0; i < 6; i++)
		{
			InputReader.GetInput(i, ref flag2, ref flag, ref flag3, ref flag4, ref flag5, ref flag6, ref flag7, ref flag8);
			up = (up || flag2);
			down = (down || flag);
			left = (left || flag3);
			right = (right || flag4);
			accept = (accept || flag5);
			if (i > 1)
			{
				accept = (accept || flag6);
			}
			decline = (decline || flag7);
		}
		return up | down | left | right | accept | decline;
	}

	public static bool GetMenuInput(int Controller, ref bool up, ref bool down, ref bool left, ref bool right, ref bool accept, ref bool decline)
	{
		if (InputReader.IsBlocked)
		{
			return false;
		}
		bool flag8;
		bool flag7;
		bool flag6;
		bool flag5;
		bool flag4;
		bool flag3;
		bool flag2;
		flag8 = false; bool flag = flag2 = (flag3 = (flag4 = (flag5 = (flag6 = (flag7 = (flag8 ))))));
		InputReader.GetInput(Controller, ref flag2, ref flag, ref flag3, ref flag4, ref flag5, ref flag6, ref flag7, ref flag8);
		up = (up || flag2);
		down = (down || flag);
		left = (left || flag3);
		right = (right || flag4);
		accept = (accept || flag5);
		if (Controller > 1)
		{
			accept = (accept || flag6);
		}
		decline = (decline || flag7);
		return up | down | left | right | accept | decline;
	}

	public static void GetKeyboard1Input(ref bool up, ref bool down, ref bool left, ref bool right, ref bool fire, ref bool buttonJump, ref bool special, ref bool highFive)
	{
		if (InputReader.IsBlocked)
		{
			return;
		}
		fire = (fire || Input.GetKey(PlayerOptions.Instance.keyPlayer1Fire));
		special = (special || Input.GetKey(PlayerOptions.Instance.keyPlayer1Special));
		highFive = (highFive || Input.GetKey(PlayerOptions.Instance.keyPlayer1HighFive));
		up = (up || Input.GetKey(PlayerOptions.Instance.keyPlayer1Up));
		buttonJump = (buttonJump || Input.GetKey(PlayerOptions.Instance.keyPlayer1Jump));
		down = (down || Input.GetKey(PlayerOptions.Instance.keyPlayer1Down));
		left = (left || Input.GetKey(PlayerOptions.Instance.keyPlayer1Left));
		right = (right || Input.GetKey(PlayerOptions.Instance.keyPlayer1Right));
	}

	public static void GetKeyboard2Input(ref bool up, ref bool down, ref bool left, ref bool right, ref bool fire, ref bool buttonJump, ref bool special, ref bool highFive)
	{
		if (InputReader.IsBlocked)
		{
			return;
		}
		fire = (fire || Input.GetKey(PlayerOptions.Instance.keyPlayer2Fire));
		special = (special || Input.GetKey(PlayerOptions.Instance.keyPlayer2Special));
		highFive = (highFive || Input.GetKey(PlayerOptions.Instance.keyPlayer2HighFive));
		up = (up || Input.GetKey(PlayerOptions.Instance.keyPlayer2Up));
		buttonJump = (buttonJump || Input.GetKey(PlayerOptions.Instance.keyPlayer2Jump));
		down = (down || Input.GetKey(PlayerOptions.Instance.keyPlayer2Down));
		left = (left || Input.GetKey(PlayerOptions.Instance.keyPlayer2Left));
		right = (right || Input.GetKey(PlayerOptions.Instance.keyPlayer2Right));
	}

	public static void GetXBoxControllerInput(string controllerIDString, ref bool up, ref bool down, ref bool left, ref bool right, ref bool fire, ref bool buttonJump, ref bool special, ref bool highFive)
	{
		if (InputReader.IsBlocked)
		{
			return;
		}
		fire = (fire || cInput.GetButton("Fire" + controllerIDString));
		buttonJump = (buttonJump || cInput.GetButton("Jump" + controllerIDString));
		highFive = (highFive || cInput.GetButton("Melee" + controllerIDString));
		special = (special || cInput.GetButton("Special" + controllerIDString));
		up = (up || cInput.GetButton("Up" + controllerIDString));
		down = (down || cInput.GetButton("Down" + controllerIDString));
		left = (left || cInput.GetButton("Left" + controllerIDString));
		right = (right || cInput.GetButton("Right" + controllerIDString));
	}

	public static int GetControllerPressingStart()
	{
		if (InputReader.IsBlocked)
		{
			return -1;
		}
		if (cInput.GetButtonDown("Start_1"))
		{
			return 2;
		}
		if (cInput.GetButtonDown("Start_2"))
		{
			return 3;
		}
		if (cInput.GetButtonDown("Start_3"))
		{
			return 4;
		}
		if (cInput.GetButtonDown("Start_4"))
		{
			return 5;
		}
		return -1;
	}

	public static int GetControllerPressingFire()
	{
		if (InputReader.IsBlocked)
		{
			return -1;
		}
		if (Input.GetKeyDown(PlayerOptions.Instance.keyPlayer1Fire))
		{
			return 0;
		}
		if (Input.GetKeyDown(PlayerOptions.Instance.keyPlayer2Fire))
		{
			return 1;
		}
		if (cInput.GetButtonDown("Fire_1"))
		{
			return 2;
		}
		if (cInput.GetButtonDown("Fire_2"))
		{
			return 3;
		}
		if (cInput.GetButtonDown("Fire_3"))
		{
			return 4;
		}
		if (cInput.GetButtonDown("Fire_4"))
		{
			return 5;
		}
		return -1;
	}

	public static int GetControllerHoldingFire()
	{
		if (InputReader.IsBlocked)
		{
			return -1;
		}
		if (Input.GetKey(PlayerOptions.Instance.keyPlayer1Fire))
		{
			return 0;
		}
		if (Input.GetKey(PlayerOptions.Instance.keyPlayer2Fire))
		{
			return 1;
		}
		if (cInput.GetButton("Fire_1"))
		{
			return 2;
		}
		if (cInput.GetButton("Fire_2"))
		{
			return 3;
		}
		if (cInput.GetButton("Fire_3"))
		{
			return 4;
		}
		if (cInput.GetButton("Fire_4"))
		{
			return 5;
		}
		return -1;
	}

	public static int GetControllerPressingJump()
	{
		if (InputReader.IsBlocked)
		{
			return -1;
		}
		if (Input.GetKeyDown(PlayerOptions.Instance.keyPlayer1Up))
		{
			return 0;
		}
		if (Input.GetKeyDown(PlayerOptions.Instance.keyPlayer2Up))
		{
			return 1;
		}
		if (cInput.GetButtonDown("Jump_1"))
		{
			return 2;
		}
		if (cInput.GetButtonDown("Jump_2"))
		{
			return 3;
		}
		if (cInput.GetButtonDown("Jump_3"))
		{
			return 4;
		}
		if (cInput.GetButtonDown("Jump_4"))
		{
			return 5;
		}
		return -1;
	}

	public static bool IsControllerPressingJump(int controllerNum)
	{
		if (InputReader.IsBlocked)
		{
			return false;
		}
		switch (controllerNum)
		{
		case 0:
			return Input.GetKeyDown(PlayerOptions.Instance.keyPlayer1Up);
		case 1:
			return Input.GetKeyDown(PlayerOptions.Instance.keyPlayer2Up);
		case 2:
			return cInput.GetButtonDown("Jump_1");
		case 3:
			return cInput.GetButtonDown("Jump_2");
		case 4:
			return cInput.GetButtonDown("Jump_3");
		case 5:
			return cInput.GetButtonDown("Jump_4");
		default:
			return false;
		}
	}

	public static bool IsControllerPressingFire(int controllerNum)
	{
		if (InputReader.IsBlocked)
		{
			return false;
		}
		switch (controllerNum)
		{
		case 0:
			return Input.GetKeyDown(PlayerOptions.Instance.keyPlayer1Fire);
		case 1:
			return Input.GetKeyDown(PlayerOptions.Instance.keyPlayer2Fire);
		case 2:
			return cInput.GetButtonDown("Fire_1");
		case 3:
			return cInput.GetButtonDown("Fire_2");
		case 4:
			return cInput.GetButtonDown("Fire_3");
		case 5:
			return cInput.GetButtonDown("Fire_4");
		default:
			return false;
		}
	}

	public static bool GetKeyDown(KeyCode key)
	{
		return !InputReader.IsBlocked && Input.GetKeyDown(key);
	}

	public static bool GetButtonDown(string button)
	{
		return !InputReader.IsBlocked && Input.GetButtonDown(button);
	}

	public static int GetControllerPressingSpecial()
	{
		if (InputReader.IsBlocked)
		{
			return -1;
		}
		if (Input.GetKeyDown(PlayerOptions.Instance.keyPlayer1Special))
		{
			return 0;
		}
		if (Input.GetKeyDown(PlayerOptions.Instance.keyPlayer2Special))
		{
			return 1;
		}
		if (cInput.GetButtonDown("Special_1"))
		{
			return 2;
		}
		if (cInput.GetButtonDown("Special_2"))
		{
			return 3;
		}
		if (cInput.GetButtonDown("Special_3"))
		{
			return 4;
		}
		if (cInput.GetButtonDown("Special_4"))
		{
			return 5;
		}
		return -1;
	}

	public static void GetDashDown()
	{
	}

	public static bool GetDashStart(int controllerNum)
	{
		if (controllerNum == 0)
		{
			return false;
		}
		if (controllerNum == 1)
		{
			return false;
		}
		if (controllerNum == 2)
		{
			return cInput.GetButton("Dash_1");
		}
		if (controllerNum == 3)
		{
			return cInput.GetButton("Dash_2");
		}
		if (controllerNum == 4)
		{
			return cInput.GetButton("Dash_3");
		}
		return controllerNum == 5 && cInput.GetButton("Dash_4");
	}

	public static void LoadKeys()
	{
		if (!InputReader.hasLoadedKeys)
		{
			PlayerOptions.InitializeOptions();
			InputReader.hasLoadedKeys = true;
		}
	}

	internal static bool IsControllerInUse(int controllerNum)
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.IsPlaying(i) && HeroController.playerControllerIDs[i] == controllerNum)
			{
				return true;
			}
		}
		return false;
	}

	internal static void GetInputIgnoreBlock(int controllerNum, ref bool up, ref bool down, ref bool left, ref bool right, ref bool fire, ref bool buttonJump, ref bool special, ref bool highFive)
	{
		if (!InputReader.hasLoadedKeys)
		{
			InputReader.LoadKeys();
		}
		if (controllerNum == 0)
		{
			InputReader.GetKeyboard1Input(ref up, ref down, ref left, ref right, ref fire, ref buttonJump, ref special, ref highFive);
		}
		if (controllerNum == 1)
		{
			InputReader.GetKeyboard2Input(ref up, ref down, ref left, ref right, ref fire, ref buttonJump, ref special, ref highFive);
		}
		if (controllerNum == 2)
		{
			InputReader.GetXBoxControllerInput("_1", ref up, ref down, ref left, ref right, ref fire, ref buttonJump, ref special, ref highFive);
		}
		if (controllerNum == 3)
		{
			InputReader.GetXBoxControllerInput("_2", ref up, ref down, ref left, ref right, ref fire, ref buttonJump, ref special, ref highFive);
		}
		if (controllerNum == 4)
		{
			InputReader.GetXBoxControllerInput("_3", ref up, ref down, ref left, ref right, ref fire, ref buttonJump, ref special, ref highFive);
		}
		if (controllerNum == 5)
		{
			InputReader.GetXBoxControllerInput("_4", ref up, ref down, ref left, ref right, ref fire, ref buttonJump, ref special, ref highFive);
		}
	}

	private static bool hasLoadedKeys;
}
