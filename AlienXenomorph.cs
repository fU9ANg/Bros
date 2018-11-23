// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class AlienXenomorph : MookDog
{
	protected override void Start()
	{
		base.Start();
		this.halfWidth = 12f;
	}

	protected override void AnimateActualNewRunningFrames()
	{
		if (!this.ducking)
		{
			int num = 0 + this.frame % 12;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 2));
		}
		else
		{
			int num2 = 12 + this.frame % 12;
			this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)(this.spritePixelHeight * 2));
		}
	}

	protected override void AnimateActualUnawareRunningFrames()
	{
		this.AnimateActualNewRunningFrames();
	}

	protected override void AnimateActualJumpingDuckingFrames()
	{
		this.AnimateActualJumpingFrames();
	}

	protected override void AnimateDeath()
	{
		if (this.showElectrifiedFrames && this.plasmaDamage > 0)
		{
			this.plasmaFrame++;
			this.DeactivateGun();
			this.frameRate = 0.033f;
			if (this.y > this.groundHeight + 0.2f && this.impaledOnSpikes == null)
			{
				this.sprite.SetLowerLeftPixel((float)((15 + this.plasmaFrame % 2 * 2) * this.spritePixelWidth), (float)(this.spritePixelHeight * 1));
			}
			else
			{
				this.sprite.SetLowerLeftPixel((float)((16 + this.plasmaFrame % 2 * 2) * this.spritePixelWidth), (float)(this.spritePixelHeight * 1));
			}
		}
		else
		{
			base.AnimateDeath();
		}
	}
}
