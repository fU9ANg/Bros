// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DoodadSignPost : Doodad
{
	protected override void Start()
	{
		base.Start();
		this.sprite = base.GetComponent<SpriteSM>();
		this.colorM = -0.5f;
		this.text.text = this.message.ToUpper();
		this.box.desiredSize = new Vector2(this.boxWidthPadding + this.text.GetComponent<Renderer>().bounds.extents.x * 2f, this.boxHeightPadding + this.text.GetComponent<Renderer>().bounds.extents.y * 2f);
		this.box.SetMinimumSize(this.box.desiredSize * 0.7f);
		this.text.gameObject.SetActive(false);
		this.box.gameObject.SetActive(false);
		this.text.transform.localPosition = new Vector3(0f, 64f + this.boxHeightPadding + this.text.GetComponent<Renderer>().bounds.extents.y * 2f, -1f);
		this.box.transform.localPosition = new Vector3(0f, 64f + this.boxHeightPadding + this.text.GetComponent<Renderer>().bounds.extents.y * 2f, 0f);
	}

	protected virtual void Update()
	{
		if (!this.reading)
		{
			this.thinkCounter += Time.deltaTime;
			if (this.thinkCounter > 0.1f)
			{
				this.thinkCounter -= 0.1f;
				int num = -1;
				if (HeroController.GetNearestPlayer(this.x, this.y + 8f, 24f, 24f, ref num))
				{
					if (!this.animating)
					{
						this.StartAnimating();
					}
					this.StartReading();
				}
			}
		}
		if (this.reading)
		{
			int num2 = -1;
			this.colorM += Time.deltaTime * 4f;
			this.text.color = new Color(0f, 0f, 0f, Mathf.Clamp01(this.colorM));
			if (!HeroController.GetNearestPlayer(this.x, this.y + 12f, 36f, 24f, ref num2))
			{
				this.StopReading();
			}
		}
		if (this.animating)
		{
			this.frameCounter += Time.deltaTime;
			if (this.frameCounter > this.frameRate)
			{
				this.frameCounter -= this.frameRate;
				this.frame++;
				if (this.frame >= 11)
				{
					this.frame = 0;
					this.animating = false;
				}
				this.sprite.SetLowerLeftPixel((float)(this.frame * 16), 32f);
			}
		}
	}

	protected void StopReading()
	{
		this.reading = false;
		this.colorM = -1f;
		this.text.color = new Color(0f, 0f, 0f, 0f);
		this.text.gameObject.SetActive(false);
		this.box.gameObject.SetActive(false);
	}

	protected void StartReading()
	{
		this.reading = true;
		this.colorM = -1f;
		this.text.color = new Color(0f, 0f, 0f, 0f);
		this.text.gameObject.SetActive(true);
		this.box.gameObject.SetActive(true);
		this.box.SetToMinimumSize();
		EffectsController.CreatePuffDisappearRingEffect(this.x, this.y + 2f, 0f, 0f);
	}

	protected void StartAnimating()
	{
		this.animating = true;
		this.frame = 0;
		this.frameCounter = 0f;
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.greeting, 0.8f, base.transform.position);
		this.sprite.SetLowerLeftPixel((float)(this.frame * 16), 32f);
	}

	protected bool reading;

	protected bool animating;

	private float thinkCounter;

	private int frame;

	private float frameCounter;

	protected SpriteSM sprite;

	public float frameRate = 0.033f;

	public TextMesh text;

	public string message;

	protected float colorM;

	public ScalingBox box;

	public float boxWidthPadding = 24f;

	public float boxHeightPadding = 12f;
}
