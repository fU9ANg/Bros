// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MapController : SingletonMono<MapController>
{
	public static int StartLevel
	{
		get
		{
			return SingletonMono<MapController>.Instance.startLevel;
		}
		set
		{
			SingletonMono<MapController>.Instance.startLevel = value;
		}
	}

	public static bool DamageGround(MonoBehaviour damageSender, int damage, DamageType damageType, float range, float x, float y, Collider[] ignoreTheseColliders = null)
	{
		bool result = false;
		Collider[] array = Physics.OverlapSphere(new Vector3(x, y, 0f), range * 0.5f, Map.groundLayer);
		if (array.Length > 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				bool flag = false;
				if (ignoreTheseColliders != null)
				{
					for (int j = 0; j < ignoreTheseColliders.Length; j++)
					{
						if (array[i] == ignoreTheseColliders[j])
						{
							flag = true;
						}
					}
				}
				if (!flag)
				{
					MapController.Damage_Networked(damageSender, array[i].gameObject, damage, damageType, 0f, 0f);
					result = true;
				}
			}
		}
		return result;
	}

	public static bool DamageGround(MonoBehaviour damageSender, int damage, DamageType damageType, float width, float height, float x, float y, bool debugDraw, Collider[] ignoreTheseColliders = null)
	{
		if (debugDraw)
		{
			Extensions.DrawRect(new Vector3(x, y, 0f), width, height, Color.red, 0.1f);
		}
		bool result = false;
		width += 8f;
		height += 8f;
		Collider[] array = Physics.OverlapSphere(new Vector3(x, y, 0f), Mathf.Max(width * 2f, height * 2f) * 0.5f, Map.groundLayer);
		if (array.Length > 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				bool flag = false;
				if (ignoreTheseColliders != null)
				{
					for (int j = 0; j < ignoreTheseColliders.Length; j++)
					{
						if (array[i] == ignoreTheseColliders[j])
						{
							flag = true;
						}
					}
				}
				if (!flag)
				{
					Vector3 position = array[i].transform.position;
					if (position.x >= x - width / 2f && position.x <= x + width / 2f && position.y >= y - height / 2f && position.y <= y + height / 2f)
					{
						MapController.Damage_Networked(damageSender, array[i].gameObject, damage, damageType, 0f, 0f);
						result = true;
					}
				}
			}
		}
		return result;
	}

	public static void DamageBlock(MonoBehaviour damageSender, Block b, int damage, DamageType damageType, float forceX, float forceY)
	{
		MapController.Damage_Networked(damageSender, b.gameObject, damage, damageType, forceX, forceY);
	}

	public static void BurnUnitsAround_NotNetworked(MonoBehaviour firedBy, int playerNum, int damage, float range, float x, float y, bool penetrates, bool setGroundAlight)
	{
		Map.BurnUnitsAround_Local(firedBy, playerNum, damage, range, x, y, penetrates, setGroundAlight);
	}

	public static void SettleBlockRPC(Block block, float X, float Y, int colOffset, float final_zAngle)
	{
		FallingBlock fallingBlock = block as FallingBlock;
		if (fallingBlock != null)
		{
			if (RPCController.LastSender == fallingBlock.beingPushedByPlayer || fallingBlock.beingPushedByPlayer == PID.NoID)
			{
				fallingBlock.SettleBlockAfterRoll(X, Y, colOffset, final_zAngle, RPCController.LastTimeStamp, RPCController.LastSender);
			}
		}
		else
		{
			MonoBehaviour.print("SettleBlockRPC: Cannot find block: " + block);
		}
	}

	public static void SendRollBlockRPC(FallingBlock block, int direction)
	{
		Networking.RPC<FallingBlock, PID, double>(PID.TargetAll, new RpcSignature<FallingBlock, PID, double>(SingletonMono<MapController>.Instance.SetBlockBeingPushedByPlayer), block, PID.MyID, Connect.Time, false);
		Networking.RPC<int>(PID.TargetAll, new RpcSignature<int>(block.RollOverRPC), direction, false);
	}

	private void SetBlockBeingPushedByPlayer(Block block, PID player, double timeStamp)
	{
		FallingBlock fallingBlock = block as FallingBlock;
		if (fallingBlock != null)
		{
			if (timeStamp < fallingBlock.beingPushedByPlayerTimeStamp)
			{
				fallingBlock.beingPushedByPlayer = player;
				fallingBlock.beingPushedByPlayerTimeStamp = timeStamp;
			}
		}
		else
		{
			MonoBehaviour.print("SetBlockBeingPushedByPlayer: Cannot find block: " + fallingBlock);
		}
	}

	public static void BurnGround_Local(float range, float x, float y, int groundLayer)
	{
		Collider[] array = Physics.OverlapSphere(new Vector3(x, y, 0f), range, groundLayer);
		if (array.Length > 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				Block component = array[i].GetComponent<Block>();
				if (component != null)
				{
					component.ForceBurn();
				}
				else
				{
					array[i].SendMessage("ForceBurn", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	public static void Damage_Networked(MonoBehaviour damageSender, GameObject damageReciever, int damage, DamageType type, float forceX, float forceY)
	{
		DamageObject damageObject = new DamageObject(damage, type, forceX, forceY, damageSender);
		PolicyType policyType = DamagePolicy.GetPolicyType(damageReciever);
		PolicyType policyType2 = DamagePolicy.GetPolicyType(damageSender);
		bool flag = damageSender is Mookopter || damageSender is TankMassiveGun || damageSender is TankrocketBattery;
		if (DamagePolicy.AcceptDamage(policyType, policyType2) || flag)
		{
			damageReciever.SendMessage("Damage", damageObject);
		}
		if (DamagePolicy.NetworkDamage(policyType, policyType2) || flag)
		{
			Networking.RPC<string, DamageObject>(PID.TargetOthers, new RpcSignature<string, DamageObject>(damageReciever.SendMessage), "Damage", damageObject, false);
		}
	}

	public static void Damage_Local(MonoBehaviour damageSender, GameObject damageReciever, int damage, DamageType type, float forceX, float forceY)
	{
		DamageObject value = new DamageObject(damage, type, forceX, forceY, damageSender);
		PolicyType policyType = DamagePolicy.GetPolicyType(damageReciever);
		PolicyType policyType2 = DamagePolicy.GetPolicyType(damageSender);
		bool flag = damageSender is Mookopter || damageSender is TankMassiveGun || damageSender is MookSuicide || damageSender is TankrocketBattery || damageSender is Tank;
		if (DamagePolicy.AcceptDamage(policyType, policyType2) || flag)
		{
			damageReciever.SendMessage("Damage", value);
		}
	}

	public static Mook SpawnMook_Networked(Mook mookPrefab, float x, float y, float xI, float yI, bool tumble, bool useParachuteDelay, bool useParachute, bool onFire, bool isAlert)
	{
		Mook component = Networking.InstantiateSceneOwned<GameObject>(mookPrefab.gameObject, new Vector3(x, y, 0f), Quaternion.identity, null, true).GetComponent<Mook>();
		Networking.RPC<float, float, bool, bool, bool, bool, bool>(PID.TargetAll, new RpcSignature<float, float, bool, bool, bool, bool, bool>(component.SetSpawnState), xI, yI, tumble, useParachuteDelay, useParachute, onFire, isAlert, true);
		return component;
	}

	public static void SpawnResource_Networked(string resourceName, float x, float y, string callMethod, string tag = null)
	{
		GameObject gameObject = Resources.Load(resourceName) as GameObject;
		if (gameObject != null)
		{
			GameObject gameObject2 = Networking.InstantiateSceneOwned<GameObject>(gameObject, new Vector3(x, y, 0f), Quaternion.identity, null, false);
			if (gameObject2 != null && callMethod.Length > 1)
			{
				Networking.RPC<string>(PID.TargetAll, new RpcSignature<string>(gameObject2.SendMessage), callMethod, false);
			}
			if (gameObject2 != null && !string.IsNullOrEmpty(tag))
			{
				Map.AddTaggedObject(gameObject2, tag.ToUpper());
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Could not instantiate resource by name of " + resourceName);
		}
	}

	public HiddenExplosives explosionPrefab;

	public int startLevel;
}
