// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Animal : TestVanDammeAnim
{
	protected override void Awake()
	{
		base.gameObject.AddComponent<DisableWhenOffCamera>();
		base.Awake();
		this.spawnTime = Time.time;
	}

	protected override void RunFalling()
	{
		base.RunFalling();
		if (this.fatAnimal)
		{
			this.invulnerable = true;
			if (Map.HitLivingUnits(this, 5, 3, DamageType.Crush, this.squashRange, this.x, this.y + 2f, base.transform.localScale.x * 100f, 30f, false, true))
			{
				this.PlaySpecialAttackSound(0.25f);
				this.yI = 160f;
			}
			this.invulnerable = false;
		}
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

	protected override void FallDamage(float yI)
	{
		if (yI < -350f)
		{
			Map.KnockAndDamageUnit(SingletonMono<MapController>.Instance, this, (this.health <= 0) ? 0 : 5, DamageType.Fall, -1f, 450f, 0, false);
		}
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		if (!this.hasDied)
		{
			this.hasDied = true;
			if (this.isRotten)
			{
				this.rottenCounter = 0.7f;
			}
		}
		base.Death(xI, yI, damage);
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		if (damageType == DamageType.Fire && this.health > 0)
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
		if (this.health > 0)
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
	}

	protected override void UseFire()
	{
	}

	protected virtual bool PanicAI()
	{
		return this.PanicAI(this.enemyAI.walkDirection, false);
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
		if (this.health <= 0 || this.blindTime > 0f || this.PanicAI())
		{
		}
		this.blindTime = Mathf.Max(this.blindTime, 0.1f);
	}

	public override void Panic(float time, bool forgetPlayer)
	{
		if (this.health <= 0 || this.blindTime > 0f || this.PanicAI())
		{
		}
		this.blindTime = Mathf.Max(this.blindTime, time);
	}

	public override void Panic(int direction, float time, bool forgetPlayer)
	{
		if (this.health <= 0 || this.blindTime > 0f || this.PanicAI((direction != 0) ? ((int)Mathf.Sign((float)direction)) : 0, true))
		{
		}
		this.blindTime = Mathf.Max(this.blindTime, time);
	}

	public override void Terrify()
	{
		this.scaredTime = 1f;
	}

	public override void Blind(float time)
	{
		this.blindTime = time;
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
		this.enemyAI.Attract(xTarget, yTarget);
	}

	public override void HearSound(float alertX, float alertY)
	{
		this.enemyAI.HearSound(alertX, alertY);
	}

	public override void Alert(float alertX, float alertY)
	{
		this.Panic((int)Mathf.Sign(this.x - alertX), 0.9f, true);
	}

	protected override void Update()
	{
		base.Update();
		if (Map.isEditing)
		{
			return;
		}
		if (this.burnTime > 0f)
		{
			this.burnCounter += this.t;
			if (this.burnCounter > 0.04f)
			{
				this.burnCounter -= 0.13f;
				EffectsController.CreateEffect(this.flames[UnityEngine.Random.Range(0, this.flames.Length)], this.x, this.y + this.height, base.transform.position.z - 1f);
			}
			this.fireSpreadCounter += this.t;
			if (this.fireSpreadCounter > this.fireSpreadRate)
			{
				this.fireSpreadCounter -= this.fireSpreadRate;
				this.BurnOthers();
			}
			this.RunBurning();
		}
		if (this.rottenCounter > 0f)
		{
			this.rottenCounter -= this.t;
			if (this.rottenCounter <= 0f)
			{
				MapController.DamageGround(this, 5, DamageType.Explosion, 16f, this.x, this.y + 3f, null);
				this.Gib(DamageType.Explosion, this.xI, this.yI + 100f);
				EffectsController.CreateExplosion(this.x, this.y, 8f, 8f, 60f, 0f, 10f, 0.4f, 0.4f, false);
				return;
			}
		}
		if (this.blindTime > 0f && this.health > 0)
		{
			this.RunBlindStars();
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
	}

	protected override void RunBlindStars()
	{
		this.blindCounter += this.t;
		if (this.blindCounter > 0.1f)
		{
			this.blindCounter -= 0.5f;
			EffectsController.CreateShrapnelBlindStar(this.x + UnityEngine.Random.value * 2f - 1f, this.y + 4f + this.height * 1.3f, 2f, 2f, 1f, 0f, 20f, base.transform);
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
		this.enemyAI.StopPanicking();
		this.actionState = ActionState.Idle;
		this.ChangeFrame();
		this.Stop();
		this.firingPlayerNum = -1;
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
		this.enemyAI.TryForgetPlayer(deadPlayerNum);
	}

	protected override void AnimateRunning()
	{
		base.AnimateRunning();
	}

	protected override void AnimateJumping()
	{
		this.AnimateIdle();
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
					MapController.Damage_Networked(this, component2.gameObject, 0, DamageType.Fire, 0f, 0f);
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
					MapController.Damage_Networked(this, component4.gameObject, 0, DamageType.Fire, 0f, 0f);
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

	protected override void Gib(DamageType damageType, float xI, float yI)
	{
		if (!this.hasDied)
		{
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
		}
	}

	protected override void SetGunPosition(float xOffset, float yOffset)
	{
	}

	protected override void AnimateIdle()
	{
		this.SetSpriteOffset(0f, 0f);
		this.frameRate = 0.066f;
		int num = 25 + this.frame % 6;
		if (this.enemyAI.GetThinkState() == MentalState.Idle)
		{
			this.frameRate = 0.066f;
		}
		else
		{
			this.frameRate = 0.12f;
		}
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)this.spritePixelHeight);
	}

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

	public bool fatAnimal = true;

	public float squashRange = 7f;

	public bool isRotten;

	protected float rottenCounter;

	[HideInInspector]
	public int firingPlayerNum = -1;

	protected float spawnTime;

	public FlickerFader[] flames;
}
