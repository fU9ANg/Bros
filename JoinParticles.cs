// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[ExecuteInEditMode]
public class JoinParticles : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (this.UpdateMaterial)
		{
			this.UpdateMaterial = false;
			ParticleSystem[] componentsInChildren = base.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem particleSystem in componentsInChildren)
			{
				particleSystem.GetComponent<Renderer>().material = this.material;
			}
		}
	}

	public bool UpdateMaterial;

	public Material material;
}
