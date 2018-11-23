// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookTrooperAI : PolymorphicAI
{
	protected override void Awake()
	{
		base.Awake();
	}

	public override void FullyAlert(float x, float y, int playerNum)
	{
		base.FullyAlert(x, y, playerNum);
	}

	protected override void DoAlertedThink()
	{
		if (!HeroController.CanSeePlayer(this.unit.x, this.unit.y, this.walkDirection, (float)this.sightRangeX, (float)this.sightRangeY, ref this.seenPlayerNum))
		{
			this.thoughtsSincePlayerSeen++;
			if (this.thoughtsSincePlayerSeen > 2)
			{
				this.walkDirection = ((UnityEngine.Random.value <= 0.5f) ? -1 : 1);
			}
		}
		else
		{
			this.thoughtsSincePlayerSeen = 0;
			if (!HeroController.IsPlayerThisWay(this.seenPlayerNum, this.mook.x, this.mook.y, this.walkDirection))
			{
				this.walkDirection = -this.walkDirection;
				base.AddAction(EnemyActionType.Move, new GridPoint(this.unit.collumn + this.walkDirection, this.unit.row));
				base.AddAction(EnemyActionType.Wait, this.sightDelay);
			}
		}
		base.AddAction(EnemyActionType.Fire, this.attackTime);
		if (this.thoughtsSincePlayerSeen > 1 && UnityEngine.Random.value > 0.7f && HeroController.IsPlayerThisWay(this.seenPlayerNum, this.mook.x, this.mook.y, this.walkDirection) && Mathf.Abs(HeroController.GetPlayerPos(this.seenPlayerNum).y - this.mook.y) < 36f)
		{
			base.AddAction(EnemyActionType.Wait, 0.27f);
			base.AddAction(EnemyActionType.Fire, this.attackTime);
		}
		for (int i = 0; i < 3; i++)
		{
			if (UnityEngine.Random.value > 0.7f && base.IsGridPointAvailable(this.unit.collumn + this.walkDirection, this.unit.row))
			{
				base.AddAction(EnemyActionType.Wait, 0.2f);
				base.AddAction(EnemyActionType.Move, new GridPoint(this.unit.collumn + this.walkDirection, this.unit.row));
				base.AddAction(EnemyActionType.Wait, 0.4f);
			}
			else if (UnityEngine.Random.value <= 0.5f || this.actionQueue[this.actionQueue.Count - 1].type == EnemyActionType.Fire)
			{
				base.AddAction(EnemyActionType.Wait, 0.4f);
			}
		}
		base.AddAction(EnemyActionType.Wait, this.delayBetweenFiring);
	}
}
