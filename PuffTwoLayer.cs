// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class PuffTwoLayer : MonoBehaviour
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
		this.spriteLowerPixel1 = (int)this.spriteLayer1.lowerLeftPixel.y;
		this.spriteLowerPixel2 = (int)this.spriteLayer2.lowerLeftPixel.y;
		if (this.useLightingMultiplier && LightingCamera.GetLightingMultiplier() > 0f)
		{
			Color color = (Color.white + LightingCamera.GetLightingColor()) / 2f;
			this.spriteLayer1.SetColor(color);
			this.spriteLayer2.SetColor(color);
		}
	}

	private void Start()
	{
	}

	public void SetColor(Color color)
	{
		this.spriteLayer1.SetColor(color);
		this.spriteLayer2.SetColor(color);
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
				if (this.frame >= this.frames && this.loops > 0)
				{
					this.frame = 0;
					this.loops--;
				}
				if (this.frame >= this.frames)
				{
					UnityEngine.Object.Destroy(base.gameObject);
					break;
				}
				this.spriteLayer1.SetLowerLeftPixel((float)(this.frame * this.spriteSize), (float)this.spriteLowerPixel1);
				this.spriteLayer2.SetLowerLeftPixel((float)(this.frame * this.spriteSize), (float)this.spriteLowerPixel2);
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

	public SpriteSM spriteLayer1;

	public SpriteSM spriteLayer2;

	protected float counter;

	public float frameRate = 0.0667f;

	private int frame;

	[HideInInspector]
	public float delay;

	public int spriteSize = 32;

	public int frames = 16;

	protected int spriteLowerPixel1 = 32;

	protected int spriteLowerPixel2 = 32;

	public int loops;

	public bool useGravity;

	public float gravityM;

	public bool correctRotation;

	public bool useLightingMultiplier;

	protected Vector3 velocity;
}
