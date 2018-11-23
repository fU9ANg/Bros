// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FluidController : MonoBehaviour
{
	public static FluidController Instance
	{
		get
		{
			if (FluidController.instance == null)
			{
				FluidController.instance = (UnityEngine.Object.FindObjectOfType(typeof(FluidController)) as FluidController);
				if (FluidController.instance == null)
				{
					FluidController.instance = SingletonMono<MapController>.Instance.gameObject.AddComponent<FluidController>();
				}
			}
			return FluidController.instance;
		}
	}

	private void Awake()
	{
	}

	private void Start()
	{
		this.myJob.RefreshSimCellStatus();
	}

	public void Setup(int width, int height)
	{
		this.Width = width;
		this.Height = height;
		this.myJob = new FluidThread();
		this.myJob.Setup(width, height);
		this.myJob.Start();
	}

	public static void RegisterWaterSource(WaterSource source)
	{
		int col = source.col;
		int row = source.row;
		if (FluidController.Instance.myJob.waterSimulator == null)
		{
			UnityEngine.Debug.Log("Fluid Sim not set up yet");
			return;
		}
		FluidCell cell = FluidController.Instance.myJob.waterSimulator.GetCell(col, row);
		if (cell != null)
		{
			cell.HasSource = true;
		}
		else
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Cannot find cell ",
				col,
				" ",
				row
			}));
		}
	}

	public static void DeregisterWaterSource(WaterSource source)
	{
		int col = source.col;
		int row = source.row;
		if (FluidController.Instance.myJob.waterSimulator == null)
		{
			UnityEngine.Debug.Log("Fluid Sim not set up yet");
			return;
		}
		FluidCell cell = FluidController.Instance.myJob.waterSimulator.GetCell(col, row);
		if (cell != null)
		{
			cell.HasSource = false;
		}
		else
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Cannot find cell ",
				col,
				" ",
				row
			}));
		}
	}

	public static bool IsSubmerged(BroforceObject obj)
	{
		return FluidController.IsSubmerged(obj.x, obj.y);
	}

	public static bool IsSubmerged(float x, float y)
	{
		FluidCell cell = FluidController.Instance.myJob.waterSimulator.GetCell(x, y);
		if (cell == null)
		{
			return false;
		}
		if (cell.IsEmpty)
		{
			return false;
		}
		if (cell.IsSolid)
		{
			return false;
		}
		if (cell.Pressure <= 0.1f)
		{
			return false;
		}
		float num = y / 16f - (float)cell.Y + 0.5f;
		return num <= cell.Pressure;
	}

	public static void RefreshFluidStatus(int X, int Y)
	{
	}

	public static bool Paused
	{
		get
		{
			return FluidController.Instance.paused;
		}
	}

	public static void Pause()
	{
		FluidController.Instance.paused = true;
	}

	public static void Unpause()
	{
		FluidController.Instance.paused = false;
	}

	public static void ClearAllFluid()
	{
	}

	private void Update()
	{
		FluidController.count = 0;
	}

	public static FluidCell GetWaterCell(int X, int Y)
	{
		return FluidController.instance.myJob.waterSimulator.GetCell(X, Y);
	}

	private void OnDestroy()
	{
		if (this.myJob != null)
		{
			this.myJob.Exit();
		}
	}

	private static FluidController instance;

	public Water fluidPrefab;

	[HideInInspector]
	public int Width = 30;

	[HideInInspector]
	public int Height = 20;

	public FluidThread myJob;

	private bool paused;

	public static int count;
}
