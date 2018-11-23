// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class StarFieldCamera : MonoBehaviour
{
	private void Start()
	{
		base.transform.position = this.startPoint;
	}

	private void Update()
	{
		base.transform.position = Vector3.Lerp(base.transform.position, this.targetPoint, Mathf.Clamp(Time.deltaTime * 0.4f, 0f, 0.033f));
	}

	public Vector3 targetPoint = new Vector3(0f, 0f, -70f);

	public Vector3 startPoint = new Vector3(0f, 0f, -200f);
}
