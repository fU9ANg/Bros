// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldLine3d : MonoBehaviour
{
	private void Start()
	{
	}

	public void DrawLine(Vector3 startPoint, Vector3 targetVector3)
	{
		startPoint = startPoint.normalized;
		targetVector3 = targetVector3.normalized;
		this.points = new List<Vector3>();
		this.points.Add(startPoint);
		Vector3 vector = startPoint;
		int num = 10;
		while ((Mathf.Abs(vector.x - targetVector3.x) > 0.0005f || Mathf.Abs(vector.y - targetVector3.y) > 0.0005f || Mathf.Abs(vector.z - targetVector3.z) > 0.0005f) && num < 120)
		{
			num++;
			vector = Vector3.RotateTowards(vector, targetVector3, 0.02f, 0.01f);
			this.points.Add(vector);
		}
		UnityEngine.Debug.Log("Transport Line Points " + this.points.Count);
		this.pointsCount = 2;
		this.line.SetVertexCount(this.pointsCount);
		for (int i = 0; i < this.pointsCount; i++)
		{
			this.line.SetPosition(i, this.points[i].normalized * this.transportHeight);
		}
	}

	private void Update()
	{
		if (this.pointsCount < this.points.Count)
		{
			this.growCounter += Time.deltaTime;
			if (this.growCounter > 0.0222f)
			{
				this.growCounter = 0f;
				this.pointsCount++;
				this.line.SetVertexCount(this.pointsCount);
				for (int i = 0; i < this.pointsCount; i++)
				{
					this.line.SetPosition(i, this.points[i].normalized * this.transportHeight);
				}
			}
		}
	}

	public LineRenderer line;

	public float transportHeight = 5.1f;

	protected int pointsCount = 2;

	protected float growCounter;

	private List<Vector3> points = new List<Vector3>();
}
