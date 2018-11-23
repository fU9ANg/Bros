// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AnimatedTextureComplicated : MonoBehaviour
{
	protected void Awake()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		this.lowerLeftPixel = this.sprite.lowerLeftPixel;
		this.pixelDimensions = this.sprite.pixelDimensions;
		this.animate = this.animateAutomatically;
	}

	protected void Start()
	{
		if (!this.chosenAtLeastOnce)
		{
			this.Restart();
		}
	}

	public void Restart()
	{
		this.sound = Sound.GetInstance();
		this.animationIndex = -1;
		this.counter = 0f;
		this.ChooseNewAnimation();
		base.gameObject.SetActive(true);
		UnityEngine.Debug.Log("Restart !! " + this.animationIndex);
	}

	public void PlayAnimation(string animationName)
	{
		UnityEngine.Debug.Log("Play Animation! " + animationName);
		bool flag = false;
		for (int i = 0; i < this.animations.Length; i++)
		{
			if (this.animations[i].name.ToLower() == animationName.ToLower())
			{
				this.animationIndex = i;
				flag = true;
			}
		}
		if (flag)
		{
			this.counter = 0f;
			this.animate = true;
			this.currentFrameRow = this.animations[this.animationIndex].startFrameRow;
			this.currentFrameCollumn = this.animations[this.animationIndex].startFrameCollumn;
			this.currentFrameRate = this.animations[this.animationIndex].frameRate;
			this.endFrameCollumn = this.animations[this.animationIndex].endFrameCollumn;
			this.loopsLeft = this.animations[this.animationIndex].loops;
			if (this.animations[this.animationIndex].soundEffect != null && this.sound != null)
			{
				this.sound.PlaySoundEffectAt(this.animations[this.animationIndex].soundEffect, this.animations[this.animationIndex].soundEffectVolume, base.transform.position);
			}
			this.sprite.SetLowerLeftPixel(new Vector2(this.lowerLeftPixel.x + (float)this.currentFrameCollumn * this.pixelDimensions.x, this.lowerLeftPixel.y + (float)this.currentFrameRow * this.pixelDimensions.y));
		}
		else
		{
			UnityEngine.Debug.LogError("Animation Name " + animationName + " Not Found");
		}
	}

	protected void ChooseNewAnimation()
	{
		if (this.animateAutomatically)
		{
			this.chosenAtLeastOnce = true;
			if (this.animations.Length <= 0)
			{
				UnityEngine.Debug.LogError("NO ANIMATIONS FOR ANIMATED TEXTURE, Numb Skulls!");
				base.gameObject.SetActive(false);
			}
			else
			{
				if (this.delayBeforeComplicatedChoices > 0f)
				{
					this.animationIndex = 0;
				}
				else
				{
					this.animationIndex = this.GetAnimationIndex();
				}
				if (this.animationIndex < this.animations.Length)
				{
					this.currentFrameRow = this.animations[this.animationIndex].startFrameRow;
					this.currentFrameCollumn = this.animations[this.animationIndex].startFrameCollumn;
					this.currentFrameRate = this.animations[this.animationIndex].frameRate;
					this.endFrameCollumn = this.animations[this.animationIndex].endFrameCollumn;
					this.loopsLeft = this.animations[this.animationIndex].loops;
					if (this.animations[this.animationIndex].soundEffect != null && this.sound != null)
					{
						this.sound.PlaySoundEffectAt(this.animations[this.animationIndex].soundEffect, this.animations[this.animationIndex].soundEffectVolume, base.transform.position);
					}
				}
				else if (this.stopOnLastFrame)
				{
					base.enabled = false;
				}
				else
				{
					base.gameObject.SetActive(false);
				}
			}
		}
		else
		{
			this.animate = false;
			this.currentFrameCollumn--;
		}
	}

	protected int GetAnimationIndex()
	{
		if (!this.playAnimationsOnceThrough)
		{
			float num = 0f;
			for (int i = 0; i < this.animations.Length; i++)
			{
				num += this.animations[i].chanceToChoose;
			}
			float num2 = UnityEngine.Random.value * num;
			for (int j = 0; j < this.animations.Length; j++)
			{
				if (num2 < this.animations[j].chanceToChoose)
				{
					return j;
				}
				num2 -= this.animations[j].chanceToChoose;
			}
			return 0;
		}
		this.animationIndex++;
		return this.animationIndex;
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		this.delayBeforeComplicatedChoices -= deltaTime;
		if (this.animate)
		{
			this.counter += deltaTime;
			int num = Mathf.FloorToInt(this.counter / this.currentFrameRate);
			for (int i = 0; i < num; i++)
			{
				this.counter -= this.currentFrameRate;
				this.currentFrameCollumn++;
				if (this.currentFrameCollumn > this.endFrameCollumn)
				{
					if (this.loopsLeft > 0)
					{
						this.loopsLeft--;
						this.currentFrameCollumn = this.animations[this.animationIndex].startFrameCollumn;
					}
					else
					{
						num = 0;
						this.ChooseNewAnimation();
					}
				}
				this.sprite.SetLowerLeftPixel(new Vector2(this.lowerLeftPixel.x + (float)this.currentFrameCollumn * this.pixelDimensions.x, this.lowerLeftPixel.y + (float)this.currentFrameRow * this.pixelDimensions.y));
			}
		}
	}

	protected SpriteSM sprite;

	protected float counter;

	protected float currentFrameRate = 0.0334f;

	protected Vector2 lowerLeftPixel;

	protected Vector2 pixelDimensions;

	protected int currentFrameCollumn;

	protected int currentFrameRow;

	protected int endFrameCollumn = 4;

	protected int loopsLeft;

	protected int animationIndex;

	protected Sound sound;

	public float delayBeforeComplicatedChoices = 1f;

	protected bool chosenAtLeastOnce;

	public bool playAnimationsOnceThrough;

	public bool stopOnLastFrame;

	public AnimatedTextureFrames[] animations;

	public bool animateAutomatically = true;

	protected bool animate = true;
}
