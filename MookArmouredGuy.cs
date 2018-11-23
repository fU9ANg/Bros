// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookArmouredGuy : MookBigGuy
{
	protected override void Awake()
	{
		base.Awake();
		this.zOffset = (3f + UnityEngine.Random.value) * 0.05f;
	}

	protected override void Start()
	{
		base.Start();
		this.frictionM = 50f;
		this.originalSpeed = this.speed;
		this.zOffset = 0.06f + UnityEngine.Random.value * 0.05f;
		this.halfWidth = 15f;
		this.feetWidth = 9f;
		if (this.miniGunAudio == null)
		{
			this.miniGunAudio = this.gunSprite.gameObject.AddComponent<AudioSource>();
			this.miniGunAudio.rolloffMode = AudioRolloffMode.Linear;
			this.miniGunAudio.minDistance = 200f;
			this.miniGunAudio.dopplerLevel = 0.1f;
			this.miniGunAudio.maxDistance = 500f;
			this.miniGunAudio.volume = 0.5f;
			this.miniGunAudio.playOnAwake = false;
			this.miniGunAudio.pitch = 0.8f;
			this.miniGunAudio.clip = this.minigunStop;
		}
		this.ladderLayer = this.groundLayer;
		this.platformLayer = this.groundLayer;
		this.originalMaterial = this.sprite.GetComponent<Renderer>().sharedMaterial;
		this.originalGunMaterial = this.gunSprite.GetComponent<Renderer>().sharedMaterial;
	}

	protected override void UseFire()
	{
		if (SetResolutionCamera.IsItVisible(base.transform.position))
		{
			this.lastIdleShing = Time.time - 1f;
			this.FireWeapon(this.x + ((!this.frontGun) ? (base.transform.localScale.x * 0f) : (base.transform.localScale.x * 24f)), this.y + 14f, base.transform.localScale.x * 700f, (float)(UnityEngine.Random.Range(0, 60) - 25));
			this.frontGun = !this.frontGun;
			this.PlayAttackSound();
			Map.DisturbWildLife(this.x, this.y, 40f, this.playerNum);
		}
	}

	protected override void RunFiring()
	{
		base.RunFiring();
		if (this.fire)
		{
			if (this.miniGunAudio.clip != this.minigunSpin)
			{
				this.miniGunAudio.clip = this.minigunSpin;
				this.miniGunAudio.loop = true;
				this.miniGunAudio.volume = 0.25f;
				this.miniGunAudio.Play();
			}
		}
		else if (this.miniGunAudio.clip != this.minigunStop)
		{
			this.miniGunAudio.clip = this.minigunStop;
			this.miniGunAudio.volume = 0.2f;
			this.miniGunAudio.loop = false;
			this.miniGunAudio.Play();
		}
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		if (this.gunFrame <= 1)
		{
			this.gunFrame = 3;
		}
		this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * this.gunFrame), (float)this.gunSpritePixelHeight);
		EffectsController.CreateMuzzleFlashBigEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f);
		EffectsController.CreateShrapnel(this.bulletShell, x + base.transform.localScale.x * -6f, y, 1f, 30f, 1f, -base.transform.localScale.x * 55f, 130f);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed, ySpeed, this.firingPlayerNum);
	}

	protected override void DealWithBounce(ref DamageType damageType, ref int damage)
	{
		damage = 0;
		damageType = DamageType.Crush;
	}

	protected override void AnimateRunning()
	{
		if (this.useHeavierWalkFrames)
		{
			this.frameRate = this.runningFrameRate;
			int num = Mathf.Clamp(0 + this.frame % 14, 0, 100);
			if (this.frame % 14 == 12)
			{
				this.PlayFootStepSound(this.soundHolderFootSteps.footStepMetalSounds, 0.15f, 1f);
			}
			if (this.frame % 14 == 2)
			{
				this.PlayFootStepSound(this.soundHolderFootSteps.landMetalSounds, 0.3f, 1f);
				if (!FluidController.IsSubmerged(this))
				{
					EffectsController.CreateFootPoofEffect(this.x + 7f * base.transform.localScale.x, this.y + 2f, 0f, Vector3.up * 1f - Vector3.right * base.transform.localScale.x * 60.5f);
				}
				if (this.pilotUnit != null)
				{
					SortOfFollow.Shake(0.1f);
				}
			}
			if (this.frame % 14 == 5)
			{
				this.PlayFootStepSound(this.soundHolderFootSteps.footStepMetalSounds, 0.15f, 1f);
			}
			if (this.frame % 14 == 9)
			{
				this.PlayFootStepSound(this.soundHolderFootSteps.landMetalSounds, 0.3f, 1f);
				if (!FluidController.IsSubmerged(this))
				{
					EffectsController.CreateFootPoofEffect(this.x - 4f * base.transform.localScale.x, this.y + 2f, 0f, Vector3.up * 1f - Vector3.right * base.transform.localScale.x * 60.5f);
				}
				if (this.pilotUnit != null)
				{
					SortOfFollow.Shake(0.1f);
				}
			}
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 4));
			if (this.gunFrame <= 0)
			{
				this.gunSprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
			}
			if (num == 1 || num == 2 || num == 3 || num == 4 || num == 8 || num == 9 || num == 10 || num == 11)
			{
				this.speed = 0f;
				this.xI = 0f;
			}
			else
			{
				this.speed = this.originalSpeed;
				this.xI = base.transform.localScale.x * this.speed;
			}
		}
		else
		{
			if (this.frame < 0)
			{
				this.frame = 0;
			}
			base.AnimateRunning();
			if (this.frame % 8 == 1 || this.frame % 8 == 2 || this.frame % 8 == 5 || this.frame % 8 == 6)
			{
				this.speed = 0f;
				this.xI = 0f;
			}
			else
			{
				this.speed = this.originalSpeed;
				this.xI = base.transform.localScale.x * this.speed;
			}
		}
	}

	protected override void CalculateMovement()
	{
		base.CalculateMovement();
		if (this.left && !this.wasLeft)
		{
			this.frame = 4;
		}
		if (this.right && !this.wasRight)
		{
			this.frame = 4;
		}
	}

	protected override void CheckInput()
	{
		base.CheckInput();
		this.wasDown = false; this.down = (this.wasDown );
	}

	public override void Knock(DamageType damageType, float xI, float yI, bool forceTumble)
	{
		if (this.health > 0)
		{
			this.knockCount++;
			if (this.knockCount % 8 == 0)
			{
				this.KnockSimple(new DamageObject(0, DamageType.Bullet, xI * 0.5f, yI * 0.3f, null));
				MonoBehaviour.print("knocksimple");
			}
		}
		else
		{
			base.Knock(damageType, xI, yI, forceTumble);
		}
	}

	protected override void CheckRescues()
	{
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		if (damageType == DamageType.Fire)
		{
			this.fireAmount += damage;
		}
		else
		{
			this.shieldDamage += damage;
		}
		if ((damageType == DamageType.Crush || this.actionState == ActionState.Panicking || this.fireAmount > 35 || this.shieldDamage > 60) && this.health > 0)
		{
			base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
		}
		else if (this.health <= 0)
		{
			this.knockCount++;
			if (this.knockCount % 4 == 0)
			{
				yI = Mathf.Min(yI + 20f, 20f);
				this.y += 2f;
				this.Knock(damageType, xI, 0f, false);
			}
			else
			{
				this.Knock(damageType, xI, 0f, false);
			}
			if (damage > 30)
			{
				this.IncreaseDeathCount();
			}
		}
		else
		{
			this.PlayDefendSound();
		}
		if (damageType == DamageType.SelfEsteem && damage >= this.health && this.health > 0)
		{
			MonoBehaviour.print("Abandonment");
			this.Death(0f, 0f, new DamageObject(damage, damageType, 0f, 0f, this));
		}
	}

	protected void IncreaseDeathCount()
	{
		this.deathCount++;
		if (this.deathCount == 2)
		{
			EffectsController.CreateExplosion(this.x, this.y + 5f, 8f, 8f, 120f, 0.5f, 100f, 1f, 0.6f, true);
		}
	}

	protected void PlayDefendSound()
	{
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.defendSounds, 0.7f + UnityEngine.Random.value * 0.4f, base.transform.position, 0.8f + 0.34f * UnityEngine.Random.value);
	}

	protected override void PlayAttackSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.attackSounds, 0.5f, base.transform.position, this.attackPitch);
		}
	}

	protected override void ChangeFrame()
	{
		if (this.landing)
		{
			this.wasButtonJump = false; this.up = (this.buttonJump = (this.wasUp = (this.wasButtonJump )));
			this.speed = 0f;
			this.ActivateGun();
			this.landingFrames++;
			if (this.landingFrames == 1)
			{
				this.frameRate = 0.13334f;
			}
			else if (this.landingFrames > 1)
			{
				this.frameRate = 0.0667f;
			}
			else
			{
				this.frameRate = 0.0334f;
			}
			int num = 20 + this.landingFrames;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(2 * this.spritePixelHeight));
			if (this.landingFrames > 5)
			{
				this.landing = false;
			}
		}
		else if (this.liftingOff)
		{
			this.speed = 0f;
			this.liftOffFrames++;
			int num2 = 11 + this.liftOffFrames;
			if (this.liftOffFrames > 4)
			{
				this.frameRate = 0.1f;
			}
			if (this.liftOffFrames == 2)
			{
				this.platform.transform.localPosition = new Vector3(0f, -2f, 0f);
			}
			if (this.liftOffFrames == 3)
			{
				this.platform.transform.localPosition = new Vector3(0f, -3f, 0f);
				if (!this.up && !this.buttonJump)
				{
					this.liftingOff = false;
					this.landing = true;
					this.landingFrames = 3;
				}
				else
				{
					this.PlayFootStepSound(this.soundHolderFootSteps.footStepMetalSounds, 0.3f, 1f);
				}
			}
			if (this.liftOffFrames > 6)
			{
				this.liftingOff = false;
				if ((this.up || this.buttonJump) && (base.CanTouchGround(-this.halfWidth) || base.CanTouchGround(this.halfWidth)))
				{
					base.Jump(false);
					MapController.DamageGround(this, 10, DamageType.Crush, 30f, this.x, this.y - 8f, null);
					EffectsController.CreateGroundWave(this.x, this.y + 10f, 64f);
					SortOfFollow.Shake(0.3f);
					this.crushingGroundLayers = 0;
					FlameWallExplosion @object = Networking.Instantiate<FlameWallExplosion>(this.liftOffBlastFlameWall, new Vector3(this.x - base.transform.localScale.x * 5f, this.y + 9f, 0f), Quaternion.identity, null, false);
					DirectionEnum arg = DirectionEnum.Any;
					Networking.RPC<int, MookArmouredGuy, DirectionEnum>(PID.TargetAll, new RpcSignature<int, MookArmouredGuy, DirectionEnum>(@object.Setup), this.playerNum, this, arg, false);
				}
			}
			else
			{
				this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)(2 * this.spritePixelHeight));
			}
		}
		else
		{
			base.ChangeFrame();
		}
		if (!this.liftingOff && this.health > 0)
		{
			this.platform.transform.localPosition = new Vector3(0f, 0f, 0f);
		}
	}

	protected override void PressSpecial()
	{
		this.DisChargePilot(150f, false);
		Networking.RPC<int>(PID.TargetAll, new RpcSignature<int>(this.SetDeathCountRpc), 9001, false);
	}

	private void SetDeathCountRpc(int value)
	{
		this.deathCount = value;
	}

	protected override void PressHighFiveMelee(bool forceHighFive = false)
	{
		UnityEngine.Debug.Log("Press PressHighFiveMelee ");
		this.DisChargePilot(130f, false);
		this.Damage(this.health + 1, DamageType.SelfEsteem, 0f, 0f, 0, this, this.x, this.y);
	}

	protected override void Jump(bool wallJump)
	{
		if (!this.liftingOff && !this.landing && (base.CanTouchGround(-this.halfWidth * 0.7f) || base.CanTouchGround(this.halfWidth * 0.7f)))
		{
			this.liftOffFrames = 0;
			this.liftingOff = true;
			this.ChangeFrame();
		}
	}

	protected void PlayJetPackSound()
	{
		if (base.GetComponent<AudioSource>() == null)
		{
			base.gameObject.AddComponent<AudioSource>();
			base.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
			base.GetComponent<AudioSource>().minDistance = 80f;
			base.GetComponent<AudioSource>().maxDistance = 300f;
			base.GetComponent<AudioSource>().spatialBlend = 0.4f;
			base.GetComponent<AudioSource>().dopplerLevel = 0.1f;
			base.GetComponent<AudioSource>().loop = true;
			base.GetComponent<AudioSource>().clip = this.jetPackAudioClip;
			base.GetComponent<AudioSource>().volume = 0.5f;
		}
		if (!base.GetComponent<AudioSource>().isPlaying)
		{
			base.GetComponent<AudioSource>().Play();
		}
	}

	protected void StopJetPackSound()
	{
		if (base.GetComponent<AudioSource>() != null && base.GetComponent<AudioSource>().isPlaying)
		{
			base.GetComponent<AudioSource>().Stop();
		}
	}

	public override void ResetSpecialAmmo()
	{
		base.ResetSpecialAmmo();
		this.jetPackFuel = 1f;
	}

	public override bool IsAmmoFull()
	{
		return this.jetPackFuel >= 1f;
	}

	protected override void Update()
	{
		base.Update();
		if (this.health > 0 && (this.jetPackFuel < 1f || base.SpecialAmmo < this.originalSpecialAmmo))
		{
			PickupableController.UsePickupables(this, 7f, this.x, this.y);
		}
		if (this.health > 0 && this.deathCount >= 2)
		{
			this.smokeCounter += this.t;
			if (this.smokeCounter >= 0.1334f)
			{
				this.smokeCounter -= 0.1334f;
				EffectsController.CreateBlackPlumeParticle(this.x - 8f + UnityEngine.Random.value * 16f, this.y + 11f + UnityEngine.Random.value * 2f, 20f, 0f, 60f, 2f, 1f);
				EffectsController.CreateSparkShower(this.x - 6f + UnityEngine.Random.value * 12f, this.y + 11f + UnityEngine.Random.value * 4f, 1, 2f, 100f, this.xI - 20f + UnityEngine.Random.value * 40f, 100f, 0.5f, 1f);
			}
		}
		if (this.deathCount > 2)
		{
			this.deathCountdownCounter += this.t * (1f + this.deathCountdown * 0.33f);
			if (this.deathCountdownCounter >= 0.4667f)
			{
				this.deathCountdownCounter -= 0.2667f;
				this.deathCountdown += 1f;
				EffectsController.CreateBlackPlumeParticle(this.x - 8f + UnityEngine.Random.value * 16f, this.y + 4f + UnityEngine.Random.value * 2f, 20f, 0f, 60f, 2f, 1f);
				if (this.deathCountdown % 2f == 1f)
				{
					this.sprite.GetComponent<Renderer>().sharedMaterial = this.hurtMaterial;
					this.gunSprite.GetComponent<Renderer>().sharedMaterial = this.hurtGunMaterial;
				}
				else
				{
					this.sprite.GetComponent<Renderer>().sharedMaterial = this.originalMaterial;
					this.gunSprite.GetComponent<Renderer>().sharedMaterial = this.originalGunMaterial;
				}
				if (this.deathCountdown >= 20f)
				{
					this.Gib(DamageType.OutOfBounds, this.xI, this.yI + 150f);
				}
			}
		}
		if (this.health > 0 && (this.up || this.buttonJump) && !this.liftingOff && !this.landing)
		{
			this.jetPackFuelCounter += this.t;
			if ((this.y > 0f && this.jetPackFuel > 0.25f) || (this.jetPackFuel > 0f && Mathf.Repeat(this.jetPackFuelCounter, this.jetPackFuel * 12f + 0.2f) > 0.1f))
			{
				this.PlayJetPackSound();
				this.jetPackFuelWarningOn = false;
				float yI = -80f;
				if (this.yI < -105f)
				{
					yI = -200f;
					this.yI += this.jetPackForce * this.t * 1.2f;
				}
				else if (this.yI < -35f)
				{
					yI = -120f;
					this.yI += this.jetPackForce * this.t;
				}
				else
				{
					this.crushingGroundLayers = 0;
					if (this.yI < 100f)
					{
						this.yI += this.jetPackForce * this.t;
					}
					else
					{
						this.yI += this.jetPackForce * this.t * 0.5f;
						yI = -60f;
					}
				}
				EffectsController.CreatePlumeParticle(this.x - 5f - base.transform.localScale.x * 5f, this.y + 13f, 4f, 0f, yI, 0.6f, 1.3f);
				EffectsController.CreatePlumeParticle(this.x + 5f - base.transform.localScale.x * 5f, this.y + 13f, 4f, 0f, yI, 0.6f, 1.3f);
				this.jetPackFuel -= this.jetPackFuelConsumption * this.t;
				this.speed = this.jetPackXSpeed;
				if (!this.left && !this.right)
				{
					this.xI *= 1f - this.t;
				}
				if (MapController.DamageGround(this, 10, DamageType.Crush, 4f, this.x - 4f, this.y + 25f, null))
				{
					this.yI *= 0.3f;
				}
				if (MapController.DamageGround(this, 10, DamageType.Crush, 4f, this.x + 4f, this.y + 25f, null))
				{
					this.yI *= 0.3f;
				}
			}
			else
			{
				this.StopJetPackSound();
				this.jetPackFuelWarningOn = true;
			}
		}
		else
		{
			this.StopJetPackSound();
		}
		if (this.pilotUnit != null)
		{
			this.pilotUnit.x = this.x;
			this.pilotUnit.y = this.y + 3f;
			this.pilotUnit.row = this.row;
			this.pilotUnit.collumn = this.collumn;
			this.pilotUnit.transform.position = new Vector3(this.pilotUnit.x, this.pilotUnit.y, 10f);
		}
	}

	protected override void AddSpeedLeft()
	{
		if (this.y > this.groundHeight)
		{
			if (this.jetPackFuel > 0f && (this.up || this.buttonJump))
			{
				this.xI = Mathf.Clamp(this.xI - this.jetPackXSpeed * 2f * this.t, -this.jetPackXSpeed, this.jetPackXSpeed * 0.5f);
			}
		}
		else
		{
			base.AddSpeedLeft();
		}
	}

	protected override void AddSpeedRight()
	{
		if (this.y > this.groundHeight)
		{
			if (this.jetPackFuel > 0f && (this.up || this.buttonJump))
			{
				this.xI = Mathf.Clamp(this.xI + this.jetPackXSpeed * 2f * this.t, -this.jetPackXSpeed * 0.5f, this.jetPackXSpeed);
			}
		}
		else
		{
			base.AddSpeedRight();
		}
	}

	protected void DisChargePilot(float disChargeYI, bool stunPilot)
	{
		Networking.RPC<float, bool>(PID.TargetAll, new RpcSignature<float, bool>(this.DisChargePilotRPC), disChargeYI, stunPilot, false);
		Networking.RPC<bool>(PID.TargetAll, new RpcSignature<bool>(base.SetSyncingInternal), false, false);
	}

	protected override void CheckForTraps()
	{
	}

	protected void DisChargePilotRPC(float disChargeYI, bool stunPilot)
	{
		UnityEngine.Debug.Log("DisChargePilotRPC ");
		if (this.pilotUnit != null)
		{
			UnityEngine.Debug.Log("Discharge Pilot ");
			this.pilotUnit.GetComponent<Renderer>().enabled = true;
			this.pilotUnit.DischargePilotingUnit(this.x, Mathf.Clamp(this.y + 6f, -6f, 100000f), this.xI + ((!stunPilot) ? 0f : UnityEngine.Random.Range(-disChargeYI, disChargeYI)), disChargeYI);
			if (stunPilot)
			{
				this.pilotUnit.Stun(1f);
			}
			this.pilotUnit = null;
			this.playerNum = -1;
			this.isHero = false;
			this.firingPlayerNum = -1;
			this.hasBeenPiloted = true;
		}
	}

	protected override void Gib(DamageType damageType, float xI, float yI)
	{
		this.DisChargePilot(180f, false);
		if (this.deathCount > 9000)
		{
			EffectsController.CreateHugeExplosion(this.x + (float)UnityEngine.Random.Range(16, 32), this.y + (float)UnityEngine.Random.Range(-16, 16), 10f, 20f, 120f, 0.8f, 100f, 1f, 0.6f, 5, 70, 200f, 90f, 0.2f, 0.4f);
			EffectsController.CreateExplosion(this.x, this.y + 5f, 8f, 8f, 120f, 0.5f, 100f, 1f, 0.6f, true);
			EffectsController.CreateHugeExplosion(this.x + (float)UnityEngine.Random.Range(-32, -16), this.y + (float)UnityEngine.Random.Range(-16, 16), 10f, 30f, 120f, 1f, 100f, 1f, 0.6f, 5, 70, 200f, 90f, 0.2f, 0.4f);
			Map.ExplodeUnits(this, 20, DamageType.Explosion, 64f, 32f, this.x, this.y + 6f, 200f, 150f, -15, true, false);
			MapController.DamageGround(this, 15, DamageType.Explosion, 64f, this.x, this.y, null);
			SortOfFollow.Shake(1f, 2f);
		}
		else
		{
			EffectsController.CreateExplosion(this.x, this.y + 5f, 8f, 8f, 120f, 0.5f, 100f, 1f, 0.6f, true);
			EffectsController.CreateHugeExplosion(this.x, this.y, 10f, 10f, 120f, 0.5f, 100f, 1f, 0.6f, 5, 70, 200f, 90f, 0.2f, 0.4f);
			MapController.DamageGround(this, 15, DamageType.Explosion, 36f, this.x, this.y, null);
			Map.ExplodeUnits(this, 20, DamageType.Explosion, 48f, 32f, this.x, this.y + 6f, 200f, 150f, -15, true, false);
		}
		base.Gib(damageType, xI, yI);
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		this.DisChargePilot(150f, false);
		MonoBehaviour.print("Death " + base.gameObject);
		if (damage != null && damage.damageType != DamageType.SelfEsteem)
		{
			this.IncreaseDeathCount();
		}
		if (this.pilotSwitch == null)
		{
			this.pilotSwitch = SwitchesController.CreatePilotMookSwitch(this);
		}
		if (base.GetComponent<Collider>() != null)
		{
			base.GetComponent<Collider>().enabled = false;
		}
		if (this.platform != null)
		{
			this.platform.transform.localPosition = new Vector3(0f, 0f, 0f);
		}
		if (this.enemyAI != null)
		{
			this.enemyAI.HideSpeachBubbles();
			this.OnlyDestroyScriptOnSync = true;
			UnityEngine.Object.Destroy(this.enemyAI);
		}
		this.DeactivateGun();
		base.Death(xI, yI, damage);
	}

	protected override void AnimateDeath()
	{
		if (this.y > this.groundHeight + 0.2f && this.impaledOnSpikes == null)
		{
			int num = 4;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)this.spritePixelHeight);
			this.platform.transform.localPosition = new Vector3(0f, 0f, 0f);
		}
		else
		{
			int num2 = 5;
			this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)this.spritePixelHeight);
			this.platform.transform.localPosition = new Vector3(0f, -4f, 0f);
		}
	}

	protected override void Land()
	{
		if (this.yI < -60f)
		{
			this.speed = 0f;
		}
		if ((!this.isHero && this.yI < this.fallDamageHurtSpeed) || (this.isHero && this.yI < this.fallDamageHurtSpeedHero))
		{
			if ((!this.isHero && this.yI < this.fallDamageDeathSpeed) || (this.isHero && this.yI < this.fallDamageDeathSpeedHero))
			{
				this.crushingGroundLayers = 5;
				MapController.DamageGround(this, 10, DamageType.Crush, 32f, this.x, this.y + 8f, null);
				SortOfFollow.Shake(0.3f);
				EffectsController.CreateGroundWave(this.x, this.y, 96f);
				Map.ShakeTrees(this.x, this.y, 144f, 64f, 128f);
			}
			else
			{
				if (this.isHero)
				{
					if (this.yI <= this.fallDamageDeathSpeedHero)
					{
						this.crushingGroundLayers = 3;
					}
					else if (this.yI < this.fallDamageHurtSpeedHero * 0.3f + this.fallDamageDeathSpeedHero * 0.7f)
					{
						this.crushingGroundLayers = 1;
					}
					else if (this.yI < (this.fallDamageHurtSpeedHero + this.fallDamageDeathSpeedHero) / 2f)
					{
						this.crushingGroundLayers = 0;
					}
				}
				else if (this.yI < (this.fallDamageHurtSpeed + this.fallDamageDeathSpeed) / 2f)
				{
					this.crushingGroundLayers = 3;
				}
				MapController.DamageGround(this, 10, DamageType.Crush, 32f, this.x, this.y + 8f, null);
				SortOfFollow.Shake(0.3f);
				EffectsController.CreateGroundWave(this.x, this.y, 80f);
				Map.ShakeTrees(this.x, this.y, 144f, 64f, 128f);
			}
		}
		else if (this.crushingGroundLayers > 0)
		{
			this.crushingGroundLayers--;
			MapController.DamageGround(this, 10, DamageType.Crush, 32f, this.x, this.y + 8f, null);
			SortOfFollow.Shake(0.3f);
			Map.ShakeTrees(this.x, this.y, 80f, 48f, 100f);
		}
		else if (this.yI < -60f && this.health > 0)
		{
			this.landing = true;
			this.landingFrames = 0;
			this.PlayFootStepSound(this.soundHolderFootSteps.landMetalSounds, 0.55f, 0.9f);
			this.gunFrame = 0;
			EffectsController.CreateGroundWave(this.x, this.y + 10f, 64f);
			SortOfFollow.Shake(0.2f);
		}
		else if (this.health > 0)
		{
			this.PlayFootStepSound(this.soundHolderFootSteps.landMetalSounds, 0.35f, 0.9f);
			SortOfFollow.Shake(0.1f);
			this.gunFrame = 0;
		}
		base.Land();
	}

	protected override void FallDamage(float yI)
	{
		this.gunSprite.SetLowerLeftPixel(0f, (float)this.gunSpritePixelHeight);
		if ((!this.isHero && yI < this.fallDamageHurtSpeed) || (this.isHero && yI < this.fallDamageHurtSpeedHero))
		{
			if (this.health > 0)
			{
				this.crushingGroundLayers = Mathf.Max(this.crushingGroundLayers, 3);
			}
			if ((!this.isHero && yI < this.fallDamageDeathSpeed) || (this.isHero && yI < this.fallDamageDeathSpeedHero))
			{
				Map.KnockAndDamageUnit(SingletonMono<MapController>.Instance, this, this.health + 40, DamageType.Crush, -1f, 450f, 0, false);
				Map.ExplodeUnits(this, 25, DamageType.Crush, 64f, 25f, this.x, this.y, 200f, 170f, this.playerNum, false, false);
			}
			else
			{
				Map.KnockAndDamageUnit(SingletonMono<MapController>.Instance, this, this.health - 10, DamageType.Crush, -1f, 450f, 0, false);
				Map.ExplodeUnits(this, 10, DamageType.Crush, 48f, 20f, this.x, this.y, 150f, 120f, this.playerNum, false, false);
			}
		}
	}

	public override bool CanPilotUnit(int newPlayerNum)
	{
		return this.health <= 0 || this.pilotUnit != null;
	}

	public override void PilotUnitRPC(Unit PilotUnit)
	{
		UnityEngine.Debug.Log("PILOT THIS MECH! " + PilotUnit.playerNum);
		if (this.pilotUnit != null)
		{
			this.DisChargePilot(280f, true);
		}
		this.pilotUnit = PilotUnit;
		this.playerNum = this.pilotUnit.playerNum;
		this.health = this.maxHealth;
		this.deathNotificationSent = false;
		this.isHero = true;
		this.firingPlayerNum = PilotUnit.playerNum;
		this.pilotUnit.StartPilotingUnit(this);
		if (this.pilotSwitch != null)
		{
		}
		this.blindTime = 0f;
		this.stunTime = 0f;
		this.burnTime = 0f;
		this.burnDamage = 0;
		this.shieldDamage = 0;
		this.ActivateGun();
		base.GetComponent<Collider>().enabled = true;
		Networking.RPC<PID>(PID.TargetAll, new RpcSignature<PID>(base.SetOwner), PilotUnit.Owner, false);
		Networking.RPC<bool>(PID.TargetAll, new RpcSignature<bool>(base.SetSyncingInternal), true, false);
	}

	public override float GetFuel()
	{
		return this.jetPackFuel;
	}

	public override bool GetFuelWarning()
	{
		return this.jetPackFuelWarningOn;
	}

	public override void AnimateActualIdleFrames()
	{
		if (this.animatingIdleShing)
		{
			this.frameRate = 0.07f;
			int num = this.frame % 6;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 5));
			if (num == 5)
			{
				this.animatingIdleShing = false;
			}
		}
		else
		{
			if (Time.time - this.lastIdleShing > 8f)
			{
				this.animatingIdleShing = true;
				this.frame = -1;
				this.lastIdleShing = Time.time;
			}
			base.AnimateActualIdleFrames();
		}
	}

	public override UnityStream PackState(UnityStream stream)
	{
		stream.Serialize<float>(this.x);
		stream.Serialize<float>(this.y);
		MonoBehaviour.print("Pack Armoured Mook");
		stream.Serialize<Unit>(this.pilotUnit);
		return base.PackState(stream);
	}

	public override UnityStream UnpackState(UnityStream stream)
	{
		this.x = (float)stream.DeserializeNext();
		this.y = (float)stream.DeserializeNext();
		MonoBehaviour.print("Unpack Armoured Mook");
		Unit x = (Unit)stream.DeserializeNext();
		if (x != null)
		{
			this.PilotUnitRPC(x);
		}
		return base.UnpackState(stream);
	}

	protected int knockCount;

	protected int fireAmount;

	protected int shieldDamage;

	protected int crushingGroundLayers;

	protected float originalSpeed;

	public bool useHeavierWalkFrames = true;

	private AudioSource miniGunAudio;

	public float attackPitch = 0.8f;

	public GameObject platform;

	protected float jetPackFuel = 1f;

	protected bool jetPackFuelWarningOn;

	public float jetPackFuelConsumption = 0.2f;

	public float jetPackXSpeed = 100f;

	public float jetPackForce = 1200f;

	protected float jetPackFuelCounter;

	public bool hasBeenPiloted;

	protected MobileSwitch pilotSwitch;

	protected int liftOffFrames;

	protected bool liftingOff;

	protected bool landing;

	protected int landingFrames;

	protected Unit pilotUnit;

	public AudioClip jetPackAudioClip;

	public AudioClip minigunSpin;

	public AudioClip minigunStop;

	protected float smokeCounter;

	protected int deathCount;

	protected float deathCountdownCounter;

	protected float deathCountdown;

	public Material hurtMaterial;

	public Material hurtGunMaterial;

	protected Material originalMaterial;

	protected Material originalGunMaterial;

	public FlameWallExplosion liftOffBlastFlameWall;

	private bool frontGun;

	public float fallDamageHurtSpeed = -250f;

	public float fallDamageHurtSpeedHero = -250f;

	public float fallDamageDeathSpeed = -400f;

	public float fallDamageDeathSpeedHero = -400f;

	private bool animatingIdleShing;

	private float lastIdleShing = 2f;
}
