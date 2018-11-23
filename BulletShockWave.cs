// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletShockWave : Projectile
{
	protected override void Awake()
	{
		base.Awake();
		this.hitUnits = new List<Unit>(15);
		this.sprite = base.GetComponent<SpriteSM>();
		this.hasMadeEffects = false;
	}

	protected override void HitUnits()
	{
		if (Map.HitUnits(this.firedBy, this.playerNum, this.damageInternal, this.damageType, 8f, this.x, this.y, this.xI, this.yI, false, false, true, ref this.hitUnits))
		{
			this.MakeEffects(false, this.x, this.y, false, Vector3.zero, Vector3.zero);
			this.penetrateCount++;
			if (this.penetrateCount >= this.maxPenetrations)
			{
				this.DeregisterProjectile();
				UnityEngine.Object.Destroy(base.gameObject);
			}
			this.damageInternal--;
		}
	}

	protected override void HitProjectiles()
	{
		if (Map.HitProjectiles(this.playerNum, this.damageInternal, this.damageType, this.projectileSize, this.x, this.y, this.xI, this.yI, 0.1f))
		{
			this.MakeEffects(false, this.x, this.y, false, Vector3.zero, Vector3.zero);
		}
		if (this.damage > 15 && Map.DeflectProjectiles(this.firedBy, this.playerNum, 16f, this.x, this.y, this.xI * 1.3f))
		{
			UnityEngine.Debug.Log("Deflect projectil!");
		}
	}

	protected override void RunLife()
	{
		base.RunLife();
		float num = this.life / this.fullLife;
		this.sprite.SetColor(Color.Lerp(this.startColor, this.endColor, (1f - num) * 3f));
	}

	protected override void MakeEffects(bool particles, float x, float y, bool useRayCast, Vector3 hitNormal, Vector3 hitPoint)
	{
		if (!this.hasMadeEffects)
		{
			EffectsController.CreateEffect(this.endPuff, base.transform.position.x, base.transform.position.y, 0f, (int)Mathf.Sign(base.transform.localScale.x), (int)Mathf.Sign(base.transform.localScale.y), BloodColor.None);
		}
		this.hasMadeEffects = true;
	}

	protected override void RunProjectile(float t)
	{
		base.RunProjectile(t);
	}

	protected override void CheckSpawnPoint()
	{
		Collider[] array = Physics.OverlapSphere(new Vector3(this.x, this.y, 0f), 5f, this.groundLayer);
		if (array.Length > 0)
		{
			this.MakeEffects(false, this.x, this.y, false, Vector3.zero, Vector3.zero);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SendMessage("Damage", new DamageObject(this.damageInternal, this.damageType, this.xI, this.yI, this.firedBy));
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			this.RegisterProjectile();
		}
		if (Map.HitUnits(this.firedBy, this, this.playerNum, this.damageInternal, this.damageType, this.projectileSize + ((this.playerNum < 0) ? 0f : (this.projectileSize * 0.5f)), this.x - ((this.playerNum < 0) ? 0f : (this.projectileSize * 0.5f)) * (float)((int)Mathf.Sign(this.xI)), this.y, this.xI, this.yI, true, false))
		{
			this.MakeEffects(false, this.x, this.y, false, Vector3.zero, Vector3.zero);
		}
	}

	public int maxPenetrations = 9;

	private int penetrateCount;

	protected List<Unit> hitUnits;

	public Color startColor = Color.white;

	public Color endColor = Color.white;

	protected SpriteSM sprite;

	public Puff endPuff;
}
