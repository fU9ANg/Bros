// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class EllenRipbro : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void UseFire()
	{
		this.FireWeapon(this.x + base.transform.localScale.x * 12f, this.y + 7.5f, base.transform.localScale.x * 700f, 0f);
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.attackSounds, 0.6f, base.transform.position, 0.95f + UnityEngine.Random.value * 0.1f);
		Map.DisturbWildLife(this.x, this.y, 80f, this.playerNum);
	}

	protected override void AnimateSpecial()
	{
		this.SetSpriteOffset(0f, 0f);
		this.DeactivateGun();
		this.frameRate = 0.0334f;
		int num = 0 + Mathf.Clamp(this.frame, 0, 6);
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 7));
		if (this.frame == 3)
		{
			this.UseSpecial();
		}
		if (this.frame >= 6)
		{
			this.frame = 0;
			this.usingSpecial = false;
			this.ActivateGun();
			this.ChangeFrame();
		}
	}

	protected override void PressSpecial()
	{
		if (base.SpecialAmmo > 0)
		{
			this.usingSpecial = true;
			this.frame = 0;
			this.ChangeFrame();
			this.counter = 0f;
		}
		else
		{
			HeroController.FlashSpecialAmmo(this.playerNum);
		}
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 3;
		this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
		EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, base.transform);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed, ySpeed, this.playerNum);
	}

	protected override void UseSpecial()
	{
		if (base.SpecialAmmo > 0)
		{
			base.SpecialAmmo--;
			FlameWallExplosion @object = Networking.Instantiate<FlameWallExplosion>(this.flameWavePrefab, new Vector3(this.x + base.transform.localScale.x * 5f, this.y + 9f, 0f), Quaternion.identity, null, false);
			DirectionEnum arg;
			if (this.right)
			{
				arg = DirectionEnum.Right;
			}
			else if (this.left)
			{
				arg = DirectionEnum.Left;
			}
			else if (base.transform.localScale.x > 0f)
			{
				arg = DirectionEnum.Right;
			}
			else
			{
				arg = DirectionEnum.Left;
			}
			Networking.RPC<int, EllenRipbro, DirectionEnum>(PID.TargetAll, new RpcSignature<int, EllenRipbro, DirectionEnum>(@object.Setup), this.playerNum, this, arg, false);
		}
		else
		{
			HeroController.FlashSpecialAmmo(this.playerNum);
			this.ActivateGun();
		}
	}

	public FlameWallExplosion flameWavePrefab;
}
