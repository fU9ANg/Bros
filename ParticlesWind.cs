// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ParticlesWind : MonoBehaviour
{
	private void Awake()
	{
		ParticlesWind.hasController = false;
	}

	private void Start()
	{
		this.particlesForce = (ParticlesWind.currentParticlesForce = new Vector3(7f, 12f, 0f));
		this.pA = base.GetComponent<ParticleAnimator>();
		if (!ParticlesWind.hasController)
		{
			ParticlesWind.hasController = true;
			this.controller = true;
		}
	}

	private void Update()
	{
		if (this.controller)
		{
			this.windCounter += Time.deltaTime;
			if (this.windCounter > 0.5f)
			{
				this.windCounter -= 0.5f + UnityEngine.Random.value * 3f;
				this.particlesForce = new Vector3(5f + UnityEngine.Random.value * 20f, 15f + UnityEngine.Random.value * 15f, 0f);
			}
			ParticlesWind.currentParticlesForce = Vector3.Lerp(ParticlesWind.currentParticlesForce, this.particlesForce, Time.deltaTime * 9f);
		}
		this.pA.force = ParticlesWind.currentParticlesForce;
	}

	protected float windCounter;

	protected Vector3 particlesForce = Vector3.up;

	protected static Vector3 currentParticlesForce = Vector3.up;

	protected ParticleAnimator pA;

	protected bool controller;

	protected static bool hasController = false;
}
