// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class ZipLine : MonoBehaviour
{
	public bool IsHorizontalZipline
	{
		get
		{
			return Mathf.Abs(this.fromAnchorPoint.y - this.toAnchorPoint.y) < 1f;
		}
	}

	public Vector3 Direction
	{
		get
		{
			return (this.toAnchorPoint - this.fromAnchorPoint).normalized;
		}
	}

	private void Start()
	{
		this.lineRenderer = base.GetComponent<LineRenderer>();
		if (this.startExtended)
		{
			this.currentEndPoint = this.toAnchorPoint;
			this.hasAttached = true;
			this.length = Vector3.Distance(this.fromAnchorPoint, this.toAnchorPoint);
			this.dir = (this.toAnchorPoint - this.fromAnchorPoint).normalized;
		}
		else
		{
			this.currentEndPoint = this.fromAnchorPoint;
		}
		this.attachedUnits = new List<Unit>();
		this.lineRenderer = base.GetComponent<LineRenderer>();
		this.lineRenderer.SetVertexCount(2);
		if (this.toAnchorPoint.x > this.fromAnchorPoint.x)
		{
			this.lineRenderer.material = this.ziplineRightMaterial;
		}
		else if (this.toAnchorPoint.x < this.fromAnchorPoint.x)
		{
			this.lineRenderer.material = this.ziplineLeftMaterial;
		}
		base.transform.position = (this.fromAnchorPoint + this.toAnchorPoint) / 2f;
		this.audioSource = base.gameObject.AddComponent<AudioSource>();
		this.audioSource.clip = this.zipLineSound;
		this.audioSource.playOnAwake = false;
		this.audioSource.rolloffMode = AudioRolloffMode.Linear;
		this.audioSource.maxDistance = 230f;
		this.audioSource.minDistance = 120f;
		this.audioSource.Stop();
		this.audioSource.loop = true;
		this.audioSource.volume = 0f;
		this.audioSource.dopplerLevel = 0.02f;
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0333334f);
		if (!this.hasAttached)
		{
			this.dir = (this.toAnchorPoint - this.currentEndPoint).normalized;
			this.currentEndPoint += this.dir * this.shootSpeed * num;
			if (Vector3.Distance(this.currentEndPoint, this.toAnchorPoint) <= this.shootSpeed * num * 1.1f)
			{
				this.currentEndPoint = this.toAnchorPoint;
				this.hasAttached = true;
				this.length = Vector3.Distance(this.fromAnchorPoint, this.toAnchorPoint);
				this.dir = (this.toAnchorPoint - this.fromAnchorPoint).normalized;
			}
			this.lineRenderer.SetPosition(0, this.fromAnchorPoint);
			this.lineRenderer.SetPosition(1, this.currentEndPoint);
		}
		else
		{
			if (!Map.isEditing)
			{
				this.CheckUnits();
			}
			this.DrawLine();
			int num2 = 0;
			for (int i = 0; i < this.attachedUnits.Count; i++)
			{
				float num3;
				if (this.ziplineDirection[i] < 0f)
				{
					num3 = -this.ziplineProgress[i];
				}
				else
				{
					num3 = this.ziplineProgress[i] - Vector3.Distance(this.fromAnchorPoint, this.toAnchorPoint);
				}
				if (this.attachedUnits[i] == null || this.attachedUnits[i].health <= 0 || num3 > -16f)
				{
					this.DetachUnit(i);
				}
				else
				{
					float num4 = 0.5f - Mathf.Abs(0.5f - this.ziplineProgress[i] / this.length);
					if (this.attachedUnits[i].GetComponent<TestVanDammeAnim>().left && ((this.IsHorizontalZipline && this.ziplineDirection[i] >= 0f) || (!this.IsHorizontalZipline && this.Direction.x > 0.01f)))
					{
						this.attachedUnits[i].slidingOnZipline = false;
						this.ziplineProgress[i] = Mathf.Clamp(this.ziplineProgress[i] - this.zipClimbSpeed * num * Mathf.Sign(this.Direction.x), 0f, this.ziplineProgress[i]);
						if (this.ziplineProgress[i] < 8f)
						{
							this.DetachUnit(i);
							goto IL_52F;
						}
						if (this.ziplineDirection[i] > 0f && this.IsHorizontalZipline)
						{
							this.ziplineDirection[i] = 0f;
						}
					}
					else if (this.attachedUnits[i].GetComponent<TestVanDammeAnim>().right && ((this.IsHorizontalZipline && this.ziplineDirection[i] <= 0f) || this.Direction.x < -0.01f))
					{
						List<float> list2;
						List<float> list = list2 = this.ziplineProgress;
						int index2;
						int index = index2 = i;
						float num5 = list2[index2];
						list[index] = num5 + this.zipClimbSpeed * num * Mathf.Sign(this.Direction.x);
						this.attachedUnits[i].slidingOnZipline = false;
						if (this.ziplineProgress[i] < 8f)
						{
							this.DetachUnit(i);
							goto IL_52F;
						}
						if (this.ziplineDirection[i] < 0f && this.IsHorizontalZipline)
						{
							this.ziplineDirection[i] = 0f;
						}
					}
					else
					{
						if (Mathf.Abs(this.ziplineDirection[i]) > 0f)
						{
							num2++;
						}
						this.attachedUnits[i].slidingOnZipline = (Mathf.Abs(this.ziplineDirection[i]) > 0.01f);
						List<float> list4;
						List<float> list3 = list4 = this.ziplineProgress;
						int index2;
						int index3 = index2 = i;
						float num5 = list4[index2];
						list3[index3] = num5 + this.zipSpeed * num * this.ziplineDirection[i];
					}
					this.attachedUnits[i].x = this.fromAnchorPoint.x + this.dir.x * this.ziplineProgress[i];
					this.attachedUnits[i].y = Mathf.Lerp(this.attachedUnits[i].y, this.fromAnchorPoint.y + this.dir.y * this.ziplineProgress[i] + this.ziplineYBend - num4 * 32f, Time.deltaTime * 20f);
					this.attachedUnits[i].xI = (this.attachedUnits[i].yI = 0f);
				}
				IL_52F:;
			}
			if (num2 > 0)
			{
				if (!this.audioSource.isPlaying)
				{
					this.audioSource.Play();
					this.audioSource.volume = 0f;
					this.audioSource.pitch = 0.5f;
				}
				else
				{
					this.audioSource.volume = Mathf.Lerp(this.audioSource.volume, this.audioVolume, num * 3f);
					this.audioSource.pitch = Mathf.Lerp(this.audioSource.pitch, 0.8f, num * 3f);
				}
			}
			else if (this.audioSource.isPlaying)
			{
				this.audioSource.pitch = Mathf.Lerp(this.audioSource.pitch, 1.5f, num * 3f);
				this.audioSource.volume -= num * 8f;
				if (this.audioSource.volume <= 0f)
				{
					this.audioSource.Stop();
				}
			}
		}
	}

	private void DetachUnit(int i)
	{
		if (this.attachedUnits[i] != null)
		{
			if (this.attachedUnits[i].GetComponent<EnemyAI>() != null)
			{
				this.attachedUnits[i].GetComponent<EnemyAI>().ForceHoldLeft(1f);
			}
			float xI = this.dir.x * this.zipSpeed * 0.33f * this.ziplineDirection[i];
			float num = this.dir.x * this.zipSpeed * 0.66f * this.ziplineDirection[i];
			float yIBlast = this.dir.y * this.zipSpeed;
			if (this.isTelephoneLine && !this.attachedUnits[i].IsPressingDown())
			{
				yIBlast = 280f;
				num = 5f * Mathf.Sign(num);
			}
			this.attachedUnits[i].SetVelocity(DamageType.Knock, xI, num, yIBlast);
			this.attachedUnits[i].attachedToZipline = null;
		}
		this.attachedUnits.RemoveAt(i);
		this.ziplineProgress.RemoveAt(i);
		this.ziplineDirection.RemoveAt(i);
		i--;
	}

	public void DetachUnit(Unit unit)
	{
		if (this.attachedUnits.Contains(unit))
		{
			this.DetachUnit(this.attachedUnits.IndexOf(unit));
			unit.attachedToZipline = null;
		}
	}

	private void DrawLine()
	{
		this.lineRenderer.SetVertexCount(this.attachedUnits.Count + 2);
		this.lineRenderer.SetPosition(0, this.toAnchorPoint);
		this.dir = (this.toAnchorPoint - this.fromAnchorPoint).normalized;
		for (int i = 0; i < this.attachedUnits.Count; i++)
		{
			float num = this.fromAnchorPoint.y + this.dir.y * this.ziplineProgress[i];
			if ((this.attachedUnits[i].transform.TransformPoint(4f, this.ziplineYOffset, 0f) + Vector3.forward * 4f).y < num)
			{
				this.lineRenderer.SetPosition(i + 1, this.attachedUnits[i].transform.TransformPoint(4f, this.ziplineYOffset, 0f) + Vector3.forward * 4f);
			}
			else
			{
				this.lineRenderer.SetPosition(i + 1, new Vector3(this.attachedUnits[i].transform.position.x, num, 0f) + Vector3.forward * 4f);
			}
		}
		this.lineRenderer.SetPosition(this.attachedUnits.Count + 1, this.fromAnchorPoint);
	}

	public void AttachUnit(Unit unit)
	{
		if (unit.health > 0)
		{
			this.attachedUnits.Add(unit);
			unit.AttachToZipline(this);
			float f = (this.fromAnchorPoint.x - unit.x) / (this.fromAnchorPoint.x - this.toAnchorPoint.x);
			this.ziplineProgress.Add(Mathf.Abs(f) * Vector3.Distance(this.fromAnchorPoint, this.toAnchorPoint));
			float num = 1f;
			if (this.fromAnchorPoint.y == this.toAnchorPoint.y)
			{
				num = unit.transform.localScale.x;
			}
			MonoBehaviour.print(num);
			this.ziplineDirection.Add(Mathf.Sign(num));
		}
	}

	public bool ShouldUnitAttach(Unit unit)
	{
		if (this.attachedUnits.Contains(unit))
		{
			return false;
		}
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0333334f);
		if ((unit.x < this.fromAnchorPoint.x && unit.x < this.toAnchorPoint.x) || (unit.x > this.fromAnchorPoint.x && unit.x > this.toAnchorPoint.x) || (unit.y < this.fromAnchorPoint.y - 16f && unit.y < this.toAnchorPoint.y - 16f) || (unit.y > this.fromAnchorPoint.y + 16f && unit.y > this.toAnchorPoint.y + 16f))
		{
			return false;
		}
		if (unit.IsPressingDown())
		{
			return false;
		}
		float f = (this.fromAnchorPoint.x - unit.x) / (this.fromAnchorPoint.x - this.toAnchorPoint.x);
		float num2 = Mathf.Lerp(this.fromAnchorPoint.y, this.toAnchorPoint.y, Mathf.Abs(f)) - 16f;
		float f2 = (this.fromAnchorPoint.x - (unit.x + unit.xI * num * 1.2f)) / (this.fromAnchorPoint.x - this.toAnchorPoint.x);
		float num3 = Mathf.Lerp(this.fromAnchorPoint.y, this.toAnchorPoint.y, Mathf.Abs(f2)) - 16f;
		float num4 = unit.y + unit.yI * num * 1.2f;
		return (unit.y > num2 && num4 < num3) || (unit.yI < 0f && unit.y > num2 && unit.y - num2 < this.attachRange);
	}

	private void CheckUnits()
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.IsPlaying(i) && HeroController.players[i].character != null)
			{
				if (this.ShouldUnitAttach(HeroController.players[i].character))
				{
					this.AttachUnit(HeroController.players[i].character);
				}
				if (HeroController.players[i].character.GetComponent<BoondockBro>() != null && HeroController.players[i].character.GetComponent<BoondockBro>().trailingBro != null)
				{
					if (this.ShouldUnitAttach(HeroController.players[i].character.GetComponent<BoondockBro>().trailingBro))
					{
						this.AttachUnit(HeroController.players[i].character.GetComponent<BoondockBro>().trailingBro);
					}
					if (HeroController.players[i].character.GetComponent<BoondockBro>().connollyBro != null && this.ShouldUnitAttach(HeroController.players[i].character.GetComponent<BoondockBro>().connollyBro))
					{
						this.AttachUnit(HeroController.players[i].character.GetComponent<BoondockBro>().connollyBro);
					}
				}
			}
		}
	}

	public bool startExtended = true;

	public bool hasAttached;

	public float ziplineYBend = -16f;

	public float ziplineYOffset = 13f;

	private float attachRange = 6f;

	public Vector3 fromAnchorPoint;

	public Vector3 toAnchorPoint;

	private Vector3 currentEndPoint;

	private LineRenderer lineRenderer;

	private float shootSpeed = 500f;

	public List<Unit> attachedUnits;

	public List<float> ziplineProgress;

	public List<float> ziplineDirection;

	public float zipSpeed = 200f;

	private float zipClimbSpeed = 60f;

	private Vector3 dir;

	private float length;

	public Material ziplineRightMaterial;

	public Material ziplineLeftMaterial;

	protected AudioSource audioSource;

	public AudioClip zipLineSound;

	public float audioVolume = 0.3f;

	public bool isTelephoneLine;
}
