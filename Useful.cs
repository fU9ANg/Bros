// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public static class Useful
{
	public static float Sin01(float t)
	{
		float num = Mathf.Sin(t);
		return (num + 1f) / 2f;
	}

	public static float Round(float f)
	{
		return (float)System.Math.Round((double)f, 1);
	}

	public static float Round(double d)
	{
		return (float)System.Math.Round(d, 1);
	}
}
