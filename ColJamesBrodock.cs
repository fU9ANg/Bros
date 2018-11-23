// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ColJamesBrodock : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void PressSpecial()
	{
		if (this.actionState != ActionState.Melee && base.SpecialAmmo > 0 && this.health > 0)
		{
			this.usingSpecial = true;
			this.frame = 0;
			this.gunFrame = 4;
			this.ChangeFrame();
		}
	}

	protected override void UseSpecial()
	{
		if (base.SpecialAmmo > 0)
		{
			this.PlayAttackSound(0.4f);
			base.SpecialAmmo--;
			if (base.IsMine)
			{
				ProjectileController.SpawnGrenadeOverNetwork(this.specialGrenade, this, this.x + Mathf.Sign(base.transform.localScale.x) * 6f, this.y + 10f, 0.001f, 0.011f, Mathf.Sign(base.transform.localScale.x) * this.shootGrenadeSpeedX + this.xI * 0.45f, this.shootGrenadeSpeedY + ((this.yI <= 0f) ? 0f : (this.yI * 0.3f)), this.playerNum);
			}
		}
		else
		{
			HeroController.FlashSpecialAmmo(this.playerNum);
			this.ActivateGun();
		}
	}

	protected override void AnimateSpecial()
	{
		this.SetSpriteOffset(0f, 0f);
		this.ActivateGun();
		this.frameRate = 0.0334f;
		this.SetGunSprite(5 - this.frame, 0);
		if (this.frame == 0)
		{
			this.UseSpecial();
		}
		if (this.frame >= 5)
		{
			this.gunFrame = 0;
			this.frame = 0;
			this.usingSpecial = false;
		}
	}

	protected override void UseFire()
	{
		if (base.IsMine)
		{
			this.FireWeapon(this.x + base.transform.localScale.x * 6f, this.y + 10f, base.transform.localScale.x * this.shootGrenadeSpeedX + this.xI * 0.45f, this.shootGrenadeSpeedY + ((this.yI <= 0f) ? 0f : (this.yI * 0.3f)));
		}
		this.PlayAttackSound(0.4f);
		Map.DisturbWildLife(this.x, this.y, 60f, this.playerNum);
		this.fireDelay = 0.6f;
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 5;
		this.SetGunSprite(this.gunFrame, 1);
		EffectsController.CreateMuzzleFlashRoundEffect(x + 1f, y + 1f, -25f, xSpeed * 0.2f, ySpeed * 0.2f, base.transform);
		this.SetGunSprite(this.gunFrame, 0);
		this.gunCounter = 0f;
		ProjectileController.SpawnGrenadeOverNetwork(this.primaryGrenade, this, x, y, 0.001f, 0.011f, xSpeed, ySpeed, this.playerNum);
	}

	public Grenade primaryGrenade;

	public float shootGrenadeSpeedX = 250f;

	public float shootGrenadeSpeedY = 60f;
}
