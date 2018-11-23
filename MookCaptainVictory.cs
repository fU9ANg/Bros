// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookCaptainVictory : Satan
{
	protected override void Start()
	{
		base.Start();
		this.health = 4;
		this.playerNum = -15;
		this.invulnerable = false;
		this.invulnerableTime = -1f;
		this.height = 20f;
		this.width = 14f;
		Map.RegisterUnit(this, false);
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		this.Gib(damageType, xI, yI);
		this.health = -11;
		this.Death(xI, yI, new DamageObject(damage, damageType, xI, yI, damageSender));
	}

	protected override void Gib(DamageType damageType, float xI, float yI)
	{
		if (!this.destroyed)
		{
			this.CreateGibEffects(damageType, xI, yI);
			this.destroyed = true;
		}
	}

	protected override void AnimateSpecial()
	{
		this.gunSprite.gameObject.SetActive(false);
		this.frameRate = 0.0667f;
		int num = 33 + this.laughFrame % 17;
		this.sprite.SetLowerLeftPixel((float)(num * 32), 64f);
		this.laughFrame++;
		if (this.laughFrame >= 142)
		{
			this.laughFrame = 0;
			this.laughCount++;
			if (this.laughCount > 5)
			{
				this.usingSpecial = false;
				this.laughCount = 0;
				this.enemyAI.ForgetPlayer();
			}
		}
	}
}
