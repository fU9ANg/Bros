// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AlienEarthBlock : BrickBlock
{
	public override void ShowForeground(bool isEarth, bool isBrick, bool onlyEdges)
	{
		base.ShowForeground(isEarth, isBrick, onlyEdges);
		if (!this.bottomSolidAtStart)
		{
			BlockPiece meshObject = this.AddForegroundDecorationPiece(this.AlienBoneDressing, -14f);
			this.AddObject(meshObject);
		}
		if (base.RightBlock != null && (double)UnityEngine.Random.value < 0.35 && !this.topSolidAtStart && base.IsRightTheSame && !base.IsTopRightTheSame)
		{
			BlockPiece meshObject2 = this.AddForegroundDecorationPiece(this.UpperDressing, -15f + UnityEngine.Random.Range(0f, 0.1f));
			this.AddObject(meshObject2);
			base.RightBlock.AddObject(meshObject2);
		}
	}

	protected override void EffectsDestroyed(float xI, float yI, float force)
	{
		EffectsController.CreateBloodGushEffect(BloodColor.Green, this.x - 2f + UnityEngine.Random.value * 4f, this.y - 2f + UnityEngine.Random.value * 4f, Mathf.Sign(xI) * -50f, -20f + UnityEngine.Random.value * 40f);
		EffectsController.CreateBloodGushEffect(BloodColor.Green, this.x - 2f + UnityEngine.Random.value * 4f, this.y - 2f + UnityEngine.Random.value * 4f, Mathf.Sign(xI) * 30f, -20f + UnityEngine.Random.value * 40f);
		EffectsController.CreateAlienParticles(this.x, this.y, 12, 5f, 20f, 0f, 0f);
		int num = 1;
		EffectsController.CreateBloodParticles(BloodColor.Green, this.x, this.y + 6f, 2, 5f, 5f, 90f, xI * 0.3f + (float)(num * 70), 25f);
		num *= -1;
		EffectsController.CreateBloodParticles(BloodColor.Green, this.x, this.y + 6f, 2, 5f, 5f, 90f, xI * 0.3f + (float)(num * 70), 25f);
		num *= -1;
		EffectsController.CreateBloodParticles(BloodColor.Green, this.x, this.y + 6f, 2, 5f, 5f, 70f, xI * 0.3f + (float)num * (70f + UnityEngine.Random.value * 50f), 25f + UnityEngine.Random.value * 200f);
		num *= -1;
		EffectsController.CreateBloodParticles(BloodColor.Green, this.x, this.y + 6f, 2, 5f, 5f, 70f, xI * 0.3f + (float)num * (70f + UnityEngine.Random.value * 50f), 25f + UnityEngine.Random.value * 200f);
		num *= -1;
		EffectsController.CreateBloodParticles(BloodColor.Green, this.x, this.y + 6f, 2, 5f, 5f, 70f, xI * 0.3f + (float)num * (70f + UnityEngine.Random.value * 50f), 25f + UnityEngine.Random.value * 200f);
		this.PlayDeathSound();
	}

	protected override void EffectsCollapse(float xI, float yI)
	{
		EffectsController.CreateBloodGushEffect(BloodColor.Green, this.x - 2f + UnityEngine.Random.value * 4f, this.y - 2f + UnityEngine.Random.value * 4f, Mathf.Sign(xI) * -50f, -20f + UnityEngine.Random.value * 40f);
		EffectsController.CreateBloodGushEffect(BloodColor.Green, this.x - 2f + UnityEngine.Random.value * 4f, this.y - 2f + UnityEngine.Random.value * 4f, Mathf.Sign(xI) * 30f, -20f + UnityEngine.Random.value * 40f);
		EffectsController.CreateAlienParticles(this.x, this.y, 12, 5f, 20f, 0f, 0f);
		int num = 1;
		EffectsController.CreateBloodParticles(BloodColor.Green, this.x, this.y + 6f, 2, 5f, 5f, 30f, xI * 0.3f + (float)(num * 30), 25f);
		num *= -1;
		EffectsController.CreateBloodParticles(BloodColor.Green, this.x, this.y + 6f, 2, 5f, 5f, 30f, xI * 0.3f + (float)(num * 30), 25f);
		num *= -1;
		EffectsController.CreateBloodParticles(BloodColor.Green, this.x, this.y + 6f, 2, 5f, 5f, 35f, xI * 0.3f + (float)num * (30f + UnityEngine.Random.value * 20f), 25f + UnityEngine.Random.value * 200f);
		num *= -1;
		EffectsController.CreateBloodParticles(BloodColor.Green, this.x, this.y + 6f, 2, 5f, 5f, 35f, xI * 0.3f + (float)num * (30f + UnityEngine.Random.value * 20f), 25f + UnityEngine.Random.value * 200f);
		num *= -1;
		EffectsController.CreateBloodParticles(BloodColor.Green, this.x, this.y + 6f, 2, 5f, 5f, 35f, xI * 0.3f + (float)num * (30f + UnityEngine.Random.value * 20f), 25f + UnityEngine.Random.value * 200f);
		this.PlayDeathSound();
	}

	public BlockPiece[] AlienBoneDressing;

	public BlockPiece[] UpperDressing;
}
