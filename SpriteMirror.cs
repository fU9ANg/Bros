// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SpriteMirror : SpriteBaseMirror
{
	public override void Mirror(SpriteBase s)
	{
		base.Mirror(s);
		this.lowerLeftPixel = ((SpriteSM)s).lowerLeftPixel;
		this.pixelDimensions = ((SpriteSM)s).pixelDimensions;
	}

	public override bool DidChange(SpriteBase s)
	{
		return base.DidChange(s) || ((SpriteSM)s).lowerLeftPixel != this.lowerLeftPixel || ((SpriteSM)s).pixelDimensions != this.pixelDimensions;
	}

	public Vector2 lowerLeftPixel;

	public Vector2 pixelDimensions;
}
