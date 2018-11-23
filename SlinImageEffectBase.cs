// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[AddComponentMenu("")]
[RequireComponent(typeof(Camera))]
public class SlinImageEffectBase : MonoBehaviour
{
	protected void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	public Material material;
}
