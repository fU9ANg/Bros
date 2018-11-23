// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SunController : MonoBehaviour
{
	private void Awake()
	{
		SunController.instance = this;
	}

	private void Start()
	{
		this.sunSprite = base.GetComponent<SpriteSM>();
	}

	public static void GoAway(bool instant)
	{
		if (SunController.instance != null)
		{
			SunController.instance.targetYOffset = -1000f;
			if (instant)
			{
				SunController.instance.yOffset = SunController.instance.targetYOffset + 1f;
			}
		}
	}

	public static void Appear(bool instant)
	{
		if (SunController.instance != null)
		{
			SunController.instance.targetYOffset = 0f;
			if (instant)
			{
				SunController.instance.yOffset = SunController.instance.targetYOffset - 1f;
			}
		}
	}

	private void Update()
	{
		if (this.targetYOffset < this.yOffset)
		{
			this.yOffset -= this.sunSpeed * Time.deltaTime;
			if (this.yOffset <= this.targetYOffset)
			{
				this.yOffset = this.targetYOffset;
			}
			this.sunSprite.SetOffset(new Vector3(this.sunSprite.offset.x, this.yOffset, this.sunSprite.offset.z));
		}
		else if (this.targetYOffset > this.yOffset)
		{
			this.yOffset += this.sunSpeed * Time.deltaTime;
			if (this.yOffset >= this.targetYOffset)
			{
				this.yOffset = this.targetYOffset;
			}
			this.sunSprite.SetOffset(new Vector3(this.sunSprite.offset.x, this.yOffset, this.sunSprite.offset.z));
		}
	}

	protected SpriteSM sunSprite;

	protected float yOffset;

	protected float targetYOffset;

	protected static SunController instance;

	public float sunSpeed = 400f;
}
