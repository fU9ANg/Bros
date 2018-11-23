// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CutsceneEffectsController : MonoBehaviour
{
	protected void Awake()
	{
		CutsceneEffectsController.instance = this;
	}

	public static void CreateBloodParticles(float x, float y, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI)
	{
		CutsceneEffectsController.CreateBloodParticles(x, y, 1f, count, offsetXRadius, offsetYRadius, force, xI, yI);
	}

	public static void CreateBloodParticles(float x, float y, float z, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI)
	{
		CutsceneEffectsController.CreateBloodParticlesBig(x, y, z, count / 3, offsetXRadius * 0.1f, offsetYRadius * 0.1f, force, xI, yI);
		CutsceneEffectsController.CreateBloodParticlesSmall(x, y, z, count / 2 + 1, offsetXRadius * 0.1f, offsetYRadius * 0.1f, force, xI, yI);
		CutsceneEffectsController.CreateBloodParticlesDots(x, y, z, count / 2 + 1, offsetXRadius * 0.1f, offsetYRadius * 0.1f, force, xI, yI, 1f);
	}

	public static void CreateBloodParticlesBig(float x, float y, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI)
	{
		CutsceneEffectsController.CreateBloodParticlesBig(x, y, 1f, count, offsetXRadius, offsetYRadius, force, xI, yI);
	}

	public static void CreateBloodParticlesBig(float x, float y, float z, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI)
	{
		for (int i = 0; i < count; i++)
		{
			float num = -1f + UnityEngine.Random.value * 2f;
			float num2 = -1f + UnityEngine.Random.value * 2f;
			float x2 = x + offsetXRadius * num;
			float y2 = y + offsetYRadius * num2;
			CutsceneEffectsController.instance.bloodParticlesBig.Emit(new Vector3(x2, y2, z), new Vector3(xI + force * num, yI + force * num2, 0f), Mathf.Lerp(CutsceneEffectsController.instance.bloodParticlesBig.minSize, CutsceneEffectsController.instance.bloodParticlesBig.maxSize, UnityEngine.Random.value), Mathf.Lerp(CutsceneEffectsController.instance.bloodParticlesBig.minEnergy, CutsceneEffectsController.instance.bloodParticlesBig.maxEnergy, UnityEngine.Random.value), Color.white * (0.8f + UnityEngine.Random.value * 0.2f));
		}
	}

	public static void CreateBloodParticlesSmall(float x, float y, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI)
	{
		CutsceneEffectsController.CreateBloodParticlesSmall(x, y, 1f, count, offsetXRadius, offsetYRadius, force, xI, yI);
	}

	public static void CreateBloodParticlesSmall(float x, float y, float z, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 2f / force;
		bool flag = false;
		for (int i = 0; i < count; i++)
		{
			float x2;
			float y2;
			if (!flag || UnityEngine.Random.value > 0.6f)
			{
				num = -1f + UnityEngine.Random.value * 2f;
				num2 = -1f + UnityEngine.Random.value * 2f;
				x2 = x + offsetXRadius * num;
				y2 = y + offsetYRadius * num2;
				flag = true;
			}
			else
			{
				flag = false;
				if (num > 0.7f)
				{
					num -= num3;
				}
				else
				{
					num += -num3;
				}
				if (num2 > 0f)
				{
					num2 -= num3;
				}
				else
				{
					num2 += -num3;
				}
				x2 = x + offsetXRadius * num;
				y2 = y + offsetYRadius * num2;
			}
			CutsceneEffectsController.instance.bloodParticlesSmall.Emit(new Vector3(x2, y2, z), new Vector3(xI + force * num, yI + force * num2, 0f), Mathf.Lerp(CutsceneEffectsController.instance.bloodParticlesSmall.minSize, CutsceneEffectsController.instance.bloodParticlesSmall.maxSize, UnityEngine.Random.value), Mathf.Lerp(CutsceneEffectsController.instance.bloodParticlesSmall.minEnergy, CutsceneEffectsController.instance.bloodParticlesSmall.maxEnergy, UnityEngine.Random.value), Color.white * (0.8f + UnityEngine.Random.value * 0.2f));
		}
	}

	public static void CreateBloodParticlesDots(float x, float y, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI, float scaleM)
	{
		CutsceneEffectsController.CreateBloodParticlesDots(x, y, 1f, count, offsetXRadius, offsetYRadius, force, xI, yI, scaleM);
	}

	public static void CreateBloodParticlesDots(float x, float y, float z, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI, float scaleM)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 2f / force;
		bool flag = false;
		if (UnityEngine.Random.value > 0.5f)
		{
			count++;
		}
		for (int i = 0; i < count; i++)
		{
			float x2;
			float y2;
			if (!flag || UnityEngine.Random.value > 0.6f)
			{
				num = -1f + UnityEngine.Random.value * 2f;
				num2 = -1f + UnityEngine.Random.value * 2f;
				x2 = x + offsetXRadius * num;
				y2 = y + offsetYRadius * num2;
				flag = true;
			}
			else
			{
				flag = false;
				if (num > 0f)
				{
					num -= num3;
				}
				else
				{
					num += num3;
				}
				if (num2 > 0f)
				{
					num2 -= num3;
				}
				else
				{
					num2 += num3;
				}
				x2 = x + offsetXRadius * num;
				y2 = y + offsetYRadius * num2;
			}
			CutsceneEffectsController.instance.bloodParticlesDots.Emit(new Vector3(x2, y2, z), new Vector3(xI + force * num, yI + force * num2, 0f), Mathf.Lerp(CutsceneEffectsController.instance.bloodParticlesDots.minSize * scaleM, CutsceneEffectsController.instance.bloodParticlesDots.maxSize * scaleM, UnityEngine.Random.value), Mathf.Lerp(CutsceneEffectsController.instance.bloodParticlesDots.minEnergy, CutsceneEffectsController.instance.bloodParticlesDots.maxEnergy, UnityEngine.Random.value), new Color(1f, 1f, 1f, 0.7f + UnityEngine.Random.value * 0.4f));
		}
	}

	public static void CreateBloodParticlesDrip(float x, float y, float offsetXRadius, float offsetYRadius, float scaleM)
	{
		float num = -1f + UnityEngine.Random.value * 2f;
		float num2 = -1f + UnityEngine.Random.value * 2f;
		float x2 = x + offsetXRadius * num;
		float y2 = y + offsetYRadius * num2;
		CutsceneEffectsController.instance.bloodParticlesDrips.Emit(new Vector3(x2, y2, 1f), Vector3.zero, Mathf.Lerp(CutsceneEffectsController.instance.bloodParticlesDrips.minSize * scaleM, CutsceneEffectsController.instance.bloodParticlesDrips.maxSize * scaleM, UnityEngine.Random.value), Mathf.Lerp(CutsceneEffectsController.instance.bloodParticlesDrips.minEnergy, CutsceneEffectsController.instance.bloodParticlesDrips.maxEnergy, UnityEngine.Random.value), Color.white);
	}

	public static void CreateGibs(GibHolder gibs, Material material, float x, float y, float xForce, float yForce, float xI, float yI)
	{
		for (int i = 0; i < gibs.transform.childCount; i++)
		{
			CutsceneEffectsController.CreateGib(gibs.transform.GetChild(i).GetComponent<Gib>(), material, x, y, xForce * (0.8f + UnityEngine.Random.value * 0.4f), yForce * (0.8f + UnityEngine.Random.value * 0.4f), xI, yI, 1);
		}
	}

	public static void CreateGibs(GibHolder gibs, float x, float y, float xForce, float yForce, float xI, float yI)
	{
		for (int i = 0; i < gibs.transform.childCount; i++)
		{
			CutsceneEffectsController.CreateGib(gibs.transform.GetChild(i).GetComponent<Gib>(), gibs.transform.GetChild(i).GetComponent<Renderer>().sharedMaterial, x, y, xForce * (0.8f + UnityEngine.Random.value * 0.4f), yForce * (0.8f + UnityEngine.Random.value * 0.4f), xI, yI, 1);
		}
	}

	public static void CreateGibs(GibHolder gibs, int count, float x, float y, float xForce, float yForce, float xI, float yI)
	{
		CutsceneEffectsController.CreateGibs(gibs, count, x, y, xForce, yForce, xI, yI, 1);
	}

	public static void CreateGibs(GibHolder gibs, int count, float x, float y, float xForce, float yForce, float xI, float yI, int flipDirection)
	{
		for (int i = 0; i < count; i++)
		{
			int index = UnityEngine.Random.Range(0, gibs.transform.childCount);
			CutsceneEffectsController.CreateGib(gibs.transform.GetChild(index).GetComponent<Gib>(), gibs.transform.GetChild(index).GetComponent<Renderer>().sharedMaterial, x, y, xForce * (0.8f + UnityEngine.Random.value * 0.4f), yForce * (0.8f + UnityEngine.Random.value * 0.4f), xI, yI, flipDirection);
		}
	}

	public static void CreateGib(Gib gibPrefab, Material material, float x, float y, float xForce, float yForce, float xI, float yI, int flipDirection)
	{
		if (gibPrefab != null)
		{
			Gib gib = UnityEngine.Object.Instantiate(gibPrefab) as Gib;
			gib.GetComponent<Renderer>().sharedMaterial = material;
			gib.gameObject.layer = LayerMask.NameToLayer("Victory");
			gib.SetupSprite(gibPrefab.doesRotate, gibPrefab.GetLowerLeftPixel(), gibPrefab.GetPixelDimensions(), gibPrefab.GetSpriteOffset(), gibPrefab.rotateFrames);
			gib.Launch(x + gibPrefab.transform.localPosition.x * (float)flipDirection, y + gibPrefab.transform.localPosition.y, gibPrefab.transform.localPosition.x * (float)flipDirection / 16f * xForce + xI, gibPrefab.transform.localPosition.y / 16f * yForce + yI);
		}
	}

	public static Puff CreateEffect(Puff puffPrefab, float x, float y, float z, float delay, Vector3 velocity)
	{
		Puff puff = UnityEngine.Object.Instantiate(puffPrefab, new Vector3(x, y, z), Quaternion.identity) as Puff;
		puff.gameObject.layer = LayerMask.NameToLayer("Victory");
		if (velocity != Vector3.zero)
		{
			velocity.z = 0f;
			puff.SetVelocity(velocity);
		}
		if (delay > 0f)
		{
			puff.Delay(delay);
		}
		return puff;
	}

	public static void CreateEffect(FlickerFader flickerFaderPrefab, float x, float y, float z, float delay, Vector3 velocity)
	{
		FlickerFader flickerFader = UnityEngine.Object.Instantiate(flickerFaderPrefab, new Vector3(x, y, z), Quaternion.identity) as FlickerFader;
		flickerFader.gameObject.layer = LayerMask.NameToLayer("Victory");
		if (velocity != Vector3.zero)
		{
			velocity.z = 0f;
			flickerFader.SetVelocity(velocity);
		}
		if (delay > 0f)
		{
			flickerFader.Delay(delay);
		}
	}

	public static void CreateSmallExplosion(float x, float y, float z, float shakeM, float volume)
	{
		CutsceneEffectsController.CreateEffect(CutsceneEffectsController.instance.explosion, x, y, z, 0f, Vector3.zero);
		Vector3 vector = UnityEngine.Random.insideUnitCircle;
		vector.z = 0f;
		switch (UnityEngine.Random.Range(0, 5))
		{
		case 1:
			CutsceneEffectsController.CreateEffect(CutsceneEffectsController.instance.flame1Prefab, x + vector.x * 5f, y + vector.y * 5f, z, 0f + UnityEngine.Random.value * 0.3f, vector);
			CutsceneEffectsController.CreateEffect(CutsceneEffectsController.instance.flame2Prefab, x - vector.x * 5f, y - vector.y * 3f, z, 0f + UnityEngine.Random.value * 0.3f, -vector);
			break;
		case 2:
			CutsceneEffectsController.CreateEffect(CutsceneEffectsController.instance.flame3Prefab, x + vector.x * 6f, y + vector.y * 6f, z, 0f + UnityEngine.Random.value * 0.3f, vector);
			break;
		case 3:
		case 4:
			CutsceneEffectsController.CreateEffect(CutsceneEffectsController.instance.flame1Prefab, x + vector.x * 5f, y + vector.y * 5f, z, 0f + UnityEngine.Random.value * 0.4f, vector);
			CutsceneEffectsController.CreateEffect(CutsceneEffectsController.instance.flame2Prefab, x - vector.x * 5f, y - vector.y * 3f, z, 0f + UnityEngine.Random.value * 0.4f, -vector);
			CutsceneEffectsController.CreateEffect(CutsceneEffectsController.instance.flame3Prefab, x, y + vector.y * 6f, z, 0f + UnityEngine.Random.value * 0.5f, vector.y * Vector3.up);
			break;
		}
		if (volume > 0f)
		{
			CutsceneEffectsController.PlayExplosionSound(x, y, volume);
		}
	}

	protected static void PlayExplosionHugeSound(float x, float y, float volume)
	{
		Sound sound = Sound.GetInstance();
		sound.PlaySoundEffectAt(CutsceneEffectsController.instance.explosionHugeSoundHolder.deathSounds, volume, Sound.GetInstance().transform.position);
	}

	protected static void PlayExplosionSound(float x, float y, float volume)
	{
		Sound sound = Sound.GetInstance();
		sound.PlaySoundEffectAt(CutsceneEffectsController.instance.explosionSoundHolder.deathSounds, volume, Sound.GetInstance().transform.position);
	}

	public ParticleEmitter bloodParticlesBig;

	public ParticleEmitter bloodParticlesSmall;

	public ParticleEmitter bloodParticlesDots;

	public ParticleEmitter bloodParticlesDrips;

	public Shrapnel explosionSparkShrapnel;

	public Puff explosion;

	public Puff explosionBig;

	public Puff explosionHuge;

	public SoundHolder explosionSoundHolder;

	public SoundHolder explosionHugeSoundHolder;

	public FlickerFader flame1Prefab;

	public FlickerFader flame2Prefab;

	public FlickerFader flame3Prefab;

	public Shrapnel fireSpark1Prefab;

	public Shrapnel fireSpark2Prefab;

	public static CutsceneEffectsController instance;
}
