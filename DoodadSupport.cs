// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DoodadSupport : Doodad
{
	protected override void Start()
	{
		base.Start();
		this.SupportedBlock = Map.GetBlock(this.collumn, this.row + 2);
		this.blockBelow = Map.GetBlock(this.collumn, this.row - 1);
		if (this.SupportedBlock != null)
		{
		}
		UnityEngine.Debug.LogError("DOODAD SUPPORT IS DEPRECIATED!... USE SCAFFOLDING ");
	}

	public override void Collapse()
	{
		base.Collapse();
		if (this.SupportedBlock != null)
		{
			Block supportedBlock = this.SupportedBlock;
			this.SupportedBlock = null;
			supportedBlock.Collapse(0f, 0f, 1f);
		}
	}

	public Block SupportedBlock;

	public Block blockBelow;
}
