// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Rocket : Projectile
{
	protected override void MakeSparkShower(float xPos, float yPos, float showerXI, float showerYI)
	{
	}

	protected override void MakeEffects(bool particles, float x, float y, bool useRayCast, Vector3 hitNormal, Vector3 point)
	{
		if (this.hasMadeEffects)
		{
			return;
		}
		if (base.IsMine)
		{
			MapController.DamageGround(this.firedBy, this.damage, this.damageType, this.range, x, y, null);
			Map.ShakeTrees(x, y, 128f, 48f, 100f);
			Map.ExplodeUnits(this.firedBy, this.damage, this.damageType, this.range * 1.5f, this.range, x, y, this.blastForce * 20f, this.blastForceExtraY, this.playerNum, false, true);
			Map.DamageDoodads(this.damageInternal, x, y, 0f, 0f, this.range * 0.6f, this.playerNum);
			Map.HitProjectiles(this.playerNum, this.damage, this.damageType, this.range, x, y, 0f, 0f, 0.25f);
		}
		EffectsController.CreateSparkShower(x, y, 90, 3f, 160f, 0f, 60f, 0.6f, 0.33f);
		Vector3 a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke1, x + a.x * this.range * 0.25f, y + 12f + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.3f, a * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke1, x + a.x * this.range * 0.25f, y + 12f + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.2f, UnityEngine.Random.insideUnitSphere * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, x + a.x * this.range * 0.25f, y + 12f + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.3f, a * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, x + a.x * this.range * 0.25f, y + 12f + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.4f, a * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.explosion, x + a.x * this.range * 0.05f, y + a.y * this.range * 0.05f, 0f, a * this.range * 1f, BloodColor.None);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire1, x + a.x * this.range * 0.25f, y + 8f + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 7f);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire2, x + a.x * this.range * 0.25f, y + 8f + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 7f);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire3, x + a.x * this.range * 0.25f, y + 8f + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f);
		SortOfFollow.Shake(1f, base.transform.position);
		this.PlayDeathSound();
		Map.DisturbWildLife(x, y, 80f, 5);
		if (this.isHeavy)
		{
			Sound.SuddenLowPass(0.4f, base.transform.position);
			FullScreenFlashEffect.FlashHot(0.7f, base.transform.position);
		}
	}

	protected override bool ReflectProjectile(RaycastHit raycastHit)
	{
		UnityEngine.Object.Destroy(base.gameObject);
		this.MakeEffects(false, this.x, this.y, false, raycastHit.normal, raycastHit.point);
		return false;
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, float damageDelay, int newPlayerNum)
	{
		if (this.isDamageable)
		{
			this.health -= damage;
		}
		if (this.health <= 0)
		{
			this.playerNum = newPlayerNum;
			if (damageDelay <= 0f || damage > 100)
			{
				this.Death();
			}
			else
			{
				base.Invoke("Death", damageDelay);
			}
		}
		this.hurtCounter = 0.0334f;
		base.gameObject.GetComponent<SpriteSM>().SetColor(Color.red);
	}

	public override void Death()
	{
		base.Death();
	}

	public override void Fire(float x, float y, float xI, float yI, int playerNum, MonoBehaviour FiredBy)
	{
		base.Fire(x, y, xI, yI, playerNum, FiredBy);
		this.lastTrailX = x;
		this.lastTrailY = y;
	}

	protected override void RegisterProjectile()
	{
		base.RegisterProjectile();
		if (this.isDamageable)
		{
			Map.RegisterDamageableProjectile(this);
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
					EffectsController.CreatePlumeParticle(this.lastTrailX - num / num3 * 5f, this.lastTrailY - num2 / num3 * 5f, this.zOffset + 0.1f, 4f, 0f, 0f, 0.7f, 1f);
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
		if (this.hurtCounter > 0f)
		{
			this.hurtCounter -= this.t;
			if (this.hurtCounter <= 0f)
			{
				base.gameObject.GetComponent<SpriteSM>().SetColor(Color.white);
			}
		}
	}

	public float range = 28f;

	public float blastForce = 20f;

	public float blastForceExtraY = 300f;

	public FlickerFader fire1;

	public FlickerFader fire2;

	public FlickerFader fire3;

	public Puff smoke1;

	public Puff smoke2;

	public Puff smoke3;

	public Puff explosion;

	public TrailType trailType = TrailType.SmokeTrail;

	public bool isHeavy;

	protected float hurtCounter;

	protected float smokeCounter;

	protected float lastTrailX;

	protected float lastTrailY;
}
