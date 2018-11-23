// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AnimalAI : PolymorphicAI
{
	protected virtual void Start()
	{
		this.animal = base.GetComponent<Animal>();
	}

	protected override void DoIdleThink()
	{
		base.AddAction(EnemyActionType.Wait, UnityEngine.Random.Range(0.5f, 3.5f));
		base.AddAction(EnemyActionType.Move, this.availableGridPoints[UnityEngine.Random.Range(0, this.availableGridPoints.Count)]);
	}

	protected override void DoPanicThink()
	{
		base.DoPanicThink();
	}

	public override bool Panic(int direction, bool forgetPlayer)
	{
		if (direction == 0)
		{
			if (UnityEngine.Random.value > 0.1f)
			{
				direction = -this.walkDirection;
			}
			else
			{
				direction = this.walkDirection;
			}
		}
		this.walkDirection = direction;
		if (this.mentalState != MentalState.Panicking || this.actionQueue.Count == 0)
		{
			this.animal.PlayPanicSound();
			this.actionQueue.Clear();
			base.AddAction(EnemyActionType.Move, new GridPoint(this.animal.collumn + direction * UnityEngine.Random.Range(2, 4), this.animal.row));
		}
		this.mentalState = MentalState.Panicking;
		return true;
	}

	public override void Attract(float xTarget, float yTarget)
	{
	}

	public override void HearSound(float alertX, float alertY)
	{
	}

	public override void FullyAlert(float x, float y, int playerNum)
	{
	}

	public override void Blind()
	{
		base.Blind();
		this.mentalState = MentalState.Idle;
	}

	private Animal animal;
}
