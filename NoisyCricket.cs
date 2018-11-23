// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class NoisyCricket : Projectile
{
	protected override void Start()
	{
		base.Start();
		this.lastBeamPos = new Vector3(this.x, this.y, 0f);
	}

	protected override void MakeEffects(bool particles, float x, float y, bool useRayCast, Vector3 hitNormal, Vector3 point)
	{
		if (useRayCast)
		{
			UnityEngine.Object.Instantiate(this.ballPuff, new Vector3(point.x, point.y, 0f), Quaternion.identity);
			UnityEngine.Object.Instantiate(this.wobblePuff, new Vector3(point.x, point.y, 0f), Quaternion.identity);
		}
		else
		{
			UnityEngine.Object.Instantiate(this.ballPuff, new Vector3(x, y, 0f), Quaternion.identity);
			UnityEngine.Object.Instantiate(this.wobblePuff, new Vector3(x, y, 0f), Quaternion.identity);
		}
		MapController.DamageGround(this.firedBy, this.damage, this.damageType, this.range, x, y, null);
		Map.ShakeTrees(x, y, 100f, 48f, 64f);
		Map.ExplodeUnits(this.firedBy, this.damage, this.damageType, this.range, this.range * 0.7f, x, y, this.blastForce * 40f, 300f, this.playerNum, false, true);
		MapController.BurnUnitsAround_NotNetworked(this.firedBy, this.playerNum, 1, this.range * 1.4f, x, y, true, true);
		EffectsController.CreateShrapnel(this.sparkBlue1, x, y, 3f, 160f, 5f, 0f, 60f);
		EffectsController.CreateShrapnel(this.sparkBlue2, x, y, 3f, 200f, 5f, 0f, 90f);
		Vector3 a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke1, x + a.x * this.range * 0.25f, y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke1, x + a.x * this.range * 0.25f, y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.2f, UnityEngine.Random.insideUnitSphere * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, x + a.x * this.range * 0.25f, y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, x + a.x * this.range * 0.25f, y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire1, x + a.x * this.range * 0.4f, y + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire2, x + a.x * this.range * 0.4f, y + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire3, x + a.x * this.range * 0.4f, y + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire1, x + a.x * this.range * 0.4f, y + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.3f, a * this.range * 0.5f * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire2, x + a.x * this.range * 0.4f, y + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.3f, a * this.range * 0.5f * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire3, x + a.x * this.range * 0.4f, y + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.3f, a * this.range * 0.5f * 3f, BloodColor.None);
		if (this.life > 0f)
		{
			SortOfFollow.Shake(0.9f, 5f, base.transform.position);
		}
		else
		{
			SortOfFollow.Shake(0.25f, 2.5f, base.transform.position);
		}
		this.PlayDeathSound();
		Map.DisturbWildLife(x, y, 2f, 5);
		Map.DamageDoodads(this.damageInternal, x, y, 0f, 0f, this.range, this.playerNum);
	}

	protected override void PlayDeathSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.deathSounds, this.soundVolume, base.transform.position, 1f, false, false);
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
		this.RunBeam();
	}

	protected virtual void RunBeam()
	{
		float f = this.x - this.lastBeamPos.x;
		while (Mathf.Abs(f) + 7f > 16f)
		{
			Puff puff = UnityEngine.Object.Instantiate(this.beamPuff, new Vector3(this.lastBeamPos.x + 8f, this.lastBeamPos.y, 0f), Quaternion.identity) as Puff;
			float frameRate = this.puffLife / 8f;
			puff.frameRate = frameRate;
			this.puffLife += 0.05f;
			this.lastBeamPos = new Vector3(this.lastBeamPos.x + Mathf.Sign(f) * 16f, this.y, 0f);
			f = this.x - this.lastBeamPos.x;
		}
	}

	protected override bool ReflectProjectile(RaycastHit raycastHit)
	{
		this.MakeEffects(false, this.x, this.y, false, raycastHit.normal, raycastHit.point);
		this.DeregisterProjectile();
		UnityEngine.Object.Destroy(base.gameObject);
		return false;
	}

	protected override void RunProjectile(float t)
	{
		base.RunProjectile(t);
		this.RunCricketeffects(t * 0.5f);
		this.RunCricketeffects(t * 0.5f);
	}

	protected virtual void RunCricketeffects(float t)
	{
		this.smokeCounter += t;
		if (this.smokeCounter > 0.00223f)
		{
			this.smokeCounter -= 0.00667f;
			this.smokeCount++;
			if (this.smokeCount % 2 == 0)
			{
				EffectsController.CreateShrapnel(this.sparkBlue1, this.x, this.y, 0.1f, 7f, 1f, 1f, 2f);
			}
			else
			{
				EffectsController.CreateShrapnel(this.sparkBlue2, this.x, this.y, 0.1f, 7f, 1f, 1f, 2f);
			}
		}
	}

	public float range = 35f;

	public float blastForce = 20f;

	public Puff fire1;

	public Puff fire2;

	public Puff fire3;

	public Puff smoke1;

	public Puff smoke2;

	public Puff explosion;

	public Shrapnel sparkBlue1;

	public Shrapnel sparkBlue2;

	public Puff beamPuff;

	public Puff ballPuff;

	public DistortionGrow wobblePuff;

	protected float puffLife = 0.16f;

	protected float smokeCounter;

	protected Vector3 lastBeamPos = Vector3.zero;

	protected int smokeCount;
}
