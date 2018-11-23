// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class SpawnBlockAction : TriggerAction
{
	public override TriggerActionInfo Info
	{
		get
		{
			return this.info;
		}
		set
		{
			this.info = (SpawnBlockActionInfo)value;
		}
	}

	public override void Start()
	{
		this.state = TriggerActionState.Done;
		TriggerManager.CreateScriptedBlock(this.info.targetPoint, (GroundType)this.info.groundType, this.info.disturbed);
	}

	public override void Update()
	{
	}

	protected SpawnBlockActionInfo info;
}
