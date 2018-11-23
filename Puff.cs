// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Puff : MonoBehaviour
{
	public void Delay(float d)
	{
		this.delay = d;
		base.GetComponent<Renderer>().enabled = false;
	}

	public void SetVelocity(Vector3 velocity)
	{
		this.velocity = velocity;
	}

	private void Awake()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		this.spriteLowerPixel = (int)this.sprite.lowerLeftPixel.y;
		if (this.useLightingMultiplier && LightingCamera.GetLightingMultiplier() > 0f)
		{
			Color color = (Color.white + LightingCamera.GetLightingColor()) / 2f;
			this.sprite.SetColor(color);
		}
	}

	public void CropBottom(float heightLoss)
	{
		this.pixelsHeightLoss = (int)heightLoss;
	}

	private void Start()
	{
		if (this.pixelsHeightLoss > 0)
		{
			this.spriteLowerPixel = (int)this.sprite.lowerLeftPixel.y - this.pixelsHeightLoss;
			this.sprite.SetPixelDimensions((int)this.sprite.pixelDimensions.x, (int)this.sprite.pixelDimensions.y - this.pixelsHeightLoss);
			this.sprite.SetSize(this.sprite.width, this.sprite.height - (float)this.pixelsHeightLoss);
		}
	}

	public void SetColor(Color color)
	{
		this.sprite.SetColor(color);
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		if (this.delay > 0f)
		{
			this.delay -= deltaTime;
			if (this.delay <= 0f)
			{
				base.GetComponent<Renderer>().enabled = true;
			}
		}
		else
		{
			base.transform.position += this.velocity * deltaTime;
			if (!this.useGravity)
			{
				this.velocity *= 1f - deltaTime * 15f;
			}
			else
			{
				this.velocity *= 1f - deltaTime * 2f;
				this.velocity.y = this.velocity.y - this.gravityM * 1000f * deltaTime;
			}
			this.counter += deltaTime;
			int num = (int)(this.counter / this.frameRate);
			for (int i = 0; i < num; i++)
			{
				this.counter -= this.frameRate;
				this.frame++;
				if (this.frame >= this.frames)
				{
					UnityEngine.Object.Destroy(base.gameObject);
					break;
				}
				this.sprite.SetLowerLeftPixel((float)(this.frame * this.spriteSize), (float)this.spriteLowerPixel);
			}
		}
	}

	private void LateUpdate()
	{
		if (this.correctRotation && base.transform.eulerAngles.z != 0f)
		{
			base.transform.eulerAngles = Vector3.zero;
		}
	}

	protected SpriteSM sprite;

	protected float counter;

	public float frameRate = 0.0667f;

	private int frame;

	[HideInInspector]
	public float delay;

	public int spriteSize = 32;

	public int frames = 16;

	protected int spriteLowerPixel = 32;

	public bool useGravity;

	public float gravityM;

	public bool correctRotation;

	public bool useLightingMultiplier;

	protected int pixelsHeightLoss;

	protected Vector3 velocity;
}
