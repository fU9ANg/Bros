// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BulletPoint : MonoBehaviour
{
	public void Deactivate()
	{
		if (this.activated)
		{
			this.activated = false;
			if (this.setup)
			{
				this.sprite.SetLowerLeftPixel(0f, (float)this.spriteWidth);
			}
		}
	}

	public void Activate()
	{
		if (!this.activated)
		{
			this.activated = true;
			if (this.setup)
			{
				this.sprite.SetLowerLeftPixel((float)this.spriteWidth, (float)this.spriteWidth);
			}
		}
		this.counter = 3f;
	}

	private void Awake()
	{
		this.sprite = base.GetComponent<SpriteSM>();
	}

	protected void Update()
	{
		if (this.activated)
		{
			float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
			if (!this.animating)
			{
				this.counter += num;
				if (this.counter - this.delay > 3.5f)
				{
					this.counter = 0f + this.delay;
					this.animating = true;
					this.frame = 0;
				}
			}
			else
			{
				this.frameCounter += num;
				if (this.frameCounter > this.frameRate)
				{
					this.frameCounter -= this.frameRate;
					this.frame++;
					if (this.frame > 6)
					{
						this.animating = false;
						this.frame = 0;
					}
					this.sprite.SetLowerLeftPixel((float)((1 + this.frame) * this.spriteWidth), (float)this.spriteWidth);
				}
			}
		}
	}

	public void Setup(float delay)
	{
		this.delay = delay;
	}

	private void Start()
	{
		this.setup = true;
		if (!this.activated)
		{
			this.sprite.SetLowerLeftPixel(0f, (float)this.spriteWidth);
		}
		else
		{
			this.sprite.SetLowerLeftPixel((float)this.spriteWidth, (float)this.spriteWidth);
		}
	}

	protected SpriteSM sprite;

	protected bool activated;

	protected bool setup;

	public int spriteWidth = 8;

	protected float delay;

	protected float frameRate = 0.0334f;

	protected bool animating;

	protected float counter;

	protected float frameCounter;

	protected int frame;
}
