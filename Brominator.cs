// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Brominator : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		if (this.miniGunAudio == null)
		{
			this.miniGunAudio = base.gameObject.AddComponent<AudioSource>();
			this.miniGunAudio.rolloffMode = AudioRolloffMode.Linear;
			this.miniGunAudio.minDistance = 200f;
			this.miniGunAudio.dopplerLevel = 0.1f;
			this.miniGunAudio.maxDistance = 500f;
			this.miniGunAudio.volume = 0.33f;
		}
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
		this.originalSpeed = this.speed;
	}

	protected override void RunGun()
	{
		if (!this.WallDrag)
		{
			if (this.fire)
			{
				this.windDownTime = 1.5f;
				if (this.fireDelay > 0f)
				{
					this.windCounter += this.t;
					if (this.windCounter >= 0.0334f)
					{
						this.windCounter -= 0.0334f;
						this.windFrame++;
						this.SetGunSprite(7 + this.windFrame % 2, 0);
					}
				}
				else
				{
					this.windCounter += this.t;
					if (this.windCounter >= 0.0334f)
					{
						this.windCounter -= 0.0334f;
						this.windFrame++;
						this.SetGunSprite(3 + this.windFrame % 4, 0);
					}
				}
			}
			else if (this.gunFrame > 0)
			{
				this.gunCounter += this.t;
				if (this.gunCounter > 0.0334f)
				{
					this.gunCounter -= 0.0334f;
					this.gunFrame--;
					this.SetGunSprite(this.gunFrame, 0);
				}
			}
			else if (this.windDownTime > 0f)
			{
				this.windDownTime -= this.t;
				this.windCounter += this.t * this.windDownTime;
				if (this.windCounter >= 0.0667f)
				{
					this.windCounter -= 0.0667f;
					this.windFrame++;
					this.SetGunSprite(7 + this.windFrame % 2, 0);
				}
			}
		}
	}

	protected override void RunFiring()
	{
		if (this.fire)
		{
			if (!this.wasFire)
			{
				this.miniGunAudio.clip = this.miniGunSoundWindup;
				this.miniGunAudio.loop = false;
				this.miniGunAudio.Play();
			}
			if (this.fireDelay > 0f)
			{
				this.fireDelay -= this.t;
				if (this.fireDelay <= 0f)
				{
					this.miniGunAudio.clip = this.miniGunSoundSpinning;
					this.miniGunAudio.loop = true;
					this.miniGunAudio.Play();
				}
			}
			else
			{
				this.fireCounter += this.t;
				if (this.fireCounter >= this.fireRate)
				{
					this.fireCounter -= this.fireRate;
					this.UseFire();
					base.FireFlashAvatar();
				}
				this.pushBackForceM = Mathf.Clamp(this.pushBackForceM + this.t * 6f, 1f, 12f);
			}
		}
		if (!this.fire)
		{
			this.pushBackForceM = 1f;
			if (this.wasFire)
			{
				this.miniGunAudio.clip = this.miniGunSoundWindDown;
				this.miniGunAudio.loop = false;
				this.miniGunAudio.Play();
			}
			if (!this.brominatorMode)
			{
				this.fireDelay = this.miniGunFireDelay;
			}
			else
			{
				this.fireDelay = this.miniGunFireDelay / 3f;
			}
		}
		this.miniGunAudio.pitch = Time.timeScale;
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 3;
		EffectsController.CreateShrapnel(this.bulletShell, x + base.transform.localScale.x * -5f, y, 1f, 30f, 1f, -base.transform.localScale.x * 40f, 70f);
		EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, base.transform);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed, ySpeed, this.playerNum);
		if (!this.brominatorMode)
		{
			this.xIBlast -= base.transform.localScale.x * 4f * this.pushBackForceM;
			if (y > this.groundHeight)
			{
				this.yI += Mathf.Clamp(3f * this.pushBackForceM, 3f, 16f);
			}
		}
		else
		{
			this.xIBlast -= base.transform.localScale.x * 5f * this.pushBackForceM;
			if (y > this.groundHeight)
			{
				this.yI = this.yI;
			}
		}
	}

	protected override void UseSpecial()
	{
		if (!this.brominatorMode)
		{
			if (base.SpecialAmmo > 0)
			{
				base.SpecialAmmo--;
				HeroController.SetSpecialAmmo(this.playerNum, base.SpecialAmmo);
				this.brominatorMode = true;
				if (!GameModeController.IsDeathMatchMode)
				{
					this.brominatorTime = 5.5f;
				}
				else
				{
					this.brominatorTime = 4.5f;
				}
				base.GetComponent<Renderer>().material = this.metalBrominator;
				HeroController.SetAvatarMaterial(this.playerNum, this.brominatorRobotAvatar);
				this.gunSprite.GetComponent<Renderer>().material = this.metalGunBrominator;
				this.speed = this.originalSpeed * 0.7f;
			}
			else
			{
				HeroController.FlashSpecialAmmo(this.playerNum);
			}
		}
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		if (!this.brominatorMode)
		{
			base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
		}
		else
		{
			this.xIBlast += xI * 0.1f + (float)damage * 0.03f;
			this.yI += yI * 0.1f + (float)damage * 0.03f;
		}
	}

	protected override void SetGunSprite(int spriteFrame, int spriteRow)
	{
		if (this.actionState == ActionState.ClimbingLadder && this.hangingOneArmed)
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * (11 + spriteFrame)), (float)(this.gunSpritePixelHeight * (1 + spriteRow)));
		}
		else if (this.attachedToZipline != null && this.actionState == ActionState.Jumping)
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * (11 + spriteFrame)), (float)(this.gunSpritePixelHeight * (1 + spriteRow)));
		}
		else
		{
			base.SetGunSprite(spriteFrame, spriteRow);
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.brominatorMode)
		{
			if (this.brominatorTime > 0f)
			{
				this.brominatorTime -= this.t;
			}
			else
			{
				this.brominatorMode = false;
				base.GetComponent<Renderer>().material = this.humanBrominator;
				this.gunSprite.GetComponent<Renderer>().material = this.humanGunBrominator;
				HeroController.SetAvatarMaterial(this.playerNum, this.brominatorHumanAvatar);
				if (!GameModeController.IsDeathMatchMode)
				{
					base.SetInvulnerable(0.5f, true);
				}
				this.speed = this.originalSpeed;
			}
		}
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		base.Death(xI, yI, damage);
		this.miniGunAudio.Stop();
	}

	protected override void AddSpeedLeft()
	{
		base.AddSpeedLeft();
		if (this.brominatorMode)
		{
			if (this.xIBlast > this.speed * 0.12f)
			{
				this.xIBlast = this.speed * 0.12f;
			}
		}
		else if (this.xIBlast > this.speed * 1.6f)
		{
			this.xIBlast = this.speed * 1.6f;
		}
	}

	protected override void AddSpeedRight()
	{
		base.AddSpeedRight();
		if (this.brominatorMode)
		{
			if (this.xIBlast < this.speed * -0.12f)
			{
				this.xIBlast = this.speed * -0.12f;
			}
		}
		else if (this.xIBlast < this.speed * -1.6f)
		{
			this.xIBlast = this.speed * -1.6f;
		}
	}

	protected float chargeUpTime;

	public float miniGunFireDelay = 1.25f;

	public AudioClip miniGunSoundWindup;

	public AudioClip miniGunSoundSpinning;

	public AudioClip miniGunSoundWindDown;

	public Material metalBrominator;

	public Material humanBrominator;

	public Material metalGunBrominator;

	public Material humanGunBrominator;

	protected bool brominatorMode;

	protected float brominatorTime;

	protected int windFrame;

	protected float windDownTime;

	protected float windCounter;

	protected float originalSpeed = 100f;

	protected float pushBackForceM = 1f;

	public Material brominatorHumanAvatar;

	public Material brominatorRobotAvatar;

	public Shrapnel bulletShell;

	protected AudioSource miniGunAudio;
}
