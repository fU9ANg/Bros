// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class HeavyShell : Projectile
{
	protected override void MakeEffects(bool particles, float x, float y, bool useRayCast, Vector3 hitNormal, Vector3 point)
	{
		if (this.hasMadeEffects)
		{
			return;
		}
		MapController.DamageGround(this.firedBy, this.damage, this.damageType, this.range, x, y, null);
		Map.ExplodeUnits(this.firedBy, this.damage, this.damageType, this.range, this.range, x, y, 350f, 300f, this.playerNum, false, false);
		Map.DamageDoodads(this.damageInternal, x, y, 0f, 0f, this.range * 0.6f, this.playerNum);
		Map.ShakeTrees(x, y, 256f, 64f, 144f);
		EffectsController.CreateHugeExplosion(x, y, 24f, 24f, 120f, 1f, 50f, 0.6f, 0.7f, 4, 120, 300f, 90f, (!this.isHeavy) ? 0f : 0.5f, (!this.isHeavy) ? 0.2f : 0.5f);
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
			if (damageDelay <= 0f)
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

	public override void Fire(float x, float y, float xI, float yI, int playerNum, MonoBehaviour FiredBy)
	{
		base.Fire(x, y, xI, yI, playerNum, FiredBy);
		this.lastTrailX = x;
		this.lastTrailY = y;
		this.dipCounter = this.dipCounterStart;
	}

	protected override void RunProjectile(float t)
	{
		this.dipCounter += t;
		if (this.dipCounter < 0.1f)
		{
			this.RunSmokeTrail();
		}
		else if (this.dipCounter < 0.35f)
		{
			this.RunSmokeTrail();
			float num = (this.dipCounter - 0.1f) / 0.25f;
			this.xI *= 1f - t * 1.5f * num;
			this.yI *= 1f - t * 1.5f * num;
		}
		else
		{
			float num2 = Mathf.Clamp(1f - (this.dipCounter - 0.35f) / 0.4f, 0f, 1f);
			this.xI *= 1f - t * 1.5f * num2;
			this.yI *= 1f - t * 1.5f * num2;
			this.yI -= 600f * t * (1f - num2);
			this.SetRotation();
		}
		base.RunProjectile(t);
	}

	protected virtual void RunSmokeTrail()
	{
		float num = this.x - this.lastTrailX;
		float num2 = this.y - this.lastTrailY;
		float num3 = Mathf.Sqrt(num * num + num2 * num2);
		int num4 = (int)(num3 / 3f);
		for (int i = num4 - 1; i >= 0; i--)
		{
			this.lastTrailX = this.x - (float)i * num / (float)num4;
			this.lastTrailY = this.y - (float)i * num2 / (float)num4;
			EffectsController.CreatePlumeParticle(this.lastTrailX - num / num3 * 5f, this.lastTrailY - num2 / num3 * 5f, 4f, 0f, 0f, 0.7f, 1f);
		}
	}

	protected float hurtCounter;

	public float range = 28f;

	protected float dipCounter;

	public float dipCounterStart;

	public bool isHeavy = true;

	protected float lastTrailX;

	protected float lastTrailY;
}
