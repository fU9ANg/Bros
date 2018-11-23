// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Flicker : MonoBehaviour
{
	private void Start()
	{
		this.index = UnityEngine.Random.Range(0, this.flickerTextures.Length);
	}

	private void Update()
	{
		if (Time.timeScale > 0f)
		{
			this.t = Mathf.Clamp(Time.deltaTime / Time.timeScale, 0f, 0.033f);
		}
		else
		{
			this.t = 0.02f;
		}
		this.counter += this.t;
		if (this.counter > this.frameRate)
		{
			this.counter -= this.frameRate;
			this.index++;
			base.GetComponent<Renderer>().material.mainTexture = this.flickerTextures[this.index % this.flickerTextures.Length];
		}
	}

	protected float counter;

	public float frameRate = 0.0667f;

	public Texture[] flickerTextures;

	private int index;

	private float t;
}
