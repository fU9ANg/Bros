// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class LightFlare : MonoBehaviour
{
	private void Start()
	{
		this.startX = base.transform.localPosition.x;
	}

	private void Update()
	{
		float num = base.transform.localPosition.x - this.startX;
		num = Mathf.Abs(num);
		float num2 = 1f - num / this.distance;
		num2 = Mathf.Clamp01(num2);
		float num3 = Mathf.Min(num2 * num2, 0.2f);
		num3 *= 1.5f;
		Color color = base.GetComponent<Renderer>().material.GetColor("_TintColor");
		color.a = num3;
		base.GetComponent<Renderer>().material.SetColor("_TintColor", color);
		if (num > this.distance)
		{
			this.Direction *= -1;
		}
		base.transform.localPosition += Vector3.right * this.speed * Time.deltaTime * (float)this.Direction;
	}

	public int Direction = 1;

	public float speed = 150f;

	public float distance = 256f;

	private float startX;
}
