// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Gib : BloodyShrapnel
{
	public override void Launch(float x, float y, float xI, float yI)
	{
		base.Launch(x, y, xI * this.forceMultiplier, yI * this.forceMultiplier);
		this.rotateFrameRate = 0.03334f + UnityEngine.Random.value * 0.0334f;
	}

	public void SetupSprite(bool doesRotate, Vector2 lowerLeft, Vector2 pixelD, Vector3 offset, int rotateFramesCount)
	{
		if (this.sprite != null)
		{
			this.lowerLeftPixel = lowerLeft;
			this.pixelDimensions = pixelD;
			this.sprite.RecalcTexture();
			this.sprite.SetLowerLeftPixel(lowerLeft);
			this.sprite.SetPixelDimensions(pixelD);
			this.sprite.SetOffset(offset);
		}
		else
		{
			UnityEngine.Debug.LogError("No Sprite!!!");
		}
		this.doesRotate = doesRotate;
		this.rotateFrames = rotateFramesCount;
	}

	public Vector2 GetLowerLeftPixel()
	{
		if (this.sprite == null)
		{
			this.sprite = base.GetComponent<SpriteSM>();
		}
		return this.sprite.lowerLeftPixel;
	}

	public Vector2 GetPixelDimensions()
	{
		if (this.sprite == null)
		{
			this.sprite = base.GetComponent<SpriteSM>();
		}
		return this.sprite.pixelDimensions;
	}

	public Vector2 GetSpriteOffset()
	{
		if (this.sprite == null)
		{
			this.sprite = base.GetComponent<SpriteSM>();
		}
		return this.sprite.offset;
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
		if (this.hasSmokeTrail)
		{
			this.smokeDelay += this.t;
			if ((double)this.smokeDelay > 0.034)
			{
				this.smokeDelay -= 0.034f;
				EffectsController.CreatePitchBlackPlumeParticle(this.x, this.y, 16f, 0f, 45f, 0.8f * Mathf.Clamp(this.life, 0.1f, 1f), 0.75f * Mathf.Clamp(this.life, 0.1f, 1f));
				if (this.lastSmokePosition.x > 0f)
				{
					float num = Vector3.Distance(this.lastSmokePosition, base.transform.position);
					if (num > 4f)
					{
						for (float num2 = 4f / num; num2 < 1f; num2 += num2)
						{
							Vector3 vector = Vector3.Lerp(this.lastSmokePosition, base.transform.position, num2);
							EffectsController.CreatePitchBlackPlumeParticle(vector.x, vector.y, 16f, 0f, 45f, 0.8f * Mathf.Clamp(this.life, 0.1f, 1f), 0.75f * Mathf.Clamp(this.life, 0.1f, 1f));
						}
					}
				}
				this.lastSmokePosition = base.transform.position;
			}
		}
		if (this.doesRotate)
		{
			this.rotateCounter += this.t;
			if (this.rotateCounter > this.rotateFrameRate)
			{
				this.rotateCounter -= this.rotateFrameRate;
				this.frame++;
				this.sprite.SetLowerLeftPixel(new Vector2(this.lowerLeftPixel.x + (float)(this.frame % this.rotateFrames) * this.pixelDimensions.x, this.lowerLeftPixel.y));
			}
		}
	}

	public int rotateFrames = 8;

	protected int frame;

	protected Vector2 lowerLeftPixel;

	protected Vector2 pixelDimensions;

	protected float rotateCounter;

	protected float rotateFrameRate = 0.045f;

	public bool doesRotate;

	public bool hasSmokeTrail;

	private Vector3 lastSmokePosition = -Vector3.one;

	public float forceMultiplier = 1f;

	private float smokeDelay;
}
