// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MechGrenadeHole : MechWeapon
{
	protected override void Awake()
	{
		base.Awake();
		this.sprite = base.GetComponent<SpriteSM>();
		this.spritePixelWidth = (int)this.sprite.pixelDimensions.x;
		this.spritePixelHeight = (int)this.sprite.pixelDimensions.y;
		this.height = 4f;
		this.width = 9f;
		this.invulnerable = true;
	}

	protected override void Update()
	{
		base.Update();
		if (this.fire)
		{
			if (this.holeFrame < 4)
			{
				this.frameCounter += this.t;
				if (this.frameCounter > 0.045f)
				{
					this.frameCounter -= 0.045f;
					this.holeFrame++;
					this.sprite.SetLowerLeftPixel((float)(this.spritePixelWidth * this.holeFrame), (float)((int)this.sprite.lowerLeftPixel.y));
				}
			}
		}
		else if (this.holeFrame > 0)
		{
			this.frameCounter += this.t;
			if (this.frameCounter > 0.045f)
			{
				this.frameCounter -= 0.045f;
				this.holeFrame--;
				this.sprite.SetLowerLeftPixel((float)(this.spritePixelWidth * this.holeFrame), (float)((int)this.sprite.lowerLeftPixel.y));
			}
		}
	}

	protected override void RunFiring()
	{
		this.fireDelay -= this.t;
		if (this.fire && this.health > 0 && this.mech.CanFire() && this.fireDelay <= 0f)
		{
			this.fireCounter += this.t;
			if (this.fireCounter > 0f)
			{
				this.fireCounter -= this.fireRate;
				this.FireWeapon(ref this.fireIndex);
			}
		}
		this.wasFire = this.fire;
	}

	public override void Fire()
	{
		if (!this.wasFire)
		{
			this.fireDelay = 0.6f;
			this.fireCounter = 0f;
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.greeting, 0.66f, base.transform.position);
		}
		this.fire = true;
	}

	protected override void FireWeapon(ref int index)
	{
		index++;
		if (index >= 5)
		{
			this.fire = false;
			this.fireDelay = 2f;
			index = 0;
		}
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.special2Sounds, 0.66f, base.transform.position);
		if (base.IsMine)
		{
			Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
			if (insideUnitCircle.y < 0f)
			{
				insideUnitCircle.y *= -1f;
			}
			if (index % 2 == 0)
			{
				if (insideUnitCircle.x < 0f)
				{
					insideUnitCircle.x = Mathf.Clamp(insideUnitCircle.x * -1f, 0.3f, 1f);
				}
			}
			else if (insideUnitCircle.x > 0f)
			{
				insideUnitCircle.x = Mathf.Clamp(insideUnitCircle.x * -1f, -1f, -0.3f);
			}
			ProjectileController.SpawnGrenadeOverNetwork(this.bigGrenade, this, base.transform.position.x, base.transform.position.y + 8f, 0.001f, 0f, insideUnitCircle.x * 100f, 170f + insideUnitCircle.y, -15);
		}
	}

	protected int spritePixelHeight;

	protected int holeFrame;

	public Grenade bigGrenade;
}
