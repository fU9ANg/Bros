// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[ExecuteInEditMode]
public class SetTextMeshColor : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (!Application.isPlaying && base.GetComponent<Renderer>().sharedMaterial.color != this.color)
		{
			base.GetComponent<Renderer>().material = new Material(base.GetComponent<Renderer>().sharedMaterial);
			base.GetComponent<Renderer>().material.color = this.color;
		}
	}

	public Color color = Color.red;
}
