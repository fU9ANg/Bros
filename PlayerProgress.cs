// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress
{
	private PlayerProgress()
	{
	}

	public int GetLastFinishedLevel(bool offline)
	{
		if (offline)
		{
			UnityEngine.Debug.Log("getting lastFinishedLevel Offline");
			return this.lastFinishedLevelOffline;
		}
		UnityEngine.Debug.Log("getting lastFinishedLevel Online");
		return this.lastFinishedLevelOnline;
	}

	public void SetLastFinishedLevel(int level)
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"SetLastFinishedLevel ",
			level,
			"  ",
			Connect.IsOffline
		}));
		if (Connect.IsOffline)
		{
			this.lastFinishedLevelOffline = level;
		}
		else
		{
			this.lastFinishedLevelOnline = level;
		}
	}

	public static PlayerProgress Instance
	{
		get
		{
			try
			{
				if (PlayerProgress.instance == null)
				{
					PlayerProgress.Initialize();
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
			return PlayerProgress.instance;
		}
	}

	private static void Initialize()
	{
		try
		{
			PlayerProgress.instance = FileIO.LoadProgress();
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Failed to load player progress! " + ex.Message);
			PlayerProgress.instance = new PlayerProgress();
		}
	}

	internal bool IsHeroUnlocked(HeroType heroType)
	{
		return this.unlockedHeroes.Contains(heroType);
	}

	internal void UnlockHero(HeroType heroType)
	{
		if (!this.unlockedHeroes.Contains(heroType))
		{
			this.unlockedHeroes.Add(heroType);
			PlayerProgress.Save();
		}
	}

	internal void ClearHeroUnlocks()
	{
		this.unlockedHeroes.Clear();
		PlayerProgress.Save();
	}

	internal void LockHero(HeroType heroType)
	{
		if (this.unlockedHeroes.Contains(heroType))
		{
			this.unlockedHeroes.Remove(heroType);
		}
	}

	internal void FreeBro()
	{
		this.freedBros++;
		UnityEngine.Debug.Log("Extra Bro Freed");
		PlayerProgress.Save();
	}

	public static void Save()
	{
		UnityEngine.Debug.Log("Saving Progress...");
		FileIO.SaveProgress();
	}

	private static PlayerProgress instance;

	public int freedBros;

	public List<HeroType> unlockedHeroes = new List<HeroType>();

	public int lastFinishedLevelOffline = -1;

	public int lastFinishedLevelOnline = -1;

	public string lastOnlineLevelId;

	public int lastOnlineLevelProgress;
}
