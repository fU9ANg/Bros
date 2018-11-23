// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TankPolyAI : PolymorphicAI
{
	private void Start()
	{
		this.tank = base.GetComponent<Tank>();
	}

	protected override void InitialSetup()
	{
		base.InitialSetup();
		base.AddAction(EnemyActionType.FacePoint, new GridPoint(0, 0));
	}

	public override void Attract(float xTarget, float yTarget)
	{
	}

	public override void HearSound(float alertX, float alertY)
	{
	}

	public void HitGround()
	{
	}

	public override void Land()
	{
		this.SetupAvailableGridPoints();
	}

	public override void FullyAlert(float x, float y, int playerNum)
	{
		base.FullyAlert(x, y, playerNum);
		Networking.RPC<int, Vector3>(PID.TargetAll, new RpcSignature<int, Vector3>(this.tank.SetTargetPlayerNum), this.seenPlayerNum, HeroController.GetPlayerPosition(playerNum), false);
	}

	protected override void GreetPlayer()
	{
		this.RestartExclamationBubble_Networked();
	}

	protected override void DoIdleThink()
	{
		for (int i = 0; i < UnityEngine.Random.Range(3, 6); i++)
		{
			ActionObject actionObject = new ActionObject(EnemyActionType.Wait, null, UnityEngine.Random.Range(1f, 2f));
			base.AddAction(actionObject, global::QueueMode.Last);
			base.AddAction(EnemyActionType.LookForPlayer);
			if (UnityEngine.Random.value > 0.4f)
			{
				actionObject = new ActionObject(EnemyActionType.Move, base.GetNewGridPoint());
				if (Mathf.Abs(this.unit.collumn - actionObject.gridPoint.collumn) >= 2)
				{
					base.AddAction(actionObject, global::QueueMode.Last);
				}
			}
			ActionObject action = new ActionObject(EnemyActionType.LookForPlayer, null, 0f);
			base.AddAction(action, global::QueueMode.Last);
		}
	}

	protected override void DoAlertedThink()
	{
		base.AddAction(EnemyActionType.Wait, this.sightDelay / 2f);
		base.AddAction(EnemyActionType.Fire, this.attackTime);
		if (!HeroController.CanSeePlayer(this.unit.x, this.unit.y, this.walkDirection, (float)this.sightRangeX * 1.2f, (float)this.sightRangeY * 1.2f, ref this.seenPlayerNum))
		{
			if (this.thoughtsWithoutSeeingPlayer++ > 4)
			{
				this.ForgetPlayer();
			}
		}
		else if (!this.IsPlayerThisWay(base.FacingDirection))
		{
			base.AddAction(EnemyActionType.FacePoint, new GridPoint(this.unit.collumn - base.FacingDirection * 2, this.unit.row));
		}
		else
		{
			this.thoughtsWithoutSeeingPlayer = 0;
		}
		this.salvosFired++;
		if (this.salvosFired <= 3)
		{
			base.AddAction(new ActionObject(EnemyActionType.Wait, UnityEngine.Random.Range(2.5f, 3.5f)));
		}
		else
		{
			this.salvosFired = 0;
			base.AddAction(new ActionObject(EnemyActionType.Wait, UnityEngine.Random.Range(4.5f, 5.5f)));
		}
	}

	protected bool IsPlayerThisWay(int direction)
	{
		if (this.seenPlayerNum >= 0)
		{
			return HeroController.IsPlayerThisWay(this.seenPlayerNum, base.transform.position.x, base.transform.position.y + 10f, direction);
		}
		return HeroController.IsPlayerThisWay(base.transform.position.x, base.transform.position.y + 10f, direction);
	}

	protected Tank tank;

	private int thoughtsWithoutSeeingPlayer;

	private int salvosFired;
}
