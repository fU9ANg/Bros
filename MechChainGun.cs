// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MechChainGun : MechWeapon
{
	protected override void Awake()
	{
		base.Awake();
		this.sprite = base.GetComponent<SpriteSM>();
		this.spritePixelWidth = (int)this.sprite.pixelDimensions.x;
		this.spritePixelHeight = (int)this.sprite.pixelDimensions.y;
	}

	protected override void Start()
	{
		base.Start();
		this.height = 12f;
		this.width = 17f;
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void RunFiring()
	{
		if (this.health <= 0)
		{
			return;
		}
		if (this.fire)
		{
			this.windUpRate = Mathf.Clamp(this.windUpRate - (this.t * 0.3f + this.t * this.windUpRate), 0.025f, 1f);
			if (this.chainGunFrame < 40)
			{
				this.frameCounter += this.t;
				if (this.frameCounter > this.windUpRate)
				{
					this.frameCounter -= this.windUpRate;
					this.chainGunFrame++;
					this.sprite.SetLowerLeftPixel((float)(this.spritePixelWidth * (this.chainGunFrame % 8)), (float)this.spritePixelHeight);
					if (this.chainGunFrame % 8 == 1)
					{
						EffectsController.CreateRedWarningDiamondHuge(base.transform.position.x + (float)this.mech.facingDirection * this.fireXOffset, base.transform.position.y + 11f, base.transform);
					}
				}
			}
			else
			{
				this.frameCounter += this.t;
				if (this.frameCounter > this.windUpRate)
				{
					this.frameCounter -= this.windUpRate;
					this.chainGunFrame++;
					this.sprite.SetLowerLeftPixel((float)(this.spritePixelWidth * (this.chainGunFrame % 8)), (float)(this.spritePixelHeight * 2));
					if (this.chainGunFrame % 4 == 0)
					{
						this.FireWeapon(ref this.fireIndex);
					}
				}
			}
		}
		else if (this.windUpRate < 1f)
		{
			this.windUpRate = Mathf.Clamp(this.windUpRate + (this.t * 0.3f + this.t * this.windUpRate * 0.5f), 0.0334f, 1.2f);
			this.frameCounter += this.t;
			if (this.frameCounter > this.windUpRate)
			{
				this.frameCounter -= this.windUpRate;
				this.chainGunFrame++;
				this.sprite.SetLowerLeftPixel((float)(this.spritePixelWidth * (this.chainGunFrame % 8)), (float)this.spritePixelHeight);
			}
			if (this.windUpRate >= 1f)
			{
				this.chainGunFrame = 0;
			}
		}
		this.wasFire = this.fire;
	}

	protected override void SetDeathFrame()
	{
		this.sprite.SetLowerLeftPixel(0f, 96f);
	}

	protected override void FireWeapon(ref int index)
	{
		index++;
		if (index >= this.chainGunFireLimit)
		{
			this.fire = false;
			this.fireDelay = 0.1f;
			index = 0;
		}
		ProjectileController.SpawnProjectileLocally(this.projectile, this, base.transform.position.x + (float)this.mech.facingDirection * this.fireXOffset, base.transform.position.y + 11f, (float)(this.mech.facingDirection * 450), -15f + UnityEngine.Random.value * 30f, -1);
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.attackSounds, 0.66f, base.transform.position);
	}

	protected int spritePixelHeight;

	protected float windUpRate = 1f;

	public float fireXOffset = 24f;

	protected int chainGunFrame;

	public int chainGunFireLimit = 20;
}
