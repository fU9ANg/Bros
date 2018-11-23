// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TankMassiveGun : TankWeapon
{
	protected override void Start()
	{
		base.Start();
		this.miniGunSource = base.gameObject.AddComponent<AudioSource>();
		this.miniGunSource.volume = 0.45f;
		this.miniGunSource.pitch = 0.6f;
		this.miniGunSource.loop = false;
		this.miniGunSource.rolloffMode = AudioRolloffMode.Linear;
		this.miniGunSource.minDistance = 150f;
		this.miniGunSource.maxDistance = 420f;
		this.miniGunSource.loop = false;
		this.miniGunSource.dopplerLevel = 0.1f;
		this.miniGunSource.clip = this.miniGunWindUpSound;
		this.miniGunSource.playOnAwake = false;
		this.miniGunSource.Stop();
		this.mookCaptain.gameObject.SetActive(false);
		this.height = 18f;
	}

	public override void SetTargetPlayerNum(int pN, Vector3 TargetPosition)
	{
		this.targetPosition = TargetPosition;
		this.targetPlayerNum = pN;
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		if (this.health > 0)
		{
			this.damageBrotalityCount += damage;
			StatisticsController.AddBrotality(this.damageBrotalityCount / 5);
			this.damageBrotalityCount -= this.damageBrotalityCount / 5;
		}
		base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
		if (this.health > 0 && this.tank.health > this.health)
		{
			this.tank.health = this.health;
		}
	}

	public override void SetSpriteTurn(int frame)
	{
		base.SetSpriteTurn(frame);
		if (frame != 0)
		{
			this.hatch.gameObject.SetActive(false);
		}
		else
		{
			this.hatch.gameObject.SetActive(true);
		}
		if (this.health > 0)
		{
			this.minigunFrame = 0;
			this.minigunAngle = -1.57079637f;
			this.SetMinigunAngle(0f);
			this.minigunForeground.SetLowerLeftPixel(new Vector2((float)((0 + frame) * this.spritePixelWidth), this.minigunForeground.lowerLeftPixel.y));
			this.minigunBackground.SetLowerLeftPixel(new Vector2((float)((0 + frame) * this.spritePixelWidth), this.minigunBackground.lowerLeftPixel.y));
		}
	}

	public void EndCutscene()
	{
		this.CloseHatch();
		LetterBoxController.ClearLetterBox(0.5f);
		this.hasFinishedCutscene = true;
	}

	public void CloseHatch()
	{
		this.hatch.Close();
	}

	protected override void Update()
	{
		base.Update();
		if (this.health > 0)
		{
			if (!this.hasShownCaptain && this.tank.enemyAI.IsAlerted())
			{
				if (this.captainDelay > 0f)
				{
					this.captainDelay -= this.t;
				}
				else
				{
					this.hatch.Invoke("Open", 2f);
					this.hasShownCaptain = true;
					this.mookCaptain.gameObject.SetActive(true);
					this.camophlageHolder.Reveal(0.6f);
					LetterBoxController.ShowLetterBox(1f, 1.5f);
				}
			}
			if (!this.hasFinishedCutscene && !this.mookCaptain.gameObject.activeInHierarchy)
			{
				LetterBoxController.ClearLetterBox(0.5f);
				this.hasFinishedCutscene = true;
			}
		}
		if (this.health <= 0)
		{
			if (this.tank.transform.localScale.x > 0f)
			{
				this.targetAngle = -2.13628316f;
			}
			else
			{
				this.targetAngle = 2.13628316f;
			}
			this.minigunAngle = Mathf.Lerp(this.minigunAngle, this.targetAngle, this.t * 4.5f);
			if (this.tank.transform.localScale.x > 0f)
			{
				this.SetMinigunAngle(-this.minigunAngle * 180f / 3.14159274f - 90f);
			}
			else
			{
				this.SetMinigunAngle(this.minigunAngle * 180f / 3.14159274f - 90f);
			}
		}
	}

	public override bool IsFiring()
	{
		return base.IsFiring() || this.fireDelay > 0f;
	}

	public override void UseSpecial()
	{
		if (!this.wasSpecial)
		{
			this.specialCounter = 0f;
			this.hatch.Open();
			this.throwingGrenadesDelay = 0.6f;
		}
		this.special = true;
	}

	protected void SetMinigunAngle(float a)
	{
		this.minigunForeground.transform.eulerAngles = new Vector3(0f, 0f, a);
		this.minigunBackground.transform.eulerAngles = new Vector3(0f, 0f, a);
	}

	protected override void RunFiring()
	{
		this.fireDelay -= this.t;
		if (this.fire && this.health > 0 && this.tank.CanFire() && this.fireDelay <= 0f)
		{
			if (this.patternCount % 2 == 0)
			{
				this.fireCounter += this.t;
				int num = (int)(this.fireCounter / this.minigunFrameRate);
				for (int i = 0; i < num; i++)
				{
					this.fireCounter -= this.minigunFrameRate;
					this.ChangeMinigunFrame();
				}
			}
			else
			{
				this.fireCounter += this.t;
				if (this.fireCounter > 0f)
				{
					this.shellFrameCounter += this.t;
					if (this.shellFrameCounter > 0.04f)
					{
						this.shellFrameCounter -= 0.04f;
						this.ChangeShellingFrame();
					}
				}
			}
		}
		this.wasFire = this.fire;
		if (this.health > 0 && this.special)
		{
			this.specialCounter += this.t;
			this.throwingGrenadesDelay -= this.t;
			if (this.throwingGrenadesDelay <= 0f)
			{
				if (this.specialCounter < 1.7f)
				{
					this.specialFireCounter += this.t;
					if (this.specialFireCounter > 0f)
					{
						this.specialFireCounter -= 0.3f;
						this.ThrowGrenade();
					}
				}
				else if (this.specialCounter > 2.3f)
				{
					this.special = false;
					this.hatch.Close();
				}
			}
		}
		this.wasSpecial = this.special;
	}

	protected void ThrowGrenade()
	{
		this.thrownGrenadeCount++;
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.special2Sounds, 0.66f, base.transform.position);
		if (base.IsMine)
		{
			Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
			if (insideUnitCircle.y < 0f)
			{
				insideUnitCircle.y *= -1f;
			}
			ProjectileController.SpawnGrenadeOverNetwork(this.bigGrenade, this, this.hatch.transform.position.x, this.hatch.transform.position.y + 8f, 0.001f, 0f, insideUnitCircle.x * 100f, 170f + insideUnitCircle.y, -15);
		}
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		base.Death(xI, yI, damage);
		this.minigunForeground.SetLowerLeftPixel((float)(this.spritePixelWidth * 5), (float)((int)this.minigunForeground.lowerLeftPixel.y));
		this.minigunBackground.SetLowerLeftPixel((float)(this.spritePixelWidth * 5), (float)((int)this.minigunBackground.lowerLeftPixel.y));
		this.hatch.gameObject.SetActive(false);
		if (this.tank.transform.localScale.x > 0f)
		{
			this.targetAngle = -2.13628316f;
		}
		else
		{
			this.targetAngle = 2.13628316f;
		}
		if (this.miniGunSource.isPlaying)
		{
			this.miniGunSource.Stop();
		}
		if (this.tank.health > 0)
		{
			this.tank.Invoke("InvokeDeath", 1f);
		}
	}

	protected void ChangeShellingFrame()
	{
		this.shellFrame++;
		if (this.shellFrame == 5)
		{
			this.FireWeapon(ref this.fireIndex);
		}
		if (this.shellFrame >= 8)
		{
			this.sprite.SetLowerLeftPixel(new Vector2(0f, this.sprite.lowerLeftPixel.y));
			this.shellFrame = 0;
			this.fireCounter -= this.fireRate;
			if (this.fireIndex >= 3)
			{
				this.fire = false;
				this.fireDelay = 0.4f;
				this.fireIndex = 0;
				this.patternCount++;
			}
		}
		else
		{
			this.sprite.SetLowerLeftPixel(new Vector2((float)((5 + this.shellFrame) * this.spritePixelWidth), this.sprite.lowerLeftPixel.y));
		}
	}

	protected void ChangeMinigunFrame()
	{
		if (this.minigunFrame == 0)
		{
			if (this.tank.transform.localScale.x > 0f)
			{
				this.minigunAngle = -1.57079637f;
			}
			else
			{
				this.minigunAngle = 1.57079637f;
			}
			this.miniGunSource.clip = this.miniGunWindUpSound;
			this.miniGunSource.Play();
		}
		this.minigunFrame++;
		if (this.targetPlayerNum >= 0)
		{
			float x = this.targetPosition.x;
			float y = this.targetPosition.y;
			if (x > 0f)
			{
				if (this.windUpCount < 1f)
				{
					float y2 = x - this.minigunForeground.transform.position.x;
					float x2 = y - this.minigunForeground.transform.position.y;
					this.targetAngle = global::Math.GetAngle(x2, y2);
				}
				bool flag = true;
				if (this.tank.transform.localScale.x > 0f)
				{
					if (this.targetAngle < -2.51327419f || this.targetAngle > -0.7853982f)
					{
						flag = false;
					}
					this.targetAngle = Mathf.Clamp(this.targetAngle, -2.13628316f, -0.7853982f);
				}
				else
				{
					if (this.targetAngle > 2.51327419f || this.targetAngle < 0.7853982f)
					{
						flag = false;
					}
					this.targetAngle = Mathf.Clamp(this.targetAngle, 0.7853982f, 2.13628316f);
				}
				if (flag)
				{
					if (this.targetAngle > this.minigunAngle + 3.14159274f)
					{
						this.minigunAngle += 6.28318548f;
					}
					else if (this.targetAngle < this.minigunAngle - 3.14159274f)
					{
						this.minigunAngle -= 6.28318548f;
					}
					this.minigunAngle = Mathf.Lerp(this.minigunAngle, this.targetAngle, 0.2f);
					this.miniGunDirection = global::Math.Point2OnCircle(this.minigunAngle, 620f);
					if (this.tank.transform.localScale.x > 0f)
					{
						this.SetMinigunAngle(-this.minigunAngle * 180f / 3.14159274f - 90f);
					}
					else
					{
						this.SetMinigunAngle(this.minigunAngle * 180f / 3.14159274f - 90f);
					}
				}
			}
		}
		if (this.minigunFrame % 4 == 0)
		{
			if (this.fireIndex < this.fireLimit)
			{
				this.windUpCount += 1f;
				this.minigunFrameRate = Mathf.Clamp(0.02f + (6f - this.windUpCount) / 6f * 0.08f, 0.02f, 0.1f);
				if (this.windUpCount > 7f)
				{
					if (this.miniGunSource.isPlaying)
					{
						this.miniGunSource.Stop();
					}
					this.FireWeapon(ref this.fireIndex);
					if (this.fireIndex >= this.fireLimit)
					{
						this.miniGunSource.clip = this.miniGunWindDownSound;
						this.miniGunSource.Play();
					}
				}
			}
			else
			{
				this.windDownCount += 1f;
				this.minigunFrameRate = Mathf.Clamp(0.02f + this.windDownCount / 3f * 0.05f, 0.02f, 0.07f);
			}
			if (this.windDownCount > 4f)
			{
				this.fire = false;
				this.fireDelay = 0.1f;
				this.fireIndex = 0;
				this.patternCount++;
				this.windUpCount = 0f;
				this.windDownCount = 0f;
				this.minigunFrameRate = 0.1f;
				this.minigunFrame = 0;
				UnityEngine.Debug.Log("Stop !");
			}
		}
		this.SetMiniGunFireFrame(this.minigunFrame, this.windUpCount > 7f && this.fireIndex < this.fireLimit);
	}

	protected void SetMiniGunFireFrame(int frame, bool actuallyFiring)
	{
		if (!actuallyFiring)
		{
			this.minigunForeground.SetLowerLeftPixel(new Vector2((float)((6 + frame % 4) * this.spritePixelWidth), this.minigunForeground.lowerLeftPixel.y));
			this.minigunBackground.SetLowerLeftPixel(new Vector2((float)((6 + frame % 4) * this.spritePixelWidth), this.minigunBackground.lowerLeftPixel.y));
		}
		else
		{
			this.minigunForeground.SetLowerLeftPixel(new Vector2((float)((10 + frame % 4) * this.spritePixelWidth), this.minigunForeground.lowerLeftPixel.y));
			this.minigunBackground.SetLowerLeftPixel(new Vector2((float)((10 + frame % 4) * this.spritePixelWidth), this.minigunBackground.lowerLeftPixel.y));
		}
	}

	protected override void FireWeapon(ref int index)
	{
		if (this.patternCount % 2 == 0)
		{
			if (index < this.fireLimit)
			{
				index++;
				Vector3 vector = this.minigunForeground.transform.TransformPoint(new Vector3(-38f, 4f, 0f));
				Vector3 vector2 = this.minigunForeground.transform.TransformDirection(Vector3.left * 600f);
				if (this.tank.transform.localScale.x < 0f)
				{
					vector2.x *= -1f;
				}
				Vector3 vector3 = UnityEngine.Random.insideUnitCircle * 20f;
				ProjectileController.SpawnProjectileLocally(this.bigBullet, this, vector.x, vector.y, vector2.x + vector3.x, vector2.y + vector3.y, -1);
				vector3 = UnityEngine.Random.insideUnitCircle * 20f;
				Vector3 vector4 = this.minigunBackground.transform.TransformPoint(new Vector3(-38f, 4f, 0f));
				ProjectileController.SpawnProjectileLocally(this.bigBulletBackground, this, vector4.x, vector4.y, vector2.x + vector3.x, vector2.y + vector3.y, -1);
				Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.attackSounds, 0.66f, (base.transform.position + SortOfFollow.GetInstance().transform.position) / 2f);
			}
		}
		else
		{
			float num = 1f + UnityEngine.Random.value * 0.5f;
			float x = this.targetPosition.x;
			float y = this.targetPosition.y;
			if (!HeroController.PlayerIsAlive(this.targetPlayerNum))
			{
				this.targetPlayerNum = -1;
			}
			if (x > 0f)
			{
				float f = x - this.tank.x;
				float num2 = y - this.tank.y;
				num = 0.2f + Mathf.Abs(f) / 200f + num2 / 300f;
				if (num < 0.5f)
				{
					num = 0.5f;
				}
			}
			if (this.IsLocalMook)
			{
				ProjectileController.SpawnProjectileOverNetwork(this.shellPrefab, this, this.x + (float)(this.tank.facingDirection * 19), this.y + 36f, (float)(this.tank.facingDirection * 150) * num * (0.9f + UnityEngine.Random.value * 0.2f), 150f * num, false, -1, false, true);
			}
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.specialSounds, 0.77f, (base.transform.position + SortOfFollow.GetInstance().transform.position) / 2f);
			index++;
		}
	}

	protected bool special;

	protected bool wasSpecial;

	protected float specialCounter;

	protected float specialFireCounter;

	public Projectile bigBullet;

	public Projectile bigBulletBackground;

	public Grenade bigGrenade;

	protected int fireLimit = 15;

	protected int patternCount = 1;

	public SpriteSM minigunForeground;

	public SpriteSM minigunBackground;

	protected int minigunFrame;

	protected float minigunFrameRate = 0.1f;

	protected float minigunFrameCounter;

	protected float minigunAngle;

	protected float shellFrameCounter;

	protected int shellFrame;

	protected float targetAngle;

	protected Vector2 miniGunDirection;

	protected float windUpCount;

	protected float windDownCount;

	public Projectile shellPrefab;

	protected int targetPlayerNum = -1;

	protected Vector3 targetPosition;

	protected float throwingGrenadesDelay = 2f;

	protected int thrownGrenadeCount;

	public TankHatch hatch;

	protected AudioSource miniGunSource;

	public AudioClip miniGunWindUpSound;

	public AudioClip miniGunWindDownSound;

	public MookCaptainCutscene mookCaptain;

	protected bool hasShownCaptain;

	protected float captainDelay = 0.5f;

	public CamophlageHolder camophlageHolder;

	protected bool hasFinishedCutscene;

	protected int damageBrotalityCount;
}
