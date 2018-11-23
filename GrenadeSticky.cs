// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class GrenadeSticky : Grenade
{
	public override void Death()
	{
		base.Death();
	}

	protected override void Bounce(bool bounceX, bool bounceY)
	{
		base.Bounce(bounceX, bounceY);
		this.xI = 0f;
		this.yI = 0f;
		if (!this.stuck)
		{
			this.stuck = true;
			MonoBehaviour.print("STUCK!");
			if (this.life > 1f + (float)this.stickGrenadeSwarmIndex * 0.25f)
			{
				this.life = 1f + (float)this.stickGrenadeSwarmIndex * 0.25f;
			}
		}
	}

	protected override void RunMovement()
	{
		if (!this.stuck && this.stuckToUnit == null)
		{
			base.RunMovement();
		}
		else
		{
			this.CheckStuck();
			this.xI = 0f;
			this.yI = 0f;
			this.rI = 0f;
		}
	}

	protected override void RunTrail()
	{
		if (!this.stopRunningTrailAfterStuck)
		{
			base.RunTrail();
		}
	}

	protected void CheckStuck()
	{
		this.stuck = false;
		if (Map.ConstrainToBlocks(this.x - this.size, this.y - this.size, this.size))
		{
			this.stuck = true;
			return;
		}
		if (Map.ConstrainToBlocks(this.x + this.size, this.y + this.size, this.size))
		{
			this.stuck = true;
			return;
		}
		if (Map.ConstrainToBlocks(this.x + this.size, this.y - this.size, this.size))
		{
			this.stuck = true;
			return;
		}
		if (Map.ConstrainToBlocks(this.x - this.size, this.y + this.size, this.size))
		{
			this.stuck = true;
			return;
		}
		if (Map.ConstrainToBlocks(this.x - this.size, this.y, this.size))
		{
			this.stuck = true;
			return;
		}
		if (Map.ConstrainToBlocks(this.x + this.size, this.y, this.size))
		{
			this.stuck = true;
			return;
		}
		if (Map.ConstrainToBlocks(this.x, this.y - this.size, this.size))
		{
			this.stuck = true;
			return;
		}
		if (Map.ConstrainToBlocks(this.x, this.y + this.size, this.size))
		{
			this.stuck = true;
			return;
		}
	}

	protected override void Update()
	{
		base.Update();
		this.TryStickToUnit();
		if (this.stuckToUnit != null)
		{
			Vector3 vector = this.stuckToUnit.transform.TransformPoint(this.stuckTolocalPos);
			this.x = vector.x;
			this.y = vector.y;
			this.xI = this.stuckToUnit.xI;
			this.yI = this.stuckToUnit.yI;
			if (this.stuckToUnit.health <= 0 && Mathf.Abs(this.stuckToUnit.xI) + Mathf.Abs(this.stuckToUnit.yI) < 100f)
			{
				if (base.IsMine)
				{
					Networking.RPC(PID.TargetAll, new RpcSignature(this.DetachFromUnit), false);
				}
				else
				{
					this.DetachFromUnit();
				}
			}
		}
	}

	protected virtual void TryStickToUnit()
	{
		if (this.stickyToUnits && this.stuckToUnit == null && base.IsMine)
		{
			this.stuckToUnit = Map.GeLivingtUnit(this.playerNum, 0f, 0f, this.x, this.y);
			if (this.stuckToUnit != null)
			{
				this.PlayStuckSound(0.7f);
				Vector3 arg = this.stuckToUnit.transform.InverseTransformPoint(base.transform.position);
				Networking.RPC<Unit, Vector3>(PID.TargetAll, new RpcSignature<Unit, Vector3>(this.StickToUnit), this.stuckToUnit, arg, false);
				this.stopRunningTrailAfterStuck = true;
				if (this.life > 1f + (float)this.stickGrenadeSwarmIndex * 0.25f)
				{
					this.life = 1f + (float)this.stickGrenadeSwarmIndex * 0.25f;
				}
			}
		}
	}

	public void StickToUnit(Unit unit, Vector3 stucklocalPos)
	{
		this.stuckToUnit = unit;
		this.stuckTolocalPos = stucklocalPos;
		if (unit.enemyAI != null)
		{
			if (unit.enemyAI.IsAlerted())
			{
				this.stuckToUnit.Panic((int)Mathf.Sign(this.xI), this.life + 0.2f, false);
			}
			else
			{
				this.stuckToUnit.enemyAI.ShowQuestionBubble();
			}
		}
	}

	protected virtual void PlayStuckSound(float v)
	{
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.specialAttackSounds, v * 0.6f, base.transform.position);
	}

	public void DetachFromUnit()
	{
		this.stuckToUnit = null;
		this.stickyToUnits = false;
	}

	protected bool stuck;

	public Unit stuckToUnit;

	protected Vector3 stuckTolocalPos;

	public bool stickyToUnits = true;

	protected bool stopRunningTrailAfterStuck;

	public int stickGrenadeSwarmIndex;
}
