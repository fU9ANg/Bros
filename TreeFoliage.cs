// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class TreeFoliage : MonoBehaviour
{
	private void Awake()
	{
		this.totalLeaves = this.maxTotalLeaves;
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
		this.boneOriginPos = this.bone.transform.position;
	}

	public void RegisterCoconut(Coconut c)
	{
		if (this.coconuts == null)
		{
			this.coconuts = new List<Grenade>();
		}
		this.coconuts.Add(c);
	}

	private void Start()
	{
		Map.RegisterTreeFoliage(this);
		base.enabled = false;
	}

	public void Shake(float xDiffNorm, float yDiffNorm, float force, float distance)
	{
		base.enabled = true;
		if (distance < this.shockWaveInstantDistance)
		{
			this.Shake(xDiffNorm, yDiffNorm, force);
		}
		else
		{
			float num = (distance - this.shockWaveInstantDistance) / this.shockWaveTravelSpeed;
			if (this.shockWaveDelay <= 0f || num < this.shockWaveDelay)
			{
				this.shockWaveDelay = num;
			}
			this.delayedXDiffNorm = xDiffNorm;
			this.delayedYDiffNorm = yDiffNorm;
			this.delayedForce = force;
		}
		this.coconutShakeAmount += force;
	}

	protected void Shake(float xDiffNorm, float yDiffNorm, float force)
	{
		force *= this.bendM;
		this.angleI = -Mathf.Sign(xDiffNorm) * (1.1f - Mathf.Abs(xDiffNorm)) * force;
		this.wobbleI = this.angleI * this.initialWobbleInheritM;
		this.resting = false;
		base.enabled = true;
		this.bendBackDelay = 0.15f;
		this.wobbleInheritDelayCounter = this.wobbleInheritDelay;
		int num = 1 + (int)((float)UnityEngine.Random.Range(5, 11) * (1.2f - Mathf.Abs(xDiffNorm)));
		num = num * this.totalLeaves / this.maxTotalLeaves;
		if (num > 0)
		{
			EffectsController.CreateLeafBurst(this.leafSpawningPoint.position.x, this.leafSpawningPoint.position.y, this.leafSpawningPoint.position.z, num, this.leafSpawningRange, (1f - Mathf.Abs(xDiffNorm)) * Mathf.Sign(xDiffNorm) * force * 0.6f, force * 0.25f + (1f - Mathf.Abs(yDiffNorm)) * Mathf.Sign(yDiffNorm) * force * 0.4f, force * 0.3f);
		}
		if (num > 2)
		{
			this.totalLeaves -= num;
		}
		this.coconutShakeAmount += force;
	}

	private void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.shockWaveDelay > 0f)
		{
			this.shockWaveDelay -= this.t;
			if (this.shockWaveDelay <= 0f)
			{
				this.Shake(this.delayedXDiffNorm, this.delayedYDiffNorm, this.delayedForce);
			}
		}
		this.RunCoconuts();
		if (!this.resting)
		{
			this.angleOffset += this.angleI * this.t;
			if (this.bendBackDelay > 0f)
			{
				this.bendBackDelay -= this.t;
			}
			else
			{
				this.angleI -= this.angleOffset * this.t * 180f;
			}
			this.angleI *= 1f - this.t * 6f;
			if (Mathf.Abs(this.angleOffset) <= 0.5f && Mathf.Abs(this.angleI) < 0.2f && Mathf.Abs(this.wobbleOffset) < 0.5f && Mathf.Abs(this.wobbleI) < 0.2f)
			{
				this.angleOffset = 0f;
				this.angleI = 0f;
				this.resting = true;
				base.enabled = false;
			}
			this.bone.transform.eulerAngles = new Vector3(0f, 0f, this.angleOffset);
			this.SetCounterRotatorsRotation();
			this.RunFoliageWobbles();
		}
	}

	private void RunCoconuts()
	{
		if (this.coconutCounter < this.coconutShakeAmount)
		{
			this.coconutCounter += this.t * this.coconutShakeAmount * 0.5f;
			if (this.coconuts.Count > 0 && this.coconutCounter > 200f)
			{
				this.coconutCounter -= 200f;
				this.coconutFallIndex = UnityEngine.Random.Range(0, this.coconuts.Count);
				if (this.coconuts[this.coconutFallIndex] != null && !this.coconuts[this.coconutFallIndex].enabled)
				{
					this.coconuts[this.coconutFallIndex].Launch(this.coconuts[this.coconutFallIndex].transform.position.x, this.coconuts[this.coconutFallIndex].transform.position.y, 0f, 0f);
				}
				this.coconuts.RemoveAt(this.coconutFallIndex);
			}
		}
	}

	private void SetCounterRotatorsRotation()
	{
		if (this.counterRotator != null)
		{
			this.counterRotator.transform.eulerAngles = new Vector3(0f, 0f, 0f);
		}
	}

	private void RunFoliageWobbles()
	{
		if (this.foliageWobble)
		{
			if (this.wobbleInheritDelayCounter <= 0f)
			{
				this.wobbleI += this.angleI * this.t * this.wobbleInherentM;
			}
			else
			{
				this.wobbleInheritDelayCounter -= this.t;
			}
			if (!this.useBendBackDelay || this.bendBackDelay <= 0f)
			{
				this.wobbleI -= this.wobbleOffset * this.t * this.wobbleReturnM;
			}
			this.wobbleOffset += this.wobbleI * this.t;
			if (this.useAngleM)
			{
				this.wobbleOffset += this.angleI * this.t * this.useAngleMM;
			}
			this.wobbleI *= 1f - this.t * this.wobbleDampening;
			this.frontFoliage.transform.eulerAngles = new Vector3(0f, 0f, this.wobbleOffset);
			if (this.backFoliage != null)
			{
				this.backFoliage.transform.eulerAngles = new Vector3(0f, 0f, this.wobbleOffset * 0.5f);
			}
		}
	}

	public Transform bone;

	public Transform frontFoliage;

	public Transform backFoliage;

	public Transform counterRotator;

	public float bendM = 1f;

	protected float wobbleI;

	protected float wobbleAngle;

	protected float wobbleCounter;

	protected float wobbleOffset;

	protected float wobbleSpeed = 1f;

	protected Vector3 boneOriginPos;

	public float x;

	public float y;

	protected float angleOffset;

	protected float angleI;

	protected bool resting = true;

	protected float bendBackDelay;

	protected float shockWaveDelay;

	protected float delayedXDiffNorm;

	protected float delayedYDiffNorm;

	protected float delayedForce;

	public float shockWaveTravelSpeed = 200f;

	public float shockWaveInstantDistance = 48f;

	public Transform leafSpawningPoint;

	public float leafSpawningRange = 20f;

	protected int totalLeaves = 30;

	public int maxTotalLeaves = 30;

	public List<Grenade> coconuts;

	protected float coconutCounter;

	protected float coconutShakeAmount;

	protected int coconutFallIndex = -1;

	public bool foliageWobble = true;

	private float t = 0.01f;

	public bool useAngleM;

	public float useAngleMM = 1f;

	public bool useBendBackDelay = true;

	public float wobbleInheritDelay;

	protected float wobbleInheritDelayCounter;

	public float wobbleInherentM = 30f;

	public float wobbleReturnM = 20f;

	public float wobbleDampening = 6f;

	public float initialWobbleInheritM = 0.5f;
}
