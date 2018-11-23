// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class PAchievementOptions : PDictionary
{
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
}
