// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DogAI : PolymorphicAI
{
	protected void Start()
	{
		this.ladderLayer = 1 << LayerMask.NameToLayer("Ladders");
		this.greetingRepeatDelay = 0.3f + UnityEngine.Random.value * 0.3f;
	}

	protected override void Update()
	{
		base.Update();
		if (base.IsMine)
		{
			this.RunCheckOwnerShip();
		}
		if (base.IsMine)
		{
		}
		base.DebugShowActionQueue();
	}

	public override void GetInput(ref bool left, ref bool right, ref bool up, ref bool down, ref bool jump, ref bool fire, ref bool special1, ref bool special2, ref bool special3, ref bool special4)
	{
		base.GetInput(ref left, ref right, ref up, ref down, ref jump, ref fire, ref special1, ref special2, ref special3, ref special4);
		fire = false;
		ActionObject actionObject = (this.actionQueue.Count != 0) ? this.actionQueue[0] : null;
		if (actionObject != null && actionObject.type == EnemyActionType.Fire && this.unit.IsOnGround())
		{
			jump = true;
			fire = false;
			this.holdFireTime = 0.14f;
			this.fireDelay = 0.13f;
		}
		if (this.holdFireTime > 0f)
		{
			if (this.fireDelay > 0f)
			{
				this.fireDelay -= Time.deltaTime;
			}
			else
			{
				if (this.fetchObject == null)
				{
					fire = true;
				}
				this.holdFireTime -= Time.deltaTime;
			}
		}
		else
		{
			fire = false;
		}
		if (Mathf.Abs(this.mook.xIBlast) > 20f)
		{
			right = false; left = (right );
		}
	}

	protected override void Think()
	{
		if (this.mentalState != MentalState.Attracted && this.CheckForCorpse())
		{
			this.mentalState = MentalState.Attracted;
		}
		else
		{
			base.Think();
		}
	}

	protected void RunCheckOwnerShip()
	{
		if (this.ownershipCheckDelay <= 0f)
		{
			this.ownershipCheckCounter += Time.deltaTime;
			if (this.ownershipCheckCounter > 0.1f)
			{
				this.ownershipCheckCounter -= 0.1f;
				this.CheckOwnerShip();
			}
		}
		else
		{
			this.ownershipCheckDelay -= Time.deltaTime;
		}
	}

	protected void CheckOwnerShip()
	{
	}

	public override void HearSound(float alertX, float alertY)
	{
		if (this.mook.health > 0 && this.mentalState != MentalState.Attracted && this.mentalState != MentalState.Alerted && this.mentalState != MentalState.Panicking)
		{
			this.RestartExclamationBubble_Networked();
			if (this.seenPlayerNum < 0)
			{
				this.seenPlayerNum = HeroController.GetFirstHeroAlive();
			}
			this.FullyAlert(alertX, alertY, this.seenPlayerNum);
			if (base.IsMine)
			{
			}
		}
	}

	protected override void LookForPlayer()
	{
		base.LookForPlayer();
		if (this.mentalState == MentalState.Alerted)
		{
			HeroController.GetPlayerPos(this.seenPlayerNum, ref this.targetAlertX, ref this.targetAlertY);
		}
	}

	protected override void DoAlertedThink()
	{
		MonoBehaviour.print("do alerted think");
		int num;
		if (this.mook.actionState == ActionState.Jumping)
		{
			num = this.walkDirection;
		}
		else if (this.seenPlayerNum >= 0 && !base.IsBlockedForward())
		{
			num = (int)Mathf.Sign(HeroController.GetPlayerPosition(this.seenPlayerNum).x - this.unit.x);
		}
		else
		{
			num = ((UnityEngine.Random.value <= 0.5f) ? 1 : -1);
		}
		if (UnityEngine.Random.value < 0.3f || this.CheckJump())
		{
			base.AddAction(EnemyActionType.Fire, 0.1f);
		}
		base.AddAction(EnemyActionType.Move, new GridPoint(this.unit.collumn + num * UnityEngine.Random.Range(1, 3), this.unit.row));
	}

	protected bool IsBlockedBackward()
	{
		return Physics.Raycast(new Vector3(this.mook.x + (float)(this.walkDirection * 3), this.mook.y + 6f, 0f), new Vector3((float)(-(float)this.walkDirection), 0f, 0f), out this.rayCastHit, 16f, Map.groundLayer | Map.fragileLayer) && Physics.Raycast(new Vector3(this.mook.x + (float)(this.walkDirection * 3), this.mook.y + 17f, 0f), new Vector3((float)(-(float)this.walkDirection), 0f, 0f), out this.rayCastHit, 16f, Map.groundLayer | Map.fragileLayer);
	}

	protected bool CheckJump()
	{
		if (this.mook.actionState != ActionState.Jumping)
		{
			if (!Physics.Raycast(new Vector3(this.mook.x + (float)(this.walkDirection * 12), this.mook.y + 6f, 0f), Vector3.down, out this.rayCastHit, 12f, Map.groundLayer | this.ladderLayer))
			{
				int num = -1;
				float num2 = 0f;
				float num3 = 0f;
				if (HeroController.IsPlayerNearby(this.mook.x, this.mook.y, 150f, 64f, ref num2, ref num3, ref num))
				{
					return true;
				}
			}
			else if (Physics.Raycast(new Vector3(this.mook.x - (float)(this.walkDirection * 3), this.mook.y + 6f, 0f), new Vector3((float)this.walkDirection, 0f, 0f), out this.rayCastHit, 16f, Map.groundLayer) && !Physics.Raycast(new Vector3(this.mook.x - (float)(this.walkDirection * 3), this.mook.y + 17f, 0f), new Vector3((float)this.walkDirection, 0f, 0f), out this.rayCastHit, 16f, Map.groundLayer))
			{
				return true;
			}
		}
		return false;
	}

	protected override void DoAttractedThink()
	{
		if (this.fetchObject != null)
		{
			this.attractPoint = Map.GetGridPoint(this.fetchObject.transform.position);
			this.walkDirection = (int)Mathf.Sign((float)(this.attractPoint.collumn - this.unit.collumn));
			if (this.attractedToCorpse && this.attractPoint.collumn == this.unit.collumn && this.attractPoint.row == this.unit.row)
			{
				base.AddAction(EnemyActionType.UseSpecial, 3f);
			}
			else
			{
				if (this.walkDirection == 0)
				{
					this.walkDirection = ((UnityEngine.Random.value <= 0.5f) ? -1 : 1);
				}
				if (base.IsBlockedForward())
				{
					base.AddAction(EnemyActionType.Fire);
				}
				if (this.attractedToCorpse)
				{
					base.AddAction(EnemyActionType.Move, this.attractPoint);
				}
				else
				{
					base.AddAction(EnemyActionType.Move, new GridPoint(this.unit.collumn + this.walkDirection * UnityEngine.Random.Range(1, 3), this.unit.row));
				}
			}
		}
		else
		{
			base.AddAction(EnemyActionType.BecomeIdle);
		}
	}

	internal override void FetchObject(Transform fetchObject)
	{
		this.fetchObject = fetchObject;
		this.fetchProximity = 8f + UnityEngine.Random.value * 8f;
		this.attractedToCorpse = false;
		this.mentalState = MentalState.Attracted;
		if (base.IsMine)
		{
		}
	}

	protected bool DogWillCheckForCorpses()
	{
		return this.mook.maxHealth <= 2 && this.mentalState != MentalState.Attracted;
	}

	protected bool CheckForCorpse()
	{
		if (this.DogWillCheckForCorpses())
		{
			Unit unit = Map.CheckForCorpse(this.mook.x, this.mook.y, 128f, 15f, ref this.nearestCorpseX);
			if (unit != null)
			{
				this.fetchObject = unit.transform;
				this.attractedToCorpse = true;
				return true;
			}
		}
		return false;
	}

	protected void StartEatingCorpse()
	{
		this.mook.StartEatingCorpse();
	}

	public override UnityStream PackState(UnityStream stream)
	{
		stream.Serialize<float>(this.nearestCorpseX);
		stream.Serialize<float>(this.eatCorpseTime);
		return base.PackState(stream);
	}

	public override UnityStream UnpackState(UnityStream stream)
	{
		this.nearestCorpseX = (float)stream.DeserializeNext();
		this.eatCorpseTime = (float)stream.DeserializeNext();
		return base.UnpackState(stream);
	}

	protected float leapTimer = 1.2f;

	protected float turnAroundTime;

	public float targetAlertX;

	public float targetAlertY;

	protected float holdFireTime;

	protected LayerMask ladderLayer;

	protected float confirmedFollowTime;

	protected float greetingRepeatDelay = 1.4f;

	protected float fireDelay = 0.1f;

	protected Transform fetchObject;

	protected float fetchObjectImpatienceCounter;

	protected Vector3 lastFetchObjectPosition;

	protected float fetchProximity = 12f;

	protected float nearestCorpseX;

	protected float corpseDiff;

	protected float eatCorpseTime;

	protected float checkCorpseDelay = 5f;

	protected float ownershipCheckCounter;

	protected float ownershipCheckDelay = 1f;

	protected float attackCheckCounter;

	private bool attractedToCorpse;

	protected bool jump1Frame;

	protected float lastAttackDebugTime;
}
