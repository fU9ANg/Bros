// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class BrocheteMachete : Projectile
{
	protected override void TryHitUnitsAtSpawn()
	{
		this.HitUnits();
	}

	protected override void Start()
	{
		base.Start();
		this.lastTrailPos = base.transform.position;
	}

	protected override void MakeEffects(bool particles, float x, float y, bool useRayCast, Vector3 hitNormal, Vector3 hitPoint)
	{
		if (!this.hasMadeEffects && useRayCast)
		{
			this.PlayDeathSound();
		}
		base.MakeEffects(particles, x, y, useRayCast, hitNormal, hitPoint);
	}

	protected override void HitUnits()
	{
		if (this.penetrationsCount < this.maxPenetrations)
		{
			Unit unit = Map.ImpaleUnits(this.firedBy, this.playerNum, 5f, this.x, this.y, true, true, ref this.hitUnits);
			if (unit != null)
			{
				if (unit.IsHeavy())
				{
					unit.Damage(this.damageInternal, DamageType.Melee, this.xI, this.yI, (int)Mathf.Sign(this.xI), this.firedBy, this.x, this.y);
					this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
					UnityEngine.Object.Destroy(base.gameObject);
				}
				else
				{
					this.hitUnits.Add(unit);
					unit.Blind();
					unit.Impale(base.transform, this.damageInternal, this.xI, this.yI, 0f);
					Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.specialSounds, 0.4f, base.transform.position);
					this.penetrationsCount++;
					SortOfFollow.Shake(0.3f);
					EffectsController.CreateBloodParticles(BloodColor.Red, this.x, this.y, 4, 4f, 5f, 60f, this.xI * 0.2f, this.yI * 0.5f + 40f);
					EffectsController.CreateMeleeStrikeEffect(this.x, this.y, this.xI * 0.2f, this.yI * 0.5f + 24f);
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
				}
			}
		}
	}

	protected void RunTrail()
	{
		Vector3 a = base.transform.position - this.lastTrailPos;
		float magnitude = a.magnitude;
		if (magnitude > this.trailDist)
		{
			SpriteSM spriteSM = UnityEngine.Object.Instantiate(this.macheteSpritePrefab, base.transform.position, base.transform.rotation) as SpriteSM;
			spriteSM.transform.localScale = base.transform.localScale;
			this.lastTrailPos += a * this.trailDist / magnitude;
		}
	}

	protected override void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.superMachete)
		{
			this.t *= 0.3334f;
			this.RunProjectile(this.t);
			this.RunTrail();
			this.RunProjectile(this.t);
			this.RunTrail();
			this.RunProjectile(this.t);
			this.RunTrail();
			this.t *= 3f;
		}
		else
		{
			this.RunProjectile(this.t);
		}
		this.RunLife();
		if (this.hitUnits.Count > 0)
		{
			this.bloodTime += this.t;
			if (this.bloodTime > 0.066f)
			{
				this.bloodTime -= 0.0667f;
				EffectsController.CreateBloodParticles(BloodColor.Red, this.x, this.y, 1 + UnityEngine.Random.Range(0, 3), 3f, 3f, 20f, this.xI * 0.2f, this.yI * 0.5f + 60f);
			}
		}
	}

	protected override void MoveProjectile()
	{
		base.MoveProjectile();
		foreach (Unit unit in this.hitUnits.ToArray())
		{
			if (unit != null && (Mathf.Abs(unit.x - (this.x + Mathf.Sign(this.xI) * 3f)) > 12f || Mathf.Abs(unit.y + 8f - this.y) > 11f))
			{
				unit.Unimpale(this.damageInternal, DamageType.Melee, this.xI, this.yI);
				unit.ReduceDeathTimer(this.playerNum, 0.01f);
				UnityEngine.Debug.Log("Reduce Death Timer ");
				this.hitUnits.Remove(unit);
				this.penetrationsCount--;
			}
		}
	}

	protected override void DeregisterProjectile()
	{
		this.ClearHitUnits();
		base.DeregisterProjectile();
	}

	protected void ClearHitUnits()
	{
		foreach (Unit unit in this.hitUnits)
		{
			if (unit != null)
			{
				unit.Unimpale(this.damageInternal * 2, DamageType.Melee, this.xI, this.yI);
				unit.ReduceDeathTimer(this.playerNum, 0.01f);
			}
		}
		this.hitUnits.Clear();
	}

	public int maxPenetrations = 3;

	protected int penetrationsCount;

	protected float bloodTime;

	public bool superMachete;

	public SpriteSM macheteSpritePrefab;

	public float trailDist = 12f;

	protected List<Unit> hitUnits = new List<Unit>();

	protected Vector3 lastTrailPos = Vector3.zero;

	public float dragUnitsSpeedM = 0.9f;
}
