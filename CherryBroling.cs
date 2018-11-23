// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CherryBroling : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void Jump(bool wallJump)
	{
		base.Jump(wallJump);
		this.hoverSinCounter = this.hoverSinStart;
		if (this.left)
		{
			this.currentFireDirection = this.fireRightDirection;
		}
		else if (this.right)
		{
			this.currentFireDirection = this.fireLeftDirection;
		}
	}

	protected override bool WallDrag
	{
		get
		{
			return base.WallDrag;
		}
		set
		{
			base.WallDrag = value;
			if (value)
			{
				this.somersaulting = false;
				this.somersaultFrame = 0;
			}
		}
	}

	protected override void Land()
	{
		base.Land();
		this.somersaulting = false;
		this.somersaultFrame = 0;
		this.hoverSinCounter = this.hoverSinStart;
	}

	protected override void AddSpeedLeft()
	{
		float speed = this.speed;
		if (this.actionState == ActionState.Running && !this.ducking)
		{
			int num = this.frame % 8;
			if (num == 4 || num == 5 || num == 6)
			{
				this.speed = speed * 0.4f;
				this.frameRate = this.runningFrameRate * 1.5f;
			}
			else
			{
				this.frameRate = this.runningFrameRate;
			}
			base.AddSpeedLeft();
			base.AddSpeedLeft();
		}
		base.AddSpeedLeft();
		this.speed = speed;
	}

	protected override void AddSpeedRight()
	{
		float speed = this.speed;
		if (this.actionState == ActionState.Running && !this.ducking)
		{
			int num = this.frame % 8;
			if (num == 4 || num == 5 || num == 6)
			{
				this.speed = speed * 0.4f;
				this.frameRate = this.runningFrameRate * 1.5f;
			}
			else
			{
				this.frameRate = this.runningFrameRate;
			}
			base.AddSpeedRight();
			base.AddSpeedRight();
		}
		base.AddSpeedRight();
		this.speed = speed;
	}

	protected override void RunFiring()
	{
		this.fireDelay -= this.t * (1f + ((this.hoverTime <= 0f) ? 0f : 0.2f));
		if (this.fire)
		{
			this.rollingFrames = 0;
		}
		if (this.fire && this.fireDelay <= 0f && !this.somersaulting)
		{
			this.fireCounter += this.t * (1f + ((this.hoverTime <= 0f) ? 0f : 0.2f));
			if (this.fireCounter >= this.fireRate)
			{
				this.fireCounter -= this.fireRate;
				this.UseFire();
				base.FireFlashAvatar();
			}
		}
	}

	protected override void PressSpecial()
	{
		if (base.SpecialAmmo > 0)
		{
			if (this.health > 0)
			{
				if (this.invulnerableTime > 0f)
				{
					this.invulnerableTime = 0f;
					this.invulnerable = false;
				}
				this.doingMelee = false;
				this.UseSpecial();
			}
		}
		else
		{
			HeroController.FlashSpecialAmmo(this.playerNum);
		}
	}

	protected override void AnimateSpecial()
	{
		this.usingSpecial = false;
		UnityEngine.Debug.LogError("Should not be using special");
	}

	protected override void Update()
	{
		this.hoverTime -= this.t;
		if (this.y > this.groundHeight + 1f && this.yI < 70f)
		{
			this.hoverSinCounter = Mathf.Clamp(this.hoverSinCounter - this.t * this.hoverSinSpeed, 0f, 10f);
		}
		if (this.actionState != ActionState.Jumping || this.WallDrag)
		{
			this.somersaulting = false;
			this.somersaultFrame = 0;
		}
		base.Update();
	}

	protected override void UseFire()
	{
		if (this.left)
		{
			if (this.canTouchLeftWalls)
			{
				this.currentFireDirection = Vector3.RotateTowards(this.currentFireDirection, this.fireLeftWallDirection, 0.4f, 45f);
			}
			else if (this.y <= this.groundHeight + 1f)
			{
				this.currentFireDirection = this.fireRightDirection;
			}
			else
			{
				this.currentFireDirection = Vector3.RotateTowards(this.currentFireDirection, this.fireRightDirection, 0.2f, 45f);
			}
			this.FireWeapon(this.x - base.transform.localScale.x * 2.5f + this.currentFireDirection.normalized.x * 9f, this.y - 1f, this.currentFireDirection.x, this.currentFireDirection.y);
		}
		else if (this.right)
		{
			if (this.canTouchRightWalls)
			{
				this.currentFireDirection = Vector3.RotateTowards(this.currentFireDirection, this.fireRightWallDirection, 0.4f, 45f);
			}
			else if (this.y <= this.groundHeight + 1f)
			{
				this.currentFireDirection = this.fireLeftDirection;
			}
			else
			{
				this.currentFireDirection = Vector3.RotateTowards(this.currentFireDirection, this.fireLeftDirection, 0.2f, 45f);
			}
			this.FireWeapon(this.x - base.transform.localScale.x * 2.5f + this.currentFireDirection.normalized.x * 9f, this.y - 1f, this.currentFireDirection.x, this.currentFireDirection.y);
		}
		else
		{
			this.currentFireDirection = Vector3.RotateTowards(this.currentFireDirection, this.fireDownDirection, 0.3f, 45f);
			this.FireWeapon(this.x - base.transform.localScale.x * 2.5f + this.currentFireDirection.normalized.x * 5f, this.y - 1f, this.currentFireDirection.x, this.currentFireDirection.y);
		}
		if (this.yI < 30f)
		{
			this.yI -= this.currentFireDirection.y * (this.defaultHoverForce + this.hoverSinForce * Mathf.Sin(this.hoverSinCounter));
		}
		else if (this.yI < 80f)
		{
			this.yI -= this.currentFireDirection.y * 0.15f;
		}
		else
		{
			this.yI -= this.currentFireDirection.y * 0.05f;
		}
		if ((this.currentFireDirection.x < 0f && this.right) || (this.currentFireDirection.x > 0f && this.left) || (!this.right && !this.left))
		{
			if (this.dashing)
			{
				this.xIBlast -= this.currentFireDirection.x * 0.025f;
			}
			else
			{
				this.xIBlast -= this.currentFireDirection.x * 0.05f;
			}
		}
		this.PlayAttackSound();
		Map.DisturbWildLife(this.x, this.y, 60f, this.playerNum);
	}

	protected override void SetGunPosition(float xOffset, float yOffset)
	{
		this.gunSprite.transform.localPosition = new Vector3(xOffset, yOffset - 2f, -1f);
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 3;
		this.actionState = ActionState.Jumping;
		this.SetGunSprite(this.gunFrame, 0);
		EffectsController.CreateMuzzleFlashRoundEffect(x, y, -25f, xSpeed * 0.04f, ySpeed * 0.04f, base.transform);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed + this.xI * 0.166f, ySpeed + this.yI * 0.33f, this.playerNum);
		this.ActivateGun();
	}

	protected override void ActivateGun()
	{
		if (!this.chimneyFlip && !this.doingMelee && !this.ledgeGrapple && (this.yI > 0f || this.y > this.groundHeight || this.gunFrame > 0 || this.actionState == ActionState.Jumping))
		{
			if (!this.gunSprite.gameObject.activeSelf)
			{
				base.ActivateGun();
				this.SetGunSprite(0, 0);
			}
		}
		else if (this.gunSprite.gameObject.activeSelf)
		{
			this.gunSprite.gameObject.SetActive(false);
			this.SetGunSprite(0, 0);
		}
	}

	protected override void SetGunSprite(int spriteFrame, int spriteRow)
	{
		if (spriteRow > 0)
		{
			spriteFrame = this.gunFrame;
			spriteRow = 0;
		}
		if (Mathf.Abs(this.currentFireDirection.x) < 50f)
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * spriteFrame), (float)(this.gunSpritePixelHeight * (1 + spriteRow)));
		}
		else if (this.currentFireDirection.x * base.transform.localScale.x < 0f)
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * (spriteFrame + 6)), (float)(this.gunSpritePixelHeight * (2 + spriteRow)));
		}
		else if (this.currentFireDirection.x * base.transform.localScale.x > 0f)
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * spriteFrame), (float)(this.gunSpritePixelHeight * (2 + spriteRow)));
		}
	}

	public override void Knock(DamageType damageType, float xI, float yI, bool forceTumble)
	{
		this.xI = Mathf.Clamp(this.xI + xI / 2f, -200f, 200f);
		this.xIBlast = Mathf.Clamp(this.xIBlast + xI / (float)(2 + ((!this.dashing) ? 0 : 1)), -200f, 200f);
		this.yI = Mathf.Clamp(this.yI + yI, -20000f, 400f);
		this.hoverTime = 1f;
	}

	protected override void AnimateJumping()
	{
		if (this.somersaulting)
		{
			this.DeactivateGun();
			this.sprite.SetLowerLeftPixel((float)(this.somersaultFrame * this.spritePixelWidth), (float)(8 * this.spritePixelHeight));
			this.somersaultFrame++;
			if (this.somersaultFrame > 10)
			{
				this.somersaulting = false;
			}
			this.frameRate = 0.04f;
		}
		else
		{
			base.AnimateJumping();
		}
	}

	protected override void UseSpecial()
	{
		if (base.SpecialAmmo > 0)
		{
			this.PlayThrowSound(0.4f);
			base.SpecialAmmo--;
			HeroController.SetSpecialAmmo(this.playerNum, base.SpecialAmmo);
			this.somersaulting = true;
			this.somersaultFrame = 0;
			this.actionState = ActionState.Jumping;
			this.AnimateJumping();
			if (base.IsMine)
			{
				if (this.left)
				{
					this.currentFireDirection = this.fireRightDirection;
				}
				else if (this.right)
				{
					this.currentFireDirection = this.fireLeftDirection;
				}
				else
				{
					this.currentFireDirection = this.fireDownDirection;
				}
				this.yI -= this.currentFireDirection.y * 0.5f;
				this.xIBlast -= this.currentFireDirection.x * 0.1f;
				ProjectileController.SpawnProjectileOverNetwork(this.rocketGrenade, this, this.x, this.y + 4f, this.currentFireDirection.x * 1.2f, this.currentFireDirection.y * 1.2f, false, this.playerNum, false, false);
			}
		}
		else
		{
			HeroController.FlashSpecialAmmo(this.playerNum);
			this.ActivateGun();
		}
	}

	protected Vector3 currentFireDirection = new Vector3(0f, -370f, 0f);

	public Rocket rocketGrenade;

	public Vector3 fireLeftDirection = new Vector3(-280f, -240f, 0f);

	public Vector3 fireLeftWallDirection = new Vector3(-330f, -160f, 0f);

	public Vector3 fireDownDirection = new Vector3(0f, -370f, 0f);

	public Vector3 fireRightDirection = new Vector3(280f, -240f, 0f);

	public Vector3 fireRightWallDirection = new Vector3(330f, -160f, 0f);

	protected float hoverSinCounter;

	public float hoverSinSpeed = 5f;

	public float hoverSinStart = 4.8f;

	public float hoverSinForce = 0.12f;

	public float defaultHoverForce = 0.2f;

	protected bool somersaulting;

	protected int somersaultFrame;

	protected float hoverTime;
}
