// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class LightManager : MonoBehaviour
{
	private void Awake()
	{
		LightManager.instance = this;
	}

	public static void SetupLightTex()
	{
		if (LightManager.instance != null)
		{
			LightManager.instance.SetLighting();
		}
	}

	private void LateUpdate()
	{
		if (LightManager.UpdateLights || this.forceUpdateLights)
		{
			LightManager.RecalcLighting();
			LightManager.UpdateLights = false;
		}
	}

	private void SetLighting()
	{
		LightManager.lightMap = new float[Map.Width, Map.Height];
		for (int i = 0; i < LightManager.lightMap.GetLength(0); i++)
		{
			for (int j = 0; j < LightManager.lightMap.GetLength(1); j++)
			{
				LightManager.lightMap[i, j] = 0f;
			}
		}
		base.GetComponent<SpriteSM>().width = (float)(Map.Width * 16);
		base.GetComponent<SpriteSM>().height = (float)(Map.Height * 16);
		base.GetComponent<SpriteSM>().SetPixelDimensions(Map.Width, Map.Height);
		base.GetComponent<SpriteSM>().SetSize(base.GetComponent<SpriteSM>().width, base.GetComponent<SpriteSM>().height);
		LightManager.RecalcLighting();
	}

	public static void LightBlock(int i, int j)
	{
		LightManager.lightMap[i, j] += LightManager.lightIntensity;
		LightManager.lightIntensity = Mathf.Clamp(LightManager.lightMap[i, j], 0f, LightManager.instance.maxBleedPerPass);
		if (Map.IsBlockSolid(i, j))
		{
			LightManager.lightIntensity *= LightManager.instance.solidBlockDecayFactor;
		}
		else if (Map.backGroundBlocks[i, j] == null)
		{
			LightManager.lightMap[i, j] += 1f;
		}
		else
		{
			LightManager.lightIntensity *= LightManager.instance.backgroundBlockDecayFactor;
		}
	}

	public static void RecalcLighting()
	{
		for (int i = 0; i < LightManager.lightMap.GetLength(0); i++)
		{
			for (int j = 0; j < LightManager.lightMap.GetLength(1); j++)
			{
				LightManager.lightMap[i, j] = 0f;
			}
		}
		for (int k = 0; k < Map.Width; k++)
		{
			LightManager.lightIntensity = 1f;
			for (int l = Map.Height - 1; l >= 0; l--)
			{
				LightManager.LightBlock(k, l);
			}
		}
		for (int m = 0; m < Map.Height; m++)
		{
			LightManager.lightIntensity = 1f;
			for (int n = Map.Width - 1; n >= 0; n--)
			{
				LightManager.LightBlock(n, m);
			}
		}
		LightManager.lightIntensity = 1f;
		for (int num = 0; num < Map.Height; num++)
		{
			LightManager.lightIntensity = 1f;
			for (int num2 = 0; num2 < Map.Width; num2++)
			{
				LightManager.LightBlock(num2, num);
			}
		}
		for (int num3 = 0; num3 < Map.Width; num3++)
		{
			for (int num4 = Map.Height - 1; num4 >= 0; num4--)
			{
				LightManager.LightBlock(num3, num4);
			}
		}
		for (int num5 = 0; num5 < Map.Height; num5++)
		{
			for (int num6 = 0; num6 < Map.Width; num6++)
			{
				LightManager.LightBlock(num6, num5);
			}
		}
		for (int num7 = 0; num7 < Map.Height; num7++)
		{
			for (int num8 = Map.Width - 1; num8 >= 0; num8--)
			{
				LightManager.LightBlock(num8, num7);
			}
		}
		for (int num9 = 0; num9 < Map.Width; num9++)
		{
			for (int num10 = 0; num10 < Map.Height; num10++)
			{
				LightManager.LightBlock(num9, num10);
			}
		}
		for (int num11 = 0; num11 < Map.Width; num11++)
		{
			for (int num12 = 0; num12 < Map.Height; num12++)
			{
				LightManager.instance.lightTexture.SetPixel(num11, num12, new Color(0f, 0f, 0f, Mathf.Lerp(0f, LightManager.instance.maxDarkAlpha, Mathf.Clamp(1f - LightManager.lightMap[num11, num12] * LightManager.instance.dimFactor, 0f, 1f))));
				if (num11 == Map.Width - 1)
				{
					LightManager.instance.lightTexture.SetPixel(num11 + 1, num12, new Color(0f, 0f, 0f, Mathf.Lerp(0f, LightManager.instance.maxDarkAlpha, Mathf.Clamp(1f - LightManager.lightMap[num11, num12] * LightManager.instance.dimFactor, 0f, 1f))));
				}
				if (num12 == Map.Height - 1)
				{
					LightManager.instance.lightTexture.SetPixel(num11, num12 + 1, new Color(0f, 0f, 0f, Mathf.Lerp(0f, LightManager.instance.maxDarkAlpha, Mathf.Clamp(1f - LightManager.lightMap[num11, num12] * LightManager.instance.dimFactor, 0f, 1f))));
				}
			}
		}
		LightManager.instance.lightTexture.Apply();
		LightManager.instance.GetComponent<SpriteSM>().UpdateUVs();
	}

	public Material lightingMaterial;

	public Texture2D lightTexture;

	public float solidBlockDecayFactor = 0.3f;

	public float backgroundBlockDecayFactor = 0.9f;

	public float maxBleedPerPass = 3f;

	public float dimFactor = 0.25f;

	public float maxDarkAlpha = 0.7f;

	private static LightManager instance;

	public static bool UpdateLights;

	public bool forceUpdateLights;

	public static float[,] lightMap;

	public static bool DoSecondVerticalPass = true;

	private static float lightIntensity = 1f;
}
