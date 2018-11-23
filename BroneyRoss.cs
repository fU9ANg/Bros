// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BroneyRoss : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void SetGunPosition(float xOffset, float yOffset)
	{
		this.gunSprite.transform.localPosition = new Vector3(xOffset + 2f, yOffset, 0f);
	}

	protected override void UseFire()
	{
		this.FireWeapon(this.x + base.transform.localScale.x * ((!this.frontgun) ? 5.5f : 16.5f), this.y + 9.5f, base.transform.localScale.x * 450f, 0f);
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

	protected override void UseSpecial()
	{
		if (base.SpecialAmmo > 0)
		{
			this.PlayThrowSound(0.4f);
			if (this.grenadesThrown == 2)
			{
				base.SpecialAmmo--;
			}
			if (base.IsMine)
			{
				Grenade grenade = null;
				if (this.grenadesThrown == 2)
				{
					grenade = ProjectileController.SpawnGrenadeOverNetwork(this.specialGrenade, this, this.x + Mathf.Sign(base.transform.localScale.x) * 8f, this.y + 8f, 0.001f, 0.011f, Mathf.Sign(base.transform.localScale.x) * 200f, 155f, this.playerNum);
				}
				if (this.grenadesThrown == 1)
				{
					grenade = ProjectileController.SpawnGrenadeOverNetwork(this.specialGrenade, this, this.x + Mathf.Sign(base.transform.localScale.x) * 8f, this.y + 8f, 0.001f, 0.011f, Mathf.Sign(base.transform.localScale.x) * 160f, 130f, this.playerNum);
				}
				if (this.grenadesThrown == 0)
				{
					grenade = ProjectileController.SpawnGrenadeOverNetwork(this.specialGrenade, this, this.x + Mathf.Sign(base.transform.localScale.x) * 8f, this.y + 8f, 0.001f, 0.011f, Mathf.Sign(base.transform.localScale.x) * 115f, 105f, this.playerNum);
				}
				if (grenade != null)
				{
					GrenadeSticky component = grenade.GetComponent<GrenadeSticky>();
					if (component != null)
					{
						component.stickGrenadeSwarmIndex = this.grenadesThrown;
					}
				}
			}
		}
		else
		{
			HeroController.FlashSpecialAmmo(this.playerNum);
			this.ActivateGun();
		}
	}

	protected override void AnimateSpecial()
	{
		this.SetSpriteOffset(0f, 0f);
		this.DeactivateGun();
		this.frameRate = 0.0334f;
		int num = 17 + Mathf.Clamp(this.frame, 0, 7);
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 5));
		if (this.frame == 4)
		{
			this.UseSpecial();
			this.grenadesThrown++;
			if (this.grenadesThrown < 3)
			{
				this.frame = 1;
			}
		}
		if (this.frame >= 7)
		{
			this.frame = 0;
			this.usingSpecial = false;
			this.ActivateGun();
			this.ChangeFrame();
			this.grenadesThrown = 0;
		}
	}

	private int grenadesThrown;

	private bool frontgun;
}
