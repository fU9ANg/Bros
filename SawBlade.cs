// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SawBlade : Doodad
{
	private void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.03334f);
		base.transform.Rotate(new Vector3(0f, 0f, -this.rotSpeed * this.t));
		if (!Map.isEditing)
		{
			this.sparkDelay -= this.t;
			if (this.hasDetached)
			{
				this.invulnerabilityTime -= this.t;
				this.RunMovement();
				this.detachDelay -= this.t;
			}
			if ((this.hitDelay -= this.t) < 0f)
			{
				this.hitDelay = 0.03334f;
				if (this.hasDetached)
				{
					int row = Map.GetRow(this.y);
					int collumn = Map.GetCollumn(this.x);
					if (Vector2.Distance(new Vector2((float)this.detachCol, (float)this.detachRow), new Vector2((float)collumn, (float)row)) > (float)this.maxTravelDistance)
					{
						this.Death();
					}
					if (this.detachDelay <= 0f && MapController.DamageGround(this, 15, DamageType.Drill, 24f, this.x + (float)(10 * this.rotationDirection), this.y, new Collider[]
					{
						base.GetComponent<Collider>()
					}))
					{
						EffectsController.CreateSparkShower(this.x + 16f, this.y, 20, 4f, 200f, 0f, -200f, 0.5f, 0.5f);
						this.rotSpeed *= 0.9f;
						if (this.hasBouncedOnGround && Vector2.Distance(this.lastTerrainHitPos, base.transform.position) > 12f)
						{
							this.lastTerrainHitPos = base.transform.position;
							this.terrainHitsLeft--;
							if (this.terrainHitsLeft <= 0)
							{
								this.Death();
							}
						}
					}
				}
				if (this.bloodCounter > 0.03f)
				{
					this.bloodCounter = Mathf.Lerp(this.bloodCounter, 0f, this.t * 4f);
					switch (UnityEngine.Random.Range(0, 4))
					{
					case 0:
						EffectsController.CreateBloodParticles(BloodColor.Red, this.x - 10f, this.y, (int)Mathf.Clamp(this.bloodCounter, 0f, 5f), 8f, 8f, 60f, 0f, 250f * Mathf.Clamp(this.bloodCounter, 0.5f, 1.2f) * (float)this.rotationDirection);
						break;
					case 1:
						EffectsController.CreateBloodParticles(BloodColor.Red, this.x + 10f, this.y, (int)Mathf.Clamp(this.bloodCounter, 0f, 5f), 8f, 8f, 60f, 0f, -250f * Mathf.Clamp(this.bloodCounter, 0.5f, 1.2f) * (float)this.rotationDirection);
						break;
					case 2:
						EffectsController.CreateBloodParticles(BloodColor.Red, this.x, this.y + 10f, (int)Mathf.Clamp(this.bloodCounter, 0f, 5f), 8f, 8f, (float)(60 * this.rotationDirection), 250f * Mathf.Clamp(this.bloodCounter, 0.5f, 1.2f) * (float)this.rotationDirection, 0f);
						break;
					case 3:
						EffectsController.CreateBloodParticles(BloodColor.Red, this.x, this.y - 10f, (int)Mathf.Clamp(this.bloodCounter, 0f, 5f), 8f, 8f, (float)(60 * this.rotationDirection), -250f * Mathf.Clamp(this.bloodCounter, 0.5f, 1.2f) * (float)this.rotationDirection, 0f);
						break;
					}
				}
				this.x = base.transform.position.x;
				this.y = base.transform.position.y;
				int damage = (!this.hasDetached) ? 1 : 5;
				if (!this.hidden)
				{
					if (Map.HitUnits(this, damage, DamageType.Drill, 10f, this.x - 8f, this.y, 0f, (float)(100 * this.rotationDirection), false, true))
					{
						this.bloodCounter += 1f;
						this.PlayHitSound();
						EffectsController.CreateBloodParticles(BloodColor.Red, this.x - 4f, this.y, 5, 8f, 8f, 60f, 0f, 350f * (float)this.rotationDirection);
						this.PossiblyIncreaseBladeBloodiness();
					}
					if (Map.HitUnits(this, damage, DamageType.Drill, 10f, this.x + 8f, this.y, 0f, -50f * (float)this.rotationDirection, false, true))
					{
						this.bloodCounter += 1f;
						EffectsController.CreateBloodParticles(BloodColor.Red, this.x + 4f, this.y, 5, 8f, 8f, 60f, 0f, -350f * (float)this.rotationDirection);
						this.PlayHitSound();
						this.PossiblyIncreaseBladeBloodiness();
					}
					if (Map.HitUnits(this, damage, DamageType.Drill, 6f, this.x, this.y + 8f, 50f * (float)this.rotationDirection, 0f, false, true))
					{
						this.bloodCounter += 1f;
						EffectsController.CreateBloodParticles(BloodColor.Red, this.x, this.y + 4f, 5, 8f, 8f, 60f, 350f * (float)this.rotationDirection, 0f);
						this.PlayHitSound();
						this.PossiblyIncreaseBladeBloodiness();
					}
					if (Map.HitUnits(this, damage, DamageType.Drill, 8f, this.x, this.y - 8f, -10f * (float)this.rotationDirection, 50f * (float)this.rotationDirection, false, true))
					{
						this.bloodCounter += 1f;
						EffectsController.CreateBloodParticles(BloodColor.Red, this.x - 4f, this.y, 5, 8f, 8f, 60f, -350f * (float)this.rotationDirection, 0f);
						this.PlayHitSound();
						this.PossiblyIncreaseBladeBloodiness();
					}
				}
			}
		}
	}

	private void PlayHitSound()
	{
		if (this.hasDetached)
		{
			Sound.GetInstance().PlaySoundEffectAt(this.detachedHitSound, 0.15f, base.transform.position);
		}
		else
		{
			Sound.GetInstance().PlaySoundEffectAt(this.hitSound, 0.25f, base.transform.position);
		}
	}

	private void PossiblyIncreaseBladeBloodiness()
	{
		if (this.bloodLevel == 0)
		{
			this.bloodLevel = 1;
			this.SetBloodinessFrame();
		}
		else if (this.bloodLevel == 1)
		{
			if (this.bloodCounter > 5f)
			{
				this.bloodLevel = 2;
			}
			this.SetBloodinessFrame();
		}
		else if (this.bloodLevel == 2)
		{
			if (this.bloodCounter > 10f)
			{
				this.bloodLevel = 3;
			}
			this.SetBloodinessFrame();
		}
	}

	private void SetBloodinessFrame()
	{
		base.GetComponent<SpriteSM>().SetLowerLeftPixel((float)(32 * this.bloodLevel), 32f);
	}

	private void RunMovement()
	{
		this.yI -= 1100f * this.t;
		if (this.detachDelay <= 0f && this.yI < 0f)
		{
			base.transform.position = new Vector3(this.x, this.y, base.transform.position.z);
			if (Physics.Raycast(base.transform.position - Vector3.left * 8f, Vector3.down, 16f, 1 << LayerMask.NameToLayer("zzzz")) || Physics.Raycast(base.transform.position, Vector3.down, 16f, 1 << LayerMask.NameToLayer("Ground")) || Physics.Raycast(base.transform.position + Vector3.left * 8f, Vector3.down, 16f, 1 << LayerMask.NameToLayer("Ground")))
			{
				this.yI *= -0.75f;
				this.hasBouncedOnGround = true;
				if (this.yI > 20f)
				{
					Sound.GetInstance().PlaySoundEffectAt(this.bounceSound, Mathf.Clamp(this.yI / 200f, 0.1f, 0.5f), base.transform.position);
				}
				if (this.sparkDelay <= 0f)
				{
					this.sparkDelay = 0.0334f;
					EffectsController.CreateSparkShower(this.x, this.y - 12f, 20, 4f, 200f, -this.xI / 2f, 0f, 0.5f, 0.5f);
				}
				if (Mathf.Abs(this.xI) < 150f)
				{
					this.xI = Mathf.Clamp(this.xI + this.rotSpeed * 0.1f, -150f, 150f);
					this.rotSpeed *= 0.9f;
				}
			}
		}
		this.x += this.xI * this.t;
		this.y += this.yI * this.t;
		base.transform.position = new Vector3(this.x, this.y, base.transform.position.z);
	}

	public override bool Damage(DamageObject damageObject)
	{
		if (damageObject.damageSender == this || (this.hasDetached && damageObject.damageType == DamageType.Drill))
		{
			return false;
		}
		UnityEngine.Debug.Log("Take Damage!");
		if (!this.hasDetached || this.invulnerabilityTime < 0f)
		{
			this.health -= damageObject.damage;
			if (this.health < 0)
			{
				if (!this.hasDetached)
				{
					this.Detach();
				}
				else
				{
					this.Death();
				}
			}
		}
		return true;
	}

	public override bool DamageOptional(DamageObject damageObject, ref bool showBulletHit)
	{
		return false;
	}

	public bool IsDetached()
	{
		return this.hasDetached;
	}

	public void Detach()
	{
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
		base.transform.parent = null;
		this.hasDetached = true;
		this.detachCol = this.collumn;
		this.detachRow = this.row;
		this.health = 3;
		this.detachTime = Time.time;
		SphereCollider component = base.GetComponent<SphereCollider>();
		if (component != null)
		{
			component.radius = 15f;
		}
	}

	public override void Death()
	{
		Sound.GetInstance().PlaySoundEffectAt(this.deathSound, 0.4f, base.transform.position);
		base.Death();
	}

	public const float GRAVITY = 1100f;

	public float rotSpeed;

	private int bloodLevel;

	private float t;

	private float hitDelay = 0.03334f;

	private float bloodCounter;

	private bool hasDetached;

	private int detachCol;

	private int detachRow;

	public int maxTravelDistance;

	private int terrainHitsLeft = 4;

	private bool hasBouncedOnGround;

	private float detachTime;

	[HideInInspector]
	public float detachDelay;

	protected float sparkDelay;

	[HideInInspector]
	public bool hidden;

	public int rotationDirection;

	public AudioClip hitSound;

	public AudioClip detachedHitSound;

	public AudioClip deathSound;

	public AudioClip[] bounceSound;

	public float invulnerabilityTime = 2.25f;

	private Vector2 lastTerrainHitPos;
}
