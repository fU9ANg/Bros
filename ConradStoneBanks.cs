// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ConradStoneBanks : TestVanDammeAnim
{
	protected override void AnimateSpecial()
	{
		UnityEngine.Debug.Log("USe Special");
		this.SetSpriteOffset(0f, 0f);
		this.DeactivateGun();
		this.frameRate = 0.0334f;
		int num = 4 + Mathf.Clamp(this.frame % 7, 0, 7);
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 7));
		if (this.frame % 7 == 5)
		{
			this.frameRate = 0.2f;
		}
	}
}
