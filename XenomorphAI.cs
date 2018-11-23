// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class XenomorphAI : DogAI
{
	protected override void DoIdleThink()
	{
		if (this.mentalState != MentalState.Alerted && SortOfFollow.IsItSortOfVisible(this.unit.x, this.unit.y, 16f, -16f))
		{
			this.seenPlayerNum = HeroController.GetNearestPlayer(this.unit.x, this.unit.y, 1000f, 1000f);
			if (this.seenPlayerNum < 0)
			{
				this.seenPlayerNum = 0;
			}
			float x = this.unit.x;
			float y = this.unit.y;
			HeroController.GetPlayerPos(this.seenPlayerNum, ref x, ref y);
			this.FullyAlert(x, y, this.seenPlayerNum);
		}
		for (int i = 0; i < UnityEngine.Random.Range(3, 6); i++)
		{
			ActionObject actionObject = new ActionObject(EnemyActionType.Wait, null, UnityEngine.Random.Range(0.3f, 1f));
			if (UnityEngine.Random.value > 0.5f)
			{
				actionObject.type = EnemyActionType.Move;
				actionObject.gridPoint = base.GetNewGridPoint();
			}
			base.AddAction(actionObject, global::QueueMode.Last);
			ActionObject action = new ActionObject(EnemyActionType.LookForPlayer, null, 0f);
			base.AddAction(action, global::QueueMode.Last);
		}
	}

	protected override void GreetPlayer()
	{
		this.mook.PlayGreetingSound();
	}

	public override void RestartExclamationBubble_Networked()
	{
		UnityEngine.Debug.Log("Restart Exclamation");
	}

	public override void RestartQuestionBubble_Networked()
	{
	}

	internal override void FetchObject(Transform fetchObject)
	{
	}
}
