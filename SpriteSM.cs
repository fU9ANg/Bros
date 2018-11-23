// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteSM : SpriteBase
{
	protected override void Awake()
	{
		this.createNormals = true;
		base.Awake();
		if (this.animations == null)
		{
			this.animations = new UVAnimation_Multi[0];
		}
		this.i = 0;
		while (this.i < this.animations.Length)
		{
			this.animations[this.i].BuildUVAnim(this);
			this.i++;
		}
		base.Init();
	}

	protected override void Start()
	{
		base.Start();
		if (this.playAnimOnStart && this.defaultAnim < this.animations.Length && Application.isPlaying)
		{
			this.PlayAnim(this.defaultAnim);
		}
	}

	public override void Clear()
	{
		base.Clear();
		if (this.curAnim != null)
		{
			base.PauseAnim();
			this.curAnim = null;
		}
	}

	public override void Copy(SpriteBase s)
	{
		if (!(s is SpriteSM))
		{
			return;
		}
		base.Copy(s);
		this.lowerLeftPixel = ((SpriteSM)s).lowerLeftPixel;
		this.pixelDimensions = ((SpriteSM)s).pixelDimensions;
		this.CalcUVs();
		base.SetBleedCompensation(s.bleedCompensation);
		if (this.autoResize || this.pixelPerfect)
		{
			base.CalcSize();
		}
		else
		{
			base.SetSize(s.width, s.height);
		}
		if (((SpriteSM)s).animations.Length > 0)
		{
			this.animations = new UVAnimation_Multi[((SpriteSM)s).animations.Length];
			((SpriteSM)s).animations.CopyTo(this.animations, 0);
		}
		this.i = 0;
		while (this.i < this.animations.Length)
		{
			this.animations[this.i].BuildUVAnim(this);
			this.i++;
		}
	}

	public override void CalcUVs()
	{
		this.tempUV = base.PixelCoordToUVCoord(this.lowerLeftPixel);
		this.uvRect.x = this.tempUV.x;
		this.uvRect.y = this.tempUV.y;
		this.tempUV = base.PixelSpaceToUVSpace(this.pixelDimensions);
		this.uvRect.xMax = this.uvRect.x + this.tempUV.x;
		this.uvRect.yMax = this.uvRect.y + this.tempUV.y;
	}

	public void AddAnimation(UVAnimation_Multi anim)
	{
		UVAnimation_Multi[] array = this.animations;
		this.animations = new UVAnimation_Multi[array.Length + 1];
		array.CopyTo(this.animations, 0);
		this.animations[this.animations.Length - 1] = anim;
	}

	public override bool StepAnim(float time)
	{
		if (this.curAnim == null)
		{
			return false;
		}
		this.timeSinceLastFrame += Mathf.Max(0f, time);
		this.framesToAdvance = (int)(this.timeSinceLastFrame / this.timeBetweenAnimFrames);
		if (this.framesToAdvance < 1)
		{
			return true;
		}
		while (this.framesToAdvance > 0)
		{
			if (!this.curAnim.GetNextFrame(ref this.uvRect))
			{
				if (this.curAnim.onAnimEnd == UVAnimation.ANIM_END_ACTION.Do_Nothing)
				{
					base.PauseAnim();
					base.SetBleedCompensation();
					if (this.autoResize || this.pixelPerfect)
					{
						base.CalcSize();
					}
				}
				else if (this.curAnim.onAnimEnd == UVAnimation.ANIM_END_ACTION.Revert_To_Static)
				{
					base.RevertToStatic();
				}
				else if (this.curAnim.onAnimEnd == UVAnimation.ANIM_END_ACTION.Play_Default_Anim)
				{
					if (this.animCompleteDelegate != null)
					{
						this.animCompleteDelegate();
					}
					this.PlayAnim(this.defaultAnim);
					return false;
				}
				if (this.animCompleteDelegate != null)
				{
					this.animCompleteDelegate();
				}
				if (!this.animating)
				{
					this.curAnim = null;
				}
				return false;
			}
			this.framesToAdvance--;
			this.timeSinceLastFrame -= this.timeBetweenAnimFrames;
		}
		base.SetBleedCompensation();
		base.UpdateUVs();
		if (this.autoResize || this.pixelPerfect)
		{
			base.CalcSize();
		}
		return true;
	}

	public void PlayAnim(UVAnimation_Multi anim)
	{
		this.curAnim = anim;
		this.curAnim.Reset();
		anim.framerate = Mathf.Max(0.0001f, anim.framerate);
		this.timeBetweenAnimFrames = 1f / anim.framerate;
		this.timeSinceLastFrame = this.timeBetweenAnimFrames;
		if (anim.GetFrameCount() > 1)
		{
			this.StepAnim(0f);
			if (!this.animating)
			{
				base.AddToAnimatedList();
			}
		}
		else
		{
			if (this.animCompleteDelegate != null)
			{
				this.animCompleteDelegate();
			}
			this.StepAnim(0f);
		}
	}

	public void PlayAnim(int index)
	{
		if (index >= this.animations.Length)
		{
			UnityEngine.Debug.LogError("ERROR: Animation index " + index + " is out of bounds!");
			return;
		}
		this.PlayAnim(this.animations[index]);
	}

	public void PlayAnim(string name)
	{
		for (int i = 0; i < this.animations.Length; i++)
		{
			if (this.animations[i].name == name)
			{
				this.PlayAnim(this.animations[i]);
				return;
			}
		}
		UnityEngine.Debug.LogError("ERROR: Animation \"" + name + "\" not found!");
	}

	public void PlayAnimInReverse(UVAnimation_Multi anim)
	{
		this.curAnim = anim;
		this.curAnim.Reset();
		this.curAnim.PlayInReverse();
		anim.framerate = Mathf.Max(0.0001f, anim.framerate);
		this.timeBetweenAnimFrames = 1f / anim.framerate;
		this.timeSinceLastFrame = this.timeBetweenAnimFrames;
		if (anim.GetFrameCount() > 1)
		{
			this.StepAnim(0f);
			if (!this.animating)
			{
				base.AddToAnimatedList();
			}
		}
		else
		{
			if (this.animCompleteDelegate != null)
			{
				this.animCompleteDelegate();
			}
			this.StepAnim(0f);
		}
	}

	public void PlayAnimInReverse(int index)
	{
		if (index >= this.animations.Length)
		{
			UnityEngine.Debug.LogError("ERROR: Animation index " + index + " is out of bounds!");
			return;
		}
		this.PlayAnimInReverse(this.animations[index]);
	}

	public void PlayAnimInReverse(string name)
	{
		for (int i = 0; i < this.animations.Length; i++)
		{
			if (this.animations[i].name == name)
			{
				this.animations[i].PlayInReverse();
				this.PlayAnimInReverse(this.animations[i]);
				return;
			}
		}
		UnityEngine.Debug.LogError("ERROR: Animation \"" + name + "\" not found!");
	}

	public override void StopAnim()
	{
		base.RemoveFromAnimatedList();
		if (this.curAnim != null)
		{
			this.curAnim.Reset();
		}
		base.RevertToStatic();
	}

	public void UnpauseAnim()
	{
		if (this.curAnim == null)
		{
			return;
		}
		base.AddToAnimatedList();
	}

	public UVAnimation_Multi GetCurAnim()
	{
		return this.curAnim;
	}

	public UVAnimation_Multi GetAnim(string name)
	{
		for (int i = 0; i < this.animations.Length; i++)
		{
			if (this.animations[i].name == name)
			{
				return this.animations[i];
			}
		}
		return null;
	}

	public void SetLowerLeftPixel(Vector2 lowerLeft)
	{
		this.lowerLeftPixel = lowerLeft;
		this.tempUV = base.PixelCoordToUVCoord(this.lowerLeftPixel);
		this.uvRect.x = this.tempUV.x;
		this.uvRect.y = this.tempUV.y;
		this.tempUV = base.PixelSpaceToUVSpace(this.pixelDimensions);
		this.uvRect.xMax = this.uvRect.x + this.tempUV.x;
		this.uvRect.yMax = this.uvRect.y + this.tempUV.y;
		base.SetBleedCompensation(this.bleedCompensation);
		if (this.autoResize || this.pixelPerfect)
		{
			base.CalcSize();
		}
	}

	public void SetLowerLeftPixel(float x, float y)
	{
		this.SetLowerLeftPixel(new Vector2(x, y));
	}

	public void SetPixelDimensions(Vector2 size)
	{
		this.pixelDimensions = size;
		this.tempUV = base.PixelSpaceToUVSpace(this.pixelDimensions);
		this.uvRect.xMax = this.uvRect.x + this.tempUV.x;
		this.uvRect.yMax = this.uvRect.y + this.tempUV.y;
		this.uvRect.xMax = this.uvRect.xMax - this.bleedCompensationUV.x * 2f;
		this.uvRect.yMax = this.uvRect.yMax - this.bleedCompensationUV.y * 2f;
		if (this.autoResize || this.pixelPerfect)
		{
			base.CalcSize();
		}
	}

	public void SetPixelDimensions(int x, int y)
	{
		this.SetPixelDimensions(new Vector2((float)x, (float)y));
	}

	public void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (this.mirror == null)
		{
			this.mirror = new SpriteMirror();
			this.mirror.Mirror(this);
		}
		this.mirror.Validate(this);
		if (this.mirror.DidChange(this))
		{
			this.Init();
			this.mirror.Mirror(this);
		}
	}

	public Vector2 lowerLeftPixelTest;

	public Vector2 pixelDimensions;

	public UVAnimation_Multi[] animations;

	protected UVAnimation_Multi curAnim;

	private SpriteMirror mirror;
}
