// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Projectile : BroforceObject
{
	public virtual float GetSuggestedSpeed()
	{
		return 120f;
	}

	public virtual void SetDamage(int d)
	{
		this.damageInternal = d;
		this.damage = d;
	}

	public virtual void Fire(float x, float y, float xI, float yI, int playerNum, MonoBehaviour FiredBy)
	{
		this.t = Time.deltaTime;
		this.damageInternal = this.damage;
		this.fullLife = this.life;
		this.fullDamage = this.damage;
		this.x = x;
		this.y = y;
		this.xI = xI;
		this.yI = yI;
		this.playerNum = playerNum;
		this.SetPosition();
		this.SetRotation();
		this.firedBy = FiredBy;
		Vector3 vector = new Vector3(xI, yI, 0f);
		this.startProjectileSpeed = vector.magnitude;
		this.CheckSpawnPoint();
	}

	public virtual void SetSpeed(float xI, float yI)
	{
		this.xI = xI;
		this.yI = yI;
		this.SetRotation();
	}

	protected virtual void CheckSpawnPoint()
	{
		Collider[] array = Physics.OverlapSphere(new Vector3(this.x, this.y, 0f), 5f, this.groundLayer);
		if (array.Length > 0)
		{
			this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
			for (int i = 0; i < array.Length; i++)
			{
				this.ProjectileApplyDamageToBlock(array[i].gameObject, this.damageInternal, this.damageType, this.xI, this.yI);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			this.RegisterProjectile();
		}
		this.CheckReturnZones();
		if ((this.canReflect && this.playerNum >= 0 && this.horizontalProjectile && Physics.Raycast(new Vector3(this.x - Mathf.Sign(this.xI) * this.projectileSize * 2f, this.y, 0f), new Vector3(this.xI, this.yI, 0f), out this.raycastHit, this.projectileSize * 3f, this.barrierLayer)) || (!this.horizontalProjectile && Physics.Raycast(new Vector3(this.x, this.y, 0f), new Vector3(this.xI, this.yI, 0f), out this.raycastHit, this.projectileSize + this.startProjectileSpeed * this.t, this.barrierLayer)))
		{
			this.ReflectProjectile(this.raycastHit);
		}
		else
		{
			this.TryHitUnitsAtSpawn();
		}
		this.CheckSpawnPointFragile();
	}

	protected virtual void TryHitUnitsAtSpawn()
	{
		if (Map.HitUnits(this.firedBy, this.firedBy, this.playerNum, this.damageInternal * 2, this.damageType, 0f + ((this.playerNum < 0) ? 0f : (this.projectileSize * 0.5f)), this.x - ((this.playerNum < 0) ? 0f : (this.projectileSize * 0.5f)) * (float)((int)Mathf.Sign(this.xI)), this.y, this.xI, this.yI, false, false))
		{
			this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	protected virtual void RegisterProjectile()
	{
		Map.RegisterProjectile(this);
	}

	protected virtual void DeregisterProjectile()
	{
		Map.RemoveProjectile(this);
	}

	protected virtual void CheckSpawnPointFragile()
	{
		Collider[] array = Physics.OverlapSphere(new Vector3(this.x, this.y, 0f), 5f, this.fragileLayer);
		for (int i = 0; i < array.Length; i++)
		{
			EffectsController.CreateProjectilePuff(this.x, this.y);
			this.ProjectileApplyDamageToBlock(array[i].gameObject, this.damageInternal, this.damageType, this.xI, this.yI);
		}
	}

	protected override void OnDestroy()
	{
		this.DeregisterProjectile();
		base.OnDestroy();
	}

	protected virtual void SetRotation()
	{
		if (this.xI > 0f)
		{
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
			base.transform.eulerAngles = new Vector3(0f, 0f, global::Math.GetAngle(this.yI, -this.xI) * 180f / 3.14159274f + 90f);
		}
		else
		{
			base.transform.localScale = new Vector3(1f, 1f, 1f);
			base.transform.eulerAngles = new Vector3(0f, 0f, global::Math.GetAngle(this.yI, -this.xI) * 180f / 3.14159274f - 90f);
		}
	}

	public virtual void SetPosition()
	{
		base.transform.position = new Vector3(Mathf.Round(this.x), Mathf.Round(this.y), this.z + this.zOffset);
	}

	protected virtual void Awake()
	{
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
		this.barrierLayer = 1 << LayerMask.NameToLayer("MobileBarriers");
		this.fragileLayer = 1 << LayerMask.NameToLayer("DirtyHippie");
		this.zOffset = (1f - UnityEngine.Random.value * 2f) * 0.04f;
	}

	public void SetSeed(int newSeed)
	{
		this.seed = newSeed;
		this.random = new Randomf(this.seed);
	}

	protected virtual void Start()
	{
		this.random = new Randomf(UnityEngine.Random.Range(0, 10000));
		this.damageBackgroundCounter = -this.random.value * 0.1f;
	}

	protected virtual void MakeEffects(bool particles, float x, float y, bool useRayCast, Vector3 hitNormal, Vector3 hitPoint)
	{
		if (this.hasMadeEffects)
		{
			return;
		}
		if (useRayCast)
		{
			if (particles)
			{
				EffectsController.CreateSparkShower(hitPoint.x + hitNormal.x * 3f, hitPoint.y + hitNormal.y * 3f, this.sparkCount, 2f, 60f, hitNormal.x * 60f, hitNormal.y * 30f, 0.2f, 0f);
			}
			EffectsController.CreateProjectilePopEffect(hitPoint.x + hitNormal.x * 3f, hitPoint.y + hitNormal.y * 3f);
		}
		else
		{
			if (particles)
			{
				EffectsController.CreateSparkShower(x, y, 10, 2f, 60f, -this.xI * 0.2f, 35f, 0.2f, 0f);
			}
			EffectsController.CreateProjectilePopEffect(x, y);
		}
		if (!particles)
		{
			Map.DamageDoodads(this.damageInternal, x, y, this.xI, this.yI, 8f, this.playerNum);
		}
		this.hasMadeEffects = true;
	}

	protected virtual void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		this.RunProjectile(this.t);
		this.RunLife();
	}

	protected virtual void RunLife()
	{
		this.life -= this.t;
		if (this.life <= 0f)
		{
			this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
			if (base.Syncronize && base.IsMine)
			{
				Networking.RPC<bool, float, float, bool, Vector3, Vector3>(PID.TargetOthers, new RpcSignature<bool, float, float, bool, Vector3, Vector3>(this.MakeEffects), false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point, false);
			}
			this.DeregisterProjectile();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	protected virtual void RunProjectile(float t)
	{
		this.RunDamageBackground(t);
		this.MoveProjectile();
		if (this.fadeDamage)
		{
			this.damageInternal = (int)Mathf.Clamp(Mathf.Floor(0.01f + (float)this.fullDamage * this.life / this.fullLife), 1f, 10000f);
		}
		this.SetPosition();
		if (this.reversing)
		{
			this.RunReversing();
		}
		if (this.HitWalls())
		{
			this.CheckReturnZones();
			if (!this.IsHeldByZone())
			{
				this.HitUnits();
			}
			this.HitWildLife();
			this.HitGrenades();
			this.HitFragile();
			this.HitProjectiles();
		}
	}

	protected virtual void RunReversing()
	{
		if (this.heldDelay > 0f)
		{
			this.heldDelay -= this.t;
		}
		if (this.IsHeldByZone())
		{
			this.xI = Mathf.Lerp(this.xI, 0f, this.t * 12f);
			this.yI = Mathf.Lerp(this.yI, 0f, this.t * 12f);
			this.life += this.t * 1.5f;
		}
		else
		{
			this.xI = Mathf.Lerp(this.xI, this.reverseXI, this.t * 9f);
			this.yI = Mathf.Lerp(this.yI, this.reverseYI, this.t * 9f);
			this.life += this.t * 0.5f;
		}
	}

	protected virtual void HitFragile()
	{
		if (Physics.Raycast(new Vector3(this.x - Mathf.Sign(this.xI) * this.projectileSize, this.y, 0f), new Vector3(this.xI, this.yI, 0f), out this.raycastHit, this.projectileSize * 2f, this.fragileLayer))
		{
			EffectsController.CreateProjectilePuff(this.raycastHit.point.x, this.raycastHit.point.y);
			this.ProjectileApplyDamageToBlock(this.raycastHit.collider.gameObject, this.damageInternal, this.damageType, this.xI, this.yI);
			if (this.raycastHit.collider.GetComponent<DoorDoodad>() != null)
			{
				this.Bounce(this.raycastHit);
			}
		}
	}

	protected virtual void RunDamageBackground(float t)
	{
		if (!this.damagedBackground)
		{
			this.damageBackgroundCounter += t;
			if (this.damageBackgroundCounter > 0f)
			{
				this.damageBackgroundCounter -= t * 2f;
				if (Map.DamageDoodads(this.damageInternal, this.x, this.y, this.xI, this.yI, this.projectileSize, this.playerNum))
				{
					this.damagedBackground = true;
					EffectsController.CreateEffect(this.flickPuff, this.x, this.y);
				}
				if (Map.PassThroughScenery(this.x, this.y, this.xI, this.yI))
				{
					this.damageBackgroundCounter -= 0.33f;
				}
			}
		}
	}

	protected virtual void MoveProjectile()
	{
		this.x += this.xI * this.t;
		this.y += this.yI * this.t;
	}

	protected virtual void HitWildLife()
	{
		Map.HurtWildLife(this.x, this.y, this.projectileSize / 2f);
	}

	protected virtual void HitGrenades()
	{
		if (this.canHitGrenades)
		{
			float num = 0f;
			float num2 = 0f;
			if (Map.HitGrenades(this.playerNum, this.projectileSize, this.x, this.y, this.xI, this.yI, ref num, ref num2))
			{
				this.MakeEffects(false, this.x, this.y, false, Vector3.zero, Vector3.zero);
				this.MakeSparkShower((this.x + num) / 2f, (this.y + num2) / 2f, Mathf.Sign(this.xI) * -125f, this.yI);
				this.Death();
			}
		}
	}

	protected virtual void MakeSparkShower(float xPos, float yPos, float showerXI, float showerYI)
	{
		EffectsController.CreateSuddenSparkShower(xPos, yPos, this.sparkCount / 2, 2f, 40f + UnityEngine.Random.value * 80f, showerXI * (0.7f + UnityEngine.Random.value * 0.5f), showerYI + (-60f + UnityEngine.Random.value * 350f), 0.2f);
	}

	protected virtual bool CheckReturnZones()
	{
		if (this.zone == null && ProjectileController.CheckReturnZone(this.x, this.y, ref this.playerNum, ref this.zone))
		{
			this.ReverseProjectile();
			return true;
		}
		return false;
	}

	protected virtual bool IsHeldByZone()
	{
		return (this.reversing && this.zone != null && this.zone.life > 0f) || this.heldDelay > 0f;
	}

	protected virtual void ReverseProjectile()
	{
		this.heldDelay = UnityEngine.Random.value * 0.333f;
		this.reverseXI = this.xI * -1f;
		this.reverseYI = this.yI * -1f;
		this.xI *= 0.7f;
		this.yI *= 0.7f;
		this.life += 0.6f;
		this.damageInternal += 12 + this.damageInternal;
		this.damage += 12;
		this.reversing = true;
	}

	public virtual void IncreaseLife(float m)
	{
		this.life += m;
		this.fullLife += m;
	}

	protected virtual bool HitWalls()
	{
		if ((this.canReflect && this.playerNum >= 0 && this.horizontalProjectile && Physics.Raycast(new Vector3(this.x - Mathf.Sign(this.xI) * this.projectileSize * 2f, this.y, 0f), new Vector3(this.xI, this.yI, 0f), out this.raycastHit, this.projectileSize * 3f, this.barrierLayer)) || (!this.horizontalProjectile && Physics.Raycast(new Vector3(this.x, this.y, 0f), new Vector3(this.xI, this.yI, 0f), out this.raycastHit, this.projectileSize + this.startProjectileSpeed * this.t, this.barrierLayer)))
		{
			return this.ReflectProjectile(this.raycastHit);
		}
		if (this.horizontalProjectile)
		{
			this.HitHorizontalWalls();
		}
		else
		{
			Vector3 a = new Vector3(this.xI, this.yI, 0f);
			float magnitude = a.magnitude;
			Vector3 vector = a / magnitude;
			if (Physics.Raycast(new Vector3(this.x - vector.x * this.projectileSize, this.y - vector.y * this.projectileSize, 0f), new Vector3(this.xI, this.yI, 0f), out this.raycastHit, this.projectileSize * 2f + magnitude * this.t, this.groundLayer) && this.raycastHit.distance < this.projectileSize + magnitude)
			{
				this.Bounce(this.raycastHit);
			}
		}
		return true;
	}

	protected virtual void HitHorizontalWalls()
	{
		float num = Mathf.Abs(this.xI) * this.t;
		if (!this.isWideProjectile)
		{
			if (Physics.Raycast(new Vector3(this.x - Mathf.Sign(this.xI) * this.projectileSize, this.y, 0f), new Vector3(this.xI, this.yI, 0f), out this.raycastHit, this.projectileSize * 2f + num, this.groundLayer) && this.raycastHit.distance < this.projectileSize + num)
			{
				this.Bounce(this.raycastHit);
			}
		}
		else
		{
			if (Physics.Raycast(new Vector3(this.x - Mathf.Sign(this.xI) * this.projectileSize, this.y + this.projectileSize * 0.5f, 0f), new Vector3(this.xI, this.yI, 0f), out this.raycastHit, this.projectileSize * 2f + num, this.groundLayer) && this.raycastHit.distance < this.projectileSize + num)
			{
				this.Bounce(this.raycastHit);
			}
			if (Physics.Raycast(new Vector3(this.x - Mathf.Sign(this.xI) * this.projectileSize, this.y - this.projectileSize * 0.5f, 0f), new Vector3(this.xI, this.yI, 0f), out this.raycastHit, this.projectileSize * 2f + num, this.groundLayer) && this.raycastHit.distance < this.projectileSize + num)
			{
				this.Bounce(this.raycastHit);
			}
		}
	}

	protected virtual void Bounce(RaycastHit raycastHit)
	{
		this.MakeEffects(true, raycastHit.point.x + raycastHit.normal.x * 3f, raycastHit.point.y + raycastHit.normal.y * 3f, true, raycastHit.normal, raycastHit.point);
		this.ProjectileApplyDamageToBlock(raycastHit.collider.gameObject, this.damageInternal, this.damageType, this.xI, this.yI);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	protected virtual bool ReflectProjectile(RaycastHit raycastHit)
	{
		if (this.playerNum < 0)
		{
			if (!(raycastHit.collider.GetComponent<Unit>() != null) || raycastHit.collider.GetComponent<Unit>().playerNum < 0)
			{
				return false;
			}
			this.playerNum = 5;
		}
		this.MakeEffects(false, raycastHit.point.x + raycastHit.normal.x * 3f, raycastHit.point.y + raycastHit.normal.y * 3f, true, raycastHit.normal, raycastHit.point);
		if (raycastHit.collider.gameObject.CompareTag("Metal") || raycastHit.collider.gameObject.CompareTag("Boss"))
		{
			EffectsController.CreateSuddenSparkShower(raycastHit.point.x + raycastHit.normal.x * 3f, raycastHit.point.y + raycastHit.normal.y * 3f, this.sparkCount / 2, 2f, 40f + UnityEngine.Random.value * 80f, raycastHit.normal.x * (100f + UnityEngine.Random.value * 210f), raycastHit.normal.y * 120f + (-60f + UnityEngine.Random.value * 350f), 0.2f);
		}
		Unit component = raycastHit.collider.GetComponent<Unit>();
		Map.KnockAndDamageUnit(this.firedBy, component, 0, this.damageType, this.xI * 0.05f, 15f, (int)Mathf.Sign(this.xI), true);
		if (raycastHit.normal.x > 0.3f)
		{
			this.xI = Mathf.Abs(this.xI) * 0.7f;
			this.yI = UnityEngine.Random.Range(-this.xI * 0.2f, this.xI * 0.4f);
		}
		if (raycastHit.normal.x < -0.3f)
		{
			this.xI = -Mathf.Abs(this.xI) * 0.7f;
			this.yI = UnityEngine.Random.Range(this.xI * 0.2f, -this.xI * 0.4f);
		}
		if (raycastHit.normal.y > 0.3f)
		{
			this.yI = Mathf.Abs(this.yI) * 0.7f;
			this.xI *= 1.3f;
		}
		if (raycastHit.normal.y < -0.3f)
		{
			this.yI = -Mathf.Abs(this.yI) * 0.7f;
			this.xI *= 1.3f;
		}
		this.MoveProjectile();
		this.SetRotation();
		return true;
	}

	protected virtual void ProjectileApplyDamageToBlock(GameObject blockObject, int damage, DamageType type, float forceX, float forceY)
	{
		MapController.Damage_Networked(this.firedBy, blockObject, damage, type, forceX, forceY);
	}

	protected virtual void HitUnits()
	{
		if (this.reversing)
		{
			if (Map.HitLivingUnits(this, this.playerNum, this.damageInternal, this.damageType, this.projectileSize, this.projectileSize / 2f, this.x, this.y, this.xI, this.yI, false, false))
			{
				this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		else if (Map.HitUnits(this.firedBy, this.firedBy, this.playerNum, this.damageInternal, this.damageType, this.projectileSize, this.projectileSize / 2f, this.x, this.y, this.xI, this.yI, false, false, true))
		{
			this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	protected virtual void HitProjectiles()
	{
		if (Map.HitProjectiles(this.playerNum, this.damageInternal, this.damageType, this.projectileSize, this.x, this.y, this.xI, this.yI, 0.1f))
		{
			this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public virtual void Damage(int damage, DamageType damageType, float xI, float yI, float damageDelay, int newPlayerNum)
	{
	}

	public virtual void Death()
	{
		UnityEngine.Object.Destroy(base.gameObject);
		this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
	}

	protected virtual void PlayDeathSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.deathSounds, this.soundVolume, base.transform.position);
		}
	}

	public virtual void AvoidRect(Transform avoidTransform, float avoidWidth, float avoidHeight)
	{
	}

	public virtual void Target(float targetX, float targetY, int playerNum)
	{
	}

	[Syncronize]
	public Vector2 Position
	{
		get
		{
			return new Vector2(this.x, this.y);
		}
		set
		{
			this.x = value.x;
			this.y = value.y;
			base.transform.position = new Vector3(value.x, value.y, base.transform.position.z);
		}
	}

	//Interpolate = false; [Syncronize(Interpolate )]
    [Syncronize(Interpolate = false)]
	public Vector2 Velocity
	{
		get
		{
			return new Vector2(this.xI, this.yI);
		}
		set
		{
			this.xI = value.x;
			this.yI = value.y;
			this.SetRotation();
		}
	}

	[HideInInspector]
	public float z;

	protected float t = 0.011f;

	[HideInInspector]
	protected LayerMask groundLayer;

	protected LayerMask barrierLayer;

	protected LayerMask fragileLayer;

	protected RaycastHit raycastHit;

	public Shrapnel shrapnel;

	public Shrapnel shrapnelSpark;

	public FlickerFader flickPuff;

	public float life = 4f;

	public float projectileSize = 8f;

	protected bool damagedBackground;

	protected float damageBackgroundCounter;

	public int damage = 1;

	[HideInInspector]
	public int damageInternal = 1;

	protected float fullLife = 1f;

	protected int fullDamage = 1;

	public bool fadeDamage;

	public DamageType damageType;

	[HideInInspector]
	public int playerNum = -1;

	public SoundHolder soundHolder;

	public bool canHitGrenades = true;

	public float soundVolume = 0.2f;

	public MonoBehaviour firedBy;

	public int seed;

	protected Randomf random;

	public int sparkCount = 10;

	public bool isDamageable;

	public bool horizontalProjectile = true;

	public bool isWideProjectile;

	[HideInInspector]
	public float zOffset;

	protected bool reversing;

	protected ProjectileReturnZone zone;

	protected float reverseYI;

	protected float reverseXI;

	public bool canReflect = true;

	protected float startProjectileSpeed = 400f;

	protected float heldDelay;

	protected bool hasMadeEffects;

	protected Sound sound;
}
