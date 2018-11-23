// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class AlienGiantSandWorm : Unit
{
	protected void Start()
	{
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
		base.transform.localScale = new Vector3(-1f, 1f, 1f);
		base.transform.eulerAngles = new Vector3(0f, 0f, -90f);
		this.animatedHead = base.GetComponent<AnimatedTexture>();
	}

	protected void Update()
	{
		if (Map.isEditing)
		{
			return;
		}
		if (!Map.isEditing && !this.hasSetup)
		{
			this.hasSetup = true;
			this.FindLowestGroundPoint();
			this.y = -128f;
			this.SetPosition(0f);
			for (int i = 0; i < this.wormBalls.Length; i++)
			{
				this.wormBalls[i].transform.parent = null;
				this.wormBalls[i].transform.position = base.transform.position;
			}
			this.SetWormPoints(this.x, this.startCurveYHeight, -1);
			this.activated = false;
			this.screamSource = base.gameObject.AddComponent<AudioSource>();
			this.screamSource.clip = this.soundHolder.greeting[0];
			this.screamSource.dopplerLevel = 0.1f;
			this.screamSource.volume = 0.8f;
			this.screamSource.loop = false;
			this.screamSource.playOnAwake = false;
			this.screamSource.bypassEffects = true;
			this.screamSource.rolloffMode = AudioRolloffMode.Linear;
			this.screamSource.minDistance = 120f;
			this.screamSource.maxDistance = 400f;
			this.screamSource.Stop();
		}
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (!this.activated)
		{
			if (SortOfFollow.IsItSortOfVisible(this.GetWormPoint((float)this.arcPointStart), this.activationScreenOffset, this.activationScreenOffset))
			{
				this.delay -= this.t;
				if (this.delay <= 0f)
				{
					this.activated = true;
				}
			}
		}
		else
		{
			if (this.wormFollowCounter < (float)this.arcPointStart || this.wormFollowCounter > (float)this.arcPointEnd)
			{
				float num = (((this.animatedHead.frame != 6) ? 0f : 0.4f) + ((this.animatedHead.frame != 0) ? 0f : 0.8f) + ((this.animatedHead.frame != 1) ? 0f : 1.2f) + ((this.animatedHead.frame != 2) ? 0f : 1.6f)) * this.wormTunnelFollowSpeed;
				this.wormFollowCounter += (this.wormTunnelFollowSpeed + num) * this.t;
			}
			else
			{
				this.wormFollowCounter += this.wormCurveFollowSpeed * this.t;
			}
			Vector2 wormPoint = this.GetWormPoint(this.wormFollowCounter);
			this.x = wormPoint.x;
			this.y = wormPoint.y;
			if (!this.hasScreamed)
			{
				if (this.wormFollowCounter > (float)this.arcPointStart * 0.2f)
				{
					this.hasScreamed = true;
					this.screamSource.Play();
				}
			}
			else
			{
				if (this.wormFollowCounter < (float)this.arcPointEnd * 1.2f)
				{
				}
				SortOfFollow.Shake(0.3f);
			}
			this.SetPosition(0f);
			this.SetWormRotation(this.wormFollowCounter, base.transform);
			this.SetWormPositions(this.wormFollowCounter);
			this.damageGroundCounter += this.t;
			if (this.damageGroundCounter > 0.033f)
			{
				this.damageGroundCounter -= 0.0334f;
				Vector2 wormPoint2 = this.GetWormPoint(this.wormFollowCounter + 40f);
				Vector2 wormPoint3 = this.GetWormPoint(this.wormFollowCounter + 60f);
				Vector2 vector = wormPoint3 - wormPoint2;
				MapController.DamageGround(this, 10, DamageType.Drill, 64f, wormPoint2.x, wormPoint2.y, null);
				Map.DamageDoodads(10, wormPoint2.x, wormPoint2.y, vector.x * 3f, vector.y * 3f, 50f, -15);
				Map.HitUnits(this, 20, DamageType.Crush, 34f, wormPoint2.x, wormPoint2.y, vector.x * 3f, vector.y * 3f, true, true);
			}
		}
	}

	protected void SetWormRotation(float index, Transform trans)
	{
		Vector2 wormPoint = this.GetWormPoint(index - 15f);
		Vector2 wormPoint2 = this.GetWormPoint(index + 15f);
		if (wormPoint != wormPoint2)
		{
			Vector2 vector = wormPoint2 - wormPoint;
			trans.eulerAngles = new Vector3(0f, 0f, global::Math.GetAngle(vector.x, vector.y) * 180f / 3.14159274f - 180f);
		}
	}

	protected Vector2 GetWormPoint(float index)
	{
		if (index <= 0f)
		{
			return this.wormPoints[0];
		}
		if (index > (float)(this.wormPoints.Count - 1))
		{
			return this.wormPoints[this.wormPoints.Count - 1];
		}
		return this.wormPoints[(int)index];
	}

	protected void SetWormPoints(float centerX, float curveStartY, int direction)
	{
		float num = -128f;
		float num2 = centerX + this.curveRadius;
		while (num < curveStartY - 1f)
		{
			num += 1f;
			this.AddWormPoint((int)num2, (int)num);
		}
		this.arcPointStart = this.wormPoints.Count;
		float num3 = 7.853982f;
		while (num3 > 4.712389f)
		{
			num3 -= 1f / (this.curveRadius * 0.7f);
			Vector2 vector = global::Math.Point2OnCircle(num3, this.curveRadius);
			this.AddWormPoint((int)(centerX + vector.x), (int)(curveStartY + vector.y));
		}
		this.arcPointEnd = this.wormPoints.Count;
		num = curveStartY;
		num2 = centerX - this.curveRadius;
		while (num > -256f)
		{
			num -= 2f;
			this.AddWormPoint((int)num2, (int)num);
		}
	}

	protected void AddWormPoint(int currentX, int currentY)
	{
		this.wormPoints.Add(new Vector2((float)currentX, (float)currentY));
	}

	protected void FindLowestGroundPoint()
	{
		float y = this.y;
		bool flag = false;
		while (this.y > 0f && !flag)
		{
			this.y -= 32f;
			if (Physics.Raycast(new Vector3(this.x, this.y - 6f, 0f), Vector3.up, out this.raycastHit, 48f, Map.groundLayer))
			{
				y = this.raycastHit.point.y;
			}
			else if (Physics.Raycast(new Vector3(this.x - 16f, this.y - 6f, 0f), Vector3.up, out this.raycastHit, 48f, Map.groundLayer))
			{
				y = this.raycastHit.point.y;
			}
			else if (Physics.Raycast(new Vector3(this.x + 16f, this.y - 6f, 0f), Vector3.up, out this.raycastHit, 48f, Map.groundLayer))
			{
				y = this.raycastHit.point.y;
			}
			else
			{
				this.y = y;
				flag = true;
			}
		}
		if (!flag)
		{
			this.y = 48f;
		}
		this.startCurveYHeight = this.y - 30f;
	}

	protected void SetPosition(float xOffset)
	{
		base.transform.position = new Vector3(this.x + xOffset, this.y, 1f);
	}

	protected void SetWormPositions(float index)
	{
		for (int i = 0; i < this.wormBalls.Length; i++)
		{
			Vector2 wormPoint = this.GetWormPoint(index - (float)(20 * (1 + i)));
			this.wormBalls[i].position = new Vector3(wormPoint.x, wormPoint.y, 0f);
			this.SetWormRotation(index - (float)(20 * (1 + i)), this.wormBalls[i]);
		}
	}

	protected List<Vector2> wormPoints = new List<Vector2>();

	protected float t = 0.01f;

	protected RaycastHit raycastHit;

	protected AnimatedTexture animatedHead;

	protected float startCurveYHeight = 48f;

	protected bool hasSetup;

	protected bool tunnellingDown;

	protected bool tunnellingUp = true;

	protected bool curvingAround;

	public float curveRadius = 128f;

	protected float curveCounter;

	protected float curveCentreX;

	protected float damageGroundCounter;

	protected int damageGroundCount;

	public float wormCurveFollowSpeed = 160f;

	public float wormTunnelFollowSpeed = 90f;

	protected float wormFollowCounter;

	protected int arcPointStart = -1;

	protected int arcPointEnd = -1;

	public Transform[] wormBalls;

	protected bool activated;

	protected bool hasScreamed;

	public SoundHolder soundHolder;

	public float activationScreenOffset;

	public float delay = 2f;

	protected AudioSource screamSource;
}
