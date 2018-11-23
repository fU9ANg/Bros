// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
	private void Update()
	{
		base.transform.LookAt(base.transform.position - (Camera.main.transform.position - base.transform.position), Vector3.up);
	}
}
