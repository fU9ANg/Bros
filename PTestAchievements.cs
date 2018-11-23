// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

internal class PTestAchievements : PTest
{
	public static void List(Action done)
	{
		string section = "PTestAchievements.List";
		UnityEngine.Debug.Log(section);
		List<PlayerAchievement> list = new List<PlayerAchievement>();
		list.Add(new PlayerAchievement(new Dictionary<string, object>
		{
			{
				"achievement",
				"Super Mega Achievement #1"
			},
			{
				"achievementkey",
				"secretkey"
			},
			{
				"playerid",
				"1"
			},
			{
				"playername",
				"ben"
			},
			{
				"fields",
				new Dictionary<string, object>
				{
					{
						"rnd",
						PTestAchievements.rnd
					}
				}
			}
		}));
		list.Add(new PlayerAchievement(new Dictionary<string, object>
		{
			{
				"achievement",
				"Super Mega Achievement #1"
			},
			{
				"achievementkey",
				"secretkey"
			},
			{
				"playerid",
				"2"
			},
			{
				"playername",
				"michelle"
			},
			{
				"fields",
				new Dictionary<string, object>
				{
					{
						"rnd",
						PTestAchievements.rnd
					}
				}
			}
		}));
		list.Add(new PlayerAchievement(new Dictionary<string, object>
		{
			{
				"achievement",
				"Super Mega Achievement #1"
			},
			{
				"achievementkey",
				"secretkey"
			},
			{
				"playerid",
				"3"
			},
			{
				"playername",
				"peter"
			},
			{
				"fields",
				new Dictionary<string, object>
				{
					{
						"rnd",
						PTestAchievements.rnd
					}
				}
			}
		}));
		list.Add(new PlayerAchievement(new Dictionary<string, object>
		{
			{
				"achievement",
				"Super Mega Achievement #2"
			},
			{
				"achievementkey",
				"secretkey2"
			},
			{
				"playerid",
				"3"
			},
			{
				"playername",
				"peter"
			},
			{
				"fields",
				new Dictionary<string, object>
				{
					{
						"rnd",
						PTestAchievements.rnd
					}
				}
			}
		}));
		list.Add(new PlayerAchievement(new Dictionary<string, object>
		{
			{
				"achievement",
				"Super Mega Achievement #2"
			},
			{
				"achievementkey",
				"secretkey2"
			},
			{
				"playerid",
				"2"
			},
			{
				"playername",
				"michelle"
			},
			{
				"fields",
				new Dictionary<string, object>
				{
					{
						"rnd",
						PTestAchievements.rnd
					}
				}
			}
		}));
		PTestAchievements.ListLoop(section, list, delegate
		{
			PAchievementOptions options = new PAchievementOptions
			{
				filters = new PDictionary
				{
					{
						"rnd",
						PTestAchievements.rnd
					}
				}
			};
			Playtomic.Achievements.List(options, delegate(List<PlayerAchievement> ach, PResponse r2)
			{
				PTest.AssertTrue(section, "Request succeeded", r2.success);
				PTest.AssertEquals(section, "No errorcode", r2.errorcode, 0);
				PTest.AssertEquals(section, "Achievement 1 is correct", ach[0].achievement, "Super Mega Achievement #1");
				PTest.AssertEquals(section, "Achievement 2 is correct", ach[1].achievement, "Super Mega Achievement #2");
				PTest.AssertEquals(section, "Achievement 3 is correct", ach[2].achievement, "Super Mega Achievement #3");
				done();
			});
		});
	}

	private static void ListLoop(string section, List<PlayerAchievement> achievements, Action finished)
	{
		PlayerAchievement achievement = achievements[0];
		achievements.RemoveAt(0);
		Playtomic.Achievements.Save(achievement, delegate(PResponse r)
		{
			PTest.AssertTrue(section, "Request succeeded (" + (5 - achievements.Count) + ")", r.success);
			PTest.AssertEquals(section, "No errorcode (" + (5 - achievements.Count) + ")", r.errorcode, 0);
			Thread.Sleep(2000);
			if (achievements.Count > 0)
			{
				PTestAchievements.ListLoop(section, achievements, finished);
				return;
			}
			finished();
		});
	}

