// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BossTank : Boss
{
	protected override void AssignAnimations()
	{
		base.AssignAnimations();
		this.animationRumble = this.bossAnimation["Rumble"];
		this.animationRumble.speed = 0.9f;
		this.animationRumble.blendMode = AnimationBlendMode.Blend;
		this.animationRumble.AddMixingTransform(this.bodyTransform);
		this.animationTreadsOpen = this.bossAnimation["TreadsOpen"];
		this.animationTreadsClose = this.bossAnimation["TreadsClose"];
		this.animationTreadsOpen.AddMixingTransform(this.treadsTransform);
		this.animationTreadsClose.AddMixingTransform(this.treadsTransform);
		this.animationTreadsClose.speed = 2f;
		this.animationTreadsOpen.layer = 8;
		this.animationTreadsClose.layer = 8;
		this.animationLeftRocketsOpen = this.bossAnimation["LeftRocketsOpen"];
		this.animationLeftRocketsClose = this.bossAnimation["LeftRocketsClose"];
		this.animationLeftRocketsOpen.AddMixingTransform(this.bodyTransform);
		this.animationLeftRocketsClose.AddMixingTransform(this.bodyTransform);
		this.animationRightRocketsOpen = this.bossAnimation["RightRocketsOpen"];
		this.animationRightRocketsClose = this.bossAnimation["RightRocketsClose"];
		this.animationRightRocketsOpen.AddMixingTransform(this.bodyTransform);
		this.animationRightRocketsClose.AddMixingTransform(this.bodyTransform);
		this.animationBothLauncherOpen = this.bossAnimation["BothLauncherOpen"];
		this.animationBothLauncherClose = this.bossAnimation["BothLauncherClose"];
		this.animationBothLauncherOpen.AddMixingTransform(this.bodyTransform);
		this.animationBothLauncherClose.AddMixingTransform(this.bodyTransform);
		this.animationLeftLauncherOpen = this.bossAnimation["LeftLauncherOpen"];
		this.animationLeftLauncherClose = this.bossAnimation["LeftLauncherClose"];
		this.animationLeftLauncherOpen.AddMixingTransform(this.bodyTransform);
		this.animationLeftLauncherClose.AddMixingTransform(this.bodyTransform);
		this.animationRightLauncherOpen = this.bossAnimation["RightLauncherOpen"];
		this.animationRightLauncherClose = this.bossAnimation["RightLauncherClose"];
		this.animationRightLauncherOpen.AddMixingTransform(this.bodyTransform);
		this.animationRightLauncherClose.AddMixingTransform(this.bodyTransform);
		this.animationLeftLauncherFire = this.bossAnimation["LeftLauncherFire"];
		this.animationRightLauncherFire = this.bossAnimation["RightLauncherFire"];
		this.animationLeftLauncherFire.AddMixingTransform(this.leftLauncherTransform);
		this.animationLeftLauncherFire.layer = 4;
		this.animationLeftLauncherFire.speed = 2f;
		this.animationRightLauncherFire.AddMixingTransform(this.rightLauncherTransform);
		this.animationRightLauncherFire.layer = 4;
		this.animationRightLauncherFire.speed = 2f;
	}

	protected override void Start()
	{
		base.Start();
		this.startXPosition = this.x;
		this.startYPosition = this.y;
		this.rayCastHits = new RaycastHit[4];
		this.thinkState = -1;
		this.GetGroundHeight();
		base.gameObject.AddComponent<AudioSource>();
		base.GetComponent<AudioSource>().clip = this.engineClip;
		base.GetComponent<AudioSource>().dopplerLevel = 0.1f;
		base.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
		base.GetComponent<AudioSource>().maxDistance = 300f;
		base.GetComponent<AudioSource>().pitch = 0.8f;
		base.GetComponent<AudioSource>().minDistance = 150f;
		base.GetComponent<AudioSource>().loop = true;
		if (this.y > this.groundHeight + 48f)
		{
			this.tankPattern = BossTank.BossTankPattern.ComingDown;
			this.thinkCounter = 18f;
			RaycastHit raycastHit;
			if (Physics.Raycast(new Vector3(this.x, this.y, 0f), Vector3.down, out raycastHit, 500f, this.groundLayer))
			{
				this.startYPosition = raycastHit.point.y;
			}
		}
		this.landPositions = new Vector2[28];
		for (int i = 0; i < 28; i++)
		{
			this.landPositions[i] = new Vector2(this.startXPosition + (float)(82 * i), this.startYPosition);
		}
		this.animationEngine.PlayAnimation(this.animationTreadsOpen);
		foreach (BossPiece bossPiece in this.bossPieces)
		{
			if (bossPiece.parentTransform == this.leftSideBossPieceTransform)
			{
				this.leftSideBossPiece = bossPiece;
				UnityEngine.Debug.Log("Found leftside ");
			}
			if (bossPiece.parentTransform == this.rightSideBossPieceTransform)
			{
				this.rightSideBossPiece = bossPiece;
				UnityEngine.Debug.Log("Found rightside ");
			}
			if (bossPiece.parentTransform == this.centerBossPieceTransform)
			{
				this.centerBossPiece = bossPiece;
				this.centerBossPiece.SetHealth(160);
				this.centerBossPiece.immortal = true;
				UnityEngine.Debug.Log("Found center ");
			}
			if (bossPiece.parentTransform == this.treadLeftBossPieceTransform)
			{
				this.treadLeftBossPiece = bossPiece;
				UnityEngine.Debug.Log("Found tread left ");
			}
			if (bossPiece.parentTransform == this.treadRightBossPieceTransform)
			{
				this.treadRightBossPiece = bossPiece;
				UnityEngine.Debug.Log("Found tread right ");
			}
		}
		LetterBoxController.ShowLetterBox(1f, 1.5f);
	}

	protected override void Update()
	{
		if (this.finished)
		{
			UnityEngine.Debug.Log("Finished Work");
			SortOfFollow.ControlledByTriggerAction = true;
			SortOfFollow.followPos += Vector3.up * 60f * Time.deltaTime;
			SortOfFollow.SetZoomLevel(SortOfFollow.zoomLevel);
			if (this.finishedCounter > 0f)
			{
				this.finishedCounter -= Time.deltaTime;
				if (this.finishedCounter <= 0f)
				{
					GameModeController.LevelFinish(LevelResult.Success);
				}
			}
			return;
		}
		base.Update();
		if (this.initialCutscene)
		{
			this.initialCutsceneTime -= this.t;
			if (this.initialCutsceneTime < 0f)
			{
				this.initialCutscene = false;
				LetterBoxController.ClearLetterBox(1f);
			}
		}
		if (Application.isEditor)
		{
			if (Input.GetKeyDown(KeyCode.O))
			{
				this.leftSideBossPiece.health = -1;
				this.rightSideBossPiece.health = -1;
			}
			if (Input.GetKeyDown(KeyCode.P))
			{
				this.centerBossPiece.health = 0;
			}
		}
		if (Map.isEditing || CutsceneController.isInCutscene)
		{
			return;
		}
		if (this.testDestroyAtStart && this.testDestroyDelay > 0f)
		{
			this.testDestroyDelay -= this.t;
			if (this.testDestroyDelay <= 0f)
			{
				this.DropSides();
				this.centerBossPiece.health = 1;
				this.centerBossPiece.Damage(new DamageObject(3, DamageType.Fire, 0f, 0f, null));
			}
		}
		if (this.hasSides && this.dropSides)
		{
			this.MakeDropSidesEffects();
		}
		if (this.centerBossPiece.health <= 0)
		{
			HeroController.SetAllHeroesInvulnerable(30f);
			if (this.tankPattern != BossTank.BossTankPattern.Dying)
			{
				this.SetTankState(BossTank.BossTankPattern.Dying);
			}
			if (this.cameraExplodingTime > 0f)
			{
				this.cameraExplodingTime -= this.t;
			}
			else
			{
				SortOfFollow.ControlledByTriggerAction = true;
				SortOfFollow.followPos = Vector3.Lerp(SortOfFollow.followPos, base.transform.position + Vector3.up * 24f, this.t * 7f);
				SortOfFollow.SetZoomLevel(SortOfFollow.zoomLevel);
			}
			this.explodingTime -= this.t;
			if (this.explodingTime <= 0f)
			{
				EffectsController.CreateExplosion(this.x, this.y + 32f, 32f, 32f, 0f, 2f, 50f, 2f, 1f, true);
				EffectsController.CreateExplosion(this.x, this.y + 32f, 48f, 48f, 0f, 2f, 50f, 2f, 0f, false);
				EffectsController.CreateExplosion(this.x, this.y + 32f, 48f, 48f, 0f, 2f, 50f, 4f, 0f, false);
				EffectsController.CreateGibs(this.centerGibHolder, this.centerBossPiece.transform.position.x, this.centerBossPiece.transform.position.y, 100f, 100f, 0f, 200f);
				base.GetComponent<AudioSource>().Stop();
				UnityEngine.Object.Destroy(base.GetComponent<AudioSource>());
				this.finished = true;
				this.bossAnimation.gameObject.SetActive(false);
				for (int i = 0; i < this.bossPieces.Count; i++)
				{
					this.bossPieces[i].gameObject.SetActive(false);
				}
				return;
			}
			this.explodingCounter += this.t;
			if (this.explodingCounter > 0f)
			{
				this.explodingCounter -= 0.2f - UnityEngine.Random.value * 0.2f;
				EffectsController.CreateSmallExplosion(this.x - 32f + UnityEngine.Random.value * 64f, this.y + UnityEngine.Random.value * 64f, -14f, 0.3f, 0.3f);
				EffectsController.CreateSmallExplosion(this.x - 32f + UnityEngine.Random.value * 64f, this.y + UnityEngine.Random.value * 64f, -14f, 0.4f, 0f);
				if (UnityEngine.Random.value > 0.6f)
				{
					EffectsController.CreateSmallExplosion(this.x - 32f + UnityEngine.Random.value * 64f, this.y + UnityEngine.Random.value * 64f, -14f, 0.4f, 0f);
				}
			}
		}
		this.RunBurning();
		this.RunCrushingTerrainToSides();
		if (!this.hasSides)
		{
			this.RunHasNoSidesYOffset();
		}
		switch (this.tankPattern)
		{
		case BossTank.BossTankPattern.Standing:
			if (!this.hasSides)
			{
				if (this.spawningMooks)
				{
					this.RunSpawningMooks();
				}
			}
			else
			{
				switch (this.thinkState)
				{
				case 1:
					this.rocketFireCounter -= this.t;
					if (this.rocketFireCounter < 0f)
					{
						this.rocketFireCounter = 0.75f;
						if (this.rocketFireIndex <= 3)
						{
							this.FireRocket();
						}
					}
					this.RunStanding();
					goto IL_87C;
				case 3:
					this.RunStanding();
					if (this.spawningMooks)
					{
						this.RunSpawningMooks();
					}
					goto IL_87C;
				case 5:
					this.rocketFireCounter -= this.t;
					if (this.rocketFireCounter < 0f)
					{
						this.rocketFireCounter = 0.32f;
						this.LaunchAirstrikeRocket();
					}
					this.RunStanding();
					goto IL_87C;
				}
				this.RunStanding();
			}
			IL_87C:
			break;
		case BossTank.BossTankPattern.Flying:
			if (!this.hasSides)
			{
				switch (this.thinkState)
				{
				case 1:
					this.RunThrusters();
					this.RunStanding();
					break;
				case 2:
					this.RunThrusters();
					this.RunFlying(this.targetYPosition, this.x);
					break;
				case 3:
					if (this.spawningMooks)
					{
						this.RunSpawningMooks();
					}
					this.RunThrusters();
					this.RunFlying(this.targetYPosition, this.targetXPosition);
					break;
				default:
					this.RunStanding();
					if (this.groundHeight > this.y - 8f && this.thinkCounter > 1f)
					{
						this.thinkCounter = 1f;
					}
					break;
				}
			}
			else
			{
				switch (this.thinkState)
				{
				case 1:
					this.RunThrusters();
					this.RunStanding();
					break;
				case 2:
					this.RunThrusters();
					this.RunFlying(this.targetYPosition, this.x);
					break;
				case 3:
					this.RunThrusters();
					this.RunFlying(this.targetYPosition, this.targetXPosition);
					break;
				default:
					if (this.groundHeight > this.y - 8f && this.thinkCounter > 0.5f)
					{
						this.thinkCounter = 0.5f;
					}
					this.RunStanding();
					break;
				}
			}
			break;
		case BossTank.BossTankPattern.ComingDown:
		{
			this.RunThrusters();
			this.yI = -40f;
			this.GetGroundHeight();
			float num = this.yI * this.t;
			if (this.y + num < this.groundHeight)
			{
				this.yI = 0f;
				this.y = this.groundHeight;
				this.CrushGround();
				this.SetTankState(BossTank.BossTankPattern.Standing);
				this.thinkCounter = 1.8f;
			}
			else
			{
				this.y += num;
			}
			this.shakeCounter += this.t * 3f;
			this.SetPosition(Mathf.Sin(this.shakeCounter) * 4f);
			break;
		}
		case BossTank.BossTankPattern.Dying:
			this.RunThrusters();
			this.RunFlying(Mathf.Max(this.startYPosition + 48f, this.y), this.x);
			this.RunSpawningMooks();
			break;
		}
		if (!this.runningThrusters)
		{
			if (this.yI < -20f)
			{
				if (base.GetComponent<AudioSource>().clip != this.windClip || !base.GetComponent<AudioSource>().isPlaying)
				{
					base.GetComponent<AudioSource>().clip = this.windClip;
					base.GetComponent<AudioSource>().Play();
				}
				base.GetComponent<AudioSource>().pitch = 0.84f + Mathf.Clamp(-this.yI * 0.003f, 0f, 0.5f);
				base.GetComponent<AudioSource>().volume = 0f + Mathf.Clamp(-this.yI * 0.006f, 0f, 0.4f);
			}
			else if (base.GetComponent<AudioSource>().isPlaying)
			{
				base.GetComponent<AudioSource>().Stop();
			}
		}
		else if (this.runningThrusters && (!base.GetComponent<AudioSource>().isPlaying || base.GetComponent<AudioSource>().clip != this.engineClip))
		{
			base.GetComponent<AudioSource>().clip = this.engineClip;
			base.GetComponent<AudioSource>().Play();
			base.GetComponent<AudioSource>().pitch = 0.8f;
			base.GetComponent<AudioSource>().volume = 0.7f;
		}
		this.runningThrusters = false;
	}

	protected override void RunAnimationCues()
	{
		switch (this.animationCue)
		{
		case BossTankAnimationCue.Standing:
		case BossTankAnimationCue.PrepareForLanding:
			if (this.animationTreadsOpen.enabled)
			{
				this.animationEngine.PlayAnimation(this.animationTreadsClose);
			}
			break;
		case BossTankAnimationCue.Flying:
			this.animationEngine.CrossfadeAnimation(this.animationTreadsOpen, 0.3f);
			if (this.animationRightRocketsOpen.enabled)
			{
				this.animationEngine.CrossfadeAnimation(this.animationRightRocketsClose, 0.2f);
			}
			if (this.animationLeftRocketsOpen.enabled)
			{
				this.animationEngine.CrossfadeAnimation(this.animationLeftRocketsClose, 0.2f);
			}
			if (this.animationLeftLauncherOpen.enabled)
			{
				this.animationEngine.CrossfadeAnimation(this.animationLeftLauncherClose, 0.2f);
			}
			if (this.animationRightLauncherOpen.enabled)
			{
				this.animationEngine.CrossfadeAnimation(this.animationRightLauncherClose, 0.2f);
			}
			if (this.animationBothLauncherOpen.enabled)
			{
				this.animationEngine.CrossfadeAnimation(this.animationBothLauncherClose, 0.2f);
			}
			break;
		case BossTankAnimationCue.PrepareForLiftOff:
			this.animationEngine.PlayAnimation(this.animationTreadsOpen);
			break;
		case BossTankAnimationCue.PrepareFiringRight:
			this.animationEngine.PlayAnimation(this.animationRightRocketsOpen);
			break;
		case BossTankAnimationCue.PrepareFiringLeft:
			this.animationEngine.PlayAnimation(this.animationLeftRocketsOpen);
			UnityEngine.Debug.LogWarning("Left Rocket Firing " + this.animationLeftRocketsOpen.enabled);
			break;
		case BossTankAnimationCue.PrepareLaunchingRight:
			this.animationEngine.PlayAnimation(this.animationRightLauncherOpen);
			break;
		case BossTankAnimationCue.PrepareLaunchingLeft:
			this.animationEngine.PlayAnimation(this.animationLeftLauncherOpen);
			break;
		case BossTankAnimationCue.PrepareLaunchingBoth:
			this.animationEngine.PlayAnimation(this.animationBothLauncherOpen);
			UnityEngine.Debug.LogWarning("Both Launcher Firing " + this.animationBothLauncherOpen.enabled);
			break;
		case BossTankAnimationCue.LaunchingRight:
			this.animationEngine.CrossfadeAnimation(this.animationRightLauncherFire, 0.1f);
			break;
		case BossTankAnimationCue.LaunchingLeft:
			this.animationEngine.CrossfadeAnimation(this.animationLeftLauncherFire, 0.1f);
			break;
		case BossTankAnimationCue.StopFiring:
		{
			UnityEngine.Debug.Log("Stop Firing");
			string text = string.Concat(new object[]
			{
				" animationLeftRocketsOpen ",
				this.animationLeftRocketsOpen.enabled,
				" ",
				this.animationLeftRocketsOpen.weight,
				" animationRightRocketsOpen ",
				this.animationRightRocketsOpen.enabled
			});
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				" animationLeftLauncherOpen ",
				this.animationLeftLauncherOpen.enabled,
				" animationRightLauncherOpen ",
				this.animationRightLauncherOpen.enabled
			});
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				" animationBothLauncherOpen ",
				this.animationBothLauncherOpen.enabled,
				" ",
				this.animationBothLauncherOpen.weight
			});
			UnityEngine.Debug.Log(text);
			if (this.animationLeftRocketsOpen.enabled || this.animationLeftRocketsOpen.weight > 0.3f)
			{
				UnityEngine.Debug.Log("animationLeftRocketsOpen Firing");
				this.animationEngine.PlayAnimation(this.animationLeftRocketsClose);
			}
			if (this.animationRightRocketsOpen.enabled || this.animationRightRocketsOpen.weight > 0.3f)
			{
				UnityEngine.Debug.Log("animationRightRocketsOpen Firing");
				this.animationEngine.PlayAnimation(this.animationRightRocketsClose);
			}
			if (this.animationLeftLauncherOpen.enabled || this.animationLeftLauncherOpen.weight > 0.3f)
			{
				UnityEngine.Debug.Log("animationLeftLauncherOpen Firing");
				this.animationEngine.PlayAnimation(this.animationLeftLauncherClose);
			}
			if (this.animationRightLauncherOpen.enabled || this.animationRightLauncherOpen.weight > 0.3f)
			{
				UnityEngine.Debug.Log("animationRightLauncherOpen Firing");
				this.animationEngine.PlayAnimation(this.animationRightLauncherClose);
			}
			if (this.animationBothLauncherOpen.enabled || this.animationBothLauncherOpen.weight > 0.3f)
			{
				UnityEngine.Debug.Log("animationBothLauncherOpen Firing");
				this.animationEngine.PlayAnimation(this.animationBothLauncherClose);
			}
			break;
		}
		}
		this.animationCue = BossTankAnimationCue.None;
	}

	protected override void RunStanding()
	{
		base.RunStanding();
		if (this.tankPattern == BossTank.BossTankPattern.Standing && this.y < this.minYValue)
		{
			this.SetTankState(BossTank.BossTankPattern.Flying);
		}
	}

	protected override void Think()
	{
		this.thinkCounter = 1f;
		this.thinkState++;
		switch (this.tankPattern)
		{
		case BossTank.BossTankPattern.Standing:
			if (!this.hasSides)
			{
				if (this.CheckFlying())
				{
					this.animationCue = BossTankAnimationCue.PrepareForLiftOff;
					this.SetTankState(BossTank.BossTankPattern.Flying);
				}
				else if (UnityEngine.Random.value > 0.3f)
				{
					this.SetTankState(BossTank.BossTankPattern.Flying);
				}
			}
			else
			{
				switch (this.thinkState)
				{
				case 0:
					if (!this.CheckFlying())
					{
						Vector3 vector = base.transform.position - Camera.main.transform.position;
						this.rocketFireDirection = 0;
						if (vector.x > 0f)
						{
							if (this.leftSideBossPiece.health > 0)
							{
								this.animationCue = BossTankAnimationCue.PrepareFiringLeft;
								this.rocketFireDirection = -1;
							}
							else if (this.rightSideBossPiece.health > 0)
							{
								this.animationCue = BossTankAnimationCue.PrepareFiringRight;
								this.rocketFireDirection = 1;
							}
						}
						else if (this.rightSideBossPiece.health > 0)
						{
							this.animationCue = BossTankAnimationCue.PrepareFiringRight;
							this.rocketFireDirection = 1;
						}
						else if (this.leftSideBossPiece.health > 0)
						{
							this.animationCue = BossTankAnimationCue.PrepareFiringLeft;
							this.rocketFireDirection = -1;
						}
						if (this.rocketFireDirection == 0)
						{
							this.DropSides();
						}
						this.thinkCounter = 1f;
					}
					else
					{
						this.animationCue = BossTankAnimationCue.PrepareForLiftOff;
						this.SetTankState(BossTank.BossTankPattern.Flying);
					}
					break;
				case 1:
					this.rocketFireIndex = 0;
					this.rocketFireCounter = 0f;
					this.thinkCounter = 3f;
					break;
				case 2:
					this.animationCue = BossTankAnimationCue.StopFiring;
					break;
				case 3:
				{
					float num = 0f;
					float num2 = 0f;
					int num3 = -1;
					if (HeroController.IsPlayerNearby(this.x, this.y + 128f, 64f, 64f, ref num, ref num2, ref num3))
					{
						this.spawningMooks = true;
						this.thinkCounter = 4.4f;
					}
					else if (HeroController.IsPlayerNearby(this.x, this.y + 80f, 256f, 80f, ref num, ref num2, ref num3))
					{
						this.spawningMooks = true;
						this.thinkCounter = 2.4f;
					}
					else
					{
						this.spawningMooks = false;
						this.thinkCounter = 0.2f;
					}
					break;
				}
				case 4:
					if (!this.CheckFlying())
					{
						this.thinkCounter = 2f;
						if (this.leftSideBossPiece.health > 0 && this.rightSideBossPiece.health > 0)
						{
							this.animationCue = BossTankAnimationCue.PrepareLaunchingBoth;
						}
						else if (this.leftSideBossPiece.health > 0)
						{
							this.animationCue = BossTankAnimationCue.PrepareLaunchingRight;
						}
						else if (this.rightSideBossPiece.health > 0)
						{
							this.animationCue = BossTankAnimationCue.PrepareLaunchingRight;
						}
						else
						{
							this.DropSides();
						}
					}
					else
					{
						this.animationCue = BossTankAnimationCue.PrepareForLiftOff;
						this.SetTankState(BossTank.BossTankPattern.Flying);
					}
					break;
				case 5:
					this.thinkCounter = 2f;
					break;
				case 6:
					this.animationCue = BossTankAnimationCue.StopFiring;
					if (this.leftSideBossPiece.health <= 0 && this.rightSideBossPiece.health <= 0)
					{
						this.DropSides();
					}
					break;
				default:
					if (HeroController.IsPlayerNearby(this.x - 1f, this.y + 16f, 1, 170f, 240f) || HeroController.IsPlayerNearby(this.x + 1f, this.y + 16f, -1, 170f, 240f))
					{
						this.animationCue = BossTankAnimationCue.PrepareForLiftOff;
						this.SetTankState(BossTank.BossTankPattern.Flying);
					}
					else
					{
						this.thinkCounter = 0.1f;
						this.thinkState = -1;
					}
					break;
				}
			}
			break;
		case BossTank.BossTankPattern.Flying:
			switch (this.thinkState)
			{
			case 0:
				this.thinkCounter = 1.6f;
				break;
			case 1:
				this.thinkCounter = 0.4f;
				break;
			case 2:
				this.targetYPosition = this.startYPosition + 96f;
				this.xPositionIndex++;
				this.GetLandPosition();
				this.targetXPosition = this.landPositions[this.currentLandPositionIndex].x;
				this.thinkCounter = 6.2f;
				break;
			case 3:
				if (!this.hasSides)
				{
					this.spawningMooks = true;
				}
				this.thinkCounter = 12f;
				break;
			case 4:
				this.thinkCounter = 8f;
				UnityEngine.Debug.Log("Treads Close");
				this.animationCue = BossTankAnimationCue.PrepareForLanding;
				break;
			case 5:
				this.SetTankState(BossTank.BossTankPattern.Standing);
				break;
			}
			break;
		}
	}

	protected void GetLandPosition()
	{
		this.currentLandPositionIndex = this.minLandPositionIndex;
		int num = 9;
		bool flag = false;
		while (num > 0 && !flag)
		{
			num--;
			this.currentLandPositionIndex = this.minLandPositionIndex + this.xPositionIndex % 2;
			if (this.currentLandPositionIndex >= this.landPositions.Length)
			{
				this.currentLandPositionIndex = this.landPositions.Length - 1;
			}
			RaycastHit raycastHit;
			if (Physics.Raycast(new Vector3(this.landPositions[this.currentLandPositionIndex].x - 24f, this.startYPosition + 80f, 0f), Vector3.down, out raycastHit, 48f, this.groundLayer) || Physics.Raycast(new Vector3(this.landPositions[this.currentLandPositionIndex].x + 24f, this.startYPosition + 80f, 0f), Vector3.down, out raycastHit, 48f, this.groundLayer))
			{
				this.minLandPositionIndex++;
			}
			else if (!Physics.Raycast(new Vector3(this.landPositions[this.currentLandPositionIndex].x - 24f, this.startYPosition + 48f, 0f), Vector3.down, out raycastHit, 80f, this.groundLayer) || !Physics.Raycast(new Vector3(this.landPositions[this.currentLandPositionIndex].x + 24f, this.startYPosition + 48f, 0f), Vector3.down, out raycastHit, 80f, this.groundLayer))
			{
				this.minLandPositionIndex++;
			}
			else
			{
				flag = true;
			}
		}
		if (!flag)
		{
			this.currentLandPositionIndex = this.minLandPositionIndex - UnityEngine.Random.Range(0, 4);
		}
	}

	protected void SetTankState(BossTank.BossTankPattern newPattern)
	{
		if (newPattern == BossTank.BossTankPattern.Flying && this.tankPattern != BossTank.BossTankPattern.Flying)
		{
			this.animationCue = BossTankAnimationCue.Flying;
		}
		if (newPattern == BossTank.BossTankPattern.Standing && this.tankPattern != BossTank.BossTankPattern.Standing)
		{
			this.animationCue = BossTankAnimationCue.Standing;
			this.shakeCounter = 0.3f;
		}
		this.tankPattern = newPattern;
		this.thinkState = -1;
		this.thinkCounter = 0.01f;
		UnityEngine.Debug.Log(" Pattern " + newPattern + "  why? ");
	}

	protected bool CheckFlying()
	{
		float num = 0f;
		float num2 = 0f;
		int num3 = -1;
		if (HeroController.IsPlayerNearby(this.x, this.y + 16f, 70f, 48f, ref num, ref num2, ref num3))
		{
			return true;
		}
		if (HeroController.IsPlayerNearby(this.x, this.y - 140f, 80f, 128f, ref num, ref num2, ref num3))
		{
			this.flyingCheckCount++;
			if (this.flyingCheckCount >= 3)
			{
				this.flyingCheckCount = 0;
				return true;
			}
		}
		return false;
	}

	protected void RunCrushingTerrainToSides()
	{
		if (this.hasSides)
		{
			this.CrushBlocksAtPosition(new Vector3(this.leftSideBossPieceNull.transform.position.x, this.rightSideBossPieceNull.transform.position.y - 8f, 0f));
			this.CrushBlocksAtPosition(new Vector3(this.rightSideBossPieceNull.transform.position.x, this.rightSideBossPieceNull.transform.position.y - 8f, 0f));
		}
	}

	protected void CrushBlocksAtPosition(Vector3 pos)
	{
		Block block = Map.GetBlock(Map.GetCollumn(pos.x - 8f), Map.GetRow(pos.y + 8f));
		if (block != null)
		{
			block.Damage(new DamageObject(5, DamageType.Crush, 0f, 0f, null));
		}
		block = Map.GetBlock(Map.GetCollumn(pos.x + 8f), Map.GetRow(pos.y + 8f));
		if (block != null)
		{
			block.Damage(new DamageObject(5, DamageType.Crush, 0f, 0f, null));
		}
		block = Map.GetBlock(Map.GetCollumn(pos.x - 8f), Map.GetRow(pos.y - 8f));
		if (block != null)
		{
			block.Damage(new DamageObject(5, DamageType.Crush, 0f, 0f, null));
		}
		block = Map.GetBlock(Map.GetCollumn(pos.x + 8f), Map.GetRow(pos.y - 8f));
		if (block != null)
		{
			block.Damage(new DamageObject(5, DamageType.Crush, 0f, 0f, null));
		}
	}

	protected void RunSpawningMooks()
	{
		if (!Connect.IsHost)
		{
			return;
		}
		this.spawnMookCounter += this.t;
		if (this.spawnMookCounter > 0.2f)
		{
			this.SpawnMook();
			if (this.hasSides)
			{
				this.spawnMookCounter -= 0.4f;
			}
			else
			{
				this.spawnMookCounter -= 0.16f;
			}
		}
		if (this.thinkCounter < 1.8f)
		{
			this.spawningMooks = false;
		}
	}

	protected void RunBurning()
	{
		this.burnCounter += this.t;
		if (this.burnCounter > 0.067f)
		{
			this.burnCounter -= 0.067f;
			if (this.centerBossPiece.health <= 0)
			{
				EffectsController.CreateBlackPlumeParticle(this.centerBossPieceNull.transform.position.x, this.centerBossPieceNull.transform.position.y, 20f, 0f, 60f, 2f, 1f);
			}
			if (this.rightSideBossPiece.health <= 0 && !this.rightSideBossPiece.gibbed)
			{
				EffectsController.CreateBlackPlumeParticle(this.rightSideBossPieceNull.transform.position.x, this.rightSideBossPieceNull.transform.position.y, 20f, 0f, 60f, 2f, 1f);
			}
			if (this.leftSideBossPiece.health <= 0 && !this.leftSideBossPiece.gibbed)
			{
				EffectsController.CreateBlackPlumeParticle(this.leftSideBossPieceNull.transform.position.x, this.leftSideBossPieceNull.transform.position.y, 20f, 0f, 60f, 3f, 1f);
			}
		}
	}

	protected void RunThrusters()
	{
		if (this.yI < -40f)
		{
			this.yI = -40f;
		}
		this.runningThrusters = true;
		this.thrusterCounter += this.t;
		if (this.thrusterCounter >= 0.0334f)
		{
			Vector3 position = this.thrusterLeftNull.position;
			Vector3 position2 = this.thrusterRightNull.position;
			float num = -150f + ((this.yI >= 0f) ? 0f : this.yI);
			EffectsController.CreatePlumeParticle(position.x, position.y, 20f, 0f, num, 0.5f, 2f);
			EffectsController.CreatePlumeParticle(position.x + 4f, position.y, 20f, 0f, num * (0.9f + UnityEngine.Random.value * 0.2f), 0.5f, 2f);
			EffectsController.CreatePlumeParticle(position.x + 8f, position.y, 20f, 0f, num * (0.9f + UnityEngine.Random.value * 0.2f), 0.5f, 2f);
			EffectsController.CreatePlumeParticle(position2.x, position2.y, 20f, 0f, num, 0.5f, 2f);
			EffectsController.CreatePlumeParticle(position2.x - 4f, position2.y, 20f, 0f, num * (0.9f + UnityEngine.Random.value * 0.2f), 0.5f, 2f);
			EffectsController.CreatePlumeParticle(position2.x - 8f, position2.y, 20f, 0f, num * (0.9f + UnityEngine.Random.value * 0.2f), 0.5f, 2f);
			if (this.hasSides)
			{
				if (Map.HitUnits(this, 20, DamageType.Crush, 24f, 24f, this.x, this.y + 8f, 0f, -80f, true, false))
				{
				}
			}
			else if (Map.HitUnits(this, 20, DamageType.Crush, 18f, 16f, this.x, this.y - this.hasNoSidesYOffset - 8f, 0f, -80f, true, false))
			{
			}
		}
	}

	protected void DropSides()
	{
		this.thinkState = -1;
		this.dropSides = true;
		this.centerBossPiece.immortal = false;
		this.centerBossPiece.canFriendly = false;
		this.animationCue = BossTankAnimationCue.PrepareForLiftOff;
		this.SetTankState(BossTank.BossTankPattern.Flying);
		this.rayCastHits = new RaycastHit[5];
	}

	private void MakeDropSidesEffects()
	{
		this.hasSides = false;
		this.leftSideBossPiece.Gib();
		this.rightSideBossPiece.Gib();
		this.treadLeftBossPiece.Gib();
		this.treadRightBossPiece.Gib();
		EffectsController.CreateGibs(this.leftSideGibHolder, this.leftSideBossPieceNull.transform.position.x, this.leftSideBossPieceNull.transform.position.y - 16f, 50f, 80f, 0f, 230f);
		EffectsController.CreateGibs(this.rightSideGibHolder, this.rightSideBossPieceNull.transform.position.x, this.rightSideBossPieceNull.transform.position.y - 16f, 50f, 80f, 0f, 230f);
		EffectsController.CreateExplosion(this.leftSideBossPieceNull.position.x, this.leftSideBossPieceNull.position.y, 16f, 16f, 16f, 0.5f, 16f, 1f, 1f, false);
		EffectsController.CreateExplosion(this.rightSideBossPieceNull.position.x, this.rightSideBossPieceNull.position.y, 16f, 16f, 16f, 0.5f, 16f, 1f, 0f, false);
		EffectsController.CreateExplosion(this.leftSideBossPieceNull.position.x, this.leftSideBossPieceNull.position.y - 32f, 16f, 16f, 16f, 0.5f, 16f, 1f, 0f, false);
		EffectsController.CreateExplosion(this.rightSideBossPieceNull.position.x, this.rightSideBossPieceNull.position.y - 32f, 16f, 16f, 16f, 0.5f, 16f, 1f, 0f, false);
	}

	protected void RunHasNoSidesYOffset()
	{
		if (this.hasNoSidesYOffset > -16f)
		{
			this.hasNoSidesYI -= 800f * this.t;
			this.hasNoSidesYOffset += this.hasNoSidesYI * this.t;
			if (this.hasNoSidesYOffset <= -16f)
			{
				this.hasNoSidesYOffset = -16f;
				SortOfFollow.Shake(0.7f);
				this.CrushGround();
			}
			this.bossAnimation.transform.localPosition = new Vector3(0f, this.hasNoSidesYOffset, 0f);
		}
	}

	protected void FireRocket()
	{
		if (!base.IsMine)
		{
			return;
		}
		bool flag = false;
		Vector3 pos = Vector3.zero;
		if (this.rocketFireDirection < 0)
		{
			if (this.leftSideBossPiece.health > 0)
			{
				pos = this.rocketLeftNull.transform.position;
				flag = true;
			}
		}
		else if (this.rocketFireDirection > 0)
		{
			if (this.rightSideBossPiece.health > 0)
			{
				pos = this.rocketRightNull.transform.position;
				flag = true;
			}
		}
		else
		{
			this.thinkCounter = 0f;
		}
		if (flag)
		{
			float arg = 0f;
			float arg2 = 0f;
			int num = -1;
			HeroController.GetRandomPlayerPos(ref arg, ref arg2, ref num);
			Projectile @object = ProjectileController.SpawnProjectileOverNetwork(this.rocketPrefab, this, pos.x, pos.y, (float)(this.rocketFireDirection * 100), 0f, false, -1, false, true);
			if (num >= 0)
			{
				Networking.RPC<float, float, int>(PID.TargetAll, new RpcSignature<float, float, int>(@object.Target), arg, arg2, num, false);
			}
			Sound.GetInstance().PlaySoundEffectAt(this.rocketSoundHolder.attackSounds, 0.5f, pos);
		}
		this.rocketFireIndex++;
	}

	protected void LaunchAirstrikeRocket()
	{
		if (!Connect.IsHost)
		{
			return;
		}
		Vector3 pos = Vector3.zero;
		bool flag = false;
		if (this.rocketFireIndex % 2 == 0)
		{
			if (this.leftSideBossPiece.health > 0)
			{
				pos = this.launcherLeftNull.position;
				this.animationCue = BossTankAnimationCue.LaunchingLeft;
				flag = true;
			}
			else if (this.rightSideBossPiece.health > 0)
			{
				pos = this.launcherRightNull.position;
				this.animationCue = BossTankAnimationCue.LaunchingRight;
				flag = true;
			}
		}
		else if (this.rightSideBossPiece.health > 0)
		{
			pos = this.launcherRightNull.position;
			this.animationCue = BossTankAnimationCue.LaunchingRight;
			flag = true;
		}
		else if (this.leftSideBossPiece.health > 0)
		{
			pos = this.launcherLeftNull.position;
			this.animationCue = BossTankAnimationCue.LaunchingLeft;
			flag = true;
		}
		if (flag)
		{
			Sound.GetInstance().PlaySoundEffectAt(this.rocketSoundHolder.attackSounds, 0.5f, pos);
			ProjectileController.SpawnProjectileOverNetwork(this.rocketAirstrikePrefab, this, pos.x, pos.y, 0f, 200f, false, -1, false, true);
		}
		this.rocketFireIndex++;
	}

	protected void SpawnMook()
	{
		Mook mookPrefab = this.mookTrooper;
		if (UnityEngine.Random.value >= 0.66f)
		{
			mookPrefab = this.mookSuicide;
		}
		bool onFire = false;
		if (this.hasSides)
		{
			MapController.SpawnMook_Networked(mookPrefab, this.mookLaunchPos.position.x, this.mookLaunchPos.position.y, 0f, 0f, false, false, false, onFire, true);
		}
		else
		{
			this.mookSpawnIndex++;
			if (this.mookSpawnIndex % 2 == 0)
			{
				MapController.SpawnMook_Networked(mookPrefab, this.x, this.y + 48f, -180f, 400f, true, this.centerBossPiece.health > 0 && UnityEngine.Random.value > 0.05f, false, false, false);
			}
			else
			{
				MapController.SpawnMook_Networked(mookPrefab, this.x, this.y + 48f, 180f, 400f, true, this.centerBossPiece.health > 0 && UnityEngine.Random.value > 0.05f, false, false, false);
			}
		}
	}

	protected override bool IsAcceptableGround(bool[] groundChecks)
	{
		return ((groundChecks[0] || groundChecks[1]) && (groundChecks[2] || groundChecks[3])) || (!groundChecks[0] && !groundChecks[1] && !groundChecks[2] && !groundChecks[3]);
	}

	protected override void SquashUnits()
	{
		if (this.hasSides)
		{
			if (Map.HitUnits(this, 20, DamageType.Crush, 24f, 2f, this.treadLeftNull.position.x, this.y - 8f, 0f, this.yI, true, false))
			{
			}
			if (Map.HitUnits(this, 20, DamageType.Crush, 24f, 2f, this.treadRightNull.position.x, this.y - 8f, 0f, this.yI, true, false))
			{
			}
		}
		else
		{
			if (Map.HitUnits(this, 20, DamageType.Crush, 12f, 4f, this.thrusterLeftNull.position.x, this.y - this.hasNoSidesYOffset - 8f, 0f, this.yI, true, false))
			{
			}
			if (Map.HitUnits(this, 20, DamageType.Crush, 12f, 4f, this.thrusterRightNull.position.x, this.y - this.hasNoSidesYOffset - 8f, 0f, this.yI, true, false))
			{
			}
		}
	}

	protected override void GetGroundHeight()
	{
		this.groundHeight = -200f;
		this.groundChecks = new bool[this.rayCastHits.Length];
		for (int i = 0; i < this.groundChecks.Length; i++)
		{
			if (this.hasSides)
			{
				if (i < 2)
				{
					this.groundChecks[i] = this.CheckGroundHeight(this.treadLeftNull.position.x - this.x - 8f + (float)(i * 16), 6f, ref this.groundHeight, ref this.rayCastHits[i]);
				}
				else
				{
					this.groundChecks[i] = this.CheckGroundHeight(this.treadRightNull.position.x - this.x - 8f + (float)((i - 2) * 16), 6f, ref this.groundHeight, ref this.rayCastHits[i]);
				}
			}
			else
			{
				this.groundChecks[i] = this.CheckGroundHeight((float)(-32 + i * 16), 6f, ref this.groundHeight, ref this.rayCastHits[i]);
			}
		}
		this.CrushGround(this.groundChecks, this.rayCastHits);
	}

	protected override void CrushGround()
	{
		if (this.hasSides)
		{
			float num = 10f;
			int num2 = 0;
			while ((float)num2 < num)
			{
				float num3 = (num - 1f) * 0.5f * -16f + (float)(num2 * 16);
				RaycastHit raycastHit;
				if (Physics.Raycast(new Vector3(this.x + num3, this.y + 6f, 0f), Vector3.down, out raycastHit, 48f, this.groundLayer) && raycastHit.point.y > this.y - 12f)
				{
					MapController.Damage_Local(this, raycastHit.collider.gameObject, 5, DamageType.Crush, 0f, 0f);
					this.yI = 0f;
				}
				num2++;
			}
		}
		else
		{
			float num4 = 5f;
			int num5 = 0;
			while ((float)num5 < num4)
			{
				float num6 = (num4 - 1f) * 0.5f * -16f + (float)(num5 * 16);
				RaycastHit raycastHit;
				if (Physics.Raycast(new Vector3(this.x + num6, this.y + 6f, 0f), Vector3.down, out raycastHit, 48f, this.groundLayer) && raycastHit.point.y > this.y - 12f)
				{
					MapController.Damage_Local(this, raycastHit.collider.gameObject, 5, DamageType.Crush, 0f, 0f);
					this.yI = 0f;
				}
				num5++;
			}
		}
	}

	protected override void CrushGround(bool[] groundChecks, RaycastHit[] hits)
	{
		if (!this.IsAcceptableGround(groundChecks))
		{
			this.CrushGround();
		}
	}

	public override byte[] Serialize()
	{
		UnityStream unityStream = new UnityStream();
		unityStream.Serialize<int>(this.thinkState);
		unityStream.Serialize<float>(this.thinkCounter);
		unityStream.Serialize<int>((int)this.animationCue);
		unityStream.Serialize<int>((int)this.tankPattern);
		unityStream.Serialize<bool>(this.dropSides);
		unityStream.Serialize<float>(this.targetXPosition);
		unityStream.Serialize<float>(this.targetYPosition);
		unityStream.Serialize<float>(this.startXPosition);
		unityStream.Serialize<float>(this.startYPosition);
		unityStream.Serialize<bool>(this.spawningMooks);
		unityStream.Serialize<int>(this.rocketFireDirection);
		unityStream.Serialize<Vector3>(base.transform.position);
		return unityStream.ByteArray;
	}

	public override void Deserialize(byte[] byteStream)
	{
		UnityStream unityStream = new UnityStream(byteStream);
		this.thinkState = (int)unityStream.DeserializeNext();
		this.thinkCounter = (float)unityStream.DeserializeNext();
		this.animationCue = (BossTankAnimationCue)((int)unityStream.DeserializeNext());
		this.tankPattern = (BossTank.BossTankPattern)((int)unityStream.DeserializeNext());
		this.dropSides = (bool)unityStream.DeserializeNext();
		this.targetXPosition = (float)unityStream.DeserializeNext();
		this.targetYPosition = (float)unityStream.DeserializeNext();
		this.startXPosition = (float)unityStream.DeserializeNext();
		this.startYPosition = (float)unityStream.DeserializeNext();
		this.spawningMooks = (bool)unityStream.DeserializeNext();
		this.rocketFireDirection = (int)unityStream.DeserializeNext();
		base.transform.position = (Vector3)unityStream.DeserializeNext();
		if (this.tankPattern == BossTank.BossTankPattern.Dying)
		{
			this.centerBossPiece.health = 0;
		}
		if (this.dropSides)
		{
			this.leftSideBossPiece.health = 0;
			this.rightSideBossPiece.health = 0;
		}
	}

	protected BossTank.BossTankPattern tankPattern;

	public bool testDestroyAtStart;

	protected float testDestroyDelay = 5.3f;

	protected int rocketFireDirection;

	protected float rocketFireCounter;

	protected int rocketFireIndex;

	protected int mookSpawnIndex;

	protected float targetXPosition;

	protected float targetYPosition;

	protected float startXPosition;

	protected float startYPosition;

	protected int xPositionIndex;

	protected float thrusterCounter;

	protected bool spawningMooks;

	protected float spawnMookCounter;

	protected float burnCounter;

	protected Vector2[] landPositions;

	protected int currentLandPositionIndex;

	protected int minLandPositionIndex;

	public Transform bodyTransform;

	public Transform treadsTransform;

	public Transform leftLauncherTransform;

	public Transform rightLauncherTransform;

	public Transform thrusterLeftNull;

	public Transform thrusterRightNull;

	public Transform rocketLeftNull;

	public Transform rocketRightNull;

	public Transform launcherLeftNull;

	public Transform launcherRightNull;

	public Transform treadLeftNull;

	public Transform treadRightNull;

	public Transform treadLeftBossPieceTransform;

	protected BossPiece treadLeftBossPiece;

	public Transform treadRightBossPieceTransform;

	protected BossPiece treadRightBossPiece;

	public Projectile rocketPrefab;

	public Projectile rocketAirstrikePrefab;

	public Transform leftSideBossPieceTransform;

	protected BossPiece leftSideBossPiece;

	public Transform leftSideBossPieceNull;

	public GibHolder leftSideGibHolder;

	public Transform rightSideBossPieceTransform;

	protected BossPiece rightSideBossPiece;

	public Transform rightSideBossPieceNull;

	public GibHolder rightSideGibHolder;

	protected float explodingTime = 7f;

	protected float cameraExplodingTime = 3f;

	protected float explodingCounter;

	public Transform centerBossPieceTransform;

	protected BossPiece centerBossPiece;

	public Transform centerBossPieceNull;

	public GibHolder centerGibHolder;

	protected int flyingCheckCount;

	protected bool dropSides;

	protected bool hasSides = true;

	protected float hasNoSidesYOffset;

	protected float hasNoSidesYI;

	public AudioClip engineClip;

	public AudioClip windClip;

	public SoundHolder rocketSoundHolder;

	public Mook mookTrooper;

	public Mook mookSuicide;

	public Transform mookLaunchPos;

	public float minYValue = 200f;

	protected AnimationState animationRumble;

	protected AnimationState animationTreadsOpen;

	protected AnimationState animationTreadsClose;

	protected AnimationState animationLeftRocketsOpen;

	protected AnimationState animationLeftRocketsClose;

	protected AnimationState animationRightRocketsOpen;

	protected AnimationState animationRightRocketsClose;

	protected AnimationState animationRightLauncherOpen;

	protected AnimationState animationRightLauncherClose;

	protected AnimationState animationBothLauncherOpen;

	protected AnimationState animationBothLauncherClose;

	protected AnimationState animationLeftLauncherOpen;

	protected AnimationState animationLeftLauncherClose;

	protected AnimationState animationRightLauncherFire;

	protected AnimationState animationLeftLauncherFire;

	protected bool finished;

	protected bool initialCutscene = true;

	public float initialCutsceneTime = 5f;

	protected float finishedCounter = 2f;

	protected bool runningThrusters;

	protected enum BossTankPattern
	{
		Standing,
		Flying,
		Moving,
		ComingDown,
		Dying
	}
}
