// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Plume : Shrapnel
{
	public override void Launch(float x, float y, float xI, float yI)
	{
		base.Launch(x, y, xI, yI);
		this.lastPosition = new Vector3(x, y, 0f);
	}

	protected override void Update()
	{
		base.Update();
		if (this.life > 0f)
		{
			Vector3 a = new Vector3(this.x, this.y, 0f);
			Vector3 a2 = a - this.lastPosition;
			float magnitude = a2.magnitude;
			Vector3 b = a2 / magnitude;
			int num = 1;
			while ((float)num < magnitude)
			{
				this.lastPosition += b;
				EffectsController.CreatePlumeParticle(this.lastPosition.x, this.lastPosition.y, 4f, 0f, 0f, 0.3f + this.life / this.startLife, 0.6f + this.life / this.startLife);
				num++;
			}
		}
	}

	protected Vector3 lastPosition;
}
