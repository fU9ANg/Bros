// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class BroniversalSoldier : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.originalSpeed = this.speed;
		this.isHero = true;
		this.armedGunMaterial = this.gunSprite.GetComponent<Renderer>().material;
		this.serumParticles.emit = false;
		base.Awake();
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.firingInputs.Count > 55)
		{
			this.firingInputs.RemoveAt(this.firingInputs.Count - 1);
		}
		if (this.health > 0)
		{
			this.firingInputs.Insert(0, this.fire);
		}
		else
		{
			this.firingInputs.Insert(0, false);
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.health >= 0 && this.serumTime > 0f)
		{
			this.serumTime -= this.t;
			if (this.serumTime <= 0f)
			{
				this.StopSerumFrenzy();
			}
			this.RunKicking();
		}
	}

	protected void RunKicking()
	{
		if (this.kickFrames > 1)
		{
			this.kickDamageCounter += this.t;
			if (this.kickDamageCounter > this.kickDamageFrameRate)
			{
				this.kickDamageCounter -= this.kickDamageFrameRate;
				Map.HitUnits(this, this, this.playerNum, 5, DamageType.Melee, 8f + Mathf.Abs(this.xI * this.t), 8f, this.x + this.xI * this.t, this.y + 7f, this.xI + base.transform.localScale.x * this.kickXForce, this.kickYForce, true, true, false);
				Map.HitProjectiles(this.playerNum, 10, DamageType.Bullet, 8f + Mathf.Abs(this.xI * this.t), this.x + base.transform.localScale.x * 7f + this.xI * this.t, this.y + 7f, this.xI + base.transform.localScale.x * this.kickXForce, this.kickYForce, 0.1f);
			}
			if (MapController.DamageGround(this, 15, DamageType.Crush, 7f, this.x + base.transform.localScale.x * 8f + this.xI * this.t, this.y + 8f, null))
			{
				this.kickFrames = 1;
				this.xI *= -0.1f;
			}
		}
	}

	protected void StopSerumFrenzy()
	{
		this.serumParticles.emit = false;
		this.speed = this.originalSpeed;
		this.serumFrenzy = false;
		this.serumTime = 0f;
		this.gunSprite.GetComponent<Renderer>().material = this.armedGunMaterial;
	}

	public override bool Revive(int playerNum, bool isUnderPlayerControl, TestVanDammeAnim reviveSource)
	{
		this.deathTime = -10f;
		this.overkillDamage = 0;
		this.serumTime = 0.25f;
		bool flag = base.Revive(playerNum, isUnderPlayerControl, reviveSource);
		if (flag)
		{
			this.fullyDead = false;
		}
		if (reviveSource == this)
		{
			HeroController.SetAvatarCalm(playerNum, this.usePrimaryAvatar);
			this.isZombie = false;
		}
		return flag;
	}

	protected override void PressSpecial()
	{
		if (this.health <= 0)
		{
			if (Time.time - this.deathTime < this.deathGracePeriod - 0.0333f && base.SpecialAmmo > 0)
			{
				this.UseSpecial();
			}
		}
		else
		{
			base.PressSpecial();
		}
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		if (this.health <= 0 && damageType != DamageType.SelfEsteem)
		{
			this.overkillDamage += damage;
			if (this.overkillDamage > 35)
			{
				this.serumFrenzy = false;
				this.serumParticles.emit = false;
				this.health = -9;
			}
		}
		base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
	}

	protected override void Gib(DamageType damageType, float xI, float yI)
	{
		if (!this.serumFrenzy || damageType == DamageType.OutOfBounds)
		{
			base.Gib(damageType, xI, yI);
			this.gibbed = true;
		}
	}

	protected override void UseFire()
	{
		if (this.serumFrenzy && this.serumTime <= 0.25f)
		{
			this.StopSerumFrenzy();
		}
		if (!this.serumFrenzy)
		{
			this.FireWeapon(this.x + base.transform.localScale.x * 14f, this.y + 8.5f, base.transform.localScale.x * 420f, 0f);
			this.fireDelay = this.extraFireDelay;
			this.PlayAttackSound();
			Map.DisturbWildLife(this.x, this.y, 80f, this.playerNum);
		}
		else
		{
			this.fireDelay = 0.4f;
			this.kickFrames = 8;
			this.xI = base.transform.localScale.x * 350f;
			this.yI = 110f;
			this.gravityM = 0.5f;
		}
	}

	protected override void ClampSpeedPressingLeft()
	{
		if (this.kickFrames < 2)
		{
			base.ClampSpeedPressingLeft();
		}
	}

	protected override void ClampSpeedPressingRight()
	{
		if (this.kickFrames < 2)
		{
			base.ClampSpeedPressingRight();
		}
	}

	protected override void AddSpeedLeft()
	{
		if (this.kickFrames < 2)
		{
			base.AddSpeedLeft();
		}
	}

	protected override void AddSpeedRight()
	{
		if (this.kickFrames < 2)
		{
			base.AddSpeedRight();
		}
	}

	protected override void ApplyFallingGravity()
	{
		if (this.chimneyFlip && this.chimneyFlipConstrained)
		{
			return;
		}
		float num = 1100f * this.t * this.gravityM;
		if (FluidController.IsSubmerged(this))
		{
			num *= 0.5f;
		}
		if (this.highFiveBoost)
		{
			num /= this.highFiveBoostM;
		}
		this.yI -= num;
	}

	protected void StopKicking()
	{
		this.kickFrames = 0;
		this.gravityM = 1f;
		if (this.serumFrenzy)
		{
			this.fireDelay = 0f;
		}
	}

	protected override void IncreaseFrame()
	{
		base.IncreaseFrame();
		if (this.kickFrames > 0)
		{
			this.kickFrames--;
			if (this.kickFrames <= 2)
			{
				this.gravityM = 1f;
				this.xI *= 0.7f;
			}
		}
	}

	protected override void ChangeFrame()
	{
		if (this.chimneyFlipFrames > 0)
		{
			this.StopKicking();
		}
		if (this.kickFrames > 0)
		{
			this.sprite.SetLowerLeftPixel((float)(0 * this.spritePixelWidth), (float)(this.spritePixelHeight * 8));
			this.DeactivateGun();
		}
		else
		{
			base.ChangeFrame();
		}
	}

	protected override void Land()
	{
		this.StopKicking();
		base.Land();
	}

	protected override void PlayAttackSound()
	{
		this.PlayAttackSound(0.3f);
	}

	protected override void PlayAttackSound(float v)
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.attackSounds, v, base.transform.position, UnityEngine.Random.Range(0.6f, 0.7f));
		}
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 3;
		this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
		EffectsController.CreateMuzzleFlashMediumEffect(x, y, -25f, xSpeed * 0.1f, ySpeed * 0.1f, base.transform);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed, ySpeed - 10f + UnityEngine.Random.value * 20f, this.playerNum);
	}

	protected override void UseSpecial()
	{
		if (base.SpecialAmmo > 0)
		{
			this.PlaySpecial4Sound(0.33f);
			base.SpecialAmmo--;
			if (base.IsMine)
			{
				ReviveBlast @object = Networking.Instantiate<ReviveBlast>(this.reviveBlastPrefab, new Vector3(base.transform.position.x, base.transform.position.y + 8f, -9f), Quaternion.identity, false);
				Networking.RPC<int, BroniversalSoldier>(PID.TargetAll, new RpcSignature<int, BroniversalSoldier>(@object.Setup), this.playerNum, this, false);
			}
		}
		else
		{
			HeroController.FlashSpecialAmmo(this.playerNum);
			this.ActivateGun();
		}
	}

	public override bool IsAlive()
	{
		return Time.time - this.deathTime <= this.deathGracePeriod || base.IsAlive();
	}

	protected override void CopyInput(TestVanDammeAnim zombie, ref float zombieDelay, ref bool up, ref bool down, ref bool left, ref bool right, ref bool fire, ref bool buttonJump, ref bool special, ref bool highFive)
	{
		base.CopyInput(zombie, ref zombieDelay, ref up, ref down, ref left, ref right, ref fire, ref buttonJump, ref special, ref highFive);
		if (this.health > 0)
		{
			if (this.firingInputs.Count > zombie.zombieTimerOffset)
			{
				fire = this.firingInputs[zombie.zombieTimerOffset];
			}
			else
			{
				fire = false;
			}
		}
		if (fire)
		{
			if (base.transform.localScale.x < 0f && zombie.transform.localScale.x > 0f)
			{
				left = true;
				zombieDelay = 0.25f;
			}
			if (base.transform.localScale.x > 0f && zombie.transform.localScale.x < 0f)
			{
				right = true;
				zombieDelay = 0.25f;
			}
		}
		if (zombieDelay <= 0f)
		{
			if (!this.left && !this.right && !fire)
			{
				if (up || down)
				{
					if (zombie.x < this.x - 5f)
					{
						right = true;
					}
					else if (zombie.x > this.x + 5f)
					{
						left = true;
					}
				}
				else if (zombie.x < this.x - zombie.speed * 0.2f + base.transform.localScale.x * zombie.zombieOffset)
				{
					right = true;
				}
				else if (zombie.x > this.x + zombie.speed * 0.2f + base.transform.localScale.x * zombie.zombieOffset)
				{
					left = true;
				}
			}
			if (!this.up && !this.down && !fire)
			{
				if (zombie.y > this.y - 32f && zombie.y < this.y - 2f)
				{
					up = true;
				}
				else if (zombie.y < this.y + 32f && zombie.y > this.y + 2f)
				{
					down = true;
				}
			}
		}
	}

	public Shrapnel bulletShell;

	protected float originalSpeed = 110f;

	public float serumSpeed = 130f;

	protected float serumTime;

	public float serumDuration = 2f;

	protected bool serumFrenzy;

	protected float gravityM = 1f;

	protected int kickFrames;

	protected float kickDamageFrameRate = 0.0334f;

	protected float kickDamageCounter = 0.0334f;

	public float kickXForce = 350f;

	public float kickYForce = 110f;

	protected int overkillDamage;

	protected bool fullyDead;

	protected DamageObject deathDamageObject;

	protected Material armedGunMaterial;

	protected bool gibbed;

	public ParticleEmitter serumParticles;

	private float deathGracePeriod = 0.33f;

	private List<bool> firingInputs = new List<bool>(60);

	public ReviveBlast reviveBlastPrefab;

	public float extraFireDelay = 0.24f;
}
