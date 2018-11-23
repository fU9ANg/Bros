// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookTrooper : Mook
{
	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
		this.soundHolderIndex = UnityEngine.Random.Range(0, 3);
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.decapitated && this.health > 0)
		{
			this.decapitationCounter -= this.t;
			if (this.decapitationCounter <= 0f)
			{
				this.y += 2f;
				this.Damage(this.health + 1, DamageType.Bullet, 0f, 12f, (int)Mathf.Sign(-base.transform.localScale.x), this, this.x, this.y + 8f);
			}
		}
	}

	protected override void ChangeFrame()
	{
		if (this.actionState != ActionState.Idle)
		{
			this.lookAroundFrames = -5;
		}
		base.ChangeFrame();
		if (this.hurtStumbleFrames > 0)
		{
			this.hurtStumbleFrames--;
			if (this.hurtStumbleFrames <= 0 && Mathf.Abs(this.xIBlast) < 15f)
			{
				this.xIBlast = 0f;
			}
		}
	}

	protected override void CheckFacingDirection()
	{
		if (!this.left && !this.right && this.hurtStumbleFrames > 0 && this.actionState == ActionState.Idle)
		{
			if (this.xIBlast < 0f)
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else if (this.xIBlast > 0f)
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
		}
		else
		{
			base.CheckFacingDirection();
		}
	}

	public override void AnimateActualIdleFrames()
	{
		if (this.plasmaCounter <= 0f && this.hurtStumbleFrames > 0)
		{
			this.frameRate = 0.033f;
			this.DeactivateGun();
			this.lookAroundFrames = -5;
			int num = 20 + Mathf.Clamp(7 - this.hurtStumbleFrames, 0, 9);
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 7));
		}
		else if (this.plasmaCounter <= 0f && this.enemyAI.GetThinkState() == MentalState.Suspicious)
		{
			this.ActivateGun();
			this.SetGunPosition(0f, 0f);
			this.frameRate = 0.0667f;
			this.lookAroundFrames++;
			int num2 = 0 + Mathf.Clamp(this.lookAroundFrames, 0, 9);
			if (this.lookAroundFrames > 28)
			{
				this.lookAroundFrames = 0;
			}
			this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)(this.spritePixelHeight * 8));
			if (this.gunFrame <= 0)
			{
				this.SetGunSprite(0, 0);
			}
		}
		else
		{
			base.AnimateActualIdleFrames();
		}
	}

	protected override void SetGunSprite(int spriteFrame, int spriteRow)
	{
		if ((this.enemyAI.GetThinkState() == MentalState.Alerted && this.gunAlertFrames <= 0) || this.gunFrame > 0)
		{
			this.gunAlertFrames = 0;
			base.SetGunSprite(spriteFrame, spriteRow);
		}
		else if (this.enemyAI.GetThinkState() != MentalState.Alerted)
		{
			this.gunAlertFrames = 6;
			base.SetGunSprite(spriteFrame, 2);
		}
		else if (this.gunAlertFrames > 0)
		{
			this.gunAlertFrames--;
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * (6 - this.gunAlertFrames)), (float)(this.gunSpritePixelHeight * 4));
		}
		else
		{
			base.SetGunSprite(spriteFrame, spriteRow);
		}
	}

	protected override void GetEnemyMovement()
	{
		if (Map.isEditing)
		{
			return;
		}
		if (this.enemyAI != null)
		{
			base.GetEnemyMovement();
		}
	}

	protected override void PlayAttackSound()
	{
		if (this.sound != null)
		{
			int num = this.soundHolderIndex;
			if (num != 0)
			{
				if (num != 1)
				{
					this.sound.PlaySoundEffectAt(this.thirdSoundHolder.attackSounds, 0.75f, base.transform.position);
				}
				else
				{
					this.sound.PlaySoundEffectAt(this.secondSoundHolder.attackSounds, 0.75f, base.transform.position);
				}
			}
			else
			{
				this.sound.PlaySoundEffectAt(this.soundHolder.attackSounds, 0.6f, base.transform.position);
			}
		}
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
		if (this.health > 0 && Mathf.Abs(this.yI) < 12f && Mathf.Abs(this.xIBlast) < 25f && (damageType == DamageType.Bullet || damageType == DamageType.Explosion))
		{
			this.hurtStumbleFrames = 7;
			this.xIBlast = Mathf.Sign(xI) * 30f;
			this.CheckFacingDirection();
			this.ChangeFrame();
		}
	}

	public override void HeadShot(int damage, DamageType damageType, float xI, float yI, int direction, float xHit, float yHit, MonoBehaviour damageSender)
	{
		if (this.decapitated)
		{
			this.Damage(damage, damageType, xI, yI, direction, damageSender, xHit, yHit);
		}
		else if ((damageType == DamageType.Bullet || damageType == DamageType.Melee || damageType == DamageType.Knifed) && this.health > 0 && damage * 3 >= this.health)
		{
			this.decapitated = true;
			this.health = 1;
			EffectsController.CreateBloodParticles(this.bloodColor, this.x, this.y + 16f, 40, 3f, 2f, 50f, xI * 0.5f + (float)(direction * 50), yI * 0.4f + 80f);
			EffectsController.CreateGibs(this.decapitationGib, base.GetComponent<Renderer>().sharedMaterial, this.x, this.y, 100f, 100f, xI * 0.5f, yI * 0.4f + 60f);
			this.PlayDecapitateSound();
			this.DeactivateGun();
			base.GetComponent<Renderer>().sharedMaterial = this.decapitatedMaterial;
			if (UnityEngine.Random.value > 0f)
			{
				this.Panic(UnityEngine.Random.Range(0, 2) * 2 - 1, 2.5f, false);
				this.decapitationCounter = 0.4f + UnityEngine.Random.value;
			}
			else
			{
				this.Damage(this.health, damageType, xI, yI, direction, damageSender, xHit, yHit);
			}
		}
		else
		{
			base.HeadShot(damage, damageType, xI, yI, direction, xHit, yHit, damageSender);
		}
	}

	protected override void AnimateJumping()
	{
		base.AnimateJumping();
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		if (Time.time - this.lastFireTime < 0.4f)
		{
			this.fireDelay = 0.6f;
		}
		this.lastFireTime = Time.time;
		base.FireWeapon(x, y, xSpeed, ySpeed);
	}

	protected override void PressSpecial()
	{
	}

	public SoundHolder secondSoundHolder;

	public SoundHolder thirdSoundHolder;

	protected int soundHolderIndex;

	public GibHolder decapitationGib;

	public Material decapitatedMaterial;

	protected int gunAlertFrames = 6;

	protected int lookAroundFrames = -28;

	protected int hurtStumbleFrames;

	protected float lastFireTime;
}
