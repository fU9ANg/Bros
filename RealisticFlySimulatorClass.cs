// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class RealisticFlySimulatorClass : MonoBehaviour
{
	public void Angry()
	{
		this.speedM = 3f;
		this.isAngry = true;
		this.angryCounter -= UnityEngine.Random.value * 0.2f;
		this.angryIndex += UnityEngine.Random.Range(0, 3);
	}

	protected void SetPosition()
	{
		base.transform.position = new Vector3(Mathf.Round(this.x), Mathf.Round(this.y), 0f);
	}

	public void Start()
	{
		if (!this.started)
		{
			this.started = true;
			if (this.optionalFollowTransform != null)
			{
				this.targetX = base.transform.position.x - this.optionalFollowTransform.position.x;
				this.targetY = base.transform.position.y - this.optionalFollowTransform.position.y;
				this.originPos = this.optionalFollowTransform.position;
				this.hasOptionalFollowTransform = true;
				this.lonelyLife = 0.1f + UnityEngine.Random.value * 2f;
			}
			else
			{
				this.targetX = base.transform.position.x;
				this.targetY = base.transform.position.y;
				this.originPos = Vector3.zero;
			}
			this.x = this.targetX + this.originPos.x + UnityEngine.Random.value * 20f - 10f;
			this.y = this.targetY + this.originPos.y + UnityEngine.Random.value * 20f - 10f;
		}
	}

	private void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.isAngry)
		{
			this.angryCounter += this.t;
			if (this.angryCounter > 0.1f)
			{
				this.angryCounter -= 0.1f;
				this.angryIndex++;
				if (this.angryIndex % 3 == 0)
				{
					base.GetComponent<Renderer>().material.SetColor("_TintColor", Color.red);
				}
				else
				{
					base.GetComponent<Renderer>().material.SetColor("_TintColor", Color.black);
				}
			}
		}
		this.xDiff = this.targetX + this.originPos.x - this.x;
		this.yDiff = this.targetY + this.originPos.y - this.y;
		this.xI += this.xDiff * this.t * this.returnForce * this.speedM;
		this.yI += this.yDiff * this.t * this.returnForce * this.speedM;
		if (this.optionalFollowTransform != null)
		{
			this.originPos = this.optionalFollowTransform.position;
		}
		this.counter += this.t;
		if (this.counter > 0.1f && UnityEngine.Random.value < 0.3f)
		{
			this.xI *= 1f - 0.2f * ((1f + this.speedM) / 2f);
			this.yI *= 1f - 0.2f * ((1f + this.speedM) / 2f);
			this.returnForce = 1f + UnityEngine.Random.value * 24f;
			this.xI += (UnityEngine.Random.value * 32f - 16f) * ((1f + this.speedM) / 2f);
			this.yI += (UnityEngine.Random.value * 32f - 16f) * ((1f + this.speedM) / 2f);
		}
		this.yI *= 1f - this.t * 0.1f * this.speedM;
		this.xI *= 1f - this.t * 0.1f * this.speedM;
		this.x += this.xI * this.t;
		this.y += this.yI * this.t;
		this.SetPosition();
		if (this.hasOptionalFollowTransform && this.dieAfterTime && (this.optionalFollowTransform == null || this.optionalFollowTransform.transform.localScale.x < 0.5f))
		{
			this.lonelyLife -= this.t;
			if (this.lonelyLife <= 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	protected float x;

	protected float y;

	protected float xI;

	protected float yI;

	protected float xDiff;

	protected float yDiff;

	protected float targetX;

	protected float targetY;

	protected float t;

	public Transform optionalFollowTransform;

	protected Vector3 originPos;

	protected bool hasOptionalFollowTransform;

	protected float lonelyLife = 2f;

	public float speedM = 1f;

	protected bool started;

	protected bool isAngry;

	protected float angryCounter;

	protected int angryIndex;

	public bool dieAfterTime = true;

	protected float returnForce = 10f;

	protected float counter;
}
