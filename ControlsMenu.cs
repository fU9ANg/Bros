// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ControlsMenu : Menu
{
	private void GoBackToMainMenu()
	{
		this.MenuActive = false;
		this.optionsMenu.MenuActive = true;
		this.optionsMenu.TransitionIn();
	}

	private void SetKeyboard1()
	{
		this.bindingMenu.SetControlToBind(KeyBindingMenu.ControlToBind.Keyboard1);
		this.MenuActive = false;
		this.bindingMenu.MenuActive = true;
		this.bindingMenu.TransitionIn();
	}

	private void SetKeyboard2()
	{
		this.bindingMenu.SetControlToBind(KeyBindingMenu.ControlToBind.Keyboard2);
		this.MenuActive = false;
		this.bindingMenu.MenuActive = true;
		this.bindingMenu.TransitionIn();
	}

	private void SetGamePads()
	{
		this.bindingMenu.SetControlToBind(KeyBindingMenu.ControlToBind.Gamepad);
		this.MenuActive = false;
		this.bindingMenu.MenuActive = true;
		this.bindingMenu.TransitionIn();
	}

	public Menu optionsMenu;

	public KeyBindingMenu bindingMenu;

	public GameObject logo;
}
