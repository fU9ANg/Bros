// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class GrenadeMolotovFire : Grenade
{
	protected override void Bounce(bool bounceX, bool bounceY)
	{
		this.Death();
	}

	public override void Death()
	{
		this.MakeEffects();
		Map.HitLivingUnits(this.firedBy, this.playerNum, 1, DamageType.Fire, 24f, this.x, this.y, this.xI, this.yI, true, false);
		Networking.RPC<float, float, float, int>(PID.TargetAll, new RpcSignature<float, float, float, int>(MapController.BurnGround_Local), 15f, this.x, this.y, this.groundLayer, false);
		Sound instance = Sound.GetInstance();
		instance.PlaySoundEffectAt(this.fireSound.deathSounds, 0.1f, base.transform.position);
		this.DestroyGrenade();
	}

	protected override void DestroyGrenade()
	{
		Map.RemoveGrenade(this);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	protected override void MakeEffects()
	{
		Vector3 a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.5f, a * this.range * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.5f, a * this.range * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire3, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.2f, UnityEngine.Random.insideUnitSphere * this.range * 3f);
	}

	protected override void Update()
	{
		base.Update();
		this.flameCounter += this.t;
		if (this.flameCounter > 0.0334f)
		{
			this.flameCounter -= 0.01667f;
			Vector3 a = UnityEngine.Random.insideUnitCircle;
			this.flameCount++;
			if (this.flameCount % 4 == 0)
			{
				Map.HitLivingUnits(this.firedBy, this.playerNum, 1, DamageType.Fire, 16f, this.x, this.y, this.xI, this.yI, true, false);
			}
			int num = UnityEngine.Random.Range(0, 3);
			if (num != 0)
			{
				if (num != 1)
				{
					EffectsController.CreateEffect(this.fire2, this.x + a.x * this.range * 0.001f, this.y + a.y * this.range * 0.001f, 0f, a * this.range);
				}
				else
				{
					EffectsController.CreateEffect(this.fire3, this.x + a.x * this.range * 0.001f, this.y + a.y * this.range * 0.001f, 0f, a * this.range);
				}
			}
			else
			{
				EffectsController.CreateEffect(this.fire1, this.x + a.x * this.range * 0.001f, this.y + a.y * this.range * 0.001f, 0f, a * this.range);
			}
		}
	}

	public SoundHolder fireSound;

	protected int flameCount;

	protected float flameCounter;
}
