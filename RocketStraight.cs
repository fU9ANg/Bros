// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class RocketStraight : Rocket
{
	protected override void Update()
	{
		base.Update();
		if (this.accelerationDelay > 0f)
		{
			this.xI *= 1f - this.t;
			this.yI *= 1f - this.t;
			this.yI -= 500f * this.t;
			this.accelerationDelay -= this.t;
			if (this.accelerationDelay <= 0f)
			{
				this.lastTrailX = this.x;
				this.lastTrailY = this.y;
				Vector3 vector = base.transform.right * 160f * Mathf.Sign(this.xI);
				this.xI *= 0.8f;
				this.yI *= 0.8f;
				this.xI += vector.x * this.t;
				this.yI += vector.y * this.t;
			}
		}
		else
		{
			this.gravityM = Mathf.Clamp(this.gravityM - this.t * 6f, 0f, 1f);
			this.yI -= 500f * this.t * this.gravityM;
			this.xI *= 1f - this.t * 9f;
			this.yI *= 1f - this.t * 9f;
			this.accelerationIActual = Mathf.Clamp(this.accelerationIActual + this.accelerationI_I * this.t, 0f, this.accelerationI);
			Vector3 vector2 = base.transform.right * this.accelerationIActual * Mathf.Sign(this.xI);
			this.xI += vector2.x * this.t;
			this.yI += vector2.y * this.t;
			float num = Mathf.Sqrt(this.yI * this.yI + this.xI * this.xI);
			if (num > this.maxSpeed)
			{
				this.xI = this.xI / num * this.maxSpeed;
				this.yI = this.yI / num * this.maxSpeed;
			}
		}
	}

	protected override void RunSmokeTrail(float t)
	{
		if (this.accelerationDelay <= 0f)
		{
			base.RunSmokeTrail(t);
		}
	}

	public float accelerationI = 800f;

	public float accelerationI_I = 1400f;

	protected float accelerationIActual = 200f;

	public float accelerationDelay = 1.2f;

	public float maxSpeed = 400f;

	protected float gravityM = 1f;
}
