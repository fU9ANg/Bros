// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookMortar : Mook
{
	protected override void UseFire()
	{
		this.FireWeapon(this.x + base.transform.localScale.x * 17f, this.y + 6.5f, 0f, 450f);
		Map.DisturbWildLife(this.x, this.y, 40f, this.playerNum);
	}

	public Material unarmedMaterial;
}
