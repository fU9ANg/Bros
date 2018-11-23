// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class GrenadeMolotov : Grenade
{
	protected override void MakeEffects()
	{
		EffectsController.CreateShrapnel(this.shrapnel, this.x, this.y, 3f, 120f, 12f, 0f, 250f);
		Vector3 a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		EffectsController.CreateEffect(this.smoke1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.2f, UnityEngine.Random.insideUnitSphere * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.2f, a * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0f, a * this.range * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0f, a * this.range * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire3, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0f, a * this.range * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire3, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire3, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f);
		SortOfFollow.Shake(0.2f);
		this.PlayDeathSound();
		Map.DisturbWildLife(this.x, this.y, 120f, 5);
	}

	public override void Death()
	{
		if (base.FiredLocally)
		{
			Networking.RPC<float, float, float, int>(PID.TargetAll, new RpcSignature<float, float, float, int>(MapController.BurnGround_Local), this.range, this.x, this.y, this.groundLayer, false);
			Map.HitLivingUnits(this.firedBy, this.playerNum, 3, DamageType.Fire, 12f, this.x, this.y, this.xI, this.yI, true, false);
		}
		this.MakeEffects();
		float num = 0.3f - UnityEngine.Random.value * 0.5f;
		Vector3 vector = global::Math.Point3OnCircle(num, 210f + UnityEngine.Random.value * 50f);
		if (base.FiredLocally)
		{
			ProjectileController.SpawnGrenadeOverNetwork(this.smallGrenadePrefab, this.firedBy, this.x, this.y + 2f, 0.001f, 0.011f, vector.x, vector.y, this.playerNum);
			vector = global::Math.Point3OnCircle(num + 0.8f + UnityEngine.Random.value * 0.2f, 210f + UnityEngine.Random.value * 50f);
			ProjectileController.SpawnGrenadeOverNetwork(this.smallGrenadePrefab, this.firedBy, this.x, this.y + 2f, 0.001f, 0.011f, vector.x, vector.y, this.playerNum);
			vector = global::Math.Point3OnCircle(num + 1.6f + UnityEngine.Random.value * 0.2f, 210f + UnityEngine.Random.value * 50f);
			ProjectileController.SpawnGrenadeOverNetwork(this.smallGrenadePrefab, this.firedBy, this.x, this.y + 2f, 0.001f, 0.011f, vector.x, vector.y, this.playerNum);
			vector = global::Math.Point3OnCircle(num + 2.4f + UnityEngine.Random.value * 0.2f, 210f + UnityEngine.Random.value * 50f);
			ProjectileController.SpawnGrenadeOverNetwork(this.smallGrenadePrefab, this.firedBy, this.x, this.y + 2f, 0.001f, 0.011f, vector.x, vector.y, this.playerNum);
		}
		this.DestroyGrenade();
	}

	protected override void RunMovement()
	{
		if (Map.HitLivingUnits(this.firedBy, this.playerNum, 1, DamageType.Fire, 24f, this.x, this.y, this.xI, this.yI, true, false))
		{
			this.Death();
		}
		else
		{
			base.RunMovement();
		}
	}

	protected override void Bounce(bool bounceX, bool bounceY)
	{
		this.Death();
	}

	public Grenade smallGrenadePrefab;
}
