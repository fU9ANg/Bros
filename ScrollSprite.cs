// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ScrollSprite : MonoBehaviour
{
	private void Start()
	{
		this.sprite = base.GetComponent<SpriteSM>();
	}

	private void Update()
	{
		Vector2 lowerLeftPixel = this.sprite.lowerLeftPixel;
		lowerLeftPixel.x += Time.deltaTime * 10f;
		this.sprite.SetLowerLeftPixel(lowerLeftPixel);
		this.sprite.UpdateUVs();
	}

	private SpriteSM sprite;
}
