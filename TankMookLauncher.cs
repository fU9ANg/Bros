// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TankMookLauncher : TankWeapon
{
	protected override void Update()
	{
		base.Update();
	}

	public override void SetTargetPlayerNum(int pN, Vector3 TargetPosition)
	{
		MonoBehaviour.print(string.Concat(new object[]
		{
			"SetTargetPlayerNum ",
			TargetPosition,
			" ",
			base.transform.position
		}));
		this.targetPlayerNum = pN;
	}

	protected override void RunFiring()
	{
		this.fireDelay -= this.t;
		if (this.fire && this.health > 0 && this.tank.CanFire() && this.fireDelay <= 0f)
		{
			this.fireCounter += this.t;
			if (this.fireCounter > 0f)
			{
				this.fireFrameCounter += this.t;
				if (this.fireFrameCounter > 0.0334f)
				{
					this.fireFrameCounter -= 0.0334f;
					this.fireFrame++;
					if (this.fireFrame == 5)
					{
						this.FireWeapon(ref this.fireIndex);
					}
					if (this.fireFrame >= 8)
					{
						this.sprite.SetLowerLeftPixel(new Vector2(0f, this.sprite.lowerLeftPixel.y));
						this.fireFrame = 0;
						this.fireCounter -= this.fireRate;
						if (this.fireIndex >= this.mookWaveCount)
						{
							this.fire = false;
							this.fireDelay = 2.1f;
							this.fireIndex = 0;
						}
					}
					else
					{
						this.sprite.SetLowerLeftPixel(new Vector2((float)((5 + this.fireFrame) * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
					}
				}
			}
		}
		this.wasFire = this.fire;
	}

	protected override void FireWeapon(ref int index)
	{
		float num = 1f + UnityEngine.Random.value * 0.5f;
		float num2 = 0f;
		float num3 = 0f;
		if (HeroController.PlayerIsAlive(this.targetPlayerNum))
		{
			HeroController.GetPlayerPos(this.targetPlayerNum, ref num2, ref num3);
		}
		else
		{
			this.targetPlayerNum = -1;
		}
		if (num2 > 0f)
		{
			float f = num2 - this.tank.x;
			float num4 = num3 - this.tank.y;
			num = 0.2f + Mathf.Abs(f) / 200f + num4 / 300f;
			if (num < 0.5f)
			{
				num = 0.5f;
			}
		}
		if (this.IsLocalMook)
		{
			MapController.SpawnMook_Networked(this.mookProjectile, this.x + (float)(this.tank.facingDirection * 11), this.y + 12f, (float)(this.tank.facingDirection * 230) * num * (0.9f + UnityEngine.Random.value * 0.2f), 260f * num, true, false, false, false, false);
		}
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.attackSounds, 0.66f, base.transform.position);
		index++;
	}

	protected bool hidingHoles;

	protected int targetPlayerNum = -1;

	public int mookWaveCount = 3;

	public Mook mookProjectile;

	protected int fireFrame;

	protected float fireFrameCounter;
}
