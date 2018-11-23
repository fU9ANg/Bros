// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BroCeasar : TestVanDammeAnim
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

	protected override void AnimateSpecial()
	{
		this.SetSpriteOffset(0f, 0f);
		this.DeactivateGun();
		this.frameRate = 0.0334f;
		int num = 0 + Mathf.Clamp(this.frame, 0, 8);
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 8));
		if (this.frame == 0)
		{
			this.frameRate = 0.18f;
		}
		else
		{
			this.frameRate = 0.034f;
			if (this.frame == 3)
			{
				this.frameRate = 0.1f;
			}
			base.CreateFaderTrailInstance();
		}
		if (this.frame == this.useSpecialAttackFrame)
		{
			if (!this.setupBlastReadiness)
			{
				this.readyForBlast = true;
				this.setupBlastReadiness = true;
			}
			if (this.readyForBlast)
			{
				this.frame -= 2;
			}
			else
			{
				this.counter = -0.06f;
			}
		}
		if (this.frame == 8)
		{
			this.counter -= 0.15f;
		}
		if (this.frame >= 10)
		{
			this.gunFrame = 0;
			this.frame = 0;
			this.ActivateGun();
			this.usingSpecial = false;
			this.ChangeFrame();
			this.stampDelay = 0f;
		}
	}

	protected override void Land()
	{
		base.Land();
		if (this.readyForBlast)
		{
			this.readyForBlast = false;
			this.frame = this.useSpecialAttackFrame + 1;
			this.ChangeFrame();
			this.UseSpecial();
			this.stampDelay = 0.4f;
		}
	}

	protected override void Start()
	{
		base.Start();
		this.originalSpeed = this.speed;
	}

	protected override void CalculateMovement()
	{
		base.CalculateMovement();
		if (this.usingSpecial)
		{
			this.canWallClimb = false;
			this.xI *= 1f - this.t * 12f;
		}
		else
		{
			this.canWallClimb = true;
		}
	}

	protected override bool IsOverLadder(float xOffset, ref float ladderXPos)
	{
		return !this.usingSpecial && base.IsOverLadder(xOffset, ref ladderXPos);
	}

	protected override void ApplyFallingGravity()
	{
		if (this.usingSpecial && !this.readyForBlast)
		{
			float num = 1100f * this.t * 0.5f;
			this.yI -= num;
		}
		else
		{
			base.ApplyFallingGravity();
		}
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		if (!this.usingSpecial)
		{
			base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
		}
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		this.readyForBlast = false;
		this.usingSpecial = false;
		base.Death(xI, yI, damage);
		this.miniGunAudio.Stop();
	}

	protected override void RunGun()
	{
		if (this.usingSpecial && this.specialShootBoosTime > 0f)
		{
			this.specialShootBoosTime -= this.t;
			this.wasFire = true; this.fire = (this.wasFire );
			this.fireDelay = 0f;
		}
		else if (this.usingSpecial)
		{
			this.wasFire = false; this.fire = (this.wasFire );
		}
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

	protected override void PressSpecial()
	{
		if (this.usingSpecial || this.health <= 0)
		{
			return;
		}
		if (base.SpecialAmmo > 0)
		{
			MapController.DamageGround(this, 20, DamageType.Normal, 16f, this.x, this.y, null);
			EffectsController.CreateGroundWave(this.x, this.y, 2f);
			if (!this.usingSpecial)
			{
				this.lastArnieTime = Time.time;
				this.specialAttackDirection = base.transform.localScale.x;
			}
			base.PressSpecial();
			if (this.actionState == ActionState.ClimbingLadder)
			{
				this.actionState = ActionState.Idle;
			}
			this.specialShootBoosTime = 0.4f;
			this.xI = base.transform.localScale.x * this.speed;
			this.dashing = true;
			this.yI = 230f;
			this.xIBlast = base.transform.localScale.x * 140f;
			this.readyForBlast = false;
			this.setupBlastReadiness = false;
			this.frame = 0;
			this.ChangeFrame();
			return;
		}
		HeroController.FlashSpecialAmmo(this.playerNum);
		this.ActivateGun();
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
		else if (!this.special || base.SpecialAmmo > 0)
		{
		}
		if (!this.fire && !this.special)
		{
			this.pushBackForceM = 1f;
			if ((this.wasFire || this.wasSpecial) && this.miniGunAudio.isPlaying)
			{
				this.miniGunAudio.clip = this.miniGunSoundWindDown;
				this.miniGunAudio.loop = false;
				this.miniGunAudio.Play();
			}
			this.fireDelay = this.miniGunFireDelay;
		}
		this.miniGunAudio.pitch = Time.timeScale;
	}

	protected override void UseFire()
	{
		if (this.usingSpecial)
		{
			this.FireWeapon(this.x - base.transform.localScale.x * 16f, this.y + 10f, base.transform.localScale.x * 400f, (float)UnityEngine.Random.Range(-20, 20));
		}
		else
		{
			this.FireWeapon(this.x + base.transform.localScale.x * 12f, this.y + 6f, base.transform.localScale.x * 400f, (float)UnityEngine.Random.Range(-20, 20));
		}
		this.PlayAttackSound();
		Map.DisturbWildLife(this.x, this.y, 60f, this.playerNum);
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		if (this.usingSpecial)
		{
			xSpeed = -xSpeed * 0.75f;
			ySpeed = Mathf.Abs(xSpeed) * -0.75f;
			this.xIBlast += base.transform.localScale.x * 1f * this.pushBackForceM;
			this.yI += 3f * this.pushBackForceM;
		}
		this.gunFrame = 3;
		EffectsController.CreateShrapnel(this.bulletShell, x + base.transform.localScale.x * -5f, y, 1f, 30f, 1f, -base.transform.localScale.x * 40f, 70f);
		EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, base.transform);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed, ySpeed, this.playerNum);
		if (!this.usingSpecial)
		{
			this.xIBlast -= base.transform.localScale.x * 4f * this.pushBackForceM;
		}
		if (y > this.groundHeight)
		{
			this.yI += Mathf.Clamp(3f * this.pushBackForceM, 3f, 16f);
		}
	}

	protected override void UseSpecial()
	{
		ExplosionGroundWave explosionGroundWave = EffectsController.CreateHugeShockWave(this.x + base.transform.localScale.x * -12f, this.y + this.headHeight, 144f);
		FullScreenFlashEffect.FlashHot(1f, base.transform.position);
		explosionGroundWave.playerNum = this.playerNum;
		explosionGroundWave.avoidObject = this;
		explosionGroundWave.origins = this;
		base.SpecialAmmo--;
		HeroController.SetSpecialAmmo(this.playerNum, base.SpecialAmmo);
		this.PlaySpecial3Sound(0.4f);
		if (base.transform.localScale.x > 0f)
		{
			explosionGroundWave.leftWave = false;
		}
		else
		{
			explosionGroundWave.rightWave = false;
		}
		this.xI = 0f;
		this.xIBlast = 0f;
		this.yI = 50f;
	}

	protected void CreateMinorGroundWave(float range)
	{
		ExplosionGroundWave explosionGroundWave = EffectsController.CreateShockWave(this.x - base.transform.localScale.x * 6f, this.y + this.headHeight, 128f);
		explosionGroundWave.playerNum = this.playerNum;
		explosionGroundWave.avoidObject = this;
		explosionGroundWave.origins = this;
		explosionGroundWave.range = range;
		if (base.transform.localScale.x > 0f)
		{
			explosionGroundWave.leftWave = false;
		}
		else
		{
			explosionGroundWave.rightWave = false;
		}
	}

	protected override void CheckInput()
	{
		base.CheckInput();
		if (this.stampDelay > 0f)
		{
			this.fire = false;
			this.special = false;
			this.highFive = false; this.down = (this.left = (this.right = (this.highFive )));
		}
		if (this.usingSpecial && base.transform.localScale.x < 0f)
		{
			this.right = false;
			this.left = true;
		}
		else if (this.usingSpecial && base.transform.localScale.x > 0f)
		{
			this.left = false;
			this.right = true;
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
		if (this.stampDelay > 0f)
		{
			this.stampDelay -= this.t;
		}
		if (this.gunFrame < 4)
		{
			this.canWallClimb = true;
		}
	}

	protected override void AddSpeedLeft()
	{
		base.AddSpeedLeft();
		if (this.xIBlast > this.speed * 1.6f)
		{
			this.xIBlast = this.speed * 1.6f;
		}
	}

	protected override void AddSpeedRight()
	{
		base.AddSpeedRight();
		if (this.xIBlast < this.speed * -1.6f)
		{
			this.xIBlast = this.speed * -1.6f;
		}
	}

	protected override void SetGunPosition(float xOffset, float yOffset)
	{
		this.gunSprite.transform.localPosition = new Vector3(xOffset + 2f, yOffset, 0f);
	}

	protected float chargeUpTime;

	public float miniGunFireDelay = 1.25f;

	public AudioClip miniGunSoundWindup;

	public AudioClip miniGunSoundSpinning;

	public AudioClip miniGunSoundWindDown;

	protected int windFrame;

	protected float windDownTime;

	protected float windCounter;

	protected float originalSpeed = 100f;

	protected float pushBackForceM = 1f;

	public Shrapnel bulletShell;

	protected AudioSource miniGunAudio;

	public Projectile specialProjectile;

	protected int useSpecialAttackFrame = 2;

	protected bool setupBlastReadiness;

	protected float holdTime;

	protected float stampDelay;

	protected float lastArnieTime;

	protected float specialAttackDirection;

	protected bool readyForBlast;

	private float specialShootBoosTime;
}