	public static void ListWithFriends(Action done)
	{
		UnityEngine.Debug.Log("PTestAchievements.ListWithFriends");
		PAchievementOptions options = new PAchievementOptions
		{
			friendslist = new List<string>(new string[]
			{
				"1",
				"2",
				"3"
			}),
			filters = new PDictionary
			{
				{
					"rnd",
					PTestAchievements.rnd
				}
			}
		};
		Playtomic.Achievements.List(options, delegate(List<PlayerAchievement> achievements, PResponse r)
		{
			PTest.AssertTrue("PTestAchievements.ListWithFriends", "Request succeeded", r.success);
			PTest.AssertEquals("PTestAchievements.ListWithFriends", "No errorcode", r.errorcode, 0);
			PTest.AssertEquals("PTestAchievements.ListWithFriends", "Achievement 1 is correct", achievements[0].achievement, "Super Mega Achievement #1");
			PTest.AssertEquals("PTestAchievements.ListWithFriends", "Achievement 2 is correct", achievements[1].achievement, "Super Mega Achievement #2");
			PTest.AssertEquals("PTestAchievements.ListWithFriends", "Achievement 3 is correct", achievements[2].achievement, "Super Mega Achievement #3");
			PTest.AssertTrue("PTestAchievements.ListWithFriends", "Achievement 1 has friends", achievements[0].friends != null);
			PTest.AssertTrue("PTestAchievements.ListWithFriends", "Achievement 2 has friends", achievements[1].friends != null);
			PTest.AssertTrue("PTestAchievements.ListWithFriends", "Achievement 3 has no friends", achievements[2].friends == null);
			PTest.AssertTrue("PTestAchievements.ListWithFriends", "Achievement 1 has 3 friends", achievements[0].friends.Count == 3);
			PTest.AssertTrue("PTestAchievements.ListWithFriends", "Achievement 1 friend 1", achievements[0].friends[0].playername == "ben");
			PTest.AssertTrue("PTestAchievements.ListWithFriends", "Achievement 1 friend 2", achievements[0].friends[1].playername == "michelle");
			PTest.AssertTrue("PTestAchievements.ListWithFriends", "Achievement 1 friend 3", achievements[0].friends[2].playername == "peter");
			PTest.AssertTrue("PTestAchievements.ListWithFriends", "Achievement 2 has 2 friend", achievements[1].friends.Count == 2);
			PTest.AssertTrue("PTestAchievements.ListWithFriends", "Achievement 2 friend 1", achievements[1].friends[0].playername == "michelle");
			PTest.AssertTrue("PTestAchievements.ListWithFriends", "Achievement 2 friend 2", achievements[1].friends[1].playername == "peter");
			done();
		});
	}

	public static void ListWithPlayer(Action done)
	{
		UnityEngine.Debug.Log("PTestAchievements.ListWithPlayer");
		PAchievementOptions options = new PAchievementOptions
		{
			playerid = "1",
			filters = new PDictionary
			{
				{
					"rnd",
					PTestAchievements.rnd
				}
			}
		};
		Playtomic.Achievements.List(options, delegate(List<PlayerAchievement> achievements, PResponse r)
		{
			PTest.AssertTrue("PTestAchievements.ListWithPlayer", "Request succeeded", r.success);
			PTest.AssertEquals("PTestAchievements.ListWithPlayer", "No errorcode", r.errorcode, 0);
			PTest.AssertEquals("PTestAchievements.ListWithPlayer", "Achievement 1 is correct", achievements[0].achievement, "Super Mega Achievement #1");
			PTest.AssertEquals("PTestAchievements.ListWithPlayer", "Achievement 2 is correct", achievements[1].achievement, "Super Mega Achievement #2");
			PTest.AssertEquals("PTestAchievements.ListWithPlayer", "Achievement 3 is correct", achievements[2].achievement, "Super Mega Achievement #3");
			PTest.AssertTrue("PTestAchievements.ListWithPlayer", "Achievement 1 has no friends", achievements[0].friends == null);
			PTest.AssertTrue("PTestAchievements.ListWithPlayer", "Achievement 2 has no friends", achievements[1].friends == null);
			PTest.AssertTrue("PTestAchievements.ListWithPlayer", "Achievement 3 has no friends", achievements[2].friends == null);
			PTest.AssertTrue("PTestAchievements.ListWithPlayer", "Achievement 1 has does have player", achievements[0].player != null);
			PTest.AssertTrue("PTestAchievements.ListWithPlayer", "Achievement 2 has no player", achievements[1].player == null);
			PTest.AssertTrue("PTestAchievements.ListWithPlayer", "Achievement 3 has no player", achievements[2].player == null);
			PTest.AssertTrue("PTestAchievements.ListWithPlayer", "Achievement 1 player is ben", achievements[0].player.playername == "ben");
			done();
		});
	}

