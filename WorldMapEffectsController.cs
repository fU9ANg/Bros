// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldMapEffectsController : MonoBehaviour
{
	private void Awake()
	{
		WorldMapEffectsController.instance = this;
	}

	public static void CreateTerroristLine(WorldTerritory3D startTterritory, WorldTerritory3D endTerritory)
	{
		TerroristArmyLine terroristArmyLine = UnityEngine.Object.Instantiate(WorldMapEffectsController.instance.terroristLine, Vector3.zero, Quaternion.identity) as TerroristArmyLine;
		terroristArmyLine.transform.parent = WorldMapEffectsController.instance.worldActualTransform;
		terroristArmyLine.transform.localRotation = Quaternion.identity;
		terroristArmyLine.DrawLine(startTterritory, endTerritory);
	}

	public static void EmitSmoke(int count, Vector3 pos, Color color, float emitRadius, float force, float forceTangent, float energy)
	{
		for (int i = 0; i < count; i++)
		{
			Vector3 a = UnityEngine.Random.insideUnitCircle;
			WorldMapEffectsController.EmitSmoke(pos + a * emitRadius, pos.normalized * forceTangent + a * force, color, energy);
		}
	}

	public static void EmitSmoke(Vector3 pos, Vector3 velocity, Color color, float energy)
	{
		WorldMapEffectsController.instance.smokeParticles.Emit(pos, velocity, 0.1f, energy, color);
	}

	public static void EmitBlood(int count, Vector3 pos, Color color, float emitRadius, Vector3 force, float forceTangent, float minEnergy, float maxEnergy)
	{
		for (int i = 0; i < count; i++)
		{
			Vector3 a = UnityEngine.Random.insideUnitCircle;
			WorldMapEffectsController.EmitBlood(pos + a * emitRadius, a * forceTangent + force, color, Mathf.Lerp(minEnergy, maxEnergy, UnityEngine.Random.value));
		}
	}

	public static void EmitBlood(Vector3 pos, Vector3 velocity, Color color, float energy)
	{
		WorldMapEffectsController.instance.bloodParticles.Emit(pos, velocity, Mathf.Lerp(0.02f, 0.04f, UnityEngine.Random.value), energy, color);
	}

	public static void EmitFire(int count, Vector3 pos, Color color, float emitRadius, Vector3 force, float forceTangent, float minEnergy, float maxEnergy)
	{
		for (int i = 0; i < count; i++)
		{
			Vector3 a = UnityEngine.Random.insideUnitCircle;
			WorldMapEffectsController.EmitFire(pos + a * emitRadius, a * forceTangent + force, color, Mathf.Lerp(minEnergy, maxEnergy, UnityEngine.Random.value));
		}
	}

	public static void EmitFire(Vector3 pos, Vector3 velocity, Color color, float energy)
	{
		switch (UnityEngine.Random.Range(0, 3))
		{
		case 0:
			WorldMapEffectsController.instance.fireParticles1.Emit(pos, velocity, Mathf.Lerp(WorldMapEffectsController.instance.fireParticles1.minSize, WorldMapEffectsController.instance.fireParticles1.maxSize, UnityEngine.Random.value), energy, color);
			break;
		case 1:
			WorldMapEffectsController.instance.fireParticles2.Emit(pos, velocity, Mathf.Lerp(WorldMapEffectsController.instance.fireParticles2.minSize, WorldMapEffectsController.instance.fireParticles2.maxSize, UnityEngine.Random.value), energy, color);
			break;
		case 2:
			WorldMapEffectsController.instance.fireParticles3.Emit(pos, velocity, Mathf.Lerp(WorldMapEffectsController.instance.fireParticles3.minSize, WorldMapEffectsController.instance.fireParticles3.maxSize, UnityEngine.Random.value), energy, color);
			break;
		}
	}

	public static void EmitFeathers(int count, Vector3 pos, Color color, float emitRadius, Vector3 force, float forceTangent, float minEnergy, float maxEnergy)
	{
		for (int i = 0; i < count; i++)
		{
			Vector3 a = UnityEngine.Random.insideUnitCircle;
			WorldMapEffectsController.EmitFeathers(pos + a * emitRadius, a * forceTangent + force, color, Mathf.Lerp(minEnergy, maxEnergy, UnityEngine.Random.value));
		}
	}

	public static void EmitFeathers(Vector3 pos, Vector3 velocity, Color color, float energy)
	{
		WorldMapEffectsController.instance.featherParticles.Emit(pos, velocity, Mathf.Lerp(0.035f, 0.05f, UnityEngine.Random.value), energy, color);
	}

	public static void EmitWaves(int count, Vector3 pos, Color color, float emitRadius, Vector3 force, float forceTangent)
	{
		for (int i = 0; i < count; i++)
		{
			Vector3 a = global::Math.RandomPointOnCircle();
			a = new Vector3(a.x, 0f, a.y);
			WorldMapEffectsController.EmitWave(pos + a * emitRadius, a * forceTangent + force, color, Mathf.Lerp(WorldMapEffectsController.instance.waveParticles.minEnergy, WorldMapEffectsController.instance.waveParticles.maxEnergy, UnityEngine.Random.value));
		}
	}

	public static void EmitWave(Vector3 pos, Vector3 velocity, Color color, float energy)
	{
		WorldMapEffectsController.instance.waveParticles.Emit(pos, velocity, Mathf.Lerp(WorldMapEffectsController.instance.waveParticles.minSize, WorldMapEffectsController.instance.waveParticles.maxSize, UnityEngine.Random.value), energy, color);
	}

	public static void EmitDust(int count, Vector3 pos, Color color, float emitRadius, Vector3 force, float forceTangent)
	{
		for (int i = 0; i < count; i++)
		{
			Vector3 a = global::Math.RandomPointOnCircle();
			a = new Vector3(a.x, 0f, a.y);
			WorldMapEffectsController.instance.dustParticles.Emit(pos + a * emitRadius, a * forceTangent + force, Mathf.Lerp(WorldMapEffectsController.instance.dustParticles.minSize, WorldMapEffectsController.instance.dustParticles.maxSize, UnityEngine.Random.value), Mathf.Lerp(WorldMapEffectsController.instance.dustParticles.minEnergy, WorldMapEffectsController.instance.dustParticles.maxEnergy, UnityEngine.Random.value), color);
		}
	}

	public static void CreateSmokeEmitters(int count, Vector3 centre, Transform transform)
	{
		for (int i = 0; i < count; i++)
		{
		}
	}

	public TerroristArmyLine terroristLine;

	public Transform worldActualTransform;

	public static WorldMapEffectsController instance;

	public ParticleEmitter smokeParticles;

	public ParticleEmitter bloodParticles;

	public ParticleEmitter featherParticles;

	public ParticleEmitter waveParticles;

	public ParticleEmitter dustParticles;

	public ParticleEmitter fireParticles1;

	public ParticleEmitter fireParticles2;

	public ParticleEmitter fireParticles3;

	public SmokeEmitter smokeEmitterPrefab;
}
