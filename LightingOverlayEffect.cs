// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Lighting Overlay")]
public class LightingOverlayEffect : SlinImageEffectBase
{
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, this.material);
	}
}
