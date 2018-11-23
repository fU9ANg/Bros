// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SuicideAI : PolymorphicAI
{
	protected override void DoIdleThink()
	{
		base.DoIdleThink();
		if (UnityEngine.Random.value < 0.2f)
		{
			base.AddAction(EnemyActionType.UseSpecial2, 1.5f);
		}
	}

	public override void FullyAlert(float x, float y, int playerNum)
	{
		if (this.mentalState != MentalState.Alerted && this.mentalState != MentalState.Attracted && this.mentalState != MentalState.Panicking)
		{
			base.FullyAlert(x, y, playerNum);
			if (playerNum > -1)
			{
				this.lastSeenPlayerPosition = HeroController.GetPlayerPosition(this.seenPlayerNum);
			}
			base.AddAction(EnemyActionType.PlaySpecialSound);
		}
	}

	protected override void DoAlertedThink()
	{
		if (this.seenPlayerNum >= 0)
		{
			if (HeroController.CanSeePlayer(this.unit.x, this.unit.y, this.walkDirection, (float)this.sightRangeX, (float)this.sightRangeY, ref this.seenPlayerNum))
			{
				this.lastSeenPlayerPosition = HeroController.GetPlayerPosition(this.seenPlayerNum);
			}
			this.walkDirection = (int)Mathf.Sign(this.lastSeenPlayerPosition.x - this.unit.x);
		}
		else
		{
			this.walkDirection = ((UnityEngine.Random.value >= 0.8f) ? (-this.walkDirection) : this.walkDirection);
		}
		base.AddAction(EnemyActionType.Move, new GridPoint(this.unit.collumn + this.walkDirection * UnityEngine.Random.Range(1, 3), this.unit.row));
	}

	protected override void Update()
	{
		base.Update();
		if (this.hasLitFuse)
		{
			this.suicideDelay -= this.t;
			this.suicideTimer -= this.t;
		}
	}

	public override void Blind()
	{
		base.Blind();
		base.AddAction(EnemyActionType.PlaySpecialSound);
	}

	public override void GetInput(ref bool left, ref bool right, ref bool up, ref bool down, ref bool jump, ref bool fire, ref bool special1, ref bool special2, ref bool special3, ref bool special4)
	{
		special2 = false;
		if (this.suicideTimer <= 0f)
		{
			fire = true;
			return;
		}
		base.GetInput(ref left, ref right, ref up, ref down, ref jump, ref fire, ref special1, ref special2, ref special3, ref special4);
		if (this.mentalState == MentalState.Alerted)
		{
			if (left || right)
			{
				this.hasLitFuse = true;
			}
			if (this.suicideDelay <= 0f && (left || right))
			{
				if (base.IsBlockedForward())
				{
					this.suicideTimer -= this.t * 2.5f;
				}
				if (this.seenPlayerNum >= 0)
				{
					if (HeroController.IsPlayerNearby(this.mook.x, this.mook.y, this.walkDirection, 24f, 24f))
					{
						fire = true;
					}
				}
				else if (Map.IsUnitNearby(-1, this.mook.x, this.mook.y, 24f, 24f, true))
				{
					fire = true;
				}
			}
		}
	}

	protected float chargeCounter;

	protected float suicideTimer = 4f;

	protected float blindTime;

	protected float suicideDelay = 0.5f;

	protected bool hasLitFuse;

	protected int cigarreteCount;

	private Vector3 lastSeenPlayerPosition;
}
