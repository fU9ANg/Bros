// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MechRocketBattery : MechWeapon
{
	protected override void Awake()
	{
		base.Awake();
		this.fireDelay = 1.1f;
		this.invulnerable = true;
	}

	protected override void Update()
	{
		base.Update();
		if (this.health <= 0)
		{
			if (!this.hidingHoles)
			{
				this.hidingHoles = true;
				foreach (TankRocketHole tankRocketHole in this.rocketHoles)
				{
					tankRocketHole.gameObject.SetActive(false);
				}
			}
		}
		else if (!this.hidingHoles && this.currentTurnFrame > 0)
		{
			this.hidingHoles = true;
			foreach (TankRocketHole tankRocketHole2 in this.rocketHoles)
			{
				tankRocketHole2.gameObject.SetActive(false);
			}
			this.fireIndex = 0;
		}
		else if (this.hidingHoles && this.currentTurnFrame == 0)
		{
			this.hidingHoles = false;
			foreach (TankRocketHole tankRocketHole3 in this.rocketHoles)
			{
				tankRocketHole3.gameObject.SetActive(true);
			}
		}
		if (this.fire)
		{
			if (this.flapFrame < 5)
			{
				this.flapCounter += this.t;
				if (this.flapCounter > 0.0667f)
				{
					this.flapCounter -= 0.0667f;
					this.flapFrame++;
					this.flap1.SetLowerLeftPixel((float)(32 * this.flapFrame), 64f);
					this.flap2.SetLowerLeftPixel((float)(32 * this.flapFrame), 64f);
				}
			}
		}
		else if (this.flapFrame > 0)
		{
			this.flapCounter += this.t;
			if (this.flapCounter > 0.0667f)
			{
				this.flapCounter -= 0.0667f;
				this.flapFrame--;
				this.flap1.SetLowerLeftPixel((float)(32 * this.flapFrame), 64f);
				this.flap2.SetLowerLeftPixel((float)(32 * this.flapFrame), 64f);
			}
		}
	}

	public override void SetTargetPlayerNum(int pN, Vector3 TargetPosition)
	{
		this.targetPlayerNum = pN;
	}

	protected override void RunFiring()
	{
		if (this.fire && !this.wasFire)
		{
			this.fireDelay = 1.1f;
		}
		base.RunFiring();
	}

	protected override void FireWeapon(ref int index)
	{
		if (index < this.rocketHoles.Length)
		{
			this.rocketHoles[index].Fire();
			if (this.IsLocalMook)
			{
				Projectile projectile = ProjectileController.SpawnProjectileOverNetwork(this.projectile, this, this.rocketHoles[index].transform.position.x + (float)(this.mech.facingDirection * 5), this.rocketHoles[index].transform.position.y + 1f, (float)(this.mech.facingDirection * 90), 0f, false, -1, false, true);
				if (this.targetPlayerNum >= 0)
				{
					Networking.RPC<float, float, int>(PID.TargetAll, true, false, false, new RpcSignature<float, float, int>(projectile.Target), projectile.transform.position.x + (float)(this.mech.facingDirection * 800), projectile.transform.position.y, this.targetPlayerNum);
				}
			}
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.attackSounds, 0.66f, this.rocketHoles[index].transform.position);
		}
		index++;
		if (index >= this.rocketHoles.Length)
		{
			this.fire = false;
			this.fireDelay = 1.1f;
			this.fireIndex = 0;
			this.flapCounter = -0.3f;
		}
	}

	public TankRocketHole[] rocketHoles;

	protected bool hidingHoles;

	protected int targetPlayerNum = -1;

	public SpriteSM flap1;

	public SpriteSM flap2;

	protected int flapFrame;

	protected float flapCounter;
}
