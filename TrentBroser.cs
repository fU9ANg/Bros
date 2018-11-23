// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TrentBroser : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void UseFire()
	{
		this.FireWeapon(this.x + base.transform.localScale.x * 14f, this.y + 8.5f, base.transform.localScale.x * 800f, (float)UnityEngine.Random.Range(-30, 30));
		this.fireDelay = this.fireRate;
		this.PlayAttackSound();
		Map.DisturbWildLife(this.x, this.y, 80f, this.playerNum);
		SortOfFollow.Shake(0.05f);
	}
}
