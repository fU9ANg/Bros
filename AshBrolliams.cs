// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AshBrolliams : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
		if (this.chainsawAudio == null)
		{
			this.chainsawAudio = base.gameObject.AddComponent<AudioSource>();
			this.chainsawAudio.rolloffMode = AudioRolloffMode.Linear;
			this.chainsawAudio.dopplerLevel = 0.1f;
			this.chainsawAudio.minDistance = 500f;
			this.chainsawAudio.volume = 0.4f;
		}
		this.normalSpeed = this.speed;
	}

	private void StopChainsawAudio()
	{
		if (this.chainsawAudio.isPlaying && this.chainsawAudio.clip == this.chainsawSpin)
		{
			this.chainsawAudio.loop = false;
			this.chainsawAudio.clip = this.chainsawWindDown;
			this.chainsawAudio.Play();
		}
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		base.Death(xI, yI, damage);
		this.rampageTime -= 100f;
		this.StopChainsawAudio();
	}

	protected override void Update()
	{
		base.Update();
		if (this.onRampage)
		{
			if (this.chainsawAudio.clip == this.chainsawStart && !this.chainsawAudio.isPlaying)
			{
				this.chainsawAudio.loop = true;
				this.chainsawAudio.clip = this.chainsawSpin;
				this.chainsawAudio.Play();
			}
			if ((this.rampageTime -= this.t) < 0f || this.isOnHelicopter)
			{
				this.onRampage = false;
				this.speed = this.normalSpeed;
				base.SetInvulnerable(0.5f, true);
				this.StopChainsawAudio();
				HeroController.SetAvatarCalm(this.playerNum, true);
			}
			else if ((this.rampageDamageDelay -= this.t) < 0f)
			{
				this.rampageDamageDelay += 0.03334f;
				if (Map.HitUnits(this, this, this.playerNum, 1, DamageType.Chainsaw, 16f, this.x + base.transform.localScale.x * this.width / 2f, this.y + this.height / 2f, base.transform.localScale.x * 70f, 70f, false, true))
				{
					Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.effortSounds, 0.5f, base.transform.position);
					EffectsController.CreateBloodParticles(this.bloodColor, this.x + base.transform.localScale.x * this.width * 0.25f, this.y + this.height / 2f, 5, 4f, 4f, 60f, base.transform.localScale.x * this.speed, 350f);
					this.chainsawAudio.pitch = Mathf.Clamp(this.chainsawAudio.pitch + 0.03f, 0.85f, 1.25f);
					if (!this.haveSwitchedMaterial && this.chainsawHits++ > 15)
					{
						this.haveSwitchedMaterial = true;
						HeroController.players[this.playerNum].hud.SetAvatar(this.bloodyAvatar);
					}
					this.hitChainsawLastFrame = true;
				}
				else
				{
					this.chainsawAudio.pitch = Mathf.Lerp(this.chainsawAudio.pitch, 0.85f, 0.1667f);
					this.hitChainsawLastFrame = false;
				}
				MapController.DamageGround(this, 3, DamageType.Normal, 4f, this.x + base.transform.localScale.x * this.width / 2f, this.y + this.height / 2f, null);
				this.DeflectProjectiles();
				Map.PanicUnits(this.x, this.y, 64f, 16f, (int)base.transform.localScale.x, 0.5f, false);
			}
		}
	}

	protected override void RunFiring()
	{
		if (this.onRampage)
		{
			this.fire = false;
		}
		if (this.fire)
		{
			this.rollingFrames = 0;
		}
		if ((this.isFiring || this.fire) && this.fireDelay <= 0f)
		{
			this.fireCounter += this.t;
			if (this.fireCounter >= this.fireRate)
			{
				this.fireCounter -= this.fireRate;
				this.UseFire();
				base.FireFlashAvatar();
				this.fireCount++;
				if (this.fireCount % 2 == 0)
				{
					this.fireDelay = 0.5f;
					this.isFiring = false;
				}
				else
				{
					this.isFiring = true;
				}
			}
		}
		else if (this.fireDelay > 0.15f)
		{
			this.fireDelay -= this.t;
			if (this.fireDelay <= 0.15f)
			{
				EffectsController.CreateShrapnel(this.bulletShell, this.x + 5f * base.transform.localScale.x, this.y + 8f, 1f, 30f, 1f, -base.transform.localScale.x * 80f, 170f);
				EffectsController.CreateShrapnel(this.bulletShell, this.x + 5f * base.transform.localScale.x, this.y + 8f, 1f, 30f, 1f, -base.transform.localScale.x * 80f, 170f);
				this.sound.PlaySoundEffectAt(this.soundHolder.special2Sounds, 0.55f, base.transform.position);
			}
		}
		else
		{
			this.fireDelay -= this.t;
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
			this.sound.PlaySoundEffectAt(this.soundHolder.attackSounds, 0.5f, base.transform.position);
		}
	}

	protected override void UseFire()
	{
		this.FireWeapon(this.x + base.transform.localScale.x * 14f, this.y + 8.5f, base.transform.localScale.x * 300f, 0f);
		this.PlayAttackSound();
		Map.DisturbWildLife(this.x, this.y, 80f, this.playerNum);
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 3;
		this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
		EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, base.transform);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed * 0.94f, ySpeed + 50f + UnityEngine.Random.value * 5f, this.playerNum);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed * 0.96f, ySpeed + 25f + UnityEngine.Random.value * 10f, this.playerNum);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed * 0.98f, ySpeed - 3f + UnityEngine.Random.value * 6f, this.playerNum);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed * 0.96f, ySpeed - 25f - UnityEngine.Random.value * 10f, this.playerNum);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed * 0.94f, ySpeed - 50f + UnityEngine.Random.value * 5f, this.playerNum);
	}

	protected override void UseSpecial()
	{
		if (!this.onRampage && base.SpecialAmmo > 0)
		{
			base.SpecialAmmo--;
			HeroController.SetSpecialAmmo(this.playerNum, base.SpecialAmmo);
			this.rampageTime = 5.5f;
			this.onRampage = true;
			this.chainsawAudio.clip = this.chainsawStart;
			this.chainsawAudio.loop = false;
			this.chainsawAudio.Play();
			HeroController.SetAvatarAngry(this.playerNum, this.usePrimaryAvatar);
		}
	}

	protected override void ChangeFrame()
	{
		base.ChangeFrame();
		if (this.onRampage && this.health > 0)
		{
			this.gunSprite.gameObject.SetActive(true);
			this.rampageFrameDelay -= this.t;
			if (this.rampageFrameDelay < 0f)
			{
				this.rampageFrameDelay = 0.02f;
				this.rampageFrame = (this.rampageFrame + 1) % 4;
			}
			this.gunSprite.SetLowerLeftPixel((float)(32 * (11 + this.rampageFrame + ((!this.hitChainsawLastFrame) ? 0 : 3))), 32f);
		}
	}

	protected override void CalculateMovement()
	{
		if (this.onRampage)
		{
			if (base.transform.localScale.x < 0f)
			{
				if (!this.right && this.left)
				{
					this.speed = this.normalSpeed * 1.6f;
				}
				else if (!this.right)
				{
					this.speed = this.normalSpeed * 1.3f;
					this.left = true;
				}
			}
			else if (!this.left && this.right)
			{
				this.speed = this.normalSpeed * 1.6f;
			}
			else if (!this.left)
			{
				this.speed = this.normalSpeed * 1.3f;
				this.right = true;
			}
		}
		base.CalculateMovement();
	}

	protected void DeflectProjectiles()
	{
		if (Map.DeflectProjectiles(this, this.playerNum, 20f, this.x + Mathf.Sign(base.transform.localScale.x) * 6f, this.y + 6f, Mathf.Sign(base.transform.localScale.x) * 200f))
		{
		}
	}

	public Shrapnel bulletShell;

	protected int fireCount;

	protected bool isFiring;

	public AudioClip chainsawStart;

	public AudioClip chainsawSpin;

	public AudioClip chainsawWindDown;

	private int chainsawHits;

	private bool haveSwitchedMaterial;

	public Material bloodyAvatar;

	protected float normalSpeed;

	protected float rampageTime;

	protected float rampageDamageDelay;

	protected bool onRampage;

	protected AudioSource chainsawAudio;

	private float rampageFrameDelay;

	private int rampageFrame;

	private bool hitChainsawLastFrame;
}
