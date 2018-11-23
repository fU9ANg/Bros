// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class PlasmaBullet : NoisyCricket
{
	protected override void Awake()
	{
		this.puffLife = 0.12f;
		base.Awake();
		if (++PlasmaBullet.upsideDownCount % 2 == 1)
		{
			this.upsideDownWave = true;
		}
	}

	protected override void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime * 0.33f, 0f, 0.0334f);
		if (!this.hasHitUnit)
		{
			this.RunProjectile(this.t);
		}
		if (!this.hasHitUnit)
		{
			this.RunProjectile(this.t);
		}
		if (!this.hasHitUnit)
		{
			this.RunProjectile(this.t);
		}
		this.life -= this.t * 3f;
		if (this.life <= 0f)
		{
			this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
			this.DeregisterProjectile();
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.RunBeam();
	}

	protected override void RunBeam()
	{
		float f = this.x - this.lastBeamPos.x;
		while (Mathf.Abs(f) + 7f > 16f)
		{
			Puff puff = UnityEngine.Object.Instantiate(this.beamPuff, new Vector3(this.lastBeamPos.x + Mathf.Sign(f) * 8f, this.lastBeamPos.y, 0f), Quaternion.identity) as Puff;
			if (this.upsideDownWave)
			{
				puff.transform.localScale = new Vector3(1f, -1f, 1f);
			}
			float frameRate = this.puffLife / 16f;
			puff.frameRate = frameRate;
			this.puffLife += 0.03f;
			this.lastBeamPos = new Vector3(this.lastBeamPos.x + Mathf.Sign(f) * 16f, this.y, 0f);
			f = this.x - this.lastBeamPos.x;
		}
	}

	protected override void HitUnits()
	{
		float xI = this.xI;
		this.xI *= 0.3333334f;
		if (this.reversing)
		{
			if (Map.HitLivingUnits(this, this.playerNum, this.damageInternal, this.damageType, this.projectileSize, this.projectileSize / 2f, this.x, this.y, this.xI, this.yI, false, false))
			{
				this.hasHitUnit = true;
				this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
				UnityEngine.Object.Destroy(base.gameObject);
				Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.hitSounds, 0.5f, base.transform.position, 0.95f + UnityEngine.Random.value * 0.15f);
			}
		}
		else if (Map.HitUnits(this.firedBy, this.firedBy, this.playerNum, this.damageInternal, this.damageType, this.projectileSize, this.projectileSize / 2f, this.x, this.y, this.xI, this.yI, false, false, true))
		{
			this.hasHitUnit = true;
			this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
			UnityEngine.Object.Destroy(base.gameObject);
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.hitSounds, 0.5f, base.transform.position, 0.95f + UnityEngine.Random.value * 0.15f);
		}
		this.xI = xI;
	}

	protected override void RunCricketeffects(float t)
	{
		this.smokeCounter += t;
		if (this.smokeCounter > 0.00223f)
		{
			this.smokeCounter -= 0.00667f;
			this.smokeCount++;
			if (this.smokeCount % 2 == 0)
			{
				EffectsController.CreateShrapnel(this.sparkBlue1, this.x, this.y, 0.1f, 2f, 1f, 2f, 1f);
			}
			else
			{
				EffectsController.CreateShrapnel(this.sparkBlue2, this.x, this.y, 0.1f, 2f, 1f, 2f, 1f);
			}
		}
	}

	protected override void MakeEffects(bool particles, float x, float y, bool useRayCast, Vector3 hitNormal, Vector3 point)
	{
		if (!this.hasHitUnit)
		{
			Map.HitLivingUnits(this.firedBy, this.playerNum, this.damageInternal, this.damageType, 16f, 16f, x, y, 0f, 0f, false, false);
		}
		EffectsController.CreateShrapnel(this.sparkWhite1, x, y, 2f, 130f, 8f, this.xI * 0.2f, 50f);
		this.hasHitUnit = true;
		EffectsController.CreateWhiteFlashPopSmall(x, y);
	}

	private static int upsideDownCount;

	private bool upsideDownWave;

	public Shrapnel sparkWhite1;

	protected bool hasHitUnit;
}
