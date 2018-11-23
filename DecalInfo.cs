// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DecalInfo
{
	public DecalInfo(DecalType type, BloodColor BloodColor, Vector3 position, DirectionEnum direction, float size, Vector3 HitNormal)
	{
		this.type = type;
		this.position = position;
		this.direction = direction;
		this.size = size;
		this.hitNormal = HitNormal;
		this.bloodColor = BloodColor;
	}

	public DecalType type;

	public Vector3 position;

	public DirectionEnum direction;

	public Vector3 hitNormal;

	public BloodColor bloodColor;

	public float size = 1f;
}
