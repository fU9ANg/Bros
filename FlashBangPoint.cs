// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class FlashBangPoint
{
	public FlashBangPoint(int collumn, int row, float time, int directionCollumn, int directionRow)
	{
		this.collumn = collumn;
		this.row = row;
		this.time = time;
		this.directionCollumn = directionCollumn;
		this.directionRow = directionRow;
	}

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"Collumn ",
			this.collumn,
			", Row ",
			this.row
		});
	}

	public int collumn;

	public int row;

	public float time;

	public int directionCollumn;

	public int directionRow;
}
