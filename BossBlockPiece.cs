// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BossBlockPiece : MonoBehaviour
{
	protected virtual void Awake()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		this.normalMaterial = base.GetComponent<Renderer>().sharedMaterial;
	}

	protected virtual void Start()
	{
		if (this.brokenState != null)
		{
			this.brokenState.SetActive(false);
		}
	}

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
		if (this.health <= 0 && !this.isDead)
		{
			this.Death();
		}
		else if (!this.isDead)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.hurtMaterial;
			this.hurtTime = 0.05f;
			base.enabled = true;
		}
		else if (this.isDead && !this.breakToBackground && this.health <= this.totalCollapsePoint)
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
		else if (this.breakToBackground)
		{
			base.GetComponent<Collider>().enabled = false;
			base.enabled = false;
			this.sprite.SetOffset(new Vector3(this.sprite.offset.x, this.sprite.offset.y, this.deathSpriteZOffset));
			if (this.createScrap)
			{
				this.CreateScrap();
			}
		}
		else if (!this.breakToDestroyedState)
		{
			base.GetComponent<Renderer>().enabled = false;
			base.GetComponent<Collider>().enabled = false;
			if (this.createScrap)
			{
				this.CreateScrap();
			}
		}
		base.GetComponent<Renderer>().sharedMaterial = this.destroyedMaterial;
		this.hurtTime = 0f;
		foreach (GameObject gameObject in this.connectedPieces)
		{
			if (gameObject != null)
			{
				gameObject.SendMessage("DelayedDestroy");
			}
		}
		if (this.isVolatile)
		{
			EffectsController.CreateExplosion(base.transform.position.x, base.transform.position.y, 8f, 8f, 120f, 1f, 100f, 0.2f, 0.3f, false);
		}
		if (this.setDeathFrameOnDeath)
		{
			this.sprite.SetLowerLeftPixel(this.deathFrameLowerLeft);
		}
		this.isDead = true;
	}

	protected void SetSpriteFrame(int spriteCollumn, int spriteRow)
	{
		this.sprite.SetLowerLeftPixel((float)((int)this.sprite.pixelDimensions.x * spriteCollumn), (float)((int)this.sprite.pixelDimensions.y * (1 + spriteRow)));
	}

	protected void TotalCollapse()
	{
		this.isDead = true;
		base.GetComponent<Collider>().enabled = false;
		base.enabled = false;
		foreach (GameObject gameObject in this.connectedPieces)
		{
			if (gameObject != null)
			{
				gameObject.SendMessage("DelayedCollapse");
			}
		}
		if (this.brokenState != null)
		{
			base.GetComponent<Collider>().enabled = false;
			base.enabled = false;
			base.GetComponent<Renderer>().enabled = false;
			this.brokenState.SetActive(true);
		}
		else if (this.breakToBackground)
		{
			this.sprite.SetOffset(new Vector3(this.sprite.offset.x, this.sprite.offset.y, this.deathSpriteZOffset));
			base.GetComponent<Renderer>().sharedMaterial = this.destroyedMaterial;
			if (this.createScrap)
			{
				this.CreateScrap();
			}
		}
		else if (!this.breakToDestroyedState)
		{
			base.GetComponent<Renderer>().enabled = false;
			base.GetComponent<Collider>().enabled = false;
			if (this.createScrap)
			{
				this.CreateScrap();
			}
		}
		this.hurtTime = 0f;
		if (this.isVolatile)
		{
			EffectsController.CreateExplosion(base.transform.position.x, base.transform.position.y, 8f, 8f, 120f, 1f, 100f, 0.2f, 0.3f, false);
		}
		if (this.setDeathFrameOnDeath)
		{
			this.sprite.SetLowerLeftPixel(this.deathFrameLowerLeft);
		}
	}

	protected virtual void CreateScrap()
	{
		UnityEngine.Debug.Log("Create Scrap!!");
		EffectsController.CreateScrapParticles(base.transform.position.x, base.transform.position.y, 18, this.scrapRadius, this.scrapRadius, 50f, 0f, 50f, 0f);
		EffectsController.CreateSparkShower(base.transform.position.x, base.transform.position.y, (int)(UnityEngine.Random.value * UnityEngine.Random.value * 6f), this.scrapRadius, 200f, 0f, 133f, 0.1f, 0f);
		if (UnityEngine.Random.value > 0.2f)
		{
			EffectsController.CreateFlameEffect(base.transform.position.x + UnityEngine.Random.value * this.scrapRadius * 0.5f - this.scrapRadius * 0.25f, base.transform.position.y + UnityEngine.Random.value * this.scrapRadius * 0.5f - this.scrapRadius * 0.25f, 0f, Vector3.zero);
			if (UnityEngine.Random.value > 0.2f)
			{
				EffectsController.CreateFlameEffect(base.transform.position.x + UnityEngine.Random.value * this.scrapRadius * 0.66f - this.scrapRadius * 0.33f, base.transform.position.y + UnityEngine.Random.value * this.scrapRadius * 0.66f - this.scrapRadius * 0.33f, 0.15f, Vector3.zero);
			}
			if (UnityEngine.Random.value > 0.5f)
			{
				EffectsController.CreateFlameEffect(base.transform.position.x + UnityEngine.Random.value * this.scrapRadius * 0.66f - this.scrapRadius * 0.33f, base.transform.position.y + UnityEngine.Random.value * this.scrapRadius * 0.66f - this.scrapRadius * 0.33f, 0.15f, Vector3.zero);
			}
		}
		EffectsController.CreateSmoke(base.transform.position.x + UnityEngine.Random.value * this.scrapRadius * 0.66f - this.scrapRadius * 0.33f, base.transform.position.y + UnityEngine.Random.value * this.scrapRadius * 0.66f - this.scrapRadius * 0.33f, UnityEngine.Random.value * 0.2f, Vector3.zero);
		EffectsController.CreateSmoke(base.transform.position.x + UnityEngine.Random.value * this.scrapRadius * 0.66f - this.scrapRadius * 0.33f, base.transform.position.y + UnityEngine.Random.value * this.scrapRadius * 0.66f - this.scrapRadius * 0.33f, UnityEngine.Random.value * 0.3f, Vector3.zero);
		this.createScrap = false;
	}

	protected virtual void Update()
	{
		if (this.hurtTime > 0f)
		{
			this.hurtTime -= Time.deltaTime;
			if (this.hurtTime <= 0f)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.normalMaterial;
			}
		}
		else if (base.GetComponent<Renderer>().sharedMaterial == this.hurtMaterial)
		{
			this.hurtTime = 0.0334f;
		}
		if (this.exploding)
		{
			this.delayedDeathTime -= Time.deltaTime;
			if (this.delayedDeathTime <= 0f)
			{
				this.exploding = false;
				this.Damage(new DamageObject(this.health + 1, DamageType.Explosion, 0f, 0f, this));
			}
		}
		if (this.collapsing)
		{
			UnityEngine.Debug.Log("Collapsing " + this.collapseDelay);
			this.collapseDelay -= Time.deltaTime;
			if (this.collapseDelay <= 0f)
			{
				this.collapsing = false;
				this.TotalCollapse();
			}
		}
	}

	public int health = 5;

	protected Material normalMaterial;

	public Material hurtMaterial;

	public Material destroyedMaterial;

	protected float hurtTime;

	public bool isVolatile;

	public float delayedDeathTime = 0.33f;

	protected bool exploding;

	protected bool collapsing;

	protected float collapseDelay = 0.33f;

	[HideInInspector]
	public BossBlocksHolder boss;

	public GameObject[] connectedPieces;

	public GameObject brokenState;

	protected SpriteSM sprite;

	public float deathSpriteZOffset = 25f;

	public bool breakToBackground;

	public bool breakToDestroyedState;

	public int totalCollapsePoint = -25;

	protected bool isDead;

	public bool setDeathFrameOnDeath;

	public Vector2 deathFrameLowerLeft = new Vector2(0f, 32f);

	public bool createScrap = true;

	public float scrapRadius = 8f;
}
