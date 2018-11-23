// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BrodellWalker : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void UseFire()
	{
		this.FireWeapon(this.x + base.transform.localScale.x * 11f, this.y + 8f, base.transform.localScale.x * 300f, 0f);
		this.fireDelay = 0.2f;
		this.PlayAttackSound();
		Map.DisturbWildLife(this.x, this.y, 80f, this.playerNum);
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		EffectsController.CreateShrapnel(this.bulletShell, x - base.transform.localScale.x * 2f, y + 6f, 1f, 30f, 1f, -base.transform.localScale.x * 80f, 170f);
		EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, base.transform);
		this.gunFrame = 3;
		this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed * 0.83f, ySpeed + 40f + UnityEngine.Random.value * 35f, this.playerNum);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed * 0.9f, ySpeed + 2f + UnityEngine.Random.value * 15f, this.playerNum);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed * 0.9f, ySpeed - 2f - UnityEngine.Random.value * 15f, this.playerNum);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed * 0.85f, ySpeed - 40f - UnityEngine.Random.value * 35f, this.playerNum);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed * 0.85f, ySpeed - 50f + UnityEngine.Random.value * 80f, this.playerNum);
	}

	public Shrapnel bulletShell;
}
