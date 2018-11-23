// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class LeeBroxmas : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void UseFire()
	{
		this.gunFrame = 9;
		this.SetGunSprite(this.gunFrame, 0);
		if (this.fireCount % 3 == 0)
		{
			this.fireDelay = 0.42f;
		}
	}

	protected override void StartFiring()
	{
		if (this.gunFrame <= 3)
		{
			this.fireDelay = -0.01f;
		}
		else
		{
			this.fireDelay = 0.5f;
		}
		base.StartFiring();
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		float num = 0f;
		if (this.fireCount % 3 == 0)
		{
			num = 20f;
		}
		if (this.fireCount % 3 == 2)
		{
			num = -20f;
		}
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed, ySpeed + num, this.playerNum);
	}

	protected override void RunGun()
	{
		if (this.gunFrame > 0)
		{
			this.gunCounter += this.t;
			if (this.gunCounter > 0.0334f)
			{
				this.gunCounter -= 0.0334f;
				this.gunFrame--;
				this.SetGunSprite(this.gunFrame, 0);
				if (this.gunFrame == 7 || this.gunFrame == 3 || this.gunFrame == 5)
				{
					this.FireWeapon(this.x + base.transform.localScale.x * 10f, this.y + 8f + (float)UnityEngine.Random.Range(0, 3), base.transform.localScale.x * this.knifeSpeed, 0f);
					this.PlayAttackSound(0.45f);
					Map.DisturbWildLife(this.x, this.y, 30f, this.playerNum);
					this.fireCount++;
					this.gunCounter -= 0.015f;
				}
			}
		}
	}

	protected void ThrowMachete(Projectile prefab, float x, float y, float xSpeed, float ySpeed)
	{
		this.PlayAttackSound(0.44f);
		ProjectileController.SpawnProjectileLocally(prefab, this, x, y, xSpeed, ySpeed, this.playerNum);
	}

	protected override void AnimatePushing()
	{
		this.frameRate = this.runningFrameRate * 3f;
		int num = 0 + this.frame % 5;
		if (this.frame % 4 == 0)
		{
			this.PlayFootStepSound();
		}
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 5));
		if (this.gunFrame <= 0)
		{
			this.SetGunSprite(num, 1);
		}
		this.SetGunPosition(1f, 0f);
		this.gunSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
	}

	protected override void PressSpecial()
	{
		if (this.usingSpecial || this.health <= 0)
		{
			return;
		}
		base.PressSpecial();
		if (base.SpecialAmmo > 0)
		{
			this.knifeSprayCountLeft = this.knifeSprayCount;
			this.knifeSprayDirection = this.GetKnifeSprayDirection();
			this.lastKnifeSprayAngle = (this.knifeSprayAngle = this.GetKnifeSprayAngle(true));
			this.fireCount = -1;
		}
		else
		{
			this.knifeSprayCountLeft = 0;
		}
	}

	protected override void SetGunPosition(float xOffset, float yOffset)
	{
		this.gunSprite.transform.localPosition = new Vector3(5f + xOffset, yOffset, -0.001f);
		this.gunSprite.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	protected void AnimateSpecial4Frame()
	{
		if (this.frame == 1 && base.SpecialAmmo > 0)
		{
			this.UseSpecial();
		}
		if (this.frame == 2 && base.SpecialAmmo > 0)
		{
			this.UseSpecial();
		}
		if (this.frame == 3 && base.SpecialAmmo > 0)
		{
			this.UseSpecial();
		}
		if (this.frame >= 4 && this.knifeSprayCountLeft > 0)
		{
			this.frame = 0;
			this.knifeSprayCountLeft--;
			this.frameRate = 0.02f;
		}
		if (this.frame >= 5)
		{
			this.Stop();
			this.frame = 0;
			this.ActivateGun();
			this.usingSpecial = false;
			if (this.y < this.groundHeight + 1f)
			{
				this.actionState = ActionState.Idle;
			}
			else
			{
				this.actionState = ActionState.Jumping;
			}
			this.ChangeFrame();
		}
	}

	protected void AnimateSpecial6Frame()
	{
		if (this.frame == 2 && base.SpecialAmmo > 0)
		{
			this.UseSpecial();
		}
		if (this.frame == 3 && base.SpecialAmmo > 0)
		{
			this.UseSpecial();
		}
		if (this.frame == 5 && base.SpecialAmmo > 0)
		{
			this.UseSpecial();
		}
		if (this.frame >= 6 && this.knifeSprayCountLeft > 0)
		{
			this.frame = 0;
			this.knifeSprayCountLeft--;
			this.frameRate = 0.02f;
		}
		if (this.frame >= 6)
		{
			base.SpecialAmmo--;
			HeroController.SetSpecialAmmo(this.playerNum, base.SpecialAmmo);
			this.frame = 0;
			this.ActivateGun();
			this.usingSpecial = false;
			this.ChangeFrame();
		}
	}

	protected override void Update()
	{
		base.Update();
		if (Application.isEditor && (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Minus)))
		{
			this.test6Frames = !this.test6Frames;
			base.SpecialAmmo = 4;
		}
	}

	protected override void AnimateSpecial()
	{
		this.SetSpriteOffset(0f, 0f);
		this.DeactivateGun();
		this.frameRate = 0.0334f;
		Vector3 vector = global::Math.Point3OnCircle((720f + this.lastKnifeSprayAngle) / 180f * 3.14159274f, 1f);
		if (Mathf.Abs(vector.x) * 0.25f > Mathf.Abs(vector.y))
		{
			if (this.test6Frames)
			{
				this.AnimateSpecial6Frame();
				this.frameRate = 0.0222f;
				int num = 0 + Mathf.Clamp(this.frame, 0, 5);
				this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 7));
			}
			else
			{
				this.AnimateSpecial4Frame();
				int num2 = 0 + Mathf.Clamp(this.frame, 0, 3);
				this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)(this.spritePixelHeight * 8));
			}
		}
		else if (Mathf.Abs(vector.x) * 1.3f > Mathf.Abs(vector.y))
		{
			this.AnimateSpecial4Frame();
			if (vector.y > 0f)
			{
				int num3 = 4 + Mathf.Clamp(this.frame, 0, 3);
				this.sprite.SetLowerLeftPixel((float)(num3 * this.spritePixelWidth), (float)(this.spritePixelHeight * 8));
			}
			else
			{
				int num4 = 12 + Mathf.Clamp(this.frame, 0, 3);
				this.sprite.SetLowerLeftPixel((float)(num4 * this.spritePixelWidth), (float)(this.spritePixelHeight * 8));
			}
		}
		else
		{
			this.AnimateSpecial4Frame();
			if (vector.y > 0f)
			{
				int num5 = 8 + Mathf.Clamp(this.frame, 0, 3);
				this.sprite.SetLowerLeftPixel((float)(num5 * this.spritePixelWidth), (float)(this.spritePixelHeight * 8));
			}
			else
			{
				int num6 = 16 + Mathf.Clamp(this.frame, 0, 3);
				this.sprite.SetLowerLeftPixel((float)(num6 * this.spritePixelWidth), (float)(this.spritePixelHeight * 8));
			}
		}
	}

	public override void Stop()
	{
		if (this.usingSpecial)
		{
			this.usingSpecial = false;
			base.SpecialAmmo--;
			HeroController.SetSpecialAmmo(this.playerNum, base.SpecialAmmo);
		}
		base.Stop();
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		base.Stop();
		base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		base.Stop();
		base.Death(xI, yI, damage);
	}

	protected Vector3 GetKnifeSprayDirection()
	{
		Vector3 vector = Vector3.zero;
		if (this.right)
		{
			vector += Vector3.right;
		}
		if (this.left)
		{
			vector += Vector3.left;
		}
		if (this.up)
		{
			vector += Vector3.up;
		}
		if (this.down)
		{
			vector += Vector3.down;
		}
		if (vector.x == 0f && vector.y == 0f)
		{
			vector += new Vector3(base.transform.localScale.x, 0f, 0f);
		}
		return vector;
	}

	protected float GetKnifeSprayAngle(bool useFacing)
	{
		float result;
		if (base.transform.localScale.x > 0f)
		{
			result = 45f;
		}
		else
		{
			result = 135f;
		}
		return result;
	}

	protected override void CalculateMovement()
	{
		if (this.usingSpecial && base.SpecialAmmo > 0)
		{
			this.xI = 0f;
			this.yI = 0f;
		}
		else
		{
			base.CalculateMovement();
		}
	}

	protected override void UseSpecial()
	{
		this.fireCount++;
		this.knifeSprayAngle = this.GetKnifeSprayAngle(false);
		this.knifeSprayDirection = global::Math.Point3OnCircle((720f + this.knifeSprayAngle - 18.25f + 9.5f * (float)(this.fireCount % 3) + 4.5f * (float)(this.fireCount % 6)) / 180f * 3.14159274f, 1f);
		this.ThrowMachete(this.macheteSprayProjectile, this.x + this.knifeSprayDirection.x * 10f, this.y + this.knifeSprayDirection.y * 8f + 8f, Mathf.Sign(this.knifeSprayDirection.x) * this.knifeSpraySpeed * UnityEngine.Random.Range(0.95f, 1.125f), this.knifeSprayDirection.y * this.knifeSpraySpeed * UnityEngine.Random.Range(0.9f, 1.125f));
		this.lastKnifeSprayAngle = this.knifeSprayAngle;
	}

	protected override void ReleaseFire()
	{
		this.fireCount = 0;
		base.ReleaseFire();
	}

	protected override void SetGunSprite(int spriteFrame, int spriteRow)
	{
		if (this.actionState == ActionState.ClimbingLadder && this.hangingOneArmed)
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * (12 + spriteFrame)), (float)(this.gunSpritePixelHeight * (1 + spriteRow)));
		}
		else if (this.attachedToZipline != null && this.actionState == ActionState.Jumping)
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * (12 + spriteFrame)), (float)(this.gunSpritePixelHeight * (1 + spriteRow)));
		}
		else
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * spriteFrame), (float)(this.gunSpritePixelHeight * (1 + spriteRow)));
		}
	}

	private int fireCount;

	protected int knifeSprayCountLeft;

	protected Vector3 knifeSprayDirection = Vector3.zero;

	protected float knifeSprayAngle;

	public float knifeSpeed = 270f;

	public float knifeSpraySpeed = 520f;

	public int knifeSprayCount = 12;

	public Projectile macheteSprayProjectile;

	public bool test6Frames;

	protected float lastKnifeSprayAngle;
}
