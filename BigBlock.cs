// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class BigBlock : Block
{
	protected override void EffectsCollapse(float xI, float yI)
	{
		EffectsController.CreateShrapnel(this.shrapnelPrefab, base.transform.position.x + 8f, base.transform.position.y - 8f, 16f, 16f, 16f, 0f, 0f);
		this.PlayDeathSound();
	}

	protected override void EffectsDestroyed(float xI, float yI, float force)
	{
		EffectsController.CreateShrapnel(this.shrapnelPrefab, base.transform.position.x + 8f, base.transform.position.y - 8f, 16f, 96f, 16f, 0f, 0f);
		this.PlayDeathSound();
	}

	public override void Collapse(float xI, float yI, float chance)
	{
		if (this.cannotCrumbleFromBelow && chance < 1f)
		{
			return;
		}
		base.Collapse(xI, yI, chance);
		if (!this.disturbedAbove)
		{
			this.disturbedAbove = true;
			this.aboveBlock = Map.GetBlock(this.collumn, this.row + 1);
			if (this.aboveBlock != null)
			{
				this.aboveBlock.DisturbNetworked();
			}
			this.aboveBlock = Map.GetBlock(this.collumn + 1, this.row + 1);
			if (this.aboveBlock != null)
			{
				this.aboveBlock.DisturbNetworked();
			}
		}
	}

	protected override void DelayedCollapseAbove()
	{
	}

	public bool cannotCrumbleFromBelow = true;

	protected bool disturbedAbove;
}
