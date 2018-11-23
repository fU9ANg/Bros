// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class HiddenExplosives : MonoBehaviour
{
	protected void MakeEffects(float x, float y)
	{
		MapController.DamageGround(this, this.damage, DamageType.Explosion, this.range, x, y, null);
		Map.ExplodeUnits(this, this.damage, DamageType.Explosion, this.range * 1.2f, this.range, x, y, this.blastForce * 20f, 300f, -1, false, false);
		MapController.BurnUnitsAround_NotNetworked(SingletonMono<MapController>.Instance, -1, 1, this.range * 4f, x, y, true, false);
		EffectsController.CreatePlumes(x, y, UnityEngine.Random.Range(3, 5), 8f, 450f, 0f, 0f);
		EffectsController.CreateShrapnel(this.shrapnel, x, y, 3f, 160f, 20f, 0f, 60f);
		Vector3 a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke1, x + a.x * this.range * 0.25f, y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke1, x + a.x * this.range * 0.25f, y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.2f, UnityEngine.Random.insideUnitSphere * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, x + a.x * this.range * 0.25f, y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, x + a.x * this.range * 0.25f, y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.explosion, x + a.x * this.range * 0.25f, y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.explosionSmall, x + a.x * this.range * 0.5f, y + a.y * this.range * 0.5f, UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.explosionSmall, x + a.x * this.range * 0.5f, y + a.y * this.range * 0.5f, UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire1, x + a.x * this.range * 0.25f, y + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire2, x + a.x * this.range * 0.25f, y + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire3, x + a.x * this.range * 0.25f, y + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f);
		SortOfFollow.Shake(1f, new Vector3(x, y, 0f));
		this.PlayDeathSound();
		Map.DisturbWildLife(x, y, 80f, -1);
	}

	protected virtual void PlayDeathSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.deathSounds, 0.6f, base.transform.position);
		}
	}

	public bool Explode()
	{
		if (!this.exploded)
		{
			this.MakeEffects(base.transform.position.x, base.transform.position.y);
			this.exploded = true;
			HeroController.RemoveHiddenExplosives(this);
			return true;
		}
		return false;
	}

	protected void Awake()
	{
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
	}

	protected void Start()
	{
		HeroController.RegisterHiddenExplosives(this);
		base.gameObject.SetActive(false);
	}

	public float range = 58f;

	public float blastForce = 32f;

	public int damage = 20;

	public FlickerFader fire1;

	public FlickerFader fire2;

	public FlickerFader fire3;

	public Puff smoke1;

	public Puff smoke2;

	public Puff explosion;

	public Puff explosionSmall;

	public Shrapnel shrapnel;

	public SoundHolder soundHolder;

	protected float smokeCounter;

	private Sound sound;

	protected bool exploded;

	protected LayerMask groundLayer;
}
