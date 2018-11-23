// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MeshTriangle
{
	public MeshTriangle(int v1, int v2, int v3)
	{
		this.vertice1Index = v1;
		this.vertice2Index = v2;
		this.vertice3Index = v3;
	}

	public MeshTriangle(int v1, int v2, int v3, Vector2 uv1, Vector2 uv2, Vector2 uv3)
	{
		this.vertice1Index = v1;
		this.vertice2Index = v2;
		this.vertice3Index = v3;
		this.originalUV1 = uv1;
		this.originalUV2 = uv2;
		this.originalUV3 = uv3;
	}

	public MeshTriangle(int v1, int v2, int v3, Vector3 v1Pos, Vector3 v2Pos, Vector3 v3Pos, Vector3 n1, Vector3 n2, Vector3 n3, Vector2 uv1, Vector2 uv2, Vector2 uv3)
	{
		this.vertice1Index = v1;
		this.vertice2Index = v2;
		this.vertice3Index = v3;
		this.vertice1Pos = v1Pos;
		this.vertice2Pos = v2Pos;
		this.vertice3Pos = v3Pos;
		this.originalUV1 = uv1;
		this.originalUV2 = uv2;
		this.originalUV3 = uv3;
		this.normal1 = n1;
		this.normal2 = n2;
		this.normal3 = n3;
	}

	public bool ContainsVertices(int v1, int v2, int v3)
	{
		return (this.vertice1Index == v1 || this.vertice1Index == v2 || this.vertice1Index == v3) && (this.vertice2Index == v1 || this.vertice2Index == v2 || this.vertice2Index == v3) && (this.vertice3Index == v1 || this.vertice3Index == v2 || this.vertice3Index == v3);
	}

	public int vertice1Index = -1;

	public int vertice2Index = -1;

	public int vertice3Index = -1;

	public Vector3 vertice1Pos;

	public Vector3 vertice2Pos;

	public Vector3 vertice3Pos;

	public Vector3 normal1;

	public Vector3 normal2;

	public Vector3 normal3;

	public int neighbour1 = -1;

	public int neighbour2 = -1;

	public int neighbour3 = -1;

	public List<int> distantNeighbours1 = new List<int>();

	public List<int> distantNeighbours2 = new List<int>();

	public List<int> distantNeighbours3 = new List<int>();

	public WorldMapTerrainType terrainType;

	public Vector2 originalUV1;

	public Vector2 originalUV2;

	public Vector2 originalUV3;

	public int territoryIndex = -1;

	public bool isKnown = true;
}
