// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BossBlockSawBlade : BossBlockWeapon
{
	protected override void RunFiring()
	{
		if (this.currentBlade == null && !this.hasDetached)
		{
			this.currentBlade = (UnityEngine.Object.Instantiate(this.sawBladeEdgePrefab, base.transform.position, Quaternion.identity) as SawBlade);
			this.currentBlade.transform.parent = base.transform;
			this.currentBlade.transform.localPosition = this.GetSawBladeLocalPosition(this.firingFrame);
			this.currentBlade.hidden = true;
			this.currentBlade.detachDelay = 0.1f;
			this.currentBlade.name = this.currentBlade.name + "_" + this.bladeCount;
			this.currentBlade.invulnerabilityTime = 0f;
			this.bladeCount++;
		}
		this.frameCounter += this.t;
		if (this.frameCounter >= 0.045f)
		{
			this.frameCounter -= 0.045f;
			this.firingFrame++;
			if (this.currentBlade != null)
			{
				this.currentBlade.transform.localPosition = this.GetSawBladeLocalPosition(this.firingFrame);
				if (this.firingFrame > 4)
				{
					this.currentBlade.hidden = false;
				}
				if (!this.hasDetached && this.currentBlade.IsDetached())
				{
					UnityEngine.Debug.Log(string.Concat(new object[]
					{
						"The Blade detached itself ",
						this.currentBlade.name,
						" bladeCount ",
						this.bladeCount
					}));
					this.firingFrame = 23;
					this.hasDetached = true;
					this.currentBlade = null;
				}
			}
			if (this.bottomPieceTransform != null)
			{
				this.bottomPieceTransform.localPosition = this.GetBottomPieceLocalPosition(this.firingFrame);
			}
			if (this.firingFrame == this.fireFrame)
			{
				this.FireWeapon();
			}
			if (this.firingFrame == 30)
			{
				this.thinkCounter = this.fireDelay;
				this.firingFrame = this.restFrame;
				this.firing = false;
			}
			this.SetSpriteFrame(this.firingFrame, 0);
		}
	}

	protected override void SetSpriteFrame(int spriteCollumn, int spriteRow)
	{
		base.SetSpriteFrame(spriteCollumn, spriteRow);
	}

	protected override void LookForPlayer()
	{
		if (!Physics.Raycast(new Vector3(base.transform.position.x, base.transform.position.y - 33f, 0f), Vector3.down, 24f, Map.groundLayer))
		{
			base.LookForPlayer();
		}
	}

	protected override void FireWeapon()
	{
		UnityEngine.Debug.Log("Fire Weapon1!   ****************** ");
		if (this.currentBlade != null && !this.currentBlade.IsDetached())
		{
			this.currentBlade.Detach();
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.attackSounds, 0.5f, base.transform.position);
		}
		this.currentBlade = null;
		this.hasDetached = true;
	}

	protected Vector3 GetBottomPieceLocalPosition(int frame)
	{
		switch (frame)
		{
		case 0:
			return new Vector3(0f, 0f, 0f);
		case 1:
			return new Vector3(0f, -1f, 0f);
		case 2:
			return new Vector3(0f, -2f, 0f);
		case 3:
			return new Vector3(0f, -4f, 0f);
		case 4:
			return new Vector3(0f, -7f, 0f);
		case 5:
			return new Vector3(0f, -10f, 0f);
		case 6:
			return new Vector3(0f, -12f, 0f);
		case 7:
			return new Vector3(0f, -13f, 0f);
		case 8:
			return new Vector3(0f, -11f, 0f);
		case 9:
		case 10:
		case 11:
		case 12:
		case 13:
			return new Vector3(0f, -10f, 0f);
		case 14:
			return new Vector3(0f, -11f, 0f);
		case 15:
			return new Vector3(0f, -12f, 0f);
		case 16:
			return new Vector3(0f, -13f, 0f);
		case 17:
			return new Vector3(0f, -14f, 0f);
		case 18:
			return new Vector3(0f, -13f, 0f);
		case 19:
			return new Vector3(0f, -13f, 0f);
		case 20:
			return new Vector3(0f, -13f, 0f);
		case 21:
			return new Vector3(0f, -13f, 0f);
		case 22:
			return new Vector3(0f, -13f, 0f);
		case 23:
			return new Vector3(0f, -12f, 0f);
		case 24:
			return new Vector3(0f, -11f, 0f);
		case 25:
			return new Vector3(0f, -9f, 0f);
		case 26:
			return new Vector3(0f, -7f, 0f);
		case 27:
			return new Vector3(0f, -5f, 0f);
		case 28:
			return new Vector3(0f, -3f, 0f);
		case 29:
			return new Vector3(0f, -1f, 0f);
		case 30:
			return new Vector3(0f, 0f, 0f);
		default:
			return Vector3.zero;
		}
	}

	protected Vector3 GetSawBladeLocalPosition(int frame)
	{
		switch (frame)
		{
		case 0:
			return new Vector3(0f, 1f, 0f);
		case 1:
			return new Vector3(0f, -3f, 0f);
		case 2:
			return new Vector3(0f, -7f, 0f);
		case 3:
			return new Vector3(0f, -11f, 0f);
		case 4:
			return new Vector3(0f, -16f, 0f);
		case 5:
			return new Vector3(0f, -22f, 0f);
		case 6:
			return new Vector3(0f, -28f, 0f);
		case 7:
			return new Vector3(0f, -34f, 0f);
		case 8:
			return new Vector3(0f, -36f, 0f);
		case 9:
			return new Vector3(0f, -38f, 0f);
		case 10:
			return new Vector3(0f, -35f, 0f);
		case 11:
			return new Vector3(0f, -32f, 0f);
		case 12:
			return new Vector3(0f, -29f, 0f);
		case 13:
			return new Vector3(0f, -30f, 0f);
		case 14:
			return new Vector3(0f, -31f, 0f);
		case 15:
			return new Vector3(0f, -33f, 0f);
		case 16:
			return new Vector3(0f, -35f, 0f);
		case 17:
			return new Vector3(0f, -37f, 0f);
		case 18:
			return new Vector3(0f, -39f, 0f);
		case 19:
			return new Vector3(0f, -41f, 0f);
		case 20:
			return new Vector3(0f, -43f, 0f);
		case 21:
			return new Vector3(0f, -42f, 0f);
		case 22:
			return new Vector3(0f, -41f, 0f);
		case 23:
			return new Vector3(0f, -39f, 0f);
		case 24:
			return new Vector3(0f, -35f, 0f);
		case 25:
			return new Vector3(0f, -33f, 0f);
		case 26:
			return new Vector3(0f, -31f, 0f);
		case 27:
			return new Vector3(0f, -27f, 0f);
		case 28:
			return new Vector3(0f, -22f, 0f);
		case 29:
			return new Vector3(0f, -18f, 0f);
		case 30:
			return new Vector3(0f, -14f, 0f);
		default:
			return Vector3.zero;
		}
	}

	protected override void StartFiring()
	{
		this.hasDetached = false;
		this.firingFrame = 0;
		base.StartFiring();
	}

	protected override void Death()
	{
		if (this.currentBlade != null)
		{
			this.currentBlade.Detach();
			this.currentBlade = null;
		}
		if (this.bottomPieceTransform != null)
		{
			this.bottomPieceTransform.GetComponent<Collider>().enabled = false;
			this.bottomPieceTransform.gameObject.SetActive(false);
		}
		base.Death();
	}

	public Transform bottomPieceTransform;

	public SawBlade sawBladeEdgePrefab;

	protected int firingFrame;

	protected int bladeCount;

	protected SawBlade currentBlade;

	private bool hasDetached;
}
