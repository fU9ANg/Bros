// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Coconut : Grenade
{
	protected override void Awake()
	{
		base.Awake();
		this.SyncDestroy = true;
	}

	protected override void Bounce(bool bounceX, bool bounceY)
	{
		if (bounceY && this.yI < -60f)
		{
			EffectsController.CreateLandPoofEffect(this.x, this.y - this.size, UnityEngine.Random.Range(0, 2) * 2 - 1);
		}
		float num = Mathf.Abs(this.xI) + Mathf.Abs(this.yI);
		if (num > 150f || (!bounceX && !bounceY && this.yI < -50f))
		{
			this.bounces++;
			if (this.bounces > 6)
			{
				this.life = -0.1f;
			}
			Map.DisturbWildLife(this.x, this.y, 32f, this.playerNum);
		}
		base.Bounce(bounceX, bounceY);
	}

	public override void Knock(float xDiff, float yDiff, float xI, float yI)
	{
		base.Knock(xDiff, yDiff, xI, yI);
		base.transform.parent = null;
		this.playerNum = -15;
		this.bulletHits++;
		if (this.bulletHits > 2)
		{
			this.bounces = 10;
		}
		if (this.bulletHits > 5)
		{
			this.life = -0.1f;
		}
	}

	public override void Launch(float x, float y, float xI, float yI)
	{
		base.Launch(x, y, xI, yI);
		base.transform.parent = null;
	}

	protected override void RunLife()
	{
		if (this.life <= 0f)
		{
			this.Death();
		}
	}

	protected override void RunMovement()
	{
		base.RunMovement();
	}

	public override void Death()
	{
		this.PlayDeathSound(0.5f);
		EffectsController.CreateDirtParticles(this.x, this.y, 12, 2f, 150f, this.xI * 0.5f, this.yI + 100f);
		EffectsController.CreateSemenParticles(BloodColor.Red, this.x, this.y, 0f, 20, 2f, 2f, 150f, this.xI * 0.5f, 100f);
		this.DestroyGrenade();
	}

	private int bounces;

	private int bulletHits;
}
