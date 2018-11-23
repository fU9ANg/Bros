// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class CutsceneSatan : CutsceneAI
{
	public override void Shoot()
	{
		base.Shoot();
		this.actionState = ActionState.Idle;
	}
}
