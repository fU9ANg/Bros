// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class GameVar : PDictionary
{
	public GameVar()
	{
	}

	public GameVar(Dictionary<string, object> data) : base(data)
	{
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

	public string value
	{
		get
		{
			return base.GetString("value");
		}
		set
		{
			base.SetProperty("value", value);
		}
	}
}
