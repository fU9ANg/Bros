// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookSuicide : Mook
{
	protected override void Start()
	{
		base.Start();
		this.deathCounter = 0.6f + UnityEngine.Random.value * 0.2f;
		this.originalMaterial = base.GetComponent<Renderer>().sharedMaterial;
	}

	protected override void UseFire()
	{
		this.PlayAttackSound();
		this.MakeEffects();
	}

	public override float GetGroundHeightGround()
	{
		if (!this.enemyAI.IsAlerted())
		{
			return base.GetGroundHeightGround();
		}
		float num = -200f;
		if (this.xI > 0f)
		{
			if (Physics.Raycast(new Vector3(this.x + 4f, this.y + 16f, 0f), Vector3.down, out this.raycastHit, 540f, this.groundLayer) && this.raycastHit.point.y > num)
			{
				num = this.raycastHit.point.y;
			}
			if (Physics.Raycast(new Vector3(this.x - 2f, this.y + 16f, 0f), Vector3.down, out this.raycastHit, 540f, this.groundLayer) && this.raycastHit.point.y > num)
			{
				num = this.raycastHit.point.y;
			}
		}
		if (this.xI < 0f)
		{
			if (Physics.Raycast(new Vector3(this.x - 4f, this.y + 16f, 0f), Vector3.down, out this.raycastHit, 540f, this.groundLayer) && this.raycastHit.point.y > num)
			{
				num = this.raycastHit.point.y;
			}
			if (Physics.Raycast(new Vector3(this.x + 2f, this.y + 16f, 0f), Vector3.down, out this.raycastHit, 540f, this.groundLayer) && this.raycastHit.point.y > num)
			{
				num = this.raycastHit.point.y;
			}
		}
		if (Physics.Raycast(new Vector3(this.x, this.y + 16f, 0f), Vector3.down, out this.raycastHit, 540f, this.groundLayer) && this.raycastHit.point.y > num)
		{
			num = this.raycastHit.point.y;
		}
		return num;
	}

	protected override void AnimateSpecial2()
	{
		if (this.actionState == ActionState.Idle)
		{
			this.frameRate = 0.045f;
			if (this.frame == 9)
			{
				this.frameRate = 1f;
			}
			if (this.frame > 16)
			{
				this.usingSpecial2 = false;
				this.AnimateIdle();
				this.enemyAI.SetMentalState(MentalState.Idle);
			}
			else
			{
				this.sprite.SetLowerLeftPixel((float)(0 + Mathf.Clamp(this.frame, 0, 16) * this.spritePixelWidth), (float)(this.spritePixelHeight * 8));
				this.DeactivateGun();
			}
		}
		else
		{
			this.usingSpecial2 = false;
			this.AnimateIdle();
		}
	}

	protected override void AnimateJumping()
	{
		if (this.decapitated)
		{
			base.AnimateDecapitated();
		}
		else if (this.IsParachuteActive && this.hasCharged)
		{
			this.AnimateRunning();
		}
		else
		{
			base.AnimateJumping();
		}
	}

	protected virtual void MakeEffects()
	{
		if (!this.exploded)
		{
			this.invulnerable = true;
			this.exploded = true;
			MapController.DamageGround(this, 10, DamageType.Explosion, this.range, this.x, this.y, null);
			Map.ExplodeUnits(this, 3, DamageType.Explosion, this.range, this.range, this.x, this.y, this.blastForce * 40f, 300f, this.playerNum, false, false);
			Map.ShakeTrees(this.x, this.y, 128f, 48f, 80f);
			MapController.BurnUnitsAround_NotNetworked(this, 5, 1, this.range * 1.75f, this.x, this.y, true, true);
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
			SortOfFollow.Shake(1f);
			Map.DisturbWildLife(this.x, this.y, 80f, this.playerNum);
			this.invulnerable = false;
			if (this.fire)
			{
				this.deathType = DeathType.Suicide;
				this.NotifyDeathType();
			}
			else
			{
				this.deathType = DeathType.Explode;
				this.NotifyDeathType();
			}
			this.Gib(DamageType.Explosion, 0f, 100f);
			Map.DamageDoodads(20, this.x, this.y, 0f, 0f, this.range, this.playerNum);
		}
	}

	public override void HeadShot(int damage, DamageType damageType, float xI, float yI, int direction, float xHit, float yHit, MonoBehaviour damageSender)
	{
		UnityEngine.Debug.Log("Head Shot ");
		if (this.decapitated)
		{
			this.Damage(damage, damageType, xI, yI, direction, damageSender, xHit, yHit);
		}
		else if ((damageType == DamageType.Bullet || damageType == DamageType.Melee || damageType == DamageType.Knifed) && this.health > 0 && damage * 3 >= this.health)
		{
			this.decapitated = true;
			EffectsController.CreateBloodParticles(this.bloodColor, this.x, this.y + 16f, 40, 3f, 2f, 50f, xI * 0.5f + (float)(direction * 50), yI * 0.4f + 80f);
			EffectsController.CreateGibs(this.decapitationGib, base.GetComponent<Renderer>().sharedMaterial, this.x, this.y, 100f, 100f, xI * 0.5f, yI * 0.4f + 60f);
			this.PlayDecapitateSound();
			this.DeactivateGun();
			base.GetComponent<Renderer>().sharedMaterial = this.decapitatedMaterial;
			if (UnityEngine.Random.value > 0f)
			{
				this.health = 1;
				this.Panic((int)Mathf.Sign(xI), 2.5f, true);
				this.decapitationCounter = 0.4f + UnityEngine.Random.value * 0.4f;
			}
			else
			{
				this.Damage(this.health, damageType, xI, yI, direction, damageSender, xHit, yHit);
			}
		}
		else
		{
			base.HeadShot(damage, damageType, xI, yI, direction, xHit, yHit, damageSender);
		}
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
		if (this.health <= 0)
		{
			this.deathCounter = 0.6f + UnityEngine.Random.value * 0.2f;
		}
		this.usingSpecial2 = false;
	}

	public override void ReduceDeathTimer(int playerNum, float newTime)
	{
		this.playerNum = playerNum;
		base.ReduceDeathTimer(playerNum, newTime);
		this.deathCounter = newTime;
	}

	protected override void Update()
	{
		base.Update();
		if (Map.isEditing || CutsceneController.isInCutscene)
		{
			return;
		}
		if (this.hasCharged)
		{
			this.panicCounter += this.t;
			if (this.panicCounter > 0.0455f)
			{
				this.panicCounter -= 0.04445f;
				EffectsController.CreateFireSparks(this.x + base.transform.localScale.x * 4f, this.y + 10f, 1, 3f, 4f, this.xI * 0.1f, 16f, 0.6f);
			}
		}
		if (this.health <= 0)
		{
			this.deathCounter -= this.t;
			this.RunWarning(Time.deltaTime, this.deathCounter);
			if (this.deathCounter < 0f)
			{
				this.MakeEffects();
				this.CheckDestroyed();
			}
		}
		if (this.decapitated && this.health > 0)
		{
			this.decapitationCounter -= this.t;
			if (this.decapitationCounter <= 0f)
			{
				this.y += 2f;
				this.Damage(this.health + 15, DamageType.Bullet, 0f, 12f, (int)Mathf.Sign(-base.transform.localScale.x), this, this.x, this.y + 8f);
			}
		}
	}

	public override void Knock(DamageType damageType, float xI, float yI, bool forceTumble)
	{
		base.Knock(damageType, xI, yI, forceTumble);
		this.usingSpecial3 = false;
		this.usingSpecial2 = false;
		this.usingSpecial4 = false;
	}

	public override void RunWarning(float t, float explosionTime)
	{
		if (!this.decapitated && explosionTime < 0.7f)
		{
			this.warningCounter += t;
			if (this.warningOn && this.warningCounter > 0.0667f)
			{
				this.warningOn = false;
				this.warningCounter -= 0.0667f;
				base.GetComponent<Renderer>().sharedMaterial = this.originalMaterial;
			}
			else if (this.warningCounter > 0.0667f && explosionTime < 0.175f)
			{
				this.warningOn = true;
				this.warningCounter -= 0.0667f;
				base.GetComponent<Renderer>().sharedMaterial = this.warningMaterial;
			}
			else if (this.warningCounter > 0.175f && explosionTime < 0.5f)
			{
				this.warningOn = true;
				this.warningCounter -= 0.175f;
				base.GetComponent<Renderer>().sharedMaterial = this.warningMaterial;
			}
		}
	}

	protected override void RunBurning()
	{
		this.RunWarning(this.t, this.burnTime);
		base.RunBurning();
	}

	protected override void StopBurning()
	{
		this.enemyAI.StopPanicking();
		this.actionState = ActionState.Idle;
		this.ChangeFrame();
		this.Stop();
		this.MakeEffects();
	}

	protected override void AnimateRunning()
	{
		this.speed = 90f;
		if (this.decapitated)
		{
			base.AnimateDecapitated();
		}
		else if (this.burnTime > 0f)
		{
			this.speed = 90f;
			this.DeactivateGun();
			this.frameRate = 0.04f;
			int num = 21 + this.frame % 4;
			this.sprite.SetLowerLeftPixel((float)(num * 32), 32f);
		}
		else if (this.enemyAI.IsAlerted())
		{
			this.speed = 140f;
			this.DeactivateGun();
			this.frameRate = 0.0334f;
			int num2 = 21 + this.frame % 4;
			this.sprite.SetLowerLeftPixel((float)(num2 * 32), 32f);
		}
		else
		{
			base.AnimateRunning();
		}
	}

	protected override void Gib(DamageType damageType, float xI, float yI)
	{
		if (!this.exploded)
		{
			this.playerNum = 10;
			this.MakeEffects();
			this.playerNum = -1;
			this.health = 0;
		}
		else
		{
			base.Gib(damageType, xI, yI);
		}
	}

	public override void PlaySpecialSound(float volume)
	{
		base.PlaySpecialSound(volume);
		this.hasCharged = true;
		Map.DisturbWildLife(this.x + base.transform.localScale.x * 20f, this.y, 70f, 5);
	}

	public float range = 40f;

	public float blastForce = 20f;

	public FlickerFader fire1;

	public FlickerFader fire2;

	public FlickerFader fire3;

	public Shrapnel shrapnel;

	public Shrapnel shrapnelFire;

	public Puff smoke1;

	public Puff smoke2;

	public Puff explosion;

	public Puff explosionSmall;

	protected bool hasCharged;

	protected float panicCounter;

	public bool exploded;

	public GibHolder decapitationGib;

	public Material decapitatedMaterial;

	protected float deathCounter = 1f;

	protected float warningCounter;

	public Material warningMaterial;

	protected Material originalMaterial;

	protected bool warningOn;
}
