// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DirtBlock : Block
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
	}

	protected override void SetCollapsedVisuals()
	{
		base.SetCollapsedVisuals();
	}

	public virtual void ShowForeground(bool isEarth, bool isBrick, bool onlyEdges)
	{
		bool flag = Map.IsBlockSolidTerrain(this.groundType, this.collumn, this.row + 1);
		bool flag2 = Map.IsBlockSolidTerrain(this.groundType, this.collumn, this.row - 1);
		bool flag3 = Map.IsBlockSolidTerrain(this.groundType, this.collumn - 1, this.row);
		bool flag4 = Map.IsBlockSolidTerrain(this.groundType, this.collumn + 1, this.row);
		bool flag5 = !Map.IsBlockSolidTerrain(this.collumn, this.row + 1);
		bool flag6 = !Map.IsBlockSolidTerrain(this.collumn, this.row - 1);
		bool flag7 = !Map.IsBlockSolidTerrain(this.collumn - 1, this.row);
		bool flag8 = !Map.IsBlockSolidTerrain(this.collumn + 1, this.row);
		if (onlyEdges)
		{
			if (flag5)
			{
				this.ShowTopEdge();
			}
		}
		else if (flag5)
		{
			if (this.groundType == GroundType.Earth)
			{
				this.groundType = GroundType.EarthTop;
			}
			this.topEdge = this.AddForegroundDecorationPiece(this.foregroundPrefabs.topEdgesOpen, (Map.MapData.theme == LevelTheme.City) ? 0.4f : -3f);
			this.bloodSprayTopPiece = this.topEdge;
			if (this.addDecorations)
			{
				if (flag8 && this.foregroundPrefabs.rightEdgesOpen.Length > 0)
				{
					this.addedObjects.Add(this.AddForegroundDecorationPiece(this.foregroundPrefabs.rightEdgesOpen));
				}
				if (flag7 && this.foregroundPrefabs.leftEdgesOpen.Length > 0)
				{
					this.addedObjects.Add(this.AddForegroundDecorationPiece(this.foregroundPrefabs.leftEdgesOpen));
				}
				if (UnityEngine.Random.value > 0.7f && this.foregroundPrefabs.topDecorations.Length > 0)
				{
					Block block = Map.GetBlock(this.collumn + 1, this.row);
					if (!Map.IsBlockSolidTerrain(this.collumn + 1, this.row + 1) && block != null && block.groundType == this.groundType && UnityEngine.Random.value > 0.5f && this.foregroundPrefabs.topDecorationsLarge.Length > 0)
					{
						BlockPiece blockPiece = this.AddForegroundDecorationPiece(this.foregroundPrefabs.topDecorationsLarge);
						block.AddObject(blockPiece);
						this.addedObjects.Add(blockPiece);
					}
					else
					{
						this.addedObjects.Add(this.AddForegroundDecorationPiece(this.foregroundPrefabs.topDecorations));
					}
				}
			}
		}
		else if (!flag)
		{
			this.ShowTopEdge();
		}
		if (onlyEdges)
		{
			if (flag7)
			{
				this.ShowLeftEdge();
			}
			if (flag8)
			{
				this.ShowRightEdge();
			}
			if (flag6)
			{
				this.ShowBottomEdge();
			}
		}
		else
		{
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
		if (flag6 && this.addDecorations && UnityEngine.Random.value > 0.6f)
		{
			this.addedObjects.Add(base.AddForegroundPiece(this.foregroundPrefabs.bottomDecorations));
		}
		if (!onlyEdges)
		{
			if (flag5)
			{
				this.centreObject = base.AddForegroundPiece(this.foregroundPrefabs.solidShallows);
			}
			else if (UnityEngine.Random.value > 0.93f)
			{
				this.centreObject = base.AddForegroundPiece(this.foregroundPrefabs.solidDeepDecorations);
			}
			else
			{
				this.centreObject = base.AddForegroundPiece(this.foregroundPrefabs.solidDeeps);
			}
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
				else if (!flag8 && !flag6 && !this.IsPrefabEmpty(prefabs.tJunctionTop))
				{
					base.AddBlockPiece(prefabs.tJunctionTop, isBackground, isForeground);
				}
				else if (!flag8 && flag6 && !this.IsPrefabEmpty(prefabs.lIntersectionRight_TopRight))
				{
					base.AddBlockPiece(prefabs.lIntersectionRight_TopRight, isBackground, isForeground);
				}
				else if (flag8 && !flag6 && !this.IsPrefabEmpty(prefabs.lIntersectionLeft_TopLeft))
				{
					base.AddBlockPiece(prefabs.lIntersectionLeft_TopLeft, isBackground, isForeground);
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
				else if (!flag8 && !flag7 && !this.IsPrefabEmpty(prefabs.tJunctionLeft))
				{
					base.AddBlockPiece(prefabs.tJunctionLeft, isBackground, isForeground);
				}
				else if (!flag8 && flag7 && !this.IsPrefabEmpty(prefabs.lIntersectionBottom_BottomLeft))
				{
					base.AddBlockPiece(prefabs.lIntersectionBottom_BottomLeft, isBackground, isForeground);
				}
				else if (flag8 && !flag7 && !this.IsPrefabEmpty(prefabs.lIntersectionTop_TopLeft))
				{
					base.AddBlockPiece(prefabs.lIntersectionTop_TopLeft, isBackground, isForeground);
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
				else if (!flag5 && !flag7 && !this.IsPrefabEmpty(prefabs.tJunctionBottom))
				{
					base.AddBlockPiece(prefabs.tJunctionBottom, isBackground, isForeground);
				}
				else if (!flag5 && flag7 && !this.IsPrefabEmpty(prefabs.lIntersectionLeft_BottomLeft))
				{
					base.AddBlockPiece(prefabs.lIntersectionLeft_BottomLeft, isBackground, isForeground);
				}
				else if (flag5 && !flag7 && !this.IsPrefabEmpty(prefabs.lIntersectionRight_BottomRight))
				{
					base.AddBlockPiece(prefabs.lIntersectionRight_BottomRight, isBackground, isForeground);
				}
				else
				{
					base.AddBlockPiece(prefabs.bottomEdges, isBackground, isForeground);
				}
			}
			else if (!flag4)
			{
				if (!flag5 && !flag6 && !this.IsPrefabEmpty(prefabs.tJunctionRight))
				{
					base.AddBlockPiece(prefabs.tJunctionRight, isBackground, isForeground);
				}
				else if (!flag5 && flag6 && !this.IsPrefabEmpty(prefabs.lIntersectionTop_TopRight))
				{
					base.AddBlockPiece(prefabs.lIntersectionTop_TopRight, isBackground, isForeground);
				}
				else if (flag5 && !flag6 && !this.IsPrefabEmpty(prefabs.lIntersectionBottom_BottomRight))
				{
					base.AddBlockPiece(prefabs.lIntersectionBottom_BottomRight, isBackground, isForeground);
				}
				else
				{
					base.AddBlockPiece(prefabs.rightEdges, isBackground, isForeground);
				}
			}
			else if (!flag5 && !flag7 && !flag6 && !flag8 && prefabs.FourWayJunction.Length > 0)
			{
				base.AddBlockPiece(prefabs.FourWayJunction, isBackground, isForeground);
			}
			else if (!flag5 && !flag7 && !this.IsPrefabEmpty(prefabs.tJunctionSolidBottom))
			{
				base.AddBlockPiece(prefabs.tJunctionSolidBottom, isBackground, isForeground);
			}
			else if (!flag5 && !flag6 && !this.IsPrefabEmpty(prefabs.tJunctionSolidRight))
			{
				base.AddBlockPiece(prefabs.tJunctionSolidRight, isBackground, isForeground);
			}
			else if (!flag8 && !flag6 && !this.IsPrefabEmpty(prefabs.tJunctionSolidTop))
			{
				base.AddBlockPiece(prefabs.tJunctionSolidTop, isBackground, isForeground);
			}
			else if (!flag8 && !flag7 && !this.IsPrefabEmpty(prefabs.tJunctionSolidLeft))
			{
				base.AddBlockPiece(prefabs.tJunctionSolidLeft, isBackground, isForeground);
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
					int collumn = this.collumn;
					int row = this.row;
					if ((this.firstBlockOfTypeCollumn - Map.lastXLoadOffset - collumn) % 2 == 0 && (this.firstBlockOfTypeRow - Map.lastYLoadOffset - row) % 2 == 0 && this.firstBlockOfTypeCollumn + this.blocksOfTypeCollumns > collumn && this.firstBlockOfTypeRow + this.blocksOfTypeRows > row)
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

	public override void ShowTopEdge()
	{
		if (!this.destroyed && this.health > 0 && this.topEdge == null)
		{
			this.topEdge = base.AddForegroundPiece(this.foregroundPrefabs.topEdges);
			this.bloodSprayTopPiece = this.topEdge;
		}
		else if (!this.topSolidAtStart && this.health <= 0 && this.backgroundEdgesPrefabs != null)
		{
			this.topEdge = base.AddForegroundPiece(this.backgroundEdgesPrefabs.topEdges);
		}
	}

	public override void ShowLeftEdge()
	{
		if (!this.destroyed && this.health > 0 && this.leftEdge == null)
		{
			this.leftEdge = base.AddForegroundPiece(this.foregroundPrefabs.leftEdges);
			this.bloodSprayLeftPiece = this.leftEdge;
		}
		else if (!this.leftSolidAtStart && this.health <= 0 && this.backgroundEdgesPrefabs != null)
		{
			this.leftEdge = base.AddForegroundPiece(this.backgroundEdgesPrefabs.leftEdges, 7f);
		}
	}

	public override void ShowRightEdge()
	{
		if (!this.destroyed && this.health > 0 && this.rightEdge == null)
		{
			this.rightEdge = base.AddForegroundPiece(this.foregroundPrefabs.rightEdges);
			this.bloodSprayRightPiece = this.rightEdge;
		}
		else if (!this.rightSolidAtStart && this.health <= 0 && this.backgroundEdgesPrefabs != null)
		{
			this.rightEdge = base.AddForegroundPiece(this.backgroundEdgesPrefabs.rightEdges, 7f);
		}
	}

	public override void ShowBottomEdge()
	{
		if (!this.destroyed && this.health > 0 && this.bottomEdge == null)
		{
			this.bottomEdge = base.AddForegroundPiece(this.foregroundPrefabs.bottomEdges);
			this.bloodSprayBottomPiece = this.bottomEdge;
		}
		else if (!this.bottomSolidAtStart && this.health <= 0 && this.backgroundEdgesPrefabs != null)
		{
			this.bottomEdge = base.AddForegroundPiece(this.backgroundEdgesPrefabs.bottomEdges);
		}
	}

	public BlockBackgroundPrefabs backgroundPrefabs;

	public BlockForegroundPrefabs foregroundPrefabs;

	public BlockForegroundPrefabs backgroundEdgesPrefabs;

	public bool rememberOriginalSurrounds;

	public bool onlyCompatibleWithSelf;

	public bool addDecorations = true;

	protected bool topSolidAtStart;

	protected bool bottomSolidAtStart;

	protected bool leftSolidAtStart;

	protected bool rightSolidAtStart;

	protected bool topLeftSolidAtStart;

	protected bool bottomLeftSolidAtStart;

	protected bool topRightSolidAtStart;

	protected bool bottomRightSolidAtStart;
}
