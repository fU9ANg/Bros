// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class GrenadePrimaryFire : Grenade
{
	protected override void Start()
	{
		base.Start();
		this.bounceYOffset = -2f;
	}

	public override void Launch(float x, float y, float xI, float yI)
	{
		if (Map.InsideWall(x, y, this.size + 1f))
		{
			MapController.DamageGround(this.firedBy, 16, DamageType.Crush, this.size + 1f, x, y, null);
		}
		base.Launch(x, y, xI, yI);
	}

	protected override void MakeEffects()
	{
		base.MakeEffects();
	}

	public override void Death()
	{
		if (base.FiredLocally)
		{
			if (!GameModeController.DoesPlayerNumDamage(0, 2))
			{
				this.playerNum = 15;
			}
			MapController.DamageGround(this.firedBy, this.damage, this.damageType, this.range, this.x, this.y, null);
			Map.ExplodeUnits(this.firedBy, this.damage, this.damageType, this.range * 1.3f, this.range, this.x, this.y, this.blastForce, 300f, this.playerNum, false, true);
		}
		this.MakeEffects();
		this.DestroyGrenade();
	}

	protected override bool CanBounceOnEnemies()
	{
		return Mathf.Abs(this.xI) > 160f || this.yI < -120f;
	}

	protected override void RunWarnings()
	{
		if (this.life < 0.5f)
		{
			if (!this.panickedMooks)
			{
				Map.PanicUnits(this.x, this.y, 28f, 0.9f, false);
			}
			this.flickerCounter += this.t;
			if (this.flickerCounter > 0.0667f)
			{
				this.flickerCounter -= 0.0667f;
				if (this.mainMaterialShowing)
				{
					base.GetComponent<Renderer>().sharedMaterial = this.otherMaterial;
					this.pulseCount++;
					if (this.pulseCount % 2 == 1 && this.soundHolder.greeting.Length > 0)
					{
						Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.greeting, 0.33f, base.transform.position, 0.8f + 0.1f * (float)this.pulseCount);
					}
				}
				else
				{
					base.GetComponent<Renderer>().sharedMaterial = this.mainMaterial;
				}
				this.mainMaterialShowing = !this.mainMaterialShowing;
			}
		}
		else
		{
			this.flickerCounter += this.t;
			if (this.flickerCounter > 0.133f)
			{
				this.flickerCounter -= 0.133f;
				if (this.mainMaterialShowing)
				{
					base.GetComponent<Renderer>().sharedMaterial = this.otherMaterial;
					this.pulseCount++;
					if (this.pulseCount % 2 == 1)
					{
					}
				}
				else
				{
					base.GetComponent<Renderer>().sharedMaterial = this.mainMaterial;
				}
				this.mainMaterialShowing = !this.mainMaterialShowing;
			}
		}
	}
}
