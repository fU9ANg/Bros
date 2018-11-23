// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class IDWrapper
{
	public IDWrapper()
	{
	}

	public IDWrapper(string ID, string _name)
	{
		this.underlyingID = ID;
		this.name = _name;
	}

	public string UnderlyingID
	{
		get
		{
			return this.underlyingID;
		}
	}

	public string Name
	{
		get
		{
			return this.name;
		}
	}

	public override string ToString()
	{
		return this.underlyingID + " " + this.name;
	}

	public override bool Equals(object obj)
	{
		IDWrapper idwrapper = obj as IDWrapper;
		return idwrapper != null && this.underlyingID == idwrapper.underlyingID;
	}

	public override int GetHashCode()
	{
		return this.underlyingID.GetHashCode();
	}

	protected string underlyingID = "not set";

	protected string name = "not set";
}
