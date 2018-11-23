// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletSnakeBroskin : Projectile
{
	protected override void Awake()
	{
		base.Awake();
		this.hitUnits = new List<Unit>(15);
	}

	protected override void HitUnits()
	{
		if (Map.HitUnits(this.firedBy, this.playerNum, this.damageInternal, this.damageType, 8f, this.x, this.y, this.xI, this.yI, false, false, true, ref this.hitUnits))
		{
			this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
			this.penetrateCount++;
			if (this.penetrateCount >= this.maxPenetrations)
			{
				this.DeregisterProjectile();
				UnityEngine.Object.Destroy(base.gameObject);
			}
			this.damageInternal--;
		}
	}

	protected override void HitProjectiles()
	{
		if (Map.HitProjectiles(this.playerNum, this.damageInternal, this.damageType, this.projectileSize, this.x, this.y, this.xI, this.yI, 0.1f))
		{
			this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
		}
	}

	protected override void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime * 0.33f, 0f, 0.0334f);
		this.RunProjectile(this.t);
		this.RunProjectile(this.t);
		this.RunProjectile(this.t);
		this.life -= this.t * 3f;
		if (this.life <= 0f)
		{
			this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
			this.DeregisterProjectile();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	protected override void RunProjectile(float t)
	{
		base.RunProjectile(t);
	}

	protected override void CheckSpawnPoint()
	{
		Collider[] array = Physics.OverlapSphere(new Vector3(this.x, this.y, 0f), 5f, this.groundLayer);
		if (array.Length > 0)
		{
			this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SendMessage("Damage", new DamageObject(this.damageInternal, this.damageType, this.xI, this.yI, this.firedBy));
			}
			if (this.penetrateWalls)
			{
				this.wallPenetrateCount++;
				this.RegisterProjectile();
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		else
		{
			this.RegisterProjectile();
		}
		if (Map.HitUnits(this.firedBy, this, this.playerNum, this.damageInternal * 2, this.damageType, this.projectileSize + ((this.playerNum < 0) ? 0f : (this.projectileSize * 0.5f)), this.x - ((this.playerNum < 0) ? 0f : (this.projectileSize * 0.5f)) * (float)((int)Mathf.Sign(this.xI)), this.y, this.xI, this.yI, true, false))
		{
			this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
		}
	}

	protected override void MakeEffects(bool particles, float x, float y, bool useRayCast, Vector3 hitNormal, Vector3 hitPoint)
	{
		if (!this.hasMadeEffects && particles && this.sparkShowerOnImpact)
		{
			EffectsController.CreateSparkShower(x, y, 30, 5f, 220f, -this.xI * 0.2f, 140f, 0.3f, 0f);
			SortOfFollow.Shake(0.4f, 1f, base.transform.position);
		}
		base.MakeEffects(particles, x, y, useRayCast, hitNormal, hitPoint);
	}

	protected override void Bounce(RaycastHit raycastHit)
	{
		this.MakeEffects(true, raycastHit.point.x + raycastHit.normal.x * 3f, raycastHit.point.y + raycastHit.normal.y * 3f, true, raycastHit.normal, raycastHit.point);
		this.ProjectileApplyDamageToBlock(raycastHit.collider.gameObject, this.damageInternal, this.damageType, this.xI, this.yI);
		if (this.penetrateWalls && this.wallPenetrateCount < this.maxWallPenetrations)
		{
			this.wallPenetrateCount++;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public int maxPenetrations = 3;

	private int penetrateCount;

	public int maxWallPenetrations = 2;

	private int wallPenetrateCount;

	public bool penetrateWalls;

	protected List<Unit> hitUnits;

	public bool sparkShowerOnImpact;
}
