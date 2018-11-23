// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AnimatedTextureUltra : MonoBehaviour
{
	private void Awake()
	{
		this.sprite = base.gameObject.GetComponent<SpriteSM>();
		this.pixelWidth = this.sprite.pixelDimensions.x;
		this.pixelHeight = this.sprite.pixelDimensions.y;
		this.lowerLeftPixel = this.sprite.lowerLeftPixel;
	}

	public void PlayAnimation(string animationName)
	{
		UnityEngine.Debug.Log("Play Animation! " + animationName);
		bool flag = false;
		for (int i = 0; i < this.animations.Length; i++)
		{
			if (this.animations[i].name.ToLower() == animationName.ToLower())
			{
				this.currentAnimation = this.animations[i];
				flag = true;
			}
		}
		if (flag && this.currentAnimation.frames.Length > 0)
		{
			this.frame = 0;
			this.counter = 0f;
			this.animating = true;
			this.frameRate = this.currentAnimation.frames[0].frameRate;
			this.sprite.SetLowerLeftPixel(new Vector2(this.lowerLeftPixel.x + (float)this.currentAnimation.frames[0].collumn * this.pixelWidth, this.lowerLeftPixel.y + (float)this.currentAnimation.frames[0].row * this.pixelHeight));
		}
		else
		{
			UnityEngine.Debug.LogError("Animation Name " + animationName + " Not Found");
		}
	}

	private void Update()
	{
		if (this.animating)
		{
			float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
			this.counter += num;
			if (this.counter >= this.frameRate)
			{
				this.counter -= this.frameRate;
				this.frame++;
				if (this.frame >= this.currentAnimation.frames.Length)
				{
					this.animating = false;
				}
				else
				{
					this.frameRate = this.currentAnimation.frames[this.frame].frameRate;
					this.sprite.SetLowerLeftPixel(new Vector2(this.lowerLeftPixel.x + (float)this.currentAnimation.frames[this.frame].collumn * this.pixelWidth, this.lowerLeftPixel.y + (float)this.currentAnimation.frames[this.frame].row * this.pixelHeight));
				}
			}
		}
	}

	public AnimatedTextureAnimation[] animations;

	protected AnimatedTextureAnimation currentAnimation;

	protected float counter;

	protected float frameRate = 0.0334f;

	protected int frame;

	protected bool animating;

	protected SpriteSM sprite;

	protected float pixelHeight = 10f;

	protected float pixelWidth = 10f;

	protected Vector2 lowerLeftPixel;
}
