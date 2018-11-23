// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerLevel : PDictionary
{
	public PlayerLevel()
	{
	}

	public PlayerLevel(IDictionary data)
	{
		foreach (object obj in data.Keys)
		{
			string text = (string)obj;
			if (text == "date")
			{
				DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);
				this.date = dateTime.AddSeconds((double)data[text]);
			}
			else
			{
				this[text] = data[text];
			}
		}
	}

	public string levelid
	{
		get
		{
			return base.GetString("levelid");
		}
		set
		{
			base.SetProperty("levelid", value);
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

	public string name
	{
		get
		{
			return base.GetString("name");
		}
		set
		{
			base.SetProperty("name", value);
		}
	}

	public string data
	{
		get
		{
			return base.GetString("data");
		}
		set
		{
			base.SetProperty("data", value);
		}
	}

	public long votes
	{
		get
		{
			return base.GetLong("votes");
		}
	}

	public long score
	{
		get
		{
			return base.GetLong("score");
		}
	}

	public double rating
	{
		get
		{
			long score = this.score;
			long votes = this.votes;
			if (score == 0L || votes == 0L)
			{
				return 0.0;
			}
			return (double)(score / votes);
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
