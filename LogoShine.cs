// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class LogoShine : MonoBehaviour
{
	private void Start()
	{
		this.material = base.GetComponent<Renderer>().material;
		this.material.SetTextureOffset("_MainTex", new Vector2(1f, 0f));
	}

	private void Update()
	{
		this.counter += Time.deltaTime;
		if (this.counter > 2f)
		{
			if (this.counter < 3f)
			{
				float num = this.counter - 2f;
				this.material.SetTextureOffset("_MainTex", new Vector2(1f - num, 0f));
			}
			else
			{
				this.material.SetTextureOffset("_MainTex", new Vector2(1f, 0f));
				if (this.counter > 5f)
				{
					this.counter = 0f - UnityEngine.Random.value * 3f;
				}
			}
		}
	}

	protected Material material;

	protected float counter;
}
