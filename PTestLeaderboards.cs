// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

internal class PTestLeaderboards : PTest
{
	public static void FirstScore(Action done)
	{
		UnityEngine.Debug.Log("TestLeaderboards.FirstScore");
		PlayerScore score = new PlayerScore
		{
			table = "scores" + PTestLeaderboards.rnd,
			playername = "person1",
			points = 10000L,
			highest = true,
			fields = new PDictionary
			{
				{
					"rnd",
					PTestLeaderboards.rnd
				}
			}
		};
		Playtomic.Leaderboards.Save(score, delegate(PResponse r)
		{
			PTest.AssertTrue("TestLeaderboards.FirstScore#1", "Request succeeded", r.success);
			PTest.AssertEquals("TestLeaderboards.FirstScore#1", "No errorcode", r.errorcode, 0);
			score.points = 9000L;
			Thread.Sleep(1000);
			Playtomic.Leaderboards.Save(score, delegate(PResponse r2)
			{
				PTest.AssertTrue("TestLeaderboards.FirstScore#2", "Request succeeded", r2.success);
				PTest.AssertEquals("TestLeaderboards.FirstScore#2", "Rejected duplicate score", r2.errorcode, 209);
				score.points = 11000L;
				Playtomic.Leaderboards.Save(score, delegate(PResponse r3)
				{
					PTest.AssertTrue("TestLeaderboards.FirstScore#3", "Request succeeded", r3.success);
					PTest.AssertEquals("TestLeaderboards.FirstScore#3", "No errorcode", r3.errorcode, 0);
					score.points = 9000L;
					score.allowduplicates = true;
					Playtomic.Leaderboards.Save(score, delegate(PResponse r4)
					{
						PTest.AssertTrue("TestLeaderboards.FirstScore#4", "Request succeeded", r4.success);
						PTest.AssertEquals("TestLeaderboards.FirstScore#4", "No errorcode", r4.errorcode, 0);
						done();
					});
				});
			});
		});
	}

	public static void SecondScore(Action done)
	{
		UnityEngine.Debug.Log("TestLeaderboards.SecondScore");
		PlayerScore score = new PlayerScore
		{
			table = "scores" + PTestLeaderboards.rnd,
			playername = "person2",
			points = 20000L,
			allowduplicates = true,
			highest = true,
			fields = new PDictionary
			{
				{
					"rnd",
					PTestLeaderboards.rnd
				}
			}
		};
		Thread.Sleep(1000);
		Playtomic.Leaderboards.Save(score, delegate(PResponse r)
		{
			PTest.AssertTrue("TestLeaderboards.SecondScore", "Request succeeded", r.success);
			PTest.AssertEquals("TestLeaderboards.SecondScore", "No errorcode", r.errorcode, 0);
			done();
		});
	}

	public static void HighScores(Action done)
	{
		UnityEngine.Debug.Log("TestLeaderboards.Highscores");
		PLeaderboardOptions options = new PLeaderboardOptions
		{
			table = "scores" + PTestLeaderboards.rnd,
			highest = true,
			filters = new PDictionary
			{
				{
					"rnd",
					PTestLeaderboards.rnd
				}
			}
		};
		Playtomic.Leaderboards.List(options, delegate(List<PlayerScore> scores, int numscores, PResponse r)
		{
			scores = (scores ?? new List<PlayerScore>());
			PTest.AssertTrue("TestLeaderboards.Highscores", "Request succeeded", r.success);
			PTest.AssertEquals("TestLeaderboards.Highscores", "No errorcode", r.errorcode, 0);
			PTest.AssertTrue("TestLeaderboards.Highscores", "Received scores", scores.Count > 0);
			PTest.AssertTrue("TestLeaderboards.Highscores", "Received numscores", numscores > 0);
			if (scores.Count > 1)
			{
				PTest.AssertTrue("TestLeaderboards.Highscores", "First score is greater than second", scores[0].points > scores[1].points);
			}
			else
			{
				PTest.AssertTrue("TestLeaderboards.Highscores", "First score is greater than second forced failure", false);
			}
			done();
		});
	}

	public static void LowScores(Action done)
	{
		UnityEngine.Debug.Log("TestLeaderboards.LowScores");
		PLeaderboardOptions options = new PLeaderboardOptions
		{
			table = "scores" + PTestLeaderboards.rnd,
			lowest = true,
			perpage = 2,
			filters = new PDictionary
			{
				{
					"rnd",
					PTestLeaderboards.rnd
				}
			}
		};
		Playtomic.Leaderboards.List(options, delegate(List<PlayerScore> scores, int numscores, PResponse r)
		{
			scores = (scores ?? new List<PlayerScore>());
			PTest.AssertTrue("TestLeaderboards.LowScores", "Request succeeded", r.success);
			PTest.AssertEquals("TestLeaderboards.LowScores", "No errorcode", r.errorcode, 0);
			PTest.AssertTrue("TestLeaderboards.LowScores", "Received scores", scores.Count == 2);
			PTest.AssertTrue("TestLeaderboards.LowScores", "Received numscores", numscores > 0);
			if (scores.Count > 1)
			{
				PTest.AssertTrue("TestLeaderboards.LowScores", "First score is less than second", scores[0].points < scores[1].points);
			}
			else
			{
				PTest.AssertTrue("TestLeaderboards.LowScores", "First score is less than second forced failure", false);
			}
			done();
		});
	}

