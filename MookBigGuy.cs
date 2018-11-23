// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookBigGuy : Mook
{
	protected override void Awake()
	{
		base.Awake();
		this.standingHeadHeight = 26f;
		this.height = 15f;
		this.headHeight = this.standingHeadHeight;
		this.zOffset = (3f + UnityEngine.Random.value) * 0.05f;
	}

	protected override void UseFire()
	{
		if (SetResolutionCamera.IsItVisible(base.transform.position))
		{
			this.FireWeapon(this.x + base.transform.localScale.x * 17f, this.y + 9f, base.transform.localScale.x * 400f, (float)(UnityEngine.Random.Range(0, 45) - 15));
			this.PlayAttackSound();
			Map.DisturbWildLife(this.x, this.y, 40f, this.playerNum);
		}
	}

	public override bool IsHeavy()
	{
		return true;
	}

	protected override void FallDamage(float yI)
	{
		this.gunSprite.SetLowerLeftPixel(0f, (float)this.gunSpritePixelHeight);
		if (yI < -350f)
		{
			if (yI < -550f)
			{
				Map.KnockAndDamageUnit(SingletonMono<MapController>.Instance, this, 55, DamageType.Bullet, -1f, 450f, 0, false);
			}
			else
			{
				Map.KnockAndDamageUnit(SingletonMono<MapController>.Instance, this, 30, DamageType.Bullet, -1f, 450f, 0, false);
			}
		}
	}

	protected override void AnimateJumping()
	{
		this.frameRate = 0.0667f;
		if (!this.ducking)
		{
			this.jumpFrame++;
			this.gunSprite.SetLowerLeftPixel((float)(4 * this.gunSpritePixelWidth), (float)this.gunSpritePixelHeight);
			this.gunSprite.transform.localPosition = new Vector3(0f, 0f, -1f);
			if (this.yI > 20f)
			{
				int num = 12;
				this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)this.spritePixelHeight);
			}
			else if (this.yI < -10f)
			{
				int num2 = 12;
				this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)this.spritePixelHeight);
			}
			else
			{
				int num3 = 12;
				this.sprite.SetLowerLeftPixel((float)(num3 * this.spritePixelWidth), (float)this.spritePixelHeight);
			}
		}
		else
		{
			this.gunSprite.transform.localPosition = new Vector3(0f, -1f, -1f);
			if (this.yI > 20f)
			{
				int num4 = 11;
				this.sprite.SetLowerLeftPixel((float)(num4 * this.spritePixelWidth), (float)this.spritePixelHeight);
			}
			else if (this.yI < -10f)
			{
				int num5 = 11;
				this.sprite.SetLowerLeftPixel((float)(num5 * this.spritePixelWidth), (float)this.spritePixelHeight);
			}
			else
			{
				int num6 = 11;
				this.sprite.SetLowerLeftPixel((float)(num6 * this.spritePixelWidth), (float)this.spritePixelHeight);
			}
		}
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		if (this.gunFrame <= 1)
		{
			this.gunFrame = 3;
		}
		this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * this.gunFrame), (float)this.gunSpritePixelHeight);
		EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, base.transform);
		EffectsController.CreateShrapnel(this.bulletShell, x + base.transform.localScale.x * -6f, y, 1f, 30f, 1f, -base.transform.localScale.x * 55f, 130f);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed, ySpeed, this.firingPlayerNum);
	}

	protected override void RunGun()
	{
		if (!this.WallDrag)
		{
			if (this.actionState != ActionState.Jumping)
			{
				if (this.gunFrame > 0)
				{
					this.gunCounter += this.t;
					if (this.gunCounter > 0.0334f)
					{
						this.gunCounter -= 0.0334f;
						this.gunFrame--;
						this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * this.gunFrame), (float)this.gunSpritePixelHeight);
					}
				}
			}
		}
	}

	protected override void AnimateRunning()
	{
		if (this.burnTime > 0f || this.blindTime > 0f)
		{
			this.gunSprite.gameObject.SetActive(false);
			this.frameRate = 0.044455f;
			int num = 13 + this.frame % 4;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)this.spritePixelHeight);
		}
		else
		{
			base.AnimateRunning();
		}
	}

	public override void AnimateActualIdleFrames()
	{
		if (!this.fire && this.showElectrifiedFrames && this.plasmaCounter > 0f)
		{
			this.frameRate = 0.033f;
			this.DeactivateGun();
			int num = 13 + this.frame % 4;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 2));
			UnityEngine.Debug.Log("Elect frame " + num);
		}
		else if (this.hurtFrames > 0)
		{
			this.frameRate = 0.033f;
			int num2 = 0 + Mathf.Clamp(4 - this.hurtFrames, 0, 4);
			this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)(this.spritePixelHeight * 2));
			if (this.hurtFrames >= 4)
			{
				this.SetGunPosition(-2f, 0f);
			}
			else if (this.hurtFrames == 3)
			{
				this.SetGunPosition(-1f, 0f);
			}
			else if (this.hurtFrames == 2)
			{
				this.SetGunPosition(-1f, 0f);
			}
			else
			{
				this.SetGunPosition(0f, 0f);
			}
		}
		else
		{
			base.AnimateActualIdleFrames();
		}
	}

	protected override void ChangeFrame()
	{
		if (this.actionState != ActionState.Idle)
		{
			this.hurtFrames = 0;
		}
		base.ChangeFrame();
		if (this.hurtFrames > 0)
		{
			this.hurtFrames--;
		}
	}

	public override void Knock(DamageType damageType, float xI, float yI, bool forceTumble)
	{
		this.xI = Mathf.Clamp(this.xI + xI / 20f, -200f, 200f);
		this.xIBlast = Mathf.Clamp(this.xIBlast + xI / 2f, -10f, 10f);
		this.yI = Mathf.Clamp(this.yI + yI / 5f, -20000f, 300f);
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
		if (this.health > 0 && Mathf.Abs(this.yI) < 12f && Mathf.Abs(this.xIBlast) < 25f)
		{
			if (damageType == DamageType.Bullet || damageType == DamageType.Explosion)
			{
				this.hurtFrames = 4;
				this.ChangeFrame();
			}
			if (this.actionState == ActionState.Idle && Mathf.Abs(this.xI) < 12f)
			{
				xI = -0.01f;
				this.CheckFacingDirection();
			}
		}
	}

	protected override void BurnOthers()
	{
	}

	protected override void PressSpecial()
	{
	}

	public Shrapnel bulletShell;

	protected int hurtFrames;

	private int jumpFrame;
}
