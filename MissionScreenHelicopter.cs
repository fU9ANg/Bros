// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MissionScreenHelicopter : MissionScreenUnit
{
	protected virtual void Start()
	{
		UnityEngine.Debug.Log("Start " + base.transform.position);
		this.restingPos = this.helicopterRestingTarget.position;
		if (this.playerNum >= 0)
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(new Vector3(0f, (float)Screen.height, 10f));
			this.x = vector.x - 8f;
			this.y = vector.y + 24f;
			this.yI = -12f;
			this.xI = 6f;
		}
		else
		{
			this.x = base.transform.position.x;
			this.y = base.transform.position.y;
		}
		this.SetPosition();
		base.GetComponent<AudioSource>().Play();
	}

	protected override void SetPosition()
	{
		base.SetPosition();
		this.currentAngle = Mathf.Lerp(this.currentAngle, this.xI / this.maxXSpeed * -15f + this.yI / this.maxYSpeed * 0f, this.t * 15f);
		base.transform.eulerAngles = new Vector3(0f, 0f, this.currentAngle);
	}

	public override void Leave()
	{
		this.arriving = false;
		this.leaving = true;
		this.fighting = false;
	}

	public void Fight()
	{
		this.arriving = false;
		this.leaving = false;
		this.fighting = true;
	}

	protected virtual void Update()
	{
		this.counter += Time.deltaTime;
		this.t = Time.deltaTime;
		if (this.arriving)
		{
			Vector3 vector = this.restingPos - base.transform.position;
			this.xI += vector.x * this.t * 15f;
			this.yI += vector.y * this.t * 15f;
			this.xI *= 1f - this.t * 9f;
			this.yI *= 1f - this.t * 4f;
			this.x += this.xI * this.t;
			this.y += this.yI * this.t;
		}
		if (this.leaving)
		{
			this.xI = Mathf.Lerp(this.xI, this.maxXSpeed, this.t * 1.5f);
			this.yI *= 1f - this.t * 4f;
			this.x += this.xI * this.t;
			this.y += this.yI * this.t;
		}
		if (this.fighting)
		{
			this.wasFire = this.fire;
			this.wasSpecial = this.special;
			this.GetInput(ref this.left, ref this.right, ref this.up, ref this.down, ref this.fire, ref this.special);
			this.CalculateMovement(true);
			this.RunMovement();
			this.RunFiring();
		}
		this.SetPosition();
	}

	protected void RunFiring()
	{
		if (!this.fire || !this.wasFire)
		{
		}
	}

	protected void RunMovement()
	{
		this.x += this.xI * this.t;
		this.y += this.yI * this.t;
	}

	protected void CalculateMovement(bool canLerpToStop)
	{
		if ((this.up && this.down) || (!this.up && !this.down))
		{
			if (canLerpToStop)
			{
				this.yI *= 1f - this.t * this.slowDownLerpM;
			}
		}
		else if (this.up)
		{
			this.yI += this.yAcceleration * this.t;
			if (this.yI < 0f)
			{
				this.yI = 0f;
			}
			if (this.yI < this.maxYSpeed * 0.5f)
			{
				this.yI += this.yAcceleration * this.t * 12f;
			}
			if (this.yI > this.maxYSpeed)
			{
				this.yI = this.maxYSpeed;
			}
		}
		else if (this.down)
		{
			this.yI -= this.yAcceleration * this.t;
			if (this.yI > 0f)
			{
				this.yI = 0f;
			}
			if (this.yI > -this.maxYSpeed * 0.5f)
			{
				this.yI -= this.yAcceleration * this.t * 12f;
			}
			if (this.yI < -this.maxYSpeed)
			{
				this.yI = -this.maxYSpeed;
			}
		}
		if ((this.left && this.right) || (!this.left && !this.right))
		{
			if (canLerpToStop)
			{
				this.xI *= 1f - this.t * this.slowDownLerpM;
			}
		}
		else if (this.right)
		{
			this.xI += this.xAcceleration * this.t;
			if (this.xI < 0f)
			{
				this.xI = 0f;
			}
			if (this.xI < this.maxXSpeed * 0.5f)
			{
				this.xI += this.xAcceleration * this.t * 12f;
			}
			if (this.xI > this.maxXSpeed)
			{
				this.xI = this.maxXSpeed;
			}
		}
		else if (this.left)
		{
			this.xI -= this.xAcceleration * this.t;
			if (this.xI > 0f)
			{
				this.xI = 0f;
			}
			if (this.xI > -this.maxXSpeed * 0.5f)
			{
				this.xI -= this.xAcceleration * this.t * 12f;
			}
			if (this.xI < -this.maxXSpeed)
			{
				this.xI = -this.maxXSpeed;
			}
		}
	}

	protected virtual void GetInput(ref bool left, ref bool right, ref bool up, ref bool down, ref bool fire, ref bool special)
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			left = true;
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			left = false;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			right = true;
		}
		if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			right = false;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			down = true;
		}
		if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			down = false;
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			up = true;
		}
		if (Input.GetKeyUp(KeyCode.UpArrow))
		{
			up = false;
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			fire = true;
		}
		if (Input.GetKeyUp(KeyCode.Z))
		{
			fire = false;
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			special = true;
		}
		if (Input.GetKeyUp(KeyCode.X))
		{
			special = false;
		}
	}

	protected float counter;

	protected bool leaving;

	protected bool arriving = true;

	protected bool fighting;

	protected Vector3 restingPos;

	public Material hueyHelicopterMaterial;

	public Material apacheHelicopterMaterial;

	public Material airwolfHelicopterMaterial;

	protected bool left;

	protected bool right;

	protected bool up;

	protected bool down;

	protected bool fire;

	protected bool special;

	protected bool wasFire;

	protected bool wasSpecial;

	public float maxXSpeed = 320f;

	public float maxYSpeed = 240f;

	public float xAcceleration = 15f;

	public float yAcceleration = 10f;

	public float slowDownLerpM = 4f;

	public Transform helicopterRestingTarget;

	protected float currentAngle;

	protected float t = 0.011f;
}
