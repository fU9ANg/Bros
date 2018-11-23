// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Wobble : MonoBehaviour
{
	private void Start()
	{
		this.scaleCounter += this.sinOffset;
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		this.scaleCounter += num;
		base.transform.localScale = Vector3.one * (((!(this.projectileOptional != null)) ? 1f : Mathf.Clamp(this.projectileOptional.life + 0.4f, 0.4f, 1.2f)) + Mathf.Sin(this.scaleCounter * this.sinSpeed) * 0.4f + 0.2f);
	}

	protected float scaleCounter;

	public Projectile projectileOptional;

	public float sinSpeed = 40f;

	public float sinOffset = 1f;
}
