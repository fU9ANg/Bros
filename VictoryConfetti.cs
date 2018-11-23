// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class VictoryConfetti : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i < this.emitters.Length; i++)
		{
			this.emitters[i].emit = false;
		}
	}

	private void Update()
	{
		if (this.counter < 5f)
		{
			this.counter += Time.smoothDeltaTime;
			if (this.counter >= 5f)
			{
				for (int i = 0; i < this.emitters.Length; i++)
				{
					this.emitters[i].emit = true;
				}
			}
		}
		else if (this.counter < 6.8f)
		{
			this.counter += Time.smoothDeltaTime;
			if (this.counter >= 6.8f)
			{
				for (int j = 0; j < this.emitters.Length; j++)
				{
					this.emitters[j].minEmission *= 0.5f;
					this.emitters[j].maxEmission = this.emitters[j].maxEmission * 0.7f;
				}
			}
		}
		else if (this.counter < 7.5f)
		{
			this.counter += Time.smoothDeltaTime;
			if ((double)this.counter >= 7.5)
			{
				for (int k = 0; k < this.emitters.Length; k++)
				{
					this.emitters[k].minEmission *= 0.2f;
					this.emitters[k].maxEmission = this.emitters[k].maxEmission * 0.3f;
				}
			}
		}
		else if (this.counter < 9f)
		{
			this.counter += Time.smoothDeltaTime;
			if (this.counter >= 9f)
			{
				for (int l = 0; l < this.emitters.Length; l++)
				{
					this.emitters[l].emit = false;
				}
			}
		}
	}

	protected float counter;

	public ParticleEmitter[] emitters;
}
