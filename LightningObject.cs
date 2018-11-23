// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class LightningObject : MonoBehaviour
{
	private void Start()
	{
		this.normalTexture = base.GetComponent<Renderer>().material.mainTexture;
		LightningFlashController.RegisterLighntingObject(this);
	}

	public void Flash()
	{
		this.flashCounter = 1f;
		base.GetComponent<Renderer>().material.mainTexture = this.lighntingTexture;
	}

	private void Update()
	{
		if (this.flashCounter > 0f)
		{
			this.flashCounter -= Time.deltaTime * 14f;
			if (this.flashCounter <= 0f)
			{
				base.GetComponent<Renderer>().material.mainTexture = this.normalTexture;
			}
		}
	}

	protected float flashCounter;

	public Texture lighntingTexture;

	protected Texture normalTexture;
}
