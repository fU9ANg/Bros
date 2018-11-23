// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Doodad : BroforceObject
{
	public float centerX
	{
		get
		{
			return this.offsetX + this.x;
		}
	}

	public float centerY
	{
		get
		{
			return this.offsetY + this.y;
		}
	}

	protected virtual void Start()
	{
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
		if (this.attatchToTerrain)
		{
			this.AttachDoodad();
		}
		this.collumn = Map.GetCollumn(base.transform.position.x);
		this.row = Map.GetRow(base.transform.position.y);
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
		SpriteSM component = base.GetComponent<SpriteSM>();
		this.offsetX = component.offset.x;
		this.offsetY = component.offset.y;
		base.transform.position = new Vector3(this.x, this.y, base.transform.position.z + UnityEngine.Random.value * 0.1f);
		if (this.isDamageable)
		{
			Map.RegisterDestroyableDoodad(this);
		}
		if (this.canBloody)
		{
			Map.RegisterDecalDoodad(this);
		}
		this.maxHealth = this.health;
		if (this.flipXRandom)
		{
			Doodad.flipCount++;
			base.transform.localScale = new Vector3((float)(Doodad.flipCount % 2 * 2 - 1), 1f, 1f);
		}
	}

	protected virtual void AttachDoodad()
	{
		if (Physics.Raycast(base.transform.position, Vector3.down, out this.groundHit, 24f, this.groundLayer))
		{
			this.groundHit.collider.gameObject.SendMessage("AttachMe", this, SendMessageOptions.DontRequireReceiver);
		}
	}

	public virtual void Collapse()
	{
		if (!this.collapsed)
		{
			this.collapsed = true;
			this.DropGibs();
			base.gameObject.SetActive(false);
			Map.RemoveDestroyableDoodad(this);
		}
	}

	protected virtual void DropGibs()
	{
		for (int i = 0; i < this.gibs.Length; i++)
		{
			if (this.gibs[i] != null)
			{
				this.gibs[i].gameObject.SetActive(true);
				this.gibs[i].transform.parent = null;
			}
		}
		if (this.gibHolderPrefab != null)
		{
			float num = 0f;
			float num2 = 0f;
			if (this.lastDamageObject != null)
			{
				num2 = this.lastDamageObject.xForce;
				num = this.lastDamageObject.yForce;
			}
			EffectsController.CreateGibs(this.gibHolderPrefab, this.centerX, this.centerY, 100f, 100f, this.xI + num2 * 0.1f, this.yI + num * 0.1f);
		}
		if (this.createWholeGib)
		{
			EffectsController.CreateDoodadGib(base.gameObject.GetComponent<SpriteSM>(), this.gibsType, base.GetComponent<Renderer>().sharedMaterial, this.x, this.y, 0f, 0f, 0f, 0f, (int)Mathf.Sign(base.transform.localScale.x));
		}
	}

	public virtual bool Damage(DamageObject damageObject)
	{
		return false;
	}

	public virtual void Bloody()
	{
		if (this.canBloody && base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.bloodyMaterial;
			Map.RemoveDecalDoodad(this);
			this.canBloody = false;
		}
	}

	public virtual bool DamageOptional(DamageObject damageObject, ref bool showBulletHit)
	{
		this.lastDamageObject = damageObject;
		if (this.damageStateMaterial == null || damageObject.damage >= this.maxHealth + 4 || this.health <= 1)
		{
			this.Death();
			showBulletHit = true;
			return true;
		}
		if (Time.time - this.lastDamageTime > this.damageDelay)
		{
			this.health--;
			float num = (float)this.health / (float)this.maxHealth;
			if (this.damageGreaterStateMaterial != null && base.GetComponent<Renderer>().sharedMaterial == this.damageStateMaterial && num <= 0.5f)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.damageGreaterStateMaterial;
				showBulletHit = true;
				this.DamageEffects(damageObject.xForce, damageObject.yForce);
			}
			else if (base.GetComponent<Renderer>().sharedMaterial != this.damageStateMaterial && (num > 0.5f || this.damageGreaterStateMaterial == null))
			{
				base.GetComponent<Renderer>().sharedMaterial = this.damageStateMaterial;
				showBulletHit = true;
				this.DamageEffects(damageObject.xForce, damageObject.yForce);
			}
			this.lastDamageTime = Time.time;
			if (this.health <= 1)
			{
				this.lastDamageTime += this.damageDelay;
			}
		}
		return false;
	}

	protected void DamageEffects(float xForce, float yForce)
	{
		if (this.soundHolder != null)
		{
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.defendSounds, 0.25f, base.transform.position);
		}
		if (this.doodadType == DoodadType.TreeBushes || this.doodadType == DoodadType.Tree)
		{
			EffectsController.CreateLeafBurst(this.centerX, this.centerY + 3f, 0f, 7, 4f, xForce * 0.2f, 25f + yForce * 0.15f, (Mathf.Abs(xForce) + Mathf.Abs(yForce)) * 0.2f);
		}
	}

	public virtual void Death()
	{
		if (this.skeletonMaterial != null)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.skeletonMaterial;
			Map.RemoveDestroyableDoodad(this);
		}
		else
		{
			this.Collapse();
			if (!this.alwaysDropGibs && this.gibs != null)
			{
				for (int i = 0; i < this.gibs.Length; i++)
				{
					this.gibs[i].Death();
				}
			}
		}
		if (this.soundHolder != null)
		{
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.deathSounds, 0.25f, base.transform.position);
		}
		if (this.doodadType == DoodadType.TreeBushes || this.doodadType == DoodadType.Tree)
		{
			EffectsController.CreateLeafBurst(this.centerX, this.centerY + 3f, 0f, 7, 4f, this.lastDamageObject.xForce * 0.2f, 25f + this.lastDamageObject.yForce * 0.15f, (Mathf.Abs(this.lastDamageObject.xForce) + Mathf.Abs(this.lastDamageObject.yForce)) * 0.2f);
		}
	}

	public DoodadPiece[] gibs;

	public bool createWholeGib;

	public DoodadGibsType gibsType;

	public int collumn;

	public int row;

	public float width = 16f;

	public float height = 16f;

	[HideInInspector]
	protected float offsetX;

	[HideInInspector]
	protected float offsetY;

	public bool isDamageable;

	public int damageStateSpriteOffset = -1;

	public Material damageStateMaterial;

	public Material damageGreaterStateMaterial;

	public Material skeletonMaterial;

	public float damageDelay = 0.33f;

	public bool canBloody;

	protected bool collapsed;

	public bool attatchToTerrain = true;

	public Material bloodyMaterial;

	protected LayerMask groundLayer;

	protected RaycastHit groundHit;

	protected DamageObject lastDamageObject;

	public SoundHolder soundHolder;

	public bool alwaysDropGibs;

	public bool flipXRandom;

	protected static int flipCount;

	public DoodadType doodadType;

	protected float lastDamageTime;

	public GibHolder gibHolderPrefab;
}
