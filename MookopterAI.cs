// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookopterAI : TankAI
{
	protected override void Start()
	{
		base.Start();
		this.kopterPattern = KopterPattern.MovingRight;
		if (!Map.isEditing)
		{
			this.CheckKopterExtents();
			this.tank.y = this.xMaxMaxHeight;
			this.tank.x = this.xMax;
		}
	}

	protected override void Update()
	{
		this.CheckAndMoveToExtents();
		UnityEngine.Debug.DrawRay(new Vector3(this.xMin, this.xMinMinHeight, 0f), Vector3.up, Color.yellow, this.xMinMaxHeight - this.xMinMinHeight, false);
		UnityEngine.Debug.DrawRay(new Vector3(this.xMax, this.xMaxMinHeight, 0f), Vector3.up, Color.yellow, this.xMaxMaxHeight - this.xMaxMinHeight, false);
		base.Update();
	}

	protected virtual void CheckAndMoveToExtents()
	{
		if (this.checkBlocksAfterEdit && !Map.isEditing)
		{
			this.CheckKopterExtents();
			this.tank.y = this.xMaxMaxHeight;
			this.tank.x = this.xMax;
		}
	}

	public override void GetKopterMovement(ref bool up, ref bool down, ref bool left, ref bool right, ref bool fire, ref bool fireLeft, ref bool fireRight, ref bool special)
	{
		special = false; up = (down = (left = (right = (fire = (fireLeft = (fireRight = (special )))))));
		switch (this.kopterPattern)
		{
		case KopterPattern.Hovering:
			if (this.thinkState == EnemyActionState.Shooting)
			{
				fire = true;
			}
			break;
		case KopterPattern.MovingLeft:
			if (this.tank.x > this.xMin)
			{
				left = true;
				if (this.thinkState == EnemyActionState.UsingSpecial)
				{
					special = true;
				}
			}
			else if (this.thinkCounter > 0.1f)
			{
				this.thinkCounter = 0.1f;
			}
			break;
		case KopterPattern.DescendingLeft:
			if (this.tank.y > this.descendHeight)
			{
				down = true;
			}
			else if (this.thinkCounter > 0.1f)
			{
				this.thinkCounter = 0.1f;
			}
			break;
		case KopterPattern.AscendingLeft:
			if (this.tank.y < this.xMinMaxHeight)
			{
				up = true;
			}
			else if (this.thinkCounter > 0.1f)
			{
				this.thinkCounter = 0.1f;
			}
			break;
		case KopterPattern.MovingRight:
			if (this.tank.x < this.xMax)
			{
				right = true;
				if (this.thinkState == EnemyActionState.UsingSpecial)
				{
					special = true;
				}
			}
			else if (this.thinkCounter > 0.1f)
			{
				this.thinkCounter = 0.1f;
			}
			break;
		case KopterPattern.DescendingRight:
			if (this.tank.y > this.descendHeight)
			{
				down = true;
			}
			else if (this.thinkCounter > 0.1f)
			{
				this.thinkCounter = 0.1f;
			}
			break;
		case KopterPattern.AscendingRight:
			if (this.tank.y < this.xMaxMaxHeight)
			{
				up = true;
			}
			else if (this.thinkCounter > 0.1f)
			{
				this.thinkCounter = 0.1f;
			}
			break;
		}
		if (this.shoot1Frame && this.tank.CanFire())
		{
			fire = true;
			this.shoot1Frame = false;
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
		if (this.fireNextThink)
		{
			this.fireNextThink = false;
			this.thinkState = EnemyActionState.Shooting;
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

	protected override void DoAwareThink()
	{
		this.PatrolThink();
		if (this.seenPlayerNum >= 0 && !HeroController.PlayerIsAlive(this.seenPlayerNum))
		{
			this.ForgetPlayer();
		}
	}

	protected override void DoUnawareThink()
	{
		this.IdlePatrolThink();
	}

	protected override void ThinkAboutShooting()
	{
		if (this.IsPlayerThisWay(this.walkDirection))
		{
			if (SortOfFollow.IsItSortOfVisible(base.transform.position, 50f, -20f))
			{
				if (HeroController.CanSeePlayer(this.tank.x, this.tank.weapon.y, this.tank.facingDirection, this.tankSightXRange, 48f, ref this.seenPlayerNum))
				{
					this.thinkState = EnemyActionState.Shooting;
					this.thinkCounter = this.attackTime;
				}
				else if (this.lastKopterPattern == KopterPattern.DescendingLeft || this.lastKopterPattern == KopterPattern.DescendingRight)
				{
					this.thinkState = EnemyActionState.Shooting;
					this.thinkCounter = this.attackTime;
				}
			}
		}
		else if (this.IsPlayerThisWay(-this.walkDirection))
		{
			if (SortOfFollow.IsItSortOfVisible(base.transform.position, 50f, -20f))
			{
				if (HeroController.CanSeePlayer(this.tank.x, this.tank.weapon.y, -this.tank.facingDirection, this.tankSightXRange, 48f, ref this.seenPlayerNum))
				{
					this.fireNextThink = true;
				}
				else if (this.lastKopterPattern == KopterPattern.DescendingLeft || this.lastKopterPattern == KopterPattern.DescendingRight)
				{
					this.fireNextThink = true;
				}
			}
			if (this.fireNextThink || this.kopterPattern == KopterPattern.DescendingRight || this.kopterPattern == KopterPattern.DescendingLeft)
			{
				this.TurnAround();
				this.thinkCounter = 0.5f;
			}
		}
	}

	protected void IdlePatrolThink()
	{
		this.thinkState = EnemyActionState.Idle;
		if (this.kopterPattern != KopterPattern.Hovering)
		{
			this.lastKopterPattern = this.kopterPattern;
			this.kopterPattern = KopterPattern.Hovering;
			this.LookForPlayer(this.tank.facingDirection);
			if (this.isAwareOfPlayer)
			{
				this.RestartExclamationBubble_Networked();
			}
			this.thinkCounter = UnityEngine.Random.value * 0.2f + 0.2f;
			if (this.lastKopterPattern == KopterPattern.DescendingLeft || this.lastKopterPattern == KopterPattern.DescendingRight || UnityEngine.Random.value > 0.7f)
			{
				this.TurnAround();
				this.thinkCounter += 0.5f;
				this.justTurnedAround = true;
			}
		}
		else
		{
			if (this.justTurnedAround)
			{
				this.justTurnedAround = false;
				this.LookForPlayer(this.tank.facingDirection);
				if (this.isAwareOfPlayer)
				{
					this.RestartExclamationBubble_Networked();
				}
			}
			if (this.lastKopterPattern == KopterPattern.AscendingRight)
			{
				this.lastKopterPattern = KopterPattern.MovingRight;
			}
			this.kopterPattern = this.lastKopterPattern + 1;
			if (this.kopterPattern == KopterPattern.Restart)
			{
				this.kopterPattern = KopterPattern.MovingLeft;
			}
			this.thinkCounter = 80f;
			if (this.kopterPattern == KopterPattern.MovingLeft)
			{
				this.walkDirection = -1;
			}
			else if (this.kopterPattern == KopterPattern.MovingRight)
			{
				this.walkDirection = 1;
			}
			if (this.kopterPattern == KopterPattern.DescendingLeft || this.kopterPattern == KopterPattern.DescendingRight)
			{
				this.ThinkDescendHeight();
			}
		}
	}

	protected void PatrolThink()
	{
		if (this.kopterPattern != KopterPattern.Hovering)
		{
			this.lastKopterPattern = this.kopterPattern;
			this.kopterPattern = KopterPattern.Hovering;
			this.ThinkAboutShooting();
			if (!this.fireNextThink && this.thinkState != EnemyActionState.Shooting)
			{
				this.thinkCounter = 0.2f;
				this.thinkState = EnemyActionState.Idle;
			}
		}
		else
		{
			this.thinkState = EnemyActionState.Idle;
			this.kopterPattern = this.lastKopterPattern + 1;
			if (this.kopterPattern == KopterPattern.Restart)
			{
				this.kopterPattern = KopterPattern.MovingLeft;
			}
			this.thinkCounter = 80f;
			if (this.kopterPattern == KopterPattern.MovingLeft && this.seenPlayerNum >= 0)
			{
				HeroController.GetPlayerPos(this.seenPlayerNum, ref this.targetX, ref this.targetY);
				if (this.targetX < this.xMin + 16f)
				{
					this.kopterPattern = KopterPattern.DescendingRight;
				}
			}
			if (this.kopterPattern == KopterPattern.MovingRight && this.seenPlayerNum >= 0)
			{
				HeroController.GetPlayerPos(this.seenPlayerNum, ref this.targetX, ref this.targetY);
				if (this.targetX > this.xMax - 16f)
				{
					this.kopterPattern = KopterPattern.DescendingLeft;
				}
			}
			if (this.kopterPattern == KopterPattern.MovingLeft)
			{
				this.walkDirection = -1;
				this.ThinkAboutSpecial();
			}
			else if (this.kopterPattern == KopterPattern.MovingRight)
			{
				this.walkDirection = 1;
				this.ThinkAboutSpecial();
			}
			if (this.kopterPattern == KopterPattern.DescendingLeft || this.kopterPattern == KopterPattern.DescendingRight)
			{
				this.CheckNearbyBlocks();
				this.ThinkDescendHeight();
			}
		}
	}

	protected override void ThinkAboutSpecial()
	{
		if (this.seenPlayerNum >= 0)
		{
			HeroController.GetPlayerPos(this.seenPlayerNum, ref this.targetX, ref this.targetY);
			if (this.targetX > this.xMin - 16f && this.targetX < this.xMax + 16f)
			{
				this.thinkState = EnemyActionState.UsingSpecial;
			}
		}
	}

	public override void HitGround()
	{
		if (this.descendHeight < this.tank.y)
		{
			this.descendHeight = this.tank.y + 1f;
		}
	}

	protected void ThinkDescendHeight()
	{
		if (this.kopterPattern == KopterPattern.DescendingLeft)
		{
			this.descendHeight = ((!this.isAwareOfPlayer) ? ((this.xMinMinHeight + this.xMinMaxHeight) / 2f) : this.xMinMinHeight);
		}
		else if (this.kopterPattern == KopterPattern.DescendingRight)
		{
			this.descendHeight = ((!this.isAwareOfPlayer) ? ((this.xMaxMinHeight + this.xMaxMaxHeight) / 2f) : this.xMaxMinHeight);
		}
		if (HeroController.CanSeePlayer(this.tank.x, this.descendHeight + 32f, this.tank.facingDirection, this.tankSightXRange, this.tankSightYRange, ref this.seenPlayerNum) || HeroController.CanSeePlayer(this.tank.x, this.descendHeight + 32f, -this.tank.facingDirection, this.tankSightXRange, this.tankSightYRange, ref this.seenPlayerNum))
		{
			HeroController.GetPlayerPos(this.seenPlayerNum, ref this.targetX, ref this.targetY);
			this.descendHeight = Mathf.Max(this.targetY + 12f, this.descendHeight);
		}
	}

	protected virtual void CheckKopterExtents()
	{
		if (!Map.isEditing)
		{
			this.checkBlocksAfterEdit = false;
			this.startX = this.unit.x;
			this.startY = this.unit.y;
			this.xMin = this.startX - 160f;
			this.xMax = this.startX + 160f;
			this.CheckGroundHeight();
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 42f, 0f), Vector3.left, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x + 64f > this.xMin)
			{
				this.xMin = this.rayCastHit.point.x + 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 56f, 0f), Vector3.left, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x + 64f > this.xMin)
			{
				this.xMin = this.rayCastHit.point.x + 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 64f, 0f), Vector3.left, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x + 64f > this.xMin)
			{
				this.xMin = this.rayCastHit.point.x + 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 80f, 0f), Vector3.left, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x + 64f > this.xMin)
			{
				this.xMin = this.rayCastHit.point.x + 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 96f, 0f), Vector3.left, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x + 64f > this.xMin)
			{
				this.xMin = this.rayCastHit.point.x + 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 112f, 0f), Vector3.left, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x + 64f > this.xMin)
			{
				this.xMin = this.rayCastHit.point.x + 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 42f, 0f), Vector3.right, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x - 64f < this.xMax)
			{
				this.xMax = this.rayCastHit.point.x - 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 56f, 0f), Vector3.right, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x - 64f < this.xMax)
			{
				this.xMax = this.rayCastHit.point.x - 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 64f, 0f), Vector3.right, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x - 64f < this.xMax)
			{
				this.xMax = this.rayCastHit.point.x - 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 80f, 0f), Vector3.right, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x - 64f < this.xMax)
			{
				this.xMax = this.rayCastHit.point.x - 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 96f, 0f), Vector3.right, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x - 64f < this.xMax)
			{
				this.xMax = this.rayCastHit.point.x - 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 112f, 0f), Vector3.right, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x - 64f < this.xMax)
			{
				this.xMax = this.rayCastHit.point.x - 64f;
			}
			if (this.xMin > this.xMax)
			{
				this.xMin = this.unit.x; this.xMax = (this.xMin );
			}
			this.CheckHeight();
			this.descendHeight = this.xMaxMinHeight;
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"CHECK BLOCKS!   xMin ",
				this.xMin,
				" xMax ",
				this.xMax,
				" xmin MIN  y ",
				this.xMinMinHeight,
				" xmax MIN y ",
				this.xMaxMinHeight,
				" xminMax y ",
				this.xMinMaxHeight,
				" xmaxMax y ",
				this.xMaxMaxHeight
			}));
			return;
		}
		UnityEngine.Debug.Log("Mook kopter editing start " + this.unit.x);
		this.checkBlocksAfterEdit = true;
	}

	protected void CheckGroundHeight()
	{
		this.groundHeight = this.startY;
		if (Physics.Raycast(new Vector3(this.unit.x, this.unit.y + 16f, 0f), Vector3.down, out this.rayCastHit, 64f, Map.groundLayer))
		{
			this.groundHeight = this.rayCastHit.point.y;
		}
		if (Physics.Raycast(new Vector3(this.unit.x - 16f, this.unit.y + 16f, 0f), Vector3.down, out this.rayCastHit, 64f, Map.groundLayer) && this.rayCastHit.point.y > this.groundHeight)
		{
			this.groundHeight = this.rayCastHit.point.y;
		}
		if (Physics.Raycast(new Vector3(this.unit.x + 16f, this.unit.y + 16f, 0f), Vector3.down, out this.rayCastHit, 64f, Map.groundLayer) && this.rayCastHit.point.y > this.groundHeight)
		{
			this.groundHeight = this.rayCastHit.point.y;
		}
	}

	protected void CheckHeight()
	{
		this.xMaxMinHeight = this.groundHeight - 64f; this.xMinMinHeight = (this.xMaxMinHeight );
		if (Physics.Raycast(new Vector3(this.xMin, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 64f, Map.groundLayer))
		{
			this.xMinMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMin + 16f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 24f > this.xMinMinHeight)
		{
			this.xMinMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMin - 16f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMinMinHeight)
		{
			this.xMinMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMin - 24f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMinMinHeight)
		{
			this.xMinMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMin + 24f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMinMinHeight)
		{
			this.xMinMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMin + 36f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMinMinHeight)
		{
			this.xMinMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMin - 36f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMinMinHeight)
		{
			this.xMinMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMin + 48f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMinMinHeight)
		{
			this.xMinMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMin - 48f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMinMinHeight)
		{
			this.xMinMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMax, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer))
		{
			this.xMaxMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMax - 16f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMaxMinHeight)
		{
			this.xMaxMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMax + 16f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMaxMinHeight)
		{
			this.xMaxMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMax + 24f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMaxMinHeight)
		{
			this.xMaxMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMax - 24f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMaxMinHeight)
		{
			this.xMaxMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMax - 36f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMaxMinHeight)
		{
			this.xMaxMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMax + 36f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMaxMinHeight)
		{
			this.xMaxMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMax - 48f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMaxMinHeight)
		{
			this.xMaxMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMax + 48f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMaxMinHeight)
		{
			this.xMaxMinHeight = this.rayCastHit.point.y + 28f;
		}
		UnityEngine.Debug.Log("xMaxMinHeight " + this.xMaxMinHeight);
		this.xMinMaxHeight = Mathf.Max(new float[]
		{
			this.xMinMinHeight + 32f,
			this.xMaxMinHeight + 16f,
			this.groundHeight + 44f
		});
		this.xMaxMaxHeight = Mathf.Max(new float[]
		{
			this.xMaxMinHeight + 32f,
			this.xMinMinHeight + 16f,
			this.groundHeight + 44f
		});
	}

	protected override void CheckNearbyBlocks()
	{
		if (this.recheckHeight)
		{
			this.CheckHeight();
		}
	}

	public override UnityStream PackState(UnityStream stream)
	{
		stream.Serialize<float>(this.descendHeight);
		stream.Serialize<KopterPattern>(this.kopterPattern);
		stream.Serialize<bool>(this.fireNextThink);
		return base.PackState(stream);
	}

	public override UnityStream UnpackState(UnityStream stream)
	{
		this.descendHeight = (float)stream.DeserializeNext();
		this.kopterPattern = (KopterPattern)((int)stream.DeserializeNext());
		this.fireNextThink = (bool)stream.DeserializeNext();
		return base.UnpackState(stream);
	}

	protected float xMinMinHeight;

	protected float xMaxMinHeight;

	protected float xMinMaxHeight;

	protected float xMaxMaxHeight;

	protected float descendHeight;

	public KopterPattern kopterPattern;

	public KopterPattern lastKopterPattern;

	protected bool fireNextThink;

	protected int notSeenCounter = 3;

	protected bool checkBlocksAfterEdit = true;

	protected bool justTurnedAround;

	protected float startX;

	protected float startY;

	public bool recheckHeight;

	protected float groundHeight = 128f;
}
