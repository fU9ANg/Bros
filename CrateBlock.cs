// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class CrateBlock : FallingBlock
{
	protected override void Start()
	{
		base.Start();
		if (this.containsPresent)
		{
			this.pickup = PickupableController.CreateAmmoBox(this.x, this.y);
			Registry.RegisterDeterminsiticGameObject(this.pickup.gameObject);
			this.pickup.gameObject.SetActive(false);
		}
	}

	protected override void EffectsCollapse(float xI, float yI)
	{
		this.MakeEffects();
		this.PlayDeathSound();
	}

	protected override void EffectsDestroyed(float xI, float yI, float force)
	{
		this.MakeEffects();
		this.PlayDeathSound();
	}

	public override void Weaken()
	{
		this.DamageInternal(this.health + 1, 0f, 100f);
	}

	protected virtual void MakeEffects()
	{
		if (!this.exploded)
		{
			EffectsController.CreateShrapnel(this.shrapnelPrefab, this.x, this.y, 12f, 150f, 7f, 0f, 120f);
			EffectsController.CreateShrapnel(this.shrapnelBitPrefab, this.x, this.y, 12f, 150f, 8f, 0f, 120f);
			EffectsController.CreateShrapnel(this.shrapnelBitPrefab3, this.x, this.y, 12f, 150f, 8f, 0f, 120f);
			this.exploded = true;
			if (this.containsPresent && !Map.isEditing)
			{
				this.pickup.Launch(this.x, this.y, 0f, 200f);
				EffectsController.CreatePuffDisappearRingEffect(this.x, this.y, 0f, 0f);
				if (this.aboveBlock != null)
				{
					this.aboveBlock.Damage(new DamageObject(10, DamageType.Crush, 0f, 100f, null));
				}
			}
		}
	}

	protected override void PlayHitSound()
	{
	}

	protected override void Land()
	{
		this.health = this.collapseHealthPoint;
		this.Collapse(0f, 0f, 1f);
	}

	protected override void Bounce()
	{
		base.Bounce();
		this.health = this.collapseHealthPoint;
		this.Collapse(0f, this.yI, 1f);
	}

	protected override void HitUnits()
	{
		if (Map.HitUnits(this, this, 10, 20, DamageType.Crush, 6f, this.x, this.y - this.halfHeight - 4f, 0f, this.yI, true, false))
		{
			this.health = this.collapseHealthPoint;
			this.Collapse(0f, 100f, 1f);
		}
		if (Map.HitLivingHeroes(this, -1, 0, DamageType.Bullet, 6f, 6f, this.x, this.y - this.halfHeight, 0f, this.yI, false, false, true))
		{
			this.health = this.collapseHealthPoint;
			this.Collapse(0f, 100f, 1f);
		}
		else if (Map.HitLivingHeroes(this, -1, 3, DamageType.Bullet, 6f, 6f, this.x, this.y - this.halfHeight, 0f, this.yI, false, false, false))
		{
			this.health = this.collapseHealthPoint;
			this.Collapse(0f, 100f, 1f);
		}
	}

	private Pickupable pickup;

	protected bool exploded;

	public Shrapnel shrapnelBitPrefab3;

	public bool containsPresent;
}
