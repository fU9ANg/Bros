// dnSpy decompiler from Assembly-CSharp.dll
using System;

public struct DrawInfo
{
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public static bool operator ==(DrawInfo x, DrawInfo y)
	{
		return x.still == y.still && x.surface == y.surface && x.falling == y.falling && x.offsetFalling == y.offsetFalling && x.stillHeight == y.stillHeight && x.fallingHeight == y.fallingHeight;
	}

	public static bool operator !=(DrawInfo x, DrawInfo y)
	{
		return !(x == y);
	}

	public bool still;

	public bool surface;

	public bool falling;

	public bool offsetFalling;

	public bool HasChanged;

	public float stillHeight;

	public float fallingHeight;
}
