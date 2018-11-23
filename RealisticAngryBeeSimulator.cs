// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class RealisticAngryBeeSimulator : MonoBehaviour
{
	public void Restart()
	{
		if (!this.restarted)
		{
			base.transform.parent = null;
			base.gameObject.SetActive(true);
			this.restarted = true;
			this.x = base.transform.position.x;
			this.y = base.transform.position.y;
			this.row = Map.GetRow(this.y);
			this.collumn = Map.GetCollumn(this.x);
			Map.GetBlocksXY(ref this.targetX, ref this.targetY, this.row, this.collumn);
			this.targetY += 5f;
			this.FindEnemy();
			for (int i = 0; i < this.flies.Length; i++)
			{
				this.flies[i].Angry();
				this.flies[i].Start();
				this.flies[i].gameObject.SetActive(true);
			}
			base.GetComponent<AudioSource>().Play();
		}
	}

	protected void FindEnemy()
	{
		int count = this.foundUnits.Count;
		for (int i = 0; i < count; i++)
		{
			if (i < this.foundUnits.Count && (this.foundUnits[i] == null || this.foundUnits[i].health <= 0 || Mathf.Abs(this.foundUnits[i].transform.position.y - this.y) > 20f))
			{
				this.foundUnits.RemoveAt(i);
				i--;
			}
		}
		this.oldClosestUnit = this.closestUnit;
		this.closestUnit = Map.GetNextClosestUnit(5, DirectionEnum.Any, 80f, 24f, this.targetX, this.targetY, this.foundUnits);
		if (this.closestUnit != null)
		{
			UnityEngine.Debug.Log("Found " + this.closestUnit.name);
			this.foundUnits.Add(this.closestUnit);
		}
		else if (this.oldClosestUnit != null && this.oldClosestUnit.health > 0)
		{
			this.closestUnit = this.oldClosestUnit;
			UnityEngine.Debug.Log("Found OLD " + this.closestUnit.name);
		}
		else if (this.foundUnits.Count > 0)
		{
			this.closestUnit = this.foundUnits[0];
		}
		else
		{
			if (UnityEngine.Random.value > 0.5f && Map.GetBlock(this.collumn + 1, this.row) == null)
			{
				this.targetX += 16f;
			}
			else if (Map.GetBlock(this.collumn - 1, this.row) == null)
			{
				this.targetX -= 16f;
			}
			else if (Map.GetBlock(this.collumn + 1, this.row) == null)
			{
				this.targetX += 16f;
			}
			UnityEngine.Debug.Log("Not Found ");
		}
	}

	private void Awake()
	{
		for (int i = 0; i < this.flies.Length; i++)
		{
			if (i % 3 > 0)
			{
				this.flies[i].gameObject.SetActive(false);
			}
		}
	}

	private void Start()
	{
		if (!this.restarted)
		{
			base.gameObject.SetActive(false);
		}
		else if (this.row <= 0)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	protected void SetPosition()
	{
		base.transform.position = new Vector3(this.x, this.y, 0f);
	}

	private void Update()
	{
		if (this.closestUnit != null)
		{
			float num = this.closestUnit.transform.position.x;
			float num2 = this.closestUnit.transform.position.y + 4f;
			Map.GetRowCollumn(num, num2, ref this.row, ref this.collumn);
			Map.GetBlocksXY(ref this.targetX, ref this.targetY, this.row, this.collumn);
			this.targetY += 6f;
		}
		else
		{
			Map.GetRowCollumn(this.x, this.y, ref this.row, ref this.collumn);
		}
		float deltaTime = Time.deltaTime;
		this.life -= deltaTime;
		if (this.life <= 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			Vector3 a = new Vector3(this.targetX, this.targetY, 0f) - new Vector3(this.x, this.y, 0f);
			float magnitude = a.magnitude;
			Vector3 vector = a / magnitude;
			if (magnitude > 3f)
			{
				this.x += vector.x * 120f * deltaTime;
				this.y += vector.y * 120f * deltaTime;
			}
			this.searchCounter -= deltaTime;
			if (this.searchCounter <= 0f)
			{
				this.searchCounter = 2f;
				this.FindEnemy();
			}
			this.panicCounter -= deltaTime;
			if (this.panicCounter <= 0f)
			{
				this.panicCounter = 0.4f + UnityEngine.Random.value * 0.3f;
				Map.PanicUnits(this.x, this.y, 16f, 20f, 0, this.panicCounter + 0.02f, true);
				this.panicCount++;
				if (this.panicCount % 2 == 0 && this.closestUnit != null && Mathf.Abs(a.x) < 18f)
				{
					this.closestUnit.Damage(this.panicCount / 2, DamageType.Bullet, vector.x * 25f, 10f, this.panicCount % 2 * 2 - 1, this, this.closestUnit.x, this.closestUnit.y + 8f);
				}
			}
			if (this.life < 2f)
			{
				base.transform.localScale = Vector3.one * 0.4f;
				base.GetComponent<AudioSource>().volume = 0.25f * this.life;
			}
			this.SetPosition();
		}
	}

	protected float x;

	protected float y;

	protected float targetX;

	protected float targetY;

	protected int row;

	protected int collumn;

	private Unit closestUnit;

	private List<Unit> foundUnits = new List<Unit>();

	protected int panicCount;

	protected float life = 7f;

	protected float searchCounter = 2f;

	protected bool restarted;

	protected float panicCounter = 0.1f;

	protected Unit oldClosestUnit;

	public RealisticFlySimulatorClass[] flies;
}
