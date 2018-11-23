// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Unit : NetworkedUnit
{
	public virtual bool IsParachuteActive
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.enemyAI = base.GetComponent<PolymorphicAI>();
		this.enemyAIOnChildOrParent = this.GetComponentRecursive<PolymorphicAI>();
	}

	protected T GetComponentRecursive<T>() where T : Component
	{
		Transform transform = base.transform;
		while (transform.parent != null)
		{
			transform = transform.parent;
		}
		return transform.GetComponentInChildren<T>();
	}

	public virtual bool IsOnGround()
	{
		return true;
	}

	public virtual void HeadShot(int damage, DamageType damageType, float xI, float yI, int direction, float xHit, float yHit, MonoBehaviour damageSender)
	{
		this.Damage(damage, damageType, xI, yI, direction, damageSender, xHit, yHit);
	}

	public virtual void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
	}

	public virtual void ReduceDeathTimer(int playerNum, float newTime)
	{
	}

	public virtual void Knock(DamageType damageType, float xI, float yI, bool forceTumble)
	{
	}

	public virtual void SetVelocity(DamageType damageType, float xI, float xIBlast, float yIBlast)
	{
	}

	public virtual void KnockSimple(DamageObject damageObject)
	{
	}

	public virtual void BurnInternal(int damage, int direction)
	{
	}

	public virtual void Impale(Transform impaleTransform, int damage, float xI, float yI, float xOffset)
	{
	}

	public virtual void Unimpale(int damage, DamageType damageType, float xI, float yI)
	{
		this.Damage(damage, damageType, xI * 0.8f, yI * 0.8f, (int)Mathf.Sign(xI), this, this.x, this.y + 8f);
	}

	public virtual void Panic(bool forgetPlayer)
	{
	}

	public virtual void Panic(float time, bool forgetPlayer)
	{
	}

	public virtual void Panic(int direction, float time, bool forgetPlayer)
	{
	}

	public virtual void Terrify()
	{
	}

	public virtual void OpenParachute()
	{
	}

	public virtual void SetCanParachute()
	{
		this.SetCanParachute(true);
	}

	public virtual void SetCanParachute(bool canParachute)
	{
	}

	public virtual void CheckDestroyed()
	{
	}

	public virtual void Blind(float time)
	{
	}

	public virtual void Stun(float time)
	{
	}

	public virtual void Blind()
	{
	}

	public virtual void Attract(float xTarget, float yTarget)
	{
	}

	public virtual void Alert(float alertX, float alertY)
	{
	}

	public virtual void FullyAlert(float x, float y, int playerNum)
	{
	}

	public virtual void HearSound(float alertX, float alertY)
	{
	}

	public virtual void ForgetPlayer(int deadPlayerNum)
	{
	}

	public virtual void FetchObject(Transform fetchObject)
	{
	}

	public virtual void Death(float xI, float yI, DamageObject damage)
	{
		if (!this.deathNotificationSent)
		{
			if (base.IsMine || !base.IsHero)
			{
				Networking.RPC<float, float, float, float>(PID.TargetOthers, new RpcSignature<float, float, float, float>(this.DeathRPC), xI, yI, this.x, this.y, false);
			}
			this.deathNotificationSent = true;
		}
		this.actionState = ActionState.Dead;
	}

	public void DeathRPC(float xI, float yI, float _x, float _y)
	{
		if (this.actionState != ActionState.Dead)
		{
			this.x = _x;
			this.y = _y;
			this.deathNotificationSent = true;
			this.Death(xI, yI, null);
		}
	}

	public virtual void CreateGibEffects(DamageType damageType, float xI, float yI)
	{
	}

	public virtual void AttachToZipline(ZipLine zipLine)
	{
		this.attachedToZipline = zipLine;
	}

	public virtual bool IsHighFiving()
	{
		return false;
	}

	public virtual MookType GetMookType()
	{
		return MookType.None;
	}

	public virtual bool IsEvil()
	{
		return false;
	}

	public virtual bool IsHeavy()
	{
		return false;
	}

	public virtual void RollOnto(int direction)
	{
	}

	public virtual void RunWarning(float t, float explosionTime)
	{
	}

	protected virtual void SetDeathType(DamageType damageType, int health)
	{
		if (health < -10)
		{
			if (damageType != DamageType.Explosion)
			{
				this.deathType = DeathType.Gibbed;
			}
			else
			{
				this.deathType = DeathType.Explode;
			}
		}
		else
		{
			switch (damageType)
			{
			case DamageType.Fire:
				this.deathType = DeathType.Fire;
				return;
			case DamageType.Explosion:
				this.deathType = DeathType.Bullet;
				return;
			case DamageType.Crush:
			case DamageType.OutOfBounds:
			case DamageType.InstaGib:
				this.deathType = DeathType.Gibbed;
				return;
			case DamageType.Melee:
			case DamageType.Knifed:
			case DamageType.Bite:
				this.deathType = DeathType.Knife;
				return;
			case DamageType.Fall:
				this.deathType = DeathType.Fall;
				return;
			case DamageType.Knock:
				this.deathType = DeathType.Fall;
				return;
			}
			this.deathType = DeathType.Bullet;
		}
	}

	public virtual bool Activate()
	{
		UnityEngine.Debug.Log("ACTIVATE!!!! " + base.name);
		return true;
	}

	public virtual bool IsPressingDown()
	{
		return false;
	}

	public virtual bool CanPilotUnit(int newPlayerNum)
	{
		return false;
	}

	public virtual void PilotUnit(Unit pilotUnit)
	{
		MonoBehaviour.print("pilotUnit " + pilotUnit);
		if (pilotUnit.IsMine)
		{
			Networking.RPC<Unit>(PID.TargetAll, new RpcSignature<Unit>(this.PilotUnitRPC), pilotUnit, false);
		}
	}

	public virtual void PilotUnitRPC(Unit pilotUnit)
	{
	}

	public virtual void DischargePilotingUnit(float x, float y, float xI, float yI)
	{
	}

	public virtual void StartPilotingUnit(Unit pilottedUnit)
	{
	}

	public virtual Unit GetPilottedUnit()
	{
		return null;
	}

	public virtual float GetFuel()
	{
		return 0f;
	}

	public virtual bool GetFuelWarning()
	{
		return false;
	}

	public virtual bool Revive(int playerNum, bool isUnderPlayerControl, TestVanDammeAnim reviveSource)
	{
		return false;
	}

	public virtual void ReviveRPC(int playerNum, bool isUnderPlayerControl, TestVanDammeAnim reviveSource)
	{
		this.Revive(playerNum, isUnderPlayerControl, reviveSource);
	}

	public virtual void DestroyCharacter()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public virtual void SetTargetPlayerNum(int pN, Vector3 TargetPosition)
	{
	}

	public float width = 8f;

	public float height = 8f;

	public int row;

	public int collumn;

	[HideInInspector]
	public float burnTime;

	protected float burnCounter;

	protected int burnDamage;

	protected int plasmaDamage;

	protected float plasmaCounter;

	public float hearingRangeX = 300f;

	public float hearingRangeY = 200f;

	public bool canHear = true;

	public bool destroyed;

	public BloodColor bloodColor;

	[HideInInspector]
	public bool deathNotificationSent;

	public ActionState actionState;

	protected DeathType deathType;

	protected float deathTime;

	protected bool hasNotifiedDeathType;

	[HideInInspector]
	public bool beingControlledByTriggerAction;

	[HideInInspector]
	public CharacterAction controllingTriggerAction;

	public bool invulnerable;

	protected bool isZombie;

	public Projectile projectile;

	public ZipLine attachedToZipline;

	public bool slidingOnZipline;
}
