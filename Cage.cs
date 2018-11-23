// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Cage : BigBlock
{
	protected override void Start()
	{
		base.Start();
		if (this.rescueBro != null)
		{
			this.rescueBro.playerNum = 5;
			this.rescueBro.x = base.transform.position.x + 8f;
			this.rescueBro.y = base.transform.position.y - 8f;
		}
		if (!Map.isEditing)
		{
			this.PlaceCageBlocks();
		}
	}

	protected void PlaceCageBlocks()
	{
		this.hasPlacedBlocks = true;
		Map.blocks[this.collumn, this.row] = this;
		Map.blocks[this.collumn + 1, this.row] = this;
		Map.blocks[this.collumn, this.row - 1] = this;
		Map.blocks[this.collumn + 1, this.row - 1] = this;
	}

	public override void DestroyBlockInternal(bool CollapseBlocksAround)
	{
		if (this.rescueBro != null)
		{
			this.rescueBro.Free();
		}
		if (this.bottomWall != null)
		{
			UnityEngine.Object.Destroy(this.bottomWall.gameObject);
		}
		base.DestroyBlockInternal(CollapseBlocksAround);
	}

	protected override void Update()
	{
		base.Update();
		if (!this.hasPlacedBlocks && !Map.isEditing)
		{
			this.PlaceCageBlocks();
		}
	}

	protected bool hasPlacedBlocks;

	public CageBar bottomWall;

	public RescueBro rescueBro;
}
