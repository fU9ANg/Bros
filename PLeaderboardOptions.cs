// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class PLeaderboardOptions : PDictionary
{
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

	public bool highest
	{
		get
		{
			return base.GetBool("highest");
		}
		set
		{
			base.SetProperty("highest", value);
		}
	}

	public bool lowest
	{
		get
		{
			return base.GetBool("lowest");
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
			return base.GetBool("allowduplicates");
		}
		set
		{
			base.SetProperty("allowduplicates", value);
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

	public List<string> friendslist
	{
		get
		{
			return base.GetList<string>("friendslist");
		}
		set
		{
			base.SetProperty("friendslist", value);
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

	public bool excludeplayerid
	{
		get
		{
			return base.GetBool("excludeplayerid");
		}
		set
		{
			base.SetProperty("excludeplayerid", value);
		}
	}

	public string mode
	{
		get
		{
			return base.GetString("mode");
		}
		set
		{
			base.SetProperty("mode", value);
		}
	}

	public int page
	{
		get
		{
			return base.GetInt("page");
		}
		set
		{
			base.SetProperty("page", value);
		}
	}

	public int perpage
	{
		get
		{
			return base.GetInt("perpage");
		}
		set
		{
			base.SetProperty("perpage", value);
		}
	}
}
