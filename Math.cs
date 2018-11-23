// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Math
{
	public static void SetupLookupTables()
	{
		if (!global::Math.setup)
		{
			global::Math.PI2 = 6.28318548f;
			global::Math.invPI2 = 1f / global::Math.PI2;
			global::Math.sinLookupTable = new float[global::Math.lookupResolution];
			for (int i = 0; i < global::Math.lookupResolution; i++)
			{
				global::Math.sinLookupTable[i] = Mathf.Sin(6.28318548f / (float)global::Math.lookupResolution * (float)i);
			}
			global::Math.cosLookupTable = new float[global::Math.lookupResolution];
			for (int j = 0; j < global::Math.lookupResolution; j++)
			{
				global::Math.cosLookupTable[j] = Mathf.Cos(6.28318548f / (float)global::Math.lookupResolution * (float)j);
			}
			global::Math.vector3LookupTable = new Vector3[global::Math.lookupResolution];
			for (int k = 0; k < global::Math.lookupResolution; k++)
			{
				global::Math.vector3LookupTable[k] = new Vector3(global::Math.Cos(6.28318548f / (float)global::Math.lookupResolution * (float)k), global::Math.Sin(6.28318548f / (float)global::Math.lookupResolution * (float)k), 0f);
			}
			global::Math.vector2LookupTable = new Vector2[global::Math.lookupResolution];
			for (int l = 0; l < global::Math.lookupResolution; l++)
			{
				global::Math.vector2LookupTable[l] = new Vector2(global::Math.Sin(6.28318548f / (float)global::Math.lookupResolution * (float)l), global::Math.Cos(6.28318548f / (float)global::Math.lookupResolution * (float)l));
			}
			global::Math.angleLookupTable = new float[global::Math.lookupResolution, global::Math.lookupResolution];
			for (int m = 0; m < global::Math.lookupResolution; m++)
			{
				for (int n = 0; n < global::Math.lookupResolution; n++)
				{
					global::Math.angleLookupTable[m, n] = Mathf.Atan2((float)(n - global::Math.lookupResolution / 2), (float)(m - global::Math.lookupResolution / 2));
				}
			}
			global::Math.pointOnSphereTable = new Vector3[global::Math.lookupResolution];
			for (int num = 0; num < global::Math.lookupResolution; num++)
			{
				global::Math.pointOnSphereTable[num] = NonDeterministicRandom.onUnitSphere;
			}
			global::Math.pointInsideSphereTable = new Vector3[global::Math.lookupResolution];
			for (int num2 = 0; num2 < global::Math.lookupResolution; num2++)
			{
				global::Math.pointInsideSphereTable[num2] = NonDeterministicRandom.insideUnitSphere;
			}
			global::Math.setup = true;
		}
	}

	public static float Sin(float a)
	{
		return global::Math.sinLookupTable[(int)(Mathf.Repeat(a, global::Math.PI2) * global::Math.invPI2 * (float)global::Math.lookupResolution)];
	}

	public static float Cos(float a)
	{
		return global::Math.cosLookupTable[(int)(Mathf.Repeat(a, global::Math.PI2) * global::Math.invPI2 * (float)global::Math.lookupResolution)];
	}

	public static Vector3 RandomPointOnCircle()
	{
		return global::Math.Point3OnCircle(UnityEngine.Random.value * global::Math.PI2, 1f);
	}

	public static Vector3 Point3OnCircle(float angle, float radius)
	{
		return global::Math.vector3LookupTable[(int)(Mathf.Repeat(angle, global::Math.PI2) * global::Math.invPI2 * (float)global::Math.lookupResolution)] * radius;
	}

	public static Vector2 Point2OnCircle(float angle, float radius)
	{
		return global::Math.vector2LookupTable[(int)(Mathf.Repeat(angle, global::Math.PI2) * global::Math.invPI2 * (float)global::Math.lookupResolution)] * radius;
	}

	public static Vector3 GetRandomPointOnSphere()
	{
		global::Math.pointIndex++;
		if (global::Math.pointIndex >= 1000)
		{
			global::Math.pointIndex = 0;
		}
		return global::Math.pointOnSphereTable[global::Math.pointIndex];
	}

	public static Vector3 GetRandomPointInsideSphere()
	{
		global::Math.pointIndex++;
		if (global::Math.pointIndex >= 1000)
		{
			global::Math.pointIndex = 0;
		}
		return global::Math.pointInsideSphereTable[global::Math.pointIndex];
	}

	public static Vector3 ForceZeroZ(Vector3 v)
	{
		return new Vector3(v.x, v.y, 0f);
	}

	public static int RandomNegativePositive()
	{
		if (UnityEngine.Random.value > 0.5f)
		{
			return -1;
		}
		return 1;
	}

	public static float GetAngle(float x, float y)
	{
		return Mathf.Atan2(y, x);
	}

	public static float GetAngle(Vector2 v)
	{
		return Mathf.Atan2(v.y, v.x);
	}

	public static float GetAngle(Vector3 v)
	{
		return Mathf.Atan2(v.y, v.x);
	}

	public static float timer;

	private static float[] sinLookupTable;

	private static float[] cosLookupTable;

	private static float[,] angleLookupTable;

	private static int lookupResolution = 1000;

	private static Vector3[] vector3LookupTable;

	private static Vector2[] vector2LookupTable;

	private static int pointIndex;

	private static Vector3[] pointOnSphereTable;

	private static Vector3[] pointInsideSphereTable;

	public static float PI2 = 1f;

	public static float invPI2 = 1f;

	private static bool setup;
}
