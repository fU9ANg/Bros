// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BulletRambro : Projectile
{
	public override void Fire(float x, float y, float xI, float yI, int playerNum, MonoBehaviour FiredBy)
	{
		base.Fire(x, y, xI, yI, playerNum, FiredBy);
		if (BulletRambro.fireCount % 2 == 1)
		{
			this.damageInternal = this.everySecondDamage; this.damage = (this.damageInternal );
		}
		BulletRambro.fireCount++;
	}

	public int everySecondDamage = 2;

	protected static int fireCount;
}
