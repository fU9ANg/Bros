// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class HutScaffoldingBlock : RoofBlock
{
	protected override void Start()
	{
		base.Start();
		if (this.health < 0)
		{
			this.ShowBrokenBackground();
			this.destroyed = true;
			base.GetComponent<Renderer>().enabled = false;
		}
		else
		{
			this.ShowForeground(true, false, false);
		}
		if (!this.leftSolidAtStart && !this.rightSolidAtStart)
		{
			base.GetComponent<Collider>().enabled = false;
			this.health = -1;
			this.canCollapseAboveBlocks = false;
		}
	}

	protected override void StartFalling()
	{
		base.StartFalling();
		base.GetComponent<Collider>().enabled = false;
		this.SetBelowBlockCollapsedVisuals();
	}

	protected override void CollapseSurroundingBackgroundBlocks()
	{
		if (this.canCollapseAboveBlocks)
		{
			base.CollapseSurroundingBackgroundBlocks();
		}
	}

	protected override void SetBelowBlockCollapsedVisuals()
	{
		if (this.canCollapseAboveBlocks && !this.collapsedBelowBlocks)
		{
			this.collapsedBelowBlocks = true;
			Block backgroundBlock = Map.GetBackgroundBlock(this.collumn, this.row - 1);
			if (backgroundBlock != null)
			{
				backgroundBlock.SetCollapsedAboveVisuals();
			}
			Block block = Map.GetBlock(this.collumn, this.row - 1);
			if (block != null && block.groundType == this.groundType)
			{
				block.SetCollapsedAboveVisuals();
			}
		}
	}

	public override void SetCollapsedAboveVisuals()
	{
		base.SetCollapsedAboveVisuals();
		SpriteSM spriteSM = UnityEngine.Object.Instantiate(this.shatteredTopPrefab, base.transform.position + Vector3.up * 8f, Quaternion.identity) as SpriteSM;
		this.addedObjects.Add(spriteSM.GetComponent<BlockPiece>());
		spriteSM.transform.parent = base.transform;
		spriteSM.SetLowerLeftPixel((float)(UnityEngine.Random.Range(0, 4) * 16), (float)((int)spriteSM.lowerLeftPixel.y));
	}

	public override bool NotBroken()
	{
		return base.GetComponent<Collider>().enabled && this.health > 0 && base.NotBroken();
	}

	protected override void PlayHitSound()
	{
		this.PlayDeathSound();
	}

	public override void DamageInternal(int damage, float xI, float yI)
	{
		if (this.health > 0 && this.health - damage > this.collapseHealthPoint)
		{
			this.health -= damage;
		}
		else if (this.health > this.collapseHealthPoint || !this.destroyed)
		{
			this.health -= damage;
			if (this.health <= this.collapseHealthPoint)
			{
				this.Collapse(xI, yI, this.collapseChance);
			}
		}
	}

	protected override void SetCollapsedVisuals()
	{
		base.HideAbove();
		base.HideLeft();
		base.HideRight();
		base.HideBelow();
		base.HideCentre();
		foreach (BlockPiece blockPiece in this.addedObjects)
		{
			if (blockPiece != null)
			{
				blockPiece.gameObject.SetActive(false);
			}
			else
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"Why is this null? ",
					base.name,
					" length ",
					this.addedObjects.Count
				}));
			}
		}
	}

	public virtual void ShowForeground(bool isEarth, bool isBrick, bool onlyEdges)
	{
		if (!onlyEdges)
		{
			this.ShowBrokenBackground(this.foregroundBrickPrefabs, false, true, false, true);
		}
	}

	public override void ShowBrokenBackground()
	{
		this.ShowBrokenBackground(this.backgroundPrefabs, true, false, true, false);
	}

	public void ShowBrokenBackground(BlockBackgroundPrefabs prefabs, bool isBackground, bool isForeground, bool isEarth, bool isBrick)
	{
		if (this.backgroundMesh == null)
		{
			bool flag;
			bool flag2;
			bool flag3;
			bool flag4;
			bool flag5;
			bool flag6;
			bool flag7;
			bool flag8;
			if (isBackground && this.rememberOriginalSurrounds)
			{
				flag = this.topSolidAtStart;
				flag2 = this.bottomSolidAtStart;
				flag3 = this.leftSolidAtStart;
				flag4 = this.rightSolidAtStart;
				flag5 = this.topLeftSolidAtStart;
				flag6 = this.bottomLeftSolidAtStart;
				flag7 = this.topRightSolidAtStart;
				flag8 = this.bottomRightSolidAtStart;
				if (!flag)
				{
					this.ShowTopEdge();
				}
				if (!flag3)
				{
					this.ShowLeftEdge();
				}
				if (!flag4)
				{
					this.ShowRightEdge();
				}
				if (!flag2)
				{
					this.ShowBottomEdge();
				}
			}
			else
			{
				flag = ((!this.onlyCompatibleWithSelf) ? ((!isBackground) ? Map.IsBlockSolidTerrain(this.collumn, this.row + 1) : Map.WasBlockEarth(this.collumn, this.row + 1)) : Map.IsTerrainTheSame(this.groundType, this.collumn, this.row + 1));
				flag2 = ((!this.onlyCompatibleWithSelf) ? ((!isBackground) ? Map.IsBlockSolidTerrain(this.collumn, this.row - 1) : Map.WasBlockEarth(this.collumn, this.row - 1)) : Map.IsTerrainTheSame(this.groundType, this.collumn, this.row - 1));
				flag3 = ((!this.onlyCompatibleWithSelf) ? ((!isBackground) ? Map.IsBlockSolidTerrain(this.collumn - 1, this.row) : Map.WasBlockEarth(this.collumn - 1, this.row)) : Map.IsTerrainTheSame(this.groundType, this.collumn - 1, this.row));
				flag4 = ((!this.onlyCompatibleWithSelf) ? ((!isBackground) ? Map.IsBlockSolidTerrain(this.collumn + 1, this.row) : Map.WasBlockEarth(this.collumn + 1, this.row)) : Map.IsTerrainTheSame(this.groundType, this.collumn + 1, this.row));
				flag5 = ((!this.onlyCompatibleWithSelf) ? ((!isBackground) ? Map.IsBlockSolidTerrain(this.collumn - 1, this.row + 1) : Map.WasBlockEarth(this.collumn - 1, this.row + 1)) : Map.IsTerrainTheSame(this.groundType, this.collumn - 1, this.row + 1));
				flag7 = ((!this.onlyCompatibleWithSelf) ? ((!isBackground) ? Map.IsBlockSolidTerrain(this.collumn + 1, this.row + 1) : Map.WasBlockEarth(this.collumn + 1, this.row + 1)) : Map.IsTerrainTheSame(this.groundType, this.collumn + 1, this.row + 1));
				flag6 = ((!this.onlyCompatibleWithSelf) ? ((!isBackground) ? Map.IsBlockSolidTerrain(this.collumn - 1, this.row - 1) : Map.WasBlockEarth(this.collumn - 1, this.row - 1)) : Map.IsTerrainTheSame(this.groundType, this.collumn - 1, this.row - 1));
				flag8 = ((!this.onlyCompatibleWithSelf) ? ((!isBackground) ? Map.IsBlockSolidTerrain(this.collumn + 1, this.row - 1) : Map.WasBlockEarth(this.collumn + 1, this.row - 1)) : Map.IsTerrainTheSame(this.groundType, this.collumn + 1, this.row - 1));
				if (isForeground && this.rememberOriginalSurrounds)
				{
					this.topSolidAtStart = flag;
					this.bottomSolidAtStart = flag2;
					this.leftSolidAtStart = flag3;
					this.rightSolidAtStart = flag4;
					this.topLeftSolidAtStart = flag5;
					this.bottomLeftSolidAtStart = flag6;
					this.topRightSolidAtStart = flag7;
					this.bottomRightSolidAtStart = flag8;
				}
			}
			if (!flag)
			{
				if (!flag3)
				{
					if (!flag2)
					{
						if (!flag4)
						{
							base.AddBlockPiece(prefabs.allAloneEdges, isBackground, isForeground);
						}
						else
						{
							base.AddBlockPiece(prefabs.topBottomLeftEdges, isBackground, isForeground);
						}
					}
					else if (!flag4)
					{
						base.AddBlockPiece(prefabs.topLeftRightEdges, isBackground, isForeground);
					}
					else if (flag8 || this.IsPrefabEmpty(prefabs.topLeftCornerSingle))
					{
						base.AddBlockPiece(prefabs.topLeftCorner, isBackground, isForeground);
					}
					else
					{
						base.AddBlockPiece(prefabs.topLeftCornerSingle, isBackground, isForeground);
					}
				}
				else if (!flag2)
				{
					if (!flag4)
					{
						base.AddBlockPiece(prefabs.topBottomRightEdges, isBackground, isForeground);
					}
					else
					{
						base.AddBlockPiece(prefabs.topBottomEdges, isBackground, isForeground);
					}
				}
				else if (!flag4)
				{
					if (flag6 || this.IsPrefabEmpty(prefabs.topRightCornerSingle))
					{
						base.AddBlockPiece(prefabs.topRightCorner, isBackground, isForeground);
					}
					else
					{
						base.AddBlockPiece(prefabs.topRightCornerSingle, isBackground, isForeground);
					}
				}
				else
				{
					base.AddBlockPiece(prefabs.topEdges, isBackground, isForeground);
				}
			}
			else if (!flag3)
			{
				if (!flag2)
				{
					if (!flag4)
					{
						base.AddBlockPiece(prefabs.bottomLeftRightEdges, isBackground, isForeground);
					}
					else if (flag7 || this.IsPrefabEmpty(prefabs.bottomLeftCornerSingle))
					{
						base.AddBlockPiece(prefabs.bottomLeftCorner, isBackground, isForeground);
					}
					else
					{
						base.AddBlockPiece(prefabs.bottomLeftCornerSingle, isBackground, isForeground);
					}
				}
				else if (!flag4)
				{
					base.AddBlockPiece(prefabs.leftRightEdges, isBackground, isForeground);
				}
				else
				{
					base.AddBlockPiece(prefabs.leftEdges, isBackground, isForeground);
				}
			}
			else if (!flag2)
			{
				if (!flag4)
				{
					if (flag5 || this.IsPrefabEmpty(prefabs.bottomRightCornerSingle))
					{
						base.AddBlockPiece(prefabs.bottomRightCorner, isBackground, isForeground);
					}
					else
					{
						base.AddBlockPiece(prefabs.bottomRightCornerSingle, isBackground, isForeground);
					}
				}
				else
				{
					base.AddBlockPiece(prefabs.bottomEdges, isBackground, isForeground);
				}
			}
			else if (!flag4)
			{
				base.AddBlockPiece(prefabs.rightEdges, isBackground, isForeground);
			}
			else if (!flag5 && !flag7 && !flag6 && !flag8 && prefabs.FourWayJunction.Length > 0)
			{
				base.AddBlockPiece(prefabs.FourWayJunction, isBackground, isForeground);
			}
			else if (!flag5)
			{
				base.AddBlockPiece(prefabs.topLeftInnerCorner, isBackground, isForeground);
			}
			else if (!flag7)
			{
				base.AddBlockPiece(prefabs.topRightInnerCorner, isBackground, isForeground);
			}
			else if (!flag6)
			{
				base.AddBlockPiece(prefabs.bottomLeftInnerCorner, isBackground, isForeground);
			}
			else if (!flag8)
			{
				base.AddBlockPiece(prefabs.bottomRightInnerCorner, isBackground, isForeground);
			}
			else if (this.useLargeWindowPieces)
			{
				if (this.blocksOfTypeRows > 1 && this.blocksOfTypeCollumns > 1)
				{
					if ((this.firstBlockOfTypeCollumn - this.collumn) % 2 == 0 && (this.firstBlockOfTypeRow - this.row) % 2 == 0 && this.firstBlockOfTypeCollumn + this.blocksOfTypeCollumns > this.collumn && this.firstBlockOfTypeRow + this.blocksOfTypeRows > this.row)
					{
						base.AddBlockPiece(prefabs.solidTwoByTwos, isBackground, isForeground);
					}
				}
				else
				{
					base.AddBlockPiece(prefabs.errorPieces, isBackground, isForeground);
				}
			}
			else if (UnityEngine.Random.value > 0.9f)
			{
				base.AddBlockPiece(prefabs.solidDecorations, isBackground, isForeground);
			}
			else
			{
				base.AddBlockPiece(prefabs.solidShallows, isBackground, isForeground);
			}
		}
	}

	protected bool IsPrefabEmpty(BlockPiece[] pieces)
	{
		return pieces == null || pieces.Length == 0;
	}

	protected void AssignOriginalSurrounds()
	{
		this.topSolidAtStart = Map.IsBlockSolidTerrain(this.collumn, this.row + 1);
		this.bottomSolidAtStart = Map.IsBlockSolidTerrain(this.collumn, this.row - 1);
		this.leftSolidAtStart = Map.IsBlockSolidTerrain(this.collumn - 1, this.row);
		this.rightSolidAtStart = Map.IsBlockSolidTerrain(this.collumn + 1, this.row);
		this.topLeftSolidAtStart = Map.IsBlockSolidTerrain(this.collumn - 1, this.row + 1);
		this.topRightSolidAtStart = Map.IsBlockSolidTerrain(this.collumn + 1, this.row + 1);
		this.bottomLeftSolidAtStart = Map.IsBlockSolidTerrain(this.collumn - 1, this.row - 1);
		this.bottomRightSolidAtStart = Map.IsBlockSolidTerrain(this.collumn + 1, this.row - 1);
	}

	public override void ShowBottomEdge()
	{
	}

	public override void ShowRightEdge()
	{
	}

	public override void ShowLeftEdge()
	{
	}

	public override void ShowTopEdge()
	{
	}

	public BlockBackgroundPrefabs foregroundBrickPrefabs;

	public SpriteSM shatteredTopPrefab;

	public BlockBackgroundPrefabs backgroundPrefabs;

	public BlockForegroundPrefabs foregroundPrefabs;

	public BlockForegroundPrefabs backgroundEdgesPrefabs;

	public bool rememberOriginalSurrounds;

	public bool onlyCompatibleWithSelf;

	public bool addDecorations = true;

	protected bool canCollapseAboveBlocks = true;

	protected bool topSolidAtStart;

	protected bool bottomSolidAtStart;

	protected bool leftSolidAtStart;

	protected bool rightSolidAtStart;

	protected bool topLeftSolidAtStart;

	protected bool bottomLeftSolidAtStart;

	protected bool topRightSolidAtStart;

	protected bool bottomRightSolidAtStart;

	protected bool collapsedBelowBlocks;
}
