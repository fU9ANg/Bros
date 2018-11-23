// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SandBlock : DirtBlock
{
	protected override void Start()
	{
		base.Start();
		this.collapseDelayTime = 0.05f;
	}

	public override void StepOn(TestVanDammeAnim unit)
	{
		if (((unit.IsMine && !unit.IsEnemy) || unit.IsEnemy) && !this.disturbed && !Map.isEditing)
		{
			Networking.RPC(PID.TargetAll, new RpcSignature(this.StepOnBlock), false);
		}
	}

	public override void Collapse(float xI, float yI, float chance)
	{
		if (!this.destroyed)
		{
			this.collapseChance = Mathf.Min(this.collapseChance, chance);
			if (this.collapseChance >= 1f)
			{
				if (this.lastDamageObject != null)
				{
					this.EffectsDestroyed(xI, yI, (float)(65 + 24 * this.lastDamageObject.damage));
				}
				else
				{
					this.EffectsDestroyed(xI, yI, 140f);
				}
			}
			else
			{
				this.EffectsCollapse(xI, yI);
			}
			this.DestroyBlockInternal(true);
		}
	}

	protected override void RunCollapseLogic()
	{
		this.collapseChance = 1f;
		base.RunCollapseLogic();
	}

	public override void StepOnBlock()
	{
		if (!this.disturbed)
		{
			this.collapseDelay = 0.24f;
			this.disturbed = true;
		}
		base.DisturbNetworked();
	}

	protected override void Update()
	{
		base.Update();
		if (this.collapseDelay > 0f)
		{
			this.collapseDelay -= Time.deltaTime;
			if (this.collapseDelay <= 0f)
			{
				this.health = 0;
				this.collapseChance = 0.1f;
				this.Collapse(0f, 0f, 1f);
			}
		}
	}

	protected override void EffectsDestroyed(float xI, float yI, float force)
	{
		EffectsController.CreateSandParticles(this.x, this.y, 45, 5f, force, 0f, 40f);
		this.PlayDeathSound();
	}

	protected override void EffectsCollapse(float xI, float yI)
	{
		EffectsController.CreateSandParticles(this.x, this.y, 33, 7f, 40f, 0f, 0f);
		this.PlayDeathSound();
	}

	public override void CrumbleBridge(float chance)
	{
		if (!this.disturbed)
		{
			this.disturbed = true;
			if (chance <= 0f)
			{
				this.collapseDelay = 0.02f;
			}
			else
			{
				this.collapseDelayTime = 0.15f;
				this.collapseDelay = 0.15f;
			}
			this.collapseChance = 0.1f;
		}
	}

	protected override void CrumbleBridges(float chance)
	{
		base.CrumbleBridges(chance);
		if (Physics.Raycast(base.transform.position, Vector3.down, out this.groundHit, 24f, this.groundLayer))
		{
			this.groundHit.collider.gameObject.SendMessage("CrumbleBridge", -1, SendMessageOptions.DontRequireReceiver);
		}
	}

	protected float collapseDelay;
}
