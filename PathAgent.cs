// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Linq;
using UnityEngine;

public class PathAgent : MonoBehaviour
{
	public NavPath CurrentPath
	{
		get
		{
			return this.currentPath;
		}
		set
		{
			this.timeStuckOnThisNode = 0f;
			this.currentNode = 0;
			this.currentPath = value;
			this.InitCurrentNavPoint();
		}
	}

	public bool hasPath
	{
		get
		{
			return this.CurrentPath != null;
		}
	}

	public void GetPath(int col, int row, float timeLimit)
	{
		NavPath navPath = this.CurrentPath;
		NavPath navPath2 = PathfindingController.FindPath(Map.GetGridPoint(base.transform.position), new GridPoint(col, row), this.capabilities, timeLimit);
		if (navPath != null && navPath2 != null)
		{
			this.currentPath = this.MergePaths(navPath, navPath2);
		}
		else
		{
			this.currentPath = navPath2;
		}
	}

	private NavPath MergePaths(NavPath oldPath, NavPath newPath)
	{
		bool flag = false;
		int i;
		for (i = 0; i < newPath.points.Count; i++)
		{
			if (oldPath.points[oldPath.points.Count - 1].point == newPath.points[i].point && oldPath.points[oldPath.points.Count - 1].moveMode == newPath.points[i].moveMode)
			{
				flag = true;
			}
		}
		if (flag)
		{
			for (int j = i; j < newPath.points.Count; j++)
			{
				oldPath.points.Add(newPath.points[j]);
			}
			return oldPath;
		}
		return newPath;
	}

	private void Awake()
	{
		this.unit = base.GetComponent<TestVanDammeAnim>();
	}

	public bool GetMove(ref bool left, ref bool right, ref bool up, ref bool down, ref bool jump)
	{
		GridPoint currentAgentGridPoint = new GridPoint();
		currentAgentGridPoint.collumn = Map.GetCollumn(base.transform.position.x);
		currentAgentGridPoint.row = Map.GetRow(base.transform.position.y);
		jump = false; left = (right = (up = (down = (jump ))));
		if ((this.direction > 0 && this.currentAgentPos.x > this.currentTargetWorldPoint.x) || (this.direction < 0 && this.currentAgentPos.x < this.currentTargetWorldPoint.x) || Vector3.SqrMagnitude(base.transform.position - Extensions.Vec2toVec3(this.currentTargetWorldPoint)) < this.agentSize * this.agentSize)
		{
			this.currentNode++;
			this.timeStuckOnThisNode = 0f;
			this.InitCurrentNavPoint();
		}
		if (this.currentNode == 0 && this.currentPath != null && this.currentPath.points != null && this.currentPath.points.Count > 1 && this.currentPath.points[0].moveMode == PathMoveMode.Normal && this.currentPath.points[1].moveMode == PathMoveMode.Normal && this.currentPath.points[0].row == this.currentPath.points[1].row)
		{
			if (Map.GetBlockCenter(this.currentPath.points[0].collumn, this.currentPath.points[0].row).x < base.transform.position.x && Map.GetBlockCenter(this.currentPath.points[1].collumn, this.currentPath.points[1].row).x > base.transform.position.x)
			{
				this.currentNode++;
				this.timeStuckOnThisNode = 0f;
				this.InitCurrentNavPoint();
			}
			else if (Map.GetBlockCenter(this.currentPath.points[0].collumn, this.currentPath.points[0].row).x > base.transform.position.x && Map.GetBlockCenter(this.currentPath.points[1].collumn, this.currentPath.points[1].row).x < base.transform.position.x)
			{
				this.currentNode++;
				this.timeStuckOnThisNode = 0f;
				this.InitCurrentNavPoint();
			}
		}
		if (this.CurrentPath != null && this.currentNode >= this.CurrentPath.points.Count)
		{
			this.CurrentPath = null;
		}
		if (this.CurrentPath == null)
		{
			return true;
		}
		this.currentTargetWorldPoint = Map.GetBlockCenter(this.CurrentPath.points[this.currentNode].collumn, this.CurrentPath.points[this.currentNode].row);
		this.direction = this.currentPath.points[this.currentNode].collumn - currentAgentGridPoint.collumn;
		if (this.direction != 0)
		{
			this.direction /= Mathf.Abs(this.direction);
		}
		this.currentAgentPos = base.transform.position;
		if (this.currentNode == this.CurrentPath.points.Count)
		{
			this.CurrentPath = null;
			return true;
		}
		PathPoint pathPoint = this.CurrentPath.points[this.currentNode];
		this.currentTargetWorldPoint = Map.GetBlockCenter(this.CurrentPath.points[this.currentNode].collumn, this.CurrentPath.points[this.currentNode].row);
		if (this.capabilities.flying)
		{
			this.ApplyNaiveDirectionalMovement(ref up, ref down, ref left, ref right);
		}
		else
		{
			this.currentAgentPos.y = this.currentAgentPos.y + 8f;
			if (pathPoint.moveMode == PathMoveMode.Jump)
			{
				if (!this.haveReachedCenter)
				{
					if (this.currentAgentPos.x > this.tileStartPos.x)
					{
						left = true;
					}
					else
					{
						right = true;
					}
					if (Mathf.Abs(this.currentAgentPos.x - this.tileStartPos.x) < this.agentSize / 2f || this.CurrentPath.points[this.currentNode].collumn == currentAgentGridPoint.collumn)
					{
						this.haveReachedCenter = true;
					}
				}
				else
				{
					this.ApplyNaiveDirectionalMovement(ref up, ref down, ref left, ref right);
					if (this.unit.actionState != ActionState.Jumping)
					{
						if (Map.IsBlockSolid(currentAgentGridPoint.collumn + this.direction, currentAgentGridPoint.row) || Mathf.Abs(this.currentAgentPos.x - this.tileStartPos.x) > this.agentSize * 0.75f || this.CurrentPath.points[this.currentNode].collumn == currentAgentGridPoint.collumn)
						{
							jump = true;
						}
					}
					else if (this.currentAgentPos.y < this.currentTargetWorldPoint.y)
					{
						jump = true;
					}
				}
			}
			else
			{
				this.ApplyNaiveDirectionalMovement(ref up, ref down, ref left, ref right);
			}
			if (this.unit.actionState == ActionState.Jumping && this.currentAgentPos.y >= this.currentTargetWorldPoint.y && ((this.tileStartPos.x < this.currentTargetWorldPoint.x && this.currentAgentPos.x >= this.currentTargetWorldPoint.x) || (this.tileStartPos.x > this.currentTargetWorldPoint.x && this.currentAgentPos.x <= this.currentTargetWorldPoint.x)))
			{
				this.currentNode++;
				this.timeStuckOnThisNode = 0f;
				this.InitCurrentNavPoint();
				right = false; left = (right );
			}
		}
		if (pathPoint.moveMode == PathMoveMode.WallClimbLeft)
		{
			jump = true;
			up = true;
			left = true;
			right = false;
		}
		if (pathPoint.moveMode == PathMoveMode.WallClimbRight)
		{
			jump = true;
			up = true;
			right = true;
			left = false;
		}
		PathPoint pathPoint2 = this.CurrentPath.points.FirstOrDefault((PathPoint p) => p.point == currentAgentGridPoint);
		if (pathPoint2 != null && this.CurrentPath.points.IndexOf(pathPoint2) > this.currentNode)
		{
			this.currentNode = this.CurrentPath.points.IndexOf(pathPoint2);
			this.timeStuckOnThisNode = 0f;
		}
		this.timeStuckOnThisNode += Time.deltaTime;
		if (this.timeStuckOnThisNode > 0.6f && this.timeStuckOnThisNode < 0.7f)
		{
			jump = false; up = (left = (right = (down = (jump ))));
		}
		else if (this.timeStuckOnThisNode > 1f)
		{
			this.CurrentPath = null;
		}
		return false;
	}

