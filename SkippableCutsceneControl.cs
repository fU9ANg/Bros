// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SkippableCutsceneControl : MonoBehaviour
{
	private void Awake()
	{
		this.startTime = Time.time;
		this.cutscene = base.GetComponent<Cutscene>();
	}

	private void Update()
	{
		if (Time.time - this.delayBeforeSkippable > this.startTime && InputReader.GetControllerPressingFire() >= 0)
		{
			this.cutscene.isFinished = true;
		}
		if (Time.time - this.startTime > 7.5f)
		{
			this.cutscene.isFinished = true;
		}
	}

	protected Cutscene cutscene;

	public float delayBeforeSkippable = 1f;

	protected float startTime;
}
