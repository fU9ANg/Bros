// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TollBroad : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void UseFire()
	{
		this.FireWeapon(this.x + base.transform.localScale.x * 10f, this.y + 8f, base.transform.localScale.x * 300f, (float)UnityEngine.Random.Range(-20, 20));
		this.PlayAttackSound();
		Map.DisturbWildLife(this.x, this.y, 60f, this.playerNum);
		this.fireDelay = 0.4f;
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 5;
		this.SetGunSprite(this.gunFrame, 1);
		FullScreenFlashEffect.FlashHot(0.5f, new Vector3(x, y, 0f));
		EffectsController.CreateSparkParticles(x + 1f, y + 1f, 33, 5f, 1f, xSpeed, 100f, 0.5f, 0.3f);
		EffectsController.CreateSparkParticles(x + 1f, y + 1f, 66, 5f, 1f, xSpeed * 0.8f, 90f, 0.5f, 0.3f);
		EffectsController.CreateSparkParticles(x + 1f, y + 1f, 66, 5f, 1f, xSpeed * 0.6f, 80f, 0.5f, 0.3f);
		EffectsController.CreateMuzzleFlashMediumEffect(x + 1f, y + 1f, -25f, xSpeed * 0.2f, ySpeed * 0.2f, base.transform);
		Map.HitUnits(this, this, this.playerNum, 1, DamageType.Fire, 32f, 12f, x + 12f * base.transform.localScale.x, y, xSpeed * 2f, ySpeed, true, false, false);
		Map.HitUnits(this, this, this.playerNum, 2, DamageType.Explosion, 32f, 12f, x + 12f * base.transform.localScale.x, y, xSpeed * 2f, ySpeed + 60f, true, true, false);
		MapController.DamageGround(this, 15, DamageType.Explosion, 4f, x + base.transform.localScale.x * 4f, y, null);
		MapController.DamageGround(this, 10, DamageType.Explosion, 4f, x + base.transform.localScale.x * 10f, y, null);
		MapController.DamageGround(this, 7, DamageType.Explosion, 4f, x + base.transform.localScale.x * 16f, y, null);
		MapController.DamageGround(this, 5, DamageType.Explosion, 4f, x + base.transform.localScale.x * 22f, y, null);
		Map.HitProjectiles(this.playerNum, 500, DamageType.Fire, 10f, x + base.transform.localScale.x * 7f, y + 10f, base.transform.localScale.x * 150f, 50f, 0f);
		Map.HitProjectiles(this.playerNum, 500, DamageType.Fire, 10f, x + base.transform.localScale.x * 21f, y + 10f, base.transform.localScale.x * 150f, 50f, 0f);
		this.SetGunSprite(this.gunFrame, 0);
		this.gunCounter = 0f;
		ProjectileController.SpawnProjectileLocally(this.flameProjectiles[UnityEngine.Random.Range(0, this.flameProjectiles.Length)], this, x, y, xSpeed * 0.83f, ySpeed + 40f + UnityEngine.Random.value * 35f, this.playerNum);
		ProjectileController.SpawnProjectileLocally(this.flameProjectiles[UnityEngine.Random.Range(0, this.flameProjectiles.Length)], this, x, y, xSpeed * 0.9f, ySpeed + 2f + UnityEngine.Random.value * 15f, this.playerNum);
		ProjectileController.SpawnProjectileLocally(this.flameProjectiles[UnityEngine.Random.Range(0, this.flameProjectiles.Length)], this, x, y, xSpeed * 0.9f, ySpeed - 2f - UnityEngine.Random.value * 15f, this.playerNum);
		ProjectileController.SpawnProjectileLocally(this.flameProjectiles[UnityEngine.Random.Range(0, this.flameProjectiles.Length)], this, x, y, xSpeed * 0.85f, ySpeed - 40f - UnityEngine.Random.value * 35f, this.playerNum);
		ProjectileController.SpawnProjectileLocally(this.flameProjectiles[UnityEngine.Random.Range(0, this.flameProjectiles.Length)], this, x, y, xSpeed * 0.85f, ySpeed - 50f + UnityEngine.Random.value * 80f, this.playerNum);
		ProjectileController.SpawnProjectileLocally(this.flameProjectiles[UnityEngine.Random.Range(0, this.flameProjectiles.Length)], this, x, y, 0.66f * xSpeed * 0.9f, ySpeed + 2f + UnityEngine.Random.value * 15f, this.playerNum);
		ProjectileController.SpawnProjectileLocally(this.flameProjectiles[UnityEngine.Random.Range(0, this.flameProjectiles.Length)], this, x, y, 0.66f * xSpeed * 0.9f, ySpeed - 2f - UnityEngine.Random.value * 15f, this.playerNum);
		ProjectileController.SpawnProjectileLocally(this.flameProjectiles[UnityEngine.Random.Range(0, this.flameProjectiles.Length)], this, x, y, 0.66f * xSpeed * 0.85f, ySpeed - 40f - UnityEngine.Random.value * 35f, this.playerNum);
		ProjectileController.SpawnProjectileLocally(this.flameProjectiles[UnityEngine.Random.Range(0, this.flameProjectiles.Length)], this, x, y, 0.33f * xSpeed * 0.9f, ySpeed - 2f - UnityEngine.Random.value * 15f, this.playerNum);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed * 1.2f, ySpeed - 2f - UnityEngine.Random.value * 15f, this.playerNum);
		if (!this.right)
		{
			if (!this.left)
			{
				if (base.transform.localScale.x > 0f)
				{
				}
			}
		}
	}

	public Projectile[] flameProjectiles;
}
