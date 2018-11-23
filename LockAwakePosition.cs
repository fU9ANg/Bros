// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class LockAwakePosition : MonoBehaviour
{
	private void Start()
	{
		this.AwakePos = base.transform.position;
	}

	private void Update()
	{
		base.transform.position = this.AwakePos;
	}

	private void LateUpdate()
	{
		base.transform.position = this.AwakePos;
	}

	private Vector3 AwakePos;
}
