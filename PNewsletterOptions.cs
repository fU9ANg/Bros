// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class PNewsletterOptions : PDictionary
{
	public string email
	{
		get
		{
			return base.GetString("email");
		}
		set
		{
			base.SetProperty("email", value);
		}
	}

	public string firstname
	{
		get
		{
			return base.GetString("firstname");
		}
		set
		{
			base.SetProperty("firstname", value);
		}
	}

	public string lastname
	{
		get
		{
			return base.GetString("lastname");
		}
		set
		{
			base.SetProperty("lastname", value);
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
}
