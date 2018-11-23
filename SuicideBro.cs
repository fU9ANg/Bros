// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SuicideBro : TestVanDammeAnim
{
	protected override void ActivateGun()
	{
	}

	protected override void Awake()
	{
		base.Awake();
		this.isHero = true;
	}

	protected override void Start()
	{
		base.Start();
		base.GetComponent<Renderer>().material = this.materials[this.playerNum];
	}

	protected override void Update()
	{
		this.invulnerable = false;
		this.DeactivateGun();
		this.primeDelay -= this.t;
		if (this.primed)
		{
			this.panicCounter += this.t;
			this.minimumPrimeDelay -= this.t;
			if (this.panicCounter > 0.0455f)
			{
				this.panicCounter -= 0.04445f;
				EffectsController.CreateFireSparks(this.x + base.transform.localScale.x * 4f, this.y + 10f, 1, 3f, 4f, this.xI * 0.1f, 16f, 0.6f);
			}
			this.primeCounter -= this.t;
			if (this.primeCounter < 0f)
			{
				this.Explode();
			}
		}
		base.Update();
	}

	protected override void CheckInput()
	{
		base.CheckInput();
		if (this.fire && !this.wasFire && this.primeDelay < 0f)
		{
			if (!this.primed)
			{
				this.PlaySpecialSound(0.6f);
				this.primed = true;
			}
			else if (this.minimumPrimeDelay < 0f)
			{
				if (this.primeCounter > 0.11f)
				{
					this.sound.PlaySoundEffectAt(this.triggerClip, 0.5f, base.transform.position);
					this.primeCounter = 0.1f;
				}
			}
			else
			{
				this.sound.PlaySoundEffectAt(this.triggerClip, 0.5f, base.transform.position);
				this.primeCounter = 0.1f + this.minimumPrimeDelay;
				this.minimumPrimeDelay = 0f;
			}
		}
		if (this.primed && !this.right && !this.left)
		{
			this.right = this.wasRight;
			this.left = this.wasLeft;
			if (!this.right && !this.left)
			{
				if (UnityEngine.Random.value > 0.5f)
				{
					this.right = true;
				}
				else
				{
					this.left = true;
				}
			}
		}
		this.wasHighFive = false; this.special = (this.wasSpecial = (this.highFive = (this.wasHighFive )));
	}

	protected override void PressSpecial()
	{
	}

	protected override void PressHighFiveMelee(bool forceHighFive = false)
	{
	}

	private void Explode()
	{
		this.MakeEffects();
	}

	protected override void RunFiring()
	{
	}

	protected override void AnimateRunning()
	{
		if (this.primed)
		{
			this.speed = 140f;
			this.DeactivateGun();
			this.frameRate = 0.0334f;
			int num = 21 + this.frame % 4;
			this.sprite.SetLowerLeftPixel((float)(num * 32), 32f);
		}
		else
		{
			base.AnimateRunning();
		}
	}

	protected override void Gib(DamageType damageType, float xI, float yI)
	{
		this.wasGibbed = true;
		this.Explode();
		base.Gib(damageType, xI, yI);
	}

	protected virtual void MakeEffects()
	{
		if (!this.exploded)
		{
			this.invulnerable = true;
			this.exploded = true;
			MapController.DamageGround(this, 10, DamageType.Explosion, this.range, this.x, this.y, null);
			if (!this.wasGibbed)
			{
				Map.ExplodeUnits(this, 3, DamageType.Explosion, this.range, this.range, this.x, this.y, this.blastForce * 40f, 300f, this.playerNum, false, false);
			}
			Map.ShakeTrees(this.x, this.y, 128f, 48f, 90f);
			if (!this.wasGibbed)
			{
				MapController.BurnUnitsAround_NotNetworked(this, 5, 1, this.range * 1.75f, this.x, this.y, true, true);
			}
			EffectsController.CreateShrapnel(this.shrapnel, this.x, this.y, 3f, 160f, 40f, 0f, 60f);
			Vector3 a = UnityEngine.Random.insideUnitCircle;
			EffectsController.CreateEffect(this.smoke1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.5f, a * this.range * 3f, BloodColor.None);
			a = UnityEngine.Random.insideUnitCircle;
			EffectsController.CreateEffect(this.smoke1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.2f, UnityEngine.Random.insideUnitSphere * this.range * 3f, BloodColor.None);
			a = UnityEngine.Random.insideUnitCircle;
			EffectsController.CreateEffect(this.smoke2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.5f, a * this.range * 3f, BloodColor.None);
			a = UnityEngine.Random.insideUnitCircle;
			EffectsController.CreateEffect(this.smoke2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.5f, a * this.range * 3f, BloodColor.None);
			a = UnityEngine.Random.insideUnitCircle;
			EffectsController.CreateEffect(this.explosionSmall, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, UnityEngine.Random.value * 0.5f, a * this.range * 3f, BloodColor.None);
			a = UnityEngine.Random.insideUnitCircle;
			EffectsController.CreateEffect(this.explosionSmall, this.x + a.x * this.range * 0.15f, this.y + a.y * this.range * 0.15f, 0f, a * this.range * 0.5f * 3f, BloodColor.None);
			a = UnityEngine.Random.insideUnitCircle;
			EffectsController.CreateEffect(this.explosionSmall, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.15f + UnityEngine.Random.value * 0.4f, a * this.range * 0.5f * 3f, BloodColor.None);
			a = UnityEngine.Random.insideUnitCircle;
			EffectsController.CreateEffect(this.fire1, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f);
			a = UnityEngine.Random.insideUnitCircle;
			EffectsController.CreateEffect(this.fire2, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f);
			a = UnityEngine.Random.insideUnitCircle;
			EffectsController.CreateEffect(this.fire3, this.x + a.x * this.range * 0.25f, this.y + a.y * this.range * 0.25f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f);
			SortOfFollow.Shake(1f, base.transform.position);
			Map.DisturbWildLife(this.x, this.y, 80f, this.playerNum);
			this.invulnerable = false;
			this.Gib(DamageType.Explosion, 0f, 100f);
			Map.DamageDoodads(20, this.x, this.y, 0f, 0f, this.range, this.playerNum);
		}
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
		if (this.health <= 0)
		{
			this.primeCounter = 0.6f + UnityEngine.Random.value * 0.2f;
			this.primed = true;
		}
	}

	public override void Death(float x, float y, DamageObject damage)
	{
		if (this.actionState != ActionState.Dead)
		{
			GameModeController.AddPoint(GameModeController.broPlayer);
		}
		base.Death(x, y, damage);
	}

	public override MookType GetMookType()
	{
		return MookType.Suicide;
	}

	private bool primed;

	private bool exploded;

	private float range = 40f;

	public float blastForce = 20f;

	private float panicCounter;

	public AudioClip triggerClip;

	private float primeCounter = 3f;

	public FlickerFader fire1;

	public FlickerFader fire2;

	public FlickerFader fire3;

	public Shrapnel shrapnel;

	public Shrapnel shrapnelFire;

	public Puff smoke1;

	public Puff smoke2;

	public Puff explosion;

	public Puff explosionSmall;

	public Material[] materials;

	private float primeDelay = 0.25f;

	private float minimumPrimeDelay = 0.5f;

	private bool wasGibbed;
}
