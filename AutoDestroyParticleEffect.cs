// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AutoDestroyParticleEffect : MonoBehaviour
{
	private void Start()
	{
		this.particles = base.GetComponent<ParticleSystem>();
	}

	private void Update()
	{
		if (!this.particles.isPlaying)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private ParticleSystem particles;
}
