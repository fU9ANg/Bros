// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AnimatedTextureFollow : MonoBehaviour
{
	private void Start()
	{
		this.sprite = base.gameObject.GetComponent<SpriteSM>();
	}

	private void LateUpdate()
	{
		this.sprite.SetLowerLeftPixel(this.followSprite.lowerLeftPixel);
	}

	protected SpriteSM sprite;

	public SpriteSM followSprite;
}
