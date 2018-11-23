// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AnimatedIcon : MonoBehaviour
{
	protected void Start()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		this.lowerLeftPixel = this.sprite.lowerLeftPixel;
		this.pixelDimensions = this.sprite.pixelDimensions;
		if (this.forcePixelDimensions)
		{
			this.pixelDimensions.x = this.pixelDimensionForcedX;
		}
	}

	public void GoAway()
	{
		if (this.frame <= this.animatedRestFrame + 1)
		{
			this.goingAway = true;
			this.frame = this.animatedRestFrame + 1;
			this.counter = this.frameRate * 0.5f;
		}
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.stayOnRestFrame && this.frame == this.animatedRestFrame)
		{
			this.stayOnRestFrame = false;
		}
		else
		{
			this.counter += num;
		}
		if (this.counter > this.frameRate)
		{
			this.counter -= this.frameRate;
			this.frame++;
			this.sprite.SetLowerLeftPixel(new Vector2(this.lowerLeftPixel.x + (float)this.frame * this.pixelDimensions.x, this.lowerLeftPixel.y));
			if (this.frame == this.animatedRestFrame)
			{
				this.counter -= this.restTime;
			}
			if (this.frame == this.animatedFinishFrame)
			{
				if (!this.destroyOnStop)
				{
					base.gameObject.SetActive(false);
				}
				else
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}
	}

	protected SpriteSM sprite;

	protected float counter;

	public float frameRate = 0.0334f;

	protected int frame;

	public int animatedRestFrame = 8;

	public int animatedFinishFrame = 18;

	public float restTime = 0.5f;

	protected Vector2 lowerLeftPixel;

	protected Vector2 pixelDimensions;

	public bool forcePixelDimensions;

	public float pixelDimensionForcedX = 4f;

	public bool destroyOnStop;

	public bool goingAway;

	public bool stayOnRestFrame;
}
