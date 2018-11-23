// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[Serializable]
public class UVAnimation_Auto : UVAnimation
{
	public Rect[] BuildUVAnim(SpriteSM s)
	{
		if (this.totalCells < 1)
		{
			return null;
		}
		return base.BuildUVAnim(s.PixelCoordToUVCoord(this.start), s.PixelSpaceToUVSpace(this.pixelsToNextColumnAndRow), this.cols, this.rows, this.totalCells);
	}

	public Vector2 start;

	public Vector2 pixelsToNextColumnAndRow;

	public int cols;

	public int rows;

	public int totalCells;
}
