// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class FlashBangExplosion : MonoBehaviour
{
	public void Setup(int num, MonoBehaviour FiredBy, DirectionEnum direction = DirectionEnum.Any)
	{
		this.playerNum = num;
		this.firedBy = FiredBy;
		this.collumn = Map.GetCollumn(base.transform.position.x);
		this.row = Map.GetCollumn(base.transform.position.y);
		this.maxGridCollumns = Map.GetMaxCollumns();
		this.maxGridRows = Map.GetMaxRows();
		this.forceDirection = direction;
		if (direction != DirectionEnum.Any)
		{
			UnityEngine.Debug.Log("Launch at direction " + direction);
			if (direction == DirectionEnum.Right)
			{
				this.minGridCollumns = this.collumn;
				this.minGridRows = this.row - 1;
				this.maxGridRows = this.row + 3;
			}
			if (direction == DirectionEnum.Left)
			{
				this.maxGridCollumns = this.collumn + 1;
				this.minGridCollumns = 0;
				this.minGridRows = this.row - 1;
				this.maxGridRows = this.row + 3;
			}
		}
	}

	private void Start()
	{
		this.random = new Randomf(this.seed);
		this.frameRate = this.explosionRate;
		this.maxTime = (float)this.totalExplosions * this.frameRate;
		this.explosionsRemaining = this.totalExplosions;
		this.startTime = Time.time;
		this.usedGridPoints = new bool[this.maxGridCollumns, this.maxGridRows];
		this.sound = Sound.GetInstance();
		if (this.collumn >= this.minGridCollumns && this.collumn < this.maxGridCollumns && this.row >= this.minGridRows && this.row < this.maxGridRows)
		{
			this.activeGridPoints.Add(new FlashBangPoint(this.collumn, this.row, this.startTime, 0, 0));
			if (this.forceDirection != DirectionEnum.Any && (this.forceDirection == DirectionEnum.Left || this.forceDirection == DirectionEnum.Right))
			{
				if (!Map.IsBlockSolid(this.collumn, this.row + 1))
				{
					this.activeGridPoints.Add(new FlashBangPoint(this.collumn, this.row + 1, this.startTime, ((this.forceDirection != DirectionEnum.Right) ? 0 : 1) + ((this.forceDirection != DirectionEnum.Left) ? 0 : -1), 0));
				}
				if (!Map.IsBlockSolid(this.collumn, this.row - 1))
				{
					this.activeGridPoints.Add(new FlashBangPoint(this.collumn, this.row - 1, this.startTime, ((this.forceDirection != DirectionEnum.Right) ? 0 : 1) + ((this.forceDirection != DirectionEnum.Left) ? 0 : -1), 0));
				}
			}
			string text = "Points ";
			foreach (FlashBangPoint flashBangPoint in this.activeGridPoints.ToArray())
			{
				text = text + " |  " + flashBangPoint.ToString();
			}
			UnityEngine.Debug.Log(text);
			this.RunPoints();
		}
		else
		{
			UnityEngine.Debug.LogError("Flash Out of bounds");
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (this.constantAttackSound)
		{
			this.constantAudioSource = base.gameObject.AddComponent<AudioSource>();
			this.constantAudioSource.rolloffMode = AudioRolloffMode.Linear;
			this.constantAudioSource.maxDistance = 500f;
			this.constantAudioSource.clip = this.flashBangSoundHolder.attackSounds[this.random.Range(0, this.flashBangSoundHolder.attackSounds.Length)];
			this.constantAudioSource.loop = true;
			this.constantAudioSource.volume = 1f;
			this.constantAudioSource.Play();
		}
	}

	protected void RunPoints()
	{
		List<FlashBangPoint> list = new List<FlashBangPoint>();
		bool flag = false;
		foreach (FlashBangPoint flashBangPoint in this.activeGridPoints.ToArray())
		{
			if (flashBangPoint.directionCollumn <= 0 && flashBangPoint.collumn - 1 >= this.minGridCollumns && this.forceDirection != DirectionEnum.Right && !this.usedGridPoints[flashBangPoint.collumn - 1, flashBangPoint.row] && !Map.IsBlockSolid(flashBangPoint.collumn - 1, flashBangPoint.row))
			{
				list.Add(this.CreateFlashBangPoint(flashBangPoint.collumn, flashBangPoint.row, -1, 0));
				flag = true;
				if (this.random.value > 0.8f && flashBangPoint.collumn - 2 >= 0 && !this.usedGridPoints[flashBangPoint.collumn - 2, flashBangPoint.row] && !Map.IsBlockSolid(flashBangPoint.collumn - 2, flashBangPoint.row))
				{
					list.Add(this.CreateFlashBangPoint(flashBangPoint.collumn, flashBangPoint.row, -2, 0));
					flag = true;
				}
			}
			if (flashBangPoint.directionCollumn >= 0 && flashBangPoint.collumn + 1 < this.maxGridCollumns && this.forceDirection != DirectionEnum.Left && !this.usedGridPoints[flashBangPoint.collumn + 1, flashBangPoint.row] && !Map.IsBlockSolid(flashBangPoint.collumn + 1, flashBangPoint.row))
			{
				list.Add(this.CreateFlashBangPoint(flashBangPoint.collumn, flashBangPoint.row, 1, 0));
				flag = true;
				if (this.random.value > 0.8f && flashBangPoint.collumn + 2 < this.maxGridCollumns && !this.usedGridPoints[flashBangPoint.collumn + 2, flashBangPoint.row] && !Map.IsBlockSolid(flashBangPoint.collumn + 2, flashBangPoint.row))
				{
					list.Add(this.CreateFlashBangPoint(flashBangPoint.collumn, flashBangPoint.row, 2, 0));
					flag = true;
				}
			}
			if (flashBangPoint.directionRow <= 0 && flashBangPoint.row - 1 >= this.minGridRows && !this.usedGridPoints[flashBangPoint.collumn, flashBangPoint.row - 1] && !Map.IsBlockSolid(flashBangPoint.collumn, flashBangPoint.row - 1))
			{
				list.Add(this.CreateFlashBangPoint(flashBangPoint.collumn, flashBangPoint.row, 0, -1));
				flag = true;
				if (this.random.value > 0.8f && flashBangPoint.row - 2 >= 0 && !this.usedGridPoints[flashBangPoint.collumn, flashBangPoint.row - 2] && !Map.IsBlockSolid(flashBangPoint.collumn, flashBangPoint.row - 2))
				{
					list.Add(this.CreateFlashBangPoint(flashBangPoint.collumn, flashBangPoint.row, 0, -2));
					flag = true;
				}
			}
			if (flashBangPoint.directionRow >= 0 && flashBangPoint.row + 1 < this.maxGridRows && !this.usedGridPoints[flashBangPoint.collumn, flashBangPoint.row + 1] && !Map.IsBlockSolid(flashBangPoint.collumn, flashBangPoint.row + 1))
			{
				list.Add(this.CreateFlashBangPoint(flashBangPoint.collumn, flashBangPoint.row, 0, 1));
				flag = true;
				if (this.random.value > 0.8f && flashBangPoint.row + 2 < this.maxGridRows && !this.usedGridPoints[flashBangPoint.collumn, flashBangPoint.row + 2] && !Map.IsBlockSolid(flashBangPoint.collumn, flashBangPoint.row + 2))
				{
					list.Add(this.CreateFlashBangPoint(flashBangPoint.collumn, flashBangPoint.row, 0, 2));
					flag = true;
				}
			}
		}
		this.activeGridPoints.Clear();
		for (int j = 0; j < list.Count; j++)
		{
			this.activeGridPoints.Add(list[j]);
		}
		if (flag && this.screenShake)
		{
			SortOfFollow.Shake(0.2f, base.transform.position);
		}
		if (!flag && this.forceDirection != DirectionEnum.Any)
		{
			this.maxTime = 0f;
		}
	}

	protected FlashBangPoint CreateFlashBangPoint(int collumn, int row, int directionCollumn, int directionRow)
	{
		this.usedGridPoints[collumn + directionCollumn, row + directionRow] = true;
		FlashBangPoint result = new FlashBangPoint(collumn + directionCollumn, row + directionRow, this.startTime, directionCollumn, directionRow);
		float blocksX = Map.GetBlocksX(collumn + directionCollumn);
		float blocksY = Map.GetBlocksY(row + directionRow);
		if (this.puffExplosionsCount == 1)
		{
			if (this.lightExplosion != null)
			{
				Puff puff = EffectsController.CreateEffect(this.lightExplosion, blocksX + 8f + (float)((!this.extendedExplosion) ? 0 : (directionCollumn * 5)), blocksY + this.yExplosionOffset + (float)((!this.extendedExplosion) ? 0 : (directionRow * 5)), 0f, this.random.value * this.maxPuffDelay, Vector3.zero);
				if (this.rotateExplosionSprite && puff != null)
				{
					if (directionRow < 0)
					{
						puff.transform.eulerAngles = new Vector3(0f, 0f, 180f);
					}
					else if (directionCollumn > 0)
					{
						puff.transform.eulerAngles = new Vector3(0f, 0f, -90f);
					}
					else if (directionCollumn < 0)
					{
						puff.transform.eulerAngles = new Vector3(0f, 0f, 90f);
					}
				}
			}
			else if (this.explosionEffects.Length > 0)
			{
				EffectsController.CreateEffect(this.explosionEffects[this.random.Range(0, this.explosionEffects.Length)], blocksX + 8f, blocksY + 4f, 0f, this.random.value * this.maxPuffDelay, Vector3.zero);
			}
			else
			{
				EffectsController.CreateEffect(this.explosionPuffs[this.random.Range(0, this.explosionPuffs.Length)], blocksX + 8f, blocksY + 4f, 0f, this.random.value * this.maxPuffDelay, Vector3.zero);
			}
		}
		else
		{
			for (int i = 0; i < this.puffExplosionsCount; i++)
			{
				Vector3 vector = this.random.onUnitSphere * 8f;
				if (this.lightExplosion != null)
				{
					EffectsController.CreateEffect(this.lightExplosion, blocksX + 8f + vector.x, blocksY + 4f + vector.y, 0f, this.random.value * this.maxPuffDelay, Vector3.zero);
				}
				else if (this.explosionEffects.Length > 0)
				{
					EffectsController.CreateEffect(this.explosionEffects[this.random.Range(0, this.explosionEffects.Length)], blocksX + 8f + vector.x, blocksY + 4f + vector.y, this.random.value * this.maxPuffDelay, Vector3.zero);
				}
				else
				{
					EffectsController.CreateEffect(this.explosionPuffs[this.random.Range(0, this.explosionPuffs.Length)], blocksX + 8f + vector.x, blocksY + 4f + vector.y, 0f, this.random.value * this.maxPuffDelay, Vector3.zero);
				}
			}
		}
		if (this.blindUnits)
		{
			Map.BlindUnits(this.playerNum, blocksX + 8f, blocksY, 18f);
		}
		if (this.damageUnits)
		{
			Map.HitUnits(this.firedBy, this.firedBy, this.playerNum, this.damageAmount, this.damageType, 12f, blocksX + 8f, blocksY + 3f, (float)(directionCollumn * 100), (float)(directionRow * 100), true, this.knockUnits);
		}
		if (this.alertUnits)
		{
			Map.AlertNearbyMooks(blocksX + 8f, blocksY, 18f, 18f, this.playerNum);
		}
		if (this.burnGround)
		{
			Map.BurnBlocksAround(1, collumn, row, this.random.value < this.forceDamageGroundChance);
		}
		if (!this.constantAttackSound && this.flashBangSoundHolder != null)
		{
			this.sound.PlaySoundEffectAt(this.flashBangSoundHolder.deathSounds, 0.3f, new Vector3(blocksX, blocksY, 0f));
		}
		this.explosionsRemaining--;
		return result;
	}

	private void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		this.counter += this.t;
		if (this.counter >= this.frameRate)
		{
			this.counter -= this.frameRate;
			this.RunPoints();
		}
		if (Time.time - this.startTime > this.maxTime || this.explosionsRemaining <= 0)
		{
			if (this.constantAudioSource != null)
			{
				this.constantAudioSource.Stop();
				this.sound.PlaySoundEffectAt(this.flashBangSoundHolder.deathSounds, 0.4f, base.transform.position);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public Puff lightExplosion;

	public bool rotateExplosionSprite;

	public FlickerFader[] explosionEffects;

	public Puff[] explosionPuffs;

	protected bool[,] usedGridPoints;

	protected List<FlashBangPoint> activeGridPoints = new List<FlashBangPoint>(400);

	protected float startTime;

	protected float t;

	protected float frameRate = 0.12f;

	protected float counter;

	protected int maxGridCollumns;

	protected int maxGridRows;

	protected int minGridCollumns;

	protected int minGridRows;

	public SoundHolder flashBangSoundHolder;

	protected Sound sound;

	public int totalExplosions = 40;

	protected int explosionsRemaining = 40;

	public float explosionRate = 0.06f;

	public bool blindUnits = true;

	public bool damageUnits;

	public bool alertUnits;

	public DamageType damageType = DamageType.Fire;

	public int damageAmount = 3;

	public bool knockUnits = true;

	public bool burnGround;

	public bool damageGround;

	public bool screenShake = true;

	public float forceDamageGroundChance = 0.1f;

	public int puffExplosionsCount = 1;

	public float maxPuffDelay;

	protected float maxTime = 2f;

	public bool extendedExplosion;

	public float yExplosionOffset = 4f;

	public bool constantAttackSound;

	protected AudioSource constantAudioSource;

	private MonoBehaviour firedBy;

	protected int playerNum = 5;

	public int seed = 1;

	protected Randomf random;

	protected int collumn;

	protected int row;

	protected DirectionEnum forceDirection = DirectionEnum.Any;
}
