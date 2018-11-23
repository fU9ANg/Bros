// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class JungleSeagull : WildLife
{
	protected override void Awake()
	{
		base.Awake();
		this.sprite = base.GetComponent<SpriteSM>();
		this.targetX = base.transform.position.x;
		this.targetY = base.transform.position.y;
	}

	protected override void Start()
	{
		base.Start();
		if (UnityEngine.Random.value > 0.5f)
		{
			this.sprite.SetSize(-this.sprite.width, this.sprite.height);
		}
	}

	public override bool Disturb()
	{
		base.Disturb();
		this.alerted = true;
		this.xI = 10f;
		this.yI = 10f;
		this.xI += UnityEngine.Random.value * 32f - 16f;
		this.yI += UnityEngine.Random.value * 32f - 16f;
		this.xTargetM = 0.75f + UnityEngine.Random.value * 0.5f;
		this.targetX += 8f * this.xTargetM;
		this.targetY += 8f;
		this.sprite.SetSize(this.sprite.width, this.sprite.height);
		JungleSeagull.soundFlapDelay = UnityEngine.Random.value * UnityEngine.Random.value * 0.3f;
		this.soundDelay = UnityEngine.Random.value * UnityEngine.Random.value * 3.6f;
		this.returnForce = 0.56f + UnityEngine.Random.value * 0.6f;
		this.sprite.SetOffset(new Vector3(0f, 0f, 30f));
		Map.RegisterDisturbedWildLife(this);
		return true;
	}

	public override bool Hurt()
	{
		Sound instance = Sound.GetInstance();
		instance.PlaySoundEffectAt(this.soundHolder.deathSounds, 0.23f + UnityEngine.Random.value * UnityEngine.Random.value * 0.1f, base.transform.position);
		EffectsController.CreateShrapnel(this.bloodBit, this.x, this.y, 3f, 100f, 20f, 0f, 20f);
		EffectsController.CreateShrapnel(this.featherBit, this.x, this.y, 2f, 50f, 10f, 0f, 30f);
		return base.Hurt();
	}

	protected void SetPosition()
	{
		base.transform.position = new Vector3(Mathf.Round(this.x), Mathf.Round(this.y), 0f);
	}

	private void Update()
	{
		if (this.alerted)
		{
			this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
			if (this.soundDelay > 0f)
			{
				this.soundDelay -= this.t;
				if (this.soundDelay <= 0f)
				{
					if (this.soundHolder != null)
					{
						Sound instance = Sound.GetInstance();
						instance.PlaySoundEffectAt(this.soundHolder.defendSounds, 0.05f + UnityEngine.Random.value * UnityEngine.Random.value * 0.3f, base.transform.position);
					}
					else
					{
						UnityEngine.Debug.LogError("Sound Null");
					}
				}
			}
			if (JungleSeagull.soundFlapDelay > 0f)
			{
				JungleSeagull.soundFlapDelay -= this.t;
				if (JungleSeagull.soundFlapDelay <= 0f)
				{
					if (this.soundHolder != null)
					{
						Sound instance2 = Sound.GetInstance();
						instance2.PlaySoundEffectAt(this.soundHolder.effortSounds, 0.25f + UnityEngine.Random.value * UnityEngine.Random.value * 0.1f, base.transform.position);
					}
					else
					{
						UnityEngine.Debug.LogError("Sound Null");
					}
				}
			}
			this.frameCounter += this.t;
			if (this.frameCounter >= 0.0334f)
			{
				this.frameCounter -= 0.0334f;
				this.frame++;
				this.sprite.SetLowerLeftPixel((float)(16 + this.frame % 8 * 16), 16f);
				if (this.frame % 8 == 4)
				{
					this.targetX += (15f + UnityEngine.Random.value * 4f) * this.xTargetM;
					this.targetY += 10f + UnityEngine.Random.value * 7f;
					if (UnityEngine.Random.value < 0.3f)
					{
						this.xI *= 0.93f;
						this.yI *= 0.93f;
						this.returnForce = 3f + UnityEngine.Random.value * 1.2f;
						this.xI += UnityEngine.Random.value * 32f - 16f;
						this.yI += UnityEngine.Random.value * 32f - 16f;
					}
					this.xI += this.xDiff * this.returnForce;
					this.yI += this.yDiff * this.returnForce;
					if (this.targetY > 2000f)
					{
						Map.RemoveDisturbedWildLife(this);
						UnityEngine.Object.Destroy(base.gameObject);
					}
					this.yI += 120f;
				}
			}
			this.yI -= 280f * this.t;
			this.xDiff = this.targetX - this.x;
			this.yDiff = this.targetY - this.y;
			this.yI *= 1f - this.t * 2.2f;
			this.xI *= 1f - this.t * 2.2f;
			this.x += this.xI * this.t;
			this.y += this.yI * this.t;
			this.SetPosition();
		}
	}

	protected bool alerted;

	protected float xI;

	protected float yI;

	protected float xDiff;

	protected float yDiff;

	protected float t;

	protected float frameCounter;

	protected int frame;

	protected float targetX;

	protected float targetY;

	protected float soundDelay;

	protected static float soundFlapDelay;

	protected SpriteSM sprite;

	public Shrapnel bloodBit;

	public Shrapnel featherBit;

	protected float xTargetM = 1f;

	public SoundHolder soundHolder;

	protected float returnForce = 0.3f;

	protected float counter;
}
