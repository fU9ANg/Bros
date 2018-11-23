// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class PPlayerLevelOptions : PDictionary
{
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

	public string datemin
	{
		get
		{
			return base.GetString("datemin");
		}
		set
		{
			base.SetProperty("datemin", value);
		}
	}

	public string datemax
	{
		get
		{
			return base.GetString("datemax");
		}
		set
		{
			base.SetProperty("datemax", value);
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

	public bool data
	{
		get
		{
			return base.GetBool("data");
		}
		set
		{
			base.SetProperty("data", value);
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
