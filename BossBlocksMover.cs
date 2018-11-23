// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class BossBlocksMover : MonoBehaviour
{
	protected virtual void Awake()
	{
		this.x = base.transform.position.x; this.targetX = (this.x );
		this.y = base.transform.position.y;
	}

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
		if (Map.isEditing)
		{
			return;
		}
		if (!this.hasSetup)
		{
			this.hasSetup = true;
			this.Setup();
		}
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		this.thinkCounter += num;
		if (this.thinkCounter > this.thinkRate)
		{
			this.thinkCounter -= this.thinkRate;
			this.Think();
		}
		if (this.moving)
		{
			if (this.targetX > this.x)
			{
				this.x += this.speed * num;
				if (this.x >= this.targetX)
				{
					this.x = this.targetX;
					this.moving = false;
				}
			}
			this.LerpVariables();
		}
		this.SetPosition();
	}

	protected void LerpVariables()
	{
		float num = base.transform.position.x - this.startX;
		float num2 = Mathf.Clamp01(num / this.distanceOfLerp);
		num2 *= num2 * num2;
		this.speed = this.speedMax * (1f - num2) + this.speedMin * num2;
		this.stayOutOfXRange = this.stayOutOfXRangeMax * (1f - num2) + this.stayOutOfXRangeMin * num2;
	}

	protected virtual void Setup()
	{
		this.stations = new List<BossRailStation>();
		this.stations.AddRange(UnityEngine.Object.FindObjectsOfType<BossRailStation>());
		UnityEngine.Debug.Log("SETUP ! ... stations: " + this.stations.Count);
		this.startX = base.transform.position.x;
		this.stayOutOfXRange = this.stayOutOfXRangeMax;
		this.speed = this.speedMax;
	}

	protected virtual void SetPosition()
	{
		base.transform.position = new Vector3(this.x, this.y, base.transform.position.z);
	}

	protected virtual void Think()
	{
		if (!this.moving && HeroController.GetNearestPlayerPos(this.x, this.y).x > this.x - this.stayOutOfXRange && this.x - this.startX < this.maxDistance)
		{
			this.targetX += 64f;
			this.moving = true;
			BossBlocksMover.activeInstance = this;
		}
	}

	public static bool CanDropHere(float xDrop, float yDrop, float width)
	{
		return BossBlocksMover.activeInstance == null || xDrop + width <= BossBlocksMover.activeInstance.x + BossBlocksMover.activeInstance.protectXMin || xDrop - width >= BossBlocksMover.activeInstance.x + BossBlocksMover.activeInstance.protectXMax;
	}

	public BossBlocksMover.BossMoverStatus status;

	private float targetX = 200f;

	[HideInInspector]
	protected float x;

	[HideInInspector]
	protected float y;

	private bool moving;

	private float thinkCounter;

	public float stayOutOfXRangeMax = 160f;

	public float stayOutOfXRangeMin = 96f;

	protected float stayOutOfXRange = 64f;

	protected float startX;

	public float distanceOfLerp = 384f;

	public float speedMax = 110f;

	public float speedMin = 48f;

	protected float speed = 80f;

	public float maxDistance = 660f;

	public float thinkRate = 1f;

	protected bool hasSetup;

	private static BossBlocksMover activeInstance;

	public float protectXMin = -64f;

	public float protectXMax = 64f;

	protected List<BossRailStation> stations;

	public enum BossMoverStatus
	{
		Waiting,
		Moving,
		Docked
	}
}
