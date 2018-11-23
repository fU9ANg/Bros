// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Brommando : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void UseFire()
	{
		if (base.IsMine)
		{
			this.FireWeapon(this.x + base.transform.localScale.x * 12f, this.y + 9f, base.transform.localScale.x * (float)(150 + ((!Demonstration.bulletsAreFast) ? 0 : 150)), 0f);
		}
		this.fireDelay = 0.6f;
		this.PlayAttackSound();
		Map.DisturbWildLife(this.x, this.y, 80f, this.playerNum);
	}

	protected override void UseSpecial()
	{
		if (base.SpecialAmmo > 0)
		{
			base.SpecialAmmo--;
			HeroController.SetSpecialAmmo(this.playerNum, base.SpecialAmmo);
			if (base.IsMine)
			{
				this.specialX = this.x + base.transform.localScale.x * 16f;
				this.specialY = this.y + 6.5f;
				this.barageDirection = (int)Mathf.Sign(base.transform.localScale.x);
				ProjectileController.SpawnProjectileOverNetwork(this.barageProjectile, this, this.specialX, this.specialY, (float)(this.barageDirection * 150), 0f, false, this.playerNum, false, false);
				this.PlayAttackSound();
				this.firingBarage = true;
				this.barageCounter = 0.1333f;
				this.barageCount = 4;
			}
		}
		else
		{
			HeroController.FlashSpecialAmmo(this.playerNum);
			this.gunSprite.gameObject.SetActive(true);
		}
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 3;
		this.SetGunSprite(this.gunFrame, 0);
		EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, base.transform);
		ProjectileController.SpawnProjectileOverNetwork(this.projectile, this, x, y, xSpeed, ySpeed, false, this.playerNum, false, false);
	}

	protected override void Update()
	{
		base.Update();
		if (this.firingBarage)
		{
			this.barageCounter -= this.t;
			if (this.barageCounter <= 0f)
			{
				this.barageCounter = 0.1333f;
				this.barageCount--;
				if (this.barageCount >= 0)
				{
					ProjectileController.SpawnProjectileOverNetwork(this.barageProjectile, this, this.specialX, this.specialY, (float)(this.barageDirection * 150), 0f, false, this.playerNum, false, false);
					this.PlayAttackSound();
				}
				else
				{
					this.firingBarage = false;
				}
			}
		}
	}

	protected override void AnimateZipline()
	{
		base.AnimateZipline();
		this.SetGunPosition(3f, 0f);
	}

	protected float specialX;

	protected float specialY;

	protected int barageDirection;

	protected int barageCount = 4;

	protected float barageCounter;

	protected bool firingBarage;

	public Projectile barageProjectile;
}
