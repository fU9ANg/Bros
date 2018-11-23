// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindingMenu : Menu
{
	public void SetControlToBind(KeyBindingMenu.ControlToBind control)
	{
		this.controlToBind = control;
		this.InstantiateItems();
	}

	protected override void Update()
	{
		if (!this.bindingKey)
		{
			base.Update();
		}
		else
		{
			TextMesh textMesh = this.items[this.highlightIndex];
			string text = (Time.time % 0.5f >= 0.25f) ? "*PRESS KEY*" : "PRESS KEY";
			this.backDrops[this.highlightIndex].text = text;
			textMesh.text = text;
		}
	}

	protected override void SetupItems()
	{
		List<MenuBarItem> list = new List<MenuBarItem>();
		switch (this.controlToBind)
		{
		case KeyBindingMenu.ControlToBind.Keyboard1:
		case KeyBindingMenu.ControlToBind.Keyboard2:
			list.Add(new MenuBarItem
			{
				color = Color.white,
				size = this.characterSizes,
				name = "UP: " + this.GetCurrentBinding(this.controlToBind, InputKey.up),
				invokeMethod = "SetKeyUp"
			});
			list.Add(new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "DOWN: " + this.GetCurrentBinding(this.controlToBind, InputKey.down),
				invokeMethod = "SetKeyDown"
			});
			list.Add(new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "LEFT: " + this.GetCurrentBinding(this.controlToBind, InputKey.left),
				invokeMethod = "SetKeyLeft"
			});
			list.Add(new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "RIGHT: " + this.GetCurrentBinding(this.controlToBind, InputKey.right),
				invokeMethod = "SetKeyRight"
			});
			list.Add(new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "JUMP (CAN USE SAME KEY AS UP): " + this.GetCurrentBinding(this.controlToBind, InputKey.jump),
				invokeMethod = "SetKeyJump"
			});
			list.Add(new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "FIRE: " + this.GetCurrentBinding(this.controlToBind, InputKey.fire),
				invokeMethod = "SetKeyFire"
			});
			list.Add(new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "SPECIAL: " + this.GetCurrentBinding(this.controlToBind, InputKey.special),
				invokeMethod = "SetKeySpecial"
			});
			list.Add(new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "HIGH FIVE/MELEE/USE: " + this.GetCurrentBinding(this.controlToBind, InputKey.melee),
				invokeMethod = "SetKeyMelee"
			});
			break;
		case KeyBindingMenu.ControlToBind.Gamepad:
			list.Add(new MenuBarItem
			{
				color = Color.white,
				size = 16f,
				name = "CLEAR",
				invokeMethod = "ClearGamepadControls"
			});
			list.Add(new MenuBarItem
			{
				color = Color.white,
				size = 16f,
				name = "UP: " + this.GetCurrentBinding(this.controlToBind, InputKey.up),
				invokeMethod = "SetKeyUp"
			});
			list.Add(new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "DOWN: " + this.GetCurrentBinding(this.controlToBind, InputKey.down),
				invokeMethod = "SetKeyDown"
			});
			list.Add(new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "LEFT: " + this.GetCurrentBinding(this.controlToBind, InputKey.left),
				invokeMethod = "SetKeyLeft"
			});
			list.Add(new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "RIGHT: " + this.GetCurrentBinding(this.controlToBind, InputKey.right),
				invokeMethod = "SetKeyRight"
			});
			list.Add(new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "JUMP: " + this.GetCurrentBinding(this.controlToBind, InputKey.jump),
				invokeMethod = "SetKeyJump"
			});
			list.Add(new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "FIRE: " + this.GetCurrentBinding(this.controlToBind, InputKey.fire),
				invokeMethod = "SetKeyFire"
			});
			list.Add(new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "SPECIAL: " + this.GetCurrentBinding(this.controlToBind, InputKey.special),
				invokeMethod = "SetKeySpecial"
			});
			list.Add(new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "HIGH FIVE/MELEE/USE: " + this.GetCurrentBinding(this.controlToBind, InputKey.melee),
				invokeMethod = "SetKeyMelee"
			});
			list.Add(new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "DASH: " + this.GetCurrentBinding(this.controlToBind, InputKey.dash),
				invokeMethod = "SetKeyDash"
			});
			list.Add(new MenuBarItem
			{
				color = list[0].color,
				size = list[0].size,
				name = "START: " + this.GetCurrentBinding(this.controlToBind, InputKey.start),
				invokeMethod = "SetKeyStart"
			});
			break;
		}
		list.Add(new MenuBarItem
		{
			color = Color.white,
			size = this.characterSizes,
			name = "BACK",
			invokeMethod = "GoBackToMainMenu"
		});
		this.itemNames = list.ToArray();
	}

	private string GetCurrentBinding(KeyBindingMenu.ControlToBind controlToBind, InputKey inp)
	{
		if (controlToBind == KeyBindingMenu.ControlToBind.Keyboard1)
		{
			switch (inp)
			{
			case InputKey.up:
				return PlayerOptions.Instance.keyPlayer1Up.ToString();
			case InputKey.down:
				return PlayerOptions.Instance.keyPlayer1Down.ToString();
			case InputKey.left:
				return PlayerOptions.Instance.keyPlayer1Left.ToString();
			case InputKey.right:
				return PlayerOptions.Instance.keyPlayer1Right.ToString();
			case InputKey.jump:
				return PlayerOptions.Instance.keyPlayer1Jump.ToString();
			case InputKey.fire:
				return PlayerOptions.Instance.keyPlayer1Fire.ToString();
			case InputKey.special:
				return PlayerOptions.Instance.keyPlayer1Special.ToString();
			case InputKey.melee:
				return PlayerOptions.Instance.keyPlayer1HighFive.ToString();
			}
		}
		else if (controlToBind == KeyBindingMenu.ControlToBind.Keyboard2)
		{
			switch (inp)
			{
			case InputKey.up:
				return PlayerOptions.Instance.keyPlayer2Up.ToString();
			case InputKey.down:
				return PlayerOptions.Instance.keyPlayer2Down.ToString();
			case InputKey.left:
				return PlayerOptions.Instance.keyPlayer2Left.ToString();
			case InputKey.right:
				return PlayerOptions.Instance.keyPlayer2Right.ToString();
			case InputKey.jump:
				return PlayerOptions.Instance.keyPlayer2Jump.ToString();
			case InputKey.fire:
				return PlayerOptions.Instance.keyPlayer2Fire.ToString();
			case InputKey.special:
				return PlayerOptions.Instance.keyPlayer2Special.ToString();
			case InputKey.melee:
				return PlayerOptions.Instance.keyPlayer2HighFive.ToString();
			}
		}
		else if (controlToBind == KeyBindingMenu.ControlToBind.Gamepad)
		{
			switch (inp)
			{
			case InputKey.up:
				return this.GetJoystickBindControllerAgnostic("Up");
			case InputKey.down:
				return this.GetJoystickBindControllerAgnostic("Down");
			case InputKey.left:
				return this.GetJoystickBindControllerAgnostic("Left");
			case InputKey.right:
				return this.GetJoystickBindControllerAgnostic("Right");
			case InputKey.jump:
				return this.GetJoystickBindControllerAgnostic("Jump");
			case InputKey.fire:
				return this.GetJoystickBindControllerAgnostic("Fire");
			case InputKey.dash:
				return this.GetJoystickBindControllerAgnostic("Dash");
			case InputKey.special:
				return this.GetJoystickBindControllerAgnostic("Special");
			case InputKey.melee:
				return this.GetJoystickBindControllerAgnostic("Melee");
			case InputKey.start:
				return this.GetJoystickBindControllerAgnostic("Start");
			}
		}
		return "???";
	}

	private string GetJoystickBindControllerAgnostic(string action)
	{
		string text = cInput.GetText(action + "_1", 1);
		string text2 = cInput.GetText(action + "_1", 2);
		if (text.Equals("None"))
		{
			return "Unset";
		}
		if (text2.Equals("None"))
		{
			return this.StripStringFromFirstNumeric(text);
		}
		return this.StripStringFromFirstNumeric(text) + " / " + this.StripStringFromFirstNumeric(text2);
	}

	private string GetActionName(InputKey inp)
	{
		switch (inp)
		{
		case InputKey.up:
			return "Up";
		case InputKey.down:
			return "Down";
		case InputKey.left:
			return "Left";
		case InputKey.right:
			return "Right";
		case InputKey.jump:
			return "Jump";
		case InputKey.fire:
			return "Fire";
		case InputKey.dash:
			return "Dash";
		case InputKey.special:
			return "Special";
		case InputKey.melee:
			return "Melee";
		case InputKey.start:
			return "Start";
		default:
			return "????";
		}
	}

	private bool DoesActionHavePrimaryBinding(string action)
	{
		return !cInput.GetText(action + "_1", 1).Equals("None");
	}

	private bool DoesActionHaveSecondaryBinding(string action)
	{
		return !cInput.GetText(action + "_1", 2).Equals("None");
	}

	private string StripStringFromFirstNumeric(string s)
	{
		char[] anyOf = new char[]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9'
		};
		int num = s.IndexOfAny(anyOf, 0) + 1;
		return s.Substring(num, s.Length - num);
	}

	private void GoBackToMainMenu()
	{
		this.MenuActive = false;
		this.optionsMenu.MenuActive = true;
		this.optionsMenu.TransitionIn();
	}

	private void SetKeyUp()
	{
		this.bindingKey = true;
		if (this.controlToBind == KeyBindingMenu.ControlToBind.Gamepad)
		{
			base.StartCoroutine(this.BindGamepadButton(InputKey.up));
		}
		else
		{
			base.StartCoroutine(this.BindKeyboardKey(InputKey.up));
		}
	}

	private void SetKeyDown()
	{
		this.bindingKey = true;
		if (this.controlToBind == KeyBindingMenu.ControlToBind.Gamepad)
		{
			base.StartCoroutine(this.BindGamepadButton(InputKey.down));
		}
		else
		{
			base.StartCoroutine(this.BindKeyboardKey(InputKey.down));
		}
	}

	private void SetKeyLeft()
	{
		this.bindingKey = true;
		if (this.controlToBind == KeyBindingMenu.ControlToBind.Gamepad)
		{
			base.StartCoroutine(this.BindGamepadButton(InputKey.left));
		}
		else
		{
			base.StartCoroutine(this.BindKeyboardKey(InputKey.left));
		}
	}

	private void SetKeyRight()
	{
		this.bindingKey = true;
		if (this.controlToBind == KeyBindingMenu.ControlToBind.Gamepad)
		{
			base.StartCoroutine(this.BindGamepadButton(InputKey.right));
		}
		else
		{
			base.StartCoroutine(this.BindKeyboardKey(InputKey.right));
		}
	}

	private void SetKeyFire()
	{
		this.bindingKey = true;
		if (this.controlToBind == KeyBindingMenu.ControlToBind.Gamepad)
		{
			base.StartCoroutine(this.BindGamepadButton(InputKey.fire));
		}
		else
		{
			base.StartCoroutine(this.BindKeyboardKey(InputKey.fire));
		}
	}

	private void SetKeySpecial()
	{
		this.bindingKey = true;
		if (this.controlToBind == KeyBindingMenu.ControlToBind.Gamepad)
		{
			base.StartCoroutine(this.BindGamepadButton(InputKey.special));
		}
		else
		{
			base.StartCoroutine(this.BindKeyboardKey(InputKey.special));
		}
	}

	private void SetKeyJump()
	{
		this.bindingKey = true;
		if (this.controlToBind == KeyBindingMenu.ControlToBind.Gamepad)
		{
			base.StartCoroutine(this.BindGamepadButton(InputKey.jump));
		}
		else
		{
			base.StartCoroutine(this.BindKeyboardKey(InputKey.jump));
		}
	}

	private void SetKeyMelee()
	{
		this.bindingKey = true;
		if (this.controlToBind == KeyBindingMenu.ControlToBind.Gamepad)
		{
			base.StartCoroutine(this.BindGamepadButton(InputKey.melee));
		}
		else
		{
			base.StartCoroutine(this.BindKeyboardKey(InputKey.melee));
		}
	}

	private void SetKeyDash()
	{
		this.bindingKey = true;
		if (this.controlToBind == KeyBindingMenu.ControlToBind.Gamepad)
		{
			base.StartCoroutine(this.BindGamepadButton(InputKey.dash));
		}
		else
		{
			base.StartCoroutine(this.BindKeyboardKey(InputKey.dash));
		}
	}

	private void SetKeyStart()
	{
		this.bindingKey = true;
		if (this.controlToBind == KeyBindingMenu.ControlToBind.Gamepad)
		{
			base.StartCoroutine(this.BindGamepadButton(InputKey.start));
		}
	}

	private IEnumerator BindGamepadButton(InputKey inp)
	{
		if (cInput.IsKeyDefined("FakeAction"))
		{
			cInput.ChangeKey("FakeAction", "None", "None");
		}
		else
		{
			cInput.SetKey("FakeAction", "None", "None");
		}
		cInput.allowDuplicates = true;
		this.bindingKey = true;
		yield return null;
		string action = this.GetActionName(inp);
		cInput.allowDuplicates = true;
		bool bindingPrimary = !this.DoesActionHavePrimaryBinding(action) || this.DoesActionHaveSecondaryBinding(action);
		string oldPrimary = cInput.GetText(action + "_1", 1);
		cInput.ChangeKey("FakeAction", 1, false, false, true, true, false);
		while (cInput.scanning)
		{
			yield return null;
		}
		yield return null;
		string newButton = cInput.GetText("FakeAction", 1);
		if (bindingPrimary)
		{
			cInput.ChangeKey(action + "_1", this.ReplaceFirstNumericWith(newButton, '1'), "None");
		}
		else
		{
			cInput.ChangeKey(action + "_1", oldPrimary, this.ReplaceFirstNumericWith(newButton, '1'));
		}
		this.CopyGamepadOneToAll();
		int index = this.highlightIndex;
		this.InstantiateItems();
		this.highlightIndex = index;
		this.lastInputTime = Time.time + 0.2f;
		this.bindingKey = false;
		yield return null;
		yield break;
	}

	private void ClearGamepadControls()
	{
		cInput.ChangeKey("Up_1", "None", "None");
		cInput.ChangeKey("Down_1", "None", "None");
		cInput.ChangeKey("Left_1", "None", "None");
		cInput.ChangeKey("Right_1", "None", "None");
		cInput.ChangeKey("Jump_1", "None", "None");
		cInput.ChangeKey("Fire_1", "None", "None");
		cInput.ChangeKey("Special_1", "None", "None");
		cInput.ChangeKey("Melee_1", "None", "None");
		cInput.ChangeKey("Dash_1", "None", "None");
		cInput.ChangeKey("Start_1", "None", "None");
		this.CopyGamepadOneToAll();
		this.InstantiateItems();
	}

	private void CopyGamepadOneToAll()
	{
		this.CopyGamepadActionToAll("Up");
		this.CopyGamepadActionToAll("Down");
		this.CopyGamepadActionToAll("Left");
		this.CopyGamepadActionToAll("Right");
		this.CopyGamepadActionToAll("Jump");
		this.CopyGamepadActionToAll("Fire");
		this.CopyGamepadActionToAll("Special");
		this.CopyGamepadActionToAll("Melee");
		this.CopyGamepadActionToAll("Dash");
		this.CopyGamepadActionToAll("Start");
	}

	private void CopyGamepadActionToAll(string action)
	{
		string text = cInput.GetText(action + "_1", 1);
		string text2 = cInput.GetText(action + "_1", 2);
		for (char c = '2'; c <= '4'; c += '\u0001')
		{
			text = ((!text.Equals("None")) ? this.ReplaceFirstNumericWith(text, c) : "None");
			text2 = ((!text2.Equals("None")) ? this.ReplaceFirstNumericWith(text2, c) : "None");
			cInput.ChangeKey(action + "_" + c, text, text2);
		}
	}

	private string ReplaceFirstNumericWith(string s, char i)
	{
		char[] anyOf = new char[]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9'
		};
		int num = s.IndexOfAny(anyOf, 0);
		char[] array = s.ToCharArray();
		array[num] = i;
		return new string(array);
	}

	private IEnumerator BindKeyboardKey(InputKey inp)
	{
		float bindWaitDelay = 0.02f;
		while ((bindWaitDelay -= Time.deltaTime) > 0f)
		{
			yield return null;
		}
		KeyCode receivedKey = KeyCode.None;
		this.bindingKey = true;
		while (receivedKey == KeyCode.None)
		{
			foreach (KeyCode key in (KeyCode[])Enum.GetValues(typeof(KeyCode)))
			{
				if (Input.GetKeyDown(key))
				{
					receivedKey = key;
				}
			}
			yield return null;
		}
		if (receivedKey != KeyCode.Escape)
		{
			if (this.controlToBind == KeyBindingMenu.ControlToBind.Keyboard1)
			{
				switch (inp)
				{
				case InputKey.up:
					PlayerOptions.Instance.keyPlayer1Up = receivedKey;
					break;
				case InputKey.down:
					PlayerOptions.Instance.keyPlayer1Down = receivedKey;
					break;
				case InputKey.left:
					PlayerOptions.Instance.keyPlayer1Left = receivedKey;
					break;
				case InputKey.right:
					PlayerOptions.Instance.keyPlayer1Right = receivedKey;
					break;
				case InputKey.jump:
					PlayerOptions.Instance.keyPlayer1Jump = receivedKey;
					break;
				case InputKey.fire:
					PlayerOptions.Instance.keyPlayer1Fire = receivedKey;
					break;
				case InputKey.special:
					PlayerOptions.Instance.keyPlayer1Special = receivedKey;
					break;
				case InputKey.melee:
					PlayerOptions.Instance.keyPlayer1HighFive = receivedKey;
					break;
				}
			}
			else if (this.controlToBind == KeyBindingMenu.ControlToBind.Keyboard2)
			{
				switch (inp)
				{
				case InputKey.up:
					PlayerOptions.Instance.keyPlayer2Up = receivedKey;
					break;
				case InputKey.down:
					PlayerOptions.Instance.keyPlayer2Down = receivedKey;
					break;
				case InputKey.left:
					PlayerOptions.Instance.keyPlayer2Left = receivedKey;
					break;
				case InputKey.right:
					PlayerOptions.Instance.keyPlayer2Right = receivedKey;
					break;
				case InputKey.jump:
					PlayerOptions.Instance.keyPlayer2Jump = receivedKey;
					break;
				case InputKey.fire:
					PlayerOptions.Instance.keyPlayer2Fire = receivedKey;
					break;
				case InputKey.special:
					PlayerOptions.Instance.keyPlayer2Special = receivedKey;
					break;
				case InputKey.melee:
					PlayerOptions.Instance.keyPlayer2HighFive = receivedKey;
					break;
				}
			}
		}
		int index = this.highlightIndex;
		this.InstantiateItems();
		this.highlightIndex = index;
		this.lastInputTime = Time.time + 0.2f;
		this.bindingKey = false;
		yield break;
	}

	public Menu optionsMenu;

	public GameObject logo;

	private KeyBindingMenu.ControlToBind controlToBind;

	private bool bindingKey;

	public enum ControlToBind
	{
		Keyboard1,
		Keyboard2,
		Gamepad
	}
}
