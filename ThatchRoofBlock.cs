// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ThatchRoofBlock : RoofBlock
{
	protected override void Start()
	{
		base.Start();
		this.ShowForeground();
	}

	public override void Collapse(float xI, float yI, float chance)
	{
		base.Collapse(xI, yI, chance);
	}

	public override void DamageInternal(int damage, float xI, float yI)
	{
		base.DamageInternal(damage, xI, yI);
		if (this.health - damage > this.collapseHealthPoint)
		{
			UnityEngine.Debug.Log("MAke Thatch Particles ");
			EffectsController.CreateShrapnel(this.otherShrpnelPrefab, this.x, this.y, 2f, 90f, 3f, 0f, 50f);
		}
		else
		{
			EffectsController.CreateShrapnel(this.otherShrpnelPrefab, this.x, this.y, 2f, 90f, 12f, 0f, 50f);
		}
	}

	protected override void MakeEffects()
	{
		if (!this.exploded)
		{
			EffectsController.CreateShrapnel(this.woodPrefab, this.x, this.y, 2f, 150f, 5f, 0f, 0f);
			EffectsController.CreateShrapnel(this.shrapnelBitPrefab, this.x, this.y, 2f, 150f, 11f, 0f, 180f);
		}
		base.MakeEffects();
	}

	protected override void PlayHitSound()
	{
		this.PlayDeathSound();
	}

	protected override void Land()
	{
		base.Land();
		this.PlayDeathSound();
	}

	public void ShowForeground()
	{
		int num = base.FindBigBlockFirstCollumn(this.collumn, GroundType.ThatchRoof);
		int num2 = base.FindBigBlockTopRow(this.row, GroundType.ThatchRoof);
		int num3 = base.FindBigBlockLastCollumn(this.collumn, GroundType.ThatchRoof);
		BlockPiece blockPiece = base.AddBlockPiece(this.blockPiecePrefabs, false, true);
		SpriteSM component = blockPiece.GetComponent<SpriteSM>();
		int num4;
		if (num == num3)
		{
			num4 = 1;
		}
		else if (num == this.collumn)
		{
			num4 = 0;
		}
		else if (num3 == this.collumn)
		{
			num4 = 5;
		}
		else
		{
			int num5 = this.random.Range(0, 19);
			num4 = 1 + (this.collumn - num + num5) % 4;
		}
		int num6;
		if (num2 == this.row)
		{
			num6 = 0;
		}
		else
		{
			num6 = 1;
			if (this.EvenMoreVariation_FuckYouJarred_IfThatsEvenYourRealName)
			{
				num6 += UnityEngine.Random.Range(0, 3);
			}
		}
		int num7 = 24 + num6 * 32;
		component.SetLowerLeftPixel((float)(16 * num4), (float)num7);
		component.SetOffset(new Vector3(0f, component.offset.y, component.offset.z + (float)(num2 - this.row) * 0.025f));
	}

	protected override void SetBelowBlockCollapsedVisuals()
	{
		Block backgroundBlock = Map.GetBackgroundBlock(this.collumn, this.row - 1);
		if (backgroundBlock != null)
		{
			backgroundBlock.SetCollapsedAboveVisuals();
		}
		Block block = Map.GetBlock(this.collumn, this.row - 1);
		if (block != null && block.groundType == this.groundType)
		{
			block.SetCollapsedAboveVisuals();
		}
	}

	public BlockPiece[] blockPiecePrefabs;

	public Shrapnel woodPrefab;

	public bool EvenMoreVariation_FuckYouJarred_IfThatsEvenYourRealName;
}
