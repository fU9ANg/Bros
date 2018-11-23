// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class Blade : TestVanDammeAnim
{
	protected override void UseFire()
	{
		this.alreadyHit.Clear();
		this.gunFrame = 6;
		this.hasPlayedAttackHitSound = false;
		this.FireWeapon(this.x + base.transform.localScale.x * 10f, this.y + 6.5f, base.transform.localScale.x * (float)(250 + ((!Demonstration.bulletsAreFast) ? 0 : 150)), (float)(UnityEngine.Random.Range(0, 40) - 20) * ((!Demonstration.bulletsAreFast) ? 0.2f : 1f));
		this.PlayAttackSound();
		Map.DisturbWildLife(this.x, this.y, 60f, this.playerNum);
	}

	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		Map.HurtWildLife(x + base.transform.localScale.x * 13f, y + 5f, 12f);
		this.deflectProjectilesCounter = this.deflectProjectilesEnergy;
		this.deflectProjectilesEnergy = 0f;
		this.DeflectProjectiles();
		this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
		float num = base.transform.localScale.x * 12f;
		this.ConstrainToFragileBarriers(ref num, 16f);
		if (Physics.Raycast(new Vector3(x - Mathf.Sign(base.transform.localScale.x) * 12f, y + 5.5f, 0f), new Vector3(base.transform.localScale.x, 0f, 0f), out this.raycastHit, 19f, this.groundLayer) || Physics.Raycast(new Vector3(x - Mathf.Sign(base.transform.localScale.x) * 12f, y + 10.5f, 0f), new Vector3(base.transform.localScale.x, 0f, 0f), out this.raycastHit, 19f, this.groundLayer))
		{
			this.MakeEffects(this.raycastHit.point.x, this.raycastHit.point.y);
			MapController.Damage_Local(this, this.raycastHit.collider.gameObject, this.groundSwordDamage, DamageType.Bullet, this.xI, 0f);
			this.hasHitWithWall = true;
			this.SwingSwordGround();
		}
		else
		{
			this.hasHitWithWall = false;
			this.SwingSwordEnemies();
		}
	}

	protected virtual void SwingSwordGround()
	{
		if (Map.HitUnits(this, this.playerNum, this.enemySwordDamage, DamageType.Bullet, 5f, this.x, this.y, base.transform.localScale.x * 420f, 360f, true, true, true, ref this.alreadyHit))
		{
			this.hasHitWithSlice = true;
		}
		else
		{
			this.hasHitWithSlice = false;
		}
	}

	protected virtual void SwingSwordEnemies()
	{
		if (Map.HitUnits(this, this.playerNum, this.enemySwordDamage, DamageType.Bullet, 13f, this.x, this.y, base.transform.localScale.x * 420f, 360f, true, true, true, ref this.alreadyHit))
		{
			this.hasHitWithSlice = true;
		}
		else
		{
			this.hasHitWithSlice = false;
		}
	}

	protected override void RunFiring()
	{
		if (this.specialAttackDashTime > 0f)
		{
			this.hitClearCounter += this.t;
			if (this.hitClearCounter >= 0.03f)
			{
				this.hitClearCounter -= 0.03f;
				this.alreadyHit.Clear();
			}
		}
		base.RunFiring();
	}

	protected override void RunMovement()
	{
		if (this.specialAttackDashTime > 0f)
		{
			this.invulnerable = true;
			this.invulnerableTime = 0.3f;
			this.specialAttackDashTime -= this.t;
			this.xI = 240f * base.transform.localScale.x;
			this.yI = 0f;
			this.y = this.specialAttackDashHeight;
			if (Map.DeflectProjectiles(this, this.playerNum, 16f, this.x + Mathf.Sign(base.transform.localScale.x) * 6f, this.y + 6f, Mathf.Sign(base.transform.localScale.x) * 200f))
			{
			}
			this.specialAttackDashCounter += this.t;
			if (this.specialAttackDashCounter > 0f)
			{
				this.specialAttackDashCounter -= 0.0333f;
				Map.HitUnits(this, this, this.playerNum, 5, DamageType.Melee, 10f, this.x, this.y, base.transform.localScale.x * 420f, 360f, true, true);
				if (base.IsMine)
				{
					MapController.DamageGround(this, 3, DamageType.Melee, 24f, this.x + base.transform.localScale.x * 16f, this.y + 7f, null);
				}
				base.CreateFaderTrailInstance();
			}
			if (this.specialAttackDashTime <= 0f)
			{
				this.gunSprite.SetLowerLeftPixel(0f, 32f);
				this.actionState = ActionState.Jumping;
				this.ChangeFrame();
			}
			Map.HurtWildLife(this.x + base.transform.localScale.x * 13f, this.y + 5f, 16f);
			base.RunMovement();
		}
		else
		{
			base.RunMovement();
		}
	}

	protected override void ChangeFrame()
	{
		if (this.specialAttackDashTime > 0f)
		{
			this.gunSprite.gameObject.SetActive(true);
			this.actionState = ActionState.Jumping;
			int num = 23 + this.frame % 2;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), 32f);
		}
		else
		{
			base.ChangeFrame();
		}
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		if ((damageType == DamageType.Melee || damageType == DamageType.Knifed) && (this.fire || this.gunFrame > 0) && Mathf.Sign(base.transform.localScale.x) != Mathf.Sign(xI))
		{
			UnityEngine.Debug.Log("Don't Give a shit");
		}
		else
		{
			base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
		}
	}

	protected override void AnimateSpecial()
	{
		if (this.frame >= 0)
		{
			this.UseSpecial();
		}
	}

	protected override void UseSpecial()
	{
		if (base.SpecialAmmo > 0 && Time.time - this.specialAttackStartTime > 0.55f)
		{
			if (this.attachedToZipline != null)
			{
				this.attachedToZipline.DetachUnit(this);
			}
			this.PlaySpecialAttackSound(0.7f);
			base.SpecialAmmo--;
			HeroController.SetSpecialAmmo(this.playerNum, base.SpecialAmmo);
			this.usingSpecial = false;
			this.specialAttackDashTime = 0.5f;
			this.specialAttackStartTime = Time.time;
			this.specialAttackDashHeight = this.y;
			this.ChangeFrame();
		}
		else
		{
			HeroController.FlashSpecialAmmo(this.playerNum);
			this.gunSprite.gameObject.SetActive(true);
			this.usingSpecial = false;
		}
	}

	protected virtual void MakeEffects(float x, float y)
	{
		EffectsController.CreateShrapnel(this.shrapnelSpark, this.raycastHit.point.x + this.raycastHit.normal.x * 3f, this.raycastHit.point.y + this.raycastHit.normal.y * 3f, 4f, 30f, 3f, this.raycastHit.normal.x * 60f, this.raycastHit.normal.y * 30f);
		EffectsController.CreateEffect(this.hitPuff, this.raycastHit.point.x + this.raycastHit.normal.x * 3f, this.raycastHit.point.y + this.raycastHit.normal.y * 3f);
	}

	public void PlaySliceSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.special2Sounds, this.sliceVolume, base.transform.position);
		}
	}

	public void PlayWallSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.defendSounds, this.wallHitVolume, base.transform.position);
		}
	}

	protected override void PlayAttackSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.attackSounds, 0.6f, base.transform.position);
		}
	}

	protected void DeflectProjectiles()
	{
		if (Map.DeflectProjectiles(this, this.playerNum, 16f, this.x + Mathf.Sign(base.transform.localScale.x) * 6f, this.y + 6f, Mathf.Sign(base.transform.localScale.x) * 200f))
		{
			this.hasHitWithWall = true;
		}
	}

	protected override void RunGun()
	{
		if (this.specialAttackDashTime > 0f)
		{
			this.gunSprite.SetLowerLeftPixel(352f, 32f);
		}
		else
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
					this.DeflectProjectiles();
				}
				this.gunCounter += this.t;
				if (this.gunCounter > 0.0334f)
				{
					this.gunCounter -= 0.0334f;
					this.gunFrame--;
					if (this.gunFrame < 0)
					{
						this.gunFrame = 0;
					}
					this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
					if (!this.hasPlayedAttackHitSound)
					{
						if (this.hasHitWithSlice)
						{
							this.PlaySliceSound();
							this.hasPlayedAttackHitSound = true;
						}
						else if (this.hasHitWithWall)
						{
							this.PlayWallSound();
							this.hasPlayedAttackHitSound = true;
						}
					}
					if (this.gunFrame >= 3)
					{
						if (this.hasHitWithWall)
						{
							this.SwingSwordGround();
						}
						else
						{
							this.SwingSwordEnemies();
						}
					}
				}
			}
		}
	}

	protected override void SetGunPosition(float xOffset, float yOffset)
	{
		this.gunSprite.transform.localPosition = new Vector3(xOffset + 4f, yOffset, -1f);
	}

	protected override void AnimateZipline()
	{
		base.AnimateZipline();
		this.SetGunSprite(4, 1);
	}

	public Shrapnel shrapnelSpark;

	public FlickerFader hitPuff;

	protected List<Unit> alreadyHit = new List<Unit>();

	protected float hitClearCounter;

	protected bool hasHitWithSlice;

	protected bool hasHitWithWall;

	protected bool hasPlayedAttackHitSound;

	public int groundSwordDamage = 1;

	public int enemySwordDamage = 5;

	protected float deflectProjectilesCounter;

	protected float deflectProjectilesEnergy;

	protected bool specialAttackDashing;

	protected float specialAttackDashTime;

	protected float specialAttackDashCounter;

	protected float specialAttackDashHeight;

	private float specialAttackStartTime;

	public float sliceVolume = 0.7f;

	public float wallHitVolume = 0.6f;
}
