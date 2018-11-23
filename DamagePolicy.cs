// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[Serializable]
public class DamagePolicy : ScriptableObject
{
	private static DamagePolicy Instance
	{
		get
		{
			return DamagePolicy.instance;
		}
	}

	private void OnEnable()
	{
		DamagePolicy.instance = this;
	}

	private void Awake()
	{
		DamagePolicy.instance = this;
	}

	public static PolicyType GetPolicyType(MonoBehaviour mono)
	{
		Block x = mono as Block;
		if (x != null)
		{
			return PolicyType.Block;
		}
		if (mono is BossBlockWeapon)
		{
			return PolicyType.Block;
		}
		NetworkedUnit networkedUnit = mono as NetworkedUnit;
		if (networkedUnit != null)
		{
			if (networkedUnit.IsEnemy)
			{
				if (networkedUnit.IsLocalMook)
				{
					return PolicyType.Light_Local_Unit;
				}
				return PolicyType.Light_Remote_Unit;
			}
			else
			{
				TestVanDammeAnim testVanDammeAnim = networkedUnit as TestVanDammeAnim;
				if (testVanDammeAnim != null)
				{
					if (testVanDammeAnim.IsMine)
					{
						return PolicyType.Hero_Local;
					}
					return PolicyType.Hero_Replicant;
				}
			}
		}
		Parachute parachute = mono as Parachute;
		if (parachute != null)
		{
			if (parachute.mook.IsLocalMook)
			{
				return PolicyType.Light_Local_Unit;
			}
			return PolicyType.Light_Remote_Unit;
		}
		else
		{
			if (mono is MapController)
			{
				return PolicyType.Scene_Owned;
			}
			if (mono is HiddenExplosives)
			{
				return PolicyType.Scene_Owned;
			}
			if (mono is Doodad)
			{
				return PolicyType.Doodad;
			}
			if (mono is DoodadPiece)
			{
				return PolicyType.Doodad;
			}
			if (mono is HeroTransport)
			{
				return PolicyType.Scene_Owned;
			}
			if (mono is Mine)
			{
				return PolicyType.Scene_Owned;
			}
			if (Connect.IsRichardsPC)
			{
				UnityEngine.Debug.Log("unknown " + mono);
			}
			return PolicyType.Unknown;
		}
	}

	public static PolicyType GetPolicyType(GameObject go)
	{
		if (go.GetComponentInHeirarchy<Block>() != null)
		{
			return PolicyType.Block;
		}
		if (go.GetComponentInHeirarchy<Doodad>() != null)
		{
			return PolicyType.Doodad;
		}
		if (go.GetComponentInHeirarchy<DoodadPiece>() != null)
		{
			return PolicyType.Doodad;
		}
		if (go.GetComponentInHeirarchy<Tank>() != null)
		{
			return PolicyType.Heavy_Local_Unit;
		}
		return PolicyType.Unknown;
	}

	public static bool AcceptDamage(PolicyType recieverType, PolicyType senderType)
	{
		return DamagePolicy.CheckMatrix(recieverType, senderType, DamagePolicy.Instance.DamageMatrix, true);
	}

	public static bool AcceptBurn(PolicyType recieverType, PolicyType senderType)
	{
		return DamagePolicy.CheckMatrix(recieverType, senderType, DamagePolicy.Instance.BurnMatrix, true);
	}

	private static bool CheckMatrix(PolicyType recieverType, PolicyType senderType, BoolArray[] matrix, bool undefinedValue)
	{
		if (recieverType == PolicyType.Unknown || senderType == PolicyType.Unknown)
		{
			return undefinedValue;
		}
		return matrix[(int)recieverType][(int)senderType];
	}

	public static bool NetworkBurn(PolicyType recieverType, PolicyType senderType)
	{
		return DamagePolicy.CheckMatrix(recieverType, senderType, DamagePolicy.Instance.NetworkedBurnMatrix, false);
	}

	public static bool NetworkDamage(PolicyType recieverType, PolicyType senderType)
	{
		return DamagePolicy.CheckMatrix(recieverType, senderType, DamagePolicy.Instance.NetworkedDamageMatrix, false);
	}

	public static int[] playerPhotonIDs = new int[]
	{
		-1,
		-1,
		-1,
		-1
	};

	private static DamagePolicy instance;

	[SerializeField]
	public BoolArray[] DamageMatrix = new BoolArray[0];

	[SerializeField]
	public BoolArray[] BurnMatrix = new BoolArray[0];

	[SerializeField]
	public BoolArray[] NetworkedDamageMatrix = new BoolArray[0];

	[SerializeField]
	public BoolArray[] NetworkedBurnMatrix = new BoolArray[0];

	private enum P
	{
		None,
		Local,
		Networked
	}
}
