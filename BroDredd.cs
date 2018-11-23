// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BroDredd : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void UseFire()
	{
		this.FireWeapon(this.x + base.transform.localScale.x * 10f, this.y + 8.5f, base.transform.localScale.x * 320f, 0f);
		this.fireDelay = 0.1f;
		this.PlayAttackSound();
		Map.DisturbWildLife(this.x, this.y, 80f, this.playerNum);
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 3;
		this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
		EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, base.transform);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed, 0f, this.playerNum);
	}

	protected override void UseSpecial()
	{
		if (base.SpecialAmmo > 0)
		{
			base.SpecialAmmo--;
			HeroController.SetSpecialAmmo(this.playerNum, base.SpecialAmmo);
			if (base.IsMine)
			{
				this.remoteProjectile = ProjectileController.SpawnProjectileOverNetwork(this.specialRocketPrefab, this, this.x + 6f * base.transform.localScale.x, this.y + 11f, base.transform.localScale.x * this.remoteProjectileSpeed, 0f, true, this.playerNum, true, false);
				if (this.remoteProjectile != null)
				{
					this.usingSpecial = false;
					this.fire = false;
					this.controllingProjectile = true;
					this.projectileTime = Time.time;
					this.frame = 0;
				}
			}
		}
		else
		{
			HeroController.FlashSpecialAmmo(this.playerNum);
			this.gunSprite.gameObject.SetActive(true);
			this.usingSpecial = false;
		}
	}

	protected override void CheckInput()
	{
		base.CheckInput();
	}

	public Projectile specialRocketPrefab;

	public float remoteProjectileSpeed = 90f;
}
