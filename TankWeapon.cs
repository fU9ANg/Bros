// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TankWeapon : Unit
{
	protected override void Awake()
	{
		base.Awake();
		this.sprite = base.GetComponent<SpriteSM>();
		if (base.transform.parent != null)
		{
			this.tank = base.GetComponentRecursive<Tank>();
		}
		else
		{
			UnityEngine.Debug.LogError("Tank Weapon has no tank");
		}
		this.spritePixelWidth = (int)this.sprite.pixelDimensions.x;
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
	}

	protected virtual void Start()
	{
		this.baseMaterial = base.GetComponent<Renderer>().sharedMaterial;
		this.width = 10f;
		this.height = 14f;
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
		this.playerNum = -1;
		this.invulnerable = false;
		Map.RegisterUnit(this, false);
	}

	protected virtual void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
		this.RunHurt();
		if (this.health > 0 && this.deathDelayTime > 0f)
		{
			this.deathDelayTime -= this.t;
			if (this.deathDelayTime <= 0f)
			{
				this.Death(0f, 0f, null);
			}
		}
		this.RunFiring();
	}

	public virtual void StopFiring()
	{
		this.fireCounter = 0f;
		this.fire = false;
	}

	public virtual void Fire()
	{
		if (!this.wasFire)
		{
			this.fireCounter = 0f;
		}
		this.fire = true;
	}

	public virtual void UseSpecial()
	{
	}

	public virtual bool IsFiring()
	{
		return this.fire;
	}

	public new virtual void SetTargetPlayerNum(int pN, Vector3 TargetPosition)
	{
	}

	protected virtual void RunFiring()
	{
		this.fireDelay -= this.t;
		if (this.fire && this.health > 0 && this.tank.CanFire() && this.fireDelay <= 0f)
		{
			this.fireCounter += this.t;
			if (this.fireCounter > 0f)
			{
				this.fireCounter -= this.fireRate;
				this.FireWeapon(ref this.fireIndex);
			}
		}
		this.wasFire = this.fire;
	}

	protected virtual void FireWeapon(ref int index)
	{
		index++;
		if (index >= 4)
		{
			this.fire = false;
			this.fireDelay = 2f;
			index = 0;
		}
	}

	protected void RunHurt()
	{
		if (this.hurtCounter > 0f)
		{
			this.hurtCounter -= this.t;
			if (this.hurtCounter <= 0f)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.baseMaterial;
			}
		}
	}

	public void ConstrainToWalls(float xI, ref float xIT)
	{
		if (xI < 0f)
		{
			if (Physics.Raycast(new Vector3(this.x + 8f, this.y + 32f, 0f), Vector3.left, out this.raycastHit, this.physicalWidth + 16f, this.groundLayer))
			{
				if (this.raycastHit.point.x > this.x + xIT - this.physicalWidth)
				{
					xIT = this.raycastHit.point.x - this.x + this.physicalWidth;
				}
			}
			else if (Physics.Raycast(new Vector3(this.x + 8f, this.y + 16f, 0f), Vector3.left, out this.raycastHit, this.physicalWidth + 16f, this.groundLayer) && this.raycastHit.point.x > this.x + xIT - this.physicalWidth)
			{
				xIT = this.raycastHit.point.x - this.x + this.physicalWidth;
			}
		}
		if (xI > 0f)
		{
			if (Physics.Raycast(new Vector3(this.x - 8f, this.y + 32f, 0f), Vector3.right, out this.raycastHit, this.physicalWidth + 16f, this.groundLayer))
			{
				if (this.raycastHit.point.x < this.x + xIT + this.physicalWidth)
				{
					xIT = this.raycastHit.point.x - this.x - this.physicalWidth;
				}
			}
			else if (Physics.Raycast(new Vector3(this.x - 8f, this.y + 16f, 0f), Vector3.right, out this.raycastHit, this.physicalWidth + 16f, this.groundLayer) && this.raycastHit.point.x < this.x + xIT + this.physicalWidth)
			{
				xIT = this.raycastHit.point.x - this.x - this.physicalWidth;
			}
		}
	}

	public virtual void SetSpriteTurn(int frame)
	{
		if (this.health > 0)
		{
			this.fire = false;
			this.currentTurnFrame = frame;
			this.sprite.SetLowerLeftPixel(new Vector2((float)((0 + frame) * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
		}
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		if (this.health > 0)
		{
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
			base.GetComponent<Renderer>().sharedMaterial = this.hurtMaterial;
			this.hurtCounter = 0.0334f;
			this.health -= damage;
			if (this.health <= 0)
			{
				this.Death(xI, yI, new DamageObject(damage, damageType, xI, yI, damageSender));
			}
		}
	}

	public virtual void DeathAtDelay(float delay)
	{
		this.deathDelayTime = delay;
	}

	protected virtual void SetDeathFrame()
	{
		this.sprite.SetLowerLeftPixel((float)(this.spritePixelWidth * 5), (float)((int)this.sprite.lowerLeftPixel.y));
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		this.health = 0;
		this.SetDeathFrame();
		this.MakeEffectsDeath(xI, yI);
		Map.RemoveUnit(this);
		base.Death(xI, yI, damage);
	}

	protected virtual void MakeEffectsDeath(float xI, float yI)
	{
		EffectsController.CreateGlassShards(this.x, this.y + this.height / 2f, 6, this.width / 2f, this.height / 2f, 120f, 180f, 0f, 100f, 0f, 1f, 0.25f);
		EffectsController.CreateExplosion(this.x, this.y + this.height / 2f, this.width / 2f, this.height / 2f, 120f, 1f, 120f, 0.5f, 0.45f, false);
		MapController.BurnUnitsAround_NotNetworked(this, -1, 1, 64f, this.x, this.y, true, true);
		Map.ExplodeUnits(this, 12, DamageType.Explosion, 32f, 16f, this.x, this.y + 7f, 200f, 300f, 10, false, false);
	}

	public Material hurtMaterial;

	protected Material baseMaterial;

	protected float hurtCounter;

	protected int frame;

	protected float t = 0.01f;

	protected SpriteSM sprite;

	protected float deathDelayTime;

	protected float frameCounter;

	protected int spritePixelWidth = 64;

	protected int currentTurnFrame;

	protected bool fire;

	protected bool wasFire;

	public float fireRate = 0.5f;

	protected float fireCounter;

	protected int fireIndex;

	protected float fireDelay;

	protected RaycastHit raycastHit;

	protected float physicalWidth = 20f;

	protected LayerMask groundLayer;

	public SoundHolder soundHolder;

	protected Tank tank;

	protected float lastDamageTime;
}
