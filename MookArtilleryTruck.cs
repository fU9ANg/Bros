// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookArtilleryTruck : MookTruck
{
	protected override void Start()
	{
		base.Start();
		this.height = 26f;
		this.spritePixelHeight = (int)this.sprite.pixelDimensions.y;
	}

	protected override void FireWeapon()
	{
		this.firing = true;
	}

	public override bool CanFire()
	{
		return base.CanFire() && this.unfoldFrame > 58;
	}

	public override void SetTargetPlayerNum(int pN, Vector3 TargetPosition)
	{
		this.targetPosition = TargetPosition;
		this.targetPlayerNum = pN;
	}

	protected override void SetDeathFrame()
	{
		if (this.unfoldFrame > 46)
		{
			this.sprite.SetLowerLeftPixel((float)(this.spritePixelWidth * 1), (float)(this.spritePixelHeight * 5));
		}
		else
		{
			this.sprite.SetLowerLeftPixel((float)(this.spritePixelWidth * 0), (float)(this.spritePixelHeight * 5));
		}
	}

	protected override void AnimateIdle()
	{
		if (this.firing)
		{
			this.frameCounter += this.t;
			if (this.frameCounter > 0.0334f)
			{
				if (this.unfoldFrame < 59)
				{
					if (this.unfoldFrame < 30)
					{
						this.frameCounter -= 0.02f;
					}
					else
					{
						this.frameCounter -= 0.033f;
					}
					this.unfoldFrame++;
					if (this.unfoldFrame == 1)
					{
						this.PlaySettleClip();
					}
					if (this.unfoldFrame >= 50)
					{
						this.height = 34f;
					}
					if (this.unfoldFrame < 64)
					{
						this.sprite.SetLowerLeftPixel(new Vector2((float)(this.unfoldFrame * this.spritePixelWidth), (float)this.spritePixelHeight));
					}
					else
					{
						this.sprite.SetLowerLeftPixel(new Vector2((float)((this.unfoldFrame - 64) * this.spritePixelWidth), (float)(this.spritePixelHeight * 2)));
					}
				}
				else
				{
					this.frameCounter -= 0.0334f;
					this.firingFrame++;
					if (this.firingFrame == 6)
					{
						float num = 0.7f + UnityEngine.Random.value * 0.2f;
						float x = this.targetPosition.x;
						float y = this.targetPosition.y;
						if (!HeroController.PlayerIsAlive(this.targetPlayerNum))
						{
							this.targetPlayerNum = -1;
						}
						if (x > 0f)
						{
							float f = x - this.x;
							float num2 = y - this.y;
							num = 0.2f + Mathf.Abs(f) / 180f + num2 / 300f;
							if (num < 0.5f)
							{
								num = 0.5f;
							}
						}
						if (this.IsLocalMook)
						{
							ProjectileController.SpawnProjectileOverNetwork(this.shellPrefab, this, this.x + (float)(this.facingDirection * 9), this.y + 68f, (float)(this.facingDirection * 150) * num * (0.85f + UnityEngine.Random.value * 0.3f), 150f * num, true, -1, false, true);
						}
						Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.attackSounds, 0.4f, (SortOfFollow.instance.transform.position + base.transform.position) / 2f);
					}
					if (this.firingFrame >= 12)
					{
						this.firingFrame = 0;
						this.fireCount++;
						if (this.fireCount > 2)
						{
							this.firing = false;
							this.fireCount = 0;
						}
					}
					this.sprite.SetLowerLeftPixel(new Vector2((float)(this.firingFrame * this.spritePixelWidth), (float)(this.spritePixelHeight * 3)));
				}
			}
		}
	}

	protected override void RunInput()
	{
		if (this.health > 0)
		{
			if (!this.left || !this.right)
			{
				if (this.left && this.y <= this.groundHeight)
				{
					if (!this.engineAudio.isPlaying)
					{
						this.PlayRollingClip();
					}
					if (this.facingDirection > 0)
					{
						this.StartTurnLeft();
					}
					else if (this.IsMoveable())
					{
						base.transform.localScale = new Vector3(1f, 1f, 1f);
						this.xI = -80f;
					}
				}
				else if (this.right && this.y <= this.groundHeight)
				{
					if (!this.engineAudio.isPlaying)
					{
						this.PlayRollingClip();
					}
					if (this.facingDirection < 0)
					{
						this.StartTurnRight();
					}
					else if (this.IsMoveable())
					{
						base.transform.localScale = new Vector3(-1f, 1f, 1f);
						this.xI = 80f;
					}
				}
				else
				{
					if (!this.engineAudio.isPlaying || !(this.engineAudio.clip == this.engineRollingClip) || this.actionState != ActionState.Turning)
					{
					}
					this.xI = 0f;
				}
				if (this.xI != 0f)
				{
					this.actionState = ActionState.Rolling;
				}
				else if (this.actionState != ActionState.Turning)
				{
					this.actionState = ActionState.Idle;
				}
			}
			else if (!this.left && !this.right && this.engineAudio.isPlaying && this.engineAudio.isPlaying && this.engineAudio.clip == this.engineRollingClip && this.actionState != ActionState.Turning)
			{
				this.engineAudio.Stop();
			}
		}
	}

	protected override void PlayTurningClip()
	{
		this.PlayRollingClip();
	}

	protected override void SetSpriteTurn(int frame)
	{
		if (this.weapon != null)
		{
			this.weapon.SetSpriteTurn(frame);
		}
		this.sprite.SetLowerLeftPixel(new Vector2((float)(frame * this.spritePixelWidth), (float)(4 * this.spritePixelHeight)));
	}

	protected override void AnimateTurning()
	{
		if (this.facingDirection < 0 && base.transform.localScale.x < 0f)
		{
			this.frameCounter += this.t;
			if (this.frameCounter > 0.067f)
			{
				this.frameCounter -= 0.067f;
				this.turnFrame++;
				if (this.turnFrame > 4)
				{
					this.turnFrame = 3;
					base.transform.localScale = new Vector3(1f, 1f, 1f);
				}
				this.SetSpriteTurn(this.turnFrame);
			}
		}
		else if (this.facingDirection > 0 && base.transform.localScale.x > 0f)
		{
			this.frameCounter += this.t;
			if (this.frameCounter > 0.067f)
			{
				this.frameCounter -= 0.067f;
				this.turnFrame++;
				if (this.turnFrame > 4)
				{
					this.turnFrame = 3;
					base.transform.localScale = new Vector3(-1f, 1f, 1f);
				}
				this.SetSpriteTurn(this.turnFrame);
			}
		}
		else if (this.turnFrame > 0)
		{
			this.frameCounter += this.t;
			if (this.frameCounter > 0.067f)
			{
				this.frameCounter -= 0.067f;
				this.turnFrame--;
			}
			this.SetSpriteTurn(this.turnFrame);
			if (this.turnFrame == 0)
			{
				this.actionState = ActionState.Idle;
				if (this.engineAudio.isPlaying)
				{
					this.engineAudio.Stop();
				}
			}
		}
	}

	public Projectile shellPrefab;

	protected bool firing;

	protected int unfoldFrame;

	protected int firingFrame;

	protected int spritePixelHeight = 96;

	protected Vector3 targetPosition;

	protected int targetPlayerNum = -1;

	protected int fireCount;
}
