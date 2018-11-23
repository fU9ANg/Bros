// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TimeBroVanDamme : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
		this.lastRealTime = Time.realtimeSinceStartup;
	}

	protected override void SetDeltaTime()
	{
		this.lastT = this.t;
		if (Time.timeScale > 0f)
		{
			this.t = Mathf.Clamp(Time.deltaTime / Time.timeScale, 0f, 0.04f);
		}
		else
		{
			this.t = 0f;
		}
	}

	protected override void RunFiring()
	{
		this.fireDelay -= this.t;
		if (this.fire)
		{
			this.rollingFrames = 0;
			this.sustainedFire = true;
		}
		if (!this.fire && this.fireDelay > 0f)
		{
			this.sustainedFire = false;
		}
		if (this.sustainedFire && this.fireDelay <= 0f)
		{
			this.fireCounter += this.t;
			if (this.fireCounter >= this.fireRate)
			{
				this.fireCounter -= this.fireRate;
				this.UseFire();
				base.FireFlashAvatar();
				this.fireCount++;
				this.totalFireCount++;
				if (this.fireCount > 5)
				{
					this.fireCount = 0;
					this.fireDelay = 0.4f;
					this.sustainedFire = false;
					this.sound.PlaySoundEffectAt(this.soundHolder.special2Sounds, 0.2f, base.transform.position, 1f, false, 0.05f);
				}
			}
		}
	}

	protected override void UseFire()
	{
		this.FireWeapon(this.x + base.transform.localScale.x * 10f, this.y + 8f, base.transform.localScale.x * 320f, (float)(-15 + this.totalFireCount % 4 * 10 + this.totalFireCount % 3 * 3));
		this.PlayAttackSound();
		Map.DisturbWildLife(this.x, this.y, 60f, this.playerNum);
	}

	protected override void PlayAttackSound()
	{
		this.PlayAttackSound(0.4f);
	}

	protected override void SetHighFiveBoostDeltaTime()
	{
		this.SetDeltaTime();
		if (this.highFiveBoostTime > 6f)
		{
		}
		if (this.highFiveBoostTime > 6f)
		{
			this.highFiveBoostTime -= this.t;
			if (this.highFiveBoostTime <= 6f)
			{
				this.PlaySpecialSound(0.4f);
			}
		}
		else
		{
			this.highFiveBoostTime -= this.t;
			if (this.highFiveBoostTime <= 0f)
			{
				this.highFiveBoost = false;
			}
		}
	}

	protected override void UseSpecial()
	{
		if (base.SpecialAmmo > 0)
		{
			if (base.IsMine)
			{
				Networking.RPC(PID.TargetAll, new RpcSignature(this.TimeBroSpecialRPC), false);
			}
		}
		else
		{
			HeroController.FlashSpecialAmmo(this.playerNum);
			this.ActivateGun();
		}
	}

	private void TimeBroSpecialRPC()
	{
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.special3Sounds, 0.6f, Camera.main.transform.position);
		base.SpecialAmmo--;
		HeroController.TimeBroBoost(2f);
		if (!GameModeController.IsDeathMatchMode && GameModeController.GameMode != GameMode.BroDown)
		{
			HeroController.TimeBroBoostHeroes(2.3f);
		}
		else
		{
			this.TimeBroBoost(2.3f);
		}
		ColorShiftController.SlowTimeEffect(2.2f);
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		HeroController.CancelTimeBroBoost();
		base.Death(xI, yI, damage);
	}

	protected float lastRealTime;

	protected int fireCount;

	protected int totalFireCount;

	protected bool sustainedFire;
}
