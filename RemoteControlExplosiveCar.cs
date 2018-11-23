// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class RemoteControlExplosiveCar : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = false;
		base.Awake();
		HeroController.TryFollow(base.transform);
		this.engineAudio = base.gameObject.AddComponent<AudioSource>();
		this.engineAudio.rolloffMode = AudioRolloffMode.Linear;
		this.engineAudio.minDistance = 200f;
		this.engineAudio.dopplerLevel = 0.1f;
		this.engineAudio.maxDistance = 500f;
		this.engineAudio.volume = 0.12f;
		this.engineAudio.loop = true;
		this.engineAudio.clip = this.engineClip;
		this.engineAudio.Play();
	}

	protected override void Start()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
	}

	protected override void CheckInput()
	{
	}

	protected override void Update()
	{
		base.Update();
		if ((this.noiseDelay -= this.t) < 0f)
		{
			Map.PanicUnits(this.x, this.y, 48f, true);
			this.noiseDelay = 0.05f;
		}
		if ((this.maxLife -= this.t) < 0f)
		{
			this.Explode();
		}
		float pitch = Mathf.Lerp(this.engineAudio.pitch, 0.9f + (Mathf.Abs(this.xI) + Mathf.Clamp(this.yI, 0f, this.speed)) / this.speed * 0.6f, this.t * 10f);
		this.engineAudio.pitch = pitch;
	}

	public void Explode()
	{
		if (!this.exploded)
		{
			this.MakeEffects();
			UnityEngine.Object.Destroy(base.gameObject);
			this.exploded = true;
		}
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		this.Explode();
	}

	protected override void Gib(DamageType damageType, float xI, float yI)
	{
		this.Explode();
	}

	private void MakeEffects()
	{
		if (this.hasMadeEffects)
		{
			return;
		}
		EffectsController.CreatePlumes(this.x, this.y, 5, 10f, 360f, 0f, 0f);
		if (base.IsMine)
		{
			Networking.RPC(PID.TargetOthers, new RpcSignature(this.MakeEffects), false);
			MapController.DamageGround(this, this.damage, DamageType.Explosion, this.range, this.x, this.y, null);
			Map.ExplodeUnits(this, this.damage, DamageType.Explosion, this.range, this.range * 0.8f, this.x, this.y - 32f, 200f, 200f, this.playerNum, true, true);
			MapController.BurnUnitsAround_NotNetworked(this, this.playerNum, 5, this.range * 1f, this.x, this.y, true, true);
			Map.HitProjectiles(this.playerNum, this.damage, DamageType.Explosion, this.range, this.x, this.y, 0f, 0f, 0.25f);
			Map.ShakeTrees(this.x, this.y, 320f, 64f, 160f);
		}
		SortOfFollow.Shake(1f);
		EffectsController.CreateHugeExplosion(this.x, this.y, 48f, 48f, 80f, 1f, 32f, 0.7f, 0.9f, 8, 20, 110f, 160f, 0.3f, 0.9f);
	}

	private bool hasMadeEffects;

	public int damage;

	public float range;

	private float noiseDelay;

	private AudioSource engineAudio;

	public AudioClip engineClip;

	private float maxLife = 10f;

	private bool exploded;
}
