// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class BrickBlockBig : BrickBlock
{
	public override void ShowForeground(bool isEarth, bool isBrick, bool onlyEdges)
	{
		int num = base.FindBigBlockFirstCollumn(this.collumn, GroundType.BigBlock);
		int num2 = base.FindBigBlockTopRow(this.row, GroundType.BigBlock);
		int num3 = (num - this.collumn) % 2;
		int num4 = (num2 - this.row) % 2;
		if (num3 == 0)
		{
			if (num4 == 0)
			{
				base.AddBlockPiece(this.foregroundBrickPrefabs.topLeftCorner, false, true);
				this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn + 1, this.row));
				this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn, this.row - 1));
			}
			else
			{
				base.AddBlockPiece(this.foregroundBrickPrefabs.bottomLeftCorner, false, true);
				this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn + 1, this.row));
				this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn, this.row + 1));
			}
		}
		else if (num4 == 0)
		{
			base.AddBlockPiece(this.foregroundBrickPrefabs.topRightCorner, false, true);
			this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn - 1, this.row));
			this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn, this.row - 1));
		}
		else
		{
			base.AddBlockPiece(this.foregroundBrickPrefabs.bottomRightCorner, false, true);
			this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn - 1, this.row));
			this.otherBigBlockPieces.Add(Map.GetBlock(this.collumn, this.row + 1));
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

	protected List<Block> otherBigBlockPieces = new List<Block>();
}
