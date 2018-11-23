// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DrunkRocket : Rocket
{
	protected override void Start()
	{
		base.Start();
		this.baseLife = this.life;
		this.sinCounter1 = this.random.value * 8f;
		this.sinCounter2 = this.random.value * 8f;
		this.sinCounter2M = (2f + this.random.value * 5f) * 1f;
		global::Math.SetupLookupTables();
		this.speed = Mathf.Abs(this.xI);
		if (this.xI > 0f)
		{
			this.baseAngle = 1.57079637f;
		}
		else
		{
			this.baseAngle = -1.57079637f;
		}
	}

	protected void FixedUpdate()
	{
		if (!this.reversing || !this.IsHeldByZone())
		{
			float num = this.life / this.baseLife;
			float num2 = Mathf.Clamp(0.7f - num * num * 0.6f, 0f, 1f);
			this.sinCounter2 += Time.fixedDeltaTime * this.drunkSpeed;
			this.sinCounter1 += (this.drunkSpeed * 0.5f + this.sinCounter2 * 0.4f + global::Math.Sin(this.sinCounter2 * this.sinCounter2M) * this.drunkSpeed * 2.5f) * Time.fixedDeltaTime;
			Vector2 vector = global::Math.Point2OnCircle(this.baseAngle + global::Math.Sin(this.sinCounter1) * num2 * 0.7f, this.speed);
			this.xI = vector.x;
			this.yI = vector.y;
			this.speed += this.acceleration * Time.fixedDeltaTime;
			this.SetRotation();
		}
	}

	protected override void ReverseProjectile()
	{
		UnityEngine.Debug.Log("Reverse Drunk Rocket ");
		this.baseAngle *= -1f;
		base.ReverseProjectile();
	}

	protected float sinCounter1;

	protected float sinCounter2;

	protected float sinCounter2M = 1f;

	protected float speed = 80f;

	protected float baseAngle;

	public float drunkSpeed = 3f;

	public float acceleration = 100f;

	protected float baseLife = 1.7f;
}
