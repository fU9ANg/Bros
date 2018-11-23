// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FullScreenFlashEffect : MonoBehaviour
{
	private void Awake()
	{
		FullScreenFlashEffect.instance = this;
	}

	private void Start()
	{
		this.colorEffect = base.GetComponent<AmplifyColorEffect>();
		this.colorEffect.enabled = false;
		if (this.colorMaskCamera != null)
		{
			this.colorMaskRenderTexture = this.colorMaskCamera.targetTexture;
		}
	}

	public static void FlashHot(float m, Vector3 pos)
	{
		if (ColorShiftController.CanFlashHot())
		{
			Vector3 vector = FullScreenFlashEffect.instance.transform.position - pos;
			m *= 1f - Mathf.Clamp(Mathf.Max(Mathf.Abs(vector.x) - FullScreenFlashEffect.instance.minRange, Mathf.Abs(vector.y) - FullScreenFlashEffect.instance.minRange), 0f, 10000f) / (FullScreenFlashEffect.instance.maxRange - FullScreenFlashEffect.instance.minRange);
			if (m > 0f)
			{
				FullScreenFlashEffect.instance.fadeTime = 0.2f;
				FullScreenFlashEffect.instance.colorM = m;
				FullScreenFlashEffect.instance.colorEffect.LutBlendTexture = FullScreenFlashEffect.instance.hotFlashTexture;
				FullScreenFlashEffect.instance.colorEffect.MaskTexture = null;
			}
		}
	}

	public static void FlashLightning(float m)
	{
		if (ColorShiftController.CanFlashHot())
		{
			FullScreenFlashEffect.instance.fadeTime = 0.1f;
			FullScreenFlashEffect.instance.colorM = m;
			FullScreenFlashEffect.instance.colorEffect.LutBlendTexture = FullScreenFlashEffect.instance.lightningGlowTexture;
			FullScreenFlashEffect.instance.colorEffect.MaskTexture = null;
		}
	}

	public static void StartMaskedRedGlow(float m)
	{
		FullScreenFlashEffect.instance.redGlowM = m;
		FullScreenFlashEffect.instance.colorEffect.LutBlendTexture = FullScreenFlashEffect.instance.redGlowTexture;
		FullScreenFlashEffect.instance.colorEffect.BlendAmount = m;
		FullScreenFlashEffect.instance.colorEffect.MaskTexture = FullScreenFlashEffect.instance.colorMaskRenderTexture;
	}

	public static void Clear()
	{
		FullScreenFlashEffect.instance.colorM = 0f;
		FullScreenFlashEffect.instance.colorEffect.BlendAmount = FullScreenFlashEffect.instance.colorM;
		FullScreenFlashEffect.instance.colorEffect.enabled = false;
	}

	private void LateUpdate()
	{
		if (this.colorM > 0f && Time.timeScale > 0f)
		{
			this.colorM -= Time.deltaTime / this.fadeTime / Time.timeScale;
			if (this.colorM > 0f)
			{
				this.colorEffect.enabled = true;
				this.colorEffect.BlendAmount = this.colorM;
			}
			else
			{
				this.colorEffect.enabled = false;
			}
		}
	}

	public Texture2D hotFlashTexture;

	public Texture2D deathFlashTexture;

	public Texture2D redGlowTexture;

	public Texture2D lightningGlowTexture;

	protected Texture colorMaskRenderTexture;

	public Camera colorMaskCamera;

	protected AmplifyColorEffect colorEffect;

	protected float colorM;

	protected float redGlowM;

	protected static FullScreenFlashEffect instance;

	public float fadeTime = 0.2f;

	public float maxRange = 256f;

	public float minRange = 150f;
}
