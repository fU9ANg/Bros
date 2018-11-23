// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerAchievement : PDictionary
{
	public PlayerAchievement()
	{
	}

	public PlayerAchievement(Dictionary<string, object> data)
	{
		foreach (string text in data.Keys)
		{
			if (text == "player")
			{
				this.player = new PlayerAward((Dictionary<string, object>)data["player"]);
			}
			else if (text == "friends")
			{
				List<object> source = (List<object>)data[text];
				List<PlayerAward> list = new List<PlayerAward>();
				list.AddRange(from object t in source
				select new PlayerAward((Dictionary<string, object>)t));
				this.friends = list;
			}
			else
			{
				this[text] = data[text];
			}
		}
	}

	public string achievement
	{
		get
		{
			return base.GetString("achievement");
		}
		set
		{
			base.SetProperty("achievement", value);
		}
	}

	public string achievementkey
	{
		get
		{
			return base.GetString("achievementkey");
		}
		set
		{
			base.SetProperty("achievementkey", value);
		}
	}

	public string source
	{
		get
		{
			return base.GetString("source");
		}
		set
		{
			base.SetProperty("source", value);
		}
	}

	public string playerid
	{
		get
		{
			return base.GetString("playerid");
		}
		set
		{
			base.SetProperty("playerid", value);
		}
	}

	public string playername
	{
		get
		{
			return base.GetString("playername");
		}
		set
		{
			base.SetProperty("playername", value);
		}
	}

	public Dictionary<string, object> fields
	{
		get
		{
			return (!this.ContainsKey("fields")) ? new Dictionary<string, object>() : ((Dictionary<string, object>)this["fields"]);
		}
		set
		{
			base.SetProperty("fields", value);
		}
	}

	public PlayerAward player
	{
		get
		{
			return (!this.ContainsKey("player")) ? null : ((PlayerAward)this["player"]);
		}
		private set
		{
			base.SetProperty("player", value);
		}
	}

	public List<PlayerAward> friends
	{
		get
		{
			return (!this.ContainsKey("friends")) ? null : ((List<PlayerAward>)this["friends"]);
		}
		private set
		{
			base.SetProperty("friends", value);
		}
	}

	public bool allowduplicates
	{
		get
		{
			return this.ContainsKey("allowduplicates") && (bool)this["allowduplicates"];
		}
		set
		{
			base.SetProperty("allowduplicates", value);
		}
	}

	public bool overwrite
	{
		get
		{
			return this.ContainsKey("overwrite") && (bool)this["overwrite"];
		}
		set
		{
			base.SetProperty("overwrite", value);
		}
	}
}
