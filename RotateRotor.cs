// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class RotateRotor : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		this.angle += Time.deltaTime * this.speed;
		if (this.yAxis)
		{
			base.transform.localEulerAngles = new Vector3(90f, this.angle, 0f);
		}
		if (this.xAxis)
		{
			base.transform.localEulerAngles = new Vector3(this.angle, 0f, 0f);
		}
		if (this.zAxis)
		{
			base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, base.transform.localEulerAngles.y, this.angle);
		}
	}

	protected float angle;

	public bool yAxis = true;

	public bool xAxis;

	public bool zAxis;

	public float speed = 2000f;
}
