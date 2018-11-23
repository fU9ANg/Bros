// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TankChaingun : TankWeapon
{
	protected override void Start()
	{
		base.Start();
		this.chaingunSource = base.gameObject.AddComponent<AudioSource>();
		this.chaingunSource.volume = 0.45f;
		this.chaingunSource.pitch = 0.6f;
		this.chaingunSource.loop = false;
		this.chaingunSource.rolloffMode = AudioRolloffMode.Linear;
		this.chaingunSource.minDistance = 150f;
		this.chaingunSource.maxDistance = 420f;
		this.chaingunSource.loop = false;
		this.chaingunSource.dopplerLevel = 0.1f;
		this.chaingunSource.clip = this.chaingunWindUp;
		this.chaingunSource.playOnAwake = false;
		this.chaingunSource.Stop();
		this.invulnerable = true;
	}

	public override void SetSpriteTurn(int frame)
	{
		if (this.health > 0)
		{
			this.fire = false;
			this.currentTurnFrame = frame;
			this.sprite.SetLowerLeftPixel(new Vector2((float)((8 + frame) * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
		}
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		base.Death(xI, yI, damage);
		this.chaingunSource.Stop();
	}

	protected override void SetDeathFrame()
	{
		this.sprite.SetLowerLeftPixel(new Vector2((float)(13 * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
	}

	protected override void RunFiring()
	{
		this.fireDelay -= this.t;
		if (this.fire && this.health > 0 && this.tank.CanFire())
		{
			if (!this.wasFire)
			{
				this.chaingunSource.clip = this.chaingunWindUp;
				this.chaingunSource.Play();
			}
			if (this.fireDelay <= 0f)
			{
				this.fireCounter += this.t;
				int num = (int)(this.fireCounter / this.chaingunFrameRate);
				for (int i = 0; i < num; i++)
				{
					this.fireCounter -= this.chaingunFrameRate;
					this.ChangeChaingunFrame();
				}
			}
		}
		this.wasFire = this.fire;
	}

	protected void ChangeChaingunFrame()
	{
		this.frame++;
		if (this.frame < this.chaingunFiringFrameCount * 5)
		{
			if (this.frame % 5 == 1)
			{
				EffectsController.CreateRedWarningDiamondHuge(base.transform.position.x + (float)this.tank.facingDirection * this.fireOffsetX, base.transform.position.y + this.fireOffsetY + 3f, base.transform);
			}
			this.sprite.SetLowerLeftPixel(new Vector2((float)((0 + this.frame % this.chaingunFiringFrameCount) * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
			this.chaingunFrameRate = 0.015f + (float)(this.chaingunFiringFrameCount * 5 - this.frame) / (float)(this.chaingunFiringFrameCount * 5) * 0.05f;
		}
		else if (this.fireIndex < this.chainGunFireLimit)
		{
			this.sprite.SetLowerLeftPixel(new Vector2((float)((4 + this.frame % this.chaingunFiringFrameCount) * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
			if (this.frame % this.chaingunFiringFrameCount == 0)
			{
				this.FireWeapon(ref this.fireIndex);
				if (this.fireIndex >= this.chainGunFireLimit)
				{
					this.windDownCount = 0;
					this.chaingunSource.clip = this.chaingunWindDown;
					this.chaingunSource.Play();
				}
			}
		}
		else if (this.windDownCount < 5 * this.chaingunFiringFrameCount)
		{
			this.sprite.SetLowerLeftPixel(new Vector2((float)((0 + this.frame % this.chaingunFiringFrameCount) * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
			this.chaingunFrameRate = 0.015f + (1f - (float)(this.chaingunFiringFrameCount * 5 - this.windDownCount) / (float)(this.chaingunFiringFrameCount * 5)) * 0.05f;
			this.windDownCount++;
		}
		else
		{
			this.sprite.SetLowerLeftPixel(new Vector2((float)(0 * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
			this.fire = false;
			this.fireDelay = 1.1f;
			this.fireIndex = 0;
			this.frame = 0;
		}
	}

	protected override void FireWeapon(ref int index)
	{
		MonoBehaviour.print("Fire Weapon");
		if (index < this.chainGunFireLimit)
		{
			if (this.propellTankBack)
			{
				this.tank.xI -= (float)(this.tank.facingDirection * 20);
			}
			this.chaingunSource.Stop();
			ProjectileController.SpawnProjectileLocally(this.projectile, this, base.transform.position.x + (float)this.tank.facingDirection * this.fireOffsetX, base.transform.position.y + this.fireOffsetY, (float)(this.tank.facingDirection * 450), -15f + UnityEngine.Random.value * 30f, -1);
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.attackSounds, 0.66f, base.transform.position);
		}
		index++;
	}

	public int chainGunFireLimit = 7;

	protected float chaingunFrameRate = 0.02f;

	public AudioClip chaingunWindUp;

	public AudioClip chaingunWindDown;

	protected AudioSource chaingunSource;

	protected int windDownCount;

	public bool propellTankBack = true;

	public float fireOffsetX = 12f;

	public float fireOffsetY;

	public int chaingunFiringFrameCount = 4;
}
