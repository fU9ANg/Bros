// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Cloud : MonoBehaviour
{
	private void Start()
	{
		base.transform.localPosition = base.transform.localPosition.normalized * this.cloudHeight;
		base.transform.forward = -base.transform.localPosition.normalized;
	}

	private void Update()
	{
	}

	public float cloudHeight = 6f;
}
