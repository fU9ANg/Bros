// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BulletSeeking : DreddBullet
{
	protected override void Start()
	{
		base.Start();
		this.targetX = this.x + this.xI * 12f;
		this.targetY = this.y + this.yI * 12f;
	}

	public override void Fire(float x, float y, float xI, float yI, int playerNum, MonoBehaviour FiredBy)
	{
		base.Fire(x, y, xI, yI, playerNum, FiredBy);
		if (xI > 0f)
		{
			this.angle = 1.57079637f;
		}
		else
		{
			this.angle = -1.57079637f;
		}
		this.originalSpeed = Mathf.Abs(xI);
		this.speed = this.originalSpeed * this.startSpeedM;
		this.targetAngle = this.angle;
	}

	protected override void Update()
	{
		base.Update();
		this.RunSeeking();
		if (this.targetAngle > this.angle + 3.14159274f)
		{
			this.angle += 6.28318548f;
		}
		else if (this.targetAngle < this.angle - 3.14159274f)
		{
			this.angle -= 6.28318548f;
		}
		if (this.reversing)
		{
			if (this.IsHeldByZone())
			{
				this.speed *= 1f - this.t * 8f;
			}
			else
			{
				this.speed = Mathf.Lerp(this.speed, this.originalSpeed, this.t * 8f);
			}
		}
		else
		{
			this.speed = Mathf.Lerp(this.speed, this.originalSpeed, this.t * 10f);
		}
		this.seekSpeedCurrent = Mathf.Lerp(this.seekSpeedCurrent, this.seekTurningSpeedM, this.seekTurningSpeedLerpM * this.t);
		this.angle = Mathf.Lerp(this.angle, this.targetAngle, this.t * this.seekSpeedCurrent);
		Vector2 vector = global::Math.Point2OnCircle(this.angle, this.speed);
		this.xI = vector.x;
		this.yI = vector.y;
		this.SetRotation();
	}

	protected void RunSeeking()
	{
		if (!this.IsHeldByZone())
		{
			this.seekCounter += this.t;
			if (this.seekCounter > 0.1f)
			{
				this.seekCounter -= 0.03f;
				this.CalculateSeek();
			}
		}
	}

	protected void CalculateSeek()
	{
		if (!this.foundMook)
		{
			Unit nearestVisibleUnit = Map.GetNearestVisibleUnit(-1, (int)this.seekRange, this.x, this.y, false);
			if (nearestVisibleUnit != null)
			{
				this.foundMook = true;
				this.targetX = nearestVisibleUnit.x;
				this.targetY = nearestVisibleUnit.y + nearestVisibleUnit.height / 2f;
			}
			else
			{
				this.targetX = this.x + this.xI;
				this.targetY = this.y + this.yI;
			}
		}
		float y = this.targetX - this.x;
		float x = this.targetY - this.y;
		this.targetAngle = global::Math.GetAngle(x, y);
	}

	protected override bool HitWalls()
	{
		bool result = base.HitWalls();
		this.targetAngle = (this.angle = global::Math.GetAngle(this.yI, this.xI));
		return result;
	}

	protected override void ReverseProjectile()
	{
		base.ReverseProjectile();
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Reverse Projectile ! ",
			this.targetX,
			"  ",
			this.targetY
		}));
		if (this.playerNum >= 0)
		{
			float num = this.targetX - this.x;
			float num2 = this.targetY - this.y;
			float num3 = Mathf.Abs(num) + Mathf.Abs(num2);
			num = num / num3 * 300f;
			num2 = num2 / num3 * 300f;
			this.targetX = this.x - num * 100f;
			this.targetY = this.y - num2 * 100f;
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Reverse Projectile towards ",
				this.targetX,
				"  ",
				this.targetY
			}));
		}
	}

	protected float angle;

	protected float avoidWidth;

	protected float avoidHeight = 32f;

	protected Transform avoidTransform;

	protected float targetX;

	protected float targetY;

	protected bool foundMook;

	protected Vector3 mookPos;

	protected float seekCounter;

	protected float targetAngle;

	protected float speed = 100f;

	public float seekTurningSpeedM = 1f;

	protected float seekSpeedCurrent;

	public float seekTurningSpeedLerpM = 4f;

	protected float reverseSpeed;

	protected float originalSpeed;

	public float seekRange = 80f;

	public float seekAheadTime = 0.4f;

	public float startSpeedM = 0.3f;
}
