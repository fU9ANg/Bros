// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookDog : Mook
{
	public override void CreateGibEffects(DamageType damageType, float xI, float yI)
	{
		if (this.isMegaDog)
		{
			this.gibs = this.megaDogGibs;
		}
		base.CreateGibEffects(damageType, xI, yI);
	}

	protected override void RegisterUnit()
	{
		Map.RegisterUnit(this, false);
	}

	protected override bool CanPassThroughBarriers()
	{
		return this.health <= 0 || (this.actionState == ActionState.Jumping && Mathf.Abs(this.xIBlast) > 1f) || this.blindTime > 0f || this.scaredTime > 0f;
	}

	protected override void AnimateActualNewRunningFrames()
	{
		int num = 0 + this.frame % 8;
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 4));
	}

	protected virtual void AnimateActualUnawareRunningFrames()
	{
		int num = 0 + this.frame % 8;
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 2));
	}

	protected override void AnimateRunning()
	{
		if (this.burnTime > 0f || this.blindTime > 0f || this.scaredTime > 0f)
		{
			if (this.useNewFrames)
			{
				this.speed = this.unawareMegaRunSpeed * 0.7f;
				this.frameRate = 0.04f;
				int num = 0 + this.frame % 8;
				this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 2));
			}
			else
			{
				this.speed = this.unawareRunSpeed * 0.7f;
				this.frameRate = 0.04f;
				int num2 = 0 + this.frame % 4;
				this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)this.spritePixelHeight);
			}
			this.DeactivateGun();
		}
		else if (this.enemyAI != null && this.enemyAI.IsAlerted())
		{
			if (this.useNewFrames)
			{
				this.speed = this.awareMegaRunSpeed;
				this.DeactivateGun();
				this.frameRate = 0.025f;
				this.AnimateActualNewRunningFrames();
			}
			else
			{
				this.speed = this.awareRunSpeed;
				this.DeactivateGun();
				this.frameRate = 0.0333f;
				int num3 = 0 + this.frame % 4;
				this.sprite.SetLowerLeftPixel((float)(num3 * this.spritePixelWidth), (float)(this.spritePixelHeight * 1));
			}
		}
		else
		{
			base.AnimateRunning();
			if (this.useNewFrames)
			{
				this.frameRate = 0.033f;
				this.speed = this.unawareMegaRunSpeed;
			}
			else
			{
				this.speed = this.unawareRunSpeed;
			}
		}
	}

	public override void StartEatingCorpse()
	{
		base.StartEatingCorpse();
		this.corpseEatingCount = 0;
	}

	protected override void AnimateSpecial()
	{
		if (this.isHowling)
		{
			this.frameRate = 0.077f;
			if (this.frame < 14)
			{
				if (this.frame == 12)
				{
					base.PlayPowerUpSound(0.5f);
				}
				int num = 14 + Mathf.Clamp(this.frame, 0, 14);
				this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 2));
			}
			else
			{
				int num2 = 28 + this.frame % 2;
				this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)(this.spritePixelHeight * 2));
			}
		}
		else
		{
			this.frameRate = 0.1334f;
			int num3 = 28 + this.frame % 2;
			this.sprite.SetLowerLeftPixel((float)(num3 * this.spritePixelWidth), 32f);
			if (this.frame % 2 == 1)
			{
				this.PlaySpecialSound(0.25f);
			}
			if (this.frame % 2 == 1)
			{
				this.corpseEatingCount++;
				if (!Map.HitDeadUnits(this, (this.corpseEatingCount % 16 != 0) ? 0 : 15, DamageType.Bite, 8f, this.x, this.y, 0f, 0f, false, false))
				{
					if (this.enemyAI != null)
					{
						this.enemyAI.SetMentalState(MentalState.Idle);
					}
				}
				else if (this.corpseEatingCount % 16 == 0 && !this.isMegaDog)
				{
					Networking.RPC(PID.TargetAll, new RpcSignature(this.TransformIntoMegaDog), true);
				}
				EffectsController.CreateBloodParticles(this.bloodColor, this.x, this.y + 3f, 1 + UnityEngine.Random.Range(0, 2), 4f, 2f, 40f, 0f, 100f);
			}
		}
	}

	public void TransformIntoMegaDog()
	{
		if (!this.isMegaDog)
		{
			this.isMegaDog = true;
			this.spritePixelWidth = 48;
			this.spritePixelHeight = 48;
			this.useNewFrames = true;
			this.canDuck = false;
			this.standingHeadHeight = 19f;
			base.GetComponent<Renderer>().sharedMaterial = this.upgradedMaterial;
			this.sprite.RecalcTexture();
			this.sprite.SetPixelDimensions(48, 48);
			this.sprite.SetSize(48f, 48f);
			this.spriteOffset.y = 24f;
			this.sprite.SetOffset(new Vector3(0f, 24f, this.sprite.offset.z));
			this.SetSpriteOffset(0f, 0f);
			this.maxHealth = 15; this.health = (this.maxHealth );
			this.halfWidth = 10f;
			this.feetWidth = 7f;
			this.speed *= 1.3f;
			this.jumpForce *= 1.15f;
			Map.DisturbWildLife(this.x, this.y, 200f, -1);
			this.isHowling = true;
			this.frame = 0;
			this.sprite.SetLowerLeftPixel((float)(15 * this.spritePixelWidth), (float)(this.spritePixelHeight * 2));
		}
	}

	protected override void PressSpecial()
	{
		this.isHowling = false;
		base.PressSpecial();
	}

	protected override void UseSpecial()
	{
	}

	protected override void Jump(bool wallJump)
	{
		base.Jump(wallJump);
	}

	protected override void RunFiring()
	{
		if (this.fire)
		{
			this.invulnerable = true;
			if (Map.HitLivingUnits(this, this.firingPlayerNum, 3, DamageType.Melee, 6f, this.x, this.y + 4f, this.xI, this.yI, false, false))
			{
				this.PlayAttackSound();
			}
			this.invulnerable = false;
		}
	}

	protected override void PlayJumpSound()
	{
		base.PlaySpecial2Sound(0.94f + UnityEngine.Random.value * 0.12f);
	}

	public override void PlayGreetingSound()
	{
		base.PlayGreetingSound();
		Map.BotherNearbyMooks(this.x, this.y, 80f, 32f, 5);
	}

	public override void FetchObject(Transform fetchObject)
	{
		if (this.enemyAI != null)
		{
			this.enemyAI.FetchObject(fetchObject);
		}
	}

	public override void AnimateActualIdleFrames()
	{
		if (this.showElectrifiedFrames && this.plasmaCounter > 0f)
		{
			this.frameRate = 0.033f;
			this.DeactivateGun();
			int num = 6 + this.frame % 2;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 5));
		}
		else
		{
			base.AnimateActualIdleFrames();
		}
	}

	protected override void CalculateZombieInput()
	{
		base.CalculateZombieInput();
		if (this.fire && (!this.wasFire || this.zombieDelay <= 0f))
		{
			this.buttonJump = true;
			this.up = false;
			this.zombieDelay = 0.5f;
		}
		if ((this.fire || this.zombieDelay > 0f) && !this.right && !this.left)
		{
			if (base.transform.localScale.x > 0f)
			{
				this.right = true;
			}
			else
			{
				this.left = true;
			}
		}
	}

	public Material upgradedMaterial;

	public float awareRunSpeed = 130f;

	public float awareMegaRunSpeed = 135f;

	public float unawareRunSpeed = 120f;

	public float unawareMegaRunSpeed = 125f;

	public GibHolder megaDogGibs;

	protected bool isMegaDog;

	protected int corpseEatingCount;

	protected bool isHowling;
}
