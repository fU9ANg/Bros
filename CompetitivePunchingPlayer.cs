// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CompetitivePunchingPlayer : CutscenePlayer
{
	public override void Setup(int pNum, IntermissionScreen controller)
	{
		base.Setup(pNum, controller);
		this.punchingController = (controller as CompetitivePunchingController);
		this.currentFist.gameObject.SetActive(false);
	}

	protected void Start()
	{
		switch (this.playerNum)
		{
		case 0:
			this.punchingDirection = DirectionEnum.Right;
			this.startX = this.controller.minX - 20f;
			this.startY = (this.controller.minY + this.controller.maxY) / 2f;
			break;
		case 1:
			this.punchingDirection = DirectionEnum.Left;
			this.startX = this.controller.maxX + 20f;
			this.startY = (this.controller.minY + this.controller.maxY) / 2f;
			break;
		case 2:
			this.punchingDirection = DirectionEnum.Up;
			this.startY = this.controller.minY - 20f;
			this.startX = (this.controller.minX + this.controller.maxX) / 2f;
			break;
		case 3:
			this.punchingDirection = DirectionEnum.Down;
			this.startY = this.controller.maxY + 20f;
			this.startX = (this.controller.minX + this.controller.maxX) / 2f;
			break;
		default:
			UnityEngine.Debug.LogError("No Good Player Num");
			break;
		}
		this.spriteWidth = this.currentFist.pixelDimensions.x;
	}

	protected override void Update()
	{
		base.Update();
		bool flag = false || (this.fire && !this.wasFire) || (this.special && !this.wasSpecial) || (this.buttonJump && !this.buttonJump) || (this.highFive && !this.wasHighFive);
		if (flag && !this.punchingController.hasHeadExploded)
		{
			this.Punch();
		}
		this.x += this.xI * this.t;
		this.y += this.yI * this.t;
		if (this.punching)
		{
			if (this.punchingController.CheckHit(ref this.x, ref this.y, this.punchingDirection))
			{
				this.currentFist.GetComponent<Renderer>().sharedMaterial = this.fistClear;
				this.punching = false;
				this.withDrawing = true;
				this.xI *= -0.3f;
				this.yI *= -0.5f;
				this.punchingController.Punch(this.punchingDirection, this.x, this.y, this.playerNum);
			}
		}
		else if (this.withDrawing)
		{
			switch (this.punchingDirection)
			{
			case DirectionEnum.Up:
				if (this.y < this.startY - 50f)
				{
					this.Stop();
				}
				break;
			case DirectionEnum.Down:
				if (this.y > this.startY + 50f)
				{
					this.Stop();
				}
				break;
			case DirectionEnum.Left:
				if (this.x > this.startX + 50f)
				{
					this.Stop();
				}
				break;
			case DirectionEnum.Right:
				if (this.x < this.startX - 50f)
				{
					this.Stop();
				}
				break;
			}
		}
		this.SetFistPosition();
	}

	protected void Stop()
	{
		this.currentFist.gameObject.SetActive(false);
	}

	protected void SetFistPosition()
	{
		this.currentFist.transform.position = new Vector3(this.x, this.y, 10f);
	}

	protected void Punch()
	{
		this.currentFist.gameObject.SetActive(true);
		this.currentFist.GetComponent<Renderer>().sharedMaterial = this.fistBlurred;
		this.punching = true;
		this.withDrawing = false;
		this.x = this.startX;
		this.y = this.startY;
		float num = 2500f;
		switch (this.punchingDirection)
		{
		case DirectionEnum.Up:
			this.xI = 0f;
			this.yI = num;
			break;
		case DirectionEnum.Down:
			this.xI = 0f;
			this.yI = -num;
			break;
		case DirectionEnum.Left:
			this.xI = -num;
			this.yI = 0f;
			break;
		case DirectionEnum.Right:
			this.xI = num;
			this.yI = 0f;
			break;
		}
	}

	protected DirectionEnum punchingDirection;

	protected float spriteWidth = 32f;

	protected int frame;

	protected bool punching;

	protected bool withDrawing;

	protected float xI;

	protected float yI;

	protected float x;

	protected float y;

	protected float startX;

	protected float startY;

	public SpriteSM currentFist;

	public Material fistBlurred;

	public Material fistClear;

	private CompetitivePunchingController punchingController;
}
