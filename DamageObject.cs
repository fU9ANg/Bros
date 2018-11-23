// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DamageObject
{
	public DamageObject(int damage, DamageType damageType, float xI, float yI, MonoBehaviour damageSender)
	{
		this.damage = damage;
		this.xForce = xI;
		this.yForce = yI;
		this.damageType = damageType;
		this.damageSender = damageSender;
	}

	public int damage;

	public float xForce;

	public float yForce;

	public DamageType damageType;

	public MonoBehaviour damageSender;

	public float x = -100f;

	public float y = -100f;
}
