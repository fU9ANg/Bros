// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class LightingCamera : MonoBehaviour
{
	private void Awake()
	{
		LightingCamera.lightingIntensity = 0f;
		LightingCamera.instance = this;
	}

	private void Start()
	{
		this.renderTexture = new RenderTexture(Screen.width, Screen.height, 1000, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		this.displacementMaterial.SetTexture("_LightingTex", this.renderTexture);
		this.thisCamera.targetTexture = this.renderTexture;
		this.thisCamera.aspect = Camera.main.aspect;
	}

	public static void SetBackgroundColor(Color color)
	{
		if (LightingCamera.instance != null)
		{
			LightingCamera.instance.thisCamera.backgroundColor = color;
		}
	}

	public void SetIntensity(float m)
	{
		LightingCamera.lightingIntensity = m;
		if (m > 0f)
		{
			this.thisCamera.enabled = true;
			this.effectScript.enabled = true;
			this.displacementMaterial.SetFloat("_Intensity", m);
		}
		else
		{
			this.thisCamera.enabled = false;
			this.effectScript.enabled = false;
		}
	}

	public static float GetLightingMultiplier()
	{
		return LightingCamera.lightingIntensity;
	}

	public static Color GetLightingColor()
	{
		if (LightingCamera.lightingIntensity <= 0f)
		{
			return Color.white;
		}
		return Color.white * (1f - LightingCamera.lightingIntensity) + LightingCamera.instance.thisCamera.backgroundColor * LightingCamera.lightingIntensity;
	}

	public RenderTexture renderTexture;

	public Material displacementMaterial;

	public Camera thisCamera;

	public LightingOverlayEffect effectScript;

	protected static float lightingIntensity;

	protected static LightingCamera instance;
}
