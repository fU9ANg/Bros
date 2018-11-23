// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Mook : TestVanDammeAnim
{
	protected override void Awake()
	{
		base.gameObject.AddComponent<DisableWhenOffCamera>();
		if (this.parachute != null)
		{
			this.parachute.gameObject.SetActive(false);
		}
		base.Awake();
		this.zOffset = (1.6f + UnityEngine.Random.value) * 0.05f;
		this.spawnTime = Time.time;
	}

	public override bool IsParachuteActive
	{
		get
		{
			return this.isParachuteActive;
		}
		set
		{
			if (this.parachute != null)
			{
				this.parachute.gameObject.SetActive(value);
				this.isParachuteActive = value;
				this.maxFallSpeed = ((!value) ? -400f : -50f);
			}
			else
			{
				this.isParachuteActive = false;
			}
		}
	}

	public bool CanAddToStatistics()
	{
		return !(this.originDoor != null);
	}

	public override float GetGroundHeightGround()
	{
		if (this.actionState != ActionState.Panicking && this.burnTime <= 0f && this.blindTime <= 0f && this.scaredTime <= 0f)
		{
			return base.GetGroundHeightGround();
		}
		float num = -200f;
		if (this.xI > 0f)
		{
			if (Physics.Raycast(new Vector3(this.x + 4f, this.y + 16f, 0f), Vector3.down, out this.raycastHit, 540f, this.groundLayer) && this.raycastHit.point.y > num)
			{
				num = this.raycastHit.point.y;
			}
			if (Physics.Raycast(new Vector3(this.x - 2f, this.y + 16f, 0f), Vector3.down, out this.raycastHit, 540f, this.groundLayer) && this.raycastHit.point.y > num)
			{
				num = this.raycastHit.point.y;
			}
		}
		if (this.xI < 0f)
		{
			if (Physics.Raycast(new Vector3(this.x - 4f, this.y + 16f, 0f), Vector3.down, out this.raycastHit, 540f, this.groundLayer) && this.raycastHit.point.y > num)
			{
				num = this.raycastHit.point.y;
			}
			if (Physics.Raycast(new Vector3(this.x + 2f, this.y + 16f, 0f), Vector3.down, out this.raycastHit, 540f, this.groundLayer) && this.raycastHit.point.y > num)
			{
				num = this.raycastHit.point.y;
			}
		}
		if (Physics.Raycast(new Vector3(this.x, this.y + 16f, 0f), Vector3.down, out this.raycastHit, 540f, this.groundLayer) && this.raycastHit.point.y > num)
		{
			num = this.raycastHit.point.y;
		}
		return num;
	}

	protected override void ApplyFallingGravity()
	{
		if (this.midAirShakeTime > 0f)
		{
			this.midAirShakeTime -= this.t;
			if (this.midAirShakeTime >= 0f || this.groundHeight < this.y - 1f)
			{
			}
		}
		else if (this.isParachuteActive)
		{
			this.yI -= 1100f * this.t * 0.17f;
			this.xI *= 1f - this.t * 1f;
		}
		else
		{
			base.ApplyFallingGravity();
		}
	}

	protected override void AddSpeedLeft()
	{
		if (this.isParachuteActive)
		{
			this.xI -= this.speed * 0.5f * this.t;
			if (this.xI < -this.speed * 0.4f)
			{
				this.xI = -this.speed * -0.4f;
			}
		}
		else if (!this.tumbling)
		{
			base.AddSpeedLeft();
		}
	}

	protected override void AddSpeedRight()
	{
		if (this.isParachuteActive)
		{
			this.xI += this.speed * 0.5f * this.t;
			if (this.xI > this.speed * 0.4f)
			{
				this.xI = this.speed * 0.4f;
			}
		}
		else if (!this.tumbling)
		{
			base.AddSpeedRight();
		}
	}

	protected override void Start()
	{
		base.Start();
		if (!this.exitingDoor)
		{
			if (Physics.Raycast(new Vector3(base.transform.position.x, base.transform.position.y + 6f, 0f), Vector3.down, out this.raycastHit, 32f, this.groundLayer))
			{
				this.SetPosition(this.raycastHit.point);
			}
			this.invulnerable = false;
		}
		else
		{
			this.invulnerable = true;
			this.invulnerableTime = 0.6f;
		}
		if (this.IsParachuteActive && this.y < SortOfFollow.GetScreenMaxY() + 4f && (this.x < SortOfFollow.GetScreenMaxX() & this.x > SortOfFollow.GetScreenMinX()))
		{
			this.y = SortOfFollow.GetScreenMaxY() + 4f;
			this.SetPosition();
		}
	}

	public override void Tumble()
	{
		base.Tumble();
		if (this.canTumble)
		{
			if (this.health > 0 && this.canWilhelm && this.yI > 50f && Time.time - this.spawnTime > 1f && !this.hasTumbled && !this.tumbling && this.soundHolder.special2Sounds.Length > 0 && UnityEngine.Random.value < 0.2f)
			{
				Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.special2Sounds, 0.2f, base.transform.position);
			}
			this.hasTumbled = true;
			this.tumbling = true;
		}
	}

	protected override void FallDamage(float yI)
	{
		if (yI < -350f)
		{
			Map.KnockAndDamageUnit(this, this, (this.health <= 0) ? 0 : 5, DamageType.Fall, -1f, 450f, 0, false);
		}
	}

	protected override void UseFire()
	{
		if (SetResolutionCamera.IsItVisible(base.transform.position))
		{
			this.FireWeapon(this.x + base.transform.localScale.x * 10f, this.y + 8f, base.transform.localScale.x * (float)(250 + ((!Demonstration.bulletsAreFast) ? 0 : 150)), (float)(UnityEngine.Random.Range(0, 4) - 2) * ((!Demonstration.bulletsAreFast) ? 0.2f : 1f));
			Map.DisturbWildLife(this.x, this.y, 40f, this.playerNum);
		}
	}

	public void RegisterOriginDoor(MookDoor mookDoor)
	{
		this.originDoor = mookDoor;
		this.exitDoorX = mookDoor.transform.position.x;
		if (!mookDoor.isDestroyed)
		{
			if (mookDoor.GetComponent<MookDoorSliding>() != null || UnityEngine.Random.value > 0.5f)
			{
				this.SetPosition(base.transform.position - Vector3.right * 16f);
				this.exitDoorDirection = 1f;
			}
			else
			{
				this.SetPosition(base.transform.position + Vector3.right * 16f);
				this.exitDoorDirection = -1f;
			}
		}
		this.exitingDoor = true;
		if (Physics.Raycast(new Vector3(this.x, this.y + 6f, 14f), Vector3.down, out this.raycastHit, 32f, this.groundLayer))
		{
			this.x = this.raycastHit.point.x;
			this.y = this.raycastHit.point.y;
		}
		this.SetPosition();
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		if (!Demonstration.projectilesHitWalls)
		{
			this.invulnerable = true;
		}
		if (this.originDoor != null)
		{
			this.originDoor.RemoveMook(this);
			this.originDoor = null;
		}
		if (!this.hasDied)
		{
			StatisticsController.NotifyMookDeath(this);
			Map.EnemyDeathEvent(this);
			this.hasDied = true;
		}
		if (this.tumbling)
		{
			this.tumbling = false;
		}
		if (this.IsParachuteActive)
		{
			this.IsParachuteActive = false;
			this.Tumble();
		}
		base.Death(xI, yI, damage);
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 3;
		this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * this.gunFrame), 32f);
		EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, base.transform);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed, ySpeed, this.firingPlayerNum);
		this.PlayAttackSound();
	}

	public override void SetPosition(Vector3 pos)
	{
		if (!this.exitingDoor)
		{
			base.SetPosition(pos);
		}
		else
		{
			base.transform.position = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), 14f);
			this.x = base.transform.position.x;
			this.y = base.transform.position.y;
		}
	}

	public override void SetPosition()
	{
		if (!this.exitingDoor)
		{
			base.SetPosition();
		}
		else
		{
			base.transform.position = new Vector3(Mathf.Round(this.x), Mathf.Round(this.y), 14f + this.zOffset);
		}
	}

	protected override void GetEnemyMovement()
	{
		if (Map.isEditing)
		{
			return;
		}
		if (!this.exitingDoor)
		{
			base.GetEnemyMovement();
		}
		else if (this.exitDoorDirection < 0f)
		{
			if (this.x > this.exitDoorX)
			{
				this.left = true;
				this.right = false;
			}
			else
			{
				this.exitingDoor = false;
			}
		}
		else if (this.x < this.exitDoorX)
		{
			this.left = false;
			this.right = true;
		}
		else
		{
			this.exitingDoor = false;
		}
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		if (damageType == DamageType.Plasma)
		{
			this.frame = 0;
			if (damage >= 15)
			{
				this.plasmaDamage += 4;
				damage = Mathf.Clamp(this.health + 1, 1, 1000);
			}
			else
			{
				this.plasmaDamage++;
				if (this.plasmaDamage > 1)
				{
					damage = 1;
				}
				else
				{
					damage = 0;
				}
			}
			if (this.plasmaDamage < 7)
			{
				this.plasmaCounter = 0.25f;
			}
			else
			{
				this.plasmaCounter = 0.1f;
			}
			if (!this.immuneToPlasmaShock)
			{
				this.stunTime = 0.25f;
				this.Stop();
			}
		}
		if (this.health <= 0)
		{
			this.canWilhelm = false;
		}
		if (damageType == DamageType.Knifed && this.health > 0 && this.health - damage < 0 && (this.enemyAI == null || !this.enemyAI.IsAlerted()))
		{
			StatisticsController.NotifyKnifedMook(this);
		}
		if (this.canBeSetOnFire && damageType == DamageType.Fire && this.health > 0)
		{
			if (this.burnDamage <= this.health * 5)
			{
				this.burnDamage += damage;
				damage = 0;
			}
			else
			{
				damage = this.burnDamage + damage;
			}
		}
		base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
		if (this.enemyAI != null && this.health > 0)
		{
			this.enemyAI.HearSound(this.x - xI, this.y);
		}
		if (this.health > 0)
		{
			if (damageType == DamageType.Fire && this.PanicAI((int)Mathf.Sign(xI), true))
			{
				this.burnTime = Mathf.Clamp(this.burnTime + 0.1f, 1f, 2f);
			}
			if (damageType == DamageType.Stun)
			{
				this.Blind(4f);
			}
		}
		if (this.canTumble && yI > 200f && yI > Mathf.Abs(xI))
		{
			this.Tumble();
		}
	}

	public void SetSpawnState(float _xI, float _yI, bool tumble, bool useParachuteDelay, bool useParachute, bool onFire, bool isAlert)
	{
		if (onFire)
		{
			Map.KnockAndDamageUnit(SingletonMono<MapController>.Instance, this, 3, DamageType.Fire, 0f, 0f, 0, false);
		}
		if (tumble)
		{
			this.Tumble();
		}
		if (useParachuteDelay)
		{
			this.SetCanParachute(true);
		}
		if (useParachute)
		{
			this.OpenParachute();
		}
		if (isAlert)
		{
			float x = 0f;
			float y = 0f;
			int playerNum = -1;
			if (this.enemyAI != null && this.health > 0 && HeroController.GetRandomPlayerPos(ref x, ref y, ref playerNum))
			{
				this.enemyAI.FullyAlert(x, y, playerNum);
			}
		}
		this.yI = _yI;
		this.xI = _xI;
	}

	protected virtual bool PanicAI(bool forgetPlayer)
	{
		return this.health > 0 && this.PanicAI(this.enemyAI.walkDirection, forgetPlayer);
	}

	protected virtual bool PanicAI(int direction, bool forgetPlayer)
	{
		return this.health > 0 && this.enemyAI.Panic(direction, forgetPlayer);
	}

	public override void BurnInternal(int damage, int direction)
	{
		this.burnDamage += damage;
		if (this.health > 0 && this.burnTime <= 0f && this.PanicAI(direction, true))
		{
			this.burnTime = Mathf.Clamp(this.burnTime + 0.4f, 1f, 2f);
		}
	}

	public override void Panic(bool forgetPlayer)
	{
		if (this.health <= 0 || this.blindTime > 0f || this.PanicAI(forgetPlayer))
		{
		}
		this.blindTime = Mathf.Max(this.blindTime, 0.1f);
	}

	public override void Panic(float time, bool forgetPlayer)
	{
		if (this.health <= 0 || this.blindTime > 0f || this.PanicAI(forgetPlayer))
		{
		}
		this.blindTime = Mathf.Max(this.blindTime, time);
	}

	public override void Panic(int direction, float time, bool forgetPlayer)
	{
		if (this.health > 0 && this.blindTime <= 0f)
		{
			this.PanicAI((direction != 0) ? ((int)Mathf.Sign((float)direction)) : 0, forgetPlayer);
		}
		this.blindTime = Mathf.Max(this.blindTime, time);
	}

	public override void Terrify()
	{
		this.scaredTime = 1f;
	}

	public override void OpenParachute()
	{
		if (!this.IsParachuteActive)
		{
			this.IsParachuteActive = true;
		}
	}

	public override void SetCanParachute(bool canParachute)
	{
		if (canParachute)
		{
			if (!this.IsParachuteActive)
			{
				this.parachuteDelay = 0.6f;
			}
		}
		else if (this.IsParachuteActive)
		{
			this.IsParachuteActive = false;
		}
	}

	public override void Stun(float time)
	{
		this.Stop();
		this.stunTime = time;
	}

	public override void Blind(float time)
	{
		this.blindTime = Mathf.Max(this.blindTime, time);
		if (this.health > 0)
		{
			this.enemyAI.Blind();
			this.firingPlayerNum = 5;
		}
	}

	public override void Blind()
	{
		this.blindTime = 9f;
		if (this.health > 0)
		{
			this.enemyAI.Blind();
			this.firingPlayerNum = 5;
		}
	}

	public override void Attract(float xTarget, float yTarget)
	{
		if (this.health > 0)
		{
			this.enemyAI.Attract(xTarget, yTarget);
		}
	}

	public override void Alert(float alertX, float alertY)
	{
		if (this.enemyAI != null && this.health > 0)
		{
			this.enemyAI.HearSound(alertX, alertY);
		}
	}

	public override void FullyAlert(float x, float y, int playerNum)
	{
		if (this.enemyAI != null && this.health > 0)
		{
			this.enemyAI.FullyAlert(x, y, playerNum);
		}
	}

	public override void HearSound(float alertX, float alertY)
	{
		if (this.enemyAI != null && this.health > 0)
		{
			this.enemyAI.HearSound(alertX, alertY);
		}
	}

	protected override void AnimateFallen()
	{
		base.AnimateFallen();
		this.frameRate = 0.045f;
		this.DeactivateGun();
		if (this.fallenTime > 0f)
		{
			int num = 20 + Mathf.Clamp(this.frame, 0, 5);
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), 128f);
		}
		else
		{
			int num2 = 25 + Mathf.Clamp(this.frame, 0, 6);
			this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), 128f);
			if (this.frame >= 6 && this.actionState == ActionState.Fallen)
			{
				this.actionState = ActionState.Running;
				this.ActivateGun();
			}
		}
	}

	protected override void RunFallen()
	{
		base.RunFallen();
		if (this.fallenTime > 0f)
		{
			this.fallenTime -= this.t;
			if (this.fallenTime <= 0f)
			{
				this.frame = 0;
			}
		}
		if (this.y < this.groundHeight + 0.5f)
		{
			this.xI = 0f;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (Map.isEditing)
		{
			return;
		}
		if (this.plasmaCounter > 0f)
		{
			this.plasmaCounter -= this.t;
			if (this.plasmaCounter <= 0f)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					" health ",
					this.health > 0,
					" plasma ",
					this.plasmaDamage
				}));
				if (this.health > 0 || this.plasmaDamage > 1)
				{
					this.Damage(this.plasmaDamage * 10, DamageType.Explosion, 0f, 100f, (int)(-(int)base.transform.localScale.x), this, this.x, this.y + 4f);
				}
				this.plasmaDamage = 0;
			}
		}
		if (this.burnTime > 0f)
		{
			this.burnCounter += this.t;
			if (this.burnCounter > 0.04f)
			{
				this.burnCounter -= 0.13f;
				if (!this.exitingDoor)
				{
					EffectsController.CreateEffect(this.flames[UnityEngine.Random.Range(0, this.flames.Length)], this.x, this.y + this.height, base.transform.position.z - 1f);
				}
			}
			this.fireSpreadCounter += this.t;
			if (this.fireSpreadCounter > this.fireSpreadRate)
			{
				this.fireSpreadCounter -= this.fireSpreadRate;
				this.BurnOthers();
			}
			this.RunBurning();
			this.parachuteDelay = 0f;
		}
		if (this.blindTime > 0f && this.health > 0)
		{
			if (!this.decapitated)
			{
				this.RunBlindStars();
			}
			this.blindTime -= this.t;
			if (this.blindTime <= 0f)
			{
				this.StopBeingBlind();
			}
		}
		if (this.scaredTime > 0f && this.health > 0)
		{
			this.scaredTime -= this.t;
		}
		if (this.parachuteDelay > 0f)
		{
			this.parachuteDelay -= this.t;
			if (this.parachuteDelay <= 0f && this.health == this.maxHealth && this.y > this.groundHeight + 32f && !this.IsParachuteActive)
			{
				this.xI *= 0.5f;
				this.IsParachuteActive = true;
			}
		}
		else if (this.IsParachuteActive)
		{
			if (this.parachute != null && this.parachute.IsDeformed())
			{
				this.maxFallSpeed = -75f;
			}
			else
			{
				this.maxFallSpeed = -50f;
			}
		}
	}

	protected virtual bool CanPassThroughBarriers()
	{
		return this.health <= 0 || (this.actionState == ActionState.Jumping && Mathf.Abs(this.xIBlast) > 1f) || this.blindTime > 0f || this.scaredTime > 0f || (this.enemyAI != null && this.enemyAI.IsAlerted());
	}

	protected override void AddParentedDiff(float xDiff, float yDiff)
	{
		if (this.health > 0 && this.yI > -50f && yDiff < 0f && this.lastParentedToTransform != null && this.lastParentedToTransform.GetComponent<FallingBlock>() != null)
		{
			this.midAirShakeTime = 0.02f;
			yDiff = 0f;
			this.yI += 25f;
			this.Stop();
			this.Tumble();
		}
		base.AddParentedDiff(xDiff, yDiff);
	}

	protected override void ConstrainToFragileBarriers(ref float xIT, float radius)
	{
		if (this.CanPassThroughBarriers())
		{
			base.ConstrainToFragileBarriers(ref xIT, radius);
		}
		else
		{
			if (xIT < 0f && Physics.Raycast(new Vector3(this.x + 2f, this.y + this.waistHeight, 0f), Vector3.left, out this.raycastHit, 23f, this.fragileLayer) && this.raycastHit.point.x > this.x - radius + xIT)
			{
				this.xI = 0f;
				xIT = this.raycastHit.point.x - (this.x - radius);
				return;
			}
			if (xIT > 0f && Physics.Raycast(new Vector3(this.x - 2f, this.y + this.waistHeight, 0f), Vector3.right, out this.raycastHit, 23f, this.fragileLayer) && this.raycastHit.point.x < this.x + radius + xIT)
			{
				this.xI = 0f;
				xIT = this.raycastHit.point.x - (this.x + radius);
				return;
			}
		}
	}

	protected override void ConstrainToMookBarriers(ref float xIT, float radius)
	{
		if (this.burnTime > 0f || this.blindTime > 0f)
		{
			base.ConstrainToMookBarriers(ref xIT, radius);
		}
	}

	protected virtual void RunBurning()
	{
		this.burnTime -= this.t;
		if (this.health > 0)
		{
			this.burnTime -= this.t;
		}
		if (this.burnTime <= 0f)
		{
			this.StopBurning();
		}
	}

	protected virtual void StopBeingBlind()
	{
		this.enemyAI.StopBeingBlind();
		this.ChangeFrame();
		this.Stop();
		this.firingPlayerNum = this.playerNum;
	}

	protected virtual void StopBurning()
	{
		this.enemyAI.StopPanicking();
		this.actionState = ActionState.Idle;
		this.ChangeFrame();
		this.Stop();
		Map.KnockAndDamageUnit(this, this, this.burnDamage, DamageType.Normal, 0f, 0f, 0, false);
	}

	public override void ForgetPlayer(int deadPlayerNum)
	{
		if (this.enemyAI != null)
		{
			this.enemyAI.TryForgetPlayer(deadPlayerNum);
		}
	}

	protected override void AnimateJumping()
	{
		if (this.decapitated)
		{
			this.AnimateDecapitated();
		}
		else if (this.IsParachuteActive)
		{
			if (this.burnTime > 0f || this.blindTime > 0f || this.scaredTime > 0f)
			{
				this.AnimateRunning();
			}
			else
			{
				this.AnimateIdle();
			}
		}
		else
		{
			base.AnimateJumping();
		}
	}

	protected override void PlayDeathGargleSound()
	{
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.deathGargleSounds, 0.35f, base.transform.position);
		this.hasPlayedDeathGargle = true;
	}

	public override void PlayPanicSound()
	{
		if (!this.decapitated)
		{
			base.PlayPanicSound();
		}
	}

	public bool IsDecapitated()
	{
		return this.decapitated;
	}

	protected void AnimateDecapitated()
	{
		this.DeactivateGun();
		this.frameRate = 0.044455f;
		int num = 21 + this.frame % 8;
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), 32f);
		if (this.frame % 4 == 0)
		{
			if (!this.hasPlayedDeathGargle && this.decapitationCounter > 0.3f && UnityEngine.Random.value > 1f)
			{
				this.PlayDeathGargleSound();
			}
			else
			{
				this.PlayBleedSound();
			}
			EffectsController.CreateBloodParticles(BloodColor.Red, this.x, this.y + 12f, 3, 2f, 1f, 60f, this.xI * 0.2f, this.yI * 0.5f + 130f);
		}
	}

	protected override void AnimateRunning()
	{
		if (this.decapitated)
		{
			this.AnimateDecapitated();
		}
		else if (this.burnTime > 0f || this.blindTime > 0f || this.scaredTime > 0f)
		{
			this.DeactivateGun();
			this.frameRate = 0.044455f;
			int num = 21 + this.frame % 8;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), 32f);
		}
		else
		{
			base.AnimateRunning();
		}
	}

	protected override void AnimateDeath()
	{
		if (!this.tumbling)
		{
			if (this.showElectrifiedFrames && this.plasmaDamage > 0)
			{
				this.plasmaFrame++;
				this.DeactivateGun();
				this.frameRate = 0.033f;
				if (this.y > this.groundHeight + 0.2f && this.impaledOnSpikes == null)
				{
					this.sprite.SetLowerLeftPixel((float)(15 * this.spritePixelWidth), (float)(this.spritePixelHeight * 4));
				}
				else
				{
					this.sprite.SetLowerLeftPixel((float)(16 * this.spritePixelWidth), (float)(this.spritePixelHeight * 4));
				}
			}
			else
			{
				base.AnimateDeath();
			}
		}
		else
		{
			this.DeactivateGun();
			this.frameRate = 0.033f;
			this.sprite.SetLowerLeftPixel((float)((21 + this.frame % 10) * this.spritePixelWidth), 64f);
		}
	}

	protected override void AnimateActualJumpingFrames()
	{
		if (!this.tumbling)
		{
			base.AnimateActualJumpingFrames();
		}
		else
		{
			this.DeactivateGun();
			this.frameRate = 0.033f;
			this.sprite.SetLowerLeftPixel((float)((21 + this.frame % 10) * this.spritePixelWidth), 64f);
		}
	}

	protected override void Land()
	{
		if (this.health <= 0 && this.yI < -200f)
		{
			Map.DisturbWildLife(this.x, this.y, 48f, 15);
		}
		base.Land();
		this.IsParachuteActive = false;
		if (this.tumbling)
		{
			this.tumbling = false;
			if (this.health <= 0)
			{
				this.yI = 50f;
				this.y += 4f;
				this.xI *= 0.5f;
			}
			else if (this.canLandOnFace)
			{
				this.actionState = ActionState.Fallen;
				this.frame = 0;
				if (this.enemyAI != null && this.health > 0)
				{
					this.enemyAI.Reassess();
					this.enemyAI.SetMentalState(MentalState.Idle);
				}
				this.fallenTime = 1f;
				this.ChangeFrame();
				this.xI = 0f;
			}
			else
			{
				if (this.enemyAI != null && this.health > 0)
				{
					this.enemyAI.Reassess();
				}
				this.yI = 80f;
				this.xI *= 0.5f;
			}
		}
	}

	public override void Knock(DamageType damageType, float xI, float yI, bool forceTumble)
	{
		base.Knock(damageType, xI, yI, forceTumble);
		if (this.IsParachuteActive && yI > 0f)
		{
			this.IsParachuteActive = false;
			this.Tumble();
		}
		if ((forceTumble || (yI > 200f && yI > Mathf.Abs(xI))) && this.canTumble)
		{
			this.Tumble();
		}
	}

	protected virtual void BurnOthers()
	{
		if (Demonstration.enemiesSpreadFire)
		{
			if (Physics.Raycast(new Vector3(this.x, this.y + 5f, 0f), Vector3.right, out this.raycastHit, 14f, this.groundLayer))
			{
				Unit component = this.raycastHit.collider.gameObject.GetComponent<Unit>();
				if (component != null)
				{
					UnityEngine.Debug.LogError("How did that happen?");
				}
				Block component2 = this.raycastHit.collider.gameObject.GetComponent<Block>();
				if (component2 != null)
				{
					MapController.Damage_Local(this, component2.gameObject, 0, DamageType.Fire, 0f, 0f);
				}
			}
			if (Physics.Raycast(new Vector3(this.x, this.y + 5f, 0f), Vector3.left, out this.raycastHit, 14f, this.groundLayer))
			{
				Unit component3 = this.raycastHit.collider.gameObject.GetComponent<Unit>();
				if (component3 != null)
				{
					UnityEngine.Debug.LogError("How did that happen?");
				}
				Block component4 = this.raycastHit.collider.gameObject.GetComponent<Block>();
				if (component4 != null)
				{
					MapController.Damage_Local(this, component4.gameObject, 0, DamageType.Fire, 0f, 0f);
				}
			}
			MapController.BurnUnitsAround_NotNetworked(this, 5, 0, 16f, this.x, this.y, true, false);
		}
	}

	public override void RollOnto(int direction)
	{
		if (!Map.IsBlockSolid(this.collumn + direction, this.row))
		{
			this.Knock(DamageType.Explosion, (float)(direction * 200), 9f, false);
		}
		else
		{
			this.Gib(DamageType.Crush, (float)(direction * 200), 8f);
		}
	}

	public override bool IsExitingDoor()
	{
		return this.exitingDoor;
	}

	protected override void Gib(DamageType damageType, float xI, float yI)
	{
		if (!this.hasDied)
		{
			StatisticsController.NotifyMookDeath(this);
			Map.EnemyDeathEvent(this);
			this.hasDied = true;
		}
		base.Gib(damageType, xI, yI);
	}

	public void Gib()
	{
		this.Gib(DamageType.Normal, 0f, 0f);
	}

	protected override void NotifyDeathType()
	{
		if (!this.hasNotifiedDeathType)
		{
			this.hasNotifiedDeathType = true;
			StatisticsController.NotifyMookDeathType(this, this.deathType);
		}
	}

	public virtual void StartEatingCorpse()
	{
	}

	public override MookType GetMookType()
	{
		return this.mookType;
	}

	public override void AnimateActualIdleFrames()
	{
		if (this.showElectrifiedFrames && this.plasmaCounter > 0f)
		{
			this.frameRate = 0.033f;
			this.DeactivateGun();
			int num = 6 + this.frame % 2;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 4));
		}
		else
		{
			base.AnimateActualIdleFrames();
		}
	}

	protected override void Unrevive()
	{
		base.Unrevive();
		this.playerNum = -1;
	}

	public override bool Revive(int playerNum, bool isUnderPlayerControl, TestVanDammeAnim reviveSource)
	{
		if (this.canBeRevived)
		{
			base.Revive(playerNum, isUnderPlayerControl, reviveSource);
			this.blindTime = 0f;
			this.scaredTime = 0f;
			this.firingPlayerNum = playerNum;
			this.decapitated = false;
			this.enemyAI.enabled = false;
			this.enemyAI.mentalState = MentalState.Alerted;
			this.speed *= 0.9f + UnityEngine.Random.value * 0.12f;
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Revived Mook decapitated ",
				this.decapitated,
				"  firingPlayerNum ",
				this.firingPlayerNum
			}));
			return true;
		}
		return false;
	}

	public MookType mookType;

	protected float blindTime;

	protected float scaredTime;

	protected bool tumbling;

	public bool canTumble;

	protected bool hasTumbled;

	protected float fireSpreadCounter;

	protected float fireSpreadRate = 0.067f;

	protected bool hasDied;

	protected float fallenTime;

	public bool canLandOnFace;

	public bool showElectrifiedFrames;

	public bool immuneToPlasmaShock;

	protected int plasmaFrame;

	protected float midAirShakeTime;

	[HideInInspector]
	public int firingPlayerNum = -1;

	protected bool isParachuteActive;

	protected float parachuteDelay;

	public bool canWilhelm;

	protected float spawnTime;

	protected bool decapitated;

	protected float decapitationCounter;

	public Parachute parachute;

	public bool exitingDoor;

	protected float exitDoorX;

	protected float exitDoorDirection;

	public FlickerFader[] flames;

	public MookDoor originDoor;

	public bool canBeRevived;

	public bool canBeSetOnFire = true;

	protected bool hasPlayedDeathGargle;
}
