// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FlashingText : MonoBehaviour
{
	private void Start()
	{
		this.text = base.GetComponent<TextMesh>();
	}

	private void Update()
	{
		this.counter += Time.deltaTime;
		if (this.counter > 0.033f)
		{
			this.counter -= 0.0334f;
			this.count++;
			if (this.count % 2 == 0)
			{
				this.text.color = this.color1;
			}
			else
			{
				this.text.color = this.color2;
			}
		}
	}

	private float counter;

	private int count;

	private TextMesh text;

	public Color color1 = Color.red;

	public Color color2 = Color.yellow;
}
