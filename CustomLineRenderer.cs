// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CustomLineRenderer : MonoBehaviour
{
	protected void Awake()
	{
		MeshFilter component = base.GetComponent<MeshFilter>();
		this.mesh = component.mesh;
		if (this.mesh == null)
		{
			this.mesh = new Mesh();
			component.mesh = this.mesh;
		}
	}

	public void Enable()
	{
		if (!base.enabled)
		{
			this.sections.Clear();
		}
		base.enabled = true;
		base.GetComponent<Renderer>().enabled = true;
		base.gameObject.SetActive(true);
	}

	public void Disable()
	{
		if (base.enabled)
		{
			this.sections.Clear();
		}
		base.enabled = false;
		base.GetComponent<Renderer>().enabled = false;
		base.gameObject.SetActive(false);
	}

	protected void LateUpdate()
	{
		Vector3 position = base.transform.position;
		float num = Time.time;
		while (this.sections.Count > 0 && num > this.sections[this.sections.Count - 1].time + this.time)
		{
			this.sections.RemoveAt(this.sections.Count - 1);
		}
		if (this.sections.Count == 0 || (this.sections[0].point - position).sqrMagnitude > this.minDistance * this.minDistance)
		{
			CustomLineRenderer.TronTrailSection tronTrailSection = new CustomLineRenderer.TronTrailSection();
			tronTrailSection.point = position;
			if (this.alwaysUp)
			{
				tronTrailSection.upDir = Vector3.up;
			}
			else
			{
				tronTrailSection.upDir = base.transform.TransformDirection(Vector3.up);
			}
			tronTrailSection.time = num;
			this.sections.Insert(0, tronTrailSection);
		}
		this.mesh.Clear();
		if (this.sections.Count < 2)
		{
			return;
		}
		Vector3[] array = new Vector3[this.sections.Count * 2];
		Color[] array2 = new Color[this.sections.Count * 2];
		Vector2[] array3 = new Vector2[this.sections.Count * 2];
		CustomLineRenderer.TronTrailSection tronTrailSection2 = this.sections[0];
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		for (int i = 0; i < this.sections.Count; i++)
		{
			tronTrailSection2 = this.sections[i];
			float num2 = 0f;
			if (i != 0)
			{
				num2 = Mathf.Clamp01((Time.time - tronTrailSection2.time) / this.time);
			}
			Vector3 upDir = tronTrailSection2.upDir;
			float d = Mathf.Lerp(this.height, this.endHeight, num2);
			array[i * 2] = worldToLocalMatrix.MultiplyPoint(tronTrailSection2.point);
			array[i * 2 + 1] = worldToLocalMatrix.MultiplyPoint(tronTrailSection2.point + upDir * d);
			array[i * 2].x = Mathf.Round(array[i * 2].x);
			array[i * 2].y = Mathf.Round(array[i * 2].y);
			array[i * 2].z = Mathf.Round(array[i * 2].z);
			array[i * 2 + 1].x = Mathf.Round(array[i * 2 + 1].x);
			array[i * 2 + 1].y = Mathf.Round(array[i * 2 + 1].y);
			array[i * 2 + 1].z = Mathf.Round(array[i * 2 + 1].z);
			array3[i * 2] = new Vector2(num2, 0f);
			array3[i * 2 + 1] = new Vector2(num2, 1f);
			Color color = Color.Lerp(this.startColor, this.endColor, num2);
			array2[i * 2] = color;
			array2[i * 2 + 1] = color;
		}
		int[] array4 = new int[(this.sections.Count - 1) * 2 * 3];
		for (int j = 0; j < array4.Length / 6; j++)
		{
			array4[j * 6] = j * 2;
			array4[j * 6 + 1] = j * 2 + 1;
			array4[j * 6 + 2] = j * 2 + 2;
			array4[j * 6 + 3] = j * 2 + 2;
			array4[j * 6 + 4] = j * 2 + 1;
			array4[j * 6 + 5] = j * 2 + 3;
		}
		this.mesh.vertices = array;
		this.mesh.colors = array2;
		this.mesh.uv = array3;
		this.mesh.triangles = array4;
	}

	public float height = 2f;

	public float endHeight;

	public float time = 2f;

	public bool alwaysUp;

	public float minDistance = 0.1f;

	private Mesh mesh;

	public Color startColor = Color.white;

	public Color endColor = new Color(1f, 1f, 1f, 0f);

	private List<CustomLineRenderer.TronTrailSection> sections = new List<CustomLineRenderer.TronTrailSection>();

	private class TronTrailSection
	{
		public Vector3 point;

		public Vector3 upDir;

		public float time;
	}
}
