// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionGroundWave : MonoBehaviour
{
	private void Awake()
	{
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
		this.platformLayer = (this.groundLayer | 1 << LayerMask.NameToLayer("Platform"));
		if (!Physics.Raycast(new Vector3(this.x, this.y + 2f, 0f), Vector3.up, out this.raycastHit, 32f, this.groundLayer))
		{
			base.transform.position += Vector3.up * 16f;
			this.verticalRange += 16f;
		}
		this.minRightY = base.transform.position.y - 48f; this.minLeftY = (this.minRightY );
	}

	private void Start()
	{
		int collumn = Map.GetCollumn(base.transform.position.x);
		this.x = (this.leftX = (this.rightX = Map.GetBlocksX(collumn) + 8f));
		this.y = base.transform.position.y;
		this.leftRow = (this.rightRow = Map.GetRow(this.y));
		this.minX = this.x - this.range;
		this.maxX = this.x + this.range;
		this.maxLeftCount = Mathf.RoundToInt(this.range / 16f);
		this.maxRightCount = Mathf.RoundToInt(this.range / 16f);
		this.maxCollumn = collumn; this.leftCollumn = (this.rightCollumn = (this.minCollumn = (this.maxCollumn )));
		if (this.rightWave && this.leftWave)
		{
			this.CreateGroundPoof(0, ref this.rightCollumn, ref this.rightRow, 0f, 1f);
			this.rightCollumn++;
			this.leftCollumn++;
		}
		else if (this.rightWave)
		{
			this.CreateGroundPoof(1, ref this.rightCollumn, ref this.rightRow, 0f, 1f);
		}
		else if (this.leftWave)
		{
			this.CreateGroundPoof(-1, ref this.leftCollumn, ref this.leftRow, 0f, 1f);
		}
		this.minRightY = this.minLeftY;
		this.leftCount = 0; this.rightCount = (this.leftCount );
		if (this.instantPoofsCount > 0 && this.leftWave)
		{
			this.leftWave = this.CreateGroundPoof(-1, ref this.leftCollumn, ref this.leftRow, 0f, 1f);
			this.leftCount++;
		}
		if (this.instantPoofsCount > 1 && this.leftWave)
		{
			this.leftWave = this.CreateGroundPoof(-1, ref this.leftCollumn, ref this.leftRow, 0f, 1f);
			this.leftCount++;
		}
		if (this.instantPoofsCount > 0 && this.rightWave)
		{
			this.rightWave = this.CreateGroundPoof(1, ref this.rightCollumn, ref this.rightRow, 0f, 1f);
			this.rightCount++;
		}
		if (this.instantPoofsCount > 1 && this.rightWave)
		{
			this.rightWave = this.CreateGroundPoof(1, ref this.rightCollumn, ref this.rightRow, 0f, 1f);
			this.rightCount++;
		}
	}

	protected void CheckRightX()
	{
		if (Physics.Raycast(new Vector3(this.x - 2f, this.y + 2f, 0f), Vector3.right, out this.raycastHit, this.range, this.groundLayer))
		{
			if (Physics.Raycast(new Vector3(this.x - 2f, this.y + 18f, 0f), Vector3.right, out this.raycastHit, this.range, this.groundLayer))
			{
				if (this.raycastHit.point.x < this.maxX)
				{
					this.maxX = this.raycastHit.point.x - 2f;
				}
			}
			else
			{
				this.y += 16f;
			}
		}
	}

	protected void CheckLeftX()
	{
		if (Physics.Raycast(new Vector3(this.leftX + 2f, this.y + 2f, 0f), Vector3.left, out this.raycastHit, 16f, this.groundLayer))
		{
			if (Physics.Raycast(new Vector3(this.leftX + 2f, this.y + 18f, 0f), Vector3.left, out this.raycastHit, 16f, this.groundLayer))
			{
				if (this.raycastHit.point.x > this.minX)
				{
					this.minX = this.raycastHit.point.x + 2f;
				}
			}
			else
			{
				this.y += 16f;
			}
		}
	}

	protected bool CreateGroundPoof(int direction, ref int currentCollumn, ref int currentRow, float heightLoss, float waveM)
	{
		float num = Map.GetBlocksX(currentCollumn) + 8f;
		float num2 = Map.GetBlocksX(currentCollumn + direction) + 8f;
		float num3 = Map.GetBlocksY(currentRow) + 8f;
		if (this.smashCrates)
		{
			Block block = Map.GetBlock(currentCollumn + direction, currentRow);
			if (block != null)
			{
				block.Weaken();
			}
			block = Map.GetBlock(currentCollumn + direction, currentRow + 1);
			if (block != null)
			{
				block.Weaken();
			}
			block = Map.GetBlock(currentCollumn + direction, currentRow - 1);
			if (block != null)
			{
				block.Weaken();
			}
		}
		if (direction != 0 && Physics.Raycast(new Vector3(num, num3 + 2f, 0f), new Vector3((float)direction, 0f, 0f), out this.raycastHit, 18f, this.groundLayer))
		{
			if (!Physics.Raycast(new Vector3(num, num3 + 16f, 0f), new Vector3((float)direction, 0f, 0f), out this.raycastHit, 18f, this.groundLayer))
			{
				currentRow++;
				num3 += 16f;
			}
			else
			{
				if (!this.damageGround)
				{
					return false;
				}
				currentRow++;
				this.hasSmashedGround = true;
				num3 += 16f;
				this.DamageGround(currentCollumn + direction, currentRow, direction, true);
			}
		}
		if (this.damageGround && this.hasSmashedGround)
		{
			if (Physics.Raycast(new Vector3(num2, num3, 0f), new Vector3(0f, -1f, 0f), out this.raycastHit, this.verticalRange, (!this.hitPlatforms) ? this.groundLayer : this.platformLayer))
			{
				this.DamageGround(currentCollumn + direction, Map.GetRow(this.raycastHit.point.y + 8f), direction, false);
			}
			else
			{
				this.DamageGround(currentCollumn + direction, currentRow, direction, false);
			}
		}
		if (!Physics.Raycast(new Vector3(num2, num3, 0f), new Vector3(0f, -1f, 0f), out this.raycastHit, this.verticalRange, (!this.hitPlatforms) ? this.groundLayer : this.platformLayer))
		{
			if (direction < 0)
			{
				this.leftGapCount++;
				currentCollumn += direction;
				if (this.leftGapCount > this.ignorGapCount)
				{
					return false;
				}
			}
			if (direction > 0)
			{
				this.rightGapCount++;
				currentCollumn += direction;
				if (this.rightGapCount > this.ignorGapCount)
				{
					return false;
				}
			}
			return true;
		}
		currentCollumn += direction;
		currentRow = Map.GetRow(this.raycastHit.point.y + 8f);
		Block component = this.raycastHit.collider.GetComponent<Block>();
		if (!this.onlyOnDirt || (component != null && (component.groundType == GroundType.EarthTop || component.groundType == GroundType.Earth)))
		{
			if (this.damageMooks)
			{
				Map.HitUnits(this.avoidObject, this.avoidObject, this.playerNum, 15, DamageType.Explosion, 8f, 24f, num2, this.raycastHit.point.y + 24f, 0f, 100f, true, false, true);
			}
			else if (this.knockMooks)
			{
				Map.ExplodeUnits(this.avoidObject, 0, DamageType.Explosion, this.hurtunitsRange, 0.1f + (float)((!this.damageMooks) ? 0 : 16), num2 - (float)(direction * 16), this.raycastHit.point.y - this.hurtunitsRange / 4f, 250f, 80f, this.playerNum, true, false);
			}
			if (this.stunUnits)
			{
				Map.StunUnits(this.playerNum, num, this.raycastHit.point.y + 12f, 6f, 1.2f);
			}
			if (this.shakeAmount > 0f)
			{
				SortOfFollow.Shake(this.shakeAmount);
				Map.ShakeTrees(num, this.raycastHit.point.y, 32f, 32f, 32f);
			}
			if (this.waveVolume > 0f)
			{
				Sound.GetInstance().PlaySoundEffectAt(this.waveSoundClips, 0.5f * this.waveVolume + 0.5f * this.waveVolume * waveM, new Vector3(num2, this.raycastHit.point.y - 2f, 0f), 0.8f + 0.2f * waveM);
			}
			Puff puff = EffectsController.CreateGroundExplodePoofEffect(this.poofPrefab, num2, this.raycastHit.point.y - 2f, UnityEngine.Random.Range(0, 2) * 2 - 1);
			puff.CropBottom(heightLoss);
			this.hasSmashedGround = true;
			if (direction > 0)
			{
				this.rightGapCount = 0;
			}
			if (direction < 0)
			{
				this.leftGapCount = 0;
			}
			if (this.verticalRange > 50f)
			{
				this.verticalRange -= 8f;
			}
			return true;
		}
		return false;
	}

	protected void DamageGround(int collumn, int row, int direction, bool instant)
	{
		if (this.damageGround)
		{
			MonoBehaviour damageSender = (!(this.origins != null)) ? this : this.origins;
			Block block = Map.GetBlock(collumn, row - 1);
			if (block != null)
			{
				if (instant)
				{
					MapController.DamageBlock(damageSender, block, 5, DamageType.Explosion, (float)(direction * 100), 200f);
				}
				else
				{
					this.blocksToDestroy.Add(block);
				}
			}
			block = Map.GetBlock(collumn, row);
			if (block != null)
			{
				if (instant)
				{
					MapController.DamageBlock(damageSender, block, 5, DamageType.Explosion, (float)(direction * 100), 200f);
				}
				else
				{
					this.blocksToDestroy.Add(block);
				}
			}
			block = Map.GetBlock(collumn, row + 1);
			if (block != null)
			{
				if (instant)
				{
					MapController.DamageBlock(damageSender, block, 5, DamageType.Explosion, (float)(direction * 100), 200f);
				}
				else
				{
					this.blocksToDestroy.Add(block);
				}
			}
			block = Map.GetBlock(collumn, row + 2);
			if (block != null)
			{
				if (instant)
				{
					MapController.DamageBlock(damageSender, block, 5, DamageType.Explosion, (float)(direction * 100), 200f);
				}
				else
				{
					this.blocksToDestroy.Add(block);
				}
			}
		}
	}

	protected void DamageGround()
	{
		if (this.damageGround)
		{
			MapController.DamageGround(this.avoidObject, 5, DamageType.Explosion, 16f, this.lastX + ((!this.rightWave) ? 0f : -8.5f) + ((!this.leftWave) ? 0f : 8.5f), this.lastY + 8f, null);
			MapController.DamageGround(this.avoidObject, 5, DamageType.Explosion, 16f, this.lastX, this.lastY + 24f, null);
			MapController.DamageGround(this.avoidObject, 5, DamageType.Explosion, 16f, this.lastX, this.lastY + 40f, null);
		}
	}

	protected void DestroyMarkedBlocks()
	{
		foreach (Block b in this.blocksToDestroy.ToArray())
		{
			MonoBehaviour damageSender = (!(this.origins != null)) ? this : this.origins;
			MapController.DamageBlock(damageSender, b, 15, DamageType.Explosion, 0f, 400f);
		}
		this.blocksToDestroy.Clear();
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		this.waveCounter += deltaTime;
		if (this.waveCounter > this.waveRate)
		{
			this.waveCounter -= this.waveRate;
			this.DestroyMarkedBlocks();
			if (!this.rightWave && !this.leftWave)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				if (this.leftWave)
				{
					float num = (float)this.leftCount / (float)this.maxLeftCount;
					float heightLoss = this.maxHeightLoss * num;
					this.leftWave = this.CreateGroundPoof(-1, ref this.leftCollumn, ref this.leftRow, heightLoss, 1f - num);
					this.leftCount++;
					if (this.leftCount >= this.maxLeftCount)
					{
						this.leftWave = false;
					}
				}
				if (this.rightWave)
				{
					float num2 = (float)this.rightCount / (float)this.maxRightCount;
					float heightLoss2 = this.maxHeightLoss * num2;
					this.rightWave = this.CreateGroundPoof(1, ref this.rightCollumn, ref this.rightRow, heightLoss2, 1f - num2);
					this.rightCount++;
					if (this.rightCount >= this.maxRightCount)
					{
						this.rightWave = false;
					}
				}
			}
		}
	}

	protected float x;

	protected float y;

	protected float minX;

	protected float maxX;

	protected float leftX;

	protected float rightX;

	protected float minLeftY;

	protected float minRightY;

	protected float lastX;

	protected float lastY;

	protected int minCollumn;

	protected int maxCollumn;

	[HideInInspector]
	public bool leftWave = true;

	[HideInInspector]
	public bool rightWave = true;

	[HideInInspector]
	public MonoBehaviour origins;

	public float range = 80f;

	public float maxHeightLoss = 15f;

	protected int leftCount;

	protected int rightCount;

	protected int maxLeftCount;

	protected int maxRightCount;

	protected int leftCollumn;

	protected int rightCollumn;

	protected int leftRow;

	protected int rightRow;

	public float verticalRange = 64f;

	private bool hasSmashedGround;

	public float shakeAmount;

	public bool onlyOnDirt = true;

	public bool knockMooks;

	public bool damageMooks;

	public float hurtunitsRange = 32f;

	public bool damageGround;

	public bool stunUnits;

	public int ignorGapCount;

	protected int leftGapCount;

	protected int rightGapCount;

	public Puff poofPrefab;

	public bool tryMaximizeWaveSize;

	public int instantPoofsCount = 2;

	public bool smashCrates;

	public AudioClip[] waveSoundClips;

	public float waveVolume = 0.2f;

	[HideInInspector]
	public int playerNum = -15;

	[HideInInspector]
	public MonoBehaviour avoidObject;

	private LayerMask groundLayer;

	private LayerMask platformLayer;

	private RaycastHit raycastHit;

	public bool hitPlatforms;

	protected List<Block> blocksToDestroy = new List<Block>();

	protected float waveCounter;

	public float waveRate = 0.11f;
}
