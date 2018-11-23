// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class SpawnResourceAction : TriggerAction
{
	public override TriggerActionInfo Info
	{
		get
		{
			return this.info;
		}
		set
		{
			this.info = (SpawnResourceActionInfo)value;
		}
	}

	public override void Start()
	{
		this.state = TriggerActionState.Done;
		float blocksX = Map.GetBlocksX(this.info.targetPoint.collumn - Map.lastXLoadOffset);
		float blocksY = Map.GetBlocksY(this.info.targetPoint.row - Map.lastYLoadOffset);
		MapController.SpawnResource_Networked(this.info.resourceName, blocksX, blocksY, this.info.callMethod, this.info.newObjectTag);
	}

	public override void Update()
	{
	}

	protected SpawnResourceActionInfo info;
}
