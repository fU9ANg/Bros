// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Playtomic : MonoBehaviour
{
	public static void Initialize(string publickey, string privatekey, string apiurl)
	{
		if (Playtomic._instance != null)
		{
			return;
		}
		GameObject gameObject = new GameObject("playtomic");
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		Playtomic._instance = (gameObject.AddComponent(typeof(Playtomic)) as Playtomic);
		Playtomic._instance._leaderboards = new PLeaderboards();
		Playtomic._instance._playerlevels = new PPlayerLevels();
		Playtomic._instance._geoip = new PGeoIP();
		Playtomic._instance._gamevars = new PGameVars();
		Playtomic._instance._achievements = new PAchievements();
		Playtomic._instance._newsletter = new PNewsletter();
		PRequest.Initialise(publickey, privatekey, apiurl);
	}

	internal static Playtomic API
	{
		get
		{
			return Playtomic._instance;
		}
	}

	public static PLeaderboards Leaderboards
	{
		get
		{
			return Playtomic._instance._leaderboards;
		}
	}

	public static PPlayerLevels PlayerLevels
	{
		get
		{
			return Playtomic._instance._playerlevels;
		}
	}

	public static PGeoIP GeoIP
	{
		get
		{
			return Playtomic._instance._geoip;
		}
	}

	public static PGameVars GameVars
	{
		get
		{
			return Playtomic._instance._gamevars;
		}
	}

	public static PAchievements Achievements
	{
		get
		{
			return Playtomic._instance._achievements;
		}
	}

	public static PNewsletter Newsletter
	{
		get
		{
			return Playtomic._instance._newsletter;
		}
	}

	private PLeaderboards _leaderboards;

	private PPlayerLevels _playerlevels;

	private PGeoIP _geoip;

	private PGameVars _gamevars;

	private PAchievements _achievements;

	private PNewsletter _newsletter;

	private static Playtomic _instance;
}
