// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DistortionGrow : MonoBehaviour
{
	private void Awake()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		this.counter = 0.2f;
		base.GetComponent<Renderer>().enabled = false;
		this.size = this.startSize;
	}

	private void Start()
	{
		this.sprite.SetColor(new Color(1f, 1f, 1f, this.alphaM));
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime * 0.5f, 0f, 0.033f);
		if (this.delay > 0f)
		{
			this.delay -= num;
		}
		else
		{
			this.fadeInCounter += num;
			float num2 = Mathf.Clamp01(this.fadeInCounter / this.fadeInTime);
			base.GetComponent<Renderer>().enabled = true;
			this.counter += num;
			this.alpha -= num;
			this.size += num * this.growRate;
			this.growRate *= 1f - num * this.growRateLerpDecrease;
			this.sprite.SetColor(new Color(this.sprite.color.r, this.sprite.color.g, this.sprite.color.b, Mathf.Clamp(this.alpha * 2f, 0f, 1f) * this.alphaM * num2));
			this.sprite.SetSize(this.size, this.size);
			if (this.alpha < 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	protected SpriteSM sprite;

	private float counter;

	public float alpha = 1f;

	public float alphaM = 1f;

	public float delay;

	public float startSize = 24f;

	public float growRate = 32f;

	public float growRateLerpDecrease = 3f;

	protected float size = 24f;

	public float fadeInTime = 0.4f;

	protected float fadeInCounter;
}
