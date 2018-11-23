// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroUnlockController
{
	public static Dictionary<int, HeroType> heroUnlockIntervals
	{
		get
		{
			if (HeroUnlockController._heroUnlockIntervals == null)
			{
				if (PlaytomicController.isExhibitionBuild)
				{
					HeroUnlockController._heroUnlockIntervals = new Dictionary<int, HeroType>();
					HeroUnlockController._heroUnlockIntervals.Add(0, HeroType.Rambro);
					List<HeroType> list = new List<HeroType>
					{
						HeroType.Brommando,
						HeroType.BaBroracus,
						HeroType.BrodellWalker,
						HeroType.BroHard,
						HeroType.SnakeBroSkin
					};
					int num = 1;
					while (list.Count > 2)
					{
						HeroType heroType = list[UnityEngine.Random.Range(0, list.Count)];
						list.Remove(heroType);
						HeroUnlockController._heroUnlockIntervals.Add(num, heroType);
						num += 2 + Mathf.Clamp(HeroUnlockController._heroUnlockIntervals.Count / 3, 0, 1);
					}
					list.Add(HeroType.McBrover);
					list.Add(HeroType.Blade);
					list.Add(HeroType.BroDredd);
					list.Add(HeroType.ColJamesBroddock);
					list.Add(HeroType.Brononymous);
					list.Add(HeroType.Brominator);
					list.Add(HeroType.AshBrolliams);
					list.Add(HeroType.TimeBroVanDamme);
					while (list.Count > 6)
					{
						HeroType heroType2 = list[UnityEngine.Random.Range(0, list.Count)];
						list.Remove(heroType2);
						HeroUnlockController._heroUnlockIntervals.Add(num, heroType2);
						num += 3;
					}
					list.Add(HeroType.Brobocop);
					list.Add(HeroType.IndianaBrones);
					list.Add(HeroType.Nebro);
					list.Add(HeroType.BoondockBros);
					list.Add(HeroType.Brochete);
					list.Add(HeroType.BronanTheBrobarian);
					list.Add(HeroType.EllenRipbro);
					list.Add(HeroType.BroniversalSoldier);
					while (list.Count > 0)
					{
						HeroType heroType3 = list[UnityEngine.Random.Range(0, list.Count)];
						list.Remove(heroType3);
						HeroUnlockController._heroUnlockIntervals.Add(num, heroType3);
						num += 3 + HeroUnlockController._heroUnlockIntervals.Count / 13;
					}
				}
				else
				{
					HeroUnlockController._heroUnlockIntervals = new Dictionary<int, HeroType>
					{
						{
							0,
							HeroType.BroneyRoss
						},
						{
							1,
							HeroType.LeeBroxmas
						},
						{
							4,
							HeroType.BronnarJensen
						},
						{
							7,
							HeroType.HaleTheBro
						},
						{
							12,
							HeroType.Broc
						},
						{
							17,
							HeroType.TollBroad
						},
						{
							25,
							HeroType.TrentBroser
						}
					};
				}
			}
			return HeroUnlockController._heroUnlockIntervals;
		}
	}

	public static int GetNumberOfRescuesToNextUnlock()
	{
		foreach (KeyValuePair<int, HeroType> keyValuePair in HeroUnlockController.heroUnlockIntervals)
		{
			int key = keyValuePair.Key;
			if (PlayerProgress.Instance.freedBros < key)
			{
				return key - PlayerProgress.Instance.freedBros;
			}
		}
		return -1;
	}

	public static int GetNumberOfRescuesFromSinceUnlock()
	{
		int num = 0;
		foreach (KeyValuePair<int, HeroType> keyValuePair in HeroUnlockController.heroUnlockIntervals)
		{
			int key = keyValuePair.Key;
			if (PlayerProgress.Instance.freedBros < key)
			{
				return PlayerProgress.Instance.freedBros - num;
			}
			num = key;
		}
		return 0;
	}

	public static HeroType GetNextHeroToBeUnlocked()
	{
		foreach (KeyValuePair<int, HeroType> keyValuePair in HeroUnlockController.heroUnlockIntervals)
		{
			int key = keyValuePair.Key;
			if (PlayerProgress.Instance.freedBros < key)
			{
				return keyValuePair.Value;
			}
		}
		return HeroType.None;
	}

	public static List<HeroType> GetUnlockedHeroes()
	{
		List<HeroType> list = new List<HeroType>();
		int num = 31;
		for (int i = 0; i < num; i++)
		{
			HeroType heroType = (HeroType)i;
			if (HeroUnlockController.IsUnlocked(heroType))
			{
				if ((GameModeController.GameMode != GameMode.BroDown && !GameModeController.IsDeathMatchMode && GameModeController.GameMode != GameMode.ExplosionRun && GameModeController.GameMode != GameMode.Race) || HeroUnlockController.IsDeathMatchBro(heroType))
				{
					list.Add(heroType);
				}
			}
		}
		return list;
	}

	protected static bool IsDeathMatchBro(HeroType nextHeroType)
	{
		if (GameModeController.IsDeathMatchMode || GameModeController.GameMode == GameMode.ExplosionRun || GameModeController.GameMode == GameMode.BroDown || GameModeController.GameMode == GameMode.Race)
		{
			if (nextHeroType == HeroType.Rambro)
			{
				return true;
			}
			if (nextHeroType == HeroType.Brominator)
			{
				return true;
			}
			if (nextHeroType == HeroType.Brommando)
			{
				return true;
			}
			if (nextHeroType == HeroType.BaBroracus)
			{
				return true;
			}
			if (nextHeroType == HeroType.Blade)
			{
				return true;
			}
			if (nextHeroType == HeroType.BroDredd)
			{
				return true;
			}
			if (nextHeroType == HeroType.BroHard)
			{
				return true;
			}
			if (nextHeroType == HeroType.Brononymous)
			{
				return true;
			}
			if (nextHeroType == HeroType.BrodellWalker)
			{
				return true;
			}
			if (nextHeroType == HeroType.McBrover)
			{
				return true;
			}
			if (nextHeroType == HeroType.SnakeBroSkin)
			{
				return true;
			}
			if (nextHeroType == HeroType.Brobocop)
			{
				return true;
			}
			if (nextHeroType == HeroType.IndianaBrones)
			{
				return true;
			}
			if (nextHeroType == HeroType.AshBrolliams)
			{
				return true;
			}
			if (nextHeroType == HeroType.Nebro)
			{
				return true;
			}
			if (nextHeroType == HeroType.Brochete)
			{
				return true;
			}
			if (nextHeroType == HeroType.BronanTheBrobarian)
			{
				return true;
			}
			if (nextHeroType == HeroType.TimeBroVanDamme)
			{
				return true;
			}
			if (nextHeroType == HeroType.ColJamesBroddock)
			{
				return true;
			}
			if (nextHeroType == HeroType.BroniversalSoldier)
			{
				return true;
			}
			if (nextHeroType == HeroType.BroneyRoss)
			{
				return true;
			}
			if (nextHeroType == HeroType.LeeBroxmas)
			{
				return true;
			}
			if (nextHeroType == HeroType.BronnarJensen)
			{
				return true;
			}
			if (nextHeroType == HeroType.HaleTheBro)
			{
				return true;
			}
			if (nextHeroType == HeroType.TrentBroser)
			{
				return true;
			}
			if (nextHeroType == HeroType.Broc)
			{
				return true;
			}
			if (nextHeroType == HeroType.TollBroad)
			{
				return true;
			}
		}
		return false;
	}

	public static void FreeBro()
	{
		if (!LevelSelectionController.loadCustomCampaign)
		{
			PlayerProgress.Instance.FreeBro();
		}
		HeroUnlockController.CheckFreedBrosUnlocks(true);
	}

	public static void UnlockAllBros()
	{
		PlayerProgress.Instance.freedBros = 99999;
		HeroUnlockController.CheckFreedBrosUnlocks(true);
	}

	public static void Initialize()
	{
		HeroType nextHeroType = HeroController.nextHeroType;
		HeroUnlockController.CheckFreedBrosUnlocks(false);
		if (nextHeroType != HeroType.Random && nextHeroType != HeroType.None)
		{
			HeroController.nextHeroType = nextHeroType;
		}
		else
		{
			HeroController.nextHeroType = HeroType.None;
		}
	}

	public static void MakeSureTheresEnoughUnlockedBrosForAllTheJoinedPlayers()
	{
		int playersPlayingCount = HeroController.GetPlayersPlayingCount();
		int[] array = HeroUnlockController.heroUnlockIntervals.Keys.ToArray<int>();
		int num = array[playersPlayingCount] - 1;
		if (PlayerProgress.Instance.freedBros < num)
		{
			PlayerProgress.Instance.freedBros = num;
			HeroUnlockController.CheckFreedBrosUnlocks(false);
		}
	}

	protected static void CheckFreedBrosUnlocks(bool showAnouncements = true)
	{
		string arg = "CheckFreedBrosUnlocks " + PlayerProgress.Instance.freedBros + " ";
		foreach (KeyValuePair<int, HeroType> keyValuePair in HeroUnlockController.heroUnlockIntervals)
		{
			int key = keyValuePair.Key;
			HeroType value = keyValuePair.Value;
			if (PlayerProgress.Instance.freedBros >= key)
			{
				arg = arg + value + " ";
				if (!HeroUnlockController.IsUnlocked(value))
				{
					HeroUnlockController.Unlock(value);
					try
					{
						if (showAnouncements)
						{
							Announcer.PlayHeroName(value, 0.2f, false);
						}
						HeroController.nextHeroType = value;
						if (showAnouncements)
						{
							string text = "unlocked " + HeroController.GetHeroName(value) + "!";
							InfoBar.Appear(4f, text, new Color(1f, 1f, 1f, 0.4f), 0.8f);
						}
					}
					catch (Exception exception)
					{
						UnityEngine.Debug.LogException(exception);
					}
				}
			}
		}
	}

	public static void ClearUnlocks()
	{
		PlayerProgress.Instance.freedBros = 0;
		PlayerProgress.Instance.ClearHeroUnlocks();
		if (PlaytomicController.isExhibitionBuild)
		{
			HeroUnlockController._heroUnlockIntervals = null;
		}
	}

	public static bool IsUnlocked(HeroType heroType)
	{
		return PlayerProgress.Instance.IsHeroUnlocked(heroType);
	}

	public static void Unlock(HeroType heroType)
	{
		PlayerProgress.Instance.UnlockHero(heroType);
	}

	public static void Lock(HeroType heroType)
	{
		PlayerProgress.Instance.LockHero(heroType);
	}

	private static Dictionary<int, HeroType> _heroUnlockIntervals;
}
