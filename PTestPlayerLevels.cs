// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

internal class PTestPlayerLevels : PTest
{
	public static void Create(Action done)
	{
		UnityEngine.Debug.Log("PTestPlaytomic.PlayerLevels.Create");
		PlayerLevel level = new PlayerLevel
		{
			name = "create level" + PTestPlayerLevels.rnd,
			playername = "ben" + PTestPlayerLevels.rnd,
			playerid = "0",
			data = "this is the level data",
			fields = new Dictionary<string, object>
			{
				{
					"rnd",
					PTestPlayerLevels.rnd
				},
				{
					"pennies",
					"lol"
				}
			}
		};
		Playtomic.PlayerLevels.Save(level, delegate(PlayerLevel l, PResponse r)
		{
			l = (l ?? new PlayerLevel());
			PTest.AssertTrue("PTestPlaytomic.PlayerLevels.Create#1", "Request succeeded", r.success);
			PTest.AssertEquals("PTestPlaytomic.PlayerLevels.Create#1", "No errorcode", r.errorcode, 0);
			PTest.AssertTrue("PTestPlaytomic.PlayerLevels.Create#1", "Returned level is not null", l.Keys.Count > 0);
			PTest.AssertTrue("PTestPlaytomic.PlayerLevels.Create#1", "Returned level has levelid", l.ContainsKey("levelid"));
			PTest.AssertEquals("PTestPlaytomic.PlayerLevels.Create#1", "Level names match", level.name, l.name);
			Playtomic.PlayerLevels.Save(level, delegate(PlayerLevel l2, PResponse r2)
			{
				PTest.AssertTrue("PTestPlaytomic.PlayerLevels.Create#2", "Request succeeded", r2.success);
				PTest.AssertEquals("PTestPlaytomic.PlayerLevels.Create#2", "Duplicate level errorcode", r2.errorcode, 405);
				done();
			});
		});
	}

	public static void List(Action done)
	{
		UnityEngine.Debug.Log("PTestPlaytomic.PlayerLevels.List");
		PPlayerLevelOptions listoptions = new PPlayerLevelOptions
		{
			page = 1,
			perpage = 10,
			data = false,
			filters = new PDictionary
			{
				{
					"rnd",
					PTestPlayerLevels.rnd
				}
			}
		};
		Playtomic.PlayerLevels.List(listoptions, delegate(List<PlayerLevel> levels, int numlevels, PResponse r)
		{
			levels = (levels ?? new List<PlayerLevel>());
			PTest.AssertTrue("PTestPlaytomic.PlayerLevels.List#1", "Request succeeded", r.success);
			PTest.AssertEquals("PTestPlaytomic.PlayerLevels.List#1", "No errorcode", r.errorcode, 0);
			PTest.AssertTrue("PTestPlaytomic.PlayerLevels.List#1", "Received levels", levels.Count > 0);
			PTest.AssertTrue("PTestPlaytomic.PlayerLevels.List#1", "Received numlevels", numlevels >= levels.Count);
			if (levels.Count > 0)
			{
				PTest.AssertFalse("PTestPlaytomic.PlayerLevels.List#1", "First level has no data", levels[0].ContainsKey("data"));
			}
			else
			{
				PTest.AssertTrue("PTestPlaytomic.PlayerLevels.List#1", "First level has no data forced failure", false);
			}
			listoptions["data"] = true;
			Playtomic.PlayerLevels.List(listoptions, delegate(List<PlayerLevel> levels2, int numlevels2, PResponse r2)
			{
				levels2 = (levels2 ?? new List<PlayerLevel>());
				PTest.AssertTrue("PTestPlaytomic.PlayerLevels.List#2", "Request succeeded", r2.success);
				PTest.AssertEquals("PTestPlaytomic.PlayerLevels.List#2", "No errorcode", r2.errorcode, 0);
				PTest.AssertTrue("PTestPlaytomic.PlayerLevels.List#2", "Received levels", levels2.Count > 0);
				PTest.AssertTrue("PTestPlaytomic.PlayerLevels.List#2", "Received numlevels", numlevels2 >= levels2.Count);
				if (levels2.Count > 0)
				{
					PTest.AssertTrue("PTestPlaytomic.PlayerLevels.List", "First level has data", levels2[0].ContainsKey("data"));
				}
				else
				{
					PTest.AssertTrue("PTestPlaytomic.PlayerLevels.List", "First level has no data forced failure", false);
				}
				done();
			});
		});
	}

