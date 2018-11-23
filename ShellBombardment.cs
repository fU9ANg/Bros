// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ShellBombardment : RocketBombardment
{
	protected override void Start()
	{
		base.Start();
		base.GetComponent<AudioSource>().pitch = 2f;
	}

	protected override void Update()
	{
		base.Update();
		base.GetComponent<AudioSource>().pitch -= Mathf.Clamp(Time.deltaTime, 0f, 0.0334f) * 0.5f;
	}

	protected override void MakeEffects(bool particles, float x, float y, bool useRayCast, Vector3 hitNormal, Vector3 point)
	{
		if (this.hasMadeEffects)
		{
			return;
		}
		this.cross.GoAway();
		EffectsController.CreatePlumes(x, y, 3, 8f, 270f, 0f, 0f);
		MapController.DamageGround(this.firedBy, this.damage, this.damageType, this.range, x, y, null);
		Networking.RPC<float, float, float, int>(PID.TargetAll, new RpcSignature<float, float, float, int>(MapController.BurnGround_Local), this.range * 0.7f, x, y, this.groundLayer, false);
		Map.ExplodeUnits(this.firedBy, this.damage, this.damageType, this.range * 1.5f, this.range * 0.8f, x, y, this.blastForce, 400f, -1, false, false);
		Map.ShakeTrees(x, y, 256f, 64f, 128f);
		EffectsController.CreateHugeExplosion(x, y, this.range * 0.25f, this.range * 0.25f, 120f, 1f, this.range * 1.5f, 1f, 0.7f, 4, 200, 400f, 160f, 0.3f, 1f);
		Map.DamageDoodads(this.damage, x, y, 0f, 0f, this.range, this.playerNum);
	}
}
