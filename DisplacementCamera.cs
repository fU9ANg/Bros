// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DisplacementCamera : MonoBehaviour
{
	private void Start()
	{
		this.renderTexture = new RenderTexture(Screen.width, Screen.height, 1000, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		this.displacementMaterial.SetTexture("_RefractTex", this.renderTexture);
		this.thisCamera.targetTexture = this.renderTexture;
		this.thisCamera.aspect = Camera.main.aspect;
	}

	private void Update()
	{
	}

	public RenderTexture renderTexture;

	public Material displacementMaterial;

	public Camera thisCamera;
}
