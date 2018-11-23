// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookTruck : Tank
{
	protected override void Start()
	{
		base.Start();
		this.height = 22f;
		this.spritePixelWidth = (int)this.sprite.pixelDimensions.x;
	}

	protected override void GiveHeroesDeathGrace()
	{
	}

	protected override void FireWeapon()
	{
		if ((this.fireDelay -= this.t) < 0f && this.mookSpawnCount < this.mooksToSpawn)
		{
			this.fireDelay = 0.6f;
			this.mookSpawnCount++;
			if (Connect.IsHost)
			{
				MapController.SpawnMook_Networked(this.mookTrooper, this.x + 24f, this.y + 32f, (float)(40 + this.mookSpawnCount % 5 * 15), 150f, true, false, false, false, this.enemyAI.mentalState == MentalState.Alerted);
			}
		}
	}

	protected override void NotifyDeath()
	{
		if (!this.notifiedDeath)
		{
			this.notifiedDeath = true;
			StatisticsController.NotifyTruckDeath(this);
		}
	}

	protected override void HitUnits()
	{
		if (this.yI < -310f || this.hasLanded)
		{
			base.HitUnits();
		}
		else
		{
			this.HitUnitsMooksOnly();
		}
	}

	protected override void Land()
	{
		if (this.yI < -210f)
		{
			this.hasLanded = true;
		}
		base.Land();
	}

	protected override void RunInput()
	{
		if (this.health > 0 && (!this.left || !this.right))
		{
			if (this.left && this.y <= this.groundHeight)
			{
				if (!this.engineAudio.isPlaying)
				{
					this.PlayRollingClip();
				}
				base.transform.localScale = new Vector3(1f, 1f, 1f);
				this.xI = -80f;
			}
			else if (this.right && this.y <= this.groundHeight)
			{
				if (!this.engineAudio.isPlaying)
				{
					this.PlayRollingClip();
				}
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
				this.xI = 80f;
			}
			else
			{
				if (this.engineAudio.isPlaying && this.engineAudio.clip == this.engineRollingClip)
				{
					this.PlaySettleClip();
				}
				this.xI = 0f;
			}
			if (this.xI != 0f)
			{
				this.actionState = ActionState.Rolling;
			}
			else
			{
				this.actionState = ActionState.Idle;
			}
			this.turnFrame = 0;
		}
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		base.Death(xI, yI, damage);
		if (!this.isDead)
		{
			this.isDead = true;
			this.BounceOnDeath();
		}
	}

	protected virtual void BounceOnDeath()
	{
		this.yI = 250f;
		this.y += 4f;
		this.xI = 0f;
		this.settled = false;
		this.colliders.localPosition = Vector3.up * -2f;
	}

	protected override void SetDeathFrame()
	{
		this.sprite.SetLowerLeftPixel((float)(this.spritePixelWidth * 2), (float)((int)this.sprite.lowerLeftPixel.y));
	}

	protected override void AnimateRolling()
	{
		this.frameCounter += this.t;
		if (this.frameCounter > 0.0334f)
		{
			this.frameCounter -= 0.0334f;
			this.frame++;
			this.sprite.SetLowerLeftPixel(new Vector2((float)(this.frame % 2 * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
			this.colliders.localPosition = Vector3.up * (float)(this.frame % 2);
		}
	}

	protected override void RunAI()
	{
		this.left = false;
		this.right = false;
		if ((this.weapon != null && this.weapon.health <= 0) || this.health <= 0)
		{
			return;
		}
		this.enemyAI.GetInput(ref this.left, ref this.right, ref this.up, ref this.down, ref this.jump, ref this.fire, ref this.special, ref this.special2, ref this.special3, ref this.special4);
		if (this.fire)
		{
			this.FireWeapon();
		}
		if (this.special)
		{
			this.UseSpecial();
		}
	}

	public Mook mookTrooper;

	protected int mookSpawnCount;

	public int mooksToSpawn;

	public Transform colliders;

	protected bool hasLanded;

	private float fireDelay = 0.6f;

	protected new bool isDead;
}
