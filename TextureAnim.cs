// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[Serializable]
public class TextureAnim
{
	public string name;

	public int loopCycles;

	public bool loopReverse;

	public float framerate = 15f;

	public UVAnimation.ANIM_END_ACTION onAnimEnd;

	public Texture2D[] frames;

	[HideInInspector]
	public Rect[] frameUVs;
}
