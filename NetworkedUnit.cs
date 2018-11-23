// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class NetworkedUnit : BroforceObject
{
	protected virtual void Awake()
	{
		this.maxHealth = this.health;
	}

	public bool IsEnemy
	{
		get
		{
			bool? flag = this.isEnemy;
			if (flag == null)
			{
				this.isEnemy = new bool?(!this.isHero && !(this is RescueBro));
			}
			bool? flag2 = this.isEnemy;
			return flag2.Value;
		}
	}

	[Syncronize]
	public Vector2 Velocity
	{
		get
		{
			return new Vector2(this.xI, this.yI);
		}
		set
		{
			if (Mathf.Abs(value.x) < 0.001f)
			{
				value.x = 0f;
			}
			if (Mathf.Abs(value.y) < 0.001f)
			{
				value.y = 0f;
			}
			this.xI = value.x;
			this.yI = value.y;
		}
	}

	[Syncronize]
	public virtual Vector2 XY
	{
		get
		{
			return new Vector2(this.x, this.y);
		}
		set
		{
			this.x = value.x;
			this.y = value.y;
		}
	}

	//Interpolate = false; [Syncronize(Interpolate )]
    [Syncronize(Interpolate = false)]
	public byte Direction
	{
		get
		{
			if (base.transform.localScale.x < 0f)
			{
				return 2;
			}
			return 1;
		}
		set
		{
			int num;
			if (value == 2)
			{
				num = -1;
			}
			else
			{
				num = 1;
			}
			base.transform.localScale.Set((float)num, base.transform.localScale.y, base.transform.localScale.z);
		}
	}

	public virtual bool IsLocalMook
	{
		get
		{
			if (this.enemyAI != null)
			{
				return base.IsMine;
			}
			return this.enemyAIOnChildOrParent != null && base.IsMine;
		}
	}

	public bool IsHero
	{
		get
		{
			return this.isHero;
		}
	}

	public override bool ReadyTobeSynced()
	{
		Vector3 vector = this.XY;
		return this.XY.x >= 0f && this.XY.y >= 0f;
	}

	public PolymorphicAI enemyAI;

	public PolymorphicAI enemyAIOnChildOrParent;

	[HideInInspector]
	public int playerNum = -1;

	protected bool isHero;

	private bool? isEnemy;
}
