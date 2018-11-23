// dnSpy decompiler from Assembly-CSharp.dll
using System;

public abstract class TriggerAction
{
	public abstract TriggerActionInfo Info { get; set; }

	public abstract void Update();

	public abstract void Start();

	public virtual void Reset()
	{
		this.timeOffsetLeft = this.Info.timeOffset;
		this.state = TriggerActionState.Waiting;
	}

	public float timeOffsetLeft;

	public bool repeat;

	public TriggerActionState state;
}
