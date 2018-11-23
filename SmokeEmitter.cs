// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SmokeEmitter : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		this.smokeCounter += Time.deltaTime;
		if (this.smokeCounter > this.smokeRate)
		{
			this.smokeCounter -= this.smokeRate;
			WorldMapEffectsController.EmitSmoke(1, this.worldTransform.InverseTransformPoint(base.transform.position), this.smokeColor, this.emitRadius, this.force, this.tangentForce, this.life);
		}
	}

	protected float smokeCounter;

	public float smokeRate = 0.045f;

	public Color smokeColor;

	public float emitRadius = 0.1f;

	public float force = 0.5f;

	public float tangentForce = 0.5f;

	public float life = 1f;

	public Transform worldTransform;
}
