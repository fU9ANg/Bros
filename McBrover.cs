// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class McBrover : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void UseFire()
	{
		this.FireWeapon(this.x + base.transform.localScale.x * 6f, this.y + 6.5f, base.transform.localScale.x * 100f + this.xI * 0.7f, 100f + ((this.yI <= 0f) ? 0f : (this.yI * 0.5f)));
		this.fireDelay = 0.3f;
		this.PlayAttackSound();
		Map.DisturbWildLife(this.x, this.y, 20f, this.playerNum);
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 3;
		this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
		if (base.IsMine)
		{
			ProjectileController.SpawnProjectileOverNetwork(this.projectile, this, x, y, xSpeed, ySpeed, true, this.playerNum, false, false);
		}
	}

	protected override void UseSpecial()
	{
		if (this.currentTurkey != null)
		{
			this.currentTurkey.Death();
		}
		else if (base.SpecialAmmo > 0)
		{
			this.PlaySpecialAttackSound(0.5f);
			base.SpecialAmmo--;
			HeroController.SetSpecialAmmo(this.playerNum, base.SpecialAmmo);
			if (base.IsMine)
			{
				this.currentTurkey = ProjectileController.SpawnProjectileOverNetwork(this.turkeyProjectile, this, this.x + base.transform.localScale.x * 6f, this.y + 6.5f, base.transform.localScale.x * 100f + this.xI * 0.7f, 100f + ((this.yI <= 0f) ? 0f : (this.yI * 0.5f)), true, this.playerNum, false, false);
			}
			this.usingSpecial = false;
		}
		else
		{
			HeroController.FlashSpecialAmmo(this.playerNum);
			this.gunSprite.gameObject.SetActive(true);
			this.usingSpecial = false;
		}
	}

	public Projectile stickyProjectile;

	public Projectile turkeyProjectile;

	protected Projectile currentTurkey;
}
