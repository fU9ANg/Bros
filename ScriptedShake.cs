// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class ScriptedShake : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(this.startDelay);
		this.shake.AddShake2(this.amplitudeX, this.amplitudeY, this.Frequency);
		yield break;
	}

	private void Update()
	{
	}

	public float startDelay;

	public float amplitudeX;

	public float amplitudeY;

	public float Frequency;

	public Shake shake;
}
