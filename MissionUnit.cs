// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MissionUnit : MonoBehaviour
{
	protected virtual void SetPosition()
	{
		base.transform.position = new Vector3(this.x, this.y, this.zOffset);
	}

	public virtual void Damage(int damage, bool knock, float xForce, float yForce)
	{
		UnityEngine.Debug.Log("Damage ");
		this.DestroyUnit();
	}

	protected virtual void DestroyUnit()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public virtual void Leave()
	{
	}

	[HideInInspector]
	public float x;

	[HideInInspector]
	public float y;

	[HideInInspector]
	public float xI;

	[HideInInspector]
	public float yI;

	[HideInInspector]
	public bool invulnerable;

	public float width = 9f;

	public float height = 5f;

	public float zOffset = -20f;

	public int playerNum = -1;

	public int health = 3;
}
