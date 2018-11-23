// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FlickerFader : MonoBehaviour
{
	private void Awake()
	{
		this.index = this.startIndex;
		base.GetComponent<Renderer>().material.mainTexture = this.flickerTextures[this.index];
	}

	private void Start()
	{
		this.index = this.startIndex;
	}

	public void SetVelocity(Vector3 velocity)
	{
		this.velocity = velocity;
	}

	public void Restart()
	{
		this.index = this.startIndex;
		base.GetComponent<Renderer>().material.mainTexture = this.flickerTextures[this.index];
		base.gameObject.SetActive(true);
		this.counter = 0f;
	}

	public void Delay(float d)
	{
		this.delay = d;
		base.GetComponent<Renderer>().enabled = false;
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.033f);
		if (this.delay > 0f)
		{
			this.delay -= num;
			if (this.delay <= 0f)
			{
				base.GetComponent<Renderer>().enabled = true;
			}
		}
		else
		{
			base.transform.position += this.velocity * num;
			this.velocity *= 1f - num * 15f;
			this.counter += num;
			if (this.counter > this.frameRate)
			{
				this.counter -= this.frameRate;
				this.index++;
				if (this.index < this.flickerTextures.Length)
				{
					base.GetComponent<Renderer>().material.mainTexture = this.flickerTextures[this.index];
				}
				else
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}
	}

	protected float counter;

	public float frameRate = 0.0667f;

	public Texture[] flickerTextures;

	private int index;

	public int startIndex;

	[HideInInspector]
	public float delay;

	protected Vector3 velocity;
}
