// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class IndianaBrones : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
		this.materialNormal = base.GetComponent<Renderer>().sharedMaterial; this.currentMaterial = (this.materialNormal );
		this.whipLineRenderer = base.GetComponentInChildren<LineRenderer>();
		this.whipLineRenderer.enabled = false;
		this.whipLineRenderer.SetPosition(0, Vector3.zero);
		this.whipLineRenderer.SetPosition(1, Vector3.zero);
		this.whipState = WhipState.Inactive;
		this.whipSprite.gameObject.SetActive(false);
	}

	protected override void Update()
	{
		base.Update();
		if (this.isOnHelicopter)
		{
			this.whipLineRenderer.enabled = false;
		}
	}

	protected void SetWhipSprite(int spriteFrame)
	{
		if (this.currentMaterial != this.materialArmless)
		{
			this.currentMaterial = this.materialArmless;
			base.GetComponent<Renderer>().sharedMaterial = this.currentMaterial;
			this.sprite.RecalcTexture();
		}
		this.gunSprite.SetPixelDimensions(64, 64);
		this.gunSprite.SetLowerLeftPixel((float)(this.whipAnimationPixelSize * spriteFrame), (float)(this.whipAnimationPixelSize * 2));
		this.gunSprite.SetSize(64f, 64f);
	}

	protected override void RunGun()
	{
		if (!this.WallDrag)
		{
			if (this.whippingAnimation)
			{
				if (this.gunFrame >= 0)
				{
					this.doingMelee = false;
					this.meleeFrame = 0;
					this.gunCounter += this.t;
					if (this.gunCounter > 0.0334f)
					{
						this.gunCounter -= 0.0334f;
						this.gunFrame--;
						if (this.gunFrame >= 1)
						{
							int num = 9 - this.gunFrame;
							this.SetWhipSprite(num);
						}
						else
						{
							this.whippingAnimation = false;
							this.gunFrame = 0;
							this.UseWhip();
						}
						if (this.gunFrame == 2)
						{
							this.StartWhip();
						}
					}
				}
			}
			else if (this.gunFrame > 0)
			{
				this.gunCounter += this.t;
				if (this.gunCounter > 0.0334f)
				{
					this.gunCounter -= 0.0334f;
					this.gunFrame--;
					this.SetGunSprite(this.gunFrame, 0);
				}
			}
		}
		if (!this.gunSprite.gameObject.activeSelf)
		{
			this.gunFrame = 0;
			this.whipState = WhipState.Inactive;
			this.whipLineRenderer.enabled = false;
		}
		if ((!this.gunSprite.gameObject.activeSelf || !this.whippingAnimation) && this.currentMaterial != this.materialNormal && !this.doingMelee)
		{
			this.currentMaterial = this.materialNormal;
			base.GetComponent<Renderer>().sharedMaterial = this.currentMaterial;
			this.sprite.RecalcTexture();
			this.gunSprite.SetPixelDimensions(32, 32);
			this.gunSprite.SetSize(32f, 32f);
			if (!this.doingMelee)
			{
				this.SetGunSprite(this.gunFrame, 0);
			}
			this.whippingAnimation = false;
		}
		else if (!this.whippingAnimation || this.gunSprite.gameObject.activeSelf)
		{
		}
	}

	protected override void SetMeleeType()
	{
		if (this.actionState == ActionState.Jumping || this.y > this.groundHeight + 1f)
		{
			this.jumpingMelee = true;
			this.standingMelee = false;
			this.dashingMelee = false;
		}
		else
		{
			this.jumpingMelee = false;
			this.standingMelee = true;
			this.dashingMelee = false;
		}
	}

	protected override void SetGunPosition(float xOffset, float yOffset)
	{
		if (!this.doingMelee)
		{
			base.SetGunPosition(xOffset, yOffset);
		}
		else
		{
			base.SetGunPosition(0f, 0f);
		}
	}

	protected override void StartMelee()
	{
		if (this.wallClimbing || this.whippingAnimation)
		{
			return;
		}
		this.showHighFiveAfterMeleeTimer = 0f;
		this.SetMeleeType();
		this.meleeHasHit = false;
		if (!this.doingMelee || this.meleeFrame > 3)
		{
			this.doingMelee = true;
			this.meleeFrame = 0;
			this.meleeCounter = -0.033f;
			this.wallHasHitForward = false; this.wallHasHit = (this.wallHasHitForward );
			this.ActivateGun();
			base.SetGunPosition(0f, 0f);
			this.currentMaterial = this.materialArmless;
			base.GetComponent<Renderer>().sharedMaterial = this.materialArmless;
			UnityEngine.Debug.Log("Set current material  start melee " + this.currentMaterial.name);
			this.AnimateMelee();
		}
		else
		{
			this.meleeFollowUp = true;
			this.doingMelee = true;
		}
	}

	protected override void RunIndependentMeleeFrames()
	{
		if (this.canDoIndependentMeleeAnimation && this.doingMelee && !this.whippingAnimation)
		{
			this.ActivateGun();
			this.meleeCounter += this.t;
			if (this.meleeCounter >= this.meleeFrameRate)
			{
				this.meleeCounter -= this.meleeFrameRate;
				this.meleeFrame++;
				this.AnimateMelee();
			}
		}
		base.RunIndependentMeleeFrames();
	}

	protected override void IncreaseFrame()
	{
		base.IncreaseFrame();
	}

	protected override void AnimateMelee()
	{
		this.SetSpriteOffset(0f, 0f);
		this.rollingFrames = 0;
		if (this.meleeFrame == 1)
		{
			this.meleeCounter -= 0.06667f;
		}
		if (this.meleeFrame <= 2)
		{
			this.wallHasHit = false;
		}
		if (this.meleeFrame >= 5 && this.meleeFollowUp)
		{
			this.meleeCounter -= 0.033f;
			this.meleeFrame = 0;
			this.meleeFollowUp = false;
			this.wallHasHitForward = false; this.wallHasHit = (this.wallHasHitForward );
		}
		if (this.meleeFrame <= 2 && this.meleeFollowUp)
		{
			this.meleeFollowUp = false;
		}
		this.frameRate = 0.0333f;
		if (this.meleeFrame == 2)
		{
			this.PlaySpecialAttackSound(0.2f);
		}
		if (this.meleeFrame == 3)
		{
			if (Map.HitClosestUnit(this, this.playerNum, 4, DamageType.Bullet, 14f, 24f, this.x + base.transform.localScale.x * 8f, this.y + 8f, base.transform.localScale.x * 200f, 500f, true, true, base.IsMine, false))
			{
				this.sound.PlaySoundEffectAt(this.soundHolder.meleeHitSound, 0.6f, base.transform.position);
				this.meleeHasHit = true;
				if (!this.wallHasHit && MapController.DamageGround(this, 8, DamageType.Crush, 8f, this.x + base.transform.localScale.x * 11f, this.y + 11f, null))
				{
					this.PlayWallSound();
					this.wallHasHit = true;
					this.wallHasHitForward = true;
					EffectsController.CreateSparkShower(this.x + base.transform.localScale.x * 13f, this.y + 7f, 8, 5f, 150f, base.transform.localScale.x * -110f, 100f, 0.4f, 0f);
					EffectsController.CreateProjectilePopEffect(this.x + base.transform.localScale.x * 11f, this.y + 7f);
				}
			}
			else if (!this.wallHasHit && MapController.DamageGround(this, 8, DamageType.Crush, 8f, this.x + base.transform.localScale.x * 16f, this.y + 11f, null))
			{
				this.PlayWallSound();
				this.wallHasHit = true;
				this.wallHasHitForward = true;
				EffectsController.CreateSparkShower(this.x + base.transform.localScale.x * 14f, this.y + 7f, 8, 5f, 150f, base.transform.localScale.x * -110f, 100f, 0.4f, 0f);
				EffectsController.CreateProjectilePopEffect(this.x + base.transform.localScale.x * 14f, this.y + 7f);
			}
			else if (!this.wallHasHit && MapController.DamageGround(this, 8, DamageType.Crush, 8f, this.x + base.transform.localScale.x * 15f, this.y + 4f, null))
			{
				this.PlayWallSound();
				this.wallHasHit = true;
				EffectsController.CreateSparkShower(this.x + base.transform.localScale.x * 11f, this.y + 2f, 8, 5f, 150f, 0f, 150f, 0.4f, 0f);
				EffectsController.CreateProjectilePopEffect(this.x + base.transform.localScale.x * 11f, this.y + 2f);
			}
			else
			{
				this.sound.PlaySoundEffectAt(this.soundHolder.missSounds, 0.3f, base.transform.position);
			}
			this.meleeChosenUnit = null;
			if (this.TryBustCage())
			{
				this.meleeHasHit = true;
			}
		}
		int spriteFrame = 17 + Mathf.Clamp(this.meleeFrame, 0, 6);
		this.SetGunSprite(spriteFrame, (!this.wallHasHitForward) ? 0 : 1);
		if (this.meleeFrame == 4)
		{
			this.meleeCounter -= 0.0334f;
		}
		if (this.meleeFrame >= 6)
		{
			this.meleeFrame = 0;
			this.CancelMelee();
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
			this.sound.PlaySoundEffectAt(this.soundHolder.special3Sounds, this.wallHitVolume, base.transform.position);
		}
	}

	protected override void SetGunSprite(int spriteFrame, int spriteRow)
	{
		if (this.wallClimbing)
		{
			this.doingMelee = false;
			this.meleeFrame = 0;
			return;
		}
		if (this.materialNormal != null && this.currentMaterial != this.materialNormal && !this.doingMelee && !this.whippingAnimation)
		{
			this.currentMaterial = this.materialNormal;
			base.GetComponent<Renderer>().sharedMaterial = this.currentMaterial;
			this.sprite.RecalcTexture();
			this.gunSprite.SetPixelDimensions(32, 32);
			this.gunSprite.SetSize(32f, 32f);
			UnityEngine.Debug.Log("Set current material setgunsprite " + this.currentMaterial.name);
		}
		base.SetGunSprite(spriteFrame, spriteRow);
	}

	protected override void PressSpecial()
	{
		if (base.SpecialAmmo > 0 && this.health > 0)
		{
			base.SpecialAmmo--;
			this.FireWeapon(this.x + base.transform.localScale.x * 14f, this.y + 9f, base.transform.localScale.x * 800f, (float)UnityEngine.Random.Range(-10, 10));
			this.PlayAttackSound();
			Map.DisturbWildLife(this.x, this.y, 60f, this.playerNum);
			SortOfFollow.Shake(0.4f, 0.4f);
			this.yI += 50f;
			this.xIBlast = -base.transform.localScale.x * 50f;
		}
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		this.CancelMelee();
		base.Death(xI, yI, damage);
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 3;
		this.SetGunSprite(this.gunFrame, 1);
		EffectsController.CreateMuzzleFlashBigEffect(x + Mathf.Sign(xSpeed) * 5f, y, -25f, xSpeed * 0.03f, ySpeed * 0.03f);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed, ySpeed, this.playerNum);
	}

	protected void UseWhip()
	{
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.special2Sounds, 0.5f, base.transform.position);
		if (this.whipAttachBlock != null || this.whipHitHelicopter)
		{
			this.curWhipRange = Vector3.Distance(base.transform.position + this.whipOffset, this.whipHitPoint);
			this.whipState = WhipState.Attached;
			this.yI = 0f; this.xI = (this.yI );
		}
		else
		{
			this.whipState = WhipState.Inactive;
			this.usingSpecial = false;
			float num = 0f;
			bool flag = Map.HitProjectiles(this.playerNum, 15, DamageType.Bullet, Mathf.Abs(this.whipHitPoint.x - this.x) * 0.5f, 8f, (this.x + this.whipHitPoint.x) / 2f, this.y + this.whipOffset.y, base.transform.localScale.x * 50f, 50f, 0f);
			if (Map.WhipUnits(this, this, this.playerNum, 8, DamageType.Normal, Mathf.Abs(this.whipHitPoint.x - this.x) * 0.5f, 5f, (int)Mathf.Sign(base.transform.localScale.x), (this.x + this.whipHitPoint.x) / 2f, this.y + this.whipOffset.y, base.transform.localScale.x * 50f, 55f, true, true, true, ref num) || flag)
			{
				if (num <= 0.2f)
				{
					Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.attack2Sounds, 0.4f, base.transform.position);
				}
				else if (num <= 0.7f)
				{
					Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.attack2Sounds, 0.5f, base.transform.position);
					Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.attack4Sounds, 0.2f, base.transform.position, 0.95f);
					SortOfFollow.Shake(0.2f);
				}
				else
				{
					Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.attack2Sounds, 0.5f, base.transform.position);
					Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.attack4Sounds, 0.5f, base.transform.position, 0.85f);
					SortOfFollow.Shake(0.5f);
				}
			}
			else
			{
				Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.missSounds, 0.5f, base.transform.position);
				if (this.actionState == ActionState.Jumping && this.special)
				{
					RaycastHit raycastHit;
					bool flag2 = Physics.Raycast(base.transform.position + this.whipOffset, this.whipDirection, out raycastHit, this.whipRange, this.groundLayer);
					if (flag2)
					{
						this.whipAttachBlock = raycastHit.collider.gameObject.GetComponent<Block>();
						UnityEngine.Debug.Log("HIT !!! " + (this.whipAttachBlock != null));
						if (this.whipAttachBlock != null)
						{
							this.whipPullToPoint = Map.GetBlockCenter(this.whipAttachBlock.collumn - (int)Mathf.Sign(this.whipDirection.x), this.whipAttachBlock.row + 1);
							UnityEngine.Debug.Log("HIT GOT PULL TO POINT !!! ");
						}
						else
						{
							flag2 = false;
						}
					}
					if (flag2)
					{
						UnityEngine.Debug.Log("REALLY WHIP !!! ");
						this.whipState = WhipState.Attached;
						this.usingSpecial = true;
					}
				}
			}
			Map.PanicUnits((this.x + this.whipHitPoint.x) / 2f, this.y, Mathf.Abs(this.whipHitPoint.x - this.x) * 0.5f, 12f, (int)Mathf.Sign(base.transform.localScale.x), 1f, false);
		}
		EffectsController.CreateEffect(this.whipHitPuff, this.whipHitPoint.x, this.whipHitPoint.y);
		this.whipDrawTime = 0.1f;
	}

	protected void StartWhip()
	{
		if (this.whipState == WhipState.Inactive || this.whipState == WhipState.Retracting)
		{
			this.whipHitHelicopter = false;
			this.whipState = WhipState.Extending;
			this.whipDirection = new Vector3(base.transform.localScale.x, 1f, 0f);
			this.frame = 0;
			this.whipExtendTime = 0.03f;
			this.whipDirection.Normalize();
			this.whipAttachBlock = null;
			RaycastHit raycastHit = default(RaycastHit);
			bool flag = false;
			Block block = null;
			Vector3 vector = Vector3.zero;
			this.fallback = false;
			int num = 0;
			if (this.actionState == ActionState.Jumping)
			{
				if (this.buttonJump || this.up)
				{
					while (!flag && this.whipDirection.y < 0.98f && num < 200)
					{
						this.whipDirection = Vector3.RotateTowards(this.whipDirection, Vector3.up, 0.08726646f, 0f);
						flag = Physics.Raycast(base.transform.position + this.whipOffset, this.whipDirection, out raycastHit, this.whipRange, this.groundLayer);
						UnityEngine.Debug.DrawRay(base.transform.position + this.whipOffset, this.whipDirection);
						if (flag)
						{
							Block component = raycastHit.collider.gameObject.GetComponent<Block>();
							if (component != null && !Map.IsBlockSolid(component.collumn, component.row + 1) && !Map.IsBlockSolid(component.collumn - (int)base.transform.localScale.x, component.row))
							{
								this.whipPullToPoint = Map.GetBlockCenter(component.collumn - (int)base.transform.localScale.x, component.row + 1);
							}
							else
							{
								if (block == null || block.row < component.row)
								{
									block = component;
									vector = raycastHit.point;
								}
								flag = false;
							}
						}
						num++;
					}
					if (num > 150)
					{
						UnityEngine.Debug.LogError("Evan is a bum head");
					}
					this.whipDirection = new Vector3(base.transform.localScale.x, 1f, 0f);
					this.whipDirection.Normalize();
					num = 0;
					while (!flag && this.whipDirection.y > -0.2f && num < 200)
					{
						this.whipDirection = Vector3.RotateTowards(this.whipDirection, new Vector3(base.transform.localScale.x, -1f, 0f), 0.08726646f, 0f);
						flag = Physics.Raycast(base.transform.position + this.whipOffset, this.whipDirection, out raycastHit, this.whipRange, this.groundLayer);
						UnityEngine.Debug.DrawRay(base.transform.position + this.whipOffset, this.whipDirection);
						if (flag)
						{
							Block component2 = raycastHit.collider.gameObject.GetComponent<Block>();
							if (component2 != null && !Map.IsBlockSolid(component2.collumn, component2.row + 1) && !Map.IsBlockSolid(component2.collumn - (int)base.transform.localScale.x, component2.row))
							{
								this.whipPullToPoint = Map.GetBlockCenter(component2.collumn - (int)base.transform.localScale.x, component2.row + 1);
							}
							else if (raycastHit.collider.gameObject.GetComponent<Helicopter>() != null)
							{
								this.whipPullToPoint = raycastHit.point;
								this.whipHitPoint = raycastHit.point;
								this.whipHitHelicopter = true;
							}
							else
							{
								if (block == null || block.row < component2.row)
								{
									block = component2;
									vector = raycastHit.point;
								}
								flag = false;
							}
						}
						num++;
					}
				}
				if (!flag && this.fire)
				{
					this.whipDirection = new Vector3(base.transform.localScale.x, 0f, 0f);
					flag = Physics.Raycast(base.transform.position + this.whipOffset, this.whipDirection, out raycastHit, this.whipRange, this.groundLayer);
					UnityEngine.Debug.DrawRay(base.transform.position + this.whipOffset, this.whipDirection);
					if (!flag)
					{
						num = 0;
						this.whipDirection = new Vector3(base.transform.localScale.x * 0.968f, 0.25f, 0f);
					}
					while (!flag && (double)this.whipDirection.y > -0.25 && num < 60)
					{
						flag = Physics.Raycast(base.transform.position + this.whipOffset, this.whipDirection, out raycastHit, this.whipRange, this.groundLayer);
						if (!flag)
						{
							this.whipDirection = Vector3.RotateTowards(this.whipDirection, new Vector3(base.transform.localScale.x * 0.968f, -0.26f, 0f), 0.08726646f, 0f);
						}
					}
					if (flag)
					{
						Block component3 = raycastHit.collider.gameObject.GetComponent<Block>();
						if (component3 != null && !Map.IsBlockSolid(component3.collumn, component3.row + 1) && !Map.IsBlockSolid(component3.collumn - (int)base.transform.localScale.x, component3.row))
						{
							this.whipPullToPoint = Map.GetBlockCenter(component3.collumn - (int)base.transform.localScale.x, component3.row + 1);
						}
						else if (raycastHit.collider.gameObject.GetComponent<Helicopter>() != null)
						{
							this.whipPullToPoint = raycastHit.point;
							this.whipHitPoint = raycastHit.point;
							this.whipHitHelicopter = true;
						}
						else
						{
							if (block == null || block.row < component3.row)
							{
								block = component3;
								vector = raycastHit.point;
							}
							flag = false;
						}
					}
				}
				if (num > 150)
				{
					UnityEngine.Debug.LogError("Evan is a bum head");
				}
			}
			if (!flag && block != null)
			{
				this.whipPullToPoint = Map.GetBlockCenter(block.collumn - (int)base.transform.localScale.x, block.row);
				flag = true;
				this.fallback = true;
			}
			if (!flag)
			{
				if (this.actionState == ActionState.Jumping && (this.buttonJump || this.up))
				{
					this.whipDirection = new Vector3(base.transform.localScale.x, 1f, 0f);
				}
				else
				{
					this.whipDirection = new Vector3(base.transform.localScale.x, 0f, 0f);
				}
				if (Physics.Raycast(base.transform.position + this.whipOffset, this.whipDirection, out raycastHit, this.whipRange * 0.8f, this.groundLayer | this.barrierLayer))
				{
					this.whipHitPoint = raycastHit.point;
				}
				else
				{
					this.whipHitPoint = base.transform.position + this.whipOffset + this.whipDirection.normalized * this.whipRange * 0.8f;
				}
			}
			if (flag)
			{
				if (!this.whipHitHelicopter)
				{
					this.whipHitPoint = ((!this.fallback) ? raycastHit.point : vector);
					this.whipAttachBlock = ((!this.fallback) ? raycastHit.collider.gameObject.GetComponent<Block>() : block);
					this.curWhipRange = Vector3.Distance(base.transform.position + this.whipOffset, (!this.fallback) ? this.whipHitPoint : vector);
					this.whipPullToPoint = ((!this.fallback) ? Map.GetBlockCenter(this.whipAttachBlock.collumn - (int)base.transform.localScale.x, this.whipAttachBlock.row + 1) : Map.GetBlockCenter(this.whipAttachBlock.collumn, this.whipAttachBlock.row));
				}
				else
				{
					this.whipPullToPoint = this.whipHitPoint;
				}
			}
			this.whipState = WhipState.Extending;
		}
	}

	protected override void UseFire()
	{
		if (this.chimneyFlip)
		{
			this.chimneyFlip = false;
			this.chimneyFlipFrames = 0;
		}
		this.whippingAnimation = true;
		this.doingMelee = false;
		this.meleeFrame = 0;
		this.frame = 0;
		if (Time.time - this.lastWhipTime < 0.133f)
		{
			UnityEngine.Debug.Log("Close follow whipping ");
			this.gunFrame = 7;
		}
		else
		{
			this.gunFrame = 9;
		}
		int num = 9 - this.gunFrame;
		this.SetWhipSprite(num);
		this.PlaySpecialAttackSound(0.3f);
	}

	protected override void PlayAttackSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.attackSounds, 0.55f, base.transform.position);
		}
	}

	protected override void CalculateMovement()
	{
		if (this.whipState == WhipState.Attached)
		{
			if (this.right || this.left)
			{
				Vector3 vector = this.whipHitPoint - (base.transform.position + this.whipOffset);
				Vector3 vector2 = new Vector3(vector.y, vector.x, 0f);
				if (this.left)
				{
					vector2.x *= -1f;
				}
				else if (this.right)
				{
					vector2.y *= -1f;
				}
				vector2.Normalize();
				if ((this.left && vector2.x < 0f) || (this.right && vector2.x > 0f))
				{
					this.xI += vector2.x * 1100f / 2f * this.t;
				}
				this.right = false; this.left = (this.right );
			}
			if (this.up)
			{
				this.curWhipRange -= this.whipRange * this.t;
			}
			if (this.down)
			{
				this.curWhipRange += this.whipRange * this.t;
			}
			this.curWhipRange = Mathf.Clamp(this.curWhipRange, 32f, this.whipRange);
		}
		base.CalculateMovement();
	}

	protected override void AnimateSpecial()
	{
		if (this.whipState != WhipState.Inactive)
		{
			this.DeactivateGun();
			this.frameRate = 0.0334f;
			int num = 16 + Mathf.Clamp(this.frame, 0, 4);
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), 32f);
			if (this.frame == 3)
			{
				this.UseWhip();
			}
			if (this.frame >= 4)
			{
				this.frame = 4;
				if (!this.fire)
				{
					this.frame = 0;
					this.ActivateGun();
					this.usingSpecial = false;
					this.whipState = WhipState.Retracting;
				}
			}
		}
	}

	protected override void RunFiring()
	{
		this.fireDelay -= this.t;
		if (this.fire)
		{
			this.rollingFrames = 0;
		}
		if (this.fire && !this.wasFire && !this.whippingAnimation)
		{
			this.UseFire();
			base.FireFlashAvatar();
		}
	}

	protected override void LateUpdate()
	{
		base.LateUpdate();
		if (this.whipState != WhipState.Inactive || this.whippingAnimation)
		{
			this.lastWhipTime = Time.time;
		}
		if (this.whipState == WhipState.Attached && this.health > 0)
		{
			this.canWallClimb = false;
			if (!this.whipHitHelicopter && (this.whipAttachBlock == null || this.whipAttachBlock.destroyed || this.whipAttachBlock.health <= 0 || !this.fire))
			{
				this.whipState = WhipState.Retracting;
				this.usingSpecial = false;
				return;
			}
			Vector3 b = base.transform.position + this.whipOffset + base.transform.right * this.xI * this.t + base.transform.up * this.yI * this.t;
			float magnitude = (this.whipHitPoint - b).magnitude;
			Vector3 a = this.whipPullToPoint - (base.transform.position + this.whipOffset);
			a.Normalize();
			Vector3 current = new Vector3(this.xI, this.yI, 0f);
			current = Vector3.MoveTowards(current, a * 1100f * Mathf.Clamp(magnitude / this.whipRange, 0.3f, 1f), 2200f * this.t);
			this.xI = current.x;
			this.yI = current.y;
			if (!this.whipHitHelicopter && !this.fallback && this.collumn == Map.GetCollumn(this.whipPullToPoint.x) && this.row == Map.GetRow(this.whipPullToPoint.y))
			{
				this.whipPullToPoint = Map.GetBlockCenter(this.whipAttachBlock.collumn, this.whipAttachBlock.row + 1);
				this.whipPullToPoint.y = this.whipPullToPoint.y - 8f;
			}
			if (base.transform.position.y < this.whipPullToPoint.y)
			{
				this.yI += 1100f * this.t;
			}
		}
		else
		{
			this.canWallClimb = true;
		}
		this.DrawWhip();
	}

	protected override void Land()
	{
		if ((this.whipState != WhipState.Inactive && this.whipHitPoint.y <= base.transform.position.y) || this.whipState == WhipState.Retracting)
		{
			this.usingSpecial = false;
			this.whipState = WhipState.Inactive;
		}
		base.Land();
	}

	protected void DrawWhip()
	{
		if (this.whipState == WhipState.Extending && this.health > 0)
		{
			float max = this.whipRange;
			RaycastHit raycastHit;
			if (this.whipAttachBlock == null)
			{
				if (Physics.Raycast(base.transform.position + this.whipOffset, this.whipDirection, out raycastHit, this.whipRange, this.groundLayer))
				{
					this.whipHitPoint = raycastHit.point;
				}
				else
				{
					this.whipHitPoint = base.transform.position + this.whipOffset + this.whipDirection.normalized * this.whipRange;
				}
			}
			if (Physics.Raycast(base.transform.position + this.whipOffset, this.whipHitPoint - (base.transform.position + this.whipOffset), out raycastHit, this.whipRange, this.groundLayer))
			{
				max = Vector3.Distance(raycastHit.point, base.transform.position + this.whipOffset);
			}
			float num = this.whipHitPoint.x - (base.transform.position.x + this.whipOffset.x);
			if (num > 0f != base.transform.localScale.x > 0f)
			{
				num *= -1f;
			}
			num *= Mathf.Sign(this.whipHitPoint.x - (base.transform.position.x + this.whipOffset.x));
			this.whipSprite.transform.eulerAngles = new Vector3(0f, 0f, 57.29578f * Mathf.Atan2(this.whipHitPoint.y - (base.transform.position.y + this.whipOffset.y), num));
			this.whipSprite.gameObject.SetActive(true);
			this.whipExtendTime += this.t;
			this.whipSprite.SetLowerLeftPixel(new Vector2(128f - Mathf.Clamp(this.whipExtendTime * 1280f, 0f, max), 8f));
			this.whipSprite.UpdateUVs();
		}
		else if (this.health > 0 && (this.whipState == WhipState.Attached || this.whipDrawTime >= 0f))
		{
			this.whipSprite.gameObject.SetActive(false);
			this.whipLineRenderer.enabled = true;
			this.whipLineRenderer.SetPosition(0, base.transform.position + this.whipOffset);
			this.whipLineRenderer.SetPosition(1, this.whipHitPoint);
			float magnitude = (this.whipHitPoint - (base.transform.position + this.whipOffset)).magnitude;
			this.whipLineRenderer.material.SetTextureScale("_MainTex", new Vector2(magnitude / 4f, 1f));
			this.whipDrawTime -= this.t;
		}
		else
		{
			this.whipSprite.gameObject.SetActive(false);
			this.whipLineRenderer.enabled = false;
		}
	}

	protected override void AddSpeedLeft()
	{
		if (this.xI > -25f)
		{
			this.xI = -25f;
		}
		if (this.xI - this.speed * 2f * this.t >= -((!this.dashing || this.ducking) ? this.speed : (this.speed * this.dashSpeedM)))
		{
			this.xI -= this.speed * ((!this.dashing || this.ducking) ? 2f : 4f) * this.t;
		}
		else if (this.xI < -((!this.dashing || this.ducking) ? this.speed : (this.speed * this.dashSpeedM)) && this.whipState == WhipState.Inactive)
		{
			this.xI = -((!this.dashing || this.ducking) ? this.speed : (this.speed * this.dashSpeedM));
		}
		else if (this.xI > -50f)
		{
			this.xI -= this.speed * 2.6f * this.t;
		}
	}

	protected override void AddSpeedRight()
	{
		if (this.xI < 25f)
		{
			this.xI = 25f;
		}
		if (this.xI + this.speed * 2f * this.t <= ((!this.dashing || this.ducking) ? this.speed : (this.speed * this.dashSpeedM)))
		{
			this.xI += this.speed * ((!this.dashing || this.ducking) ? 2f : 4f) * this.t;
		}
		else if (this.xI > ((!this.dashing || this.ducking) ? this.speed : (this.speed * this.dashSpeedM)) && this.whipState == WhipState.Inactive)
		{
			this.xI = ((!this.dashing || this.ducking) ? this.speed : (this.speed * this.dashSpeedM));
		}
		else if (this.xI < 50f)
		{
			this.xI += this.speed * 2.6f * this.t;
		}
	}

	protected WhipState whipState;

	protected Vector3 whipDirection;

	protected float whipVelocity = 350f;

	public float whipRange = 128f;

	public FlickerFader whipHitPuff;

	private bool fallback;

	protected Block whipAttachBlock;

	protected Vector3 whipHitPoint;

	protected Vector3 whipPullToPoint;

	protected float curWhipRange;

	private LineRenderer whipLineRenderer;

	public SpriteSM whipSprite;

	public Material whipMaterial;

	public int whipAnimationPixelSize = 64;

	public Material materialArmless;

	protected Material materialNormal;

	protected Material currentMaterial;

	protected bool whippingAnimation;

	protected float lastWhipTime;

	protected bool wallHasHit;

	protected bool wallHasHitForward;

	protected int meleeFrame;

	protected float meleeCounter;

	protected float meleeFrameRate = 0.03334f;

	protected Vector3 whipOffset = new Vector3(0f, 8f, 0f);

	protected float whipExtendTime;

	public float wallHitVolume = 0.2f;

	private bool whipHitHelicopter;

	private float whipDrawTime = -1f;
}
