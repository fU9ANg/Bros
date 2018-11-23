// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DoodadScaffolding : Doodad
{
	protected override void Start()
	{
		base.Start();
		Map.RegisterStaticDoodad(this);
		if (this.offerRoofSupport)
		{
			this.SupportedBlock = Map.GetBlock(this.collumn, this.row + 2);
			this.blockBelow = Map.GetBlock(this.collumn, this.row - 1);
			if (this.SupportedBlock != null)
			{
				this.SupportedBlock.supportedBy = this;
			}
		}
	}

	protected virtual void Update()
	{
		if (this.firstFrame)
		{
			this.firstFrame = false;
			this.aboveDoodad = Map.GetStaticDoodad(this.collumn, this.row + 1);
			if (this.aboveDoodad == null && this.isTall)
			{
				this.aboveDoodad = Map.GetStaticDoodad(this.collumn, this.row + 2);
			}
		}
	}

	public override void Collapse()
	{
		base.Collapse();
		if (this.aboveDoodad != null)
		{
			this.aboveDoodad.Invoke("Collapse", 0.25f);
		}
		Block block = Map.GetBlock(this.collumn, this.row + 1);
		if (block != null)
		{
			block.Invoke("CollapseForced", 0.25f);
			block = Map.GetBlock(this.collumn - 1, this.row + 1);
			if (block != null && Map.GetBlock(this.collumn - 1, this.row) == null)
			{
				block.Invoke("CollapseForced", 0.5f);
			}
			block = Map.GetBlock(this.collumn + 1, this.row + 1);
			if (block != null && Map.GetBlock(this.collumn + 1, this.row) == null)
			{
				block.Invoke("CollapseForced", 0.5f);
			}
		}
		if (this.offerRoofSupport && this.SupportedBlock != null)
		{
			Block supportedBlock = this.SupportedBlock;
			this.SupportedBlock = null;
			supportedBlock.Collapse(0f, 0f, 1f);
		}
	}

	protected bool firstFrame = true;

	protected Doodad aboveDoodad;

	public bool offerRoofSupport;

	[HideInInspector]
	public Block SupportedBlock;

	[HideInInspector]
	public Block blockBelow;

	public bool isTall;
}
