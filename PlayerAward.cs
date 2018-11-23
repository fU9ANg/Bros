// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class PlayerAward : PDictionary
{
	public PlayerAward()
	{
	}

	public PlayerAward(Dictionary<string, object> data)
	{
		foreach (string text in data.Keys)
		{
			if (text == "date")
			{
				DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);
				this.date = dateTime.AddSeconds((double)data[text]);
			}
			else if (text == "awarded")
			{
				this.awarded = new PlayerAchievement((Dictionary<string, object>)data[text]);
			}
			else
			{
				this[text] = data[text];
			}
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

	public PlayerAchievement awarded
	{
		get
		{
			return (!this.ContainsKey("awarded")) ? null : ((PlayerAchievement)this["awarded"]);
		}
		private set
		{
			base.SetProperty("awarded", value);
		}
	}

	public long awards
	{
		get
		{
			return base.GetLong("awards");
		}
		set
		{
			base.SetProperty("awards", value);
		}
	}

	public DateTime date
	{
		get
		{
			return (!this.ContainsKey("date")) ? DateTime.Now : ((DateTime)this["date"]);
		}
		private set
		{
			base.SetProperty("date", value);
		}
	}

	public string rdate
	{
		get
		{
			return base.GetString("rdate");
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
}
