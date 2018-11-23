// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BossBlockRocketBattery : BossBlockWeapon
{
	protected override void FireWeapon()
	{
		base.GetComponent<Collider>().enabled = false;
		float x = base.transform.position.x + this.fireOffset.x;
		float y = base.transform.position.y + this.fireOffset.y + 8f - (float)(this.weaponFireCount % 3 * 10);
		float x2 = this.fireDirection.x;
		float y2 = this.fireDirection.y;
		float arg = 0f;
		float arg2 = 0f;
		int num = -1;
		HeroController.GetRandomPlayerPos(ref arg, ref arg2, ref num);
		Projectile projectile = ProjectileController.SpawnProjectileOverNetwork(this.projectile, this, x, y, this.fireDirection.x, this.fireDirection.y, false, -1, false, true);
		projectile.zOffset = 13f;
		if (num >= 0)
		{
			Networking.RPC<float, float, int>(PID.TargetAll, new RpcSignature<float, float, int>(projectile.Target), arg, arg2, num, false);
		}
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.attackSounds, 0.5f, new Vector3(x, y, 0f));
		base.GetComponent<Collider>().enabled = true;
	}

	protected override void RunFiring()
	{
		this.frameCounter += this.t;
		if (this.frameCounter >= 0.045f)
		{
			this.frame++;
			this.frameCounter -= 0.045f;
			if (this.frame == this.fireFrame)
			{
				this.FireWeapon();
			}
			if (this.frame == this.fireFramesPerBarrel)
			{
				this.weaponFireCount++;
				if (this.weaponFireCount >= 3)
				{
					this.firing = false;
					this.weaponFireCount = 0;
					this.thinkCounter = this.fireDelay;
					this.frame = this.restFrame;
					this.firing = false;
					this.SetSpriteFrame(this.frame, 0);
				}
				else
				{
					this.frameCounter -= this.delayBetweenFiring;
					this.frame = 0;
					this.SetSpriteFrame(this.restFrame + this.weaponFireCount * this.fireFramesPerBarrel + this.frame, 0);
				}
			}
			else
			{
				this.SetSpriteFrame(this.restFrame + this.weaponFireCount * this.fireFramesPerBarrel + this.frame, 0);
			}
		}
	}

	protected override void StartFiring()
	{
		this.firing = true;
		this.frame = 0;
		this.frameCounter = 0f;
	}

	protected override void SetDeathFrame()
	{
		this.SetSpriteFrame(this.deathFrame, 0);
	}

	protected int weaponFireCount;

	public int fireFramesPerBarrel = 5;

	public float delayBetweenFiring = 0.333f;
}
