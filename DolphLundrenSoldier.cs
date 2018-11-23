// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DolphLundrenSoldier : Mook
{
	protected override void SetDeltaTime()
	{
		if (PlayerOptions.Instance.hardMode)
		{
			if (this.groundHeight > this.y - 1f)
			{
				this.lastT = this.t;
				this.t = Mathf.Clamp(Time.deltaTime * 2f, 0f, 0.0334f);
			}
			else
			{
				base.SetDeltaTime();
			}
		}
		else
		{
			base.SetDeltaTime();
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.dolfAI = base.gameObject.GetComponent<DolfLundgrenAI>();
	}

	protected override void Start()
	{
		base.Start();
		this.standingHeadHeight = 26f;
		this.headHeight = this.standingHeadHeight;
		this.halfWidth = 12f;
		this.width = 12f;
		this.height = 18f;
		this.feetWidth = 6f;
		this.healthReviveAmount = 90 + 30 * HeroController.GetPlayersPlayingCount();
	}

	protected override void AnimateRunning()
	{
		float burnTime = this.burnTime;
		float blindTime = this.blindTime;
		this.scaredTime = 0f;
		this.burnTime = 0f;
		this.blindTime = 0f;
		base.AnimateRunning();
		this.blindTime = blindTime;
		this.burnTime = burnTime;
	}

	protected override void Update()
	{
		this.bonusHealth -= this.t * (float)this.healthReviveAmount * 0.25f;
		if (this.reviveTime > 0f)
		{
			this.reviveTime -= this.t;
			if (this.getUpQuick)
			{
				this.reviveTime -= this.t * 1.5f;
			}
			this.health = this.healthReviveAmount;
			this.burnDamage = 0;
			if (this.y <= this.groundHeight)
			{
				this.actionState = ActionState.Idle;
			}
			else
			{
				this.actionState = ActionState.Jumping;
			}
			if (this.reviveTime <= 0f)
			{
				UnityEngine.Debug.Log("Finish Revived  " + this.deathCount);
				this.dolfAI.Revived();
				if (this.deathCount == 1)
				{
					UnityEngine.Debug.Log("Finish Revive Zoom ");
					SortOfFollow.ReturnToNormal(1f);
				}
			}
			this.frame = 0;
		}
		base.Update();
		RaycastHit raycastHit;
		RaycastHit raycastHit2;
		if (Physics.Raycast(new Vector3(this.x, this.y + 6f, 0f), Vector3.left, out raycastHit, 16f, Map.groundLayer) && Physics.Raycast(new Vector3(this.x, this.y + 6f, 0f), Vector3.right, out raycastHit2, 16f, Map.groundLayer))
		{
			raycastHit.collider.SendMessage("Damage", new DamageObject(10, DamageType.Crush, 0f, 100f, null), SendMessageOptions.DontRequireReceiver);
			raycastHit2.collider.SendMessage("Damage", new DamageObject(10, DamageType.Crush, 0f, 100f, null), SendMessageOptions.DontRequireReceiver);
		}
		if (this.y > this.groundHeight + 1f && Physics.Raycast(new Vector3(this.x, this.y + 2f, 0f), Vector3.up, out raycastHit, 16f, Map.groundLayer) && !Physics.Raycast(new Vector3(this.x, this.y + this.headHeight - 4f, 0f), new Vector3(base.transform.localScale.x, 0f, 0f), out raycastHit, 16f, Map.groundLayer) && Physics.Raycast(new Vector3(this.x, this.y + 6f, 0f), new Vector3(base.transform.localScale.x, 0f, 0f), out raycastHit2, 16f, Map.groundLayer))
		{
			raycastHit2.collider.SendMessage("Damage", new DamageObject(10, DamageType.Crush, 0f, 100f, null), SendMessageOptions.DontRequireReceiver);
		}
		if (this.health < -100)
		{
			this.invulnerable = true;
		}
		if (this.health <= 0)
		{
			this.deathDownTime += this.t;
			if (this.deathDownTime > 2f && this.groundHeight > this.y - 1f)
			{
				this.invulnerable = false;
				this.deathCount++;
				this.deathDownTime = 0f;
				this.health = this.healthReviveAmount;
				this.bonusHealth = (float)this.healthReviveAmount / 2f;
				if (this.y <= this.groundHeight)
				{
					this.actionState = ActionState.Idle;
				}
				else
				{
					this.actionState = ActionState.Jumping;
				}
				this.burnDamage = 0;
				this.ChangeFrame();
				this.reviveTime = 1.7f;
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Revive! ",
					this.health,
					" deathCount ",
					this.deathCount
				}));
				this.ChangeFrame();
				EffectsController.CreateWhiteFlashPop(this.x, this.y + 12f);
				this.getUpQuick = (this.deathCount > 1 && HeroController.IsPlayerNearby(this.x, this.y, 64f, 64f));
				if (this.deathCount == 1)
				{
					if (SortOfFollow.IsItSortOfVisible(this.x, this.y, -32f, -32f))
					{
						UnityEngine.Debug.Log("Revive!  DO ZOOM ");
						SortOfFollow.ControlledByTriggerAction = true;
						SortOfFollow.ForceSlowSnapBack(0.5f, 0.5f);
						SortOfFollow.followPos = base.transform.position + Vector3.up * 24f;
					}
					else
					{
						this.deathCount--;
					}
				}
			}
		}
		if (this.isHoldingSuperJump && this.yI > 200f)
		{
			this.isHoldingSuperJump = false;
			EffectsController.CreateAirDashPoofEffect(this.x, this.y + 4f, Vector3.up);
			EffectsController.CreateGroundWave(this.x, this.y + 1f, 80f);
			SortOfFollow.Shake(0.5f);
			base.PlayDashSound(0.7f);
		}
		if (this.slowState == DolphLundrenSoldier.SlomMoState.hasNotStartedSlowing)
		{
			if (this.yI < -100f && this.y > this.groundHeight && this.groundHeight <= 0f && this.y < 100f)
			{
				Networking.RPC<float, float, float, float>(PID.TargetAll, true, false, new RpcSignature<float, float, float, float>(this.SetSlowMotion), this.x, this.y, this.xI, this.yI);
			}
		}
		else if (this.slowState == DolphLundrenSoldier.SlomMoState.Slowing)
		{
			Time.timeScale = Mathf.Lerp(Time.timeScale, 0.1f, this.t * 35f);
			SortOfFollow.followPos = new Vector3(this.x, 0f, 0f);
			if (this.y < 0f)
			{
				Networking.RPC(PID.TargetAll, new RpcSignature(this.SetRegularMotion), false);
			}
		}
	}

	private void SetSlowMotion(float X, float Y, float XI, float YI)
	{
		if (this.slowState == DolphLundrenSoldier.SlomMoState.hasNotStartedSlowing)
		{
			this.x = X;
			this.y = Y;
			this.yI = YI;
			this.xI = XI;
			this.SetPosition();
			SortOfFollow.ControlledByTriggerAction = true;
			SortOfFollow.ForceSlowSnapBack(0.2f, 0.667f);
			this.slowState = DolphLundrenSoldier.SlomMoState.Slowing;
			Sound.SetPitch(0.7f);
		}
	}

	private void SetRegularMotion()
	{
		if (this.slowState != DolphLundrenSoldier.SlomMoState.FinishedSlowing)
		{
			Time.timeScale = 1f;
			SortOfFollow.ReturnToNormal(0.5f);
			Sound.SetPitch(1f);
			this.slowState = DolphLundrenSoldier.SlomMoState.FinishedSlowing;
		}
	}

	protected override void Gib(DamageType damageType, float xI, float yI)
	{
		SortOfFollow.ReturnToNormal(1.5f);
		Time.timeScale = 1f;
		Sound.SetPitch(1f);
		GameModeController.LevelFinish(LevelResult.Success);
		base.Gib(damageType, xI, yI);
	}

	protected override void Land()
	{
		if (this.yI < -330f)
		{
			this.MakeLandBlast(this.x, this.groundHeight + 16f, true);
			SortOfFollow.Shake(1f);
			UnityEngine.Debug.Log("Need footstep sound");
			this.dolfAI.Crouch();
		}
		base.Land();
	}

	public override void Blind()
	{
		base.Blind();
		this.blindTime = 1.8f;
		this.firingPlayerNum = this.playerNum;
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		if (this.bonusHealth > (float)damage)
		{
			this.bonusHealth -= (float)damage;
			damage = 0;
		}
		this.dolfAI.AddDamagePressure(damage);
		if (SortOfFollow.IsItSortOfVisible(this.x, this.y + 10f, 24f, 32f))
		{
			base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
		}
		if (this.health <= 0)
		{
			this.rocketLauncherGlow.enabled = false;
		}
		if (this.health > 0 && this.health - damage <= 0)
		{
			UnityEngine.Debug.Log("health is 0");
			Networking.RPC<int, float, float, float, float>(PID.TargetOthers, new RpcSignature<int, float, float, float, float>(this.SetHealthToZeroRPC), this.health, this.x, this.y, xI, yI, false);
		}
	}

	public void SetHealthToZeroRPC(int Health, float X, float Y, float xI, float yI)
	{
		if (this.health > Health)
		{
			this.health = Health;
		}
		float num = 10f;
		if (Mathf.Abs(this.x - X) > num)
		{
			this.x = X;
		}
		if (Mathf.Abs(this.y - Y) > num)
		{
			this.y = Y;
		}
	}

	public virtual bool Damage(DamageObject Damage)
	{
		this.dolfAI.AddDamagePressure(Damage.damage);
		return SortOfFollow.IsItSortOfVisible(this.x, this.y + 10f, 24f, 32f);
	}

	public override bool IsHeavy()
	{
		return true;
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

	protected override void RunFiring()
	{
		this.fireDelay -= this.t;
		if (this.fire)
		{
			this.rollingFrames = 0;
		}
		if (this.fire && this.reviveTime <= 0f)
		{
			if (this.siegeModeFrame < 3 || this.fireDelay > 0f)
			{
				this.rocketLauncherBeepCounter += this.t;
				if (this.rocketLauncherBeepCounter > 0.067f)
				{
					this.rocketLauncherBeepCounter -= 0.067f;
					this.rocketLauncherBeepCount++;
					this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * (6 + this.rocketLauncherBeepCount % 2)), (float)this.gunSpritePixelHeight);
					this.rocketLauncherGlow.enabled = (this.rocketLauncherBeepCount % 2 == 0);
					if (this.rocketLauncherBeepCount % 2 == 0)
					{
						this.sound.PlaySoundEffectAt(this.soundHolder.missSounds, 0.005f + (float)this.rocketLauncherBeepCount * 0.01f, base.transform.position, 0.5f);
					}
				}
			}
			if (this.siegeModeFrame < 3)
			{
				this.siegeModeCounter += this.t;
				if (this.siegeModeCounter > 0.0667f)
				{
					this.siegeModeCounter -= 0.0667f;
					this.siegeModeFrame++;
					this.ChangeFrame();
					if (this.siegeModeFrame == 3)
					{
						this.fireDelay = 0.5f;
					}
				}
			}
			else if (this.fireDelay <= 0f)
			{
				this.rocketLauncherGlow.enabled = false;
				this.fireCounter += this.t;
				if (this.fireCounter >= this.fireRate)
				{
					this.fireCounter -= this.fireRate;
					this.UseFire();
				}
			}
		}
		else if (this.siegeModeFrame > 0)
		{
			this.siegeModeCounter += this.t;
			if (this.siegeModeCounter > 0.0667f)
			{
				this.siegeModeCounter -= 0.0667f;
				this.siegeModeFrame--;
				if (this.actionState == ActionState.Idle)
				{
					this.ChangeFrame();
				}
			}
		}
		if (!this.fire)
		{
			this.rocketLauncherBeepCount = 0;
			this.rocketLauncherGlow.enabled = false;
		}
	}

	protected override void AnimateSpecial()
	{
		this.SetSpriteOffset(0f, 0f);
		this.DeactivateGun();
		this.frameRate = 0.0667f;
		int num = 18 + Mathf.Clamp(this.frame, 0, 8);
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)this.spritePixelHeight);
		if (this.frame == 5)
		{
			this.UseSpecial();
			this.frameRate = 0.12f;
		}
		if (this.frame == 3 || this.frame == 4)
		{
			this.frameRate = 0.025f;
		}
		if (this.frame >= 9)
		{
			this.frame = 0;
			this.usingSpecial = false;
		}
	}

	protected override void AnimateSpecial2()
	{
		this.SetSpriteOffset(0f, 0f);
		this.frameRate = 0.0667f;
		int num = Mathf.Clamp(this.frame, 0, 4);
		int num2 = 19 + num;
		this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
		this.SetGunPosition((float)num, (float)Mathf.Clamp(-num + 1, -3, 0));
		this.isHoldingSuperJump = true;
		this.xI = 0f;
		this.xIBlast = 0f;
	}

	protected override void AnimateSpecial3()
	{
		this.SetSpriteOffset(0f, 0f);
		this.DeactivateGun();
		this.frameRate = 0.0445f;
		if (this.frame <= 4)
		{
			int frame = this.frame;
			int num = 23 + frame;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 4));
		}
		else
		{
			int num2 = (this.frame - 5) % 6;
			int num3 = 28 + num2;
			this.sprite.SetLowerLeftPixel((float)(num3 * this.spritePixelWidth), (float)(this.spritePixelHeight * 4));
		}
		if (this.frame == 11)
		{
			this.dolfAI.StartBombardment();
		}
		this.xI = 0f;
		this.xIBlast = 0f;
	}

	protected override void UseSpecial()
	{
		float num = 128f;
		float num2 = 32f;
		bool playerRange = this.enemyAI.GetPlayerRange(ref num, ref num2);
		this.PlaySpecialAttackSound(0.35f);
		float num3 = 100f + UnityEngine.Random.value * 10f;
		float num4 = 180f;
		if (playerRange)
		{
			float num5 = Mathf.Clamp((50f + num * 0.7f + num2 * 0.33f) * this.grenadeTossDistanceSpeedM, 0.5f, 1.5f);
			num3 *= num5;
			num4 *= num5;
		}
		if (base.IsMine)
		{
			ProjectileController.SpawnGrenadeOverNetwork(this.specialGrenade, this, this.x + Mathf.Sign(base.transform.localScale.x) * 18f, this.y + 22f, 0.001f, 0.011f, Mathf.Sign(base.transform.localScale.x) * num3, num4, this.playerNum);
		}
	}

	public override void PlaySpecialAttackSound(float v)
	{
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.specialAttackSounds, v, base.transform.position, 0.95f + UnityEngine.Random.value * 0.08f);
	}

	protected override void AnimateWallClimb()
	{
		if (this.knifeHand % 2 == 0)
		{
			int num = 11 + Mathf.Clamp(this.frame, 0, 3);
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)this.spritePixelHeight);
			if (this.frame == 1)
			{
				this.yI = 190f;
				this.PlayKnifeClimbSound();
			}
			if (this.frame >= 5)
			{
				this.frame = 0;
				this.knifeHand++;
				this.RunStepOnWalls();
			}
			else if (this.frame > 1 && !FluidController.IsSubmerged(this))
			{
				EffectsController.CreateFootPoofEffect(this.x + base.transform.localScale.x * 6f, this.y + 12f, 0f, Vector3.up * 1f);
			}
		}
		else
		{
			int num2 = 14 + Mathf.Clamp(this.frame, 0, 3);
			this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)this.spritePixelHeight);
			if (this.frame == 1)
			{
				this.yI = 190f;
				this.PlayKnifeClimbSound();
			}
			if (this.frame >= 5)
			{
				this.frame = 0;
				this.knifeHand++;
			}
			else if (this.frame > 1 && !FluidController.IsSubmerged(this))
			{
				EffectsController.CreateFootPoofEffect(this.x + base.transform.localScale.x * 6f, this.y + 12f, 0f, Vector3.up * 1f);
			}
		}
	}

	protected override void FallDamage(float yI)
	{
	}

	protected override void AnimateIdle()
	{
		if (this.reviveTime > 0f)
		{
			this.DeactivateGun();
			if (this.reviveTime < 0.4f)
			{
				SortOfFollow.Shake(0.3f);
				int num = 17 + Mathf.Clamp(this.reviveRoarFrame, 0, 3);
				this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 4));
				this.reviveRoarFrame++;
				if (this.reviveRoarFrame > 4)
				{
					this.reviveRoarFrame = 3;
					this.frameRate = 0.033f;
				}
				else
				{
					this.frameRate = 0.066f;
				}
				if (!this.hasLaughed)
				{
					this.hasLaughed = true;
					base.PlayPowerUpSound(0.7f);
					this.hasRumbled = false;
				}
			}
			else if (this.reviveTime < 1f)
			{
				this.frameRate = 0.066f;
				int num2 = 13 + this.frame % 4;
				this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)(this.spritePixelHeight * 4));
				this.reviveRoarFrame = 0;
				this.hasRumbled = false;
			}
			else
			{
				this.frameRate = 0.065f;
				int num3 = 11 + this.frame % 2;
				this.sprite.SetLowerLeftPixel((float)(num3 * this.spritePixelWidth), (float)(this.spritePixelHeight * 4));
				this.reviveRoarFrame = 0;
				this.hasLaughed = false;
				if (!this.hasRumbled && !this.getUpQuick)
				{
					this.hasRumbled = true;
					this.PlayBassDropSoundSound();
				}
			}
		}
		else if (this.fire || this.siegeModeFrame > 0)
		{
			int num4 = Mathf.Clamp(this.siegeModeFrame, 0, 3);
			int num5 = 11 + num4;
			this.sprite.SetLowerLeftPixel((float)(num5 * this.spritePixelWidth), (float)(this.spritePixelHeight * 2));
			this.SetGunPosition(0f, (float)(-(float)num4));
			this.ActivateGun();
		}
		else
		{
			base.AnimateIdle();
		}
	}

	protected override void UseFire()
	{
		this.FireWeapon(this.x + base.transform.localScale.x * 17f, this.y + 18f, base.transform.localScale.x * 160f, (float)(UnityEngine.Random.Range(0, 45) - 15));
		this.PlayAttackSound();
		Map.DisturbWildLife(this.x, this.y, 40f, this.playerNum);
	}

	protected void MakeLandBlast(float xPoint, float yPoint, bool groundWave)
	{
		this.invulnerable = true;
		Map.ExplodeUnits(this, 25, DamageType.Crush, 32f, 20f, xPoint, yPoint, 150f, 90f, this.playerNum, false, false);
		this.invulnerable = false;
		MapController.DamageGround(this, 15, DamageType.Explosion, 56f, xPoint, yPoint, null);
		if (groundWave)
		{
			EffectsController.CreateGroundWave(xPoint, yPoint + 1f, 80f);
			Map.ShakeTrees(this.x, this.y, 64f, 32f, 64f);
		}
		Map.DisturbWildLife(this.x, this.y, 48f, this.playerNum);
		this.PlayFallDamageSound(0.8f);
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

	public override void Knock(DamageType damageType, float xI, float yI, bool forceTumble)
	{
		this.xIBlast = Mathf.Clamp(this.xIBlast + xI / 2f, -10f, 10f);
		if (this.health <= 0)
		{
			this.yI = Mathf.Clamp(this.yI + yI / 5f, -20000f, 300f);
		}
	}

	public override void Panic(bool forgetPlayer)
	{
	}

	public override void Panic(float time, bool forgetPlayer)
	{
	}

	public override void Panic(int direction, float time, bool forgetPlayer)
	{
	}

	protected override bool PanicAI(bool forgetPlayer)
	{
		return false;
	}

	public override bool Activate()
	{
		this.currentEnemyNum = HeroController.GetNearestPlayer(this.x, this.y, 30000f, 4000f);
		if (this.currentEnemyNum >= 0)
		{
			this.searchingAnimation = true;
			this.frame = 0;
			this.counter -= 1.334f;
			this.ChangeFrame();
			if (this.enemyAI != null)
			{
				this.enemyAI.SetMentalState(MentalState.Idle);
			}
			this.xI = -0.1f;
			return true;
		}
		return false;
	}

	protected override void ChangeFrame()
	{
		if (this.searchingAnimation)
		{
			this.frameRate = 0.045f;
			if (this.frame == 1)
			{
				this.SetGunPosition(1f, 0f);
			}
			else if (this.frame == 1)
			{
				this.SetGunPosition(1f, -1f);
			}
			else if (this.frame >= 2 && this.frame <= 12)
			{
				this.SetGunPosition(2f, -2f);
			}
			else if (this.frame == 13)
			{
				this.SetGunPosition(1f, -1f);
			}
			else if (this.frame == 14)
			{
				this.SetGunPosition(1f, 0f);
			}
			if (this.frame == 9 || this.frame == 4)
			{
				this.counter -= 0.33f;
			}
			if (this.frame == 12)
			{
				this.searchingCount++;
				if (this.searchingCount < 2)
				{
					this.frame = 2;
				}
			}
			int num = 19 + Mathf.Clamp(this.frame, 0, 12);
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 2));
			if (this.frame >= 14)
			{
				this.searchingAnimation = false;
				this.enemyAI.FullyAlert(this.x, this.y, this.currentEnemyNum);
			}
			this.frame++;
		}
		else
		{
			base.ChangeFrame();
		}
	}

	public Shrapnel bulletShell;

	protected int siegeModeFrame;

	protected float siegeModeCounter;

	protected DolfLundgrenAI dolfAI;

	public int healthReviveAmount = 50;

	protected float reviveTime;

	protected bool isHoldingSuperJump;

	public Renderer rocketLauncherGlow;

	protected float bonusHealth;

	protected float deathDownTime;

	protected int deathCount;

	private DolphLundrenSoldier.SlomMoState slowState;

	protected bool wasSlowMoOnDeath;

	protected float rocketLauncherBeepCounter;

	protected int rocketLauncherBeepCount;

	public float grenadeTossDistanceSpeedM = 0.01f;

	private int reviveRoarFrame;

	protected bool getUpQuick;

	protected bool hasLaughed;

	protected bool hasRumbled;

	protected bool searchingAnimation;

	protected int currentEnemyNum;

	protected int searchingCount;

	private enum SlomMoState
	{
		hasNotStartedSlowing,
		Slowing,
		FinishedSlowing
	}
}
