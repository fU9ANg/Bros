// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class DrillCarrierAI : MooktruckAI
{
	public override void GetTankMovement(ref bool left, ref bool right, ref bool fire, ref bool fireLeft, ref bool fireRight, ref bool special)
	{
		special = false; left = (right = (fireLeft = (fireRight = (fire = (special )))));
		if (Map.isEditing || CutsceneController.isInCutscene)
		{
			return;
		}
		switch (this.thinkState)
		{
		case EnemyActionState.Moving:
			if (this.walkDirection > 0)
			{
				if (this.tank.x >= this.xMax)
				{
					this.tank.x = this.xMax;
					this.thinkCounter = 0.01f;
					this.thinkCount = -1;
					this.tank.Stop();
				}
				else
				{
					right = true;
				}
			}
			else if (this.walkDirection < 0)
			{
				if (this.tank.x <= this.xMin)
				{
					this.tank.x = this.xMin;
					this.thinkCounter = 0.01f;
					this.thinkCount = -1;
					this.tank.Stop();
				}
				else
				{
					left = true;
				}
			}
			break;
		case EnemyActionState.Shooting:
			this.mookCounter += this.t;
			if (this.mookCounter > 0f)
			{
				this.mookCounter -= 0.6f;
				fire = true;
			}
			else
			{
				fire = false;
			}
			break;
		}
	}
}
