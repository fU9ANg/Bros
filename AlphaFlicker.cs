// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AlphaFlicker : MonoBehaviour
{
	private void Start()
	{
		this.sprite = base.GetComponent<SpriteSM>();
	}

	private void Update()
	{
		this.counter += Time.deltaTime;
		if (this.counter > this.flickerRate)
		{
			this.count++;
			this.counter -= this.flickerRate;
			this.sprite.SetColor(new Color(this.sprite.color.r, this.sprite.color.g, this.sprite.color.b, Mathf.Lerp(this.minAlpha, this.maxAlpha, (float)(this.count % 2))));
		}
	}

	protected float counter;

	public float flickerRate = 0.1f;

	public float maxAlpha = 0.5f;

	public float minAlpha = 0.05f;

	private int count;

	protected SpriteSM sprite;
}
