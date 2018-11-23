// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookopterPolyAI : PolymorphicAI
{
	private void Start()
	{
		if (!Map.isEditing)
		{
			this.CheckKopterExtents();
		}
		else
		{
			this.checkBlocksAfterEdit = true;
		}
	}

	protected override void Update()
	{
		if (!Map.isEditing && this.checkBlocksAfterEdit)
		{
			this.checkBlocksAfterEdit = false;
			this.CheckKopterExtents();
		}
		base.Update();
	}

	protected override void DoIdleThink()
	{
		base.AddAction(EnemyActionType.Wait, 0.3f);
		base.AddAction(EnemyActionType.LookForPlayer);
		if (Time.time - this.lastTurnTime > 1.2f)
		{
			this.lastTurnTime = Time.time;
			base.AddAction(EnemyActionType.FacePoint, new GridPoint(this.unit.collumn - base.FacingDirection * 10, this.unit.row));
		}
	}

	protected override void Think()
	{
		KopterPattern kopterPattern = this.kopterPattern;
		base.Think();
		if (kopterPattern != this.kopterPattern)
		{
			Networking.RPC<KopterPattern>(PID.TargetOthers, new RpcSignature<KopterPattern>(this.SyncPattern), this.kopterPattern, false);
		}
	}

	private void SyncPattern(KopterPattern pattern)
	{
		MonoBehaviour.print("SyncPattern " + pattern);
		this.kopterPattern = pattern;
	}

	protected override void DoAlertedThink()
	{
		MonoBehaviour.print(this.kopterPattern);
		this.kopterPattern++;
		float num = 0f;
		float value = 0f;
		int nearestPlayer = HeroController.GetNearestPlayer(base.transform.position.x, base.transform.position.y, (float)this.sightRangeX, (float)this.sightRangeY);
		if (nearestPlayer >= 0)
		{
			this.seenPlayerNum = nearestPlayer;
		}
		HeroController.GetPlayerPos(this.seenPlayerNum, ref num, ref value);
		switch (this.kopterPattern)
		{
		case KopterPattern.Hovering:
			base.AddAction(EnemyActionType.Wait, UnityEngine.Random.Range(0.5f, 1.5f));
			this.kopterPattern = KopterPattern.MovingLeft;
			break;
		case KopterPattern.MovingLeft:
			base.AddAction(EnemyActionType.Move, new GridPoint
			{
				row = this.unit.row,
				collumn = Map.GetCollumn(this.xMin)
			});
			base.AddAction(EnemyActionType.Wait, 1.5f);
			break;
		case KopterPattern.DescendingLeft:
			if (num < this.xMax)
			{
				base.AddAction(EnemyActionType.Move, new GridPoint
				{
					row = Map.GetRow(Mathf.Clamp(value, this.xMinMinHeight, this.xMinMaxHeight)),
					collumn = Map.GetCollumn(this.xMin)
				});
				base.AddAction(EnemyActionType.FacePoint, new GridPoint(Map.GetCollumn(num), this.unit.row));
				base.AddAction(EnemyActionType.Wait, 1f);
				base.AddAction(EnemyActionType.Fire);
				base.AddAction(EnemyActionType.Wait, 2f);
			}
			else
			{
				this.kopterPattern--;
				base.AddAction(EnemyActionType.Wait, 1f);
			}
			break;
		case KopterPattern.AscendingLeft:
			base.AddAction(EnemyActionType.Move, new GridPoint
			{
				row = Map.GetRow(Mathf.Clamp(value, this.xMinMinHeight, this.xMinMaxHeight)),
				collumn = Map.GetCollumn(this.xMin)
			});
			base.AddAction(EnemyActionType.FacePoint, new GridPoint(Map.GetCollumn(num), this.unit.row));
			base.AddAction(EnemyActionType.Wait, 1f);
			base.AddAction(EnemyActionType.Fire);
			base.AddAction(EnemyActionType.Wait, 2f);
			base.AddAction(EnemyActionType.Move, new GridPoint
			{
				row = Map.GetRow(this.xMinMaxHeight * 0.75f),
				collumn = Map.GetCollumn(this.xMin)
			});
			base.AddAction(EnemyActionType.Wait, 1f);
			break;
		case KopterPattern.MovingRight:
			base.AddAction(EnemyActionType.Move, new GridPoint
			{
				row = Map.GetRow(this.xMaxMaxHeight * 0.75f),
				collumn = Map.GetCollumn(this.xMax)
			});
			base.AddAction(EnemyActionType.Wait, 1.5f);
			break;
		case KopterPattern.DescendingRight:
			if (num > this.xMin)
			{
				base.AddAction(EnemyActionType.Move, new GridPoint
				{
					row = Map.GetRow(Mathf.Clamp(value, this.xMaxMinHeight, this.xMaxMaxHeight)),
					collumn = Map.GetCollumn(this.xMax)
				});
				base.AddAction(EnemyActionType.FacePoint, new GridPoint(Map.GetCollumn(num), this.unit.row));
				base.AddAction(EnemyActionType.Wait, 1f);
				base.AddAction(EnemyActionType.Fire);
				base.AddAction(EnemyActionType.Wait, 2f);
			}
			else
			{
				this.kopterPattern--;
				base.AddAction(EnemyActionType.Wait, 1f);
			}
			break;
		case KopterPattern.AscendingRight:
			base.AddAction(EnemyActionType.Move, new GridPoint
			{
				row = Map.GetRow(Mathf.Clamp(value, this.xMaxMinHeight, this.xMaxMaxHeight)),
				collumn = Map.GetCollumn(this.xMax)
			});
			base.AddAction(EnemyActionType.FacePoint, new GridPoint(Map.GetCollumn(num), this.unit.row));
			base.AddAction(EnemyActionType.Wait, 1f);
			base.AddAction(EnemyActionType.Fire);
			base.AddAction(EnemyActionType.Wait, 2f);
			base.AddAction(EnemyActionType.Move, new GridPoint
			{
				row = Map.GetRow(this.xMaxMaxHeight * 0.75f),
				collumn = Map.GetCollumn(this.xMax)
			});
			base.AddAction(EnemyActionType.Wait, 1f);
			break;
		case KopterPattern.Restart:
			this.kopterPattern = KopterPattern.Hovering;
			break;
		}
	}

	public override void Attract(float xTarget, float yTarget)
	{
	}

	public override void HearSound(float alertX, float alertY)
	{
	}

	public override void Blind()
	{
	}

	protected override void RunQueue()
	{
		if (base.CurrentAction != null)
		{
			EnemyActionType type = base.CurrentAction.type;
			if (type != EnemyActionType.Move)
			{
				base.RunQueue();
			}
			else
			{
				if (Mathf.Abs(this.unit.x - base.GetTargetX(base.CurrentAction.gridPoint)) < 4f && Mathf.Abs(this.unit.y - base.GetTargetY(base.CurrentAction.gridPoint)) < 4f)
				{
					this.actionQueue.Remove(base.CurrentAction);
				}
				this.walkDirection = base.FacingDirection;
			}
		}
	}

	public override void GetInput(ref bool left, ref bool right, ref bool up, ref bool down, ref bool jump, ref bool fire, ref bool special1, ref bool special2, ref bool special3, ref bool special4)
	{
		special4 = false; left = (right = (up = (down = (jump = (fire = (special1 = (special2 = (special3 = (special4 )))))))));
		ActionObject actionObject = (this.actionQueue.Count <= 0) ? null : this.actionQueue[0];
		if (actionObject != null)
		{
			switch (actionObject.type)
			{
			case EnemyActionType.Fire:
				fire = true;
				break;
			case EnemyActionType.Move:
				if (Mathf.Abs(base.transform.position.y - base.GetTargetY(actionObject.gridPoint)) > 2f)
				{
					if (base.transform.position.y < base.GetTargetY(base.CurrentAction.gridPoint))
					{
						up = true;
					}
					else if (base.transform.position.y > base.GetTargetY(base.CurrentAction.gridPoint))
					{
						down = true;
					}
				}
				else if (Mathf.Abs(base.transform.position.x - base.GetTargetX(actionObject.gridPoint)) > 2f)
				{
					if (base.transform.position.x > base.GetTargetX(actionObject.gridPoint))
					{
						left = true;
					}
					else if (base.transform.position.x < base.GetTargetX(actionObject.gridPoint))
					{
						right = true;
					}
				}
				break;
			case EnemyActionType.FacePoint:
				MonoBehaviour.print(string.Concat(new object[]
				{
					this.unit.collumn,
					", ",
					actionObject.gridPoint.collumn,
					", ",
					base.FacingDirection
				}));
				if (this.unit.collumn > actionObject.gridPoint.collumn && base.FacingDirection > 0)
				{
					left = true;
				}
				if (this.unit.collumn < actionObject.gridPoint.collumn && base.FacingDirection < 0)
				{
					right = true;
				}
				break;
			case EnemyActionType.UseSpecial:
				special1 = true;
				break;
			case EnemyActionType.UseSpecial2:
				special2 = true;
				break;
			case EnemyActionType.UseSpecial3:
				special3 = true;
				break;
			case EnemyActionType.UseSpecial4:
				special4 = true;
				break;
			}
			if ((this.kopterPattern == KopterPattern.MovingLeft || this.kopterPattern == KopterPattern.MovingRight) && base.CurrentAction.type != EnemyActionType.Wait)
			{
				special1 = true;
			}
		}
	}

	protected virtual void CheckKopterExtents()
	{
		if (!Map.isEditing)
		{
			this.checkBlocksAfterEdit = false;
			this.startX = this.unit.x;
			this.startY = this.unit.y;
			this.xMin = this.startX - 160f;
			this.xMax = this.startX + 160f;
			this.CheckGroundHeight();
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 42f, 0f), Vector3.left, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x + 64f > this.xMin)
			{
				this.xMin = this.rayCastHit.point.x + 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 56f, 0f), Vector3.left, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x + 64f > this.xMin)
			{
				this.xMin = this.rayCastHit.point.x + 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 64f, 0f), Vector3.left, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x + 64f > this.xMin)
			{
				this.xMin = this.rayCastHit.point.x + 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 80f, 0f), Vector3.left, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x + 64f > this.xMin)
			{
				this.xMin = this.rayCastHit.point.x + 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 96f, 0f), Vector3.left, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x + 64f > this.xMin)
			{
				this.xMin = this.rayCastHit.point.x + 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 112f, 0f), Vector3.left, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x + 64f > this.xMin)
			{
				this.xMin = this.rayCastHit.point.x + 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 42f, 0f), Vector3.right, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x - 64f < this.xMax)
			{
				this.xMax = this.rayCastHit.point.x - 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 56f, 0f), Vector3.right, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x - 64f < this.xMax)
			{
				this.xMax = this.rayCastHit.point.x - 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 64f, 0f), Vector3.right, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x - 64f < this.xMax)
			{
				this.xMax = this.rayCastHit.point.x - 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 80f, 0f), Vector3.right, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x - 64f < this.xMax)
			{
				this.xMax = this.rayCastHit.point.x - 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 96f, 0f), Vector3.right, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x - 64f < this.xMax)
			{
				this.xMax = this.rayCastHit.point.x - 64f;
			}
			if (Physics.Raycast(new Vector3(this.unit.x, this.groundHeight + 112f, 0f), Vector3.right, out this.rayCastHit, 224f, Map.groundLayer) && this.rayCastHit.point.x - 64f < this.xMax)
			{
				this.xMax = this.rayCastHit.point.x - 64f;
			}
			if (this.xMin > this.xMax)
			{
				this.xMin = this.unit.x; this.xMax = (this.xMin );
			}
			this.CheckHeight();
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"CHECK BLOCKS!   xMin ",
				this.xMin,
				" xMax ",
				this.xMax,
				" xmin MIN  y ",
				this.xMinMinHeight,
				" xmax MIN y ",
				this.xMaxMinHeight,
				" xminMax y ",
				this.xMinMaxHeight,
				" xmaxMax y ",
				this.xMaxMaxHeight
			}));
			return;
		}
		UnityEngine.Debug.Log("Mook kopter editing start " + this.unit.x);
		this.checkBlocksAfterEdit = true;
	}

	protected void CheckGroundHeight()
	{
		this.groundHeight = this.startY;
		if (Physics.Raycast(new Vector3(this.unit.x, this.unit.y + 16f, 0f), Vector3.down, out this.rayCastHit, 64f, Map.groundLayer))
		{
			this.groundHeight = this.rayCastHit.point.y;
		}
		if (Physics.Raycast(new Vector3(this.unit.x - 16f, this.unit.y + 16f, 0f), Vector3.down, out this.rayCastHit, 64f, Map.groundLayer) && this.rayCastHit.point.y > this.groundHeight)
		{
			this.groundHeight = this.rayCastHit.point.y;
		}
		if (Physics.Raycast(new Vector3(this.unit.x + 16f, this.unit.y + 16f, 0f), Vector3.down, out this.rayCastHit, 64f, Map.groundLayer) && this.rayCastHit.point.y > this.groundHeight)
		{
			this.groundHeight = this.rayCastHit.point.y;
		}
	}

	protected void CheckHeight()
	{
		this.xMaxMinHeight = this.groundHeight - 48f; this.xMinMinHeight = (this.xMaxMinHeight );
		if (Physics.Raycast(new Vector3(this.xMin, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 64f, Map.groundLayer))
		{
			this.xMinMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMin + 16f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 24f > this.xMinMinHeight)
		{
			this.xMinMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMin - 16f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMinMinHeight)
		{
			this.xMinMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMin - 24f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMinMinHeight)
		{
			this.xMinMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMin + 24f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMinMinHeight)
		{
			this.xMinMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMin + 36f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMinMinHeight)
		{
			this.xMinMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMin - 36f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMinMinHeight)
		{
			this.xMinMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMin + 48f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMinMinHeight)
		{
			this.xMinMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMin - 48f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMinMinHeight)
		{
			this.xMinMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMax, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer))
		{
			this.xMaxMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMax - 16f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMaxMinHeight)
		{
			this.xMaxMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMax + 16f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMaxMinHeight)
		{
			this.xMaxMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMax + 24f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMaxMinHeight)
		{
			this.xMaxMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMax - 24f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMaxMinHeight)
		{
			this.xMaxMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMax - 36f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMaxMinHeight)
		{
			this.xMaxMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMax + 36f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMaxMinHeight)
		{
			this.xMaxMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMax - 48f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMaxMinHeight)
		{
			this.xMaxMinHeight = this.rayCastHit.point.y + 28f;
		}
		if (Physics.Raycast(new Vector3(this.xMax + 48f, this.groundHeight + 64f, 0f), Vector3.down, out this.rayCastHit, 90f, Map.groundLayer) && this.rayCastHit.point.y + 28f > this.xMaxMinHeight)
		{
			this.xMaxMinHeight = this.rayCastHit.point.y + 28f;
		}
		UnityEngine.Debug.Log("xMaxMinHeight " + this.xMaxMinHeight);
		this.xMinMaxHeight = Mathf.Max(new float[]
		{
			this.xMinMinHeight + 32f,
			this.xMaxMinHeight + 16f,
			this.groundHeight + 128f
		});
		this.xMaxMaxHeight = Mathf.Max(new float[]
		{
			this.xMaxMinHeight + 32f,
			this.xMinMinHeight + 16f,
			this.groundHeight + 128f
		});
	}

	public KopterPattern kopterPattern;

	protected float xMin;

	protected float xMax;

	protected float xMinMinHeight;

	protected float xMaxMinHeight;

	protected float xMinMaxHeight;

	protected float xMaxMaxHeight;

	protected float startX;

	protected float startY;

	protected float groundHeight = 128f;

	protected bool checkBlocksAfterEdit;

	private float lastTurnTime;
}
