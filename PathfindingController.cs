// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingController
{
	public static NavPath FindPath(GridPoint fromPoint, GridPoint toPoint, PathAgentCapabilities capabilities, float timeLimitInMilliseconds)
	{
		PathfindingController.openSet.Clear();
		PathfindingController.closedSet.Clear();
		PathPoint pathPoint = new PathPoint(fromPoint, 0f);
		pathPoint.fScore = (pathPoint.fScoreHeuristic = (float)PathfindingController.CalcFScore(fromPoint, toPoint));
		PathfindingController.openSet.Add(pathPoint);
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		while (PathfindingController.openSet.Count > 0)
		{
			PathPoint pathPoint2 = (from point in PathfindingController.openSet
			orderby point.fScore
			select point).First<PathPoint>();
			if (pathPoint2.point == toPoint)
			{
				return PathfindingController.ConstructPath(pathPoint2);
			}
			PathPoint p;
			if ((Time.realtimeSinceStartup - realtimeSinceStartup) * 1000f > timeLimitInMilliseconds)
			{
                /*
				PathPoint last = (from point in PathfindingController.openSet.Union(PathfindingController.closedSet)
				orderby point.fScoreHeuristic
				select point).First((PathPoint p) => p.moveMode == PathMoveMode.Normal && p.point != fromPoint && p.previous.moveMode != PathMoveMode.Jump && Map.IsBlockSolid(p.collumn, p.row - 1));
				return PathfindingController.ConstructPath(last);
                */
			}
			PathfindingController.openSet.Remove(pathPoint2);
			PathfindingController.closedSet.Add(pathPoint2);
			foreach (PathPoint p2 in PathfindingController.GetAccessiblePoints(pathPoint2, capabilities))
			{
				p = p2;
				if (!PathfindingController.IsInSet(p, PathfindingController.closedSet))
				{
					if (PathfindingController.IsInSet(p.point, PathfindingController.openSet))
					{
						PathPoint pathPoint3 = PathfindingController.openSet.First((PathPoint point) => point.collumn == p.collumn && point.row == p.row && p != point);
						if (pathPoint3.fScore <= p.fScore)
						{
							continue;
						}
						PathfindingController.openSet.Remove(pathPoint3);
						PathfindingController.closedSet.Add(pathPoint3);
					}
					p.previous = pathPoint2;
					p.fScoreHeuristic = (float)PathfindingController.CalcFScore(p.point, toPoint);
					p.fScore = p.gValue + p.fScoreHeuristic;
					PathfindingController.openSet.Add(p);
				}
			}
		}
		if (PathfindingController.openSet.Count == 0 && PathfindingController.closedSet.Count == 0)
		{
			return null;
		}
		PathPoint pathPoint4 = (from point in PathfindingController.openSet.Union(PathfindingController.closedSet)
		orderby point.fScoreHeuristic
		select point).FirstOrDefault((PathPoint p) => p.moveMode == PathMoveMode.Normal && p.point != fromPoint && p.previous.moveMode != PathMoveMode.Jump && Map.IsBlockSolid(p.collumn, p.row - 1));
		if (pathPoint4 == null)
		{
			return null;
		}
		return PathfindingController.ConstructPath(pathPoint4);
	}

	private static NavPath ConstructPath(PathPoint last)
	{
		NavPath navPath = new NavPath();
		do
		{
			navPath.AddPointToStart(last);
			last = last.previous;
		}
		while (last != null);
		return navPath;
	}

	private static IEnumerable<PathPoint> GetAccessiblePoints(PathPoint point, PathAgentCapabilities capabilities)
	{
		if (capabilities.flying)
		{
			if (!Map.IsBlockSolid(point.collumn, point.row + 1))
			{
				yield return new PathPoint(point.collumn, point.row + 1, point.gValue + 1f);
			}
			if (!Map.IsBlockSolid(point.collumn, point.row - 1))
			{
				yield return new PathPoint(point.collumn, point.row - 1, point.gValue + 1f);
			}
			if (!Map.IsBlockSolid(point.collumn - 1, point.row))
			{
				yield return new PathPoint(point.collumn - 1, point.row, point.gValue + 1f);
			}
			if (!Map.IsBlockSolid(point.collumn + 1, point.row))
			{
				yield return new PathPoint(point.collumn + 1, point.row, point.gValue + 1f);
			}
			yield break;
		}
		GridPoint nextPoint;
		if (Map.IsBlockSolid(point.collumn, point.row - 1) || Map.IsBlockLadder(point.collumn, point.row - 1))
		{
			nextPoint = new GridPoint(point.point.collumn + 1, point.point.row);
			if (!Map.IsBlockSolid(nextPoint.collumn, nextPoint.row) && !PathfindingController.IsInSet(nextPoint, PathfindingController.closedSet))
			{
				yield return new PathPoint(new GridPoint(point.point.collumn + 1, point.point.row), point.gValue + 1f);
			}
			nextPoint = new GridPoint(point.point.collumn - 1, point.point.row);
			if (!Map.IsBlockSolid(point.collumn - 1, point.row) && !PathfindingController.IsInSet(nextPoint, PathfindingController.closedSet))
			{
				yield return new PathPoint(new GridPoint(point.point.collumn - 1, point.point.row), point.gValue + 1f);
			}
			if (capabilities.canJump)
			{
				if (!Map.IsBlockLadder(point.collumn, point.row))
				{
					for (int i = 1; i < capabilities.jumpHeight; i++)
					{
						if (!Map.IsBlockSolid(point.collumn, point.row + i))
						{
							yield return new PathPoint(new GridPoint(point.collumn, point.row + i), point.gValue + 2f + (float)i, PathMoveMode.Jump);
						}
						else
						{
							i += capabilities.jumpHeight;
						}
					}
				}
				for (int j = 1; j < capabilities.jumpHeight; j++)
				{
					if (!Map.IsBlockSolid(point.collumn - j, point.row + j))
					{
						yield return new PathPoint(new GridPoint(point.collumn - j, point.row + j), point.gValue + 2.5f + (float)j * 0.75f, PathMoveMode.Jump);
					}
					else
					{
						j += capabilities.jumpHeight;
					}
				}
				for (int k = 1; k < capabilities.jumpHeight; k++)
				{
					if (!Map.IsBlockSolid(point.collumn + k, point.row + k))
					{
						yield return new PathPoint(new GridPoint(point.collumn + k, point.row + k), point.gValue + 2.5f + (float)k * 0.75f, PathMoveMode.Jump);
					}
					else
					{
						k += capabilities.jumpHeight;
					}
				}
			}
		}
		else
		{
			bool canDriftSideWays = false;
			PathPoint p = point;
			for (int l = 0; l < 2; l++)
			{
				if (p.previous == null || Map.IsBlockSolid(p.previous.collumn, p.previous.row - 1) || p.previous.moveMode == PathMoveMode.Jump)
				{
					canDriftSideWays = true;
					break;
				}
				p = p.previous;
				if (p == null)
				{
					break;
				}
			}
			if (canDriftSideWays)
			{
				nextPoint = new GridPoint(point.collumn - 1, point.row - 1);
				if (!Map.IsBlockSolid(point.collumn - 1, point.row) && !Map.IsBlockSolid(point.collumn - 1, point.row - 1) && !PathfindingController.IsInSet(nextPoint, PathfindingController.openSet))
				{
					yield return new PathPoint(new GridPoint(point.collumn - 1, point.row - 1), point.gValue + 2.5f);
				}
				nextPoint = new GridPoint(point.collumn + 1, point.row - 1);
				if (!Map.IsBlockSolid(point.collumn + 1, point.row) && !Map.IsBlockSolid(point.collumn + 1, point.row - 1) && !PathfindingController.IsInSet(nextPoint, PathfindingController.openSet))
				{
					yield return new PathPoint(new GridPoint(point.collumn + 1, point.row - 1), point.gValue + 2.5f);
				}
			}
		}
		if (capabilities.canUseLadders && Map.IsBlockLadder(point.collumn, point.row))
		{
			nextPoint = new GridPoint(point.collumn, point.row + 1);
			if (!PathfindingController.IsInSet(nextPoint, PathfindingController.closedSet))
			{
				yield return new PathPoint(nextPoint, point.gValue + 0.8f);
			}
		}
		if (capabilities.canJump && capabilities.canWallClimb && point.collumn > 0 && point.collumn < Map.Width - 1 && !Map.IsBlockLadder(point.collumn, point.row) && (point.moveMode == PathMoveMode.Jump || point.moveMode == PathMoveMode.WallClimbRight || point.moveMode == PathMoveMode.WallClimbLeft || !Map.IsBlockSolid(point.collumn, point.row - 1)))
		{
			if (Map.IsBlockSolid(point.collumn - 1, point.row) && Map.IsBlockSolid(point.collumn - 1, point.row + 1) && !Map.IsBlockSolid(point.collumn, point.row + 1))
			{
				yield return new PathPoint(new GridPoint(point.collumn, point.row + 1), point.gValue + 2f, PathMoveMode.WallClimbLeft);
			}
			else if (Map.IsBlockSolid(point.collumn - 1, point.row) && !Map.IsBlockSolid(point.collumn - 1, point.row + 1) && !Map.IsBlockSolid(point.collumn - 1, point.row + 1))
			{
				yield return new PathPoint(new GridPoint(point.collumn - 1, point.row + 1), point.gValue + 2f, PathMoveMode.WallClimbLeft);
			}
			if (Map.IsBlockSolid(point.collumn + 1, point.row) && Map.IsBlockSolid(point.collumn + 1, point.row + 1) && !Map.IsBlockSolid(point.collumn, point.row + 1))
			{
				yield return new PathPoint(new GridPoint(point.collumn, point.row + 1), point.gValue + 2f, PathMoveMode.WallClimbRight);
			}
			else if (Map.IsBlockSolid(point.collumn + 1, point.row) && !Map.IsBlockSolid(point.collumn + 1, point.row + 1) && !Map.IsBlockSolid(point.collumn + 1, point.row + 1))
			{
				yield return new PathPoint(new GridPoint(point.collumn + 1, point.row + 1), point.gValue + 2f, PathMoveMode.WallClimbRight);
			}
		}
		nextPoint = new GridPoint(point.collumn, point.row - 1);
		if (capabilities.canUseLadders && Map.IsBlockLadder(nextPoint.collumn, nextPoint.row) && !PathfindingController.IsInSet(nextPoint, PathfindingController.closedSet))
		{
			yield return new PathPoint(nextPoint, point.gValue + 1f);
		}
		if (!Map.IsBlockSolid(point.point.collumn, point.point.row - 1) && !Map.IsBlockLadder(point.collumn, point.row - 1))
		{
			int dropDist = 1;
			while (!Map.IsBlockSolid(point.collumn, point.row - dropDist) && point.row - dropDist > 0)
			{
				dropDist++;
			}
			nextPoint = new GridPoint(point.collumn, point.row - 1);
			if (dropDist < capabilities.maxDropDistance && !PathfindingController.IsInSet(nextPoint, PathfindingController.closedSet))
			{
				yield return new PathPoint(new GridPoint(point.point.collumn, point.point.row - 1), point.gValue + 1f);
			}
		}
		yield break;
	}

	private static bool IsInSet(PathPoint point, List<PathPoint> set)
	{
		for (int i = 0; i < set.Count; i++)
		{
			if (set[i].collumn == point.collumn && set[i].row == point.row && set[i].moveMode == point.moveMode)
			{
				return true;
			}
		}
		return false;
	}

	private static bool IsInSet(GridPoint point, List<PathPoint> set)
	{
		for (int i = 0; i < set.Count; i++)
		{
			if (set[i].collumn == point.collumn && set[i].row == point.row)
			{
				return true;
			}
		}
		return false;
	}

	private static int CalcFScore(GridPoint start, GridPoint end)
	{
		return Mathf.Abs(start.row - end.row) + Mathf.Abs(start.collumn - end.collumn);
	}

	protected static List<PathPoint> openSet = new List<PathPoint>();

	protected static List<PathPoint> closedSet = new List<PathPoint>();
}
