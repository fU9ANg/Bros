// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class SatanAI : PolymorphicAI
{
	protected void Start()
	{
		CheckPoint nearestCheckPoint = Map.GetNearestCheckPoint(250, base.transform.position.x, base.transform.position.y);
		if (nearestCheckPoint != null)
		{
			this.nearestCheckPointX = nearestCheckPoint.transform.position.x;
		}
	}

	public override void GetInput(ref bool left, ref bool right, ref bool up, ref bool down, ref bool jump, ref bool fire, ref bool special1, ref bool special2, ref bool special3, ref bool special4)
	{
		base.GetInput(ref left, ref right, ref up, ref down, ref jump, ref fire, ref special1, ref special2, ref special3, ref special4);
		ActionObject actionObject = (this.actionQueue.Count != 0) ? this.actionQueue[0] : null;
		if (actionObject != null && actionObject.type == EnemyActionType.Fire)
		{
			fire = false;
			special1 = true;
		}
	}

	protected float nearestCheckPointX;
}
