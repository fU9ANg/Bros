// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BloodyShrapnel : Shrapnel
{
	public override void Launch(float x, float y, float xI, float yI)
	{
		base.Launch(x, y, xI, yI);
		this.lastPosition = base.transform.position;
		this.bloodyM = 1f;
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
		if (this.hasBloodTrail && this.bloodyM > 0f)
		{
			this.bloodyM -= this.t * 1.4f;
			Vector3 vector = new Vector3(this.x, this.y, 0f) - this.lastPosition;
			float magnitude = vector.magnitude;
			int num = (int)(magnitude * 0.2f * this.bloodyM);
			for (int i = 0; i < num; i++)
			{
				float num2 = 1f - (float)i / (float)num;
				Vector3 vector2 = new Vector3(this.x - vector.x * num2 * this.t, this.y - vector.y * num2 * this.t, 0f);
				if (UnityEngine.Random.value > this.bloodyM - 0.5f)
				{
					EffectsController.CreateBloodParticlesDots(this.color, vector2.x, vector2.y, 1f, 1, 0f, 0f, 9f, this.xI * 0.6f, this.yI * 0.6f, 1f);
				}
				else
				{
					EffectsController.CreateBloodParticlesBig(this.color, vector2.x, vector2.y, 1f, 1, 0f, 0f, 12f, this.xI * 0.6f, this.yI * 0.6f);
				}
				this.lastPosition = vector2;
			}
		}
	}

	protected Vector3 lastPosition = Vector3.zero;

	protected float bloodyM = 1f;

	public bool hasBloodTrail = true;

	public BloodColor color;
}
