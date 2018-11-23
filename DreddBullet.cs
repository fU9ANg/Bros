// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DreddBullet : Projectile
{
	public override void Fire(float x, float y, float xI, float yI, int playerNum, MonoBehaviour FiredBy)
	{
		base.Fire(x, y, xI, yI, playerNum, FiredBy);
		this.fireTime = Time.time;
	}

	protected override bool HitWalls()
	{
		if (Physics.Raycast(new Vector3(this.x - Mathf.Sign(this.xI) * 7f, this.y, 0f), new Vector3(this.xI, this.yI, 0f), out this.raycastHit, 19f, this.groundLayer))
		{
			if (Time.time - this.fireTime < 0.066f || this.raycastHit.collider.gameObject.GetComponent<BarrelBlock>() != null)
			{
				this.ProjectileApplyDamageToBlock(this.raycastHit.collider.gameObject, this.damageInternal, this.damageType, this.xI, this.yI);
				this.MakeEffects(false, this.raycastHit.point.x + this.raycastHit.normal.x * 3f, this.raycastHit.point.y + this.raycastHit.normal.y * 3f, true, this.raycastHit.normal, this.raycastHit.point);
				this.DeregisterProjectile();
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				this.MakeEffects(false, this.raycastHit.point.x + this.raycastHit.normal.x * 3f, this.raycastHit.point.y + this.raycastHit.normal.y * 3f, true, this.raycastHit.normal, this.raycastHit.point);
				this.ProjectileApplyDamageToBlock(this.raycastHit.collider.gameObject, this.damageInternal, this.damageType, this.xI, this.yI);
				if (this.bounceCount < this.maxBounceCount / 2)
				{
					this.life += 0.25f - (float)this.bounceCount * 0.05f;
				}
				this.bounceCount++;
				if (this.bounceCount >= this.maxBounceCount)
				{
					this.DeregisterProjectile();
					UnityEngine.Object.Destroy(base.gameObject);
				}
				else
				{
					if (this.yI == 0f)
					{
						DreddBullet.bulletCount++;
						if (DreddBullet.bulletCount % 2 == 1)
						{
							this.xI *= 0.5f;
							this.yI = Mathf.Abs(this.xI);
						}
						else
						{
							this.xI *= 0.5f;
							this.yI = -Mathf.Abs(this.xI);
						}
					}
					if (this.raycastHit.normal.y < -0.5f)
					{
						this.yI = Mathf.Abs(this.yI) * -1f;
					}
					else if (this.raycastHit.normal.y > 0.5f)
					{
						this.yI = Mathf.Abs(this.yI) * 1f;
					}
					if (this.raycastHit.normal.x < -0.5f)
					{
						this.xI = Mathf.Abs(this.xI) * -1f;
					}
					else if (this.raycastHit.normal.x > 0.5f)
					{
						this.xI = Mathf.Abs(this.xI) * 1f;
					}
				}
				this.SetRotation();
				this.x = this.raycastHit.point.x + this.xI * 0.01f;
				this.y = this.raycastHit.point.y + this.yI * 0.01f;
				EffectsController.CreateEffect(this.flickPuff, this.raycastHit.point.x + this.raycastHit.normal.x * 3f, this.raycastHit.point.y + this.raycastHit.normal.y * 3f);
			}
		}
		return true;
	}

	private int bounceCount;

	public int maxBounceCount = 8;

	protected float fireTime;

	private static int bulletCount;
}
