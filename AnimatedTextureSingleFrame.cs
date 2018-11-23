// dnSpy decompiler from Assembly-CSharp.dll
using System;

[Serializable]
public class AnimatedTextureSingleFrame
{
	public AnimatedTextureSingleFrame()
	{
		this.row = 0; this.collumn = (this.row );
		this.frameRate = 0.0334f;
	}

	public AnimatedTextureSingleFrame(int c, int r)
	{
		this.collumn = c;
		this.row = r;
	}

	public int collumn;

	public int row;

	public float frameRate = 0.0334f;
}
