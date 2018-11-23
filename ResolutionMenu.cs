// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionMenu : Menu
{
	protected override void SetupItems()
	{
		List<MenuBarItem> list = new List<MenuBarItem>();
		foreach (Resolution resolution in Screen.resolutions)
		{
			list.Add(new MenuBarItem
			{
				color = Color.white,
				size = this.characterSizes,
				name = resolution.width.ToString() + "x" + resolution.height,
				invokeMethod = "SetResolution"
			});
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

	private void SetResolution()
	{
		Resolution resolution = Screen.resolutions[this.highlightIndex];
		PlayerOptions.Instance.resolutionW = resolution.width;
		PlayerOptions.Instance.resolutionH = resolution.height;
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}

	private void GoBackToMainMenu()
	{
		this.MenuActive = false;
		this.optionsMenu.MenuActive = true;
		this.optionsMenu.TransitionIn();
	}

	public Menu optionsMenu;
}
