// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class RocketLaunch : Rocket
{
	protected override void MakeEffects(bool particles, float x, float y, bool useRayCast, Vector3 hitNormal, Vector3 point)
	{
		if (this.hasMadeEffects)
		{
			return;
		}
		if (this.life > 0f)
		{
			base.MakeEffects(particles, x, y, useRayCast, this.raycastHit.normal, this.raycastHit.point);
		}
		if (this.seekPlayer)
		{
			float num = 0f;
			float num2 = 0f;
			int num3 = -1;
			if (HeroController.GetRandomPlayerPos(ref num, ref num2, ref num3))
			{
				num += (float)(16 * (RocketLaunch.offsetAmount % 7) * RocketLaunch.offsetSide);
				RocketLaunch.offsetAmount += UnityEngine.Random.Range(1, 3);
				float y2 = SortOfFollow.GetScreenMaxY() + 500f;
				ProjectileController.SpawnProjectileOverNetwork(this.rocketBombardmentPrefab, this.firedBy, Mathf.Round(num / 16f) * 16f, y2, 0f, -300f, false, -1, false, false);
				RocketLaunch.offsetSide *= -1;
			}
		}
		else
		{
			ProjectileController.SpawnProjectileOverNetwork(this.rocketBombardmentPrefab, this.firedBy, Mathf.Round(x / 16f) * 16f - 160f + (float)(UnityEngine.Random.Range(0, 21) * 16), y + 420f, 0f, -300f, false, -1, false, false);
		}
	}

	public Projectile rocketBombardmentPrefab;

	public bool seekPlayer = true;

	private static int offsetSide = 1;

	private static int offsetAmount;
}
