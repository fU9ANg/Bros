// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[Serializable]
public class AnimatedTextureFrames
{
	public AnimatedTextureFrames()
	{
		this.name = "Idle";
		this.chanceToChoose = 0.5f;
		this.startFrameCollumn = 0;
		this.startFrameRow = 0;
		this.endFrameCollumn = 4;
		this.frameRate = 0.0334f;
		this.loops = 0;
	}

	public string name = "Idle";

	public float chanceToChoose = 0.5f;

	public int startFrameCollumn;

	public int startFrameRow;

	public int endFrameCollumn = 4;

	public float frameRate = 0.0334f;

	public int loops;

	public AudioClip[] soundEffect;

	public float soundEffectVolume = 0.4f;
}
