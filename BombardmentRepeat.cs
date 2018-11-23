// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BombardmentRepeat
{
	public BombardmentRepeat(Vector3 target, int seed)
	{
		this.random = new Randomf(seed);
		this.targetPoint = target;
	}

	public float countDown = 3f;

	public Vector3 targetPoint;

	public Randomf random;
}
