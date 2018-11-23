// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BurnAction : TriggerAction
{
	public override TriggerActionInfo Info
	{
		get
		{
			return this.info;
		}
		set
		{
			this.info = (BurnActionInfo)value;
		}
	}

	public override void Start()
	{
		this.state = TriggerActionState.Done;
		float blocksX = Map.GetBlocksX(this.info.targetPoint.collumn);
		float blocksY = Map.GetBlocksY(this.info.targetPoint.row);
		TriggerManager.CreateScriptedBurn(new Vector3(blocksX, blocksY, 0f));
	}

	public override void Update()
	{
	}

	private const float range = 5f;

	protected BurnActionInfo info;
}
