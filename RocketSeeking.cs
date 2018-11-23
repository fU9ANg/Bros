// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class RocketSeeking : Rocket
{
	protected override void Start()
	{
		base.Start();
		this.originalSpeed = this.speed;
	}

	protected override void Update()
	{
		base.Update();
		this.RunSeeking();
		this.sinCounter += this.t * this.sinCounterSpeed;
		this.sinM += this.t * this.sinWobbleMGrow;
		if (this.sinM > 1f)
		{
			this.sinM = 1f;
		}
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
		this.seekSpeedCurrent = Mathf.Lerp(this.seekSpeedCurrent, this.seekTurningSpeedM, this.seekTurningSpeedLerpM * this.t);
		this.angle = Mathf.Lerp(this.angle, this.targetAngle, this.t * this.seekSpeedCurrent);
		Vector2 vector = global::Math.Point2OnCircle(this.angle + global::Math.Sin(this.sinCounter) * this.sinWobbleRadian * this.sinM, this.speed);
		this.xI = vector.x;
		this.yI = vector.y;
		this.SetRotation();
	}

	protected void RunSeeking()
	{
		if (!this.IsHeldByZone())
		{
			this.seekCounter += this.t;
			if (this.seekCounter > 0.2f)
			{
				this.seekCounter -= 0.25f;
				this.CalculateSeek();
			}
		}
	}

	protected void CalculateSeek()
	{
		if (this.targettingPlayerNum >= 0)
		{
			HeroController.GetPlayerPos(this.targettingPlayerNum, ref this.targetX, ref this.targetY);
		}
		float y = this.targetX - this.x;
		float x = this.targetY - this.y;
		this.targetAngle = global::Math.GetAngle(x, y);
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
		this.speed = Mathf.Abs(xI);
		this.targetAngle = this.angle;
		this.sinCounter = UnityEngine.Random.value * 12f;
	}

	public override void AvoidRect(Transform avoidTransform, float avoidWidth, float avoidHeight)
	{
		this.avoidTransform = avoidTransform;
		this.avoidWidth = avoidWidth;
		this.avoidHeight = avoidHeight;
	}

	public override void Target(float targetX, float targetY, int playerNum)
	{
		this.targetX = targetX;
		this.targetY = targetY;
		this.targettingPlayerNum = playerNum;
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
			this.targettingPlayerNum = -1;
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

	protected float sinCounter;

	protected float targetX;

	protected float targetY;

	protected int targettingPlayerNum = -1;

	protected float seekCounter;

	protected float targetAngle;

	protected float speed = 100f;

	protected float sinM;

	public float seekTurningSpeedM = 1f;

	protected float seekSpeedCurrent;

	public float seekTurningSpeedLerpM = 4f;

	public float sinCounterSpeed = 6f;

	public float sinWobbleRadian = 0.5f;

	public float sinWobbleMGrow = 0.25f;

	protected float reverseSpeed;

	protected float originalSpeed;
}
