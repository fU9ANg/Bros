// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Bob : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		Vector3 localPosition = base.transform.localPosition;
		localPosition.y -= this.offset;
		this.theta += Time.deltaTime * 10f;
		if (this.theta < 3.14159274f)
		{
			this.offset = global::Math.Sin(this.theta) * 3f;
		}
		else
		{
			this.offset = 0f;
		}
		if (this.theta > 18.849556f)
		{
			this.theta -= 18.849556f;
		}
		localPosition.y += this.offset;
		base.transform.localPosition = localPosition;
	}

	public float timeoffset;

	private float interval;

	private float lullLnterval;

	public float theta;

	private float offset;
}
