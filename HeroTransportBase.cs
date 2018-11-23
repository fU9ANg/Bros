// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroTransportBase : MonoBehaviour
{
	protected virtual void Awake()
	{
		HeroTransportBase.instance = this;
	}

	public static void AddBroToTransport(TestVanDammeAnim bro)
	{
		if (HeroTransportBase.instance != null)
		{
			if (!HeroTransportBase.instance.containedBros.Contains(bro))
			{
				HeroTransportBase.instance.containedBros.Add(bro);
				bro.gunSprite.gameObject.SetActive(false);
				bro.GetComponent<Renderer>().enabled = false;
				bro.enabled = false;
				bro.invulnerable = true;
				bro.transform.position = HeroTransportBase.instance.transform.position;
			}
		}
		else
		{
			UnityEngine.Debug.LogError("NO INSTANCE to ADD BRO");
		}
	}

	public static HeroTransportBase instance;

	protected List<TestVanDammeAnim> containedBros = new List<TestVanDammeAnim>();
}
