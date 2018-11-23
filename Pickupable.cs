// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Pickupable : BroforceObject
{
	protected virtual void Awake()
	{
		this.pickupFullBubble.gameObject.SetActive(false);
	}

	public virtual void Collect(TestVanDammeAnim hero)
	{
		if (!hero.IsAmmoFull() || GameModeController.GameMode == GameMode.ExplosionRun)
		{
			EffectsController.CreateAmmoBubble(this.x, this.y - 15f);
			hero.ResetSpecialAmmo();
			this.Collect();
			if (GameModeController.GameMode == GameMode.ExplosionRun)
			{
				SortOfFollow.SlowTimeDown(1.25f);
				SortOfFollow.ResetSpeed();
			}
		}
		else if (!this.pickupFullBubble.gameObject.activeSelf)
		{
			this.pickupFullBubble.RestartBubble();
		}
	}

	protected virtual void Collect()
	{
		if (this.soundHolder != null)
		{
			if (!this.unshiftedSound)
			{
				if (Time.time - Pickupable.lastPickupTime < 0.25f)
				{
					Pickupable.pitchCount++;
					if (Pickupable.pitchCount > 15)
					{
						Pickupable.pitchCount = 15;
					}
				}
				else
				{
					Pickupable.pitchCount = 0;
				}
				Pickupable.lastPickupTime = Time.time;
				Sound instance = Sound.GetInstance();
				instance.PlaySoundEffectAt(this.soundHolder.deathSounds, 0.5f, base.transform.position, 0.8f + (float)Pickupable.pitchCount * 0.05f);
			}
			else
			{
				Sound instance2 = Sound.GetInstance();
				instance2.PlaySoundEffectAt(this.soundHolder.deathSounds, 0.5f, base.transform.position);
			}
		}
		this.Death();
	}

	protected virtual void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		this.pickupDelay -= this.t;
		this.yI -= 700f * this.t;
		this.RunMovement();
	}

	protected virtual void SetPosition(float xOffset)
	{
		base.transform.position = new Vector3(this.x + xOffset, this.y, 0f);
	}

	protected virtual void RunMovement()
	{
		bool bounceY = false;
		bool bounceX = false;
		this.yIT = this.yI * this.t;
		this.xIT = this.xI * this.t;
		if (this.settled)
		{
			if (!Map.ConstrainToBlocks(this.x, this.y, this.radius, ref this.xIT, ref this.yIT, ref bounceX, ref bounceY))
			{
				this.settled = false;
				this.shakeTime = 0.23f;
			}
			else
			{
				this.yI = 0f; this.xI = (this.yI );
			}
			this.SetPosition(0f);
		}
		else if (this.shakeTime > 0f)
		{
			this.shakeTime -= this.t * (0.4f + this.shakeTime * 3f);
			this.SetPosition(Mathf.Sin(this.shakeTime * 120f));
			this.yI = 0f; this.xI = (this.yI );
		}
		else
		{
			if (Map.ConstrainToBlocks(this.x, this.y, this.radius, ref this.xIT, ref this.yIT, ref bounceX, ref bounceY))
			{
				this.Bounce(bounceX, bounceY);
			}
			this.x += this.xIT;
			this.y += this.yIT;
			this.SetPosition(0f);
		}
	}

	protected virtual void Bounce(bool bounceX, bool bounceY)
	{
		if (bounceX)
		{
			this.xI *= -0.6f;
			this.yI *= 0.8f;
		}
		if (bounceY)
		{
			this.xI *= 0.8f;
			if (Mathf.Abs(this.yI) > 30f)
			{
				if (Mathf.Abs(this.yI) > 130f)
				{
					EffectsController.CreateLandPoofEffect(this.x + this.xIT, this.y + this.yIT - 4f, 0);
				}
				this.yI *= -0.4f;
				Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.hitSounds, 0.02f + Mathf.Clamp(Mathf.Abs(this.yI) * 0.002f, 0f, 0.3f), base.transform.position);
			}
			else
			{
				this.yI = 0f;
				this.settled = true;
			}
		}
	}

	public void Launch(float x, float y, float xI, float yI)
	{
		base.gameObject.SetActive(true);
		this.x = x;
		this.y = y;
		this.xI = xI;
		this.yI = yI;
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.effortSounds, 0.33f, base.transform.position);
		this.AddPickupable();
	}

	protected virtual void AddPickupable()
	{
		this.pickupValue.amount = UnityEngine.Random.Range(this.pickupValue.minAmount, this.pickupValue.maxAmount);
		PickupableController.AddPickupable(this);
	}

	protected virtual void Death()
	{
		UnityEngine.Object.Destroy(base.gameObject);
		PickupableController.RemovePickupable(this);
		EffectsController.CreatePuffDisappearEffect(this.x, this.y + 2f, 0f, 0f);
	}

	protected virtual void RunLife()
	{
	}

	public SoundHolder soundHolder;

	public float radius = 4f;

	public float yOffset;

	public PickupValue pickupValue;

	public Color sparkleColor;

	protected static int pitchCount;

	protected static float lastPickupTime;

	public bool unshiftedSound;

	protected float t = 0.033f;

	protected bool settled;

	protected float shakeTime;

	public float pickupDelay = 0.3f;

	private float yIT;

	private float xIT;

	protected Sound sound;

	public ReactionBubble pickupFullBubble;
}
