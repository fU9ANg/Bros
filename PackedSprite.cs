// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class PackedSprite : SpriteBase
{
	protected override void Awake()
	{
		base.Awake();
		if (this.textureAnimations == null)
		{
			this.textureAnimations = new TextureAnim[0];
		}
		this.animations = new UVAnimation[this.textureAnimations.Length];
		this.i = 0;
		while (this.i < this.textureAnimations.Length)
		{
			this.animations[this.i] = new UVAnimation();
			this.animations[this.i].SetAnim(this.textureAnimations[this.i].frameUVs);
			this.animations[this.i].name = this.textureAnimations[this.i].name;
			this.animations[this.i].loopCycles = this.textureAnimations[this.i].loopCycles;
			this.animations[this.i].loopReverse = this.textureAnimations[this.i].loopReverse;
			this.animations[this.i].framerate = this.textureAnimations[this.i].framerate;
			this.animations[this.i].onAnimEnd = this.textureAnimations[this.i].onAnimEnd;
			this.i++;
		}
		this.Init();
	}

	protected override void Init()
	{
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
		if (!(s is PackedSprite))
		{
			return;
		}
		base.Copy(s);
		this.staticUVs = ((PackedSprite)s).staticUVs;
		if (this.autoResize || this.pixelPerfect)
		{
			base.CalcSize();
		}
		else
		{
			base.SetSize(s.width, s.height);
		}
		if (((PackedSprite)s).animations.Length > 0)
		{
			this.animations = new UVAnimation[((PackedSprite)s).animations.Length];
			((PackedSprite)s).animations.CopyTo(this.animations, 0);
		}
	}

	public override void CalcUVs()
	{
		this.uvRect = this.staticUVs;
	}

	public void AddAnimation(UVAnimation anim)
	{
		UVAnimation[] array = this.animations;
		this.animations = new UVAnimation[array.Length + 1];
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

	public void PlayAnim(UVAnimation anim)
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

	public void PlayAnimInReverse(UVAnimation anim)
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

	public UVAnimation GetCurAnim()
	{
		return this.curAnim;
	}

	public UVAnimation GetAnim(string name)
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

	public void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (this.mirror == null)
		{
			this.mirror = new PackedSpriteMirror();
			this.mirror.Mirror(this);
		}
		this.mirror.Validate(this);
		if (this.mirror.DidChange(this))
		{
			this.Init();
			this.mirror.Mirror(this);
		}
	}

	public Texture2D staticTexture;

	[HideInInspector]
	public Rect staticUVs;

	public TextureAnim[] textureAnimations;

	[HideInInspector]
	public UVAnimation[] animations;

	protected UVAnimation curAnim;

	private PackedSpriteMirror mirror;
}
