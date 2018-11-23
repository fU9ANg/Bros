// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class VariableAction : TriggerAction
{
	public override TriggerActionInfo Info
	{
		get
		{
			return this.info;
		}
		set
		{
			this.info = (VariableActionInfo)value;
		}
	}

	public override void Start()
	{
		this.state = TriggerActionState.Done;
		TriggerManager.AlterVariableValue(this.info.variableName, this.info.alterByValue);
	}

	public override void Update()
	{
	}

	protected VariableActionInfo info;
}
