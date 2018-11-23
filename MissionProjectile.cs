// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MissionProjectile : MonoBehaviour
{
	private void Start()
	{
	}

	public void Launch(int playerNum, float x, float y, float xI, float yI)
	{
		this.playerNum = playerNum;
		this.x = x;
		this.y = y;
		this.xI = xI;
		this.yI = yI;
		this.SetPosition();
	}

	private void Update()
	{
		this.t = Time.deltaTime;
		this.x += this.xI * this.t;
		this.y += this.yI * this.t;
		this.xI += Mathf.Sign(this.xI) * 30f * this.t;
		this.SetPosition();
		this.life -= this.t;
		if (this.life < 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	protected void SetPosition()
	{
		base.transform.position = new Vector3(this.x, this.y, -50f);
	}

	[HideInInspector]
	public float x;

	[HideInInspector]
	public float y;

	[HideInInspector]
	public float xI;

	[HideInInspector]
	public float yI;

	protected float life = 2f;

	public int playerNum = -1;

	private float t = 0.01f;
}
