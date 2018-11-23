// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletGuidedRobrocop : BulletSnakeBroskin
{
	protected override void Start()
	{
		base.Start();
		this.pathAgent = base.GetComponent<PathAgent>();
		base.SetSyncingInternal(true);
	}

	protected override void RunProjectile(float t)
	{
		if (base.IsMine)
		{
			if (this.pathAgent.CurrentPath == null && (this.minimumPathDelay -= t) < 0f)
			{
				Unit unit = null;
				while (this.targetUnits.Count > 0 && unit == null)
				{
					if (this.targetUnits[0] != null && !this.hitUnits.Contains(this.targetUnits[0]) && this.targetUnits[0].health > 0)
					{
						unit = this.targetUnits[0];
					}
					else
					{
						this.targetUnits.RemoveAt(0);
						if (this.targetIcons[0] != null)
						{
							this.targetIcons[0].GoAway();
						}
						this.targetIcons.RemoveAt(0);
					}
				}
				if (unit != null)
				{
					this.minimumPathDelay = 0.1f;
					this.pathAgent.GetPath(Map.GetCollumn(unit.transform.position.x), Map.GetRow(unit.transform.position.y), 10f);
				}
			}
			if (this.pathAgent.CurrentPath != null)
			{
				bool flag5;
				bool flag4;
				bool flag3;
				bool flag2;
				flag5 = false; bool flag = flag2 = (flag3 = (flag4 = (flag5 )));
				this.pathAgent.GetMove(ref flag3, ref flag4, ref flag2, ref flag, ref flag5);
				if (flag2 || flag || flag3 || flag4)
				{
					this.yI = 0f; this.xI = (this.yI );
					if (flag2)
					{
						this.yI = this.projSpeed;
					}
					if (flag)
					{
						this.yI = -this.projSpeed;
					}
					if (flag4)
					{
						this.xI = this.projSpeed;
					}
					if (flag3)
					{
						this.xI = -this.projSpeed;
					}
					this.SetRotation();
				}
			}
		}
		base.RunProjectile(t);
	}

	private new void OnDestroy()
	{
		if (this.targetIcons != null && this.targetIcons.Count > 0 && this.targetIcons[0] != null && !this.targetIcons[0].goingAway)
		{
			this.targetIcons[0].GoAway();
		}
	}

	protected override bool HitWalls()
	{
		if (this.playerNum >= 0 && Physics.Raycast(new Vector3(this.x - Mathf.Sign(this.xI) * this.projectileSize, this.y, 0f), new Vector3(this.xI, this.yI, 0f), out this.raycastHit, this.projectileSize * 2f, this.barrierLayer))
		{
			return this.ReflectProjectile(this.raycastHit);
		}
		if (Physics.Raycast(new Vector3(this.x - Mathf.Sign(this.xI) * this.projectileSize, this.y, 0f), new Vector3(this.xI, this.yI, 0f), out this.raycastHit, this.projectileSize * 2f, this.groundLayer))
		{
			this.MakeEffects(true, this.raycastHit.point.x + this.raycastHit.normal.x * 3f, this.raycastHit.point.y + this.raycastHit.normal.y * 3f, true, this.raycastHit.normal, this.raycastHit.point);
			this.ProjectileApplyDamageToBlock(this.raycastHit.collider.gameObject, this.damageInternal, this.damageType, this.xI, this.yI);
			this.wallHitCount--;
			if (this.wallHitCount <= 0 && this.firedBy)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		return true;
	}

	protected override void HitUnits()
	{
		base.HitUnits();
		if (this.damageInternal < 5)
		{
			this.damageInternal = 5;
		}
	}

	[Syncronize]
	private Vector2 SyncPosition
	{
		get
		{
			return base.transform.position;
		}
		set
		{
			Vector3 position = value;
			position.z = base.transform.position.z;
			base.transform.position = position;
		}
	}

	[Syncronize]
	private Vector2 SyncVelocity
	{
		get
		{
			return new Vector2(this.xI, this.yI);
		}
		set
		{
			this.xI = value.x;
			this.yI = value.y;
		}
	}

	protected override void SetRotation()
	{
		if (this.xI > 0f)
		{
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
			base.transform.eulerAngles = new Vector3(0f, 0f, global::Math.GetAngle(this.yI, -this.xI) * 180f / 3.14159274f + 90f);
		}
		else
		{
			base.transform.localScale = new Vector3(1f, 1f, 1f);
			base.transform.eulerAngles = new Vector3(0f, 0f, global::Math.GetAngle(this.yI, -this.xI) * 180f / 3.14159274f - 90f);
		}
	}

	public List<Unit> targetUnits;

	public List<AnimatedIcon> targetIcons;

	private float minimumPathDelay;

	protected int wallHitCount = 7;

	public float projSpeed = 400f;

	protected PathAgent pathAgent;
}
