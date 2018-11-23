// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class EffectsHolderTheme : MonoBehaviour
{
	public static EffectsHolderTheme Instance
	{
		get
		{
			if (EffectsHolderTheme.instance == null)
			{
				UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
			}
			return EffectsHolderTheme.instance;
		}
	}

	protected void Awake()
	{
		EffectsHolderTheme.instance = this;
	}

	public static Color GetRandomWoodColor()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return Color.Lerp(EffectsHolderTheme.instance.woodColor1, EffectsHolderTheme.instance.woodColor2, UnityEngine.Random.value);
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return Color.black;
	}

	public static Color GetRandomDirtColor()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return Color.Lerp(EffectsHolderTheme.instance.dirtColor1, EffectsHolderTheme.instance.dirtColor2, UnityEngine.Random.value);
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return Color.black;
	}

	public static Color GetRandomSandColor()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return Color.Lerp(EffectsHolderTheme.instance.sandColor1, EffectsHolderTheme.instance.sandColor2, UnityEngine.Random.value);
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return Color.black;
	}

	public static ParticleEmitter GetDotsDirtEmitter()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.particlesDirtDots;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetSmallMetalEmitter1()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.particlesMetalSmall1;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetSmallSandEmitter1()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.particlesSandSmall1;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetDotsSandEmitter()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.particlesSandDots;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetSmallDirtEmitter1()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.particlesDirtSmall1;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetSmallDirtEmitter2()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.particlesDirtSmall2;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetSmallDirtEmitter3()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.particlesDirtSmall3;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetMetalEmitter1()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.metalParticles1;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetMetalEmitter2()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.metalParticles2;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetScrapEmitter1()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.scrapParticles1;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetScrapEmitter2()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.scrapParticles2;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetScrapEmitter3()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.scrapParticles3;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetScrapEmitter4()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.scrapParticles4;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetScrapEmitter5()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.scrapParticles5;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetScrapEmitterTiny1()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.scrapParticlesTiny1;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetScrapEmitterTiny2()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.scrapParticlesTiny2;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetWoodEmitter1()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.woodParticles1;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetWoodEmitter2()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.woodParticles2;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetWoodEmitter3()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.woodParticles3;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public static ParticleEmitter GetWoodEmitterDots()
	{
		if (EffectsHolderTheme.instance != null)
		{
			return EffectsHolderTheme.instance.woodParticlesDots;
		}
		UnityEngine.Debug.LogError("Please put EffectsHolderTheme prefab in your scene");
		return null;
	}

	public Color dirtColor1 = new Color(0.5f, 0.4f, 0.25f);

	public Color dirtColor2 = new Color(0.35f, 0.27f, 0.16f);

	public Color woodColor1 = new Color(0.66f, 0.53f, 0.3f);

	public Color woodColor2 = new Color(0.35f, 0.27f, 0.2f);

	public Color sandColor1 = new Color(0.52f, 0.33f, 0.167f);

	public Color sandColor2 = new Color(0.41f, 0.33f, 0.3f);

	public ParticleEmitter particlesAlienEarthDots;

	public ParticleEmitter particlesDirtDots;

	public ParticleEmitter particlesDirtSmall1;

	public ParticleEmitter particlesDirtSmall2;

	public ParticleEmitter particlesDirtSmall3;

	public ParticleEmitter particlesSandDots;

	public ParticleEmitter particlesSandSmall1;

	public ParticleEmitter particlesMetalSmall1;

	public ParticleEmitter woodParticles1;

	public ParticleEmitter woodParticles2;

	public ParticleEmitter woodParticles3;

	public ParticleEmitter woodParticlesDots;

	public ParticleEmitter metalParticles1;

	public ParticleEmitter metalParticles2;

	public ParticleEmitter scrapParticles1;

	public ParticleEmitter scrapParticles2;

	public ParticleEmitter scrapParticles3;

	public ParticleEmitter scrapParticles4;

	public ParticleEmitter scrapParticles5;

	public ParticleEmitter scrapParticlesTiny1;

	public ParticleEmitter scrapParticlesTiny2;

	protected static EffectsHolderTheme instance;
}
