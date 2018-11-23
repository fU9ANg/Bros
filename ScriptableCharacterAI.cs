// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class ScriptableCharacterAI : PolymorphicAI
{
	protected override void Think()
	{
		this.mentalState = MentalState.Idle;
		base.AddAction(EnemyActionType.Wait, 1f);
	}
}
