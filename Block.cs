// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class Block : BroforceObject
{
	public Block RightBlock
	{
		get
		{
			return Map.GetBlock(this.collumn + 1, this.row);
		}
	}

	public Block LeftBlock
	{
		get
		{
			return Map.GetBlock(this.collumn - 1, this.row);
		}
	}

	public Block AboveBlock
	{
		get
		{
			return Map.GetBlock(this.collumn, this.row + 1);
		}
	}

	public Block BelowBlock
	{
		get
		{
			return Map.GetBlock(this.collumn, this.row - 1);
		}
	}

	public bool IsLeftTheSame
	{
		get
		{
			return Map.IsTerrainTheSame(this.groundType, this.collumn - 1, this.row);
		}
	}

	public bool IsRightTheSame
	{
		get
		{
			return Map.IsTerrainTheSame(this.groundType, this.collumn + 1, this.row);
		}
	}

	public bool IsBelowTheSame
	{
		get
		{
			return Map.IsTerrainTheSame(this.groundType, this.collumn, this.row - 1);
		}
	}

	public bool IsAboveTheSame
	{
		get
		{
			return Map.IsTerrainTheSame(this.groundType, this.collumn, this.row + 1);
		}
	}

	public bool IsTopRightTheSame
	{
		get
		{
			return Map.IsTerrainTheSame(this.groundType, this.collumn + 1, this.row + 1);
		}
	}

	public bool IsTopLeftTheSame
	{
		get
		{
			return Map.IsTerrainTheSame(this.groundType, this.collumn - 1, this.row + 1);
		}
	}

	public bool LeftIsEmpty
	{
		get
		{
			return this.LeftBlock == null || this.LeftBlock.destroyed;
		}
	}

	public bool RightIsEmpty
	{
		get
		{
			return this.RightBlock == null || this.RightBlock.destroyed;
		}
	}

	public bool AboveIsEmpty
	{
		get
		{
			return this.AboveBlock == null || this.AboveBlock.destroyed;
		}
	}

	public bool BelowIsEmpty
	{
		get
		{
			return this.BelowBlock == null || this.BelowBlock.destroyed;
		}
	}

	public void UseLargePieces(int firstCollumn, int firstRow, int collumns, int rows, int randomOffset, bool windowPieces, bool muralPieces)
	{
		this.useLargeWindowPieces = windowPieces;
		this.useLargeMuralPieces = muralPieces;
		this.firstBlockOfTypeCollumn = firstCollumn;
		this.firstBlockOfTypeRow = firstRow;
		this.blocksOfTypeCollumns = collumns;
		this.blocksOfTypeRows = rows;
		this.tilingOffset = randomOffset;
	}

	protected virtual void Awake()
	{
		int num = (int)base.transform.position.sqrMagnitude;
		this.random = new Randomf(num + Networking.RandomSeed);
		this.collapseRandom = new Randomf(num * 2 + Networking.RandomSeed);
		this.burnRandom = new Randomf(num * 3 + Networking.RandomSeed);
		this.effectsRandom = new Randomf(num * 4 + Networking.RandomSeed);
		this.otherAttachments = new List<GameObject>();
		this.doodadAttachments = new List<Doodad>();
		this.addedObjects = new List<BlockPiece>(8);
		this.burnCounter = -this.burnRandom.value * 0.3f;
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
		this.maxHealth = this.health;
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
		this.row = Map.GetRow(base.transform.position.y);
		this.collumn = Map.GetRow(base.transform.position.x);
		this.initialColumn = this.collumn;
		this.initialRow = this.row;
	}

	protected virtual void Start()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		if (this.sprite != null)
		{
			this.lowerLeftPixelX = (int)this.sprite.lowerLeftPixel.x;
			this.lowerLeftPixelY = (int)this.sprite.lowerLeftPixel.y;
			this.spriteOffset = this.sprite.offset;
		}
		if (this.bloodSprayTop != null)
		{
			this.bloodSprayTop.gameObject.SetActive(false);
		}
		if (this.bloodSprayBottom != null)
		{
			this.bloodSprayBottom.gameObject.SetActive(false);
		}
		if (this.bloodSprayLeft != null)
		{
			this.bloodSprayLeft.gameObject.SetActive(false);
		}
		if (this.bloodSprayRight != null)
		{
			this.bloodSprayRight.gameObject.SetActive(false);
		}
		if (this.bloodSprayTopHidden != null)
		{
			this.bloodSprayTopHidden.gameObject.SetActive(false);
		}
		if (this.bloodSprayBottomHidden != null)
		{
			this.bloodSprayBottomHidden.gameObject.SetActive(false);
		}
		if (this.bloodSprayLeftHidden != null)
		{
			this.bloodSprayLeftHidden.gameObject.SetActive(false);
		}
		if (this.bloodSprayRightHidden != null)
		{
			this.bloodSprayRightHidden.gameObject.SetActive(false);
		}
		this.CheckDestroyed();
		this.HideBrokenEdges();
		this.sound = Sound.GetInstance();
	}

	private void OnDisable()
	{
	}

	public virtual void Rotate(int direction)
	{
	}

	public void DisturbNetworked()
	{
		Networking.RPC(PID.TargetAll, new RpcSignature(this.Disturb), false);
	}

	public override void Disturb()
	{
		base.Disturb();
	}

	public void ReplaceBlockWith(GroundType groundType)
	{
		Block block = Map.Instance.PlaceGround(groundType, this.collumn, this.row, ref Map.blocks);
		if (block != null)
		{
			block.SetupBlock(this.row, this.collumn, Map.blocks[this.collumn, this.row + 1], Map.blocks[this.collumn, this.row - 1]);
			base.gameObject.transform.position += Vector3.forward * 20f;
			GameObject gameObject = base.gameObject;
			gameObject.name = gameObject.name + " replaced with " + block.name;
		}
		else
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Could not replace ",
				base.gameObject,
				" with ",
				groundType
			}));
		}
	}

	public void SetPosition(int row, int col)
	{
		Vector3 position = base.transform.position;
		position.x = (float)(col * 16);
		position.y = (float)(row * 16);
		base.transform.position = position;
		this.x = position.x;
		this.y = position.y;
	}

	protected virtual void CheckDestroyed()
	{
		if (base.GetComponent<Collider>() == null || !base.GetComponent<Collider>().enabled || this.health <= 0)
		{
			this.destroyed = true;
			this.destroyCounter = 100f;
			if (this is PropaneBlock)
			{
				MonoBehaviour.print("CheckDestroyed " + this.destroyed);
			}
		}
	}

	public void HideCentre()
	{
		if (this.centreObject != null)
		{
			this.centreObject.gameObject.SetActive(false);
		}
	}

	public void HideLeft()
	{
		this.hiddenLeft = true;
		for (int i = 0; i < this.attachedObjectsLeft.Length; i++)
		{
			this.attachedObjectsLeft[i].gameObject.SetActive(false);
		}
		if (this.bloodSprayLeft != null)
		{
			this.bloodSprayLeft.gameObject.SetActive(false);
		}
		if (this.bloodSprayLeftHidden != null)
		{
			this.bloodSprayLeftHidden.gameObject.SetActive(false);
		}
		if (this.leftEdge != null)
		{
			this.leftEdge.gameObject.SetActive(false);
		}
	}

	public void HideRight()
	{
		this.hiddenRight = true;
		for (int i = 0; i < this.attachedObjectsRight.Length; i++)
		{
			this.attachedObjectsRight[i].gameObject.SetActive(false);
		}
		if (this.bloodSprayRight != null)
		{
			this.bloodSprayRight.gameObject.SetActive(false);
		}
		if (this.bloodSprayRightHidden != null)
		{
			this.bloodSprayRightHidden.gameObject.SetActive(false);
		}
		if (this.rightEdge != null)
		{
			this.rightEdge.gameObject.SetActive(false);
		}
	}

	public void HideBelow()
	{
		this.hiddenBottom = true;
		for (int i = 0; i < this.attachedObjectsBelow.Length; i++)
		{
			this.attachedObjectsBelow[i].gameObject.SetActive(false);
		}
		if (this.bloodSprayBottom != null)
		{
			this.bloodSprayBottom.gameObject.SetActive(false);
		}
		if (this.bloodSprayBottomHidden != null)
		{
			this.bloodSprayBottomHidden.gameObject.SetActive(false);
		}
		if (this.bottomEdge != null)
		{
			this.bottomEdge.gameObject.SetActive(false);
		}
	}

	public void HideAbove()
	{
		this.hiddenTop = true;
		for (int i = 0; i < this.attachedObjects.Length; i++)
		{
			this.attachedObjects[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < this.otherAttachments.Count; j++)
		{
			this.otherAttachments[j].gameObject.SetActive(false);
		}
		for (int k = 0; k < this.doodadAttachments.Count; k++)
		{
			if (this.doodadAttachments[k] != null)
			{
				this.doodadAttachments[k].Collapse();
			}
			else
			{
				UnityEngine.Debug.LogError("Bugger you Ruan, your doodads under the cursor in the level editor spawn and destroy themselves and add themselves to blocks when editing");
			}
		}
		if (this.bloodSprayTop != null)
		{
			this.bloodSprayTop.gameObject.SetActive(false);
		}
		if (this.bloodSprayTopHidden != null)
		{
			this.bloodSprayTopHidden.gameObject.SetActive(false);
		}
		if (this.topEdge != null)
		{
			this.topEdge.gameObject.SetActive(false);
		}
	}

	public virtual void AddObject(BlockPiece meshObject)
	{
		this.addedObjects.Add(meshObject);
	}

	public virtual void ShowBrokenBackground()
	{
		if (this.backgroundEdgeBroken != null)
		{
			this.backgroundEdgeBroken.gameObject.SetActive(true);
			base.GetComponent<Renderer>().enabled = false;
		}
	}

	public virtual void ShowTopEdge()
	{
		if (!this.destroyed && this.topEdgeBroken != null)
		{
			this.topEdgeBroken.gameObject.SetActive(true);
		}
	}

	public virtual void ShowBottomEdge()
	{
		if (!this.destroyed && this.bottomEdgeBroken != null)
		{
			this.bottomEdgeBroken.gameObject.SetActive(true);
		}
	}

	public virtual void ShowLeftEdge()
	{
		if (!this.destroyed && this.leftEdgeBroken != null)
		{
			this.leftEdgeBroken.gameObject.SetActive(true);
		}
	}

	public virtual void ShowRightEdge()
	{
		if (!this.destroyed && this.rightEdgeBroken != null)
		{
			this.rightEdgeBroken.gameObject.SetActive(true);
		}
	}

	public void HideBrokenEdges()
	{
		if (this.backgroundEdgeBroken != null)
		{
			this.backgroundEdgeBroken.gameObject.SetActive(false);
			this.backgroundEdgeBroken.SetOffset(new Vector3(this.backgroundEdgeBroken.offset.x, this.backgroundEdgeBroken.offset.y, (float)(this.row % 4 + this.collumn % 4)));
		}
		this.HideSideBrokenEdges();
	}

	public void HideSideBrokenEdges()
	{
		if (this.topEdgeBroken != null)
		{
			this.topEdgeBroken.gameObject.SetActive(false);
		}
		if (this.bottomEdgeBroken != null)
		{
			this.bottomEdgeBroken.gameObject.SetActive(false);
		}
		if (this.leftEdgeBroken != null)
		{
			this.leftEdgeBroken.gameObject.SetActive(false);
		}
		if (this.rightEdgeBroken != null)
		{
			this.rightEdgeBroken.gameObject.SetActive(false);
		}
	}

	public virtual void DamageInternal(int damage, float xI, float yI)
	{
		if (this.health > 0)
		{
			this.health -= damage;
			if (this.health <= 0)
			{
				this.Collapse(xI, yI, this.collapseChance);
			}
		}
	}

	public virtual void Weaken()
	{
		this.health = 1;
	}

	public void AttachMe(Transform trans)
	{
		this.otherAttachments.Add(trans.gameObject);
	}

	public void AttachMe(Doodad doodad)
	{
		this.doodadAttachments.Add(doodad);
		if (doodad is Spikes)
		{
			this.spikes = (doodad as Spikes);
		}
	}

	public virtual void CreateDecal(DecalInfo decalInfo)
	{
		if (this.CanBloody(decalInfo.direction) && decalInfo.type == DecalType.Bloody)
		{
			this.Bloody(decalInfo.direction, decalInfo.bloodColor);
		}
	}

	protected virtual bool CanBloody(DirectionEnum direction)
	{
		switch (direction)
		{
		case DirectionEnum.Up:
			return !Map.IsBlockSolid(this.collumn, this.row + 1);
		case DirectionEnum.Down:
			return !Map.IsBlockSolid(this.collumn, this.row - 1);
		case DirectionEnum.Left:
			return !Map.IsBlockSolid(this.collumn - 1, this.row);
		case DirectionEnum.Right:
			return !Map.IsBlockSolid(this.collumn + 1, this.row);
		default:
			return false;
		}
	}

	public virtual void Bloody(DirectionEnum direction, BloodColor color)
	{
		Color bloodColor = EffectsController.GetBloodColor(color);
		switch (direction)
		{
		case DirectionEnum.Up:
			foreach (BlockPiece blockPiece in this.addedObjects)
			{
				blockPiece.Bloody(DirectionEnum.Up, color);
			}
			if (this.topEdge != null)
			{
				this.topEdge.Bloody(DirectionEnum.Any, color);
			}
			if (this.bloodSprayTopPiece != null)
			{
				this.bloodSprayTopPiece.Bloody(color);
			}
			else
			{
				this.bloodyTop = true;
			}
			if (this.bloodSprayTopHidden != null && this.hiddenTop)
			{
				this.bloodSprayTopHidden.gameObject.SetActive(true);
			}
			else if (this.bloodSprayTop != null)
			{
				this.bloodSprayTop.gameObject.SetActive(true);
				SpriteSM component = this.bloodSprayTop.GetComponent<SpriteSM>();
				if (component != null)
				{
					component.SetColor(bloodColor);
				}
			}
			break;
		case DirectionEnum.Down:
			foreach (BlockPiece blockPiece2 in this.addedObjects)
			{
				blockPiece2.Bloody(DirectionEnum.Down, color);
			}
			if (this.bottomEdge != null)
			{
				this.bottomEdge.Bloody(DirectionEnum.Any, color);
			}
			if (this.bloodSprayBottomPiece != null)
			{
				this.bloodSprayBottomPiece.Bloody(color);
			}
			else if (this.addedObjects.Count <= 0)
			{
				this.bloodyBottom = true;
			}
			if (this.bloodSprayBottomHidden != null && this.hiddenBottom)
			{
				this.bloodSprayBottomHidden.gameObject.SetActive(true);
			}
			else if (this.bloodSprayBottom != null)
			{
				this.bloodSprayBottom.gameObject.SetActive(true);
				SpriteSM component2 = this.bloodSprayBottom.GetComponent<SpriteSM>();
				if (component2 != null)
				{
					component2.SetColor(bloodColor);
				}
			}
			break;
		case DirectionEnum.Left:
			foreach (BlockPiece blockPiece3 in this.addedObjects)
			{
				blockPiece3.Bloody(DirectionEnum.Left, color);
			}
			if (this.leftEdge != null)
			{
				this.leftEdge.Bloody(DirectionEnum.Any, color);
			}
			if (this.bloodSprayLeftPiece != null)
			{
				this.bloodSprayLeftPiece.Bloody(color);
			}
			else
			{
				this.bloodyLeft = true;
			}
			if (this.bloodSprayLeftHidden != null && this.hiddenLeft)
			{
				this.bloodSprayLeftHidden.gameObject.SetActive(true);
			}
			else if (this.bloodSprayLeft != null)
			{
				this.bloodSprayLeft.gameObject.SetActive(true);
				SpriteSM component3 = this.bloodSprayLeft.GetComponent<SpriteSM>();
				if (component3 != null)
				{
					component3.SetColor(bloodColor);
				}
			}
			break;
		case DirectionEnum.Right:
			foreach (BlockPiece blockPiece4 in this.addedObjects)
			{
				blockPiece4.Bloody(DirectionEnum.Right, color);
			}
			if (this.rightEdge != null)
			{
				this.rightEdge.Bloody(DirectionEnum.Any, color);
			}
			if (this.bloodSprayRightPiece != null)
			{
				this.bloodSprayRightPiece.Bloody(color);
			}
			else
			{
				this.bloodyRight = true;
			}
			if (this.bloodSprayRightHidden != null && this.hiddenRight)
			{
				this.bloodSprayRightHidden.gameObject.SetActive(true);
			}
			else if (this.bloodSprayRight != null)
			{
				this.bloodSprayRight.gameObject.SetActive(true);
				SpriteSM component4 = this.bloodSprayRight.GetComponent<SpriteSM>();
				if (component4 != null)
				{
					component4.SetColor(bloodColor);
				}
			}
			break;
		}
	}

	public virtual void Bloody(Vector3 normal, BloodColor color)
	{
		if (normal.y > 0.2f && !this.bloodyTop)
		{
			this.Bloody(DirectionEnum.Up, color);
		}
		else if (normal.y < -0.2f && !this.bloodyBottom)
		{
			this.Bloody(DirectionEnum.Down, color);
		}
		else if (normal.x > 0.2f && !this.bloodyRight)
		{
			this.Bloody(DirectionEnum.Right, color);
		}
		else if (normal.x < -0.2f && !this.bloodyLeft)
		{
			this.Bloody(DirectionEnum.Left, color);
		}
	}

	public void AttachAboveBlock(Block above)
	{
		this.aboveBlock = above;
	}

	public void AttachBelowBlock(Block below)
	{
		this.belowBlock = below;
	}

	protected virtual void EffectsCollapse(float xI, float yI)
	{
		EffectsController.CreateDirtParticles(this.x, this.y, 20, 5f, 50f, 0f, 0f);
		this.PlayDeathSound();
	}

	protected virtual void EffectsDestroyed(float xI, float yI, float force)
	{
		EffectsController.CreateDirtParticles(this.x, this.y, 25, 5f, force, 0f, 40f);
		this.PlayDeathSound();
	}

	public virtual void Collapse(float xI, float yI, float chance)
	{
		if (!this.destroyed && chance > 0f)
		{
			this.collapseChance = Mathf.Min(this.collapseChance, chance);
			if (this.collapseChance > 0f)
			{
				this.ActuallyCollapse(xI, yI, true);
				if (this.lastDamageObject == null || !(this.lastDamageObject.damageSender is HelicopterBossChaingun))
				{
					Networking.RPC<float, float, bool>(PID.TargetOthers, false, false, true, new RpcSignature<float, float, bool>(this.ActuallyCollapse), xI, yI, false);
				}
			}
		}
	}

	public void ActuallyCollapse(float xI, float yI, bool collapseBlocksAround)
	{
		if (!this.destroyed)
		{
			if (this.collapseChance >= 1f)
			{
				if (this.lastDamageObject != null)
				{
					this.EffectsDestroyed(xI, yI, (float)(65 + 24 * this.lastDamageObject.damage));
				}
				else
				{
					this.EffectsDestroyed(xI, yI, 140f);
				}
			}
			else
			{
				this.EffectsCollapse(xI, yI);
			}
			this.DestroyBlockInternal(collapseBlocksAround);
		}
	}

	public virtual void DestroyBlockInternal(bool CollapseBlocksAround)
	{
		if (this.supportedBy != null)
		{
			this.supportedBy.Collapse();
			this.supportedBy = null;
		}
		this.collapseBlocksAround = CollapseBlocksAround;
		if (base.GetComponent<Collider>() != null)
		{
			base.GetComponent<Collider>().enabled = false;
		}
		this.health = 0;
		this.SetCollapsedVisuals();
		if (!this.destroyed)
		{
			StatisticsController.NotifyBlockDestroyed(this);
		}
		this.destroyed = true;
		Block block = Map.GetBlock(this.collumn, this.row + 1);
		if (block != null)
		{
			block.ShowBottomEdge();
		}
		Block block2 = Map.GetBlock(this.collumn, this.row - 1);
		if (block2 != null)
		{
			block2.ShowTopEdge();
		}
		Block block3 = Map.GetBlock(this.collumn - 1, this.row);
		if (block3 != null)
		{
			block3.ShowRightEdge();
		}
		Block block4 = Map.GetBlock(this.collumn + 1, this.row);
		if (block4 != null)
		{
			block4.ShowLeftEdge();
		}
		if (this.replaceOnCollapse)
		{
			this.replaceOnCollapse = false;
			this.ReplaceBlockWith(this.replacementBlockType);
		}
		this.SetBelowBlockCollapsedVisuals();
		if (this.mine != null)
		{
			this.mine.Detonate(true);
		}
		FluidController.RefreshFluidStatus(this.collumn, this.row);
	}

	protected override void OnDestroy()
	{
		FluidController.RefreshFluidStatus(this.collumn, this.row);
	}

	public virtual void CollapseForced()
	{
		this.CollapseForced(0f, 0f, 1f);
	}

	public virtual void CollapseForced(float xI, float yI, float chance)
	{
		this.Collapse(xI, yI, chance);
	}

	protected virtual void SetBelowBlockCollapsedVisuals()
	{
	}

	public virtual void SetCollapsedAboveVisuals()
	{
	}

	protected virtual void SetCollapsedVisuals()
	{
		if (this.sprite != null)
		{
			this.spriteOffset.z = 12f;
			this.sprite.SetOffset(this.spriteOffset);
			this.sprite.SetLowerLeftPixel((float)(this.lowerLeftPixelX + (int)this.sprite.width), (float)this.lowerLeftPixelY);
		}
		base.GetComponent<Renderer>().enabled = false;
		this.HideAbove();
		this.HideLeft();
		this.HideRight();
		this.HideBelow();
		this.HideCentre();
		this.ShowBrokenBackground();
		this.HideSideBrokenEdges();
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

	public virtual void Push(float xINormalized)
	{
		this.pushForce += xINormalized * Time.deltaTime;
	}

	public virtual bool NotBroken()
	{
		return this.health > 0 && !this.destroyed;
	}

	public virtual bool NotRolling()
	{
		return true;
	}

	protected virtual void DelayedCollapseAbove()
	{
		if (this.aboveBlock != null && (this.collapseRandom.value <= this.collapseChance || this.aboveBlock.groundType == GroundType.HutScaffolding || this.aboveBlock.groundType == GroundType.Bridge || this.aboveBlock.groundType == GroundType.Bridge2 || this.aboveBlock.groundType == GroundType.Barrel || this.aboveBlock.groundType == GroundType.BuriedRocket || this.aboveBlock.groundType == GroundType.FallingBlock) && (this.aboveBlock.groundType == this.groundType || this.aboveBlock.groundType == GroundType.Bridge || this.aboveBlock.groundType == GroundType.Bridge2 || this.groundType == GroundType.Bridge || this.groundType == GroundType.Bridge2 || this.aboveBlock.groundType == GroundType.HutScaffolding || this.groundType == GroundType.FallingBlock || this.aboveBlock.groundType == GroundType.FallingBlock || this.groundType == GroundType.Barrel || this.aboveBlock.groundType == GroundType.Barrel || this.groundType == GroundType.BuriedRocket || this.aboveBlock.groundType == GroundType.BuriedRocket || this.groundType == GroundType.Earth || this.groundType == GroundType.CaveRock || this.groundType == GroundType.Wall || this.groundType == GroundType.EarthTop || this.aboveBlock.groundType == GroundType.Cage) && this.aboveBlock.groundType != GroundType.Steel)
		{
			this.aboveBlock.lastDamageObject = this.lastDamageObject;
			this.aboveBlock.Collapse(0f, 0f, this.collapseChance * 0.5f);
		}
	}

	protected virtual void Update()
	{
		if (this.destroyed && this.destroyCounter < this.collapseDelayTime)
		{
			this.destroyCounter += Time.deltaTime;
			if (this.destroyCounter >= this.collapseDelayTime)
			{
				this.RunCollapseLogic();
			}
		}
		if (!this.destroyed)
		{
			this.RunPushLogic();
		}
	}

	protected virtual void FixedUpdate()
	{
		if (!this.destroyed)
		{
			this.RunBurnLogic();
		}
	}

	public virtual bool IsSolid()
	{
		return true;
	}

	protected virtual void RunCollapseLogic()
	{
		if (this.collapseBlocksAround)
		{
			this.aboveBlock = Map.GetBlock(this.collumn, this.row + 1);
			this.DelayedCollapseAbove();
			this.CrumbleBridges(1f);
		}
		this.collapseBlocksAround = true;
	}

	private void RunPushLogic()
	{
		if (this.pushForce != 0f)
		{
			if (this.pushForce > 0f)
			{
				this.pushForce -= Time.deltaTime * 0.3f;
				if (this.pushForce < 0f)
				{
					this.pushForce = 0f;
				}
			}
			else if (this.pushForce < 0f)
			{
				this.pushForce += Time.deltaTime * 0.3f;
				if (this.pushForce > 0f)
				{
					this.pushForce = 0f;
				}
			}
		}
	}

	private void RunBurnLogic()
	{
		if (this.flamable && this.burnTime > 0f)
		{
			this.burnTime -= Time.fixedDeltaTime;
			if (this.burnTime < 0f)
			{
				this.DamageInternal(this.burnDamage, 0f, 0f);
			}
			else
			{
				this.burnCounter += Time.fixedDeltaTime;
				this.burnUnitsCounter += Time.fixedDeltaTime;
				this.flameCounter += Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
				if (this.burnUnitsCounter > 0.25f)
				{
					this.burnUnitsCounter -= 0.25f;
					Map.BurnUnitsAround_Local(this, 5, 1, 12f, base.transform.position.x, base.transform.position.y + 8f, true, false);
				}
				if (this.burnCounter > 1f)
				{
					this.burnCounter -= 3f;
					Map.BurnBlocksAround(1, this.collumn, this.row, false);
					if (this.burnDamage > this.BurnCollapsePoint() - 2)
					{
						this.burnTime = 0f;
						this.DamageInternal(this.burnDamage, 0f, 0f);
					}
				}
				if (this.flameCounter > 0.05f)
				{
					this.flameCounter -= 0.04f + Time.deltaTime;
					Vector3 direction = this.random.insideUnitCircle * 6f;
					this.CreateFlames(direction);
				}
			}
		}
		else if (this.heatable && this.burnTime > 0f)
		{
			this.burnTime -= Time.fixedDeltaTime;
			if (this.burnTime < 0f)
			{
				this.burnDamage -= Map.BurnBlocksAround(1, this.collumn, this.row, false);
				this.burnDamage--;
			}
		}
		else if (this.burnTime > 0f)
		{
			this.burnTime -= Time.fixedDeltaTime;
			this.burnUnitsCounter += Time.fixedDeltaTime;
			this.burnCounter += Time.fixedDeltaTime;
			this.flameCounter += Time.fixedDeltaTime;
			if (this.burnUnitsCounter > 0.25f)
			{
				this.burnUnitsCounter -= 0.25f;
				Map.BurnUnitsAround_Local(this, 5, 1, 12f, base.transform.position.x, base.transform.position.y + 8f, true, false);
			}
			if (this.burnCounter > 1f)
			{
				this.burnCounter -= 3f;
				Map.BurnBlocksAround(1, this.collumn, this.row, false);
				if (this.burnDamage > this.BurnCollapsePoint() - 2)
				{
					this.burnTime = 0f;
					this.DamageInternal(this.burnDamage, 0f, 0f);
				}
			}
			if (this.flameCounter > 0.05f)
			{
				this.flameCounter -= 0.05f;
				Vector3 direction2 = this.burnRandom.insideUnitCircle * 6f;
				this.CreateFlames(direction2);
			}
		}
	}

	private void CreateFlames(Vector3 direction)
	{
		EffectsController.CreateFlameEffect(base.transform.position.x + direction.x, base.transform.position.y + 8f + direction.y * 1.5f, this.burnRandom.value * 0.033f, direction);
	}

	protected virtual void CrumbleBridges(float chance)
	{
		if (Physics.Raycast(base.transform.position, Vector3.right, out this.groundHit, 24f, this.groundLayer))
		{
			this.groundHit.collider.gameObject.SendMessage("CrumbleBridge", chance, SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			Block block = Map.GetBlock(this.collumn + 1, this.row);
			if (block != null)
			{
				block.CrumbleBridge(chance);
			}
		}
		if (Physics.Raycast(base.transform.position, Vector3.left, out this.groundHit, 24f, this.groundLayer))
		{
			this.groundHit.collider.gameObject.SendMessage("CrumbleBridge", chance, SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			Block block2 = Map.GetBlock(this.collumn - 1, this.row);
			if (block2 != null)
			{
				block2.CrumbleBridge(chance);
			}
		}
	}

	public virtual void Rustle(float y, float x, float xI)
	{
	}

	public override void CrumbleBridge(float chance)
	{
		base.CrumbleBridge(chance);
	}

	public void SetupBlock(int row, int collumn, Block aboveBlock, Block belowBlock)
	{
		this.aboveBlock = aboveBlock;
		this.belowBlock = belowBlock;
		if (aboveBlock != null)
		{
			aboveBlock.belowBlock = this;
		}
		if (belowBlock != null)
		{
			belowBlock.aboveBlock = this;
		}
		this.row = row;
		this.collumn = collumn;
		if (!this.named)
		{
			this.named = true;
			base.name = string.Concat(new object[]
			{
				base.name,
				"_",
				collumn,
				"_",
				row
			});
		}
	}

	public void Damage(DamageObject damgeObject)
	{
		this.lastDamageObject = damgeObject;
		if (damgeObject.damageType == DamageType.InstaGib)
		{
			if (!this.destroyed)
			{
				this.EffectsCollapse(0f, 0f);
				this.DestroyBlockInternal(true);
			}
			return;
		}
		if (damgeObject.damageType == DamageType.Fire && this.flamable)
		{
			this.Burn(damgeObject);
		}
		else if (damgeObject.damageType == DamageType.Plasma)
		{
			this.DamageInternal(damgeObject.damage + 4, damgeObject.xForce, damgeObject.yForce);
		}
		else
		{
			this.DamageInternal(damgeObject.damage, damgeObject.xForce, damgeObject.yForce);
			if (damgeObject.damageType == DamageType.Bullet && Sound.GetInstance() != null && this.soundHolder != null)
			{
				Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.defendSounds, 0.8f, base.transform.position);
			}
		}
	}

	public virtual void SetAlight_Local()
	{
		if (!this.destroyed && this.flamable && this.burnDamage == 0)
		{
			if (this.burnTime == 0f)
			{
			}
			this.burnTime += 4f;
			this.burnDamage++;
		}
	}

	public virtual void Burn(DamageObject damgeObject)
	{
		if (!this.destroyed)
		{
			if (this.flamable)
			{
				if (this.destroyed || this.burnTime == 0f)
				{
				}
				this.burnTime = 8f;
				this.burnDamage += damgeObject.damage;
				if (this.burnDamage > this.BurnCollapsePoint())
				{
					this.burnTime = 0f;
					this.DamageInternal(this.burnDamage, 0f, 0f);
				}
			}
			else if (this.heatable)
			{
				this.burnTime += 0.5f;
				this.burnDamage++;
			}
			else if (this.explosive)
			{
				this.DamageInternal(this.burnDamage, 0f, 0f);
			}
		}
	}

	protected virtual int BurnCollapsePoint()
	{
		return this.health * 3;
	}

	public virtual void ForceBurn()
	{
		if (!this.destroyed)
		{
			if (this.destroyed || this.burnTime != 0f || this.flamable)
			{
			}
			this.burnTime += 6f + this.random.value * 4f;
			this.burnDamage += 2;
			if (this.burnDamage > this.BurnCollapsePoint())
			{
				this.burnTime = 0f;
				this.DamageInternal(this.burnDamage, 0f, 0f);
			}
		}
		else if (this.heatable)
		{
			this.burnTime += 0.5f;
			this.burnDamage++;
		}
		else if (this.explosive)
		{
			this.DamageInternal(1, 0f, 0f);
		}
	}

	protected virtual void PlayDeathSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null && this.soundHolder != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.deathSounds, this.soundVolume, base.transform.position);
		}
	}

	public void CheckForMine()
	{
		if (this.mine != null && !this.mine.exploded && !this.mine.detonated)
		{
			Networking.RPC<bool>(PID.TargetAll, new RpcSignature<bool>(this.mine.Detonate), false, false);
		}
	}

	public virtual void StepOn(TestVanDammeAnim unit)
	{
	}

	public virtual void StepOn(Grenade grenade)
	{
	}

	public virtual void StepOnBlock()
	{
	}

	public virtual void StepOnBlockForced()
	{
	}

	private void Destroy()
	{
		MonoBehaviour.print("Destroy " + base.gameObject);
	}

	protected int FindBigBlockFirstCollumn(int c, GroundType currentGroundType)
	{
		bool flag = false;
		int num = 200;
		while (!flag && c >= 0)
		{
			flag = (Map.GetGroundType(c - 1, this.row, GroundType.BrickBehind) != currentGroundType);
			if (!flag)
			{
				c--;
			}
			num--;
			if (num < 0)
			{
				UnityEngine.Debug.LogError("WTF");
				break;
			}
		}
		return c;
	}

	protected int FindBackgroundBlockFirstCollumn(int c, GroundType currentGroundType)
	{
		bool flag = false;
		int num = 200;
		while (!flag && c >= 0)
		{
			flag = (Map.GetBackgroundGroundType(c - 1, this.row, GroundType.BrickBehind) != currentGroundType);
			if (!flag)
			{
				c--;
			}
			num--;
			if (num < 0)
			{
				UnityEngine.Debug.LogError("WTF");
				break;
			}
		}
		return c;
	}

	protected int FindBigBlockLastCollumn(int c, GroundType currentGroundType)
	{
		bool flag = false;
		int num = 200;
		while (!flag && c < Map.MapData.Width)
		{
			flag = (Map.GetGroundType(c + 1, this.row, GroundType.BrickBehind) != currentGroundType);
			if (!flag)
			{
				c++;
			}
			num--;
			if (num < 0)
			{
				UnityEngine.Debug.LogError("WTF");
				break;
			}
		}
		return c;
	}

	protected int FindBackgroundBlockLastCollumn(int c, GroundType currentGroundType)
	{
		bool flag = false;
		int num = 200;
		while (!flag && c < Map.MapData.Width)
		{
			flag = (Map.GetBackgroundGroundType(c + 1, this.row, GroundType.BrickBehind) != currentGroundType);
			if (!flag)
			{
				c++;
			}
			num--;
			if (num < 0)
			{
				UnityEngine.Debug.LogError("WTF");
				break;
			}
		}
		return c;
	}

	protected int FindBigBlockTopRow(int r, GroundType currentGroundType)
	{
		bool flag = false;
		int num = 200;
		while (!flag)
		{
			GroundType groundType = Map.GetGroundType(this.collumn, r + 1, GroundType.BrickBehind);
			flag = (groundType != currentGroundType);
			if (!flag)
			{
				r++;
			}
			num--;
			if (num < 0)
			{
				UnityEngine.Debug.LogError("WTF");
				break;
			}
		}
		return r;
	}

	protected int FindBigBlockBottomRow(int r, GroundType currentGroundType)
	{
		bool flag = false;
		int num = 200;
		while (!flag)
		{
			flag = (Map.GetGroundType(this.collumn, r - 1, GroundType.BrickBehind) != currentGroundType);
			if (!flag)
			{
				r--;
			}
			num--;
			if (num < 0)
			{
				UnityEngine.Debug.LogError("WTF");
				break;
			}
		}
		return r;
	}

	protected int FindBackgroundBlockTopRow(int r, GroundType currentGroundType)
	{
		bool flag = false;
		int num = 200;
		while (!flag)
		{
			flag = (Map.GetBackgroundGroundType(this.collumn, r + 1, GroundType.BrickBehind) != currentGroundType);
			if (!flag)
			{
				r++;
			}
			num--;
			if (num < 0)
			{
				UnityEngine.Debug.LogError("WTF");
				break;
			}
		}
		return r;
	}

	protected BlockPiece AddBlockPiece(BlockPiece[] prefabs, bool isBackgroundMesh, bool isCentreTile)
	{
		if (isBackgroundMesh)
		{
			this.backgroundMesh = (UnityEngine.Object.Instantiate(prefabs[UnityEngine.Random.Range(0, prefabs.Length)], base.transform.position + Vector3.forward * 12f, Quaternion.identity) as BlockPiece);
			this.backgroundMesh.transform.parent = base.transform;
			if (this.forceCenterTileBackgroundMaterial != null)
			{
				this.backgroundMesh.GetComponent<Renderer>().sharedMaterial = this.forceCenterTileBackgroundMaterial;
			}
			return this.backgroundMesh;
		}
		if (isCentreTile)
		{
			this.centreObject = (UnityEngine.Object.Instantiate(prefabs[UnityEngine.Random.Range(0, prefabs.Length)], base.transform.position + Vector3.forward * (0.01f + this.foregroundCentreZOffset), Quaternion.identity) as BlockPiece);
			this.centreObject.transform.parent = base.transform;
			if (this.forceCenterTileForegroundMaterial != null)
			{
				this.centreObject.GetComponent<Renderer>().sharedMaterial = this.forceCenterTileForegroundMaterial;
			}
			return this.centreObject;
		}
		BlockPiece blockPiece = UnityEngine.Object.Instantiate(prefabs[UnityEngine.Random.Range(0, prefabs.Length)], base.transform.position - Vector3.forward * 3f, Quaternion.identity) as BlockPiece;
		blockPiece.transform.parent = base.transform;
		if (this.forceEdgeForegroundMaterial != null)
		{
			MonoBehaviour.print(blockPiece);
			blockPiece.GetComponent<Renderer>().sharedMaterial = this.forceEdgeForegroundMaterial;
		}
		return blockPiece;
	}

	protected BlockPiece AddForegroundPiece(BlockPiece[] prefabs)
	{
		return this.AddForegroundPiece(prefabs, 2f - UnityEngine.Random.value * 0.06f);
	}

	protected virtual BlockPiece AddForegroundDecorationPiece(BlockPiece[] prefabs)
	{
		return this.AddForegroundDecorationPiece(prefabs, -3f);
	}

	protected virtual BlockPiece AddForegroundDecorationPiece(BlockPiece[] prefabs, float zOffset)
	{
		BlockPiece blockPiece = UnityEngine.Object.Instantiate(prefabs[UnityEngine.Random.Range(0, prefabs.Length)], base.transform.position + Vector3.forward * zOffset, Quaternion.identity) as BlockPiece;
		blockPiece.transform.parent = base.transform;
		return blockPiece;
	}

	protected BlockPiece AddForegroundPiece(BlockPiece[] prefabs, float zOffset)
	{
		BlockPiece blockPiece = UnityEngine.Object.Instantiate(prefabs[UnityEngine.Random.Range(0, prefabs.Length)], base.transform.position + Vector3.forward * zOffset, Quaternion.identity) as BlockPiece;
		blockPiece.transform.parent = base.transform;
		return blockPiece;
	}

	protected BlockPiece AddEdgePiece(BlockPiece[] prefabs)
	{
		BlockPiece blockPiece = UnityEngine.Object.Instantiate(prefabs[UnityEngine.Random.Range(0, prefabs.Length)], base.transform.position + Vector3.forward * 6f, Quaternion.identity) as BlockPiece;
		blockPiece.transform.parent = base.transform;
		return blockPiece;
	}

	public override UnityStream PackState(UnityStream stream)
	{
		if (!this.destroyed && this is FallingBlock && (this.collumn != this.initialColumn || this.row != this.initialRow))
		{
			stream.Serialize<int>(this.collumn);
			stream.Serialize<int>(this.row);
			stream.Serialize<int>(((FallingBlock)this).Rotation);
		}
		return base.PackState(stream);
	}

	public override UnityStream UnpackState(UnityStream stream)
	{
		int num = (int)stream.DeserializeNext();
		int col = (int)stream.DeserializeNext();
		int num2 = (int)stream.DeserializeNext();
		this.SetPosition(num, col);
		((FallingBlock)this).SetRotation((float)num2);
		return base.UnpackState(stream);
	}

	public virtual bool IsSupported()
	{
		if (this.fallingStructure)
		{
			if (!this.destroyed && this.health > 0 && !this.disturbed)
			{
				int num = 3;
				for (int i = 0; i < num; i++)
				{
					Block block = Map.GetBlock(this.collumn + i, this.row);
					if (block == null)
					{
						break;
					}
					if (block.health <= 0)
					{
						break;
					}
					if (block.supportedBy != null || block.IsSolid())
					{
						return true;
					}
				}
				for (int j = 0; j < num; j++)
				{
					Block block2 = Map.GetBlock(this.collumn - j, this.row);
					if (block2 == null)
					{
						break;
					}
					if (block2.health <= 0)
					{
						break;
					}
					if (block2.supportedBy != null)
					{
						return true;
					}
				}
			}
			return false;
		}
		return true;
	}

	public GroundType replacementBlockType = GroundType.Empty;

	public bool replaceOnCollapse;

	public bool flamable;

	public bool heatable;

	public bool explosive;

	public int burnDamage;

	public float burnTime;

	public float burnCounter;

	public float burnUnitsCounter;

	public float flameCounter;

	public int size = 1;

	public int height = 1;

	public Shrapnel shrapnelPrefab;

	public Shrapnel shrapnelBitPrefab;

	public GameObject[] attachedObjects;

	public GameObject[] attachedObjectsLeft;

	public GameObject[] attachedObjectsRight;

	public GameObject[] attachedObjectsBelow;

	public bool disturbed;

	public bool fallingStructure;

	public BlockPiece topEdge;

	public BlockPiece bottomEdge;

	public BlockPiece leftEdge;

	public BlockPiece rightEdge;

	public BlockPiece centreObject;

	protected BlockPiece backgroundMesh;

	public Mine mine;

	public Spikes spikes;

	public List<BlockPiece> addedObjects;

	public SpriteSM backgroundEdgeBroken;

	public GameObject topEdgeBroken;

	public GameObject bottomEdgeBroken;

	public GameObject leftEdgeBroken;

	public GameObject rightEdgeBroken;

	public GameObject bloodSprayTop;

	public GameObject bloodSprayBottom;

	public GameObject bloodSprayLeft;

	public GameObject bloodSprayRight;

	public BlockPiece bloodSprayTopPiece;

	public BlockPiece bloodSprayBottomPiece;

	public BlockPiece bloodSprayLeftPiece;

	public BlockPiece bloodSprayRightPiece;

	public GameObject bloodSprayTopHidden;

	public GameObject bloodSprayBottomHidden;

	public GameObject bloodSprayLeftHidden;

	public GameObject bloodSprayRightHidden;

	protected List<GameObject> otherAttachments = new List<GameObject>();

	protected List<Doodad> doodadAttachments = new List<Doodad>();

	public int row;

	public int collumn = -1;

	public int initialRow;

	public int initialColumn = -1;

	public GroundType groundType = GroundType.Earth;

	public Block aboveBlock;

	public Block belowBlock;

	protected float destroyCounter;

	[HideInInspector]
	public bool destroyed;

	private bool collapseBlocksAround = true;

	public float collapseChance = 1f;

	public SoundHolder soundHolder;

	protected LayerMask groundLayer;

	protected int lowerLeftPixelX;

	protected int lowerLeftPixelY = 16;

	protected Vector3 spriteOffset;

	public DoodadScaffolding supportedBy;

	protected DamageObject lastDamageObject;

	protected SpriteSM sprite;

	protected Randomf random;

	protected Randomf burnRandom;

	protected Randomf effectsRandom;

	protected Randomf collapseRandom;

	protected float collapseDelayTime = 0.2f;

	protected bool useLargeWindowPieces;

	protected bool useLargeMuralPieces;

	protected int firstBlockOfTypeCollumn;

	protected int firstBlockOfTypeRow;

	protected int blocksOfTypeCollumns;

	protected int blocksOfTypeRows;

	protected int tilingOffset;

	public float foregroundCentreZOffset = 2f;

	protected bool bloodyTop;

	protected bool bloodyBottom;

	protected bool bloodyRight;

	protected bool bloodyLeft;

	protected bool hiddenTop;

	protected bool hiddenBottom;

	protected bool hiddenRight;

	protected bool hiddenLeft;

	protected bool collapseBelow;

	protected RaycastHit groundHit;

	protected float pushForce;

	protected bool named;

	protected Sound sound;

	public float soundVolume = 0.22f;

	public Material forceCenterTileBackgroundMaterial;

	public Material forceCenterTileForegroundMaterial;

	public Material forceEdgeForegroundMaterial;
}
