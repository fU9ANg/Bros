// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookTruckPolyAI : PolymorphicAI
{
	protected override void Update()
	{
		if (!Map.isEditing && this.checkBlocks)
		{
			this.checkBlocks = false;
			this.CheckNearbyBlocks();
		}
		if (this.mentalState != MentalState.Alerted && base.CurrentAction != null && base.CurrentAction.type == EnemyActionType.Move && HeroController.IsPlayerNearby(this.unit.x, this.unit.y, base.FacingDirection, (float)this.sightRangeX, (float)this.sightRangeY))
		{
			this.mentalState = MentalState.Alerted;
			this.actionQueue.Clear();
			base.AddAction(EnemyActionType.Fire, 1E+12f);
		}
		base.Update();
	}

	public override void HearSound(float alertX, float alertY)
	{
	}

	public override void Attract(float xTarget, float yTarget)
	{
	}

	public override void Blind()
	{
	}

	protected override void DoIdleThink()
	{
		base.AddAction(EnemyActionType.Wait, 0.4f);
		base.AddAction(EnemyActionType.LookForPlayer);
		base.AddAction(EnemyActionType.Wait, 0.4f);
	}

	protected override void LookForPlayer()
	{
		float num = 0f;
		float num2 = 0f;
		if (SortOfFollow.IsItSortOfVisible(base.transform.position + Vector3.up * 16f, 128f, 12f, ref num, ref num2))
		{
			this.actionQueue.Clear();
			base.AddAction(EnemyActionType.Move, new GridPoint(Map.GetCollumn(this.xMin), this.unit.row));
			base.AddAction(EnemyActionType.Fire, 1E+12f);
		}
	}

	protected override void DoAlertedThink()
	{
		base.AddAction(EnemyActionType.Move, new GridPoint(Map.GetCollumn(this.xMin), this.unit.row));
		base.AddAction(EnemyActionType.Fire, 1E+12f);
	}

	protected virtual void CheckNearbyBlocks()
	{
		this.xMin = -1000f;
		this.xMax = 5000f;
		if (Physics.Raycast(new Vector3(this.unit.x, this.unit.y + 6f, 0f), Vector3.down, out this.rayCastHit, 64f, Map.groundLayer))
		{
			int num = 0;
			int num2 = 0;
			this.unit.y = this.rayCastHit.point.y;
			Map.GetRowCollumn(this.unit.x, this.unit.y + 4f, ref num, ref num2);
			if (!Map.IsBlockSolid(num2 - 3, num - 1) || Map.IsBlockSolid(num2 - 3, num) || Map.IsBlockSolid(num2 - 4, num + 2))
			{
				this.xMin = this.unit.x - 16f;
			}
			else if (!Map.IsBlockSolid(num2 - 4, num - 1) || Map.IsBlockSolid(num2 - 4, num) || Map.IsBlockSolid(num2 - 5, num + 2))
			{
				this.xMin = this.unit.x - 32f;
			}
			else if (!Map.IsBlockSolid(num2 - 5, num - 1) || Map.IsBlockSolid(num2 - 5, num) || Map.IsBlockSolid(num2 - 6, num + 2))
			{
				this.xMin = this.unit.x - 48f;
			}
			else if (!Map.IsBlockSolid(num2 - 6, num - 1) || Map.IsBlockSolid(num2 - 6, num) || Map.IsBlockSolid(num2 - 7, num + 2))
			{
				this.xMin = this.unit.x - 64f;
			}
			else if (!Map.IsBlockSolid(num2 - 7, num - 1) || Map.IsBlockSolid(num2 - 7, num) || Map.IsBlockSolid(num2 - 8, num + 2))
			{
				this.xMin = this.unit.x - 80f;
			}
			else if (!Map.IsBlockSolid(num2 - 8, num - 1) || Map.IsBlockSolid(num2 - 8, num) || Map.IsBlockSolid(num2 - 9, num + 2))
			{
				this.xMin = this.unit.x - 96f;
			}
			else if (!Map.IsBlockSolid(num2 - 9, num - 1) || Map.IsBlockSolid(num2 - 9, num) || Map.IsBlockSolid(num2 - 10, num + 2))
			{
				this.xMin = this.unit.x - 112f;
			}
			else if (!Map.IsBlockSolid(num2 - 10, num - 1) || Map.IsBlockSolid(num2 - 10, num) || Map.IsBlockSolid(num2 - 11, num + 2))
			{
				this.xMin = this.unit.x - 128f;
			}
			else if (!Map.IsBlockSolid(num2 - 11, num - 1) || Map.IsBlockSolid(num2 - 11, num) || Map.IsBlockSolid(num2 - 12, num + 2))
			{
				this.xMin = this.unit.x - 144f;
			}
			else if (!Map.IsBlockSolid(num2 - 12, num - 1) || Map.IsBlockSolid(num2 - 12, num) || Map.IsBlockSolid(num2 - 13, num + 2))
			{
				this.xMin = this.unit.x - 160f;
			}
			else if (!Map.IsBlockSolid(num2 - 13, num - 1) || Map.IsBlockSolid(num2 - 13, num) || Map.IsBlockSolid(num2 - 14, num + 2))
			{
				this.xMin = this.unit.x - 176f;
			}
			else if (!Map.IsBlockSolid(num2 - 14, num - 1) || Map.IsBlockSolid(num2 - 14, num) || Map.IsBlockSolid(num2 - 15, num + 2))
			{
				this.xMin = this.unit.x - 192f;
			}
			else if (!Map.IsBlockSolid(num2 - 15, num - 1) || Map.IsBlockSolid(num2 - 15, num) || Map.IsBlockSolid(num2 - 16, num + 2))
			{
				this.xMin = this.unit.x - 208f;
			}
			else if (!Map.IsBlockSolid(num2 - 16, num - 1) || Map.IsBlockSolid(num2 - 16, num) || Map.IsBlockSolid(num2 - 17, num + 2))
			{
				this.xMin = this.unit.x - 224f;
			}
			else
			{
				this.xMin = this.unit.x - 240f;
			}
			if (!Map.IsBlockSolid(num2 + 1, num - 1) || Map.IsBlockSolid(num2 + 1, num))
			{
				this.xMax = this.unit.x;
			}
			else if (!Map.IsBlockSolid(num2 + 2, num - 1) || Map.IsBlockSolid(num2 + 2, num))
			{
				this.xMax = this.unit.x;
			}
			else if (!Map.IsBlockSolid(num2 + 3, num - 1) || Map.IsBlockSolid(num2 + 3, num))
			{
				this.xMax = this.unit.x + 16f;
			}
			else if (!Map.IsBlockSolid(num2 + 4, num - 1) || Map.IsBlockSolid(num2 + 4, num))
			{
				this.xMax = this.unit.x + 32f;
			}
			else if (!Map.IsBlockSolid(num2 + 5, num - 1) || Map.IsBlockSolid(num2 + 5, num))
			{
				this.xMax = this.unit.x + 48f;
			}
			else
			{
				this.xMax = this.unit.x + 64f;
			}
		}
		else
		{
			base.enabled = false;
			UnityEngine.Debug.LogError("fail setting move extents ");
			this.xMin = this.unit.x - 32f;
			this.xMax = this.unit.x + 32f;
		}
	}

	private float xMin;

	private float xMax;

	private bool checkBlocks = true;
}
