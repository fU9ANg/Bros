// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class VictoryMookDeath : SimpleSpriteWrapper
{
	protected override void Awake()
	{
		base.Awake();
		if (!Map.isEditing && Map.IsFinished())
		{
			VictoryMookDeath.victoryDeathCount++;
			base.name += VictoryMookDeath.victoryDeathCount;
		}
	}

	protected override void Start()
	{
		base.Start();
		if (!this.hasSetup || this.deathObject.deathType == DeathType.None)
		{
			base.gameObject.SetActive(false);
		}
	}

	public virtual void Setup(DeathObject deathObject, float time, Transform parent, ShakeM shakeObject)
	{
		this.hasSetup = true;
		base.transform.parent = parent;
		base.gameObject.SetActive(true);
		this.deathObject = deathObject;
		this.shakeObject = shakeObject;
		this.deathCountDown = time;
		this.x = base.transform.localPosition.x;
		this.y = base.transform.localPosition.y; this.groundHeight = (this.y );
		base.transform.localScale = new Vector3(-1f, 1f, 1f);
		if (deathObject.deathType == DeathType.Suicide)
		{
			this.deathCountDown += 0.5f;
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.specialSounds, 0.44f + UnityEngine.Random.value * 0.08f, Sound.GetInstance().transform.position, 0.9f + UnityEngine.Random.value * 0.2f);
		}
	}

	private void Update()
	{
		if (this.isDead)
		{
			if (base.RunFrameCounter(Time.deltaTime, 0.0334f))
			{
				this.ChangeFrame();
			}
			DeathType deathType = this.deathObject.deathType;
			if (deathType != DeathType.Bullet)
			{
			}
			this.yI -= 700f * Time.deltaTime;
			this.x += this.xI * Time.deltaTime;
			this.y += this.yI * Time.deltaTime;
			if (this.y <= this.groundHeight)
			{
				this.yI = 0f;
				this.y = this.groundHeight;
				this.ChangeFrame();
				this.xI = 0f;
				this.Disable();
			}
			this.SetPosition();
			if (this.deathObject.mookType == MookType.RiotShield)
			{
				this.disarmedGunTime -= Time.deltaTime;
				if (this.disarmedGunTime <= 0f)
				{
					this.gunSprite.GetComponent<Renderer>().enabled = false;
					this.Disable();
				}
				else
				{
					this.gunyI -= 800f * Time.deltaTime;
					this.gunX += this.gunXI * Time.deltaTime;
					this.gunY += this.gunyI * Time.deltaTime;
					this.gunSprite.transform.Rotate(0f, 0f, -this.gunXI * Time.deltaTime * 20f, Space.World);
					this.gunSprite.transform.localPosition = new Vector3(this.gunX, this.gunY, this.gunSprite.transform.localPosition.z);
					if (this.gunY <= this.groundHeight - 6f)
					{
						this.gunyI *= -0.4f;
						this.gunY = this.groundHeight - 6f;
						this.gunXI *= 1.5f;
						if (Mathf.Abs(this.gunyI) < 40f)
						{
							this.disarmedGunTime = -1f;
						}
					}
				}
			}
		}
		else
		{
			if (this.deathObject.deathType == DeathType.Suicide && base.RunFrameCounter(Time.deltaTime, 0.045f))
			{
				this.ChangeFrame();
			}
			this.deathCountDown -= Time.deltaTime;
			if (this.deathCountDown <= 0f)
			{
				this.Death();
			}
		}
	}

	private void LateUpdate()
	{
		VictoryMookDeath.playedSoundThisFrame = false;
		VictoryMookDeath.playedGibSoundThisFrame = false;
	}

	protected void SetPosition()
	{
		base.transform.localPosition = new Vector3(Mathf.Round(this.x), Mathf.Round(this.y), base.transform.localPosition.z);
	}

	protected void Death()
	{
		this.isDead = true;
		switch (this.deathObject.deathType)
		{
		case DeathType.Suicide:
			if (!VictoryMookDeath.playedGibSoundThisFrame)
			{
				Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.deathSounds, 0.5f + UnityEngine.Random.value * 0.08f, Sound.GetInstance().transform.position, 0.9f + UnityEngine.Random.value * 0.2f);
				VictoryMookDeath.playedSoundThisFrame = true;
				VictoryMookDeath.playedGibSoundThisFrame = true;
			}
			if (this.shakeObject != null)
			{
				this.shakeObject.m += 0.7f;
			}
			CutsceneEffectsController.CreateSmallExplosion(base.transform.position.x, base.transform.position.y - 8f, base.transform.parent.position.z - 10f, 1f, 0f);
			CutsceneEffectsController.CreateGibs(this.gibs, base.GetComponent<Renderer>().sharedMaterial, base.transform.position.x, base.transform.position.y - 16f, 180f, 40f, this.deathObject.xForce * 0.2f, 100f);
			CutsceneEffectsController.CreateBloodParticles(base.transform.position.x, base.transform.position.y, 8, 5f, 5f, 80f, this.deathObject.xForce * 0.2f, 90f);
			this.SetBloodSplat();
			this.Disable();
			this.DeactivateGun();
			return;
		case DeathType.Explode:
			if (this.shakeObject != null)
			{
				this.shakeObject.m += 0.8f;
			}
			CutsceneEffectsController.CreateSmallExplosion(base.transform.position.x, base.transform.position.y - 8f, base.transform.parent.position.z + 1f, 1f, VictoryMookDeath.playedGibSoundThisFrame ? 0f : 0.5f);
			VictoryMookDeath.playedGibSoundThisFrame = true;
			CutsceneEffectsController.CreateGibs(this.gibs, base.GetComponent<Renderer>().sharedMaterial, base.transform.position.x, base.transform.position.y - 16f, 180f, 40f, this.deathObject.xForce * 0.2f, 100f);
			CutsceneEffectsController.CreateBloodParticles(base.transform.position.x, base.transform.position.y, 8, 5f, 5f, 80f, this.deathObject.xForce * 0.2f, 90f);
			if (this.deathObject.mookType == MookType.BigGuy)
			{
				CutsceneEffectsController.CreateBloodParticles(base.transform.position.x, base.transform.position.y + 6f, 20, 5f, 5f, 80f, this.deathObject.xForce * 0.2f, 90f);
			}
			this.SetBloodSplat();
			base.transform.Translate(0f, 0f, 35f, Space.World);
			this.Disable();
			this.DeactivateGun();
			return;
		case DeathType.Gibbed:
			if (!VictoryMookDeath.playedGibSoundThisFrame)
			{
				Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.deathSounds, 0.44f + UnityEngine.Random.value * 0.08f, Sound.GetInstance().transform.position, 0.9f + UnityEngine.Random.value * 0.2f);
				VictoryMookDeath.playedSoundThisFrame = true;
				VictoryMookDeath.playedGibSoundThisFrame = true;
			}
			if (this.shakeObject != null)
			{
				this.shakeObject.m += 0.3f;
			}
			CutsceneEffectsController.CreateGibs(this.gibs, base.GetComponent<Renderer>().sharedMaterial, base.transform.position.x, base.transform.position.y - 16f, 180f, 40f, this.deathObject.xForce * 0.2f, 100f);
			CutsceneEffectsController.CreateBloodParticles(base.transform.position.x, base.transform.position.y + 6f, 8, 5f, 5f, 80f, this.deathObject.xForce * 0.2f, 90f);
			if (this.deathObject.mookType == MookType.BigGuy)
			{
				CutsceneEffectsController.CreateBloodParticles(base.transform.position.x, base.transform.position.y + 6f, 20, 5f, 5f, 80f, this.deathObject.xForce * 0.2f, 90f);
			}
			this.SetBloodSplat();
			base.transform.Translate(0f, 0f, 35f, Space.World);
			this.Disable();
			this.DeactivateGun();
			return;
		}
		if (!VictoryMookDeath.playedSoundThisFrame)
		{
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.hitSounds, 0.24f + UnityEngine.Random.value * 0.08f, Sound.GetInstance().transform.position, 0.9f + UnityEngine.Random.value * 0.2f);
			VictoryMookDeath.playedSoundThisFrame = true;
		}
		this.y += 5f;
		this.yI = 45f;
		if (this.deathObject.xForce >= 0f)
		{
			this.xI = 15f;
		}
		else
		{
			this.xI = -15f;
		}
		this.DeactivateGun();
		CutsceneEffectsController.CreateBloodParticles(base.transform.position.x, base.transform.position.y + 6f, 4, 5f, 5f, 40f, 0f, 40f);
		this.ChangeFrame();
	}

	protected virtual void SetBloodSplat()
	{
		if (this.deathObject.mookType != MookType.BigGuy)
		{
			base.SetSpriteFrame(11 + UnityEngine.Random.Range(0, 4), 0);
		}
		else
		{
			base.SetSpriteFrame(17 + UnityEngine.Random.Range(0, 4), 0);
		}
		base.transform.localScale = new Vector3((float)(UnityEngine.Random.Range(0, 2) * 2 - 1), 1f, 1f);
	}

	protected void Disable()
	{
		if (this.deathObject.mookType != MookType.RiotShield || this.disarmedGunTime <= 0f)
		{
			base.enabled = false;
		}
	}

	protected void DeactivateGun()
	{
		if (this.deathObject.mookType != MookType.RiotShield)
		{
			if (this.gunSprite != null)
			{
				this.gunSprite.gameObject.SetActive(false);
			}
		}
		else
		{
			this.gunSprite.transform.parent = base.transform.parent;
			this.gunX = this.gunSprite.transform.localPosition.x;
			this.gunY = this.gunSprite.transform.localPosition.y;
			this.gunXI = UnityEngine.Random.value * 60f - 30f + this.deathObject.xForce * 0.15f;
			this.gunyI = 250f;
			this.disarmedGunTime = 3.5f;
			this.gunSprite.SetOffset(-2.5f, 5f, -5f);
			this.gunSprite.Frame = 4;
		}
	}

	protected void ChangeFrame()
	{
		if (!this.isDead)
		{
			if (this.deathObject.deathType == DeathType.Suicide)
			{
				base.Frame = 21 + (base.Frame + 1) % 4;
			}
			else
			{
				base.SetSpriteFrame(0, 0);
			}
		}
		else
		{
			switch (this.deathObject.deathType)
			{
			case DeathType.Suicide:
			case DeathType.Explode:
			case DeathType.Gibbed:
				return;
			}
			if (this.xI > 0f)
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			if (this.y > this.groundHeight)
			{
				base.SetSpriteFrame(4, 0);
			}
			else
			{
				base.SetSpriteFrame(5, 0);
			}
		}
	}

	public SoundHolder soundHolder;

	public GibHolder gibs;

	protected DeathObject deathObject;

	protected ShakeM shakeObject;

	protected bool hasSetup;

	protected float deathCountDown = 0.1f;

	protected float x;

	protected float y;

	protected float xI;

	protected float yI;

	protected float groundHeight = 100f;

	protected static bool playedSoundThisFrame;

	protected static bool playedGibSoundThisFrame;

	protected bool isDead;

	private static int victoryDeathCount;

	public SimpleSpriteWrapper gunSprite;

	protected float gunX;

	protected float gunY;

	protected float gunXI;

	protected float gunyI;

	protected float disarmedGunTime = 5f;
}
