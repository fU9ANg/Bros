// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class EnemyAI : NetworkObject
{
	protected virtual void Awake()
	{
		this.walkDirection = UnityEngine.Random.Range(0, 1) * 2 - 1;
		this.thinkCount = UnityEngine.Random.Range(4, 6);
		this.mook = base.GetComponent<Mook>();
		this.unit = base.GetComponent<Unit>();
		this.panicDirection = UnityEngine.Random.Range(0, 1) * 2 - 1;
		this.pathAgent = base.GetComponent<PathAgent>();
	}

	public bool IsUnawares()
	{
		return !this.isAwareOfPlayer && (!this.isAlerted || (!this.hasNotifiedStatisticsOfIsAware && this.confusionCounter > 0f));
	}

	public bool IsAlerted()
	{
		return this.isAlerted;
	}

	public bool IsAwareOfPlayer()
	{
		return this.isAwareOfPlayer;
	}

	public EnemyActionState GetThinkState()
	{
		return this.thinkState;
	}

	public virtual void SetPatrolRange(int range)
	{
		this.maxPatrolBlockRange = range;
	}

	public virtual void ShowQuestionBubble()
	{
		if (this.unit.health > 0 && this.mook != null)
		{
			this.mook.PlayConfusedSound();
			this.RestartQuestionBubble_Networked();
		}
	}

	protected virtual void CheckNearbyBlocks()
	{
		if (this.isBlind)
		{
			this.xMin = -1000f;
			this.xMax = 5000f;
			return;
		}
		if (Physics.Raycast(new Vector3(this.unit.x, this.unit.y + 6f, 0f), Vector3.down, out this.rayCastHit, 64f, Map.groundLayer))
		{
			int num = 0;
			int num2 = 0;
			Map.GetRowCollumn(this.unit.x, this.unit.y, ref num, ref num2);
			if (!Map.IsBlockSolid(num2 - 1, num - 1) || Map.IsBlockSolid(num2 - 1, num))
			{
				this.xMin = this.unit.x - 4f;
			}
			else if (!Map.IsBlockSolid(num2 - 2, num - 1) || Map.IsBlockSolid(num2 - 2, num))
			{
				this.xMin = this.unit.x - 20f;
			}
			else if (this.maxPatrolBlockRange < 3 || !Map.IsBlockSolid(num2 - 3, num - 1) || Map.IsBlockSolid(num2 - 3, num))
			{
				this.xMin = this.unit.x - 36f;
			}
			else if (this.maxPatrolBlockRange < 4 || !Map.IsBlockSolid(num2 - 4, num - 1) || Map.IsBlockSolid(num2 - 4, num))
			{
				this.xMin = this.unit.x - 48f;
			}
			else if (this.maxPatrolBlockRange < 5 || !Map.IsBlockSolid(num2 - 5, num - 1) || Map.IsBlockSolid(num2 - 5, num))
			{
				this.xMin = this.unit.x - 64f;
			}
			else
			{
				this.xMin = this.unit.x - 80f;
			}
			if (!Map.IsBlockSolid(num2 + 1, num - 1) || Map.IsBlockSolid(num2 + 1, num))
			{
				this.xMax = this.unit.x + 4f;
			}
			else if (!Map.IsBlockSolid(num2 + 2, num - 1) || Map.IsBlockSolid(num2 + 2, num))
			{
				this.xMax = this.unit.x + 20f;
			}
			else if (this.maxPatrolBlockRange < 3 || !Map.IsBlockSolid(num2 + 3, num - 1) || Map.IsBlockSolid(num2 + 3, num))
			{
				this.xMax = this.unit.x + 36f;
			}
			else if (this.maxPatrolBlockRange < 4 || !Map.IsBlockSolid(num2 + 4, num - 1) || Map.IsBlockSolid(num2 + 4, num))
			{
				this.xMax = this.unit.x + 48f;
			}
			else if (this.maxPatrolBlockRange < 5 || !Map.IsBlockSolid(num2 + 5, num - 1) || Map.IsBlockSolid(num2 + 5, num))
			{
				this.xMax = this.unit.x + 64f;
			}
			else
			{
				this.xMax = this.unit.x + 80f;
			}
		}
		else
		{
			this.xMin = this.unit.x - 32f;
			this.xMax = this.unit.x + 32f;
		}
	}

	protected virtual void Start()
	{
		if (this.unit != null)
		{
			this.CheckNearbyBlocks();
		}
		this.xLength = this.xMax - this.xMin;
		this.walkDirection = (int)base.transform.localScale.x;
	}

	protected virtual bool ReadyToThink
	{
		get
		{
			return this.thinkCounter > 1f;
		}
	}

	protected virtual void SetDeltaTime()
	{
		this.t = Time.deltaTime;
	}

	protected virtual void Update()
	{
		if (Map.isEditing || CutsceneController.isInCutscene)
		{
			return;
		}
		this.UpdateOwnership();
		this.SetDeltaTime();
		this.outOfSyncTimer += this.t;
		this.thinkCounter += this.t;
		if (base.IsMine && this.ReadyToThink)
		{
			this.thinkCounter = 0f - UnityEngine.Random.value * 0.1f;
			this.Think_Networked();
		}
		if (this.isAttractedTime > 0f)
		{
			this.isAttractedTime -= Time.deltaTime;
			if (this.isAttractedTime <= 0f)
			{
				this.isAttracted = false;
			}
		}
		if (this.confusionCounter > 0f)
		{
			this.confusionCounter -= Time.deltaTime;
			if (this.confusionCounter <= 0f && !this.isAwareOfPlayer && this.unit.health > 0)
			{
				this.mook.PlayConfusedSound();
				this.RestartQuestionBubble_Networked();
			}
		}
		if (this.panicDirectionCounter > 0f)
		{
			this.panicDirectionCounter -= Time.deltaTime;
		}
		if (this.forgetPlayerCounter > 0f)
		{
			this.forgetPlayerCounter -= Time.deltaTime;
			if (this.forgetPlayerCounter <= 0f)
			{
				this.ForgetPlayer();
			}
		}
		this.CheckNotifyStatisticsShooting();
	}

	protected virtual void CheckNotifyStatisticsShooting()
	{
		if (!this.hasNotifiedStatisticsOfShooting && this.thinkState == EnemyActionState.Shooting)
		{
			this.hasNotifiedStatisticsOfShooting = true;
			if (this.mook.CanAddToStatistics())
			{
				StatisticsController.NotifyMookTryShootAtBBro(this.mook);
			}
		}
	}

	protected virtual void CheckNotifyStatisticsAlerted()
	{
		if (!this.hasNotifiedStatisticsOfIsAware && this.mook.CanAddToStatistics())
		{
			this.hasNotifiedStatisticsOfIsAware = true;
			StatisticsController.NotifyMookSeenBro(this.mook);
		}
	}

	protected virtual void CheckNotifyStatisticsHeardSound()
	{
		if (!this.hasNotifiedStatisticsOfHeardSound && this.mook.CanAddToStatistics())
		{
			this.hasNotifiedStatisticsOfHeardSound = true;
			StatisticsController.NotifyMookHeardSound(this.mook);
		}
	}

	protected void Think_Networked()
	{
		if ((base.IsMine || false) && this.thinkState != EnemyActionState.Minion)
		{
			this.Think();
			byte[] state = base.GetState();
			if (!base.Syncronize)
			{
				Networking.RPC<byte[]>(PID.TargetOthers, new RpcSignature<byte[]>(base.SetState), state, true);
			}
		}
	}

	protected virtual void UpdateOwnership()
	{
		if (this.seenPlayerNum != this.prevSeenPlayerNum)
		{
			this.prevSeenPlayerNum = this.seenPlayerNum;
			PID pid = null;
			if (this.seenPlayerNum >= 0 && this.seenPlayerNum < 4)
			{
				pid = HeroController.PIDS[this.seenPlayerNum];
			}
			if (pid != null)
			{
				base.gameObject.SetOwnerNetworked(base.Owner);
			}
		}
		if (base.Owner == null)
		{
			base.gameObject.SetOwnerNetworked(PID.ServerID);
		}
		if (base.Owner == null)
		{
			base.gameObject.SetOwnerNetworked(PID.ServerID);
			MonoBehaviour.print("Stange... Owner is null");
		}
	}

	protected void DoFollowingPathThink()
	{
		this.LookForPlayer((int)base.transform.localScale.x);
		this.CheckGreetPlayer();
	}

	protected void DoExitingPathThink()
	{
		this.LookForPlayer((int)base.transform.localScale.x);
		this.CheckGreetPlayer();
		if (this.thinkCount > 1)
		{
			this.thinkState = EnemyActionState.Idle;
		}
	}

	protected void DoAttractedThink()
	{
		if (Mathf.Sign((float)this.walkDirection) != Mathf.Sign(this.alertedX - this.unit.x))
		{
			this.TurnAround();
			this.thinkCounter = 0.6f - UnityEngine.Random.value * 0.5f;
			this.thinkState = EnemyActionState.Idle;
			this.confusionCounter = 1f - this.thinkCounter;
		}
		else
		{
			this.walkDirection = (int)Mathf.Sign(this.alertedX - this.unit.x);
			this.thinkState = EnemyActionState.Moving;
			this.thinkCounter = 0.7f - UnityEngine.Random.value * 0.2f;
		}
	}

	protected virtual void DoBlindThink()
	{
		switch (this.thinkCount % 4)
		{
		case 0:
			if (HeroController.CanSeePlayer(this.unit.x, this.unit.y, this.walkDirection, (float)this.sightRangeX, (float)this.sightRangeY, ref this.seenPlayerNum))
			{
				this.TurnAround();
			}
			this.SetThinkState(EnemyActionState.Shooting);
			this.thinkCounter = 1f - this.attackTime * 0.2f + 0.05f;
			break;
		case 1:
			this.TurnAround();
			this.thinkState = EnemyActionState.Idle;
			this.thinkCounter = 1f - UnityEngine.Random.value * 0.3f - 0.06f;
			break;
		case 2:
			if (HeroController.CanSeePlayer(this.unit.x, this.unit.y, this.walkDirection, (float)this.sightRangeX, (float)this.sightRangeY, ref this.seenPlayerNum))
			{
				this.thinkCounter = 0.9f - UnityEngine.Random.value * 0.9f;
				this.thinkState = EnemyActionState.Moving;
			}
			else
			{
				this.SetThinkState(EnemyActionState.Shooting);
				this.thinkCounter = 1f - this.attackTime * 0.2f + 0.05f;
			}
			break;
		case 3:
			this.TurnAround();
			this.thinkState = EnemyActionState.Moving;
			this.thinkCounter = 1f - UnityEngine.Random.value * 0.9f - 0.1f;
			break;
		}
		if (this.thinkState != EnemyActionState.Shooting && Physics.Raycast(new Vector3(this.unit.x - (float)(this.walkDirection * 5), this.unit.y + 6f, 0f), new Vector3((float)this.walkDirection, 0f, 0f), 24f, Map.groundLayer))
		{
			this.TurnAround();
		}
	}

	protected virtual void DoPanicThink()
	{
		if (this.panicDirectionCounter <= 0f)
		{
			if (Physics.Raycast(new Vector3(this.unit.x - (float)(this.walkDirection * 5), this.unit.y + 6f, 0f), new Vector3((float)this.walkDirection, 0f, 0f), 24f, Map.groundLayer))
			{
				this.TurnAround();
			}
			else if (UnityEngine.Random.value > 0.85f)
			{
				this.TurnAround();
			}
		}
	}

	protected virtual void DoIdleThink()
	{
		switch (this.thinkCount % 7)
		{
		case 0:
			if (UnityEngine.Random.value > 0.7f)
			{
				this.TurnAround();
			}
			this.LookForPlayer(this.walkDirection);
			this.CheckGreetPlayer();
			return;
		case 1:
			if (UnityEngine.Random.value > 0.7f)
			{
				this.StartMoving();
				this.thinkCounter = 1f - this.walkTime * 0.6f - UnityEngine.Random.value * this.walkTime * 0.4f;
			}
			else
			{
				this.LookForPlayer(this.walkDirection);
				this.CheckGreetPlayer();
			}
			return;
		case 4:
			this.thinkState = EnemyActionState.Idle;
			this.TurnAround();
			this.thinkCounter = 0.9f - UnityEngine.Random.value * 0.1f;
			return;
		case 5:
			this.StartMoving();
			this.thinkCounter = 1f - this.walkTime * 0.6f - UnityEngine.Random.value * this.walkTime * 0.3f;
			return;
		}
		this.thinkState = EnemyActionState.Idle;
		this.thinkCounter = 0.1f - UnityEngine.Random.value * 0.5f;
		this.LookForPlayer(this.walkDirection);
		this.CheckGreetPlayer();
	}

	protected virtual void DoAlertedThink()
	{
		this.CheckNotifyStatisticsAlerted();
		if (Time.time - this.alertTime > 6f)
		{
			if (this.thinkCount < 0)
			{
				this.DoFirstAwareThink();
			}
			else
			{
				switch (this.thinkCount % 4)
				{
				case 0:
					this.SetThinkState(EnemyActionState.Shooting);
					this.thinkCounter = 1f - this.attackTime;
					break;
				case 1:
					this.thinkState = EnemyActionState.Idle;
					this.thinkCounter = -1.8f - UnityEngine.Random.value * 0.55f;
					if (UnityEngine.Random.value > 0.8f && !HeroController.PlayerIsAlive(this.seenPlayerNum))
					{
						this.isAwareOfPlayer = false;
					}
					if (!this.IsPlayerThisWay(this.walkDirection))
					{
						this.TurnAround();
					}
					break;
				case 2:
					if (!this.IsPlayerThisWay(this.walkDirection))
					{
						this.StartMoving();
						this.thinkCounter = 0.840000033f - UnityEngine.Random.value * 0.05f;
						this.TurnAround();
					}
					else if (UnityEngine.Random.value < 0.75f && HeroController.IsPlayerNearby(base.transform.position.x - (float)(this.walkDirection * 30), base.transform.position.y + 10f, this.walkDirection, 400f, 50f))
					{
						this.SetThinkState(EnemyActionState.Shooting);
						this.thinkCounter = 1f - this.attackTime;
					}
					else if (UnityEngine.Random.value < 0.3f)
					{
						this.StartMoving();
						this.thinkCounter = 0.840000033f - UnityEngine.Random.value * 0.05f;
						if (UnityEngine.Random.value < 0.66f)
						{
							this.TurnAround();
						}
					}
					else
					{
						this.thinkState = EnemyActionState.Idle;
					}
					break;
				case 3:
					this.thinkState = EnemyActionState.Idle;
					this.thinkCounter = 0.74f;
					if (!this.IsPlayerThisWay(this.walkDirection))
					{
						this.thinkCounter -= 0.5f;
						this.TurnAround();
					}
					break;
				}
			}
		}
		else if (this.thinkCount < 0)
		{
			this.DoFirstAwareThink();
		}
		else
		{
			switch (this.thinkCount % 4)
			{
			case 0:
				this.SetThinkState(EnemyActionState.Shooting);
				this.thinkCounter = 1f - this.attackTime;
				break;
			case 1:
				this.thinkState = EnemyActionState.Idle;
				this.thinkCounter = -1f;
				if (UnityEngine.Random.value > 0.3f && this.seenPlayerNum >= 0 && this.seenPlayerNum < 5 && !HeroController.PlayerIsAlive(this.seenPlayerNum))
				{
					this.isAwareOfPlayer = false;
				}
				break;
			case 2:
				if (UnityEngine.Random.value < 0.7f && HeroController.IsPlayerNearby(base.transform.position.x - (float)(this.walkDirection * 30), base.transform.position.y + 10f, this.walkDirection, 300f, 50f))
				{
					this.SetThinkState(EnemyActionState.Shooting);
					this.thinkCounter = 1f - this.attackTime;
				}
				else
				{
					this.thinkCounter = 1f - UnityEngine.Random.value * 0.1f;
					this.thinkState = EnemyActionState.Idle;
				}
				break;
			case 3:
				this.thinkState = EnemyActionState.Idle;
				this.thinkCounter = 0.74f;
				break;
			}
		}
	}

	protected void DoFirstAwareThink()
	{
		if (!this.IsPlayerThisWay(this.walkDirection))
		{
			this.StartMoving();
			this.thinkCounter = 1f - this.sightDelay * (0.5f + UnityEngine.Random.value * 0.1f);
			this.TurnAround();
			base.transform.localScale = new Vector3((float)this.walkDirection, 1f, 1f);
		}
		else
		{
			this.SetThinkState(EnemyActionState.Shooting);
			this.thinkCounter = 1f - this.attackTime;
			this.thinkCount = 0;
		}
	}

	protected virtual void DoSuspiciousThink()
	{
		this.CheckNotifyStatisticsHeardSound();
		if (this.isAlerted && Time.time - this.alertTime < 8f)
		{
			int num = this.thinkCount % 6;
			if (num != 0)
			{
				this.thinkState = EnemyActionState.Idle;
				this.thinkCounter = 0.1f + UnityEngine.Random.value * 0.5f;
				this.LookForPlayer(this.walkDirection);
				this.CheckGreetPlayer();
			}
			else
			{
				this.thinkState = EnemyActionState.Moving;
				this.thinkCounter = 1f - this.walkTime - UnityEngine.Random.value * 0.06f;
			}
			if (this.thinkCount >= 4)
			{
				this.thinkCount = 1;
			}
		}
		else
		{
			int num = this.thinkCount % 2;
			if (num != 0)
			{
				if (num == 1)
				{
					this.thinkState = EnemyActionState.Idle;
					this.thinkCounter = 0.840000033f - UnityEngine.Random.value * 2.4f;
					this.LookForPlayer(this.walkDirection);
					base.transform.localScale = new Vector3((float)this.walkDirection, 1f, 1f);
					if (this.isAwareOfPlayer)
					{
						this.thinkCount = -2;
						this.mook.PlayGreetingSound();
						this.RestartExclamationBubble_Networked();
						this.thinkState = EnemyActionState.Idle;
						this.thinkCounter = 1f - this.sightDelay * (1f + UnityEngine.Random.value * 0.33f);
					}
				}
			}
			else
			{
				this.StartMoving();
				float num2 = (this.xMin + this.xMax) / 2f;
				if (this.unit.x > num2)
				{
					this.walkDirection = -1;
				}
				else
				{
					this.walkDirection = 1;
				}
				this.thinkCounter = 1f - this.walkTime - UnityEngine.Random.value * 0.3f * (this.xLength / 48f);
				this.LookForPlayer(this.walkDirection);
				base.transform.localScale = new Vector3((float)this.walkDirection, 1f, 1f);
				if (this.isAwareOfPlayer)
				{
					this.thinkCount = -2;
					this.mook.PlayGreetingSound();
					this.RestartExclamationBubble_Networked();
					this.thinkState = EnemyActionState.Idle;
					this.thinkCounter = 1f - this.sightDelay * (1f + UnityEngine.Random.value * 0.33f);
				}
			}
		}
	}

	protected virtual void Think()
	{
		if (this.unit.health <= 0)
		{
			return;
		}
		this.thinkCount++;
		if (this.thinkState == EnemyActionState.FollowingPath)
		{
			this.DoFollowingPathThink();
		}
		else if (this.thinkState == EnemyActionState.ExitingPath)
		{
			this.DoExitingPathThink();
		}
		else
		{
			if (this.isAttracted)
			{
				this.DoAttractedThink();
				return;
			}
			if (this.isBlind)
			{
				this.DoBlindThink();
			}
			else if (this.thinkState == EnemyActionState.Panic)
			{
				this.DoPanicThink();
			}
			else if (!this.isAlerted && !this.isAwareOfPlayer)
			{
				this.DoIdleThink();
			}
			else if (this.isAwareOfPlayer)
			{
				this.DoAlertedThink();
			}
			else
			{
				this.DoSuspiciousThink();
			}
		}
	}

	public void TryForgetPlayer(int deadPayerNum)
	{
		if (this.seenPlayerNum == deadPayerNum)
		{
			this.forgetPlayerCounter = 0.5f + UnityEngine.Random.value;
		}
	}

	protected void CheckGreetPlayer()
	{
		if (this.isAwareOfPlayer && this.thinkState != EnemyActionState.Panic)
		{
			this.alertTime = Time.time;
			this.thinkCount = -2;
			this.mook.PlayGreetingSound();
			this.RestartExclamationBubble_Networked();
			this.thinkCounter = 1f - this.sightDelay * (1f + UnityEngine.Random.value * 0.33f);
			this.thinkState = EnemyActionState.Idle;
		}
	}

	public virtual void TurnAround_Networked(float x, float y, int walkDirection)
	{
		if (base.IsMine)
		{
			Networking.RPC<float, float, int>(PID.TargetOthers, new RpcSignature<float, float, int>(this.TurnAround_Local), x, y, walkDirection, false);
		}
		this.TurnAround_Local(x, y, walkDirection);
	}

	public virtual void TurnAround_Local(float x, float y, int walkDirection)
	{
		if (this.mook.health <= 0)
		{
			return;
		}
		this.TurnAround();
		this.SetThinkCounter(0.4f);
		this.ForgetPlayer();
		if (x < this.xMin || x > this.xMax)
		{
			this.CheckNearbyBlocks();
		}
		this.mook.x = x;
		this.mook.y = y;
	}

	public virtual void Jump_Networked(float x, float y, int walkDirection, float jumpSpeed)
	{
	}

	public virtual void StartAttack_Networked()
	{
	}

	public virtual void RestartExclamationBubble_Networked()
	{
		if (base.IsMine)
		{
			Networking.RPC(PID.TargetOthers, new RpcSignature(this.exclamationMark.RestartBubble), true);
		}
		this.exclamationMark.RestartBubble();
	}

	public void RestartQuestionBubble_Networked()
	{
		if (base.IsMine)
		{
			Networking.RPC(PID.TargetOthers, new RpcSignature(this.questionMark.RestartBubble), true);
		}
		this.questionMark.RestartBubble();
	}

	public void QuestionMarkRPC()
	{
		if (this.unit.health > 0)
		{
			this.questionMark.RestartBubble();
		}
	}

	protected virtual void LookForPlayer(int direction)
	{
		if (Demonstration.enemiesAlreadyAware)
		{
			this.isAwareOfPlayer = true;
			if (!this.IsPlayerThisWay(this.walkDirection))
			{
				this.TurnAround();
			}
		}
		else
		{
			if (!SetResolutionCamera.IsItVisible(base.transform.position))
			{
				return;
			}
			this.isAwareOfPlayer = HeroController.CanSeePlayer(this.unit.x, this.unit.y, direction, (float)this.sightRangeX, (float)this.sightRangeY, ref this.seenPlayerNum);
			if (this.isAwareOfPlayer)
			{
				this.thinkCounter = 1f - this.sightDelay * (1f + UnityEngine.Random.value * 0.33f);
				if (!this.IsPlayerThisWay(this.walkDirection))
				{
					this.TurnAround();
				}
				else if (this.walkDirection < 0)
				{
					this.moveLeft1Frame = true;
				}
				else if (this.walkDirection > 0)
				{
					this.moveRight1Frame = true;
				}
				else
				{
					UnityEngine.Debug.LogError("Walking like a fuckup");
				}
			}
		}
		if (this.isAwareOfPlayer && (!this.isAlerted || Time.time - this.alertTime > 10f))
		{
			this.alertTime = Time.time;
			this.isAlerted = true;
		}
	}

	protected virtual bool IsPlayerThisWay(int direction)
	{
		if (this.seenPlayerNum >= 0)
		{
			return HeroController.IsPlayerThisWay(this.seenPlayerNum, base.transform.position.x, base.transform.position.y + 10f, direction);
		}
		return HeroController.IsPlayerThisWay(base.transform.position.x, base.transform.position.y + 10f, direction);
	}

	public virtual bool Panic(int direction)
	{
		if (direction == 0)
		{
			if (UnityEngine.Random.value > 0.1f)
			{
				direction = -this.walkDirection;
			}
			else
			{
				direction = this.walkDirection;
			}
		}
		this.panicDirection = direction; this.walkDirection = (this.panicDirection );
		base.transform.localScale = new Vector3((float)this.walkDirection, 1f, 1f);
		if (!this.isBlind)
		{
			if (this.thinkState != EnemyActionState.Panic)
			{
				this.mook.PlayPanicSound();
			}
			this.thinkState = EnemyActionState.Panic;
			this.thinkCounter = 0.7f;
			return true;
		}
		return false;
	}

	public virtual float GetThinkCounter()
	{
		return this.thinkCounter;
	}

	public virtual void SetThinkState(EnemyActionState newThinkState)
	{
		this.thinkState = newThinkState;
		if (this.thinkState == EnemyActionState.Shooting && (this.unit.actionState != ActionState.Jumping || this.mook.IsParachuteActive))
		{
			this.shoot1Frame = true;
		}
		if (base.transform.localScale.x < 0f && this.walkDirection > 0)
		{
			this.moveRight1Frame = true;
		}
		if (base.transform.localScale.x > 0f && this.walkDirection < 0)
		{
			this.moveLeft1Frame = true;
		}
	}

	public virtual void SetThinkCounter(float time)
	{
		this.thinkCounter = 1f - time;
		this.panicDirectionCounter = time;
	}

	public virtual bool Blind()
	{
		if (!this.isBlind)
		{
		}
		this.isBlind = true;
		this.thinkState = EnemyActionState.Idle;
		this.thinkCounter = -0.85f - UnityEngine.Random.value;
		this.isAwareOfPlayer = false;
		this.isAlerted = false;
		this.xMin = -1000f;
		this.xMax = 5000f;
		return true;
	}

	public virtual void Alert(float x, float y)
	{
		this.Alert(x, y, false);
	}

	public virtual void Alert(float x, float y, bool showAlertaInstantly)
	{
		if (!this.isBlind && this.thinkState != EnemyActionState.Panic && !this.isAwareOfPlayer)
		{
			if (!this.isAlerted && Time.time - this.alertTime > 3f)
			{
				if (showAlertaInstantly)
				{
					this.confusionCounter = 0.1f + UnityEngine.Random.value * 0.2f;
				}
				else
				{
					this.confusionCounter = 1f + UnityEngine.Random.value;
				}
				this.alertedCounter = 13f - UnityEngine.Random.value * 6f;
				this.thinkCounter = 0.4f + UnityEngine.Random.value * 0.2f;
				this.CheckNotifyStatisticsHeardSound();
				this.isAlerted = true;
				this.thinkCount = 0;
				this.alertedX = x;
				this.alertedY = y;
				this.alertTime = Time.time - 2f + UnityEngine.Random.value * 0.5f;
				if (x < this.unit.x && this.walkDirection > 0)
				{
					this.TurnAround();
				}
				else if (x > this.unit.x && this.walkDirection < 0)
				{
					this.TurnAround();
				}
			}
			else if (this.isAlerted && Time.time - this.alertTime > 3f)
			{
				this.alertTime = Time.time - 2f + UnityEngine.Random.value * 0.5f;
				this.thinkCounter = 0.4f + UnityEngine.Random.value * 0.2f;
				this.thinkCount = 0;
				this.isSearching = true;
				if (x < this.unit.x && this.walkDirection > 0)
				{
					this.TurnAround();
				}
				else if (x > this.unit.x && this.walkDirection < 0)
				{
					this.TurnAround();
				}
			}
		}
	}

	public virtual void FullyAlert(float x, float y, int playerNum)
	{
		if (this.thinkState != EnemyActionState.Panic && !this.isBlind && !this.isAttracted)
		{
			if (!this.isAwareOfPlayer)
			{
				this.isAwareOfPlayer = true; this.isAlerted = (this.isAwareOfPlayer );
				this.seenPlayerNum = playerNum;
				this.CheckGreetPlayer();
			}
			if (Mathf.Sign(x - this.unit.x) != Mathf.Sign((float)this.walkDirection) && this.thinkCount > 1 && HeroController.CanSeePlayer(this.unit.x, this.unit.y, this.walkDirection, (float)this.sightRangeX, (float)this.sightRangeY, ref this.seenPlayerNum))
			{
				this.thinkCount = -2;
				this.thinkState = EnemyActionState.Idle;
				this.TurnAround();
				this.thinkCounter = 1f - this.sightDelay * (1f + UnityEngine.Random.value * 0.33f);
			}
		}
	}

	public virtual void Attract(float xTarget, float yTarget)
	{
		this.alertedX = xTarget;
		this.alertedY = yTarget;
		this.isAttracted = true;
		this.isAttractedTime = 0.4f;
		this.isAlerted = false;
		this.isAwareOfPlayer = false;
		if (this.thinkCounter < 0.2f)
		{
			this.thinkCounter = 0.2f;
		}
	}

	public virtual void SetTurnAroundDelay()
	{
		this.thinkCounter = Mathf.Min(0.5f - this.sightDelay - UnityEngine.Random.value * 0.12f, this.thinkCounter);
	}

	public virtual void HearSound(float alertX, float alertY)
	{
		if (!this.isBlind && this.thinkState != EnemyActionState.Panic)
		{
			if (this.isAwareOfPlayer)
			{
				if (this.thinkState != EnemyActionState.Shooting && Mathf.Sign((float)this.walkDirection) != Mathf.Sign(alertX - this.unit.x))
				{
					if (HeroController.CanSeePlayer(this.unit.x, this.unit.y, (int)Mathf.Sign(alertX - this.unit.x), (float)this.sightRangeX, (float)(this.sightRangeY + 80), ref this.seenPlayerNum))
					{
						this.TurnAround();
						this.SetTurnAroundDelay();
						this.alertTime = Time.time;
					}
				}
				else if (HeroController.CanSeePlayer(this.unit.x, this.unit.y, (int)Mathf.Sign(alertX - this.unit.x), (float)this.sightRangeX, (float)(this.sightRangeY + 80), ref this.seenPlayerNum))
				{
					this.alertTime = Time.time;
				}
			}
			else if (this.isAlerted && this.thinkState != EnemyActionState.Panic && Time.time - this.alertTime > 1.5f && Mathf.Sign((float)this.walkDirection) != Mathf.Sign(alertX - this.unit.x))
			{
				if (HeroController.CanSeePlayer(this.unit.x, this.unit.y, (int)Mathf.Sign(alertX - this.unit.x), (float)this.sightRangeX, (float)(this.sightRangeY + 80), ref this.seenPlayerNum))
				{
					this.TurnAround();
					this.thinkState = EnemyActionState.Idle;
					if (Mathf.Sign(this.alertedX - this.unit.x) == Mathf.Sign(alertX - this.unit.x))
					{
						this.thinkCounter = 0.13f;
					}
					else
					{
						this.thinkCounter = -0.37f;
						this.confusionCounter = 0.5f + UnityEngine.Random.value * 0.5f;
					}
					this.alertTime = Time.time;
					this.alertedX = alertX;
					this.alertedY = alertY;
				}
				else
				{
					this.TurnAround();
					this.thinkState = EnemyActionState.Idle;
					this.thinkCounter = -0.37f;
					this.confusionCounter = 0.5f + UnityEngine.Random.value * 0.5f;
					this.alertTime = Time.time;
					this.alertedX = alertX;
					this.alertedY = alertY;
				}
			}
		}
	}

	public virtual void Reassess()
	{
		if (!this.isBlind && this.thinkState != EnemyActionState.Panic)
		{
			if (this.mook.xI > 0f)
			{
				this.walkDirection = 1;
			}
			else if (this.mook.xI < 0f)
			{
				this.walkDirection = -1;
			}
			this.CheckNearbyBlocks();
			if (this.thinkCounter > 0f && this.thinkState != EnemyActionState.Shooting)
			{
				this.thinkCounter = 1f - this.sightDelay;
			}
			if (this.isAlerted && this.isAwareOfPlayer && HeroController.CanSeePlayer(this.unit.x, this.unit.y, -this.walkDirection, 300f, 20f, ref this.seenPlayerNum))
			{
				this.thinkCount = -2;
				UnityEngine.Debug.Log("is Now Aware from reassess");
				this.thinkState = EnemyActionState.Idle;
				this.TurnAround();
				this.thinkCounter = 1f - this.sightDelay * (1f + UnityEngine.Random.value * 0.33f);
			}
			if (base.IsMine)
			{
			}
		}
	}

	public virtual void StopPanicking()
	{
		this.CheckNearbyBlocks();
		this.thinkState = EnemyActionState.Idle;
		this.thinkCounter = -0.5f;
		this.isBlind = false;
	}

	public virtual void StopBeingBlind()
	{
		if (this.isBlind)
		{
			this.CheckNearbyBlocks();
		}
		this.thinkState = EnemyActionState.Idle;
		this.thinkCounter = -0.5f;
		this.isBlind = false;
		this.ForgetPlayer();
	}

	public virtual void ForgetPlayer()
	{
		this.isAlerted = false;
		this.thinkState = EnemyActionState.Idle;
		this.isAwareOfPlayer = false;
		this.seenPlayerNum = -1;
	}

	protected virtual void GetMovementIdle(ref bool left, ref bool right, ref bool up, ref bool down, ref bool jump, ref bool fire, ref bool special)
	{
		left = false;
		right = false;
		up = false;
		down = false;
		fire = false;
		special = false;
		jump = false;
	}

	protected virtual void GetMovementMoving(ref bool left, ref bool right, ref bool up, ref bool down, ref bool jump, ref bool fire, ref bool special1, ref bool special2)
	{
		up = false;
		fire = false;
		special1 = false;
		left = false;
		right = false;
		if (!this.ReadyToThink)
		{
			if (!this.isAttracted)
			{
				if (this.walkDirection < 0 && this.unit.x < this.xMin && !this.isAwareOfPlayer)
				{
					this.TurnAround();
				}
				if (this.walkDirection > 0 && this.unit.x > this.xMax && !this.isAwareOfPlayer)
				{
					this.TurnAround();
				}
			}
			if (this.walkDirection < 0)
			{
				left = true;
				right = false;
			}
			else if (this.walkDirection > 0)
			{
				left = false;
				right = true;
			}
		}
	}

	public virtual void GetMovement(ref bool left, ref bool right, ref bool up, ref bool down, ref bool jump, ref bool fire, ref bool special1, ref bool special2, ref bool special3, ref bool special4)
	{
		special2 = false;
		special3 = false;
		special4 = false;
		if ((this.forceHoldLeftTime -= Time.deltaTime) > 0f)
		{
			left = true;
			return;
		}
		if (CutsceneController.isInCutscene)
		{
			this.GetMovementIdle(ref left, ref right, ref up, ref down, ref jump, ref fire, ref special1);
			return;
		}
		switch (this.thinkState)
		{
		case EnemyActionState.Idle:
			this.GetMovementIdle(ref left, ref right, ref up, ref down, ref jump, ref fire, ref special1);
			break;
		case EnemyActionState.Moving:
			this.GetMovementMoving(ref left, ref right, ref up, ref down, ref jump, ref fire, ref special1, ref special2);
			break;
		case EnemyActionState.Shooting:
			left = false;
			right = false;
			up = false;
			if (this.shoot1Frame || ((this.unit.actionState != ActionState.Jumping || this.mook.IsParachuteActive) && !this.ReadyToThink))
			{
				fire = true;
				this.shoot1Frame = false;
			}
			else
			{
				fire = false;
			}
			special1 = false;
			break;
		case EnemyActionState.Panic:
			if (this.walkDirection < 0)
			{
				left = true;
				right = false;
			}
			else if (this.walkDirection > 0)
			{
				left = false;
				right = true;
			}
			up = false;
			fire = false;
			special1 = false;
			break;
		case EnemyActionState.FollowingPath:
		{
			PathAgent component = base.GetComponent<PathAgent>();
			if (component == null)
			{
				UnityEngine.Debug.LogError("Enemy wants to follow path but has no PathAgent Component!");
			}
			else if (component.CurrentPath == null)
			{
				this.thinkState = EnemyActionState.ExitingPath;
				this.walkDirection = global::Math.RandomNegativePositive();
				this.SetPatrolRange(5);
				this.CheckNearbyBlocks();
				this.thinkCounter = 0f + UnityEngine.Random.value * 0.8f;
				this.thinkCount = 0;
			}
			else
			{
				component.GetMove(ref left, ref right, ref up, ref down, ref jump);
			}
			break;
		}
		case EnemyActionState.ExitingPath:
			this.GetMovementMoving(ref left, ref right, ref up, ref down, ref jump, ref fire, ref special1, ref special2);
			break;
		case EnemyActionState.UsingSpecial:
			left = false;
			right = false;
			up = false;
			fire = false;
			special1 = true;
			break;
		case EnemyActionState.UsingSpecial2:
			left = false;
			right = false;
			up = false;
			fire = false;
			special1 = false;
			special2 = true;
			break;
		case EnemyActionState.UsingSpecial3:
			left = false;
			right = false;
			up = false;
			fire = false;
			special1 = false;
			special3 = true;
			break;
		case EnemyActionState.UsingSpecial4:
			left = false;
			right = false;
			up = false;
			fire = false;
			special1 = false;
			special4 = true;
			break;
		}
		if (this.moveLeft1Frame)
		{
			this.moveLeft1Frame = false;
			right = false;
			left = false;
			this.mook.xI = -0.1f;
		}
		if (this.moveRight1Frame)
		{
			this.moveRight1Frame = false;
			right = false;
			left = false;
			this.mook.xI = 0.1f;
		}
	}

	protected virtual void TurnAround()
	{
		this.walkDirection *= -1;
		base.transform.localScale = new Vector3((float)this.walkDirection, 1f, 1f);
		if (this.walkDirection < 0)
		{
			this.moveLeft1Frame = true;
			this.moveRight1Frame = false;
		}
		else
		{
			this.moveRight1Frame = true;
			this.moveLeft1Frame = false;
		}
	}

	protected virtual void StartMoving()
	{
		this.thinkState = EnemyActionState.Moving;
	}

	public void FollowPath(NavPath path)
	{
		base.GetComponent<PathAgent>().CurrentPath = path;
		this.thinkState = EnemyActionState.FollowingPath;
	}

	public virtual void FetchObject(Transform fetchObject)
	{
	}

	internal void ForceHoldLeft(float t)
	{
		this.forceHoldLeftTime = t;
	}

	public override UnityStream PackState(UnityStream stream)
	{
		byte byteFromBoolArray = TypeSerializer.GetByteFromBoolArray(new bool[]
		{
			this.isAlerted,
			this.isSearching,
			this.isBlind,
			this.isAttracted,
			this.moveLeft1Frame,
			this.moveRight1Frame,
			this.shoot1Frame,
			this.isAwareOfPlayer
		});
		stream.Serialize<byte>(byteFromBoolArray);
		stream.Serialize<float>(this.alertTime);
		stream.Serialize<float>(this.confusionCounter);
		stream.Serialize<float>(this.alertedCounter);
		stream.Serialize<float>(this.alertedX);
		stream.Serialize<float>(this.alertedY);
		stream.Serialize<float>(this.isAttractedTime);
		stream.Serialize<int>((int)this.thinkState);
		stream.Serialize<float>(this.thinkCounter);
		stream.Serialize<int>(this.thinkCount);
		stream.Serialize<int>(this.walkDirection);
		stream.Serialize<float>(this.attackTime);
		stream.Serialize<float>(this.sightDelay);
		stream.Serialize<float>(this.targetX);
		stream.Serialize<float>(this.xMin);
		stream.Serialize<float>(this.xMax);
		stream.Serialize<float>(this.xLength);
		stream.Serialize<int>(this.panicDirection);
		stream.Serialize<float>(this.forgetPlayerCounter);
		stream.Serialize<int>(this.seenPlayerNum);
		stream.Serialize<Vector3>(base.transform.position);
		stream.Serialize<Vector3>(base.transform.localScale);
		return base.PackState(stream);
	}

	public override UnityStream UnpackState(UnityStream stream)
	{
		byte b = (byte)stream.DeserializeNext();
		bool[] boolArrayFromByte = TypeSerializer.GetBoolArrayFromByte(b);
		this.isAlerted = boolArrayFromByte[0];
		this.isSearching = boolArrayFromByte[1];
		this.isBlind = boolArrayFromByte[2];
		this.isAttracted = boolArrayFromByte[3];
		this.moveLeft1Frame = boolArrayFromByte[4];
		this.moveRight1Frame = boolArrayFromByte[5];
		this.shoot1Frame = boolArrayFromByte[6];
		this.isAwareOfPlayer = boolArrayFromByte[7];
		this.alertTime = (float)stream.DeserializeNext();
		this.confusionCounter = (float)stream.DeserializeNext();
		this.alertedCounter = (float)stream.DeserializeNext();
		this.alertedX = (float)stream.DeserializeNext();
		this.alertedY = (float)stream.DeserializeNext();
		this.isAttractedTime = (float)stream.DeserializeNext();
		this.thinkState = (EnemyActionState)((int)stream.DeserializeNext());
		this.thinkCounter = (float)stream.DeserializeNext();
		this.thinkCount = (int)stream.DeserializeNext();
		int num = (int)stream.DeserializeNext();
		this.attackTime = (float)stream.DeserializeNext();
		this.sightDelay = (float)stream.DeserializeNext();
		this.targetX = (float)stream.DeserializeNext();
		this.xMin = (float)stream.DeserializeNext();
		this.xMax = (float)stream.DeserializeNext();
		this.xLength = (float)stream.DeserializeNext();
		this.panicDirection = (int)stream.DeserializeNext();
		this.forgetPlayerCounter = (float)stream.DeserializeNext();
		this.seenPlayerNum = (int)stream.DeserializeNext();
		Vector3 position = (Vector3)stream.DeserializeNext();
		if (this != null)
		{
			base.transform.localScale = (Vector3)stream.DeserializeNext();
			this.walkDirection = num;
			UnityEngine.Debug.DrawRay(base.transform.position, Vector3.right * base.transform.localScale.x * 32f, Color.magenta, 1f);
			float num2 = 8f;
			float num3 = Mathf.Abs(position.x - base.transform.position.x);
			float num4 = Mathf.Abs(position.y - base.transform.position.y);
			if (this.unit.actionState == ActionState.Dead)
			{
				this.unit.x = position.x;
				if (position.y > base.transform.position.y)
				{
					this.unit.y = position.y;
				}
			}
			if (this is MookopterAI)
			{
				base.transform.position = position;
				this.unit.x = position.x;
				this.unit.y = position.y;
			}
			bool flag = false;
			if (this.unit.yI == 0f)
			{
				if (num3 > num2 || num4 != 0f)
				{
					flag = true;
				}
				else
				{
					this.outOfSyncUpdatesCounter = 0;
					this.outOfSyncTimer = 0f;
				}
				if (position.y > base.transform.position.y && !(this is MookopterAI))
				{
					flag = false;
				}
			}
			if (flag)
			{
				this.outOfSyncUpdatesCounter++;
				if (this.outOfSyncUpdatesCounter > this.maxOutOfSyncUpdates || this.outOfSyncTimer > 10f)
				{
					this.outOfSyncTimer = 0f;
					this.outOfSyncUpdatesCounter = 0;
					base.transform.position = position;
					this.unit.x = position.x;
					this.unit.y = position.y;
				}
			}
		}
		return base.UnpackState(stream);
	}

	protected float targetX;

	protected float targetY;

	protected PathAgent pathAgent;

	protected bool isAlerted;

	protected bool isSearching;

	protected float alertTime = -4f;

	protected float confusionCounter;

	protected float alertedCounter;

	protected float alertedX;

	protected float alertedY;

	protected bool isAwareOfPlayer;

	protected bool isBlind;

	protected bool isAttracted;

	protected float isAttractedTime;

	public EnemyActionState thinkState;

	public int sightRangeX = 300;

	public int sightRangeY = 20;

	protected float thinkCounter;

	protected int thinkCount;

	[HideInInspector]
	public int walkDirection = 1;

	public float attackTime = 0.06f;

	public float sightDelay = 0.55f;

	public float walkTime = 0.5f;

	protected int maxPatrolBlockRange = 3;

	public Mook mook;

	public Unit unit;

	public ReactionBubble exclamationMark;

	public ReactionBubble questionMark;

	protected float xMin;

	protected float xMax;

	protected float xLength;

	protected RaycastHit rayCastHit;

	protected int panicDirection;

	private int maxOutOfSyncUpdates = 2;

	private int outOfSyncUpdatesCounter;

	private float outOfSyncTimer;

	protected float forgetPlayerCounter;

	protected int seenPlayerNum = -1;

	protected bool moveLeft1Frame;

	protected bool moveRight1Frame;

	protected bool shoot1Frame;

	protected bool special1Frame;

	protected int prevSeenPlayerNum = -1;

	protected float forceHoldLeftTime;

	private bool hasNotifiedStatisticsOfShooting;

	protected float t = 0.01f;

	private bool hasNotifiedStatisticsOfIsAware;

	private bool hasNotifiedStatisticsOfHeardSound;

	protected float panicDirectionCounter;
}
