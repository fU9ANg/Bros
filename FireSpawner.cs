// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FireSpawner : MonoBehaviour
{
	private void Update()
	{
		this.offScreenDelay -= Time.deltaTime;
		if (this.offScreenDelay <= 0f)
		{
			this.fireCounter += Time.deltaTime;
			if (this.fireCounter > this.frequency)
			{
				this.fireCounter -= this.frequency;
				if (SortOfFollow.IsItSortOfVisible(base.transform.position, 24f, 36f, ref this.xOffScreen, ref this.yOffScreen))
				{
					EffectsController.CreateBackgroundFlameParticle(base.transform.position.x + this.xOffset + (1f - UnityEngine.Random.value * 2f) * this.range, base.transform.position.y + this.yOffset + (1f - UnityEngine.Random.value * 2f) * this.range, this.fireZ);
					if (this.sparks)
					{
						EffectsController.CreateSparkParticle(base.transform.position.x + this.xOffset, base.transform.position.y + this.yOffset, this.range, this.range, 0f, 0f, 0.4f + UnityEngine.Random.value * 0.3f);
					}
				}
				else
				{
					this.offScreenDelay = Mathf.Min(Mathf.Min(this.xOffScreen, this.yOffScreen) / 150f, 5f);
				}
			}
		}
	}

	protected float fireCounter;

	public float xOffset;

	public float yOffset = 4f;

	public float fireZ = 9f;

	public float range = 4f;

	public float frequency = 0.04f;

	public bool sparks = true;

	protected float xOffScreen;

	protected float yOffScreen;

	protected float offScreenDelay = 0.003f;
}
