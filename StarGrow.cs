// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class StarGrow : MonoBehaviour
{
	private void Awake()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		this.z = base.transform.position.z;
		this.counter = 0.2f;
		base.GetComponent<Renderer>().enabled = false;
	}

	private void Start()
	{
		this.sprite.SetColor(new Color(1f, 1f, 1f, 0f));
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
			base.GetComponent<Renderer>().enabled = true;
			this.counter += num * 2f;
			this.alpha -= num * 3f;
			float r = 0.8f + Mathf.Sin(this.counter * 5f - Time.time * 3f) * 0.8f;
			float g = 0.8f + Mathf.Sin(this.counter * 5f - Time.time * 3f + 1.7f) * 0.8f;
			float b = 0.8f + Mathf.Sin(this.counter * 5f - Time.time * 3f + 4.23f) * 0.8f;
			this.sprite.SetColor(new Color(r, g, b, Mathf.Clamp(this.alpha * 2f, 0f, 1f)));
			base.transform.localScale = Vector3.one * Mathf.Clamp(Mathf.Sqrt(this.counter) * 3f, 0f, 2.5f);
			base.transform.localPosition = new Vector3(0f, 36f, this.z + this.counter * 0.1f);
			if (this.alpha < 0f)
			{
				this.counter = 0.2f;
				this.alpha = 3f;
			}
		}
	}

	protected SpriteSM sprite;

	private float counter;

	private float alpha = 3f;

	public float delay;

	private float z;
}
