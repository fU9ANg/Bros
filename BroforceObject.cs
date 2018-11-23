// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BroforceObject : NetworkObject
{
	public event global::EventHandler OnDisturb;

	public event global::EventHandler OnCrumbleBridge;

	public virtual void Disturb()
	{
		if (this.OnDisturb != null)
		{
			this.OnDisturb();
		}
	}

	public virtual void CrumbleBridge(float chance)
	{
		if (this.OnCrumbleBridge != null)
		{
			this.OnCrumbleBridge();
		}
	}

	public int health = 3;

	[HideInInspector]
	public int maxHealth = -1;

	public float x;

	public float y;

	[HideInInspector]
	public float xI;

	[HideInInspector]
	public float yI;
}
