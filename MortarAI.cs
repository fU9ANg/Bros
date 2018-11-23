// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MortarAI : EnemyAI
{
	protected override void StartMoving()
	{
	}

	protected override void DoAlertedThink()
	{
		if (Time.time - this.alertTime > 6f)
		{
			if (this.thinkCount < 0)
			{
				if (!this.IsPlayerThisWay(this.walkDirection))
				{
					this.StartMoving();
					this.thinkCounter = 1f - this.sightDelay * (0.5f + UnityEngine.Random.value * 0.1f);
					this.TurnAround();
					base.transform.localScale = new Vector3((float)this.walkDirection, 1f, 1f);
				}
				else
				{
					this.StartMoving();
					this.thinkCounter = 1f - this.sightDelay * (0.5f + UnityEngine.Random.value * 0.1f);
					if (!Demonstration.enemiesHaveDelayOnAlert)
					{
						this.thinkCounter = 0.9f;
					}
				}
			}
			else
			{
				switch (this.thinkCount % 4)
				{
				case 0:
					this.thinkState = EnemyActionState.Shooting;
					this.thinkCounter = 1f - this.attackTime;
					break;
				case 1:
					this.thinkState = EnemyActionState.Idle;
					this.thinkCounter = -1.8f - UnityEngine.Random.value * 0.55f;
					if (UnityEngine.Random.value > 0.8f && !HeroController.PlayerIsAlive(this.seenPlayerNum))
					{
						this.isAwareOfPlayer = false;
					}
					if (!this.IsPlayerThisWay(this.walkDirection))
					{
						this.TurnAround();
					}
					break;
				case 2:
					if (!this.IsPlayerThisWay(this.walkDirection))
					{
						this.StartMoving();
						this.thinkCounter = 0.840000033f - UnityEngine.Random.value * 0.05f;
						this.TurnAround();
					}
					else if (UnityEngine.Random.value < 0.75f && HeroController.IsPlayerNearby(base.transform.position.x - (float)(this.walkDirection * 30), base.transform.position.y + 10f, this.walkDirection, 400f, 50f))
					{
						this.thinkState = EnemyActionState.Shooting;
						this.thinkCounter = 1f - this.attackTime;
					}
					else if (UnityEngine.Random.value < 0.3f)
					{
						this.StartMoving();
						this.thinkCounter = 0.840000033f - UnityEngine.Random.value * 0.05f;
						if (UnityEngine.Random.value < 0.66f)
						{
							this.TurnAround();
						}
					}
					else
					{
						this.thinkState = EnemyActionState.Idle;
					}
					break;
				case 3:
					this.thinkState = EnemyActionState.Idle;
					this.thinkCounter = 0.74f;
					if (!this.IsPlayerThisWay(this.walkDirection))
					{
						this.thinkCounter -= 0.5f;
						this.TurnAround();
					}
					break;
				}
			}
		}
		else
		{
			switch (this.thinkCount % 4)
			{
			case 0:
				this.thinkState = EnemyActionState.Shooting;
				this.thinkCounter = 1f - this.attackTime;
				break;
			case 1:
				this.thinkState = EnemyActionState.Idle;
				this.thinkCounter = -1f;
				if (UnityEngine.Random.value > 0.3f && this.seenPlayerNum >= 0 && this.seenPlayerNum < 5 && !HeroController.PlayerIsAlive(this.seenPlayerNum))
				{
					this.isAwareOfPlayer = false;
				}
				break;
			case 2:
				if (UnityEngine.Random.value < 0.7f && HeroController.IsPlayerNearby(base.transform.position.x - (float)(this.walkDirection * 30), base.transform.position.y + 10f, this.walkDirection, 300f, 50f))
				{
					this.thinkState = EnemyActionState.Shooting;
					this.thinkCounter = 1f - this.attackTime;
				}
				else
				{
					this.thinkCounter = 1f - UnityEngine.Random.value * 0.1f;
					this.thinkState = EnemyActionState.Idle;
				}
				break;
			case 3:
				this.thinkState = EnemyActionState.Idle;
				this.thinkCounter = 0.74f;
				break;
			}
		}
	}
}
