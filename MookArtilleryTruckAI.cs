// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookArtilleryTruckAI : TankAI
{
	protected override void Start()
	{
		base.Start();
		if (!this.moveAtStart)
		{
			this.thinkState = EnemyActionState.Idle;
		}
		else
		{
			this.thinkState = EnemyActionState.Moving;
		}
		this.walkDirection = -1;
	}

	protected override void Update()
	{
		this.t = Time.deltaTime;
		this.UpdateOwnership();
		this.thinkCounter -= Time.deltaTime;
		if (this.thinkCounter < 0f && base.IsMine)
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
		this.thinkCount++;
		this.thinkCounter += 0.5f;
		if (Map.isEditing || CutsceneController.isInCutscene)
		{
			return;
		}
		switch (this.thinkState)
		{
		case EnemyActionState.Idle:
		{
			float f = 0f;
			float f2 = 0f;
			if (SortOfFollow.IsItSortOfVisible(base.transform.position + Vector3.up * 16f, 48f, 12f, ref f, ref f2))
			{
				if (this.tank.x < this.xMin + 6f)
				{
					this.StartShooting();
				}
				else
				{
					this.thinkCount = -1;
					this.thinkCounter = 0.5f;
					this.thinkState = EnemyActionState.Moving;
				}
			}
			else
			{
				this.thinkCounter = 0.05f + Mathf.Max(Mathf.Abs(f), Mathf.Abs(f2)) * 0.007f;
			}
			break;
		}
		case EnemyActionState.Moving:
			if (this.tank.x < this.xMin + 6f)
			{
				this.StartShooting();
			}
			else if (SortOfFollow.IsItSortOfVisible(this.tank.x, this.tank.y, -12f, 32f) && HeroController.IsPlayerNearby(this.tank.x, this.tank.y, this.tank.facingDirection, this.tankSightXRange, this.tankSightYRange, ref this.seenPlayerNum))
			{
				this.isAwareOfPlayer = true;
				this.StartShooting();
				this.xMin = this.tank.x;
			}
			else
			{
				this.thinkCount = -1;
				this.thinkCounter = 0.5f;
			}
			break;
		case EnemyActionState.Shooting:
			UnityEngine.Debug.Log("Think Shooting !");
			this.thinkCounter = 0.4f;
			this.thinkState = EnemyActionState.Idle;
			break;
		}
	}

	protected void StartShooting()
	{
		UnityEngine.Debug.Log(" visible ? " + SortOfFollow.IsItSortOfVisible(this.tank.x, this.tank.y, -24f, 32f));
		if ((this.hasAlreadyFired || SortOfFollow.IsItSortOfVisible(this.tank.x, this.tank.y, -24f, 32f)) && HeroController.IsPlayerThisWay(this.tank.x - (float)(this.tank.facingDirection * 16), this.tank.y, this.tank.facingDirection, ref this.seenPlayerNum))
		{
			if (this.seenPlayerNum >= 0)
			{
				Vector3 playerPos = HeroController.GetPlayerPos(this.seenPlayerNum);
				if (playerPos.x >= 0f)
				{
					this.tank.SetTargetPlayerNum(this.seenPlayerNum, playerPos);
					UnityEngine.Debug.Log("Start Shooting ! from " + this.thinkState);
					this.thinkState = EnemyActionState.Shooting;
					this.thinkCounter = 8f;
					this.fireCount = 0;
				}
				else
				{
					this.thinkState = EnemyActionState.Idle;
					this.thinkCounter = 0.5f;
				}
			}
			else
			{
				this.thinkState = EnemyActionState.Idle;
				this.thinkCounter = 0.5f;
			}
		}
		else
		{
			this.thinkState = EnemyActionState.Idle;
			this.thinkCounter = 0.5f;
		}
	}

	public override void ForgetPlayer()
	{
		this.isAwareOfPlayer = false;
		this.thinkCounter = 0.1f;
		this.thinkCount = -1;
		base.RestartQuestionBubble_Networked();
		this.CheckNearbyBlocks();
		this.seenPlayerNum = -1;
	}

	public override void GetTankMovement(ref bool left, ref bool right, ref bool fire, ref bool fireLeft, ref bool fireRight, ref bool special)
	{
		special = false; left = (right = (fireLeft = (fireRight = (fire = (special )))));
		if (Map.isEditing || CutsceneController.isInCutscene)
		{
			return;
		}
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
			this.fireCounter += this.t;
			if (this.fireCounter > 0f)
			{
				this.fireCounter -= 0.6f;
				if (this.fireCount < 3)
				{
					Vector3 playerPos = HeroController.GetPlayerPos(this.seenPlayerNum);
					if (playerPos.x >= 0f && Mathf.Sign(playerPos.x - this.tank.x) == Mathf.Sign((float)this.tank.facingDirection))
					{
						this.tank.SetTargetPlayerNum(this.seenPlayerNum, playerPos);
					}
					fire = true;
				}
				else if (this.thinkCounter > 1f)
				{
					this.thinkCounter = 1f;
				}
				if (this.tank.CanFire())
				{
					this.fireCount++;
					this.hasAlreadyFired = true;
				}
			}
			else
			{
				fire = false;
			}
			break;
		}
	}

	protected override void TurnAround()
	{
	}

	public override void Land()
	{
		this.CheckNearbyBlocks();
	}

	protected override void CheckNearbyBlocks()
	{
		this.xMin = -1000f;
		this.xMax = 5000f;
		if (Physics.Raycast(new Vector3(this.tank.x, this.tank.y + 6f, 0f), Vector3.down, out this.rayCastHit, 64f, Map.groundLayer))
		{
			int num = 0;
			int num2 = 0;
			this.tank.y = this.rayCastHit.point.y;
			Map.GetRowCollumn(this.tank.x, this.tank.y + 4f, ref num, ref num2);
			if (!Map.IsBlockSolid(num2 - 3, num - 1) || Map.IsBlockSolid(num2 - 3, num) || Map.IsBlockSolid(num2 - 4, num + 2))
			{
				this.xMin = this.tank.x - 16f;
			}
			else if (!Map.IsBlockSolid(num2 - 4, num - 1) || Map.IsBlockSolid(num2 - 4, num) || Map.IsBlockSolid(num2 - 5, num + 2))
			{
				this.xMin = this.tank.x - 32f;
			}
			else if (!Map.IsBlockSolid(num2 - 5, num - 1) || Map.IsBlockSolid(num2 - 5, num) || Map.IsBlockSolid(num2 - 6, num + 2))
			{
				this.xMin = this.tank.x - 48f;
			}
			else if (!Map.IsBlockSolid(num2 - 6, num - 1) || Map.IsBlockSolid(num2 - 6, num) || Map.IsBlockSolid(num2 - 7, num + 2))
			{
				this.xMin = this.tank.x - 64f;
			}
			else if (!Map.IsBlockSolid(num2 - 7, num - 1) || Map.IsBlockSolid(num2 - 7, num) || Map.IsBlockSolid(num2 - 8, num + 2))
			{
				this.xMin = this.tank.x - 80f;
			}
			else if (!Map.IsBlockSolid(num2 - 8, num - 1) || Map.IsBlockSolid(num2 - 8, num) || Map.IsBlockSolid(num2 - 9, num + 2))
			{
				this.xMin = this.tank.x - 96f;
			}
			else if (!Map.IsBlockSolid(num2 - 9, num - 1) || Map.IsBlockSolid(num2 - 9, num) || Map.IsBlockSolid(num2 - 10, num + 2))
			{
				this.xMin = this.tank.x - 112f;
			}
			else if (!Map.IsBlockSolid(num2 - 10, num - 1) || Map.IsBlockSolid(num2 - 10, num) || Map.IsBlockSolid(num2 - 11, num + 2))
			{
				this.xMin = this.tank.x - 128f;
			}
			else
			{
				this.xMin = this.tank.x - 144f;
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

	public bool moveAtStart;

	protected float fireCounter;

	protected int fireCount;

	protected bool hasAlreadyFired;
}
