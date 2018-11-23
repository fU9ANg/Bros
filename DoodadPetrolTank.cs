// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class DoodadPetrolTank : DoodadDestroyable
{
	private bool hasLaunched
	{
		get
		{
			return this.launchedLeft || this.launchedRight;
		}
	}

	private void Awake()
	{
		this.collidersToIgnore = base.GetComponentsInChildren<Collider>();
	}

	protected override void Update()
	{
		base.Update();
		if (this.hasLaunched && this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
			if (this.timer <= 0f)
			{
				this.BeginExplode();
			}
		}
		if (this.shakeTime <= 0f)
		{
			if (this.launchedLeft)
			{
				this.xI -= this.acceleration * Time.deltaTime;
			}
			if (this.launchedRight)
			{
				this.xI += this.acceleration * Time.deltaTime;
			}
		}
		this.xI = Mathf.Clamp(this.xI, -this.maxSpeed, this.maxSpeed);
		float num = Mathf.Round(this.xI);
		Vector3 a = Vector3.zero;
		if (num > 0f)
		{
			a += Vector3.right;
		}
		else if (num < 0f)
		{
			a -= Vector3.right;
		}
		Vector3 vector = new Vector3(this.x + a.x * 24f, this.y + 16f, 0f);
	}

	private void BeginExplode()
	{
		base.StartCoroutine(this.ExplodeRoutine());
	}

	private IEnumerator ExplodeRoutine()
	{
		if (this.exploding)
		{
			yield break;
		}
		this.exploding = true;
		for (int i = 0; i < this.tanks.Length; i++)
		{
			if (this.launchedRight)
			{
				this.tanks[i].Death();
			}
			if (this.launchedLeft)
			{
				this.tanks[this.tanks.Length - i - 1].Death();
			}
			yield return new WaitForSeconds(0.35f);
		}
		base.gameObject.SetActive(false);
		yield break;
	}

	public void Launch(Vector3 direction)
	{
		if (!this.hasLaunched)
		{
			this.shakeTime = 1f;
		}
		MonoBehaviour.print("Launch tank " + direction);
		if (direction.x < 0f)
		{
			this.launchedLeft = true;
		}
		else if (direction.x > 0f)
		{
			this.launchedRight = true;
		}
	}

	private bool launchedLeft;

	private bool launchedRight;

	private float acceleration = 40f;

	private float maxSpeed = 100f;

	private float timer = 3.8f;

	[HideInInspector]
	public Collider[] collidersToIgnore;

	public PetrolTankCompartment[] tanks;

	private bool exploding;
}
