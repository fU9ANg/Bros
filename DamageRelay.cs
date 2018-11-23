// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DamageRelay : MonoBehaviour
{
	public void Damage(DamageObject damageObject)
	{
		if (this.unit != null && damageObject.damageType != this.immunity)
		{
			this.unit.Damage(damageObject.damage, damageObject.damageType, damageObject.xForce, damageObject.yForce, (int)Mathf.Sign(damageObject.xForce), damageObject.damageSender, damageObject.x, damageObject.y);
		}
		else if (this.doodad != null && damageObject.damageType != this.immunity)
		{
			this.doodad.Damage(damageObject);
		}
		else if (this.bossWeapon != null && damageObject.damageType != this.immunity)
		{
			this.bossWeapon.Damage(damageObject);
		}
	}

	public Unit unit;

	public Doodad doodad;

	public BossBlockWeapon bossWeapon;

	public DamageType immunity = DamageType.Drill;
}
