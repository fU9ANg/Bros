// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Mookopter : Tank
{
	protected override void Start()
	{
		this.kopterTargetX = base.transform.position.x;
		this.kopterTargetY = base.transform.position.y;
		base.Start();
		this.kopterTargetX = this.x;
		this.kopterTargetY = this.y;
		this.height = 25f;
		this.width = 20f;
		if (base.GetComponent<Collider>() != null)
		{
			base.GetComponent<Collider>().enabled = false;
		}
		this.engineAudio.clip = this.engineRollingClip;
		this.engineAudio.loop = true;
		this.engineAudio.Stop();
		if (HeroController.GetPlayersPlayingCount() > 1)
		{
			this.health = (this.maxHealth = (int)((float)this.health * (1f + (float)(HeroController.GetPlayersPlayingCount() - 1) * 0.33f)));
		}
	}

	protected override void StopEngineSound()
	{
		if (this.health <= 0)
		{
			base.StopEngineSound();
		}
	}

	protected override void PlayRollingClip()
	{
	}

	protected override void PlayTurningClip()
	{
	}

	protected override void PlaySettleClip()
	{
	}

	protected override void SetToGroundHeight()
	{
	}

	protected override void Update()
	{
		base.Update();
		if (this.health <= 0)
		{
			if (!this.crashed)
			{
				this.RunSmoke();
			}
			else if (this.crashTime > 0f)
			{
				this.crashTime -= this.t;
				this.RunSmoke();
			}
		}
		else if (!Map.isEditing)
		{
			this.RotorHitUnits();
			if (!this.engineAudio.isPlaying)
			{
				this.engineAudio.Play();
			}
		}
		if (!Map.isEditing && !this.startedAudio)
		{
			base.GetComponent<AudioSource>().Play();
			this.startedAudio = true;
		}
	}

	protected override void SetSpriteTurn(int frame)
	{
		base.SetSpriteTurn(frame);
		if (frame == 0)
		{
			this.rotorMain.gameObject.SetActive(true);
			this.rotorSide.gameObject.SetActive(true);
			this.missiles.gameObject.SetActive(true);
		}
		else
		{
			this.rotorMain.gameObject.SetActive(false);
			this.rotorSide.gameObject.SetActive(false);
			this.missiles.gameObject.SetActive(false);
		}
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		base.Death(xI, yI, damage);
		this.rotorMain.gameObject.SetActive(false);
		this.rotorSide.gameObject.SetActive(false);
		this.missiles.gameObject.SetActive(false);
		this.weapon.gameObject.SetActive(false);
		if (base.GetComponent<Collider>() != null)
		{
			base.GetComponent<Collider>().enabled = true;
		}
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		if (this.health > 0)
		{
			this.damageBrotalityCount += damage;
			StatisticsController.AddBrotality(this.damageBrotalityCount / 3);
			this.damageBrotalityCount -= this.damageBrotalityCount / 3 * 3;
		}
		base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
	}

	protected override void SetDeathFrame()
	{
		this.sprite.SetLowerLeftPixel((float)(this.spritePixelWidth * 9), (float)((int)this.sprite.lowerLeftPixel.y));
	}

	protected virtual void RotorHitUnits()
	{
		this.invulnerable = true;
		if (Map.HitUnits(this, 20, DamageType.Melee, 16f, 2f, this.x, this.y + 32f, 0f, this.yI, true, false))
		{
		}
		this.invulnerable = false;
	}

	protected override void UseSpecial()
	{
		this.specialCounter += this.t;
		if (this.specialCounter > 0.4f)
		{
			this.specialCounter -= 0.4f;
			this.bombDropCount++;
			int num = this.bombDropCount % 5;
			if (num > 0 && num < 5)
			{
				Vector3 vector = Vector3.zero;
				switch (num)
				{
				case 1:
					vector = -Vector3.right * 27f;
					ProjectileController.SpawnProjectileLocally(this.bombProjectile, this, base.transform.position.x + vector.x * (float)this.facingDirection, base.transform.position.y + 3f, (float)(this.facingDirection * 60), 0f, -1);
					break;
				case 2:
					vector = Vector3.right * 25f + Vector3.forward * 8f;
					ProjectileController.SpawnProjectileLocally(this.bombProjectileBackground, this, base.transform.position.x + vector.x * (float)this.facingDirection, base.transform.position.y + 3f, (float)(this.facingDirection * 60), 0f, -1);
					break;
				case 3:
					vector = -Vector3.right * 16f;
					ProjectileController.SpawnProjectileLocally(this.bombProjectile, this, base.transform.position.x + vector.x * (float)this.facingDirection, base.transform.position.y + 3f, (float)(this.facingDirection * 60), 0f, -1);
					break;
				case 4:
					vector = Vector3.right * 14f + Vector3.forward * 8f;
					ProjectileController.SpawnProjectileLocally(this.bombProjectileBackground, this, base.transform.position.x + vector.x * (float)this.facingDirection, base.transform.position.y + 3f, (float)(this.facingDirection * 60), 0f, -1);
					break;
				default:
					UnityEngine.Debug.LogError("Logical Mistake");
					break;
				}
				Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.specialSounds, 0.7f, base.transform.position);
			}
			if (num == 4)
			{
				this.bombDropLooping = true;
				this.bombDropLoopFrame = 0;
				this.bombDropLoopFrameCounter = -0.2f;
				this.specialCounter -= 0.2f;
			}
			this.missiles.Frame = num;
		}
		if (this.bombDropLooping)
		{
			this.bombDropLoopFrameCounter += this.t;
			if (this.bombDropLoopFrameCounter > 0.0667f)
			{
				this.bombDropLoopFrameCounter -= 0.0667f;
				this.bombDropLoopFrame++;
				if (this.bombDropLoopFrame >= 5)
				{
					this.missiles.Frame = 0;
					this.bombDropLooping = false;
				}
				this.missiles.Frame = 4 + this.bombDropLoopFrame;
			}
		}
	}

	protected override void Land()
	{
		base.Land();
		if (this.health <= 0)
		{
			this.crashed = true;
		}
	}

	protected override void CheckCrushGroundWhenStanding()
	{
	}

	protected virtual void RunSmoke()
	{
		this.smokeCounter += this.t;
		if (this.smokeCounter > 0.0334f)
		{
			this.smokeCounter -= 0.0334f;
			EffectsController.CreateBlackPlumeParticle(this.x - 8f, this.y + 24f, 16f, 0f, 45f, 1.5f, 1f);
			EffectsController.CreateBlackPlumeParticle(this.x + 8f, this.y + 24f, 16f, 0f, 45f, 1.5f, 1f);
		}
	}

	protected override void RunAI()
	{
		this.wasUp = this.up;
		this.wasDown = this.down;
		this.wasLeft = this.left;
		this.wasRight = this.right;
		this.up = false;
		this.down = false;
		this.left = false;
		this.right = false;
		if ((this.weapon != null && this.weapon.health <= 0) || this.health <= 0)
		{
			return;
		}
		this.enemyAI.GetInput(ref this.left, ref this.right, ref this.up, ref this.down, ref this.jump, ref this.fire, ref this.special, ref this.special2, ref this.special3, ref this.special4);
		this.ShowInputDebug();
		if (this.fire)
		{
			this.FireWeapon();
		}
		if (this.special)
		{
			this.UseSpecial();
		}
		else
		{
			this.bombDropLooping = false;
			if (this.bombDropCount > 0)
			{
				this.bombDropCount = 0;
				this.missiles.Frame = this.bombDropCount;
			}
		}
		if (!this.up && this.wasUp)
		{
			this.kopterTargetY = this.y;
		}
		if (!this.left && this.wasLeft)
		{
			this.kopterTargetX = this.x;
		}
		if (!this.right && this.wasRight)
		{
			this.kopterTargetX = this.x;
		}
		if (!this.down && this.wasDown)
		{
			this.kopterTargetY = this.y;
		}
	}

	private void ShowInputDebug()
	{
		string str = string.Concat(new string[]
		{
			"\n",
			(!this.up) ? "   " : " U ",
			(!this.down) ? "   " : " D ",
			(!this.left) ? "   " : " L ",
			(!this.right) ? "   " : " R ",
			(!this.fire) ? "   " : " F "
		});
		LevelEditorGUI.DebugText += str;
	}

	protected override void RunMovement()
	{
		if (this.health > 0)
		{
			float num = this.kopterTargetX - this.x;
			float num2 = this.kopterTargetY - this.y;
			this.xI += num * this.t * 20f;
			this.xI *= 1f - this.t * 5f;
			this.yI += num2 * this.t * 20f;
			this.yI *= 1f - this.t * 5f;
			float num3 = this.xI * this.t;
			float num4 = this.yI * this.t;
			if (this.xI != 0f)
			{
				this.ConstrainToBlocks((int)Mathf.Sign(this.xI), ref num3);
			}
			if (this.yI != 0f && this.y + num4 < this.groundHeight + 8f)
			{
				num4 = this.groundHeight + 8f - this.y;
				this.yI = 0f;
				if (this.tankAi != null)
				{
					this.tankAi.HitGround();
				}
			}
			this.y += num4;
			this.x += num3;
			this.row = (int)((this.y + 16f) / 16f);
			this.collumn = (int)((this.x + 8f) / 16f);
			this.SetPosition(0f);
		}
	}

	protected override void RunInput()
	{
		if (this.health > 0)
		{
			if (this.up)
			{
				this.kopterTargetY += this.verticalSpeed * 0.5f * this.t;
			}
			if (this.down)
			{
				this.kopterTargetY -= this.verticalSpeed * 0.5f * this.t;
			}
			if (this.left)
			{
				if (this.special)
				{
					this.kopterTargetX -= this.tankSpeed * 0.7f * this.t;
				}
				else
				{
					this.kopterTargetX -= this.tankSpeed * 0.9f * this.t;
				}
			}
			if (this.right)
			{
				if (this.special)
				{
					this.kopterTargetX += this.tankSpeed * 0.7f * this.t;
				}
				else
				{
					this.kopterTargetX += this.tankSpeed * 0.9f * this.t;
				}
			}
		}
		base.RunInput();
	}

	protected override void SetLeftSpeed()
	{
	}

	protected override void SetRightSpeed()
	{
	}

	protected override void SetStopSpeed()
	{
	}

	protected override bool IsMoveable()
	{
		return this.actionState != ActionState.Turning;
	}

	protected override void CheckAboveGround()
	{
	}

	protected override void RunStanding()
	{
		if (this.health <= 0)
		{
			base.RunStanding();
		}
		else
		{
			this.GetGroundHeight();
		}
	}

	protected bool crashed;

	protected float crashTime = 1f;

	protected float smokeCounter;

	public float verticalSpeed = 90f;

	protected float specialCounter;

	protected float kopterTargetX;

	protected float kopterTargetY;

	protected int bombDropCount;

	protected bool bombDropLooping;

	protected int bombDropLoopFrame;

	protected float bombDropLoopFrameCounter;

	protected bool wasUp;

	protected bool wasLeft;

	protected bool wasRight;

	protected bool wasDown;

	public Projectile bombProjectile;

	public Projectile bombProjectileBackground;

	public SpriteSM rotorMain;

	public SpriteSM rotorSide;

	public SimpleSpriteWrapper missiles;

	protected bool startedAudio;

	protected int damageBrotalityCount;
}
