// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : NetworkObject
{
	public static Map Instance
	{
		get
		{
			if (Map.inst == null)
			{
				Map.inst = (UnityEngine.Object.FindObjectOfType(typeof(Map)) as Map);
			}
			return Map.inst;
		}
	}

	public static MapData MapData { get; set; }

	public bool HasBeenSetup
	{
		get
		{
			return this.hasBeenSetup;
		}
	}

	public static void RegisterUnit(Unit unit, bool addToStatistics)
	{
		if (Map.units == null)
		{
			UnityEngine.Debug.LogError("Floating Unit at Start, should not be hanging out in level");
		}
		else
		{
			Map.units.Add(unit);
			Mook component = unit.GetComponent<Mook>();
			if (component && addToStatistics)
			{
				StatisticsController.RegisterMook(component);
			}
		}
	}

	public static void RemoveUnit(Unit unit)
	{
		Map.units.Remove(unit);
	}

	public static void ForgetPlayer(int playerNum)
	{
		if (playerNum >= 0 && playerNum < 4)
		{
			foreach (Unit unit in Map.units.ToArray())
			{
				if (unit != null && unit.GetComponent<PolymorphicAI>() != null)
				{
					unit.GetComponent<PolymorphicAI>().TryForgetPlayer(playerNum);
				}
			}
		}
	}

	public static void RegisterProjectile(Projectile projectile)
	{
		Map.projectiles.Add(projectile);
	}

	public static void RemoveProjectile(Projectile projectile)
	{
		if (Map.projectiles.Contains(projectile))
		{
			Map.projectiles.Remove(projectile);
		}
		if (Map.damageableProjectiles.Contains(projectile))
		{
			Map.damageableProjectiles.Remove(projectile);
		}
	}

	public static void RegisterGrenade(Grenade grenade)
	{
		Map.grenades.Add(grenade);
	}

	public static void RemoveGrenade(Grenade grenade)
	{
		if (Map.grenades.Contains(grenade))
		{
			Map.grenades.Remove(grenade);
		}
	}

	public static void RegisterShootableGrenade(Grenade shootableGrenade)
	{
		Map.shootableGrenades.Add(shootableGrenade);
	}

	public static void RemoveShootableGrenade(Grenade shootableGrenade)
	{
		if (Map.shootableGrenades.Contains(shootableGrenade))
		{
			Map.shootableGrenades.Remove(shootableGrenade);
		}
	}

	public static void RegisterDamageableProjectile(Projectile projectile)
	{
		Map.damageableProjectiles.Add(projectile);
	}

	public static void RegisterMookDoor(MookDoor door)
	{
		if (Map.mookDoors == null)
		{
			Map.mookDoors = new List<MookDoor>();
		}
		Map.mookDoors.Add(door);
	}

	public static void RegisterSwitch(Switch swit)
	{
		if (Map.switches == null)
		{
			Map.switches = new List<Switch>();
		}
		Map.switches.Add(swit);
	}

	public static void RemoveSwitch(Switch swit)
	{
		if (Map.switches != null)
		{
			Map.switches.Remove(swit);
		}
	}

	public static void RegisterDamageableScenerye(DamageableScenery tree)
	{
		if (Map.damageableScenery == null)
		{
			Map.damageableScenery = new List<DamageableScenery>();
		}
		if (!Map.damageableScenery.Contains(tree))
		{
			Map.damageableScenery.Add(tree);
		}
	}

	public static void RegisterTreeFoliage(TreeFoliage tree)
	{
		if (Map.treeFoliage == null)
		{
			Map.treeFoliage = new List<TreeFoliage>();
		}
		if (!Map.treeFoliage.Contains(tree))
		{
			Map.treeFoliage.Add(tree);
		}
	}

	public static void RegisterEnemyDeathListener(Trigger trig)
	{
		if (Map.enemyDeathListeners == null)
		{
			Map.enemyDeathListeners = new List<Trigger>();
		}
		if (!Map.enemyDeathListeners.Contains(trig))
		{
			Map.enemyDeathListeners.Add(trig);
		}
	}

	public static void RemoveEnemyDeathListener(Trigger trig)
	{
		if (Map.enemyDeathListeners != null)
		{
			Map.enemyDeathListeners.Remove(trig);
		}
	}

	public static void EnemyDeathEvent(Unit unit)
	{
		foreach (Trigger trigger in Map.enemyDeathListeners)
		{
			trigger.EvaluateEnemyDeathEvent(unit);
		}
	}

	public static MookDoor GetNearestMookDoor(int col, int row)
	{
		return (from md in Map.mookDoors
		orderby Mathf.Abs(md.collumn - col) + Mathf.Abs(md.row - row)
		select md).FirstOrDefault((MookDoor md) => !md.isDestroyed);
	}

	public static void RegisterWildLife(WildLife wildLifeObject)
	{
		if (Map.wildLife == null)
		{
			Map.wildLife = new List<WildLife>();
		}
		Map.wildLife.Add(wildLifeObject);
	}

	public static void RegisterDisturbedWildLife(WildLife wildLifeObject)
	{
		if (Map.disturbedWildLife == null)
		{
			Map.disturbedWildLife = new List<WildLife>();
		}
		Map.disturbedWildLife.Add(wildLifeObject);
	}

	public static void RemoveWildLife(WildLife wildLifeObject)
	{
		Map.wildLife.Remove(wildLifeObject);
	}

	public static void RemoveDisturbedWildLife(WildLife wildLifeObject)
	{
		Map.disturbedWildLife.Remove(wildLifeObject);
	}

	public static void RegisterDestroyableDoodad(Doodad doodad)
	{
		if (Map.destroyableDoodads == null)
		{
			Map.destroyableDoodads = new List<Doodad>();
		}
		Map.destroyableDoodads.Add(doodad);
	}

	public static void RemoveDestroyableDoodad(Doodad doodad)
	{
		Map.destroyableDoodads.Remove(doodad);
	}

	public static void RegisterStaticDoodad(Doodad doodad)
	{
		if (Map.staticDoodads == null)
		{
			Map.staticDoodads = new List<Doodad>();
		}
		Map.staticDoodads.Add(doodad);
	}

	public static void RemoveStaticDoodad(Doodad doodad)
	{
		Map.staticDoodads.Remove(doodad);
	}

	public static void RegisterDecalDoodad(Doodad doodad)
	{
		if (Map.decalDoodads == null)
		{
			Map.decalDoodads = new List<Doodad>();
		}
		Map.decalDoodads.Add(doodad);
	}

	public static void RemoveDecalDoodad(Doodad doodad)
	{
		Map.decalDoodads.Remove(doodad);
	}

	public static Doodad GetStaticDoodad(int collumn, int row)
	{
		foreach (Doodad doodad in Map.staticDoodads)
		{
			if (doodad.collumn == collumn && doodad.row == row)
			{
				return doodad;
			}
		}
		return null;
	}

	public static Vector3 GetSpawnPointPosition(int playerNum)
	{
		if (Map.spawnPoints != null && Map.spawnPoints.Count > 0 && playerNum >= 0)
		{
			return Map.spawnPoints[(playerNum + Map.spawnPointOffset) % Map.spawnPoints.Count].transform.position;
		}
		return -Vector3.one;
	}

	public static SpawnPoint GetSpawnPoint(int playerNum)
	{
		if (Map.spawnPoints != null && Map.spawnPoints.Count > 0 && playerNum >= 0)
		{
			return Map.spawnPoints[(playerNum + Map.spawnPointOffset) % Map.spawnPoints.Count];
		}
		return null;
	}

	public static void RegisterSpawnPoint(SpawnPoint spawnPoint)
	{
		if (Map.spawnPoints == null)
		{
			Map.spawnPoints = new List<SpawnPoint>();
		}
		Map.spawnPoints.Add(spawnPoint);
	}

	public static void RegisterCheckPoint(CheckPoint checkPoint)
	{
		if (Map.checkPoints == null)
		{
			Map.checkPoints = new List<CheckPoint>();
		}
		Map.checkPoints.Add(checkPoint);
	}

	public static void RemoveCheckPoint(CheckPoint checkPoint)
	{
		Map.checkPoints.Remove(checkPoint);
	}

	public static void RegisterHelicopter(Helicopter heli)
	{
		Map.newestHelicopter = heli;
	}

	public static void BloodyDoodads(float x, float y, float range)
	{
		int num = Map.destroyableDoodads.Count;
		int num2 = 0;
		while (num2 < Map.decalDoodads.Count && num > 0)
		{
			if (Map.decalDoodads[num2] != null)
			{
				num--;
				if (Mathf.Abs(Map.decalDoodads[num2].centerX - x) <= range + Map.decalDoodads[num2].width / 2f && Mathf.Abs(Map.decalDoodads[num2].centerY - y) <= range + Map.decalDoodads[num2].height / 2f)
				{
					Map.decalDoodads[num2].Bloody();
					num2--;
				}
			}
			num2++;
		}
	}

	public static bool DamageDoodads(int damage, float x, float y, float xI, float yI, float range, int playerNum)
	{
		bool result = false;
		int num = Map.destroyableDoodads.Count;
		int num2 = 0;
		while (num2 < Map.destroyableDoodads.Count && num > 0)
		{
			num--;
			if (num2 >= 0 && Map.destroyableDoodads[num2] != null && Mathf.Abs(Map.destroyableDoodads[num2].centerX - x) <= range + Map.destroyableDoodads[num2].width / 2f && Mathf.Abs(Map.destroyableDoodads[num2].centerY - y) <= range + Map.destroyableDoodads[num2].height / 2f && Map.destroyableDoodads[num2].DamageOptional(new DamageObject(damage, DamageType.Bullet, xI, yI, null), ref result))
			{
				num2--;
			}
			num2++;
		}
		return result;
	}

	public static void ShakeTrees(float x, float y, float xRange, float yRange, float force)
	{
		for (int i = 0; i < Map.treeFoliage.Count; i++)
		{
			if (i >= 0 && Map.treeFoliage[i] != null)
			{
				float num = Map.treeFoliage[i].x - x;
				if (Mathf.Abs(num) <= xRange)
				{
					float num2 = Map.treeFoliage[i].y - y;
					if (Mathf.Abs(num2) <= yRange)
					{
						Map.treeFoliage[i].Shake(num / xRange, num2 / yRange, force, Mathf.Abs(num) + Mathf.Abs(num2));
					}
				}
			}
		}
	}

	public static bool PassThroughScenery(float x, float y, float xI, float yI)
	{
		bool result = false;
		for (int i = 0; i < Map.damageableScenery.Count; i++)
		{
			if (i >= 0 && Map.damageableScenery[i] != null)
			{
				float f = Map.damageableScenery[i].x - x;
				if (Mathf.Abs(f) <= Map.damageableScenery[i].xRadius)
				{
					float f2 = Map.damageableScenery[i].y - y;
					if (Mathf.Abs(f2) <= Map.damageableScenery[i].yRadius)
					{
						result = Map.damageableScenery[i].Knock(xI, yI);
					}
				}
			}
		}
		return result;
	}

	public static void DisturbWildLife(float x, float y, float range, int playerNum)
	{
		if (Map.isEditing)
		{
			return;
		}
		int num = Map.wildLife.Count;
		int num2 = 0;
		while (num2 < Map.wildLife.Count && num > 0)
		{
			num--;
			if (num2 >= 0 && Map.wildLife[num2] != null && Mathf.Abs(Map.wildLife[num2].x - x) <= range && Mathf.Abs(Map.wildLife[num2].y - y) <= range * 3f && Map.wildLife[num2].Disturb())
			{
				num2--;
			}
			num2++;
		}
		if (playerNum >= 0)
		{
			foreach (Unit unit in Map.units.ToArray())
			{
				if (unit != null && unit.canHear && unit.playerNum < 0)
				{
					float f = unit.x - x;
					if (Mathf.Abs(f) < range + unit.hearingRangeX)
					{
						float f2 = unit.y - y;
						if (Mathf.Abs(f2) < range + unit.hearingRangeY)
						{
							unit.Alert(x, y);
							unit.HearSound(x, y);
						}
					}
				}
			}
		}
	}

	public static void HurtWildLife(float x, float y, float range)
	{
		if (Map.isEditing)
		{
			return;
		}
		int num = Map.disturbedWildLife.Count;
		int num2 = 0;
		while (num2 < Map.disturbedWildLife.Count && num > 0)
		{
			num--;
			if (num2 >= 0 && Map.disturbedWildLife[num2] != null && Mathf.Abs(Map.disturbedWildLife[num2].x - x) <= range && Mathf.Abs(Map.disturbedWildLife[num2].y - y) <= range * 3f && Map.disturbedWildLife[num2].Hurt())
			{
				num2--;
			}
			num2++;
		}
	}

	public static void CheckCheckPoint(float xI, float x, float y)
	{
		for (int i = 0; i < Map.checkPoints.Count; i++)
		{
			if (i >= 0 && Map.checkPoints[i] != null && !Map.checkPoints[i].activated)
			{
				float f = Map.checkPoints[i].x - x;
				bool flag = Mathf.Abs(xI) > 30f && Mathf.Sign(xI) == Mathf.Sign(f);
				if (Mathf.Abs(f) <= (float)((!flag) ? 48 : 10) && Mathf.Abs(Map.checkPoints[i].y + 20f - y) <= 40f && !Map.checkPoints[i].IsBlockedByUnit())
				{
					Networking.RPC(PID.TargetAll, new RpcSignature(Map.checkPoints[i].ActivateInternal), false);
				}
			}
		}
	}

	public static bool IsNearCheckPoint(float x, float y, ref Vector2 checkPointPos)
	{
		for (int i = 0; i < Map.checkPoints.Count; i++)
		{
			if (i >= 0 && Map.checkPoints[i] != null && Mathf.Abs(Map.checkPoints[i].x - x) <= 48f && Mathf.Abs(Map.checkPoints[i].y + 20f - y) <= 40f)
			{
				checkPointPos = new Vector2(Map.checkPoints[i].x, Map.checkPoints[i].y);
				return true;
			}
		}
		return false;
	}

	public static bool HitGrenades(int playerNum, float range, float x, float y, float xI, float yI, ref float grenadeX, ref float grenadeY)
	{
		for (int i = 0; i < Map.shootableGrenades.Count; i++)
		{
			if (Map.shootableGrenades[i] != null)
			{
				float num = Map.shootableGrenades[i].x - x;
				if (Mathf.Abs(num) < range + Map.shootableGrenades[i].size)
				{
					float num2 = Map.shootableGrenades[i].y - y;
					if (Mathf.Abs(num2) < range + Map.shootableGrenades[i].size)
					{
						grenadeX = Map.shootableGrenades[i].x;
						grenadeY = Map.shootableGrenades[i].y;
						if (Map.shootableGrenades[i] is Coconut)
						{
							Networking.RPC<float, float, float, float>(PID.TargetOthers, new RpcSignature<float, float, float, float>(Map.shootableGrenades[i].Knock), num, num2, xI, yI, false);
						}
						Map.shootableGrenades[i].Knock(num, num2, xI, yI);
						return true;
					}
				}
			}
		}
		return false;
	}

	public static Grenade GetNearbyGrenade(int playerNum, float range, float x, float y)
	{
		int num = -1;
		Map.nearestDist = range + 1f;
		bool flag = false;
		for (int i = 0; i < Map.grenades.Count; i++)
		{
			if (Map.grenades[i] != null)
			{
				float f = Map.grenades[i].x - x;
				if (Mathf.Abs(f) < Map.nearestDist)
				{
					float f2 = Map.grenades[i].y - y;
					if (Mathf.Abs(f2) < Map.nearestDist)
					{
						float num2 = Mathf.Abs(f) + Mathf.Abs(f2);
						if (num2 < Map.nearestDist)
						{
							Map.nearestDist = num2;
							num = i;
						}
					}
				}
			}
			else
			{
				flag = true;
			}
		}
		if (flag)
		{
			UnityEngine.Debug.LogWarning("There is a null reference in the grenade list");
		}
		if (num >= 0)
		{
			return Map.grenades[num];
		}
		return null;
	}

	public static bool DeflectProjectiles(MonoBehaviour newOwner, int playerNum, float range, float x, float y, float xI)
	{
		bool result = false;
		foreach (Projectile projectile in Map.projectiles.ToArray())
		{
			if (projectile != null && GameModeController.DoesPlayerNumDamage(projectile.playerNum, playerNum))
			{
				float f = projectile.x - x;
				if (Mathf.Abs(f) - range < 10f)
				{
					float f2 = projectile.y - y;
					if (Mathf.Abs(f2) - range < 6f)
					{
						projectile.playerNum = playerNum;
						projectile.firedBy = newOwner;
						if (Mathf.Sign(xI) != Mathf.Sign(projectile.xI))
						{
							projectile.xI *= -1f;
							projectile.yI += Mathf.Abs(projectile.xI) * (UnityEngine.Random.value * 0.4f - 0.2f);
							projectile.SetDamage(5);
							projectile.life += 0.5f;
							result = true;
							if (projectile.isDamageable)
							{
								projectile.Damage(500, DamageType.Bullet, xI, 0f, 0.1f, playerNum);
							}
						}
					}
				}
			}
		}
		return result;
	}

	public static bool HitProjectiles(int playerNum, int damage, DamageType damageType, float range, float x, float y, float xI, float yI, float delay)
	{
		return Map.HitProjectiles(playerNum, damage, damageType, range, range, x, y, xI, yI, delay);
	}

	public static bool HitProjectiles(int playerNum, int damage, DamageType damageType, float xRange, float yRange, float x, float y, float xI, float yI, float delay)
	{
		bool result = false;
		foreach (Projectile projectile in Map.damageableProjectiles.ToArray())
		{
			if (projectile != null && GameModeController.DoesPlayerNumDamage(playerNum, projectile.playerNum))
			{
				float f = projectile.x - x;
				if (Mathf.Abs(f) - xRange < projectile.projectileSize)
				{
					float f2 = projectile.y - y;
					if (Mathf.Abs(f2) - yRange < projectile.projectileSize)
					{
						Map.DamageProjectile(projectile, damage, damageType, xI, yI, delay, playerNum);
						result = true;
					}
				}
			}
		}
		return result;
	}

	public static bool HitUnits(MonoBehaviour damageSender, MonoBehaviour avoidID, int playerNum, int damage, DamageType damageType, float range, float x, float y, float xI, float yI, bool penetrates, bool knock)
	{
		return Map.HitUnits(damageSender, avoidID, playerNum, damage, damageType, range, range, x, y, xI, yI, penetrates, knock, true);
	}

	public static bool HitUnits(MonoBehaviour damageSender, MonoBehaviour avoidID, int playerNum, int damage, DamageType damageType, float range, float x, float y, float xI, float yI, bool penetrates, bool knock, bool canGib)
	{
		return Map.HitUnits(damageSender, avoidID, playerNum, damage, damageType, range, range, x, y, xI, yI, penetrates, knock, canGib);
	}

	public static bool HitUnits(MonoBehaviour damageSender, MonoBehaviour avoidID, int playerNum, int damage, DamageType damageType, float xRange, float yRange, float x, float y, float xI, float yI, bool penetrates, bool knock, bool canGib)
	{
		bool result = false;
		int num = 999999;
		bool flag = false;
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && GameModeController.DoesPlayerNumDamage(playerNum, unit.playerNum) && !unit.invulnerable && unit.health <= num)
			{
				float f = unit.x - x;
				if (Mathf.Abs(f) - xRange < unit.width)
				{
					float num2 = unit.y + unit.height / 2f + 4f - y;
					if (Mathf.Abs(num2) - yRange < unit.height && (avoidID == null || avoidID != unit))
					{
						if (!penetrates && unit.health > 0)
						{
							num = 0;
							flag = true;
						}
						if (!canGib && unit.health <= 0)
						{
							Map.KnockAndDamageUnit(damageSender, unit, 0, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
						}
						else if (num2 < -unit.height)
						{
							Map.HeadShotUnit(damageSender, unit, damage, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
						}
						else
						{
							Map.KnockAndDamageUnit(damageSender, unit, damage, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
						}
						result = true;
						if (flag)
						{
							return result;
						}
					}
				}
			}
		}
		return result;
	}

	public static bool WhipUnits(MonoBehaviour damageSender, MonoBehaviour avoidID, int playerNum, int damage, DamageType damageType, float xRange, float yRange, int whipDirection, float x, float y, float xI, float yI, bool penetrates, bool knock, bool canGib, ref float maxDamageM)
	{
		bool result = false;
		int num = 999999;
		bool flag = false;
		maxDamageM = 0f;
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && GameModeController.DoesPlayerNumDamage(playerNum, unit.playerNum) && !unit.invulnerable && unit.health <= num)
			{
				float num2 = unit.x - x;
				if (Mathf.Abs(num2) - xRange < unit.width)
				{
					float f = unit.y + unit.height / 2f + 4f - y;
					if (Mathf.Abs(f) - yRange < unit.height && (avoidID == null || avoidID != unit))
					{
						float num3 = Mathf.Max(xRange, 96f);
						if (whipDirection > 0)
						{
							num2 += xRange / 2f;
						}
						else
						{
							num2 -= xRange / 2f;
						}
						float num4 = Mathf.Clamp(num2 * (float)whipDirection - num3 * 0.2f, 0f, num3) / num3 * 1.25f;
						if (num4 > maxDamageM)
						{
							maxDamageM = num4;
						}
						int damage2 = Mathf.CeilToInt((float)damage * num4 + (float)((unit.playerNum < 0) ? 0 : 1));
						if (!penetrates && unit.health > 0)
						{
							num = 0;
							flag = true;
						}
						if (!canGib && unit.health <= 0)
						{
							Map.KnockAndDamageUnit(damageSender, unit, 0, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
						}
						else
						{
							Map.KnockAndDamageUnit(damageSender, unit, damage2, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
						}
						if (num4 > 0.7f)
						{
							EffectsController.CreateProjectileLargePopEffect(unit.x, y);
						}
						else if (num4 > 0.2f)
						{
							EffectsController.CreateProjectilePopEffect(unit.x, y);
						}
						result = true;
						if (flag)
						{
							return result;
						}
					}
				}
			}
		}
		return result;
	}

	public static bool HitLivingUnits(MonoBehaviour damageSender, int playerNum, int damage, DamageType damageType, float range, float x, float y, float xI, float yI, bool penetrates, bool knock)
	{
		return Map.HitLivingUnits(damageSender, playerNum, damage, damageType, range, range, x, y, xI, yI, penetrates, knock);
	}

	public static bool HitLivingUnits(MonoBehaviour damageSender, int playerNum, int damage, DamageType damageType, float xRange, float yRange, float x, float y, float xI, float yI, bool penetrates, bool knock)
	{
		bool result = false;
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && GameModeController.DoesPlayerNumDamage(playerNum, unit.playerNum) && !unit.invulnerable && unit.health > 0)
			{
				float f = unit.x - x;
				if (Mathf.Abs(f) - xRange < unit.width)
				{
					float num = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(num) - yRange < unit.height && (Demonstration.projectilesHitWalls || unit.health > 0))
					{
						if (num < -unit.height)
						{
							Map.HeadShotUnit(damageSender, unit, damage, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
						}
						else
						{
							Map.KnockAndDamageUnit(damageSender, unit, damage, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
						}
						if (!penetrates)
						{
							return true;
						}
						result = true;
					}
				}
			}
		}
		return result;
	}

	public static bool HitAllLivingUnits(MonoBehaviour damageSender, int playerNum, int damage, DamageType damageType, float xRange, float yRange, float x, float y, float xI, float yI, bool penetrates, bool knock)
	{
		bool result = false;
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && playerNum != unit.playerNum && !unit.invulnerable && unit.health > 0)
			{
				float f = unit.x - x;
				if (Mathf.Abs(f) - xRange < unit.width)
				{
					float num = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(num) - yRange < unit.height && (Demonstration.projectilesHitWalls || unit.health > 0))
					{
						if (num < -unit.height)
						{
							Map.HeadShotUnit(damageSender, unit, damage, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
						}
						else
						{
							Map.KnockAndDamageUnit(damageSender, unit, damage, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
						}
						if (!penetrates)
						{
							return true;
						}
						result = true;
					}
				}
			}
		}
		return result;
	}

	public static bool HitAllLivingUnits(MonoBehaviour damageSender, int playerNum, int damage, DamageType damageType, float xRange, float yRange, float x, float y, float xI, float yI, bool penetrates, bool knock, ref List<Unit> alreadyHit)
	{
		bool result = false;
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && playerNum != unit.playerNum && !unit.invulnerable && unit.health > 0 && !alreadyHit.Contains(unit))
			{
				float f = unit.x - x;
				if (Mathf.Abs(f) - xRange < unit.width)
				{
					float num = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(num) - yRange < unit.height && (Demonstration.projectilesHitWalls || unit.health > 0))
					{
						alreadyHit.Add(unit);
						if (num < -unit.height)
						{
							Map.HeadShotUnit(damageSender, unit, damage, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
						}
						else
						{
							Map.KnockAndDamageUnit(damageSender, unit, damage, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
						}
						if (!penetrates)
						{
							return true;
						}
						result = true;
					}
				}
			}
		}
		return result;
	}

	public static bool HitLivingHeroes(MonoBehaviour damageSender, int playerNum, int damage, DamageType damageType, float xRange, float yRange, float x, float y, float xI, float yI, bool penetrates, bool knock, bool hitHeroes)
	{
		bool result = false;
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && GameModeController.DoesPlayerNumDamage(playerNum, unit.playerNum) && !unit.invulnerable && unit.health > 0 && ((unit.IsHero && hitHeroes) || (!unit.IsHero && !hitHeroes)))
			{
				float f = unit.x - x;
				if (Mathf.Abs(f) - xRange < unit.width)
				{
					float f2 = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f2) - yRange < unit.height && (Demonstration.projectilesHitWalls || unit.health > 0))
					{
						Map.KnockAndDamageUnit(damageSender, unit, damage, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
						if (!penetrates)
						{
							return true;
						}
						result = true;
					}
				}
			}
		}
		return result;
	}

	public static bool HitDeadUnits(MonoBehaviour damageSender, int damage, DamageType damageType, float range, float x, float y, float xI, float yI, bool penetrates, bool knock)
	{
		bool result = false;
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && !unit.invulnerable && unit.health <= 0)
			{
				float f = unit.x - x;
				if (Mathf.Abs(f) - range < unit.width)
				{
					float f2 = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f2) - range < unit.height && (Demonstration.projectilesHitWalls || unit.health > 0))
					{
						Map.KnockAndDamageUnit(damageSender, unit, damage, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
						if (!penetrates)
						{
							return true;
						}
						result = true;
					}
				}
			}
		}
		return result;
	}

	public static Unit CheckForCorpse(float x, float y, float xRange, float yRange, ref float corpseX)
	{
		Unit result = null;
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && !unit.invulnerable && unit.health <= 0)
			{
				float f = unit.y + unit.height / 2f + 3f - y;
				if (Mathf.Abs(f) < yRange)
				{
					float f2 = unit.x - x;
					if (Mathf.Abs(f2) < xRange)
					{
						Vector3 vector = new Vector3(x, y + 6f, 0f);
						Vector3 direction = new Vector3(unit.x, unit.y, 0f) - vector;
						if ((Mathf.Abs(direction.x) < 8f && Mathf.Abs(direction.y) < 8f) || !Physics.Raycast(vector, direction, direction.magnitude - 6f, Map.groundLayer))
						{
							xRange = Mathf.Abs(f2);
							corpseX = unit.x;
							result = unit;
						}
					}
				}
			}
		}
		return result;
	}

	public static Unit ImpaleUnits(MonoBehaviour damageSender, int playerNum, float range, float x, float y, bool onlyLiving, bool includeSuicide, ref List<Unit> alreadyHitUnits)
	{
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && GameModeController.DoesPlayerNumDamage(playerNum, unit.playerNum) && !unit.invulnerable && (!onlyLiving || unit.health > 0 || (includeSuicide && unit.GetMookType() == MookType.Suicide)))
			{
				float f = unit.x - x;
				if (Mathf.Abs(f) - range < unit.width)
				{
					float f2 = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f2) - range < unit.height && !alreadyHitUnits.Contains(unit))
					{
						return unit;
					}
				}
			}
		}
		return null;
	}

	public static bool CollectUnits(MonoBehaviour damageSender, int playerNum, float range, float x, float y, bool penetrates, bool onlyLiving, ref List<Unit> alreadyHitUnits)
	{
		bool result = false;
		bool flag = false;
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && GameModeController.DoesPlayerNumDamage(playerNum, unit.playerNum) && !unit.invulnerable && (!onlyLiving || unit.health > 0))
			{
				float f = unit.x - x;
				if (Mathf.Abs(f) - range < unit.width)
				{
					float f2 = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f2) - range < unit.height && !alreadyHitUnits.Contains(unit))
					{
						alreadyHitUnits.Add(unit);
						if (!penetrates)
						{
							flag = true;
						}
						result = true;
						if (flag)
						{
							return result;
						}
					}
				}
			}
		}
		return result;
	}

	public static bool HitUnits(MonoBehaviour damageSender, int playerNum, int damage, DamageType damageType, float range, float x, float y, float xI, float yI, bool penetrates, bool knock, bool canGib, ref List<Unit> alreadyHitUnits)
	{
		return Map.HitUnits(damageSender, playerNum, damage, damage, damageType, range, x, y, xI, yI, penetrates, knock, canGib, ref alreadyHitUnits);
	}

	public static bool HitUnits(MonoBehaviour damageSender, int playerNum, int damage, int corpseDamage, DamageType damageType, float range, float x, float y, float xI, float yI, bool penetrates, bool knock, bool canGib, ref List<Unit> alreadyHitUnits)
	{
		bool result = false;
		bool flag = false;
		int num = 999999;
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && GameModeController.DoesPlayerNumDamage(playerNum, unit.playerNum) && !unit.invulnerable && unit.health <= num)
			{
				float f = unit.x - x;
				if (Mathf.Abs(f) - range < unit.width)
				{
					float f2 = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f2) - range < unit.height && !alreadyHitUnits.Contains(unit))
					{
						alreadyHitUnits.Add(unit);
						if (!penetrates && unit.health > 0)
						{
							num = 0;
							flag = true;
						}
						if (!canGib && unit.health <= 0)
						{
							Map.KnockAndDamageUnit(damageSender, unit, 0, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
						}
						else if (unit.health <= 0)
						{
							Map.KnockAndDamageUnit(damageSender, unit, corpseDamage, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
						}
						else
						{
							Map.KnockAndDamageUnit(damageSender, unit, damage, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
						}
						result = true;
						if (flag)
						{
							return result;
						}
					}
				}
			}
		}
		return result;
	}

	public static Unit GeLivingtUnit(int playerNum, float xRange, float yRange, float x, float y)
	{
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && !unit.invulnerable && unit.health > 0 && GameModeController.DoesPlayerNumDamage(playerNum, unit.playerNum))
			{
				float f = unit.x - x;
				float num = Mathf.Min(unit.width, 4f) + unit.width * 0.5f;
				if (Mathf.Abs(f) - xRange < num)
				{
					float num2 = Mathf.Min(unit.height, 4f) + unit.width * 0.5f;
					float f2 = unit.y + num2 / 2f + 5f - y;
					if (Mathf.Abs(f2) - yRange < num2)
					{
						return unit;
					}
				}
			}
		}
		return null;
	}

	public static bool HitUnits(MonoBehaviour damageSender, int damage, DamageType damageType, float range, float x, float y, float xI, float yI, bool penetrates, bool knock)
	{
		return Map.HitUnits(damageSender, damage, damageType, range, range, x, y, xI, yI, penetrates, knock);
	}

	public static bool HitUnits(MonoBehaviour damageSender, int damage, DamageType damageType, float xRange, float yRange, float x, float y, float xI, float yI, bool penetrates, bool knock)
	{
		bool result = false;
		int num = 999999;
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && !unit.invulnerable && unit.health <= num)
			{
				float f = unit.x - x;
				if (Mathf.Abs(f) - xRange < unit.width)
				{
					float f2 = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f2) - yRange < unit.height)
					{
						Map.KnockAndDamageUnit(damageSender, unit, damage, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
						if (!penetrates && unit.health > 0)
						{
							num = 0;
						}
						result = true;
					}
				}
			}
		}
		return result;
	}

	public static bool HitClosestUnit(MonoBehaviour damageSender, int playerNum, int damage, DamageType damageType, float xRange, float yRange, float x, float y, float xI, float yI, bool knock, bool canGib, bool firedLocally, bool checkIfUnitIsLocallyOwned)
	{
		int num = 999999;
		float num2 = Mathf.Max(xRange, yRange);
		float num3 = Mathf.Max(xRange, yRange);
		Unit unit = null;
		Unit unit2 = null;
		foreach (Unit unit3 in Map.units.ToArray())
		{
			if (unit3 != null && !unit3.invulnerable && unit3.health <= num && GameModeController.DoesPlayerNumDamage(playerNum, unit3.playerNum))
			{
				float f = unit3.x - x;
				if (Mathf.Abs(f) - xRange < unit3.width)
				{
					float f2 = unit3.y + unit3.height / 2f + 3f - y;
					if (Mathf.Abs(f2) - yRange < unit3.height)
					{
						float num4 = Mathf.Abs(f) + Mathf.Abs(f2);
						if (num4 < num2)
						{
							if (unit3.health <= 0 && unit == null)
							{
								if (num4 < num3)
								{
									num3 = num4;
									unit2 = unit3;
								}
							}
							else if (unit3.health > 0)
							{
								unit = unit3;
								num2 = num4;
							}
						}
					}
				}
			}
		}
		Vector3 vector = new Vector3(x, y + 5f, 0f);
		if (unit != null && !Physics.Raycast(vector, unit.transform.position - vector, (unit.transform.position - vector).magnitude * 0.8f, Map.groundLayer))
		{
			Map.KnockAndDamageUnit(damageSender, unit, damage, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
			return true;
		}
		if (unit2 != null && !Physics.Raycast(vector, unit2.transform.position - vector, (unit2.transform.position - vector).magnitude * 0.8f, Map.groundLayer))
		{
			if (!canGib)
			{
				damage = 0;
			}
			Map.KnockAndDamageUnit(damageSender, unit2, damage, damageType, xI, yI, (int)Mathf.Sign(xI), knock, x, y);
			return true;
		}
		return false;
	}

	public static Unit GetNextClosestUnit(int playerNum, DirectionEnum direction, float xRange, float yRange, float x, float y, List<Unit> alreadyFoundUnits)
	{
		float num = xRange;
		Unit unit = null;
		foreach (Unit unit2 in Map.units.ToArray())
		{
			if (unit2 != null && !unit2.invulnerable && unit2.health > 0 && GameModeController.DoesPlayerNumDamage(playerNum, unit2.playerNum) && !alreadyFoundUnits.Contains(unit2))
			{
				float num2 = unit2.y + unit2.height / 2f + 3f - y;
				if (Mathf.Abs(num2) - yRange < unit2.height)
				{
					float num3 = unit2.x - x;
					if (Mathf.Abs(num3) - num < unit2.width && ((direction == DirectionEnum.Down && num2 < 0f) || (direction == DirectionEnum.Up && num2 > 0f) || (direction == DirectionEnum.Right && num3 > 0f) || (direction == DirectionEnum.Left && num3 < 0f) || direction == DirectionEnum.Any))
					{
						unit = unit2;
						num = Mathf.Abs(num2);
					}
				}
			}
		}
		if (unit != null)
		{
			return unit;
		}
		return null;
	}

	public static bool IsUnitNearby(int playerNum, float x, float y, float xRange, float yRange, bool includeDeadUnits)
	{
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && playerNum == unit.playerNum && !unit.invulnerable && (includeDeadUnits || unit.health > 0))
			{
				float num = unit.x - x;
				if (Mathf.Abs(num) - xRange < unit.width && (unit.y != y || num != 0f))
				{
					float f = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f) - yRange < unit.height)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool IsUnitNearby(int playerNum, float x, float y, float xRange, float yRange, bool includeDeadUnits, out Unit nearestUnit)
	{
		float num = xRange + yRange + 1f;
		bool result = false;
		nearestUnit = null;
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && playerNum == unit.playerNum && !unit.invulnerable && (includeDeadUnits || unit.health > 0))
			{
				float num2 = unit.x - x;
				if (Mathf.Abs(num2) - xRange < unit.width && (unit.y != y || num2 != 0f))
				{
					float f = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f) - yRange < unit.height)
					{
						if (Mathf.Abs(f) + Mathf.Abs(num2) < num)
						{
							num = Mathf.Abs(f) + Mathf.Abs(num2);
							nearestUnit = unit;
						}
						result = true;
					}
				}
			}
		}
		return result;
	}

	public static Switch GetNearbySwitch(float x, float y)
	{
		foreach (Switch @switch in Map.switches)
		{
			if (@switch != null)
			{
				float f = @switch.x - x;
				if (Mathf.Abs(f) < 16f)
				{
					float f2 = @switch.y - y;
					if (Mathf.Abs(f2) < 24f)
					{
						return @switch;
					}
				}
			}
		}
		Collider[] array = Physics.OverlapSphere(new Vector3(x, y), 8f, Map.switchesLayer);
		if (array.Length > 0)
		{
			return array[0].GetComponent<Switch>();
		}
		return null;
	}

	public static void DamageProjectile(Projectile projectile, int damage, DamageType damageType, float xI, float yI, float delay, int newPlayerNum)
	{
		if (projectile != null)
		{
			projectile.Damage(damage, damageType, xI, yI, delay, newPlayerNum);
		}
	}

	public static void HeadShotUnit(MonoBehaviour damageSender, Unit unit, int damage, DamageType damageType, float xI, float yI, int direction, bool knock, float xHit, float yHit)
	{
		PolicyType policyType = DamagePolicy.GetPolicyType(damageSender);
		if (unit != null)
		{
			PolicyType policyType2 = DamagePolicy.GetPolicyType(unit);
			bool flag = DamagePolicy.AcceptDamage(policyType2, policyType);
			if (flag)
			{
				bool flag2 = DamagePolicy.NetworkDamage(policyType2, policyType);
				if (knock)
				{
					unit.Knock(damageType, xI, yI, false);
					if (flag2)
					{
						Networking.RPC<DamageType, float, float, bool>(PID.TargetOthers, new RpcSignature<DamageType, float, float, bool>(unit.Knock), damageType, xI, yI, false, false);
					}
				}
				unit.HeadShot(damage, damageType, xI, yI, direction, xHit, yHit, damageSender);
				if (flag2)
				{
					Networking.RPC<int, DamageType, float, float, int, float, float, MonoBehaviour>(PID.TargetOthers, new RpcSignature<int, DamageType, float, float, int, float, float, MonoBehaviour>(unit.HeadShot), damage, damageType, xI, yI, direction, xHit, yHit, damageSender, false);
				}
			}
		}
	}

	public static void KnockAndDamageUnit(MonoBehaviour damageSender, Unit unit, int damage, DamageType damageType, float xI, float yI, int direction, bool knock)
	{
		Map.KnockAndDamageUnit(damageSender, unit, damage, damageType, xI, yI, direction, knock, -100f, -100f);
	}

	public static void KnockAndDamageUnit(MonoBehaviour damageSender, Unit unit, int damage, DamageType damageType, float xI, float yI, int direction, bool knock, float hitX, float hitY)
	{
		if (unit != null)
		{
			PolicyType policyType = DamagePolicy.GetPolicyType(damageSender);
			if (float.IsNaN(xI))
			{
				xI = 0f;
			}
			if (float.IsNaN(yI))
			{
				yI = 0f;
			}
			PolicyType policyType2 = DamagePolicy.GetPolicyType(unit);
			bool flag = DamagePolicy.AcceptDamage(policyType2, policyType);
			if (knock)
			{
				unit.Knock(damageType, xI, yI, false);
			}
			if (flag)
			{
				bool flag2 = DamagePolicy.NetworkDamage(policyType2, policyType);
				unit.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
				if (flag2)
				{
					Networking.RPC<int, DamageType, float, float, int, MonoBehaviour, float, float>(PID.TargetOthers, new RpcSignature<int, DamageType, float, float, int, MonoBehaviour, float, float>(unit.Damage), damage, damageType, xI, yI, direction, damageSender, hitX, hitY, false);
				}
			}
		}
	}

	public static void BurnUnitsAround_Local(MonoBehaviour firedBy, int playerNum, int damage, float range, float x, float y, bool penetrates, bool setGroundAlight)
	{
		int num = 999999;
		if (Demonstration.enemiesSetOnFire)
		{
			foreach (Unit unit in Map.units.ToArray())
			{
				if (unit != null && GameModeController.DoesPlayerNumDamage(playerNum, unit.playerNum) && !unit.invulnerable && unit.health <= num && unit != firedBy)
				{
					float num2 = unit.x - x;
					if (Mathf.Abs(num2) - range < unit.width && (unit.y != y || num2 != 0f))
					{
						float f = unit.y + unit.height / 2f + 3f - y;
						if (Mathf.Abs(f) - range < unit.height)
						{
							if (damage > 0 || unit.burnTime <= 0.5f)
							{
								PolicyType policyType = DamagePolicy.GetPolicyType(firedBy);
								PolicyType policyType2 = DamagePolicy.GetPolicyType(unit);
								if (DamagePolicy.AcceptBurn(policyType2, policyType))
								{
									unit.BurnInternal(damage, (int)Mathf.Sign(num2));
								}
							}
							if (!penetrates && unit.health > 0)
							{
								Map.hasHit = true;
								num = 0;
							}
							else
							{
								Map.hasHit = true;
							}
						}
					}
				}
			}
		}
		if (setGroundAlight && Map.blocks != null)
		{
			Collider[] array2 = Physics.OverlapSphere(new Vector3(x, y, 0f), range / 1.5f);
			foreach (Collider collider in array2)
			{
				collider.SendMessage("SetAlight_Local", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public static bool ReviveDeadUnits(float x, float y, float range, int playerNum, int maxReviveCount, bool isPlayerControlled, TestVanDammeAnim reviveSource)
	{
		bool flag = false;
		int num = 0;
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && num < maxReviveCount && unit.health <= 0 && !unit.invulnerable)
			{
				float num2 = unit.x - x;
				if (Mathf.Abs(num2) - range < unit.width && (unit.y != y || num2 != 0f))
				{
					float num3 = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(num3) - range < unit.height && num3 * num3 + num2 * num2 < range * range)
					{
						UnityEngine.Debug.Log(string.Concat(new object[]
						{
							"Try Revive ! ",
							playerNum,
							" unit ",
							unit.name,
							" isPlayerControlled ",
							isPlayerControlled
						}));
						bool flag2 = unit.Revive(playerNum, isPlayerControlled, reviveSource);
						if (flag2)
						{
							Networking.RPC<int, bool, TestVanDammeAnim>(PID.TargetOthers, new RpcSignature<int, bool, TestVanDammeAnim>(unit.ReviveRPC), playerNum, isPlayerControlled, reviveSource, false);
						}
						flag = (flag2 || flag);
						num++;
					}
				}
			}
		}
		return flag;
	}

	public static void StunUnits(int playerNum, float x, float y, float range, float stunTime)
	{
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && !unit.invulnerable && GameModeController.DoesPlayerNumDamage(playerNum, unit.playerNum))
			{
				float num = unit.x - x;
				if (Mathf.Abs(num) - range < unit.width && (unit.y != y || num != 0f))
				{
					float f = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f) - range < unit.height)
					{
						unit.Stun(stunTime);
					}
				}
			}
		}
	}

	public static void BlindUnits(int playerNum, float x, float y, float range)
	{
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && !unit.invulnerable && GameModeController.DoesPlayerNumDamage(playerNum, unit.playerNum))
			{
				float num = unit.x - x;
				if (Mathf.Abs(num) - range < unit.width && (unit.y != y || num != 0f))
				{
					float f = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f) - range < unit.height)
					{
						unit.Blind();
					}
				}
			}
		}
	}

	public static void AttractMooks(float x, float y, float xRange, float yRange)
	{
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && unit.playerNum < 0 && !unit.invulnerable)
			{
				float num = unit.x - x;
				if (Mathf.Abs(num) - xRange < unit.width && (unit.y != y || num != 0f))
				{
					float f = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f) - yRange < unit.height)
					{
						unit.Attract(x, y);
					}
				}
			}
		}
	}

	public static void AlertNearbyMooks(float x, float y, float xRange, float yRange, int playerNum, GridPoint startPoint)
	{
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && unit.playerNum < 0 && !unit.invulnerable)
			{
				float num = unit.x - x;
				if (Mathf.Abs(num) - xRange < unit.width && (unit.y != y || num != 0f))
				{
					float f = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f) - yRange < unit.height)
					{
						unit.FullyAlert(x, y, playerNum);
					}
				}
			}
		}
	}

	public static void AlertNearbyMooks(float x, float y, float xRange, float yRange, int playerNum)
	{
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && unit.playerNum < 0 && unit.health > 0 && !unit.invulnerable)
			{
				float num = unit.x - x;
				if (Mathf.Abs(num) - xRange < unit.width && (unit.y != y || num != 0f))
				{
					float f = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f) - yRange < unit.height)
					{
						unit.FullyAlert(x, y, playerNum);
					}
				}
			}
		}
	}

	public static void BotherNearbyMooks(float x, float y, float xRange, float yRange, int playerNum)
	{
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && unit.playerNum < 0 && unit.health > 0 && !unit.invulnerable && unit.canHear)
			{
				float num = unit.x - x;
				if (Mathf.Abs(num) - xRange < unit.width && (unit.y != y || num != 0f))
				{
					float f = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f) - yRange < unit.height)
					{
						unit.HearSound(x, y);
					}
				}
			}
		}
	}

	public static void RegisterFetchObject(float x, float y, float xRange, float yRange, Transform fetchTransform)
	{
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && unit.playerNum < 0 && unit.health > 0 && !unit.invulnerable && unit.canHear)
			{
				float num = unit.x - x;
				if (Mathf.Abs(num) < xRange && (unit.y != y || num != 0f))
				{
					float f = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f) < yRange)
					{
						unit.FetchObject(fetchTransform);
					}
				}
			}
		}
	}

	public static void PanicUnits(float x, float y, float range, bool forgetPlayer)
	{
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && unit.playerNum < 0 && !unit.invulnerable)
			{
				float num = unit.x - x;
				if (Mathf.Abs(num) - range < unit.width && (unit.y != y || num != 0f))
				{
					float f = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f) - range < unit.height)
					{
						unit.Panic((int)Mathf.Sign(num), 0.1f, forgetPlayer);
					}
				}
			}
		}
	}

	public static void PanicUnits(float x, float y, float range, float time, bool forgetPlayer)
	{
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && unit.playerNum < 0 && !unit.invulnerable)
			{
				float num = unit.x - x;
				if (Mathf.Abs(num) - range < unit.width && (unit.y != y || num != 0f))
				{
					float f = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f) - range < unit.height)
					{
						unit.Panic(time, forgetPlayer);
					}
				}
			}
		}
	}

	public static void PanicUnits(float x, float y, float xRange, float yRange, int direction, float time, bool forgetPlayer)
	{
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && unit.playerNum < 0 && !unit.invulnerable)
			{
				float num = unit.x - x;
				if (Mathf.Abs(num) - xRange < unit.width && (unit.y != y || num != 0f))
				{
					float f = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f) - yRange < unit.height)
					{
						unit.Panic(direction, time, forgetPlayer);
					}
				}
			}
		}
	}

	public static bool CheckHighFive(int playerNum, float x, float y, float xRange, float yRange)
	{
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && unit.playerNum != playerNum && unit.playerNum >= 0 && playerNum >= 0)
			{
				float num = unit.x + unit.transform.localScale.x * 8f - x;
				if (Mathf.Abs(num) < xRange && (unit.y != y || num != 0f))
				{
					float f = unit.y - y;
					if (Mathf.Abs(f) < yRange && unit.IsHighFiving())
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool CanRollOntoUnits(int collumn, int row, int direction)
	{
		float blocksX = Map.GetBlocksX(collumn);
		float blocksY = Map.GetBlocksY(row);
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && !unit.invulnerable)
			{
				float num = unit.x - blocksX;
				if (Mathf.Abs(num) - 8f < unit.width && (unit.y != blocksY || num != 0f))
				{
					float f = unit.y + unit.height / 2f + 3f - blocksY;
					if (Mathf.Abs(f) - 12f < unit.height && unit.IsHeavy())
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public static void RollOntoUnits(int collumn, int row, int direction)
	{
		float blocksX = Map.GetBlocksX(collumn);
		float blocksY = Map.GetBlocksY(row);
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && !unit.invulnerable)
			{
				float num = unit.x - blocksX;
				if (Mathf.Abs(num) - 8f < unit.width && (unit.y != blocksY || num != 0f))
				{
					float f = unit.y + unit.height / 2f + 3f - blocksY;
					if (Mathf.Abs(f) - 12f < unit.height)
					{
						if (!unit.IsHeavy())
						{
							unit.RollOnto(direction);
						}
					}
				}
			}
		}
	}

	public static void CrushUnitsAgainstWalls(MonoBehaviour damageSender, float x, float y, float range, int xDirection, int yDirection)
	{
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && !unit.invulnerable)
			{
				float num = unit.x - x;
				if (Mathf.Abs(num) - range < unit.width && (unit.y != y || num != 0f))
				{
					float f = unit.y + unit.height / 2f + 3f - y;
					if (Mathf.Abs(f) - range < unit.height)
					{
						if (Map.IsBlockSolid(unit.x + unit.width * (float)xDirection / 2f, unit.y + 1f + unit.height * (float)yDirection))
						{
							Map.KnockAndDamageUnit(damageSender, unit, 20, DamageType.Crush, 0f, 0f, 0, false, x, y);
						}
						else if (xDirection > 0)
						{
							if (unit.x - unit.width * 0.5f < x)
							{
								unit.x = x + unit.width * 0.5f;
							}
						}
						else if (xDirection < 0 && unit.x + unit.width * 0.5f > x)
						{
							unit.x = x - unit.width * 0.5f;
						}
					}
				}
			}
		}
	}

	public static int BurnBlocksAround(int damage, int collumn, int row, bool forced)
	{
		DamageObject value = new DamageObject(damage, DamageType.Fire, 0f, 0f, null);
		int num = 0;
		Vector3 origin = Map.GetBlockCenter(collumn, row);
		RaycastHit raycastHit;
		if (Physics.Raycast(origin, Vector3.up, out raycastHit, 9f, Map.groundLayer))
		{
			num++;
			if (forced)
			{
				raycastHit.collider.gameObject.SendMessage("ForceBurn", SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				raycastHit.collider.gameObject.SendMessage("Burn", value, SendMessageOptions.DontRequireReceiver);
			}
		}
		if (Physics.Raycast(origin, Vector3.down, out raycastHit, 9f, Map.groundLayer))
		{
			num++;
			if (forced)
			{
				raycastHit.collider.gameObject.SendMessage("ForceBurn", SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				raycastHit.collider.gameObject.SendMessage("Burn", value, SendMessageOptions.DontRequireReceiver);
			}
		}
		if (Physics.Raycast(origin, Vector3.left, out raycastHit, 9f, Map.groundLayer))
		{
			num++;
			if (forced)
			{
				raycastHit.collider.gameObject.SendMessage("ForceBurn", SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				raycastHit.collider.gameObject.SendMessage("Burn", value, SendMessageOptions.DontRequireReceiver);
			}
		}
		if (Physics.Raycast(origin, Vector3.right, out raycastHit, 9f, Map.groundLayer))
		{
			num++;
			if (forced)
			{
				raycastHit.collider.gameObject.SendMessage("ForceBurn", SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				raycastHit.collider.gameObject.SendMessage("Burn", value, SendMessageOptions.DontRequireReceiver);
			}
		}
		return num;
	}

	public static bool ExplodeUnits(MonoBehaviour damageSender, int damage, DamageType damageType, float range, float killRange, float x, float y, float force, float yI, int playerNum, bool forceTumble, bool knockSelf)
	{
		bool result = false;
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && !unit.invulnerable)
			{
				float num = unit.x - x;
				if (Mathf.Abs(num) < range)
				{
					float num2 = unit.y + unit.height / 2f - y;
					if (Mathf.Abs(num2) < range)
					{
						float num3 = Mathf.Sqrt(num * num + num2 * num2);
						if (num3 < killRange)
						{
							if (unit.playerNum < 0 || GameModeController.DoesPlayerNumDamage(playerNum, unit.playerNum))
							{
								if (Mathf.Abs(num) < range * 0.4f)
								{
									num *= 0.66f;
									yI *= 1.5f;
								}
								Map.KnockAndDamageUnit(damageSender, unit, damage, damageType, num / num3 * force, num2 / num3 * force + yI, (int)Mathf.Sign(num), false, x, y);
							}
							else if (damageSender != unit || knockSelf)
							{
								if (num3 < 65f)
								{
									num3 = 65f;
								}
								unit.Knock(damageType, num / num3 * force, num2 / num3 * force + yI, forceTumble);
							}
							result = true;
						}
						else if (num3 < range)
						{
							if (num3 < 65f)
							{
								num3 = 65f;
							}
							if (damageSender != unit || GameModeController.DoesPlayerNumDamage(playerNum, unit.playerNum))
							{
								unit.Knock(damageType, num / num3 * force, num2 / num3 * force + yI, true);
								if (GameModeController.DoesPlayerNumDamage(playerNum, unit.playerNum))
								{
									Map.KnockAndDamageUnit(damageSender, unit, 0, damageType, num / num3 * force, num2 / num3 * force + yI, 0, forceTumble, x, y);
								}
								result = true;
							}
						}
					}
				}
			}
		}
		return result;
	}

	public static Unit GetNearestUnitWithXBias(int playerNum, int range, float x, float y, bool includeDead)
	{
		Map.nearestDist = (float)range;
		Map.nearestUnit = null;
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && (includeDead || unit.health > 0) && playerNum == unit.playerNum)
			{
				float num = Mathf.Abs(unit.x - x);
				if (num < Map.nearestDist)
				{
					float num2 = Mathf.Abs(unit.y + 6f - y);
					if (num2 < Map.nearestDist)
					{
						float num3 = num + num2;
						if (num3 < Map.nearestDist)
						{
							Map.nearestUnit = unit;
							Map.nearestDist = num3;
						}
					}
				}
			}
		}
		return Map.nearestUnit;
	}

	public static Unit GetNearestUnit(int playerNum, int range, float x, float y, bool includeDead)
	{
		float num = (float)range;
		Unit result = null;
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && (includeDead || unit.health > 0) && playerNum == unit.playerNum)
			{
				float num2 = Mathf.Abs(unit.x - x);
				float num3 = Mathf.Abs(unit.y + 6f - y);
				if (num2 + num3 < num)
				{
					float num4 = num2 + num3;
					if (num4 < num)
					{
						result = unit;
						num = num4;
					}
				}
			}
		}
		return result;
	}

	public static Unit GetNearestVisibleUnit(int playerNum, int range, float x, float y, bool includeDead)
	{
		float num = (float)range;
		Unit result = null;
		foreach (Unit unit in Map.units.ToArray())
		{
			if (unit != null && (includeDead || unit.health > 0) && playerNum == unit.playerNum)
			{
				float num2 = unit.x - x;
				float num3 = unit.y + 6f - y;
				if (Mathf.Abs(num2) < num && Mathf.Abs(num3) < num)
				{
					Vector3 direction = new Vector3(num2, num3, 0f);
					float magnitude = direction.magnitude;
					if (magnitude < num && !Physics.Raycast(new Vector3(x, y, 0f), direction, magnitude + 2f, Map.groundLayer))
					{
						result = unit;
						num = magnitude;
					}
				}
			}
		}
		return result;
	}

	public static bool IsUnitInRange(Unit unit, int range, float x, float y)
	{
		if (unit != null)
		{
			float num = Mathf.Abs(unit.x - x);
			if (num < (float)range)
			{
				float num2 = Mathf.Abs(unit.y + 6f - y);
				if (num2 < (float)range)
				{
					float num3 = num + num2;
					if (num3 < (float)range)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static CheckPoint GetNearestCheckPoint(int range, float x, float y)
	{
		Map.nearestDist = (float)range;
		Map.nearestCheckPoint = null;
		foreach (CheckPoint checkPoint in Map.checkPoints.ToArray())
		{
			if (checkPoint != null)
			{
				float num = Mathf.Abs(checkPoint.x - x);
				if (num < Map.nearestDist)
				{
					float num2 = Mathf.Abs(checkPoint.y + 6f - y);
					if (num2 < Map.nearestDist)
					{
						float num3 = num + num2;
						if (num3 < Map.nearestDist)
						{
							Map.nearestCheckPoint = checkPoint;
							Map.nearestDist = num3;
						}
					}
				}
			}
		}
		return Map.nearestCheckPoint;
	}

	public static CheckPoint GetNearestCheckPointToRight(float x, float y, bool onlyUnactivatedCheckpoints)
	{
		Map.nearestDist = float.PositiveInfinity;
		CheckPoint result = null;
		foreach (CheckPoint checkPoint in Map.checkPoints)
		{
			if (checkPoint != null && !checkPoint.activated && checkPoint.x > x)
			{
				float num = Mathf.Abs(checkPoint.x - x);
				if (num < Map.nearestDist)
				{
					float num2 = Mathf.Abs(checkPoint.y + 6f - y);
					if (num2 < Map.nearestDist)
					{
						float num3 = num + num2;
						if (num3 < Map.nearestDist && (!onlyUnactivatedCheckpoints || !checkPoint.activated))
						{
							result = checkPoint;
							Map.nearestDist = num3;
						}
					}
				}
			}
		}
		return result;
	}

	public static RescueBro GetNearestRescueBro(float x, float y)
	{
		RescueBro result = null;
		float num = float.PositiveInfinity;
		if (HeroController.Instance != null && HeroController.Instance.rescueBros != null)
		{
			foreach (RescueBro rescueBro in HeroController.Instance.rescueBros)
			{
				if (rescueBro != null && rescueBro.gameObject.activeInHierarchy)
				{
					float num2 = Mathf.Abs(rescueBro.x - x);
					if (num2 < num)
					{
						float num3 = Mathf.Abs(rescueBro.y + 6f - y);
						if (num3 < num)
						{
							float num4 = num2 + num3;
							if (num4 < num)
							{
								result = rescueBro;
								num = num4;
							}
						}
					}
				}
			}
		}
		return result;
	}

	public static bool IsFinished()
	{
		return Map.finished;
	}

	public static bool FinishLevel()
	{
		if (Map.finished)
		{
			return false;
		}
		Map.finished = true;
		if (!LevelSelectionController.loadCustomCampaign)
		{
			PlayerProgress.Instance.SetLastFinishedLevel(Map.levelNum);
			PlayerProgress.Save();
		}
		GameModeController.LevelFinish(LevelResult.Unknown);
		if (Map.levelNum >= Map.levelsLength)
		{
			Map.levelNum = 0;
			return true;
		}
		return false;
	}

	public static void ExitLevel()
	{
		Map.blocks = null;
	}

	public static void ContinueLevel()
	{
		Map.ClearSuperCheckpointStatus();
		int lastFinishedLevel = PlayerProgress.Instance.GetLastFinishedLevel(true);
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Continue Level! ",
			lastFinishedLevel,
			" out of ",
			Map.levelsCount
		}));
		if (lastFinishedLevel >= 0)
		{
			LevelSelectionController.CurrentLevelNum = lastFinishedLevel;
		}
		else
		{
			LevelSelectionController.CurrentLevelNum = 0;
		}
	}

	public static void ClearSuperCheckpointStatus()
	{
		Map.startFromSuperCheckPoint = false;
		Map.superCheckpointStartPos.r = -1; Map.superCheckpointStartPos.c = (Map.superCheckpointStartPos.r );
		Map.nextYLoadOffset = 0; Map.nextXLoadOffset = (Map.nextYLoadOffset );
	}

	private void Awake()
	{
		Map.highestSolidBlock = 0;
		Map.highestSolidBlockLadder = 0;
		Map.groundLayer = 1 << LayerMask.NameToLayer("Ground");
		Map.platformLayer = 1 << LayerMask.NameToLayer("Platform");
		Map.ladderLayer = 1 << LayerMask.NameToLayer("Ladders");
		Map.victoryLayer = 1 << LayerMask.NameToLayer("Finish");
		Map.barrierLayer = 1 << LayerMask.NameToLayer("MobileBarriers");
		Map.fragileLayer = 1 << LayerMask.NameToLayer("DirtyHippie");
		Map.switchesLayer = 1 << LayerMask.NameToLayer("Switches");
		this.activeTheme = this.jungleTheme;
		Map.finished = false;
		Map.units = new List<Unit>();
		Map.mookDoors = new List<MookDoor>();
		Map.switches = new List<Switch>();
		Map.wildLife = new List<WildLife>();
		Map.disturbedWildLife = new List<WildLife>();
		Map.checkPoints = new List<CheckPoint>();
		Map.projectiles = new List<Projectile>();
		Map.grenades = new List<Grenade>();
		Map.shootableGrenades = new List<Grenade>();
		Map.damageableProjectiles = new List<Projectile>();
		Map.spawnPoints = new List<SpawnPoint>();
		Map.destroyableDoodads = new List<Doodad>();
		Map.decalDoodads = new List<Doodad>();
		Map.staticDoodads = new List<Doodad>();
		Map.enemyDeathListeners = new List<Trigger>();
		Map.treeFoliage = new List<TreeFoliage>();
		Map.damageableScenery = new List<DamageableScenery>();
		this.taggedObjects = new Dictionary<string, GameObject>();
		if (Connect.IsOffline)
		{
			Networking.SetSeed(NonDeterministicRandom.Range(0, 100000));
		}
		Map.nextLevelToLoad = LevelSelectionController.CurrentLevelNum;
		ParallaxController.ResetParallaxGlobalValues(0f);
		base.StartCoroutine(this.SetupBlocksCoroutine());
	}

	private void Start()
	{
		if (Connect.IsHost)
		{
			Networking.InstantiateSceneOwned<GameObject>(Map.Instance.activeTheme.helicopterPrefab.gameObject, new object[]
			{
				true
			}, false);
		}
	}

	private IEnumerator SetupBlocksCoroutine()
	{
		this.waitingForSeed = false;
		UnityEngine.Random.seed = Networking.RandomSeed;
		Map.spawnPointOffset = UnityEngine.Random.Range(0, 55);
		Map.woodBlockCount = UnityEngine.Random.Range(0, 100);
		if (Application.isEditor && this.startLevel >= 0 && this.forceTestLevel)
		{
			Map.nextLevelToLoad = this.startLevel;
			this.startLevel = -1;
		}
		if (Map.nextLevelToLoad == -1)
		{
			Map.nextLevelToLoad = 0;
		}
		LevelSelectionController.CurrentLevelNum = Map.nextLevelToLoad;
		Map.MapData = LevelSelectionController.GetCurrentMap();
		if (Map.MapData.forcedBros != null && Map.MapData.forcedBros.Count == 0)
		{
			Map.MapData.forcedBros = null;
		}
		FluidController.Instance.Setup(Map.Width, Map.Height);
		this.LoadArea();
		if (GameModeController.GameMode == GameMode.ExplosionRun)
		{
			SortOfFollow.GetInstance().moveSpeed = Map.MapData.cameraSpeed;
			if (Map.MapData.cameraFollowMode == CameraFollowMode.ForcedHorizontal || Map.MapData.cameraFollowMode == CameraFollowMode.ForcedVertical)
			{
				SortOfFollow.GetInstance().followMode = Map.MapData.cameraFollowMode;
			}
		}
		if (GameModeController.GameMode == GameMode.SuicideHorde)
		{
			SortOfFollow.GetInstance().moveSpeed = Map.MapData.cameraSpeed;
			if (Map.MapData.cameraFollowMode == CameraFollowMode.ForcedHorizontal || Map.MapData.cameraFollowMode == CameraFollowMode.ForcedVertical || Map.MapData.cameraFollowMode == CameraFollowMode.Horizontal || Map.MapData.cameraFollowMode == CameraFollowMode.Vertical)
			{
				SortOfFollow.GetInstance().followMode = Map.MapData.cameraFollowMode;
			}
		}
		if (GameModeController.GameMode == GameMode.Race)
		{
			if (Map.MapData.cameraFollowMode == CameraFollowMode.Normal)
			{
				Map.MapData.cameraFollowMode = CameraFollowMode.Horizontal;
			}
			SortOfFollow.GetInstance().followMode = Map.MapData.cameraFollowMode;
		}
		ParallaxFollow parallax = UnityEngine.Object.Instantiate(this.activeTheme.sky, new Vector3(0f, 600f, 16f), Quaternion.identity) as ParallaxFollow;
		parallax.SetFollow(this.followCameraTransform);
		parallax.transform.parent = base.transform;
		if (this.activeTheme.cloudScatterType == CloudScatterType.BlueSky)
		{
			for (int i = 0; i < 5; i++)
			{
				for (int cloudIndex = 0; cloudIndex < this.activeTheme.parallaxClouds.Length; cloudIndex++)
				{
					int cloudIAmount = cloudIndex + 1;
					parallax = (UnityEngine.Object.Instantiate(this.activeTheme.parallaxClouds[cloudIndex], new Vector3((float)(-1024 + i * 1024) + UnityEngine.Random.value * 768f, (float)(230 + cloudIAmount * cloudIAmount * 20 + cloudIAmount * 60) + UnityEngine.Random.value * (float)(34 + cloudIAmount * 100), 8f), Quaternion.identity) as ParallaxFollow);
					parallax.transform.parent = base.transform;
					parallax.SetFollow(this.followCameraTransform);
				}
			}
		}
		else
		{
			for (int j = 0; j < 32; j++)
			{
				float randomNumber = UnityEngine.Random.value;
				parallax = (UnityEngine.Object.Instantiate(this.activeTheme.parallaxClouds[(int)Mathf.Clamp(randomNumber * (float)this.activeTheme.parallaxClouds.Length, 0f, (float)(this.activeTheme.parallaxClouds.Length - 1))], new Vector3((float)(-1024 + j * 256) + UnityEngine.Random.value * 256f, -32f + (randomNumber * 2f + UnityEngine.Random.value) * 768f * 0.5f, 16f), Quaternion.identity) as ParallaxFollow);
				parallax.SetFollow(this.followCameraTransform);
				parallax.transform.parent = base.transform;
			}
		}
		if (SortOfFollow.instance != null)
		{
			SortOfFollow.ForcePosition(Map.FindStartLocation());
		}
		else
		{
			MonoBehaviour.print(" No SortOfFollow instance");
		}
		Helicopter.DropOffHeliInstance = (UnityEngine.Object.Instantiate(Map.Instance.activeTheme.helicopterPrefab) as Helicopter);
		Registry.RegisterDeterminsiticGameObject(Helicopter.DropOffHeliInstance.gameObject);
		HeroTransport transport = UnityEngine.Object.Instantiate(Map.Instance.activeTheme.heroTransportPrefab) as HeroTransport;
		Registry.RegisterDeterminsiticGameObject(transport.gameObject);
		Connect.AllDeterminsiticObjectsHaveBeenRegistered();
		this.hasBeenSetup = true;
		yield return null;
		yield break;
	}

	private static bool ShouldTheBrosArriveByHeli(Vector3 startLocation)
	{
		while (startLocation.x > 0f)
		{
			Block block = Map.GetBlock(startLocation);
			if (block != null)
			{
				return true;
			}
			Block block2 = Map.GetBlock(startLocation - Extensions.Vec2toVec3(new Vector2(0f, 16f)));
			if (block2 == null)
			{
				return true;
			}
			startLocation.x -= 16f;
		}
		return false;
	}

	public static void AddBroToHeroTransport(TestVanDammeAnim Bro)
	{
		Vector3 startLocation = Map.FindStartLocation();
		bool flag = GameModeController.IsDeathMatchMode || (Map.startFromSuperCheckPoint || Map.MapData.heroSpawnMode == HeroSpawnMode.Helicopter) || (Map.MapData.heroSpawnMode != HeroSpawnMode.Truck && Map.ShouldTheBrosArriveByHeli(startLocation));
		if (flag)
		{
			if (Helicopter.DropOffHeliInstance == null)
			{
				Helicopter.CreateDropoffInstance();
			}
			if (!Helicopter.DropOffHeliInstance.hasBeenCalledDown)
			{
				Map.CallInTransport(startLocation, true);
			}
			if (Helicopter.DropOffHeliInstance != null && !Helicopter.DropOffHeliInstance.hasReleasedHeros)
			{
				Helicopter.DropOffHeliInstance.AddBroToTransport(Bro);
			}
			else
			{
				UnityEngine.Debug.LogError("Could not spawn transport");
			}
		}
		else
		{
			if (!HeroTransport.instance.hasEnteredLevel)
			{
				Map.CallInTransport(startLocation, false);
			}
			if (HeroTransport.instance != null)
			{
				HeroTransport.AddBroToTransport(Bro);
			}
		}
	}

	public static void CallInTransport(Vector3 startLocation, bool ArriveByHelicopter)
	{
		Networking.RPC<Vector3, bool>(PID.TargetAll, new RpcSignature<Vector3, bool>(Map.Instance.CallInTransport_RPC), startLocation, ArriveByHelicopter, false);
	}

	[RPC]
	private void CallInTransport_RPC(Vector3 startLocation, bool ArriveByHelicopter)
	{
		if (ArriveByHelicopter)
		{
			Helicopter.DropOffHeliInstance.Enter(startLocation, 0f);
			Helicopter.DropOffHeliInstance.SetToDroppingHerosOff();
		}
		else
		{
			HeroTransport.EnterLevel(startLocation);
		}
	}

	public static void SpawnFaceHugger(float x, float y, float xI, float yI)
	{
		Map.Instance.SpawnFaceHuggerInternal(x, y, xI, yI);
	}

	public void SpawnFaceHuggerInternal(float x, float y, float xI, float yI)
	{
		TestVanDammeAnim alienFaceHugger = this.activeTheme.alienFaceHugger;
		Mook mook = (UnityEngine.Object.Instantiate(alienFaceHugger, new Vector3(x, y + 8f, 0f), Quaternion.identity) as TestVanDammeAnim) as Mook;
		mook.actionState = ActionState.Jumping;
		mook.KnockSimple(new DamageObject(0, DamageType.Knock, xI, yI, null));
		mook.yI = yI;
	}

	public GameObject PlaceDoodad(DoodadInfo doodad)
	{
		if ((doodad.position.c < Map.Width && doodad.position.r < Map.Height && doodad.position.c >= Map.lastXLoadOffset && doodad.position.r >= Map.lastYLoadOffset) || doodad.type == DoodadType.TreeFoliageBackground || doodad.type == DoodadType.Parallax1 || doodad.type == DoodadType.Parallax2 || doodad.type == DoodadType.Parallax3 || doodad.type == DoodadType.Cloud)
		{
			GridPos position = doodad.position;
			position.c -= Map.lastXLoadOffset;
			position.r -= Map.lastYLoadOffset;
			GameObject gameObject = null;
			Vector3 vector = new Vector3((float)(position.c * 16), (float)(position.r * 16), 5f);
			bool flag = false;
			int num;
			if (doodad.variation >= this.GetDoodadVariationAmount(doodad.type) || doodad.variation < 0)
			{
				flag = true;
				num = UnityEngine.Random.Range(0, this.GetDoodadVariationAmount(doodad.type));
			}
			else
			{
				num = doodad.variation;
			}
			if (this.GetDoodadVariationAmount(doodad.type) == 0)
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"NO VARIATIONS FOR THIS DOODAD IN THIS THEME ",
					doodad.type.ToString(),
					", ",
					doodad.type
				}));
				return null;
			}
			switch (doodad.type)
			{
			case DoodadType.Cage:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabCage, vector, Quaternion.identity) as Cage).gameObject;
				gameObject.GetComponent<Cage>().row = position.r;
				gameObject.GetComponent<Cage>().collumn = position.c;
				break;
			case DoodadType.CageEmpty:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabCageEmpty, vector, Quaternion.identity) as Cage).gameObject;
				gameObject.GetComponent<Cage>().row = position.r;
				gameObject.GetComponent<Cage>().collumn = position.c;
				Map.blocks[position.c, position.r] = gameObject.GetComponent<Cage>();
				Map.blocks[position.c + 1, position.r] = gameObject.GetComponent<Cage>();
				Map.blocks[position.c, position.r - 1] = gameObject.GetComponent<Cage>();
				Map.blocks[position.c + 1, position.r - 1] = gameObject.GetComponent<Cage>();
				break;
			case DoodadType.CheckPoint:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.checkPointPrefab, vector, Quaternion.identity) as CheckPoint).gameObject;
				break;
			case DoodadType.PureEvil:
				if (num == 0)
				{
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.satan, vector, Quaternion.identity) as TestVanDammeAnim).gameObject;
				}
				else if (num == 1)
				{
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.conradBroneBanks, vector, Quaternion.identity) as TestVanDammeAnim).gameObject;
				}
				else
				{
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.mookGeneral, vector, Quaternion.identity) as TestVanDammeAnim).gameObject;
				}
				break;
			case DoodadType.Mook:
			{
				if (flag)
				{
					float value = UnityEngine.Random.value;
					if (value < Map.MapData.suicideMookSpawnProbability)
					{
						num = 1;
					}
					else if (value < Map.MapData.suicideMookSpawnProbability + Map.MapData.riotShieldMookSpawnProbability)
					{
						num = 2;
					}
					else if (value < Map.MapData.suicideMookSpawnProbability + Map.MapData.riotShieldMookSpawnProbability + Map.MapData.bigMookSpawnProbability)
					{
						num = 3;
					}
					else
					{
						num = 0;
					}
				}
				TestVanDammeAnim original = null;
				if (num == 0)
				{
					original = this.activeTheme.mook;
				}
				if (num == 1)
				{
					original = this.activeTheme.mookSuicide;
				}
				if (num == 2)
				{
					original = this.activeTheme.mookRiotShield;
				}
				if (num == 3)
				{
					original = this.activeTheme.mookBigGuy;
				}
				if (num == 4)
				{
					original = this.activeTheme.mookScout;
				}
				if (num == 5)
				{
					original = this.activeTheme.mookDog;
				}
				if (num == 6)
				{
					original = this.activeTheme.mookArmoured;
				}
				if (num == 7)
				{
					original = this.activeTheme.mookGrenadier;
				}
				if (num == 8)
				{
					original = this.activeTheme.mookBazooka;
				}
				if (num == 9)
				{
					original = this.activeTheme.mookBazooka;
				}
				if (num == 10)
				{
					original = this.activeTheme.mookMortar;
				}
				gameObject = (UnityEngine.Object.Instantiate(original, vector, Quaternion.identity) as TestVanDammeAnim).gameObject;
				break;
			}
			case DoodadType.Alien:
			{
				if (flag)
				{
					num = UnityEngine.Random.Range(0, 2);
				}
				TestVanDammeAnim original2 = null;
				if (num == 0)
				{
					original2 = this.activeTheme.alienFaceHugger;
				}
				if (num == 1)
				{
					original2 = this.activeTheme.alienXenomorph;
				}
				gameObject = (UnityEngine.Object.Instantiate(original2, vector, Quaternion.identity) as TestVanDammeAnim).gameObject;
				break;
			}
			case DoodadType.Vehicle:
				switch (num)
				{
				case 0:
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.mookTankRockets, vector, Quaternion.identity) as Unit).gameObject;
					break;
				case 1:
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.mookTankMookLauncher, vector, Quaternion.identity) as Unit).gameObject;
					break;
				case 2:
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.mookTruck, vector, Quaternion.identity) as Unit).gameObject;
					break;
				case 3:
					if (this.activeTheme.mookKopter != null)
					{
						gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.mookKopter, vector, Quaternion.identity) as Unit).gameObject;
					}
					else
					{
						gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.mookTruck, vector, Quaternion.identity) as Unit).gameObject;
					}
					break;
				case 4:
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.mookDrillCarrier, vector, Quaternion.identity) as Unit).gameObject;
					break;
				case 5:
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.mookArtilleryTruck, vector, Quaternion.identity) as Unit).gameObject;
					break;
				case 6:
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.mookClonePod, vector, Quaternion.identity) as Doodad).gameObject;
					break;
				default:
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.mookTankRockets, vector, Quaternion.identity) as Unit).gameObject;
					break;
				}
				break;
			case DoodadType.Animal:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.animals[num], vector, Quaternion.identity) as Animal).gameObject;
				break;
			case DoodadType.Miniboss:
				switch (num)
				{
				case 0:
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.mookMammothTank, vector, Quaternion.identity) as Unit).gameObject;
					break;
				case 1:
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.mookKopterMiniBoss, vector, Quaternion.identity) as Unit).gameObject;
					break;
				case 2:
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.mookDolfLundgren, vector, Quaternion.identity) as Unit).gameObject;
					break;
				case 3:
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.mookKopterMammoth, vector, Quaternion.identity) as Unit).gameObject;
					break;
				case 4:
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.goliathMech, vector, Quaternion.identity) as Unit).gameObject;
					break;
				}
				break;
			case DoodadType.MookDoor:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.mookDoorPrefab, vector, Quaternion.identity) as MookDoor).gameObject;
				break;
			case DoodadType.BackgroundWindowFactory:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.backgroundwindowsFactory[num], vector, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.BackgroundWindowStone:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.backgroundwindowsStone[num], vector, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.PetrolTanker:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.petrolTanker, vector, Quaternion.identity) as DoodadPetrolTank).gameObject;
				break;
			case DoodadType.OutdoorDoodad:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.outdoorDoodads[num], vector, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.IndoorDoodad:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.indoorDoodads[num], vector, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.TreeBushes:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.treeBushes[num], vector, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.Tree:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.trees[num], vector + Vector3.forward * UnityEngine.Random.value * 0.05f, Quaternion.identity) as GameObject);
				break;
			case DoodadType.Parallax3:
			{
				ParallaxFollow parallaxFollow = UnityEngine.Object.Instantiate(this.activeTheme.parallax3[num], vector, Quaternion.identity) as ParallaxFollow;
				parallaxFollow.SetFollow(this.followCameraTransform);
				gameObject = parallaxFollow.gameObject;
				break;
			}
			case DoodadType.Parallax2:
			{
				ParallaxFollow parallaxFollow = UnityEngine.Object.Instantiate(this.activeTheme.parallax2[num], vector, Quaternion.identity) as ParallaxFollow;
				parallaxFollow.SetFollow(this.followCameraTransform);
				gameObject = parallaxFollow.gameObject;
				break;
			}
			case DoodadType.Parallax1:
			{
				ParallaxFollow parallaxFollow = UnityEngine.Object.Instantiate(this.activeTheme.parallax1[num], vector, this.activeTheme.parallax1[num].transform.rotation) as ParallaxFollow;
				parallaxFollow.SetFollow(this.followCameraTransform);
				gameObject = parallaxFollow.gameObject;
				break;
			}
			case DoodadType.Cloud:
			{
				ParallaxFollow parallaxFollow = UnityEngine.Object.Instantiate(this.activeTheme.parallaxCloudDoodads[num], vector, Quaternion.identity) as ParallaxFollow;
				parallaxFollow.SetFollow(this.followCameraTransform);
				gameObject = parallaxFollow.gameObject;
				break;
			}
			case DoodadType.TreeBackground:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.treesBackground[num], vector + Vector3.forward * UnityEngine.Random.value * 0.05f, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.TreeBushBackground:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.treeBushesBackground[num], vector + Vector3.forward * UnityEngine.Random.value * 0.05f, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.Trap:
				if (flag)
				{
					float num2 = UnityEngine.Random.value * (Map.MapData.spikeTrapSpawnProbability + Map.MapData.mineFieldSpawnProbability);
					if (num2 > 0f && num2 < Map.MapData.spikeTrapSpawnProbability)
					{
						num = 0;
					}
					else if (num2 >= Map.MapData.spikeTrapSpawnProbability && num2 < Map.MapData.mineFieldSpawnProbability)
					{
						num = 1;
					}
					else
					{
						num2 = (float)Mathf.RoundToInt(UnityEngine.Random.value * (float)this.activeTheme.trapDoodads.Length);
					}
				}
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.trapDoodads[num], vector, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.Fence:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.fenceDoodads[num], vector, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.Pole:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.poleDoodads[num], vector, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.Switch:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.switchPrefab, vector, Quaternion.identity) as Switch).gameObject;
				if (doodad.triggerInfo == null)
				{
					doodad.triggerInfo = new TriggerInfo();
					doodad.triggerInfo.type = TriggerType.Entity;
				}
				gameObject.GetComponent<Switch>().triggerInfo = doodad.triggerInfo;
				break;
			case DoodadType.Alarm:
				UnityEngine.Debug.Log("Create Alarm " + num);
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.alarmDoodads[num % this.activeTheme.alarmDoodads.Length], vector, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.Door:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.doorDoodads[num], vector, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.Scaffolding:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.scaffoldingForegroundDoodads[num], vector, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.ScaffoldingBackground:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.scaffoldingBackgroundDoodads[num], vector, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.HangingDoodads:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.hangingDoodads[num], vector, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.TerrainRotators:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.terrainHelpers[num], vector, Quaternion.identity) as TerrainHelper).gameObject;
                //gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.terrainHelpers[num], vector, Quaternion.identity) as GameObject);
				break;
			case DoodadType.WaterSource:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.waterSourceDoodads[num], vector, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.VerticalCheckPoint:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.verticalCheckpointPrefab, vector, Quaternion.identity) as SuperCheckpoint).gameObject;
				break;
			case DoodadType.HorizontalCheckPoint:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.horizontalCheckpointPrefab, vector, Quaternion.identity) as SuperCheckpoint).gameObject;
				break;
			case DoodadType.HiddenExplosives:
				break;
			case DoodadType.SpawnPoint:
				if (num == 1)
				{
					gameObject = (UnityEngine.Object.Instantiate(this.spawnPointInvisiblePrefab, vector, Quaternion.identity) as SpawnPoint).gameObject;
				}
				else
				{
					gameObject = (UnityEngine.Object.Instantiate(this.spawnPointPrefab, vector, Quaternion.identity) as SpawnPoint).gameObject;
					GridPos pos = new GridPos(position.c - 1, position.r + 1);
					GameObject gameObject2 = this.PlaceDoodad(new DoodadInfo(pos, DoodadType.CageEmpty));
					if (gameObject2 == null)
					{
						MonoBehaviour.print("cage is null");
					}
					if (gameObject2.GetComponent<CageTemporary>() == null)
					{
						MonoBehaviour.print("no cage component");
					}
					if (gameObject.GetComponent<SpawnPoint>() == null)
					{
						MonoBehaviour.print("spawnpoint is null");
					}
					gameObject.GetComponent<SpawnPoint>().cage = gameObject2.GetComponent<CageTemporary>();
				}
				break;
			case DoodadType.CheckPointRunHorizontal:
				if (num == 1)
				{
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.checkPointRunHorizontalFastPrefab, vector, Quaternion.identity) as CheckPoint).gameObject;
				}
				else
				{
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.checkPointRunHorizontalPrefab, vector, Quaternion.identity) as CheckPoint).gameObject;
				}
				break;
			case DoodadType.CheckPointRunVertical:
				if (num == 1)
				{
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.checkPointRunVerticalFastPrefab, vector, Quaternion.identity) as CheckPoint).gameObject;
				}
				else
				{
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.checkPointRunVerticalPrefab, vector, Quaternion.identity) as CheckPoint).gameObject;
				}
				break;
			case DoodadType.CheckPointRunDescent:
				if (num == 1)
				{
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.checkPointRunDescentFastPrefab, vector, Quaternion.identity) as CheckPoint).gameObject;
				}
				else
				{
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.checkPointRunDescentPrefab, vector, Quaternion.identity) as CheckPoint).gameObject;
				}
				break;
			case DoodadType.Signpost:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.signPostDoodad, vector, Quaternion.identity) as SpriteSM).gameObject;
				gameObject.GetComponent<DoodadSignPost>().message = doodad.tag;
				break;
			case DoodadType.PureEvilDecor:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.pureEvilDecor[num], vector, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.TreeFoliageBackground:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.foliageBackground[num], vector + Vector3.forward * vector.y / 5120f, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.Villager:
			{
				TestVanDammeAnim original3 = this.activeTheme.villager1[num];
				gameObject = (UnityEngine.Object.Instantiate(original3, vector, Quaternion.identity) as TestVanDammeAnim).gameObject;
				break;
			}
			case DoodadType.Zipline:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.ziplineDoodads[num], vector, Quaternion.identity) as ZiplinePoint).gameObject;
				break;
			case DoodadType.AlienGiantSandWorm:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.alienGiantSandWorm, vector, Quaternion.identity) as AlienGiantSandWorm).gameObject;
				break;
			case DoodadType.AlienSpawner:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.alienSpawnerPrefab, vector, Quaternion.identity) as AlienSpawner).gameObject;
				break;
			case DoodadType.Crate:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.crateDoodads[num], vector, Quaternion.identity) as SpriteSM).gameObject;
				break;
			case DoodadType.WallOfGuns:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.wallOfGuns[num], vector, Quaternion.identity) as GameObject).gameObject;
				break;
			case DoodadType.Elevator:
				gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.elevators[num], vector, Quaternion.identity) as Elevator).gameObject;
				break;
			case DoodadType.ExpendabrosBoss:
				switch (num)
				{
				case 0:
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.expendabrosBoss, vector, Quaternion.identity) as GameObject).gameObject;
					break;
				case 1:
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.expendabrosBossStation, vector, Quaternion.identity) as BossRailStation).gameObject;
					break;
				case 2:
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.expendabrosBossRail, vector, Quaternion.identity) as GameObject).gameObject;
					break;
				case 3:
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.expendabrosExplosionChase, vector, Quaternion.identity) as GameObject).gameObject;
					break;
				default:
					gameObject = (UnityEngine.Object.Instantiate(this.activeTheme.expendabrosBoss, vector, Quaternion.identity) as GameObject).gameObject;
					break;
				}
				break;
			default:
				UnityEngine.Debug.LogWarning("Did not place doodad of type: " + doodad.type);
				break;
			}
			if (gameObject != null)
			{
				doodad.entity = gameObject;
				gameObject.transform.parent = base.transform;
				Registry.RegisterDeterminsiticGameObject(gameObject.gameObject);
				return gameObject;
			}
		}
		return null;
	}

	public int GetDoodadVariationAmount(DoodadType type)
	{
		switch (type)
		{
		case DoodadType.PureEvil:
			return 3;
		case DoodadType.Mook:
			return 9;
		case DoodadType.Alien:
			return 2;
		case DoodadType.Vehicle:
			return 7;
		case DoodadType.Animal:
			return this.activeTheme.animals.Length;
		case DoodadType.Miniboss:
			return 5;
		case DoodadType.BackgroundWindowFactory:
			return this.activeTheme.backgroundwindowsFactory.Length;
		case DoodadType.BackgroundWindowStone:
			return this.activeTheme.backgroundwindowsStone.Length;
		case DoodadType.OutdoorDoodad:
			return this.activeTheme.outdoorDoodads.Length;
		case DoodadType.IndoorDoodad:
			return this.activeTheme.indoorDoodads.Length;
		case DoodadType.TreeBushes:
			return this.activeTheme.treeBushes.Length;
		case DoodadType.Tree:
			return this.activeTheme.trees.Length;
		case DoodadType.Parallax3:
			return this.activeTheme.parallax3.Length;
		case DoodadType.Parallax2:
			return this.activeTheme.parallax2.Length;
		case DoodadType.Parallax1:
			return this.activeTheme.parallax1.Length;
		case DoodadType.Cloud:
			return this.activeTheme.parallaxCloudDoodads.Length;
		case DoodadType.TreeBackground:
			return this.activeTheme.treesBackground.Length;
		case DoodadType.TreeBushBackground:
			return this.activeTheme.treeBushesBackground.Length;
		case DoodadType.Trap:
			return this.activeTheme.trapDoodads.Length;
		case DoodadType.Fence:
			return this.activeTheme.fenceDoodads.Length;
		case DoodadType.Pole:
			return this.activeTheme.poleDoodads.Length;
		case DoodadType.Alarm:
			return this.activeTheme.alarmDoodads.Length;
		case DoodadType.Door:
			return this.activeTheme.doorDoodads.Length;
		case DoodadType.Scaffolding:
			return this.activeTheme.scaffoldingForegroundDoodads.Length;
		case DoodadType.ScaffoldingBackground:
			return this.activeTheme.scaffoldingBackgroundDoodads.Length;
		case DoodadType.HangingDoodads:
			return this.activeTheme.hangingDoodads.Length;
		case DoodadType.TerrainRotators:
			return this.activeTheme.terrainHelpers.Length;
		case DoodadType.SpawnPoint:
			return 2;
		case DoodadType.PureEvilDecor:
			return this.activeTheme.pureEvilDecor.Length;
		case DoodadType.TreeFoliageBackground:
			return this.activeTheme.foliageBackground.Length;
		case DoodadType.Villager:
			return this.activeTheme.villager1.Length;
		case DoodadType.Zipline:
			return this.activeTheme.ziplineDoodads.Length;
		case DoodadType.Crate:
			return this.activeTheme.crateDoodads.Length;
		case DoodadType.WallOfGuns:
			return this.activeTheme.wallOfGuns.Length;
		case DoodadType.Elevator:
			return this.activeTheme.elevators.Length;
		case DoodadType.ExpendabrosBoss:
			return 4;
		}
		return 1;
	}

	public static Vector3 GetBlocksXYPosition(int collumn, int row)
	{
		float blocksX = Map.GetBlocksX(collumn);
		float blocksY = Map.GetBlocksY(row);
		return new Vector3(blocksX, blocksY, 0f);
	}

	public static void GetBlocksXY(ref float x, ref float y, int row, int collumn)
	{
		x = Map.GetBlocksX(collumn);
		y = Map.GetBlocksY(row);
	}

	public static void GetRowCollumn(float x, float y, ref int row, ref int collumn)
	{
		row = Map.GetRow(y);
		collumn = Map.GetCollumn(x);
	}

	public static int GetRow(float y)
	{
		return (int)((y + 8f) / 16f);
	}

	public static int GetCollumn(float x)
	{
		return (int)((x + 8f) / 16f);
	}

	public static GridPoint GetGridPoint(Vector3 pos)
	{
		return new GridPoint(Map.GetCollumn(pos.x), Map.GetRow(pos.y));
	}

	public static float GetBlocksY(int row)
	{
		return (float)(row * 16 - 8);
	}

	public static float GetBlocksX(int collumn)
	{
		return (float)(collumn * 16 - 8);
	}

	public static Vector2 GetBlockCenter(int collumn, int row)
	{
		return new Vector2((float)collumn * 16f, (float)row * 16f);
	}

	public static bool IsBlockInvulnerable(int collumn, int row)
	{
		return Map.blocks[collumn, row] != null && Map.blocks[collumn, row].groundType == GroundType.Steel;
	}

	public static bool IsBlockSolid(float x, float y)
	{
		int row = Map.GetRow(y);
		int collumn = Map.GetCollumn(x);
		return Map.IsBlockSolid(collumn, row);
	}

	public static bool AssignBlock(Block block, int collumn, int row)
	{
		if (Map.blocks[collumn, row] == null || !Map.blocks[collumn, row].NotBroken() || Map.blocks[collumn, row].groundType == GroundType.Ladder)
		{
			Map.blocks[collumn, row] = block;
			return true;
		}
		return false;
	}

	public static bool SetBlockEmpty(Block block, int collumn, int row)
	{
		if (Map.blocks[collumn, row] == block)
		{
			Map.blocks[collumn, row] = null;
			return true;
		}
		UnityEngine.Debug.LogError(string.Concat(new object[]
		{
			"Not The Correct Block  Block is : ",
			block,
			"  Position is : ",
			Map.blocks[collumn, row],
			"  collumn ",
			collumn,
			" row ",
			row
		}));
		return false;
	}

	public static bool SetBackgroundBlockEmpty(Block block, int collumn, int row)
	{
		if (Map.backGroundBlocks[collumn, row] == block)
		{
			Map.backGroundBlocks[collumn, row] = null;
			return true;
		}
		UnityEngine.Debug.LogError(string.Concat(new object[]
		{
			"Not The Correct Background Block  Block is : ",
			block,
			"  Position is : ",
			Map.blocks[collumn, row],
			"  collumn ",
			collumn,
			" row ",
			row
		}));
		return false;
	}

	public static Block GetBlock(int collumn, int row)
	{
		if (collumn < 0 || collumn >= Map.blocks.GetUpperBound(0))
		{
			return null;
		}
		if (row < 0 || row >= Map.blocks.GetUpperBound(1))
		{
			return null;
		}
		return Map.blocks[collumn, row];
	}

	public static Block GetBackgroundBlock(int collumn, int row)
	{
		if (collumn < 0 || collumn >= Map.backGroundBlocks.GetUpperBound(0))
		{
			return null;
		}
		if (row < 0 || row >= Map.backGroundBlocks.GetUpperBound(1))
		{
			return null;
		}
		return Map.backGroundBlocks[collumn, row];
	}

	public static Block GetBlock(Vector2 worldXY)
	{
		return Map.blocks[Map.GetCollumn(worldXY.x), Map.GetCollumn(worldXY.y)];
	}

	public static GroundType GetGroundType(int collumn, int row, GroundType currentGroundType = GroundType.Empty)
	{
		if (collumn < 0 || collumn >= Map.blocks.GetUpperBound(0))
		{
			return currentGroundType;
		}
		if (row < 0 || row >= Map.blocks.GetUpperBound(1))
		{
			return currentGroundType;
		}
		if (Map.blocks[collumn, row] != null)
		{
			return Map.blocks[collumn, row].groundType;
		}
		return currentGroundType;
	}

	public static GroundType GetBackgroundGroundType(int collumn, int row, GroundType currentGroundType = GroundType.Empty)
	{
		if (collumn < 0 || collumn >= Map.blocks.GetUpperBound(0))
		{
			return currentGroundType;
		}
		if (row < 0 || row >= Map.blocks.GetUpperBound(1))
		{
			return currentGroundType;
		}
		if (Map.backGroundBlocks[collumn, row] != null)
		{
			return Map.backGroundBlocks[collumn, row].groundType;
		}
		if (Map.blocks[collumn, row] != null)
		{
			return Map.blocks[collumn, row].groundType;
		}
		return currentGroundType;
	}

	public static void ClearBackgroundBlock(int c, int r)
	{
		if (Map.backGroundBlocks[c, r] != null)
		{
			UnityEngine.Object.Destroy(Map.backGroundBlocks[c, r].gameObject);
		}
	}

	public static void ClearForegroundBlock(int c, int r)
	{
		if (Map.blocks[c, r] != null)
		{
			if (Map.blocks[c, r].height == 2 && r + 1 < Map.blocks.GetUpperBound(1))
			{
				Map.blocks[c, r + 1] = null;
			}
			UnityEngine.Object.Destroy(Map.blocks[c, r].gameObject);
			Map.blocks[c, r] = null;
		}
	}

	public static void RotateBlock(int c, int r, int direction)
	{
		if (Map.blocks[c, r] != null)
		{
			Map.blocks[c, r].Rotate(direction);
		}
	}

	public static bool IsWithinBounds(int c, int r)
	{
		return c >= 0 && c < Map.Width && r >= 0 && r < Map.Height;
	}

	public static bool PushBlock(int collumn, int row, float xI)
	{
		if (Map.isEditing)
		{
			return false;
		}
		if (collumn >= 0 && collumn < Map.Width && row >= 0 && row < Map.Height)
		{
			if (!(Map.blocks[collumn, row] != null))
			{
				return false;
			}
			if (Map.blocks[collumn, row].NotBroken())
			{
				Map.blocks[collumn, row].Push(xI);
				return true;
			}
		}
		return false;
	}

	public static bool DoesForegroundBlockHaveBackground(GroundType foregroundType)
	{
		return foregroundType != GroundType.Ladder && foregroundType != GroundType.Bridge2 && foregroundType != GroundType.Bridge && foregroundType != GroundType.Barrel && foregroundType != GroundType.BuriedRocket && foregroundType != GroundType.PropaneBarrel && foregroundType != GroundType.WoodenBlock && foregroundType != GroundType.TyreBlock && foregroundType != GroundType.HutScaffolding && foregroundType != GroundType.FallingBlock && foregroundType != GroundType.Beehive && foregroundType != GroundType.Empty && foregroundType != GroundType.Roof && foregroundType != GroundType.WatchTower && foregroundType != GroundType.ThatchRoof && foregroundType != GroundType.FactoryRoof;
	}

	public static bool DoesForegroundBlockHaveBackground(TerrainType foregroundType)
	{
		return foregroundType != TerrainType.Ladder && foregroundType != TerrainType.Bridge && foregroundType != TerrainType.Bridge2 && foregroundType != TerrainType.Barrel && foregroundType != TerrainType.BuriedRocket && foregroundType != TerrainType.PropaneBarrel && foregroundType != TerrainType.Wood && foregroundType != TerrainType.Tyre && foregroundType != TerrainType.HutScaffolding && foregroundType != TerrainType.Beehive && foregroundType != TerrainType.FallingBlock && foregroundType != TerrainType.Empty;
	}

	public static bool IsBlockSolid(int collumn, int row)
	{
		return Map.blocks != null && (collumn < 0 || collumn >= Map.Width || row < 0 || row >= Map.Height || (Map.blocks[collumn, row] != null && Map.blocks[collumn, row].groundType != GroundType.Ladder && Map.blocks[collumn, row].NotBroken()));
	}

	public static bool IsBlockSolidToWater(int collumn, int row)
	{
		if (Map.blocks == null)
		{
			return false;
		}
		if (collumn < 0 || collumn >= Map.Width || row < 0 || row >= Map.Height)
		{
			return true;
		}
		if (Map.blocks[collumn, row] != null)
		{
			GroundType groundType = Map.blocks[collumn, row].groundType;
			return groundType != GroundType.Ladder && groundType != GroundType.Bridge && groundType != GroundType.Bridge2 && Map.blocks[collumn, row].NotBroken();
		}
		return false;
	}

	public static bool IsBlockSolidTerrain(int collumn, int row)
	{
		return Map.blocks != null && (collumn < 0 || collumn >= Map.Width || row < 0 || row >= Map.Height || (Map.blocks[collumn, row] != null && Map.blocks[collumn, row].NotBroken() && (Map.blocks[collumn, row].groundType == GroundType.EarthTop || Map.blocks[collumn, row].groundType == GroundType.Earth || Map.blocks[collumn, row].groundType == GroundType.CaveRock || Map.blocks[collumn, row].groundType == GroundType.Sand || Map.blocks[collumn, row].groundType == GroundType.Brick || Map.blocks[collumn, row].groundType == GroundType.AlienEarth || Map.blocks[collumn, row].groundType == GroundType.Steel || Map.blocks[collumn, row].groundType == GroundType.Bunker || Map.blocks[collumn, row].groundType == GroundType.BigBlock || Map.blocks[collumn, row].groundType == GroundType.Statue)));
	}

	public static bool WasBlockEarth(int collumn, int row)
	{
		collumn += Map.lastXLoadOffset;
		row += Map.lastYLoadOffset;
		if (collumn < 0 || collumn >= Map.Width || row < 0 || row >= Map.Height)
		{
			return true;
		}
		if (Map.MapData.backgroundBlocks[collumn, row] != TerrainType.Empty)
		{
			return Map.MapData.backgroundBlocks[collumn, row] == TerrainType.BackgroundEarth || Map.MapData.backgroundBlocks[collumn, row] == TerrainType.BackgroundFactory || Map.MapData.backgroundBlocks[collumn, row] == TerrainType.BackgroundStone || Map.MapData.backgroundBlocks[collumn, row] == TerrainType.BackgroundStoneDoodads || Map.MapData.backgroundBlocks[collumn, row] == TerrainType.BackgroundBunker || Map.MapData.backgroundBlocks[collumn, row] == TerrainType.BackgroundShaft || Map.MapData.backgroundBlocks[collumn, row] == TerrainType.BackgroundBathroom;
		}
		return Map.MapData.foregroundBlocks[collumn, row] != TerrainType.Empty && (Map.MapData.foregroundBlocks[collumn, row] == TerrainType.Earth || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.CaveRock || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.Sand || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.Stone || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.Steel || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.Pipe || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.AssMouth || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.Bunker || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.BigBlock || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.Statue || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.Earth || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.AlienEarth || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.FallingBlock);
	}

	public static bool WasBlockOriginallySolid(int collumn, int row)
	{
		collumn += Map.lastXLoadOffset;
		row += Map.lastYLoadOffset;
		return collumn < 0 || collumn >= Map.Width || row < 0 || row >= Map.Height || (Map.MapData.foregroundBlocks[collumn, row] != TerrainType.Empty && (Map.MapData.foregroundBlocks[collumn, row] == TerrainType.Earth || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.CaveRock || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.Sand || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.Stone || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.Steel || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.Pipe || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.AssMouth || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.AlienEarth || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.Bunker || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.BigBlock || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.Statue || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.Earth || Map.MapData.foregroundBlocks[collumn, row] == TerrainType.FallingBlock));
	}

	public static bool IsTerrainTheSame(GroundType sourceGroundType, int collumn, int row)
	{
		return collumn >= 0 && collumn < Map.Width && row >= 0 && row < Map.Height && Map.blocks[collumn, row] != null && sourceGroundType == Map.blocks[collumn, row].groundType;
	}

	protected static bool IsTerrainCompatible(GroundType sourceGroundType, GroundType otherGroundType)
	{
		switch (sourceGroundType)
		{
		case GroundType.Sand:
			if (otherGroundType == GroundType.Sand)
			{
				return true;
			}
			break;
		case GroundType.Pipe:
			if (otherGroundType == GroundType.Pipe)
			{
				return true;
			}
			break;
		default:
			switch (sourceGroundType)
			{
			case GroundType.EarthTop:
			case GroundType.Earth:
				if (otherGroundType == GroundType.Earth || otherGroundType == GroundType.EarthTop)
				{
					return true;
				}
				break;
			default:
				if (sourceGroundType != GroundType.Brick)
				{
					if (sourceGroundType == GroundType.CaveRock)
					{
						if (otherGroundType == GroundType.CaveRock)
						{
							return true;
						}
					}
				}
				else if (otherGroundType == GroundType.Brick)
				{
					return true;
				}
				break;
			}
			break;
		case GroundType.HutScaffolding:
			if (otherGroundType == GroundType.HutScaffolding)
			{
				return true;
			}
			break;
		case GroundType.ThatchRoof:
			if (otherGroundType == GroundType.ThatchRoof)
			{
				return true;
			}
			break;
		case GroundType.WoodBackground:
			if (otherGroundType == GroundType.ThatchRoof || otherGroundType == GroundType.WoodBackground)
			{
				return true;
			}
			break;
		case GroundType.AlienEarth:
			if (otherGroundType == GroundType.AlienEarth)
			{
				return true;
			}
			break;
		case GroundType.AssMouth:
			if (otherGroundType == GroundType.AssMouth)
			{
				return true;
			}
			break;
		case GroundType.FactoryRoof:
			if (otherGroundType == GroundType.FactoryRoof)
			{
				return true;
			}
			break;
		}
		return false;
	}

	public static bool IsBlockSolidTerrain(GroundType groundType, int collumn, int row)
	{
		return collumn < 0 || collumn >= Map.Width || row < 0 || row >= Map.Height || (Map.blocks[collumn, row] != null && Map.IsTerrainCompatible(groundType, Map.blocks[collumn, row].groundType));
	}

	public static bool IsBlockLadder(int collumn, int row)
	{
		return collumn >= 0 && collumn < Map.Width && row >= 0 && row < Map.Height && Map.MapData.foregroundBlocks[collumn, row] == TerrainType.Ladder;
	}

	public static bool IsBlockLadder(float x, float y)
	{
		int collumn = Map.GetCollumn(x);
		int row = Map.GetRow(y);
		return Map.IsBlockLadder(collumn, row);
	}

	public static bool FindLadderNearPosition(float xPos, float yPos, ref float characterX, ref float characterY)
	{
		return Map.FindLadderNearPosition(xPos, yPos, 8, ref characterX, ref characterY);
	}

	public static bool FindLadderNearPosition(float xPos, float yPos, int range, ref float characterX, ref float characterY)
	{
		int row = Map.GetRow(yPos);
		int collumn = Map.GetCollumn(xPos);
		if (!Map.IsWithinBounds(collumn, row))
		{
			return false;
		}
		for (int i = collumn - range / 4; i < collumn + range; i++)
		{
			if (i >= 0 && i < Map.Width && Map.groundTypes[i, row] == GroundType.Ladder)
			{
				characterX = Map.GetBlocksX(i) + 8f;
				characterY = yPos;
				return true;
			}
		}
		for (int j = collumn - range; j < collumn - range / 4; j++)
		{
			if (j >= 0 && j < Map.Width && Map.groundTypes[j, row] == GroundType.Ladder)
			{
				characterX = Map.GetBlocksX(j);
				characterY = yPos;
				return true;
			}
		}
		return false;
	}

	public static bool FindHoleToJumpThroughAndAppear(float xPos, float yPos, ref float characterX, ref float characterY, ref int jumpDirection)
	{
		int row = Map.GetRow(yPos);
		int collumn = Map.GetCollumn(xPos);
		for (int i = collumn - 9; i < collumn + 9; i++)
		{
			if (i >= 0 && i < Map.Width)
			{
				for (int j = row; j < row + 3; j++)
				{
					if (j >= 0 && j < Map.Height && Map.IsBlockFloor(i, j))
					{
						if (Map.IsCollumnEmpty(i - 1, j + 1, -4))
						{
							characterX = Map.GetBlocksX(i - 1) + 8f;
							characterY = Map.GetBlocksY(j - 1) + 8f;
							jumpDirection = 1;
							return true;
						}
						if (Map.IsCollumnEmpty(i + 1, j + 1, -4))
						{
							characterX = Map.GetBlocksX(i + 1) + 8f;
							characterY = Map.GetBlocksY(j - 1) + 8f;
							jumpDirection = -1;
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	protected static bool IsBlockFloor(int c, int r)
	{
		return Map.IsBlockSolid(c, r) && !Map.IsBlockSolid(c, r + 1) && !Map.IsBlockSolid(c, r + 2);
	}

	protected static bool IsCollumnEmpty(int c, int r, int rowSpan)
	{
		if (rowSpan < 0)
		{
			for (int i = r; i >= r + rowSpan; i--)
			{
				if (i >= 0 && i < Map.Height && Map.IsBlockSolid(c, i))
				{
					return false;
				}
			}
		}
		else
		{
			for (int j = r; j <= r + rowSpan; j++)
			{
				if (j >= 0 && j < Map.Height && Map.IsBlockSolid(c, j))
				{
					return false;
				}
			}
		}
		return true;
	}

	public static int Width
	{
		get
		{
			if (Map.MapData != null)
			{
				return Map.MapData.Width;
			}
			return 0;
		}
	}

	public static int Height
	{
		get
		{
			if (Map.MapData != null)
			{
				return Map.MapData.Height;
			}
			return 0;
		}
	}

	public static int GetMaxCollumns()
	{
		return Map.Width;
	}

	public static int GetMaxRows()
	{
		return Map.Height;
	}

	protected void LoadArea()
	{
		if (!Map.isEditing)
		{
			Map.lastXLoadOffset = Map.nextXLoadOffset;
			Map.lastYLoadOffset = Map.nextYLoadOffset;
		}
		else
		{
			Map.nextYLoadOffset = 0; Map.lastXLoadOffset = (Map.lastYLoadOffset = (Map.nextXLoadOffset = (Map.nextYLoadOffset )));
		}
		Map.groundTypes = new GroundType[Map.Width, Map.Height];
		switch (Map.MapData.theme)
		{
		case LevelTheme.Jungle:
			this.activeTheme = this.jungleTheme;
			break;
		case LevelTheme.City:
			this.activeTheme = this.cityTheme;
			break;
		case LevelTheme.BurningJungle:
			this.activeTheme = this.burningJungleTheme;
			break;
		case LevelTheme.Forest:
			this.activeTheme = this.forestTheme;
			break;
		default:
			UnityEngine.Debug.LogError("Could not load in theme, defaulting to jungle");
			this.activeTheme = this.jungleTheme;
			break;
		}
		Map.blocks = new Block[Map.Width, Map.Height];
		Map.backGroundBlocks = new Block[Map.Width, Map.Height];
		this.randomOffset = UnityEngine.Random.Range(0, 55);
		for (int i = Map.lastXLoadOffset; i < Map.Width; i++)
		{
			for (int j = Map.lastYLoadOffset; j < Map.Height; j++)
			{
				try
				{
					GroundType foregroundGroundType = Map.MapData.GetForegroundGroundType(i, j);
					if (!Map.DoesForegroundBlockHaveBackground(foregroundGroundType))
					{
						this.PlaceGround(Map.MapData.GetBackgroundGroundType(i, j), i - Map.lastXLoadOffset, j - Map.lastYLoadOffset, ref Map.blocks);
					}
					this.PlaceGround(foregroundGroundType, i - Map.lastXLoadOffset, j - Map.lastYLoadOffset, ref Map.blocks);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError(string.Concat(new object[]
					{
						"Unable to place piece at ",
						i,
						", ",
						j,
						"\n",
						ex.Message
					}));
				}
			}
		}
		for (int k = 0; k < Map.Width; k++)
		{
			for (int l = 0; l < Map.Height; l++)
			{
				if (Map.blocks[k, l] != null && Map.blocks[k, l].size == 1)
				{
					if (Map.blocks[k, l] != null && (Map.blocks[k, l].groundType != GroundType.Bridge || Map.blocks[k, l].groundType != GroundType.Bridge2))
					{
						if (k > 0 && Map.blocks[k - 1, l] != null)
						{
							Map.blocks[k, l].HideLeft();
						}
						if (k < Map.MapData.Width - 1 && Map.blocks[k + 1, l] != null)
						{
							Map.blocks[k, l].HideRight();
						}
						if (l > 0 && Map.blocks[k, l - 1] != null)
						{
							Map.blocks[k, l].HideBelow();
						}
						if (l < Map.MapData.Height - 1 && Map.blocks[k, l + 1] != null)
						{
							Map.blocks[k, l].HideAbove();
						}
					}
					Block aboveBlock = null;
					Block belowBlock = null;
					if (l < Map.MapData.Height - 1 && Map.blocks[k, l + 1] != null)
					{
						aboveBlock = Map.blocks[k, l + 1];
					}
					if (l > 0 && Map.blocks[k, l - 1] != null)
					{
						belowBlock = Map.blocks[k, l - 1];
					}
					Map.blocks[k, l].SetupBlock(l, k, aboveBlock, belowBlock);
				}
			}
		}
		foreach (DoodadInfo doodadInfo in Map.MapData.DoodadList)
		{
			try
			{
				this.PlaceDoodad(doodadInfo);
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"Unable to place doodad ",
					doodadInfo.type,
					" at ",
					doodadInfo.position.ToString(),
					"\n",
					ex2.Message
				}));
			}
		}
		this.ResetZiplines();
		TriggerManager.LoadTriggers(Map.MapData.TriggerList);
		StatisticsController.NotifyLevelLoaded();
	}

	public static float GetGroundHeight(float x, float y)
	{
		float result = 0f;
		RaycastHit raycastHit;
		if (Physics.Raycast(new Vector3(x, y, 0f), Vector3.down, out raycastHit, (float)(1 << LayerMask.NameToLayer("Ground"))))
		{
			result = raycastHit.point.y;
		}
		else if (Physics.Raycast(new Vector3(x + 16f, y, 0f), Vector3.down, out raycastHit, (float)(1 << LayerMask.NameToLayer("Ground"))))
		{
			result = raycastHit.point.y;
		}
		else if (Physics.Raycast(new Vector3(x - 16f, y, 0f), Vector3.down, out raycastHit, (float)(1 << LayerMask.NameToLayer("Ground"))))
		{
			result = raycastHit.point.y;
		}
		return result;
	}

	public static void AddTaggedObject(GameObject obj, string tag)
	{
		if (string.IsNullOrEmpty(tag))
		{
			UnityEngine.Debug.LogError("Trying to add tagged object with null or empty tag");
		}
		else
		{
			Map.Instance.taggedObjects.Add(tag.ToUpper(), obj);
		}
	}

	public static GameObject GetDoodadByTag(string tag)
	{
		for (int i = 0; i < Map.MapData.DoodadList.Count; i++)
		{
			if (!string.IsNullOrEmpty(Map.MapData.DoodadList[i].tag) && tag.ToUpper().Equals(Map.MapData.DoodadList[i].tag.ToUpper()))
			{
				return Map.MapData.DoodadList[i].entity;
			}
		}
		return Map.Instance.taggedObjects[tag.ToUpper()];
	}

	public static List<GameObject> GetDoodadsByTag(string tag)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < Map.MapData.DoodadList.Count; i++)
		{
			if (!string.IsNullOrEmpty(Map.MapData.DoodadList[i].tag) && tag.ToUpper().Equals(Map.MapData.DoodadList[i].tag.ToUpper()))
			{
				list.Add(Map.MapData.DoodadList[i].entity);
			}
		}
		if (Map.Instance.taggedObjects.ContainsKey(tag.ToUpper()))
		{
			list.Add(Map.Instance.taggedObjects[tag.ToUpper()]);
		}
		return list;
	}

	public static Vector3 FindStartLocation()
	{
		Vector3 position = new Vector3(80f, 0f, 0f);
		if (Map.MapData != null)
		{
			position.y = (float)(Map.MapData.Height * 16);
		}
		if (Map.startFromSuperCheckPoint && Map.superCheckpointStartPos.c >= 0)
		{
			return Map.GetBlockCenter(Map.superCheckpointStartPos.c, Map.superCheckpointStartPos.r);
		}
		for (int i = 0; i < Map.Height; i++)
		{
			if (Map.GetBlock(Map.GetCollumn(position.x), Map.MapData.Height - i) != null)
			{
				position.y = (float)((Map.MapData.Height - i + 1) * 16 - 8);
				break;
			}
		}
		if (GameModeController.IsDeathMatchMode)
		{
			Map.deathMatchHelicopterCount++;
			if (Map.deathMatchHelicopterCount % 2 == 0)
			{
				position.x = SortOfFollow.minX + 48f;
			}
			else
			{
				position.x = SortOfFollow.maxX - 48f;
			}
			position.y = Map.GetBlocksY(Map.GetHighestSolidBlock() - 1);
		}
		if (GameModeController.GameMode == GameMode.SuicideHorde)
		{
			position = Map.GetSpawnPoint(0).transform.position;
		}
		return position;
	}

	protected void FindLargeBlockDimensions(int x, int y, ref int firstCollumn, ref int firstRow, ref int collumns, ref int rows, TerrainType currentTerrain, TerrainType[,] terrainTypes)
	{
		firstCollumn = x;
		firstRow = y;
		collumns = 1;
		rows = 1;
		int num = x;
		int num2 = y;
		bool flag = false;
		while (firstCollumn - 1 >= 0 && !flag)
		{
			if (terrainTypes[firstCollumn - 1, y] != currentTerrain)
			{
				flag = true;
			}
			else
			{
				firstCollumn--;
			}
		}
		flag = false;
		while (firstRow - 1 >= 0 && !flag)
		{
			if (terrainTypes[x, firstRow - 1] != currentTerrain)
			{
				flag = true;
			}
			else
			{
				firstRow--;
			}
		}
		flag = false;
		while (num + 1 < Map.MapData.Width && !flag)
		{
			if (terrainTypes[num + 1, y] != currentTerrain)
			{
				flag = true;
			}
			else
			{
				num++;
			}
		}
		flag = false;
		while (num2 + 1 < Map.MapData.Height && !flag)
		{
			if (terrainTypes[x, num2 + 1] != currentTerrain)
			{
				flag = true;
			}
			else
			{
				num2++;
			}
		}
		collumns = num - firstCollumn + 1;
		rows = num2 - firstRow + 1;
		if (collumns > 1 && rows > 1 && terrainTypes[firstCollumn + (collumns - 1), firstRow + (rows - 1)] != currentTerrain)
		{
			rows = 1; collumns = (rows );
			firstRow = y;
			firstCollumn = x;
		}
	}

	public Block PlaceGround(GroundType placeGroundType, int x, int y, ref Block[,] newBlocks)
	{
		Vector3 vector = new Vector3((float)(x * 16), (float)(y * 16), 5f);
		this.currentBlock = null;
		this.currentBackgroundBlock = null;
		if (y > Map.highestSolidBlock && placeGroundType != GroundType.Empty && placeGroundType != GroundType.Ladder)
		{
			Map.highestSolidBlock = y;
			Map.highestSolidBlockLadder = y + 5;
		}
		switch (placeGroundType)
		{
		case GroundType.EarthTop:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabEarthTop, vector, Quaternion.identity) as Block);
			break;
		case GroundType.EarthMiddle:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabEarthMiddle, vector, Quaternion.identity) as Block);
			break;
		case GroundType.Earth:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabEarth, vector, Quaternion.identity) as Block);
			break;
		case GroundType.EarthBehind:
			this.currentBackgroundBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabEarthBehind, vector, Quaternion.identity) as Block);
			break;
		case GroundType.BrickTop:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabBrickTop, vector, Quaternion.identity) as Block);
			break;
		case GroundType.BrickMiddle:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabBrickMiddle, vector, Quaternion.identity) as Block);
			break;
		case GroundType.Brick:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabBrick, vector, Quaternion.identity) as Block);
			break;
		case GroundType.BrickBehind:
			this.currentBackgroundBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabBrickBehind, vector, Quaternion.identity) as Block);
			break;
		case GroundType.Wall:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabWall, vector, Quaternion.identity) as Block);
			break;
		case GroundType.WallTop:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabWallTop, vector, Quaternion.identity) as Block);
			break;
		case GroundType.Bridge:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabBridge, vector, Quaternion.identity) as Block);
			break;
		case GroundType.Ladder:
			if (Map.IsBlockLadder(x, y + 1) && !Map.IsBlockFloor(x - 1, y) && !Map.IsBlockFloor(x + 1, y))
			{
				this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.ladderPrefabs[UnityEngine.Random.Range(0, this.activeTheme.ladderPrefabs.Length)], vector, Quaternion.identity) as Block);
			}
			else if (Map.IsBlockFloor(x - 1, y) && Map.IsBlockFloor(x + 1, y))
			{
				this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.ladderPrefabTopMiddle, vector, Quaternion.identity) as Block);
			}
			else if (Map.IsBlockFloor(x - 1, y))
			{
				this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.ladderPrefabTopLeft, vector, Quaternion.identity) as Block);
			}
			else if (Map.IsBlockFloor(x + 1, y))
			{
				this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.ladderPrefabTopRight, vector, Quaternion.identity) as Block);
			}
			else
			{
				this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.ladderPrefabTopAlone, vector, Quaternion.identity) as Block);
			}
			break;
		case GroundType.BigBlock:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabBigBlock, vector, Quaternion.identity) as Block);
			break;
		case GroundType.Mook:
			UnityEngine.Object.Instantiate(this.activeTheme.mook, vector + Vector3.up * 6f, Quaternion.identity);
			break;
		case GroundType.Barrel:
			if (Map.MapData.propaneTankSpawnProbability > UnityEngine.Random.value && y < Map.MapData.Height - 1 && Map.MapData.foregroundBlocks[x + Map.lastXLoadOffset, y + 1 + Map.lastYLoadOffset] == TerrainType.Empty)
			{
				this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabBarrels[2], vector, Quaternion.identity) as Block);
			}
			else if (Map.MapData.oilBarrelSpawnProbability > UnityEngine.Random.value)
			{
				this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabBarrels[1], vector, Quaternion.identity) as Block);
			}
			else
			{
				this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabBarrels[0], vector, Quaternion.identity) as Block);
			}
			break;
		case GroundType.PropaneBarrel:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabBarrels[2], vector, Quaternion.identity) as Block);
			break;
		case GroundType.FallingBlock:
			this.currentBackgroundBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabBrickBehind, vector, Quaternion.identity) as Block);
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabFalling, vector, Quaternion.identity) as Block);
			break;
		case GroundType.WoodenBlock:
			Map.woodBlockCount++;
			if (Map.woodBlockCount % (GameModeController.IsDeathMatchMode ? 15 : 23) != 0 || !Map.MapData.spawnAmmoCrates)
			{
				this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabWood[0], vector, Quaternion.identity) as Block);
			}
			else
			{
				Map.woodBlockCount += UnityEngine.Random.Range(0, 8);
				this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabWood[1], vector, Quaternion.identity) as Block);
			}
			break;
		case GroundType.Bunker:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabBunker, vector, Quaternion.identity) as Block);
			break;
		case GroundType.BunkerBehind:
			this.currentBackgroundBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabBunkerBehind, vector, Quaternion.identity) as Block);
			break;
		case GroundType.Steel:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabSteel, vector, Quaternion.identity) as Block);
			break;
		case GroundType.Roof:
			if (this.activeTheme.blockRoof != null)
			{
				this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockRoof, vector, Quaternion.identity) as Block);
			}
			else
			{
				UnityEngine.Debug.LogError("Why place roof in this theme?");
			}
			break;
		case GroundType.CaveRock:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabCaveRock, vector, Quaternion.identity) as Block);
			break;
		case GroundType.WatchTower:
			if (this.activeTheme.blockWatchTower != null)
			{
				this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockWatchTower, vector, Quaternion.identity) as Block);
			}
			break;
		case GroundType.Beehive:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockBeeHive, vector, Quaternion.identity) as Block);
			break;
		case GroundType.BathroomBehind:
			this.currentBackgroundBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabBathroomBehind, vector, Quaternion.identity) as Block);
			break;
		case GroundType.ShaftBehind:
			this.currentBackgroundBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabShaftBehind, vector, Quaternion.identity) as Block);
			break;
		case GroundType.AlienEgg:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockAlienEgg, vector, Quaternion.identity) as Block);
			break;
		case GroundType.Sand:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabSand, vector, Quaternion.identity) as Block);
			break;
		case GroundType.Pipe:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabPipe, vector, Quaternion.identity) as Block);
			break;
		case GroundType.BrickBehindDoodads:
		{
			this.currentBackgroundBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabBrickBehind, vector, Quaternion.identity) as Block);
			int firstCollumn = 0;
			int firstRow = 0;
			int collumns = 0;
			int rows = 0;
			this.FindLargeBlockDimensions(x + Map.lastXLoadOffset, y + Map.lastYLoadOffset, ref firstCollumn, ref firstRow, ref collumns, ref rows, Map.MapData.backgroundBlocks[x + Map.lastXLoadOffset, y + Map.lastYLoadOffset], Map.MapData.backgroundBlocks);
			this.currentBackgroundBlock.UseLargePieces(firstCollumn, firstRow, collumns, rows, this.randomOffset, true, false);
			break;
		}
		case GroundType.HutScaffolding:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabHutScaffolding, vector, Quaternion.identity) as Block);
			break;
		case GroundType.ThatchRoof:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabThatchRoof, vector, Quaternion.identity) as Block);
			break;
		case GroundType.WoodBackground:
			this.currentBackgroundBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabWoodenBackground, vector, Quaternion.identity) as Block);
			Registry.RegisterDeterminsiticGameObject(this.currentBackgroundBlock.gameObject);
			break;
		case GroundType.Statue:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabStatueBlock, vector, Quaternion.identity) as Block);
			break;
		case GroundType.AlienEarth:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabAlienEarth, vector, Quaternion.identity) as Block);
			break;
		case GroundType.AssMouth:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabAssMouth, vector, Quaternion.identity) as Block);
			break;
		case GroundType.BuriedRocket:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockBuriedRocket, vector, Quaternion.identity) as Block);
			break;
		case GroundType.TyreBlock:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockTyre, vector, Quaternion.identity) as Block);
			break;
		case GroundType.FactoryBehind:
			this.currentBackgroundBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabFactoryBehind, vector, Quaternion.identity) as Block);
			break;
		case GroundType.FactoryRoof:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabFactoryRoof, vector, Quaternion.identity) as Block);
			break;
		case GroundType.Bridge2:
			this.currentBlock = (UnityEngine.Object.Instantiate(this.activeTheme.blockPrefabBridge2, vector, Quaternion.identity) as Block);
			break;
		}
		newBlocks[x, y] = this.currentBlock;
		Map.groundTypes[x, y] = placeGroundType;
		if (this.currentBlock != null && this.currentBlock.groundType == GroundType.Earth && this.currentBlock.size == 2)
		{
			Map.SetBlockEmpty(newBlocks[x + 1, y], x + 1, y);
			newBlocks[x + 1, y] = this.currentBlock;
			Map.SetBlockEmpty(newBlocks[x, y - 1], x, y - 1);
			newBlocks[x, y - 1] = this.currentBlock;
			Map.SetBlockEmpty(newBlocks[x + 1, y - 1], x + 1, y - 1);
			newBlocks[x + 1, y - 1] = this.currentBlock;
		}
		if (this.currentBackgroundBlock != null)
		{
			Map.backGroundBlocks[x, y] = this.currentBackgroundBlock;
			this.currentBackgroundBlock.transform.parent = base.transform;
		}
		if (this.currentBlock != null)
		{
			this.currentBlock.transform.parent = base.transform;
			Registry.RegisterDeterminsiticGameObject(this.currentBlock.gameObject);
		}
		FluidController.RefreshFluidStatus(x, y);
		return this.currentBlock;
	}

	protected bool IsGroundEmpty(Texture2D tex, int x, int y)
	{
		this.tempEmptyColor = tex.GetPixel(x, y);
		return this.tempEmptyColor.a < 0.94f;
	}

	public static bool ConstrainToBlocks(float x, float y, float size, ref float xIT, ref float yIT, ref bool bounceX, ref bool bounceY)
	{
		Map.hasHit = false;
		if (xIT > 0f)
		{
			Map.xNew = x + xIT + size;
		}
		else if (xIT < 0f)
		{
			Map.xNew = x + xIT - size;
		}
		if (yIT > 0f)
		{
			Map.yNew = y + yIT + size;
		}
		else if (yIT < 0f)
		{
			Map.yNew = y + yIT - size;
		}
		int num = Mathf.FloorToInt((x + 8f) / 16f);
		int num2 = Mathf.FloorToInt((y + 8f) / 16f);
		int num3 = Mathf.FloorToInt((Map.xNew + 8f) / 16f);
		int num4 = Mathf.FloorToInt((Map.yNew + 8f) / 16f);
		if (Physics.OverlapSphere(new Vector3((float)(num * 16), (float)(num4 * 16), 0f), 0.1f, Map.groundLayer).Length > 0)
		{
			if (yIT < 0f)
			{
				yIT = (float)(num4 * 16 + 8) + size - y;
				Map.hasHit = true;
			}
			else if (yIT > 0f)
			{
				yIT = (float)(num4 * 16 - 8) - size - y;
				Map.hasHit = true;
			}
			bounceY = true;
		}
		if (Physics.OverlapSphere(new Vector3((float)(num3 * 16), (float)(num2 * 16), 0f), 0.1f, Map.groundLayer).Length > 0)
		{
			if (xIT < 0f)
			{
				xIT = (float)(num3 * 16 + 8) + size - x;
				Map.hasHit = true;
			}
			else if (xIT > 0f)
			{
				xIT = (float)(num3 * 16 - 8) - size - x;
				Map.hasHit = true;
			}
			bounceX = true;
		}
		if (!bounceX && !bounceY && xIT != 0f && yIT != 0f && Physics.OverlapSphere(new Vector3((float)(num3 * 16), (float)(num4 * 16), 0f), 0.1f, Map.groundLayer).Length > 0)
		{
			if (xIT < 0f)
			{
				xIT = (float)(num3 * 16 + 8) + size - x;
			}
			else if (xIT > 0f)
			{
				xIT = (float)(num3 * 16 - 8) - size - x;
			}
			bounceX = true;
			if (yIT < 0f)
			{
				yIT = (float)(num4 * 16 + 8) + size - y;
			}
			else if (yIT > 0f)
			{
				yIT = (float)(num4 * 16 - 8) - size - y;
			}
			bounceY = true;
			Map.hasHit = true;
		}
		return Map.hasHit;
	}

	public static bool InsideWall(float x, float y, float size)
	{
		return Physics.OverlapSphere(new Vector3(x, y, 0f), size / 2f, Map.groundLayer).Length > 0;
	}

	public static bool ConstrainToBlocks(float x, float y, float size)
	{
		Map.hasHit = false;
		int num = Mathf.FloorToInt((x + 8f) / 16f);
		int num2 = Mathf.FloorToInt((y + 8f) / 16f);
		if (Physics.OverlapSphere(new Vector3(x, y, 0f), size, Map.groundLayer).Length > 0)
		{
			Map.hasHit = true;
		}
		return Map.hasHit;
	}

	public static void StartLevelEndExplosionsOverNetwork()
	{
		if (!Map.Instance.explosionsHaveStarted)
		{
			Networking.RPC(PID.TargetAll, new RpcSignature(Map.Instance.StartLevelEndExplosions), false);
		}
	}

	private void StartLevelEndExplosions()
	{
		if (!this.explosionsHaveStarted)
		{
			this.explosionsHaveStarted = true;
			Map.Instance.StartCoroutine(Map.Instance.DoLevelEndExplosions());
		}
	}

	protected IEnumerator DoLevelEndExplosions()
	{
		for (;;)
		{
			for (int i = 0; i < 50; i++)
			{
				Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width * UnityEngine.Random.value * 1.4f - (float)Screen.width * 0.2f, (float)Screen.height * UnityEngine.Random.value * 1.4f - (float)Screen.height * 0.2f, 0f));
				int row = Map.GetRow(pos.y);
				int collumn = Map.GetCollumn(pos.x);
				if (row >= 0 && row < Map.Height && collumn >= 0 && collumn < Map.Width && (Map.IsBlockSolid(collumn, row) || Map.backGroundBlocks[collumn, row] != null || Map.IsBlockSolid(collumn, row - 2) || Map.IsBlockSolid(collumn, row - 2) || Map.IsBlockLadder(collumn, row)))
				{
					HiddenExplosives expl = UnityEngine.Object.Instantiate(this.hiddenExplosivePrefab, pos, Quaternion.identity) as HiddenExplosives;
					expl.Explode();
					i += 50;
				}
			}
			yield return new WaitForSeconds(0.1f + UnityEngine.Random.value * 0.3f);
		}
		yield break;
	}

	public static void MakeAllBlocksUltraTough()
	{
		for (int i = 0; i < Map.blocks.GetUpperBound(0); i++)
		{
			for (int j = 0; j < Map.blocks.GetUpperBound(1); j++)
			{
				if (Map.blocks[i, j] != null)
				{
					Map.blocks[i, j].health = 1000000;
				}
			}
		}
	}

	public static int GetDescentOffset(bool forceCollapse, int current)
	{
		return current;
	}

	public static int GetHighestSolidBlock()
	{
		return Map.highestSolidBlock;
	}

	public static int CollapseTop()
	{
		Map.highestSolidBlock--;
		if (Map.highestSolidBlock < 7)
		{
			Map.highestSolidBlock = 7;
		}
		for (int i = 0; i < Map.Width; i++)
		{
			for (int j = Map.highestSolidBlock + 1; j < Mathf.Min(Map.highestSolidBlock + 2, Map.Height); j++)
			{
				if (Map.blocks[i, j] != null && Map.blocks[i, j].groundType != GroundType.Barrel && Map.blocks[i, j].groundType != GroundType.BuriedRocket && Map.blocks[i, j].groundType != GroundType.PropaneBarrel && Map.blocks[i, j].groundType != GroundType.TyreBlock && Map.blocks[i, j].groundType != GroundType.WoodenBlock && Map.blocks[i, j].groundType != GroundType.Ladder)
				{
					Map.blocks[i, j].gameObject.SendMessage("Damage", new DamageObject(1, DamageType.InstaGib, 0f, 0f, null));
				}
			}
		}
		return Map.highestSolidBlock;
	}

	public static int CollapseTopLadders()
	{
		Map.highestSolidBlockLadder--;
		if (Map.highestSolidBlockLadder < 13)
		{
			Map.highestSolidBlock = 13;
		}
		for (int i = 0; i < Map.Width; i++)
		{
			for (int j = Map.highestSolidBlockLadder + 1; j < Mathf.Min(Map.highestSolidBlockLadder + 2, Map.Height); j++)
			{
				if (Map.blocks[i, j] != null && Map.blocks[i, j].groundType == GroundType.Ladder)
				{
					Map.blocks[i, j].gameObject.SendMessage("Damage", new DamageObject(1, DamageType.InstaGib, 0f, 0f, null));
				}
			}
		}
		return Map.highestSolidBlock;
	}

	public static bool IsInvulnerableAbove(float x, float y)
	{
		int collumn = Map.GetCollumn(x);
		int row = Map.GetRow(y);
		for (int i = row; i < Map.MapData.Height; i++)
		{
			if (Map.IsBlockInvulnerable(collumn, i))
			{
				return true;
			}
		}
		return false;
	}

	internal static ZiplinePoint GetOtherZiplinePoint(ZiplinePoint zipLinePoint)
	{
		int i = -1;
		for (i = 0; i < Map.MapData.DoodadList.Count; i++)
		{
			if (Map.MapData.DoodadList[i].entity == zipLinePoint.gameObject)
			{
				break;
			}
		}
		if (i < Map.MapData.DoodadList.Count)
		{
			GridPoint tagAsGridPoint = Map.MapData.DoodadList[i].TagAsGridPoint;
			if (tagAsGridPoint != null)
			{
				foreach (DoodadInfo doodadInfo in Map.MapData.DoodadList)
				{
					if (doodadInfo != null && doodadInfo.position.c == tagAsGridPoint.collumn && doodadInfo.position.r == tagAsGridPoint.row && doodadInfo.type == DoodadType.Zipline && doodadInfo.entity.GetComponent<ZiplinePoint>().otherPoint == null)
					{
						return doodadInfo.entity.GetComponent<ZiplinePoint>();
					}
				}
			}
		}
		int j = i - 1;
		while (j >= 0)
		{
			if (Map.MapData.DoodadList[j].type == DoodadType.Zipline)
			{
				if (Map.MapData.DoodadList[j].entity != null && Map.MapData.DoodadList[j].entity.GetComponent<ZiplinePoint>() != null && Map.MapData.DoodadList[j].entity.GetComponent<ZiplinePoint>().otherPoint == null)
				{
					return Map.MapData.DoodadList[j].entity.GetComponent<ZiplinePoint>();
				}
				return null;
			}
			else
			{
				j--;
			}
		}
		return null;
	}

	public void ResetZiplines()
	{
		foreach (DoodadInfo doodadInfo in Map.MapData.DoodadList)
		{
			if (doodadInfo.type == DoodadType.Zipline && doodadInfo.entity != null)
			{
				doodadInfo.entity.GetComponent<ZiplinePoint>().ResetZipline();
			}
		}
		foreach (DoodadInfo doodadInfo2 in Map.MapData.DoodadList)
		{
			if (doodadInfo2.type == DoodadType.Zipline && doodadInfo2.entity != null && doodadInfo2.TagAsGridPoint != null && doodadInfo2.entity.GetComponent<ZiplinePoint>() != null)
			{
				doodadInfo2.entity.GetComponent<ZiplinePoint>().otherPoint = Map.GetOtherZiplinePoint(doodadInfo2.entity.GetComponent<ZiplinePoint>());
				doodadInfo2.entity.GetComponent<ZiplinePoint>().SetupZipline();
			}
		}
		foreach (DoodadInfo doodadInfo3 in Map.MapData.DoodadList)
		{
			if (doodadInfo3.type == DoodadType.Zipline && doodadInfo3.entity != null && doodadInfo3.TagAsGridPoint == null)
			{
				doodadInfo3.entity.GetComponent<ZiplinePoint>().otherPoint = Map.GetOtherZiplinePoint(doodadInfo3.entity.GetComponent<ZiplinePoint>());
				doodadInfo3.entity.GetComponent<ZiplinePoint>().SetupZipline();
			}
		}
		MonoBehaviour.print("reset ziplines");
	}

	public static float GetUnitXOffset()
	{
		Map.unitOffsetCount++;
		return -5f + (float)(Map.unitOffsetCount % 4) / 3f * 6f + (float)(Map.unitOffsetCount % 7) / 8f * 2f + (float)(Map.unitOffsetCount % 25) / 24f * 2f;
	}

	internal static Vector3 GetBlockCenter(GridPoint gridPoint)
	{
		return Map.GetBlockCenter(gridPoint.collumn, gridPoint.row);
	}

	public ThemeHolder jungleTheme;

	public ThemeHolder cityTheme;

	public ThemeHolder burningJungleTheme;

	public ThemeHolder forestTheme;

	public ThemeHolder activeTheme;

	public SpawnPoint spawnPointPrefab;

	public SpawnPoint spawnPointInvisiblePrefab;

	public HiddenExplosives hiddenExplosivePrefab;

	public static Helicopter newestHelicopter;

	public Transform followCameraTransform;

	public static List<Unit> units;

	public static List<WildLife> wildLife;

	public static List<MookDoor> mookDoors;

	public static List<WildLife> disturbedWildLife;

	public static List<CheckPoint> checkPoints;

	public static List<Projectile> projectiles;

	public static List<Grenade> grenades;

	public static List<Grenade> shootableGrenades;

	public static List<Projectile> damageableProjectiles;

	public static List<SpawnPoint> spawnPoints;

	public static List<Doodad> decalDoodads;

	public static List<Doodad> destroyableDoodads;

	public static List<Doodad> staticDoodads;

	public static List<Switch> switches;

	public static List<Trigger> enemyDeathListeners;

	public static List<TreeFoliage> treeFoliage;

	public static List<DamageableScenery> damageableScenery;

	private Dictionary<string, GameObject> taggedObjects;

	public static LayerMask groundLayer;

	public static LayerMask platformLayer;

	public static LayerMask barrierLayer;

	public static LayerMask fragileLayer;

	public static LayerMask ladderLayer;

	public static LayerMask victoryLayer;

	public static LayerMask switchesLayer;

	protected static int levelNum;

	protected static int levelsCount;

	public static int nextLevelToLoad = -1;

	public static string LevelFileName;

	public static bool isEditing;

	private static Map inst;

	public string testLevelFileName;

	public bool forceTestLevel;

	public static MapData MapDataToLoad;

	public static int woodBlockCount;

	public static int nextXLoadOffset;

	public static int nextYLoadOffset;

	public static int lastXLoadOffset;

	public static int lastYLoadOffset;

	public static bool startFromSuperCheckPoint;

	public static GridPos superCheckpointStartPos;

	private bool hasBeenSetup;

	private static int spawnPointOffset;

	private bool explosionsHaveStarted;

	private static int highestSolidBlock;

	private static int highestSolidBlockLadder;

	[HideInInspector]
	public bool waitingForSeed;

	[HideInInspector]
	public bool waitingForSync;

	private static float nearestDist;

	private static Unit nearestUnit;

	private static CheckPoint nearestCheckPoint;

	public int startLevel;

	protected static bool finished;

	protected static int levelsLength;

	public bool reallySetupScene = true;

	public static Block[,] blocks;

	public static Block[,] backGroundBlocks;

	private static GroundType[,] groundTypes;

	protected static int deathMatchHelicopterCount;

	protected int randomOffset;

	protected GroundType placeGroundType;

	protected Block currentBlock;

	protected Block currentBackgroundBlock;

	protected Color tempEmptyColor;

	protected Color tempColor;

	protected GroundType tempGroundType;

	protected static float xNew;

	protected static float yNew;

	protected static bool hasHit;

	private int explCount;

	protected static int unitOffsetCount;
}
