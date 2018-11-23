// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SimpleSpriteWrapper : MonoBehaviour
{
	public int Frame
	{
		get
		{
			return this._frame;
		}
		set
		{
			this._frame = value;
			this.sprite.SetLowerLeftPixel((float)(this._frame * this.spritePixelWidth), (float)(this.spritePixelHeight + this._row * this.spritePixelHeight));
		}
	}

	public int Row
	{
		get
		{
			return this._row;
		}
		set
		{
			this._row = value;
			this.sprite.SetLowerLeftPixel((float)(this._frame * this.spritePixelWidth), (float)(this.spritePixelHeight + this._row * this.spritePixelHeight));
		}
	}

	protected bool RunFrameCounter(float t, float rate)
	{
		this.frameCounter += t;
		if (this.frameCounter > rate)
		{
			this.frameCounter -= rate;
			return true;
		}
		return false;
	}

	public void SetSpriteFrame(int collumn, int row)
	{
		this._frame = collumn;
		this._row = row;
		this.sprite.SetLowerLeftPixel((float)(this._frame * this.spritePixelWidth), (float)(this.spritePixelHeight + this._row * this.spritePixelHeight));
	}

	public void SetOffset(float x, float y, float z)
	{
		this.sprite.SetOffset(new Vector3(x, y, z));
	}

	protected virtual void Start()
	{
	}

	protected virtual void Awake()
	{
		this.sprite = base.GetComponent<SpriteSM>();
	}

	public void RecalcTexture()
	{
		this.sprite.RecalcTexture();
		this.sprite.SetSize(this.sprite.width, this.sprite.height);
		this.sprite.SetLowerLeftPixel(new Vector2(this.sprite.lowerLeftPixel.x, this.sprite.lowerLeftPixel.y));
	}

	protected SpriteSM sprite;

	private int _frame;

	private int _row;

	protected float frameCounter;

	public int spritePixelWidth = 32;

	public int spritePixelHeight = 32;
}
