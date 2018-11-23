// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ParticlesCollision : MonoBehaviour
{
	private void Start()
	{
	}

	private void OnParticleCollision(GameObject other)
	{
		int safeCollisionEventSize = base.GetComponent<ParticleSystem>().GetSafeCollisionEventSize();
		if (this.collisionEvents.Length < safeCollisionEventSize)
		{
			this.collisionEvents = new ParticleCollisionEvent[safeCollisionEventSize];
		}
		int num = base.GetComponent<ParticleSystem>().GetCollisionEvents(base.gameObject, this.collisionEvents);
		int i = 0;
		MonoBehaviour.print(num);
		while (i < num)
		{
			Vector3 intersection = this.collisionEvents[i].intersection;
			Vector3 b = this.collisionEvents[i].velocity * 10f;
			this.splashParticlePrefab = (UnityEngine.Object.Instantiate(this.splashParticlePrefab) as ParticleSystem);
			this.splashParticlePrefab.transform.position = intersection;
			this.splashParticlePrefab.transform.LookAt(intersection + b);
			i++;
		}
	}

	private ParticleCollisionEvent[] collisionEvents = new ParticleCollisionEvent[16];

	public ParticleSystem rainSystem;

	public ParticleSystem splashParticlePrefab;
}
