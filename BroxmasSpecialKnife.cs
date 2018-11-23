// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BroxmasSpecialKnife : BrocheteMachete
{
	protected override void RunProjectile(float t)
	{
		base.RunProjectile(t);
		this.yI -= 1400f * t;
		this.SetRotation();
	}

	protected override void ProjectileApplyDamageToBlock(GameObject blockObject, int damage, DamageType type, float forceX, float forceY)
	{
		MapController.Damage_Networked(this.firedBy, blockObject, 2, type, forceX, forceY);
	}
}
