// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BulletBroniversalSoldier : Projectile
{
	public override void Fire(float x, float y, float xI, float yI, int playerNum, MonoBehaviour FiredBy)
	{
		base.Fire(x, y, xI, yI, playerNum, FiredBy);
		this.delay = (float)(BulletBroniversalSoldier.fireCount % 2) * this.delayScale + (float)(BulletBroniversalSoldier.fireCount % 3) * this.delayScale * 2f;
		if (BulletBroniversalSoldier.fireCount % 5 == 0)
		{
			BulletBroniversalSoldier.fireCount++;
		}
		if (this.delay > 0f)
		{
			base.GetComponent<Renderer>().enabled = false;
		}
		if (BulletBroniversalSoldier.fireCount % 2 == 1)
		{
			this.damageInternal = this.everySecondDamage; this.damage = (this.damageInternal );
		}
		BulletBroniversalSoldier.fireCount++;
	}

	protected override void Update()
	{
		if (this.delay > 0f)
		{
			this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
			this.delay -= this.t;
			if (this.delay <= 0f)
			{
				base.GetComponent<Renderer>().enabled = true;
			}
		}
		else
		{
			this.t = Mathf.Clamp(Time.deltaTime * 0.5f, 0f, 0.0334f);
			this.RunProjectile(this.t);
			this.RunProjectile(this.t);
			this.life -= this.t * 2f;
			if (this.life <= 0f)
			{
				this.MakeEffects(false, this.x, this.y, false, this.raycastHit.normal, this.raycastHit.point);
				this.DeregisterProjectile();
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	protected override void LateUpdate()
	{
		if (this.delay <= 0f)
		{
			base.LateUpdate();
		}
	}

	public int everySecondDamage = 2;

	protected static int fireCount;

	protected float delay;

	public float delayScale = 0.015f;
}
