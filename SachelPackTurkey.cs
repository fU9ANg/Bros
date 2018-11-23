// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SachelPackTurkey : SachelPack
{
	protected override void Update()
	{
		if (this.attractMooks)
		{
			this.attractCounter += this.t;
			if (this.attractCounter >= 0.0334f)
			{
				this.attractCounter -= 0.0334f;
				Map.AttractMooks(this.x, this.y, 200f, 100f);
			}
		}
		base.Update();
	}

	protected override void MakeEffects(bool particles, float x, float y, bool useRayCast, Vector3 hitNormal, Vector3 point)
	{
		if (this.hasMadeEffects)
		{
			return;
		}
		EffectsController.CreatePlumes(x, y, 5, 10f, 360f, 0f, 0f);
		if (base.IsMine)
		{
			Networking.RPC<bool, float, float, bool, Vector3, Vector3>(PID.TargetOthers, new RpcSignature<bool, float, float, bool, Vector3, Vector3>(this.MakeEffects), particles, x, y, useRayCast, hitNormal, point, false);
			MapController.DamageGround(this.firedBy, this.damage, this.damageType, this.range, x, y, null);
			Map.ExplodeUnits(this.firedBy, this.damage, DamageType.Explosion, this.range, this.range * 0.6f, x, y - 32f, this.blastForce * 50f, (float)this.upwardBlastForce, this.playerNum, true, true);
			MapController.BurnUnitsAround_NotNetworked(this.firedBy, this.playerNum, 5, this.range * 1f, x, y, true, true);
			Map.HitProjectiles(this.playerNum, this.damage, this.damageType, this.range, x, y, 0f, 0f, 0.25f);
			Map.ShakeTrees(x, y, 320f, 64f, 160f);
		}
		if (this.flameWaveExplosion != null)
		{
			MonoBehaviour.print("should network this");
			UnityEngine.Object.Instantiate(this.flameWaveExplosion, base.transform.position + Vector3.up, Quaternion.identity);
		}
		EffectsController.CreateHugeExplosion(x, y, 48f, 48f, 80f, 1f, 32f, 0.7f, 0.9f, 8, 20, 110f, 160f, 0.3f, 0.9f);
	}

	protected override void SetRotation()
	{
		if (Mathf.Abs(this.xI) < 10f && Mathf.Abs(this.yI) < 50f)
		{
			base.transform.eulerAngles = new Vector3(0f, 0f, 0f);
		}
		else if (this.xI < 0f)
		{
			base.transform.eulerAngles = new Vector3(0f, 0f, global::Math.GetAngle(this.yI, -this.xI) * 180f / 3.14159274f - 90f);
		}
		else
		{
			base.transform.eulerAngles = new Vector3(0f, 0f, 180f + global::Math.GetAngle(this.yI, -this.xI) * 180f / 3.14159274f - 90f);
		}
	}

	protected float attractCounter;

	public FlashBangExplosion flameWaveExplosion;

	public bool attractMooks;

	public bool fullExplosion;

	public int upwardBlastForce = 300;
}
