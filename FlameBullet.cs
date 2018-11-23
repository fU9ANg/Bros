// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FlameBullet : Projectile
{
	public override void Fire(float x, float y, float xI, float yI, int playerNum, MonoBehaviour FiredBy)
	{
		base.Fire(x, y, xI, yI, playerNum, FiredBy);
		if (xI < 0f)
		{
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	protected override void SetRotation()
	{
	}

	protected override bool ReflectProjectile(RaycastHit raycastHit)
	{
		return false;
	}

	protected override void MakeEffects(bool particles, float x, float y, bool useRayCast, Vector3 hitNormal, Vector3 point)
	{
		if (this.hasMadeEffects)
		{
			return;
		}
		if (this.life > 0f)
		{
			if (useRayCast)
			{
				if (particles)
				{
					EffectsController.CreateShrapnel(this.shrapnel, this.raycastHit.point.x + this.raycastHit.normal.x * 3f, this.raycastHit.point.y + this.raycastHit.normal.y * 3f, 4f, 30f, 2f, this.raycastHit.normal.x * 60f, this.raycastHit.normal.y * 30f);
					EffectsController.CreateShrapnel(this.shrapnelSpark, this.raycastHit.point.x + this.raycastHit.normal.x * 3f, this.raycastHit.point.y + this.raycastHit.normal.y * 3f, 4f, 30f, 3f, this.raycastHit.normal.x * 60f, this.raycastHit.normal.y * 30f);
				}
				if (this.life > 0.1f || useRayCast)
				{
					EffectsController.CreateEffect(this.flickPuff, this.raycastHit.point.x + this.raycastHit.normal.x * 3f, this.raycastHit.point.y + this.raycastHit.normal.y * 3f);
				}
			}
			else
			{
				if (particles)
				{
					EffectsController.CreateShrapnel(this.shrapnel, x, y, 4f, 30f, 2f, -this.xI * 0.2f, 20f);
					EffectsController.CreateShrapnel(this.shrapnelSpark, x, y, 4f, 30f, 2f, -this.xI * 0.2f, 20f);
				}
				if (this.life > 0.1f || useRayCast)
				{
					EffectsController.CreateEffect(this.flickPuff, x, y);
				}
			}
		}
	}

	protected override void HitUnits()
	{
		if (Map.HitUnits(this.firedBy, this.firedBy, this.playerNum, this.damageInternal, this.damageType, 8f, this.x, this.y, this.xI, this.yI, true, false))
		{
			this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
			this.DeregisterProjectile();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	protected override void Update()
	{
		if (this.flameObjectTranform != null)
		{
			this.flameObjectTranform.Translate(Vector3.down * Time.deltaTime * 32f);
		}
		base.Update();
	}

	public Transform flameObjectTranform;
}
