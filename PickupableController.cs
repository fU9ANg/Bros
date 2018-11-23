// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class PickupableController : MonoBehaviour
{
	private void Awake()
	{
		PickupableController.instance = this;
	}

	private void Update()
	{
	}

	public static Pickupable CreateAmmoBox(float x, float y)
	{
		Pickupable pickupable = UnityEngine.Object.Instantiate(PickupableController.instance.ammoPickupPrefab, new Vector3(x, y, 0f), Quaternion.identity) as Pickupable;
		pickupable.gameObject.SetActive(true);
		return pickupable;
	}

	public static Pickupable CollectPickupables(TestVanDammeAnim self, float range, float x, float y)
	{
		foreach (Pickupable pickupable in PickupableController.pickupables)
		{
			if (pickupable != null)
			{
				float f = pickupable.x - x;
				if (Mathf.Abs(f) - range < pickupable.radius)
				{
					float f2 = pickupable.y + pickupable.yOffset + pickupable.radius - y;
					if (Mathf.Abs(f2) - range < pickupable.radius && pickupable.pickupDelay <= 0f)
					{
						return pickupable;
					}
				}
			}
		}
		return null;
	}

	public static void UsePickupables(TestVanDammeAnim self, float range, float x, float y)
	{
		foreach (Pickupable pickupable in PickupableController.pickupables.ToArray())
		{
			if (pickupable != null)
			{
				float f = pickupable.x - x;
				if (Mathf.Abs(f) - range < pickupable.radius)
				{
					float f2 = pickupable.y + pickupable.yOffset - y;
					if (Mathf.Abs(f2) - range < pickupable.radius && pickupable.pickupDelay <= 0f)
					{
						Networking.RPC<TestVanDammeAnim>(PID.TargetAll, new RpcSignature<TestVanDammeAnim>(pickupable.Collect), self, false);
					}
				}
			}
		}
	}

	public static void AddPickupable(Pickupable p)
	{
		PickupableController.pickupables.Add(p);
	}

	public static void RemovePickupable(Pickupable p)
	{
		PickupableController.pickupables.Remove(p);
	}

	public Pickupable ammoPickupPrefab;

	protected static PickupableController instance;

	protected static List<Pickupable> pickupables = new List<Pickupable>();
}
