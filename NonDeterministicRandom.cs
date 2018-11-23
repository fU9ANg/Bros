// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public static class NonDeterministicRandom
{
	private static Randomf RandomVal
	{
		get
		{
			if (NonDeterministicRandom.random == null)
			{
				int seed = new System.Random().Next();
				NonDeterministicRandom.random = new Randomf(seed);
			}
			return NonDeterministicRandom.random;
		}
	}

	public static float value
	{
		get
		{
			return NonDeterministicRandom.RandomVal.value;
		}
	}

	public static Vector3 onUnitSphere
	{
		get
		{
			return NonDeterministicRandom.RandomVal.onUnitSphere;
		}
	}

	public static Vector3 insideUnitSphere
	{
		get
		{
			return NonDeterministicRandom.RandomVal.insideUnitSphere;
		}
	}

	public static Vector2 insideUnitCircle
	{
		get
		{
			return NonDeterministicRandom.RandomVal.insideUnitCircle;
		}
	}

	public static Vector2 onUnitCirle
	{
		get
		{
			return NonDeterministicRandom.RandomVal.onUnitCirle;
		}
	}

	public static float Range(float min, float max)
	{
		return NonDeterministicRandom.RandomVal.Range(min, max);
	}

	public static int Range(int min, int max)
	{
		return NonDeterministicRandom.RandomVal.Range(min, max);
	}

	private static Randomf random;
}
