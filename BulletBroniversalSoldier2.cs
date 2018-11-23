// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletBroniversalSoldier2 : Projectile
{
	protected override void Awake()
	{
		base.Awake();
	}

	protected override void HitUnits()
	{
		if (this.hitUnits.Count == 0)
		{
			Unit unit = Map.ImpaleUnits(this.firedBy, this.playerNum, 5f, this.x, this.y, false, true, ref this.hitUnits);
			if (unit != null)
			{
				if (unit.IsHeavy())
				{
					unit.Damage(this.damageInternal * 2, this.damageType, this.xI, this.yI, (int)Mathf.Sign(this.xI), this.firedBy, this.x, this.y);
					this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
					UnityEngine.Object.Destroy(base.gameObject);
				}
				else
				{
					this.penetrateCount++;
					this.hitUnits.Add(unit);
					unit.Blind();
					unit.Impale(base.transform, this.damageInternal, this.xI, this.yI, 0f);
					Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.specialSounds, 0.4f, base.transform.position);
					SortOfFollow.Shake(0.2f);
					EffectsController.CreateBloodParticles(BloodColor.Red, this.x, this.y, 4, 4f, 5f, 60f, this.xI * 0.2f, this.yI * 0.5f + 40f);
					EffectsController.CreateProjectilePopEffect(this.x, this.y);
					if (this.xI > 0f)
					{
						if (unit.x < this.x + 3f)
						{
							unit.x = this.x + 3f;
						}
					}
					else if (this.xI < 0f && unit.x > this.x - 3f)
					{
						unit.x = this.x - 3f;
					}
					if (this.life > 0.166f)
					{
						this.life = 0.166f;
					}
				}
			}
			else if (Map.HitUnits(this.firedBy, this.firedBy, this.playerNum, this.damageInternal, this.damageType, 4f, 4f, this.x, this.y, this.xI, this.yI, false, false, true))
			{
				this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
				this.DeregisterProjectile();
				UnityEngine.Object.Destroy(base.gameObject);
			}
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
		this.t = Mathf.Clamp(Time.deltaTime * 0.5f, 0f, 0.0334f);
		this.RunProjectile(this.t);
		this.RunProjectile(this.t);
		this.life -= this.t * 2f;
		if (this.life <= 0f)
		{
			this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
			this.DeregisterProjectile();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	protected override void MoveProjectile()
	{
		base.MoveProjectile();
		foreach (Unit unit in this.hitUnits.ToArray())
		{
			if (unit != null && (Mathf.Abs(unit.x - (this.x + Mathf.Sign(this.xI) * 3f)) > 12f || Mathf.Abs(unit.y + 8f - this.y) > 11f))
			{
				unit.Unimpale(this.damageInternal, this.damageType, this.xI * 3f, this.yI);
				unit.ReduceDeathTimer(this.playerNum, 0.01f);
				this.hitUnits.Remove(unit);
				this.Death();
			}
		}
	}

	protected override void OnDestroy()
	{
		this.UnimpaleUnits();
		base.OnDestroy();
	}

	protected void UnimpaleUnits()
	{
		if (this.hitUnits.Count > 0)
		{
			foreach (Unit unit in this.hitUnits.ToArray())
			{
				unit.Unimpale(this.damageInternal, this.damageType, this.xI * 3f, this.yI);
				unit.ReduceDeathTimer(this.playerNum, 0.01f);
				this.hitUnits.Remove(unit);
			}
		}
	}

	protected override void TryHitUnitsAtSpawn()
	{
		this.HitUnits();
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

	private int penetrateCount;

	public int maxWallPenetrations = 2;

	private int wallPenetrateCount;

	public bool penetrateWalls;

	protected List<Unit> hitUnits = new List<Unit>();

	public bool sparkShowerOnImpact;

	public float dragUnitsSpeedM = 0.9f;
}
