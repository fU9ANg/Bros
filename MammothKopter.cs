// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class MammothKopter : Mookopter
{
	protected override void Start()
	{
		base.Start();
		this.mamKopAI = base.GetComponent<MammothKopterAI>();
		this.originalVerticalSpeed = this.verticalSpeed;
		this.facingDirection = 1;
		base.transform.localScale = new Vector3(-1f, 1f, 1f);
		base.GetComponent<Collider>().enabled = true;
	}

	public void BoostSpeed()
	{
		this.verticalSpeed = this.originalVerticalSpeed * 2f;
	}

	public void ReturnToRegularSpeed()
	{
		this.verticalSpeed = this.originalVerticalSpeed;
	}

	protected override void Update()
	{
		base.Update();
		if (Application.isEditor && Input.GetKeyDown(KeyCode.Space))
		{
			Networking.RPC<PropaneBlock>(PID.TargetAll, new RpcSignature<PropaneBlock>(this.PropaneHit), null, false);
		}
		this.plumeDelay += this.t;
		if (this.plumeDelay > 0.03f)
		{
			this.plumeDelay -= 0.03f;
			if (this.propaneHitsLeft < 5)
			{
				Vector3 position = this.smokePoints[UnityEngine.Random.Range(0, this.smokePoints.Length)].transform.position;
				EffectsController.CreateBlackPlumeParticle(position.x, position.y, 16f, 0f, 45f, 3f, 1f);
				EffectsController.CreateFireSparks(this.x + (float)UnityEngine.Random.Range(-40, 80), this.y + (float)UnityEngine.Random.Range(-40, 40), 1, 10f, 7f, 0f, 15f, 0.65f);
			}
			if (this.propaneHitsLeft < 3)
			{
				for (int i = 0; i < this.smokePoints.Length; i++)
				{
					Vector3 position2 = this.smokePoints[i].transform.position;
					EffectsController.CreateBlackPlumeParticle(position2.x, position2.y, 16f, 0f, 45f, 3f, UnityEngine.Random.value);
				}
				EffectsController.CreateBlackPlumeParticle(this.x + (float)UnityEngine.Random.Range(-40, 80), this.y + (float)UnityEngine.Random.Range(-40, 40), 16f, 0f, 45f, 3f, UnityEngine.Random.value);
				EffectsController.CreateFireSparks(this.x + (float)UnityEngine.Random.Range(-40, 80), this.y + (float)UnityEngine.Random.Range(-40, 40), 1, 10f, 7f, 0f, 15f, 0.65f);
			}
		}
		if (this.painAnimExplosions > 0)
		{
			this.painAnimExplosionDelay -= this.t;
			if (this.painAnimExplosionDelay < 0f)
			{
				this.painAnimExplosions--;
				this.painAnimExplosionDelay = 0.03f;
				EffectsController.CreateExplosion(this.x + (float)UnityEngine.Random.Range(-60, 80), this.y + (float)UnityEngine.Random.Range(-60, 60), 10f, 10f, 0f, UnityEngine.Random.value, 24f, 1f, 0.3f, false);
			}
		}
		if (this.mamKopAI.mamKopterThinkState == MammothKopterAI.ThinkState.DeployMooks)
		{
			switch (this.ziplinePhase)
			{
			case 0:
				if (this.zipline.hasAttached)
				{
					this.ziplinePhase = 1;
					this.mooksToZipline = UnityEngine.Random.Range(3, 5);
					this.nextMookZiplineDelay = 0.3f;
				}
				break;
			case 1:
				if (this.mooksToZipline > 0)
				{
					if ((this.nextMookZiplineDelay -= this.t) < 0f)
					{
						this.mooksToZipline--;
						Mook unit = MapController.SpawnMook_Networked(this.mookPrefabs[UnityEngine.Random.Range(0, this.mookPrefabs.Length)], base.transform.position.x, base.transform.position.y - 12f, 0f, 0f, false, false, false, false, true);
						this.zipline.AttachUnit(unit);
						this.nextMookZiplineDelay = 0.3f;
					}
				}
				else if (this.zipline.attachedUnits.Count == 0 && (this.nextMookZiplineDelay -= this.t) < 0f)
				{
					this.ziplinePhase = 2;
				}
				break;
			case 2:
				UnityEngine.Object.Destroy(this.zipline.gameObject);
				this.ziplinePhase++;
				break;
			}
		}
		if (this.grenadeLauncherEnabled && this.mamKopAI.mamKopterThinkState == MammothKopterAI.ThinkState.ShootAtBottom && this.fire)
		{
			this.mookLauncher.Fire();
		}
		else
		{
			this.mookLauncher.StopFiring();
		}
		if (this.health <= 0)
		{
			this.deathRattleTime -= this.t;
			if (this.deathRattleTime < 0f)
			{
				GameModeController.LevelFinish(LevelResult.Success);
			}
		}
		Map.HitUnits(this, 20, DamageType.Melee, 60f, 10f, base.transform.position.x, base.transform.position.y + 70f, UnityEngine.Random.Range(-300f, 300f), 250f, false, true);
	}

	internal void StartZipline(Vector3 toAnchorPoint)
	{
	}

	protected override void AnimateRolling()
	{
	}

	protected override void AnimateIdle()
	{
	}

	protected override void AnimateTurning()
	{
	}

	protected override void UseSpecial()
	{
		if (!this.hasPlayedBombIntro)
		{
			if (!this.hasPlayedBombOpenClip)
			{
				this.hasPlayedBombOpenClip = true;
				Sound.GetInstance().PlayAudioClip(this.bombDoorOpenClip, base.transform.position, 0.4f);
			}
			this.bombDoorAnimDelay += this.t;
			if (this.bombDoorAnimDelay > 0.1f)
			{
				this.bombDoorAnimDelay -= 0.1f;
				this.currentBombDoorFrame++;
				this.bombBayDoorsSprite.SetLowerLeftPixel((float)(this.bombDoorFrameWidth * this.currentBombDoorFrame), (float)((int)this.bombBayDoorsSprite.lowerLeftPixel.y));
				if (this.currentBombDoorFrame == this.bombDoorsFrames)
				{
					this.hasPlayedBombIntro = true;
				}
			}
		}
		else
		{
			this.specialCounter += this.t;
			if (this.specialCounter > 0.1f)
			{
				this.specialCounter -= 0.1f;
				this.bombDropCount++;
				int num = this.bombDropCount % 5;
				if (num > 0 && num < 5)
				{
					Vector3 a = new Vector3(-45f, -20f, 0f);
					switch (num)
					{
					case 1:
						a -= Vector3.right * 2f;
						ProjectileController.SpawnProjectileLocally(this.bombProjectile, this, base.transform.position.x + a.x * (float)this.facingDirection, base.transform.position.y + a.y, (float)(-(float)this.facingDirection * 20) + this.xI * 0.5f, 0f, -1);
						break;
					case 2:
						a += Vector3.right * 2f + Vector3.forward * 8f;
						ProjectileController.SpawnProjectileLocally(this.bombProjectile, this, base.transform.position.x + a.x * (float)this.facingDirection, base.transform.position.y + a.y, (float)(-(float)this.facingDirection * 10) + this.xI * 0.5f, 0f, -1);
						break;
					case 3:
						a -= Vector3.right * 4f;
						ProjectileController.SpawnProjectileLocally(this.bombProjectile, this, base.transform.position.x + a.x * (float)this.facingDirection, base.transform.position.y + a.y, (float)(-(float)this.facingDirection * 30) + this.xI * 0.5f, 0f, -1);
						break;
					case 4:
						a += Vector3.right * 4f + Vector3.forward * 8f;
						ProjectileController.SpawnProjectileLocally(this.bombProjectile, this, base.transform.position.x + a.x * (float)this.facingDirection, base.transform.position.y + a.y, (float)(-(float)this.facingDirection * 10) + this.xI * 0.5f, 0f, -1);
						break;
					default:
						UnityEngine.Debug.LogError("Logical Mistake");
						break;
					}
					Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.special2Sounds, 0.7f, base.transform.position);
				}
				if (num == 4)
				{
					this.bombDropLooping = true;
					this.bombDropLoopFrame = 0;
					this.bombDropLoopFrameCounter = -0.2f;
					this.specialCounter = 0.1f;
				}
			}
			if (this.bombDropLooping)
			{
				this.bombDropLoopFrameCounter += this.t;
				if (this.bombDropLoopFrameCounter > 0.0667f)
				{
					this.bombDropLoopFrameCounter -= 0.0667f;
					this.bombDropLoopFrame++;
					if (this.bombDropLoopFrame >= 5)
					{
						this.bombDropLooping = false;
					}
				}
			}
		}
	}

	public void PropaneHit(PropaneBlock propaneTank)
	{
		if (propaneTank != null)
		{
			if (this.tanksThatHaveAlreadyHit.Contains(propaneTank))
			{
				return;
			}
			this.tanksThatHaveAlreadyHit.Add(propaneTank);
		}
		else
		{
			UnityEngine.Debug.LogWarning("Propane tank is null");
		}
		this.propaneHitsLeft--;
		if (this.propaneHitsLeft == 0)
		{
			TimeController.StopTime(5f, 0.2f, 0.6f, true, true);
		}
		EffectsController.CreateGibs(this.helicopterGibs, this.helicopterGibs.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial, this.x, this.y, 200f, 50f, this.xI * 0.5f, 100f);
		if (this.propaneHitsLeft == 4)
		{
			if (!this.fire)
			{
				foreach (SpriteSM spriteSM in this.spritesToSwapOnDamage)
				{
					spriteSM.GetComponent<Renderer>().material = this.damagedMaterial;
					this.weapon.GetComponent<SpriteSM>().GetComponent<Renderer>().enabled = false;
					this.mookLauncher.GetComponent<SpriteSM>().SetLowerLeftPixel((float)((int)this.mookLauncher.GetComponent<SpriteSM>().pixelDimensions.x * 21), (float)((int)this.mookLauncher.GetComponent<SpriteSM>().lowerLeftPixel.y));
				}
			}
		}
		else if (this.propaneHitsLeft == 2)
		{
			foreach (SpriteSM spriteSM2 in this.spritesToSwapOnDamage)
			{
				spriteSM2.GetComponent<Renderer>().material = this.completelyFuckedMaterial;
			}
		}
		this.yI += 180f;
		this.y += 4f;
		this.painAnimExplosions = 15;
		if (this.propaneHitsLeft == 0)
		{
			this.deathPos = base.transform.position;
			this.Damage(this.health * 2, DamageType.Crush, 0f, 0f, 0, this, -100f, -100f);
		}
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		if (damageType == DamageType.Crush)
		{
			TriggerManager.ClearActions();
			SortOfFollow.ControlledByTriggerAction = true;
			SortOfFollow.ForceSlowSnapBack(2.5f, 1f);
			SortOfFollow.followPos = this.deathPos + Vector3.up * 24f;
			base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
		}
	}

	protected override void SetUnhurtMaterial()
	{
	}

	protected override void SetHurtMaterial()
	{
		this.hurtCounter = 0.0334f;
	}

	protected override void SetDeathFrame()
	{
		this.sprite.SetLowerLeftPixel((float)(this.spritePixelWidth * 9), (float)((int)this.sprite.lowerLeftPixel.y));
	}

	protected override void RunStanding()
	{
		if (this.health <= 0)
		{
			if (this.deathRattleTime < 0f)
			{
				base.RunStanding();
			}
		}
		else
		{
			this.GetGroundHeight();
		}
	}

	protected override void RunSmoke()
	{
		this.smokeCounter += this.t;
		if (this.smokeCounter > 0.15f)
		{
			this.smokeCounter -= 0.15f;
			EffectsController.CreateBlackPlumeParticle(this.x - 8f, this.y + 24f, 16f, 0f, 45f, 1.5f, 1f);
			EffectsController.CreateBlackPlumeParticle(this.x + 8f, this.y + 24f, 16f, 0f, 45f, 1.5f, 1f);
			EffectsController.CreateExplosion(this.x + (float)UnityEngine.Random.Range(-60, 60), this.y + (float)UnityEngine.Random.Range(-60, 60), 10f, 10f, 0f, UnityEngine.Random.value, 24f, 1f, 0.3f, false);
			EffectsController.CreateGibs(this.helicopterGibs, 1, this.x, this.y, 200f, 50f, this.xI * 0.5f, 100f);
			this.DamageGroundBelow(true);
		}
	}

	public override bool CanFire()
	{
		return true;
	}

	protected override void StartTurnLeft()
	{
	}

	protected override void StartTurnRight()
	{
	}

	public void EnableGrenadeLauncher()
	{
		this.grenadeLauncherEnabled = true;
	}

	internal void TeleportTo(int row, int col)
	{
		Map.GetBlocksXY(ref this.x, ref this.y, row - Map.lastYLoadOffset, col - Map.lastXLoadOffset);
		base.transform.position = new Vector3(this.x, this.y, base.transform.position.z);
		this.kopterTargetX = this.x;
		this.kopterTargetY = this.y;
	}

	public void PropaneHitWarning()
	{
		if (!this.hasWarned)
		{
			this.hasWarned = true;
			TimeController.StopTime(4.5f, 0.2f, 0.65f, true, true);
		}
	}

	protected override void RunAI()
	{
		this.wasUp = this.up;
		this.wasDown = this.down;
		this.wasLeft = this.left;
		this.wasRight = this.right;
		this.up = false;
		this.down = false;
		this.left = false;
		this.right = false;
		if ((this.weapon != null && this.weapon.health <= 0) || this.health <= 0)
		{
			return;
		}
		base.GetComponent<MammothKopterAI>().GetInput(ref this.left, ref this.right, ref this.up, ref this.down, ref this.jump, ref this.fire, ref this.special, ref this.special2, ref this.special3, ref this.special4);
		if (this.fire)
		{
			this.FireWeapon();
		}
		if (this.special)
		{
			this.UseSpecial();
		}
		else
		{
			this.bombDropLooping = false;
			if (this.bombDropCount > 0)
			{
				this.bombDropCount = 0;
				this.missiles.Frame = this.bombDropCount;
			}
		}
		if (!this.up && this.wasUp)
		{
			this.kopterTargetY = this.y;
		}
		if (!this.left && this.wasLeft)
		{
			this.kopterTargetX = this.x;
		}
		if (!this.right && this.wasRight)
		{
			this.kopterTargetX = this.x;
		}
		if (!this.down && this.wasDown)
		{
			this.kopterTargetY = this.y;
		}
	}

	public MammothKopterAI mamKopAI;

	public ZipLine ziplinePrefab;

	public ZipLine zipline;

	private int ziplinePhase;

	public Mook[] mookPrefabs;

	private Block zipToBlock;

	public GibHolder helicopterGibs;

	public AudioClip bombDoorOpenClip;

	private bool hasPlayedBombOpenClip;

	public SpriteSM bombBayDoorsSprite;

	private bool hasPlayedBombIntro;

	private int bombDoorsFrames = 11;

	private int currentBombDoorFrame;

	private float bombDoorAnimDelay;

	private int bombDoorFrameWidth = 96;

	private float deathRattleTime = 5f;

	private float originalVerticalSpeed;

	private int mooksToZipline;

	private float nextMookZiplineDelay;

	private int propaneHitsLeft = 5;

	public HelicopterMookLauncher mookLauncher;

	private float plumeDelay;

	private Vector3 deathPos;

	public bool grenadeLauncherEnabled;

	public SpriteSM[] spritesToSwapOnDamage;

	public Material damagedMaterial;

	public Material completelyFuckedMaterial;

	public Transform[] smokePoints;

	private float painAnimExplosionDelay;

	private int painAnimExplosions;

	private List<PropaneBlock> tanksThatHaveAlreadyHit = new List<PropaneBlock>();

	private bool hasWarned;
}
