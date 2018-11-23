// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookMiniGunBullet : Projectile
{
	protected override void CheckSpawnPoint()
	{
		base.CheckSpawnPoint();
		if (!this.reversing)
		{
		}
	}

	protected override bool HitWalls()
	{
		if (this.canReflect && Physics.Raycast(new Vector3(this.x - Mathf.Sign(this.xI) * this.projectileSize, this.y, 0f), new Vector3(this.xI, this.yI, 0f), out this.raycastHit, this.projectileSize * 2f, this.barrierLayer))
		{
			return this.ReflectProjectile(this.raycastHit);
		}
		if (Physics.Raycast(new Vector3(this.x - Mathf.Sign(this.xI) * 7f, this.y, 0f), new Vector3(this.xI, this.yI, 0f), out this.raycastHit, 19f, this.groundLayer))
		{
			this.MakeEffects(true, this.raycastHit.point.x + this.raycastHit.normal.x * 3f, this.raycastHit.point.y + this.raycastHit.normal.y * 3f, true, this.raycastHit.normal, this.raycastHit.point);
			MookMiniGunBullet.miniGunBulletCount++;
			if (MookMiniGunBullet.miniGunBulletCount % 3 == 0)
			{
				this.ProjectileApplyDamageToBlock(this.raycastHit.collider.gameObject, this.damageInternal, this.damageType, this.xI, this.yI);
			}
			else
			{
				this.ProjectileApplyDamageToBlock(this.raycastHit.collider.gameObject, 0, this.damageType, this.xI, this.yI);
			}
			this.DeregisterProjectile();
			UnityEngine.Object.Destroy(base.gameObject);
		}
		return true;
	}

	private static int miniGunBulletCount;
}