	public static void AllScores(Action done)
	{
		UnityEngine.Debug.Log("TestLeaderboards.AllScores");
		PLeaderboardOptions options = new PLeaderboardOptions
		{
			table = "scores" + PTestLeaderboards.rnd,
			mode = "newest",
			perpage = 2
		};
		Playtomic.Leaderboards.List(options, delegate(List<PlayerScore> scores, int numscores, PResponse r)
		{
			scores = (scores ?? new List<PlayerScore>());
			PTest.AssertTrue("TestLeaderboards.AllScores", "Request succeeded", r.success);
			PTest.AssertEquals("TestLeaderboards.AllScores", "No errorcode", r.errorcode, 0);
			PTest.AssertTrue("TestLeaderboards.AllScores", "Received scores", scores.Count > 0);
			PTest.AssertTrue("TestLeaderboards.AllScores", "Received numscores", numscores > 0);
			if (scores.Count > 1)
			{
				PTest.AssertTrue("TestLeaderboards.AllScores", "First score is newer or equal to second", scores[0].date >= scores[1].date);
			}
			else
			{
				PTest.AssertTrue("TestLeaderboards.AllScores", "First score is newer or equal to second forced failure", false);
			}
			done();
		});
	}

	public static void FriendsScores(Action done)
	{
		UnityEngine.Debug.Log("TestLeaderboards.FriendsScores");
		List<object> playerids = new List<object>
		{
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9",
			"10"
		};
		PTestLeaderboards.FriendsScoresLoop(playerids, 0, delegate
		{
			PLeaderboardOptions options = new PLeaderboardOptions
			{
				table = "friends" + PTestLeaderboards.rnd,
				perpage = 3,
				friendslist = new List<string>
				{
					"1",
					"2",
					"3"
				}
			};
			Playtomic.Leaderboards.List(options, delegate(List<PlayerScore> scores, int numscores, PResponse r2)
			{
				scores = (scores ?? new List<PlayerScore>());
				PTest.AssertTrue("TestLeaderboards.FriendsScores", "Request succeeded", r2.success);
				PTest.AssertEquals("TestLeaderboards.FriendsScores", "No errorcode", r2.errorcode, 0);
				PTest.AssertTrue("TestLeaderboards.FriendsScores", "Received 3 scores", scores.Count == 3);
				PTest.AssertTrue("TestLeaderboards.FriendsScores", "Received numscores 3", numscores == 3);
				PTest.AssertTrue("TestLeaderboards.FriendsScores", "Player id #1", scores[0].playerid == "3");
				PTest.AssertTrue("TestLeaderboards.FriendsScores", "Player id #2", scores[1].playerid == "2");
				PTest.AssertTrue("TestLeaderboards.FriendsScores", "Player id #3", scores[2].playerid == "1");
				done();
			});
		});
	}

	private static void FriendsScoresLoop(List<object> playerids, int points, Action finished)
	{
		Thread.Sleep(500);
		points += 1000;
		string text = playerids[0].ToString();
		playerids.RemoveAt(0);
		PlayerScore score = new PlayerScore
		{
			playername = "playerid" + text,
			playerid = text,
			table = "friends" + PTestLeaderboards.rnd,
			points = (long)points,
			highest = true,
			fields = 
			{
				{
					"rnd",
					PTestLeaderboards.rnd
				}
			}
		};
		Playtomic.Leaderboards.Save(score, delegate(PResponse r)
		{
			if (playerids.Count > 0)
			{
				PTestLeaderboards.FriendsScoresLoop(playerids, points, finished);
				return;
			}
			finished();
		});
	}

