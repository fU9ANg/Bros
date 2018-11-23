// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brobocop : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
		if (base.GetComponent<AudioSource>() == null)
		{
			base.gameObject.AddComponent<AudioSource>();
			base.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
			base.GetComponent<AudioSource>().minDistance = 200f;
			base.GetComponent<AudioSource>().dopplerLevel = 0.1f;
			base.GetComponent<AudioSource>().maxDistance = 500f;
			base.GetComponent<AudioSource>().volume = 0.33f;
		}
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		base.Death(xI, yI, damage);
		for (int i = 0; i < this.targettedTargets.Count; i++)
		{
			this.targettedTargets[i].GoAway();
		}
	}

	protected override void Start()
	{
		base.Start();
		this.targettedUnits = new List<Unit>();
		AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
		audioSource.clip = this.chargingSound;
		audioSource.volume = 0.5f;
		audioSource.pitch = 1f;
		audioSource.loop = false;
		audioSource.playOnAwake = false;
		audioSource.Stop();
		audioSource.rolloffMode = AudioRolloffMode.Linear;
		audioSource.maxDistance = 384f;
		audioSource.minDistance = 80f;
		audioSource.dopplerLevel = 0.1f;
	}

	private void CreateTargetOnUnit(Unit newTargettedUnit)
	{
		FollowingObject followingObject = UnityEngine.Object.Instantiate(ProjectileController.instance.targetPrefab) as FollowingObject;
		followingObject.Follow(newTargettedUnit.transform, Vector3.up * 8f);
		this.targettedTargets.Add(followingObject.GetComponent<AnimatedIcon>());
		if (this.targetSystem != null)
		{
			this.targetSystem.PlayHitSound();
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.usingTargetingSystem && this.targetSystem != null)
		{
			Unit nextClosestUnit = Map.GetNextClosestUnit(this.playerNum, this.targetSystem.TravelDirection, this.scanningRange, this.scanningRange, this.targetSystem.transform.position.x, this.targetSystem.transform.position.y, this.targettedUnits);
			if (nextClosestUnit != null)
			{
				this.targettedUnits.Add(nextClosestUnit);
				Networking.RPC<Unit>(PID.TargetAll, new RpcSignature<Unit>(this.CreateTargetOnUnit), nextClosestUnit, false);
			}
		}
		if (this.shooting && this.health > 0 && (this.burstDelay -= this.t) <= 0f)
		{
			this.burstDelay = 0.033334f;
			for (int i = 0; i < 3; i++)
			{
				if (this.bulletsToFire > 0)
				{
					this.bulletVariation = Mathf.Clamp(this.bulletVariation + 0.2f, 0f, 2f);
					this.FireWeapon(this.x + base.transform.localScale.x * 15f, this.y + 9.5f, base.transform.localScale.x * 300f + 100f * (UnityEngine.Random.value - 0.5f) * this.bulletVariation, UnityEngine.Random.Range(-50f, 50f) * this.bulletVariation);
					this.PlayAttackSound();
					Map.DisturbWildLife(this.x, this.y, 80f, this.playerNum);
					this.bulletsToFire--;
				}
				else
				{
					this.shooting = false;
				}
			}
		}
	}

	protected override void RunFiring()
	{
		if (this.fire && !this.shooting)
		{
			if (!this.wasFire)
			{
				base.GetComponent<AudioSource>().clip = this.chargingSound;
				base.GetComponent<AudioSource>().volume = 0.4f;
				base.GetComponent<AudioSource>().Play();
				this.chargingFire = true;
				this.chargeTime = 0f;
			}
			if (this.chargeTime < 2.65f)
			{
				this.chargeTime += this.t;
				if (this.chargeTime >= 2.65f)
				{
					base.GetComponent<AudioSource>().clip = this.finishedChargingSound;
					base.GetComponent<AudioSource>().volume = 0.7f;
					base.GetComponent<AudioSource>().Play();
				}
			}
		}
		else if (this.wasFire && !this.shooting)
		{
			base.GetComponent<AudioSource>().Stop();
			this.UseFire();
			this.chargeTime = 0f;
			this.chargingFire = false;
		}
	}

	protected override void UseFire()
	{
		this.shooting = true;
		this.chargeTime = Mathf.Clamp(this.chargeTime, 0.01f, 2.625f);
		float num = this.chargeTime / 2.625f;
		float num2 = this.chargeTime / this.chargeTimePerBulletFired * num * 0.5f + this.chargeTime / this.chargeTimePerBulletFired * 0.5f;
		this.bulletsToFire = (int)num2;
		this.bulletsToFire = Mathf.Clamp(this.bulletsToFire, 1, 35);
		this.bulletVariation = 0f;
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 3;
		this.SetGunSprite(3, 0);
		base.FireFlashAvatar();
		EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, base.transform);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed * 1.25f, ySpeed - 10f + UnityEngine.Random.value * 20f, this.playerNum);
	}

	private IEnumerator ShootSpecialEnumerator()
	{
		while (this.targettedUnits.Count > 0)
		{
			if (this.health <= 0)
			{
				yield break;
			}
			if (this.targettedUnits[0] != null && this.targettedUnits[0].health > 0)
			{
				Projectile remB = ProjectileController.SpawnProjectileOverNetwork(this.remoteProjectile, this, this.x, this.y + 6.5f, base.transform.localScale.x * 400f, 0f, false, this.playerNum, false, false);
				remB.GetComponent<BulletGuidedRobrocop>().targetUnits = new List<Unit>();
				remB.GetComponent<BulletGuidedRobrocop>().targetUnits.Add(this.targettedUnits[0]);
				this.targettedUnits.RemoveAt(0);
				remB.GetComponent<BulletGuidedRobrocop>().targetIcons = new List<AnimatedIcon>();
				remB.GetComponent<BulletGuidedRobrocop>().targetIcons.Add(this.targettedTargets[0]);
				this.targettedTargets.RemoveAt(0);
				this.PlayAttackSound();
				this.gunFrame = 3;
				this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
				yield return new WaitForSeconds(0.12f);
			}
			else
			{
				this.targettedUnits.RemoveAt(0);
				UnityEngine.Object.Destroy(this.targettedTargets[0]);
				this.targettedTargets.RemoveAt(0);
			}
		}
		this.targettedUnits = null;
		this.usingTargetingSystem = false;
		this.firingSpecial = false;
		yield break;
	}

	protected void FireSpecial()
	{
		this.targetSystem.DestroyNetworked();
		this.firingSpecial = true;
		base.StartCoroutine(this.ShootSpecialEnumerator());
	}

	private void SetTargettingSystemOverTheNetwork(RobrocopTargetingSystem TargetSystem)
	{
		this.targetSystem = TargetSystem;
	}

	protected override void UseSpecial()
	{
		if (base.IsMine)
		{
			if (base.SpecialAmmo > 0 && !this.usingTargetingSystem && !this.firingSpecial)
			{
				base.SpecialAmmo--;
				HeroController.SetSpecialAmmo(this.playerNum, base.SpecialAmmo);
				this.usingTargetingSystem = true;
				this.targetingTime = 5f;
				DirectionEnum arg = (base.transform.localScale.x <= 0f) ? DirectionEnum.Left : DirectionEnum.Right;
				this.targetSystem = Networking.Instantiate<RobrocopTargetingSystem>(this.targetSystemPrefab, base.transform.position + Vector3.up * 6.5f, Quaternion.identity, null, false);
				Networking.RPC<RobrocopTargetingSystem>(PID.TargetOthers, new RpcSignature<RobrocopTargetingSystem>(this.SetTargettingSystemOverTheNetwork), this.targetSystem, false);
				Networking.RPC<Brobocop, DirectionEnum>(PID.TargetAll, new RpcSignature<Brobocop, DirectionEnum>(this.targetSystem.SetupTargetting), this, arg, false);
				this.targettedUnits = new List<Unit>();
				this.targettedTargets = new List<AnimatedIcon>();
			}
			else if (this.usingTargetingSystem && !this.firingSpecial)
			{
				this.FireSpecial();
			}
			else
			{
				if (base.SpecialAmmo <= 0)
				{
					HeroController.FlashSpecialAmmo(this.playerNum);
				}
				this.ActivateGun();
			}
		}
	}

	protected override void CheckInput()
	{
		base.CheckInput();
		if (this.usingTargetingSystem && this.targetSystem != null)
		{
			if (this.up)
			{
				this.targetSystem.TravelDirection = DirectionEnum.Up;
			}
			if (this.down)
			{
				this.targetSystem.TravelDirection = DirectionEnum.Down;
			}
			if (this.left)
			{
				this.targetSystem.TravelDirection = DirectionEnum.Left;
			}
			if (this.right)
			{
				this.targetSystem.TravelDirection = DirectionEnum.Right;
			}
			this.up = false;
			this.left = false;
			this.right = false;
			this.down = false;
			this.buttonJump = false;
			if ((this.targetingTime -= this.t) < 0f)
			{
				this.FireSpecial();
			}
		}
	}

	public ReactionBubble helpBubble;

	public int burstSize = 5;

	public float burstDelay;

	public Transform scanLine;

	protected int scanningDirection;

	protected float scanningRange = 10f;

	public bool usingTargetingSystem;

	public float targetingTime;

	protected bool firingSpecial;

	protected float chargeTime;

	protected bool chargingFire;

	protected float chargeTimePerBulletFired = 0.075f;

	protected bool shooting;

	protected int bulletsToFire;

	protected float bulletVariation;

	public List<Unit> targettedUnits;

	public List<AnimatedIcon> targettedTargets;

	public RobrocopTargetingSystem targetSystemPrefab;

	private RobrocopTargetingSystem targetSystem;

	public AudioClip chargingSound;

	public AudioClip finishedChargingSound;
}
