// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FluidCell
{
	public FluidCell(FluidSimulator grid, int x, int y)
	{
		this.Grid = grid;
		this.X = x;
		this.Y = y;
	}

	public bool IsSolid
	{
		get
		{
			return this.isSolid;
		}
	}

	public bool IsDrain
	{
		get
		{
			return this.isDrain;
		}
	}

	public void RefreshCellStatus()
	{
		this.isDrain = (Map.GetBackgroundGroundType(this.X, this.Y, GroundType.Empty) == GroundType.Empty);
		this.isSolid = Map.IsBlockSolidToWater(this.X, this.Y);
	}

	private bool IsOil
	{
		get
		{
			return this.Grid.Depth == 1;
		}
	}

	public bool IsOpenAndUnderPressure
	{
		get
		{
			return !this.IsSolid && !this.IsFull;
		}
	}

	public FluidCell Above
	{
		get
		{
			if (this.above == null)
			{
				this.above = this.Grid.GetCell(this.X, this.Y + 1);
			}
			return this.above;
		}
	}

	public FluidCell Below
	{
		get
		{
			if (this.below == null)
			{
				this.below = this.Grid.GetCell(this.X, this.Y - 1);
			}
			return this.below;
		}
	}

	public FluidCell Left
	{
		get
		{
			if (this.left == null)
			{
				this.left = this.Grid.GetCell(this.X - 1, this.Y);
			}
			return this.left;
		}
	}

	public FluidCell Right
	{
		get
		{
			if (this.right == null)
			{
				this.right = this.Grid.GetCell(this.X + 1, this.Y);
			}
			return this.right;
		}
	}

	public void Poke()
	{
		this.Asleep = false;
		if (this.Left != null)
		{
			this.Left.Asleep = false;
		}
		if (this.Right != null)
		{
			this.Right.Asleep = false;
		}
		if (this.Above != null)
		{
			this.Above.Asleep = false;
		}
		if (this.Below != null)
		{
			this.Below.Asleep = false;
		}
	}

	public void RunSource()
	{
		if (this.HasSource)
		{
			this.FallingPressure += 0.5f;
			this.FallingPressure = Mathf.Min(this.FallingPressure, 1f);
			this.Asleep = false;
		}
	}

	public void UpdateIndepenedentStates()
	{
		if (this.Grid.type == FluidSimulator.FluidType.Oil)
		{
			FluidCell cell = FluidController.Instance.myJob.waterSimulator.GetCell(this.X, this.Y);
			float b = 1f - cell.Pressure;
			float num = Mathf.Min(this.Pressure, b);
			float num2 = this.Pressure - num;
			if (this.above != null)
			{
				if (num2 + this.above.Pressure >= this.Grid.Viscosity || num == 0f)
				{
					this.Pressure = num;
					this.above.Pressure += num2;
					this.above.Pressure = Mathf.Min(1f, this.above.Pressure);
				}
			}
			else
			{
				this.Pressure = num;
			}
		}
		this.adjacentGreaterThanEqualViscosityAndSupported = false;
		this.IsSupported = false;
		this.isFalling = false;
		if (this.Below == null)
		{
			this.IsSupported = true;
		}
		else if (!this.Below.isFalling && !this.Below.FallExpansion && (this.Below.IsSolid || this.Below.IsFull))
		{
			this.IsSupported = true;
		}
		if (this.Left != null && this.Left.Pressure >= this.Grid.Viscosity && this.Left.IsSupported)
		{
			this.adjacentGreaterThanEqualViscosityAndSupported = true;
		}
		if (this.Right != null && this.Right.Pressure >= this.Grid.Viscosity && this.Right.IsSupported)
		{
			this.adjacentGreaterThanEqualViscosityAndSupported = true;
		}
		this.IsAboveABlockThatIsNotSurrounded = false;
		if (this.Below != null && !this.Below.IsSolid && !this.IsEmpty)
		{
			if (this.Below.IsAboveABlockThatIsNotSurrounded)
			{
				this.IsAboveABlockThatIsNotSurrounded = true;
			}
			else if (this.Below.Left != null && this.Below.Left.IsOpenAndUnderPressure)
			{
				this.IsAboveABlockThatIsNotSurrounded = true;
			}
			else if (this.Below.Right != null && this.Below.Right.IsOpenAndUnderPressure)
			{
				this.IsAboveABlockThatIsNotSurrounded = true;
			}
			else if (this.Below.Below != null && this.Below.Below.IsOpenAndUnderPressure)
			{
				this.IsAboveABlockThatIsNotSurrounded = true;
			}
		}
		if (this.FallingPressure != 0f && !this.FallExpansion && (this.Above == null || this.Above.IsSolid || (this.Above.FallingPressure == 0f && this.Above.IsEmpty)))
		{
			this.isFalling = true;
		}
		if (this.IsFull)
		{
			this.FallingPressure = 0f;
		}
		this.IsEmpty = (this.Pressure == 0f);
		this.IsFull = (this.Pressure >= 1f);
		if (this.Grid.type == FluidSimulator.FluidType.Oil)
		{
			FluidCell waterCell = FluidController.GetWaterCell(this.X, this.Y);
			this.IsFull = (this.Pressure + waterCell.Pressure >= 1f);
		}
		this.BelowIsSolid = false;
		this.BelowIsFull = false;
		if (this.Below != null)
		{
			if (this.Below.IsFull)
			{
				this.BelowIsFull = true;
			}
			if (this.Below.IsSolid)
			{
				this.BelowIsSolid = true;
			}
		}
		this.PressureIsGreaterTheEqualViscosity = (this.Pressure >= this.Grid.Viscosity);
	}

	public void UpdateDependentStates()
	{
		if (this.Pressure > 0f || this.FallingPressure > 0f)
		{
			this.Asleep = false;
			if (this.Left != null)
			{
				this.Left.Asleep = false;
			}
			if (this.Right != null)
			{
				this.Right.Asleep = false;
			}
			if (this.Above != null)
			{
				this.Above.Asleep = false;
			}
			if (this.Below != null)
			{
				this.Below.Asleep = false;
			}
		}
		else
		{
			this.Asleep = true;
			if (this.Left != null && !this.Left.IsEmpty)
			{
				this.Asleep = false;
			}
			if (this.Right != null && !this.Right.IsEmpty)
			{
				this.Asleep = false;
			}
			if (this.Above != null && !this.Above.IsEmpty)
			{
				this.Asleep = false;
			}
			if (this.Below != null && !this.Below.IsEmpty)
			{
				this.Asleep = false;
			}
		}
		if (this.above == null || this.Above.IsSolid || this.FallingPressure == 1f || this.IsFull)
		{
			this.FallExpansion = false;
		}
		if (this.Above != null && !this.IsSolid && !this.IsFull && (this.Above.isFalling || !this.Above.IsEmpty || this.Above.FallingPressure == 1f) && (this.Above.Pressure >= this.Grid.Viscosity || this.Above.FallingPressure > 0f) && this.FallingPressure < 1f)
		{
			this.FallExpansion = true;
		}
		if (this.IsDrain)
		{
			this.IsDraining = true;
		}
		else if (this.Above != null && this.Above.IsDrain)
		{
			this.IsDraining = true;
		}
		else if (this.Below != null && this.Below.IsDraining)
		{
			this.IsDraining = true;
		}
		this.IsEdge = false;
		if (this.Below != null && !this.Below.IsSolid && this.Pressure > 0f)
		{
			if (this.Left.BelowIsSolid && this.Left.PressureIsGreaterTheEqualViscosity)
			{
				this.IsEdge = true;
			}
			else if (this.Right.BelowIsSolid && this.Right.PressureIsGreaterTheEqualViscosity)
			{
				this.IsEdge = true;
			}
		}
	}

	public void UpdateFalling()
	{
		float num = FluidThread.dt * this.Grid.FallRate;
		if (this.FallExpansion)
		{
			this.FallingPressure += num;
		}
		else if (this.isFalling)
		{
			this.FallingPressure -= num;
		}
		this.FallingPressure = Mathf.Clamp(this.FallingPressure, 0f, 1f);
	}

	public void Update()
	{
		float num = FluidThread.dt * this.Grid.FlowRate;
		if (this.IsSolid)
		{
			this.Pressure = 0f;
			this.FallingPressure = 0f;
			return;
		}
		this.dragFluidDown = false;
		this.drainFluidDown = false;
		if (this.Above == null || this.Above.IsEmpty || this.Above.IsEdge)
		{
			if (this.isBeingDragged)
			{
				if (!this.hasSource)
				{
					this.dragFluidDown = true;
				}
				else if (this.Pressure > this.Grid.Viscosity)
				{
					this.dragFluidDown = true;
				}
				else if (!this.adjacentGreaterThanEqualViscosityAndSupported && !this.BelowIsSolid)
				{
					this.dragFluidDown = true;
				}
			}
			if (this.DrainDown)
			{
				if (this.Pressure > this.Grid.Viscosity)
				{
					this.drainFluidDown = true;
				}
				else if (!this.AtDrainLevel)
				{
					if (!this.IsEdge)
					{
						this.drainFluidDown = true;
					}
					else if (!this.hasSource)
					{
						this.drainFluidDown = true;
					}
				}
			}
		}
		if (!this.dragFluidDown && !this.drainFluidDown)
		{
			if (this.Pressure < this.AverageSurfacePressure)
			{
				this.Pressure += num;
				this.Pressure = Mathf.Min(this.AverageSurfacePressure, this.Pressure);
			}
			else if (this.Pressure > this.AverageSurfacePressure && (this.Above == null || this.Above.IsEmpty))
			{
				this.Pressure -= num;
				this.Pressure = Mathf.Max(this.AverageSurfacePressure, this.Pressure);
			}
		}
		if (this.dragFluidDown || this.drainFluidDown)
		{
			this.Pressure -= num;
			this.Pressure = Mathf.Max(0f, this.Pressure);
		}
		else if (this.Pressure < this.Grid.Viscosity)
		{
			if (this.adjacentGreaterThanEqualViscosityAndSupported)
			{
				this.Pressure += num;
				this.Pressure = Mathf.Min(this.Grid.Viscosity, this.Pressure);
			}
			else if (this.hasSource && (this.Below == null || this.Below.IsSolid || this.Below.IsFull))
			{
				this.Pressure += num;
				this.Pressure = Mathf.Min(this.Grid.Viscosity, this.Pressure);
			}
			else if (!this.hasSource && !this.partOfCompleteSurface)
			{
				this.Pressure -= num / 2f;
				this.Pressure = Mathf.Max(0f, this.Pressure);
			}
		}
		else if (this.Pressure < 1f && this.partOfCompleteSurface && this.hasSource && !this.isBeingDragged && !this.DrainDown)
		{
			this.Pressure += num;
			this.Pressure = Mathf.Min(1f, this.Pressure);
		}
		this.Pressure = Mathf.Clamp(this.Pressure, 0f, 1f);
	}

	public void DrawGUI()
	{
	}

	private void DrawValue(float x, float y, float value)
	{
		this.DrawLabel(x, y, string.Empty + System.Math.Round((double)value, 1));
	}

	private void DrawLabel(float x, float y, string value)
	{
		Vector3 position = new Vector3(x, y, 0f) * 16f;
		Vector3 vector = Camera.main.WorldToScreenPoint(position);
		if (vector.x < 0f || vector.y < 0f)
		{
			return;
		}
		if (vector.x > (float)Screen.width || vector.y > (float)Screen.height)
		{
			return;
		}
		Rect position2 = new Rect(vector.x, (float)Screen.height - vector.y, 100f, 100f);
		GUI.Label(position2, value);
	}

	public DrawInfo GetDrawInfo()
	{
		bool flag = this.Pressure > 0f;
		bool flag2 = this.FallingPressure > 0f;
		bool surface = false;
		if (flag)
		{
			if (this.Above == null)
			{
				surface = true;
			}
			else if (this.Above.IsEmpty && (!this.Above.IsSolid || !this.IsFull))
			{
				surface = true;
			}
		}
		return new DrawInfo
		{
			still = flag,
			surface = surface,
			falling = flag2,
			offsetFalling = this.FallExpansion,
			stillHeight = this.Pressure / 1f,
			fallingHeight = this.FallingPressure / 1f
		};
	}

	public void Draw(DrawInfo drawInfo)
	{
		if ((drawInfo.still || drawInfo.surface || drawInfo.falling) && this.fluidInstance == null)
		{
			this.fluidInstance = (UnityEngine.Object.Instantiate(FluidController.Instance.fluidPrefab, new Vector3((float)this.X, (float)this.Y, 0f) * 16f, Quaternion.identity) as Water);
		}
		if (this.fluidInstance != null)
		{
			this.fluidInstance.Draw(drawInfo.still, drawInfo.surface, drawInfo.falling, drawInfo.offsetFalling, drawInfo.stillHeight, drawInfo.fallingHeight);
		}
	}

	public const float MaxPressure = 1f;

	public bool hasSource;

	public bool isBeingDragged;

	public bool isFalling;

	private bool FallExpansion;

	private bool adjacentGreaterThanEqualViscosityAndSupported;

	public bool BelowIsFull;

	public bool BelowIsSolid;

	private bool isSolid;

	private bool isDrain;

	public bool DrainDown;

	public bool IsDraining;

	public bool AtDrainLevel;

	public bool IsFull;

	public bool IsEmpty = true;

	public bool PressureIsGreaterTheEqualViscosity;

	public bool IsSupported;

	public bool partOfSurface;

	public bool aboveCompleteSurface;

	public bool partOfCompleteSurface;

	public bool IsStill;

	public bool IsAboveABlockThatIsNotSurrounded;

	public bool Asleep;

	private bool IsEdge;

	public int X = 1;

	public int Y = 1;

	public float FallingPressure;

	public float Pressure;

	public float AverageSurfacePressure;

	private FluidCell above;

	private FluidCell below;

	private FluidCell left;

	private FluidCell right;

	private FluidSimulator Grid;

	private Water fluidInstance;

	private GameObject still;

	private GameObject falling;

	private GameObject ground;

	public bool HasSource;

	private bool dragFluidDown;

	private bool drainFluidDown;
}