	public static void Rate(Action done)
	{
		UnityEngine.Debug.Log("TestPlaytomic.PlayerLevels.Rate");
		PlayerLevel level = new PlayerLevel
		{
			name = "rate " + PTestPlayerLevels.rnd,
			playername = "ben" + PTestPlayerLevels.rnd,
			playerid = "0",
			data = "this is the level data",
			fields = new Dictionary<string, object>
			{
				{
					"rnd",
					PTestPlayerLevels.rnd
				}
			}
		};
		Playtomic.PlayerLevels.Save(level, delegate(PlayerLevel l, PResponse r)
		{
			l = (l ?? new PlayerLevel());
			PTest.AssertTrue("TestPlaytomic.PlayerLevels.Rate#1", "Request succeeded", r.success);
			PTest.AssertEquals("TestPlaytomic.PlayerLevels.Rate#1", "No errorcode", r.errorcode, 0);
			PTest.AssertTrue("TestPlaytomic.PlayerLevels.Rate#1", "Returned level is not null", l.Keys.Count > 0);
			PTest.AssertTrue("TestPlaytomic.PlayerLevels.Rate#1", "Returned level has levelid", l.ContainsKey("levelid"));
			Playtomic.PlayerLevels.Rate(l.levelid, 70, delegate(PResponse r2)
			{
				PTest.AssertFalse("TestPlaytomic.PlayerLevels.Rate#2", "Request failed", r2.success);
				PTest.AssertEquals("TestPlaytomic.PlayerLevels.Rate#2", "Invalid rating errorcode", r2.errorcode, 401);
				Playtomic.PlayerLevels.Rate(l.levelid, 7, delegate(PResponse r3)
				{
					PTest.AssertTrue("TestPlaytomic.PlayerLevels.Rate#3", "Request succeeded", r3.success);
					PTest.AssertEquals("TestPlaytomic.PlayerLevels.Rate#3", "No errrorcode", r3.errorcode, 0);
					Playtomic.PlayerLevels.Rate(l.levelid, 6, delegate(PResponse r4)
					{
						PTest.AssertFalse("TestPlaytomic.PlayerLevels.Rate#4", "Request failed", r4.success);
						PTest.AssertEquals("TestPlaytomic.PlayerLevels.Rate#4", "Already rated errorcode", r4.errorcode, 402);
						done();
					});
				});
			});
		});
	}

	public static void Load(Action done)
	{
		UnityEngine.Debug.Log("TestPlaytomic.PlayerLevels.Load");
		PlayerLevel level = new PlayerLevel
		{
			name = "sample loading level " + PTestPlayerLevels.rnd,
			playername = "ben" + PTestPlayerLevels.rnd,
			playerid = PTestPlayerLevels.rnd.ToString(CultureInfo.InvariantCulture),
			data = "this is the level data",
			fields = new Dictionary<string, object>
			{
				{
					"rnd",
					PTestPlayerLevels.rnd
				}
			}
		};
		Playtomic.PlayerLevels.Save(level, delegate(PlayerLevel l, PResponse r)
		{
			PTest.AssertTrue("TestPlaytomic.PlayerLevels.Load#1", "Request succeeded", r.success);
			PTest.AssertEquals("TestPlaytomic.PlayerLevels.Load#1", "No errorcode", r.errorcode, 0);
			PTest.AssertTrue("TestPlaytomic.PlayerLevels.Load#1", "Name is correct", l.ContainsKey("levelid"));
			PTest.AssertEquals("TestPlaytomic.PlayerLevels.Load#1", "Name is correct", level.name, l.name);
			PTest.AssertEquals("TestPlaytomic.PlayerLevels.Load#1", "Data is correct", level.data, l.data);
			Playtomic.PlayerLevels.Load(l.levelid, delegate(PlayerLevel l2, PResponse r2)
			{
				PTest.AssertTrue("TestPlaytomic.PlayerLevels.Load", "Request succeeded", r2.success);
				PTest.AssertEquals("TestPlaytomic.PlayerLevels.Load", "No errorcode", r2.errorcode, 0);
				PTest.AssertEquals("TestPlaytomic.PlayerLevels.Load", "Name is correct", level.name, l2.name);
				PTest.AssertEquals("TestPlaytomic.PlayerLevels.Load", "Data is correct", level.data, l2.data);
				done();
			});
		});
	}

	public static int rnd;
}
