// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FluidSimulator
{
	public FluidSimulator(FluidSimulator.FluidType Type, int depth, float viscosity, float flowRate, float fallRate)
	{
		this.Viscosity = viscosity;
		this.FlowRate = flowRate;
		this.FallRate = fallRate;
		this.Width = FluidController.Instance.Width;
		this.Height = FluidController.Instance.Height;
		this.Depth = depth;
		this.type = Type;
		this.WriteOnlyGrid = new FluidCell[this.Width, this.Height];
		this.drawInfos = new DrawInfo[this.Width, this.Height];
		for (int i = 0; i < this.Width; i++)
		{
			for (int j = 0; j < this.Height; j++)
			{
				this.WriteOnlyGrid[i, j] = new FluidCell(this, i, j);
			}
		}
	}

	public void Update()
	{
		for (int i = 0; i < this.Height; i++)
		{
			for (int j = 0; j < this.Width; j++)
			{
				this.WriteOnlyGrid[j, i].RunSource();
			}
		}
		for (int k = 0; k < this.Height; k++)
		{
			for (int l = 0; l < this.Width; l++)
			{
				if (!this.WriteOnlyGrid[l, k].Asleep)
				{
					this.WriteOnlyGrid[l, k].UpdateIndepenedentStates();
				}
			}
		}
		for (int m = 0; m < this.Height; m++)
		{
			for (int n = 0; n < this.Width; n++)
			{
				if (!this.WriteOnlyGrid[n, m].Asleep)
				{
					this.WriteOnlyGrid[n, m].UpdateDependentStates();
				}
			}
		}
		this.CalculateSurfaces();
		this.CalculateContigious();
		this.CalculateAboveComplete();
		for (int num = 0; num < this.Height; num++)
		{
			for (int num2 = 0; num2 < this.Width; num2++)
			{
				if (!this.WriteOnlyGrid[num2, num].Asleep)
				{
					this.WriteOnlyGrid[num2, num].UpdateFalling();
				}
			}
		}
		for (int num3 = 0; num3 < this.Height; num3++)
		{
			for (int num4 = 0; num4 < this.Width; num4++)
			{
				if (!this.WriteOnlyGrid[num4, num3].Asleep)
				{
					this.WriteOnlyGrid[num4, num3].Update();
				}
			}
		}
	}

	private bool AppoximatelyEqual(float a, float b)
	{
		if (a == b)
		{
			return true;
		}
		float num = 0.0001f;
		float num2 = a - b;
		return (num2 >= 0f && num2 < num) || (num2 <= 0f && num2 > -num);
	}

	private void CalculateAboveComplete()
	{
		for (int i = 0; i < this.Height; i++)
		{
			int num = -1;
			int num2 = 0;
			bool flag = false;
			for (int j = 0; j < this.Width; j++)
			{
				this.WriteOnlyGrid[j, i].aboveCompleteSurface = false;
				if (this.WriteOnlyGrid[j, i].FallingPressure > 0f)
				{
					this.WriteOnlyGrid[j, i].hasSource = true;
				}
			}
			for (int k = 0; k < this.Width; k++)
			{
				FluidCell fluidCell = this.WriteOnlyGrid[k, i];
				if (fluidCell.BelowIsFull)
				{
					if (fluidCell.Below != null && fluidCell.Below.partOfCompleteSurface && !fluidCell.IsSolid)
					{
						if (num == -1)
						{
							num = k;
						}
						if (num != -1)
						{
							num2++;
							if (fluidCell.Below != null)
							{
								if (fluidCell.Left != null && fluidCell.Left.Pressure >= this.Viscosity && fluidCell.Left.Below.IsSolid)
								{
									flag = true;
								}
								if (fluidCell.Right != null && fluidCell.Right.Pressure >= this.Viscosity && fluidCell.Right.Below.IsSolid)
								{
									flag = true;
								}
							}
							if (fluidCell.FallingPressure > 0f)
							{
								flag = true;
							}
							FluidCell right = fluidCell.Right;
							if (num != -1 && (right == null || !right.Below.partOfCompleteSurface || right.IsSolid || !fluidCell.Below.IsFull))
							{
								for (int l = num; l < num + num2; l++)
								{
									this.WriteOnlyGrid[l, i].aboveCompleteSurface = true;
									if (flag && !this.WriteOnlyGrid[l, i].partOfCompleteSurface)
									{
										this.WriteOnlyGrid[l, i].hasSource = flag;
									}
								}
								num = -1;
								num2 = 0;
								flag = false;
							}
						}
						else
						{
							num = -1;
							num2 = 0;
							flag = false;
						}
					}
				}
			}
		}
	}

	private void CalculateContigious()
	{
		for (int i = 0; i < this.Height; i++)
		{
			for (int j = 0; j < this.Width; j++)
			{
				this.WriteOnlyGrid[j, i].isBeingDragged = false;
				this.WriteOnlyGrid[j, i].hasSource = false;
				this.WriteOnlyGrid[j, i].DrainDown = false;
				this.WriteOnlyGrid[j, i].AtDrainLevel = false;
				this.WriteOnlyGrid[j, i].AverageSurfacePressure = this.WriteOnlyGrid[j, i].Pressure;
			}
			int num = -1;
			int num2 = 0;
			bool isBeingDragged = false;
			bool drainDown = false;
			bool atDrainLevel = false;
			bool hasSource = false;
			float num3 = 1f;
			float num4 = 0f;
			for (int k = 0; k < this.Width; k++)
			{
				FluidCell fluidCell = this.WriteOnlyGrid[k, i];
				if (!fluidCell.IsEmpty)
				{
					if (num == -1)
					{
						num = k;
					}
					if (num != -1)
					{
						num2++;
						if (fluidCell.Below != null && ((!fluidCell.Below.IsFull && fluidCell.Below.FallingPressure != 0f) || fluidCell.Below.isBeingDragged))
						{
							isBeingDragged = true;
						}
						if (fluidCell.Below != null && fluidCell.Below.DrainDown)
						{
							drainDown = true;
						}
						if (fluidCell.IsDrain)
						{
							drainDown = true;
							if (fluidCell.Below == null || !fluidCell.Below.DrainDown)
							{
								atDrainLevel = true;
							}
						}
						if (fluidCell.Above != null && fluidCell.Above.FallingPressure > 0f)
						{
							hasSource = true;
						}
						if (fluidCell.FallingPressure > 0f)
						{
							hasSource = true;
						}
						num3 = Mathf.Min(num3, fluidCell.Pressure);
						num4 = Mathf.Max(num4, fluidCell.Pressure);
						if (num != -1 && (fluidCell.Right == null || fluidCell.Right.IsSolid || fluidCell.Right.IsEmpty))
						{
							for (int l = num; l < num + num2; l++)
							{
								this.WriteOnlyGrid[l, i].DrainDown = drainDown;
								if (this.WriteOnlyGrid[l, i].Below == null || !this.WriteOnlyGrid[l, i].Below.DrainDown)
								{
									this.WriteOnlyGrid[l, i].AtDrainLevel = atDrainLevel;
								}
								this.WriteOnlyGrid[l, i].isBeingDragged = isBeingDragged;
								this.WriteOnlyGrid[l, i].hasSource = hasSource;
								if (this.WriteOnlyGrid[l, i].Pressure >= this.Viscosity)
								{
									this.WriteOnlyGrid[l, i].AverageSurfacePressure = num3 + (num4 - num3) / 2f;
								}
							}
							num = -1;
							num2 = 0;
							isBeingDragged = false;
							hasSource = false;
							num3 = 1f;
							num4 = 0f;
							drainDown = false;
							atDrainLevel = false;
						}
					}
					else
					{
						num = -1;
						num2 = 0;
						isBeingDragged = false;
						hasSource = false;
						num3 = 1f;
						num4 = 0f;
						drainDown = false;
						atDrainLevel = false;
					}
				}
			}
		}
	}

	private void CalculateSurfaces()
	{
		for (int i = 0; i < this.Height; i++)
		{
			for (int j = 0; j < this.Width; j++)
			{
				this.WriteOnlyGrid[j, i].partOfCompleteSurface = false;
				this.WriteOnlyGrid[j, i].IsStill = false;
			}
			int num = -1;
			int num2 = 0;
			bool isStill = true;
			for (int k = 0; k < this.Width; k++)
			{
				FluidCell fluidCell = this.WriteOnlyGrid[k, i];
				if (fluidCell.PressureIsGreaterTheEqualViscosity)
				{
					if (num == -1 && (fluidCell.Left == null || fluidCell.Left.IsSolid))
					{
						num = k;
					}
					if (num != -1)
					{
						num2++;
						if (!fluidCell.IsFull)
						{
							isStill = false;
						}
						if ((num != -1 && fluidCell.Right == null) || fluidCell.Right.IsSolid)
						{
							for (int l = num; l < num + num2; l++)
							{
								this.WriteOnlyGrid[l, i].partOfCompleteSurface = true;
								this.WriteOnlyGrid[l, i].IsStill = isStill;
							}
							num = -1;
							num2 = 0;
							isStill = false;
						}
					}
				}
				else
				{
					num = -1;
					num2 = 0;
					isStill = true;
				}
			}
		}
	}

	public void OnGUI()
	{
		GUI.color = Color.black;
		for (int i = 0; i < this.Width; i++)
		{
			for (int j = 0; j < this.Height; j++)
			{
				this.WriteOnlyGrid[i, j].DrawGUI();
			}
		}
	}

	public FluidCell GetCell(int x, int y)
	{
		if (x < 0)
		{
			return null;
		}
		if (y < 0)
		{
			return null;
		}
		if (x >= this.Width)
		{
			return null;
		}
		if (y >= this.Height)
		{
			return null;
		}
		return this.WriteOnlyGrid[x, y];
	}

	public FluidCell GetCell(float world_x, float world_y)
	{
		return this.GetCell(Mathf.RoundToInt(world_x / 16f), Mathf.RoundToInt(world_y / 16f));
	}

	public FluidCell GetCell(Vector2 worldPos)
	{
		return this.GetCell(worldPos.x, worldPos.y);
	}

	public void UpdateDrawInfo()
	{
		DrawInfo[,] array = new DrawInfo[this.Width, this.Height];
		for (int i = 0; i < this.Height; i++)
		{
			for (int j = 0; j < this.Width; j++)
			{
				if (!this.WriteOnlyGrid[j, i].Asleep)
				{
					array[j, i] = this.WriteOnlyGrid[j, i].GetDrawInfo();
				}
				else
				{
					array[j, i] = this.drawInfos[j, i];
				}
				array[j, i].HasChanged = (array[j, i] != this.drawInfos[j, i]);
			}
		}
		this.drawInfos = array;
	}

	public void Draw()
	{
		for (int i = 0; i < this.Height; i++)
		{
			for (int j = 0; j < this.Width; j++)
			{
				if (this.drawInfos[j, i].HasChanged)
				{
					FluidController.count++;
					this.WriteOnlyGrid[j, i].Draw(this.drawInfos[j, i]);
				}
			}
		}
	}

	public void AddFluid(int X, int Y)
	{
		FluidCell cell = this.GetCell(X, Y);
		if (cell != null)
		{
			cell.FallingPressure = 1f;
			cell.Poke();
		}
	}

	public void RemoveFluid(int X, int Y)
	{
		FluidCell cell = this.GetCell(X, Y);
		if (cell != null)
		{
			cell.Pressure = 0f;
			cell.Poke();
		}
	}

	public void ClearAllFluid()
	{
		for (int i = 0; i < this.Width; i++)
		{
			for (int j = 0; j < this.Height; j++)
			{
				this.WriteOnlyGrid[i, j].Pressure = 0f;
				this.WriteOnlyGrid[i, j].FallingPressure = 0f;
			}
		}
	}

	public FluidSimulator.FluidType type;

	public float Viscosity = 0.2f;

	public float FlowRate = 3.3f;

	public float FallRate = 6f;

	public int Width;

	public int Height;

	public int Depth;

	public FluidCell[,] ReadOnlyGrid;

	public FluidCell[,] WriteOnlyGrid;

	public DrawInfo[,] drawInfos;

	public enum FluidType
	{
		Water,
		Oil,
		Acid,
		Lava,
		Semen
	}
}
