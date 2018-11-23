// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class GrenadeCluster : Grenade
{
	protected override void Awake()
	{
		base.Awake();
	}

	public override void Death()
	{
		this.wavesOfClusters--;
		if (this.wavesOfClusters <= 0)
		{
			base.Death();
		}
		else
		{
			this.life += this.waveDelay;
			this.PlaySpecialSound(0.2f);
			EffectsController.CreateSmallExplosion(this.x, this.y, 0f, 0.5f, 0f);
			this.forceM *= 1.6f;
		}
		if (this.smallGrenadePrefab != null)
		{
			float num = this.startRadianAngle;
			Vector3 vector = global::Math.Point3OnCircle(num - 0.1f + UnityEngine.Random.value * 0.2f, 200f + (40f + this.random.value * 75f) * this.forceM);
			ProjectileController.SpawnGrenadeOverNetwork(this.smallGrenadePrefab, this.firedBy, this.x, this.y, 0.001f, 0.011f, vector.x, vector.y, this.playerNum);
			vector = global::Math.Point3OnCircle(num - 0.2f - this.random.value * 0.3f, 200f + (40f + this.random.value * 75f) * this.forceM);
			ProjectileController.SpawnGrenadeOverNetwork(this.smallGrenadePrefab, this.firedBy, this.x, this.y, 0.001f, 0.011f, vector.x, vector.y, this.playerNum);
			vector = global::Math.Point3OnCircle(num + 0.2f + this.random.value * 0.3f, 200f + (40f + this.random.value * 75f) * this.forceM);
			ProjectileController.SpawnGrenadeOverNetwork(this.smallGrenadePrefab, this.firedBy, this.x, this.y, 0.001f, 0.011f, vector.x, vector.y, this.playerNum);
		}
		if (base.FiredLocally && this.flameWavePrefab != null)
		{
			FlashBangExplosion @object = Networking.Instantiate<FlashBangExplosion>(this.flameWavePrefab, base.transform.position + Vector3.up, Quaternion.identity, null, false);
			Networking.RPC<int, MonoBehaviour, DirectionEnum>(PID.TargetAll, new RpcSignature<int, MonoBehaviour, DirectionEnum>(@object.Setup), this.playerNum, this.firedBy, DirectionEnum.Any, false);
		}
	}

	protected override void MakeEffects()
	{
		EffectsController.CreateShrapnel(this.shrapnel, this.x, this.y, 3f, 150f, 20f, 0f, 150f);
		Vector3 a = this.random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, this.random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = this.random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, this.random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		EffectsController.CreateEffect(this.smoke1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, this.random.value * 0.2f, this.random.insideUnitSphere * this.range * 3f, BloodColor.None);
		a = this.random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, this.random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = this.random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, this.random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = this.random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, this.random.value * 0.2f, a * this.range * 3f, BloodColor.None);
		a = this.random.insideUnitCircle;
		EffectsController.CreateEffect(this.explosion, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + this.random.value * 0.2f, a * this.range * 3f, BloodColor.None);
		a = this.random.insideUnitCircle;
		EffectsController.CreateEffect(this.explosionBig, this.x + a.x * this.range * 0.05f, this.y + a.y * this.range * 0.05f, 0f, Vector3.zero, BloodColor.None);
		SortOfFollow.Shake(1f, base.transform.position);
		this.PlayDeathSound();
		Map.DisturbWildLife(this.x, this.y, 120f, 5);
	}

	public float startRadianAngle = 1.57079637f;

	public int wavesOfClusters = 1;

	public float waveDelay = 0.1f;

	protected float forceM = 0.5f;

	public Grenade smallGrenadePrefab;

	public FlashBangExplosion flameWavePrefab;
}
