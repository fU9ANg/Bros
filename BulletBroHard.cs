// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BulletBroHard : Projectile
{
	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Update()
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

	protected override void RunProjectile(float t)
	{
		base.RunProjectile(t);
	}
}
