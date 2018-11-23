// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MammothTankPolyAI : TankPolyAI
{
	public override void HearSound(float alertX, float alertY)
	{
	}

	public override void Attract(float xTarget, float yTarget)
	{
	}

	public override void Blind()
	{
	}

	public override bool Panic(int direction, bool forgetPlayer)
	{
		return false;
	}

	protected override void DoIdleThink()
	{
		base.AddAction(EnemyActionType.Wait, 0.15f);
		base.AddAction(EnemyActionType.LookForPlayer);
		base.AddAction(EnemyActionType.Wait, 0.15f);
	}

	protected override void DoAlertedThink()
	{
		if (HeroController.IsPlayerNearby(this.unit.x, this.unit.y + 20f, 90f, 120f, ref this.targetX, ref this.targetY, ref this.seenPlayerNum))
		{
			base.AddAction(EnemyActionType.UseSpecial);
			base.AddAction(EnemyActionType.Wait, 2.5f);
		}
		else if (UnityEngine.Random.value < 0.8f)
		{
			Networking.RPC<int, Vector3>(PID.TargetAll, new RpcSignature<int, Vector3>(this.tank.SetTargetPlayerNum), this.seenPlayerNum, HeroController.GetPlayerPosition(this.seenPlayerNum), false);
			base.AddAction(EnemyActionType.FacePoint, Map.GetGridPoint(HeroController.GetPlayerPosition(this.seenPlayerNum)));
			base.AddAction(EnemyActionType.Fire, 3f);
			base.AddAction(EnemyActionType.Wait, 1.5f);
		}
		else
		{
			base.AddAction(EnemyActionType.Move, new GridPoint(this.unit.collumn - base.FacingDirection * 2, this.unit.row));
		}
	}

	protected override void LookForPlayer()
	{
		if (SetResolutionCamera.IsItSortOfVisible(base.transform.position) && HeroController.CanSeePlayer(this.unit.x, this.unit.y + 40f, this.walkDirection, (float)(this.sightRangeX + 100), (float)this.sightRangeY, ref this.seenPlayerNum))
		{
			this.mentalState = MentalState.Alerted;
			if (this.firstSighting)
			{
				this.firstSighting = false;
				base.AddAction(EnemyActionType.Wait, 6.7f);
			}
			else
			{
				base.AddAction(EnemyActionType.Wait, 1.5f);
			}
		}
		else if (SetResolutionCamera.IsItSortOfVisible(base.transform.position) && HeroController.CanSeePlayer(this.unit.x, this.unit.y + 40f, -this.walkDirection, (float)(this.sightRangeX + 100), (float)this.sightRangeY, ref this.seenPlayerNum))
		{
			this.mentalState = MentalState.Alerted;
			if (this.firstSighting)
			{
				this.firstSighting = false;
				base.AddAction(EnemyActionType.Wait, 6.7f);
			}
			else
			{
				base.AddAction(EnemyActionType.Wait, 2f);
			}
		}
	}

	public override void GetInput(ref bool left, ref bool right, ref bool up, ref bool down, ref bool jump, ref bool fire, ref bool special1, ref bool special2, ref bool special3, ref bool special4)
	{
		base.GetInput(ref left, ref right, ref up, ref down, ref jump, ref fire, ref special1, ref special2, ref special3, ref special4);
		if (base.CurrentAction != null && base.CurrentAction.type == EnemyActionType.Move)
		{
			special4 = true;
		}
	}

	private float targetX;

	private float targetY;

	private bool firstSighting = true;
}
