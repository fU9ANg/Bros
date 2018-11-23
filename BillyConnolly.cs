// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BillyConnolly : BoondockBro
{
	public override BroFollowMode FollowMode
	{
		get
		{
			return this.followMode;
		}
		set
		{
			this.followMode = value;
			if (this.followMode == BroFollowMode.CopyCat)
			{
				base.StopDashing();
			}
			else if (this.followMode == BroFollowMode.CatchUp)
			{
				if (this.aggroTarget == null)
				{
					base.ForceStartDash();
				}
			}
			else if (this.followMode == BroFollowMode.Calibrate)
			{
				this.calibrateTimer = 0.1f;
			}
		}
	}

	protected override void Start()
	{
		base.Start();
		this.projectile.damageInternal = 5;
		this.isLeadBro = false;
	}

	protected override void Update()
	{
		if (this.mode == BillyConnolly.BillyMode.Aggro)
		{
			this.timeAggroOnThisTarget += Time.deltaTime;
			if (!this.enraged)
			{
				this.speed = this.defaultSpeed * 1.3f;
			}
			if (this.timeAggroOnThisTarget > 4f || this.aggroTarget == null || this.aggroTarget.health <= 0 || this.aggroTarget.actionState == ActionState.Dead)
			{
				this.aggroTarget = null;
				this.mode = BillyConnolly.BillyMode.Conga;
				this.FollowMode = BroFollowMode.CatchUp;
				this.lastTargetCol = this.leadingBro.collumn;
				this.lastTargetRow = this.leadingBro.row;
				base.GetComponent<PathAgent>().CurrentPath = null;
				this.ReleaseHighFive();
				if (this.timeAggroOnThisTarget < 4f)
				{
					this.CheckForAggroTarget();
				}
				else
				{
					this.aggroCheckDelay = 1f;
				}
			}
		}
		if (this.mode == BillyConnolly.BillyMode.Conga)
		{
			if (!this.enraged)
			{
				this.speed = this.defaultSpeed;
			}
			if ((this.aggroCheckDelay -= Time.deltaTime) < 0f && this.actionState != ActionState.Jumping)
			{
				this.CheckForAggroTarget();
			}
		}
		base.Update();
	}

	protected override void CheckInput()
	{
		if (this.mode == BillyConnolly.BillyMode.Conga)
		{
			base.CheckInput();
		}
		else if (this.mode == BillyConnolly.BillyMode.Aggro)
		{
			this.FollowMode = BroFollowMode.CatchUp;
			if (this.actionState == ActionState.Dead)
			{
				this.ClearAllInput();
				return;
			}
			this.wasUp = this.up;
			this.wasButtonJump = this.buttonJump;
			this.wasDown = this.down;
			this.wasLeft = this.left;
			this.wasRight = this.right;
			this.wasFire = this.fire;
			this.wasSpecial = this.special;
			this.wasHighFive = this.highFive;
			this.wasButtonTaunt = this.buttonTaunt;
			this.buttonJump = false; this.left = (this.right = (this.up = (this.down = (this.buttonJump ))));
			this.buttonHighFive = false; this.fire = (this.special = (this.buttonHighFive ));
			if (base.GetComponent<PathAgent>().CurrentPath == null)
			{
				base.GetComponent<PathAgent>().GetPath(Map.GetCollumn(this.aggroTarget.x), Map.GetRow(this.aggroTarget.y), 10f);
				LevelEditorGUI.displayPath = base.GetComponent<PathAgent>().CurrentPath;
			}
			if (base.GetComponent<PathAgent>().CurrentPath != null)
			{
				base.GetComponent<PathAgent>().GetMove(ref this.left, ref this.right, ref this.up, ref this.down, ref this.buttonJump);
			}
			if (Mathf.Abs(this.aggroTarget.y - this.y) < 16f && Mathf.Sign(base.transform.localScale.x) == Mathf.Sign(this.aggroTarget.transform.position.x - base.transform.position.x))
			{
				Vector3 position = base.transform.position;
				position.y += 4f;
				Vector3 position2 = this.aggroTarget.transform.position;
				position2.y += 4f;
				if (!Physics.Raycast(new Ray(position, position2 - position), Vector3.Distance(position, position2)))
				{
					this.fire = true;
					this.right = false; this.left = (this.right );
				}
				if (Vector3.Distance(base.transform.position, this.aggroTarget.transform.position) < 16f)
				{
					this.PressHighFiveMelee(false);
				}
				else
				{
					this.ReleaseHighFive();
				}
			}
			else
			{
				if (Vector3.Distance(base.transform.position, this.aggroTarget.transform.position) < 48f && this.xI < 10f)
				{
					this.left = (Time.time % 0.4f < 0.2f);
					this.right = !this.left;
				}
				if (Map.GetNearestUnit(-1, 32, this.x, this.y, false) != null)
				{
					this.PressHighFiveMelee(false);
				}
			}
			base.TrackInputStateChanges();
		}
	}

	protected override void ReduceLives(bool destroyed)
	{
	}

	private void CheckForAggroTarget()
	{
		this.aggroTarget = Map.GetNearestUnit(-1, this.aggroRange, this.leadingBro.x, this.leadingBro.y, false);
		this.aggroCheckDelay = 0.3f;
		if (this.aggroTarget != null)
		{
			base.GetComponent<PathAgent>().CurrentPath = null;
			this.mode = BillyConnolly.BillyMode.Aggro;
			this.timeAggroOnThisTarget = 0f;
		}
	}

	private BillyConnolly.BillyMode mode;

	private Unit aggroTarget;

	private int aggroRange = 140;

	private float aggroCheckDelay = 0.3f;

	private float timeAggroOnThisTarget;

	public enum BillyMode
	{
		Conga,
		Aggro
	}
}
