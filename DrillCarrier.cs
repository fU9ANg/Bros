// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DrillCarrier : MookTruck
{
	protected override void Start()
	{
		base.Start();
		if (Physics.Raycast(new Vector3(this.x - 32f, this.y + 24f, 0f), Vector3.down, out this.raycastHit, 48f, this.groundLayer) && this.raycastHit.point.y > this.y)
		{
			this.y = this.raycastHit.point.y;
		}
		if (Physics.Raycast(new Vector3(this.x + 32f, this.y + 24f, 0f), Vector3.down, out this.raycastHit, 48f, this.groundLayer) && this.raycastHit.point.y > this.y)
		{
			this.y = this.raycastHit.point.y;
		}
		this.targetY = this.y - 12f;
		this.FindLowestGroundPoint();
		this.drillSprite.enabled = false;
		this.tracksSprite.enabled = false;
	}

	protected override void RunMovement()
	{
	}

	protected override void SetPosition(float xOffset)
	{
		if (this.forceXPos > 0f)
		{
			this.x = this.forceXPos;
		}
		base.SetPosition(xOffset);
	}

	protected override void BounceOnDeath()
	{
		if (this.health <= 0)
		{
			this.yI += 125f;
		}
		this.settled = false;
	}

	protected override void Update()
	{
		if (!Map.isEditing && !this.setupActivate)
		{
			this.setupActivate = true;
			this.CreateActivateAction();
			this.forceXPos = this.x;
			this.SetPosition(0f);
		}
		base.Update();
		if (this.health > 0 && this.chuckingMooks)
		{
			this.chuckingMooksCounter += this.t;
			if (this.chuckingMooksCounter > 0.5f)
			{
				this.chuckingMooksCounter -= 0.5f;
				this.FireWeapon();
				this.spawnFrame = 5;
				this.SetSpawnFrame();
			}
		}
		if (this.health > 0)
		{
			if (this.opening)
			{
				this.openingCounter += this.t;
				if (this.openingCounter >= 0.0334f)
				{
					this.openingCounter -= 0.0334f;
					this.openingFrame++;
					if (this.openingFrame >= 3)
					{
						this.opening = false;
						this.openingFrame = 3;
						this.spawnFrame = 2;
					}
					this.sprite.SetLowerLeftPixel((float)(this.openingFrame * 64), (float)((int)this.sprite.lowerLeftPixel.y));
				}
			}
			else if (!this.opening && this.spawnFrame > 0)
			{
				this.spawnCouner += this.t;
				if (this.spawnCouner >= 0.0444f)
				{
					this.spawnCouner -= 0.044f;
					this.spawnFrame--;
					this.SetSpawnFrame();
				}
			}
		}
	}

	protected override void SetHurtMaterial()
	{
		base.SetHurtMaterial();
		this.tracksSprite.GetComponent<Renderer>().sharedMaterial = this.hurtMaterial;
	}

	protected override void SetUnhurtMaterial()
	{
		base.SetUnhurtMaterial();
		this.tracksSprite.GetComponent<Renderer>().sharedMaterial = this.baseMaterial;
	}

	protected override void RunAnimations()
	{
	}

	protected void SetSpawnFrame()
	{
		switch (this.spawnFrame)
		{
		case 0:
			this.sprite.SetLowerLeftPixel(320f, (float)((int)this.sprite.lowerLeftPixel.y));
			return;
		case 1:
			this.sprite.SetLowerLeftPixel(256f, (float)((int)this.sprite.lowerLeftPixel.y));
			return;
		case 5:
			this.sprite.SetLowerLeftPixel(256f, (float)((int)this.sprite.lowerLeftPixel.y));
			return;
		}
		this.sprite.SetLowerLeftPixel(192f, (float)((int)this.sprite.lowerLeftPixel.y));
	}

	protected void CheckTargetY()
	{
		if (!Physics.Raycast(new Vector3(this.x - 32f, this.y + 24f, 0f), Vector3.up, out this.raycastHit, 32f, this.groundLayer) && Physics.Raycast(new Vector3(this.x - 32f, this.y + 48f, 0f), Vector3.down, out this.raycastHit, 80f, this.groundLayer) && this.raycastHit.point.y - 12f < this.targetY)
		{
			this.targetY = this.raycastHit.point.y - 12f;
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Stop Here ",
				this.targetY,
				" current ",
				this.y
			}));
		}
		if (!Physics.Raycast(new Vector3(this.x + 32f, this.y + 24f, 0f), Vector3.up, out this.raycastHit, 32f, this.groundLayer) && Physics.Raycast(new Vector3(this.x + 32f, this.y + 48f, 0f), Vector3.down, out this.raycastHit, 80f, this.groundLayer) && this.raycastHit.point.y - 12f < this.targetY)
		{
			this.targetY = this.raycastHit.point.y - 12f;
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Stop Here ",
				this.targetY,
				" current ",
				this.y
			}));
		}
	}

	protected void CheckDislodged()
	{
		bool flag = true;
		bool flag2 = true;
		bool flag3 = false;
		if (Physics.Raycast(new Vector3(this.x - 32f, this.y + 16f, 0f), Vector3.down, out this.raycastHit, 32f, this.groundLayer))
		{
			if (this.raycastHit.point.y - this.y < 0f)
			{
				flag3 = true;
			}
		}
		else
		{
			flag = false;
		}
		if (Physics.Raycast(new Vector3(this.x + 32f, this.y + 16f, 0f), Vector3.down, out this.raycastHit, 32f, this.groundLayer))
		{
			if (this.raycastHit.point.y - this.y < 0f && flag3)
			{
				flag = false;
				flag2 = false;
			}
		}
		else
		{
			flag2 = false;
		}
		if (!flag || !flag2)
		{
			this.Dislodge();
		}
	}

	protected void Dislodge()
	{
		this.health = -1;
		UnityEngine.Debug.Log("Dislodge");
		this.dislodged = true;
		this.settled = false;
		this.drillSprite.enabled = false;
		this.tracksSprite.enabled = false;
		base.GetComponent<AudioSource>().Stop();
	}

	protected override void GetGroundHeight()
	{
		this.groundHeight = -200f;
		this.leftGround = false;
		this.rightGround = false;
		this.midLeftGround = false;
		this.midRightGround = false;
		this.middleGround = false;
		float num = Mathf.Round(this.x) + 0.5f;
		if (Physics.Raycast(new Vector3(num, this.y + 6f, 0f), Vector3.down, out this.raycastHitMiddle, 64f, this.groundLayer))
		{
			if (this.raycastHitMiddle.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitMiddle.point.y;
			}
			if (this.raycastHitMiddle.point.y > this.y - 9f)
			{
				this.middleGround = true;
			}
		}
		if (Physics.Raycast(new Vector3(num - 8f, this.y + 6f, 0f), Vector3.down, out this.raycastHitMidLeft, 64f, this.groundLayer))
		{
			if (this.raycastHitMidLeft.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitMidLeft.point.y;
			}
			if (this.raycastHitMidLeft.point.y > this.y - 9f)
			{
				this.midLeftGround = true;
			}
		}
		if (Physics.Raycast(new Vector3(num + 8f, this.y + 6f, 0f), Vector3.down, out this.raycastHitMidRight, 64f, this.groundLayer))
		{
			if (this.raycastHitMidRight.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitMidRight.point.y;
			}
			if (this.raycastHitMidRight.point.y > this.y - 9f)
			{
				this.midRightGround = true;
			}
		}
		if (!Map.isEditing)
		{
			this.CheckCrushGroundWhenStanding();
		}
	}

	protected override void CheckCrushGroundWhenStanding()
	{
		if ((this.health <= 0 || this.dislodged) && ((!this.middleGround) ? 1 : 0) + ((!this.midLeftGround) ? 1 : 0) + ((!this.midRightGround) ? 1 : 0) >= 2)
		{
			this.DamageGroundBelow(true);
		}
	}

	protected override void DamageGroundBelow(bool forced)
	{
		if (this.delayDestroyTime > 0f)
		{
			return;
		}
		if (this.middleGround || this.midLeftGround || this.midRightGround)
		{
			this.yI = 0f;
		}
		if (this.middleGround && this.raycastHitMiddle.collider != null)
		{
			MapController.Damage_Local(this, this.raycastHitMiddle.collider.gameObject, 5, DamageType.Crush, 0f, 0f);
		}
		if (this.midLeftGround && this.raycastHitMidLeft.collider != null)
		{
			MapController.Damage_Local(this, this.raycastHitMidLeft.collider.gameObject, 5, DamageType.Crush, 0f, 0f);
		}
		if (this.midRightGround && this.raycastHitMidRight.collider != null)
		{
			MapController.Damage_Local(this, this.raycastHitMidRight.collider.gameObject, 5, DamageType.Crush, 0f, 0f);
		}
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		this.drillSprite.enabled = false;
		this.tracksSprite.enabled = false;
		base.GetComponent<AudioSource>().Stop();
		base.Death(xI, yI, damage);
		if (this.openingFrame > 1)
		{
			this.sprite.SetLowerLeftPixel(448f, (float)((int)this.sprite.lowerLeftPixel.y));
		}
		else
		{
			this.sprite.SetLowerLeftPixel(384f, (float)((int)this.sprite.lowerLeftPixel.y));
		}
	}

	protected void CreateActivateAction()
	{
		ActivateUnitWhenOnScreen activateUnitWhenOnScreen = base.GetComponent<ActivateUnitWhenOnScreen>();
		if (activateUnitWhenOnScreen == null)
		{
			activateUnitWhenOnScreen = base.gameObject.AddComponent<ActivateUnitWhenOnScreen>();
		}
		activateUnitWhenOnScreen.SetExtents(this.x - 16f, this.y, this.x + 16f, this.targetY);
	}

	protected override void RunAI()
	{
		this.wasUp = this.up;
		this.up = false; this.left = (this.right = (this.up ));
		if (this.health > 0 && this.activated && this.y < this.targetY)
		{
			this.up = true;
		}
	}

	protected override void RunInput()
	{
		if (this.health > 0 && this.up)
		{
			this.yI = 40f;
		}
	}

	protected void StopDrilling()
	{
		this.chuckingMooks = true;
		UnityEngine.Debug.Log("Stop Drilling " + base.name);
		if (Mathf.Abs(this.y - this.targetY) < 8f)
		{
			this.y = this.targetY;
		}
		this.SetPosition(0f);
		this.drillSprite.enabled = false;
		this.tracksSprite.enabled = false;
		this.activated = false;
		this.opening = true;
		base.GetComponent<AudioSource>().Stop();
		base.GetComponent<AudioSource>().clip = this.stopSound;
		base.GetComponent<AudioSource>().loop = false;
		base.GetComponent<AudioSource>().Play();
	}

	protected override void FireWeapon()
	{
		this.mookSpawnCount++;
		if (Connect.IsHost)
		{
			int num = this.mookSpawnCount % 2 * 2 - 1;
			int num2 = num;
			switch (num2 + 1)
			{
			case 0:
				if (Physics.Raycast(new Vector3(this.x + 8f, this.y + 24f, 0f), Vector3.left, out this.raycastHitMidRight, 64f, this.groundLayer) && !Physics.Raycast(new Vector3(this.x - 8f, this.y + 24f, 0f), Vector3.right, out this.raycastHitMidRight, 64f, this.groundLayer))
				{
					num *= -1;
				}
				break;
			case 2:
				if (Physics.Raycast(new Vector3(this.x - 8f, this.y + 24f, 0f), Vector3.right, out this.raycastHitMidRight, 64f, this.groundLayer) && !Physics.Raycast(new Vector3(this.x + 8f, this.y + 24f, 0f), Vector3.left, out this.raycastHitMidRight, 64f, this.groundLayer))
				{
					num *= -1;
				}
				break;
			}
			Sound.GetInstance().PlaySoundEffectAt(this.throwSound, 0.7f, base.transform.position, 0.6f + UnityEngine.Random.value * 0.2f);
			MapController.SpawnMook_Networked(this.mookTrooper, this.x + (float)(8 * num), base.transform.position.y + 32f, (float)((100 + this.mookSpawnCount % 5 * 25) * num), 150f, true, false, false, false, false);
		}
		if (this.mookSpawnCount > 7)
		{
			this.chuckingMooks = false;
		}
	}

	protected void BloodyDrill()
	{
		this.drillSprite.GetComponent<Renderer>().sharedMaterial = this.bloodyDrillMaterial;
	}

	protected override void RunStanding()
	{
		if (this.health > 0 && !this.dislodged)
		{
			if (this.activated)
			{
				if (this.y > 16f)
				{
					this.CheckTargetY();
				}
				float num = this.yI * this.t;
				this.y += num;
				if (this.up)
				{
					this.shakeCounter += this.t;
					this.SetPosition(global::Math.Sin(this.shakeCounter * 60f) * 1f);
					if (Map.HitUnits(this, 20, DamageType.Crush, 24f, 8f, this.x, this.y + 66f, 0f, this.yI, true, false))
					{
						this.BloodyDrill();
					}
					this.drillCounter += this.t;
					if (this.drillCounter > 0.0334f)
					{
						this.drillCounter -= 0.0334f;
						this.drillCount++;
						if (this.drillCount % 2 == 0)
						{
							MapController.DamageGround(this, 10, DamageType.Drill, 9f, this.x - 8f, this.y + 60f, null);
							MapController.DamageGround(this, 10, DamageType.Drill, 9f, this.x + 8f, this.y + 60f, null);
						}
						else
						{
							MapController.DamageGround(this, 10, DamageType.Drill, 4f, this.x, this.y + 72f, null);
						}
					}
				}
				if (this.y >= this.targetY)
				{
					if (this.y > this.targetY + 24f)
					{
						this.Dislodge();
					}
					else
					{
						this.StopDrilling();
					}
				}
			}
			else if (this.y > 16f)
			{
				this.CheckDislodged();
			}
		}
		else
		{
			base.RunStanding();
		}
	}

	protected void FindLowestGroundPoint()
	{
		float y = this.y;
		bool flag = false;
		while (this.y > 0f && !flag)
		{
			this.y -= 32f;
			if (Physics.Raycast(new Vector3(this.x, this.y - 6f, 0f), Vector3.up, out this.raycastHit, 48f, this.groundLayer))
			{
				y = this.raycastHit.point.y;
			}
			else if (Physics.Raycast(new Vector3(this.x - 16f, this.y - 6f, 0f), Vector3.up, out this.raycastHit, 48f, this.groundLayer))
			{
				y = this.raycastHit.point.y;
			}
			else if (Physics.Raycast(new Vector3(this.x + 16f, this.y - 6f, 0f), Vector3.up, out this.raycastHit, 48f, this.groundLayer))
			{
				y = this.raycastHit.point.y;
			}
			else
			{
				this.y = y;
				flag = true;
			}
		}
		if (!flag)
		{
			this.y = -64f;
		}
	}

	public override bool Activate()
	{
		UnityEngine.Debug.Log("ACTIVATE!!!!");
		this.activated = true;
		this.drillSprite.enabled = true;
		this.tracksSprite.enabled = true;
		this.drillingUpwards = true;
		base.GetComponent<AudioSource>().pitch = 0.85f + 0.1f * UnityEngine.Random.value;
		base.GetComponent<AudioSource>().Play();
		return true;
	}

	protected float targetY;

	public bool activated;

	protected bool wasUp;

	protected float shakeCounter;

	protected bool setupActivate;

	public AnimatedTexture drillSprite;

	public AnimatedTexture tracksSprite;

	protected bool middleGround;

	protected RaycastHit raycastHitMiddle;

	protected bool supportedBySides = true;

	protected bool dislodged;

	protected bool chuckingMooks;

	protected float chuckingMooksCounter;

	public Material bloodyDrillMaterial;

	protected float openingCounter;

	protected int openingFrame;

	protected bool opening;

	protected int spawnFrame;

	protected float spawnCouner;

	protected bool drillingUpwards = true;

	public AudioClip stopSound;

	public AudioClip throwSound;

	protected float forceXPos;

	protected float drillCounter;

	protected int drillCount;
}