	public static void ListWithPlayerAndFriends(Action done)
	{
		UnityEngine.Debug.Log("PTestAchievements.ListWithPlayerAndFriends");
		PAchievementOptions options = new PAchievementOptions
		{
			playerid = "1",
			filters = new PDictionary
			{
				{
					"rnd",
					PTestAchievements.rnd
				}
			},
			friendslist = new List<string>(new string[]
			{
				"1",
				"2",
				"3"
			})
		};
		Playtomic.Achievements.List(options, delegate(List<PlayerAchievement> achievements, PResponse r)
		{
			PTest.AssertTrue("PTestAchievements.ListWithPlayerAndFriends", "Request succeeded", r.success);
			PTest.AssertEquals("PTestAchievements.ListWithPlayerAndFriends", "No errorcode", r.errorcode, 0);
			PTest.AssertEquals("PTestAchievements.ListWithPlayerAndFriends", "Achievement 1 is correct", achievements[0].achievement, "Super Mega Achievement #1");
			PTest.AssertEquals("PTestAchievements.ListWithPlayerAndFriends", "Achievement 2 is correct", achievements[1].achievement, "Super Mega Achievement #2");
			PTest.AssertEquals("PTestAchievements.ListWithPlayerAndFriends", "Achievement 3 is correct", achievements[2].achievement, "Super Mega Achievement #3");
			PTest.AssertTrue("PTestAchievements.ListWithPlayerAndFriends", "Achievement 1 has player", achievements[0].player != null);
			PTest.AssertTrue("PTestAchievements.ListWithPlayerAndFriends", "Achievement 1 has friends", achievements[0].friends != null);
			PTest.AssertTrue("PTestAchievements.ListWithPlayerAndFriends", "Achievement 2 has friends", achievements[1].friends != null);
			PTest.AssertTrue("PTestAchievements.ListWithPlayerAndFriends", "Achievement 2 has no player", achievements[1].player == null);
			PTest.AssertTrue("PTestAchievements.ListWithPlayerAndFriends", "Achievement 3 has no friends", achievements[2].friends == null);
			PTest.AssertTrue("PTestAchievements.ListWithPlayerAndFriends", "Achievement 3 has no player", achievements[2].player == null);
			PTest.AssertTrue("PTestAchievements.ListWithPlayerAndFriends", "Achievement 1 player", achievements[0].player.playername == "ben");
			PTest.AssertTrue("PTestAchievements.ListWithPlayerAndFriends", "Achievement 1 has 2 friend", achievements[0].friends.Count == 2);
			PTest.AssertTrue("PTestAchievements.ListWithPlayerAndFriends", "Achievement 1 friend 1", achievements[0].friends[0].playername == "michelle");
			PTest.AssertTrue("PTestAchievements.ListWithPlayerAndFriends", "Achievement 1 friend 2", achievements[0].friends[1].playername == "peter");
			PTest.AssertTrue("PTestAchievements.ListWithPlayerAndFriends", "Achievement 2 has 2 friend", achievements[1].friends.Count == 2);
			PTest.AssertTrue("PTestAchievements.ListWithPlayerAndFriends", "Achievement 2 friend 1", achievements[1].friends[0].playername == "michelle");
			PTest.AssertTrue("PTestAchievements.ListWithPlayerAndFriends", "Achievement 2 friend 2", achievements[1].friends[1].playername == "peter");
			done();
		});
	}

