// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TextWritesSelf : MonoBehaviour
{
	private void Awake()
	{
		this.texMesh = base.GetComponent<TextMesh>();
	}

	public void Write(string s, float delay)
	{
		this.delay = delay;
		this.Write(s);
	}

	public void Write(string s)
	{
		base.gameObject.SetActive(true);
		if (this.writeString == s)
		{
			return;
		}
		this.writeString = s;
		this.stringLength = 0f;
		if (this.writeString.Length > 120)
		{
			this.writingRate = 0.0334f;
			this.writingInc = 4f;
		}
		else if (this.writeString.Length > 90)
		{
			this.writingRate = 0.0334f;
			this.writingInc = 3f;
		}
		else if (this.writeString.Length > 60)
		{
			this.writingRate = 0.0334f;
			this.writingInc = 2f;
		}
		else if (this.writeString.Length < 30)
		{
			this.writingRate = 0.0334f;
			this.writingInc = 1f;
		}
		else
		{
			this.writingRate = 0.0334f;
			this.writingInc = 1.5f;
		}
		this.currentWritingRate = this.writingRate;
		this.writing = true;
		this.Rewrite();
		this.fading = false;
		this.fadingCount = 0f;
		base.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
	}

	public void Fade()
	{
		this.fading = true;
		if (this.fadingCount < 1f)
		{
			this.fadingCount = 0f;
		}
	}

	protected void Rewrite()
	{
		this.texMesh.text = this.writeString.Substring(0, (int)Mathf.Clamp(this.stringLength, 0f, (float)this.writeString.Length));
	}

	private void Update()
	{
		if (this.writing)
		{
			if (this.delay > 0f)
			{
				this.delay -= Time.deltaTime;
			}
			else
			{
				this.writingCounter += Time.deltaTime;
				if (this.writingCounter > this.currentWritingRate)
				{
					this.writingCounter -= this.currentWritingRate;
					if (this.currentWritingRate > this.writingRate)
					{
						this.currentWritingRate *= 0.8f;
					}
					this.stringLength += this.writingInc;
					this.Rewrite();
					UnityEngine.Debug.Log(string.Concat(new object[]
					{
						"Rewriting + ",
						this.stringLength,
						"   ",
						this.writeString
					}));
					if (this.stringLength >= (float)this.writeString.Length)
					{
						this.writing = false;
					}
				}
			}
		}
		else if (this.fading)
		{
			this.fadingCount += Time.deltaTime * 3f;
			if (this.fadingCount >= 1f)
			{
				this.fadingCount = 1f;
				this.fading = false;
			}
			base.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f - this.fadingCount, 1f - this.fadingCount, 1f - this.fadingCount, 1f - this.fadingCount));
		}
	}

	protected TextMesh texMesh;

	protected float stringLength;

	protected string writeString;

	protected bool writing;

	protected float writingCounter;

	public float writingRate = 0.0667f;

	protected float writingInc = 1f;

	protected float fadingCount;

	protected bool fading;

	[HideInInspector]
	public float delay;

	protected float currentWritingRate = 0.1f;
}
