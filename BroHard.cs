// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BroHard : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void UseFire()
	{
		this.FireWeapon(this.x + base.transform.localScale.x * ((!this.frontgun) ? 4.5f : 12.5f), this.y + 8.5f, base.transform.localScale.x * 450f, 0f);
		this.PlayAttackSound();
		Map.DisturbWildLife(this.x, this.y, 80f, this.playerNum);
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		if (this.attachedToZipline == null)
		{
			this.gunFrame = ((!this.frontgun) ? 6 : 3);
		}
		else
		{
			this.gunFrame = 3;
		}
		this.SetGunSprite(this.gunFrame, 0);
		Vector3 vector = global::Math.Point3OnCircle(UnityEngine.Random.value * 0.2f - 0.05f, 450f);
		if (xSpeed < 0f)
		{
			vector.x = Mathf.Abs(vector.x) * -1f;
		}
		else
		{
			vector.x = Mathf.Abs(vector.x);
		}
		EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, base.transform);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, vector.x * 1f, vector.y, this.playerNum);
		if (this.attachedToZipline == null)
		{
			this.frontgun = !this.frontgun;
		}
		else
		{
			this.frontgun = false;
		}
	}

	protected override void RunGun()
	{
		if (!this.WallDrag && this.gunFrame > 0)
		{
			this.gunCounter += this.t;
			if (this.gunCounter > 0.0334f)
			{
				this.gunCounter -= 0.0334f;
				this.gunFrame--;
				if (this.gunFrame == 3)
				{
					this.gunFrame = 0;
				}
				this.SetGunSprite(this.gunFrame, 0);
			}
		}
	}

	protected override void SetGunSprite(int spriteFrame, int spriteRow)
	{
		if (this.actionState == ActionState.ClimbingLadder && this.hangingOneArmed)
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * (9 + spriteFrame)), (float)(this.gunSpritePixelHeight * (1 + spriteRow)));
		}
		else if (this.attachedToZipline != null && this.actionState == ActionState.Jumping)
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * (9 + spriteFrame)), (float)(this.gunSpritePixelHeight * (1 + spriteRow)));
		}
		else
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * spriteFrame), (float)(this.gunSpritePixelHeight * (1 + spriteRow)));
		}
	}

	private bool frontgun;
}
