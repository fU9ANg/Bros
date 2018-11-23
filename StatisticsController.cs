// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StatisticsController : SingletonNetObj<StatisticsController>
{
	private void Awake()
	{
		SingletonNetObj<StatisticsController>.Instance.currentStats = new LevelStats();
		this.currentStats.destruction = 0;
	}

	private void Start()
	{
		this.levelStartTime = Time.time;
		this.currentStats.totalTime = -1f;
		StatisticsController.timePercentile = -1f; StatisticsController.brotalityPercentile = (StatisticsController.timePercentile );
		this.brotalityGrace = 6f;
		StatisticsController.brotalityLevel = 0;
		if (StatisticsController.ShowBrotalityScore())
		{
			Dictionary<int, LevelStats> campaignStats = StatisticsController.GetCampaignStats();
			if (campaignStats != null)
			{
				foreach (KeyValuePair<int, LevelStats> keyValuePair in campaignStats)
				{
					SingletonNetObj<StatisticsController>.Instance.currentStats.totalBrotality = keyValuePair.Value.totalBrotality;
					SingletonNetObj<StatisticsController>.Instance.currentStats.totalBrotalityPenalty = keyValuePair.Value.totalBrotalityPenalty;
				}
			}
		}
	}

	public static bool ShowBrotalityScore()
	{
		return LevelSelectionController.currentCampaign != null && LevelSelectionController.currentCampaign.header != null && !Map.isEditing && LevelSelectionController.currentCampaign.header.hasBrotalityScoreboard;
	}

	public static float GetBrotalometerValue()
	{
		return SingletonNetObj<StatisticsController>.Instance.brotalitometerValue;
	}

	public static float GetMusicIntensity()
	{
		return SingletonNetObj<StatisticsController>.Instance.musicIntensity;
	}

	public static int GetDeathsCount()
	{
		return SingletonNetObj<StatisticsController>.Instance.currentStats.deathList.Count;
	}

	public static void NotifyLevelLoaded()
	{
		SingletonNetObj<StatisticsController>.Instance.currentStats.mooksAtStart = StatisticsController.totalMooks;
	}

	public static float GetTime()
	{
		if (SingletonNetObj<StatisticsController>.Instance.currentStats.totalTime < 0f)
		{
			return Time.time - SingletonNetObj<StatisticsController>.Instance.levelStartTime;
		}
		return SingletonNetObj<StatisticsController>.Instance.currentStats.totalTime;
	}

	public static string GetTimeString()
	{
		return StatisticsController.GetTimeString(SingletonNetObj<StatisticsController>.Instance.currentStats.totalTime);
	}

	public static string GetTimeString(float time)
	{
		float num = (float)((int)(time / 60f));
		float num2 = (float)((int)(time - num * 60f));
		float num3 = (float)((int)((time - num * 60f - num2) * 100f));
		return string.Concat(new string[]
		{
			string.Empty,
			num.ToString("00"),
			":",
			num2.ToString("00"),
			".",
			num3.ToString("0")
		});
	}

	public static void RegisterMook(Mook mook)
	{
		if (mook.CanAddToStatistics())
		{
			StatisticsController.totalMooks++;
		}
	}

	public static void RegisterRescueBro()
	{
		SingletonNetObj<StatisticsController>.Instance.currentStats.totalCages++;
	}

	public static void CacheStats()
	{
		SingletonNetObj<StatisticsController>.Instance.currentStats.totalTime = StatisticsController.GetTime();
		if (StatisticsController.cachedStats == null)
		{
			StatisticsController.cachedStats = SingletonNetObj<StatisticsController>.Instance.currentStats;
		}
		else
		{
			StatisticsController.cachedStats.Add(SingletonNetObj<StatisticsController>.Instance.currentStats);
		}
		SingletonNetObj<StatisticsController>.Instance.currentStats = new LevelStats();
	}

	public static DeathObject GetDeathObject(int deathIndex)
	{
		if (SingletonNetObj<StatisticsController>.Instance.currentStats.deathList.Count > deathIndex)
		{
			return SingletonNetObj<StatisticsController>.Instance.currentStats.deathList[deathIndex];
		}
		return null;
	}

	public static void NotifyCaptureCheckPoint()
	{
		if (!GameModeController.LevelFinished)
		{
			SingletonNetObj<StatisticsController>.Instance.brotalityGrace = 5f;
			SingletonNetObj<StatisticsController>.Instance.brotalityGrace = 1f;
			SingletonNetObj<StatisticsController>.Instance.currentStats.totalBrotality += 3f;
		}
	}

	public static void NotifyTankDeath(Tank tank)
	{
		SingletonNetObj<StatisticsController>.Instance.lastKillTime = Time.time;
		if (!GameModeController.LevelFinished)
		{
			SingletonNetObj<StatisticsController>.Instance.brotalityGrace = 5f;
			SingletonNetObj<StatisticsController>.Instance.brotalityGrace = 1f;
			SingletonNetObj<StatisticsController>.Instance.currentStats.totalBrotality += 10f;
			SingletonNetObj<StatisticsController>.Instance.musicIntensity += 4f;
		}
	}

	public static void NotifyTruckDeath(Tank tank)
	{
		SingletonNetObj<StatisticsController>.Instance.lastKillTime = Time.time;
		if (!GameModeController.LevelFinished)
		{
			SingletonNetObj<StatisticsController>.Instance.brotalityGrace = 4f;
			SingletonNetObj<StatisticsController>.Instance.brotalityGrace = 1f;
			SingletonNetObj<StatisticsController>.Instance.currentStats.totalBrotality += 5f;
			SingletonNetObj<StatisticsController>.Instance.musicIntensity += 3f;
		}
	}

	public static void NotifyMookDeath(Mook mook)
	{
		if (mook.CanAddToStatistics())
		{
			SingletonNetObj<StatisticsController>.Instance.currentStats.mooksKilled++;
			if (!GameModeController.LevelFinished)
			{
				if (Time.time - SingletonNetObj<StatisticsController>.Instance.lastKillTime < 0.5f)
				{
					float a = 2.5f + (float)(4 - StatisticsController.GetBrotalityLevel()) * 0.33f;
					SingletonNetObj<StatisticsController>.Instance.brotalityGrace = Mathf.Max(a, SingletonNetObj<StatisticsController>.Instance.brotalityGrace);
					SingletonNetObj<StatisticsController>.Instance.brotalityGrace = 1.5f;
				}
				else
				{
					if (SingletonNetObj<StatisticsController>.Instance.brotalityGrace < 1f)
					{
						SingletonNetObj<StatisticsController>.Instance.brotalityGrace = 1f;
					}
					SingletonNetObj<StatisticsController>.Instance.brotalityGrace = 1.5f;
				}
				SingletonNetObj<StatisticsController>.Instance.currentStats.totalBrotality += 1f;
				SingletonNetObj<StatisticsController>.Instance.musicIntensity += 1f;
			}
			SingletonNetObj<StatisticsController>.Instance.lastKillTime = Time.time;
			if (!mook.enemyAI.IsAlerted())
			{
				SingletonNetObj<StatisticsController>.Instance.currentStats.mooksKilledUnawares++;
			}
		}
	}

	public static void NotifyDeathType(MookType mookType, DeathType deathType, float xI, float yI)
	{
		if (deathType != DeathType.None && deathType != DeathType.Unassigned)
		{
			SingletonNetObj<StatisticsController>.Instance.currentStats.deathList.Add(new DeathObject(deathType, mookType, xI, yI));
		}
	}

	public static void NotifyMookDeathType(Mook mook, DeathType deathType)
	{
		if (mook != null && deathType != DeathType.None && deathType != DeathType.Unassigned)
		{
			SingletonNetObj<StatisticsController>.Instance.currentStats.deathList.Add(new DeathObject(deathType, mook.mookType, mook.xI, mook.yI));
		}
	}

	public static void NotifyMookDeathType(TestVanDammeAnim vanDamme, DeathType deathType)
	{
		if (vanDamme != null && deathType != DeathType.None)
		{
			SingletonNetObj<StatisticsController>.Instance.currentStats.deathList.Add(new DeathObject(deathType, vanDamme.heroType, vanDamme.xI, vanDamme.yI));
		}
	}

	public static void NotifyMookHeardSound(Mook mook)
	{
		if (mook.CanAddToStatistics() && mook.health > 0)
		{
			SingletonNetObj<StatisticsController>.Instance.currentStats.mooksHeardSound++;
		}
	}

	public static void NotifyMookSeenBro(Mook mook)
	{
		if (mook.CanAddToStatistics() && mook.health > 0)
		{
			SingletonNetObj<StatisticsController>.Instance.currentStats.mooksHalfAlerted++;
		}
	}

	public static void NotifyKnifedMook(Mook mook)
	{
		if (mook.CanAddToStatistics())
		{
			SingletonNetObj<StatisticsController>.Instance.currentStats.mooksKnifed++;
		}
	}

	public static void NotifyMookTryShootAtBBro(Mook mook)
	{
		if (mook.CanAddToStatistics() && mook.health > 0)
		{
			SingletonNetObj<StatisticsController>.Instance.currentStats.mooksFullyAlerted++;
		}
	}

	public static void NotifyBlockDestroyed(Block block)
	{
		if (!GameModeController.LevelFinished)
		{
			SingletonNetObj<StatisticsController>.Instance.currentStats.totalBrotality += 0.1f;
		}
		SingletonNetObj<StatisticsController>.Instance.currentStats.destruction++;
	}

	public static void NotifyLevelFinished(LevelResult result)
	{
		MonoBehaviour.print("NotifyLevelFinished, attempts " + StatisticsController.levelAttempts);
		if (result == LevelResult.Success)
		{
			if (StatisticsController.cachedStats != null)
			{
				SingletonNetObj<StatisticsController>.Instance.currentStats.Add(StatisticsController.cachedStats);
				StatisticsController.cachedStats = null;
			}
			SingletonNetObj<StatisticsController>.Instance.CalculateStealth();
			StatisticsController.LogLevelScore(LevelSelectionController.CurrentLevelNum, SingletonNetObj<StatisticsController>.Instance.currentStats.totalTime, 0L, 0L, SingletonNetObj<StatisticsController>.Instance.currentStats.mooksKilled, StatisticsController.levelAttempts);
			StatisticsController.levelAttempts = 0;
		}
		else
		{
			StatisticsController.levelAttempts++;
		}
	}

	private static void LogLevelScore(int levelNum, float time, long brotality, long stealth, int kills, int levelAttempts)
	{
		if (StatisticsController.campaignScore == null)
		{
			StatisticsController.campaignScore = new Dictionary<int, LevelStats>();
			if (levelNum != 0)
			{
				UnityEngine.Debug.LogError("Level num was not 0 when Campaign Score was created - something is weird with the campaign initialization");
			}
		}
		if (StatisticsController.campaignScore.ContainsKey(levelNum))
		{
			UnityEngine.Debug.LogError("Campaign Score already contains score for level " + levelNum + " - this means that either the level was completed twice somehow, or scores were not properly reset when campaign was switched/restarted.");
			StatisticsController.campaignScore.Remove(levelNum);
		}
		StatisticsController.CalculateTotalTime();
		StatisticsController.campaignScore.Add(levelNum, SingletonNetObj<StatisticsController>.Instance.currentStats);
	}

	public static void CalculateTotalTime()
	{
		float arg = Time.time - SingletonNetObj<StatisticsController>.Instance.levelStartTime;
		if (Connect.IsHost)
		{
			Networking.RPC<float>(PID.TargetAll, true, false, new RpcSignature<float>(StatisticsController.SetTotalTime), arg);
		}
	}

	private static void SetTotalTime(float totatTime)
	{
		SingletonNetObj<StatisticsController>.Instance.currentStats.totalTime = totatTime;
	}

	public static void AddBrotalityGrace(float grace)
	{
		if (SingletonNetObj<StatisticsController>.Instance.brotalityGrace < grace)
		{
			SingletonNetObj<StatisticsController>.Instance.brotalityGrace = grace;
		}
	}

	public static void AddBrotality(int extra)
	{
		if (!GameModeController.LevelFinished)
		{
			SingletonNetObj<StatisticsController>.Instance.currentStats.totalBrotality += (float)extra;
			SingletonNetObj<StatisticsController>.Instance.musicIntensity += (float)extra;
			if (extra < 5)
			{
				if (SingletonNetObj<StatisticsController>.Instance.brotalityGrace < 1f)
				{
					SingletonNetObj<StatisticsController>.Instance.brotalityGrace = 1f;
				}
			}
			else
			{
				SingletonNetObj<StatisticsController>.Instance.brotalityGrace = 1f;
			}
		}
	}

	private void Update()
	{
		if (this.currentStats != null)
		{
			float num = this.currentStats.totalBrotality + ((StatisticsController.cachedStats == null) ? 0f : StatisticsController.cachedStats.totalBrotality);
			float num2 = (StatisticsController.cachedStats == null) ? 0f : StatisticsController.cachedStats.totalBrotalityPenalty;
			if (!GameModeController.LevelFinished && !Map.isEditing)
			{
				if (Application.isEditor && Input.GetKeyDown(KeyCode.End))
				{
					UnityEngine.Debug.Log("Cheat Add Brotality");
					this.currentStats.totalBrotality += 10f;
					this.brotalityGrace = 3f;
					this.brotalityGrace = 1f;
				}
				if (CutsceneController.isInCutscene || LetterBoxController.IsShowingLetterBox())
				{
					if (this.brotalityGrace < 2f)
					{
						this.brotalityGrace = 2f;
					}
					this.brotalityGrace = 1f;
				}
				if (this.brotalityGrace > 0f)
				{
					this.brotalityGrace -= Time.deltaTime;
				}
				else
				{
					this.brotalityGrace -= Time.deltaTime;
					float num3;
					switch (StatisticsController.GetBrotalityLevel())
					{
					case 0:
						num3 = 3f;
						break;
					case 1:
						num3 = 6.25f;
						break;
					case 2:
						num3 = 12.5f;
						break;
					case 3:
						num3 = 18.75f;
						break;
					default:
						num3 = 50f;
						break;
					}
					if (this.brotalityGrace <= -6f)
					{
						num3 *= 3f;
					}
					else if (this.brotalityGrace <= -4f)
					{
						num3 *= 2f;
					}
					else if (this.brotalityGrace <= -2f)
					{
						num3 *= 1.5f;
					}
					if (LevelSelectionController.currentCampaign != null && LevelSelectionController.currentCampaign.header != null && LevelSelectionController.currentCampaign.header.hasTimeScoreBoard)
					{
						num3 *= 1.4f;
					}
					this.currentStats.totalBrotalityPenalty += num3 * Time.deltaTime;
					if (this.currentStats.totalBrotalityPenalty + num2 > num)
					{
						this.currentStats.totalBrotalityPenalty = num - num2;
					}
				}
			}
			this.brotalitometerValue = num - (this.currentStats.totalBrotalityPenalty + num2);
			if (this.brotalityGrace <= 0f)
			{
				this.musicIntensity -= 0.5f * Time.deltaTime;
				this.musicIntensity *= 1f - Time.deltaTime * 0.66f;
				if (this.musicIntensity < 0f)
				{
					this.musicIntensity = 0f;
				}
			}
			StatisticsController.CalculateBrotalityLevel(this.brotalitometerValue);
		}
	}

	public static int GetBrotalityLevel()
	{
		return StatisticsController.brotalityLevel;
	}

	public static long CalculateBrotality(LevelStats stats)
	{
		return (long)Mathf.Ceil((stats.totalBrotality - stats.totalBrotalityPenalty) * 10f) * 10L;
	}

	public static void CalculateBrotalityLevel(float brotalityValue)
	{
		if (brotalityValue <= 12f)
		{
			StatisticsController.brotalityLevel = 0;
		}
		else if (brotalityValue <= 37f)
		{
			StatisticsController.brotalityLevel = 1;
		}
		else if (brotalityValue <= 87f)
		{
			StatisticsController.brotalityLevel = 2;
		}
		else if (brotalityValue <= 162f)
		{
			StatisticsController.brotalityLevel = 3;
		}
		else if (brotalityValue <= 362f)
		{
			StatisticsController.brotalityLevel = 4;
		}
		else
		{
			StatisticsController.brotalityLevel = 5;
		}
	}

	private void CalculateStealth()
	{
	}

	public static void NotifyRescue()
	{
		if (!GameModeController.LevelFinished)
		{
			float a = 2f + (float)(4 - StatisticsController.GetBrotalityLevel()) * 0.33f;
			SingletonNetObj<StatisticsController>.Instance.brotalityGrace = Mathf.Max(a, SingletonNetObj<StatisticsController>.Instance.brotalityGrace);
			SingletonNetObj<StatisticsController>.Instance.brotalityGrace = 1f;
			SingletonNetObj<StatisticsController>.Instance.currentStats.totalBrotality += 3f;
		}
		SingletonNetObj<StatisticsController>.Instance.currentStats.rescues++;
	}

	private void NotOnGUI()
	{
		float num = Time.time - SingletonNetObj<StatisticsController>.Instance.levelStartTime;
		float num2 = (float)((int)(num / 60f));
		float num3 = (float)((int)(num - num2 * 60f));
		float num4 = (float)((int)((num - num2 * 60f - num3) * 100f));
		GUILayout.BeginArea(new Rect(0f, 0f, 200f, 200f));
		GUILayout.Label("Destruction: " + this.currentStats.destruction, new GUILayoutOption[0]);
		GUILayout.Label(string.Concat(new object[]
		{
			"Kills: ",
			this.currentStats.mooksKilled,
			"//",
			this.currentStats.mooksAtStart
		}), new GUILayoutOption[0]);
		GUILayout.Label(string.Concat(new string[]
		{
			"Time: ",
			num2.ToString("00"),
			":",
			num3.ToString("00"),
			".",
			num4.ToString("00")
		}), new GUILayoutOption[0]);
		GUILayout.Label(string.Concat(new object[]
		{
			"Rescues: ",
			this.currentStats.rescues,
			"//",
			this.currentStats.totalCages
		}), new GUILayoutOption[0]);
		GUILayout.Label("Brotality: " + StatisticsController.CalculateBrotality(this.currentStats), new GUILayoutOption[0]);
		GUILayout.Label("Brotalitometer: " + this.brotalitometerValue, new GUILayoutOption[0]);
		GUILayout.HorizontalScrollbar(this.brotalitometerValue, 1f, 0f, 50f, new GUILayoutOption[0]);
		GUILayout.EndArea();
	}

	public virtual byte[] SerializeStats()
	{
		UnityStream unityStream = new UnityStream();
		unityStream.Serialize<int>(this.currentStats.destruction);
		unityStream.Serialize<int>(this.currentStats.mooksKilled);
		unityStream.Serialize<int>(this.currentStats.rescues);
		unityStream.Serialize<int>(this.currentStats.mooksHeardSound);
		unityStream.Serialize<int>(this.currentStats.mooksHalfAlerted);
		unityStream.Serialize<int>(this.currentStats.mooksFullyAlerted);
		unityStream.Serialize<int>(this.currentStats.mooksKnifed);
		unityStream.Serialize<int>(this.currentStats.mooksKilledUnawares);
		unityStream.Serialize<float>(this.levelStartTime);
		unityStream.Serialize<float>(this.currentStats.totalTime);
		unityStream.Serialize<int>(StatisticsController.levelAttempts);
		return unityStream.ByteArray;
	}

	public virtual void DeserializeStats(byte[] byteStream)
	{
		UnityStream unityStream = new UnityStream(byteStream);
		this.currentStats.destruction = (int)unityStream.DeserializeNext();
		this.currentStats.mooksKilled = (int)unityStream.DeserializeNext();
		this.currentStats.rescues = (int)unityStream.DeserializeNext();
		this.currentStats.mooksHeardSound = (int)unityStream.DeserializeNext();
		this.currentStats.mooksHalfAlerted = (int)unityStream.DeserializeNext();
		this.currentStats.mooksFullyAlerted = (int)unityStream.DeserializeNext();
		this.currentStats.mooksKnifed = (int)unityStream.DeserializeNext();
		this.currentStats.mooksKilledUnawares = (int)unityStream.DeserializeNext();
		this.levelStartTime = (float)unityStream.DeserializeNext();
		this.currentStats.totalTime = (float)unityStream.DeserializeNext();
		StatisticsController.levelAttempts = (int)unityStream.DeserializeNext();
	}

	internal static void NotifyPause(bool paused)
	{
		if (SingletonNetObj<StatisticsController>.Instance != null)
		{
			if (paused)
			{
				SingletonNetObj<StatisticsController>.Instance.pauseTime = Time.time;
			}
			else if (SingletonNetObj<StatisticsController>.Instance.pauseTime > 0f)
			{
				float num = Time.time - SingletonNetObj<StatisticsController>.Instance.pauseTime;
				SingletonNetObj<StatisticsController>.Instance.levelStartTime += num;
				SingletonNetObj<StatisticsController>.Instance.pauseTime = -1f;
			}
		}
	}

	internal static Dictionary<int, LevelStats> GetCampaignStats()
	{
		return StatisticsController.campaignScore;
	}

	internal static void CalcAndSubmitCampaignScore()
	{
		long num = 0L;
		float num2 = 0f;
		Dictionary<int, LevelStats> campaignStats = StatisticsController.GetCampaignStats();
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<int, LevelStats> keyValuePair in campaignStats)
		{
			stringBuilder.Append("\n\nLevel : " + keyValuePair.Key);
			stringBuilder.Append("\nKills: " + keyValuePair.Value.mooksKilled);
			stringBuilder.Append("\nStealth Kills: " + keyValuePair.Value.mooksKilledUnawares);
			stringBuilder.Append("\nTime: " + StatisticsController.GetTimeString(keyValuePair.Value.totalTime));
			stringBuilder.Append("\nBrotality: " + StatisticsController.CalculateBrotality(keyValuePair.Value));
			num2 += keyValuePair.Value.totalTime;
			num = StatisticsController.CalculateBrotality(keyValuePair.Value);
			stringBuilder.Append("\n\nTotal Brotality: " + num);
			stringBuilder.Append("\nTotal Time: " + StatisticsController.GetTimeString(num2));
			stringBuilder.Append("\n");
		}
		PlaytomicController.SubmitCampaignScores(LevelSelectionController.currentCampaign.header.hasTimeScoreBoard, num2, LevelSelectionController.currentCampaign.header.hasBrotalityScoreboard, num);
		UnityEngine.Debug.Log(stringBuilder);
	}

	internal static void ResetScore()
	{
		StatisticsController.campaignScore = null;
	}

	public override UnityStream PackState(UnityStream stream)
	{
		stream.Serialize<float>(this.levelStartTime);
		return base.PackState(stream);
	}

	public override UnityStream UnpackState(UnityStream stream)
	{
		this.levelStartTime = (float)stream.DeserializeNext();
		return base.UnpackState(stream);
	}

	public const float brotalityFirstBarValue = 12f;

	public const float brotalitySecondBarValue = 25f;

	public const float brotalityThirdBarValue = 50f;

	public const float brotalityFourthBarValue = 75f;

	public const float brotalityFifthBarValue = 200f;

	public static int brotalityRank;

	public static int timeRank;

	public static int stealthRank;

	public static float brotalityPercentile;

	public static float timePercentile;

	public static float stealthPercentile;

	private static int totalMooks;

	private static int levelAttempts;

	private static Dictionary<int, LevelStats> campaignScore;

	private float levelStartTime;

	public LevelStats currentStats;

	public static LevelStats cachedStats;

	private float brotalitometerValue;

	private float musicIntensity;

	public static int killScore;

	public static int rescueScore;

	public static int efficiencyScore;

	protected float brotalityGrace;

	protected int lastBrotality;

	protected float lastKillTime;

	public static int brotalityLevel;

	private float pauseTime = -1f;
}
