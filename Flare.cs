// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Flare : Projectile
{
	protected override void SetRotation()
	{
		base.SetRotation();
	}

	protected override void MakeEffects(bool particles, float x, float y, bool useRayCast, Vector3 hitNormal, Vector3 point)
	{
		if (this.hasMadeEffects)
		{
			return;
		}
		base.RunDamageBackground(this.t);
		Networking.RPC<float, float, float, int>(PID.TargetAll, new RpcSignature<float, float, float, int>(MapController.BurnGround_Local), this.range * 0.5f, x, y, this.groundLayer, false);
		Map.HitUnits(this.firedBy, this.firedBy, this.playerNum, this.damage, this.damageType, this.range, x, y, this.xI, this.yI, false, false);
		EffectsController.CreateSparkShower(x, y, 6, 3f, 100f, 0f, 60f, 0.6f, 0.33f);
		Vector3 a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke1, x + a.x * this.range * 0.25f, y + 12f + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.3f, a * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, x + a.x * this.range * 0.25f, y + 12f + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.3f, a * this.range * 3f, BloodColor.None);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire1, x, y - 2f, 0f, a * this.range * 0.5f * 7f);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire2, x + a.x * this.range * 0.25f, y - 2f + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 7f);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire3, x + a.x * this.range * 0.25f, y - 2f + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f);
		this.PlayDeathSound();
	}

	protected override void RunProjectile(float t)
	{
		base.RunProjectile(t);
		if (this.hasBounced)
		{
			this.yI -= 500f * t;
		}
		else
		{
			this.yI -= 300f * t;
		}
	}

	protected override void HitUnits()
	{
		float projectileSize = this.projectileSize;
		this.projectileSize = this.hitUnitsSize;
		base.HitUnits();
		this.projectileSize = projectileSize;
	}

	public override void Fire(float x, float y, float xI, float yI, int playerNum, MonoBehaviour FiredBy)
	{
		base.Fire(x, y, xI, yI, playerNum, FiredBy);
		this.lastTrailX = x;
		this.lastTrailY = y;
	}

	protected override void Bounce(RaycastHit raycastHit)
	{
		this.ProjectileApplyDamageToBlock(raycastHit.collider.gameObject, this.damageInternal, this.damageType, this.xI, this.yI);
		if (!this.hasBounced && Mathf.Abs(raycastHit.normal.y) < 0.5f)
		{
			this.hasBounced = true;
			this.life += 0.5f;
			this.xI = -this.xI * UnityEngine.Random.Range(0.2f, 0.25f);
			this.yI = UnityEngine.Random.Range(60f, 80f);
			EffectsController.CreateEffect(this.flickPuff, raycastHit.point.x + raycastHit.normal.x * 3f, raycastHit.point.y + raycastHit.normal.y * 3f);
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.deathSounds, this.soundVolume * 0.5f, base.transform.position, 1.2f);
		}
		else
		{
			this.MakeEffects(true, raycastHit.point.x + raycastHit.normal.x * 3f, raycastHit.point.y + raycastHit.normal.y * 3f, true, raycastHit.normal, raycastHit.point);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	protected virtual void RunSmokeTrail(float t)
	{
		TrailType trailType = this.trailType;
		if (trailType != TrailType.SmokeTrail)
		{
			if (trailType == TrailType.FireTrail)
			{
				float num = this.x - this.lastTrailX;
				float num2 = this.y - this.lastTrailY;
				float num3 = Mathf.Sqrt(num * num + num2 * num2);
				int num4 = (int)(num3 / 3f);
				for (int i = num4 - 1; i >= 0; i--)
				{
					this.lastTrailX = this.x - (float)i * num / (float)num4;
					this.lastTrailY = this.y - (float)i * num2 / (float)num4;
					EffectsController.CreatePlumeParticle(this.lastTrailX - num / num3 * 5f, this.lastTrailY - num2 / num3 * 5f, 4f, 0f, 0f, 0.4f, 1.3f);
					EffectsController.CreateSparkShower(this.x, this.y, 1, 1f, 30f, 0f, 0f, 0.6f, 0.33f);
				}
			}
		}
		else
		{
			this.smokeCounter += t;
			if (this.smokeCounter > 0.0334f)
			{
				this.smokeCounter -= 0.0334f;
				Vector3 a = UnityEngine.Random.insideUnitCircle;
				int num5 = UnityEngine.Random.Range(0, 3);
				if (num5 != 0)
				{
					if (num5 != 1)
					{
						EffectsController.CreateEffect(this.smoke2, this.x - Mathf.Sign(this.xI) * 8f, this.y - 6f, 0f, a * 45f, BloodColor.None);
					}
					else
					{
						EffectsController.CreateEffect(this.smoke2, this.x - Mathf.Sign(this.xI) * 8f, this.y - 6f, 0f, a * 45f, BloodColor.None);
					}
				}
				else
				{
					EffectsController.CreateEffect(this.smoke1, this.x - Mathf.Sign(this.xI) * 8f, this.y - 6f, 0f, a * 45f, BloodColor.None);
				}
			}
		}
	}

	protected override void Update()
	{
		base.Update();
		this.RunSmokeTrail(this.t);
		this.SetRotation();
	}

	protected override void RunDamageBackground(float t)
	{
	}

	public float range = 4f;

	public float blastForce = 4f;

	public float hitUnitsSize = 7f;

	public FlickerFader fire1;

	public FlickerFader fire2;

	public FlickerFader fire3;

	public Puff smoke1;

	public Puff smoke2;

	public Puff smoke3;

	public Puff explosion;

	public TrailType trailType = TrailType.SmokeTrail;

	protected bool hasBounced;

	protected float smokeCounter;

	protected float lastTrailX;

	protected float lastTrailY;
}
