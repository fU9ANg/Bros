// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class GoliathMech : Mech
{
	public override void Death(float xI, float yI, DamageObject damage)
	{
		base.Death(xI, yI, damage);
		this.eye.gameObject.SetActive(false);
		GameModeController.LevelFinish(LevelResult.Success);
	}

	public override bool IsHeavy()
	{
		return true;
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
		if (this.health < this.maxHealth / 2)
		{
			this.eye.GetComponent<Renderer>().sharedMaterial = this.angryEyeMaterial;
		}
	}

	public SpriteSM eye;

	public Material angryEyeMaterial;
}
