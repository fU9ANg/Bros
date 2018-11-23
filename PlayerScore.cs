// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;

public class PlayerScore : PDictionary
{
	public PlayerScore()
	{
	}

	public PlayerScore(IDictionary data)
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

	public long points
	{
		get
		{
			return base.GetLong("points");
		}
		set
		{
			base.SetProperty("points", value);
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

	public long rank
	{
		get
		{
			return base.GetLong("rank");
		}
		set
		{
			base.SetProperty("rank", value);
		}
	}

	public string table
	{
		get
		{
			return base.GetString("table");
		}
		set
		{
			base.SetProperty("table", value);
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

	public PDictionary fields
	{
		get
		{
			return base.GetDictionary("fields");
		}
		set
		{
			base.SetProperty("fields", value);
		}
	}

	public PDictionary filters
	{
		get
		{
			return base.GetDictionary("filters");
		}
		set
		{
			base.SetProperty("filters", value);
		}
	}

	public bool highest
	{
		get
		{
			return this.ContainsKey("highest") && (bool)this["highest"];
		}
		set
		{
			base.SetProperty("highest", value);
		}
	}

	public bool submitted
	{
		get
		{
			return this.ContainsKey("submitted") && (bool)this["submitted"];
		}
		set
		{
			base.SetProperty("submitted", value);
		}
	}

	public bool lowest
	{
		get
		{
			return this.ContainsKey("lowest") && (bool)this["lowest"];
		}
		set
		{
			base.SetProperty("lowest", value);
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

	public long perpage
	{
		get
		{
			return base.GetLong("perpage");
		}
		set
		{
			base.SetProperty("perpage", value);
		}
	}
}
