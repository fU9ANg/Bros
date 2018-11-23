// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Shrapnel
{
	public bool FiredLocally
	{
		get
		{
			bool? flag = this.firedLocally;
			if (flag == null)
			{
				NetworkObject networkObject = this.firedBy as NetworkObject;
				if (networkObject != null)
				{
					this.firedLocally = new bool?(networkObject.IsMine);
				}
				else
				{
					this.firedLocally = new bool?(base.IsMine);
				}
			}
			bool? flag2 = this.firedLocally;
			return flag2.Value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.fragileLayer = 1 << LayerMask.NameToLayer("DirtyHippie");
		if (this.trailRenderer == null)
		{
		}
		if (!this.bounceOffEnemiesMultiple)
		{
			this.alreadyBouncedOffUnits = new List<Unit>();
		}
	}

	protected override void Start()
	{
		this.mainMaterial = base.GetComponent<Renderer>().sharedMaterial;
		base.Start();
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
		if (this.shootable)
		{
			Map.RegisterShootableGrenade(this);
		}
		Map.RegisterGrenade(this);
		if (this.disabledAtStart)
		{
			this.x = base.transform.position.x;
			this.y = base.transform.position.y;
			this.life = (2f + this.random.value) * this.lifeM;
			this.r = 0f;
			base.enabled = false;
		}
	}

	public virtual void Knock(float xDiff, float yDiff, float xI, float yI)
	{
		base.enabled = true;
		if (Mathf.Abs(xI) > 150f)
		{
			this.y += 2f;
		}
		float num = xI - this.xI;
		float num2 = yI - this.yI;
		this.rI -= xI * 2f / this.size;
		float num3 = num * num / 400f;
		this.xI += Mathf.Sign(num) * num3 * 0.05f / this.weight;
		this.yI += (num2 * 0.03f + 60f) / this.weight;
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.defendSounds, 0.4f + UnityEngine.Random.value * 0.2f, base.transform.position, 0.8f + 0.34f * UnityEngine.Random.value);
	}

	public override void Launch(float x, float y, float xI, float yI)
	{
		base.Launch(x, y, xI, yI);
		base.enabled = true;
		Map.RegisterFetchObject(x, y, 112f, 48f, base.transform);
		this.lastTrailX = x;
		this.lastTrailY = y;
		this.trailDrawDelay = 2;
		if (Map.InsideWall(x, y, this.size))
		{
			if (xI > 0f && !Map.InsideWall(x - 8f, y, this.size))
			{
				float num = 8f;
				float num2 = 0f;
				bool flag = false;
				bool flag2 = false;
				if (Map.ConstrainToBlocks(x - 8f, y, this.size, ref num, ref num2, ref flag, ref flag2))
				{
					x = x - 8f + num;
					y += num2;
				}
				xI = -xI * 0.6f;
			}
			else if (xI > 0f)
			{
				float num3 = xI * 0.03f;
				float num4 = yI * 0.03f;
				bool bounceX = false;
				bool bounceY = false;
				if (Map.ConstrainToBlocks(x, y, this.size, ref num3, ref num4, ref bounceX, ref bounceY))
				{
					this.Bounce(bounceX, bounceY);
				}
				x += num3;
				y += num4;
				xI = -xI * 0.6f;
			}
			if (xI < 0f && !Map.InsideWall(x + 8f, y, this.size))
			{
				float num5 = -8f;
				float num6 = 0f;
				bool flag3 = false;
				bool flag4 = false;
				if (Map.ConstrainToBlocks(x + 8f, y, this.size, ref num5, ref num6, ref flag3, ref flag4))
				{
					x = x + 8f + num5;
					y += num6;
				}
				xI = -xI * 0.6f;
			}
			else if (xI < 0f)
			{
				float num7 = xI * 0.03f;
				float num8 = yI * 0.03f;
				bool bounceX2 = false;
				bool bounceY2 = false;
				if (Map.ConstrainToBlocks(x, y, this.size, ref num7, ref num8, ref bounceX2, ref bounceY2))
				{
					this.Bounce(bounceX2, bounceY2);
				}
				x += num7;
				y += num8;
				xI = -xI * 0.6f;
			}
		}
		this.SetPosition();
	}

	public override void Death()
	{
		if (this.FiredLocally)
		{
			MapController.DamageGround(this.firedBy, this.damage, this.damageType, this.range, this.x, this.y, null);
			Map.ExplodeUnits(this.firedBy, this.damage, this.damageType, this.range, this.range, this.x, this.y, this.blastForce, 400f, -1, false, true);
			Map.ExplodeUnits(this.firedBy, this.damage, this.damageType, this.range * 1.5f, this.range * 1.33f, this.x, this.y, this.blastForce, 400f, 5, false, true);
		}
		this.MakeEffects();
		this.DestroyGrenade();
	}

	protected virtual void DestroyGrenade()
	{
		Map.RemoveGrenade(this);
		Map.RemoveShootableGrenade(this);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	protected override void Bounce(bool bounceX, bool bounceY)
	{
		if (bounceX)
		{
			this.yI *= 0.8f;
			if (this.useAngularFriction)
			{
				this.rI = this.yI * -Mathf.Sign(this.xI) * this.angularFrictionM;
			}
			this.xI *= -0.6f;
		}
		if (bounceY)
		{
			if (this.yI < 0f)
			{
				this.xI *= 0.8f * this.frictionM;
				if (this.useAngularFriction)
				{
					this.rI = -this.xI * this.angularFrictionM;
				}
				RaycastHit raycastHit;
				if (this.yI < -40f && base.IsMine && Physics.Raycast(new Vector3(this.x, this.y + 5f, 0f), Vector3.down, out raycastHit, 12f, this.groundLayer))
				{
					raycastHit.collider.SendMessage("StepOn", this, SendMessageOptions.DontRequireReceiver);
					Block component = raycastHit.collider.GetComponent<Block>();
					if (component != null)
					{
						component.CheckForMine();
					}
				}
			}
			else
			{
				this.xI *= 0.8f;
				if (this.useAngularFriction)
				{
					this.rI = this.xI * this.angularFrictionM;
				}
			}
			this.yI *= -0.4f;
		}
		float num = Mathf.Abs(this.xI) + Mathf.Abs(this.yI);
		if (num > this.minVelocityBounceSound)
		{
			float num2 = num / this.maxVelocityBounceVolume;
			float v = 0.05f + Mathf.Clamp(num2 * num2, 0f, 0.25f);
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.hitSounds, v, base.transform.position);
		}
	}

	protected virtual void MakeEffects()
	{
		if (this.hugeExplosion)
		{
			EffectsController.CreateHugeExplosion(this.x, this.y, this.range * 0.25f, this.range * 0.25f, 120f, 1f, 24f, 1f, 0.5f, 2, 140, 250f, this.range * 1.5f, 0.3f, 0.2f);
		}
		else
		{
			EffectsController.CreateExplosion(this.x, this.y, this.range * 0.25f, this.range * 0.25f, 120f, 1f, this.range * 1.5f, 1f, 0f, true);
		}
		this.PlayDeathSound();
		Map.DamageDoodads(this.damage, this.x, this.y, 0f, 0f, this.range, this.playerNum);
	}

	protected virtual void PlayDeathSound()
	{
		this.PlayDeathSound(0.7f);
	}

	protected virtual void PlayDeathSound(float v)
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.deathSounds, v, base.transform.position);
		}
	}

	protected virtual void PlaySpecialSound(float v)
	{
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.specialSounds, v, (base.transform.position + Camera.main.transform.position) / 2f);
	}

	public void SetMinLife(float amount)
	{
		if (this.life < amount)
		{
			this.life = amount;
		}
	}

	public void RunUpdate()
	{
		this.Update();
	}

	protected override void Update()
	{
		base.Update();
		if (this.life < 1f)
		{
			this.RunWarnings();
		}
		this.RunTrail();
	}

	protected virtual void RunWarnings()
	{
		if (this.life < 0.5f)
		{
			if (!this.panickedMooks)
			{
				Map.PanicUnits(this.x, this.y, 28f, 0.9f, true);
			}
			this.flickerCounter += this.t;
			if (this.flickerCounter > 0.0667f)
			{
				this.flickerCounter -= 0.0667f;
				if (this.mainMaterialShowing)
				{
					base.GetComponent<Renderer>().sharedMaterial = this.otherMaterial;
					this.pulseCount++;
					if (this.pulseCount % 2 == 1)
					{
						if (!this.largeWarning)
						{
							EffectsController.CreateRedWarningEffect(this.x, this.y, base.transform);
						}
						else
						{
							EffectsController.CreateRedWarningDiamondHuge(this.x, this.y, base.transform);
						}
						if (this.soundHolder.greeting.Length > 0)
						{
							Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.greeting, 0.33f, base.transform.position, 0.8f + 0.1f * (float)this.pulseCount);
						}
					}
				}
				else
				{
					base.GetComponent<Renderer>().sharedMaterial = this.mainMaterial;
				}
				this.mainMaterialShowing = !this.mainMaterialShowing;
			}
		}
		else
		{
			this.flickerCounter += this.t;
			if (this.flickerCounter > 0.133f)
			{
				this.flickerCounter -= 0.133f;
				if (this.mainMaterialShowing)
				{
					base.GetComponent<Renderer>().sharedMaterial = this.otherMaterial;
					this.pulseCount++;
					if (this.pulseCount % 2 == 1)
					{
						if (!this.largeWarning)
						{
							EffectsController.CreateRedWarningEffect(this.x, this.y, base.transform);
						}
						else
						{
							EffectsController.CreateRedWarningDiamondHuge(this.x, this.y, base.transform);
						}
					}
				}
				else
				{
					base.GetComponent<Renderer>().sharedMaterial = this.mainMaterial;
				}
				this.mainMaterialShowing = !this.mainMaterialShowing;
			}
		}
	}

	public void ResetTrail()
	{
		this.lastTrailX = this.x;
		this.lastTrailY = this.y;
		this.lastTrailAlphaM = 0.5f;
	}

	protected virtual void RunTrail()
	{
		if (this.trailDrawDelay > 0)
		{
			this.trailDrawDelay--;
			return;
		}
		if (this.trailType == TrailType.FireTrail)
		{
			float num = this.x - this.lastTrailX;
			float num2 = this.y - this.lastTrailY;
			float num3 = Mathf.Sqrt(num * num + num2 * num2);
			int num4 = (int)(num3 / 3f);
			for (int i = num4 - 1; i >= 0; i--)
			{
				this.lastTrailX = this.x - (float)i * num / (float)num4;
				this.lastTrailY = this.y - (float)i * num2 / (float)num4;
				EffectsController.CreateSparkParticle(this.lastTrailX - num / num3 * 3f, this.lastTrailY - num2 / num3 * 3f, 0.01f, 30f, 0f, 0f, UnityEngine.Random.value);
			}
		}
		else if (this.trailType == TrailType.ColorTrail && this.playerNum >= 0 && this.playerNum < 5)
		{
			float velocity = Mathf.Sqrt(this.xI * this.xI + this.yI * this.yI);
			EffectsController.CreateGrenadeTrailEffect(this.x, this.y, 5f, 0.2f, velocity, 200f, 3f, ref this.lastTrailX, ref this.lastTrailY, HeroController.GetHeroColor(this.playerNum), ref this.lastTrailAlphaM);
		}
		else if (this.trailType == TrailType.ColorTrail)
		{
			float velocity2 = Mathf.Sqrt(this.xI * this.xI + this.yI * this.yI);
			EffectsController.CreateGrenadeTrailEffect(this.x, this.y, 5f, 0.2f, velocity2, 200f, 3f, ref this.lastTrailX, ref this.lastTrailY, Color.black, ref this.lastTrailAlphaM);
		}
	}

	protected override void RunMovement()
	{
		this.HitFragile();
		if (this.bounceOffEnemies)
		{
			this.BounceOffEnemies();
		}
		base.RunMovement();
	}

	protected virtual bool CanBounceOnEnemies()
	{
		return this.yI < -120f;
	}

	protected virtual void BounceOffEnemies()
	{
		if (this.CanBounceOnEnemies())
		{
			if (this.bounceOffEnemiesMultiple)
			{
				if (Map.HitAllLivingUnits(this.firedBy, this.playerNum, 3, DamageType.Bounce, this.size, this.size, this.x, this.y + this.bounceYOffset, this.xI, 30f, false, true))
				{
					this.Bounce(false, false);
					this.yI = 50f + 90f / this.weight;
					this.xI = Mathf.Clamp(this.xI * 0.25f, -100f, 100f);
					EffectsController.CreateBulletPoofEffect(this.x, this.y - this.size * 0.5f);
				}
			}
			else if (Map.HitAllLivingUnits(this.firedBy, this.playerNum, 3, DamageType.Bounce, this.size, this.size, this.x, this.y + this.bounceYOffset, this.xI, 30f, false, true, ref this.alreadyBouncedOffUnits))
			{
				this.Bounce(false, false);
				if (this.alreadyBouncedOffUnits.Count > 0)
				{
					this.yI = (50f + 90f / this.weight) / (float)this.alreadyBouncedOffUnits.Count;
				}
				else
				{
					this.yI = 50f + 90f / this.weight;
				}
				this.xI = Mathf.Clamp(this.xI * 0.25f, -100f, 100f);
				EffectsController.CreateBulletPoofEffect(this.x, this.y - this.size * 0.5f);
			}
		}
	}

	protected virtual void HitFragile()
	{
		Vector3 vector = new Vector3(this.xI, this.yI, 0f);
		Vector3 normalized = vector.normalized;
		Collider[] array = Physics.OverlapSphere(new Vector3(this.x + normalized.x * 2f, this.y + normalized.y * 2f, 0f), 2f, this.fragileLayer);
		for (int i = 0; i < array.Length; i++)
		{
			EffectsController.CreateProjectilePuff(this.x, this.y);
			if (array[i].GetComponent<DoorDoodad>() != null)
			{
				this.Bounce(true, false);
			}
			else
			{
				array[i].gameObject.SendMessage("Damage", new DamageObject(1, this.damageType, this.xI, this.yI, this), SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public void SetToDisable(bool disabledAtStart)
	{
		this.disabledAtStart = disabledAtStart;
	}

	public override void SetPosition()
	{
		base.SetPosition();
	}

	public void SetupGrenade(int newSeed, int pNum, MonoBehaviour _FiredBy)
	{
		this.playerNum = pNum;
		this.seed = newSeed;
		this.random = new Randomf(this.seed);
		this.firedBy = _FiredBy;
		if (this.trailRenderer != null)
		{
			if (this.playerNum >= 0 && this.playerNum <= 3)
			{
				this.trailRenderer.startColor = (this.trailRenderer.endColor = HeroController.GetHeroColor(this.playerNum));
			}
			else
			{
				this.trailRenderer.startColor = Color.red;
				this.trailRenderer.endColor = Color.yellow;
			}
		}
	}

	public override UnityStream PackState(UnityStream stream)
	{
		stream.Serialize<float>(this.x);
		stream.Serialize<float>(this.y);
		stream.Serialize<bool>(base.enabled);
		return base.PackState(stream);
	}

	public override UnityStream UnpackState(UnityStream stream)
	{
		this.x = (float)stream.DeserializeNext();
		this.y = (float)stream.DeserializeNext();
		base.enabled = (bool)stream.DeserializeNext();
		return base.UnpackState(stream);
	}

	public int damage = 5;

	public DamageType damageType;

	protected LayerMask fragileLayer;

	public float range = 48f;

	public float blastForce = 50f;

	public FlickerFader fire1;

	public FlickerFader fire2;

	public FlickerFader fire3;

	public Puff smoke1;

	public Puff smoke2;

	public Puff explosion;

	public Puff explosionBig;

	public Shrapnel shrapnel;

	public SoundHolder soundHolder;

	protected Material mainMaterial;

	public Material otherMaterial;

	protected float flickerCounter;

	protected bool mainMaterialShowing = true;

	protected bool panickedMooks;

	public TrailType trailType;

	public bool useAngularFriction;

	public float angularFrictionM = 1f;

	public float minVelocityBounceSound = 33f;

	public float maxVelocityBounceVolume = 210f;

	protected List<Unit> alreadyBouncedOffUnits;

	public bool largeWarning;

	public float weight = 1f;

	public int playerNum = -1;

	private int trailDrawDelay;

	public bool bounceOffEnemies;

	public bool bounceOffEnemiesMultiple;

	protected float bounceYOffset = 2f;

	public MonoBehaviour firedBy;

	public CustomLineRenderer trailRenderer;

	protected bool disabledAtStart;

	public bool shootable;

	public bool hasHeroTrail;

	private bool? firedLocally;

	protected int pulseCount;

	public bool hugeExplosion;

	protected Sound sound;

	protected float lastTrailX;

	protected float lastTrailY;

	protected float lastTrailAlphaM = 0.1f;
}
