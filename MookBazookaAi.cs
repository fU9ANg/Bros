// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookBazookaAi : PolymorphicAI
{
	protected override void DoAlertedThink()
	{
		if (!HeroController.CanSeePlayer(this.unit.x, this.unit.y, this.walkDirection, (float)this.sightRangeX, (float)this.sightRangeY, ref this.seenPlayerNum))
		{
			this.thoughtsSincePlayerSeen++;
			if (this.thoughtsSincePlayerSeen > 1)
			{
				base.SetMentalState(MentalState.Suspicious);
				base.AddAction(EnemyActionType.Wait, 0.5f);
				this.RestartQuestionBubble_Networked();
				this.thoughtsSincePlayerSeen = 0;
			}
		}
		else
		{
			this.thoughtsSincePlayerSeen = 0;
		}
		if (this.mentalState == MentalState.Alerted)
		{
			base.AddAction(EnemyActionType.Wait, this.delayBetweenFiring / 2f);
			base.AddAction(EnemyActionType.Fire, this.attackTime);
			base.AddAction(EnemyActionType.Wait, this.delayBetweenFiring / 2f);
		}
		for (int i = 0; i < 3; i++)
		{
			if (UnityEngine.Random.value > 0.7f && base.IsGridPointAvailable(this.unit.collumn + this.walkDirection, this.unit.row))
			{
				base.AddAction(EnemyActionType.Move, new GridPoint(this.unit.collumn + this.walkDirection, this.unit.row));
				base.AddAction(EnemyActionType.Wait, 0.4f);
			}
			else if (UnityEngine.Random.value <= 0.5f || this.actionQueue[this.actionQueue.Count - 1].type == EnemyActionType.Fire)
			{
				base.AddAction(EnemyActionType.Wait, 0.4f);
			}
		}
	}
}
