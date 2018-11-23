// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class GrenadeSmall : Grenade
{
	protected override void Bounce(bool bounceX, bool bounceY)
	{
		base.Bounce(bounceX, bounceY);
		if (this.explodeOnImpact)
		{
			this.Death();
		}
	}

	protected override bool CanBounceOnEnemies()
	{
		return this.bounceOnEnemiesAnyDirection || base.CanBounceOnEnemies();
	}

	protected override void MakeEffects()
	{
		EffectsController.CreateShrapnel(this.shrapnel, this.x, this.y, 1f, 130f, 6f, 0f, 150f);
		EffectsController.CreateEffect(this.explosion, this.x, this.y, 0f, Vector3.zero, BloodColor.None);
		Vector3 a = this.random.insideUnitCircle;
		a = this.random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire1, this.x + a.x * this.range * 0.15f, this.y + a.y * this.range * 0.25f, 0.1f + this.random.value * 0.2f, a * this.range * 0.5f * 3f);
		a = this.random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire2, this.x + a.x * this.range * 0.15f, this.y + a.y * this.range * 0.25f, 0.1f + this.random.value * 0.2f, a * this.range * 0.5f * 3f);
		SortOfFollow.Shake(0.3f, base.transform.position);
		this.PlayDeathSound();
		Map.DisturbWildLife(this.x, this.y, 60f, 5);
	}

	protected override void Update()
	{
		base.Update();
		if (this.life > 1f)
		{
			this.flickerCounter += this.t;
			if (this.flickerCounter > 0.0667f)
			{
				this.flickerCounter -= 0.0667f;
				if (this.mainMaterialShowing)
				{
					base.GetComponent<Renderer>().sharedMaterial = this.otherMaterial;
				}
				else
				{
					base.GetComponent<Renderer>().sharedMaterial = this.mainMaterial;
				}
				this.mainMaterialShowing = !this.mainMaterialShowing;
			}
		}
	}

	public bool explodeOnImpact = true;

	public bool bounceOnEnemiesAnyDirection;
}
