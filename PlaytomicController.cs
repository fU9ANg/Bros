// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlaytomicController
{
	public static bool isExhibitionBuild
	{
		get
		{
			return false;
		}
	}

	public static bool isOnlineBuild
	{
		get
		{
			return true;
		}
	}

	public static bool isExpendabrosBuild
	{
		get
		{
			return true;
		}
	}

	public static void Init()
	{
		if (PlaytomicController.hasInitialised)
		{
			return;
		}
		PlaytomicController.hasInitialised = true;
		Playtomic.Initialize("broforce", "IndieDarlings5001", "http://broforcebeta.herokuapp.com/");
		Playtomic.GameVars.Load(new Action<Dictionary<string, GameVar>, PResponse>(PlaytomicController.RevisionCallback));
	}

	private static void RevisionCallback(Dictionary<string, GameVar> vars, PResponse response)
	{
		if (response.success)
		{
			foreach (KeyValuePair<string, GameVar> keyValuePair in vars)
			{
				UnityEngine.Debug.LogWarning("Key: " + keyValuePair.Key + " Value: " + keyValuePair.Value.value);
			}
		}
		else
		{
			UnityEngine.Debug.Log("REMOTE REVISION RETRIEVAL FAILED");
		}
	}

	public static bool IsThisBuildOutOfDate()
	{
		return PlaytomicController.remoteBetaRevision > 1280L;
	}

	public static void LogPreorderClick()
	{
		PlaytomicController.Init();
		if (!PlaytomicController.hasLoggedPreorderClick)
		{
			PlaytomicController.hasLoggedPreorderClick = true;
			PlayerScore score = new PlayerScore
			{
				playername = "Test",
				points = 1L,
				table = "preorder",
				allowduplicates = true,
				source = "Test",
				fields = new PDictionary
				{
					{
						"excludeplayerid",
						"false"
					}
				}
			};
			Playtomic.Leaderboards.Save(score, new Action<PResponse>(PlaytomicController.SaveStatsCallback));
		}
	}

	public static void AchievementCallback(PResponse response)
	{
		if (response.success)
		{
			UnityEngine.Debug.Log("PLAYTOMIC SUCCESS");
		}
		else
		{
			UnityEngine.Debug.Log("PLAYTOMIC FAIL");
			UnityEngine.Debug.LogError(response.errormessage);
		}
	}

	public static void SubmitCampaignScores(bool submitTime, float time, bool submitBrotality, long brotality)
	{
		if (!Connect.IsOffline)
		{
			UnityEngine.Debug.Log("Do not submit score in online mode");
			return;
		}
		PlaytomicController.brotalityList = null;
		PlaytomicController.speedList = null;
		PlaytomicController.Init();
		if (submitBrotality)
		{
			PlayerScore score = new PlayerScore
			{
				playername = PlayerOptions.Instance.PlayerName,
				points = brotality,
				table = "brotality",
				allowduplicates = true,
				source = "Test",
				perpage = 10L,
				filters = new PDictionary
				{
					{
						"level",
						LevelSelectionController.currentCampaign.header.md5
					}
				},
				fields = new PDictionary
				{
					{
						"level",
						LevelSelectionController.currentCampaign.header.md5
					},
					{
						"revision",
						861L
					}
				}
			};
			Playtomic.Leaderboards.SaveAndList(score, new Action<List<PlayerScore>, int, PResponse>(PlaytomicController.SaveBrotalityCallback));
		}
		if (submitTime)
		{
			PlaytomicController.speedScore = new PlayerScore
			{
				playername = PlayerOptions.Instance.PlayerName,
				points = (long)(time * 1000f),
				table = "time",
				perpage = 10L,
				allowduplicates = true,
				highest = false,
				lowest = true,
				source = "Test",
				filters = new PDictionary
				{
					{
						"level",
						LevelSelectionController.currentCampaign.header.md5
					}
				},
				fields = new PDictionary
				{
					{
						"level",
						LevelSelectionController.currentCampaign.header.md5
					},
					{
						"revision",
						861L
					}
				}
			};
			Playtomic.Leaderboards.SaveAndList(PlaytomicController.speedScore, new Action<List<PlayerScore>, int, PResponse>(PlaytomicController.SaveSpeedCallback));
		}
		StatisticsController.brotalityPercentile = -1f;
		StatisticsController.timePercentile = -1f;
		StatisticsController.stealthPercentile = -1f;
		StatisticsController.timeRank = 9999; StatisticsController.brotalityRank = (StatisticsController.stealthRank = (StatisticsController.timeRank ));
	}

	public static void SaveSpeedCallback(List<PlayerScore> scores, int no, PResponse response)
	{
		if (response.success)
		{
			PlaytomicController.speedList = scores;
			PlaytomicController.DoZeroRankFix(scores);
		}
		else
		{
			UnityEngine.Debug.LogError("Could not submit score to Playtomic: " + response.errormessage);
			Playtomic.Leaderboards.SaveAndList(PlaytomicController.speedScore, new Action<List<PlayerScore>, int, PResponse>(PlaytomicController.SaveSpeedCallback));
		}
	}

	public static void SaveBrotalityCallback(List<PlayerScore> scores, int no, PResponse response)
	{
		if (response.success)
		{
			PlaytomicController.brotalityList = scores;
			PlaytomicController.DoZeroRankFix(scores);
		}
		else
		{
			UnityEngine.Debug.LogError("Could not submit score to Playtomic: " + response.errormessage);
		}
	}

	private static void DoZeroRankFix(List<PlayerScore> scores)
	{
		int num = 1;
		for (int i = 0; i < scores.Count; i++)
		{
			if (scores[i].rank != 0L)
			{
				num = (int)scores[i].rank - i;
			}
		}
		for (int j = 0; j < scores.Count; j++)
		{
			if (scores[j].rank == 0L)
			{
				scores[j].rank = (long)(num + j);
				scores[j].submitted = true;
			}
		}
	}

	public static void SaveStatsCallback(PResponse response)
	{
	}

	public static void SaveStealthCallback(List<PlayerScore> scores, int no, PResponse response)
	{
		if (response.success)
		{
			PlayerScore playerScore = (scores.Count<PlayerScore>() <= 0) ? null : scores[0];
			if (playerScore != null)
			{
				StatisticsController.stealthPercentile = (float)((long)no - playerScore.rank + 1L) / (float)no * 100f;
				StatisticsController.stealthRank = (int)playerScore.rank;
			}
			else
			{
				UnityEngine.Debug.Log("could not find submitted Stealth in return list");
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Could not submit score to Playtomic: " + response.errormessage);
		}
	}

	internal static void UploadLevel(string fileName, CampaignHeader header)
	{
		PlaytomicController.Init();
		PlayerLevel level = new PlayerLevel
		{
			name = header.name,
			data = FileIO.GetPublishedLevelAsString(fileName),
			playername = PlayerOptions.Instance.PlayerName,
			fields = new Dictionary<string, object>
			{
				{
					"author",
					header.author
				},
				{
					"description",
					header.description
				},
				{
					"length",
					header.length
				},
				{
					"gamemode",
					header.gameMode.ToString()
				},
				{
					"md5",
					header.md5
				}
			}
		};
		Playtomic.PlayerLevels.Save(level, new Action<PlayerLevel, PResponse>(PlaytomicController.LevelSaveComplete));
		PlaytomicController.levelUploadStatus = RemoteOperationStatus.Busy;
	}

	private static void LevelSaveComplete(PlayerLevel level, PResponse r)
	{
		if (r.success)
		{
			PlaytomicController.levelUploadStatus = RemoteOperationStatus.Success;
		}
		else
		{
			PlaytomicController.levelUploadStatus = RemoteOperationStatus.Fail;
			UnityEngine.Debug.LogError(r.errormessage);
		}
	}

	public static void ListLevels(int pageNo)
	{
		PlaytomicController.Init();
		PPlayerLevelOptions options = new PPlayerLevelOptions
		{
			mode = ((!PlaytomicController.listNewLevels) ? "last90daysaverage" : "newest"),
			data = false,
			perpage = 20,
			page = pageNo
		};
		PlaytomicController.listingLevels = RemoteOperationStatus.Busy;
		Playtomic.PlayerLevels.List(options, new Action<List<PlayerLevel>, int, PResponse>(PlaytomicController.ListLoaded));
	}

	private static void ListLoaded(List<PlayerLevel> levels, int numLevels, PResponse respons)
	{
		if (respons.success)
		{
			PlaytomicController.listingLevels = RemoteOperationStatus.Success;
			PlaytomicController.remoteLevels = levels;
		}
		else
		{
			PlaytomicController.listingLevels = RemoteOperationStatus.Fail;
		}
	}

	public static void LoadLevel(string levelId)
	{
		PlaytomicController.Init();
		PlaytomicController.loadedLevel = null;
		Playtomic.PlayerLevels.Load(levelId, new Action<PlayerLevel, PResponse>(PlaytomicController.LevelLoadComplete));
		PlaytomicController.loadingLevel = RemoteOperationStatus.Busy;
	}

	private static void LevelLoadComplete(PlayerLevel level, PResponse response)
	{
		if (response.success)
		{
			PlaytomicController.loadingLevel = RemoteOperationStatus.Success;
			PlayerProgress.Instance.lastOnlineLevelId = level.levelid;
			PlaytomicController.loadedLevel = level;
		}
	}

	internal static void RateLevel(int rating)
	{
		if (PlaytomicController.loadedLevel != null)
		{
			Playtomic.PlayerLevels.Rate(PlaytomicController.loadedLevel.levelid, rating * 2, new Action<PResponse>(PlaytomicController.RateCallback));
		}
		else
		{
			UnityEngine.Debug.LogError("Tried to submit rating but loaded online level is null");
		}
	}

	private static void RateCallback(PResponse r)
	{
	}

	internal static void GetCampaignScores()
	{
		if (!Connect.IsOffline)
		{
			UnityEngine.Debug.Log("Do not submit score in online mode");
			return;
		}
		bool hasBrotalityScoreboard = LevelSelectionController.currentCampaign.header.hasBrotalityScoreboard;
		bool hasTimeScoreBoard = LevelSelectionController.currentCampaign.header.hasTimeScoreBoard;
		PlaytomicController.brotalityList = null;
		PlaytomicController.speedList = null;
		PlaytomicController.Init();
		if (hasBrotalityScoreboard)
		{
			PLeaderboardOptions options = new PLeaderboardOptions
			{
				table = "brotality",
				perpage = 10,
				page = 1,
				filters = new PDictionary
				{
					{
						"level",
						LevelSelectionController.currentCampaign.header.md5
					}
				}
			};
			Playtomic.Leaderboards.List(options, new Action<List<PlayerScore>, int, PResponse>(PlaytomicController.SaveBrotalityCallback));
		}
		if (hasTimeScoreBoard)
		{
			PLeaderboardOptions options2 = new PLeaderboardOptions
			{
				table = "time",
				perpage = 10,
				highest = false,
				lowest = true,
				filters = new PDictionary
				{
					{
						"level",
						LevelSelectionController.currentCampaign.header.md5
					}
				}
			};
			Playtomic.Leaderboards.List(options2, new Action<List<PlayerScore>, int, PResponse>(PlaytomicController.SaveSpeedCallback));
		}
		StatisticsController.brotalityPercentile = -1f;
		StatisticsController.timePercentile = -1f;
		StatisticsController.stealthPercentile = -1f;
		StatisticsController.timeRank = 9999; StatisticsController.brotalityRank = (StatisticsController.stealthRank = (StatisticsController.timeRank ));
	}

	public const long mercurialRevisionNo = 861L;

	public const long betaRevisionNo = 1280L;

	private static long remoteBetaRevision = -1L;

	private static bool hasInitialised;

	public static RemoteOperationStatus listingLevels;

	public static List<PlayerLevel> remoteLevels;

	public static RemoteOperationStatus loadingLevel;

	public static PlayerLevel loadedLevel;

	public static RemoteOperationStatus levelUploadStatus;

	private static PlayerScore speedScore;

	private static bool hasLoggedPreorderClick;

	public static List<PlayerScore> speedList;

	public static List<PlayerScore> brotalityList;

	public static bool listNewLevels;
}
