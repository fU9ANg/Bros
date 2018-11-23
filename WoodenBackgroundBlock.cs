// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WoodenBackgroundBlock : RoofBlock
{
	protected override void Start()
	{
		base.Start();
		this.ShowForeground();
		this.invulnerable = false;
		this.destroyed = false;
	}

	public override void Collapse(float xI, float yI, float chance)
	{
		base.Collapse(xI, yI, chance);
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void StartFalling()
	{
		base.StartFalling();
	}

	protected override void ClearBlock()
	{
		Map.SetBackgroundBlockEmpty(this, this.collumn, this.row);
	}

	protected override void HitUnits()
	{
		if (Map.HitUnits(this, this, 10, 20, DamageType.Crush, 6f, this.x, this.y - this.halfHeight - 4f, 0f, this.yI, true, false))
		{
			this.health = this.collapseHealthPoint;
			this.Collapse(0f, 100f, 1f);
		}
	}

	public override void SetCollapsedAboveVisuals()
	{
		base.SetCollapsedAboveVisuals();
		if (this.backgroundSprite != null && this.spriteRow == 1 && Map.GetBackgroundGroundType(this.collumn, this.row + 1, GroundType.WoodBackground) == GroundType.WoodBackground)
		{
			this.spriteRow = 0;
			if (Map.WasBlockEarth(this.collumn, this.row - 1))
			{
				this.backgroundSprite.SetPixelDimensions(16 + this.pixelWidthAdd, 32);
				this.backgroundSprite.RecalcTexture();
				this.backgroundSprite.SetSize((float)(16 + this.pixelWidthAdd), 32f);
				this.backgroundSprite.SetLowerLeftPixel((float)(16 * this.spriteCollumn + this.pixelLeftOffset), (float)((this.spriteRow != 0) ? 80 : 32));
			}
			else
			{
				this.backgroundSprite.SetPixelDimensions(16 + this.pixelWidthAdd, 16);
				this.backgroundSprite.RecalcTexture();
				this.backgroundSprite.SetSize((float)(16 + this.pixelWidthAdd), 16f);
				this.backgroundSprite.SetLowerLeftPixel((float)(16 * this.spriteCollumn + this.pixelLeftOffset), (float)((this.spriteRow != 0) ? 64 : 16));
			}
		}
	}

	public void ShowForeground()
	{
		int num = base.FindBackgroundBlockFirstCollumn(this.collumn, GroundType.WoodBackground);
		this.bigBlockFirstRow = base.FindBackgroundBlockTopRow(this.row, GroundType.WoodBackground);
		int num2 = base.FindBackgroundBlockLastCollumn(this.collumn, GroundType.WoodBackground);
		BlockPiece blockPiece = base.AddBlockPiece(this.blockPiecePrefabs, false, true);
		this.backgroundSprite = blockPiece.GetComponent<SpriteSM>();
		this.spriteCollumn = 0;
		this.spriteRow = 0;
		if (num == num2)
		{
			this.spriteCollumn = 1;
		}
		else if (num == this.collumn && !Map.WasBlockEarth(num - 1, this.row))
		{
			this.spriteCollumn = 0;
		}
		else if (num2 == this.collumn && !Map.WasBlockEarth(num2 + 1, this.row))
		{
			this.spriteCollumn = 5;
		}
		else
		{
			int num3 = this.random.Range(0, 19);
			this.spriteCollumn = 1 + (this.collumn + num3) % 4;
		}
		if (this.bigBlockFirstRow == this.row)
		{
			if (Map.IsBlockSolid(this.collumn, this.bigBlockFirstRow + 1))
			{
				this.spriteRow = 1;
			}
			else
			{
				this.spriteRow = 0;
			}
		}
		else
		{
			this.spriteRow = 1;
		}
		this.pixelLeftOffset = 0;
		this.pixelWidthAdd = 0;
		this.xOffsetOffset = 0;
		if (Map.WasBlockEarth(this.collumn - 1, this.row))
		{
			this.pixelLeftOffset = -8;
			this.pixelWidthAdd = 8;
			this.xOffsetOffset -= 4;
		}
		if (Map.WasBlockEarth(this.collumn + 1, this.row))
		{
			this.pixelWidthAdd = 8;
			this.xOffsetOffset += 4;
		}
		if (Map.WasBlockEarth(this.collumn, this.row - 1))
		{
			this.backgroundSprite.SetPixelDimensions(16 + this.pixelWidthAdd, 32);
			this.backgroundSprite.RecalcTexture();
			this.backgroundSprite.SetSize((float)(16 + this.pixelWidthAdd), 32f);
			this.backgroundSprite.SetLowerLeftPixel((float)(16 * this.spriteCollumn + this.pixelLeftOffset), (float)((this.spriteRow != 0) ? 80 : 32));
			this.backgroundSprite.SetOffset(new Vector3((float)this.xOffsetOffset, this.backgroundSprite.offset.y - 8f, this.backgroundSprite.offset.z + (float)(this.bigBlockFirstRow - this.row) * 0.025f));
		}
		else
		{
			this.backgroundSprite.SetPixelDimensions(16 + this.pixelWidthAdd, 16);
			this.backgroundSprite.RecalcTexture();
			this.backgroundSprite.SetSize((float)(16 + this.pixelWidthAdd), 16f);
			this.backgroundSprite.SetLowerLeftPixel((float)(16 * this.spriteCollumn + this.pixelLeftOffset), (float)((this.spriteRow != 0) ? 64 : 16));
			this.backgroundSprite.SetOffset(new Vector3((float)this.xOffsetOffset, this.backgroundSprite.offset.y, this.backgroundSprite.offset.z + (float)(this.bigBlockFirstRow - this.row) * 0.025f));
		}
	}

	public BlockPiece[] blockPiecePrefabs;

	private int spriteCollumn;

	private int spriteRow;

	private int bigBlockFirstRow;

	private int pixelLeftOffset;

	private int pixelWidthAdd;

	private int xOffsetOffset;

	private SpriteSM backgroundSprite;
}
