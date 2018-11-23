// dnSpy decompiler from Assembly-CSharp.dll
using System;
using ManagedSteam;
using ManagedSteam.CallbackStructures;
using ManagedSteam.Exceptions;
using ManagedSteam.SteamTypes;
using UnityEngine;

public class SteamController : MonoBehaviour
{
	public static Steam SteamInterface
	{
		get
		{
			try
			{
				if (!SteamController.initialized)
				{
					SteamController.InitializeSteam();
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
			return SteamController.steamInterface;
		}
	}

	public static string PlayerNick
	{
		get
		{
			return SteamController.steamInterface.Friends.GetPersonaName();
		}
	}

	public static bool IsSteamEnabled()
	{
		return SteamController.SteamInterface != null;
	}

	private static void InitializeSteam()
	{
		if (!SteamController.initialized)
		{
			SteamController.initialized = true;
			bool flag = false;
			try
			{
				Steam.RestartAppIfNecessary(312990u);
				SteamController.steamInterface = Steam.Initialize();
			}
			catch (AlreadyLoadedException ex)
			{
				string message = "The native dll is already loaded, this should not happen if ReleaseManagedResources is used and Steam.Initialize() is only called once.";
				UnityEngine.Debug.LogError(message);
				UnityEngine.Debug.LogError(ex.Message);
				flag = true;
			}
			catch (SteamInitializeFailedException ex2)
			{
				string message2 = "Could not initialize the native Steamworks API. This is usually caused by a missing steam_appid.txt file or if the Steam client is not running.";
				UnityEngine.Debug.LogError(message2);
				UnityEngine.Debug.LogError(ex2.Message);
				flag = true;
			}
			catch (SteamInterfaceInitializeFailedException ex3)
			{
				string message3 = "Could not initialize the wanted versions of the Steamworks API. Make sure that you have the correct Steamworks SDK version. See the documentation for more info.";
				UnityEngine.Debug.LogError(message3);
				UnityEngine.Debug.LogError(ex3.Message);
				flag = true;
			}
			catch (DllNotFoundException ex4)
			{
				string message4 = "Could not load a dll file. Make sure that the steam_api.dll/libsteam_api.dylib file is placed at the correct location. See the documentation for more info.";
				UnityEngine.Debug.LogError(message4);
				UnityEngine.Debug.LogError(ex4.Message);
				flag = true;
			}
			if (flag)
			{
				SteamController.steamInterface = null;
			}
			else
			{
				SteamController.steamInterface.ExceptionThrown += SteamController.ExceptionThrown;
				SteamController.steamInterface.Friends.GameOverlayActivated += SteamController.OverlayToggle;
				SteamController.hasLicense = SteamController.steamInterface.User.UserHasLicenseForApp(SteamController.steamInterface.User.GetSteamID(), SteamController.steamInterface.AppID);
				if (SteamController.hasLicense != UserHasLicenseForAppResult.HasLicense)
				{
					UnityEngine.Debug.LogError("User does not have license for app! Go buy broforce already (" + SteamController.hasLicense.ToString() + ")");
				}
			}
		}
	}

	private static void OverlayToggle(GameOverlayActivated value)
	{
		if (value.Active)
		{
			UnityEngine.Debug.Log("Overlay shown");
		}
		else
		{
			UnityEngine.Debug.Log("Overlay closed");
		}
	}

	private static void ExceptionThrown(Exception e)
	{
		UnityEngine.Debug.LogError(string.Concat(new string[]
		{
			"Steam Exception: ",
			e.GetType().Name,
			": ",
			e.Message,
			"\n",
			e.StackTrace
		}));
	}

	private void Update()
	{
		if (SteamController.steamInterface != null)
		{
			SteamController.steamInterface.Update();
		}
	}

	private static bool initialized;

	private static Steam steamInterface;

	private static UserHasLicenseForAppResult hasLicense;
}
