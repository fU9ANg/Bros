// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ColouredLogo : MonoBehaviour
{
	private void Start()
	{
		base.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 1f, this.alpha));
		this.alpha = 0.5f;
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
			this.alpha += num * 4f;
			this.colourM = Mathf.Clamp(this.colourM + num, 0f, 1f);
			float num2 = 1f - this.colourM;
			this.counter += num + this.counter * 0.7f * num;
			float r = 1f * num2 + 0.8f + Mathf.Sin(this.counter * 5f - Time.time * 1.5f) * 0.8f * this.colourM;
			float g = 1f * num2 + 0.8f + Mathf.Sin(this.counter * 5f - Time.time * 1.5f + 1.7f) * 0.8f * this.colourM;
			float b = 1f * num2 + 0.8f + Mathf.Sin(this.counter * 5f - Time.time * 1.5f + 4.23f) * 0.8f * this.colourM;
			base.GetComponent<Renderer>().material.SetColor("_Color", new Color(r, g, b, this.alpha));
		}
	}

	private float delay = 1f;

	protected float colourM;

	protected float alpha;

	private float counter;
}
