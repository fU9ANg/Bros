// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ScrollingSprite : MonoBehaviour
{
	private void Start()
	{
		this.sprite = base.GetComponent<SpriteSM>();
	}

	private void Update()
	{
		this.sprite.SetLowerLeftPixel(new Vector2(this.sprite.lowerLeftPixel.x + this.scrollXSpeed * Time.deltaTime, this.sprite.lowerLeftPixel.y));
	}

	protected SpriteSM sprite;

	public float scrollXSpeed = 256f;
}
