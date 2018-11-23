// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CutsceneAI : EnemyAI
{
	protected override void Awake()
	{
		base.Awake();
		this.walkDirection = 1;
		this.vanDamme = base.GetComponent<TestVanDammeAnim>();
		this.vanDamme.health = 1000000;
	}

	public void Walk(int direction, float targetX, float startX)
	{
		this.unit.x = startX;
		this.Walk(direction, targetX);
	}

	public void Walk(int direction, float targetX)
	{
		this.targetX = targetX;
		this.walkDirection = direction;
		this.actionState = ActionState.Running;
		this.specialTime = 0f;
		this.forceWalkDirection = direction;
	}

	public void Jump()
	{
		this.jumpTime = 0.2f;
	}

	public virtual void Shoot()
	{
		this.shootTime = (this.shootTimeMin + UnityEngine.Random.value * 0.2f) * this.shootingTimeM;
	}

	public override bool Blind()
	{
		return true;
	}

	public void PanicRun(int direction, float targetX, float startX)
	{
		this.PanicRun(direction, targetX);
		this.unit.x = startX;
	}

	public void PanicRun(int direction, float targetX)
	{
		this.targetX = targetX;
		this.walkDirection = direction;
		this.actionState = ActionState.Panicking;
		((TestVanDammeAnim)this.unit).PlayPanicSound();
		this.unit.yI += 130f;
		this.specialTime = 0f;
		this.forceWalkDirection = direction;
	}

	public void Exclaim(int direction)
	{
		base.transform.localScale = new Vector3((float)direction, 1f, 1f);
		((TestVanDammeAnim)this.unit).PlayGreetingSound();
		if (this.exclamationMark != null)
		{
			this.exclamationMark.RestartBubble();
		}
		this.specialTime = 0f;
	}

	public void Question(int direction)
	{
		base.transform.localScale = new Vector3((float)direction, 1f, 1f);
		((TestVanDammeAnim)this.unit).PlayGreetingSound();
		if (this.questionMark != null)
		{
			this.questionMark.RestartBubble();
		}
		this.specialTime = 0f;
	}

	public void Special()
	{
		this.specialTime = 3f;
	}

	public void Special(float time)
	{
		this.specialTime = time;
	}

	protected override void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.shootTime > 0f)
		{
			this.shootTime -= num;
		}
		if (this.jumpTime > 0f)
		{
			this.jumpTime -= num;
		}
		if (this.specialTime > 0f)
		{
			this.specialTime -= num;
		}
	}

	public override void GetMovement(ref bool left, ref bool right, ref bool up, ref bool down, ref bool jump, ref bool fire, ref bool special1, ref bool special2, ref bool special3, ref bool special4)
	{
		if (this.forceWalkDirection != 0)
		{
			this.walkDirection = this.forceWalkDirection;
		}
		switch (this.actionState)
		{
		case ActionState.Running:
			if (this.walkDirection > 0)
			{
				right = true;
				left = false;
			}
			else if (this.walkDirection < 0)
			{
				left = true;
				right = false;
			}
			break;
		case ActionState.Panicking:
			if (this.walkDirection > 0)
			{
				right = true;
				left = false;
			}
			else if (this.walkDirection < 0)
			{
				left = true;
				right = false;
			}
			this.unit.Terrify();
			break;
		}
		if (this.shootTime > 0f)
		{
			fire = true;
		}
		else
		{
			fire = false;
		}
		if (this.specialTime > 0f)
		{
			special1 = true;
		}
		else
		{
			special1 = false;
		}
		if (this.jumpTime > 0f)
		{
			up = true;
		}
		else
		{
			up = false;
		}
	}

	public float shootingTimeM = 1f;

	public float shootTimeMin = 0.15f;

	protected ActionState actionState;

	protected float jumpTime;

	protected float shootTime;

	protected float specialTime;

	protected int forceWalkDirection;

	protected TestVanDammeAnim vanDamme;
}
