// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BarrelBlock : FallingBlock
{
	protected override void EffectsCollapse(float xI, float yI)
	{
		this.MakeEffects();
	}

	protected override void EffectsDestroyed(float xI, float yI, float force)
	{
		this.MakeEffects();
	}

	protected override void Start()
	{
		base.Start();
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
		this.originalMaterial = base.GetComponent<Renderer>().sharedMaterial;
		this.shakeTime = 0.5f;
	}

	protected virtual void MakeEffects()
	{
		if (!this.exploded)
		{
			Networking.RPC(PID.TargetAll, new RpcSignature(this.Explode), false);
		}
	}

	public virtual void Explode()
	{
		if (this.exploded)
		{
			return;
		}
		this.exploded = true;
		if (this.fullExplosion)
		{
			EffectsController.CreateSparkShower(this.x, this.y, 110, 3f, 200f, 0f, 250f, 0.6f, 0.5f);
			EffectsController.CreatePlumes(this.x, this.y, 3, 8f, 315f, 0f, 0f);
		}
		else
		{
			EffectsController.CreateSparkShower(this.x, this.y, 70, 3f, 200f, 0f, 250f, 0.6f, 0.5f);
			EffectsController.CreatePlumes(this.x, this.y, 3, 8f, 270f, 0f, 0f);
		}
		Vector3 a = this.random.insideUnitCircle;
		if (this.fullExplosion)
		{
			EffectsController.CreateEffect(this.smoke1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, this.random.value * 0.5f, a * this.range * 3f, BloodColor.None);
			a = this.random.insideUnitCircle;
			EffectsController.CreateEffect(this.smoke1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, this.random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		}
		EffectsController.CreateEffect(this.smoke1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, this.random.value * 0.2f, this.random.insideUnitSphere * this.range * 3f, BloodColor.None);
		if (this.fullExplosion)
		{
			a = this.random.insideUnitCircle;
			EffectsController.CreateEffect(this.smoke2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, this.random.value * 0.5f, a * this.range * 3f, BloodColor.None);
			a = this.random.insideUnitCircle;
			EffectsController.CreateEffect(this.smoke2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, this.random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		}
		a = this.random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, this.random.value * 0.2f, a * this.range * 3f, BloodColor.None);
		if (this.fullExplosion)
		{
			a = this.random.insideUnitCircle;
			EffectsController.CreateEffect(this.explosionBig, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, this.random.value * 0.5f, a * this.range * 3f, BloodColor.None);
			a = this.random.insideUnitCircle;
			EffectsController.CreateEffect(this.explosionBig, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, this.random.value * 0.5f, a * this.range * 0.5f * 3f, BloodColor.None);
			a = this.random.insideUnitCircle;
			EffectsController.CreateEffect(this.explosionBig, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0f, a * this.range * 0.3f * 3f, BloodColor.None);
		}
		a = this.random.insideUnitCircle;
		EffectsController.CreateEffect(this.explosion, this.x + a.x * this.range * 0.15f, this.y + a.y * this.range * 0.15f, 0f, a * this.range * 0.5f * 3f, BloodColor.None);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + this.random.value * 0.2f, a * this.range * 3f);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + this.random.value * 0.2f, a * this.range * 3f);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire3, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + this.random.value * 0.2f, a * this.range * 3f);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + this.random.value * 0.2f, a * this.range * 3f);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + this.random.value * 0.2f, a * this.range * 3f);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire3, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + this.random.value * 0.2f, a * this.range * 3f);
		if (!this.fullExplosion)
		{
			a = global::Math.RandomPointOnCircle();
			if (a.y < 0f)
			{
				a.y *= -1f;
			}
			EffectsController.CreateEffect(this.fire1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.15f + this.random.value * 0.4f, a * this.range * 3f);
			a = global::Math.RandomPointOnCircle();
			if (a.y < 0f)
			{
				a.y *= -1f;
			}
			EffectsController.CreateEffect(this.fire2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.15f + this.random.value * 0.4f, a * this.range * 4f);
			a = global::Math.RandomPointOnCircle();
			if (a.y < 0f)
			{
				a.y *= -1f;
			}
			EffectsController.CreateEffect(this.fire3, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.15f + this.random.value * 0.4f, a * this.range * 4f);
			a = global::Math.RandomPointOnCircle();
			if (a.y < 0f)
			{
				a.y *= -1f;
			}
			EffectsController.CreateEffect(this.fire1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + this.random.value * 0.3f, a * this.range * 4f);
		}
		SortOfFollow.Shake(1f);
		this.PlayDeathSound();
		Map.DisturbWildLife(this.x, this.y, 120f, 5);
		MapController.DamageGround(this, 12, this.damageType, this.range, this.x, this.y, null);
		Map.ShakeTrees(this.x, this.y, 256f, 64f, 128f);
		if (!this.fullExplosion)
		{
			MapController.BurnUnitsAround_NotNetworked(this, -1, 1, this.range * 2f, this.x, this.y, true, true);
		}
		Map.ExplodeUnits(this, 12, this.damageType, this.range * 1.2f, this.range, this.x, this.y - 6f, 200f, 300f, 15, false, false);
		Map.ExplodeUnits(this, 1, this.damageType, this.range * 1f, this.range, this.x, this.y - 6f, 200f, 300f, -1, false, false);
		this.sprite.SetLowerLeftPixel((float)((int)this.sprite.width), (float)((int)this.sprite.lowerLeftPixel.y));
		base.gameObject.SetActive(false);
	}

	protected override void SetCollapsedVisuals()
	{
		base.SetCollapsedVisuals();
		base.gameObject.SetActive(false);
	}

	public override void DamageInternal(int damage, float xI, float yI)
	{
		if (SetResolutionCamera.IsItVisible(base.transform.position) || this.forceExplosions)
		{
			if (this.delayExplosionTime > 0f)
			{
				if (this.explosionCounter <= 0f)
				{
					this.explosionCounter = this.delayExplosionTime;
				}
			}
			else
			{
				this.health = this.collapseHealthPoint;
				this.Collapse(0f, 30f, 1f);
			}
		}
	}

	protected override void Land()
	{
		this.row = Map.GetRow(this.y);
		this.collumn = Map.GetCollumn(this.x);
		this.disturbed = false;
		this.health = this.collapseHealthPoint;
		this.collapseChance = 2f;
		this.Collapse(0f, 30f, 1f);
	}

	public override void Burn(DamageObject damgeObject)
	{
		if (this.delayExplosionTime > 0f)
		{
			this.explosionCounter = this.delayExplosionTime;
		}
		else
		{
			this.health = this.collapseHealthPoint;
			this.Collapse(0f, 30f, 1f);
		}
	}

	public override void ForceBurn()
	{
		if (this.delayExplosionTime > 0f)
		{
			this.explosionCounter = this.delayExplosionTime;
		}
		else
		{
			this.health = this.collapseHealthPoint;
			this.Collapse(0f, 30f, 1f);
		}
	}

	protected override void Update()
	{
		if (this.firstFrame)
		{
			this.firstFrame = false;
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position, Vector3.down, out raycastHit, 24f, this.groundLayer))
			{
				raycastHit.collider.gameObject.SendMessage("AttachAboveBlock", this);
			}
		}
		base.Update();
		if (this.explosionCounter > 0f)
		{
			this.flameCounter += this.t;
			if (this.flameCounter > 0f && this.explosionCounter > 0.3f)
			{
				this.flameCounter -= 0.0667f;
				Vector3 vector = this.random.insideUnitCircle;
				switch (this.random.Range(0, 3))
				{
				case 0:
					EffectsController.CreateEffect(this.fire1, this.x + vector.x * 6f, this.y + vector.y * 9f - 2f, this.random.value * 0.0434f, Vector3.zero);
					break;
				case 1:
					EffectsController.CreateEffect(this.fire2, this.x + vector.x * 6f, this.y + vector.y * 9f - 2f, this.random.value * 0.0434f, Vector3.zero);
					break;
				case 2:
					EffectsController.CreateEffect(this.fire3, this.x + vector.x * 6f, this.y + vector.y * 9f - 2f, this.random.value * 0.0434f, Vector3.zero);
					break;
				}
			}
			this.RunWarning(this.t, this.explosionCounter);
			this.explosionCounter -= this.t;
			if (this.explosionCounter <= 0f)
			{
				this.health = this.collapseHealthPoint;
				this.Collapse(0f, 50f, 1f);
			}
			if (this.shakeWhileBurning)
			{
				this.shakeCounter += this.t * 60f;
				base.SetPosition(global::Math.Sin(this.shakeCounter));
			}
		}
	}

	protected virtual void RunWarning(float t, float explosionTime)
	{
		if (explosionTime < 0.7f)
		{
			this.warningCounter += t;
			if (this.warningOn && this.warningCounter > 0.0667f)
			{
				this.warningOn = false;
				this.warningCounter -= 0.0667f;
				base.GetComponent<Renderer>().sharedMaterial = this.originalMaterial;
			}
			else if (this.warningCounter > 0.0667f && explosionTime < 0.175f)
			{
				this.warningOn = true;
				this.warningCounter -= 0.0667f;
				base.GetComponent<Renderer>().sharedMaterial = this.warningMaterial;
			}
			else if (this.warningCounter > 0.175f && explosionTime < 0.5f)
			{
				this.warningOn = true;
				this.warningCounter -= 0.175f;
				base.GetComponent<Renderer>().sharedMaterial = this.warningMaterial;
			}
		}
	}

	public FlickerFader fire1;

	public FlickerFader fire2;

	public FlickerFader fire3;

	public Puff smoke1;

	public Puff smoke2;

	public Puff explosion;

	public Puff explosionBig;

	public float range = 60f;

	public bool fullExplosion = true;

	public bool shakeWhileBurning;

	protected bool forceExplosions;

	public Shrapnel shrapnelPrefab2;

	public float delayExplosionTime;

	protected float explosionCounter;

	protected float warningCounter;

	public Material warningMaterial;

	protected Material originalMaterial;

	protected bool warningOn;

	public DamageType damageType = DamageType.Fire;

	protected bool firstFrame = true;

	protected bool exploded;
}
