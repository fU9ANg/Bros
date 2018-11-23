// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WildLife : MonoBehaviour
{
	protected virtual void Awake()
	{
	}

	protected virtual void Start()
	{
		if (UnityEngine.Random.value > this.chanceToSpawn)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			Map.RegisterWildLife(this);
		}
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
	}

	public virtual bool Disturb()
	{
		Map.RemoveWildLife(this);
		return true;
	}

	public virtual bool Hurt()
	{
		Map.RemoveDisturbedWildLife(this);
		UnityEngine.Object.Destroy(base.gameObject);
		return true;
	}

	public float chanceToSpawn = 0.33f;

	[HideInInspector]
	public float x;

	[HideInInspector]
	public float y;
}
