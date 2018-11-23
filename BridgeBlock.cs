// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BridgeBlock : Block
{
	protected override void Start()
	{
		base.Start();
	}

	protected override void EffectsCollapse(float xI, float yI)
	{
		if (base.gameObject.CompareTag("Wood"))
		{
			EffectsController.CreateWoodParticles(this.x, this.y, 24, 9f, 20f, 0f, 0f, 250f);
		}
		else if (base.gameObject.CompareTag("Metal"))
		{
			EffectsController.CreateMetalParticles(this.x, this.y, 15, 9f, 40f, 0f, 0f, 320f);
			EffectsController.CreateSparkShower(this.x, this.y, 3, 7f, 100f, xI * 0.5f, 100f, 0.2f, 0f);
		}
		else if (base.gameObject.CompareTag("Wood"))
		{
			UnityEngine.Debug.LogError("Effects not implemented");
		}
		this.PlayDeathSound();
	}

	protected override void EffectsDestroyed(float xI, float yI, float force)
	{
		if (base.gameObject.CompareTag("Wood"))
		{
			EffectsController.CreateWoodParticles(this.x, this.y, 24, 9f, force, 20f, 0f, 2000f);
		}
		else if (base.gameObject.CompareTag("Metal"))
		{
			EffectsController.CreateMetalParticles(this.x, this.y, 15, 9f, 40f, 0f, 0f, 320f);
			EffectsController.CreateSparkShower(this.x, this.y, 3, 7f, 100f, xI * 0.5f, 100f, 0.2f, 0f);
		}
		else
		{
			UnityEngine.Debug.LogError("Effects not implemented");
		}
		this.PlayDeathSound();
	}

	protected override int BurnCollapsePoint()
	{
		return this.health * 5;
	}

	protected override void CrumbleBridges(float chance)
	{
		chance = this.collapseChance;
		base.CrumbleBridges(chance);
	}

	public override void CrumbleBridge(float chance)
	{
		base.CrumbleBridge(chance);
		if (chance <= 0.7f && !this.IsConnectedToGround())
		{
			chance = 2f;
		}
		if (chance > 0.7f)
		{
			this.health = 0;
			this.collapseChance = chance;
			this.Collapse(0f, 0f, this.collapseChance * 0.8f);
		}
	}

	public override void Collapse(float xI, float yI, float chance)
	{
		base.Collapse(xI, yI, chance);
		if (base.BelowBlock != null)
		{
			base.BelowBlock.CrumbleBridge(1f);
		}
	}

	public override bool IsSolid()
	{
		return this.IsConnectedToGround();
	}

	protected virtual bool IsConnectedToGround()
	{
		int num = 0;
		bool flag = false;
		bool flag2 = false;
		int num2 = this.collumn;
		while (num < 20 && !flag && !flag2)
		{
			num++;
			num2++;
			Block block = Map.GetBlock(num2, this.row);
			if (block != null && block.health > 0)
			{
				if (block.groundType != GroundType.Bridge || block.groundType != GroundType.Bridge2)
				{
					flag = true;
				}
			}
			else
			{
				flag2 = true;
			}
		}
		if (flag)
		{
			return true;
		}
		num = 0;
		num2 = this.collumn;
		flag = false;
		flag2 = false;
		while (num < 20 && !flag && !flag2)
		{
			num++;
			num2--;
			Block block2 = Map.GetBlock(num2, this.row);
			if (block2 != null && block2.health > 0)
			{
				if (block2.groundType != GroundType.Bridge)
				{
					flag = true;
				}
			}
			else
			{
				flag2 = true;
			}
		}
		return flag;
	}

	public Shrapnel otherShrpnelPrefab;

	public Renderer railing;
}
