// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Shing : MonoBehaviour
{
	private void Start()
	{
		this.meshFilter = base.GetComponent<MeshFilter>();
		if (this.mesh == null)
		{
			this.meshFilter.sharedMesh = new Mesh();
			this.mesh = this.meshFilter.sharedMesh;
		}
		this.BuildMesh();
		this.faces[0] = 0;
		this.faces[1] = 2;
		this.faces[2] = 1;
		if (this.mesh != null)
		{
			this.mesh.triangles = this.faces;
		}
		this.SetColor(new Color(1f, 1f, 1f, 1f));
		this.UpdateUVs();
		this.mesh.RecalculateBounds();
	}

	public void SetColor(Color c)
	{
		this.colors[0] = c;
		this.colors[1] = c;
		this.colors[2] = c;
		this.mesh.colors = this.colors;
	}

	public void UpdateUVs()
	{
		this.uvs[0].x = 0f;
		this.uvs[0].y = 1f;
		this.uvs[1].x = 0f;
		this.uvs[1].y = 0f;
		this.uvs[2].x = 1f;
		this.uvs[2].y = 0f;
		this.mesh.uv = this.uvs;
	}

	protected void BuildMesh()
	{
		if (this.mesh == null)
		{
			UnityEngine.Debug.LogError("Awake has not been called yet");
		}
		this.vertices[0].x = -0.5f;
		this.vertices[0].y = 1f;
		this.vertices[0].z = 0f;
		this.vertices[1].x = 0f;
		this.vertices[1].y = 0f;
		this.vertices[1].z = 0f;
		this.vertices[2].x = 0.5f;
		this.vertices[2].y = 1f;
		this.vertices[2].z = 0f;
		this.mesh.vertices = this.vertices;
	}

	private void Update()
	{
	}

	protected Mesh mesh;

	protected Vector3[] vertices = new Vector3[3];

	protected Color[] colors = new Color[3];

	protected Vector2[] uvs = new Vector2[3];

	protected int[] faces = new int[3];

	protected MeshFilter meshFilter;

	[HideInInspector]
	public float rotationSpeed;

	[HideInInspector]
	public float rotation;

	[HideInInspector]
	public float life;

	[HideInInspector]
	public float xScale = 1f;
}
