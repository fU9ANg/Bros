// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MadMaxBrotansky : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void UseFire()
	{
		this.FireWeapon(this.x + base.transform.localScale.x * 12f, this.y + 6.5f, base.transform.localScale.x * 210f, 0f);
		this.fireDelay = 0.2f;
		this.PlayAttackSound();
		Map.DisturbWildLife(this.x, this.y, 80f, this.playerNum);
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 3;
		this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed * 0.9f, ySpeed + 90f + UnityEngine.Random.value * 20f, this.playerNum);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed * 0.9f, ySpeed - 90f - UnityEngine.Random.value * 20f, this.playerNum);
	}
}
