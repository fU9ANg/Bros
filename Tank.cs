// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Tank : Unit
{
	protected override void Awake()
	{
		base.Awake();
		this.sprite = base.GetComponent<SpriteSM>();
		if (this.sprite != null)
		{
			this.spritePixelWidth = (int)this.sprite.pixelDimensions.x;
		}
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
		this.tankAi = base.GetComponent<TankPolyAI>();
		this.zOffset = (4f + UnityEngine.Random.value * 1f) * 0.05f;
	}

	protected virtual void Start()
	{
		if (base.GetComponent<Renderer>() != null)
		{
			this.baseMaterial = base.GetComponent<Renderer>().sharedMaterial;
		}
		this.width = 16f;
		this.height = 8f;
		this.playerNum = -1;
		this.invulnerable = false;
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
		Map.RegisterUnit(this, true);
		this.GetGroundHeight();
		this.SetToGroundHeight();
		this.SetPosition(0f);
		this.testTurnCounter = UnityEngine.Random.value * 2f;
	}

	public override bool IsHeavy()
	{
		return true;
	}

	protected virtual void SetToGroundHeight()
	{
		this.y = this.groundHeight;
	}

	public override void SetTargetPlayerNum(int pN, Vector3 TargetPosition)
	{
		base.SetTargetPlayerNum(pN, TargetPosition);
		if (this.weapon != null)
		{
			this.weapon.SetTargetPlayerNum(pN, TargetPosition);
		}
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
			if (this.left && this.facingDirection > 0 && !this.special4)
			{
				this.StartTurnLeft();
			}
			else if (this.left && (this.facingDirection < 0 || this.special4) && this.IsMoveable())
			{
				if (this.actionState != ActionState.Rolling)
				{
					this.actionState = ActionState.Rolling;
					this.PlayRollingClip();
				}
				this.SetLeftSpeed();
			}
			if (this.right && this.facingDirection < 0 && !this.special4)
			{
				this.StartTurnRight();
			}
			else if (this.right && (this.facingDirection > 0 || this.special4) && this.IsMoveable())
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
			this.CheckAboveGround();
		}
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
		return this.actionState != ActionState.Turning && this.y < this.groundHeight + 1f;
	}

	protected virtual void CheckAboveGround()
	{
		if (this.y > this.groundHeight + 1f)
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
			if (!this.ConstrainToBlock(ref xIT, this.x - (float)(8 * direction), this.y + 22f, (direction <= 0) ? Vector3.left : Vector3.right, this.widthAgainstTerrain + this.damageExtraDistance, this.widthAgainstTerrain, false))
			{
				if (this.ConstrainToBlock(ref xIT, this.x - (float)(8 * direction), this.y + 8f, (direction <= 0) ? Vector3.left : Vector3.right, this.widthAgainstTerrain + this.damageExtraDistance, this.widthAgainstTerrain, true))
				{
				}
			}
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
		this.tankAi.GetInput(ref this.left, ref this.right, ref this.up, ref this.down, ref this.jump, ref this.fire, ref this.special, ref this.special2, ref this.special3, ref this.special4);
		if (this.fire)
		{
			this.FireWeapon();
		}
		if (this.special)
		{
			this.UseSpecial();
		}
	}

	protected virtual void UseSpecial()
	{
		this.weapon.UseSpecial();
	}

	protected virtual void FireWeapon()
	{
		this.weapon.Fire();
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
			this.sprite.SetLowerLeftPixel(new Vector2((float)(this.frame % 3 * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
		}
	}

	protected virtual void AnimateIdle()
	{
		this.frameCounter += this.t;
		if (this.frameCounter > 0.0334f)
		{
			this.frameCounter -= 0.0334f;
			this.sprite.SetLowerLeftPixel(new Vector2(0f, this.sprite.lowerLeftPixel.y));
		}
	}

	protected virtual void AnimateTurning()
	{
		if (this.facingDirection < 0 && base.transform.localScale.x < 0f)
		{
			this.frameCounter += this.t;
			if (this.frameCounter > 0.067f)
			{
				this.frameCounter -= 0.067f;
				this.turnFrame++;
				if (this.turnFrame > 4)
				{
					this.turnFrame = 3;
					base.transform.localScale = new Vector3(1f, 1f, 1f);
				}
				this.SetSpriteTurn(this.turnFrame);
			}
		}
		else if (this.facingDirection > 0 && base.transform.localScale.x > 0f)
		{
			this.frameCounter += this.t;
			if (this.frameCounter > 0.067f)
			{
				this.frameCounter -= 0.067f;
				this.turnFrame++;
				if (this.turnFrame > 4)
				{
					this.turnFrame = 3;
					base.transform.localScale = new Vector3(-1f, 1f, 1f);
				}
				this.SetSpriteTurn(this.turnFrame);
			}
		}
		else if (this.turnFrame > 0)
		{
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

	public virtual bool CanFire()
	{
		return base.transform.localScale.x == (float)(this.facingDirection * -1) && this.turnFrame == 0;
	}

	protected virtual void SetSpriteTurn(int frame)
	{
		if (this.weapon != null)
		{
			this.weapon.SetSpriteTurn(frame);
		}
		this.sprite.SetLowerLeftPixel(new Vector2((float)((3 + frame) * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
	}

	protected virtual void RunStanding()
	{
		if (this.shakeTime > 0f)
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
					float num = this.yI * this.t;
					if (num + this.y <= this.groundHeight)
					{
						this.Land();
						num = this.groundHeight - this.y;
					}
					this.y += num;
					if (this.y < -40f && !this.isDead)
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
			this.Damage((this.yI >= -500f) ? ((this.yI >= -340f) ? 15 : 40) : 90, DamageType.Crush, 0f, 0f, 0, this, this.x, this.y);
		}
		else
		{
			this.Damage(5, DamageType.Crush, 0f, 0f, 0, this, this.x, this.y);
			this.settled = true;
			if (this.tankAi != null)
			{
				this.tankAi.Land();
			}
		}
		this.yI = 0f;
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.hitSounds, 0.7f, base.transform.position);
		SortOfFollow.Shake(0.3f, new Vector3(this.x, this.y, 0f));
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
		}
	}

	protected virtual void CheckCrushGroundWhenStanding()
	{
		if (((!this.leftGround) ? 1 : 0) + ((!this.midLeftGround) ? 1 : 0) + ((!this.midRightGround) ? 1 : 0) + ((!this.rightGround) ? 1 : 0) >= 2)
		{
			this.DamageGroundBelow(true);
		}
	}

	public void Stop()
	{
		this.actionState = ActionState.Idle;
		this.xI = 0f;
	}

	protected virtual void DamageGroundBelow(bool forced)
	{
		if (this.delayDestroyTime > 0f)
		{
			return;
		}
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
			MapController.Damage_Local(this, this.raycastHitMidRight.collider.gameObject, 5, DamageType.Crush, 0f, 0f);
		}
		if ((!this.midRightGround || forced) && this.midLeftGround && this.raycastHitMidLeft.collider != null)
		{
			MapController.Damage_Local(this, this.raycastHitMidLeft.collider.gameObject, 5, DamageType.Crush, 0f, 0f);
		}
		if (forced)
		{
			if (this.leftGround && this.raycastHitLeft.collider != null)
			{
				MapController.Damage_Local(this, this.raycastHitLeft.collider.gameObject, 5, DamageType.Crush, 0f, 0f);
			}
			if (this.rightGround && this.raycastHitRight.collider != null)
			{
				MapController.Damage_Local(this, this.raycastHitRight.collider.gameObject, 5, DamageType.Crush, 0f, 0f);
			}
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
			this.HitUnits();
		}
	}

	protected virtual void HitUnits()
	{
		this.invulnerable = true;
		if (Map.HitUnits(this, 20, DamageType.Crush, 18f, 2f, this.x, this.y - 8f, 0f, this.yI, true, false))
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
		base.GetComponent<Renderer>().sharedMaterial = this.baseMaterial;
	}

	protected virtual void SetHurtMaterial()
	{
		base.GetComponent<Renderer>().sharedMaterial = this.hurtMaterial;
		this.hurtCounter = 0.0334f;
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
		}
	}

	protected virtual void SetDeathFrame()
	{
		this.sprite.SetLowerLeftPixel((float)(this.spritePixelWidth * 8), (float)((int)this.sprite.lowerLeftPixel.y));
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
		if (this.weapon != null)
		{
			this.weapon.DeathAtDelay(0.6f);
		}
		Map.RemoveUnit(this);
		this.NotifyDeath();
		this.isDead = true;
		base.Death(xI, yI, damage);
	}

	protected virtual void NotifyDeath()
	{
		if (!this.notifiedDeath)
		{
			this.notifiedDeath = true;
			Map.EnemyDeathEvent(this);
			StatisticsController.NotifyTankDeath(this);
		}
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
		this.tankAi.Blind(this.stunTime + 0.1f);
		this.tankAi.ForgetPlayer();
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
		this.tankAi.Blind(this.stunTime + 0.1f);
		this.tankAi.ForgetPlayer();
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
		this.tankAi.Blind(this.stunTime + 0.1f);
		this.tankAi.ForgetPlayer();
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

	public TankWeapon weapon;

	protected SpriteSM sprite;

	public Material hurtMaterial;

	protected Material baseMaterial;

	protected float hurtCounter;

	protected int frame;

	protected int turnFrame;

	protected float frameCounter;

	protected float t = 0.01f;

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

	protected float delayDestroyTime = 4f;

	protected float testTurnCounter;

	[HideInInspector]
	public int facingDirection = -1;

	protected int spritePixelWidth = 64;

	protected float shakeTime;

	protected bool settled = true;

	protected LayerMask groundLayer;

	protected TankPolyAI tankAi;

	public float tankSpeed = 80f;

	public SoundHolder soundHolder;

	protected bool left;

	protected bool right;

	protected bool up;

	protected bool down;

	protected bool jump;

	protected bool special;

	protected bool special2;

	protected bool special3;

	protected bool special4;

	protected bool fire;

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

	public float damageExtraDistance = 18f;

	public float widthAgainstTerrain = 32f;

	protected bool isDead;

	protected float lastDamageTime;

	protected DamageType lastDamageType = DamageType.Normal;
}
