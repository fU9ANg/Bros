// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[Serializable]
public class UVAnimation_Multi
{
	public UVAnimation_Multi()
	{
		if (this.clips == null)
		{
			this.clips = new UVAnimation_Auto[0];
		}
	}

	public UVAnimation_Auto GetCurrentClip()
	{
		return this.clips[this.curClip];
	}

	public UVAnimation_Auto[] BuildUVAnim(SpriteSM s)
	{
		this.i = 0;
		while (this.i < this.clips.Length)
		{
			this.clips[this.i].BuildUVAnim(s);
			this.i++;
		}
		this.CalcDuration();
		return this.clips;
	}

	public bool GetNextFrame(ref Rect uv)
	{
		if (this.clips.Length < 1)
		{
			return false;
		}
		this.ret = this.clips[this.curClip].GetNextFrame(ref uv);
		if (!this.ret)
		{
			if (this.curClip + this.stepDir >= this.clips.Length || this.curClip + this.stepDir < 0)
			{
				if (this.stepDir > 0 && this.loopReverse)
				{
					this.stepDir = -1;
					this.curClip += this.stepDir;
					this.curClip = Mathf.Clamp(this.curClip, 0, this.clips.Length - 1);
					this.clips[this.curClip].Reset();
					this.clips[this.curClip].PlayInReverse();
				}
				else
				{
					if (this.numLoops + 1 > this.loopCycles && this.loopCycles != -1)
					{
						return false;
					}
					this.numLoops++;
					if (this.loopReverse)
					{
						this.stepDir *= -1;
						this.curClip += this.stepDir;
						this.curClip = Mathf.Clamp(this.curClip, 0, this.clips.Length - 1);
						this.clips[this.curClip].Reset();
						if (this.stepDir < 0)
						{
							this.clips[this.curClip].PlayInReverse();
						}
					}
					else
					{
						this.curClip = 0;
						this.clips[this.curClip].Reset();
					}
				}
			}
			else
			{
				this.curClip += this.stepDir;
				this.clips[this.curClip].Reset();
				if (this.stepDir < 0)
				{
					this.clips[this.curClip].PlayInReverse();
				}
			}
			return true;
		}
		return true;
	}

	public void AppendAnim(int index, Rect[] anim)
	{
		if (index >= this.clips.Length)
		{
			return;
		}
		this.clips[index].AppendAnim(anim);
		this.CalcDuration();
	}

	public void AppendClip(UVAnimation clip)
	{
		UVAnimation[] array = this.clips;
		this.clips = new UVAnimation_Auto[this.clips.Length + 1];
		array.CopyTo(this.clips, 0);
		this.clips[this.clips.Length - 1] = (UVAnimation_Auto)clip;
		this.CalcDuration();
	}

	public void PlayInReverse()
	{
		this.i = 0;
		while (this.i < this.clips.Length)
		{
			this.clips[this.i].PlayInReverse();
			this.i++;
		}
		this.stepDir = -1;
		this.curClip = this.clips.Length - 1;
	}

	public void SetAnim(int index, Rect[] frames)
	{
		if (index >= this.clips.Length)
		{
			return;
		}
		this.clips[index].SetAnim(frames);
		this.CalcDuration();
	}

	public void Reset()
	{
		this.curClip = 0;
		this.stepDir = 1;
		this.numLoops = 0;
		this.i = 0;
		while (this.i < this.clips.Length)
		{
			this.clips[this.i].Reset();
			this.i++;
		}
	}

	public void SetPosition(float pos)
	{
		pos = Mathf.Clamp01(pos);
		if (this.loopCycles < 1)
		{
			this.SetAnimPosition(pos);
			return;
		}
		float num = 1f / (float)this.loopCycles;
		this.numLoops = Mathf.FloorToInt(pos / num);
		float num2 = pos - (float)this.numLoops * num;
		this.SetAnimPosition(num2 / num);
	}

	public void SetAnimPosition(float pos)
	{
		int num = 0;
		float num2 = pos;
		for (int i = 0; i < this.clips.Length; i++)
		{
			num += this.clips[i].GetFramesDisplayed();
		}
		if (this.loopReverse)
		{
			if (pos < 0.5f)
			{
				this.stepDir = 1;
				num2 *= 2f;
				for (int j = 0; j < this.clips.Length; j++)
				{
					float num3 = (float)(this.clips[j].GetFramesDisplayed() / num);
					if (num2 <= num3)
					{
						this.curClip = j;
						this.clips[this.curClip].SetPosition(num2 / num3);
						return;
					}
					num2 -= num3;
				}
			}
			else
			{
				this.stepDir = -1;
				num2 = (num2 - 0.5f) / 0.5f;
				for (int k = this.clips.Length - 1; k >= 0; k--)
				{
					float num3 = (float)(this.clips[k].GetFramesDisplayed() / num);
					if (num2 <= num3)
					{
						this.curClip = k;
						this.clips[this.curClip].SetPosition(1f - num2 / num3);
						this.clips[this.curClip].SetStepDir(-1);
						return;
					}
					num2 -= num3;
				}
			}
		}
		else
		{
			for (int l = 0; l < this.clips.Length; l++)
			{
				float num3 = (float)(this.clips[l].GetFramesDisplayed() / num);
				if (num2 <= num3)
				{
					this.curClip = l;
					this.clips[this.curClip].SetPosition(num2 / num3);
					return;
				}
				num2 -= num3;
			}
		}
	}

	protected void CalcDuration()
	{
		if (this.loopCycles < 0)
		{
			this.duration = -1f;
			return;
		}
		this.duration = 0f;
		for (int i = 0; i < this.clips.Length; i++)
		{
			this.duration += this.clips[i].GetDuration();
		}
		if (this.loopReverse)
		{
			this.duration *= 2f;
		}
		this.duration += (float)this.loopCycles * this.duration;
	}

	public float GetDuration()
	{
		return this.duration;
	}

	public int GetFrameCount()
	{
		int num = 0;
		for (int i = 0; i < this.clips.Length; i++)
		{
			num += this.clips[i].GetFramesDisplayed();
		}
		return num;
	}

	public string name;

	public int loopCycles;

	public bool loopReverse;

	public float framerate = 15f;

	public UVAnimation.ANIM_END_ACTION onAnimEnd;

	public UVAnimation_Auto[] clips;

	protected int curClip;

	protected int stepDir = 1;

	protected int numLoops;

	protected float duration;

	protected bool ret;

	protected int i;
}
