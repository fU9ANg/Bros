// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Mine : BroforceObject
{
	private void Start()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
		NID nid = Registry.AllocateDeterministicID();
		Registry.RegisterGameObject(ref nid, base.gameObject);
	}

	protected virtual void Update()
	{
		if (this.detonating)
		{
			this.DetonationProcess();
		}
	}

	public void Detonate(bool immediate = false)
	{
		if (!this.detonated)
		{
			this.StartDetonationProcess();
			this.PlayDetonateSound();
			this.detonated = true;
		}
		if (immediate && !this.exploded)
		{
			this.Explode(5);
			this.PlayDetonateSound();
		}
	}

	protected void StartDetonationProcess()
	{
		this.sprite.lowerLeftPixel.x = 16f;
		this.sprite.SetLowerLeftPixel(this.sprite.lowerLeftPixel);
		this.sprite.SetOffset(new Vector3(0f, 0f, -20f));
		this.detonating = true;
	}

	protected void DetonationProcess()
	{
		this.detonationTime -= Time.deltaTime;
		if (this.detonationTime <= 0f)
		{
			this.Explode(-10);
			this.detonating = false;
		}
	}

	private void Explode(int playerNum)
	{
		if (this.exploded)
		{
			return;
		}
		Randomf randomf = new Randomf(this.randomSeed);
		this.exploded = true;
		Vector3 a = randomf.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, randomf.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = randomf.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, randomf.value * 0.5f, a * this.range * 3f, BloodColor.None);
		EffectsController.CreateEffect(this.smoke1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, randomf.value * 0.2f, randomf.insideUnitSphere * this.range * 3f, BloodColor.None);
		a = randomf.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, randomf.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = randomf.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, randomf.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = randomf.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, randomf.value * 0.2f, a * this.range * 3f, BloodColor.None);
		a = randomf.insideUnitCircle;
		EffectsController.CreateEffect(this.explosionBig, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, randomf.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = randomf.insideUnitCircle;
		EffectsController.CreateEffect(this.explosionBig, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, randomf.value * 0.5f, a * this.range * 0.5f * 3f, BloodColor.None);
		a = randomf.insideUnitCircle;
		EffectsController.CreateEffect(this.explosionBig, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0f, a * this.range * 0.3f * 3f, BloodColor.None);
		a = randomf.insideUnitCircle;
		EffectsController.CreateEffect(this.explosion, this.x + a.x * this.range * 0.15f, this.y + a.y * this.range * 0.15f, 0f, a * this.range * 0.5f * 3f, BloodColor.None);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + randomf.value * 0.2f, a * this.range * 3f);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + randomf.value * 0.2f, a * this.range * 3f);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire3, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + randomf.value * 0.2f, a * this.range * 3f);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + randomf.value * 0.2f, a * this.range * 3f);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + randomf.value * 0.2f, a * this.range * 3f);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire3, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + randomf.value * 0.2f, a * this.range * 3f);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.15f + randomf.value * 0.4f, a * this.range * 3f);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.15f + randomf.value * 0.4f, a * this.range * 4f);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire3, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.15f + randomf.value * 0.4f, a * this.range * 4f);
		a = global::Math.RandomPointOnCircle();
		if (a.y < 0f)
		{
			a.y *= -1f;
		}
		EffectsController.CreateEffect(this.fire1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + randomf.value * 0.3f, a * this.range * 4f);
		SortOfFollow.Shake(1f);
		Map.DisturbWildLife(this.x, this.y, 120f, 5);
		MapController.DamageGround(this, 12, DamageType.Explosion, this.range, this.x, this.y, null);
		Map.ShakeTrees(this.x, this.y, 128f, 48f, 80f);
		Map.ExplodeUnits(this, 20, DamageType.OutOfBounds, this.range * 2f, this.range * 2f, this.x, this.y - 6f, 200f, 300f, playerNum, false, false);
		base.gameObject.SetActive(false);
	}

	protected virtual void PlayDetonateSound()
	{
		this.sound = Sound.GetInstance();
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.detonateClip, this.soundVolume, base.transform.position);
		}
	}

	public bool exploded;

	public bool detonated;

	protected bool detonating;

	public AudioClip detonateClip;

	private float soundVolume = 0.5f;

	public FlickerFader fire1;

	public FlickerFader fire2;

	public FlickerFader fire3;

	public Puff smoke1;

	public Puff smoke2;

	public Puff explosion;

	public Puff explosionBig;

	private float range = 20f;

	public int randomSeed = -1;

	private float detonationTime = 0.3f;

	private SpriteSM sprite;

	private Sound sound;
}
