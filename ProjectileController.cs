// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
	public static ProjectileController instance
	{
		get
		{
			if (ProjectileController.inst == null)
			{
				ProjectileController.inst = (UnityEngine.Object.FindObjectOfType(typeof(ProjectileController)) as ProjectileController);
			}
			return ProjectileController.inst;
		}
	}

	private void Awake()
	{
		global::Math.SetupLookupTables();
	}

	public static void RegisterReturnZone(ProjectileReturnZone zone)
	{
		ProjectileController.instance.returnZones.Add(zone);
	}

	public static void RemoveReturnZone(ProjectileReturnZone zone)
	{
		ProjectileController.instance.returnZones.Remove(zone);
	}

	public static bool CheckReturnZone(float x, float y, ref int playerNum, ref ProjectileReturnZone returnZone)
	{
		foreach (ProjectileReturnZone projectileReturnZone in ProjectileController.instance.returnZones)
		{
			if (playerNum != projectileReturnZone.playerNum)
			{
				float f = projectileReturnZone.x - x;
				if (Mathf.Abs(f) < projectileReturnZone.radius)
				{
					float f2 = projectileReturnZone.y - y;
					if (Mathf.Abs(f2) < projectileReturnZone.radius)
					{
						playerNum = projectileReturnZone.playerNum;
						returnZone = projectileReturnZone;
						return true;
					}
				}
			}
		}
		return false;
	}

	public static Projectile SpawnProjectileOverNetwork(Projectile prefab, MonoBehaviour FiredBy, float x, float y, float xI, float yI, bool synced, int playerNum, bool AddTemporaryPlayerTarget, bool executeImmediately)
	{
		int arg = UnityEngine.Random.Range(-10000, 10000);
		GameObject gameObject = Networking.Instantiate<GameObject>(prefab.gameObject, null, executeImmediately);
		Projectile component = gameObject.GetComponent<Projectile>();
		Networking.RPC<float, float, float, float, int, MonoBehaviour>(PID.TargetAll, new RpcSignature<float, float, float, float, int, MonoBehaviour>(component.Fire), x, y, xI, yI, playerNum, FiredBy, executeImmediately);
		Networking.RPC<int>(PID.TargetAll, new RpcSignature<int>(component.SetSeed), arg, executeImmediately);
		if (synced)
		{
			NetworkObject component2 = gameObject.GetComponent<NetworkObject>();
			component2.EnableSyncing(true, executeImmediately);
		}
		if (AddTemporaryPlayerTarget)
		{
			HeroController.AddTemporaryPlayerTarget(playerNum, component.transform);
		}
		return component;
	}

	public static Projectile SpawnProjectileLocally(Projectile projectilePrefab, MonoBehaviour FiredBy, float x, float y, float xI, float yI, int playerNum)
	{
		Projectile projectile = UnityEngine.Object.Instantiate(projectilePrefab, new Vector3(x, y, 0f), Quaternion.identity) as Projectile;
		projectile.Fire(x, y, xI, yI, playerNum, FiredBy);
		return projectile;
	}

	public static Grenade SpawnGrenadeOverNetwork(Grenade grenadePrefab, MonoBehaviour firedBy, float x, float y, float radius, float force, float xI, float yI, int playerNum)
	{
		int arg = UnityEngine.Random.Range(0, 10000);
		GameObject gameObject = Networking.Instantiate<GameObject>(grenadePrefab.gameObject, null, false);
		Grenade component = gameObject.GetComponent<Grenade>();
		Networking.RPC<int, int, MonoBehaviour>(PID.TargetAll, new RpcSignature<int, int, MonoBehaviour>(component.SetupGrenade), arg, playerNum, firedBy, false);
		Networking.RPC<float, float, float, float>(PID.TargetAll, new RpcSignature<float, float, float, float>(component.Launch), x, y, xI, yI, false);
		Networking.RPC<PID>(PID.TargetAll, new RpcSignature<PID>(component.NetworkSetup), PID.MyID, false);
		return component;
	}

	private static ProjectileController inst;

	public List<Projectile> allProjectilePrefabs = new List<Projectile>();

	public List<Grenade> allGrenadePrefabs = new List<Grenade>();

	public Rocket remoteRocketPrefab;

	public Projectile brobocop;

	public RobrocopTargetingSystem targetSystemPrefab;

	public FollowingObject targetPrefab;

	public List<ProjectileReturnZone> returnZones = new List<ProjectileReturnZone>();

	public Projectile shellBombardment;
}
