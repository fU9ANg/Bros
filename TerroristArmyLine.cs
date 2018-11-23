// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class TerroristArmyLine : MonoBehaviour
{
	public void DrawLine(WorldTerritory3D startTerritory, WorldTerritory3D targetTerritory)
	{
		this.territory = targetTerritory;
		this.DrawLine(startTerritory.GetCentreWorldLocal(), targetTerritory.GetCentreWorldLocal());
	}

	public void DrawLine(Vector3 startPoint, Vector3 targetVector3)
	{
		startPoint = startPoint.normalized;
		targetVector3 = targetVector3.normalized;
		this.points = new List<Vector3>();
		this.points.Add(startPoint);
		Vector3 vector = startPoint;
		int num = 0;
		while ((Mathf.Abs(vector.x - targetVector3.x) > 0.0005f || Mathf.Abs(vector.y - targetVector3.y) > 0.0005f || Mathf.Abs(vector.z - targetVector3.z) > 0.0005f) && num < 120)
		{
			if (num > 0)
			{
				this.points.Add(vector);
			}
			num++;
			vector = Vector3.RotateTowards(vector, targetVector3, 0.02f, 0.01f);
		}
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"DRAW Terror Line Points ",
			this.points.Count,
			" start Point ",
			startPoint,
			" endPoint ",
			targetVector3,
			"  attempts ",
			num
		}));
		if (this.points.Count > 0)
		{
			for (int i = 0; i < this.points.Count; i++)
			{
				float num2 = 1f - Mathf.Abs((float)i - (float)this.points.Count / 2f) / (float)this.points.Count * 2f;
				num2 = Mathf.Sqrt(Mathf.Sqrt(num2));
				List<Vector3> list2;
				List<Vector3> list = list2 = this.points;
				int index2;
				int index = index2 = i;
				Vector3 a = list2[index2];
				list[index] = a * (1f + this.bulgeDistanceM * num2);
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Bulge i : ",
					i,
					" m ",
					this.bulgeDistanceM * num2
				}));
			}
			UnityEngine.Debug.Log("Terror Line Points " + this.points.Count);
			this.pointsCount = 2;
			this.startPointIndex = 0;
			this.endPointIndex = this.pointsCount;
			this.DrawLinePoints();
		}
		else
		{
			UnityEngine.Debug.LogError("No Points in Line!");
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	protected void DrawLinePoints()
	{
		int num = this.endPointIndex - this.startPointIndex;
		this.line.SetVertexCount(num + 1);
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Draw Terror Line.. startPointIndex  ",
			this.startPointIndex,
			" end ",
			this.endPointIndex,
			"  points count ",
			this.points.Count
		}));
		for (int i = 0; i <= num; i++)
		{
			this.line.SetPosition(i, this.points[this.startPointIndex + i] * this.transportHeight);
		}
	}

	private void Update()
	{
		this.growCounter += Time.deltaTime;
		if (this.growCounter > 0.0222f)
		{
			this.growCounter = 0f;
			this.pointsCount++;
			if (this.pointsCount < this.points.Count)
			{
				this.startPointIndex = 0;
				this.endPointIndex = this.pointsCount;
			}
			else
			{
				if (!this.addedTerrorToTerritory)
				{
					this.addedTerrorToTerritory = true;
					this.territory.AddTerrorVisual(1);
				}
				this.startPointIndex = this.pointsCount - this.points.Count;
				this.endPointIndex = this.points.Count - 1;
			}
			if (this.startPointIndex > this.points.Count - 2)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				this.DrawLinePoints();
			}
		}
	}

	public LineRenderer line;

	public float transportHeight = 5.1f;

	protected int pointsCount = 2;

	protected float growCounter;

	protected int startPointIndex;

	protected int endPointIndex;

	public float bulgeDistanceM = 0.3f;

	public WorldTerritory3D territory;

	protected bool addedTerrorToTerritory;

	private List<Vector3> points = new List<Vector3>();
}
