// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Image Refraction")]
[ExecuteInEditMode]
public class ImageRefractionEffect : SlinImageEffectBase
{
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, this.material);
	}
}
