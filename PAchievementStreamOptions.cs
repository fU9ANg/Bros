// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class PAchievementStreamOptions : PDictionary
{
	public bool group
	{
		get
		{
			return base.GetBool("group");
		}
		set
		{
			base.SetProperty("group", value);
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
}
