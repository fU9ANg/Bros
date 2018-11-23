// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class InfoBar : MonoBehaviour
{
	protected void Awake()
	{
		InfoBar.instance = this;
		base.gameObject.SetActive(false);
	}

	protected void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.growing)
		{
			this.growCounter += this.t;
			float num = Mathf.Clamp(this.growCounter / this.tweenTime, 0f, 1f);
			this.bar.SetSize(1000f, 1f + this.barSize * num);
		}
		else if (this.fading)
		{
			this.fadeCounter += this.t;
			float num2 = Mathf.Clamp(this.fadeCounter / this.tweenTime, 0f, 1f);
			this.bar.SetSize(1000f, 1f + this.barSize * (1f - num2));
		}
		else
		{
			this.bar.SetSize(1000f, 1f + this.barSize);
		}
		if (this.growing)
		{
			this.particleCounter += this.t;
			if (this.particleCounter > 0.0667f)
			{
				this.particleCounter -= 0.0667f;
			}
			if (this.growing)
			{
				this.RunParticles(this.t * this.particleSpeedM);
			}
			this.particleSpeedM = Mathf.Lerp(this.particleSpeedM, 1f, this.t * 8f);
		}
		else if (this.fading)
		{
			this.RunParticles(this.t * this.particleSpeedM);
			this.particleSpeedM = Mathf.Lerp(this.particleSpeedM, 1f, this.t * 15f);
		}
		else
		{
			this.RunParticles(this.t * this.particleSpeedM);
			this.particleSpeedM = Mathf.Lerp(this.particleSpeedM, 0.1f, this.t * 15f);
		}
		if (this.textFadingIn)
		{
			this.textCounter += this.t;
			float a = Mathf.Clamp(this.textCounter * 2f / this.tweenTime, 0f, 1f);
			if (this.textCounter > this.tweenTime)
			{
				this.textFadingIn = false;
				this.textCounter = 0f;
				a = 1f;
				this.textMesh.GetComponent<Renderer>().material = this.textMaterialNormal;
			}
			this.textMesh.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, a));
			this.textMesh.transform.localPosition = Vector3.Lerp(this.textMesh.transform.localPosition, Vector3.zero, this.t * 18f);
		}
		else if (this.textFadingOut)
		{
			this.textMesh.transform.localPosition = Vector3.Lerp(this.textMesh.transform.localPosition, Vector3.right * 100f, this.t * 8f);
			this.textCounter += this.t;
			float num3 = Mathf.Clamp(this.textCounter * 2f / this.tweenTime, 0f, 1f);
			this.textMesh.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 1f - num3));
			this.raysOfLight.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 1f - num3));
		}
		else
		{
			this.textMesh.transform.localPosition = Vector3.zero;
		}
		if (this.timeCounter < this.tweenTime)
		{
			this.timeCounter += this.t;
			if (this.timeCounter >= this.tweenTime)
			{
				this.growing = false;
				this.fadeCounter = 0f;
			}
		}
		else if (this.timeCounter < this.tweenTime + this.textTime)
		{
			this.timeCounter += this.t;
			if (this.timeCounter >= this.tweenTime + this.textTime)
			{
				this.fading = true;
				if (this.textTime < 1f)
				{
					this.particleSpeedM = 1f;
				}
				this.growCounter = 0f;
				this.textMesh.GetComponent<Renderer>().material = this.textMaterialItalic;
				this.textFadingOut = true;
				this.textFadingIn = false;
			}
		}
		else if (this.timeCounter < this.tweenTime * 2f + this.textTime)
		{
			this.timeCounter += this.t;
			if (this.timeCounter >= this.tweenTime * 2f + this.textTime)
			{
				this.Disappear();
			}
		}
		if (this.disappearing)
		{
			this.disappearCounter += this.t;
			if (this.disappearCounter > 0.2f)
			{
				this.raysOfLight.ClearParticles();
				base.gameObject.SetActive(false);
			}
		}
	}

	protected void RunParticles(float t)
	{
		this.particles = this.raysOfLight.particles;
		for (int i = 0; i < this.particles.Length; i++)
		{
			Particle[] array = this.particles;
			int num = i;
			array[num].position = array[num].position + this.particles[i].velocity * t;
		}
		this.raysOfLight.particles = this.particles;
	}

	protected void AppearInternal(float time, string text, Color c, float scale)
	{
		base.gameObject.SetActive(true);
		this.textTime = time;
		this.color = c;
		this.textMesh.text = text;
		this.raysOfLight.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 1f));
		this.textMesh.GetComponent<Renderer>().material = this.textMaterialItalic;
		this.textMesh.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0f));
		this.bar.SetColor(this.color);
		float num = 6f + this.textTime * 5f;
		float num2 = (300f + 330f * this.textTime) / num;
		int num3 = 0;
		while ((float)num3 < 1f + this.textTime * 8f)
		{
			this.particleCount++;
			float num4 = 1.5f + UnityEngine.Random.value * 1f;
			Vector3 pos = new Vector3(-60f - this.textTime * 100f - UnityEngine.Random.value * 50f - (10f + 40f * this.textTime) * num4 - num2 * (float)num3, (float)(8 - this.particleCount % 5 * 4) - 1.5f + UnityEngine.Random.value * 3f, 2f);
			this.raysOfLight.Emit(pos, Vector3.right * 600f * num4, 1f, 2f + UnityEngine.Random.value * this.tweenTime * 0.5f, new Color(this.color.r * 1.2f + 0.1f, this.color.g * 1.2f + 0.1f, this.color.b * 1.2f + 0.1f, 0.3f));
			num3++;
		}
		this.growing = true;
		this.fading = false;
		this.textMesh.transform.localScale = Vector3.one * scale;
		this.timeCounter = 0f;
		this.textCounter = 0f;
		this.textFadingIn = true;
		this.textFadingOut = false;
		this.particleSpeedM = 0.5f;
		this.disappearing = false;
		this.tweenTime = Mathf.Min(0.5f, this.textTime / 2f);
		this.textMesh.transform.localPosition = Vector3.right * -100f;
	}

	public static void Appear(float time, string text, Color color, float scale)
	{
		if (InfoBar.instance != null)
		{
			InfoBar.instance.AppearInternal(time, text, color, scale);
		}
	}

	protected void Disappear()
	{
		this.disappearing = true;
		this.disappearCounter = 0.25f;
	}

	protected float growCounter;

	protected float timeCounter;

	protected float fadeCounter;

	protected float textTime = 0.5f;

	protected float tweenTime = 0.5f;

	protected bool growing;

	protected bool fading;

	protected float textCounter;

	protected bool textFadingIn;

	protected bool textFadingOut;

	public Material textMaterialNormal;

	public Material textMaterialItalic;

	protected float particleCounter;

	protected bool disappearing;

	protected float particleSpeedM = 1f;

	protected float disappearCounter;

	public float barSize = 19f;

	protected static InfoBar instance;

	public TextMesh textMesh;

	public SpriteSM bar;

	public ParticleEmitter raysOfLight;

	protected Color color;

	protected int particleCount;

	protected float t = 0.01f;

	protected List<Particle> particlesList;

	protected Particle[] particles;
}
