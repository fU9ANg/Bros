// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[Serializable]
public class UVAnimation
{
	public UVAnimation()
	{
		this.frames = new Rect[0];
	}

	public void Reset()
	{
		this.curFrame = -1;
		this.stepDir = 1;
		this.numLoops = 0;
		this.playInReverse = false;
	}

	public void PlayInReverse()
	{
		this.stepDir = -1;
		this.curFrame = this.frames.Length;
		this.numLoops = 0;
		this.playInReverse = true;
	}

	public void SetStepDir(int dir)
	{
		if (dir < 0)
		{
			this.stepDir = -1;
			this.playInReverse = true;
		}
		else
		{
			this.stepDir = 1;
		}
	}

	public bool GetNextFrame(ref Rect uv)
	{
		if (this.frames.Length < 1)
		{
			return false;
		}
		if (this.curFrame + this.stepDir >= this.frames.Length || this.curFrame + this.stepDir < 0)
		{
			if (this.stepDir > 0 && this.loopReverse)
			{
				this.stepDir = -1;
				this.curFrame += this.stepDir;
				this.curFrame = Mathf.Clamp(this.curFrame, 0, this.frames.Length - 1);
				uv = this.frames[this.curFrame];
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
					this.curFrame += this.stepDir;
					this.curFrame = Mathf.Clamp(this.curFrame, 0, this.frames.Length - 1);
				}
				else if (this.playInReverse)
				{
					this.curFrame = this.frames.Length - 1;
				}
				else
				{
					this.curFrame = 0;
				}
				uv = this.frames[this.curFrame];
			}
		}
		else
		{
			this.curFrame += this.stepDir;
			uv = this.frames[this.curFrame];
		}
		return true;
	}

	public Rect[] BuildUVAnim(Vector2 start, Vector2 cellSize, int cols, int rows, int totalCells)
	{
		int num = 0;
		this.frames = new Rect[totalCells];
		this.frames[0].x = start.x;
		this.frames[0].y = start.y;
		this.frames[0].xMax = start.x + cellSize.x;
		this.frames[0].yMax = start.y + cellSize.y;
		for (int i = 0; i < rows; i++)
		{
			int num2 = 0;
			while (num2 < cols && num < totalCells)
			{
				this.frames[num].x = start.x + cellSize.x * (float)num2;
				this.frames[num].y = start.y - cellSize.y * (float)i;
				this.frames[num].xMax = this.frames[num].x + cellSize.x;
				this.frames[num].yMax = this.frames[num].y + cellSize.y;
				num++;
				num2++;
			}
		}
		this.CalcLength();
		return this.frames;
	}

	public void SetAnim(Rect[] anim)
	{
		this.frames = anim;
		this.CalcLength();
	}

	public void AppendAnim(Rect[] anim)
	{
		Rect[] array = this.frames;
		this.frames = new Rect[this.frames.Length + anim.Length];
		array.CopyTo(this.frames, 0);
		anim.CopyTo(this.frames, array.Length);
		this.CalcLength();
	}

	public void SetCurrentFrame(int f)
	{
		if (f < 0 || f >= this.frames.Length)
		{
			return;
		}
		this.curFrame = f;
	}

	public void SetPosition(float pos)
	{
		pos = Mathf.Clamp01(pos);
		if (this.loopCycles < 1)
		{
			this.SetClipPosition(pos);
			return;
		}
		float num = 1f / (float)this.loopCycles;
		this.numLoops = Mathf.FloorToInt(pos / num);
		float num2 = pos - (float)this.numLoops * num;
		float num3 = num2 / num;
		if (this.loopReverse)
		{
			if (num3 < 0.5f)
			{
				this.curFrame = (int)(((float)this.frames.Length - 1f) * (num3 / 0.5f));
				this.stepDir = 1;
			}
			else
			{
				this.curFrame = this.frames.Length - 1 - (int)(((float)this.frames.Length - 1f) * ((num3 - 0.5f) / 0.5f));
				this.stepDir = -1;
			}
		}
		else
		{
			this.curFrame = (int)(((float)this.frames.Length - 1f) * num3);
		}
	}

	public void SetClipPosition(float pos)
	{
		this.curFrame = (int)(((float)this.frames.Length - 1f) * pos);
	}

	protected void CalcLength()
	{
		this.length = 1f / this.framerate * (float)this.frames.Length;
	}

	public float GetLength()
	{
		return this.length;
	}

	public float GetDuration()
	{
		if (this.loopCycles < 0)
		{
			return -1f;
		}
		float num = this.GetLength();
		if (this.loopReverse)
		{
			num *= 2f;
		}
		return num + (float)this.loopCycles * num;
	}

	public int GetFrameCount()
	{
		return this.frames.Length;
	}

	public int GetFramesDisplayed()
	{
		if (this.loopCycles == -1)
		{
			return -1;
		}
		int num = this.frames.Length + this.frames.Length * this.loopCycles;
		if (this.loopReverse)
		{
			num *= 2;
		}
		return num;
	}

	protected Rect[] frames;

	protected int curFrame = -1;

	protected int stepDir = 1;

	protected int numLoops;

	protected bool playInReverse;

	protected float length;

	public string name;

	public int loopCycles;

	public bool loopReverse;

	[HideInInspector]
	public float framerate = 15f;

	[HideInInspector]
	public UVAnimation.ANIM_END_ACTION onAnimEnd;

	public enum ANIM_END_ACTION
	{
		Do_Nothing,
		Revert_To_Static,
		Play_Default_Anim
	}
}
