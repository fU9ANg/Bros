// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class TankAIBig : TankAI
{
	protected override void Awake()
	{
		this.walkDirection = 1;
		base.Awake();
		this.moveLeft1Frame = true;
	}

	protected override void ThinkAboutSpecial()
	{
		if (HeroController.IsPlayerNearby(this.tank.x, this.tank.y + 20f, 90f, 120f, ref this.targetX, ref this.targetY, ref this.seenPlayerNum))
		{
			this.special1Frame = true;
			if (this.thinkCounter < 3f)
			{
				this.thinkCounter = 3f;
			}
		}
	}

	protected void BecomeAware(bool turnAround)
	{
		this.thinkCount = -1;
		this.isAwareOfPlayer = true;
		this.isAlerted = true;
		this.RestartExclamationBubble_Networked();
		if (turnAround)
		{
			this.TurnAround();
			this.thinkCounter = 1.5f;
		}
		else
		{
			this.thinkCounter = 1f;
		}
		if (this.firstSighting)
		{
			this.thinkCounter = 6.7f;
		}
		this.firstSighting = false;
	}

	protected override void ThinkLookForPlayer(float timeM, bool turnAround)
	{
		if (SetResolutionCamera.IsItSortOfVisible(base.transform.position) && HeroController.CanSeePlayer(this.tank.x, this.tank.y + 40f, this.walkDirection, this.tankSightXRange + 100f, this.tankSightYRange, ref this.seenPlayerNum))
		{
			this.BecomeAware(false);
		}
		else if (SetResolutionCamera.IsItSortOfVisible(base.transform.position) && HeroController.CanSeePlayer(this.tank.x, this.tank.y + 40f, -this.walkDirection, this.tankSightXRange + 100f, this.tankSightYRange, ref this.seenPlayerNum))
		{
			this.BecomeAware(true);
		}
		else
		{
			this.thinkState = EnemyActionState.Idle;
			if (turnAround && this.walkDirection > 0)
			{
				this.TurnAround();
			}
			if (this.isAlerted)
			{
				this.thinkCounter = 0.4f * timeM;
			}
			else
			{
				this.thinkCounter = 0.7f * timeM;
			}
		}
	}

	protected override void ThinkPatrolMove(float timeM)
	{
		this.thinkCounter = 0.2f;
	}

	protected override void CheckNearbyBlocks()
	{
		if (this.startX < 0f)
		{
			this.startX = this.tank.x;
		}
		this.xMin = this.startX - 150f;
		this.xMax = this.startX + 460f;
	}

	public override void GetTankMovement(ref bool left, ref bool right, ref bool fire, ref bool fireLeft, ref bool fireRight, ref bool special)
	{
		special = false; left = (right = (fireLeft = (fireRight = (special ))));
		switch (this.thinkState)
		{
		case EnemyActionState.Moving:
			if (this.walkDirection > 0)
			{
				if (this.tank.x >= this.xMax)
				{
					this.tank.x = this.xMax;
					this.thinkCounter = 0.01f;
					this.thinkCount = -1;
					this.tank.Stop();
				}
				else
				{
					right = true;
				}
			}
			else if (this.walkDirection < 0)
			{
				if (this.tank.x <= this.xMin)
				{
					this.tank.x = this.xMin;
					this.thinkCounter = 0.01f;
					this.thinkCount = -1;
					this.tank.Stop();
				}
				else
				{
					left = true;
				}
			}
			break;
		case EnemyActionState.Shooting:
			if (this.targetX > this.tank.x && this.tank.facingDirection > 0)
			{
				fireRight = true;
			}
			else if (this.targetX < this.tank.x && this.tank.facingDirection < 0)
			{
				fireLeft = true;
			}
			else
			{
				fire = true;
			}
			if (fireRight && this.tank.x > this.xMax)
			{
				left = true;
			}
			else if (fireLeft && this.tank.x < this.xMax)
			{
				right = true;
			}
			break;
		}
		if (this.moveLeft1Frame)
		{
			this.moveLeft1Frame = false;
			left = true;
		}
		if (this.moveRight1Frame)
		{
			this.moveRight1Frame = false;
			right = true;
		}
		if (this.special1Frame)
		{
			this.special1Frame = false;
			special = true;
		}
	}

	protected float startX = -100f;

	protected bool firstSighting = true;
}
