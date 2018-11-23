// dnSpy decompiler from Assembly-CSharp.dll
using System;
using ManagedSteam.SteamTypes;
using UnityEngine;

public class SteamIDWrapper : IDWrapper
{
	public SteamIDWrapper(SteamID ID, string _name)
	{
		this.name = SteamController.SteamInterface.Friends.GetPlayerNickname(ID);
		this.underlyingID = ID.AsUInt64 + string.Empty;
		this.id = ID;
	}

	public SteamIDWrapper(string str, string _name)
	{
		this.underlyingID = str;
		this.name = _name;
	}

	public override bool Equals(object obj)
	{
		UnityEngine.Debug.Log("Steam equals ");
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	private SteamID id;
}
