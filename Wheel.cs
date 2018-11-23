// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Wheel : MonoBehaviour
{
	private void Start()
	{
		this.prevX = base.transform.position.x;
	}

	private void Update()
	{
		float num = base.transform.position.x - this.prevX;
		this.prevX = base.transform.position.x;
		float num2 = 4f;
		float num3 = num;
		float num4 = num2 * num3;
		base.transform.Rotate(0f, 0f, -num4);
	}

	public float rotationM = 1f;

	private float prevX;
}
