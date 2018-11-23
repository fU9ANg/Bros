// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BombardmentAction : TriggerAction
{
	public BombardmentAction()
	{
		this.seed = UnityEngine.Random.Range(0, 1000);
	}

	public override TriggerActionInfo Info
	{
		get
		{
			return this.info;
		}
		set
		{
			this.info = (BombardmentActionInfo)value;
		}
	}

	public override void Start()
	{
		this.state = TriggerActionState.Done;
		float blocksX = Map.GetBlocksX(this.info.targetPoint.collumn - Map.lastXLoadOffset);
		float blocksY = Map.GetBlocksY(this.info.targetPoint.row - Map.lastYLoadOffset);
		TriggerManager.CreateScriptedBombardment(new Vector3(blocksX, blocksY, 0f), this.info.repeat, this.seed);
	}

	public override void Update()
	{
	}

	private const float range = 5f;

	protected BombardmentActionInfo info;

	private int seed = -1;
}
