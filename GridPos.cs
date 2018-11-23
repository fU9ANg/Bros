// dnSpy decompiler from Assembly-CSharp.dll
using System;

public struct GridPos
{
	public GridPos(int c, int r)
	{
		this.c = c;
		this.r = r;
	}

	public new string ToString()
	{
		return string.Concat(new string[]
		{
			"(",
			this.c.ToString(),
			", ",
			this.r.ToString(),
			")"
		});
	}

	public int c;

	public int r;
}
