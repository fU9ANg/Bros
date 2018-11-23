// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Nebro : Blade
{
	protected override void Start()
	{
		this.minDashTapTime = 0.33f;
		this.materialNormal = base.GetComponent<Renderer>().material;
		base.Start();
	}

	protected override void Jump(bool wallJump)
	{
		if (this.holdingHighFive && this.airdashUpAvailable)
		{
			this.up = true;
			base.Airdash(true);
		}
		else
		{
			base.Jump(wallJump);
		}
	}

	protected override void AnimateIdle()
	{
		if (this.stampDelay > 0f)
		{
			this.DeactivateGun();
			this.frameRate = 0.066f;
			this.sprite.SetLowerLeftPixel((float)((18 + Mathf.Clamp(this.frame, 0, 3)) * this.spritePixelWidth), 96f);
			if (this.frame >= 7)
			{
				this.stampDelay = 0f;
			}
		}
		else
		{
			base.AnimateIdle();
		}
	}

	protected override void Update()
	{
		base.Update();
		this.stampDelay -= this.t;
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		if (this.attachedToZipline != null)
		{
			this.attachedToZipline.DetachUnit(this);
			if (base.transform.localScale.x > 0f)
			{
				this.AirDashRight();
			}
			else
			{
				this.AirDashLeft();
			}
			return;
		}
		Map.HurtWildLife(x + base.transform.localScale.x * 13f, y + 5f, 12f);
		this.gunFrame = 1;
		this.punchingIndex++;
		this.gunCounter = 0f;
		this.SetGunFrame();
		float num = base.transform.localScale.x * 12f;
		this.ConstrainToFragileBarriers(ref num, 16f);
		if (Physics.Raycast(new Vector3(x - Mathf.Sign(base.transform.localScale.x) * 12f, y + 5.5f, 0f), new Vector3(base.transform.localScale.x, 0f, 0f), out this.raycastHit, 19f, this.groundLayer) || Physics.Raycast(new Vector3(x - Mathf.Sign(base.transform.localScale.x) * 12f, y + 10.5f, 0f), new Vector3(base.transform.localScale.x, 0f, 0f), out this.raycastHit, 19f, this.groundLayer))
		{
			this.MakeEffects(this.raycastHit.point.x + base.transform.localScale.x * 4f, this.raycastHit.point.y);
			MapController.Damage_Local(this, this.raycastHit.collider.gameObject, 9, DamageType.Bullet, this.xI + base.transform.localScale.x * 200f, 0f);
			this.hasHitWithWall = true;
			if (Map.HitUnits(this, this.playerNum, 5, DamageType.Melee, 6f, x, y, base.transform.localScale.x * 520f, 460f, false, true, false, ref this.alreadyHit))
			{
				this.hasHitWithSlice = true;
			}
			else
			{
				this.hasHitWithSlice = false;
			}
			Map.DisturbWildLife(x, y, 80f, this.playerNum);
		}
		else
		{
			this.hasHitWithWall = false;
			if (Map.HitUnits(this, this, this.playerNum, 5, DamageType.Melee, 12f, 9f, x, y, base.transform.localScale.x * 520f, 460f, false, true, false))
			{
				this.hasHitWithSlice = true;
			}
			else
			{
				this.hasHitWithSlice = false;
			}
		}
	}

	protected override void MakeEffects(float x, float y)
	{
		EffectsController.CreateWhiteFlashPopSmall(x, y);
	}

	protected override void PlayAidDashSound()
	{
		this.PlaySpecialAttackSound(1f);
	}

	protected override void PlayAidDashChargeUpSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.soundHolder != null && this.sound != null)
		{
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.panic, 0.6f, base.transform.position);
		}
	}

	protected override void UseSpecial()
	{
		if (base.SpecialAmmo > 0 && Time.time - this.lastReturnZoneTime > 0.33f && (this.currentZone == null || this.currentZone.life <= 0f))
		{
			base.SetInvulnerable(0.2f, false);
			base.SpecialAmmo--;
			HeroController.SetSpecialAmmo(this.playerNum, base.SpecialAmmo);
			this.currentZone = (UnityEngine.Object.Instantiate(this.returnZonePrefab, base.transform.position + Vector3.right * base.transform.localScale.x * 10f + Vector3.up * 7f, Quaternion.identity) as ProjectileReturnZone);
			this.currentZone.playerNum = this.playerNum;
			this.currentZone.transform.parent = base.transform;
		}
	}

	protected override void ReleaseSpecial()
	{
		if (this.currentZone != null)
		{
			this.currentZone.life = Mathf.Clamp(this.currentZone.life - 1f, 0.01f, this.returnZonePrefab.life);
		}
		else
		{
			UnityEngine.Debug.Log("No Current Zone");
		}
	}

	protected override void AnimateSpecial()
	{
		this.SetSpriteOffset(0f, 0f);
		this.DeactivateGun();
		this.frameRate = 0.0334f;
		int num = 16 + Mathf.Clamp(this.frame, 0, 4);
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), 32f);
		if (this.frame == 2)
		{
			this.UseSpecial();
		}
		if (this.frame >= 4)
		{
			this.frame = 0;
			this.ActivateGun();
			this.usingSpecial = false;
		}
	}

	protected override void PressHighFiveMelee(bool forceHighFive = false)
	{
		if (this.up && this.CanAirDash(DirectionEnum.Up))
		{
			if (!this.wasHighFive)
			{
				base.Airdash(true);
			}
		}
		else if (this.right && this.CanAirDash(DirectionEnum.Right))
		{
			if (!this.wasHighFive)
			{
				base.Airdash(true);
			}
		}
		else if (this.left && this.CanAirDash(DirectionEnum.Left))
		{
			if (!this.wasHighFive)
			{
				base.Airdash(true);
			}
		}
		else if (this.down && this.CanAirDash(DirectionEnum.Down))
		{
			if (!this.wasHighFive)
			{
				base.Airdash(true);
			}
		}
		else if (this.airdashTime <= 0f)
		{
			base.PressHighFiveMelee(false);
		}
	}

	protected override void RunLeftAirDash()
	{
		base.RunLeftAirDash();
		this.specialAttackDashCounter += this.t;
		if (this.specialAttackDashCounter > 0f)
		{
			this.specialAttackDashCounter -= 0.0333f;
			Map.HitUnits(this, this, this.playerNum, 1, DamageType.Crush, 9f, this.x, this.y, this.xI * 0.3f, 500f + UnityEngine.Random.value * 200f, true, true);
		}
		if (this.airDashDelay <= 0f)
		{
			this.airdashFadeCounter += Time.deltaTime;
			if (this.airdashFadeCounter > this.airdashFadeRate)
			{
				this.airdashFadeCounter -= this.airdashFadeRate;
				base.CreateFaderTrailInstance();
			}
		}
	}

	protected override void SetGunSprite(int spriteFrame, int spriteRow)
	{
		if (this.actionState == ActionState.ClimbingLadder && this.hangingOneArmed)
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * (11 + spriteFrame)), (float)(this.gunSpritePixelHeight * (1 + spriteRow)));
		}
		else if (this.attachedToZipline != null && this.actionState == ActionState.Jumping)
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * 11), (float)(this.gunSpritePixelHeight * 2));
		}
		else
		{
			base.SetGunSprite(spriteFrame, spriteRow);
		}
	}

	protected override void RunRightAirDash()
	{
		base.RunRightAirDash();
		this.specialAttackDashCounter += this.t;
		if (this.specialAttackDashCounter > 0f)
		{
			this.specialAttackDashCounter -= 0.0333f;
			Map.HitUnits(this, this, this.playerNum, 1, DamageType.Crush, 9f, this.x, this.y, this.xI * 0.3f, 500f + UnityEngine.Random.value * 200f, true, true);
		}
		if (this.airDashDelay <= 0f)
		{
			this.airdashFadeCounter += Time.deltaTime;
			if (this.airdashFadeCounter > this.airdashFadeRate)
			{
				this.airdashFadeCounter -= this.airdashFadeRate;
				base.CreateFaderTrailInstance();
			}
		}
	}

	protected override void RunDownwardDash()
	{
		base.RunDownwardDash();
		this.specialAttackDashCounter += this.t;
		if (this.specialAttackDashCounter > 0f)
		{
			this.specialAttackDashCounter -= 0.0333f;
			Map.HitUnits(this, this, this.playerNum, 3, DamageType.Crush, 9f, this.x, this.y - 5f, 0f, 200f, true, true);
		}
		if (this.yI < -100f)
		{
			this.airdashFadeCounter += Time.deltaTime;
			if (this.airdashFadeCounter > this.airdashFadeRate)
			{
				this.airdashFadeCounter -= this.airdashFadeRate;
				base.CreateFaderTrailInstance();
			}
			if (this.y < this.groundHeight + 30f)
			{
				base.SetInvulnerable(0.35f, false);
			}
		}
	}

	protected override void HitCeiling(RaycastHit raycastHit)
	{
		if (this.airdashTime > 0f && this.airdashDirection == DirectionEnum.Up)
		{
			this.MakeDashBlast(this.x, this.y + this.headHeight + 5f, false);
			this.airdashTime = 0f;
		}
		base.HitCeiling(raycastHit);
	}

	protected override void RunUpwardDash()
	{
		if (this.specialAttackDashCounter > 0f)
		{
			this.specialAttackDashCounter -= 0.0333f;
			Map.HitUnits(this, this, this.playerNum, 3, DamageType.Crush, 9f, this.x, this.y + 8f, 0f, 200f, true, true);
		}
		base.RunUpwardDash();
		this.airdashFadeCounter += Time.deltaTime;
		if (this.airdashFadeCounter > this.airdashFadeRate)
		{
			this.airdashFadeCounter -= this.airdashFadeRate;
			base.CreateFaderTrailInstance();
		}
	}

	protected override void RunGun()
	{
		if (this.specialAttackDashTime > 0f)
		{
			this.gunFrame = 11;
			this.SetGunFrame();
		}
		else if (!this.WallDrag)
		{
			if (this.gunFrame > 0)
			{
				base.GetComponent<Renderer>().material = this.materialArmless;
				if (this.deflectProjectilesCounter > 0f)
				{
					base.DeflectProjectiles();
				}
				this.gunCounter += this.t;
				if (this.gunCounter > 0.0334f)
				{
					this.gunCounter -= 0.0334f;
					this.gunFrame++;
					if (this.gunFrame >= 6)
					{
						this.gunFrame = 0;
						this.SetGunFrame();
					}
					else
					{
						this.SetGunFrame();
					}
					if (this.gunFrame == 2)
					{
						if (this.hasHitWithSlice)
						{
							base.PlaySliceSound();
						}
						else if (this.hasHitWithWall)
						{
							base.PlayWallSound();
						}
					}
				}
			}
			else if (this.currentZone != null)
			{
				this.gunSprite.SetLowerLeftPixel(0f, 128f);
			}
		}
		if (!this.gunSprite.gameObject.activeSelf || this.gunFrame == 0)
		{
			base.GetComponent<Renderer>().material = this.materialNormal;
		}
	}

	protected void SetGunFrame()
	{
		if (!this.ducking)
		{
			int num = this.punchingIndex % 2;
			if (num != 0)
			{
				if (num == 1)
				{
					this.gunSprite.SetLowerLeftPixel((float)(32 * (5 + this.gunFrame)), 32f);
				}
			}
			else
			{
				this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
			}
		}
		else
		{
			int num = this.punchingIndex % 2;
			if (num != 0)
			{
				if (num == 1)
				{
					this.gunSprite.SetLowerLeftPixel((float)(32 * (15 + this.gunFrame)), 32f);
				}
			}
			else
			{
				this.gunSprite.SetLowerLeftPixel((float)(32 * (10 + this.gunFrame)), 32f);
			}
		}
	}

	protected override void SetGunPosition(float xOffset, float yOffset)
	{
		this.gunSprite.transform.localPosition = new Vector3(xOffset + 4f, yOffset, -1f);
	}

	protected override void HitRightWall()
	{
		if (this.airdashTime > 0f && this.airdashDirection == DirectionEnum.Right)
		{
			this.MakeDashBlast(this.x + 7f, this.y + 5f, true);
			this.xIBlast = -100f;
			this.yI += 50f;
			this.airdashDirection = DirectionEnum.Any;
			this.airdashTime = 0f;
		}
	}

	protected override void HitLeftWall()
	{
		if (this.airdashTime > 0f && this.airdashDirection == DirectionEnum.Left)
		{
			this.MakeDashBlast(this.x - 7f, this.y + 5f, true);
			this.xIBlast = 100f;
			this.yI += 50f;
			this.airdashDirection = DirectionEnum.Any;
			this.airdashTime = 0f;
		}
	}

	protected override void Land()
	{
		base.SetAirdashAvailable();
		if (this.airdashTime > 0f && this.airdashDirection == DirectionEnum.Down)
		{
			this.MakeDashBlast(this.x, this.groundHeight, true);
			float groundHeightGround = this.GetGroundHeightGround();
			if (Mathf.Abs(groundHeightGround - this.y) < 24f)
			{
				this.y = groundHeightGround;
				this.groundHeight = groundHeightGround;
			}
			this.stampDelay = 0.4f;
			this.frame = -3;
			base.SetInvulnerable(this.downSlamInvulnerabilityTime, false);
		}
		base.Land();
	}

	protected void MakeDashBlast(float xPoint, float yPoint, bool groundWave)
	{
		Map.ExplodeUnits(this, 25, DamageType.Crush, 64f, 20f, xPoint, yPoint, 300f, 240f, this.playerNum, false, false);
		MapController.DamageGround(this, 15, DamageType.Explosion, 25f, xPoint, yPoint, null);
		EffectsController.CreateWhiteFlashPop(xPoint, yPoint);
		if (groundWave)
		{
			EffectsController.CreateGroundWave(xPoint, yPoint + 1f, 80f);
			Map.ShakeTrees(this.x, this.y, 64f, 32f, 64f);
		}
		Map.DisturbWildLife(this.x, this.y, 48f, this.playerNum);
	}

	protected int punchingIndex;

	protected float stampDelay;

	public ProjectileReturnZone returnZonePrefab;

	public Material materialArmless;

	protected Material materialNormal;

	public FaderSprite faderSpriteRightPrefab;

	public FaderSprite faderSpriteUpPrefab;

	public FaderSprite faderSpriteDownPrefab;

	protected float airdashFadeCounter;

	public float airdashFadeRate = 0.1f;

	protected float lastReturnZoneTime;

	protected ProjectileReturnZone currentZone;

	public float downSlamInvulnerabilityTime = 0.33f;
}
