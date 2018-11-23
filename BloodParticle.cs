// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BloodParticle
{
	public BloodParticle(Vector3 position, Vector3 velocity)
	{
		this.x = position.x;
		this.y = position.y;
		this.z = position.z;
		this.xI = velocity.x;
		this.yI = velocity.y;
		this.zI = velocity.z;
		this.life = 2f;
	}

	public void Run(float t)
	{
		if (this.life > -0.2f)
		{
			this.x += this.xI * t;
			this.y += this.yI * t;
			this.z += this.zI * t;
			this.yI += -700f * t;
			this.life -= t;
		}
	}

	public Vector3 GetPos()
	{
		return new Vector3(this.x, this.y, this.z);
	}

	public Vector3 GetVelocity()
	{
		return new Vector3(this.xI, this.yI, this.zI);
	}

	private float x;

	public float y;

	private float z;

	private float xI;

	private float yI;

	private float zI;

	public float life = 1f;

	public bool alive = true;
}
