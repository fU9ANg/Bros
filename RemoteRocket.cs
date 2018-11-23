// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class RemoteRocket : Rocket
{
	protected override void Start()
	{
		base.Start();
		AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
		audioSource.clip = this.rocketSound;
		audioSource.volume = 0.3f;
		audioSource.loop = true;
		audioSource.rolloffMode = AudioRolloffMode.Linear;
		audioSource.maxDistance = 1000f;
		audioSource.dopplerLevel = 0.1f;
		audioSource.Play();
		base.SetSyncingInternal(true);
	}

	public override void Fire(float x, float y, float xI, float yI, int playerNum, MonoBehaviour FiredBy)
	{
		base.Fire(x, y, xI, yI, playerNum, FiredBy);
		this.hasMadeEffects = false;
	}

	protected override void CheckSpawnPoint()
	{
	}

	protected override void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		this.RunProjectile(this.t);
		this.life -= this.t;
		if (this.life <= 0f)
		{
			this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.RunSmokeTrail(this.t);
		Map.PanicUnits(this.x, this.y, 35f, true);
	}

	protected override void MakeEffects(bool particles, float x, float y, bool useRayCast, Vector3 hitNormal, Vector3 point)
	{
		if (this.hasMadeEffects)
		{
			return;
		}
		if (base.IsMine)
		{
			Networking.RPC<bool, float, float, bool, Vector3, Vector3>(PID.TargetOthers, new RpcSignature<bool, float, float, bool, Vector3, Vector3>(this.MakeEffects), particles, x, y, useRayCast, this.raycastHit.normal, this.raycastHit.point, false);
		}
		base.MakeEffects(particles, x, y, useRayCast, this.raycastHit.normal, this.raycastHit.point);
		EffectsController.CreatePlumes(x, y, 3, 8f, 270f, 0f, 0f);
	}

	protected override void RunProjectile(float t)
	{
		base.RunProjectile(t);
	}

	protected override void HitUnits()
	{
		if (Map.HitLivingUnits(this.firedBy, this.playerNum, this.damageInternal, this.damageType, this.projectileSize, this.x, this.y, this.xI, this.yI, false, false))
		{
			this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
			this.DeregisterProjectile();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public AudioClip rocketSound;
}
