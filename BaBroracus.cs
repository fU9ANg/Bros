// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BaBroracus : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void UseFire()
	{
		this.FireWeapon(this.x + base.transform.localScale.x * 11f, this.y + 6f, base.transform.localScale.x * 120f + this.xI, UnityEngine.Random.value * 60f - 30f);
		this.FireWeapon(this.x + base.transform.localScale.x * 11f, this.y + 7f, base.transform.localScale.x * 150f + this.xI, UnityEngine.Random.value * 70f - 35f);
		Map.DisturbWildLife(this.x, this.y, 60f, this.playerNum);
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		base.Death(xI, yI, damage);
		this.flameSource.Stop();
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 3;
		this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
		EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, base.transform);
		switch (UnityEngine.Random.Range(0, 3))
		{
		case 0:
			ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed, ySpeed, this.playerNum);
			break;
		case 1:
			ProjectileController.SpawnProjectileLocally(this.projectile2, this, x, y, xSpeed, ySpeed, this.playerNum);
			break;
		case 2:
			ProjectileController.SpawnProjectileLocally(this.projectile3, this, x, y, xSpeed, ySpeed, this.playerNum);
			break;
		}
	}

	protected override void StartFiring()
	{
		base.StartFiring();
		this.flameSource.Play();
	}

	protected override void StopFiring()
	{
		base.StopFiring();
		this.flameSource.Stop();
		Sound instance = Sound.GetInstance();
		instance.PlaySoundEffectAt(this.flameSoundEnd, 0.5f, base.transform.position);
	}

	protected override void Start()
	{
		base.Start();
		this.flameSource = base.gameObject.AddComponent<AudioSource>();
		this.flameSource.rolloffMode = AudioRolloffMode.Linear;
		this.flameSource.maxDistance = 500f;
		this.flameSource.volume = 0.4f;
		this.flameSource.clip = this.flameSound;
		this.flameSource.loop = true;
		this.flameSource.playOnAwake = false;
		this.flameSource.Stop();
	}

	protected override void RunFiring()
	{
		base.RunFiring();
		this.fireDelay -= this.t;
		if (this.fire && this.fireDelay <= 0f)
		{
			this.fireCounter += this.t;
			if (this.fireCounter >= this.fireRate)
			{
				this.fireCounter -= this.fireRate;
				this.UseFire();
				base.FireFlashAvatar();
			}
		}
	}

	public AudioClip flameSound;

	public AudioClip flameSoundEnd;

	public AudioSource flameSource;

	public Projectile projectile2;

	public Projectile projectile3;
}
