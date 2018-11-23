// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldTerritory3DTerrain : MonoBehaviour
{
	private void Start()
	{
		this.renderers = base.GetComponentsInChildren<Renderer>();
	}

	private void Update()
	{
	}

	public void SetMaterial(Material[] m)
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].sharedMaterials = m;
		}
	}

	protected Renderer[] renderers;
}
