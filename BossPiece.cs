// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class BossPiece : NetworkedUnit
{
	protected override void Awake()
	{
		base.Awake();
		this.health = this.fullHealth;
	}

	private void Start()
	{
		this.baseHealth = this.health;
		if (this.parentTransform == null)
		{
			this.parentTransform = base.transform.parent;
		}
		if (base.GetComponent<Renderer>() != null)
		{
			this.baseMaterial = base.GetComponent<Renderer>().sharedMaterial;
		}
	}

	public void Damage(DamageObject damageObject)
	{
		if (SortOfFollow.IsItSortOfVisible(base.transform.position, 80f, 80f))
		{
			this.DamageInternal(damageObject);
			foreach (BossPiece bossPiece in this.connectedPieces)
			{
				if (bossPiece != this)
				{
					bossPiece.DamageInternal(damageObject);
				}
			}
		}
		else
		{
			UnityEngine.Debug.Log("Offscreen");
		}
	}

	public void Death()
	{
		if (!this.isDead)
		{
			this.isDead = true;
			this.explodeDelay = 0f + UnityEngine.Random.value * 0.6f;
			if (this.health > 0)
			{
				this.health = 0;
			}
			base.GetComponent<Renderer>().sharedMaterial = this.deadMaterial;
			foreach (BossPiece bossPiece in this.connectedPieces)
			{
				if (bossPiece != this)
				{
					bossPiece.Death();
				}
			}
		}
	}

	public void Gib()
	{
		if (!this.gibbed)
		{
			this.gibbed = true;
			if (base.GetComponent<Collider>() != null)
			{
				base.GetComponent<Collider>().enabled = false;
			}
			base.GetComponent<Renderer>().enabled = false;
			base.gameObject.SetActive(false);
			foreach (BossPiece bossPiece in this.connectedPieces)
			{
				if (bossPiece != this)
				{
					bossPiece.Gib();
				}
			}
		}
	}

	public void SetHealth(int h)
	{
		this.health = h;
		this.baseHealth = h;
	}

	public void DamageInternal(DamageObject damageObject)
	{
		if (this.health > 0)
		{
			this.damageBrotalityCount += damageObject.damage;
			StatisticsController.AddBrotality(this.damageBrotalityCount / 20);
			this.damageBrotalityCount -= this.damageBrotalityCount / 20 * 20;
		}
		if (!this.damagedThisFrame)
		{
			if (!this.immortal)
			{
				this.health -= damageObject.damage;
			}
			if (this.health <= 0)
			{
				this.Death();
			}
			else if (!this.isDamaged && (float)this.health < (float)this.baseHealth * 0.667f)
			{
				this.isDamaged = true;
			}
			if (base.GetComponent<Renderer>() != null && !this.isDead)
			{
				this.flashCounter = 0.0334f;
				if (this.isDamaged)
				{
					base.GetComponent<Renderer>().sharedMaterial = this.damagedHurtMaterial;
				}
				else
				{
					base.GetComponent<Renderer>().sharedMaterial = this.hurtMaterial;
				}
			}
			this.damagedThisFrame = true;
		}
	}

	protected virtual void Update()
	{
		this.damagedThisFrame = false;
		if (Mathf.Round(this.parentTransform.position.x) != Mathf.Round(this.lastX) || Mathf.Round(this.parentTransform.position.y) != Mathf.Round(this.lastY))
		{
			this.lastX = this.parentTransform.position.x;
			this.lastY = this.parentTransform.position.y;
			base.transform.position = new Vector3(Mathf.Round(this.parentTransform.position.x), Mathf.Round(this.parentTransform.position.y), this.parentTransform.position.z);
			base.transform.rotation = this.parentTransform.rotation;
		}
		if (this.explodeDelay > 0f)
		{
			this.explodeDelay -= Time.deltaTime;
			if (this.explodeDelay <= 0f)
			{
				EffectsController.CreateSmallExplosion(base.transform.position.x, base.transform.position.y, base.transform.position.z - 8f, 0.4f, (UnityEngine.Random.value <= 0.6f) ? 0f : 0.4f);
			}
		}
		if (this.flashCounter > 0f)
		{
			this.flashCounter -= Time.deltaTime;
			if (this.flashCounter <= 0f)
			{
				if (this.isDead)
				{
					base.GetComponent<Renderer>().sharedMaterial = this.deadMaterial;
				}
				else if (!this.isDamaged)
				{
					base.GetComponent<Renderer>().sharedMaterial = this.baseMaterial;
				}
				else
				{
					base.GetComponent<Renderer>().sharedMaterial = this.damagedMaterial;
				}
			}
		}
		if (!this.parentTransform.gameObject.activeInHierarchy)
		{
			base.GetComponent<Renderer>().enabled = false;
		}
		else
		{
			base.GetComponent<Renderer>().enabled = true;
		}
	}

	protected float lastX;

	protected float lastY;

	public Material hurtMaterial;

	public Material damagedMaterial;

	public Material damagedHurtMaterial;

	public Material deadMaterial;

	protected Material baseMaterial;

	public bool flashHurt = true;

	protected float flashCounter;

	[HideInInspector]
	public int fullHealth = 100;

	protected int baseHealth = 10;

	protected bool isDead;

	[HideInInspector]
	public bool gibbed;

	protected bool isDamaged;

	protected float explodeDelay;

	[HideInInspector]
	public bool immortal;

	[HideInInspector]
	public bool canFriendly = true;

	protected bool damagedThisFrame;

	public List<BossPiece> connectedPieces = new List<BossPiece>();

	public Transform parentTransform;

	protected int damageBrotalityCount;
}