	public static void Stream(Action done)
	{
		UnityEngine.Debug.Log("PTestAchievements.Stream");
		PAchievementStreamOptions options = new PAchievementStreamOptions
		{
			filters = new PDictionary
			{
				{
					"rnd",
					PTestAchievements.rnd
				}
			}
		};
		Playtomic.Achievements.Stream(options, delegate(List<PlayerAward> achievements, int numachievements, PResponse r)
		{
			PTest.AssertTrue("PTestAchievements.Stream", "Request succeeded", r.success);
			PTest.AssertEquals("PTestAchievements.Stream", "No errorcode", r.errorcode, 0);
			PTest.AssertTrue("PTestAchievements.Stream", "5 achievements returned", achievements.Count == 5);
			PTest.AssertTrue("PTestAchievements.Stream", "5 achievements in total", numachievements == 5);
			PTest.AssertTrue("PTestAchievements.Stream", "Achievement 1 person", achievements[0].playername == "michelle");
			PTest.AssertTrue("PTestAchievements.Stream", "Achievement 1 achievement", achievements[0].awarded.achievement == "Super Mega Achievement #2");
			PTest.AssertTrue("PTestAchievements.Stream", "Achievement 2 person", achievements[1].playername == "peter");
			PTest.AssertTrue("PTestAchievements.Stream", "Achievement 2 achievement", achievements[1].awarded.achievement == "Super Mega Achievement #2");
			PTest.AssertTrue("PTestAchievements.Stream", "Achievement 3 person", achievements[2].playername == "peter");
			PTest.AssertTrue("PTestAchievements.Stream", "Achievement 3 achievement", achievements[2].awarded.achievement == "Super Mega Achievement #1");
			PTest.AssertTrue("PTestAchievements.Stream", "Achievement 4 person", achievements[3].playername == "michelle");
			PTest.AssertTrue("PTestAchievements.Stream", "Achievement 4 achievement", achievements[3].awarded.achievement == "Super Mega Achievement #1");
			PTest.AssertTrue("PTestAchievements.Stream", "Achievement 5 person", achievements[4].playername == "ben");
			PTest.AssertTrue("PTestAchievements.Stream", "Achievement 5 achievement", achievements[4].awarded.achievement == "Super Mega Achievement #1");
			done();
		});
	}

	public static void StreamWithFriends(Action done)
	{
		UnityEngine.Debug.Log("PTestAchievements.StreamWithFriends");
		PAchievementStreamOptions options = new PAchievementStreamOptions
		{
			group = true,
			friendslist = new List<string>(new string[]
			{
				"2",
				"3"
			}),
			filters = new PDictionary
			{
				{
					"rnd",
					PTestAchievements.rnd
				}
			}
		};
		Playtomic.Achievements.Stream(options, delegate(List<PlayerAward> achievements, int numachievements, PResponse r)
		{
			PTest.AssertTrue("PTestAchievements.StreamWithFriends", "Request succeeded", r.success);
			PTest.AssertEquals("PTestAchievements.StreamWithFriends", "No errorcode", r.errorcode, 0);
			PTest.AssertTrue("PTestAchievements.StreamWithFriends", "2 achievements returned", achievements.Count == 2);
			PTest.AssertTrue("PTestAchievements.StreamWithFriends", "2 achievements in total", numachievements == 2);
			PTest.AssertTrue("PTestAchievements.StreamWithFriends", "Achievement 1 awards", achievements[0].awards == 2L);
			PTest.AssertTrue("PTestAchievements.StreamWithFriends", "Achievement 1 achievement", achievements[0].awarded.achievement == "Super Mega Achievement #2");
			PTest.AssertTrue("PTestAchievements.StreamWithFriends", "Achievement 1 person", achievements[0].playername == "michelle");
			PTest.AssertTrue("PTestAchievements.StreamWithFriends", "Achievement 2 awards", achievements[1].awards == 2L);
			PTest.AssertTrue("PTestAchievements.StreamWithFriends", "Achievement 2 achievement", achievements[1].awarded.achievement == "Super Mega Achievement #2");
			PTest.AssertTrue("PTestAchievements.StreamWithFriends", "Achievement 2 person", achievements[1].playername == "peter");
			done();
		});
	}

