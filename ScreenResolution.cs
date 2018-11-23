// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ScreenResolution : MonoBehaviour
{
	private void Awake()
	{
		if (Application.platform != RuntimePlatform.WindowsEditor && !ScreenResolution.screenResolutionSet)
		{
			if (PlayerOptions.Instance.resolutionW != 0)
			{
				Screen.SetResolution(PlayerOptions.Instance.resolutionW, PlayerOptions.Instance.resolutionH, PlayerOptions.Instance.fullscreen);
			}
			else
			{
				Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
			}
			ScreenResolution.screenResolutionSet = true;
		}
	}

	private static bool screenResolutionSet;
}
