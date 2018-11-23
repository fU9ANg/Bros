// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TankAI : EnemyAI
{
	protected override void Awake()
	{
		base.Awake();
		this.TurnAround();
	}

	protected override void Start()
	{
		this.tank = base.GetComponent<Tank>();
		this.targetX = this.tank.x;
		this.CheckNearbyBlocks();
	}

	protected override bool ReadyToThink
	{
		get
		{
			return this.thinkCounter < 0f;
		}
	}

	protected override void Update()
	{
		this.UpdateOwnership();
		this.thinkCounter -= Time.deltaTime;
		if (this.ReadyToThink && base.IsMine)
		{
			base.Think_Networked();
		}
	}

	protected override void Think()
	{
		if (this.tank.health <= 0)
		{
			base.enabled = false;
			return;
		}
		if (this.tank.weapon.IsFiring())
		{
			this.thinkState = EnemyActionState.Idle;
			this.thinkCounter += 0.2f;
			return;
		}
		this.thinkCount++;
		this.thinkCounter += 1f;
		if (this.tank.facingDirection != this.walkDirection)
		{
			if (this.walkDirection < 0)
			{
				this.moveLeft1Frame = true;
			}
			else if (this.walkDirection > 0)
			{
				this.moveRight1Frame = true;
			}
			return;
		}
		if (this.isAwareOfPlayer)
		{
			this.DoAwareThink();
		}
		else
		{
			this.DoUnawareThink();
		}
	}

	protected virtual void DoAwareThink()
	{
		int thinkCount = this.thinkCount;
		if (thinkCount != 0)
		{
			if (thinkCount != 1)
			{
				this.thinkCount = -1;
				this.thinkCounter = 0.01f;
			}
			else
			{
				this.thinkState = EnemyActionState.Idle;
				this.thinkCounter = 2f;
				this.ThinkAboutSpecial();
			}
		}
		else
		{
			this.ThinkAboutShooting();
		}
	}

	protected virtual void DoUnawareThink()
	{
		switch (this.thinkCount)
		{
		case 0:
			this.ThinkLookForPlayer(1f, true);
			break;
		case 1:
		case 2:
		case 3:
			this.ThinkLookForPlayer(0.4f, false);
			break;
		case 4:
			this.ThinkPatrolMove(1f);
			break;
		default:
			this.thinkCounter = 0.01f;
			this.thinkCount = -1;
			break;
		}
	}

	protected virtual void ThinkAboutSpecial()
	{
	}

	protected virtual void ThinkLookForPlayer(float timeM, bool turnAround)
	{
		if (HeroController.CanSeePlayer(this.tank.x, this.tank.y + 40f, this.walkDirection, this.tankSightXRange + 100f, this.tankSightYRange, ref this.seenPlayerNum))
		{
			this.thinkCount = -1;
			this.thinkCounter = 1f;
			this.isAwareOfPlayer = true;
			this.isAlerted = true;
			this.RestartExclamationBubble_Networked();
		}
		else
		{
			this.thinkState = EnemyActionState.Idle;
			if (turnAround)
			{
				this.TurnAround();
			}
			if (this.isAlerted)
			{
				if (this.xMin == this.xMax)
				{
					this.thinkCounter = 0.7f * timeM;
				}
				else
				{
					this.thinkCounter = 0.4f * timeM;
				}
			}
			else if (this.xMin == this.xMax)
			{
				this.thinkCounter = 1f * timeM;
			}
			else
			{
				this.thinkCounter = 0.7f * timeM;
			}
		}
	}

	protected virtual void ThinkPatrolMove(float timeM)
	{
		if (HeroController.CanSeePlayer(this.tank.x, this.tank.y + 40f, this.walkDirection, this.tankSightXRange + 100f, this.tankSightYRange, ref this.seenPlayerNum))
		{
			this.thinkCount = -1;
			this.thinkCounter = 1f;
			this.isAwareOfPlayer = true;
			this.isAlerted = true;
			this.RestartExclamationBubble_Networked();
		}
		else
		{
			this.thinkState = EnemyActionState.Moving;
			this.thinkCounter = 2.5f * timeM;
		}
	}

	protected virtual void ThinkAboutShooting()
	{
		if (this.IsPlayerThisWay(this.walkDirection))
		{
			if (HeroController.IsPlayerNearby(this.tank.x, this.tank.y, this.tank.facingDirection, this.tankSightXRange, this.tankSightYRange, ref this.seenPlayerNum))
			{
				this.targetX = this.tank.x + (float)(600 * this.tank.facingDirection);
				Vector3 zero = Vector3.zero;
				HeroController.GetPlayerPos(this.seenPlayerNum, ref zero.x, ref zero.y);
				Networking.RPC<int, Vector3>(PID.TargetAll, new RpcSignature<int, Vector3>(this.tank.SetTargetPlayerNum), this.seenPlayerNum, zero, false);
				this.thinkState = EnemyActionState.Shooting;
				this.thinkCounter = this.attackTime;
			}
			else
			{
				this.thinkCount = -1;
				this.thinkCounter = this.turnThinkDelay;
			}
		}
		else
		{
			this.TurnAround();
			this.thinkCounter = this.turnThinkDelay + 0.5f;
			this.thinkCount = -1;
			this.notFoundCount++;
			if (this.notFoundCount > 2)
			{
				this.ForgetPlayer();
				this.notFoundCount = 0;
			}
			this.ThinkAboutSpecial();
		}
	}

	public override void ForgetPlayer()
	{
		this.isAwareOfPlayer = false;
		this.thinkCounter = 0.1f;
		this.thinkCount = -1;
		base.RestartQuestionBubble_Networked();
		this.CheckNearbyBlocks();
		this.notFoundCount = 0;
		this.seenPlayerNum = -1;
	}

	public virtual void GetKopterMovement(ref bool up, ref bool down, ref bool left, ref bool right, ref bool fire, ref bool fireLeft, ref bool fireRight, ref bool special)
	{
	}

	public virtual void GetTankMovement(ref bool left, ref bool right, ref bool fire, ref bool fireLeft, ref bool fireRight, ref bool special)
	{
		special = false; left = (right = (fire = (fireLeft = (fireRight = (special )))));
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
			fire = true;
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
	}

	protected override void TurnAround()
	{
		this.walkDirection *= -1;
		if (this.walkDirection < 0)
		{
			this.moveLeft1Frame = true;
		}
		else
		{
			this.moveRight1Frame = true;
		}
	}

	protected override bool IsPlayerThisWay(int direction)
	{
		if (this.seenPlayerNum >= 0)
		{
			return HeroController.IsPlayerThisWay(this.seenPlayerNum, base.transform.position.x, base.transform.position.y + 10f, direction);
		}
		return HeroController.IsPlayerThisWay(base.transform.position.x, base.transform.position.y + 10f, direction);
	}

	public virtual void Land()
	{
		this.CheckNearbyBlocks();
		this.thinkState = EnemyActionState.Idle;
	}

	protected override void CheckNearbyBlocks()
	{
		this.xMin = -1000f;
		this.xMax = 5000f;
		if (Physics.Raycast(new Vector3(this.tank.x, this.tank.y + 6f, 0f), Vector3.down, out this.rayCastHit, 64f, Map.groundLayer))
		{
			UnityEngine.Debug.Log("Check Nearby Blocks");
			int num = 0;
			int num2 = 0;
			this.tank.y = this.rayCastHit.point.y;
			Map.GetRowCollumn(this.tank.x, this.tank.y + 4f, ref num, ref num2);
			if (!Map.IsBlockSolid(num2 - 1, num - 1) || Map.IsBlockSolid(num2 - 1, num))
			{
				this.xMin = this.tank.x;
			}
			else if (!Map.IsBlockSolid(num2 - 2, num - 1) || Map.IsBlockSolid(num2 - 2, num))
			{
				this.xMin = this.tank.x;
			}
			else if (!Map.IsBlockSolid(num2 - 3, num - 1) || Map.IsBlockSolid(num2 - 3, num))
			{
				this.xMin = this.tank.x - 16f;
			}
			else if (!Map.IsBlockSolid(num2 - 4, num - 1) || Map.IsBlockSolid(num2 - 4, num))
			{
				this.xMin = this.tank.x - 32f;
			}
			else if (!Map.IsBlockSolid(num2 - 5, num - 1) || Map.IsBlockSolid(num2 - 5, num))
			{
				this.xMin = this.tank.x - 48f;
			}
			else
			{
				this.xMin = this.tank.x - 64f;
			}
			if (!Map.IsBlockSolid(num2 + 1, num - 1) || Map.IsBlockSolid(num2 + 1, num))
			{
				this.xMax = this.tank.x;
			}
			else if (!Map.IsBlockSolid(num2 + 2, num - 1) || Map.IsBlockSolid(num2 + 2, num))
			{
				this.xMax = this.tank.x;
			}
			else if (!Map.IsBlockSolid(num2 + 3, num - 1) || Map.IsBlockSolid(num2 + 3, num))
			{
				this.xMax = this.tank.x + 16f;
			}
			else if (!Map.IsBlockSolid(num2 + 4, num - 1) || Map.IsBlockSolid(num2 + 4, num))
			{
				this.xMax = this.tank.x + 32f;
			}
			else if (!Map.IsBlockSolid(num2 + 5, num - 1) || Map.IsBlockSolid(num2 + 5, num))
			{
				this.xMax = this.tank.x + 48f;
			}
			else
			{
				this.xMax = this.tank.x + 64f;
			}
		}
		else
		{
			base.enabled = false;
			UnityEngine.Debug.LogError("fail setting move extents ");
			this.xMin = this.tank.x - 32f;
			this.xMax = this.tank.x + 32f;
		}
	}

	public virtual void HitGround()
	{
	}

	protected Tank tank;

	protected int notFoundCount;

	public float tankSightXRange = 200f;

	public float tankSightYRange = 90f;

	public float turnThinkDelay = 1.5f;
}
