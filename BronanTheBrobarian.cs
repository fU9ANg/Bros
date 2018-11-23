// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BronanTheBrobarian : Blade
{
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

	protected override void SetGunPosition(float xOffset, float yOffset)
	{
		this.gunSprite.transform.localPosition = new Vector3(xOffset + 3f, yOffset, -1f);
	}

	protected override void RunFiring()
	{
		this.fireDelay -= this.t;
		if (this.fire)
		{
			this.rollingFrames = 0;
		}
		if (this.health <= 0)
		{
			return;
		}
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
		else
		{
			UnityEngine.Debug.Log("Special Invulnerability");
		}
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		this.readyForBlast = false;
		this.usingSpecial = false;
		base.Death(xI, yI, damage);
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
				this.PlaySpecialAttackSound(0.5f);
			}
			base.PressSpecial();
			if (this.actionState == ActionState.ClimbingLadder)
			{
				this.actionState = ActionState.Idle;
			}
			if (this.dashing)
			{
				this.yI = 280f;
				this.xIBlast += base.transform.localScale.x * 200f;
			}
			else
			{
				this.yI = 210f;
				this.xIBlast += base.transform.localScale.x * 90f;
			}
			this.readyForBlast = false;
			this.setupBlastReadiness = false;
			this.frame = 0;
			this.ChangeFrame();
			this.mashCount = 0;
			return;
		}
		HeroController.FlashSpecialAmmo(this.playerNum);
		this.ActivateGun();
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

	protected override void StartFiring()
	{
		this.holdTime = 0f;
		this.mashCount++;
		if (this.gunFrame < 4)
		{
			this.UseFire();
		}
	}

	protected override void ReleaseFire()
	{
		base.ReleaseFire();
		if (this.y < this.groundHeight + 1f)
		{
			this.yI = 95f;
			this.xIBlast += base.transform.localScale.x * 20f;
		}
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

	protected override void UseFire()
	{
		this.gunFrame = 17;
		this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
	}

	protected override void RunGun()
	{
		if (this.specialAttackDashTime > 0f)
		{
			this.gunSprite.SetLowerLeftPixel(352f, 32f);
		}
		else if (!this.usingSpecial)
		{
			this.deflectProjectilesEnergy += this.t * 0.5f;
			if (this.deflectProjectilesEnergy > 0.45f)
			{
				this.deflectProjectilesEnergy = 0.45f;
			}
			this.deflectProjectilesCounter -= this.t;
			if (!this.WallDrag && this.gunFrame > 0)
			{
				if (this.deflectProjectilesCounter > 0f)
				{
					base.DeflectProjectiles();
				}
				this.gunCounter += this.t;
				if (this.gunCounter > 0.033f)
				{
					if (this.holdTime < 0.6f)
					{
						this.gunCounter -= 0.0333f;
					}
					else if (this.holdTime < 1.6f)
					{
						this.gunCounter -= 0.0252f;
					}
					else
					{
						this.gunCounter -= 0.0121f;
					}
					this.gunFrame--;
					if (this.gunFrame >= 15)
					{
						this.gunCounter -= 0.05f;
					}
					if (this.gunFrame == 4 && this.fire)
					{
						this.holdTime += this.frameRate * 5f;
						this.gunFrame += 5;
						this.canWallClimb = false;
					}
					if (this.gunFrame >= 4 && this.gunFrame <= 15 && !this.fire)
					{
						this.FireWeapon(this.x + base.transform.localScale.x * 15f, this.y + 6.5f, base.transform.localScale.x * (float)(250 + ((!Demonstration.bulletsAreFast) ? 0 : 150)), (float)(UnityEngine.Random.Range(0, 40) - 20) * ((!Demonstration.bulletsAreFast) ? 0.2f : 1f));
						this.gunFrame = 4;
					}
					if (this.gunFrame < 0)
					{
						this.gunFrame = 0;
					}
					this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
					if (this.gunFrame == 4)
					{
						this.canWallClimb = true;
						if (this.hasHitWithSlice)
						{
							base.PlaySliceSound();
						}
						else if (this.hasHitWithWall)
						{
							base.PlayWallSound();
						}
					}
				}
			}
		}
	}

	protected override void SwingSwordGround()
	{
		if (Map.HitUnits(this, this.playerNum, 3, DamageType.Bullet, 8f, this.x, this.y, base.transform.localScale.x * 420f + base.transform.localScale.x * 400f, 360f, true, true, true, ref this.alreadyHit))
		{
			this.hasHitWithSlice = true;
		}
		else
		{
			this.hasHitWithSlice = false;
		}
	}

	protected override void SwingSwordEnemies()
	{
		this.hasHitWithSlice = false;
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.mashCount = (int)(this.holdTime * 3f);
		float num = 0.3333f + ((this.holdTime <= 0.6f) ? 0f : 0.333f) + ((this.holdTime <= 2f) ? 0f : 0.333f);
		base.FireWeapon(x, y, xSpeed, ySpeed);
		Projectile projectile = ProjectileController.SpawnProjectileLocally(this.projectile, this, x, this.y + 14f, this.xI + base.transform.localScale.x * 250f * num, 0f, this.playerNum);
		projectile.SetDamage(projectile.damage + Mathf.Clamp(this.mashCount * this.mashCount * 3, 0, 30));
		if (this.holdTime > 4f)
		{
			projectile.IncreaseLife(0.05f);
		}
		if (this.holdTime > 6f)
		{
			projectile.IncreaseLife(0.05f);
		}
		this.PlayAttackSound();
		Map.DisturbWildLife(x, y, 60f, this.playerNum);
		if (this.mashCount > 2)
		{
			this.CreateMinorGroundWave((float)(48 + Mathf.Clamp(this.mashCount - 2, 0, 11) * 8));
		}
		if (this.mashCount > 5)
		{
			SortOfFollow.Shake(0.4f);
		}
		this.mashCount = 0;
	}

	protected override void SetSpriteOffset(float xOffset, float yOffset)
	{
		base.SetSpriteOffset(xOffset, yOffset - 1f);
	}

	protected int useSpecialAttackFrame = 2;

	protected bool setupBlastReadiness;

	protected float holdTime;

	protected float stampDelay;

	protected int mashCount;

	protected float lastArnieTime;

	protected float specialAttackDirection;

	protected bool readyForBlast;
}
