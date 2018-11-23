// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BigGuyAI : PolymorphicAI
{
	protected override void Awake()
	{
		base.Awake();
		this.OnlyDestroyScriptOnSync = true;
	}

	protected override void SetupAvailableGridPoints()
	{
		this.availableGridPoints.Clear();
		UnityEngine.Debug.Log(" SETU P!!  " + this.unit.IsOnGround());
		int num = 0;
		int num2 = 0;
		Map.GetRowCollumn(this.unit.x, this.unit.y, ref num, ref num2);
		if (Map.IsBlockSolid(num2 - 1, num - 1) && !Map.IsBlockSolid(num2 - 1, num) && !Map.IsBlockSolid(num2 - 1, num + 1) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 - 1, num + 1)))
		{
			this.availableGridPoints.Add(new GridPoint(num2 - 1, num));
			if (this.patrolBlocksRadius > 1 && Map.IsBlockSolid(num2 - 2, num - 1) && !Map.IsBlockSolid(num2 - 2, num) && !Map.IsBlockSolid(num2 - 2, num + 1) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 - 2, num + 1)))
			{
				this.availableGridPoints.Insert(0, new GridPoint(num2 - 2, num));
				if (this.patrolBlocksRadius > 2 && Map.IsBlockSolid(num2 - 3, num - 1) && !Map.IsBlockSolid(num2 - 3, num) && !Map.IsBlockSolid(num2 - 3, num + 1) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 - 3, num + 1)))
				{
					this.availableGridPoints.Insert(0, new GridPoint(num2 - 3, num));
					if (this.patrolBlocksRadius > 3 && Map.IsBlockSolid(num2 - 4, num - 1) && !Map.IsBlockSolid(num2 - 4, num) && !Map.IsBlockSolid(num2 - 4, num + 1) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 - 4, num + 1)))
					{
						this.availableGridPoints.Insert(0, new GridPoint(num2 - 4, num));
						if (this.patrolBlocksRadius > 4 && Map.IsBlockSolid(num2 - 5, num - 1) && !Map.IsBlockSolid(num2 - 5, num) && !Map.IsBlockSolid(num2 - 5, num + 1) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 - 5, num + 1)))
						{
							this.availableGridPoints.Insert(0, new GridPoint(num2 - 5, num));
						}
					}
				}
			}
		}
		this.availableGridPoints.Add(new GridPoint(num2, num));
		if (Map.IsBlockSolid(num2 + 1, num - 1) && !Map.IsBlockSolid(num2 + 1, num) && !Map.IsBlockSolid(num2 + 1, num + 1) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 + 1, num + 1)))
		{
			this.availableGridPoints.Add(new GridPoint(num2 + 1, num));
			if (this.patrolBlocksRadius > 1 && Map.IsBlockSolid(num2 + 2, num - 1) && !Map.IsBlockSolid(num2 + 2, num + 1) && !Map.IsBlockSolid(num2 + 2, num) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 + 2, num + 1)))
			{
				this.availableGridPoints.Add(new GridPoint(num2 + 2, num));
				if (this.patrolBlocksRadius > 2 && Map.IsBlockSolid(num2 + 3, num - 1) && !Map.IsBlockSolid(num2 + 3, num + 1) && !Map.IsBlockSolid(num2 + 3, num) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 + 3, num + 1)))
				{
					this.availableGridPoints.Add(new GridPoint(num2 + 3, num));
					if (this.patrolBlocksRadius > 3 && Map.IsBlockSolid(num2 + 4, num - 1) && !Map.IsBlockSolid(num2 + 4, num + 1) && !Map.IsBlockSolid(num2 + 4, num) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 + 4, num + 1)))
					{
						this.availableGridPoints.Add(new GridPoint(num2 + 4, num));
						if (this.patrolBlocksRadius > 4 && Map.IsBlockSolid(num2 + 5, num - 1) && !Map.IsBlockSolid(num2 + 5, num + 1) && !Map.IsBlockSolid(num2 + 5, num) && (this.willClimbThroughVents || !Map.IsBlockSolid(num2 + 5, num + 1)))
						{
							this.availableGridPoints.Add(new GridPoint(num2 + 5, num));
						}
					}
				}
			}
		}
	}

	public override bool Panic(int direction, bool forgetPlayer)
	{
		return this.canBeScared && base.Panic(direction, forgetPlayer);
	}

	public bool canBeScared = true;
}
