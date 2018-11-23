// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;

public class PlayerCountry : PDictionary
{
	public PlayerCountry()
	{
	}

	public PlayerCountry(IDictionary data)
	{
		foreach (object obj in data.Keys)
		{
			string key = (string)obj;
			this[key] = data[key];
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

	public string code
	{
		get
		{
			return base.GetString("code");
		}
		set
		{
			base.SetProperty("code", value);
		}
	}
}
