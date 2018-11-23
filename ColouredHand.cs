// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ColouredHand : MonoBehaviour
{
	private void Awake()
	{
		this.sprite = base.GetComponent<SpriteSM>();
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
			this.colourM = Mathf.Clamp(this.colourM + num * 0.5f, 0f, 1f);
			float num2 = 1f - this.colourM;
			this.counter += num + this.counter * 0.7f * num;
			float r = 1f * num2 + 0.8f + Mathf.Sin(this.counter * 5f) * 0.8f * this.colourM;
			float g = 1f * num2 + 0.8f + Mathf.Sin(this.counter * 5f + 1.7f) * 0.8f * this.colourM;
			float b = 1f * num2 + 0.8f + Mathf.Sin(this.counter * 5f + 4.23f) * 0.8f * this.colourM;
			this.sprite.SetColor(new Color(r, g, b, 1f));
		}
	}

	protected float delay = 1f;

	protected SpriteSM sprite;

	protected float colourM;

	private float counter;
}
