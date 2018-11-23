// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FluidThread : ThreadedJob
{
	public void Exit()
	{
		this.exit = true;
	}

	public void RefreshSimCellStatus()
	{
		foreach (FluidSimulator fluidSimulator in this.simulators)
		{
			for (int i = 0; i < fluidSimulator.Width; i++)
			{
				for (int j = 0; j < fluidSimulator.Height; j++)
				{
					fluidSimulator.WriteOnlyGrid[i, j].RefreshCellStatus();
				}
			}
		}
	}

	public void Setup(int width, int height)
	{
		this.simulators = new List<FluidSimulator>();
		this.waterSimulator = new FluidSimulator(FluidSimulator.FluidType.Water, 0, 0.1f, 1f, 8f);
		this.simulators.Add(this.waterSimulator);
	}

	protected override void ThreadFunction()
	{
		while (!this.exit)
		{
			FluidThread.dt = (float)(DateTime.Now - this.prevT).TotalSeconds;
			this.prevT = DateTime.Now;
			DateTime now = DateTime.Now;
			this.Update();
			float num = (float)(DateTime.Now - now).TotalSeconds;
			float num2 = Mathf.Max(0f, 0.1f - num);
			Thread.Sleep((int)(num2 * 1000f));
		}
	}

	public void Update()
	{
		if (!this.Paused)
		{
			for (int i = 0; i < this.simulators.Count; i++)
			{
				this.simulators[i].Update();
			}
			for (int j = 0; j < this.simulators.Count; j++)
			{
				this.simulators[j].UpdateDrawInfo();
			}
		}
	}

	public void Draw()
	{
		for (int i = 0; i < this.simulators.Count; i++)
		{
			this.simulators[i].Draw();
		}
	}

	protected override void OnFinished()
	{
		for (int i = 0; i < this.InData.Length; i++)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Results(",
				i,
				"): ",
				this.InData[i]
			}));
		}
	}

	public Vector3[] InData;

	public Vector3[] OutData;

	public FluidSimulator waterSimulator;

	public FluidSimulator oilSimulator;

	private List<FluidSimulator> simulators;

	private bool exit;

	private bool Paused;

	public static float dt;

	public DateTime prevT = DateTime.Now;
}
