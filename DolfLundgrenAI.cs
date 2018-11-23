// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class DolfLundgrenAI : PolymorphicAI
{
	protected void Start()
	{
		this.centerX = this.mook.x;
	}

	public void AddDamagePressure(int damage)
	{
		this.damagePressure += damage;
	}

	protected override void LookForPlayer()
	{
		if (SortOfFollow.IsItSortOfVisible(this.mook.x, this.mook.y, -64f, -64f))
		{
			this.seenPlayerNum = HeroController.GetNearestPlayer(this.mook.x, this.mook.y, 4000f, 4000f);
			if (this.seenPlayerNum >= 0)
			{
				this.FullyAlert(this.mook.x, this.mook.y, this.seenPlayerNum);
			}
		}
	}

	protected override void SetDeltaTime()
	{
		if (PlayerOptions.Instance.hardMode)
		{
			this.t = Time.deltaTime * 2f;
		}
		else
		{
			base.SetDeltaTime();
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.mentalState == MentalState.Alerted && !CutsceneController.isInCutscene)
		{
			HeroController.TryFollow(base.transform);
			HeroController.GetGetFollowPosition(ref this.heroesFollowPos);
		}
		else
		{
			HeroController.StopFollowing(base.transform);
			this.heroesFollowPos = base.transform.position;
		}
		this.RunBombardment();
	}

	protected override void RunQueue()
	{
		if (this.unit.health > 0 && this.actionQueue.Count > 0)
		{
			ActionObject actionObject = this.actionQueue[0];
			EnemyActionType type = actionObject.type;
			if (type != EnemyActionType.UseSpecial2)
			{
				base.RunQueue();
			}
			else
			{
				actionObject.duration -= this.t;
				if (actionObject.duration <= 0f && this.unit.IsOnGround())
				{
					this.actionQueue.Remove(actionObject);
				}
			}
		}
	}

	public override void Attract(float xTarget, float yTarget)
	{
	}

	public override void ForgetPlayer()
	{
	}

	public override void Blind()
	{
		this.actionQueue.Clear();
		base.AddAction(EnemyActionType.Wait, UnityEngine.Random.Range(1f, 2f));
	}

	public void Revived()
	{
		this.reviveCount++;
		this.actionQueue.Clear();
		base.AddAction(EnemyActionType.Wait, 1f);
		float num = Mathf.Floor(this.mook.x / 16f) * 16f + 92f;
		if (num > this.centerX)
		{
			this.centerX = num;
		}
		else
		{
			this.centerX += 92f;
		}
		if (this.reviveCount < 3 || this.reviveCount % 2 == 1)
		{
			this.wantToJumpToSafety = true;
		}
		if (this.wantToJumpToSafety)
		{
			int num2 = (int)(this.centerX - this.unit.x) / 32;
			if (num2 < 0)
			{
			}
			base.AddAction(EnemyActionType.UseSpecial2, new GridPoint(Map.GetCollumn(this.centerX), this.unit.row + 32), 4f);
		}
		else
		{
			base.AddAction(EnemyActionType.UseSpecial3, 2f);
		}
	}

	public void StartBombardment()
	{
		this.bombardmentCounter = 0f;
		this.bombardmentPositions = new List<Vector3>();
		if (this.seenPlayerNum >= 0)
		{
			Vector3 playerPos = HeroController.GetPlayerPos(this.seenPlayerNum);
			if (Mathf.Abs(playerPos.x - this.mook.x) > 160f)
			{
				if (!HeroController.GetNearestPlayer(this.mook.x, this.mook.y, 300f, 300f, ref this.seenPlayerNum))
				{
					return;
				}
				playerPos = HeroController.GetPlayerPos(this.seenPlayerNum);
			}
			for (int i = -5; i <= 5; i++)
			{
				this.bombardmentPositions.Add(playerPos + Vector3.right * (float)i * 48f);
			}
		}
	}

	protected void RunBombardment()
	{
		if (this.bombardmentPositions.Count > 0)
		{
			this.bombardmentCounter -= Time.deltaTime;
			if (this.bombardmentCounter <= 0f)
			{
				this.bombardmentCounter += 1f;
				Vector3 vector = this.bombardmentPositions[0];
				this.bombardmentPositions.RemoveAt(0);
				ProjectileController.SpawnProjectileLocally(ProjectileController.instance.shellBombardment, SingletonMono<MapController>.Instance, vector.x + 300f, vector.y + 450f, -187.5f, -281.25f, -1);
			}
		}
	}

	protected bool IsPlayerTooClose()
	{
		return HeroController.IsPlayerNearby(this.mook.x, this.mook.y, 36f, 36f);
	}

	protected override void DoIdleThink()
	{
		base.AddAction(EnemyActionType.Wait, 0.3f);
		base.AddAction(EnemyActionType.LookForPlayer);
	}

	protected override void DoAlertedThink()
	{
		if (!SortOfFollow.IsItSortOfVisible(this.mook.x, this.mook.y + 10f, 36f, 36f))
		{
			base.AddAction(EnemyActionType.Wait, 0.3f);
			return;
		}
		if (UnityEngine.Random.value < 0.55f && this.IsPlayerTooClose() && !Map.IsInvulnerableAbove(this.mook.x - (float)(this.walkDirection * 3), this.mook.y) && !Map.IsInvulnerableAbove(this.mook.x + (float)(this.walkDirection * 16), this.mook.y))
		{
			this.superJumping = true;
			this.jumpChargeCounter = 0.3f;
			GridPoint gridPoint = new GridPoint();
			if (UnityEngine.Random.value < 0.8f)
			{
				gridPoint.collumn = this.unit.collumn;
			}
			else if (this.CanGoTo(this.mook.x - 32f) && UnityEngine.Random.value < 0.8f)
			{
				gridPoint.collumn = this.unit.collumn - 1;
			}
			else
			{
				gridPoint.collumn = this.unit.collumn + 1;
			}
			gridPoint.row = this.unit.row + 6;
			base.AddAction(EnemyActionType.UseSpecial2, gridPoint, 2.5f);
		}
		else if (!this.IsPlayerThisWay((int)base.transform.localScale.x))
		{
			base.AddAction(EnemyActionType.FacePoint, new GridPoint(this.unit.collumn - (int)base.transform.localScale.x * 5, this.unit.row));
			base.AddAction(EnemyActionType.Wait, UnityEngine.Random.Range(0.75f, 1.5f));
		}
		else if (UnityEngine.Random.value < 0.5f && !Physics.Raycast(new Vector3(this.mook.x - (float)(this.walkDirection * 3), this.mook.y + 6f, 0f), new Vector3((float)this.walkDirection, 0f, 0f), out this.rayCastHit, 50f, Map.groundLayer) && !Physics.Raycast(new Vector3(this.mook.x - (float)(this.walkDirection * 3), this.mook.y + 24f, 0f), new Vector3((float)this.walkDirection, 0f, 0f), out this.rayCastHit, 50f, Map.groundLayer))
		{
			base.AddAction(EnemyActionType.UseSpecial, this.specialActionTime);
			base.AddAction(EnemyActionType.Wait, UnityEngine.Random.Range(0.75f, 1.5f));
		}
		else if (UnityEngine.Random.value < 0.6f)
		{
			base.AddAction(EnemyActionType.Fire, this.attackTime);
			base.AddAction(EnemyActionType.Wait, UnityEngine.Random.Range(0.75f, 1.5f));
		}
		else
		{
			this.possibleMoveBackCount++;
			if (this.possibleMoveBackCount % 2 == 1 && HeroController.IsPlayerNearby(this.mook.x, this.mook.y, 150f, 150f))
			{
				this.MoveCenterXBack();
			}
			else if (this.damagePressure > 32)
			{
				this.MoveCenterXBack();
				this.damagePressure = 0;
			}
			this.moveCount++;
			switch (this.moveCount % 4)
			{
			case 0:
				this.targetX = this.centerX - 64f;
				break;
			case 1:
				if (this.CanGoTo(this.centerX + 64f))
				{
					this.targetX = this.centerX + 64f;
				}
				else
				{
					this.targetX = this.centerX - 16f;
				}
				break;
			case 2:
				this.targetX = this.centerX - 32f;
				break;
			case 3:
				if (this.CanGoTo(this.centerX + 32f))
				{
					this.targetX = this.centerX + 32f;
				}
				else
				{
					this.targetX = this.centerX - 16f;
				}
				break;
			}
			base.AddAction(EnemyActionType.Move, new GridPoint(Map.GetCollumn(this.targetX), this.unit.row), 3f);
			this.targetDirection = (int)Mathf.Sign(this.targetX - this.mook.x);
		}
	}

	protected bool CanGoTo(float x)
	{
		return Physics.Raycast(new Vector3(this.centerX + 64f, this.mook.y + 6f, 0f), Vector3.down, 1400f, Map.groundLayer);
	}

	protected void MoveCenterXBack()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(new Vector3(this.centerX + 176f, 1200f, 0f), Vector3.down, out raycastHit, 1400f, Map.groundLayer) || Physics.Raycast(new Vector3(this.centerX + 192f, 1200f, 0f), Vector3.down, out raycastHit, 1400f, Map.groundLayer))
		{
			this.centerX += 96f;
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

	public override void GetInput(ref bool left, ref bool right, ref bool up, ref bool down, ref bool jump, ref bool fire, ref bool special1, ref bool special2, ref bool special3, ref bool special4)
	{
		if (this.mentalState == MentalState.Alerted && base.CurrentAction != null)
		{
			up = false;
			fire = false;
			special1 = false;
			left = false;
			right = false;
			jump = false;
			special2 = false;
			if (!Map.isEditing)
			{
				if (this.crouchTime > 0f)
				{
					this.crouchTime -= Time.deltaTime;
					special2 = true;
					return;
				}
				if (base.CurrentAction.type != EnemyActionType.UseSpecial2)
				{
					this.jumpChargeCounter = 0.4f;
				}
				if (base.CurrentAction.type == EnemyActionType.UseSpecial2)
				{
					if (this.jumpChargeCounter > 0f)
					{
						special2 = true;
						this.jumpChargeCounter -= Time.deltaTime;
						if (this.jumpChargeCounter <= 0f)
						{
							this.superJumping = true;
							if (Physics.Raycast(new Vector3(this.mook.x + (float)(this.walkDirection * 24), this.mook.y + 1000f, 0f), Vector3.down, out this.rayCastHit, 1000f, Map.groundLayer))
							{
								float num = this.rayCastHit.point.y - this.mook.y;
								this.superJumpCounter = 0.12f + num / 880f;
							}
							else if (Physics.Raycast(new Vector3(this.mook.x + (float)(this.walkDirection * 40), this.mook.y + 1000f, 0f), Vector3.down, out this.rayCastHit, 1000f, Map.groundLayer))
							{
								float num2 = this.rayCastHit.point.y - this.mook.y;
								this.superJumpCounter = 0.12f + num2 / 880f;
							}
							else
							{
								this.superJumpCounter = 0.01f;
							}
						}
						this.hasReachApex = false;
					}
					else
					{
						this.superJumpCounter -= Time.deltaTime;
						if (this.unit.collumn < base.CurrentAction.gridPoint.collumn && !this.hasReachApex)
						{
							jump = true;
							this.mook.yI = 440f;
							if (this.unit.collumn < base.CurrentAction.gridPoint.collumn)
							{
								right = true;
							}
							else if (this.unit.collumn > base.CurrentAction.gridPoint.collumn)
							{
								this.unit.xI = 0f;
							}
						}
						else
						{
							this.unit.xI = 0f;
							this.superJumping = false;
							this.hasReachApex = true;
							this.wantToJumpToSafety = false;
						}
					}
				}
				else if (this.jumpHeldTime > 0f)
				{
					this.jumpHeldTime -= Time.deltaTime;
					if (this.jumpHeldTime > 0f)
					{
						jump = true;
					}
				}
				else if (base.CurrentAction.type == EnemyActionType.Move)
				{
					if (base.CurrentAction.gridPoint.collumn < this.unit.collumn)
					{
						left = true;
					}
					else if (base.CurrentAction.gridPoint.collumn > this.unit.collumn)
					{
						right = true;
					}
					else if (this.walkDirection < 0)
					{
						left = true;
					}
					else
					{
						right = true;
					}
					if (Physics.Raycast(new Vector3(this.mook.x - (float)(this.walkDirection * 3), this.mook.y + 6f, 0f), new Vector3((float)this.walkDirection, 0f, 0f), out this.rayCastHit, 16f, Map.groundLayer) || Physics.Raycast(new Vector3(this.mook.x - (float)(this.walkDirection * 3), this.mook.y + 18f, 0f), new Vector3((float)this.walkDirection, 0f, 0f), out this.rayCastHit, 16f, Map.groundLayer))
					{
						if (!Physics.Raycast(new Vector3(this.mook.x - (float)(this.walkDirection * 3), this.mook.y + 24f, 0f), new Vector3((float)this.walkDirection, 0f, 0f), out this.rayCastHit, 24f, Map.groundLayer) && !Physics.Raycast(new Vector3(this.mook.x - (float)(this.walkDirection * 3), this.mook.y + 36f, 0f), new Vector3((float)this.walkDirection, 0f, 0f), out this.rayCastHit, 24f, Map.groundLayer) && !Physics.Raycast(new Vector3(this.mook.x - (float)(this.walkDirection * 3), this.mook.y + 54f, 0f), new Vector3((float)this.walkDirection, 0f, 0f), out this.rayCastHit, 24f, Map.groundLayer))
						{
							jump = true;
						}
						else if (!Map.IsInvulnerableAbove(this.mook.x - (float)(this.walkDirection * 3), this.mook.y) && !Map.IsInvulnerableAbove(this.mook.x + (float)(this.walkDirection * 16), this.mook.y))
						{
							if (this.mook.GetGroundHeightGround() > this.mook.y - 2f)
							{
								this.jumpChargeCounter = 0.4f;
								this.superJumping = true;
							}
						}
						else
						{
							this.targetX = this.mook.x;
							UnityEngine.Debug.Log("Invulnerable above");
						}
					}
					else if (Physics.Raycast(new Vector3(this.mook.x, this.mook.y + 6f, 0f), Vector3.down, out this.rayCastHit, 16f, Map.groundLayer) && !Physics.Raycast(new Vector3(this.mook.x + (float)(this.walkDirection * 8), this.mook.y + 6f, 0f), Vector3.down, out this.rayCastHit, 16f, Map.groundLayer))
					{
						jump = true;
						this.jumpHeldTime = 0.06f;
					}
				}
				else if (base.CurrentAction.type == EnemyActionType.UseSpecial)
				{
					right = false; left = (right );
					special1 = true;
				}
				else if (base.CurrentAction.type == EnemyActionType.Fire)
				{
					fire = true;
				}
				else
				{
					base.GetInput(ref left, ref right, ref up, ref down, ref jump, ref fire, ref special1, ref special2, ref special3, ref special4);
				}
				if (jump || this.superJumping)
				{
					if (Physics.Raycast(new Vector3(this.mook.x, this.mook.y + 6f, 0f), Vector3.up, out this.rayCastHit, 24f, Map.groundLayer))
					{
						this.rayCastHit.collider.SendMessage("Damage", new DamageObject(10, DamageType.Crush, 0f, 100f, null), SendMessageOptions.DontRequireReceiver);
					}
					if (Physics.Raycast(new Vector3(this.mook.x - (float)(this.walkDirection * 16), this.mook.y + 6f, 0f), Vector3.up, out this.rayCastHit, 24f, Map.groundLayer))
					{
						this.rayCastHit.collider.SendMessage("Damage", new DamageObject(10, DamageType.Crush, 0f, 100f, null), SendMessageOptions.DontRequireReceiver);
					}
					if (Physics.Raycast(new Vector3(this.mook.x + (float)(this.walkDirection * 6), this.mook.y + 6f, 0f), Vector3.up, out this.rayCastHit, 24f, Map.groundLayer))
					{
						this.rayCastHit.collider.SendMessage("Damage", new DamageObject(10, DamageType.Crush, 0f, 100f, null), SendMessageOptions.DontRequireReceiver);
					}
				}
			}
		}
	}

	public void Crouch()
	{
		this.crouchTime = 0.33f;
	}

	public override bool Panic(int direction, bool forgetPlayer)
	{
		return false;
	}

	public override void FullyAlert(float x, float y, int playerNum)
	{
		if (this.mentalState != MentalState.Panicking && this.mentalState != MentalState.Alerted)
		{
			this.mentalState = MentalState.Alerted;
			this.seenPlayerNum = playerNum;
			this.actionQueue.Clear();
			base.AddAction(EnemyActionType.GreetPlayer);
			base.AddAction(EnemyActionType.Wait, 2f);
			DisableWhenOffCamera component = base.gameObject.GetComponent<DisableWhenOffCamera>();
			if (component != null)
			{
				component.enabled = false;
				UnityEngine.Debug.LogWarning("This is a hack!");
			}
		}
	}

	public override void HearSound(float alertX, float alertY)
	{
	}

	public float specialActionTime = 1f;

	protected float centerX;

	protected int targetDirection;

	protected float jumpHeldTime;

	protected int moveCount;

	protected bool superJumping;

	protected float superJumpCounter;

	protected int damagePressure;

	protected Vector3 heroesFollowPos = Vector3.zero;

	protected float jumpChargeCounter;

	protected bool callInAirstrike;

	private float targetX;

	protected int reviveCount;

	private bool wantToJumpToSafety;

	protected List<Vector3> bombardmentPositions = new List<Vector3>();

	protected float bombardmentCounter;

	protected int possibleMoveBackCount;

	private bool hasReachApex;

	protected float crouchTime;
}
