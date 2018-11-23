// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Randomf
{
	public Randomf(int Seed)
	{
		this.seed = Seed;
		this.randomGenerator = new System.Random(Seed);
	}

	public int NoOfCalls
	{
		get
		{
			return this.noOfCalls;
		}
		set
		{
			for (int i = 0; i < value; i++)
			{
				float value2 = this.value;
				if (value2 < 0f)
				{
				}
			}
		}
	}

	public float value
	{
		get
		{
			this.noOfCalls++;
			return (float)this.randomGenerator.NextDouble();
		}
	}

	public Vector3 onUnitSphere
	{
		get
		{
			Vector3 vector = new Vector3(this.value, this.value, this.value);
			return vector.normalized;
		}
	}

	public Vector2 onUnitCirle
	{
		get
		{
			float f = this.value * global::Math.PI2;
			return new Vector2
			{
				x = Mathf.Cos(f),
				y = Mathf.Sin(f)
			};
		}
	}

	public Vector3 insideUnitSphere
	{
		get
		{
			Vector3 vector = new Vector3(this.value, this.value, this.value);
			return vector.normalized * this.value;
		}
	}

	public Vector2 insideUnitCircle
	{
		get
		{
			return this.onUnitCirle * this.value;
		}
	}

	public float Range(float min, float max)
	{
		return min + (max - min) * this.value;
	}

	public int Range(int min, int max)
	{
		return min + Mathf.FloorToInt((float)(max - min) * this.value);
	}

	public int seed = -1;

	private int noOfCalls;

	private System.Random randomGenerator;
}
