// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class LadderBlock : Block
{
	protected override void EffectsCollapse(float xI, float yI)
	{
		if (base.gameObject.CompareTag("Wood"))
		{
			EffectsController.CreateWoodParticles(this.x, this.y, 24, 9f, 40f, 0f, 0f, 2000f);
		}
		else if (base.gameObject.CompareTag("Metal"))
		{
			EffectsController.CreateMetalParticles(this.x, this.y, 15, 9f, 40f, 0f, 0f, 320f);
			EffectsController.CreateSparkShower(this.x, this.y, 3, 7f, 100f, xI * 0.5f, 100f, 0.2f, 0f);
		}
		else
		{
			UnityEngine.Debug.LogError("Effects not implemented");
		}
	}

	protected override void EffectsDestroyed(float xI, float yI, float force)
	{
		if (base.gameObject.CompareTag("Wood"))
		{
			EffectsController.CreateWoodParticles(this.x, this.y, 24, 9f, force, 0f, 0f, 2000f);
		}
		else if (base.gameObject.CompareTag("Metal"))
		{
			EffectsController.CreateMetalParticles(this.x, this.y, 15, 9f, 40f, 0f, 0f, 320f);
			EffectsController.CreateSparkShower(this.x, this.y, 3, 7f, 100f, xI * 0.5f, 100f, 0.2f, 0f);
		}
		else
		{
			UnityEngine.Debug.LogError("Effects not implemented");
		}
	}
}
