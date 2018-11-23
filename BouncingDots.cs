// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BouncingDots : MonoBehaviour
{
	private void Update()
	{
		this.t += Time.deltaTime * this.speed;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			float value = this.curve.Evaluate(this.t - this.offset * (float)i);
			base.transform.GetChild(i).SetLocalY(value);
		}
	}

	public AnimationCurve curve;

	private float t;

	public float offset = 0.2f;

	public float speed = 3f;
}
