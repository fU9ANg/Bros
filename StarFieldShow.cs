// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class StarFieldShow : MonoBehaviour
{
	private void Start()
	{
		this.pR = base.GetComponent<ParticleRenderer>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F6))
		{
			Application.LoadLevel(Application.loadedLevel);
		}
		this.starFieldM += Mathf.Clamp(Time.deltaTime * 0.5f, 0f, 0.033f);
		float num = Mathf.Clamp01(this.starFieldM);
		this.pR.velocityScale = -(this.minLength * num + this.maxLength * (1f - num));
		UnityEngine.Debug.Log(this.pR.velocityScale);
	}

	protected float starFieldM = -0.5f;

	protected float minLength = 0.1f;

	protected float maxLength = 0.22f;

	private ParticleRenderer pR;
}
