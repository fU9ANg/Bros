// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class HUDFPS : MonoBehaviour
{
	private void Start()
	{
		this.timeleft = this.updateInterval;
	}

	private void Update()
	{
		this.timeleft -= Time.deltaTime;
		this.accum += Time.timeScale / Time.deltaTime;
		this.frames++;
		if ((double)this.timeleft <= 0.0)
		{
			float num = this.accum / (float)this.frames;
			this.format = string.Format("{0:F2} FPS", num);
			this.timeleft = this.updateInterval;
			this.accum = 0f;
			this.frames = 0;
		}
	}

	private void OnGUI()
	{
		GUI.Label(new Rect((float)(Screen.width / 2 - 100), 0f, 100f, 100f), this.format);
	}

	public float updateInterval = 0.5f;

	private float accum;

	private int frames;

	private float timeleft;

	private string format = string.Empty;
}
