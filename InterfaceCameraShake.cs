// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class InterfaceCameraShake : MonoBehaviour
{
	public static void Shake(float m, float xM, float yM)
	{
		InterfaceCameraShake.shakeM = m;
		InterfaceCameraShake.sinXRate = (32f + UnityEngine.Random.value * 34f) * xM;
		InterfaceCameraShake.sinYRate = (30f + UnityEngine.Random.value * 34f) * yM;
		InterfaceCameraShake.sinXCounter = 6.28318548f + 3.14159274f * xM;
		InterfaceCameraShake.sinYCounter = 6.28318548f + 3.14159274f * yM;
	}

	private void Awake()
	{
		global::Math.SetupLookupTables();
		this.centerPos = base.transform.position;
	}

	private void LateUpdate()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		InterfaceCameraShake.sinXCounter += InterfaceCameraShake.sinXRate * num;
		InterfaceCameraShake.sinYCounter += InterfaceCameraShake.sinYRate * num;
		InterfaceCameraShake.sinXRate *= 1f - num * 4f;
		InterfaceCameraShake.sinYRate *= 1f - num * 4f;
		if (InterfaceCameraShake.shakeM > 0f)
		{
			InterfaceCameraShake.shakeM -= num * 2f;
			if (InterfaceCameraShake.shakeM <= 0f)
			{
				InterfaceCameraShake.shakeM = 0f;
			}
			base.transform.position = new Vector3(this.centerPos.x + global::Math.Sin(InterfaceCameraShake.sinXCounter) * InterfaceCameraShake.shakeM * 20f, this.centerPos.y + global::Math.Sin(InterfaceCameraShake.sinYCounter) * InterfaceCameraShake.shakeM * 20f, this.centerPos.z);
		}
	}

	protected Vector3 centerPos;

	protected static float sinXRate;

	protected static float sinYRate;

	protected static float sinXCounter;

	protected static float sinYCounter;

	protected static float shakeM;
}
