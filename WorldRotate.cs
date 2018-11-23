// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldRotate : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(0f, 10f * Time.deltaTime, 0f, Space.Self);
	}
}
