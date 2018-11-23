// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class HelicopterBossChaingun : TankWeapon
{
	protected override void Start()
	{
		base.Start();
		this.chaingunSource = base.gameObject.AddComponent<AudioSource>();
		this.chaingunSource.volume = 0.45f;
		this.chaingunSource.pitch = 0.6f;
		this.chaingunSource.loop = false;
		this.chaingunSource.rolloffMode = AudioRolloffMode.Linear;
		this.chaingunSource.minDistance = 150f;
		this.chaingunSource.maxDistance = 420f;
		this.chaingunSource.loop = false;
		this.chaingunSource.dopplerLevel = 0.1f;
		this.chaingunSource.clip = this.chaingunWindUp;
		this.chaingunSource.playOnAwake = false;
		this.chaingunSource.Stop();
		this.spriteWidth = this.sprite.width;
		this.spriteHeight = this.sprite.height;
		this.invulnerable = true;
	}

	protected override void Update()
	{
		if (this.playingIntroAnim)
		{
			this.animCounter += Time.deltaTime;
			if (this.animCounter >= this.introAnimSpeed)
			{
				if (!this.windUp)
				{
					this.introFrame++;
					if (this.introFrame < this.introFrames)
					{
						this.animCounter -= this.introAnimSpeed;
						this.sprite.SetLowerLeftPixel((float)((int)(this.sprite.lowerLeftPixel.x + this.spriteWidth)), (float)((int)this.sprite.lowerLeftPixel.y));
					}
					else
					{
						this.windUp = true;
						this.introAnimSpeed = 0.21f;
						if (HelicopterBossChaingun.introPlayedCount > 1)
						{
							this.introAnimSpeed = 0.1f;
						}
						this.animCounter = 0f;
						this.introFrame = 0;
						Sound.GetInstance().PlayAudioClip(this.windupClip, base.transform.position, 0.3f);
					}
				}
				else
				{
					this.introFrame++;
					if (this.introFrame < 4)
					{
						this.animCounter = 0f;
						this.sprite.SetLowerLeftPixel((float)((int)((float)this.introFrame * this.spriteWidth)), (float)((int)(this.spriteHeight * 2f)));
					}
					else
					{
						this.introFrame = 0;
						this.animCounter = 0f;
						if (HelicopterBossChaingun.introPlayedCount < 2)
						{
							this.introAnimSpeed *= 0.75f;
						}
						else
						{
							this.playingIntroAnim = false;
						}
						if (this.introAnimSpeed < 0.0333334f)
						{
							this.playingIntroAnim = false;
							HelicopterBossChaingun.introPlayedCount++;
						}
					}
				}
			}
		}
		else
		{
			base.Update();
		}
		LevelEditorGUI.DebugText = this.playingIntroAnim.ToString();
	}

	public override void SetSpriteTurn(int frame)
	{
		if (this.health > 0)
		{
			this.fire = false;
			this.currentTurnFrame = frame;
			this.sprite.SetLowerLeftPixel(new Vector2((float)((8 + frame) * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
		}
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		base.Death(xI, yI, damage);
		this.chaingunSource.Stop();
	}

	protected override void SetDeathFrame()
	{
		this.sprite.SetLowerLeftPixel(new Vector2((float)(13 * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
	}

	protected override void RunFiring()
	{
		this.fireDelay -= this.t;
		if (this.fire && this.health > 0 && this.tank.CanFire())
		{
			if (!this.wasFire)
			{
				this.chaingunSource.clip = this.chaingunWindUp;
				this.chaingunSource.Play();
			}
			if (this.fireDelay <= 0f)
			{
				this.fireCounter += this.t;
				int num = (int)(this.fireCounter / this.chaingunFrameRate);
				for (int i = 0; i < num; i++)
				{
					this.fireCounter -= this.chaingunFrameRate;
					this.ChangeChaingunFrame();
				}
			}
		}
		this.wasFire = this.fire;
	}

	protected void ChangeChaingunFrame()
	{
		this.frame++;
		if (this.frame < this.chaingunFiringFrameCount * 5)
		{
			this.sprite.SetLowerLeftPixel(new Vector2((float)((0 + this.frame % this.chaingunFiringFrameCount) * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
			this.chaingunFrameRate = 0.015f + (float)(this.chaingunFiringFrameCount * 5 - this.frame) / (float)(this.chaingunFiringFrameCount * 5) * 0.05f;
		}
		else if (this.fireIndex < this.chainGunFireLimit)
		{
			this.sprite.SetLowerLeftPixel(new Vector2((float)((4 + this.frame % this.chaingunFiringFrameCount) * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
			if (this.frame % this.chaingunFiringFrameCount == 0)
			{
				this.FireWeapon(ref this.fireIndex);
				if (this.fireIndex >= this.chainGunFireLimit)
				{
					this.windDownCount = 0;
					this.chaingunSource.clip = this.chaingunWindDown;
					this.chaingunSource.loop = false;
					this.chaingunSource.Play();
				}
			}
		}
		else if (this.windDownCount < 5 * this.chaingunFiringFrameCount)
		{
			this.sprite.SetLowerLeftPixel(new Vector2((float)((0 + this.frame % this.chaingunFiringFrameCount) * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
			this.chaingunFrameRate = 0.015f + (1f - (float)(this.chaingunFiringFrameCount * 5 - this.windDownCount) / (float)(this.chaingunFiringFrameCount * 5)) * 0.05f;
			this.windDownCount++;
		}
		else
		{
			this.sprite.SetLowerLeftPixel(new Vector2((float)(0 * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
			this.fire = false;
			this.fireDelay = 1.1f;
			this.fireIndex = 0;
			this.frame = 0;
		}
	}

	protected override void FireWeapon(ref int index)
	{
		if (index < this.chainGunFireLimit)
		{
			if (this.propellTankBack)
			{
				this.tank.xI -= (float)(this.tank.facingDirection * 20);
			}
			if (this.chaingunSource.clip != this.chaingunHum)
			{
				this.chaingunSource.Stop();
				this.chaingunSource.clip = this.chaingunHum;
				this.chaingunSource.loop = true;
				this.chaingunSource.volume = 0.23f;
				this.chaingunSource.Play();
			}
			ProjectileController.SpawnProjectileLocally(this.projectile, this, base.transform.position.x + ((float)this.tank.facingDirection * this.fireOffsetX - 3f), base.transform.position.y + this.fireOffsetY + 3f, (float)(this.tank.facingDirection * 450), -15f + UnityEngine.Random.value * 30f, -15);
			EffectsController.CreateShrapnel(this.bulletShell, this.x + base.transform.localScale.x * -15f, this.y + 3f, 1f, 30f, 1f, -base.transform.localScale.x * 20f, 0f);
			EffectsController.CreateMuzzleFlashBigEffect(base.transform.position.x + ((float)this.tank.facingDirection * this.fireOffsetX - 3f), base.transform.position.y + this.fireOffsetY + 3f, 0f, 150f, 0f);
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.attackSounds, 0.46f, base.transform.position);
		}
		index++;
	}

	internal void StopShooting()
	{
		this.fireIndex = this.chainGunFireLimit - 1;
	}

	public void StartIntroAnim()
	{
		this.playingIntroAnim = true;
		Sound.GetInstance().PlayAudioClip(this.introClip, base.transform.position, 0.3f);
		if (HelicopterBossChaingun.introPlayedCount > 1)
		{
			this.introAnimSpeed *= 0.7f;
		}
	}

	public void ExtendChaingunInstantly()
	{
		this.sprite.SetLowerLeftPixel((float)((int)this.spriteWidth), (float)((int)(this.spriteHeight * 2f)));
	}

	public int chainGunFireLimit = 7;

	protected float chaingunFrameRate = 0.02f;

	public AudioClip chaingunWindUp;

	public AudioClip chaingunWindDown;

	public AudioClip introClip;

	public AudioClip windupClip;

	public AudioClip chaingunHum;

	public Shrapnel bulletShell;

	protected AudioSource chaingunSource;

	protected int windDownCount;

	public bool propellTankBack = true;

	private static int introPlayedCount;

	private int introFrames = 21;

	private float spriteWidth;

	private float spriteHeight;

	public bool playingIntroAnim;

	private bool windUp;

	private float animCounter;

	private float introAnimSpeed = 0.1f;

	private int introFrame;

	public float fireOffsetX = 12f;

	public float fireOffsetY;

	public int chaingunFiringFrameCount = 4;
}
