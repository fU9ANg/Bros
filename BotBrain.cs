// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BotBrain : MonoBehaviour
{
	public void SetUnit(Unit unit)
	{
		if (unit == null)
		{
			return;
		}
		this.unit = unit;
		if (unit.GetComponent<PathAgent>() == null)
		{
			this.pathAgent = unit.gameObject.AddComponent<PathAgent>();
			this.pathAgent.agentSize = 9f;
			this.pathAgent.capabilities = new PathAgentCapabilities();
			this.pathAgent.capabilities.canUseLadders = true; this.pathAgent.capabilities.canJump = (this.pathAgent.capabilities.canWallClimb = (this.pathAgent.capabilities.canUseLadders ));
			this.pathAgent.capabilities.flying = false;
			this.pathAgent.capabilities.maxDropDistance = 8;
			this.pathAgent.capabilities.jumpHeight = 6;
		}
	}

	public void SetUnitToFollow(Unit unit)
	{
		this.leadingBro = unit;
	}

	public void GetInput(ref bool up, ref bool down, ref bool left, ref bool right, ref bool fire, ref bool buttonJump, ref bool special, ref bool highFive)
	{
		highFive = false; up = (down = (left = (right = (fire = (buttonJump = (special = (highFive )))))));
		if (this.unit == null)
		{
			return;
		}
		if (this.leadingBro == null || this.leadingBro.health <= 0)
		{
			if (this.rescueTarget == null)
			{
				this.rescueTarget = Map.GetNearestRescueBro(this.unit.x, this.unit.y);
			}
			if (this.rescueTarget != null && !this.rescueTarget.freed && Mathf.Abs(this.rescueTarget.y - this.unit.y) < 16f && Mathf.Sign(this.unit.transform.localScale.x) == Mathf.Sign(this.rescueTarget.transform.position.x - this.unit.transform.position.x))
			{
				fire = true;
			}
			if (this.pathAgent.CurrentPath == null && this.rescueTarget != null)
			{
				if (this.unit.collumn == this.rescueTarget.collumn && Mathf.Abs(this.unit.row - this.rescueTarget.row) < 2)
				{
					if (this.unit.row > this.rescueTarget.row)
					{
						left = true;
					}
					else
					{
						right = true;
					}
					if (!this.rescueTarget.freed)
					{
						fire = true;
					}
				}
				else
				{
					this.pathAgent.GetPath(this.rescueTarget.collumn, this.rescueTarget.row, 15f);
				}
			}
			if (this.pathAgent.CurrentPath != null)
			{
				this.pathAgent.GetMove(ref left, ref right, ref up, ref down, ref buttonJump);
			}
		}
		else if (this.aggroMode == BillyMode.Aggro && this.aggroTarget != null)
		{
			buttonJump = false; left = (right = (up = (down = (buttonJump ))));
			highFive = false; fire = (special = (highFive ));
			this.rescueTarget = null;
			if (this.pathAgent.CurrentPath == null)
			{
				this.pathAgent.GetPath(Map.GetCollumn(this.aggroTarget.x), Map.GetRow(this.aggroTarget.y), 10f);
			}
			if (this.pathAgent.CurrentPath != null)
			{
				this.pathAgent.GetMove(ref left, ref right, ref up, ref down, ref buttonJump);
			}
			if (Mathf.Abs(this.aggroTarget.y - this.unit.y) < 16f && Mathf.Sign(this.unit.transform.localScale.x) == Mathf.Sign(this.aggroTarget.transform.position.x - this.unit.transform.position.x))
			{
				Vector3 position = this.unit.transform.position;
				position.y += 4f;
				Vector3 position2 = this.aggroTarget.transform.position;
				position2.y += 4f;
				if (!Physics.Raycast(new Ray(position, position2 - position), Vector3.Distance(position, position2)))
				{
					fire = true;
				}
				if (Vector3.Distance(this.unit.transform.position, this.aggroTarget.transform.position) < 16f)
				{
					highFive = ((double)Time.time % 0.3 < 0.15);
				}
				else
				{
					highFive = false;
				}
			}
			else
			{
				if (Vector3.Distance(this.unit.transform.position, this.aggroTarget.transform.position) < 48f && this.unit.xI < 10f)
				{
					left = (Time.time % 0.4f < 0.2f);
					right = !left;
				}
				if (Map.GetNearestUnit(-1, 32, this.unit.x, this.unit.y, false) != null)
				{
					highFive = ((double)Time.time % 0.3 < 0.15);
				}
			}
		}
		else
		{
			this.rescueTarget = null;
			if (this.pathAgent.CurrentPath == null && this.leadingBro.collumn > 0 && this.leadingBro.row > 0)
			{
				this.pathAgent.GetPath(this.leadingBro.collumn, this.leadingBro.row, 15f);
			}
			if (this.pathAgent.CurrentPath != null)
			{
				this.pathAgent.GetMove(ref left, ref right, ref up, ref down, ref buttonJump);
			}
		}
		Unit unit;
		if (Map.IsUnitNearby(-1, this.unit.x, this.unit.y, 128f, 16f, false, out unit) && unit != null && Mathf.Sign(this.unit.transform.localScale.x) == Mathf.Sign(unit.transform.position.x - this.unit.transform.position.x))
		{
			fire = true;
		}
	}

	private void Update()
	{
		if (this.unit == null)
		{
			return;
		}
		if (this.leadingBro == null)
		{
			return;
		}
		if (this.aggroMode == BillyMode.Aggro)
		{
			this.timeAggroOnThisTarget += Time.deltaTime;
			if (this.timeAggroOnThisTarget > 4f || this.aggroTarget == null || this.aggroTarget.health <= 0 || this.aggroTarget.actionState == ActionState.Dead)
			{
				this.aggroTarget = null;
				this.aggroMode = BillyMode.Conga;
				this.pathAgent.CurrentPath = null;
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
		if (this.aggroMode == BillyMode.Conga && (this.aggroCheckDelay -= Time.deltaTime) < 0f && this.unit.actionState != ActionState.Jumping)
		{
			this.CheckForAggroTarget();
		}
	}

	private void CheckForAggroTarget()
	{
		this.aggroTarget = Map.GetNearestUnit(-1, this.aggroRange, this.leadingBro.x, this.leadingBro.y, false);
		this.aggroCheckDelay = 0.3f;
		if (this.aggroTarget != null)
		{
			if (this.pathAgent != null)
			{
				this.pathAgent.CurrentPath = null;
			}
			this.aggroMode = BillyMode.Aggro;
			this.timeAggroOnThisTarget = 0f;
		}
	}

	private Unit unit;

	private Unit leadingBro;

	private Unit aggroTarget;

	private BillyMode aggroMode;

	private PathAgent pathAgent;

	private float timeAggroOnThisTarget;

	private float aggroCheckDelay;

	private int aggroRange = 200;

	private RescueBro rescueTarget;
}
