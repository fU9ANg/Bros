// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class StatueBlock : BrickBlock
{
	public override void ShowForeground(bool isEarth, bool isBrick, bool onlyEdges)
	{
		int num = base.FindBigBlockFirstCollumn(this.collumn, GroundType.Statue);
		int num2 = base.FindBigBlockTopRow(this.row, GroundType.Statue);
		int num3 = base.FindBigBlockBottomRow(this.row, GroundType.Statue);
		int num4 = base.FindBigBlockLastCollumn(this.collumn, GroundType.Statue);
		int num5 = 1 + num4 - num;
		int num6 = 1 + num2 - num3;
		int num7 = num2 - this.row;
		int num8 = this.collumn - num;
		BlockPiece blockPiece = base.AddBlockPiece(this.blockPiecePrefabs, false, true);
		SpriteSM component = blockPiece.GetComponent<SpriteSM>();
		if (num5 == 1)
		{
			if (num6 > 1)
			{
				component.SetLowerLeftPixel(48f, (float)(64 + num7 % 2 * 16));
				if (num7 % 2 == 0)
				{
					this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn, this.row + 1));
				}
				else
				{
					this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn, this.row - 1));
				}
			}
			else
			{
				component.SetLowerLeftPixel(48f, 32f);
			}
		}
		else if (num6 == 1)
		{
			component.SetLowerLeftPixel((float)(48 + num8 % 2 * 16), 16f);
			if (num8 % 2 == 0)
			{
				this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn + 1, this.row));
			}
			else
			{
				this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn - 1, this.row));
			}
		}
		else if (num6 <= 3)
		{
			if (num7 % 3 == 2)
			{
				component.SetLowerLeftPixel((float)(num8 % 2 * 16), 64f);
				if (num8 % 2 == 0)
				{
					this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn + 1, this.row));
				}
				else
				{
					this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn - 1, this.row));
				}
				this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn, this.row + 1));
			}
			else
			{
				component.SetLowerLeftPixel((float)(num8 % 2 * 16), (float)(16 + num7 % 3 * 16));
				if (num8 % 2 == 0)
				{
					this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn + 1, this.row));
				}
				else
				{
					this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn - 1, this.row));
				}
				if (num7 % 3 == 0)
				{
					this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn, this.row + 1));
				}
				else
				{
					this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn, this.row - 1));
					if (num2 > this.row)
					{
						this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn, this.row + 1));
					}
				}
			}
		}
		else
		{
			component.SetLowerLeftPixel((float)(num8 % 2 * 16), (float)(16 + num7 % 4 * 16));
			if (num8 % 2 == 0)
			{
				this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn + 1, this.row));
			}
			else
			{
				this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn - 1, this.row));
			}
			switch (num7 % 4)
			{
			case 0:
				if (num3 < this.row)
				{
					this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn, this.row - 1));
				}
				break;
			case 1:
			case 2:
				if (num3 < this.row)
				{
					this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn, this.row - 1));
				}
				if (num2 > this.row)
				{
					this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn, this.row + 1));
				}
				break;
			case 3:
				this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn, this.row + 1));
				break;
			}
		}
		base.ShowForeground(false, true, true);
		if (this.rememberOriginalSurrounds)
		{
			base.AssignOriginalSurrounds();
		}
	}

	protected override void DelayedCollapseAbove()
	{
		for (int i = 0; i < this.otherBigBlockPieces.Count; i++)
		{
			if (this.otherBigBlockPieces[i] != null && this.otherBigBlockPieces[i].health > 0)
			{
				this.otherBigBlockPieces[i].Collapse(0f, 0f, 1.1f);
			}
		}
		this.collapseChance = 0.001f;
		base.DelayedCollapseAbove();
	}

	public BlockPiece[] blockPiecePrefabs;

	protected List<Block> otherBigBlockPieces = new List<Block>();
}
