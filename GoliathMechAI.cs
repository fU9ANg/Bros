// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class GoliathMechAI : PolymorphicAI
{
	protected override void Awake()
	{
		base.Awake();
		this.mech = base.GetComponent<Mech>();
	}

	protected override void DoIdleThink()
	{
		this.suspciousThinkCount++;
		if (this.suspciousThinkCount % 3 == 0)
		{
			base.AddAction(EnemyActionType.Wait, 0.4f + UnityEngine.Random.value * 0.7f);
			base.AddAction(EnemyActionType.LookForPlayer, 0.1f);
		}
		else if (this.suspciousThinkCount % 3 == 1)
		{
			base.AddAction(EnemyActionType.Wait, 0.4f + UnityEngine.Random.value * 0.7f);
			base.AddAction(EnemyActionType.LookForPlayer, 0.1f);
		}
		else
		{
			this.walkDirection *= -1;
			base.AddAction(EnemyActionType.FacePoint, new GridPoint(this.unit.collumn + 12 * this.walkDirection, this.unit.row), 0.04f);
			base.AddAction(EnemyActionType.Wait, 0.5f);
			base.AddAction(EnemyActionType.LookForPlayer, 0.1f);
		}
	}

	protected override void LookForPlayer()
	{
		if (HeroController.CanSeePlayer(this.unit.x, this.unit.y, base.FacingDirection, (float)this.sightRangeX, (float)this.sightRangeY, ref this.seenPlayerNum))
		{
			this.FullyAlert(HeroController.GetPlayerPosition(this.seenPlayerNum).x, HeroController.GetPlayerPosition(this.seenPlayerNum).y, this.seenPlayerNum);
		}
		if (this.mentalState != MentalState.Alerted && SortOfFollow.IsItSortOfVisible(this.unit.x, this.unit.y, -16f, -16f))
		{
			this.seenPlayerNum = HeroController.GetNearestPlayer(this.unit.x, this.unit.y, 10000f, 10000f);
			Networking.RPC<int, Vector3>(PID.TargetAll, new RpcSignature<int, Vector3>(this.unit.SetTargetPlayerNum), this.seenPlayerNum, HeroController.GetPlayerPosition(this.seenPlayerNum), false);
			this.mentalState = MentalState.Alerted;
		}
	}

	protected override void DoSuspiciousThink()
	{
		this.DoIdleThink();
	}

	protected override void DoAlertedThink()
	{
		UnityEngine.Debug.Log("Do Alerted think " + this.patternType);
		if (HeroController.GetPlayerAliveNum() <= 0)
		{
			this.seenPlayerNum = -1;
			this.DoIdleThink();
			return;
		}
		if (this.seenPlayerNum < 0)
		{
			this.seenPlayerNum = HeroController.GetNearestPlayer(this.unit.x, this.unit.y, 10000f, 10000f);
			Networking.RPC<int, Vector3>(PID.TargetAll, new RpcSignature<int, Vector3>(this.unit.SetTargetPlayerNum), this.seenPlayerNum, HeroController.GetPlayerPosition(this.seenPlayerNum), false);
		}
		if (this.seenPlayerNum < 0)
		{
			this.DoIdleThink();
			return;
		}
		switch (this.patternType)
		{
		case MechAIPattern.Stomping:
			if (this.IsPlayerOnTopOfMe())
			{
				base.AddAction(EnemyActionType.UseSpecial2, 0.4f);
				base.AddAction(EnemyActionType.Wait, 2f);
			}
			else
			{
				int num = this.patternIndex;
				GridPoint gridPoint = Map.GetGridPoint(HeroController.GetPlayerPosition(this.seenPlayerNum));
				this.walkDirection = (int)Mathf.Sign((float)(gridPoint.collumn - this.unit.collumn));
				if (!this.UnitIsAboveHalfHealth())
				{
					base.AddAction(EnemyActionType.FacePoint, gridPoint, 0.3f);
				}
				if (!this.UnitIsAboveHalfHealth() && this.IsPlayerOnTopOfMe())
				{
					UnityEngine.Debug.Log(" Health critical acction!!  GERNADES! ");
					base.AddAction(EnemyActionType.UseSpecial2, 0.4f);
					base.AddAction(EnemyActionType.Wait, 0.4f);
				}
				if (!this.UnitIsAboveHalfHealth())
				{
					UnityEngine.Debug.Log(" Health critical acction!!  FIRE ");
					base.AddAction(EnemyActionType.Fire, 0.4f);
					base.AddAction(EnemyActionType.Wait, 1f);
				}
				base.AddAction(EnemyActionType.Hover, new GridPoint(this.unit.collumn, Map.GetRow(this.originalGroundHeight + 1f) + 5));
				if (!this.UnitIsAboveHalfHealth())
				{
					base.AddAction(EnemyActionType.Fire, 0.4f);
				}
				GridPoint gridPoint2 = new GridPoint(Mathf.Clamp(gridPoint.collumn - this.unit.collumn, (!this.UnitIsAboveHalfHealth()) ? -16 : -8, (!this.UnitIsAboveHalfHealth()) ? 16 : 8), this.unit.row);
				base.AddAction(EnemyActionType.Hover, new GridPoint(this.unit.collumn + gridPoint2.collumn, this.unit.row + 6));
				base.AddAction(EnemyActionType.Stomp, 2f);
				base.AddAction(EnemyActionType.Wait, 0.2f);
				if (this.UnitIsAboveHalfHealth())
				{
					if (this.mech.CanFrontAssault())
					{
						this.patternType = MechAIPattern.FrontAssault;
					}
					else
					{
						this.patternType = MechAIPattern.Bombardment;
					}
				}
				else
				{
					this.patternType = MechAIPattern.Stomping;
				}
				this.patternIndex++;
			}
			break;
		case MechAIPattern.Bombardment:
			if (this.IsPlayerOnTopOfMe())
			{
				base.AddAction(EnemyActionType.UseSpecial2, 0.4f);
				base.AddAction(EnemyActionType.Wait, 1.3f);
			}
			else
			{
				bool flag = this.CheckThinkTurnaround();
				if (flag)
				{
					base.AddAction(EnemyActionType.Wait, 1.3f);
				}
				else
				{
					base.AddAction(EnemyActionType.Wait, 0.5f);
				}
				base.AddAction(EnemyActionType.Fire, this.attackTime);
				base.AddAction(EnemyActionType.Wait, 1.5f);
				this.patternType = MechAIPattern.Stomping;
				this.patternIndex = 0;
			}
			break;
		case MechAIPattern.FrontAssault:
			if (this.IsPlayerOnTopOfMe())
			{
				base.AddAction(EnemyActionType.UseSpecial2, 0.4f);
				base.AddAction(EnemyActionType.Wait, 0.5f);
				this.lastGrenadeDropTime = Time.time;
			}
			else
			{
				this.CheckThinkTurnaround();
				base.AddAction(EnemyActionType.UseSpecial3, this.specialUseTime * 0.2f);
				base.AddAction(EnemyActionType.Wait, this.specialUseTime * 0.8f);
			}
			if (this.UnitIsAboveHalfHealth())
			{
				this.patternType = MechAIPattern.Bombardment;
			}
			else
			{
				this.patternType = MechAIPattern.Stomping;
			}
			this.patternIndex = 0;
			break;
		default:
			this.patternIndex = 0;
			this.patternType = MechAIPattern.Bombardment;
			base.AddAction(EnemyActionType.Wait, 0.25f);
			break;
		}
	}

	public bool UnitIsAboveHalfHealth()
	{
		return this.unit.health > this.unit.maxHealth / 2;
	}

	public bool IsPlayerOnTopOfMe()
	{
		int nearestPlayer = HeroController.GetNearestPlayer(this.unit.x, this.unit.y, 10000f, 10000f);
		if (nearestPlayer >= 0)
		{
			Vector3 playerPos = HeroController.GetPlayerPos(nearestPlayer);
			if (Mathf.Abs(playerPos.y - (this.unit.y + 48f)) < 48f && Mathf.Abs(playerPos.x - this.unit.x) < 48f)
			{
				return true;
			}
		}
		return false;
	}

	public override void Land()
	{
		if (!this.landed)
		{
			this.landed = true;
			base.Reassess();
		}
		if (this.originalGroundHeight <= 0f)
		{
			this.originalGroundHeight = this.unit.y;
		}
		UnityEngine.Debug.Log("Land ! " + this.actionQueue.Count);
		this.StopStomping();
	}

	public override void StopStomping()
	{
		base.StopStomping();
		if (this.actionQueue.Count > 0 && this.actionQueue[0].type == EnemyActionType.Stomp)
		{
			this.actionQueue.RemoveAt(0);
		}
		else if (this.actionQueue.Count > 0 && this.actionQueue[0].type == EnemyActionType.Hover)
		{
			this.actionQueue.RemoveAt(0);
		}
		else
		{
			base.Land();
		}
	}

	protected override void RunQueue()
	{
		base.RunQueue();
	}

	public override void GetInput(ref bool left, ref bool right, ref bool up, ref bool down, ref bool jump, ref bool fire, ref bool special1, ref bool special2, ref bool special3, ref bool special4)
	{
		special4 = false; left = (right = (up = (down = (jump = (fire = (special1 = (special2 = (special3 = (special4 )))))))));
		ActionObject actionObject = (this.actionQueue.Count <= 0) ? null : this.actionQueue[0];
		if (actionObject != null)
		{
			EnemyActionType type = actionObject.type;
			if (type != EnemyActionType.Hover)
			{
				if (type != EnemyActionType.Stomp)
				{
					base.GetInput(ref left, ref right, ref up, ref down, ref jump, ref fire, ref special1, ref special2, ref special3, ref special4);
				}
				else
				{
					special4 = true;
					down = true;
				}
			}
			else
			{
				special4 = true;
				if (this.unit.row < actionObject.gridPoint.row)
				{
					up = true;
				}
				else if (this.unit.collumn > actionObject.gridPoint.collumn)
				{
					left = true;
				}
				else if (this.unit.collumn < actionObject.gridPoint.collumn)
				{
					right = true;
				}
				else if (this.unit.row > actionObject.gridPoint.row)
				{
					down = true;
				}
			}
		}
	}

	protected bool CheckThinkTurnaround()
	{
		if (this.seenPlayerNum >= 0)
		{
			this.walkDirection = (int)Mathf.Sign(HeroController.GetPlayerPosition(this.seenPlayerNum).x - base.transform.position.x);
			if (this.walkDirection != (int)base.transform.localScale.x)
			{
				UnityEngine.Debug.Log("Turn Around! " + this.walkDirection);
				base.AddAction(EnemyActionType.FacePoint, Map.GetGridPoint(HeroController.GetPlayerPosition(this.seenPlayerNum)), 0.2f);
				return true;
			}
		}
		else
		{
			this.walkDirection = (int)Mathf.Sign(SortOfFollow.GetInstance().transform.position.x - base.transform.position.x);
			if (this.walkDirection != (int)base.transform.localScale.x)
			{
				UnityEngine.Debug.Log("Turn Around! " + this.walkDirection);
				base.AddAction(EnemyActionType.FacePoint, Map.GetGridPoint(HeroController.GetPlayerPosition(this.seenPlayerNum)), 0.2f);
				return true;
			}
		}
		return false;
	}

	public override void ForgetPlayer()
	{
		if (this.mentalState == MentalState.Alerted && HeroController.GetPlayersAliveCount() > 0)
		{
			if (!HeroController.GetNearestPlayer(this.unit.x, this.unit.y, 200f, 200f, ref this.seenPlayerNum))
			{
				base.ForgetPlayer();
			}
		}
		else
		{
			base.ForgetPlayer();
		}
	}

	protected MechAIPattern patternType;

	protected int patternIndex;

	public float specialUseTime = 2f;

	protected bool landed;

	protected Mech mech;

	protected float originalGroundHeight = -200f;

	protected float lastGrenadeDropTime;

	protected float bombardTime;
}
