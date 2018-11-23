// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Mech : Unit
{
	protected override void Awake()
	{
		base.Awake();
		int playersPlayingCount = HeroController.GetPlayersPlayingCount();
		float num = 1f + (float)(playersPlayingCount - 1) * 0.2f;
		this.maxHealth = (this.health = (int)((float)this.health * num));
		this.chainGun1.maxHealth = (this.chainGun1.health = (int)((float)this.chainGun1.health * num));
		this.chainGun2.maxHealth = (this.chainGun2.health = (int)((float)this.chainGun2.health * num));
		this.sprite = base.GetComponent<SpriteSM>();
		if (this.sprite != null)
		{
			this.spritePixelWidth = (int)this.sprite.pixelDimensions.x;
		}
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
		this.mechAI = base.GetComponent<PolymorphicAI>();
		this.zOffset = (4f + UnityEngine.Random.value * 1f) * 0.05f;
	}

	protected virtual void Start()
	{
		if (base.GetComponent<Renderer>() != null)
		{
			this.baseMaterial = base.GetComponent<Renderer>().sharedMaterial;
		}
		this.height = 28f;
		this.width = 35f;
		this.playerNum = -1;
		this.invulnerable = false;
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
		Map.RegisterUnit(this, true);
		this.GetGroundHeight();
		if (this.y < this.groundHeight + 64f)
		{
			this.SetToGroundHeight();
		}
		this.SetPosition(0f);
		this.testTurnCounter = UnityEngine.Random.value * 2f;
	}

	public void ForceFaceLeft()
	{
		this.facingDirection = -1;
		base.transform.localScale = new Vector3(-1f, 1f, 1f);
		this.mechAI.walkDirection = -1;
	}

	public override bool IsHeavy()
	{
		return true;
	}

	protected virtual void SetToGroundHeight()
	{
		this.y = this.groundHeight;
	}

	protected virtual void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		this.delayDestroyTime -= this.t;
		this.RunHurt();
		this.RunCheckDeath();
		if (this.stunTime > 0f)
		{
			this.stunTime -= this.t;
			this.RunBlindStars();
			if (this.stunTime <= 0f && this.weapon != null)
			{
				this.weapon.enabled = true;
			}
		}
		if (!Map.isEditing)
		{
			if (base.IsMine || !base.Syncronize)
			{
				this.RunAI();
			}
			if (base.IsMine || !base.Syncronize)
			{
				this.RunInput();
			}
			this.RunAnimations();
			if (base.IsMine || !base.Syncronize)
			{
				this.RunMovement();
			}
			if (base.IsMine || !base.Syncronize)
			{
				this.RunStanding();
			}
			this.grenadeHatch.invulnerable = true;
			this.weapon.invulnerable = true;
		}
		if (this.health < this.maxHealth / 2)
		{
			this.healthFlashCounter += this.t;
			if (this.healthFlashCounter > 0.667f)
			{
				this.healthFlashCounter -= 0.667f;
				this.SetHurtMaterial();
			}
		}
	}

	protected virtual void RunInput()
	{
		if (this.health > 0 && this.stunTime <= 0f)
		{
			if (this.left && this.right)
			{
				this.SetStopSpeed();
				if (this.actionState != ActionState.Turning)
				{
					this.actionState = ActionState.Idle;
				}
				return;
			}
			if (this.left && this.actionState != ActionState.Turning && base.transform.localScale.x > 0f)
			{
				this.StartTurnLeft();
			}
			else if (this.left && this.facingDirection < 0 && this.IsMoveable())
			{
				if (this.actionState != ActionState.Rolling)
				{
					this.actionState = ActionState.Rolling;
					this.PlayRollingClip();
				}
				this.SetLeftSpeed();
			}
			if (this.right && this.actionState != ActionState.Turning && base.transform.localScale.x < 0f)
			{
				this.StartTurnRight();
			}
			else if (this.right && this.facingDirection > 0 && this.IsMoveable())
			{
				if (this.actionState != ActionState.Rolling)
				{
					this.actionState = ActionState.Rolling;
					this.PlayRollingClip();
				}
				this.SetRightSpeed();
			}
			if (!this.left && !this.right && this.actionState != ActionState.Turning)
			{
				this.SetStopSpeed();
				if (this.actionState != ActionState.Idle)
				{
					this.actionState = ActionState.Idle;
					this.StopEngineSound();
				}
			}
			if (this.jetpacksOn)
			{
				if (this.y > this.groundHeight + 1f)
				{
					if ((this.up && this.down) || (!this.up && !this.down))
					{
						this.yI *= 1f - this.t * 10f;
					}
					else if (this.down)
					{
						this.yI -= 1100f * this.t;
						if (this.yI < this.terminalVelocity)
						{
							this.yI = this.terminalVelocity;
						}
					}
					else if (this.up)
					{
						this.yI += 100f * this.t;
						if (this.yI > this.maxUpwardVelocity)
						{
							this.yI = this.maxUpwardVelocity;
						}
					}
					if (this.up || this.left || this.right)
					{
						this.jetpackFlamesCounter += this.t;
						if (this.jetpackFlamesCounter >= 0.025f)
						{
							this.jetpackFlamesCounter -= 0.025f;
							Vector3 vector = this.shouldersTransform.TransformPoint(-32f, -10f, 10f);
							EffectsController.CreatePlumeParticle(vector.x - 2f, vector.y, 9f, 20f, -15f + UnityEngine.Random.value * 30f + this.xI * -0.1f, -130f + UnityEngine.Random.value * 50f, 0.5f, 2f);
							EffectsController.CreatePlumeParticle(vector.x + 2f, vector.y, 9f, 20f, -15f + UnityEngine.Random.value * 30f + this.xI * -0.1f, -130f + UnityEngine.Random.value * 50f, 0.5f, 2f);
							EffectsController.CreatePlumeParticle(vector.x + 2f, vector.y, 9f, 20f, -15f + UnityEngine.Random.value * 30f + this.xI * -0.1f, -130f + UnityEngine.Random.value * 50f, 0.5f, 2f);
							vector = this.shouldersTransform.TransformPoint(32f, -10f, 10f);
							EffectsController.CreatePlumeParticle(vector.x - 2f, vector.y, 9f, 20f, -15f + UnityEngine.Random.value * 30f + this.xI * -0.1f, -130f + UnityEngine.Random.value * 50f, 0.5f, 2f);
							EffectsController.CreatePlumeParticle(vector.x + 2f, vector.y, 9f, 20f, -15f + UnityEngine.Random.value * 30f + this.xI * -0.1f, -130f + UnityEngine.Random.value * 50f, 0.5f, 2f);
							EffectsController.CreatePlumeParticle(vector.x + 2f, vector.y, 9f, 20f, -15f + UnityEngine.Random.value * 30f + this.xI * -0.1f, -130f + UnityEngine.Random.value * 50f, 0.5f, 2f);
						}
					}
				}
				else if ((this.up && this.mechAnimation.clip == null) || this.mechAnimation.clip.name != "Jump")
				{
					this.mechAnimation["Jump"].speed = 1f;
					this.mechAnimation.Play("Jump", PlayMode.StopAll);
				}
			}
			this.CheckAboveGround();
		}
	}

	public virtual void ExecuteJump()
	{
		UnityEngine.Debug.Log("Mech ExecuteJump! ");
		this.settled = false;
		this.yI = this.maxUpwardVelocity;
		this.y += 2f;
		EffectsController.CreateGroundWave(this.x, this.y + 8f, 128f);
	}

	protected virtual void StopEngineSound()
	{
		this.engineAudio.Stop();
	}

	protected virtual void PlayRollingClip()
	{
		this.engineAudio.clip = this.engineRollingClip;
		this.engineAudio.Play();
	}

	protected virtual void PlayTurningClip()
	{
		this.engineAudio.clip = this.engineTurningClip;
		this.engineAudio.Play();
	}

	protected virtual void PlaySettleClip()
	{
		this.engineAudio.clip = this.engineSettleClip;
		this.engineAudio.Play();
	}

	protected virtual void SetStopSpeed()
	{
		this.xI = 0f;
	}

	protected virtual void SetLeftSpeed()
	{
		this.xI = -this.tankSpeed;
	}

	protected virtual void SetRightSpeed()
	{
		this.xI = this.tankSpeed;
	}

	protected virtual bool IsMoveable()
	{
		return this.actionState != ActionState.Turning && (this.y < this.groundHeight + 1f || this.jetpacksOn);
	}

	protected virtual void CheckAboveGround()
	{
		if (this.y > this.groundHeight + 1f && !this.jetpacksOn)
		{
			if (this.actionState != ActionState.Turning)
			{
				this.actionState = ActionState.Idle;
			}
			this.xI = 0f;
		}
	}

	protected virtual void StartTurnLeft()
	{
		this.facingDirection = -1;
		this.actionState = ActionState.Turning;
		this.xI = 0f;
		this.PlayTurningClip();
	}

	protected virtual void StartTurnRight()
	{
		this.facingDirection = 1;
		this.actionState = ActionState.Turning;
		this.xI = 0f;
		this.PlayTurningClip();
	}

	protected bool ConstrainToBlock(ref float xIT, float xOrigin, float yOrigin, Vector3 direction, float distance, float distanceStop, bool damage)
	{
		bool result = false;
		if (xIT != 0f && Physics.Raycast(new Vector3(xOrigin, yOrigin, 0f), direction, out this.raycastHit, distance, this.groundLayer))
		{
			if (xIT > 0f)
			{
				if (this.x + distanceStop + xIT > this.raycastHit.point.x)
				{
					xIT = this.raycastHit.point.x - (this.x + distanceStop);
					result = true;
				}
				if (damage)
				{
					this.raycastHit.collider.SendMessage("Damage", new DamageObject(5, DamageType.Crush, 0f, 0f, null), SendMessageOptions.DontRequireReceiver);
				}
			}
			else if (xIT < 0f)
			{
				if (this.x - distanceStop + xIT < this.raycastHit.point.x)
				{
					xIT = this.raycastHit.point.x - (this.x - distanceStop);
					result = true;
				}
				if (damage)
				{
					this.raycastHit.collider.SendMessage("Damage", new DamageObject(5, DamageType.Crush, 0f, 0f, null), SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		return result;
	}

	protected virtual void ConstrainToBlocks(int direction, ref float xIT)
	{
		if (direction != 0)
		{
			if (!this.ConstrainToBlock(ref xIT, this.x - (float)(8 * direction), this.y + 22f, (direction <= 0) ? Vector3.left : Vector3.right, 50f, 32f, false))
			{
				if (this.ConstrainToBlock(ref xIT, this.x - (float)(8 * direction), this.y + 8f, (direction <= 0) ? Vector3.left : Vector3.right, 50f, 32f, true))
				{
				}
			}
		}
	}

	protected void SwitchToDamagedMaterial()
	{
		foreach (Renderer renderer in this.damagedStateRenderers)
		{
			renderer.sharedMaterial = this.damagedMaterial;
		}
	}

	protected virtual void RunMovement()
	{
		float num = this.xI * this.t;
		if (this.xI != 0f)
		{
			this.ConstrainToBlocks((int)Mathf.Sign(this.xI), ref num);
		}
		if (this.weapon != null)
		{
			this.weapon.ConstrainToWalls(this.xI, ref num);
		}
		this.x += num;
		this.row = (int)((this.y + 16f) / 16f);
		this.collumn = (int)((this.x + 8f) / 16f);
	}

	protected virtual void RunAI()
	{
		this.left = false;
		this.right = false;
		if ((this.weapon != null && this.weapon.health <= 0) || this.health <= 0)
		{
			return;
		}
		this.mechAI.GetInput(ref this.left, ref this.right, ref this.up, ref this.down, ref this.jump, ref this.fire, ref this.special, ref this.special2, ref this.special3, ref this.jetpacksOn);
		if (this.fire)
		{
			this.FireWeapon();
		}
		if (this.special)
		{
			this.UseSpecial();
		}
		if (this.special2)
		{
			this.UseSpecial2();
		}
		if (this.special3)
		{
			this.UseSpecial3();
		}
	}

	protected virtual void UseSpecial()
	{
		if (this.weapon != null)
		{
			this.weapon.UseSpecial();
		}
	}

	protected virtual void UseSpecial2()
	{
		if (this.grenadeHatch != null)
		{
			this.grenadeHatch.Fire();
		}
	}

	protected virtual void UseSpecial3()
	{
		if (this.chainGun1 != null)
		{
			this.chainGun1.Fire();
		}
		if (this.chainGun2 != null)
		{
			this.chainGun2.Fire();
		}
	}

	protected virtual void FireWeapon()
	{
		if (this.weapon != null)
		{
			this.weapon.Fire();
		}
	}

	protected virtual void RunAnimations()
	{
		if (this.health > 0)
		{
			ActionState actionState = this.actionState;
			if (actionState != ActionState.Idle)
			{
				if (actionState != ActionState.Rolling)
				{
					if (actionState == ActionState.Turning)
					{
						this.AnimateTurning();
					}
				}
				else
				{
					this.AnimateRolling();
				}
			}
			else
			{
				this.AnimateIdle();
			}
		}
		else
		{
			this.xI = 0f;
		}
	}

	protected virtual void AnimateRolling()
	{
		this.frameCounter += this.t;
		if (this.frameCounter > 0.0334f)
		{
			this.frameCounter -= 0.0334f;
			this.frame++;
		}
	}

	protected virtual void AnimateIdle()
	{
		this.frameCounter += this.t;
		if (this.frameCounter > 0.0334f)
		{
			this.frameCounter -= 0.0334f;
		}
	}

	protected virtual void AnimateTurning()
	{
		if (this.facingDirection < 0 && base.transform.localScale.x > 0f)
		{
			this.frameCounter += this.t;
			if (this.frameCounter > 0.067f)
			{
				this.frameCounter -= 0.067f;
				this.turnFrame++;
				if (this.turnFrame >= 6)
				{
					base.transform.localScale = new Vector3(-1f, 1f, 1f);
					this.FinishTurning();
				}
				else
				{
					this.SetSpriteTurn(this.turnFrame);
				}
			}
		}
		else if (this.facingDirection > 0 && base.transform.localScale.x < 0f)
		{
			this.frameCounter += this.t;
			if (this.frameCounter > 0.067f)
			{
				this.frameCounter -= 0.067f;
				this.turnFrame++;
				if (this.turnFrame >= 6)
				{
					base.transform.localScale = new Vector3(1f, 1f, 1f);
					this.FinishTurning();
				}
				else
				{
					this.SetSpriteTurn(this.turnFrame);
				}
			}
		}
		else if (this.turnFrame > 0)
		{
			UnityEngine.Debug.LogError("Should never reach!");
			this.frameCounter += this.t;
			if (this.frameCounter > 0.067f)
			{
				this.frameCounter -= 0.067f;
				this.turnFrame--;
			}
			this.SetSpriteTurn(this.turnFrame);
			if (this.turnFrame == 0)
			{
				this.actionState = ActionState.Idle;
				this.PlaySettleClip();
			}
		}
	}

	public bool CanFrontAssault()
	{
		return this.chainGun1.health > 0 || this.chainGun2.health > 0;
	}

	protected void FinishTurning()
	{
		this.turnFrame = 0;
		base.GetComponent<Renderer>().enabled = false;
		this.mechAnimation.gameObject.SetActive(true);
		this.shoulderPlatformTransform.parent = this.shouldersTransform;
		this.shoulderPlatformTransform.localPosition = Vector3.zero;
		this.actionState = ActionState.Idle;
		this.PlaySettleClip();
	}

	public virtual bool CanFire()
	{
		return base.transform.localScale.x == (float)this.facingDirection && this.turnFrame == 0;
	}

	protected virtual void SetSpriteTurn(int frame)
	{
		base.GetComponent<Renderer>().enabled = true;
		this.mechAnimation.gameObject.SetActive(false);
		this.shoulderPlatformTransform.parent = base.transform;
		this.sprite.SetLowerLeftPixel(new Vector2((float)(frame * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
	}

	protected virtual void RunStanding()
	{
		if (this.jetpacksOn && this.health > 0)
		{
			this.GetGroundHeight();
			float num = this.yI * this.t;
			if (this.yI < 0f && num + this.y <= this.groundHeight)
			{
				this.Land();
				num = this.groundHeight - this.y;
			}
			this.y += num;
			if (this.yI > 10f)
			{
				MapController.DamageGround(this, 10, DamageType.Crush, 17f, this.x - 16f, this.y + 32f, null);
				MapController.DamageGround(this, 10, DamageType.Crush, 17f, this.x + 16f, this.y + 32f, null);
			}
			this.SetPosition(0f);
		}
		else if (this.shakeTime > 0f)
		{
			this.shakeTime -= this.t;
			this.SetPosition(global::Math.Sin(this.shakeTime * 60f) * 1.5f);
			this.settled = false;
		}
		else
		{
			this.GetGroundHeight();
			if (this.y > this.groundHeight)
			{
				if (this.settled)
				{
					this.shakeTime = 0.3f;
					this.settled = false;
				}
				else
				{
					this.yI -= 800f * this.t;
					float num2 = this.yI * this.t;
					if (num2 + this.y <= this.groundHeight)
					{
						this.Land();
						num2 = this.groundHeight - this.y;
					}
					this.y += num2;
					if (this.y < -40f)
					{
						this.Death(0f, 0f, null);
					}
					this.RunHitUnits();
					if (this.yI > 10f)
					{
						MapController.DamageGround(this, 10, DamageType.Crush, 17f, this.x - 16f, this.y + 32f, null);
						MapController.DamageGround(this, 10, DamageType.Crush, 17f, this.x + 16f, this.y + 32f, null);
					}
				}
			}
			this.SetPosition(0f);
		}
	}

	protected virtual void Land()
	{
		this.y = this.groundHeight;
		if (this.yI < -210f)
		{
			this.DamageGroundBelow(true);
			this.ForceDamageGroundHack();
			this.HitUnits(0f, 0f);
			this.HitUnits(0f, -8f);
			this.HitUnits(0f, 8f);
			if (!this.jetpacksOn)
			{
				if (this.takesFallDamage)
				{
					this.Damage((this.yI >= -500f) ? ((this.yI >= -340f) ? 15 : 20) : 40, DamageType.Crush, 0f, 0f, 0, this, this.x, this.y);
				}
				SortOfFollow.Shake(0.3f, new Vector3(this.x, this.y, 0f));
			}
			else
			{
				UnityEngine.Debug.Log("big shaswke juimp ! ");
				SortOfFollow.Shake(1f);
			}
		}
		else
		{
			if (this.takesFallDamage)
			{
				this.Damage(5, DamageType.Crush, 0f, 0f, 0, this, this.x, this.y);
			}
			this.settled = true;
			if (this.mechAI != null)
			{
				this.mechAI.Land();
			}
			SortOfFollow.Shake(0.3f, new Vector3(this.x, this.y, 0f));
		}
		if (this.jetpacksOn)
		{
			this.stompCount++;
		}
		if (this.health > 0 && this.jetpacksOn && this.stompCount % this.doubleStompFrequency == 0)
		{
			this.mechAnimation.Play("JumpAgain", PlayMode.StopAll);
			this.shakeTime = 0f;
			UnityEngine.Debug.Log("Re juimp ! ");
			this.settled = false;
			this.mechAI.AddAction(EnemyActionType.Wait, 1.2f);
		}
		else if (!this.mechAnimation["JumpAgain"].enabled || this.mechAnimation["JumpAgain"].normalizedTime >= 1f)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Now Land ! ",
				this.mechAnimation["JumpAgain"].normalizedTime,
				" enabled ",
				this.mechAnimation["JumpAgain"].enabled
			}));
			this.mechAnimation.Play("Land", PlayMode.StopAll);
		}
		this.yI = 0f;
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.hitSounds, 0.7f, base.transform.position);
	}

	public override void SetTargetPlayerNum(int pN, Vector3 TargetPosition)
	{
		base.SetTargetPlayerNum(pN, TargetPosition);
		if (this.weapon != null)
		{
			this.weapon.SetTargetPlayerNum(pN, TargetPosition);
		}
	}

	protected virtual void GetGroundHeight()
	{
		this.groundHeight = -200f;
		this.leftGround = false;
		this.rightGround = false;
		this.midLeftGround = false;
		this.midRightGround = false;
		float num = Mathf.Round(this.x) + 0.5f;
		if (Physics.Raycast(new Vector3(num - 8f, this.y + 6f, 0f), Vector3.down, out this.raycastHitMidLeft, 64f, this.groundLayer))
		{
			if (this.raycastHitMidLeft.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitMidLeft.point.y;
			}
			if (this.raycastHitMidLeft.point.y > this.y - 9f)
			{
				this.midLeftGround = true;
			}
		}
		if (Physics.Raycast(new Vector3(num + 8f, this.y + 6f, 0f), Vector3.down, out this.raycastHitMidRight, 64f, this.groundLayer))
		{
			if (this.raycastHitMidRight.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitMidRight.point.y;
			}
			if (this.raycastHitMidRight.point.y > this.y - 9f)
			{
				this.midRightGround = true;
			}
		}
		if (Physics.Raycast(new Vector3(num + 24f, this.y + 6f, 0f), Vector3.down, out this.raycastHitRight, 64f, this.groundLayer))
		{
			if (this.raycastHitRight.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitRight.point.y;
			}
			if (this.raycastHitRight.point.y > this.y - 9f)
			{
				this.rightGround = true;
			}
		}
		if (Physics.Raycast(new Vector3(num - 24f, this.y + 6f, 0f), Vector3.down, out this.raycastHitLeft, 64f, this.groundLayer))
		{
			if (this.raycastHitLeft.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitLeft.point.y;
			}
			if (this.raycastHitLeft.point.y > this.y - 9f)
			{
				this.leftGround = true;
			}
		}
		if (!Map.isEditing)
		{
			this.CheckCrushGroundWhenStanding();
			if (this.crushGroundUnderArms)
			{
				if (Physics.Raycast(new Vector3(this.x, this.y + 6f, 0f), Vector3.right, out this.raycastHit, this.armDistance, this.groundLayer))
				{
					MapController.Damage_Local(this, this.raycastHit.collider.gameObject, 5, DamageType.Crush, 0f, 0f);
				}
				if (Physics.Raycast(new Vector3(this.x, this.y + 22f, 0f), Vector3.right, out this.raycastHit, this.armDistance, this.groundLayer))
				{
					MapController.Damage_Local(this, this.raycastHit.collider.gameObject, 5, DamageType.Crush, 0f, 0f);
				}
				if (Physics.Raycast(new Vector3(this.x, this.y + 6f, 0f), Vector3.left, out this.raycastHit, this.armDistance, this.groundLayer))
				{
					MapController.Damage_Local(this, this.raycastHit.collider.gameObject, 5, DamageType.Crush, 0f, 0f);
				}
				if (Physics.Raycast(new Vector3(this.x, this.y + 22f, 0f), Vector3.left, out this.raycastHit, this.armDistance, this.groundLayer))
				{
					MapController.Damage_Local(this, this.raycastHit.collider.gameObject, 5, DamageType.Crush, 0f, 0f);
				}
			}
		}
	}

	protected virtual void CheckCrushGroundWhenStanding()
	{
		if (this.yI > -200f)
		{
			int num = ((!this.leftGround) ? 1 : 0) + ((!this.midLeftGround) ? 1 : 0) + ((!this.midRightGround) ? 1 : 0) + ((!this.rightGround) ? 1 : 0);
			if (num >= 2 && num < 4)
			{
				this.DamageGroundBelow(true);
			}
		}
	}

	public void Stop()
	{
		this.actionState = ActionState.Idle;
		this.xI = 0f;
	}

	protected virtual void ForceDamageGroundHack()
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"ForceDamageGroundHack  y ",
			this.y,
			"  ground height ",
			this.groundHeight
		}));
		if (Physics.Raycast(new Vector3(this.x + 8f, this.y + 6f, 0f), Vector3.down, out this.raycastHit, 14f, this.groundLayer))
		{
			UnityEngine.Debug.Log("ForceDamageGroundHack success");
			MapController.Damage_Local(this, this.raycastHit.collider.gameObject, 7, DamageType.Crush, 0f, 0f);
		}
		if (Physics.Raycast(new Vector3(this.x - 8f, this.y + 6f, 0f), Vector3.down, out this.raycastHit, 14f, this.groundLayer))
		{
			UnityEngine.Debug.Log("ForceDamageGroundHack success");
			MapController.Damage_Local(this, this.raycastHit.collider.gameObject, 7, DamageType.Crush, 0f, 0f);
		}
		if (Physics.Raycast(new Vector3(this.x + 24f, this.y + 6f, 0f), Vector3.down, out this.raycastHit, 14f, this.groundLayer))
		{
			UnityEngine.Debug.Log("ForceDamageGroundHack success");
			MapController.Damage_Local(this, this.raycastHit.collider.gameObject, 7, DamageType.Crush, 0f, 0f);
		}
		if (Physics.Raycast(new Vector3(this.x - 24f, this.y + 6f, 0f), Vector3.down, out this.raycastHit, 14f, this.groundLayer))
		{
			UnityEngine.Debug.Log("ForceDamageGroundHack success");
			MapController.Damage_Local(this, this.raycastHit.collider.gameObject, 7, DamageType.Crush, 0f, 0f);
		}
	}

	protected virtual void DamageGroundBelow(bool forced)
	{
		if (this.delayDestroyTime > 0f)
		{
			return;
		}
		bool flag = this.yI < -150f && this.jetpacksOn;
		if ((!this.leftGround && !this.midLeftGround) || (!this.rightGround && !this.midRightGround))
		{
			forced = true;
		}
		if (this.leftGround || this.midLeftGround || this.midRightGround || this.rightGround)
		{
			this.yI = 0f;
		}
		if ((!this.midLeftGround || forced) && this.midRightGround && this.raycastHitMidRight.collider != null)
		{
			MapController.Damage_Local(this, this.raycastHitMidRight.collider.gameObject, 7, DamageType.Crush, 0f, 0f);
		}
		if ((!this.midRightGround || forced) && this.midLeftGround && this.raycastHitMidLeft.collider != null)
		{
			MapController.Damage_Local(this, this.raycastHitMidLeft.collider.gameObject, 7, DamageType.Crush, 0f, 0f);
		}
		if (forced)
		{
			if (this.leftGround && this.raycastHitLeft.collider != null)
			{
				MapController.Damage_Local(this, this.raycastHitLeft.collider.gameObject, 7, DamageType.Crush, 0f, 0f);
			}
			if (this.rightGround && this.raycastHitRight.collider != null)
			{
				MapController.Damage_Local(this, this.raycastHitRight.collider.gameObject, 7, DamageType.Crush, 0f, 0f);
			}
		}
		if (forced)
		{
			this.settled = false;
			this.shakeTime = 0f;
		}
		if (forced && flag)
		{
			if (this.mechAI != null)
			{
				this.mechAI.StopStomping();
			}
			ExplosionGroundWave explosionGroundWave = EffectsController.CreateHugeShockWave(this.x + 8f, this.y + 8f, 20f);
			explosionGroundWave.playerNum = -15;
			explosionGroundWave.avoidObject = this;
			explosionGroundWave.origins = this;
			explosionGroundWave.rightWave = true;
			explosionGroundWave.leftWave = false;
			explosionGroundWave = EffectsController.CreateHugeShockWave(this.x - 8f, this.y + 8f, 20f);
			explosionGroundWave.playerNum = -15;
			explosionGroundWave.avoidObject = this;
			explosionGroundWave.origins = this;
			explosionGroundWave.leftWave = true;
			explosionGroundWave.rightWave = false;
		}
	}

	protected virtual void SetPosition(float xOffset)
	{
		base.transform.position = new Vector3(this.x + xOffset, this.y, this.zOffset);
	}

	protected virtual void RunHitUnits()
	{
		if (this.yI < -70f)
		{
			this.HitUnits(0f, -8f);
		}
	}

	protected virtual void HitUnits(float xOffset, float yOffset)
	{
		this.invulnerable = true;
		if (Map.HitUnits(this, 20, DamageType.Crush, 18f, 2f, this.x, this.y, 0f, this.yI, true, false))
		{
		}
		this.invulnerable = false;
	}

	protected virtual void HitUnitsMooksOnly()
	{
		this.invulnerable = true;
		if (Map.HitUnits(this, this, 5, 20, DamageType.Crush, 21f, this.x, this.y - 8f, 0f, this.yI, true, false))
		{
		}
		this.invulnerable = false;
	}

	protected void RunCheckDeath()
	{
		if (!this.liveOnAfterWeaponDeath && this.weapon != null && this.weapon.health <= 0 && this.liveOnCounter < 1f)
		{
			this.liveOnCounter += this.t;
			if (this.liveOnCounter >= 0.6f)
			{
				this.Damage(this.health + 1, DamageType.Crush, 0f, 0f, 0, this, this.x, this.y);
			}
		}
	}

	protected void RunHurt()
	{
		if (this.hurtCounter > 0f)
		{
			this.hurtCounter -= this.t;
			if (this.hurtCounter <= 0f)
			{
				this.SetUnhurtMaterial();
			}
		}
	}

	protected virtual void SetUnhurtMaterial()
	{
		if (this.health > 0)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.baseMaterial;
			foreach (Renderer renderer in this.damageFlashRenderers)
			{
				renderer.sharedMaterial = this.damageFlashBaseMaterial;
			}
		}
	}

	protected virtual void SetHurtMaterial()
	{
		if (this.health > 0)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.hurtMaterial;
			this.hurtCounter = 0.0334f;
			foreach (Renderer renderer in this.damageFlashRenderers)
			{
				renderer.sharedMaterial = this.damageFlashHurtMaterial;
			}
		}
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		if (damageType == DamageType.Bounce)
		{
			damage = 0;
		}
		if (this.health > 0)
		{
			this.SetHurtMaterial();
			if (Time.time - this.lastDamageTime < 0.096f)
			{
				if (damage > 1 && damageType != DamageType.Fire && damageType != DamageType.Melee)
				{
					damage = 1;
				}
				if (damage > 3 && damageType == DamageType.Melee)
				{
					damage = 3;
				}
			}
			if (Time.time - this.lastDamageTime > 0.096f)
			{
				this.lastDamageTime = Time.time;
			}
			this.health -= damage;
			this.lastDamageType = damageType;
			if (this.health <= 0)
			{
				this.Death(xI, yI, new DamageObject(damage, damageType, xI, yI, damageSender));
			}
			if (this.health < this.maxHealth / 2)
			{
				this.tankSpeed = this.ultraTankSpeed;
				this.maxUpwardVelocity = this.ultraMaxUpwardVelocity;
			}
		}
	}

	protected virtual void SetDeathFrame()
	{
		UnityEngine.Debug.Log("Set Death FGRame");
	}

	public void InvokeDeath()
	{
		this.Death(0f, 0f, null);
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		if (this.health > 0)
		{
			this.health = 0;
		}
		this.actionState = ActionState.Dead;
		this.SetDeathFrame();
		this.MakeEffectsDeath(xI, yI);
		this.StopEngineSound();
		this.SwitchToDamagedMaterial();
		if (this.weapon != null)
		{
			this.weapon.DeathAtDelay(0.6f);
		}
		Map.RemoveUnit(this);
		this.NotifyDeath();
		base.Death(xI, yI, damage);
	}

	protected virtual void NotifyDeath()
	{
		UnityEngine.Debug.Log("No Notification");
	}

	protected virtual void MakeEffectsDeath(float xI, float yI)
	{
		EffectsController.CreateGlassShards(this.x, this.y + this.height / 2f, 6, this.width / 2f, this.height / 2f, 120f, 180f, 0f, 100f, 0f, 1f, 0.25f);
		EffectsController.CreateExplosion(this.x, this.y + this.height / 2f, this.width / 2f, this.height / 2f, 120f, 1f, 120f, 0.5f, 0.45f, false);
		MapController.BurnUnitsAround_NotNetworked(this, -1, 1, 64f, this.x, this.y, true, true);
		Map.ExplodeUnits(this, 12, DamageType.Explosion, 48f, 32f, this.x, this.y, 200f, 120f, 10, false, false);
		this.GiveHeroesDeathGrace();
	}

	protected virtual void GiveHeroesDeathGrace()
	{
		HeroController.SetHerosInvulnerable(this.x, this.y, 48f, 0.33f);
	}

	public override void Stun(float time)
	{
		this.Stop();
		if (this.weapon != null)
		{
			this.weapon.enabled = false;
			this.weapon.StopFiring();
		}
		this.stunTime = time;
		this.mechAI.Blind(this.stunTime + 0.1f);
		this.mechAI.ForgetPlayer();
	}

	public override void Blind(float time)
	{
		this.Stop();
		if (this.weapon != null)
		{
			this.weapon.enabled = false;
			this.weapon.StopFiring();
		}
		this.stunTime = time;
		this.mechAI.Blind(this.stunTime + 0.1f);
		this.mechAI.ForgetPlayer();
	}

	public override void Blind()
	{
		this.Stop();
		if (this.weapon != null)
		{
			this.weapon.enabled = false;
			this.weapon.StopFiring();
		}
		this.stunTime = 3f;
		this.mechAI.Blind(this.stunTime + 0.1f);
		this.mechAI.ForgetPlayer();
	}

	protected virtual void RunBlindStars()
	{
		this.blindCounter += this.t;
		if (this.blindCounter > 0.1f)
		{
			this.blindCounter -= 0.5f;
			EffectsController.CreateShrapnelBlindStar(this.x + UnityEngine.Random.value * 2f - 1f, this.y + 10f + this.height * 1.4f, 2f, 2f, 1f, 0f, 20f, base.transform);
		}
	}

	public MechWeapon weapon;

	public MechWeapon grenadeHatch;

	public MechWeapon chainGun1;

	public MechWeapon chainGun2;

	public Animation mechAnimation;

	protected SpriteSM sprite;

	public Renderer[] damageFlashRenderers;

	public Renderer[] damagedStateRenderers;

	public Material damageFlashBaseMaterial;

	public Material damageFlashHurtMaterial;

	public Material hurtMaterial;

	public Material damagedMaterial;

	protected Material baseMaterial;

	protected float hurtCounter;

	protected int frame;

	protected int turnFrame;

	protected float frameCounter;

	protected float t = 0.01f;

	public bool crushGroundUnderArms = true;

	public float armDistance = 48f;

	protected float groundHeight;

	protected RaycastHit raycastHitMidLeft;

	protected RaycastHit raycastHitMidRight;

	protected RaycastHit raycastHitLeft;

	protected RaycastHit raycastHitRight;

	protected RaycastHit raycastHit;

	protected bool leftGround;

	protected bool rightGround;

	protected bool midLeftGround;

	protected bool midRightGround;

	protected float delayDestroyTime = 0.1f;

	protected int stompCount;

	protected int doubleStompFrequency = 2;

	protected float testTurnCounter;

	[HideInInspector]
	public int facingDirection = -1;

	protected int spritePixelWidth = 128;

	protected float shakeTime;

	protected bool settled = true;

	protected LayerMask groundLayer;

	protected PolymorphicAI mechAI;

	public float tankSpeed = 80f;

	public float ultraTankSpeed = 120f;

	public SoundHolder soundHolder;

	protected bool left;

	protected bool right;

	protected bool up;

	protected bool down;

	protected bool jump;

	protected bool special;

	protected bool special2;

	protected bool special3;

	protected bool jetpacksOn;

	protected bool fire;

	public bool takesFallDamage;

	public float terminalVelocity = -600f;

	public float maxUpwardVelocity = 120f;

	public float ultraMaxUpwardVelocity = 150f;

	protected float jetpackFlamesCounter;

	public Transform shouldersTransform;

	public Transform shoulderPlatformTransform;

	public AudioSource engineAudio;

	public AudioClip engineRollingClip;

	public AudioClip engineTurningClip;

	public AudioClip engineSettleClip;

	protected bool notifiedDeath;

	protected float zOffset;

	public bool liveOnAfterWeaponDeath;

	protected float liveOnCounter;

	protected float stunTime;

	protected float blindCounter;

	protected float healthFlashCounter;

	protected float lastDamageTime;

	protected DamageType lastDamageType = DamageType.Normal;
}
