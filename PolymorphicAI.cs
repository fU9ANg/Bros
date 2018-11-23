// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class PolymorphicAI : NetworkObject
{
	public int FacingDirection
	{
		get
		{
			if (this.unit.GetComponent<Tank>() != null)
			{
				return this.unit.GetComponent<Tank>().facingDirection;
			}
			return (int)base.transform.localScale.x;
		}
	}

	public int GetSeenPlayerNum()
	{
		return this.seenPlayerNum;
	}

	protected float GetTargetX(GridPoint target)
	{
		return (float)(target.collumn * 16) + this.blockOffset;
	}

	protected float GetTargetY(GridPoint target)
	{
		return (float)target.row * 16f + 12f;
	}

	protected ActionObject CurrentAction
	{
		get
		{
			return (this.actionQueue.Count <= 0) ? null : this.actionQueue[0];
		}
	}

	protected virtual void Think()
	{
		switch (this.mentalState)
		{
		case MentalState.Idle:
			this.DoIdleThink();
			break;
		case MentalState.Suspicious:
			this.DoSuspiciousThink();
			break;
		case MentalState.Alerted:
			this.DoAlertedThink();
			break;
		case MentalState.Panicking:
			this.DoPanicThink();
			break;
		case MentalState.Attracted:
			this.DoAttractedThink();
			break;
		}
	}

	protected virtual void DoAttractedThink()
	{
		int num = (int)Mathf.Sign((float)(this.attractPoint.collumn - this.unit.collumn));
		if (num == 0)
		{
			num = ((UnityEngine.Random.value <= 0.5f) ? -1 : 1);
		}
		if (UnityEngine.Random.value < 0.7f)
		{
			this.AddAction(EnemyActionType.QuestionMark);
		}
		this.AddAction(EnemyActionType.Wait, 0.3f);
		this.AddAction(EnemyActionType.Move, new GridPoint(this.unit.collumn + num * UnityEngine.Random.Range(2, 4), this.unit.row));
		this.AddAction(EnemyActionType.BecomeIdle);
	}

	protected virtual void DoPanicThink()
	{
		this.AddAction(EnemyActionType.Move, new GridPoint(this.unit.collumn + this.walkDirection * UnityEngine.Random.Range(2, 4), this.unit.row));
	}

	protected virtual void DoAlertedThink()
	{
		this.AddAction(EnemyActionType.Fire, this.attackTime);
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
		}
		for (int i = 0; i < 3; i++)
		{
			if (UnityEngine.Random.value > 0.7f && this.IsGridPointAvailable(this.unit.collumn + this.walkDirection, this.unit.row))
			{
				this.AddAction(EnemyActionType.Move, new GridPoint(this.unit.collumn + this.walkDirection, this.unit.row));
				this.AddAction(EnemyActionType.Wait, 0.4f);
			}
			else if (UnityEngine.Random.value <= 0.5f || this.actionQueue[this.actionQueue.Count - 1].type == EnemyActionType.Fire)
			{
				this.AddAction(EnemyActionType.Wait, 0.4f);
			}
		}
		this.AddAction(EnemyActionType.Wait, this.delayBetweenFiring);
	}

	protected virtual void DoSuspiciousThink()
	{
		this.suspciousThinkCount++;
		this.AddAction(new ActionObject(EnemyActionType.Wait, 0.3f));
		this.AddAction(new ActionObject(EnemyActionType.LookForPlayer));
		if (this.suspciousThinkCount > 10 && UnityEngine.Random.value > 0.8f)
		{
			this.suspciousThinkCount = 8;
			this.AddAction(EnemyActionType.FacePoint, new GridPoint(this.unit.collumn - this.FacingDirection * 2, this.unit.row));
		}
		this.AddAction(new ActionObject(EnemyActionType.Wait, 0.3f));
	}

	protected virtual void DoIdleThink()
	{
		for (int i = 0; i < UnityEngine.Random.Range(3, 6); i++)
		{
			ActionObject actionObject = new ActionObject(EnemyActionType.Wait, null, UnityEngine.Random.Range(0.3f, 1f));
			if (UnityEngine.Random.value > 0.5f)
			{
				actionObject.type = EnemyActionType.Move;
				actionObject.gridPoint = this.GetNewGridPoint();
			}
			this.AddAction(actionObject, global::QueueMode.Last);
			ActionObject action = new ActionObject(EnemyActionType.LookForPlayer, null, 0f);
			this.AddAction(action, global::QueueMode.Last);
		}
	}

	protected bool IsGridPointAvailable(int c, int r)
	{
		if (this.mentalState == MentalState.Panicking)
		{
			return true;
		}
		foreach (GridPoint gridPoint in this.availableGridPoints)
		{
			if (gridPoint.collumn == c && gridPoint.row == r)
			{
				return true;
			}
		}
		return false;
	}

	protected virtual bool ReadyToThink
	{
		get
		{
			return this.actionQueue.Count == 0;
		}
	}

	protected float UnitHalfWidth
	{
		get
		{
			if (this.mook != null)
			{
				return this.mook.HalfWidth;
			}
			return 4f;
		}
	}

	protected virtual void Awake()
	{
		this.unit = base.GetComponent<Unit>();
		this.mook = base.GetComponent<Mook>();
		this.pathAgent = base.GetComponent<PathAgent>();
		this.blockOffset = Map.GetUnitXOffset();
	}

	protected virtual void Update()
	{
		if (Map.isEditing || CutsceneController.isInCutscene || this.unit.health <= 0 || (this.mentalState == MentalState.Idle && !SortOfFollow.IsItSortOfVisible(base.transform.position.x, base.transform.position.y, 128f, 128f)))
		{
			return;
		}
		if (this.firstFrame)
		{
			this.firstFrame = false;
			this.InitialSetup();
		}
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.033334f);
		if (PlayerOptions.Instance.hardMode)
		{
			this.t *= 3f;
		}
		if (base.IsMine && this.ReadyToThink)
		{
			if (!this.unit.beingControlledByTriggerAction)
			{
				this.Think_Networked();
			}
			else if (this.unit.controllingTriggerAction != null && this.unit.controllingTriggerAction.GetCurrentCommand(this.unit).type == CharacterCommandType.AICommand)
			{
				this.unit.controllingTriggerAction.CompleteCurrentCommand(this.unit);
			}
		}
	}

	public virtual bool GetPlayerRange(ref float xRange, ref float yRange)
	{
		if (this.seenPlayerNum >= 0)
		{
			float num = 0f;
			float num2 = 0f;
			HeroController.GetPlayerPos(this.seenPlayerNum, ref num, ref num2);
			xRange = Mathf.Abs(this.mook.x - num);
			yRange = num2 - this.mook.y;
			return true;
		}
		return false;
	}

	protected virtual void RunQueue()
	{
		if (this.unit.health > 0 && this.actionQueue.Count > 0)
		{
			ActionObject actionObject = this.actionQueue[0];
			if (actionObject != null)
			{
				switch (actionObject.type)
				{
				case EnemyActionType.Wait:
				case EnemyActionType.Fire:
				case EnemyActionType.UseSpecial:
				case EnemyActionType.UseSpecial2:
				case EnemyActionType.Stomp:
					actionObject.duration -= this.t;
					if (actionObject.duration <= 0f)
					{
						this.actionQueue.Remove(actionObject);
					}
					return;
				case EnemyActionType.Move:
					if (Mathf.Abs(this.unit.x - this.GetTargetX(this.CurrentAction.gridPoint)) < this.UnitHalfWidth)
					{
						this.actionQueue.Remove(actionObject);
						this.timeStuckAgainstWall = 0f;
						this.timeOnSameSpot = 0f;
					}
					this.walkDirection = this.FacingDirection;
					if (this.IsBlockedForward() && (this.timeStuckAgainstWall += this.t) > 0.3f)
					{
						this.actionQueue.Remove(actionObject);
						this.timeStuckAgainstWall = 0f;
					}
					if (this.unit.row == this.lastRow && this.unit.collumn == this.lastCollumn)
					{
						if ((this.timeOnSameSpot += this.t) > 2f)
						{
							this.actionQueue.Remove(actionObject);
							this.timeOnSameSpot = 0f;
						}
					}
					else
					{
						this.lastCollumn = this.unit.collumn;
						this.lastRow = this.unit.row;
						this.timeOnSameSpot = 0f;
					}
					return;
				case EnemyActionType.LookForPlayer:
					this.LookForPlayer();
					this.actionQueue.Remove(actionObject);
					return;
				case EnemyActionType.GreetPlayer:
					this.GreetPlayer();
					this.actionQueue.Remove(actionObject);
					return;
				case EnemyActionType.QuestionMark:
					this.ShowQuestionBubble();
					this.actionQueue.Remove(actionObject);
					return;
				case EnemyActionType.FacePoint:
					if ((double)(actionObject.duration -= this.t) < -0.05)
					{
						this.actionQueue.Remove(actionObject);
					}
					return;
				case EnemyActionType.BecomeIdle:
					this.mentalState = MentalState.Idle;
					this.actionQueue.Clear();
					return;
				case EnemyActionType.PlaySpecialSound:
					this.unit.GetComponent<TestVanDammeAnim>().PlaySpecialSound(0.6f);
					this.actionQueue.Remove(actionObject);
					return;
				case EnemyActionType.FollowPath:
					if (this.CurrentAction.gridPoint == null || (this.unit.collumn == this.CurrentAction.gridPoint.collumn && this.unit.row == this.CurrentAction.gridPoint.row))
					{
						this.actionQueue.Remove(actionObject);
					}
					else if (this.pathAgent.CurrentPath == null)
					{
						this.pathAgent.GetPath(this.CurrentAction.gridPoint.collumn, this.CurrentAction.gridPoint.row, 10f);
					}
					return;
				case EnemyActionType.BecomeAlert:
					this.mentalState = MentalState.Alerted;
					this.actionQueue.Remove(actionObject);
					return;
				case EnemyActionType.BecomePanicked:
					this.unit.Panic(actionObject.duration, true);
					this.actionQueue.Remove(actionObject);
					return;
				case EnemyActionType.Hover:
					if (this.CurrentAction.gridPoint.collumn == this.unit.collumn && this.CurrentAction.gridPoint.row == this.unit.row)
					{
						this.actionQueue.Remove(actionObject);
						this.timeStuckAgainstWall = 0f;
						this.timeOnSameSpot = 0f;
					}
					this.walkDirection = this.FacingDirection;
					if (this.unit.row == this.lastRow && this.unit.collumn == this.lastCollumn)
					{
						if ((this.timeOnSameSpot += this.t) > 2f)
						{
							UnityEngine.Debug.Log("Time on same spot finished ");
							this.actionQueue.Remove(actionObject);
							this.timeOnSameSpot = 0f;
						}
					}
					else
					{
						this.lastCollumn = this.unit.collumn;
						this.lastRow = this.unit.row;
						this.timeOnSameSpot = 0f;
					}
					return;
				}
				this.CurrentAction.duration -= this.t;
				if (actionObject.duration < 0f)
				{
					this.actionQueue.Remove(actionObject);
				}
			}
		}
	}

	public virtual void GetInput(ref bool left, ref bool right, ref bool up, ref bool down, ref bool jump, ref bool fire, ref bool special1, ref bool special2, ref bool special3, ref bool special4)
	{
		special4 = false; left = (right = (up = (down = (jump = (fire = (special1 = (special2 = (special3 = (special4 )))))))));
		ActionObject actionObject = (this.actionQueue.Count <= 0) ? null : this.actionQueue[0];
		if (actionObject != null)
		{
			switch (actionObject.type)
			{
			case EnemyActionType.Fire:
				if (this.unit.IsOnGround())
				{
					fire = true;
				}
				break;
			case EnemyActionType.Move:
				if (Mathf.Abs(base.transform.position.x - this.GetTargetX(actionObject.gridPoint)) > 1f)
				{
					if (base.transform.position.x > this.GetTargetX(actionObject.gridPoint))
					{
						left = true;
					}
					else if (base.transform.position.x < this.GetTargetX(actionObject.gridPoint))
					{
						right = true;
					}
				}
				break;
			case EnemyActionType.FacePoint:
				if (this.unit.collumn > actionObject.gridPoint.collumn && this.FacingDirection > 0)
				{
					left = true;
				}
				if (this.unit.collumn < actionObject.gridPoint.collumn && this.FacingDirection < 0)
				{
					right = true;
				}
				break;
			case EnemyActionType.UseSpecial:
				special1 = true;
				break;
			case EnemyActionType.UseSpecial2:
				special2 = true;
				break;
			case EnemyActionType.UseSpecial3:
				special3 = true;
				break;
			case EnemyActionType.UseSpecial4:
				special4 = true;
				break;
			case EnemyActionType.FollowPath:
				if (this.pathAgent.CurrentPath != null)
				{
					this.pathAgent.GetMove(ref left, ref right, ref up, ref down, ref jump);
				}
				break;
			}
		}
	}

	protected virtual bool UnitCanSetup()
	{
		return this.unit.IsOnGround();
	}

	protected virtual void InitialSetup()
	{
		int num = 0;
		int num2 = 0;
		Map.GetRowCollumn(this.unit.x, this.unit.y, ref num2, ref num);
		this.SetupAvailableGridPoints();
	}

	protected GridPoint GetNewGridPoint()
	{
		if (this.availableGridPoints.Count <= 1)
		{
			return new GridPoint(this.unit.collumn, this.unit.row);
		}
		int i = 0;
		while (i < this.availableGridPoints.Count * 3)
		{
			i++;
			int index = UnityEngine.Random.Range(0, this.availableGridPoints.Count);
			if (this.availableGridPoints[index].collumn != this.unit.collumn || this.availableGridPoints[index].row != this.unit.row)
			{
				return this.availableGridPoints[index];
			}
		}
		for (int j = 0; j < this.availableGridPoints.Count; j++)
		{
			if (this.availableGridPoints[j].collumn != this.unit.collumn || this.availableGridPoints[j].row != this.unit.row)
			{
				return this.availableGridPoints[j];
			}
		}
		return new GridPoint(this.unit.collumn, this.unit.row);
	}

	protected virtual void SetupAvailableGridPoints()
	{
		this.availableGridPoints.Clear();
		int num = 0;
		int num2 = 0;
		Map.GetRowCollumn(this.unit.x, this.unit.y, ref num, ref num2);
		if (Map.IsBlockSolid(num2 - 1, num - 1) && !Map.IsBlockSolid(num2 - 1, num) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 - 1, num + 1)))
		{
			this.availableGridPoints.Add(new GridPoint(num2 - 1, num));
			if (this.patrolBlocksRadius > 1 && Map.IsBlockSolid(num2 - 2, num - 1) && !Map.IsBlockSolid(num2 - 2, num) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 - 2, num + 1)))
			{
				this.availableGridPoints.Insert(0, new GridPoint(num2 - 2, num));
				if (this.patrolBlocksRadius > 2 && Map.IsBlockSolid(num2 - 3, num - 1) && !Map.IsBlockSolid(num2 - 3, num) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 - 3, num + 1)))
				{
					this.availableGridPoints.Insert(0, new GridPoint(num2 - 3, num));
					if (this.patrolBlocksRadius > 3 && Map.IsBlockSolid(num2 - 4, num - 1) && !Map.IsBlockSolid(num2 - 4, num) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 - 4, num + 1)))
					{
						this.availableGridPoints.Insert(0, new GridPoint(num2 - 4, num));
						if (this.patrolBlocksRadius > 4 && Map.IsBlockSolid(num2 - 5, num - 1) && !Map.IsBlockSolid(num2 - 5, num) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 - 5, num + 1)))
						{
							this.availableGridPoints.Insert(0, new GridPoint(num2 - 5, num));
						}
					}
				}
			}
		}
		this.availableGridPoints.Add(new GridPoint(num2, num));
		if (Map.IsBlockSolid(num2 + 1, num - 1) && !Map.IsBlockSolid(num2 + 1, num) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 + 1, num + 1)))
		{
			this.availableGridPoints.Add(new GridPoint(num2 + 1, num));
			if (this.patrolBlocksRadius > 1 && Map.IsBlockSolid(num2 + 2, num - 1) && !Map.IsBlockSolid(num2 + 2, num) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 + 2, num + 1)))
			{
				this.availableGridPoints.Add(new GridPoint(num2 + 2, num));
				if (this.patrolBlocksRadius > 2 && Map.IsBlockSolid(num2 + 3, num - 1) && !Map.IsBlockSolid(num2 + 3, num) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 + 3, num + 1)))
				{
					this.availableGridPoints.Add(new GridPoint(num2 + 3, num));
					if (this.patrolBlocksRadius > 3 && Map.IsBlockSolid(num2 + 4, num - 1) && !Map.IsBlockSolid(num2 + 4, num) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 + 4, num + 1)))
					{
						this.availableGridPoints.Add(new GridPoint(num2 + 4, num));
						if (this.patrolBlocksRadius > 4 && Map.IsBlockSolid(num2 + 5, num - 1) && !Map.IsBlockSolid(num2 + 5, num) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 + 5, num + 1)))
						{
							this.availableGridPoints.Add(new GridPoint(num2 + 5, num));
						}
					}
				}
			}
		}
	}

	protected void Think_Networked()
	{
		if (this.unit.health > 0)
		{
			this.Think();
		}
	}

	protected override void LateUpdate()
	{
		this.RunQueue();
		if (this.hasActionQueueChanged && base.IsMine && this.unit.health > 0)
		{
			this.framesSinceSync = 0;
			Networking.RPC<ActionObject[], MentalState, float, float, int>(PID.TargetOthers, true, false, new RpcSignature<ActionObject[], MentalState, float, float, int>(this.Sync), this.actionQueue.ToArray(), this.mentalState, this.unit.x, this.unit.y, this.seenPlayerNum);
			this.hasActionQueueChanged = false;
		}
		else
		{
			this.framesSinceSync++;
		}
	}

	public void Sync(ActionObject[] queue, MentalState mentalState, float x, float y, int seenPlayer)
	{
		this.mentalState = mentalState;
		this.actionQueue.Clear();
		this.actionQueue.AddRange(queue);
		if (this.unit.health > 0)
		{
			this.unit.x = x;
			if (!this.unit.IsParachuteActive && this.unit.IsOnGround())
			{
				this.unit.y = y;
			}
		}
		this.seenPlayerNum = seenPlayer;
	}

	public virtual byte[] Serialize()
	{
		UnityStream unityStream = new UnityStream();
		return null;
	}

	public virtual void Deserialize(byte[] byteStream)
	{
		UnityStream unityStream = new UnityStream(byteStream);
	}

	protected void AddAction(ActionObject action, global::QueueMode queueMode)
	{
		this.hasActionQueueChanged = true;
		if (queueMode == global::QueueMode.Clear)
		{
			this.actionQueue.Clear();
			this.actionQueue.Add(action);
		}
		else if (queueMode == global::QueueMode.First)
		{
			this.actionQueue.Insert(0, action);
		}
		else if (queueMode == global::QueueMode.Last)
		{
			this.actionQueue.Add(action);
		}
		else if (queueMode == global::QueueMode.AfterCurrent)
		{
			if (this.actionQueue.Count > 0)
			{
				this.actionQueue.Insert(1, action);
			}
			else
			{
				this.actionQueue.Add(action);
			}
		}
	}

	protected void AddAction(ActionObject action)
	{
		this.AddAction(action, global::QueueMode.Last);
	}

	protected void AddAction(EnemyActionType type)
	{
		this.AddAction(new ActionObject(type));
	}

	protected void AddAction(EnemyActionType type, global::QueueMode Qmode)
	{
		this.AddAction(new ActionObject(type), Qmode);
	}

	public void AddAction(EnemyActionType type, float duration)
	{
		this.AddAction(new ActionObject(type, duration));
	}

	protected void AddAction(EnemyActionType type, float duration, global::QueueMode Qmode)
	{
		this.AddAction(new ActionObject(type, duration), Qmode);
	}

	protected void AddAction(EnemyActionType type, GridPoint point)
	{
		this.AddAction(new ActionObject(type, point));
	}

	protected void AddAction(EnemyActionType type, GridPoint point, global::QueueMode Qmode)
	{
		this.AddAction(new ActionObject(type, point), Qmode);
	}

	public void AddAction(EnemyActionType type, GridPoint point, float duration)
	{
		this.AddAction(new ActionObject(type, point, duration));
	}

	public virtual void Attract(float xTarget, float yTarget)
	{
		this.attractPoint = Map.GetGridPoint(new Vector3(xTarget, yTarget, 0f));
		if (this.mentalState != MentalState.Attracted)
		{
			this.ForgetPlayer();
			this.actionQueue.Clear();
			this.AddAction(EnemyActionType.Wait, UnityEngine.Random.value * 0.3f);
			this.SetMentalState(MentalState.Attracted);
		}
	}

	public virtual void HearSound(float alertX, float alertY)
	{
		this.thoughtsSincePlayerSeen = 0;
		this.suspciousThinkCount = 0;
		if (this.mentalState == MentalState.Idle)
		{
			this.SetMentalState(MentalState.Suspicious);
			this.actionQueue.Clear();
			this.AddAction(EnemyActionType.Wait, 0.1f + UnityEngine.Random.value);
			this.AddAction(EnemyActionType.QuestionMark);
			this.AddAction(EnemyActionType.FacePoint, new GridPoint(Map.GetCollumn(alertX), Map.GetRow(alertY)));
			this.AddAction(EnemyActionType.Wait, 0.4f);
		}
		else if ((this.mentalState == MentalState.Suspicious || this.mentalState == MentalState.Alerted) && Mathf.Abs(alertX - this.unit.x) > 12f && ((this.FacingDirection < 0 && alertX > this.unit.x) || (this.FacingDirection > 0 && this.unit.x > alertX)))
		{
			if (!this.actionQueue.Exists((ActionObject ao) => ao.type == EnemyActionType.FacePoint))
			{
				if (this.mentalState == MentalState.Alerted)
				{
					this.actionQueue.Clear();
					this.AddAction(EnemyActionType.Wait, this.sightDelay, global::QueueMode.AfterCurrent);
				}
				this.AddAction(EnemyActionType.FacePoint, new GridPoint(Map.GetCollumn(alertX), Map.GetRow(alertY)), global::QueueMode.First);
			}
		}
	}

	public virtual void Blind()
	{
		this.actionQueue.Clear();
		this.seenPlayerNum = -1;
		this.AddAction(EnemyActionType.Wait, UnityEngine.Random.Range(1f, 1.85f));
		this.SetMentalState(MentalState.Alerted);
	}

	public virtual void Blind(float time)
	{
		this.actionQueue.Clear();
		this.seenPlayerNum = -1;
		this.AddAction(EnemyActionType.Wait, time);
		this.SetMentalState(MentalState.Alerted);
	}

	public virtual void ForgetPlayer()
	{
		this.seenPlayerNum = -1;
		if (this.mentalState == MentalState.Alerted)
		{
			this.AddAction(EnemyActionType.BecomeIdle);
		}
	}

	protected void DebugShowActionQueue()
	{
		if (this.mook != null)
		{
			string text = this.mook.actionState.ToString() + " \n ";
			foreach (ActionObject actionObject in this.actionQueue)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					actionObject.type.ToString(),
					", ",
					actionObject.duration,
					"\n"
				});
			}
			LevelEditorGUI.DebugText = text;
		}
		else if (this.unit != null)
		{
			string text3 = this.unit.actionState.ToString() + " \n ";
			foreach (ActionObject actionObject2 in this.actionQueue)
			{
				string text2 = text3;
				text3 = string.Concat(new object[]
				{
					text2,
					actionObject2.type.ToString(),
					", ",
					actionObject2.duration,
					"\n"
				});
			}
			LevelEditorGUI.DebugText = text3;
		}
	}

	public virtual void FullyAlert(float x, float y, int playerNum)
	{
		if (this.mentalState != MentalState.Panicking && this.mentalState != MentalState.Attracted)
		{
			this.seenPlayerNum = playerNum;
			if (this.mentalState != MentalState.Alerted)
			{
				this.actionQueue.Clear();
				this.AddAction(EnemyActionType.Wait, 0.2f);
				this.AddAction(EnemyActionType.GreetPlayer);
				this.AddAction(EnemyActionType.Wait, this.sightDelay);
				if (this.seenPlayerNum > -1)
				{
					this.walkDirection = (int)Mathf.Sign(HeroController.GetPlayerPosition(this.seenPlayerNum).x - base.transform.position.x);
					this.AddAction(EnemyActionType.FacePoint, Map.GetGridPoint(HeroController.GetPlayerPosition(this.seenPlayerNum)));
				}
				else
				{
					this.walkDirection = ((UnityEngine.Random.value <= 0.5f) ? 1 : -1);
				}
				this.SetMentalState(MentalState.Alerted);
			}
		}
	}

	public virtual bool Panic(int direction, bool forgetPlayer)
	{
		if (direction != this.walkDirection)
		{
			this.walkDirection = direction;
			this.actionQueue.Clear();
		}
		if (this.mentalState != MentalState.Panicking)
		{
			this.mook.PlayPanicSound();
			this.actionQueue.Clear();
			this.walkDirection = direction;
			this.SetMentalState(MentalState.Panicking);
			if (forgetPlayer)
			{
				this.seenPlayerNum = -1;
			}
			return true;
		}
		return false;
	}

	internal void StopPanicking()
	{
		if (this.mentalState == MentalState.Panicking)
		{
			this.Reassess();
			this.actionQueue.Clear();
			if (this.seenPlayerNum < 0)
			{
				this.SetMentalState(MentalState.Idle);
			}
			else
			{
				this.SetMentalState(MentalState.Alerted);
			}
		}
	}

	public virtual void StopBeingBlind()
	{
		if (this.mentalState == MentalState.Panicking)
		{
			this.StopPanicking();
		}
	}

	public void TryForgetPlayer(int deadPayerNum)
	{
		if (this.seenPlayerNum == deadPayerNum)
		{
			this.AddAction(EnemyActionType.BecomeIdle, global::QueueMode.Last);
		}
	}

	internal MentalState GetThinkState()
	{
		return this.mentalState;
	}

	internal bool IsAlerted()
	{
		return this.mentalState == MentalState.Alerted;
	}

	internal void Reassess()
	{
		if (this.mentalState != MentalState.Panicking)
		{
			this.SetupAvailableGridPoints();
		}
	}

	internal void SetMentalState(MentalState newMentalState)
	{
		this.mentalState = newMentalState;
		if (this.mentalState == MentalState.Suspicious)
		{
			this.suspciousThinkCount = 0;
		}
	}

	internal void ShowQuestionBubble()
	{
		if (this.unit.health > 0 && this.mook != null)
		{
			if (this.mentalState == MentalState.Attracted && UnityEngine.Random.value < 0.7f)
			{
				this.mook.PlayAttractedSound();
			}
			else
			{
				this.mook.PlayConfusedSound();
			}
			this.RestartQuestionBubble_Networked();
		}
	}

	internal void Alert(float alertX, float alertY, bool showBubbleInstantly)
	{
		throw new NotImplementedException();
	}

	protected virtual void LookForPlayer()
	{
		if (HeroController.CanSeePlayer(this.unit.x, this.unit.y, this.FacingDirection, (float)this.sightRangeX, (float)this.sightRangeY, ref this.seenPlayerNum))
		{
			this.FullyAlert(HeroController.GetPlayerPosition(this.seenPlayerNum).x, HeroController.GetPlayerPosition(this.seenPlayerNum).y, this.seenPlayerNum);
		}
	}

	public virtual void HideSpeachBubbles()
	{
		this.exclamationMark.gameObject.SetActive(false);
		this.questionMark.gameObject.SetActive(false);
	}

	public virtual void RestartExclamationBubble_Networked()
	{
		if (base.IsMine)
		{
			Networking.RPC(PID.TargetOthers, new RpcSignature(this.exclamationMark.RestartBubble), true);
		}
		this.exclamationMark.RestartBubble();
	}

	public virtual void RestartQuestionBubble_Networked()
	{
		if (base.IsMine)
		{
			Networking.RPC(PID.TargetOthers, new RpcSignature(this.questionMark.RestartBubble), true);
		}
		this.questionMark.RestartBubble();
	}

	protected virtual void GreetPlayer()
	{
		if (this.mook != null)
		{
			this.mook.PlayGreetingSound();
		}
		this.RestartExclamationBubble_Networked();
	}

	protected bool IsBlockedForward()
	{
		RaycastHit raycastHit;
		return Physics.Raycast(new Vector3(this.unit.x - (float)(this.walkDirection * 3), this.unit.y + 6f, 0f), new Vector3((float)this.walkDirection, 0f, 0f), out raycastHit, 16f, Map.groundLayer | Map.fragileLayer);
	}

	internal virtual void FetchObject(Transform obj)
	{
	}

	protected virtual void SetDeltaTime()
	{
		this.t = Time.deltaTime * (float)((!PlayerOptions.Instance.hardMode) ? 3 : 1);
	}

	internal void FollowPath(NavPath path)
	{
		this.pathAgent.CurrentPath = path;
		this.AddAction(EnemyActionType.FollowPath, path.TargetPoint, global::QueueMode.Clear);
	}

	internal void ClearActionQueue()
	{
		this.actionQueue.Clear();
		this.hasActionQueueChanged = true;
	}

	public virtual void Land()
	{
	}

	public virtual void StopStomping()
	{
	}

	public MentalState mentalState;

	public float blockOffset;

	protected RaycastHit rayCastHit;

	protected bool firstFrame = true;

	public List<ActionObject> actionQueue = new List<ActionObject>();

	public ReactionBubble exclamationMark;

	public ReactionBubble questionMark;

	protected PathAgent pathAgent;

	protected float t;

	protected Unit unit;

	protected Mook mook;

	protected int seenPlayerNum = -1;

	protected int prevSeenPlayerNum = -1;

	protected List<GridPoint> availableGridPoints = new List<GridPoint>();

	public int sightRangeX = 300;

	public int sightRangeY = 20;

	public float attackTime = 0.06f;

	public float sightDelay = 0.55f;

	public float delayBetweenFiring = 0.55f;

	public bool willClimbThroughVents;

	public int patrolBlocksRadius = 2;

	public int walkDirection = -1;

	protected int thoughtsSincePlayerSeen;

	protected int suspciousThinkCount;

	private float timeStuckAgainstWall;

	private float timeOnSameSpot;

	private int lastCollumn;

	private int lastRow;

	private int framesSinceSync = 100;

	private bool hasActionQueueChanged;

	protected GridPoint attractPoint;
}
