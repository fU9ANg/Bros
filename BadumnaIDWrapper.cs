// dnSpy decompiler from Assembly-CSharp.dll
using System;
using Badumna.Match;
using UnityEngine;

public class BadumnaIDWrapper : IDWrapper
{
	public BadumnaIDWrapper(MemberIdentity ID)
	{
		this.underlyingID = ID.ToString();
	}

	public override bool Equals(object obj)
	{
		UnityEngine.Debug.Log("BadumnaIDWrapper ");
		return base.Equals(obj);
	}
}
