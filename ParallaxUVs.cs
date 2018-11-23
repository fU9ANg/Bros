// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ParallaxUVs : MonoBehaviour
{
	private void Start()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		this.spriteStartLeft = this.sprite.lowerLeftPixel.x;
	}

	private void Update()
	{
		this.sprite.SetLowerLeftPixel(new Vector2(this.spriteStartLeft + Mathf.Repeat(200f + (base.transform.position.x + SortOfFollow.instance.transform.position.x) * this.parallaxM, this.maxUVDist), this.sprite.lowerLeftPixel.y));
	}

	public float parallaxM = 0.8f;

	public float maxUVDist = 32f;

	protected float spriteStartLeft;

	protected SpriteSM sprite;
}
