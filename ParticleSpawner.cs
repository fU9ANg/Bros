// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
	private void Start()
	{
		this.counter = 0.2f + UnityEngine.Random.value;
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
	}

	public void SetDripColor(BloodColor Col)
	{
		this.color = Col;
	}

	private void Update()
	{
		this.counter -= Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.counter < 0f)
		{
			if (SetResolutionCamera.IsItVisible(base.transform.position))
			{
				this.counterInc = Mathf.Clamp(this.counterInc + this.delay * this.delayMaxM * 0.3f, 0f, this.delay * this.delayMaxM);
				this.counter = this.delay + UnityEngine.Random.value + this.counterInc;
				EffectsController.CreateBloodParticlesDrip(this.x, this.y, 3f, 0f, 1f, this.color);
			}
			else
			{
				this.counter = 3f + this.delay;
			}
		}
	}

	protected float counter;

	protected float counterInc;

	protected float x;

	protected float y;

	public float delay = 0.2f;

	public float delayMaxM = 7f;

	public float force;

	public float count = 1f;

	public float xForce;

	public float yForce;

	[HideInInspector]
	public BloodColor color;
}
