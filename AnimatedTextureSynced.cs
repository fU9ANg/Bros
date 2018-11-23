// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AnimatedTextureSynced : MonoBehaviour
{
	protected void Start()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		if (this.offsetDelaysBasedOnX)
		{
			this.delayOffset = base.transform.position.x / this.delayOffsetXM * 0.25f;
		}
	}

	private void Update()
	{
		int num = Mathf.FloorToInt((Time.time - this.delayOffset) / this.frameRate);
		num %= this.frames + this.frameDelay;
		if (num >= this.frames)
		{
			this.frame = 0;
		}
		else
		{
			this.frame = num;
		}
		Vector2 pixelDimensions = this.sprite.pixelDimensions;
		if (this.forcePixelDimensions)
		{
			pixelDimensions.x = this.pixelDimensionForcedX;
		}
		this.sprite.SetLowerLeftPixel(new Vector2((float)this.frame * pixelDimensions.x, this.sprite.lowerLeftPixel.y));
	}

	public void Animate()
	{
	}

	protected SpriteSM sprite;

	protected float counter;

	public float frameRate = 0.0334f;

	protected int frame;

	public int frames = 6;

	public int frameDelay;

	public bool forcePixelDimensions;

	public float pixelDimensionForcedX = 4f;

	public bool offsetDelaysBasedOnX;

	protected float delayOffsetXM = 16f;

	protected float delayOffset;

	protected bool animating;
}
