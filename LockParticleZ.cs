// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[ExecuteInEditMode]
public class LockParticleZ : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		ParticleSystem.Particle[] array = new ParticleSystem.Particle[base.GetComponent<ParticleSystem>().particleCount];
		base.GetComponent<ParticleSystem>().GetParticles(array);
		for (int i = 0; i < array.Length; i++)
		{
			Vector3 position = array[i].position;
			position.z = base.transform.position.z;
			array[i].position = position;
		}
		base.GetComponent<ParticleSystem>().SetParticles(array, array.Length);
	}
}