	private void ApplyNaiveDirectionalMovement(ref bool up, ref bool down, ref bool left, ref bool right)
	{
		if (this.currentAgentPos.x - this.currentTargetWorldPoint.x < -1f)
		{
			right = true;
		}
		if (this.currentAgentPos.x - this.currentTargetWorldPoint.x > 1f)
		{
			left = true;
		}
		if (this.unit != null && this.direction == 0 && Mathf.Abs(this.currentAgentPos.x - this.currentTargetWorldPoint.x) < 2f)
		{
			this.unit.xI *= 0.5f;
		}
		if (this.currentAgentPos.y - this.currentTargetWorldPoint.y < 0f)
		{
			up = true;
		}
		if (this.currentAgentPos.y - this.currentTargetWorldPoint.y > 0f)
		{
			down = true;
		}
	}

	private void InitCurrentNavPoint()
	{
		this.haveReachedCenter = false;
		if (this.currentNode > 0 && this.currentNode < this.CurrentPath.points.Count)
		{
			this.tileStartPos = Map.GetBlockCenter(this.CurrentPath.points[this.currentNode - 1].collumn, this.CurrentPath.points[this.currentNode - 1].row);
		}
		else
		{
			GridPoint gridPoint = new GridPoint();
			gridPoint.collumn = Map.GetCollumn(base.transform.position.x);
			gridPoint.row = Map.GetRow(base.transform.position.y);
			this.tileStartPos = Map.GetBlockCenter(gridPoint.collumn, gridPoint.row);
		}
	}

	internal void GetMove(InputState input)
	{
		this.GetMove(ref input.left, ref input.right, ref input.up, ref input.down, ref input.jump);
	}

	public PathAgentCapabilities capabilities;

	public float agentSize = 5f;

	private Vector3 tileStartPos;

	private NavPath currentPath;

	protected int currentNode;

	private float timeStuckOnThisNode;

	protected TestVanDammeAnim unit;

	private bool haveReachedCenter;

	private Vector2 currentAgentPos;

	private Vector2 currentTargetWorldPoint;

	private int direction;
}
