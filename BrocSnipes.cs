// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class BrocSnipes : TestVanDammeAnim
{
	protected override void UseFire()
	{
		this.alreadyHit.Clear();
		this.gunFrame = 6;
		this.hasPlayedAttackHitSound = false;
		this.FireWeapon(this.x + base.transform.localScale.x * 10f, this.y + 6.5f, base.transform.localScale.x * (float)(250 + ((!Demonstration.bulletsAreFast) ? 0 : 150)), (float)(UnityEngine.Random.Range(0, 40) - 20) * ((!Demonstration.bulletsAreFast) ? 0.2f : 1f));
		this.PlayAttackSound();
		Map.DisturbWildLife(this.x, this.y, 60f, this.playerNum);
	}

	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		Map.HurtWildLife(x + base.transform.localScale.x * 13f, y + 5f, 12f);
		this.deflectProjectilesCounter = this.deflectProjectilesEnergy;
		this.deflectProjectilesEnergy = 0f;
		this.DeflectProjectiles();
		this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
		float num = base.transform.localScale.x * 12f;
		this.ConstrainToFragileBarriers(ref num, 16f);
		if (Physics.Raycast(new Vector3(x - Mathf.Sign(base.transform.localScale.x) * 12f, y + 5.5f, 0f), new Vector3(base.transform.localScale.x, 0f, 0f), out this.raycastHit, 19f, this.groundLayer) || Physics.Raycast(new Vector3(x - Mathf.Sign(base.transform.localScale.x) * 12f, y + 10.5f, 0f), new Vector3(base.transform.localScale.x, 0f, 0f), out this.raycastHit, 19f, this.groundLayer))
		{
			this.MakeEffects(this.raycastHit.point.x, this.raycastHit.point.y);
			MapController.Damage_Networked(this, this.raycastHit.collider.gameObject, this.groundSwordDamage, DamageType.Bullet, this.xI, 0f);
			this.hasHitWithWall = true;
			this.SwingSwordGround();
			this.attackHasHit = true;
		}
		else
		{
			this.hasHitWithWall = false;
			this.SwingSwordEnemies();
		}
	}

	protected void FireWeaponGround(float x, float y, Vector3 raycastDirection, float distance, float xSpeed, float ySpeed)
	{
		if (Physics.Raycast(new Vector3(x, y, 0f), raycastDirection, out this.raycastHit, distance, this.groundLayer))
		{
			if (!this.hasHitWithWall)
			{
				this.MakeEffects(this.raycastHit.point.x, this.raycastHit.point.y);
			}
			MapController.Damage_Networked(this, this.raycastHit.collider.gameObject, this.groundSwordDamage, DamageType.Bullet, this.xI, 0f);
			this.hasHitWithWall = true;
			this.attackHasHit = true;
			this.PlayWallSound();
		}
	}

	protected void FireWeapon(float x, float y, Vector3 raycastDirection, float xSpeed, float ySpeed)
	{
		Map.HurtWildLife(x + base.transform.localScale.x * 13f, y + 5f, 12f);
		this.deflectProjectilesCounter = this.deflectProjectilesEnergy;
		this.deflectProjectilesEnergy = 0f;
		this.DeflectProjectiles();
		this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
		float num = base.transform.localScale.x * 12f;
		this.ConstrainToFragileBarriers(ref num, 16f);
		if (Physics.Raycast(new Vector3(x - Mathf.Sign(base.transform.localScale.x) * 12f, y + 5.5f, 0f), raycastDirection, out this.raycastHit, 19f, this.groundLayer) || Physics.Raycast(new Vector3(x - Mathf.Sign(base.transform.localScale.x) * 12f, y + 10.5f, 0f), new Vector3(base.transform.localScale.x, 0f, 0f), out this.raycastHit, 19f, this.groundLayer))
		{
			this.MakeEffects(this.raycastHit.point.x, this.raycastHit.point.y);
			MapController.Damage_Networked(this, this.raycastHit.collider.gameObject, this.groundSwordDamage, DamageType.Bullet, this.xI, 0f);
			this.hasHitWithWall = true;
			this.SwingSwordGround();
			this.attackHasHit = true;
		}
		else
		{
			this.hasHitWithWall = false;
			this.SwingSwordEnemies();
		}
	}

	protected virtual void SwingSwordGround()
	{
		if (Map.HitUnits(this, this.playerNum, this.enemySwordDamage, 1, DamageType.Bullet, 5f, this.x, this.y, base.transform.localScale.x * 420f, 360f, true, true, true, ref this.alreadyHit))
		{
			this.hasHitWithSlice = true;
			this.attackHasHit = true;
		}
		else
		{
			this.hasHitWithSlice = false;
		}
	}

	protected virtual void SwingSwordEnemies()
	{
		if (Map.HitUnits(this, this.playerNum, this.enemySwordDamage, 1, DamageType.Bullet, 13f, this.x, this.y, base.transform.localScale.x * 420f, 360f, true, true, true, ref this.alreadyHit))
		{
			this.hasHitWithSlice = true;
			this.attackHasHit = true;
		}
		else
		{
			this.hasHitWithSlice = false;
		}
	}

	protected override void RunFiring()
	{
		if (this.specialAttackDashTime > 0f)
		{
			this.hitClearCounter += this.t;
			if (this.hitClearCounter >= 0.03f)
			{
				this.hitClearCounter -= 0.03f;
				this.alreadyHit.Clear();
			}
		}
		if (this.fire)
		{
			this.rollingFrames = 0;
		}
		if (this.attackUpwards || this.attackForwards || this.attackDownwards)
		{
			if (!this.attackHasHit)
			{
				if (this.attackForwards && this.attackFrames >= this.attackForwardsStrikeFrame - 1)
				{
					this.DeflectProjectiles();
				}
				else if (this.attackUpwards && this.attackFrames >= this.attackUpwardsStrikeFrame - 1)
				{
					this.DeflectProjectiles();
				}
				else if (this.attackDownwards && this.attackFrames >= this.attackDownwardsStrikeFrame - 1)
				{
					this.DeflectProjectiles();
				}
			}
			if (this.attackForwards && this.attackFrames >= this.attackForwardsStrikeFrame && this.attackFrames <= 5)
			{
				this.lastAttackingTime = Time.time;
				if (Map.HitUnits(this, this.playerNum, this.enemySwordDamage, 1, DamageType.Bullet, 13f, this.x + base.transform.localScale.x * 7f, this.y + 7f, base.transform.localScale.x * 420f, 160f, true, true, true, ref this.alreadyHit))
				{
					if (!this.hasHitWithSlice)
					{
						this.PlaySliceSound();
					}
					this.hasHitWithSlice = true;
					this.attackHasHit = true;
					this.hasAttackedDownwards = false;
					this.hasAttackedUpwards = false;
				}
				if (!this.attackHasHit)
				{
					this.DeflectProjectiles();
				}
				if (!this.attackHasHit)
				{
					this.FireWeaponGround(this.x + base.transform.localScale.x * 3f, this.y + 6f, new Vector3(base.transform.localScale.x, 0f, 0f), 9f, base.transform.localScale.x * 180f, 80f);
					this.FireWeaponGround(this.x + base.transform.localScale.x * 3f, this.y + 12f, new Vector3(base.transform.localScale.x, 0f, 0f), 9f, base.transform.localScale.x * 180f, 80f);
				}
			}
			if (this.attackUpwards && this.attackFrames >= this.attackUpwardsStrikeFrame && this.attackFrames <= 5)
			{
				this.lastAttackingTime = Time.time;
				if (Map.HitUnits(this, this.playerNum, this.enemySwordDamage, 1, DamageType.Bullet, 13f, this.x + base.transform.localScale.x * 6f, this.y + 12f, base.transform.localScale.x * 80f, 1100f, true, true, true, ref this.alreadyHit))
				{
					if (!this.hasHitWithSlice)
					{
						this.PlaySliceSound();
					}
					this.hasHitWithSlice = true;
					this.attackHasHit = true;
					this.hasAttackedDownwards = false;
					this.hasAttackedForwards = false;
				}
				if (!this.attackHasHit)
				{
					this.DeflectProjectiles();
				}
				if (!this.attackHasHit)
				{
					this.FireWeaponGround(this.x + base.transform.localScale.x * 3f, this.y + 6f, new Vector3(base.transform.localScale.x * 0.5f, 1f, 0f), 12f, base.transform.localScale.x * 80f, 280f);
					this.FireWeaponGround(this.x + base.transform.localScale.x * 3f, this.y + 6f, new Vector3(base.transform.localScale.x, 0.5f, 0f), 12f, base.transform.localScale.x * 80f, 280f);
				}
			}
			if (this.attackDownwards && this.attackFrames >= this.attackDownwardsStrikeFrame && this.attackFrames <= 6)
			{
				this.lastAttackingTime = Time.time;
				if (Map.HitUnits(this, this.playerNum, this.enemySwordDamage, 1, DamageType.Bullet, 13f, this.x + base.transform.localScale.x * 6f, this.y + 4f, base.transform.localScale.x * 120f, 100f, true, true, true, ref this.alreadyHit))
				{
					if (!this.hasHitWithSlice)
					{
						this.PlaySliceSound();
					}
					this.hasHitWithSlice = true;
					this.attackHasHit = true;
					this.hasAttackedForwards = false;
					this.hasAttackedUpwards = false;
				}
				if (!this.attackHasHit)
				{
					this.DeflectProjectiles();
				}
				if (!this.attackHasHit)
				{
					this.FireWeaponGround(this.x + base.transform.localScale.x * 3f, this.y + 6f, new Vector3(base.transform.localScale.x * 0.4f, -1f, 0f), 14f, base.transform.localScale.x * 80f, -180f);
					this.FireWeaponGround(this.x + base.transform.localScale.x * 3f, this.y + 6f, new Vector3(base.transform.localScale.x * 0.8f, -0.2f, 0f), 12f, base.transform.localScale.x * 80f, -180f);
				}
			}
		}
	}

	protected override void Jump(bool wallJump)
	{
		if (!this.attackUpwards && this.attackFrames < this.attackUpwardsStrikeFrame && (!this.attackForwards || this.attackFrames > this.attackForwardsStrikeFrame))
		{
			base.Jump(wallJump);
		}
	}

	protected void ClearCurrentAttackVariables()
	{
		this.alreadyHit.Clear();
		this.hasHitWithSlice = false;
		this.attackHasHit = false;
		this.hasHitWithWall = false;
	}

	protected override void StartFiring()
	{
		this.startNewAttack = false;
		this.hasPlayedAttackHitSound = false;
		if (this.y < this.groundHeight + 1f)
		{
			this.StopAirDashing();
		}
		if ((this.attackForwards || this.attackDownwards || this.attackUpwards) && this.attackFrames < this.attackForwardsStrikeFrame + 1)
		{
			this.startNewAttack = true;
		}
		else if (this.up && !this.hasAttackedUpwards)
		{
			if (this.actionState == ActionState.ClimbingLadder)
			{
				this.actionState = ActionState.Jumping;
			}
			this.StopBrocAttack();
			this.hasAttackedUpwards = true;
			this.attackFrames = 0;
			this.attackUpwards = true;
			if (this.yI > 50f)
			{
				this.yI = 50f;
			}
			this.jumpTime = 0f;
			this.ChangeFrame();
			this.groundSwordDamage = 5;
			this.airdashDirection = DirectionEnum.Up;
			this.ClearCurrentAttackVariables();
		}
		else if (this.down && !this.hasAttackedDownwards)
		{
			this.actionState = ActionState.Jumping;
			this.StopBrocAttack();
			this.hasAttackedDownwards = true;
			this.yI = 150f;
			this.xI = base.transform.localScale.x * 80f;
			this.attackFrames = 0;
			this.attackDownwards = true;
			this.jumpTime = 0f;
			this.ChangeFrame();
			this.groundSwordDamage = 5;
			this.airdashDirection = DirectionEnum.Down;
			this.ClearCurrentAttackVariables();
		}
		else if (this.left && !this.hasAttackedForwards)
		{
			if (this.actionState == ActionState.ClimbingLadder)
			{
				this.actionState = ActionState.Jumping;
			}
			this.StopBrocAttack();
			this.hasAttackedForwards = true;
			this.xIAttackExtra = -90f;
			this.attackFrames = 0;
			this.yI = 0f;
			this.attackForwards = true;
			this.jumpTime = 0f;
			this.ChangeFrame();
			base.CreateFaderTrailInstance();
			this.groundSwordDamage = 5;
			this.airdashDirection = DirectionEnum.Left;
			this.ClearCurrentAttackVariables();
		}
		else if (this.right && !this.hasAttackedForwards)
		{
			if (this.actionState == ActionState.ClimbingLadder)
			{
				this.actionState = ActionState.Jumping;
			}
			this.StopBrocAttack();
			this.hasAttackedForwards = true;
			this.xIAttackExtra = 90f;
			this.attackFrames = 0;
			this.yI = 0f;
			this.attackForwards = true;
			this.jumpTime = 0f;
			this.ChangeFrame();
			base.CreateFaderTrailInstance();
			this.groundSwordDamage = 5;
			this.airdashDirection = DirectionEnum.Right;
			this.ClearCurrentAttackVariables();
		}
		else
		{
			this.attackHasHit = false;
			this.groundSwordDamage = 2;
			this.ClearCurrentAttackVariables();
			this.UseFire();
			base.FireFlashAvatar();
		}
	}

	protected override bool IsOverLadder(float xOffset, ref float ladderXPos)
	{
		return !this.attackDownwards && !this.attackForwards && !this.attackUpwards && base.IsOverLadder(xOffset, ref ladderXPos);
	}

	protected override void RunMovement()
	{
		if (this.health <= 0)
		{
			this.xIAttackExtra = 0f;
		}
		if (this.specialAttackDashTime > 0f)
		{
			this.invulnerable = true;
			this.invulnerableTime = 0.3f;
			this.specialAttackDashTime -= this.t;
			this.xI = 240f * base.transform.localScale.x;
			this.yI = 0f;
			this.y = this.specialAttackDashHeight;
			if (Map.DeflectProjectiles(this, this.playerNum, 16f, this.x + Mathf.Sign(base.transform.localScale.x) * 6f, this.y + 6f, Mathf.Sign(base.transform.localScale.x) * 200f))
			{
			}
			this.specialAttackDashCounter += this.t;
			if (this.specialAttackDashCounter > 0f)
			{
				this.specialAttackDashCounter -= 0.0333f;
				if (base.IsMine)
				{
					MapController.DamageGround(this, 3, DamageType.Melee, 8f, this.x + base.transform.localScale.x * 16f, this.y + 7f, null);
				}
				Unit unit = Map.ImpaleUnits(this, this.playerNum, 16f, this.x + base.transform.localScale.x * 24f, this.y + 4f, false, true, ref this.impaledWithSpecialUnits);
				if (unit != null)
				{
					this.impaledWithSpecialUnits.Add(unit);
					unit.Impale(base.transform, 0, this.xI, this.yI, base.transform.localScale.x * 8f);
				}
				base.CreateFaderTrailInstance();
			}
			if (this.specialAttackDashTime <= 0f)
			{
				this.gunSprite.SetLowerLeftPixel(0f, 32f);
				this.actionState = ActionState.Jumping;
				if (this.impaledWithSpecialUnits.Count > 0)
				{
					SortOfFollow.Shake(0.6f);
					this.sound.PlaySoundEffectAt(this.soundHolder.special2Sounds, this.sliceVolume * 2f, base.transform.position);
				}
				for (int i = this.impaledWithSpecialUnits.Count - 1; i >= 0; i--)
				{
					if (this.impaledWithSpecialUnits[i] != null)
					{
						this.impaledWithSpecialUnits[i].Unimpale(5, DamageType.Melee, 0f, UnityEngine.Random.Range(250f, 500f));
						this.impaledWithSpecialUnits[i].Knock(DamageType.Normal, UnityEngine.Random.Range(-100f, 100f) + this.xI / 10f, UnityEngine.Random.Range(5000f, 10000f), true);
						this.impaledWithSpecialUnits[i].yI += UnityEngine.Random.Range(0f, 150f);
					}
					this.impaledWithSpecialUnits.RemoveAt(i);
				}
				this.yI = this.jumpForce;
				this.xI = 0f;
				this.chimneyFlip = true;
				this.chimneyFlipFrames = 11;
				this.chimneyFlipDirection = 0;
				this.AnimateChimneyFlip();
				this.ChangeFrame();
			}
			Map.HurtWildLife(this.x + base.transform.localScale.x * 13f, this.y + 5f, 16f);
			base.RunMovement();
		}
		else
		{
			base.RunMovement();
		}
	}

	protected override void IncreaseFrame()
	{
		base.IncreaseFrame();
		if (this.attackUpwards || this.attackDownwards || this.attackForwards)
		{
			this.attackFrames++;
		}
	}

	protected override void ChangeFrame()
	{
		if (this.health <= 0 || this.chimneyFlip)
		{
			base.ChangeFrame();
		}
		else if (this.attackUpwards)
		{
			this.AnimateAttackUpwards();
		}
		else if (this.attackDownwards)
		{
			this.AnimateAttackDownwards();
		}
		else if (this.attackForwards)
		{
			this.AnimateAttackForwards();
		}
		else if (this.specialAttackDashTime > 0f)
		{
			this.gunSprite.gameObject.SetActive(true);
			this.actionState = ActionState.Jumping;
			int num = 23 + this.frame % 2;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), 32f);
		}
		else
		{
			base.ChangeFrame();
		}
	}

	private void AnimateAttackUpwards()
	{
		this.DeactivateGun();
		if (this.attackFrames < this.attackUpwardsStrikeFrame)
		{
			this.frameRate = 0.0667f;
		}
		else if (this.attackFrames < 5)
		{
			this.frameRate = 0.045f;
		}
		else
		{
			this.frameRate = 0.045f;
		}
		if (this.attackFrames == this.attackUpwardsStrikeFrame)
		{
			this.xI = base.transform.localScale.x * 50f;
			this.yI = 240f;
			this.PlayAttackSound();
		}
		if (this.attackFrames < this.attackUpwardsStrikeFrame + 2)
		{
			base.CreateFaderTrailInstance();
		}
		if (this.startNewAttack && this.attackFrames == this.attackUpwardsStrikeFrame + 1)
		{
			this.startNewAttack = false;
			this.StartFiring();
		}
		if (this.attackFrames >= 10 || (this.attackFrames == 6 && this.startNewAttack))
		{
			this.StopBrocAttack();
			this.ChangeFrame();
		}
		else
		{
			int num = 0 + this.attackFrames;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 7));
		}
	}

	protected override void Land()
	{
		if (this.attackDownwards)
		{
			UnityEngine.Debug.Log("Land downwards! ");
			if (!this.attackHasHit && this.attackFrames < 7)
			{
				this.FireWeaponGround(this.x + base.transform.localScale.x * 16.5f, this.y + 16.5f, Vector3.down, 18f + Mathf.Abs(this.yI * this.t), base.transform.localScale.x * 80f, 100f);
			}
			if (!this.attackHasHit && this.attackFrames < 7)
			{
				this.FireWeaponGround(this.x + base.transform.localScale.x * 5.5f, this.y + 16.5f, Vector3.down, 18f + Mathf.Abs(this.yI * this.t), base.transform.localScale.x * 80f, 100f);
			}
			this.attackDownwards = false;
			this.attackFrames = 0;
		}
		base.Land();
	}

	protected override void StopAirDashing()
	{
		base.StopAirDashing();
		this.hasAttackedDownwards = false;
		this.hasAttackedUpwards = false;
		this.hasAttackedForwards = false;
	}

	private void AnimateAttackDownwards()
	{
		this.DeactivateGun();
		if (this.attackFrames < this.attackDownwardsStrikeFrame)
		{
			this.frameRate = 0.0667f;
		}
		else if (this.attackFrames <= 5)
		{
			this.frameRate = 0.045f;
		}
		else
		{
			this.frameRate = 0.066f;
		}
		if (this.attackFrames < this.attackDownwardsStrikeFrame + 2)
		{
			base.CreateFaderTrailInstance();
		}
		if (this.startNewAttack && this.attackFrames == this.attackDownwardsStrikeFrame + 1)
		{
			this.startNewAttack = false;
			this.StartFiring();
		}
		if (this.attackFrames == this.attackDownwardsStrikeFrame)
		{
			this.yI = -250f;
			this.xI = base.transform.localScale.x * 60f;
			this.PlayAttackSound();
		}
		if (this.attackFrames >= 9 || (this.attackFrames == 6 && this.startNewAttack))
		{
			this.StopBrocAttack();
			this.ChangeFrame();
		}
		else
		{
			int num = 23 + this.attackFrames;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 8));
		}
	}

	private void AnimateAttackForwards()
	{
		this.DeactivateGun();
		if (this.attackFrames < this.attackForwardsStrikeFrame)
		{
			this.frameRate = 0.045f;
		}
		else if (this.attackFrames < 5)
		{
			this.frameRate = 0.045f;
		}
		else
		{
			this.frameRate = 0.045f;
		}
		if (this.attackFrames < this.attackForwardsStrikeFrame + 1)
		{
			base.CreateFaderTrailInstance();
		}
		if (this.startNewAttack && this.attackFrames == this.attackUpwardsStrikeFrame + 1)
		{
			this.startNewAttack = false;
			this.StartFiring();
		}
		if (this.attackFrames == this.attackForwardsStrikeFrame)
		{
			this.FireWeaponGround(this.x + base.transform.localScale.x * 9f, this.y + 6f, new Vector3(base.transform.localScale.x, 0f, 0f), 8f, base.transform.localScale.x * 180f, 80f);
			this.PlayAttackSound();
		}
		if (this.attackFrames == this.attackForwardsStrikeFrame + 1)
		{
			this.xIAttackExtra = 0f;
		}
		if (this.attackFrames >= 8 || (this.attackFrames == 6 && this.startNewAttack))
		{
			this.StopBrocAttack();
			this.ChangeFrame();
		}
		else
		{
			int num = 24 + this.attackFrames;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 6));
		}
	}

	protected bool CanAddXSpeed()
	{
		return !this.attackDownwards && !this.attackUpwards;
	}

	protected override void AddSpeedLeft()
	{
		if (this.CanAddXSpeed())
		{
			base.AddSpeedLeft();
			if (this.attackForwards && this.attackFrames > 4 && this.xI < -this.speed * 0.5f)
			{
				this.xI = -this.speed * 0.5f;
			}
		}
	}

	protected override void AddSpeedRight()
	{
		if (this.CanAddXSpeed())
		{
			base.AddSpeedRight();
			if (this.attackForwards && this.attackFrames > 4 && this.xI > this.speed * 0.5f)
			{
				this.xI = this.speed * 0.5f;
			}
		}
	}

	private void StopBrocAttack()
	{
		this.attackForwards = false; this.attackUpwards = (this.attackDownwards = (this.attackForwards ));
		this.attackFrames = 0;
		this.frame = 0;
		this.xIAttackExtra = 0f;
		this.usingSpecial = false;
		if (this.y > this.groundHeight + 1f)
		{
			this.actionState = ActionState.Jumping;
		}
		else if (this.right || this.left)
		{
			this.actionState = ActionState.Running;
		}
		else
		{
			this.actionState = ActionState.Idle;
		}
		if (this.startNewAttack)
		{
			this.startNewAttack = false;
			this.StartFiring();
		}
		if (this.y < this.groundHeight + 1f)
		{
			this.StopAirDashing();
		}
	}

	protected override void HitCeiling(RaycastHit ceilingHit)
	{
		base.HitCeiling(ceilingHit);
		if (this.attackUpwards)
		{
			if (!this.attackHasHit && this.attackFrames < 7)
			{
				this.FireWeaponGround(this.x + base.transform.localScale.x * 16.5f, this.y + 2f, Vector3.up, this.headHeight + Mathf.Abs(this.yI * this.t), base.transform.localScale.x * 80f, 100f);
			}
			if (!this.attackHasHit && this.attackFrames < 7)
			{
				this.FireWeaponGround(this.x + base.transform.localScale.x * 4.5f, this.y + 2f, Vector3.up, this.headHeight + Mathf.Abs(this.yI * this.t), base.transform.localScale.x * 80f, 100f);
			}
			this.attackUpwards = false;
			this.attackFrames = 0;
		}
	}

	protected override void ApplyFallingGravity()
	{
		if (this.chimneyFlip)
		{
			base.ApplyFallingGravity();
		}
		else if (!this.attackForwards || this.attackFrames > this.attackForwardsStrikeFrame)
		{
			if (this.attackDownwards && this.attackFrames < this.attackDownwardsStrikeFrame)
			{
				this.yI -= 1100f * this.t * 0.3f;
			}
			else if (this.attackUpwards && this.attackFrames >= this.attackUpwardsStrikeFrame)
			{
				this.yI -= 1100f * this.t * 0.5f;
			}
			else
			{
				base.ApplyFallingGravity();
			}
		}
	}

	protected bool IsAttacking()
	{
		return (this.fire && this.gunFrame > 1) || Time.time - this.lastAttackingTime < 0.0445f || ((this.attackDownwards || this.attackForwards || this.attackUpwards) && this.attackFrames > 1 && this.attackFrames < this.attackForwardsStrikeFrame + 2);
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Damage ",
			damageType,
			" is attacking ",
			this.IsAttacking()
		}));
		if ((damageType == DamageType.Drill || damageType == DamageType.Melee || damageType == DamageType.Knifed) && this.IsAttacking() && (Mathf.Sign(base.transform.localScale.x) != Mathf.Sign(xI) || damageType == DamageType.Drill))
		{
			UnityEngine.Debug.Log("Don't Give a shit");
		}
		else
		{
			base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
		}
	}

	protected override void AnimateSpecial()
	{
		if (this.frame >= 0)
		{
			this.UseSpecial();
		}
	}

	protected override void UseSpecial()
	{
		if (base.SpecialAmmo > 0 && Time.time - this.specialAttackStartTime > 0.55f)
		{
			this.ClearCurrentAttackVariables();
			this.StopAirDashing();
			if (this.attachedToZipline != null)
			{
				this.attachedToZipline.DetachUnit(this);
			}
			this.PlaySpecialAttackSound(0.7f);
			base.SpecialAmmo--;
			HeroController.SetSpecialAmmo(this.playerNum, base.SpecialAmmo);
			this.usingSpecial = false;
			this.specialAttackDashTime = 0.5f;
			this.specialAttackStartTime = Time.time;
			this.specialAttackDashHeight = this.y;
			this.ChangeFrame();
		}
		else
		{
			HeroController.FlashSpecialAmmo(this.playerNum);
			this.gunSprite.gameObject.SetActive(true);
			this.usingSpecial = false;
		}
	}

	protected virtual void MakeEffects(float x, float y)
	{
		EffectsController.CreateShrapnel(this.shrapnelSpark, this.raycastHit.point.x + this.raycastHit.normal.x * 3f, this.raycastHit.point.y + this.raycastHit.normal.y * 3f, 4f, 30f, 3f, this.raycastHit.normal.x * 60f, this.raycastHit.normal.y * 30f);
		EffectsController.CreateEffect(this.hitPuff, this.raycastHit.point.x + this.raycastHit.normal.x * 3f, this.raycastHit.point.y + this.raycastHit.normal.y * 3f);
	}

	public void PlaySliceSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.special2Sounds, this.sliceVolume, base.transform.position);
		}
	}

	public void PlayWallSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.attack2Sounds, this.wallHitVolume, base.transform.position);
		}
	}

	protected override void PlayAttackSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.attackSounds, 0.6f, base.transform.position);
		}
	}

	protected void DeflectProjectiles()
	{
		if (Map.DeflectProjectiles(this, this.playerNum, 10f, this.x + Mathf.Sign(base.transform.localScale.x) * 6f, this.y + 6f, Mathf.Sign(base.transform.localScale.x) * 200f))
		{
			if (!this.hasHitWithWall)
			{
				this.PlayWallSound();
			}
			this.hasHitWithWall = true;
			this.attackHasHit = true;
		}
	}

	protected override void RunGun()
	{
		if (this.specialAttackDashTime > 0f)
		{
			this.gunSprite.SetLowerLeftPixel(352f, 32f);
		}
		else
		{
			this.deflectProjectilesEnergy += this.t * 0.5f;
			if (this.deflectProjectilesEnergy > 0.45f)
			{
				this.deflectProjectilesEnergy = 0.45f;
			}
			this.deflectProjectilesCounter -= this.t;
			if (!this.WallDrag && this.gunFrame > 0)
			{
				if (this.deflectProjectilesCounter > 0f)
				{
					this.DeflectProjectiles();
				}
				this.gunCounter += this.t;
				if (this.gunCounter > 0.0334f)
				{
					this.gunCounter -= 0.0334f;
					this.gunFrame--;
					if (this.gunFrame < 0)
					{
						this.gunFrame = 0;
					}
					this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
					if (!this.hasPlayedAttackHitSound)
					{
						if (this.hasHitWithSlice)
						{
							this.PlaySliceSound();
							this.hasPlayedAttackHitSound = true;
						}
						else if (this.hasHitWithWall)
						{
							this.PlayWallSound();
							this.hasPlayedAttackHitSound = true;
						}
					}
					if (this.gunFrame >= 3)
					{
						if (this.hasHitWithWall)
						{
							this.SwingSwordGround();
						}
						else
						{
							this.SwingSwordEnemies();
						}
					}
				}
			}
		}
	}

	protected override void SetGunPosition(float xOffset, float yOffset)
	{
		this.gunSprite.transform.localPosition = new Vector3(xOffset + 4f, yOffset, -1f);
	}

	protected override void AnimateZipline()
	{
		base.AnimateZipline();
		this.SetGunSprite(4, 1);
	}

	public Shrapnel shrapnelSpark;

	public FlickerFader hitPuff;

	protected List<Unit> alreadyHit = new List<Unit>();

	protected List<Unit> impaledWithSpecialUnits = new List<Unit>();

	protected float hitClearCounter;

	protected bool hasHitWithSlice;

	protected bool hasHitWithWall;

	protected bool hasPlayedAttackHitSound;

	public int groundSwordDamage = 1;

	public int enemySwordDamage = 5;

	public bool attackUpwards;

	public bool attackDownwards;

	public bool attackForwards;

	public bool hasAttackedUpwards;

	public bool hasAttackedDownwards;

	public bool hasAttackedForwards;

	protected int attackUpwardsStrikeFrame = 2;

	protected int attackDownwardsStrikeFrame = 3;

	protected int attackForwardsStrikeFrame = 3;

	protected bool attackHasHit;

	protected int attackFrames;

	protected float deflectProjectilesCounter;

	protected float deflectProjectilesEnergy;

	protected float lastAttackingTime;

	protected bool startNewAttack;

	protected bool specialAttackDashing;

	protected float specialAttackDashTime;

	protected float specialAttackDashCounter;

	protected float specialAttackDashHeight;

	private float specialAttackStartTime;

	public float sliceVolume = 0.7f;

	public float wallHitVolume = 0.3f;
}
