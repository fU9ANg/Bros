// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class DoodadDestroyable : Doodad
{
	public bool Falling
	{
		get
		{
			return this.falling;
		}
	}

	protected override void Start()
	{
		base.Start();
		this.sprite = base.GetComponent<SpriteSM>();
		this.spritePixelWidth = (int)this.sprite.pixelDimensions.x;
		if (this.collidersPristine != null)
		{
			this.collidersPristine.SetActive(true);
		}
		if (this.collidersDamaged != null)
		{
			this.collidersDamaged.SetActive(false);
		}
		if (this.collidersDestroyed != null)
		{
			this.collidersDestroyed.SetActive(false);
		}
	}

	protected override void AttachDoodad()
	{
		if (this.attachToSides)
		{
			if (Physics.Raycast(base.transform.position, Vector3.left, out this.groundHit, 14f, this.groundLayer))
			{
				this.groundHit.collider.gameObject.SendMessage("AttachMe", this);
			}
			if (Physics.Raycast(base.transform.position, Vector3.right, out this.groundHit, 14f, this.groundLayer))
			{
				this.groundHit.collider.gameObject.SendMessage("AttachMe", this);
			}
		}
		else
		{
			base.AttachDoodad();
		}
		if (this.attachToTopAsWell && Physics.Raycast(base.transform.position + Vector3.up * 4f, Vector3.up, out this.groundHit, 32f, this.groundLayer))
		{
			this.groundHit.collider.gameObject.SendMessage("AttachMe", this);
		}
	}

	protected virtual void Update()
	{
		if (Map.isEditing)
		{
			return;
		}
		if (this.damageDelay > 0f)
		{
			this.damageDelay -= Time.deltaTime;
		}
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		float position = 0f;
		if (this.shakeTime > 0f)
		{
			this.shakeTime -= num;
			position = global::Math.Sin(this.shakeTime * 60f) * 1.5f;
		}
		this.GetGroundHeight();
		if (this.falling && !Map.isEditing && !this.waitForDamageToFall)
		{
			if (this.shakeTime > 0f)
			{
				this.settled = false;
			}
			else if (this.y > this.groundHeight + 8f)
			{
				if (this.settled)
				{
					this.shakeTime = 0.3f;
					this.settled = false;
				}
				else
				{
					this.yI -= 800f * num;
					float num2 = this.yI * num;
					if (num2 + this.y <= this.groundHeight + 8f)
					{
						this.y = this.groundHeight + 8f;
						if (this.yI < -40f && !this.isDead && this.takeDamageOnFall)
						{
							this.Death();
						}
						this.yI = 0f;
						this.Land();
						this.settled = true;
						this.falling = false;
					}
					else
					{
						this.y += num2;
					}
					this.HitUnits();
				}
			}
		}
		float num3 = this.xI * num;
		this.x += num3;
		this.SetPosition(position);
	}

	private void FixedUpdate()
	{
		this.RunBurnLogic();
	}

	private void Land()
	{
		if (this.shootPlasmaBoltsOnGroundImpact)
		{
			this.shootPlasmaBoltsOnGroundImpact = false;
			ProjectileController.SpawnProjectileLocally(this.plasmaProjectile, this, base.centerX + this.width / 2f + 2f, base.centerY, 300f, 0f, 1);
			ProjectileController.SpawnProjectileLocally(this.plasmaProjectile, this, base.centerX - this.width / 2f - 2f, base.centerY, -300f, 0f, 1);
			ProjectileController.SpawnProjectileLocally(this.plasmaProjectile, this, base.centerX + this.width / 2f + 2f, base.centerY, 200f, 0f, 1);
			ProjectileController.SpawnProjectileLocally(this.plasmaProjectile, this, base.centerX - this.width / 2f - 2f, base.centerY, -200f, 0f, 1);
			ProjectileController.SpawnProjectileLocally(this.plasmaProjectile, this, base.centerX + this.width / 2f + 2f, base.centerY, 100f, 0f, 1);
			ProjectileController.SpawnProjectileLocally(this.plasmaProjectile, this, base.centerX - this.width / 2f - 2f, base.centerY, -100f, 0f, 1);
			if (this.createSparksOnDeath)
			{
				EffectsController.CreateSparkParticles(base.centerX, base.centerY, 30, this.width / 2f, 1f, 0f, 50f, 0.25f, 0.3f);
			}
		}
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.hitSounds, 0.7f, base.transform.position);
		SortOfFollow.Shake(this.screenShakeAmountOnFall, new Vector3(this.x, this.y, 0f));
	}

	protected virtual void HitUnits()
	{
		if (this.crushUnitsAndTerrain && ((this.dontCrushOnceDestroyed && !this.isDead) || !this.dontCrushOnceDestroyed))
		{
			if (Map.HitUnits(this, this, 15, 18, DamageType.Crush, this.width / 2f, this.height / 2f, base.centerX, base.centerY, 0f, this.yI, true, false, true))
			{
				this.Bloody();
			}
			else if (Map.HitUnits(this, this, -1, 24, DamageType.Crush, this.width / 2f, this.height / 2f, base.centerX, base.centerY, 0f, this.yI, true, false, true))
			{
				this.Bloody();
			}
		}
	}

	public override void Bloody()
	{
		base.Bloody();
	}

	protected virtual void SetPosition(float xOffset)
	{
		base.transform.position = new Vector3(this.x + xOffset, this.y, 0f);
	}

	protected virtual void GetGroundHeight()
	{
		if (this.widthInBlocks > 0)
		{
			this.groundHeight = -200f;
			List<RaycastHit> list = new List<RaycastHit>();
			Vector3 a = base.transform.position + this.bottomLeftOffset;
			Vector3 origin = Vector3.zero;
			for (int i = 0; i < this.widthInBlocks; i++)
			{
				origin = a + Vector3.right * (float)i * 16f;
				if (Extensions.Raycast(origin, Vector3.down, out this.raycastHit, 64f, this.groundLayer, false, Color.magenta, 10f))
				{
					if (this.raycastHit.point.y > this.groundHeight)
					{
						this.groundHeight = this.raycastHit.point.y;
					}
					if (this.raycastHit.point.y > this.y - 9f)
					{
						list.Add(this.raycastHit);
					}
				}
			}
			float num = (float)list.Count / (float)this.widthInBlocks;
			if (num <= 0.5f)
			{
				foreach (RaycastHit raycastHit in list)
				{
					raycastHit.collider.gameObject.SendMessage("Damage", new DamageObject(5, DamageType.Crush, 0f, 0f, null));
				}
			}
			if (num == 0f)
			{
				this.StartFalling();
			}
		}
	}

	private void TryCollapseAbove()
	{
		if (!this.CollapseDoodadsAboveWhenDestroyed)
		{
			return;
		}
		List<RaycastHit> list = new List<RaycastHit>();
		Vector3 a = base.transform.position + this.topLeftOffset;
		Vector3 origin = Vector3.zero;
		for (int i = 0; i < this.widthInBlocks; i++)
		{
			origin = a + Vector3.right * (float)i * 16f;
			if (Extensions.Raycast(origin, Vector3.up, out this.raycastHit, 32f, this.groundLayer, true, Color.magenta, 0f) && this.raycastHit.distance < 9f)
			{
				Doodad component = this.raycastHit.collider.GetComponent<Doodad>();
				if (component != null)
				{
					component.Collapse();
				}
				FallingBlock component2 = this.raycastHit.collider.GetComponent<FallingBlock>();
				if (component2 != null)
				{
					component2.DisturbNetworked();
				}
			}
		}
	}

	private void StartFalling()
	{
		if (this.canFall)
		{
			this.falling = true;
			this.TryCollapseAbove();
		}
	}

	public override void Collapse()
	{
		if (this.canFall)
		{
			this.StartFalling();
		}
		else
		{
			if (!this.isDamaged && !this.isDead)
			{
				this.Damage(new DamageObject(this.health + 1, DamageType.Crush, 0f, 0f, null));
			}
			this.isDead = true;
			base.Collapse();
		}
	}

	protected virtual void OnCollisionEnter(Collision collisio)
	{
	}

	public override bool DamageOptional(DamageObject damageObject, ref bool showBulletHit)
	{
		this.waitForDamageToFall = false;
		showBulletHit = true;
		return this.Damage(damageObject);
	}

	public override bool Damage(DamageObject damageObject)
	{
		if (!this.acceptDamageCalls)
		{
			return false;
		}
		this.waitForDamageToFall = false;
		this.lastDamageObject = damageObject;
		if (this.damageDelay <= 0f && !this.isDead)
		{
			if (damageObject.damageType == DamageType.Fire && this.flamable)
			{
				this.Burn(damageObject);
			}
			this.health -= damageObject.damage;
			for (int i = 0; i < this.prinstineChildren.Length; i++)
			{
				this.prinstineChildren[i].SetActive(false);
			}
			if (this.health <= 0)
			{
				if (!this.isDead)
				{
					this.Death();
					return true;
				}
			}
			else if (!this.isDamaged)
			{
				this.Hurt();
				this.damageDelay = 0.33f;
			}
			if (this.fallOnReceivingAnyDamage)
			{
				this.StartFalling();
			}
		}
		return false;
	}

	protected virtual void Hurt()
	{
		if (this.changeSpriteToShowDamage)
		{
			this.sprite.SetLowerLeftPixel((float)(this.spritePixelWidth * 1), (float)((int)this.sprite.lowerLeftPixel.y));
		}
		this.MakeEffectsDamaged();
		if (this.collidersPristine != null)
		{
			this.collidersPristine.SetActive(false);
		}
		if (this.collidersDamaged != null)
		{
			this.collidersDamaged.SetActive(true);
		}
		if (this.collidersDestroyed != null)
		{
			this.collidersDestroyed.SetActive(false);
		}
		this.isDamaged = true;
	}

	public override void Death()
	{
		if (!this.isDead)
		{
			this.isDead = true;
			Map.RemoveDestroyableDoodad(this);
			if (this.collidersPristine != null)
			{
				this.collidersPristine.SetActive(false);
			}
			if (this.collidersDamaged != null)
			{
				this.collidersDamaged.SetActive(false);
			}
			if (this.collidersDestroyed != null)
			{
				this.collidersDestroyed.SetActive(true);
			}
			if (base.GetComponent<Collider>() != null)
			{
				base.GetComponent<Collider>().enabled = false;
			}
			this.burnTime = Mathf.Min(this.burnTime, 0.4f);
			if (this.canFall)
			{
				this.Collapse();
			}
			this.MakeEffectsDeath();
			this.TryCollapseAbove();
			if (!this.destroyOnDeath)
			{
				this.sprite.SetLowerLeftPixel((float)(this.spritePixelWidth * 2), (float)((int)this.sprite.lowerLeftPixel.y));
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	protected virtual void MakeEffectsDamaged()
	{
		float num = 0f + ((this.lastDamageObject == null) ? 0f : this.lastDamageObject.xForce);
		float num2 = 0f + ((this.lastDamageObject == null) ? 0f : this.lastDamageObject.yForce);
		if (this.createGlassFragmentsOnDeath)
		{
			EffectsController.CreateGlassShards(base.centerX, base.centerY, 20, this.width / 2f, this.height / 2f, 120f, 90f, 0f + num * 0.3f, 60f + num2 * 0.3f, 0.5f, 1f, 0.55f);
		}
	}

	protected virtual void MakeEffectsDeath()
	{
		if (base.GetComponent<Collider>() != null)
		{
			base.GetComponent<Collider>().enabled = false;
		}
		float num = 0f + ((this.lastDamageObject == null) ? 0f : this.lastDamageObject.xForce);
		float num2 = 0f + ((this.lastDamageObject == null) ? 0f : this.lastDamageObject.yForce);
		if (this.alwaysDropGibs)
		{
			this.DropGibs();
		}
		if (this.willExplode)
		{
			if (this.createGlassFragmentsOnDeath)
			{
				EffectsController.CreateGlassShards(base.centerX, base.centerY, 14, this.width / 2f, this.height / 2f, 120f, 180f, 0f, 100f, 0f, 1f, 0.25f);
			}
			this.CreateExplosion();
		}
		else
		{
			if (this.createSparksOnDeath)
			{
				EffectsController.CreateSparkParticles(base.centerX, base.centerY, 30, this.width / 2f, 1f, 0f, 50f, 0.25f, 0.3f);
			}
			if (this.createGlassFragmentsOnDeath)
			{
				EffectsController.CreateGlassShards(base.centerX, base.centerY, 24, this.width / 2f, this.height / 2f, 120f, 80f, 0f + num * 0.3f, 50f + num2 * 0.3f, 0.5f, 15f, 0.25f);
			}
			Map.DisturbWildLife(base.centerX, base.centerY, 60f, 5);
		}
	}

	protected virtual void CreateExplosion()
	{
		EffectsController.CreateExplosion(base.centerX, base.centerY, this.width / 2f, this.height / 2f, 120f, 1f, 120f, 0.5f, 0.45f, false);
		MapController.BurnUnitsAround_NotNetworked(this, -1, 1, 64f, this.x, this.y, true, true);
		Map.ExplodeUnits(this, 12, DamageType.Explosion, 48f, 32f, this.x, this.y, 200f, 300f, 10, false, false);
		Map.DisturbWildLife(base.centerX, base.centerY, 130f, 5);
	}

	private void RunBurnLogic()
	{
		if (this.flamable && this.burnTime > 0f)
		{
			this.burnTime -= Time.fixedDeltaTime;
			if (this.burnTime < 0f)
			{
				this.Damage(new DamageObject(this.burnDamage, DamageType.Fire, 0f, 0f, this));
			}
			else
			{
				this.burnCounter += Time.fixedDeltaTime;
				this.burnUnitsCounter += Time.fixedDeltaTime;
				this.flameCounter += Mathf.Clamp(Time.fixedDeltaTime, 0f, 0.0334f);
				if (this.burnUnitsCounter > 0.25f)
				{
					this.burnUnitsCounter -= 0.25f;
					Map.BurnUnitsAround_Local(this, 5, 1, this.width, base.centerX, base.centerY, true, false);
				}
				if (this.burnCounter > 1f)
				{
					this.burnCounter -= 3f;
					for (int i = this.collumn; i < this.collumn + this.widthInBlocks; i++)
					{
						for (int j = this.row; j < this.row + this.heightInBlocks; j++)
						{
							Map.BurnBlocksAround(1, i, j, false);
						}
					}
					if (this.burnDamage > this.BurnCollapsePoint() - 2)
					{
						this.burnTime = 0f;
						this.Damage(new DamageObject(this.burnDamage, DamageType.Fire, 0f, 0f, this));
					}
				}
				if (this.flameCounter > 0.05f)
				{
					this.flameCounter -= 0.04f + Time.deltaTime;
					Vector3 direction = UnityEngine.Random.insideUnitCircle * 6f;
					this.CreateFlames(direction);
				}
			}
		}
		else if (this.heatable && this.burnTime > 0f)
		{
			this.burnTime -= Time.fixedDeltaTime;
			if (this.burnTime < 0f)
			{
				this.burnDamage -= Map.BurnBlocksAround(1, this.collumn, this.row, false);
				this.burnDamage--;
			}
		}
		else if (this.burnTime > 0f)
		{
			this.burnTime -= Time.fixedDeltaTime;
			this.burnUnitsCounter += Time.fixedDeltaTime;
			this.burnCounter += Time.fixedDeltaTime;
			this.flameCounter += Time.fixedDeltaTime;
			if (this.burnUnitsCounter > 0.25f)
			{
				this.burnUnitsCounter -= 0.25f;
				Map.BurnUnitsAround_Local(this, 5, 1, 12f, base.transform.position.x, base.transform.position.y + 8f, true, false);
			}
			if (this.burnCounter > 1f)
			{
				this.burnCounter -= 3f;
				for (int k = this.collumn; k < this.collumn + this.widthInBlocks; k++)
				{
					for (int l = this.row; l < this.row + this.heightInBlocks; l++)
					{
						Map.BurnBlocksAround(1, k, l, false);
					}
				}
				if (this.burnDamage > this.BurnCollapsePoint() - 2)
				{
					this.burnTime = 0f;
					this.Damage(new DamageObject(this.burnDamage, DamageType.Fire, 0f, 0f, this));
				}
			}
			if (this.flameCounter > 0.05f)
			{
				this.flameCounter -= 0.05f;
				Vector3 direction2 = UnityEngine.Random.insideUnitCircle * 6f;
				this.CreateFlames(direction2);
			}
		}
	}

	private void CreateFlames(Vector3 direction)
	{
		Vector3 b = this.bottomLeftOffset + base.transform.position;
		for (int i = 0; i < this.widthInBlocks; i++)
		{
			for (int j = 0; j < this.heightInBlocks; j++)
			{
				Vector3 vector = new Vector3((float)i, (float)j, 0f) * 16f + b;
				vector.x += direction.x;
				vector.y += 8f + direction.y * 1.5f;
				EffectsController.CreateFlameEffect(vector.x, vector.y, UnityEngine.Random.value * 0.033f, direction);
			}
		}
	}

	protected int BurnCollapsePoint()
	{
		return this.health * 3;
	}

	public virtual void Burn(DamageObject damgeObject)
	{
		if (!this.isDead)
		{
			if (this.flamable)
			{
				if (this.isDead || this.burnTime == 0f)
				{
				}
				this.burnTime = 8f;
				this.burnDamage += damgeObject.damage;
				if (this.burnDamage > this.BurnCollapsePoint() - 2)
				{
					this.burnTime = 0f;
					this.Damage(new DamageObject(this.burnDamage, DamageType.Fire, 0f, 0f, this));
				}
			}
			else if (this.heatable)
			{
				this.burnTime += 0.5f;
				this.burnDamage++;
			}
			else if (this.explosive)
			{
				this.Damage(new DamageObject(this.burnDamage, DamageType.Fire, 0f, 0f, this));
			}
		}
	}

	public virtual void SetAlight_Local()
	{
		if (!this.isDead && this.flamable)
		{
			this.burnTime = 8f;
			if (this.burnDamage <= 0)
			{
				this.burnDamage = 1;
			}
		}
	}

	protected SpriteSM sprite;

	protected int spritePixelWidth = 64;

	public bool changeSpriteToShowDamage = true;

	protected float shakeTime;

	protected bool settled = true;

	public bool createSparksOnDeath;

	public bool createGlassFragmentsOnDeath = true;

	public GameObject collidersPristine;

	public GameObject collidersDamaged;

	public GameObject collidersDestroyed;

	public GameObject[] prinstineChildren;

	public bool attachToTopAsWell;

	public bool attachToSides;

	protected bool isDamaged;

	protected bool isDead;

	public bool destroyOnDeath;

	public bool waitForDamageToFall;

	public bool falling;

	public bool canFall = true;

	public bool willExplode = true;

	public bool takeDamageOnFall = true;

	public bool crushUnitsAndTerrain;

	public bool dontCrushOnceDestroyed;

	public bool fallOnReceivingAnyDamage;

	public bool shootPlasmaBoltsOnGroundImpact;

	public float screenShakeAmountOnFall = 0.3f;

	public Projectile plasmaProjectile;

	protected float groundHeight;

	protected RaycastHit raycastHit;

	protected RaycastHit raycastHitLeft;

	protected RaycastHit raycastHitRight;

	public Vector3 bottomLeftOffset;

	public Vector3 topLeftOffset;

	public int widthInBlocks = 1;

	public int heightInBlocks = 1;

	public bool CollapseDoodadsAboveWhenDestroyed;

	public bool acceptDamageCalls = true;

	public bool flamable;

	public bool heatable;

	public bool explosive;

	public int burnDamage;

	public float burnTime;

	public float burnCounter;

	public float burnUnitsCounter;

	public float flameCounter;
}
