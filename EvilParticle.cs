// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class EvilParticle : Shrapnel
{
	protected override void Update()
	{
		base.Update();
		if (this.life > 0f)
		{
			this.evilSparkCounter += this.t;
			if (this.evilSparkCounter > 0.0667f)
			{
				this.evilSparkCounter -= 0.0667f;
				EffectsController.CreateShrapnel(this.evilSpark, this.x, this.y, 0.1f, 0.1f, 1f, this.xI / 170f, this.yI / 170f);
			}
		}
	}

	public Shrapnel evilSpark;

	protected float evilSparkCounter;
}
