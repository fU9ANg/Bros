// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AnimatedTextureMultiple : MonoBehaviour
{
	private void Awake()
	{
		if (this.sprites.Length <= 0)
		{
			base.enabled = false;
		}
	}

	protected void Start()
	{
		this.Recalc();
	}

	public void Recalc()
	{
		this.lowerLeftPixel = this.sprites[0].lowerLeftPixel;
		this.pixelDimensions = this.sprites[0].pixelDimensions;
		if (this.forcePixelDimensions)
		{
			this.pixelDimensions.x = this.pixelDimensionForcedX;
		}
		this.rowLength = base.GetComponent<Renderer>().sharedMaterial.mainTexture.width / (int)this.pixelDimensions.x;
		this.rows = base.GetComponent<Renderer>().sharedMaterial.mainTexture.height / this.rowLength;
	}

	public void Restart()
	{
		base.gameObject.SetActive(true);
		this.frame = 0;
		this.counter = 0f;
		this.SetSpritesLowerLeft(new Vector2(this.lowerLeftPixel.x, this.lowerLeftPixel.y));
	}

	public void SetSpritesLowerLeft(Vector2 v2)
	{
		foreach (SpriteSM spriteSM in this.sprites)
		{
			spriteSM.SetLowerLeftPixel(v2);
		}
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		if (this.pauseTimeLeft < 0f)
		{
			this.counter += deltaTime;
		}
		else
		{
			this.pauseTimeLeft -= deltaTime;
		}
		int num = Mathf.FloorToInt(this.counter / this.frameRate);
		for (int i = 0; i < num; i++)
		{
			this.counter -= this.frameRate;
			this.frame += ((!this.pingingBack) ? 1 : -1);
			if (this.pingPong)
			{
				if (this.pingingBack && this.frame <= 0)
				{
					this.pingingBack = false;
				}
				else if (this.frame >= this.frames - 1)
				{
					this.pingingBack = true;
				}
			}
			else if (this.loop)
			{
				if (this.frame + this.deviatedOffset >= this.frames)
				{
					if (this.deviateLoops)
					{
						float value = UnityEngine.Random.value;
						if (this.deviatedOffset > 0 || value > 0.5f)
						{
							if (this.frame < this.frames)
							{
								this.deviatedOffset--;
							}
							this.frame = 0;
						}
						else
						{
							this.frame = this.frames - 1;
							this.deviatedOffset++;
						}
					}
					else
					{
						this.frame = 0;
					}
				}
			}
			else
			{
				this.frame = Mathf.Clamp(this.frame, 0, this.frames - 1);
			}
			if (this.pauseOnFrameZero && this.frame == 0)
			{
				this.pauseTimeLeft = this.pauseTime;
			}
			if (this.forceFrameSpacing)
			{
				this.SetSpritesLowerLeft(new Vector2(this.lowerLeftPixel.x + (float)(this.frame * this.frameSpacingWidth), this.lowerLeftPixel.y));
			}
			else if (this.lowerLeftPixel.x != 0f)
			{
				this.SetSpritesLowerLeft(new Vector2(this.lowerLeftPixel.x + (float)this.frame * this.pixelDimensions.x, this.lowerLeftPixel.y));
			}
			else
			{
				this.SetSpritesLowerLeft(new Vector2(this.lowerLeftPixel.x + (float)this.frame * this.pixelDimensions.x, this.lowerLeftPixel.y));
			}
		}
		if (this.frame >= this.frames)
		{
			base.gameObject.SetActive(false);
		}
	}

	public int GetFrame()
	{
		return this.frame;
	}

	public bool loop = true;

	public SpriteSM[] sprites;

	public float counter;

	public float frameRate = 0.0334f;

	public int frame;

	public int frames = 6;

	protected Vector2 lowerLeftPixel;

	protected Vector2 pixelDimensions;

	public bool forcePixelDimensions;

	public float pixelDimensionForcedX = 4f;

	public bool deviateLoops;

	protected int deviatedOffset;

	public bool forceFrameSpacing;

	public int frameSpacingWidth = 128;

	protected int rows = 1;

	protected int rowLength = 500;

	public bool animateOnceOnly;

	public bool pingPong;

	protected bool pingingBack;

	public bool useStartLowerPixel;

	public bool pauseOnFrameZero;

	public float pauseTime;

	private float pauseTimeLeft;
}
