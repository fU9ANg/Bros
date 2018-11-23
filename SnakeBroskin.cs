// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SnakeBroskin : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void AnimateZipline()
	{
		base.AnimateZipline();
		this.SetGunPosition(3f, 0f);
	}

	protected override void UseFire()
	{
		this.FireWeapon(this.x + base.transform.localScale.x * 14f, this.y + 8.5f, base.transform.localScale.x * 750f, 0f);
		this.fireDelay = 0.4f;
		this.PlayAttackSound();
		Map.DisturbWildLife(this.x, this.y, 120f, this.playerNum);
		SortOfFollow.Shake(0.15f);
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 3;
		this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
		EffectsController.CreateMuzzleFlashMediumEffect(x, y, -25f, xSpeed * 0.06f, ySpeed * 0.06f, base.transform);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed, ySpeed - 10f + UnityEngine.Random.value * 20f, this.playerNum);
	}
}
