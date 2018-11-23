// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BulletTimeBro : BulletRambro
{
	public override void Fire(float x, float y, float xI, float yI, int playerNum, MonoBehaviour FiredBy)
	{
		base.Fire(x, y, xI, yI, playerNum, FiredBy);
		this.runProjectileCount = UnityEngine.Random.Range(1, 5);
	}

	protected override void Update()
	{
		if (Time.timeScale > 0f)
		{
			this.t = Mathf.Clamp(Time.deltaTime / (0.3f + Time.timeScale * 0.7f), 0f, 0.0334f);
		}
		else
		{
			this.t = 0f;
		}
		int num = 0;
		while (num < this.runProjectileCount && this.life > 0f && !this.hasMadeEffects)
		{
			this.RunProjectile(this.t);
			this.life -= this.t;
			num++;
		}
		if (this.life <= 0f)
		{
			this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
			this.DeregisterProjectile();
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.runProjectileCount = 4;
	}

	protected override bool HitWalls()
	{
		return base.HitWalls();
	}

	protected override void MakeEffects(bool particles, float x, float y, bool useRayCast, Vector3 hitNormal, Vector3 hitPoint)
	{
		if (this.hasMadeEffects)
		{
			return;
		}
		if (useRayCast)
		{
			if (particles)
			{
				EffectsController.CreateSparkShower(hitPoint.x + hitNormal.x * 5f, hitPoint.y + hitNormal.y * 5f, this.sparkCount, 2f, 240f, hitNormal.x * 120f, 40f + hitNormal.y * 60f, 0.2f, 0.5f);
			}
			EffectsController.CreateProjectilePopWhiteEffect(hitPoint.x + hitNormal.x * 3f, hitPoint.y + hitNormal.y * 3f);
		}
		else
		{
			if (particles)
			{
				EffectsController.CreateSparkShower(x, y, 10, 2f, 240f, -this.xI * 0.6f, 60f, 0.2f, 0.5f);
			}
			EffectsController.CreateProjectilePopWhiteEffect(x, y);
		}
		UnityEngine.Object.Instantiate(this.wobblePuffPrefab, new Vector3(x, y, 0f), Quaternion.identity);
		if (!particles)
		{
			Map.DamageDoodads(this.damageInternal, x, y, this.xI, this.yI, 8f, this.playerNum);
		}
		this.hasMadeEffects = true;
	}

	protected override bool ReflectProjectile(RaycastHit raycastHit)
	{
		bool flag = base.ReflectProjectile(raycastHit);
		if (flag)
		{
			this.hasMadeEffects = false;
		}
		return flag;
	}

	public DistortionGrow wobblePuffPrefab;

	protected int runProjectileCount = 4;
}
