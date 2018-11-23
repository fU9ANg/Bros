// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Satan : Mook
{
	protected override void AnimateRunning()
	{
		if (this.usingSpecial)
		{
			this.AnimateSpecial();
		}
		else
		{
			this.frameRate = 0.044455f;
			int num = 1 + this.frame % 3;
			this.sprite.SetLowerLeftPixel((float)(num * 32), 32f);
			if (this.frame % 2 == 0)
			{
				EffectsController.CreateFootPoofEffect(this.x, this.y + 2f, 0f, Vector3.up * 1f - Vector3.right * base.transform.localScale.x * 60.5f);
			}
		}
	}

	protected override void AnimateSpecial()
	{
		this.gunSprite.gameObject.SetActive(false);
		this.frameRate = 0.0667f;
		int num = 17 + this.laughFrame % 4;
		this.sprite.SetLowerLeftPixel((float)(num * 32), 32f);
		this.laughFrame++;
		if (this.laughFrame >= 12)
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

	protected override void UseFire()
	{
	}

	protected override void Update()
	{
		base.Update();
		if (this.health <= 0)
		{
			this.evilSpunkCounter += this.t;
			if (this.evilSpunkCounter > 0.2f)
			{
				this.evilSpunkCounter -= 0.2f;
			}
		}
	}

	protected override void AnimateDeath()
	{
		if (this.deadFrames > 6)
		{
			int num = 25 + this.frame;
			this.sprite.SetLowerLeftPixel((float)(num * 32), 32f);
			if (this.frame % 3 == 1)
			{
				this.ShootOffEvilSpunk(1);
			}
			if (this.frame > 21)
			{
				this.destroyed = true;
			}
		}
		else if (this.y > this.groundHeight + 0.2f)
		{
			this.deadFrames = 0;
			int num2 = 4;
			this.sprite.SetLowerLeftPixel((float)(num2 * 32), 32f);
		}
		else
		{
			int num3 = 5;
			this.sprite.SetLowerLeftPixel((float)(num3 * 32), 32f);
			this.deadFrames++;
			if (this.deadFrames > 6)
			{
				this.invulnerable = true;
				this.frame = 0;
			}
		}
	}

	protected override void Gib(DamageType damageType, float xI, float yI)
	{
		base.Gib(damageType, xI, yI);
		this.SprayBlood(40f);
		this.SprayBlood(40f);
		for (int i = 0; i < this.blood.Length; i++)
		{
			EffectsController.CreateShrapnel(this.blood[i], this.x, this.y + 8f, 8f, 100f, 6f, xI * 0.5f, yI * 0.4f + 60f);
		}
		this.frame = 0;
		this.ShootOffEvilSpunk(8);
		this.destroyed = true;
	}

	protected void ShootOffEvilSpunk(int count)
	{
		for (int i = 0; i < count; i++)
		{
			if (this.evilSpunkCount < 8)
			{
				this.evilSpunkCount++;
				EffectsController.CreateShrapnel(this.evilParticlePrefab, this.x, this.y, 12f, 50f, 1f, this.xI * 0.6f, 50f + this.yI * 0.3f);
			}
		}
	}

	public override bool IsEvil()
	{
		return true;
	}

	protected int laughCount;

	protected int laughFrame;

	protected int evilSpunkCount;

	protected float evilSpunkCounter;

	public Shrapnel[] evilGibs;

	public Shrapnel evilParticlePrefab;

	protected int deadFrames;
}
