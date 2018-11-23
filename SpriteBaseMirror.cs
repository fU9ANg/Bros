// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SpriteBaseMirror
{
	public virtual void Mirror(SpriteBase s)
	{
		this.plane = s.plane;
		this.winding = s.winding;
		this.width = s.width;
		this.height = s.height;
		this.bleedCompensation = s.bleedCompensation;
		this.anchor = s.anchor;
		this.offset = s.offset;
		this.color = s.color;
		this.pixelPerfect = s.pixelPerfect;
		this.autoResize = s.autoResize;
	}

	public virtual bool Validate(SpriteBase s)
	{
		if (s.pixelPerfect)
		{
			s.autoResize = true;
		}
		return true;
	}

	public virtual bool DidChange(SpriteBase s)
	{
		return s.plane != this.plane || s.winding != this.winding || s.width != this.width || s.height != this.height || s.bleedCompensation != this.bleedCompensation || s.anchor != this.anchor || s.offset != this.offset || (s.color.r != this.color.r || s.color.g != this.color.g || s.color.b != this.color.b || s.color.a != this.color.a) || s.pixelPerfect != this.pixelPerfect || s.autoResize != this.autoResize;
	}

	public SpriteBase.SPRITE_PLANE plane;

	public SpriteBase.WINDING_ORDER winding;

	public float width;

	public float height;

	public Vector2 bleedCompensation;

	public SpriteBase.ANCHOR_METHOD anchor;

	public Vector3 offset;

	public Color color;

	public bool pixelPerfect;

	public bool autoResize;
}
