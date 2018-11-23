// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class VillagerAI : PolymorphicAI
{
	protected void Start()
	{
		this.villager = base.GetComponent<Villager>();
	}

	protected override void DoPanicThink()
	{
	}

	public override void GetInput(ref bool left, ref bool right, ref bool up, ref bool down, ref bool jump, ref bool fire, ref bool special1, ref bool special2, ref bool special3, ref bool special4)
	{
		if (this.inMinionMode)
		{
			if (base.GetComponent<FollowerMinion>().leader == null)
			{
				base.GetComponent<FollowerMinion>().FindLeader();
			}
			base.GetComponent<FollowerMinion>().GetInput(ref left, ref right, ref up, ref down, ref jump, ref fire);
		}
		else
		{
			base.GetInput(ref left, ref right, ref up, ref down, ref jump, ref fire, ref special1, ref special2, ref special3, ref special4);
		}
	}

	internal void EnterMinionMode()
	{
		this.inMinionMode = true;
	}

	protected override void LookForPlayer()
	{
	}

	public override void HearSound(float alertX, float alertY)
	{
	}

	public override void FullyAlert(float x, float y, int playerNum)
	{
	}

	public override bool Panic(int direction, bool forgetPlayer)
	{
		return false;
	}

	private Villager villager;

	private bool inMinionMode;
}
