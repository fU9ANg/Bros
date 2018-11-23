// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BossGattlingMech : Boss
{
	protected override void Start()
	{
		this.startX = base.transform.position.x;
		this.bossAnimation["JumpFull"].speed = 0.5f;
		this.bossAnimation["Idle"].speed = 0.9f;
		this.bossAnimation["Crouch"].speed = 0.9f;
		this.bossAnimation["Fire"].speed = 0.25f;
		this.thinkCounter = 55f;
		this.bossAnimation.Play("Idle");
		this.bossAnimation.CrossFade("JumpFull", 0.5f);
		this.rayCastHits = new RaycastHit[6];
	}

	protected override void Update()
	{
		base.Update();
		switch (this.thinkState)
		{
		case 0:
			this.RunStanding();
			break;
		case 1:
			this.RunStanding();
			break;
		case 2:
			this.RunStanding();
			if (this.fireDelay > 0f)
			{
				this.fireDelay -= this.t;
			}
			else if (this.thinkCounter > 0.5f)
			{
				this.fireCounter += this.t;
				if (this.fireCounter > 0.03f)
				{
					this.fireCounter -= 0.05f;
					this.nozzleCount++;
					ProjectileController.SpawnProjectileLocally(this.bigBullet, this, this.nozzles[this.nozzleCount % this.nozzles.Length].position.x - base.transform.localScale.x * 16f, this.nozzles[this.nozzleCount % this.nozzles.Length].position.y, -base.transform.localScale.x * 620f, 2f - UnityEngine.Random.value * 1f, -1);
				}
			}
			break;
		case 3:
			this.RunStanding();
			break;
		case 4:
			this.RunStanding();
			this.x += (float)(this.walkDirection * 30) * this.t;
			if (this.walkDirection < 0 && this.x < this.startX - 100f)
			{
				this.walkDirection *= -1;
			}
			else if (this.walkDirection > 0 && this.x > this.startX + 250f)
			{
				this.walkDirection *= -1;
			}
			break;
		default:
			this.RunStanding();
			this.thinkState = 0;
			break;
		}
	}

	protected override void Think()
	{
		switch (this.thinkState)
		{
		case 0:
			this.thinkState += 3;
			this.bossAnimation.CrossFade("Crouch", 0.2f);
			this.thinkCounter = 1.5f;
			break;
		case 1:
			this.thinkState++;
			this.bossAnimation.CrossFade("Fire", 0.2f);
			this.fireDelay = 0.4f;
			this.thinkCounter = 3f;
			break;
		case 2:
			this.thinkState++;
			this.bossAnimation["Crouch"].enabled = true;
			this.bossAnimation["Crouch"].time = this.bossAnimation["Crouch"].length;
			this.thinkCounter = 1f;
			break;
		case 3:
			this.thinkState++;
			this.bossAnimation.CrossFade("Idle", 0.9f);
			this.thinkCounter = 1.4f;
			break;
		case 4:
			this.thinkState++;
			this.bossAnimation.CrossFade("JumpFull", 0.5f);
			this.thinkCounter = 5.5f;
			break;
		default:
			this.thinkState = 0;
			this.thinkCounter = 0.2f;
			break;
		}
	}

	protected override void RunStanding()
	{
		base.RunStanding();
	}

	protected override void GetGroundHeight()
	{
		this.groundHeight = -200f;
		bool[] array = new bool[this.rayCastHits.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.CheckGroundHeight((float)(array.Length - 1) * 0.5f * -16f + (float)(i * 16), 6f, ref this.groundHeight, ref this.rayCastHits[0]);
		}
		this.CrushGround(array, this.rayCastHits);
	}

	protected override void CrushGround(bool[] groundChecks, RaycastHit[] hits)
	{
		if (!this.IsAcceptableGround(groundChecks))
		{
			for (int i = 0; i < groundChecks.Length; i++)
			{
				if (groundChecks[i])
				{
					hits[i].collider.gameObject.SendMessage("Damage", new DamageObject(5, DamageType.Crush, 0f, 0f, null), SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	public Transform[] nozzles;

	protected int nozzleCount;

	protected float fireDelay = 0.5f;

	protected float fireCounter;

	protected int walkDirection = -1;

	protected float startX;

	public Projectile bigBullet;
}
