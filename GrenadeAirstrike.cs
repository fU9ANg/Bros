// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class GrenadeAirstrike : Grenade
{
	public override void Death()
	{
		float num = 200f + this.random.value * 30f;
		if (base.FiredLocally)
		{
			float screenMaxY = SortOfFollow.GetScreenMaxY();
			float num2 = screenMaxY - this.y;
			ProjectileController.SpawnProjectileOverNetwork(this.airstrikeProjectile, this.firedBy, this.x - 480f - num2, this.y + 580f + num2, num, -num, false, -10, false, false);
			num = 200f + this.random.value * 30f;
			ProjectileController.SpawnProjectileOverNetwork(this.airstrikeProjectile, this.firedBy, this.x - 480f - num2, this.y + 540f + num2, num, -num, false, -10, false, false);
			num = 200f + this.random.value * 30f;
			ProjectileController.SpawnProjectileOverNetwork(this.airstrikeProjectile, this.firedBy, this.x - 480f - num2, this.y + 500f + num2, num, -num, false, -10, false, false);
			num = 200f + this.random.value * 30f;
			ProjectileController.SpawnProjectileOverNetwork(this.airstrikeProjectile, this.firedBy, this.x - 480f - num2, this.y + 460f + num2, num, -num, false, -10, false, false);
			num = 200f + this.random.value * 30f;
			ProjectileController.SpawnProjectileOverNetwork(this.airstrikeProjectile, this.firedBy, this.x - 480f - num2, this.y + 420f + num2, num, -num, false, -10, false, false);
		}
		this.MakeEffects();
		this.DestroyGrenade();
	}

	protected override void MakeEffects()
	{
		EffectsController.CreateShrapnel(this.shrapnel, this.x, this.y, 3f, 140f, 20f, 0f, 150f);
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
		SortOfFollow.Shake(0.1f, base.transform.position);
		this.PlayDeathSound();
		Map.DisturbWildLife(this.x, this.y, 40f, 5);
	}

	public Projectile airstrikeProjectile;
}
