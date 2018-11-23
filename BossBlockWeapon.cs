// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BossBlockWeapon : MonoBehaviour
{
	public void DelayedDestroy()
	{
		if (this.health > 0)
		{
			this.exploding = true;
		}
	}

	public void DelayedCollapse()
	{
		UnityEngine.Debug.Log("DelayedCollapse");
		if (this.health > 0)
		{
			this.Damage(new DamageObject(this.health + 1, DamageType.Crush, 0f, 0f, this));
		}
		if (this.health > this.totalCollapsePoint)
		{
			this.collapsing = true;
		}
	}

	private void Awake()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		this.normalMaterial = base.GetComponent<Renderer>().sharedMaterial;
	}

	private void Start()
	{
		if (this.brokenState != null)
		{
			this.brokenState.SetActive(false);
		}
		this.thinkCounter = this.thinkRate;
		this.spritePixelWidth = (int)this.sprite.pixelDimensions.x;
		this.spritePixelHeight = (int)this.sprite.pixelDimensions.y;
		this.isRevealed = this.startRevealed;
	}

	protected virtual void SetSpriteFrame(int spriteCollumn, int spriteRow)
	{
		this.sprite.SetLowerLeftPixel((float)(this.spritePixelWidth * spriteCollumn), (float)(this.spritePixelHeight * (1 + spriteRow)));
	}

	protected void SetMaterial(Material m)
	{
		base.GetComponent<Renderer>().sharedMaterial = m;
		if (m.mainTexture != this.currentTexture)
		{
			this.sprite.RecalcTexture();
		}
		this.currentTexture = m.mainTexture;
	}

	public void ForceBurn()
	{
		this.Damage(new DamageObject(1, DamageType.Fire, 0f, 0f, null));
	}

	public void Burn()
	{
		this.Damage(new DamageObject(1, DamageType.Fire, 0f, 0f, null));
	}

	public void Damage(DamageObject damgeObject)
	{
		this.health -= damgeObject.damage;
		if (!this.isDead && this.health <= 0)
		{
			this.Death();
		}
		else if (!this.isDead)
		{
			this.SetMaterial(this.hurtMaterial);
			this.hurtTime = 0.05f;
			base.enabled = true;
		}
		else if (this.isDead && this.breakToBackground && this.health <= this.totalCollapsePoint)
		{
			this.TotalCollapse();
		}
	}

	protected virtual void Death()
	{
		if (this.brokenState != null)
		{
			base.GetComponent<Collider>().enabled = false;
			base.enabled = false;
			base.GetComponent<Renderer>().enabled = false;
			this.brokenState.SetActive(true);
		}
		else
		{
			if (this.breakToBackground)
			{
				base.GetComponent<Collider>().enabled = false;
				base.enabled = false;
				this.sprite.SetOffset(new Vector3(this.sprite.offset.x, this.sprite.offset.y, this.deathSpriteZOffset));
			}
			this.SetMaterial(this.destroyedMaterial);
			this.SetDeathFrame();
		}
		this.hurtTime = 0f;
		EffectsController.CreateExplosion(base.transform.position.x, base.transform.position.y, 8f, 8f, 120f, 1f, 100f, 0.2f, 0.3f, false);
		foreach (GameObject gameObject in this.connectedPieces)
		{
			gameObject.SendMessage("DelayedDestroy");
		}
		this.isDead = true;
	}

	protected void TotalCollapse()
	{
		base.GetComponent<Collider>().enabled = false;
		if (this.sprite != null)
		{
			this.sprite.GetComponent<Renderer>().enabled = false;
		}
		base.enabled = false;
		foreach (GameObject gameObject in this.connectedPieces)
		{
			gameObject.SendMessage("DelayedCollapse");
		}
	}

	protected virtual void LookForPlayer()
	{
		if (HeroController.CanSeePlayer(base.transform.position.x + this.fireOffset.x + (float)this.sightXOffset, base.transform.position.y + this.fireOffset.y + (float)this.sightYOffset, (int)Mathf.Sign(this.fireDirection.x), (float)this.sightXRange, (float)this.sightYRange, ref this.seenPlayerNum))
		{
			this.revealing = true;
			this.isRevealed = true;
			this.frame = this.hiddenFrame;
			this.SetSpriteFrame(this.frame, 0);
		}
	}

	protected bool CanFireAtPlayer()
	{
		return HeroController.CanSeePlayer(base.transform.position.x + this.fireOffset.x + (float)this.sightXOffset, base.transform.position.y + this.fireOffset.y + (float)this.sightYOffset, (int)Mathf.Sign(this.fireDirection.x), (float)this.sightXRange, (float)this.sightYRange, ref this.seenPlayerNum);
	}

	protected virtual void FireWeapon()
	{
		base.GetComponent<Collider>().enabled = false;
		float x = base.transform.position.x + this.fireOffset.x;
		float y = base.transform.position.y + this.fireOffset.y;
		float x2 = this.fireDirection.x;
		float y2 = this.fireDirection.y;
		float arg = 0f;
		float arg2 = 0f;
		int num = -1;
		HeroController.GetRandomPlayerPos(ref arg, ref arg2, ref num);
		Projectile @object = ProjectileController.SpawnProjectileOverNetwork(this.projectile, this, x, y, this.fireDirection.x, this.fireDirection.y, false, -1, false, true);
		if (num >= 0)
		{
			Networking.RPC<float, float, int>(PID.TargetAll, new RpcSignature<float, float, int>(@object.Target), arg, arg2, num, false);
		}
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.attackSounds, 0.5f, new Vector3(x, y, 0f));
		base.GetComponent<Collider>().enabled = true;
	}

	private void Update()
	{
		if (Map.isEditing)
		{
			return;
		}
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.hurtTime > 0f)
		{
			this.hurtTime -= this.t;
			if (this.hurtTime <= 0f)
			{
				this.SetMaterial(this.normalMaterial);
			}
		}
		if (this.health > 0)
		{
			if (!this.isRevealed)
			{
				this.thinkCounter -= this.t;
				if (this.thinkCounter <= 0f)
				{
					this.thinkCounter += this.thinkRate * 0.5f;
					if (SortOfFollow.IsItSortOfVisible(base.transform.position.x, base.transform.position.y, this.sightScreenOffset, this.sightScreenOffset))
					{
						this.LookForPlayer();
					}
					this.thinkCounter += this.thinkRate * 0.5f;
				}
			}
			if (this.revealing)
			{
				this.frameCounter += this.t;
				if (this.frameCounter >= 0.045f)
				{
					this.frame++;
					this.frameCounter -= 0.045f;
					this.SetSpriteFrame(this.frame, 0);
					if (this.frame >= this.restFrame)
					{
						this.revealing = false;
						this.isRevealed = true;
					}
				}
			}
			else if (this.isRevealed && !this.firing)
			{
				this.thinkCounter -= this.t;
				if (this.thinkCounter <= 0f)
				{
					this.thinkCounter += 0.25f;
					if (SortOfFollow.IsItSortOfVisible(base.transform.position.x, base.transform.position.y, this.sightScreenOffset, this.sightScreenOffset))
					{
						if (this.CanFireAtPlayer())
						{
							this.StartFiring();
						}
					}
					else
					{
						this.thinkCounter += 0.25f;
					}
				}
			}
			else if (this.isRevealed && this.firing)
			{
				this.RunFiring();
			}
		}
		if (this.exploding)
		{
			this.explodeDelay -= this.t;
			if (this.explodeDelay <= 0f)
			{
				this.exploding = false;
				this.Damage(new DamageObject(this.health + 1, DamageType.Explosion, 0f, 0f, this));
			}
		}
		if (this.collapsing)
		{
			this.collapseDelay -= this.t;
			if (this.collapseDelay <= 0f)
			{
				this.collapsing = false;
				this.TotalCollapse();
			}
		}
	}

	protected virtual void RunFiring()
	{
		this.frameCounter += this.t;
		if (this.frameCounter >= 0.045f)
		{
			this.frame++;
			this.frameCounter -= 0.045f;
			if (this.frame == this.fireFrame)
			{
				this.FireWeapon();
			}
			if (this.frame == this.returnToRestFrame)
			{
				this.thinkCounter = this.fireDelay;
				this.frame = this.restFrame;
				this.firing = false;
			}
			this.SetSpriteFrame(this.frame, 0);
		}
	}

	protected virtual void StartFiring()
	{
		this.firing = true;
		this.frame = this.restFrame;
		this.frameCounter = 0f;
	}

	protected virtual void SetDeathFrame()
	{
		this.SetSpriteFrame(this.deathFrame, 0);
	}

	public int health = 5;

	protected Material normalMaterial;

	public Material hurtMaterial;

	public Material destroyedMaterial;

	protected Texture currentTexture;

	protected float hurtTime;

	public GameObject brokenState;

	[HideInInspector]
	public BossBlocksHolder boss;

	protected int spritePixelWidth = 32;

	protected int spritePixelHeight = 32;

	public Projectile projectile;

	protected bool firing;

	protected bool revealing;

	protected bool isRevealed;

	public bool startRevealed;

	protected float frameCounter;

	protected int frame;

	public int hiddenFrame;

	public int restFrame = 8;

	public int fireFrame = 12;

	public int returnToRestFrame = 16;

	public float fireDelay = 0.5f;

	protected float thinkCounter = 0.6f;

	public Vector3 fireOffset = Vector3.zero;

	public Vector3 fireDirection = new Vector3(-300f, 0f, 0f);

	protected int seenPlayerNum = -1;

	public int sightXRange = 256;

	public int sightYRange = 32;

	public int sightXOffset;

	public int sightYOffset;

	public float sightScreenOffset = -24f;

	public SoundHolder soundHolder;

	protected bool collapsing;

	protected float collapseDelay = 0.33f;

	protected bool exploding;

	protected float explodeDelay = 0.33f;

	public float deathSpriteZOffset = 25f;

	protected SpriteSM sprite;

	public GameObject[] connectedPieces;

	public bool breakToBackground;

	public int totalCollapsePoint = -25;

	protected bool isDead;

	public int deathFrame;

	protected float t = 0.01f;

	public float thinkRate = 0.5f;
}
