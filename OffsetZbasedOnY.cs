// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class OffsetZbasedOnY : MonoBehaviour
{
	private void Awake()
	{
		Vector3 a = Vector3.forward;
		if (this.type == OffsetZbasedOnY.Type.LowerIsCloser)
		{
			a = -Vector3.forward;
		}
		base.transform.position -= a * 0.001f * base.transform.position.y;
	}

	private void Update()
	{
	}

	public OffsetZbasedOnY.Type type;

	public enum Type
	{
		LowerIsCloser,
		LowerIsFurther
	}
}
