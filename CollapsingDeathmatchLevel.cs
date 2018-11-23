// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CollapsingDeathmatchLevel : MonoBehaviour
{
	private void Start()
	{
		CollapsingDeathmatchLevel.collapsing = false;
		if (GameModeController.GameMode == GameMode.SuicideHorde)
		{
			this.fallInterval = 5f;
		}
	}

	public static bool IsCollapsing()
	{
		return CollapsingDeathmatchLevel.collapsing;
	}

	private void Update()
	{
		if ((!Map.isEditing && GameModeController.IsDeathMatchMode) || GameModeController.GameMode == GameMode.SuicideHorde)
		{
			if (this.firstFrame)
			{
				this.firstFrame = false;
				this.originalLives = (this.currentLives = HeroController.GetTotalLives());
				this.lastFallTime = Time.time;
			}
			this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
			if ((GameModeController.IsDeathMatchMode || GameModeController.GameMode == GameMode.SuicideHorde) && (!GameModeController.InSwitchingScenesPhase() || GameModeController.InRewardPhase()))
			{
				if (CollapsingDeathmatchLevel.collapsing)
				{
					this.collapsingCounter += this.t;
					if (this.collapsingCounter > 0.2f)
					{
						this.collapsingCounter -= 0.2f;
						Map.CollapseTop();
						Map.CollapseTopLadders();
						this.collapsingCount--;
						if (this.collapsingCount == 0)
						{
							CollapsingDeathmatchLevel.collapsing = false;
						}
					}
				}
				else
				{
					this.descentCouner += this.t;
					if (this.descentCouner > this.fallInterval / 3f)
					{
						this.descentCouner = this.fallInterval / 3f;
						if ((Helicopter.DropOffHeliInstance == null || !Helicopter.DropOffHeliInstance.IsLanding()) && ((HeroController.GetTotalLives() < this.currentLives - 2 && Time.time - this.lastFallTime > this.fallInterval / 3f) || Time.time - this.lastFallTime > this.fallInterval))
						{
							this.lastFallTime = Time.time;
							this.descentTimeCount = (this.descentTimeCount + 1) * 100;
							this.currentLives = HeroController.GetTotalLives();
							CollapsingDeathmatchLevel.collapsing = true;
							this.collapsingCount = 3;
							this.descentCouner -= this.fallInterval / 3f;
						}
					}
				}
			}
		}
	}

	private float descentCouner;

	private int descentTimeCount;

	private float t = 0.01f;

	protected int originalLives = 20;

	protected bool firstFrame = true;

	protected int currentLives = 20;

	protected float lastFallTime;

	public float fallInterval = 3f;

	private float collapsingCounter;

	private int collapsingCount;

	private static bool collapsing;
}
