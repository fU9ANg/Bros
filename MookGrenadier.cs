// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookGrenadier : Mook
{
	protected override void Start()
	{
		base.Start();
		this.height = 11f;
	}

	protected override void RunFiring()
	{
		if (this.fire && !this.wasFire && !this.usingGrenade)
		{
			this.UseFire();
		}
	}

	protected override void UseFire()
	{
		if (this.y <= this.groundHeight + 1f)
		{
			this.usingGrenade = true;
			this.frame = 0;
			this.counter = 0f;
			this.AnimateThrowingGrenade();
		}
	}

	protected override void AnimateIdle()
	{
		if (this.usingGrenade)
		{
			this.AnimateThrowingGrenade();
		}
		else
		{
			base.AnimateIdle();
		}
	}

	protected override void AnimateRunning()
	{
		if (this.usingGrenade)
		{
			UnityEngine.Debug.Log("animate Throwing Grenade ");
			this.AnimateThrowingGrenade();
		}
		else
		{
			base.AnimateRunning();
		}
	}

	protected override void AnimateActualNewRunningFrames()
	{
		this.frameRate = this.runningFrameRate;
		int num = 0 + this.frame % 10;
		if (this.frame % 5 == 0 && !FluidController.IsSubmerged(this))
		{
			EffectsController.CreateFootPoofEffect(this.x, this.y + 2f, 0f, Vector3.up * 1f - Vector3.right * base.transform.localScale.x * 60.5f);
		}
		if (this.frame % 5 == 0)
		{
			this.PlayFootStepSound();
		}
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 2));
		if (this.gunFrame <= 0)
		{
			this.gunSprite.SetLowerLeftPixel((float)(num * this.gunSpritePixelWidth), (float)(this.gunSpritePixelHeight * 2));
		}
	}

	protected override void RunMovement()
	{
		base.RunMovement();
		if (this.usingGrenade)
		{
			this.xI = 0f;
		}
	}

	protected override void AnimateJumping()
	{
		this.usingGrenade = false;
		base.AnimateJumping();
	}

	protected override void Update()
	{
		base.Update();
		if (Application.isEditor && Input.GetKeyDown(KeyCode.F9))
		{
			this.playerNum = 0;
			this.isHero = true;
		}
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		bool flag = this.health > 0;
		if (damageType == DamageType.Knifed)
		{
			damage = this.health;
		}
		base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
		if (!this.destroyed && !flag && this.health <= 0 && this.deathGrenadeDropCount == 0)
		{
			this.DropDeathGrenade();
			this.deathGrenadeDropPos = base.transform.position;
			this.deathGrenadeDropTime = Time.time;
		}
	}

	protected void DropDeathGrenade()
	{
		if (base.IsMine)
		{
			ProjectileController.SpawnGrenadeOverNetwork(this.specialGrenade, this, this.x, this.y + 4f, 0.001f, 0.011f, this.xI * 0.3f / this.specialGrenade.weight, this.yI * 0.5f / this.specialGrenade.weight + 110f, this.playerNum);
		}
		this.deathGrenadeDropCount++;
	}

	public override void CreateGibEffects(DamageType damageType, float xI, float yI)
	{
		if (!this.destroyed && (this.deathGrenadeDropCount == 0 || Time.time - this.deathGrenadeDropTime > 1f || (base.transform.position - this.deathGrenadeDropPos).sqrMagnitude > 256f))
		{
			this.DropDeathGrenade();
		}
		base.CreateGibEffects(damageType, xI, yI);
	}

	protected override void AnimateThrowingGrenade()
	{
		if (this.y > this.groundHeight + 1f)
		{
			this.DeactivateGun();
			this.usingGrenade = false;
			this.AnimateIdle();
		}
		int num = 13 + Mathf.Clamp(this.frame, 0, 7);
		this.frameRate = 0.045f;
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 2));
		if (this.frame == 5)
		{
			float num2 = 128f;
			float num3 = 32f;
			bool playerRange = this.enemyAI.GetPlayerRange(ref num2, ref num3);
			this.PlaySpecialAttackSound(0.25f);
			float num4 = 130f;
			float num5 = 130f;
			if (playerRange)
			{
				float num6 = Mathf.Clamp((this.grenadeTossDistanceSpeedMinValue + num2 * this.grenadeTossXRangeM + num3 * this.grenadeTossYRangeM) * this.grenadeTossDistanceSpeedM, 0.5f, 1.5f);
				num6 = num6 * (1f - this.grenadeTossV / 2f) + this.grenadeTossV * UnityEngine.Random.value;
				num4 *= num6;
				num5 *= num6;
			}
			if (base.IsMine)
			{
				ProjectileController.SpawnGrenadeOverNetwork(this.specialGrenade, this, this.x + Mathf.Sign(base.transform.localScale.x) * 8f, this.y + 24f, 0.001f, 0.011f, Mathf.Sign(base.transform.localScale.x) * num4, num5, this.playerNum);
			}
		}
		if (this.frame >= 7)
		{
			this.usingGrenade = false;
		}
	}

	protected override void RunGun()
	{
		if (this.usingGrenade)
		{
		}
	}

	public override MookType GetMookType()
	{
		return MookType.Grenadier;
	}

	protected override void PressSpecial()
	{
	}

	protected bool usingGrenade;

	public float grenadeTossDistanceSpeedM = 0.008f;

	public float grenadeTossDistanceSpeedMinValue = 70f;

	public float grenadeTossXRangeM = 0.6f;

	public float grenadeTossYRangeM = 0.4f;

	public float grenadeTossV = 0.3f;

	protected int deathGrenadeDropCount;

	protected Vector3 deathGrenadeDropPos = Vector3.zero;

	protected float deathGrenadeDropTime;
}