	public static void StreamWithPlayerAndFriends(Action done)
	{
		UnityEngine.Debug.Log("PTestAchievements.StreamWithPlayerAndFriends");
		PAchievementStreamOptions options = new PAchievementStreamOptions
		{
			group = true,
			friendslist = new List<string>(new string[]
			{
				"2",
				"3"
			}),
			playerid = "1",
			filters = new PDictionary
			{
				{
					"rnd",
					PTestAchievements.rnd
				}
			}
		};
		Playtomic.Achievements.Stream(options, delegate(List<PlayerAward> achievements, int numachievements, PResponse r)
		{
			PTest.AssertTrue("PTestAchievements.StreamWithPlayerAndFriends", "Request succeeded", r.success);
			PTest.AssertEquals("PTestAchievements.StreamWithPlayerAndFriends", "No errorcode", r.errorcode, 0);
			PTest.AssertTrue("PTestAchievements.StreamWithPlayerAndFriends", "3 achievements returned", achievements.Count == 3);
			PTest.AssertTrue("PTestAchievements.StreamWithPlayerAndFriends", "3 achievements in total", numachievements == 3);
			PTest.AssertTrue("PTestAchievements.StreamWithPlayerAndFriends", "Achievement 1 person", achievements[0].playername == "michelle");
			PTest.AssertTrue("PTestAchievements.StreamWithPlayerAndFriends", "Achievement 1 awards", achievements[0].awards == 2L);
			PTest.AssertTrue("PTestAchievements.StreamWithPlayerAndFriends", "Achievement 1 achievement", achievements[0].awarded.achievement == "Super Mega Achievement #2");
			PTest.AssertTrue("PTestAchievements.StreamWithPlayerAndFriends", "Achievement 2 person", achievements[1].playername == "peter");
			PTest.AssertTrue("PTestAchievements.StreamWithPlayerAndFriends", "Achievement 2 awards", achievements[1].awards == 2L);
			PTest.AssertTrue("PTestAchievements.StreamWithPlayerAndFriends", "Achievement 2 achievement", achievements[1].awarded.achievement == "Super Mega Achievement #2");
			PTest.AssertTrue("PTestAchievements.StreamWithPlayerAndFriends", "Achievement 3 person", achievements[2].playername == "ben");
			PTest.AssertTrue("PTestAchievements.StreamWithPlayerAndFriends", "Achievement 3 awards", achievements[2].awards == 1L);
			PTest.AssertTrue("PTestAchievements.StreamWithPlayerAndFriends", "Achievement 3 achievement", achievements[2].awarded.achievement == "Super Mega Achievement #1");
			done();
		});
	}

	public static void Save(Action done)
	{
		UnityEngine.Debug.Log("PTestAchievements.Save");
		PlayerAchievement achievement = new PlayerAchievement
		{
			{
				"achievement",
				"Super Mega Achievement #1"
			},
			{
				"achievementkey",
				"secretkey"
			},
			{
				"playerid",
				PTestAchievements.rnd.ToString()
			},
			{
				"playername",
				"a random name " + PTestAchievements.rnd
			},
			{
				"fields",
				new Dictionary<string, object>
				{
					{
						"rnd",
						PTestAchievements.rnd
					}
				}
			}
		};
		Playtomic.Achievements.Save(achievement, delegate(PResponse r)
		{
			PTest.AssertTrue("PTestAchievements.Save#1", "Request succeeded", r.success);
			PTest.AssertEquals("PTestAchievements.Save#1", "No errorcode", r.errorcode, 0);
			Playtomic.Achievements.Save(achievement, delegate(PResponse r2)
			{
				PTest.AssertFalse("PTestAchievements.Save#2", "Request failed", r2.success);
				PTest.AssertEquals("PTestAchievements.Save#2", "Already had achievement errorcode", r2.errorcode, 505);
				achievement.overwrite = true;
				Playtomic.Achievements.Save(achievement, delegate(PResponse r3)
				{
					PTest.AssertTrue("PTestAchievements.Save#3", "Request succeeded", r3.success);
					PTest.AssertEquals("PTestAchievements.Save#3", "Already had achievement errorcode", r3.errorcode, 506);
					achievement.allowduplicates = true;
					achievement.Remove("overwrite");
					Playtomic.Achievements.Save(achievement, delegate(PResponse r4)
					{
						PTest.AssertTrue("PTestAchievements.Save#4", "Request succeeded", r4.success);
						PTest.AssertEquals("PTestAchievements.Save#4", "Already had achievement errorcode", r4.errorcode, 506);
						done();
					});
				});
			});
		});
	}

	public static int rnd;
}
