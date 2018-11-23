// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class LightningController : MonoBehaviour
{
	private void Start()
	{
		GameObject gameObject = SortOfFollow.GetInstance().gameObject;
		if (gameObject != null)
		{
			this.contrastEnhance = gameObject.GetComponent<ContrastEnhance>();
			this.contrastEnhance.enabled = false;
		}
		else
		{
			UnityEngine.Debug.LogError("No Sort of follow");
		}
		this.skyMaterial.mainTexture = this.skyTexture;
	}

	private void Update()
	{
		if (!this.lightningOn)
		{
			this.lightningCounter += Time.deltaTime;
			if (this.lightningCounter > 2f)
			{
				if (UnityEngine.Random.value < 0.5f)
				{
					this.lightningCounter = 1.4f + UnityEngine.Random.value * 0.5f;
				}
				else
				{
					this.lightningCounter = -4f + UnityEngine.Random.value * 3.9f;
				}
				this.lightningOn = true;
				this.lightningTime = 0f;
				this.skyMaterial.mainTexture = this.brightSkyTexture;
				this.contrastEnhance.enabled = true;
			}
		}
		else
		{
			this.lightningTime += Time.deltaTime;
			if (this.lightningTime >= 0.04f)
			{
				this.lightningOn = false;
				this.skyMaterial.mainTexture = this.skyTexture;
				this.contrastEnhance.enabled = false;
			}
		}
	}

	public Material skyMaterial;

	public Texture skyTexture;

	public Texture brightSkyTexture;

	protected float lightningCounter;

	protected float lightningTime;

	protected bool lightningOn;

	protected ContrastEnhance contrastEnhance;
}
