// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MoveSlowly : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		Vector3 vector = this.velocity + this.acceleration * Time.deltaTime;
		if (this.accelerationDelay <= 0f)
		{
			if (this.DontReverse)
			{
				if (vector.x * this.velocity.x < 0f)
				{
					this.acceleration.x = 0f;
					this.velocity.x = 0f;
				}
				if (vector.y * this.velocity.y < 0f)
				{
					this.acceleration.y = 0f;
					this.velocity.y = 0f;
				}
			}
			this.velocity = vector;
		}
		else
		{
			this.accelerationDelay -= Time.deltaTime;
		}
		base.transform.Translate(this.velocity * Time.deltaTime);
	}

	public Vector3 velocity;

	public Vector3 acceleration;

	public bool DontReverse;

	public float accelerationDelay;
}
