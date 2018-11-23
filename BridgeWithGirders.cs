// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BridgeWithGirders : BridgeBlock
{
	protected override void Start()
	{
		base.Start();
		BridgeUnderPiece componentInChildren = base.GetComponentInChildren<BridgeUnderPiece>();
		if (base.AboveBlock != null && (base.AboveBlock.groundType == this.groundType || base.AboveBlock.groundType == GroundType.Earth || base.AboveBlock.groundType == GroundType.EarthTop || base.AboveBlock.groundType == GroundType.Brick || base.AboveBlock.groundType == GroundType.Steel || base.AboveBlock.groundType == GroundType.Roof || base.AboveBlock.groundType == GroundType.ThatchRoof))
		{
			base.GetComponent<Renderer>().enabled = false;
			this.railing.gameObject.SetActive(false);
			base.GetComponent<Collider>().enabled = false;
			if (this.railingOnTheRightHandSide != null)
			{
				this.railingOnTheRightHandSide.SetActive(false);
			}
			if (this.railingOnTheLeftHandSide != null)
			{
				this.railingOnTheLeftHandSide.SetActive(false);
			}
		}
		else
		{
			componentInChildren.gameObject.SetActive(false);
			if (base.AboveBlock == null || base.AboveBlock.groundType == GroundType.Ladder || base.AboveBlock.groundType == GroundType.WoodenBlock || base.AboveBlock.groundType == GroundType.Barrel)
			{
				if (this.railingOnTheRightHandSide != null)
				{
					Block block = Map.GetBlock(this.collumn + 1, this.row);
					if (block == null || block.groundType != this.groundType)
					{
						this.railingOnTheRightHandSide.SetActive(true);
					}
					else
					{
						this.railingOnTheRightHandSide.SetActive(false);
					}
				}
				if (this.railingOnTheLeftHandSide != null)
				{
					Block block2 = Map.GetBlock(this.collumn - 1, this.row);
					if (block2 == null || block2.groundType != this.groundType)
					{
						this.railingOnTheLeftHandSide.SetActive(true);
					}
					else
					{
						this.railingOnTheLeftHandSide.SetActive(false);
					}
				}
			}
		}
		if (base.LeftBlock == null)
		{
			this.sprite.OffsetLowerLeftPixel(0f, 32f);
		}
		else if (base.RightBlock == null)
		{
			this.sprite.OffsetLowerLeftPixel(0f, 48f);
		}
		else
		{
			int num = UnityEngine.Random.Range(0, 2) * 16;
			this.sprite.OffsetLowerLeftPixel(0f, (float)num);
		}
		if (this.collumn % 2 == 0)
		{
			this.sprite.OffsetLowerLeftPixel(16f, 0f);
		}
	}

	public GameObject railingOnTheRightHandSide;

	public GameObject railingOnTheLeftHandSide;
}
