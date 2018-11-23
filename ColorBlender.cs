// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ColorBlender : MonoBehaviour
{
	private void Update()
	{
		if (this.delay > 0f)
		{
			this.delay -= Time.deltaTime;
			return;
		}
		this.fadeTimer -= Time.deltaTime * this.fadeRate;
		float num = this.fadeTimer / 1f;
		num = Mathf.Clamp01(num);
		foreach (AmplifyColorEffect amplifyColorEffect in this.colorEffects)
		{
			amplifyColorEffect.BlendAmount = num;
		}
	}

	public AmplifyColorEffect[] colorEffects;

	private float fadeTimer = 1f;

	public float fadeRate = 1f;

	public float delay;
}
