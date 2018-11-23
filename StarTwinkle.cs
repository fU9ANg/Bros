// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class StarTwinkle : MonoBehaviour
{
	private void Start()
	{
		this.sprite = base.gameObject.GetComponent<SpriteSM>();
		this.counter = UnityEngine.Random.value * 5f - 5.1f;
		this.baseScale = base.transform.localScale.x;
	}

	private void Update()
	{
		this.counter += Time.deltaTime * 1.5f;
		if (this.counter >= 0f)
		{
			if (this.counter < 1f)
			{
				float num = this.counter;
				base.transform.localScale = Vector3.one * Mathf.Sqrt(Mathf.Sqrt(num)) * 1f * this.baseScale;
				if (this.counter < 0.5f)
				{
					this.sprite.SetColor(new Color(1f, 1f, 1f, Mathf.Clamp01(num * 5f)));
				}
				else
				{
					this.sprite.SetColor(new Color(1f, 1f, 1f, Mathf.Clamp01(5f - num * 5f)));
				}
				base.GetComponent<Renderer>().enabled = true;
				base.transform.Rotate(0f, 0f, -(10f + 200f * num) * Time.deltaTime, Space.Self);
			}
			else
			{
				this.counter = UnityEngine.Random.value * 2f - 5f;
				base.GetComponent<Renderer>().enabled = false;
			}
		}
		else
		{
			base.GetComponent<Renderer>().enabled = false;
		}
	}

	protected float counter;

	protected SpriteSM sprite;

	protected float baseScale = 1f;
}
