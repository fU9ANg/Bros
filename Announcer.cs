// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Announcer : MonoBehaviour
{
	public static bool HasInstance()
	{
		return Announcer.instance != null;
	}

	public static bool HadRecentAnnouncement()
	{
		return Time.time - Announcer.lastAnnouncementTime < 0.7f;
	}

	private void Awake()
	{
		Announcer.currentAnouncement = string.Empty;
		if (Announcer.instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			Announcer.instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(this);
		}
	}

	private void Update()
	{
		if (Announcer.goDelay > 0f)
		{
			Announcer.goDelay -= Time.deltaTime;
			if (Announcer.goDelay <= 0f)
			{
				Announcer.PlayGo(0.25f, 1f, 0f);
			}
		}
	}

	public static void AnnounceBroforce()
	{
	}

	public void BROFORCE()
	{
	}

	public static void AnnounceBroforce(float v, float p)
	{
		UnityEngine.Debug.Log(Sound.GetInstance() != null);
		if (Announcer.instance != null && Announcer.instance.soundHolder != null)
		{
			Sound.GetInstance().PlayAnnouncer(Announcer.instance.soundHolder.broforce, v, p, 0f);
		}
	}

	public static void AnnounceFailure(float v, float p)
	{
		if (Announcer.instance != null && Announcer.instance.soundHolder != null)
		{
			if (GameModeController.campaignLevelFailCount == 3 || (GameModeController.campaignLevelFailCount > 3 && UnityEngine.Random.value > 0.3f))
			{
				UnityEngine.Debug.Log("NO!! ");
				int num = UnityEngine.Random.Range(0, Announcer.instance.soundHolder.missionFailedDesperate.Length);
				int num2 = num;
				if (num2 != 0)
				{
					if (num2 != 1)
					{
						Announcer.currentAnouncement = "NOOOOOO!";
					}
					else
					{
						Announcer.currentAnouncement = "NOOOOOO!";
					}
				}
				else
				{
					Announcer.currentAnouncement = "NOOOOOO!";
				}
				Announcer.PlayClip(Announcer.instance.soundHolder.missionFailedDesperate[num], v, p, 0f);
			}
			else
			{
				int num3 = UnityEngine.Random.Range(0, Announcer.instance.soundHolder.missionFailed.Length);
				if (LevelSelectionController.CurrentLevelNum < 1 && num3 > 4)
				{
					num3 %= 5;
				}
				switch (num3)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
					Announcer.currentAnouncement = "MISSION FAILED!";
					break;
				case 5:
				case 6:
					Announcer.currentAnouncement = "REST IN PEACE BRO!";
					break;
				}
				Announcer.PlayClip(Announcer.instance.soundHolder.missionFailed[num3], v, p, 0f);
			}
		}
	}

	public static void AnnounceBroSelect(float v, float p, float delay)
	{
		if (Announcer.instance != null && Announcer.instance.soundHolder != null)
		{
			UnityEngine.Debug.Log(Sound.GetInstance() != null);
			Sound.GetInstance().PlaySoundEffectAt(Announcer.instance.soundHolder.chooseYourBro, v, Camera.main.transform.position, p, true, delay);
		}
	}

	public static void PlayGo(float v, float p, float delay)
	{
		if (Announcer.instance != null && Announcer.instance.soundHolder != null)
		{
			if (GameModeController.campaignLevelFailCount > 2)
			{
				Announcer.currentAnouncement = "GO!";
				Announcer.PlayClip(Announcer.instance.soundHolder.goDesperate[Announcer.countDownCount % Announcer.instance.soundHolder.goDesperate.Length], v, p, delay);
			}
			else
			{
				int num = UnityEngine.Random.Range(0, Announcer.instance.soundHolder.go.Length);
				if (GameModeController.campaignLevelFailCount == 0 && (UnityEngine.Random.value > 0.7f || LevelSelectionController.CurrentLevelNum == 0))
				{
					num = 0;
				}
				if (GameModeController.campaignLevelFailCount == 0 && (num == 3 || num == 4))
				{
					num %= 3;
				}
				else if (GameModeController.campaignLevelFailCount == 2)
				{
					num = 3 + UnityEngine.Random.Range(0, 2);
				}
				switch (num)
				{
				case 0:
					Announcer.currentAnouncement = "GO!";
					break;
				case 1:
				case 2:
					Announcer.currentAnouncement = "LET'S GO!";
					break;
				case 3:
				case 4:
					Announcer.currentAnouncement = "GO GO GO!";
					break;
				}
				Announcer.PlayClip(Announcer.instance.soundHolder.go[num], v, p, delay);
			}
		}
	}

	protected static void PlayClip(AudioClip clip, float v, float p, float delay)
	{
		if (Announcer.instance != null)
		{
			Announcer.lastAnnouncementTime = Time.time;
			Vector3 vector = Vector3.zero;
			if (SortOfFollow.GetInstance() != null)
			{
				vector = SortOfFollow.GetInstance().transform.position;
			}
			else
			{
				vector = Camera.main.transform.position;
			}
			Sound.GetInstance().PlayAnnouncer(clip, v, p, delay);
		}
	}

	protected static void PlayClips(AudioClip[] clips, float v, float p)
	{
		if (Announcer.instance != null)
		{
			Vector3 vector = Vector3.zero;
			if (SortOfFollow.GetInstance() != null)
			{
				vector = SortOfFollow.GetInstance().transform.position;
			}
			else
			{
				vector = Camera.main.transform.position;
			}
			Sound.GetInstance().PlayAnnouncer(clips, v, p, 0f);
		}
	}

	public static void Announce3(float v, float p)
	{
		if (Announcer.instance != null && Announcer.instance.soundHolder != null && !Map.isEditing && !LevelEditorGUI.IsActive)
		{
			Announcer.countDownCount++;
			if (GameModeController.campaignLevelFailCount > 2)
			{
				Announcer.PlayClip(Announcer.instance.soundHolder.start3Desperate[Announcer.countDownCount % Announcer.instance.soundHolder.start3Desperate.Length], v, p, 0f);
			}
			else
			{
				Announcer.PlayClips(Announcer.instance.soundHolder.start3, v, p);
			}
		}
	}

	public static void Announce2(float v, float p)
	{
		if (Announcer.instance != null && Announcer.instance.soundHolder != null && !Map.isEditing && !LevelEditorGUI.IsActive)
		{
			if (GameModeController.campaignLevelFailCount > 2)
			{
				Announcer.PlayClip(Announcer.instance.soundHolder.start2Desperate[Announcer.countDownCount % Announcer.instance.soundHolder.start2Desperate.Length], v, p, 0f);
			}
			else
			{
				Announcer.PlayClips(Announcer.instance.soundHolder.start2, v, p);
			}
		}
	}

	public static void Announce1(float v, float p)
	{
		if (Announcer.instance != null && Announcer.instance.soundHolder != null && !Map.isEditing && !LevelEditorGUI.IsActive)
		{
			if (GameModeController.campaignLevelFailCount > 2)
			{
				Announcer.PlayClip(Announcer.instance.soundHolder.start1Desperate[Announcer.countDownCount % Announcer.instance.soundHolder.start1Desperate.Length], v, p, 0f);
			}
			else
			{
				Announcer.PlayClips(Announcer.instance.soundHolder.start1, v, p);
			}
		}
	}

	public static void CancelCurrentDelayedGo()
	{
		Announcer.goDelay = 0f;
	}

	public static void AnnounceGo(float v, float p, float delay)
	{
		if (Announcer.instance != null && Announcer.instance.soundHolder != null && !Map.isEditing && !LevelEditorGUI.IsActive)
		{
			Announcer.PlayGo(v, p, delay);
		}
	}

	public static void AnnounceRightOn(float v, float p, float delay)
	{
		if (Announcer.instance != null && Announcer.instance.soundHolder != null)
		{
			Vector3 pos = Vector3.zero;
			if (SortOfFollow.GetInstance() != null)
			{
				pos = SortOfFollow.GetInstance().transform.position;
			}
			else
			{
				pos = Camera.main.transform.position;
			}
			Sound.GetInstance().PlaySoundEffectAt(Announcer.instance.soundHolder.rightOn, v, pos, p, true, delay);
		}
	}

	protected static bool PlayHeroName(AudioClip clip)
	{
		if (clip != null)
		{
			Vector3 pos = Vector3.zero;
			if (SortOfFollow.GetInstance() != null)
			{
				pos = SortOfFollow.GetInstance().transform.position;
			}
			else
			{
				pos = Camera.main.transform.position;
			}
			Sound.GetInstance().PlaySoundEffectAt(clip, 0.3f, pos, 1f, true, 0.15f);
			return true;
		}
		return false;
	}

	protected static bool PlayHeroName(AudioClip[] clip)
	{
		if (clip != null)
		{
			Announcer.PlayHeroName(clip[UnityEngine.Random.Range(0, clip.Length)]);
			return true;
		}
		return false;
	}

	public static bool PlayHeroName(HeroType heroType, float volume, bool andExplosion = false)
	{
		if (Announcer.instance == null || Announcer.instance.soundHolder == null)
		{
			return false;
		}
		bool result = false;
		switch (heroType)
		{
		case HeroType.Rambro:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.Rambro);
			break;
		case HeroType.Brommando:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.Brommando);
			break;
		case HeroType.BaBroracus:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.BaBroracus);
			break;
		case HeroType.BrodellWalker:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.BrodellWalker);
			break;
		case HeroType.Blade:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.Blade);
			break;
		case HeroType.McBrover:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.McBrover);
			break;
		case HeroType.Brononymous:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.Brononymous);
			break;
		case HeroType.Brobocop:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.Brobocop);
			break;
		case HeroType.BroDredd:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.BroDredd);
			break;
		case HeroType.BroHard:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.BroHard);
			break;
		case HeroType.MadMaxBrotansky:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.Brommando);
			break;
		case HeroType.SnakeBroSkin:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.SnakeBroSkin);
			break;
		case HeroType.Brominator:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.Brominator);
			break;
		case HeroType.IndianaBrones:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.IndianaBrones);
			break;
		case HeroType.AshBrolliams:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.AshBrolliams);
			break;
		case HeroType.Nebro:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.Nebro);
			break;
		case HeroType.BoondockBros:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.BoondockBros);
			break;
		case HeroType.Brochete:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.random);
			break;
		case HeroType.BronanTheBrobarian:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.BronanTheBrobarian);
			break;
		case HeroType.EllenRipbro:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.EllenRipbro);
			break;
		case HeroType.CherryBroling:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.CherryBroling);
			break;
		case HeroType.TimeBroVanDamme:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.TimeCopVanDamme);
			break;
		case HeroType.ColJamesBroddock:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.ColJamesBroddock);
			break;
		case HeroType.BroniversalSoldier:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.BroniversalSoldier);
			break;
		case HeroType.BroneyRoss:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.BroneyRoss);
			break;
		case HeroType.LeeBroxmas:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.LeeBroxmas);
			break;
		case HeroType.BronnarJensen:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.BronnarJensen);
			break;
		case HeroType.HaleTheBro:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.HaleTheBro);
			break;
		case HeroType.TrentBroser:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.TrentBroser);
			break;
		case HeroType.Broc:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.Broc);
			break;
		case HeroType.TollBroad:
			result = Announcer.PlayHeroName(Announcer.instance.soundHolder.TollBroad);
			break;
		}
		Sound.GetInstance().PlaySoundEffect(Announcer.instance.soundHolder.explosionBig, 0.4f);
		return result;
	}

	public SoundHolderAnnouncer soundHolder;

	protected static Announcer instance;

	protected static float lastGoTime;

	protected static float goDelay;

	public static string currentAnouncement = string.Empty;

	protected static float lastAnnouncementTime;

	protected static int countDownCount;
}