	public static void OwnScores(Action done)
	{
		UnityEngine.Debug.Log("TestLeaderboards.OwnScores");
		PTestLeaderboards.OwnScoresLoop(0, 0, delegate
		{
			PlayerScore score = new PlayerScore
			{
				playername = "test account",
				playerid = "test@testuri.com",
				table = "personal" + PTestLeaderboards.rnd,
				points = 2500L,
				highest = true,
				allowduplicates = true,
				fields = 
				{
					{
						"rnd",
						PTestLeaderboards.rnd
					}
				},
				perpage = 5L
			};
			Playtomic.Leaderboards.SaveAndList(score, delegate(List<PlayerScore> scores, int numscores, PResponse r2)
			{
				scores = (scores ?? new List<PlayerScore>());
				PTest.AssertTrue("TestLeaderboards.OwnScores", "Request succeeded", r2.success);
				PTest.AssertEquals("TestLeaderboards.OwnScores", "No errorcode", r2.errorcode, 0);
				PTest.AssertTrue("TestLeaderboards.OwnScores", "Received 5 scores", scores.Count == 5);
				PTest.AssertTrue("TestLeaderboards.OwnScores", "Received numscores 10", numscores == 10);
				PTest.AssertTrue("TestLeaderboards.OwnScores", "Score 1 ranked 6", scores[0].rank == 6L);
				PTest.AssertTrue("TestLeaderboards.OwnScores", "Score 2 ranked 7", scores[1].rank == 7L);
				PTest.AssertTrue("TestLeaderboards.OwnScores", "Score 3 ranked 8", scores[2].rank == 8L);
				PTest.AssertTrue("TestLeaderboards.OwnScores", "Score 4 ranked 9", scores[3].rank == 9L);
				PTest.AssertTrue("TestLeaderboards.OwnScores", "Score 5 ranked 10", scores[4].rank == 10L);
				PTest.AssertTrue("TestLeaderboards.OwnScores", "Score 1 points", scores[0].points == 4000L);
				PTest.AssertTrue("TestLeaderboards.OwnScores", "Score 2 points", scores[1].points == 3000L);
				PTest.AssertTrue("TestLeaderboards.OwnScores", "Score 3 points", scores[2].points == 2500L);
				PTest.AssertTrue("TestLeaderboards.OwnScores", "Score 3 submitted", scores[2].submitted);
				PTest.AssertTrue("TestLeaderboards.OwnScores", "Score 4 points", scores[3].points == 2000L);
				PTest.AssertTrue("TestLeaderboards.OwnScores", "Score 5 points", scores[4].points == 1000L);
				done();
			});
		});
	}

	private static void OwnScoresLoop(int count, int points, Action finished)
	{
		Thread.Sleep(500);
		points += 1000;
		count++;
		PlayerScore score = new PlayerScore
		{
			playername = "test account",
			playerid = "test@testuri.com",
			table = "personal" + PTestLeaderboards.rnd,
			points = (long)points,
			highest = true,
			allowduplicates = true,
			fields = 
			{
				{
					"rnd",
					PTestLeaderboards.rnd
				}
			}
		};
		Playtomic.Leaderboards.Save(score, delegate(PResponse r)
		{
			if (count < 9)
			{
				PTestLeaderboards.OwnScoresLoop(count, points, finished);
				return;
			}
			finished();
		});
	}

	public static void SaveAndListNoDuplicates(Action done)
	{
		UnityEngine.Debug.Log("TestLeaderboards.SaveAndListNoDuplicates");
		PlayerScore score = new PlayerScore
		{
			table = "scoresnodup" + PTestLeaderboards.rnd,
			playername = "person1",
			points = 10000L,
			highest = true,
			allowduplicates = false,
			fields = new PDictionary
			{
				{
					"rnd",
					PTestLeaderboards.rnd
				}
			}
		};
		Playtomic.Leaderboards.SaveAndList(score, delegate(List<PlayerScore> retScores, int no, PResponse r)
		{
			PTest.AssertTrue("TestLeaderboards.SaveAndListNoDuplicates#1", "Request succeeded", r.success);
			PTest.AssertEquals("TestLeaderboards.SaveAndListNoDuplicates#1", "No errorcode", r.errorcode, 0);
			score.points = 9000L;
			Thread.Sleep(1000);
			Playtomic.Leaderboards.SaveAndList(score, delegate(List<PlayerScore> retScores2, int no2, PResponse r2)
			{
				PTest.AssertTrue("TestLeaderboards.SaveAndListNoDuplicates#2", "Request succeeded", r2.success);
				PTest.AssertEquals("TestLeaderboards.SaveAndListNoDuplicates#2", "Rejected duplicate score", r2.errorcode, 209);
				score.points = 11000L;
				Playtomic.Leaderboards.SaveAndList(score, delegate(List<PlayerScore> retScores3, int no3, PResponse r3)
				{
					PTest.AssertTrue("TestLeaderboards.SaveAndListNoDuplicates#3", "Request succeeded", r3.success);
					PTest.AssertEquals("TestLeaderboards.SaveAndListNoDuplicates#3", "No errorcode", r3.errorcode, 0);
					score.points = 9000L;
					score.allowduplicates = false;
					Playtomic.Leaderboards.SaveAndList(score, delegate(List<PlayerScore> retScores4, int no4, PResponse r4)
					{
						PTest.AssertTrue("TestLeaderboards.SaveAndListNoDuplicates#4", "Request succeeded", r4.success);
						PTest.AssertEquals("TestLeaderboards.SaveAndListNoDuplicates#4", "No errorcode", r4.errorcode, 0);
						done();
					});
				});
			});
		});
	}

	public static int rnd;
}
