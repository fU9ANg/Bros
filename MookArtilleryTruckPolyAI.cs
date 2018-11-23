// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookArtilleryTruckPolyAI : PolymorphicAI
{
	protected override void Update()
	{
		if (!Map.isEditing && this.checkBlocks)
		{
			this.checkBlocks = false;
			this.CheckNearbyBlocks();
		}
		if (!Map.isEditing && this.mentalState != MentalState.Alerted && HeroController.IsPlayerNearby(this.unit.x, this.unit.y + 32f, base.FacingDirection, (float)this.sightRangeX, (float)this.sightRangeY) && HeroController.CanSeePlayer(this.unit.x, this.unit.y + 32f, base.FacingDirection, (float)this.sightRangeX, (float)this.sightRangeY, ref this.seenPlayerNum))
		{
			this.mentalState = MentalState.Alerted;
			this.actionQueue.Clear();
			UnityEngine.Debug.Log("Found Bro !");
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
		if (SortOfFollow.IsItSortOfVisible(base.transform.position + Vector3.up * 16f, 48f, 12f, ref num, ref num2))
		{
			this.actionQueue.Clear();
			base.AddAction(EnemyActionType.Move, new GridPoint(Map.GetCollumn(this.xMin), this.unit.row));
		}
	}

	protected override void DoAlertedThink()
	{
		if ((SortOfFollow.IsItSortOfVisible(this.unit.x, this.unit.y, -4f, 32f) || this.hasAlreadyFired) && (this.seenPlayerNum > -1 || HeroController.IsPlayerNearby(this.unit.x, this.unit.y, base.FacingDirection, (float)this.sightRangeX, (float)this.sightRangeY, ref this.seenPlayerNum)))
		{
			Vector3 playerPos = HeroController.GetPlayerPos(this.seenPlayerNum);
			if (!HeroController.CanSeePlayer(this.unit.x, this.unit.y + 32f, (float)(this.sightRangeX * 2), (float)(this.sightRangeY * 2), ref this.seenPlayerNum) && !HeroController.CanSeePlayer(this.unit.x + (float)(base.FacingDirection * 48), this.unit.y + 48f, (float)this.sightRangeX, (float)(this.sightRangeY * 2), ref this.seenPlayerNum))
			{
				this.cannotSeePlayerCount++;
				UnityEngine.Debug.Log("CanNOT  See Player!... is facing ? " + (base.FacingDirection == (int)Mathf.Sign(playerPos.x - this.unit.x)));
			}
			else
			{
				UnityEngine.Debug.Log("Can See Player!");
				this.cannotSeePlayerCount = 0;
			}
			Vector3 playerPos2 = HeroController.GetPlayerPos(this.seenPlayerNum);
			if (this.cannotSeePlayerCount == 0 || (this.cannotSeePlayerCount < 3 && base.FacingDirection == (int)Mathf.Sign(playerPos2.x - this.unit.x)))
			{
				if (playerPos2.x >= 0f)
				{
					this.hasAlreadyFired = true;
					base.GetComponent<MookArtilleryTruck>().SetTargetPlayerNum(this.seenPlayerNum, playerPos2);
					base.AddAction(EnemyActionType.FacePoint, Map.GetGridPoint(playerPos2));
					base.AddAction(EnemyActionType.Wait, this.sightDelay);
					base.AddAction(EnemyActionType.Fire, this.attackTime);
					base.AddAction(EnemyActionType.Wait, 3f);
				}
			}
			else
			{
				base.AddAction(EnemyActionType.Wait, 0.5f);
			}
		}
		else
		{
			base.AddAction(EnemyActionType.Wait, 0.5f);
		}
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

	public override void GetInput(ref bool left, ref bool right, ref bool up, ref bool down, ref bool jump, ref bool fire, ref bool special1, ref bool special2, ref bool special3, ref bool special4)
	{
		base.GetInput(ref left, ref right, ref up, ref down, ref jump, ref fire, ref special1, ref special2, ref special3, ref special4);
	}

	private float xMin;

	private float xMax;

	private bool checkBlocks = true;

	protected int cannotSeePlayerCount;

	protected bool hasAlreadyFired;
}
