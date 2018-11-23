// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DoodadBeehive : FallingBlock
{
	protected override void Start()
	{
		base.Start();
		if (Physics.Raycast(base.transform.position, Vector3.up, out this.groundHit, 624f, this.groundLayer))
		{
			this.ropeLength = this.groundHit.point.y - (this.y + 6f);
			if (this.ropeLength > 500f)
			{
				this.ropeLength = 500f;
			}
			this.beehiveRope.SetSize(2f, this.ropeLength);
			this.beehiveRope.SetLowerLeftPixel(0f, (float)((int)this.ropeLength));
			this.beehiveRope.SetPixelDimensions(0, (int)this.ropeLength);
			this.attachedBlock = this.groundHit.collider.GetComponent<Block>();
			if (this.attachedBlock != null)
			{
				this.wasAttached = true;
			}
		}
	}

	protected void DetachRope()
	{
		if (!this.detached)
		{
			this.detached = true;
			this.beehiveRope.gameObject.SetActive(false);
			int num = 0;
			while ((float)num < this.ropeLength)
			{
				EffectsController.CreateDustParticles(this.x, this.y + 6f + (float)num, 1, 0.1f, 6f, 0f, 1f, new Color(0.380392164f, 0.2627451f, 0.145098045f, 0.9f));
				num += 2;
			}
			this.honeyEmitter.gameObject.SetActive(false);
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!Map.isEditing && this.wasAttached && (this.attachedBlock == null || this.attachedBlock.health < 0 || this.attachedBlock.disturbed || this.attachedBlock.destroyed))
		{
			base.DisturbNetworked();
		}
	}

	public override void Disturb()
	{
		base.Disturb();
		this.DetachRope();
	}

	protected override void Land()
	{
		base.Land();
		this.CollapseForced(0f, this.yI, 1f);
	}

	public override void Collapse(float xI, float yI, float chance)
	{
		this.honeyEmitter.enabled = false;
		base.Collapse(xI, yI, chance);
		this.DetachRope();
		this.beeSimulator.Restart();
	}

	public override void CollapseForced(float xI, float yI, float chance)
	{
		this.health = -300;
		base.CollapseForced(xI, yI, chance);
	}

	protected override void EffectsCollapse(float xI, float yI)
	{
		EffectsController.CreateGibs(this.gibHolder, base.transform.position.x, base.transform.position.y, 60f, 70f, 0f, 40f);
		EffectsController.CreateDustParticles(base.transform.position.x, base.transform.position.y, 140, 6f, 40f, 0f, 30f, new Color(0.7529412f, 0.533333361f, 0.184313729f, 0.9f));
	}

	protected override void EffectsDestroyed(float xI, float yI, float force)
	{
		EffectsController.CreateGibs(this.gibHolder, base.transform.position.x, base.transform.position.y, 140f, 170f, 0f, 140f);
		EffectsController.CreateDustParticles(base.transform.position.x, base.transform.position.y, 140, 6f, 130f, 0f, 100f, new Color(0.854901969f, 0.65882355f, 0.172549024f, 0.9f));
	}

	protected override void HitUnits()
	{
		if (Map.HitUnits(this, this, -10, 0, DamageType.Crush, 6f, this.x, this.y - this.halfHeight, 0f, this.yI, true, false))
		{
			this.CollapseForced(0f, this.yI, 1f);
		}
	}

	public SpriteSM beehiveRope;

	public RealisticAngryBeeSimulator beeSimulator;

	public GibHolder gibHolder;

	protected float ropeLength = 16f;

	protected Block attachedBlock;

	protected bool wasAttached;

	public ParticleEmitter honeyEmitter;

	protected bool detached;
}
