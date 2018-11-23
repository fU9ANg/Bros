// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ProjectileReturnZone : BroforceObject
{
	private void Start()
	{
		ProjectileController.RegisterReturnZone(this);
		this.puffCounter = this.puffRate;
		base.gameObject.AddComponent<AudioSource>();
		base.GetComponent<AudioSource>().dopplerLevel = 0.1f;
		base.GetComponent<AudioSource>().volume = 0.6f;
		base.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
		base.GetComponent<AudioSource>().maxDistance = 300f;
		base.GetComponent<AudioSource>().minDistance = 200f;
		base.GetComponent<AudioSource>().clip = this.chargeClip;
		base.GetComponent<AudioSource>().loop = false;
		base.GetComponent<AudioSource>().Play();
	}

	protected override void OnDestroy()
	{
		if (this.deathGrace >= 0f)
		{
			ProjectileController.RemoveReturnZone(this);
		}
	}

	protected override void LateUpdate()
	{
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
		if (this.life > 0f)
		{
			this.life -= Time.deltaTime;
			if (!HeroController.PlayerIsAlive(this.playerNum))
			{
				this.life = 0f;
				if (this.deathGrace > 1f)
				{
					this.deathGrace = 1f;
				}
			}
		}
		else if (this.deathGrace > 0f)
		{
			this.deathGrace -= Time.deltaTime;
			if (this.deathGrace <= 1f)
			{
				if (!this.playedDeathCry)
				{
					Sound.GetInstance().PlaySoundEffectAt(this.releaseClip, 0.6f, base.transform.position);
					this.playedDeathCry = true;
				}
				base.GetComponent<AudioSource>().volume -= Time.deltaTime;
			}
			else if (this.deathGrace > 1f && !HeroController.PlayerIsAlive(this.playerNum))
			{
				this.deathGrace = 1f;
			}
		}
		else
		{
			ProjectileController.RemoveReturnZone(this);
			EffectsController.CreateDistortionWobbleRingEffect(this.x, this.y, 0f);
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.puffCounter += Time.deltaTime;
		if (this.puffCounter >= this.puffRate && (this.life > 0f || this.deathGrace > 0.3f))
		{
			this.puffCounter -= this.puffRate;
			EffectsController.CreateEffect((this.life <= 0f) ? this.puffRedPrefab : this.puffGreenPrefab, this.x, this.y, 0f, 1, 1, BloodColor.None);
			EffectsController.CreateDistortionWobblePinchEffect(this.x - 24f + UnityEngine.Random.value * 48f, this.y - 24f + UnityEngine.Random.value * 48f, 0f);
		}
	}

	public float radius = 32f;

	public float life = 3f;

	public int playerNum = -1;

	protected float puffCounter;

	public float puffRate = 0.333f;

	public Puff puffGreenPrefab;

	public Puff puffRedPrefab;

	protected float deathGrace = 2.1f;

	protected bool playedDeathCry;

	public AudioClip chargeClip;

	public AudioClip releaseClip;
}
