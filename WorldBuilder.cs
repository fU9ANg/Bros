// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
	private void Awake()
	{
		WorldBuilder.instance = this;
	}

	private void Start()
	{
		UnityEngine.Debug.Log("Start ");
		this.currentSelectionIndex = -1;
		base.gameObject.layer = 0;
		if (this.vertices == null || this.meshTriangles == null || this.meshTriangles.Count < 10)
		{
			UnityEngine.Debug.Log("Rebuild ");
			this.RebuildMeshTriangles();
		}
		else
		{
			this.RebuildMesh();
		}
	}

	public WorldTerritory3D GetTerritory(int territoryIndex)
	{
		if (territoryIndex >= 0 && territoryIndex < this.territories.Count)
		{
			return this.territories[territoryIndex];
		}
		return null;
	}

	public void Clear()
	{
		this.meshTriangles.Clear();
		this.worldTrianglesHolder.meshTriangles = new MeshTriangle[0];
		this.RebuildMeshTriangles();
	}

	public void RebuildMeshTriangles()
	{
		this.meshFilter = base.GetComponent<MeshFilter>();
		this.triangles = this.sourceMesh.triangles;
		this.vertices = this.sourceMesh.vertices;
		this.uv = this.sourceMesh.uv;
		this.normals = this.sourceMesh.normals;
		Mesh mesh = new Mesh();
		mesh.vertices = this.vertices;
		mesh.uv = this.uv;
		mesh.normals = this.normals;
		mesh.triangles = this.triangles;
		mesh.name = "World Mesh";
		this.mesh = mesh;
		this.meshFilter.sharedMesh = mesh;
		this.triangles = this.sourceMesh.triangles;
		if (this.triangles != null && this.triangles.Length > 10)
		{
			if (this.worldTrianglesHolder.meshTriangles == null || this.worldTrianglesHolder.meshTriangles.Length < 10)
			{
				for (int i = 0; i < this.triangles.Length / 3; i++)
				{
					this.meshTriangles.Add(new MeshTriangle(this.triangles[i * 3], this.triangles[i * 3 + 1], this.triangles[i * 3 + 2], this.vertices[this.triangles[i * 3]], this.vertices[this.triangles[i * 3 + 1]], this.vertices[this.triangles[i * 3 + 2]], this.normals[this.triangles[i * 3]], this.normals[this.triangles[i * 3 + 1]], this.normals[this.triangles[i * 3 + 2]], this.uv[this.triangles[i * 3]], this.uv[this.triangles[i * 3 + 1]], this.uv[this.triangles[i * 3 + 2]]));
				}
			}
			else
			{
				UnityEngine.Debug.Log("Get mesh triangles from meshTriangle holder ");
				this.meshTriangles.Clear();
				for (int j = 0; j < this.worldTrianglesHolder.meshTriangles.Length; j++)
				{
					this.meshTriangles.Add(this.worldTrianglesHolder.meshTriangles[j]);
				}
			}
			for (int k = 0; k < this.meshTriangles.Count; k++)
			{
				this.FindNeighbours(this.meshTriangles[k]);
				this.FindDistantNeighbours(this.meshTriangles[k]);
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int l = 0; l < this.meshTriangles.Count; l++)
			{
				if (this.meshTriangles[l].neighbour1 >= 0)
				{
					num++;
				}
				if (this.meshTriangles[l].neighbour2 >= 0)
				{
					num2++;
				}
				if (this.meshTriangles[l].neighbour3 >= 0)
				{
					num3++;
				}
			}
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"NEighbours 1 ",
				num,
				" neighbours 2 ",
				num2,
				" neighbours 3 ",
				num3
			}));
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"SubmeshCount ",
				this.mesh.subMeshCount,
				" meshTriangles ",
				this.meshTriangles.Count
			}));
			this.RebuildMesh();
		}
		else
		{
			UnityEngine.Debug.LogError("Triangles is unassigned");
		}
	}

	public Mesh CreateCollisionMesh(int territoryIndex)
	{
		Mesh mesh = new Mesh();
		mesh.name = "CollisionMesh " + territoryIndex;
		List<int> list = new List<int>();
		List<Vector3> list2 = new List<Vector3>();
		List<Vector3> list3 = new List<Vector3>();
		List<Vector2> list4 = new List<Vector2>();
		for (int i = 0; i < this.meshTriangles.Count; i++)
		{
			if (this.meshTriangles[i].territoryIndex == territoryIndex)
			{
				this.AddTriangleToLists(this.meshTriangles[i], ref list, ref list2, ref list3, ref list4, true, false, false, false, false, false);
			}
		}
		mesh.subMeshCount = 1;
		mesh.vertices = list2.ToArray();
		mesh.uv = list4.ToArray();
		mesh.normals = list3.ToArray();
		mesh.SetTriangles(list.ToArray(), 0);
		return mesh;
	}

	public void RebuildMesh()
	{
		UnityEngine.Debug.Log("REBUILD MESH ! " + this.meshTriangles.Count);
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		List<Vector2> list3 = new List<Vector2>();
		List<int> list4 = new List<int>();
		List<int> list5 = new List<int>();
		List<int> list6 = new List<int>();
		List<int> list7 = new List<int>();
		List<int> list8 = new List<int>();
		List<int> list9 = new List<int>();
		List<int> list10 = new List<int>();
		List<int> list11 = new List<int>();
		List<int> list12 = new List<int>();
		List<int> list13 = new List<int>();
		List<int> list14 = new List<int>();
		List<int> list15 = new List<int>();
		List<int> list16 = new List<int>();
		for (int i = 0; i < this.meshTriangles.Count; i++)
		{
			if (this.meshTriangles[i].territoryIndex != -1 && this.meshTriangles[i].terrainType != WorldMapTerrainType.Ocean)
			{
				int territoryIndex = this.meshTriangles[i].territoryIndex;
				this.AddTriangleToLists(this.meshTriangles[i], ref list15, ref list, ref list2, ref list3, this.meshTriangles[this.meshTriangles[i].neighbour1].territoryIndex != territoryIndex && this.meshTriangles[this.meshTriangles[i].neighbour1].terrainType == WorldMapTerrainType.Ocean, this.meshTriangles[this.meshTriangles[i].neighbour2].territoryIndex != territoryIndex && this.meshTriangles[this.meshTriangles[i].neighbour2].terrainType == WorldMapTerrainType.Ocean, this.meshTriangles[this.meshTriangles[i].neighbour3].territoryIndex != territoryIndex && this.meshTriangles[this.meshTriangles[i].neighbour3].terrainType == WorldMapTerrainType.Ocean, this.ContainsOtherTerritory(this.meshTriangles[i], this.meshTriangles[i].distantNeighbours1) && this.ContainsOcean(this.meshTriangles[i].distantNeighbours1), this.ContainsOtherTerritory(this.meshTriangles[i], this.meshTriangles[i].distantNeighbours2) && this.ContainsOcean(this.meshTriangles[i].distantNeighbours2), this.ContainsOtherTerritory(this.meshTriangles[i], this.meshTriangles[i].distantNeighbours3) && this.ContainsOcean(this.meshTriangles[i].distantNeighbours3));
				this.AddTriangleToLists(this.meshTriangles[i], ref list16, ref list, ref list2, ref list3, this.meshTriangles[this.meshTriangles[i].neighbour1].territoryIndex != territoryIndex && this.meshTriangles[this.meshTriangles[i].neighbour1].terrainType != WorldMapTerrainType.Ocean, this.meshTriangles[this.meshTriangles[i].neighbour2].territoryIndex != territoryIndex && this.meshTriangles[this.meshTriangles[i].neighbour2].terrainType != WorldMapTerrainType.Ocean, this.meshTriangles[this.meshTriangles[i].neighbour3].territoryIndex != territoryIndex && this.meshTriangles[this.meshTriangles[i].neighbour3].terrainType != WorldMapTerrainType.Ocean, this.ContainsOtherTerritory(this.meshTriangles[i], this.meshTriangles[i].distantNeighbours1) && !this.ContainsOcean(this.meshTriangles[i].distantNeighbours1), this.ContainsOtherTerritory(this.meshTriangles[i], this.meshTriangles[i].distantNeighbours2) && !this.ContainsOcean(this.meshTriangles[i].distantNeighbours2), this.ContainsOtherTerritory(this.meshTriangles[i], this.meshTriangles[i].distantNeighbours3) && !this.ContainsOcean(this.meshTriangles[i].distantNeighbours3));
			}
			if (!this.meshTriangles[i].isKnown && this.meshTriangles[i].terrainType != WorldMapTerrainType.Ocean)
			{
				this.AddTriangleToLists(this.meshTriangles[i], ref list11, ref list, ref list2, ref list3, true, false, false, false, false, false);
				this.AddShores(this.meshTriangles[i], ref list12, ref list, ref list2, ref list3, this.meshTriangles[this.meshTriangles[i].neighbour1].terrainType == WorldMapTerrainType.Ocean || this.meshTriangles[this.meshTriangles[i].neighbour1].isKnown, this.meshTriangles[this.meshTriangles[i].neighbour2].terrainType == WorldMapTerrainType.Ocean || this.meshTriangles[this.meshTriangles[i].neighbour2].isKnown, this.meshTriangles[this.meshTriangles[i].neighbour3].terrainType == WorldMapTerrainType.Ocean || this.meshTriangles[this.meshTriangles[i].neighbour3].isKnown, this.ContainsOcean(this.meshTriangles[i].distantNeighbours1) || this.ContainsKnown(this.meshTriangles[i].distantNeighbours1), this.ContainsOcean(this.meshTriangles[i].distantNeighbours2) || this.ContainsKnown(this.meshTriangles[i].distantNeighbours2), this.ContainsOcean(this.meshTriangles[i].distantNeighbours3) || this.ContainsKnown(this.meshTriangles[i].distantNeighbours3));
			}
			else if (this.meshTriangles[i].terrainType == WorldMapTerrainType.Ice)
			{
				this.AddTriangleToLists(this.meshTriangles[i], ref list13, ref list, ref list2, ref list3, true, false, false, false, false, false);
				this.AddShores(this.meshTriangles[i], ref list14, ref list, ref list2, ref list3, this.meshTriangles[this.meshTriangles[i].neighbour1].terrainType == WorldMapTerrainType.Ocean, this.meshTriangles[this.meshTriangles[i].neighbour2].terrainType == WorldMapTerrainType.Ocean, this.meshTriangles[this.meshTriangles[i].neighbour3].terrainType == WorldMapTerrainType.Ocean, this.ContainsOcean(this.meshTriangles[i].distantNeighbours1), this.ContainsOcean(this.meshTriangles[i].distantNeighbours2), this.ContainsOcean(this.meshTriangles[i].distantNeighbours3));
			}
			else if (this.meshTriangles[i].terrainType == WorldMapTerrainType.Jungle)
			{
				this.AddTriangleToLists(this.meshTriangles[i], ref list6, ref list, ref list2, ref list3, true, false, false, false, false, false);
				this.AddShores(this.meshTriangles[i], ref list7, ref list, ref list2, ref list3, this.meshTriangles[this.meshTriangles[i].neighbour1].terrainType == WorldMapTerrainType.Ocean, this.meshTriangles[this.meshTriangles[i].neighbour2].terrainType == WorldMapTerrainType.Ocean, this.meshTriangles[this.meshTriangles[i].neighbour3].terrainType == WorldMapTerrainType.Ocean, this.ContainsOcean(this.meshTriangles[i].distantNeighbours1), this.ContainsOcean(this.meshTriangles[i].distantNeighbours2), this.ContainsOcean(this.meshTriangles[i].distantNeighbours3));
			}
			else
			{
				this.uv[this.meshTriangles[i].vertice1Index] = this.meshTriangles[i].originalUV1;
				this.uv[this.meshTriangles[i].vertice2Index] = this.meshTriangles[i].originalUV2;
				this.uv[this.meshTriangles[i].vertice3Index] = this.meshTriangles[i].originalUV3;
				list.Add(this.meshTriangles[i].vertice1Pos);
				list2.Add(this.meshTriangles[i].normal1);
				list4.Add(list.Count - 1);
				list3.Add(new Vector2(0.5f, 1f));
				list.Add(this.meshTriangles[i].vertice2Pos);
				list2.Add(this.meshTriangles[i].normal2);
				list4.Add(list.Count - 1);
				list3.Add(new Vector2(1f, 0f));
				list.Add(this.meshTriangles[i].vertice3Pos);
				list2.Add(this.meshTriangles[i].normal3);
				list4.Add(list.Count - 1);
				list3.Add(new Vector2(0f, 0f));
				if (this.IsSurroundedByGround(this.meshTriangles[i]))
				{
					this.AddTriangleToLists(this.meshTriangles[i], ref list9, ref list, ref list2, ref list3, true, true, true, false, false, false);
				}
				else if (this.IsSurroundedByOcean(this.meshTriangles[i]))
				{
					if (this.ContainsGround(this.meshTriangles[i].distantNeighbours3))
					{
						this.AddTriangleToLists(this.meshTriangles[i], ref list9, ref list, ref list2, ref list3, false, false, false, false, false, true);
					}
					if (this.ContainsGround(this.meshTriangles[i].distantNeighbours1))
					{
						this.AddTriangleToLists(this.meshTriangles[i], ref list9, ref list, ref list2, ref list3, false, false, false, true, false, false);
					}
					if (this.ContainsGround(this.meshTriangles[i].distantNeighbours2))
					{
						this.AddTriangleToLists(this.meshTriangles[i], ref list9, ref list, ref list2, ref list3, false, false, false, false, true, false);
					}
				}
				else if (this.HasGroundOnOneSide(this.meshTriangles[i]))
				{
					if (this.meshTriangles[this.meshTriangles[i].neighbour1].terrainType != WorldMapTerrainType.Ocean)
					{
						this.AddTriangleToLists(this.meshTriangles[i], ref list9, ref list, ref list2, ref list3, true, false, false, false, false, this.ContainsGround(this.meshTriangles[i].distantNeighbours3));
					}
					else if (this.meshTriangles[this.meshTriangles[i].neighbour3].terrainType != WorldMapTerrainType.Ocean)
					{
						this.AddTriangleToLists(this.meshTriangles[i], ref list9, ref list, ref list2, ref list3, false, false, true, false, this.ContainsGround(this.meshTriangles[i].distantNeighbours2), false);
					}
					else if (this.meshTriangles[this.meshTriangles[i].neighbour2].terrainType != WorldMapTerrainType.Ocean)
					{
						this.AddTriangleToLists(this.meshTriangles[i], ref list9, ref list, ref list2, ref list3, false, true, false, this.ContainsGround(this.meshTriangles[i].distantNeighbours1), false, false);
					}
				}
				else if (this.HasGroundOnTwoSides(this.meshTriangles[i]))
				{
					if (this.meshTriangles[this.meshTriangles[i].neighbour3].terrainType == WorldMapTerrainType.Ocean)
					{
						this.AddTriangleToLists(this.meshTriangles[i], ref list9, ref list, ref list2, ref list3, true, true, false, false, false, false);
					}
					else if (this.meshTriangles[this.meshTriangles[i].neighbour1].terrainType == WorldMapTerrainType.Ocean)
					{
						this.AddTriangleToLists(this.meshTriangles[i], ref list9, ref list, ref list2, ref list3, false, true, true, false, false, false);
					}
					else if (this.meshTriangles[this.meshTriangles[i].neighbour2].terrainType == WorldMapTerrainType.Ocean)
					{
						this.AddTriangleToLists(this.meshTriangles[i], ref list9, ref list, ref list2, ref list3, true, false, true, false, false, false);
					}
				}
			}
			if (this.currentSelectionIndex >= 0 && this.meshTriangles[i].territoryIndex == this.currentSelectionIndex)
			{
				if (this.IsSurroundedByOtherTerritory(this.meshTriangles[i]))
				{
					this.AddTriangleToLists(this.meshTriangles[i], ref list10, ref list, ref list2, ref list3, true, true, true, false, false, false);
				}
				else if (!this.IsSurroundedBySameTerritory(this.meshTriangles[i]))
				{
					if (this.HasOtherTerritoryOnOneSide(this.meshTriangles[i]))
					{
						if (this.meshTriangles[this.meshTriangles[i].neighbour1].territoryIndex != this.meshTriangles[i].territoryIndex)
						{
							this.AddTriangleToLists(this.meshTriangles[i], ref list10, ref list, ref list2, ref list3, true, false, false, false, false, false);
						}
						else if (this.meshTriangles[this.meshTriangles[i].neighbour3].territoryIndex != this.meshTriangles[i].territoryIndex)
						{
							this.AddTriangleToLists(this.meshTriangles[i], ref list10, ref list, ref list2, ref list3, false, false, true, false, false, false);
						}
						else if (this.meshTriangles[this.meshTriangles[i].neighbour2].territoryIndex != this.meshTriangles[i].territoryIndex)
						{
							this.AddTriangleToLists(this.meshTriangles[i], ref list10, ref list, ref list2, ref list3, false, true, false, false, false, false);
						}
					}
					else if (this.HasOtherTerritoryOnTwoSides(this.meshTriangles[i]))
					{
						if (this.meshTriangles[this.meshTriangles[i].neighbour3].territoryIndex == this.meshTriangles[i].territoryIndex)
						{
							this.AddTriangleToLists(this.meshTriangles[i], ref list10, ref list, ref list2, ref list3, true, true, false, false, false, false);
						}
						else if (this.meshTriangles[this.meshTriangles[i].neighbour1].territoryIndex == this.meshTriangles[i].territoryIndex)
						{
							this.AddTriangleToLists(this.meshTriangles[i], ref list10, ref list, ref list2, ref list3, false, true, true, false, false, false);
						}
						else if (this.meshTriangles[this.meshTriangles[i].neighbour2].territoryIndex == this.meshTriangles[i].territoryIndex)
						{
							this.AddTriangleToLists(this.meshTriangles[i], ref list10, ref list, ref list2, ref list3, true, false, true, false, false, false);
						}
					}
				}
			}
		}
		this.mesh.subMeshCount = 1;
		this.mesh.SetTriangles(new int[0], 0);
		this.mesh.vertices = list.ToArray();
		this.mesh.uv = list3.ToArray();
		this.mesh.normals = list2.ToArray();
		this.mesh.subMeshCount = 12;
		this.mesh.SetTriangles(list4.ToArray(), 0);
		this.mesh.SetTriangles(list6.ToArray(), 1);
		this.mesh.SetTriangles(list11.ToArray(), 2);
		this.mesh.SetTriangles(list12.ToArray(), 3);
		this.mesh.SetTriangles(list7.ToArray(), 4);
		this.mesh.SetTriangles(list8.ToArray(), 5);
		this.mesh.SetTriangles(list9.ToArray(), 6);
		this.mesh.SetTriangles(list10.ToArray(), 7);
		this.mesh.SetTriangles(list13.ToArray(), 8);
		this.mesh.SetTriangles(list14.ToArray(), 9);
		this.mesh.SetTriangles(list15.ToArray(), 10);
		this.mesh.SetTriangles(list16.ToArray(), 11);
	}

	private void AddShores(MeshTriangle meshTriangle, ref List<int> triListJungleShoreOneSideBottom, ref List<Vector3> verticeList, ref List<Vector3> normalList, ref List<Vector2> uvList, bool neighbour1IsOther, bool neighbour2IsOther, bool neighbour3IsOther, bool distantOtherNeighbour1, bool distantOtherNeighbour2, bool distantOtherNeighbour3)
	{
		if (neighbour1IsOther && neighbour2IsOther && neighbour3IsOther)
		{
			this.AddTriangleToLists(meshTriangle, ref triListJungleShoreOneSideBottom, ref verticeList, ref normalList, ref uvList, true, true, true, false, false, false);
		}
		else if (!neighbour1IsOther && !neighbour2IsOther && !neighbour3IsOther)
		{
			if (distantOtherNeighbour3)
			{
				this.AddTriangleToLists(meshTriangle, ref triListJungleShoreOneSideBottom, ref verticeList, ref normalList, ref uvList, false, false, false, false, false, true);
			}
			if (distantOtherNeighbour1)
			{
				this.AddTriangleToLists(meshTriangle, ref triListJungleShoreOneSideBottom, ref verticeList, ref normalList, ref uvList, false, false, false, true, false, false);
			}
			if (distantOtherNeighbour2)
			{
				this.AddTriangleToLists(meshTriangle, ref triListJungleShoreOneSideBottom, ref verticeList, ref normalList, ref uvList, false, false, false, false, true, false);
			}
		}
		else if (((!neighbour1IsOther) ? 0 : 1) + ((!neighbour2IsOther) ? 0 : 1) + ((!neighbour3IsOther) ? 0 : 1) == 1)
		{
			if (neighbour1IsOther)
			{
				this.AddTriangleToLists(meshTriangle, ref triListJungleShoreOneSideBottom, ref verticeList, ref normalList, ref uvList, true, false, false, false, false, distantOtherNeighbour3);
			}
			else if (neighbour3IsOther)
			{
				this.AddTriangleToLists(meshTriangle, ref triListJungleShoreOneSideBottom, ref verticeList, ref normalList, ref uvList, false, false, true, false, distantOtherNeighbour2, false);
			}
			else if (neighbour2IsOther)
			{
				this.AddTriangleToLists(meshTriangle, ref triListJungleShoreOneSideBottom, ref verticeList, ref normalList, ref uvList, false, true, false, distantOtherNeighbour1, false, false);
			}
		}
		else if (((!neighbour1IsOther) ? 0 : 1) + ((!neighbour2IsOther) ? 0 : 1) + ((!neighbour3IsOther) ? 0 : 1) == 2)
		{
			if (!neighbour3IsOther)
			{
				this.AddTriangleToLists(meshTriangle, ref triListJungleShoreOneSideBottom, ref verticeList, ref normalList, ref uvList, true, true, false, false, false, false);
			}
			else if (!neighbour1IsOther)
			{
				this.AddTriangleToLists(meshTriangle, ref triListJungleShoreOneSideBottom, ref verticeList, ref normalList, ref uvList, false, true, true, false, false, false);
			}
			else if (!neighbour2IsOther)
			{
				this.AddTriangleToLists(meshTriangle, ref triListJungleShoreOneSideBottom, ref verticeList, ref normalList, ref uvList, true, false, true, false, false, false);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("No Material Option ");
		}
	}

	private void AddTriangleToLists(MeshTriangle meshTriangle, ref List<int> triListJungleShoreOneSideBottom, ref List<Vector3> verticeList, ref List<Vector3> normalList, ref List<Vector2> uvList, bool side1, bool side2, bool side3, bool tip1, bool tip2, bool tip3)
	{
		if (side1 && side2 && side3)
		{
			Vector3 a = (meshTriangle.vertice1Pos + meshTriangle.vertice2Pos + meshTriangle.vertice3Pos) / 3f;
			Vector2 item = new Vector2(0.5f, 0.5f);
			verticeList.Add(meshTriangle.vertice1Pos * 1.0001f);
			normalList.Add(meshTriangle.normal1);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(1f, 0f));
			verticeList.Add(meshTriangle.vertice2Pos * 1.0001f);
			normalList.Add(meshTriangle.normal2);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0f, 0f));
			verticeList.Add(a * 1.0001f);
			normalList.Add(meshTriangle.normal3);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(item);
			verticeList.Add(a * 1.0001f);
			normalList.Add(meshTriangle.normal1);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(item);
			verticeList.Add(meshTriangle.vertice2Pos * 1.0001f);
			normalList.Add(meshTriangle.normal2);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(1f, 0f));
			verticeList.Add(meshTriangle.vertice3Pos * 1.0001f);
			normalList.Add(meshTriangle.normal3);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0f, 0f));
			verticeList.Add(meshTriangle.vertice1Pos * 1.0001f);
			normalList.Add(meshTriangle.normal1);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0f, 0f));
			verticeList.Add(a * 1.0001f);
			normalList.Add(meshTriangle.normal2);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(item);
			verticeList.Add(meshTriangle.vertice3Pos * 1.0001f);
			normalList.Add(meshTriangle.normal3);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(1f, 0f));
		}
		else if (side1 && side2)
		{
			verticeList.Add(meshTriangle.vertice1Pos * 1.0001f);
			normalList.Add(meshTriangle.normal1);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(1f, 0f));
			verticeList.Add(meshTriangle.vertice2Pos * 1.0001f);
			normalList.Add(meshTriangle.normal2);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0f, 0f));
			verticeList.Add((meshTriangle.vertice3Pos + meshTriangle.vertice1Pos) / 2f * 1.0001f);
			normalList.Add(meshTriangle.normal3);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0.75f, 0.5f));
			verticeList.Add((meshTriangle.vertice3Pos + meshTriangle.vertice1Pos) / 2f * 1.0001f);
			normalList.Add(meshTriangle.normal1);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0.25f, 0.5f));
			verticeList.Add(meshTriangle.vertice2Pos * 1.0001f);
			normalList.Add(meshTriangle.normal2);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(1f, 0f));
			verticeList.Add(meshTriangle.vertice3Pos * 1.0001f);
			normalList.Add(meshTriangle.normal3);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0f, 0f));
		}
		else if (side2 && side3)
		{
			verticeList.Add(meshTriangle.vertice1Pos * 1.0001f);
			normalList.Add(meshTriangle.normal1);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0f, 0f));
			verticeList.Add((meshTriangle.vertice2Pos + meshTriangle.vertice1Pos) / 2f * 1.0001f);
			normalList.Add(meshTriangle.normal2);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0.25f, 0.5f));
			verticeList.Add(meshTriangle.vertice3Pos * 1.0001f);
			normalList.Add(meshTriangle.normal3);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(1f, 0f));
			verticeList.Add((meshTriangle.vertice2Pos + meshTriangle.vertice1Pos) / 2f * 1.0001f);
			normalList.Add(meshTriangle.normal1);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0.75f, 0.5f));
			verticeList.Add(meshTriangle.vertice2Pos * 1.0001f);
			normalList.Add(meshTriangle.normal2);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(1f, 0f));
			verticeList.Add(meshTriangle.vertice3Pos * 1.0001f);
			normalList.Add(meshTriangle.normal3);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0f, 0f));
		}
		else if (side3 && side1)
		{
			verticeList.Add(meshTriangle.vertice1Pos * 1.0001f);
			normalList.Add(meshTriangle.normal1);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(1f, 0f));
			verticeList.Add(meshTriangle.vertice2Pos * 1.0001f);
			normalList.Add(meshTriangle.normal2);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0f, 0f));
			verticeList.Add((meshTriangle.vertice3Pos + meshTriangle.vertice2Pos) / 2f * 1.0001f);
			normalList.Add(meshTriangle.normal3);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0.75f, 0.5f));
			verticeList.Add(meshTriangle.vertice1Pos * 1.0001f);
			normalList.Add(meshTriangle.normal1);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0f, 0f));
			verticeList.Add((meshTriangle.vertice3Pos + meshTriangle.vertice2Pos) / 2f * 1.0001f);
			normalList.Add(meshTriangle.normal2);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0.75f, 0.5f));
			verticeList.Add(meshTriangle.vertice3Pos * 1.0001f);
			normalList.Add(meshTriangle.normal3);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(1f, 0f));
		}
		else if (side1 || side2 || side3)
		{
			verticeList.Add(meshTriangle.vertice1Pos);
			normalList.Add(meshTriangle.normal1);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			verticeList.Add(meshTriangle.vertice2Pos);
			normalList.Add(meshTriangle.normal2);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			verticeList.Add(meshTriangle.vertice3Pos);
			normalList.Add(meshTriangle.normal3);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			if (side1)
			{
				uvList.Add(new Vector2(1f, 0f));
				uvList.Add(new Vector2(0f, 0f));
				uvList.Add(new Vector2(0.5f, 1f));
			}
			else if (side2)
			{
				uvList.Add(new Vector2(0.5f, 1f));
				uvList.Add(new Vector2(1f, 0f));
				uvList.Add(new Vector2(0f, 0f));
			}
			else if (side3)
			{
				uvList.Add(new Vector2(0f, 0f));
				uvList.Add(new Vector2(0.5f, 1f));
				uvList.Add(new Vector2(1f, 0f));
			}
		}
		if (tip1)
		{
			verticeList.Add(meshTriangle.vertice1Pos);
			normalList.Add(meshTriangle.normal1);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0.5f, 0f));
			verticeList.Add(meshTriangle.vertice2Pos);
			normalList.Add(meshTriangle.normal2);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0f, 1f));
			verticeList.Add(meshTriangle.vertice3Pos);
			normalList.Add(meshTriangle.normal3);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(1f, 1f));
		}
		if (tip2)
		{
			verticeList.Add(meshTriangle.vertice1Pos);
			normalList.Add(meshTriangle.normal1);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(1f, 1f));
			verticeList.Add(meshTriangle.vertice2Pos);
			normalList.Add(meshTriangle.normal2);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0.5f, 0f));
			verticeList.Add(meshTriangle.vertice3Pos);
			normalList.Add(meshTriangle.normal3);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0f, 1f));
		}
		if (tip3)
		{
			verticeList.Add(meshTriangle.vertice1Pos);
			normalList.Add(meshTriangle.normal1);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0f, 1f));
			verticeList.Add(meshTriangle.vertice2Pos);
			normalList.Add(meshTriangle.normal2);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(1f, 1f));
			verticeList.Add(meshTriangle.vertice3Pos);
			normalList.Add(meshTriangle.normal3);
			triListJungleShoreOneSideBottom.Add(verticeList.Count - 1);
			uvList.Add(new Vector2(0.5f, 0f));
		}
	}

	protected bool IsSurroundedByOtherTerritory(MeshTriangle meshTriangle)
	{
		return this.meshTriangles[meshTriangle.neighbour1].territoryIndex != meshTriangle.territoryIndex && this.meshTriangles[meshTriangle.neighbour2].territoryIndex != meshTriangle.territoryIndex && this.meshTriangles[meshTriangle.neighbour3].territoryIndex != meshTriangle.territoryIndex;
	}

	protected bool IsSurroundedByOcean(MeshTriangle meshTriangle)
	{
		if (meshTriangle.neighbour1 >= this.meshTriangles.Count || meshTriangle.neighbour1 < 0)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Neighbour1 greater ",
				meshTriangle.neighbour1,
				" than ",
				this.meshTriangles.Count
			}));
		}
		if (meshTriangle.neighbour2 >= this.meshTriangles.Count || meshTriangle.neighbour2 < 0)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Neighbour2 greater ",
				meshTriangle.neighbour2,
				" than ",
				this.meshTriangles.Count
			}));
		}
		if (meshTriangle.neighbour3 >= this.meshTriangles.Count || meshTriangle.neighbour3 < 0)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Neighbour3 greater ",
				meshTriangle.neighbour3,
				" than ",
				this.meshTriangles.Count
			}));
		}
		return this.meshTriangles[meshTriangle.neighbour1].terrainType == WorldMapTerrainType.Ocean && this.meshTriangles[meshTriangle.neighbour2].terrainType == WorldMapTerrainType.Ocean && this.meshTriangles[meshTriangle.neighbour3].terrainType == WorldMapTerrainType.Ocean;
	}

	protected bool IsSurroundedByGround(MeshTriangle meshTriangle)
	{
		return this.meshTriangles[meshTriangle.neighbour1].terrainType != WorldMapTerrainType.Ocean && this.meshTriangles[meshTriangle.neighbour2].terrainType != WorldMapTerrainType.Ocean && this.meshTriangles[meshTriangle.neighbour3].terrainType != WorldMapTerrainType.Ocean;
	}

	protected bool IsSurroundedBySameTerritory(MeshTriangle meshTriangle)
	{
		return this.meshTriangles[meshTriangle.neighbour1].territoryIndex == meshTriangle.territoryIndex && this.meshTriangles[meshTriangle.neighbour2].territoryIndex == meshTriangle.territoryIndex && this.meshTriangles[meshTriangle.neighbour3].territoryIndex == meshTriangle.territoryIndex;
	}

	protected bool IsSurroundedByJungle(MeshTriangle meshTriangle)
	{
		return this.meshTriangles[meshTriangle.neighbour1].terrainType == WorldMapTerrainType.Jungle && this.meshTriangles[meshTriangle.neighbour2].terrainType == WorldMapTerrainType.Jungle && this.meshTriangles[meshTriangle.neighbour3].terrainType == WorldMapTerrainType.Jungle;
	}

	protected bool HasOtherTerritoryOnOneSide(MeshTriangle meshTriangle)
	{
		return ((this.meshTriangles[meshTriangle.neighbour1].territoryIndex != meshTriangle.territoryIndex) ? 1 : 0) + ((this.meshTriangles[meshTriangle.neighbour2].territoryIndex != meshTriangle.territoryIndex) ? 1 : 0) + ((this.meshTriangles[meshTriangle.neighbour3].territoryIndex != meshTriangle.territoryIndex) ? 1 : 0) == 1;
	}

	protected bool HasOtherTerritoryOnTwoSides(MeshTriangle meshTriangle)
	{
		return ((this.meshTriangles[meshTriangle.neighbour1].territoryIndex != meshTriangle.territoryIndex) ? 1 : 0) + ((this.meshTriangles[meshTriangle.neighbour2].territoryIndex != meshTriangle.territoryIndex) ? 1 : 0) + ((this.meshTriangles[meshTriangle.neighbour3].territoryIndex != meshTriangle.territoryIndex) ? 1 : 0) == 2;
	}

	protected bool HasOceanOnOneSide(MeshTriangle meshTriangle)
	{
		return ((this.meshTriangles[meshTriangle.neighbour1].terrainType != WorldMapTerrainType.Ocean) ? 0 : 1) + ((this.meshTriangles[meshTriangle.neighbour2].terrainType != WorldMapTerrainType.Ocean) ? 0 : 1) + ((this.meshTriangles[meshTriangle.neighbour3].terrainType != WorldMapTerrainType.Ocean) ? 0 : 1) == 1;
	}

	protected bool HasGroundOnOneSide(MeshTriangle meshTriangle)
	{
		return ((this.meshTriangles[meshTriangle.neighbour1].terrainType != WorldMapTerrainType.Ocean) ? 1 : 0) + ((this.meshTriangles[meshTriangle.neighbour2].terrainType != WorldMapTerrainType.Ocean) ? 1 : 0) + ((this.meshTriangles[meshTriangle.neighbour3].terrainType != WorldMapTerrainType.Ocean) ? 1 : 0) == 1;
	}

	protected bool HasOceanOnTwoSides(MeshTriangle meshTriangle)
	{
		return ((this.meshTriangles[meshTriangle.neighbour1].terrainType != WorldMapTerrainType.Ocean) ? 0 : 1) + ((this.meshTriangles[meshTriangle.neighbour2].terrainType != WorldMapTerrainType.Ocean) ? 0 : 1) + ((this.meshTriangles[meshTriangle.neighbour3].terrainType != WorldMapTerrainType.Ocean) ? 0 : 1) == 2;
	}

	protected bool HasGroundOnTwoSides(MeshTriangle meshTriangle)
	{
		return ((this.meshTriangles[meshTriangle.neighbour1].terrainType != WorldMapTerrainType.Ocean) ? 1 : 0) + ((this.meshTriangles[meshTriangle.neighbour2].terrainType != WorldMapTerrainType.Ocean) ? 1 : 0) + ((this.meshTriangles[meshTriangle.neighbour3].terrainType != WorldMapTerrainType.Ocean) ? 1 : 0) == 2;
	}

	protected int[] CreateTriangleArray(List<MeshTriangle> meshTriangles)
	{
		int[] array = new int[meshTriangles.Count * 3];
		for (int i = 0; i < meshTriangles.Count; i++)
		{
			array[i * 3] = meshTriangles[i].vertice1Index;
			array[i * 3 + 1] = meshTriangles[i].vertice2Index;
			array[i * 3 + 2] = meshTriangles[i].vertice3Index;
		}
		return array;
	}

	protected void FindNeighbours(MeshTriangle meshTriangle)
	{
		for (int i = 0; i < this.meshTriangles.Count; i++)
		{
			if (this.meshTriangles[i] != meshTriangle)
			{
				if (this.MeshTriangleContainsVertices(this.meshTriangles[i], this.vertices[meshTriangle.vertice1Index], this.vertices[meshTriangle.vertice2Index], 0.001f))
				{
					meshTriangle.neighbour1 = i;
				}
				if (this.MeshTriangleContainsVertices(this.meshTriangles[i], this.vertices[meshTriangle.vertice2Index], this.vertices[meshTriangle.vertice3Index], 0.001f))
				{
					meshTriangle.neighbour2 = i;
				}
				if (this.MeshTriangleContainsVertices(this.meshTriangles[i], this.vertices[meshTriangle.vertice3Index], this.vertices[meshTriangle.vertice1Index], 0.001f))
				{
					meshTriangle.neighbour3 = i;
				}
			}
		}
	}

	protected void FindDistantNeighbours(MeshTriangle meshTriangle)
	{
		for (int i = 0; i < this.meshTriangles.Count; i++)
		{
			if (meshTriangle.neighbour1 >= 0 && meshTriangle.neighbour2 >= 0 && meshTriangle.neighbour3 >= 0)
			{
				if (this.meshTriangles[i] != meshTriangle && this.meshTriangles[i] != this.meshTriangles[meshTriangle.neighbour1] && this.meshTriangles[i] != this.meshTriangles[meshTriangle.neighbour2] && this.meshTriangles[i] != this.meshTriangles[meshTriangle.neighbour3])
				{
					if (this.MeshTriangleContainsVertices(this.meshTriangles[i], this.vertices[meshTriangle.vertice1Index], 0.001f))
					{
						meshTriangle.distantNeighbours1.Add(i);
					}
					if (this.MeshTriangleContainsVertices(this.meshTriangles[i], this.vertices[meshTriangle.vertice2Index], 0.001f))
					{
						meshTriangle.distantNeighbours2.Add(i);
					}
					if (this.MeshTriangleContainsVertices(this.meshTriangles[i], this.vertices[meshTriangle.vertice3Index], 0.001f))
					{
						meshTriangle.distantNeighbours3.Add(i);
					}
				}
			}
		}
	}

	protected bool ContainsOcean(List<int> meshTriangleIndexes)
	{
		for (int i = 0; i < meshTriangleIndexes.Count; i++)
		{
			if (this.meshTriangles[meshTriangleIndexes[i]].terrainType == WorldMapTerrainType.Ocean)
			{
				return true;
			}
		}
		return false;
	}

	protected bool ContainsKnown(List<int> meshTriangleIndexes)
	{
		for (int i = 0; i < meshTriangleIndexes.Count; i++)
		{
			if (this.meshTriangles[meshTriangleIndexes[i]].isKnown)
			{
				return true;
			}
		}
		return false;
	}

	protected bool ContainsGround(List<int> meshTriangleIndexes)
	{
		for (int i = 0; i < meshTriangleIndexes.Count; i++)
		{
			if (this.meshTriangles[meshTriangleIndexes[i]].terrainType != WorldMapTerrainType.Ocean)
			{
				return true;
			}
		}
		return false;
	}

	protected bool ContainsOtherTerritory(MeshTriangle meshTriangle, List<int> meshTriangleIndexes)
	{
		for (int i = 0; i < meshTriangleIndexes.Count; i++)
		{
			if (this.meshTriangles[meshTriangleIndexes[i]].territoryIndex != meshTriangle.territoryIndex)
			{
				return true;
			}
		}
		return false;
	}

	protected bool MeshTriangleContainsVertices(MeshTriangle meshTriangle, Vector3 v1, Vector3 v2, float minDist)
	{
		float num = minDist * minDist;
		return ((this.vertices[meshTriangle.vertice1Index] - v1).sqrMagnitude < num || (this.vertices[meshTriangle.vertice2Index] - v1).sqrMagnitude < num || (this.vertices[meshTriangle.vertice3Index] - v1).sqrMagnitude < num) && ((this.vertices[meshTriangle.vertice1Index] - v2).sqrMagnitude < num || (this.vertices[meshTriangle.vertice2Index] - v2).sqrMagnitude < num || (this.vertices[meshTriangle.vertice3Index] - v2).sqrMagnitude < num);
	}

	protected bool MeshTriangleContainsVertices(MeshTriangle meshTriangle, Vector3 v1, float minDist)
	{
		float num = minDist * minDist;
		return (this.vertices[meshTriangle.vertice1Index] - v1).sqrMagnitude < num || (this.vertices[meshTriangle.vertice2Index] - v1).sqrMagnitude < num || (this.vertices[meshTriangle.vertice3Index] - v1).sqrMagnitude < num;
	}

	private void Update()
	{
	}

	protected float GetTotalSquareDistance(MeshTriangle mTri, Vector3 localPosition)
	{
		return (this.vertices[mTri.vertice1Index] - localPosition).sqrMagnitude + (this.vertices[mTri.vertice2Index] - localPosition).sqrMagnitude + (this.vertices[mTri.vertice3Index] - localPosition).sqrMagnitude;
	}

	public void Click(Vector3 localPosition, WorldMapTerrainType terrainType)
	{
		UnityEngine.Debug.Log("Click!");
		UnityEngine.Debug.Log(" Vertices null " + (this.vertices == null));
		if (this.vertices == null || this.meshTriangles == null || this.meshTriangles.Count < 10)
		{
			this.RebuildMeshTriangles();
		}
		WorldBuilder.lastClickedLocalPosition = localPosition;
		int num = -1;
		int num2 = -1;
		float num3 = 100f;
		float num4 = 100f;
		List<int> list = new List<int>();
		for (int i = 0; i < this.vertices.Length; i++)
		{
			list.Add(i);
		}
		UnityEngine.Debug.Log("meshTriangles  !  " + (this.meshTriangles != null));
		UnityEngine.Debug.Log("meshTriangles length !  " + this.meshTriangles.Count);
		int num5 = 0;
		float num6 = this.GetTotalSquareDistance(this.meshTriangles[num5], localPosition);
		for (int j = 1; j < this.meshTriangles.Count; j++)
		{
			float totalSquareDistance = this.GetTotalSquareDistance(this.meshTriangles[j], localPosition);
			if (totalSquareDistance < num6)
			{
				num6 = totalSquareDistance;
				num5 = j;
			}
		}
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Found nearest vertices ",
			num5,
			" , ",
			num,
			" , ",
			num2,
			"  ...dists ",
			num6,
			" + ",
			num3,
			" + ",
			num4
		}));
		this.meshTriangles[num5].terrainType = terrainType;
		if (terrainType == WorldMapTerrainType.Jungle)
		{
			this.meshTriangles[num5].territoryIndex = this.currentSelectionIndex;
			this.meshTriangles[num5].isKnown = !this.placingUnknownBlocks;
		}
		else
		{
			this.meshTriangles[num5].territoryIndex = -1;
		}
		if (this.worldTrianglesHolder.meshTriangles == null || this.worldTrianglesHolder.meshTriangles.Length < num5)
		{
			this.worldTrianglesHolder.meshTriangles = this.meshTriangles.ToArray();
		}
		else
		{
			this.worldTrianglesHolder.meshTriangles[num5] = this.meshTriangles[num5];
		}
		this.RebuildMesh();
	}

	public WorldTrianglesHolder worldTrianglesHolder;

	public Mesh sourceMesh;

	protected Mesh mesh;

	protected MeshFilter meshFilter;

	protected int[] triangles;

	protected Vector3[] vertices;

	protected Vector3[] normals;

	protected Vector2[] uv;

	protected List<MeshTriangle> meshTriangles = new List<MeshTriangle>();

	public WorldMapTerrainType currentWorldMapTerrainType = WorldMapTerrainType.Jungle;

	public int currentSelectionIndex = -1;

	public bool placingUnknownBlocks;

	public List<WorldTerritory3D> territories = new List<WorldTerritory3D>();

	public static Vector3 lastClickedLocalPosition;

	public static WorldBuilder instance;
}
