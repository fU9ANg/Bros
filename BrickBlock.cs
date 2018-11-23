// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class BrickBlock : DirtBlock
{
	public override void ShowForeground(bool isEarth, bool isBrick, bool onlyEdges)
	{
		if (!onlyEdges)
		{
			base.ShowBrokenBackground(this.foregroundBrickPrefabs, false, true, false, true);
		}
		base.ShowForeground(false, true, true);
	}

	public override void ShowTopEdge()
	{
		if (!this.destroyed && this.health > 0 && this.topEdge == null)
		{
			this.topEdge = base.AddEdgePiece(this.foregroundPrefabs.topEdges);
		}
		else if (!this.topSolidAtStart && this.health <= 0 && this.backgroundEdgesPrefabs != null)
		{
			this.topEdge = base.AddForegroundPiece(this.backgroundEdgesPrefabs.topEdges, 7f);
		}
	}

	public override void ShowLeftEdge()
	{
		if (!this.destroyed && this.health > 0 && this.leftEdge == null)
		{
			this.leftEdge = base.AddEdgePiece(this.foregroundPrefabs.leftEdges);
		}
		else if (!this.leftSolidAtStart && this.health <= 0 && this.backgroundEdgesPrefabs != null)
		{
			this.leftEdge = base.AddForegroundPiece(this.backgroundEdgesPrefabs.leftEdges, 7f);
		}
	}

	public override void ShowRightEdge()
	{
		if (!this.destroyed && this.health > 0 && this.rightEdge == null)
		{
			this.rightEdge = base.AddEdgePiece(this.foregroundPrefabs.rightEdges);
		}
		else if (!this.rightSolidAtStart && this.health <= 0 && this.backgroundEdgesPrefabs != null)
		{
			this.rightEdge = base.AddForegroundPiece(this.backgroundEdgesPrefabs.rightEdges, 7f);
		}
	}

	public override void ShowBottomEdge()
	{
		if (!this.destroyed && this.health > 0 && this.bottomEdge == null)
		{
			this.bottomEdge = base.AddEdgePiece(this.foregroundPrefabs.bottomEdges);
		}
		else if (!this.bottomSolidAtStart && this.health <= 0 && this.backgroundEdgesPrefabs != null)
		{
			this.bottomEdge = base.AddForegroundPiece(this.backgroundEdgesPrefabs.bottomEdges, 7f);
		}
	}

	public BlockBackgroundPrefabs foregroundBrickPrefabs;
}
