// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ScoutAI : PolymorphicAI
{
	protected override void DoIdleThink()
	{
		for (int i = 0; i < UnityEngine.Random.Range(3, 6); i++)
		{
			ActionObject actionObject = new ActionObject(EnemyActionType.Wait, null, UnityEngine.Random.Range(0.3f, 0.6f));
			if (UnityEngine.Random.value > 0.5f)
			{
				actionObject.type = EnemyActionType.Move;
				actionObject.gridPoint = base.GetNewGridPoint();
			}
			base.AddAction(actionObject, global::QueueMode.Last);
			if (SortOfFollow.IsItSortOfVisible(this.unit.x, this.unit.y, -8f, -8f))
			{
				ActionObject action = new ActionObject(EnemyActionType.LookForPlayer, null, 0f);
				base.AddAction(action, global::QueueMode.Last);
			}
		}
	}

	public override void ForgetPlayer()
	{
		base.ForgetPlayer();
		this.startPoint = null;
		this.targetDoor = null;
		this.runDirection = DirectionEnum.None;
		base.Reassess();
	}

	public override void HearSound(float alertX, float alertY)
	{
		if (this.mentalState != MentalState.Alerted)
		{
			base.HearSound(alertX, alertY);
		}
	}

	public override void FullyAlert(float x, float y, int playerNum)
	{
		if (this.mentalState != MentalState.Panicking && this.mentalState != MentalState.Attracted && this.mentalState != MentalState.Alerted)
		{
			base.FullyAlert(x, y, playerNum);
			if (this.seenPlayerNum < 0 || this.seenPlayerNum > 4)
			{
				this.startPoint = new GridPoint(this.mook.collumn, this.mook.row);
			}
			else
			{
				this.startPoint = Map.GetGridPoint(HeroController.players[this.seenPlayerNum].GetCharacterPosition());
			}
			this.mook.speed = this.mook.GetComponent<ScoutMook>().runSpeed;
			GridPoint gridPoint = null;
			if (this.targetDoor == null)
			{
				this.targetDoor = Map.GetNearestMookDoor(this.mook.collumn, this.mook.row);
				if (this.targetDoor != null)
				{
					gridPoint = new GridPoint(this.targetDoor.collumn, this.targetDoor.row);
				}
				else
				{
					gridPoint = Map.GetGridPoint(base.transform.position);
					if (this.runDirection == DirectionEnum.None)
					{
						Vector2 vector = new Vector2(this.mook.x - x, this.mook.y - y);
						if (vector.x < 0f)
						{
							this.runDirection = DirectionEnum.Left;
							gridPoint.collumn = Mathf.Clamp(gridPoint.collumn - 32, 1, gridPoint.collumn);
						}
						else
						{
							this.runDirection = DirectionEnum.Right;
							gridPoint.collumn = Mathf.Clamp(gridPoint.collumn + 32, gridPoint.collumn, Map.Width);
						}
					}
				}
			}
			this.actionQueue.Clear();
			base.AddAction(EnemyActionType.GreetPlayer);
			base.AddAction(EnemyActionType.Wait, this.sightDelay);
			base.AddAction(EnemyActionType.FollowPath, new GridPoint(gridPoint.collumn, gridPoint.row));
			base.AddAction(EnemyActionType.UseSpecial2);
			base.AddAction(EnemyActionType.Move, new GridPoint(gridPoint.collumn - 1, gridPoint.row));
			base.AddAction(EnemyActionType.Move, new GridPoint(gridPoint.collumn + 1, gridPoint.row));
			base.AddAction(EnemyActionType.Move, new GridPoint(gridPoint.collumn - 1, gridPoint.row));
			base.AddAction(EnemyActionType.Move, new GridPoint(gridPoint.collumn + 1, gridPoint.row));
			base.AddAction(EnemyActionType.Move, new GridPoint(gridPoint.collumn - 1, gridPoint.row));
			base.AddAction(EnemyActionType.Move, new GridPoint(gridPoint.collumn + 1, gridPoint.row));
			base.AddAction(EnemyActionType.Move, new GridPoint(gridPoint.collumn - 1, gridPoint.row));
			base.AddAction(EnemyActionType.Move, new GridPoint(gridPoint.collumn + 1, gridPoint.row));
			base.AddAction(EnemyActionType.Move, new GridPoint(gridPoint.collumn - 1, gridPoint.row));
			base.AddAction(EnemyActionType.Move, new GridPoint(gridPoint.collumn + 1, gridPoint.row));
			base.AddAction(EnemyActionType.Move, new GridPoint(gridPoint.collumn - 1, gridPoint.row));
			base.AddAction(EnemyActionType.Move, new GridPoint(gridPoint.collumn + 1, gridPoint.row));
			base.AddAction(EnemyActionType.BecomeIdle);
		}
	}

	public override void GetInput(ref bool left, ref bool right, ref bool up, ref bool down, ref bool jump, ref bool fire, ref bool special1, ref bool special2, ref bool special3, ref bool special4)
	{
		base.GetInput(ref left, ref right, ref up, ref down, ref jump, ref fire, ref special1, ref special2, ref special3, ref special4);
		if ((base.CurrentAction != null && base.CurrentAction.type == EnemyActionType.UseSpecial) || (this.mentalState == MentalState.Alerted && (this.alertDelay -= this.t) < 0f))
		{
			this.alertDelay = UnityEngine.Random.Range(0.7f, 1.5f);
			if (base.CurrentAction != null && base.CurrentAction.type == EnemyActionType.FollowPath)
			{
				Map.AlertNearbyMooks(base.transform.position.x, base.transform.position.y, 64f, 40f, this.seenPlayerNum);
			}
			this.mook.PlaySpecialSound(0.9f, 0.9f + UnityEngine.Random.value * 0.4f);
			fire = true;
			this.ramboBubble.RestartBubble(this.alertDelay / 2f);
		}
		if (base.CurrentAction != null && base.CurrentAction.type == EnemyActionType.UseSpecial2 && this.targetDoor != null && Vector3.Distance(base.transform.position, this.targetDoor.transform.position) < 32f)
		{
			this.targetDoor.AlarmMooks(this.startPoint);
		}
	}

	protected DirectionEnum runDirection;

	protected GridPoint startPoint;

	protected MookDoor targetDoor;

	public ReactionBubble ramboBubble;

	private float alertDelay;
}
