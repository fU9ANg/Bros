// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(0f, 0f, this.speed * Time.deltaTime, Space.Self);
	}

	public float speed = 500f;
}
