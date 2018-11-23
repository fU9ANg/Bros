// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookRiotShield : Mook
{
	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		if (damageType == DamageType.Fire)
		{
			this.fireAmount += damage;
		}
		else
		{
			this.shieldDamage += damage;
		}
		if (damageType == DamageType.Crush || this.actionState == ActionState.Panicking || (!this.hasShield && this.shieldTime < 0f) || this.health <= 0 || this.fireAmount > 35 || direction == 0 || direction == (int)Mathf.Sign(base.transform.localScale.x))
		{
			base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
		}
		else if (damageType != DamageType.Fire)
		{
			this.Knock(damageType, xI * 0.05f, 15f, false);
		}
		if (this.health > 0 && (damageType == DamageType.Explosion || damageType == DamageType.Melee || damageType == DamageType.Knifed || this.shieldDamage > 35) && this.hasShield)
		{
			this.DisarmShield(xI);
		}
	}

	public override void Knock(DamageType damageType, float xI, float yI, bool forceTumble)
	{
		if (Mathf.Sign(xI) != Mathf.Sign(base.transform.localScale.x))
		{
			this.KnockSimple(new DamageObject(0, DamageType.Bullet, xI, yI * 0.3f, null));
		}
		else
		{
			base.Knock(damageType, xI, yI, forceTumble);
		}
	}

	public override void BurnInternal(int damage, int direction)
	{
		this.fireAmount += damage;
		if (this.fireAmount > 35)
		{
			base.BurnInternal(damage, direction);
		}
	}

	protected override bool PanicAI(int direction, bool forgetPlayer)
	{
		if (this.hasShield)
		{
			this.DisarmShield(this.xI);
		}
		return base.PanicAI(direction, forgetPlayer);
	}

	protected void DisarmShield(float xForce)
	{
		if (this.health > 0)
		{
			this.yI += 70f;
			this.xI += xForce * 0.3f;
		}
		if (this.hasShield)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.unarmedMaterial;
		}
		this.shieldTime = 0.08f;
		this.hasShield = false;
		EffectsController.CreateShrapnel(this.shieldShrapnel, this.x + base.transform.localScale.x * 8f, this.y + 8f, 0.1f, 96f, 1f, this.xI * 0.4f, this.yI * 0.3f + 200f);
		this.DeactivateGun();
	}

	public override void Alert(float alertX, float alertY)
	{
		if (!this.hasShield)
		{
			this.Panic((int)Mathf.Sign(this.x - alertX), 0.9f, false);
		}
		else
		{
			base.Alert(alertX, alertY);
		}
	}

	public override void HearSound(float alertX, float alertY)
	{
		if (this.blindTime <= 0f)
		{
			base.HearSound(alertX, alertY);
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!this.hasShield)
		{
			this.shieldTime -= this.t;
		}
	}

	protected override void ConstrainToMookBarriers(ref float xIT, float radius)
	{
		if (!this.hasShield)
		{
			base.ConstrainToMookBarriers(ref xIT, radius);
		}
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		base.GetComponent<Collider>().enabled = false;
		if (this.hasShield)
		{
			this.DisarmShield(xI);
		}
		base.Death(xI, yI, damage);
	}

	protected override void Gib(DamageType damageType, float xI, float yI)
	{
		if (this.hasShield)
		{
			this.DisarmShield(0f);
		}
		base.Gib(damageType, xI, yI);
	}

	protected override void DeactivateGun()
	{
		base.GetComponent<Collider>().enabled = false;
		base.DeactivateGun();
	}

	protected override void ActivateGun()
	{
		if (this.hasShield)
		{
			base.GetComponent<Collider>().enabled = true;
			base.ActivateGun();
		}
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
	}

	protected int fireAmount;

	protected int shieldDamage;

	protected bool hasShield = true;

	protected float shieldTime = 0.2f;

	public Shrapnel shieldShrapnel;

	public Material unarmedMaterial;
}
