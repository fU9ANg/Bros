// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ClonePod : Doodad
{
	protected override void AttachDoodad()
	{
		base.AttachDoodad();
	}

	protected override void Start()
	{
		base.Start();
		this.sprite = base.GetComponent<SpriteSM>();
		this.spritePixelWidth = (int)this.sprite.pixelDimensions.x;
		this.normalMaterial = this.sprite.GetComponent<Renderer>().sharedMaterial;
		this.offsetX = 16f;
		this.offsetY = 32f;
		this.startHealth = this.health;
		this.health = 200;
	}

	private void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (Map.isEditing)
		{
			return;
		}
		if (this.hurtCounter > 0f)
		{
			this.hurtCounter -= this.t;
			if (this.hurtCounter <= 0f)
			{
				this.sprite.GetComponent<Renderer>().sharedMaterial = this.normalMaterial;
			}
		}
		if (this.waitingToDrop)
		{
			if (this.offScreenDelay > 0f)
			{
				this.offScreenDelay -= this.t;
			}
			this.dropCounter += this.t;
			if (this.dropCounter > 0.0333f)
			{
				this.dropCounter -= 0.0333f;
				float f = 0f;
				float num = 0f;
				if (SortOfFollow.IsItSortOfVisible(base.transform.position, 16f, 32f, ref f, ref num))
				{
					int num2 = -1;
					if (HeroController.CanSeePlayer(this.x + 16f, this.y - 32f, 24f, 256f, ref num2))
					{
						this.Disturb();
					}
				}
				else if (Mathf.Abs(f) > 150f)
				{
				}
			}
		}
		else if (this.disturbed)
		{
			if (this.shakeDelay > 0f)
			{
				this.shakeDelay -= this.t;
				float position = global::Math.Sin(this.shakeDelay * 60f) * 2f * (this.shakeDelay * 3f);
				this.SetPosition(position);
			}
			else
			{
				this.GetGroundHeight();
				if (this.y + this.bottomYOffset > this.groundHeight)
				{
					this.yI -= 1100f * this.t;
					if (this.yI >= -150f || Map.HitUnits(this, 25, DamageType.Crush, 20f, 6f, this.x + 16f, this.y - 6f, 0f, 0f, true, true))
					{
					}
					if (this.yI < -600f)
					{
						this.yI = -600f;
					}
					float num3 = this.yI * this.t;
					if (this.y + num3 + this.bottomYOffset <= this.groundHeight)
					{
						if (this.yI < -210f)
						{
							this.y = this.groundHeight - this.bottomYOffset;
							this.yI = 0f;
							this.extraGroundCrush = true;
							this.GetGroundHeight();
							this.DamageGroundBelow(true);
							SortOfFollow.Shake(1f, 1f, base.transform.position);
							if (this.health > this.startHealth)
							{
								this.health = this.startHealth;
							}
						}
						else if (this.extraGroundCrush)
						{
							this.extraGroundCrush = false;
							this.y = this.groundHeight - this.bottomYOffset;
							this.yI = 0f;
							this.GetGroundHeight();
							this.DamageGroundBelow(true);
							SortOfFollow.Shake(0.2f, 1f, base.transform.position);
						}
						else
						{
							this.y = this.groundHeight - this.bottomYOffset;
							this.yI = 0f;
							this.disturbed = false;
							if (this.openingFrame < 5)
							{
								this.opening = true;
							}
							this.doorsCollider.gameObject.SetActive(false);
							UnityEngine.Debug.Log("Un disturb ");
							this.openingCounter = -0.5f;
							SortOfFollow.Shake(0.1f, 1f, base.transform.position);
						}
					}
					else
					{
						this.y += num3;
					}
					this.SetPosition(0f);
				}
			}
		}
		else
		{
			this.GetGroundHeight();
			if (this.groundHeight + 1f < this.y + this.bottomYOffset)
			{
				this.Disturb();
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
					if (this.openingFrame >= 5)
					{
						this.opening = false;
						this.openingFrame = 5;
						this.spawnFrame = 2;
						this.chuckingMooks = true;
					}
					this.sprite.SetLowerLeftPixel((float)(this.openingFrame * this.spritePixelWidth), (float)((int)this.sprite.lowerLeftPixel.y));
				}
			}
			else if (this.spawnFrame > 0)
			{
				this.openingCounter += this.t;
				if (this.openingCounter >= 0.0334f)
				{
					this.openingCounter -= 0.0334f;
					this.spawnFrame--;
					this.SetSpawnFrame();
				}
			}
			if (!this.waitingToDrop && !this.disturbed && this.openingFrame >= 5 && this.chuckingMooks)
			{
				this.spawnCouner += this.t;
				if (this.spawnCouner > 0.5f)
				{
					this.spawnCouner -= 0.5f;
					UnityEngine.Debug.Log("Spawn");
					this.SpawnMook();
					this.spawnFrame = 5;
					this.openingCounter = 0f;
					this.SetSpawnFrame();
				}
			}
		}
	}

	protected virtual void SpawnMook()
	{
		this.mookSpawnCount++;
		if (Connect.IsHost)
		{
			int num = this.mookSpawnCount % 2 * 2 - 1;
			int num2 = num;
			switch (num2 + 1)
			{
			case 0:
				if (Physics.Raycast(new Vector3(this.x + 8f, this.y + 24f, 0f), Vector3.left, out this.raycastHitLeft, 64f, this.groundLayer) && !Physics.Raycast(new Vector3(this.x - 8f, this.y + 24f, 0f), Vector3.right, out this.raycastHitLeft, 64f, this.groundLayer))
				{
					num *= -1;
				}
				break;
			case 2:
				if (Physics.Raycast(new Vector3(this.x - 8f, this.y + 24f, 0f), Vector3.right, out this.raycastHitRight, 64f, this.groundLayer) && !Physics.Raycast(new Vector3(this.x + 8f, this.y + 24f, 0f), Vector3.left, out this.raycastHitRight, 64f, this.groundLayer))
				{
					num *= -1;
				}
				break;
			}
			Sound.GetInstance().PlaySoundEffectAt(this.throwSound, 0.7f, base.transform.position, 0.6f + UnityEngine.Random.value * 0.2f);
			MapController.SpawnMook_Networked(this.mookPrefab, this.x + 16f + (float)(12 * num), base.transform.position.y + 32f, (float)((100 + this.mookSpawnCount % 5 * 25) * num), 150f, true, false, false, false, false);
		}
		if (this.mookSpawnCount > 7)
		{
			this.chuckingMooks = false;
		}
	}

	protected void SetSpawnFrame()
	{
		switch (this.spawnFrame)
		{
		case 0:
			this.sprite.SetLowerLeftPixel((float)(5 * this.spritePixelWidth), (float)((int)this.sprite.lowerLeftPixel.y));
			return;
		case 1:
			this.sprite.SetLowerLeftPixel((float)(4 * this.spritePixelWidth), (float)((int)this.sprite.lowerLeftPixel.y));
			return;
		case 5:
			this.sprite.SetLowerLeftPixel((float)(4 * this.spritePixelWidth), (float)((int)this.sprite.lowerLeftPixel.y));
			return;
		}
		this.sprite.SetLowerLeftPixel((float)(3 * this.spritePixelWidth), (float)((int)this.sprite.lowerLeftPixel.y));
	}

	public new void Disturb()
	{
		if (BossBlocksMover.CanDropHere(this.x + 16f, this.y, 32f))
		{
			this.waitingToDrop = false;
			this.disturbed = true;
			this.shakeDelay = 0.33f;
			UnityEngine.Debug.Log("Disturb !! ");
			this.isDamageable = true;
			Map.RegisterDestroyableDoodad(this);
		}
	}

	public override bool DamageOptional(DamageObject damageObject, ref bool showBulletHit)
	{
		if (Time.time - this.lastDoodadHurtTime > 0.06f)
		{
			this.lastDoodadHurtTime = Time.time;
			return this.Damage(damageObject);
		}
		return false;
	}

	public override bool Damage(DamageObject damageObject)
	{
		if (this.health > 0)
		{
			this.health -= damageObject.damage;
			if (this.health > 0)
			{
				this.hurtCounter = 0.045f;
				this.sprite.GetComponent<Renderer>().sharedMaterial = this.hurtMaterial;
				if (this.waitingToDrop)
				{
					this.Disturb();
				}
			}
			else
			{
				EffectsController.CreateGibs(this.gibHolderPrefab, this.x + 16f, this.y, 100f, 80f, 0f, 0f);
				Map.RemoveDestroyableDoodad(this);
				this.yI = 220f;
				this.y += 1f;
				this.disturbed = true;
				this.shakeDelay = 0f;
				this.extraGroundCrush = true;
				this.sprite.SetLowerLeftPixel((float)(7 * this.spritePixelWidth), (float)((int)this.sprite.lowerLeftPixel.y));
				this.spawnFrame = 0;
				this.opening = false;
				this.ceilingCollider.gameObject.SetActive(false);
				this.doorsCollider.gameObject.SetActive(false);
				EffectsController.CreateExplosion(this.x + 16f, this.y + 16f, 16f, 16f, 150f, 1f, 100f, 1f, 1f, false);
			}
		}
		return true;
	}

	protected virtual void GetGroundHeight()
	{
		this.groundHeight = -200f;
		this.leftGround = false;
		this.rightGround = false;
		this.middleGround = false;
		float num = Mathf.Round(this.x) + 0.5f;
		if (Physics.Raycast(new Vector3(num + 16f, this.y + 1f, 0f), Vector3.down, out this.raycastHitMiddle, 64f, this.groundLayer))
		{
			if (this.raycastHitMiddle.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitMiddle.point.y;
			}
			if (this.raycastHitMiddle.point.y > this.y + this.bottomYOffset - 9f)
			{
				this.middleGround = true;
			}
		}
		if (Physics.Raycast(new Vector3(num + 32f, this.y + 1f, 0f), Vector3.down, out this.raycastHitRight, 64f, this.groundLayer))
		{
			if (this.raycastHitRight.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitRight.point.y;
			}
			if (this.raycastHitRight.point.y > this.y + this.bottomYOffset - 9f)
			{
				this.rightGround = true;
			}
		}
		if (Physics.Raycast(new Vector3(num, this.y + 1f, 0f), Vector3.down, out this.raycastHitLeft, 64f, this.groundLayer))
		{
			if (this.raycastHitLeft.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitLeft.point.y;
			}
			if (this.raycastHitLeft.point.y > this.y + this.bottomYOffset - 9f)
			{
				this.leftGround = true;
			}
		}
		if (!Map.isEditing)
		{
			this.CheckCrushGroundWhenStanding();
		}
	}

	protected virtual void CheckCrushGroundWhenStanding()
	{
		if (((!this.leftGround) ? 1 : 0) + ((!this.middleGround) ? 1 : 0) + ((!this.rightGround) ? 1 : 0) >= 2)
		{
			this.DamageGroundBelow(true);
		}
	}

	protected virtual void DamageGroundBelow(bool forced)
	{
		if ((!this.leftGround && !this.middleGround) || (!this.rightGround && !this.middleGround))
		{
		}
		if (this.leftGround || this.middleGround || this.rightGround)
		{
			this.yI = 0f;
		}
		if (this.middleGround && this.raycastHitMiddle.collider != null)
		{
			MapController.Damage_Local(this, this.raycastHitMiddle.collider.gameObject, 5, DamageType.Crush, 0f, 0f);
		}
		if (this.leftGround && this.raycastHitLeft.collider != null)
		{
			MapController.Damage_Local(this, this.raycastHitLeft.collider.gameObject, 5, DamageType.Crush, 0f, 0f);
		}
		if (this.rightGround && this.raycastHitRight.collider != null)
		{
			MapController.Damage_Local(this, this.raycastHitRight.collider.gameObject, 5, DamageType.Crush, 0f, 0f);
		}
	}

	protected virtual void SetPosition(float xOffset)
	{
		base.transform.position = new Vector3(this.x + xOffset, this.y, 5f);
	}

	protected float shakeDelay = 0.3f;

	protected bool disturbed;

	protected bool waitingToDrop = true;

	protected float dropCounter;

	protected float offScreenDelay = 0.1f;

	protected float bottomYOffset = -8f;

	protected float openingCounter;

	protected int openingFrame;

	protected bool opening;

	protected int spawnFrame;

	protected float spawnCouner;

	protected bool chuckingMooks;

	protected int mookSpawnCount;

	public AudioClip stopSound;

	public AudioClip throwSound;

	public Material hurtMaterial;

	protected Material normalMaterial;

	protected float hurtCounter;

	public Collider doorsCollider;

	public Collider ceilingCollider;

	protected int startHealth = 20;

	public Mook mookPrefab;

	protected SpriteSM sprite;

	protected int spritePixelWidth = 70;

	protected bool extraGroundCrush = true;

	private float t;

	protected float lastDoodadHurtTime;

	protected float groundHeight;

	protected RaycastHit raycastHitMiddle;

	protected RaycastHit raycastHitLeft;

	protected RaycastHit raycastHitRight;

	protected RaycastHit raycastHit;

	protected bool leftGround;

	protected bool rightGround;

	protected bool middleGround;
}
