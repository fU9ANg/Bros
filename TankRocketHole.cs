// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TankRocketHole : MonoBehaviour
{
	private void Awake()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		this.lowerLeftPixel = this.sprite.lowerLeftPixel;
		this.pixelDimensions = this.sprite.pixelDimensions;
	}

	private void Update()
	{
		if (this.fired)
		{
			this.fireCounter += Time.deltaTime;
			if (this.fireCounter > 0.067f)
			{
				this.fireCounter -= 0.067f;
				this.frame++;
				this.Animate();
			}
		}
	}

	public void Fire()
	{
		this.fired = true;
		this.frame = 1;
		this.Animate();
		this.fireCounter -= 0.133f;
	}

	public void Animate()
	{
		if (this.frame == this.pauseFrame)
		{
			this.fireCounter -= 0.5f;
		}
		else if (this.frame == this.endFrame)
		{
			this.frame = 0;
			this.fired = false;
		}
		this.sprite.SetLowerLeftPixel(new Vector2(this.lowerLeftPixel.x + (float)this.frame * this.pixelDimensions.x, this.lowerLeftPixel.y));
	}

	protected int frame;

	protected SpriteSM sprite;

	protected Vector2 lowerLeftPixel;

	protected Vector2 pixelDimensions;

	protected int pauseFrame = 4;

	protected bool fired;

	protected float fireCounter;

	protected int endFrame = 8;
}
