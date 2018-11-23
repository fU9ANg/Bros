// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WorldMapParticleAnimator : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		this.particles = base.GetComponent<ParticleEmitter>().particles;
		this.particleList.Clear();
		for (int i = 0; i < this.particles.Length; i++)
		{
			Particle[] array = this.particles;
			int num2 = i;
			array[num2].energy = array[num2].energy - num;
			Particle[] array2 = this.particles;
			int num3 = i;
			array2[num3].velocity = array2[num3].velocity + this.gravity * this.particles[i].position.normalized * num;
			Particle[] array3 = this.particles;
			int num4 = i;
			array3[num4].velocity = array3[num4].velocity * (1f - num * this.dampening);
			Particle[] array4 = this.particles;
			int num5 = i;
			array4[num5].position = array4[num5].position + this.particles[i].velocity * num;
			Particle[] array5 = this.particles;
			int num6 = i;
			array5[num6].size = array5[num6].size + this.growAmount * num;
			float num7 = this.particles[i].energy / this.particles[i].startEnergy;
			if (this.useFireColors)
			{
				this.particles[i].color = new Color(0.2f + Mathf.Clamp(num7 + num7 * num7, 0f, 1f), 0.15f + Mathf.Clamp(num7 * num7 * num7 * 1f, 0f, 1f), (1f - num7) * 0.1f, Mathf.Clamp(num7 * 3f, 0f, 1f));
			}
			else
			{
				this.particles[i].color = new Color(this.particles[i].color.r, this.particles[i].color.g, this.particles[i].color.b, Mathf.Clamp(num7 * 2f, 0f, 1f));
			}
			if (this.particles[i].energy > 0f && this.particles[i].size > 0f)
			{
				this.particleList.Add(this.particles[i]);
			}
		}
		base.GetComponent<ParticleEmitter>().particles = this.particleList.ToArray();
	}

	protected Particle[] particles;

	public float gravity = 1f;

	protected List<Particle> particleList = new List<Particle>();

	public float growAmount;

	public bool fade = true;

	public float dampening = 1f;

	public bool useFireColors = true;
}
