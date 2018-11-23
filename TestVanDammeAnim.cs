// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(InvulnerabilityFlash))]
public class TestVanDammeAnim : Unit
{
	protected override void Awake()
	{
		base.Awake();
		this.sprite = base.GetComponent<SpriteSM>();
		this.spriteOffset = this.sprite.offset;
		this.spritePixelWidth = (int)this.sprite.pixelDimensions.x;
		this.spritePixelHeight = (int)this.sprite.pixelDimensions.y;
		if (this.gunSprite != null)
		{
			this.gunSpritePixelWidth = (int)this.gunSprite.pixelDimensions.x;
			this.gunSpritePixelHeight = (int)this.gunSprite.pixelDimensions.y;
		}
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
		this.platformLayer = 1 << LayerMask.NameToLayer("Platform");
		this.ladderLayer = 1 << LayerMask.NameToLayer("Ladders");
		this.victoryLayer = 1 << LayerMask.NameToLayer("Finish");
		this.barrierLayer = 1 << LayerMask.NameToLayer("MobileBarriers");
		this.fragileLayer = 1 << LayerMask.NameToLayer("DirtyHippie");
		this.openDoorsLayer = 1 << LayerMask.NameToLayer("Movetivate");
		this.RegisterUnit();
		if (!Demonstration.herosClimbWalls)
		{
			this.canWallClimb = false;
		}
		this.zOffset = (1f - UnityEngine.Random.value * 2f) * 0.05f;
		this.zombieOffset = (float)(UnityEngine.Random.Range(0, 2) * 2 - 1) * (14f + UnityEngine.Random.value * 14f);
		this.zombieTimerOffset = UnityEngine.Random.Range(2, 21);
		this.headHeight = this.standingHeadHeight;
		if (this.canAirdash)
		{
			this.SetAirdashAvailable();
		}
		if (this.isHero)
		{
			base.SetSyncingInternal(true);
		}
	}

	protected virtual void RegisterUnit()
	{
		Map.RegisterUnit(this, true);
	}

	public void SetUpHero(int PlayerNum, HeroType heroTypeEnum)
	{
		this.heroType = heroTypeEnum;
		this.playerNum = PlayerNum;
		HeroController.RegisterHeroToPlayer(this, this.playerNum, heroTypeEnum);
	}

	public virtual void SetPosition(Vector3 pos)
	{
		base.transform.position = pos;
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
	}

	public virtual void SetPosition()
	{
		base.transform.position = new Vector3(Mathf.Round(this.x), Mathf.Round(this.y), 0f + this.zOffset);
	}

	public virtual void TempInvulnerability(float time)
	{
		this.invulnerable = true;
		this.firedWhileInvulnerable = false;
		if (time > 0f)
		{
			this.invulnerableTime = time;
		}
		else
		{
			this.invulnerableTime = this.t * 1.2f;
		}
	}

	protected virtual void Start()
	{
		this.SpecialAmmo = this.originalSpecialAmmo;
		if (GameModeController.IsDeathMatchMode)
		{
			this.SpecialAmmo = this.originalSpecialAmmo;
			this.SpecialAmmo = HeroController.GetSpecialAmmo(this.playerNum, this.SpecialAmmo);
			if (this.disarmedGunMaterial != null && GameModeController.InRewardPhase() && this.playerNum != GameModeController.GetWinnerNum())
			{
				this.gunSprite.GetComponent<Renderer>().sharedMaterial = this.disarmedGunMaterial;
			}
		}
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		HeroController.SetAvatarCalm(this.playerNum, this.usePrimaryAvatar);
		this.ShowStartBubble();
		if (GameModeController.GameMode == GameMode.Campaign)
		{
			this.SetInvulnerable(2f, false);
		}
		else if (GameModeController.IsDeathMatchMode)
		{
			this.SetInvulnerable(0.05f, false);
		}
		if (this.playerNum >= 0 && this.playerNum < 4)
		{
			EffectsController.AttachLight(this);
		}
		if (HeroControllerTestInfo.HerosAreInvulnerable && Info.IsDevBuild && !base.IsEnemy)
		{
			this.SetInvulnerable(float.PositiveInfinity, true);
		}
		HeroController.SetOriginalSpecialAmmoCount(this.playerNum, this.originalSpecialAmmo);
		this.collumn = (int)(this.x + 8f) / 16;
		this.row = (int)(this.y + 12f) / 16;
		if (base.IsHero)
		{
			this.wallDragAudio = base.gameObject.AddComponent<AudioSource>();
			this.wallDragAudio.playOnAwake = false;
			this.wallDragAudio.rolloffMode = AudioRolloffMode.Linear;
			this.wallDragAudio.minDistance = 170f;
			this.wallDragAudio.dopplerLevel = 0.2f;
			this.wallDragAudio.maxDistance = 300f;
			this.wallDragAudio.volume = 0f;
			this.wallDragAudio.loop = true;
			if (base.GetComponent<PathAgent>() == null)
			{
				PathAgent pathAgent = base.gameObject.AddComponent<PathAgent>();
				pathAgent.agentSize = 9f;
				pathAgent.capabilities = new PathAgentCapabilities();
				pathAgent.capabilities.canWallClimb = true;
				pathAgent.capabilities.canJump = true;
				pathAgent.capabilities.canUseLadders = true;
				pathAgent.capabilities.flying = false;
				pathAgent.capabilities.jumpHeight = 4;
				pathAgent.capabilities.maxDropDistance = 5;
			}
		}
	}

	public void DestroyEllipsis()
	{
		if (base.IsMine)
		{
			Networking.RPC(PID.TargetAll, new RpcSignature(this.DestroyEllipsisRPC), false);
		}
	}

	public void DestroyEllipsisRPC()
	{
		if (this.ellipsis != null)
		{
			this.ellipsis.transform.parent = null;
			UnityEngine.Object.Destroy(this.ellipsis);
		}
	}

	public void CreateEllipsisOnHero()
	{
		if (base.IsMine)
		{
			Networking.RPC(PID.TargetAll, new RpcSignature(this.CreateEllipsisOnHeroRPC), false);
		}
	}

	public void CreateEllipsisOnHeroRPC()
	{
		if (this.health > 0)
		{
			this.DestroyEllipsisRPC();
			GameObject original = Resources.Load("Player/ChatEllipsis") as GameObject;
			this.ellipsis = (UnityEngine.Object.Instantiate(original) as GameObject);
			this.ellipsis.transform.parent = base.transform;
			this.ellipsis.transform.localPosition = Vector3.zero;
		}
	}

	public virtual void ResetSpecialAmmo()
	{
		this.SpecialAmmo = this.originalSpecialAmmo;
		HeroController.SetSpecialAmmo(this.playerNum, this.SpecialAmmo);
	}

	public void ShowStartBubble()
	{
		if (HeroController.mustShowHUDS)
		{
			this.RestartBubble();
			if (this.idleTime > 3f)
			{
				HeroController.FlashAvatar(this.playerNum, 2.5f, this.usePrimaryAvatar);
			}
			else
			{
				HeroController.FlashAvatar(this.playerNum, 1f, this.usePrimaryAvatar);
			}
		}
	}

	public ReactionBubble playerBubble
	{
		get
		{
			switch (this.playerNum)
			{
			case 0:
				return this.player1Bubble;
			case 1:
				return this.player2Bubble;
			case 2:
				return this.player3Bubble;
			case 3:
				return this.player4Bubble;
			default:
				return null;
			}
		}
	}

	public int SpecialAmmo
	{
		get
		{
			return this.specialAmmo;
		}
		set
		{
			if (base.IsMine)
			{
				this.specialAmmo = value;
				Networking.RPC<int>(PID.TargetAll, new RpcSignature<int>(this.SetSpecialAmmoRPC), this.specialAmmo, false);
			}
		}
	}

	public virtual bool IsAmmoFull()
	{
		return this.SpecialAmmo >= this.originalSpecialAmmo;
	}

	public void SetSpecialAmmoRPC(int ammo)
	{
		this.specialAmmo = ammo;
		HeroController.SetSpecialAmmo(this.playerNum, ammo);
	}

	protected virtual bool WallDrag
	{
		get
		{
			return this.wallDrag;
		}
		set
		{
			if (value)
			{
				if (!this.wallDrag && base.IsHero && this.soundHolderFootSteps.wallDragLoops.Length > 0)
				{
					if (this.wallDragAudio != null)
					{
						this.wallDragAudio.clip = this.soundHolderFootSteps.wallDragLoops[UnityEngine.Random.Range(0, this.soundHolderFootSteps.wallDragLoops.Length)];
						this.wallDragAudio.Play();
						this.wallDragAudio.pitch = 0.7f;
					}
					if (!this.useNewKnifeClimbingFrames)
					{
						this.PlayKnifeClimbSound();
					}
				}
				if (this.buttonJump && !this.wallClimbing && this.canWallClimb && this.wallClimbAnticipation)
				{
					this.wallClimbing = true;
					this.lastKnifeClimbStabY = this.y + this.knifeClimbStabHeight;
				}
			}
			else if (this.wallDrag && this.wallDragAudio != null)
			{
				this.wallDragAudio.Stop();
			}
			this.wallDrag = value;
		}
	}

	protected virtual void SetDeltaTime()
	{
		this.lastT = this.t;
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
	}

	protected virtual void SetHighFiveBoostDeltaTime()
	{
		this.t = Mathf.Clamp(Time.deltaTime * this.highFiveBoostM, 0f, 0.04f);
		if (this.timeBroBoostTime > 0f)
		{
			this.timeBroBoostTime -= this.t;
			if (Time.timeScale > 0f)
			{
				this.t = Mathf.Clamp(Time.deltaTime / Time.timeScale, 0f, 0.04f);
			}
			else
			{
				this.t = 0f;
			}
		}
		else if (this.highFiveBoostTime > 6f)
		{
			this.highFiveBoostTime -= this.t;
			if (this.highFiveBoostTime <= 6f)
			{
				this.PlaySpecialSound(0.4f);
			}
		}
		else
		{
			this.highFiveBoostTime -= this.t;
			if (this.highFiveBoostTime <= 0f)
			{
				this.highFiveBoost = false;
			}
		}
	}

	protected override void LateUpdate()
	{
		short inputBits = this.InputBits;
		base.LateUpdate();
		this.InputBits = inputBits;
	}

	private bool IsStandingStill
	{
		get
		{
			return this.IsOnGround() && Mathf.Abs(this.xI) < 0.5f && Mathf.Abs(this.yI) < 0.5f;
		}
	}

	protected virtual void FixedUpdate()
	{
	}

	public void Kick()
	{
		if (!this.hasBeenKicked)
		{
			Networking.RPC(PID.TargetAll, new RpcSignature(this.KickRPC), false);
		}
	}

	public void KickRPC()
	{
		if (!this.hasBeenKicked)
		{
			this.hasBeenKicked = true;
			this.ShowKickBubble();
			base.StartCoroutine(this.KickRoutine());
		}
	}

	private IEnumerator KickRoutine()
	{
		float interval = 0.2f;
		for (int i = 0; i < 14; i++)
		{
			this.kickPlayerBubble.GetComponent<Renderer>().material = EffectsController.instance.kickConfirmedMaterial;
			yield return new WaitForSeconds(interval);
			interval -= 0.01f;
			this.kickPlayerBubble.GetComponent<Renderer>().material = EffectsController.instance.kickConfirmMaterial;
			yield return new WaitForSeconds(interval);
			interval -= 0.01f;
		}
		this.Gib(DamageType.Fall, 0f, 1f);
		MapController.DamageGround(Map.Instance, 25, DamageType.Explosion, 36f, this.x, this.y, null);
		Map.ExplodeUnits(Map.Instance, 25, DamageType.Explosion, 36f, 36f, this.x, this.y, 50f, 400f, -1, false, true);
		Map.ExplodeUnits(Map.Instance, 25, DamageType.Explosion, 54f, 47.88f, this.x, this.y, 50f, 400f, 5, false, true);
		HeroController.Dropout(this.playerNum, true);
		HeroController.KickPlayersIfHeHasNotJoined(base.Owner);
		yield break;
	}

	private void ShowKickBubble()
	{
		if (this.kickPlayerBubble == null)
		{
			this.kickPlayerBubble = EffectsController.CreateKickPlayerBubble(this.x, this.y + 20f);
			this.kickPlayerBubble.transform.parent = base.transform;
			this.kickPlayerBubble.RefreshYStart();
		}
		if (this.kickPlayerBubble.IsHidden)
		{
			this.kickPlayerBubble.RestartBubble();
		}
	}

	private void StopKickBubble()
	{
		if (this.kickPlayerBubble != null)
		{
			this.kickPlayerBubble.StopBubble();
		}
	}

	private void CheckForKick()
	{
		if (base.IsHero)
		{
			if (this.ElgilbleToBeKicked)
			{
				this.ShowKickBubble();
			}
			if (!this.hasBeenKicked)
			{
				bool flag = false;
				foreach (Player player in HeroController.players)
				{
					if (player != null && player.character != null && player.character != this && player.character.XY.ManhattanDistance(this.XY) < 16f)
					{
						flag = true;
						if (player.character.ElgilbleToBeKicked && base.IsMine && this.highFive && !this.wasHighFive)
						{
							player.character.Kick();
						}
					}
				}
				if (this.kickPlayerBubble != null)
				{
					if (flag)
					{
						this.kickPlayerBubble.GetComponent<Renderer>().material = EffectsController.instance.kickConfirmMaterial;
					}
					else
					{
						this.kickPlayerBubble.GetComponent<Renderer>().material = EffectsController.instance.kickdDefaultMaterial;
					}
				}
			}
		}
	}

	protected virtual void Update()
	{
		if (this.ellipsis != null && base.IsMine && !ChatSystem.IsFocused)
		{
			this.DestroyEllipsis();
		}
		if (!this.highFiveBoost && this.timeBroBoostTime <= 0f)
		{
			this.SetDeltaTime();
		}
		else
		{
			this.SetHighFiveBoostDeltaTime();
		}
		if (!Map.Instance.HasBeenSetup)
		{
			return;
		}
		if (this.beingControlledByTriggerAction)
		{
			if (this.controllingTriggerAction == null)
			{
				UnityEngine.Debug.LogError("Being Controlled By Trigger Action, but Controlling Trigger Action is null!");
			}
			else
			{
				this.CheckTriggerActionInput();
			}
		}
		else if (this.stunTime <= 0f)
		{
			this.CheckInput();
		}
		else if (this.health > 0)
		{
			this.stunTime -= this.t;
			this.RunBlindStars();
			this.ClearAllInput();
		}
		if (this.health > 0 && this.isZombie)
		{
			if (this.reviveSource == null || this.reviveSource.health <= 0)
			{
				this.reviveZombieTime -= this.t;
				if (this.reviveZombieTime <= 0f)
				{
					this.Unrevive();
				}
			}
			this.reviveZombieCounter += this.t;
			if (this.reviveZombieCounter > 0.4f)
			{
				UnityEngine.Debug.Log("Make zombie lines ");
				this.reviveZombieCounter -= 2f + UnityEngine.Random.value * 2f;
				EffectsController.CreateRevivedZombiePassiveEffect(this.x, this.y, base.transform.position.z - 0.01f, base.transform);
			}
		}
		if (this.down && this.IsStandingStill && !this.ducking && this.health > 0)
		{
			Map.HitDeadUnits(this, 0, DamageType.SelfEsteem, 2f, this.x, this.y, 0f, 0f, false, false);
			this.StartDucking();
		}
		if (this.highFive && !this.holdingHighFive)
		{
			this.showHighFiveAfterMeleeTimer += Time.deltaTime;
			if (this.showHighFiveAfterMeleeTimer > 1.5f)
			{
				this.PressHighFiveMelee(true);
			}
		}
		SetResolutionCamera.GetScreenExtents(ref this.screenMinX, ref this.screenMaxX, ref this.screenMinY, ref this.screenMaxY);
		this.CheckConstrainToScreenTop();
		if (this.playerNum >= 0 && Map.isEditing && LevelEditorGUI.NoClip)
		{
			this.yI = 0f; this.xI = (this.yI );
			if (this.left)
			{
				this.x -= this.noclipSpeed * this.t;
			}
			if (this.right)
			{
				this.x += this.noclipSpeed * this.t;
			}
			if (this.up)
			{
				this.y += this.noclipSpeed * this.t;
			}
			if (this.down)
			{
				this.y -= this.noclipSpeed * this.t;
			}
			if (this.up || this.down || this.left || this.right)
			{
				this.noclipSpeed += Time.deltaTime * 300f;
			}
			else
			{
				this.noclipSpeed = 200f;
			}
			this.SetPosition();
			return;
		}
		if (this.playerNum < 0 && Map.isEditing)
		{
			return;
		}
		this.footstepDelay -= this.t;
		this.jumpGrace -= this.t;
		this.hangGrace -= this.t;
		this.ignoreHighFivePressTime -= this.t;
		this.parentHasMovedTime -= this.t;
		this.counter += this.t;
		if (this.counter > this.frameRate)
		{
			this.counter -= this.frameRate;
			this.IncreaseFrame();
			this.ChangeFrame();
		}
		this.ShiftUnitWithParent();
		this.CalculateMovement();
		if (this.impaledTransform == null)
		{
			this.RunMovement();
		}
		else
		{
			this.RunImpaledMovement();
		}
		if (base.IsHero && base.IsMine && this.y < this.screenMinY - 30f && TriggerManager.destroyOffscreenPlayers)
		{
			this.Gib(DamageType.OutOfBounds, this.xI, 840f);
		}
		this.RunGun();
		if (this.canDoIndependentMeleeAnimation)
		{
			this.RunIndependentMeleeFrames();
		}
		this.CheckFacingDirection();
		this.RunFiring();
		this.RunAvatarFiring();
		this.RunAvatarRunning();
		if (!Map.isEditing && this.playerNum >= 0 && this.playerNum <= 3 && base.IsHero)
		{
			if (base.IsMine)
			{
				this.CheckRescues();
			}
			Map.CheckCheckPoint(this.xI, this.x, this.y);
			if (this.y > this.groundHeight + 48f && this.y < this.groundHeight + 160f)
			{
				Map.CheckCheckPoint(this.xI, this.x, this.groundHeight + 32f);
			}
			if (base.IsMine && Time.time - this.lastAlertTime > 0.1f && this.health > 0)
			{
				Map.AlertNearbyMooks(this.x, this.y, 8f, 8f, this.playerNum);
			}
			PickupableController.UsePickupables(this, 4f, this.x, this.y);
		}
		if (this.recalling)
		{
			this.recallCounter += this.t;
			if (this.recallCounter > 1f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				if (base.IsMine)
				{
					Networking.RPC<GameObject>(PID.TargetOthers, new RpcSignature<GameObject>(UnityEngine.Object.Destroy), base.gameObject, false);
				}
				Map.RemoveUnit(this);
			}
			else
			{
				this.sprite.SetColor(new Color(this.sprite.color.r, this.sprite.color.g, this.sprite.color.b, 1f - this.recallCounter));
				if (this.gunSprite != null)
				{
					this.gunSprite.SetColor(new Color(this.sprite.color.r, this.sprite.color.g, this.sprite.color.b, 1f - this.recallCounter));
				}
			}
		}
		if (this.invulnerableTime > 0f)
		{
			this.invulnerableTime -= this.t;
			if (this.invulnerableTime <= 0f)
			{
				this.invulnerable = false;
			}
			if (this.fire && !this.wasFire && !this.firedWhileInvulnerable && (GameModeController.IsDeathMatchMode || GameModeController.GameMode == GameMode.BroDown))
			{
				this.firedWhileInvulnerable = true;
				this.invulnerableTime = Mathf.Max(this.invulnerableTime - 0.33f, 0.01f);
			}
		}
		this.CheckDucking();
		this.CheckDestroyed();
		this.RunTrail();
		this.AssignParentedPos();
		if (this.collumn != this.lastCollumn || this.row != this.lastRow)
		{
			this.lastCollumn = this.collumn;
			this.lastRow = this.row;
		}
		if ((this.actionState == ActionState.Dead || this.deathNotificationSent) && !this.hasNotifiedDeathType && Time.time - this.deathTime > 0.33f)
		{
			this.NotifyDeathType();
		}
		this.CheckConstrainToScreenTop();
		this.RunBubbles();
		this.RunWallDraggingAudio();
		this.CheckForKick();
	}

	protected virtual void RunWallDraggingAudio()
	{
		if (base.IsHero && this.canWallClimb && this.wallDragAudio.isPlaying)
		{
			if (this.wallClimbing || this.yI > -20f)
			{
				this.wallDragAudio.pitch = 0.7f;
				this.wallDragAudio.volume = 0f;
			}
			else
			{
				this.wallDragAudio.pitch = Mathf.Lerp(this.wallDragAudio.pitch, 1f, this.t * 5f);
				this.wallDragAudio.volume = Mathf.Lerp(this.wallDragAudio.volume, 0.08f, this.t * 5f);
			}
		}
	}

	protected virtual void CheckFacingDirection()
	{
		if (!this.chimneyFlip && (!this.canAirdash || this.airDashDelay <= 0f))
		{
			if (this.xI < 0f || (this.left && this.health > 0))
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else if (this.xI > 0f || (this.right && this.health > 0))
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
	}

	public override void StartPilotingUnit(Unit pilottedUnit)
	{
		base.enabled = false;
		base.GetComponent<Renderer>().enabled = false;
		this.gunSprite.GetComponent<Renderer>().enabled = false;
		this.gunSprite.enabled = false;
		this.invulnerable = true;
		this.health = 10000;
		this.lastParentedToTransform = null;
		this.pilottedUnit = pilottedUnit;
		UnityEngine.Debug.Log("Start Piloting .. active in hierarchy " + base.gameObject.activeInHierarchy);
	}

	public override Unit GetPilottedUnit()
	{
		return this.pilottedUnit;
	}

	public override void DischargePilotingUnit(float x, float y, float xI, float yI)
	{
		this.SetInvulnerable(0.1f, false);
		if (this.gunSprite != null)
		{
			this.gunSprite.enabled = true;
			this.gunSprite.GetComponent<Renderer>().enabled = true;
		}
		this.ignoreHighFivePressTime = 0.1f;
		this.jumpTime = 0.13f;
		base.enabled = true;
		this.health = 1;
		this.pilottedUnit = null;
		this.SpecialAmmo = this.SpecialAmmo;
		this.x = x;
		this.y = y;
		this.yI = yI;
		this.xIBlast = xI;
	}

	protected virtual void CheckRescues()
	{
		if (HeroController.CheckRescueBros(this.playerNum, this.x, this.y, 12f))
		{
			this.ShowStartBubble();
			this.SetInvulnerable(2f, false);
			StatisticsController.AddBrotalityGrace(3f);
		}
	}

	private void RunBubbles()
	{
		this.bubbleTimer -= Time.deltaTime;
		if (FluidController.IsSubmerged(this) && this.bubbleTimer <= 0f)
		{
			EffectsController.CreateBubbles(this.x + (float)(base.Direction * 5), this.y + 16f, base.transform.position.z - 1f, UnityEngine.Random.Range(2, 4), 3f, 4f);
			this.bubbleTimer = this.bubbleInterval + UnityEngine.Random.Range(0f, this.bubbleInterval);
		}
	}

	protected virtual void RunBlindStars()
	{
		this.blindCounter += this.t;
		if (this.blindCounter > 0.1f)
		{
			this.blindCounter -= 0.5f;
			EffectsController.CreateShrapnelBlindStar(this.x + UnityEngine.Random.value * 2f - 1f, this.y + 6f + this.height * 1.4f, 2f, 2f, 1f, 0f, 20f, base.transform);
		}
	}

	public override void CheckDestroyed()
	{
		if (this.destroyed)
		{
			if (this.playerNum >= 0 && this.playerNum < 4)
			{
				Map.ForgetPlayer(this.playerNum);
			}
			Map.RemoveUnit(this);
			this.ReduceLives(true);
			this.DestroyUnit();
		}
	}

	public virtual void DestroyUnit()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void PlaySpecial2Sound(float pitch)
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.special2Sounds, 0.5f, base.transform.position);
		}
	}

	public virtual void PlayThrowSound(float v)
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.soundHolder != null && this.sound != null)
		{
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.throwSounds, v, base.transform.position);
		}
	}

	public virtual void PlaySpecialAttackSound(float v)
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.soundHolder != null && this.sound != null)
		{
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.specialAttackSounds, v, base.transform.position);
		}
	}

	public virtual void PlaySpecial3Sound(float v)
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.soundHolder != null && this.sound != null)
		{
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.special3Sounds, v, base.transform.position);
		}
	}

	public virtual void PlaySpecial4Sound(float v)
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.soundHolder != null && this.sound != null)
		{
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.special4Sounds, v, base.transform.position);
		}
	}

	public void PlayDashSound(float volume)
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.dashSounds, volume, base.transform.position);
		}
	}

	public void PlaySpecialSound()
	{
		this.PlaySpecialSound(0.8f, 1f);
	}

	public virtual void PlaySpecialSound(float volume)
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.specialSounds, volume, base.transform.position);
		}
	}

	public void PlaySpecialSound(float volume, float pitch, bool bypassReverb)
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.specialSounds, volume, base.transform.position, pitch, bypassReverb);
		}
	}

	public void PlaySpecialSound(float volume, float pitch)
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.specialSounds, volume, base.transform.position, pitch);
		}
	}

	public void PlayPowerUpSound(float volume)
	{
		this.PlayPowerUpSound(volume, 1f);
	}

	public void PlayPowerUpSound(float volume, float pitch)
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.powerUp, volume, base.transform.position, pitch);
		}
	}

	public virtual void PlayPanicSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.panic, 0.65f, base.transform.position, UnityEngine.Random.Range(1f, 1.175f));
		}
	}

	public virtual void PlayGreetingSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.greeting, 1f, base.transform.position);
		}
	}

	public void PlayConfusedSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.confused, 0.66f, base.transform.position);
		}
	}

	public void PlayAttractedSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null && this.soundHolder.attractSounds != null && this.soundHolder.attractSounds.Length > 0)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.attractSounds, 0.86f, base.transform.position);
		}
	}

	protected virtual void UseFire()
	{
		this.FireWeapon(this.x + base.transform.localScale.x * 10f, this.y + 8f, base.transform.localScale.x * 400f, (float)UnityEngine.Random.Range(-20, 20));
		this.PlayAttackSound();
		Map.DisturbWildLife(this.x, this.y, 60f, this.playerNum);
	}

	public virtual void PlayHighFiveSound()
	{
		if (Time.time - TestVanDammeAnim.lastHighFiveTime > 0.3f)
		{
			this.sound.PlaySoundEffectSpecial(this.soundHolder.greeting, 0.9f);
			TestVanDammeAnim.lastHighFiveTime = Time.time;
		}
	}

	public virtual void PlayBassDropSoundSound()
	{
		if (Time.time - TestVanDammeAnim.lastBassTime > 0.3f)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.bassDrop, 0.6f, base.transform.position);
			TestVanDammeAnim.lastBassTime = Time.time;
		}
	}

	protected virtual void PlayAttackSound()
	{
		this.PlayAttackSound(0.3f);
	}

	protected virtual void PlayAttackSound(float v)
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.attackSounds, v, base.transform.position);
		}
	}

	protected virtual void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 3;
		this.SetGunSprite(this.gunFrame, 1);
		EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.15f, ySpeed * 0.15f, base.transform);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed, ySpeed, this.playerNum);
	}

	protected virtual void UseSpecial()
	{
		if (this.SpecialAmmo > 0)
		{
			this.PlayThrowSound(0.4f);
			this.SpecialAmmo--;
			if (base.IsMine)
			{
				ProjectileController.SpawnGrenadeOverNetwork(this.specialGrenade, this, this.x + Mathf.Sign(base.transform.localScale.x) * 8f, this.y + 8f, 0.001f, 0.011f, Mathf.Sign(base.transform.localScale.x) * 200f, 150f, this.playerNum);
			}
		}
		else
		{
			HeroController.FlashSpecialAmmo(this.playerNum);
			this.ActivateGun();
		}
	}

	protected virtual void CheckForTraps()
	{
		if (Map.isEditing)
		{
			return;
		}
		if (!base.IsEnemy && !base.IsMine)
		{
			return;
		}
		RaycastHit raycastHit;
		if (Physics.Raycast(new Vector3(this.x, this.y + 5f, 0f), Vector3.down, out raycastHit, 16f, this.groundLayer))
		{
			Block component = raycastHit.collider.GetComponent<Block>();
			if (component != null)
			{
				if (base.IsMine || base.IsEnemy)
				{
					component.CheckForMine();
				}
				float num = 8f;
				if (this.yI < -50f && this.actionState != ActionState.Dead && raycastHit.distance > 4f && raycastHit.distance < num && component.spikes != null)
				{
					this.y = raycastHit.point.y + 3f;
					Networking.RPC<TestVanDammeAnim>(PID.TargetAll, new RpcSignature<TestVanDammeAnim>(component.spikes.ImpaleUnit), this, false);
				}
			}
		}
	}

	protected virtual void SetSpriteOffset(float xOffset, float yOffset)
	{
		this.sprite.SetOffset(new Vector3(this.spriteOffset.x + xOffset, this.spriteOffset.y + yOffset, this.spriteOffset.z));
	}

	protected virtual void AnimateMelee()
	{
		this.SetSpriteOffset(0f, 0f);
		this.rollingFrames = 0;
		if (this.frame == 1)
		{
			this.counter -= 0.0334f;
		}
		if (this.frame == 6 && this.meleeFollowUp)
		{
			this.counter -= 0.08f;
			this.frame = 1;
			this.meleeFollowUp = false;
		}
		this.frameRate = 0.025f;
		if (this.standingMelee)
		{
			int num = 25 + Mathf.Clamp(this.frame, 0, 6);
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)this.spritePixelHeight);
		}
		else if (this.jumpingMelee)
		{
			int num2 = 17 + Mathf.Clamp(this.frame, 0, 6);
			this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)(this.spritePixelHeight * 6));
		}
		else if (this.dashingMelee)
		{
			int num3 = 17 + Mathf.Clamp(this.frame, 0, 6);
			this.sprite.SetLowerLeftPixel((float)(num3 * this.spritePixelWidth), (float)(this.spritePixelHeight * 6));
			if (this.frame == 4)
			{
				this.counter -= 0.0334f;
			}
			else if (this.frame == 5)
			{
				this.counter -= 0.0334f;
			}
		}
		if (this.frame == 3)
		{
			if (Map.HitClosestUnit(this, this.playerNum, 4, DamageType.Knifed, 14f, 24f, this.x + base.transform.localScale.x * 8f, this.y + 8f, base.transform.localScale.x * 200f, 500f, true, false, base.IsMine, false))
			{
				this.sound.PlaySoundEffectAt(this.soundHolder.meleeHitSound, 1f, base.transform.position);
				this.meleeHasHit = true;
			}
			else
			{
				this.sound.PlaySoundEffectAt(this.soundHolder.missSounds, 0.3f, base.transform.position);
			}
			this.meleeChosenUnit = null;
			if (this.TryBustCage())
			{
				this.meleeHasHit = true;
			}
		}
		else if (this.frame > 3 && !this.meleeHasHit && Map.HitClosestUnit(this, this.playerNum, 4, DamageType.Knifed, 12f, 14f, this.x + base.transform.localScale.x * 8f, this.y + 8f, base.transform.localScale.x * 200f, 500f, true, false, base.IsMine, false))
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.meleeHitSound, 0.3f, base.transform.position);
			this.meleeHasHit = true;
		}
		if (this.frame >= 6)
		{
			this.frame = 0;
			this.CancelMelee();
		}
	}

	protected virtual void AnimateThrowingGrenade()
	{
		this.DeactivateGun();
		this.SetSpriteOffset(0f, 0f);
		this.frameRate = 0.0334f;
		if (!this.useNewThrowingFrames)
		{
			int num = 17 + Mathf.Clamp(this.frame, 0, 5);
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)this.spritePixelHeight);
			switch (this.frame)
			{
			case 0:
				this.SetHeldGrenadePos(-8f, 17f);
				break;
			case 1:
				this.SetHeldGrenadePos(6f, 19f);
				break;
			case 2:
				this.SetHeldGrenadePos(6f, 19f);
				this.ReleaseGrenade(true);
				break;
			case 3:
				this.throwingGrenade = false;
				break;
			}
		}
		else
		{
			int num2 = 17 + Mathf.Clamp(this.frame, 0, 7);
			this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)(this.spritePixelHeight * 5));
			switch (this.frame)
			{
			case 0:
				this.SetHeldGrenadePos(0f, 8f);
				break;
			case 1:
				this.SetHeldGrenadePos(-8f, 7f);
				break;
			case 2:
				this.SetHeldGrenadePos(-8f, 15f);
				break;
			case 3:
				this.SetHeldGrenadePos(-3f, 20f);
				break;
			case 4:
				this.SetHeldGrenadePos(7f, 18f);
				this.ReleaseGrenade(true);
				break;
			case 7:
				this.throwingGrenade = false;
				break;
			}
		}
	}

	protected void ReleaseGrenade(bool thrown)
	{
		if (this.heldGrenade != null)
		{
			float num;
			float num2;
			if (thrown)
			{
				num = Mathf.Sign(base.transform.localScale.x) * 250f / this.heldGrenade.weight;
				num2 = 110f + 40f / this.heldGrenade.weight;
			}
			else
			{
				num = -this.xI * 2f / this.heldGrenade.weight;
				num2 = 80f;
			}
			if (base.IsMine)
			{
				Networking.RPC<Grenade, float, float, float, float>(PID.TargetOthers, new RpcSignature<Grenade, float, float, float, float>(this.ReleaseGrenadeRPC), this.heldGrenade, num, num2, this.heldGrenade.transform.position.x, this.heldGrenade.transform.position.y, false);
			}
			this.ReleaseGrenadeRPC(this.heldGrenade, num, num2, this.heldGrenade.transform.position.x, this.heldGrenade.transform.position.y);
		}
	}

	protected void ReleaseGrenadeRPC(Grenade GrenadeToRelease, float XI, float YI, float X, float Y)
	{
		if (this.heldGrenade != null && this.throwingGrenade != this.heldGrenade)
		{
			this.ThrowGrenade(this.heldGrenade, XI, YI, X, Y);
		}
		this.ThrowGrenade(GrenadeToRelease, XI, YI, X, Y);
		this.heldGrenade = null;
	}

	protected void ThrowGrenade(Grenade GrenadeToThrow, float XI, float YI, float X, float Y)
	{
		if (GrenadeToThrow != null)
		{
			GrenadeToThrow.enabled = true;
			GrenadeToThrow.transform.parent = null;
			GrenadeToThrow.x = X;
			GrenadeToThrow.y = Y;
			GrenadeToThrow.xI = XI;
			GrenadeToThrow.yI = YI;
			GrenadeToThrow.transform.localScale = Vector3.one;
			GrenadeToThrow.ResetTrail();
			GrenadeToThrow.RunUpdate();
			GrenadeToThrow.SetMinLife(0.7f);
			GrenadeToThrow.playerNum = this.playerNum;
		}
	}

	protected virtual void SetHeldGrenadePos(float xOffset, float yOffset)
	{
		if (this.heldGrenade != null)
		{
			this.heldGrenade.transform.localPosition = new Vector3(xOffset, yOffset, -1f);
		}
	}

	protected virtual void AnimateSpecial()
	{
		if (!this.useNewThrowingFrames)
		{
			this.SetSpriteOffset(0f, 0f);
			this.DeactivateGun();
			this.frameRate = 0.0334f;
			int num = 16 + Mathf.Clamp(this.frame, 0, 4);
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)this.spritePixelHeight);
			if (this.frame == 2)
			{
				this.UseSpecial();
			}
			if (this.frame >= 4)
			{
				this.frame = 0;
				this.ActivateGun();
				this.usingSpecial = false;
			}
		}
		else
		{
			this.SetSpriteOffset(0f, 0f);
			this.DeactivateGun();
			this.frameRate = 0.0334f;
			int num2 = 17 + Mathf.Clamp(this.frame, 0, 7);
			this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)(this.spritePixelHeight * 5));
			if (this.frame == 4)
			{
				this.UseSpecial();
			}
			if (this.frame >= 7)
			{
				this.frame = 0;
				this.usingSpecial = false;
				this.ActivateGun();
				this.ChangeFrame();
			}
		}
	}

	protected virtual void AnimateSpecial2()
	{
	}

	protected virtual void AnimateSpecial3()
	{
	}

	protected virtual void AnimateSpecial4()
	{
	}

	protected virtual void AnimateHighFiveHold()
	{
		this.SetSpriteOffset(0f, 0f);
		this.DeactivateGun();
		this.frameRate = 0.0334f;
		int num = 16 + Mathf.Clamp(this.frame, 0, 2);
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)this.spritePixelHeight);
		this.highfiveHoldTime += this.t;
		if (this.highfiveHoldTime > 0.5f)
		{
			this.highfiveHoldTime = -1.5f;
			this.high5Bubble.RestartBubble();
		}
		if (this.frame >= 2)
		{
			this.frame = 2;
			this.CheckHighFive();
		}
	}

	protected virtual void AnimateHighFiveRelease()
	{
		this.SetSpriteOffset(0f, 0f);
		this.DeactivateGun();
		this.frameRate = 0.0667f;
		int num = 16 + Mathf.Clamp(this.frame, 2, 4);
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)this.spritePixelHeight);
		this.CheckHighFive();
		if (this.frame >= 4)
		{
			this.frame = 0;
			this.ActivateGun();
			this.releasingHighFive = false;
		}
	}

	protected virtual void CheckHighFive()
	{
		if (!this.highFiveBoost && Map.CheckHighFive(this.playerNum, this.x + base.transform.localScale.x * 8f, this.y, 9f, 4f))
		{
			if (base.IsMine)
			{
				Networking.RPC(PID.TargetAll, new RpcSignature(this.HighFiveBoost), false);
			}
			this.ReleaseHighFive();
		}
	}

	public override bool IsHighFiving()
	{
		if (this.holdingHighFive || this.releasingHighFive)
		{
			this.holdingHighFive = false;
			this.ReleaseHighFive();
			if (base.IsMine)
			{
				HeroController.HighFiveBoostNetworked(this.playerNum);
			}
			return true;
		}
		return false;
	}

	public virtual void HighFiveBoost()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		SortOfFollow.Shake(0.35f, base.transform.position);
		this.PlayHighFiveSound();
		this.PlayBassDropSoundSound();
		this.highFiveBoost = true;
		this.highFiveBoostTime = 3.4f;
		HeroController.HighFiveBoost(3.4f);
		EffectsController.CreateBulletPoofEffect(this.x + base.transform.localScale.x * 8f, this.y + 11f);
	}

	public virtual void Boost(float time)
	{
		this.highFiveBoost = true;
		this.highFiveBoostTime = time;
	}

	public virtual void TimeBroBoost(float time)
	{
		this.timeBroBoostTime = time;
	}

	protected virtual void AnimateHanging()
	{
		this.SetSpriteOffset(0f, -2f);
		if (this.right || this.left)
		{
			this.DeactivateGun();
			this.frameRate = 0.0667f;
			int num = 11 + this.frame % 12;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)((int)(4f * this.sprite.pixelDimensions.y)));
			this.wasHangingMoving = true;
		}
		else
		{
			if (this.wasHangingMoving)
			{
				this.frame = 0;
				this.wasHangingMoving = false;
			}
			this.frameRate = 0.045f;
			int num2 = 23 + Mathf.Clamp(this.frame, 0, 5);
			this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)((int)(4f * this.sprite.pixelDimensions.y)));
			this.ActivateGun();
		}
	}

	protected virtual void AnimateAirdash()
	{
		switch (this.airdashDirection)
		{
		case DirectionEnum.Up:
			this.DeactivateGun();
			this.sprite.SetLowerLeftPixel((float)((10 + this.frame % 3) * this.spritePixelWidth), (float)(this.spritePixelHeight * 8));
			break;
		case DirectionEnum.Down:
			if (this.yI > -50f)
			{
				this.AnimateJumping();
			}
			else
			{
				this.DeactivateGun();
				this.sprite.SetLowerLeftPixel((float)((3 + this.frame % 3) * this.spritePixelWidth), (float)(this.spritePixelHeight * 8));
			}
			break;
		case DirectionEnum.Left:
		case DirectionEnum.Right:
			if (this.airDashDelay > 0f)
			{
				this.sprite.SetLowerLeftPixel((float)((13 + this.frame % 3) * this.spritePixelWidth), (float)(this.spritePixelHeight * 8));
			}
			else
			{
				this.sprite.SetLowerLeftPixel((float)((0 + this.frame % 3) * this.spritePixelWidth), (float)(this.spritePixelHeight * 8));
			}
			break;
		default:
			this.StopAirDashing();
			break;
		}
	}

	protected virtual void AnimateJumping()
	{
		this.rollingFrames = 0;
		this.SetSpriteOffset(0f, 0f);
		this.frameRate = 0.0667f;
		if (this.attachedToZipline != null)
		{
			this.AnimateZipline();
		}
		else if (this.chimneyFlip)
		{
			this.AnimateChimneyFlip();
		}
		else if (!this.ducking)
		{
			if (this.doingMelee && !this.canDoIndependentMeleeAnimation)
			{
				this.DeactivateGun();
				this.AnimateMelee();
			}
			else if (this.releasingHighFive)
			{
				this.AnimateHighFiveRelease();
			}
			else if (this.holdingHighFive)
			{
				this.AnimateHighFiveHold();
			}
			else if (this.useNewLedgeGrappleFrames && !this.fire && this.ledgeGrapple)
			{
				this.AnimateLedgeGrapple();
			}
			else
			{
				this.AnimateActualJumpingFrames();
			}
		}
		else
		{
			this.AnimateActualJumpingDuckingFrames();
		}
	}

	protected virtual void AnimateActualJumpingDuckingFrames()
	{
		this.SetGunPosition(2f, -1f);
		if (this.yI > 20f)
		{
			int num = 8;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)this.spritePixelHeight);
		}
		else if (this.yI < -10f)
		{
			int num2 = 9;
			this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)this.spritePixelHeight);
		}
		else
		{
			int num3 = 9;
			this.sprite.SetLowerLeftPixel((float)(num3 * this.spritePixelWidth), (float)this.spritePixelHeight);
		}
	}

	protected virtual void AnimateLedgeGrapple()
	{
		this.frameRate = 0.01f;
		this.DeactivateGun();
		int num = (int)(13f - this.ledgeOffsetY);
		if (num <= 13 && num >= 0)
		{
			this.sprite.SetLowerLeftPixel((float)((12 + num) * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
		}
		else
		{
			this.AnimateActualJumpingFrames();
		}
	}

	public override void AttachToZipline(ZipLine zipLine)
	{
		base.AttachToZipline(zipLine);
		this.frame = 0;
		this.SetGunSprite(this.gunFrame, 0);
	}

	protected virtual void AnimateZipline()
	{
		if (!this.slidingOnZipline && ((this.left && (this.attachedToZipline.Direction.x > 0f || this.attachedToZipline.IsHorizontalZipline)) || (this.right && (this.attachedToZipline.Direction.x < 0f || this.attachedToZipline.IsHorizontalZipline))))
		{
			this.DeactivateGun();
			int num = 11 + this.frame % 12;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 4));
		}
		else
		{
			this.ActivateGun();
			this.frameRate = 0.075f;
			int num2 = 23 + Mathf.Clamp(this.frame, 0, 5);
			this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)(this.spritePixelHeight * 4));
			if (!this.attachedToZipline.IsHorizontalZipline)
			{
				EffectsController.CreateSuddenSparkShower(this.x + base.transform.localScale.x * 5f, this.y + 18f, 4, 1f, 80f, base.transform.localScale.x * 60f, this.yI, 0.2f);
			}
		}
	}

	protected virtual void AnimateChimneyFlip()
	{
		this.DeactivateGun();
		this.frameRate = 0.033f;
		if (this.chimneyFlipFrames <= 0)
		{
			this.chimneyFlip = false;
			this.chimneyFlipFrames = 0;
		}
		int num = 22 - this.chimneyFlipFrames;
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 7));
	}

	protected virtual void AnimateActualJumpingFrames()
	{
		this.ActivateGun();
		this.SetGunPosition(0f, 0f);
		if (this.gunFrame <= 0 && !this.doingMelee)
		{
			this.SetGunSprite(0, 0);
		}
		if (this.yI > 20f)
		{
			if (this.useNewFrames)
			{
				if (Mathf.Abs(this.xI) > 50f)
				{
					this.sprite.SetLowerLeftPixel((float)(this.frame % 3 * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
				}
				else
				{
					this.sprite.SetLowerLeftPixel((float)((6 + this.frame % 3) * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
				}
			}
			else
			{
				int num = 1;
				this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)this.spritePixelHeight);
			}
			this.SetGunPosition(0f, 0f);
		}
		else if (this.yI < -55f)
		{
			if (this.useNewFrames)
			{
				if (Mathf.Abs(this.xI) > 50f)
				{
					this.sprite.SetLowerLeftPixel((float)((3 + this.frame % 3) * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
				}
				else
				{
					this.sprite.SetLowerLeftPixel((float)((9 + this.frame % 3) * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
				}
			}
			else
			{
				int num2 = 2;
				this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)this.spritePixelHeight);
			}
			this.SetGunPosition(0f, 2f);
		}
		else
		{
			if (this.useNewFrames)
			{
				if (Mathf.Abs(this.xI) > 50f)
				{
					this.sprite.SetLowerLeftPixel((float)((3 + this.frame % 3) * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
				}
				else
				{
					this.sprite.SetLowerLeftPixel((float)((9 + this.frame % 3) * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
				}
			}
			else
			{
				int num3 = 2;
				this.sprite.SetLowerLeftPixel((float)(num3 * this.spritePixelWidth), (float)this.spritePixelHeight);
			}
			this.SetGunPosition(0f, 1f);
		}
	}

	protected virtual void SetGunPosition(float xOffset, float yOffset)
	{
		this.gunSprite.transform.localPosition = new Vector3(xOffset, yOffset, -0.001f);
		this.gunSprite.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	protected virtual void AnimateRolling()
	{
		this.frameRate = 0.025f;
		this.sprite.SetLowerLeftPixel((float)((19 + (13 - this.rollingFrames)) * this.spritePixelWidth), (float)(this.spritePixelHeight * 2));
		this.DeactivateGun();
	}

	protected virtual void AnimateImpaled()
	{
		this.DeactivateGun();
		this.SetSpriteOffset(0f, 0f);
		int num = 10 + Mathf.Clamp(this.frame, 0, 2);
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 2));
	}

	protected virtual void AnimateIdle()
	{
		this.SetSpriteOffset(0f, 0f);
		if (this.throwingGrenade)
		{
			this.AnimateThrowingGrenade();
		}
		else if (!this.ducking)
		{
			if (this.doingMelee && !this.canDoIndependentMeleeAnimation)
			{
				this.DeactivateGun();
				this.AnimateMelee();
			}
			else if (this.holdingHighFive)
			{
				this.AnimateHighFiveHold();
			}
			else if (this.releasingHighFive)
			{
				this.AnimateHighFiveRelease();
			}
			else if (this.usingSpecial)
			{
				this.AnimateSpecial();
			}
			else if (this.health > 0 && this.usingSpecial2)
			{
				this.AnimateSpecial2();
			}
			else if (this.usingSpecial3)
			{
				this.AnimateSpecial3();
			}
			else if (this.usingSpecial4)
			{
				this.AnimateSpecial4();
			}
			else if (this.rollingFrames > 0)
			{
				this.AnimateRolling();
			}
			else
			{
				this.AnimateActualIdleFrames();
			}
		}
		else if (this.doingMelee && !this.canDoIndependentMeleeAnimation)
		{
			this.DeactivateGun();
			this.AnimateMelee();
		}
		else if (this.usingSpecial)
		{
			this.AnimateSpecial();
		}
		else if (this.usingSpecial2)
		{
			this.AnimateSpecial2();
		}
		else if (this.usingSpecial3)
		{
			this.AnimateSpecial3();
		}
		else if (this.usingSpecial4)
		{
			this.AnimateSpecial4();
		}
		else if (this.rollingFrames > 0)
		{
			this.AnimateRolling();
		}
		else
		{
			this.AnimateActualIdleDuckingFrames();
		}
	}

	public virtual void AnimateActualIdleFrames()
	{
		this.ActivateGun();
		this.SetGunPosition(0f, 0f);
		this.frameRate = 0.0667f;
		int num = 0 + this.frame % 1;
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)this.spritePixelHeight);
		if (this.gunFrame <= 0 && !this.doingMelee)
		{
			this.SetGunSprite(0, 0);
		}
	}

	public void AnimateActualIdleDuckingFrames()
	{
		this.ActivateGun();
		this.SetGunPosition(2f, -1f);
		this.frameRate = 0.0667f;
		int num = 6 + this.frame % 1;
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)this.spritePixelHeight);
	}

	protected virtual void AnimateRunning()
	{
		this.SetSpriteOffset(0f, 0f);
		if (this.throwingGrenade)
		{
			this.AnimateThrowingGrenade();
		}
		else if (this.doingMelee && !this.canDoIndependentMeleeAnimation)
		{
			this.DeactivateGun();
			this.AnimateMelee();
		}
		else if (this.releasingHighFive)
		{
			this.AnimateHighFiveRelease();
		}
		else if (this.usingSpecial)
		{
			this.AnimateSpecial();
		}
		else if (this.usingSpecial2)
		{
			this.AnimateSpecial2();
		}
		else if (this.usingSpecial3)
		{
			this.AnimateSpecial3();
		}
		else if (this.usingSpecial4)
		{
			this.AnimateSpecial4();
		}
		else if (this.pushingTime > 0f && this.useNewPushingFrames && !this.fire)
		{
			this.AnimatePushing();
		}
		else if (this.rollingTime > 0f)
		{
			this.frameRate = 0.0334f;
			int num = 16 + Mathf.Clamp(this.frame, 0, 10000) % 8;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)this.spritePixelHeight);
		}
		else if (this.rollingFrames > 0)
		{
			this.AnimateRolling();
		}
		else
		{
			this.ActivateGun();
			if (!this.ducking)
			{
				this.SetGunPosition((float)(0 + ((!this.useDashFrames || !this.dashing) ? 0 : 1)), 0f);
				if (this.useNewFrames)
				{
					this.AnimateActualNewRunningFrames();
				}
				else
				{
					this.frameRate = 0.045f;
					int num2 = 0 + this.frame % 4;
					if (this.frame % 2 == 0 && !FluidController.IsSubmerged(this))
					{
						EffectsController.CreateFootPoofEffect(this.x, this.y + 2f, 0f, Vector3.up * 1f - Vector3.right * base.transform.localScale.x * 60.5f);
					}
					if (this.frame % 2 == 0)
					{
						this.PlayFootStepSound();
					}
					this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)this.spritePixelHeight);
				}
			}
			else
			{
				this.SetGunPosition(2f, -1f);
				if (this.useNewDuckingFrames)
				{
					this.frameRate = 0.025f;
					int num3 = 8 + this.frame % 8;
					if (this.frame % 4 == 0)
					{
						if (!FluidController.IsSubmerged(this))
						{
							EffectsController.CreateFootPoofEffect(this.x, this.y + 2f, 0f, Vector3.up * 1f - Vector3.right * base.transform.localScale.x * 60.5f);
						}
						this.PlayFootStepSound();
					}
					this.sprite.SetLowerLeftPixel((float)(num3 * this.spritePixelWidth), (float)(this.spritePixelHeight * 2));
					if (this.gunFrame <= 0 && !this.doingMelee)
					{
						this.SetGunSprite(num3, 1);
					}
				}
				else
				{
					this.frameRate = 0.033f;
					int num4 = 6 + this.frame % 4;
					this.sprite.SetLowerLeftPixel((float)(num4 * this.spritePixelWidth), (float)this.spritePixelHeight);
					if (this.frame % 2 == 0 && !FluidController.IsSubmerged(this))
					{
						EffectsController.CreateFootPoofEffect(this.x, this.y + 2f, 0f, Vector3.up * 1f - Vector3.right * base.transform.localScale.x * 60.5f);
					}
				}
			}
		}
	}

	protected virtual void AnimateActualNewRunningFrames()
	{
		this.frameRate = this.runningFrameRate;
		int num = 0 + Mathf.Clamp(this.frame, 0, 10000) % 8;
		if (this.frame % 4 == 0 && !FluidController.IsSubmerged(this))
		{
			EffectsController.CreateFootPoofEffect(this.x, this.y + 2f, 0f, Vector3.up * 1f - Vector3.right * base.transform.localScale.x * 60.5f);
		}
		if (this.frame % 4 == 0 && !this.ledgeGrapple)
		{
			this.PlayFootStepSound();
		}
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)((!this.dashing || !this.useDashFrames) ? (this.spritePixelHeight * 2) : (this.spritePixelHeight * 4)));
		if (this.gunFrame <= 0 && !this.doingMelee)
		{
			this.SetGunSprite(num, 1);
		}
	}

	protected virtual void AnimatePushing()
	{
		this.frameRate = this.runningFrameRate * 3f;
		int num = 0 + this.frame % 8;
		if (this.frame % 4 == 0)
		{
			this.PlayFootStepSound();
		}
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 5));
		if (this.gunFrame <= 0 && !this.doingMelee)
		{
			this.SetGunSprite(num, 1);
		}
		this.SetGunPosition(0f, 0f);
		this.gunSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
	}

	protected virtual void PlayKnifeClimbSound()
	{
		this.knifeSoundCount++;
		if (this.sound != null && this.soundHolderFootSteps != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolderFootSteps.knifeWallSounds[this.knifeSoundCount % this.soundHolderFootSteps.knifeWallSounds.Length], 0.2f, base.transform.position);
		}
	}

	protected virtual void PlayFootStepSound(AudioClip[] clips, float v, float p)
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.footstepDelay <= 0f)
		{
			if (this.playerNum >= 0)
			{
				this.sound.PlaySoundEffectAt(clips, v, base.transform.position, p);
			}
			else if (SetResolutionCamera.IsItVisible(base.transform.position))
			{
				this.sound.PlaySoundEffectAt(clips, v * 0.5f, base.transform.position, p);
			}
		}
	}

	protected virtual void PlayLandSound()
	{
		if (this.soundHolderFootSteps != null && this.currentFootStepGroundType.Length > 1)
		{
			string text = this.currentFootStepGroundType;
			switch (text)
			{
			case "Dirt":
				this.PlayFootStepSound(this.soundHolderFootSteps.landDirtSounds, 0.6f, 1f);
				break;
			case "Stone":
				this.PlayFootStepSound(this.soundHolderFootSteps.landConcreteSounds, 0.6f, 1f);
				break;
			case "Metal":
				this.PlayFootStepSound(this.soundHolderFootSteps.landMetalSounds, 0.6f, 1f);
				break;
			case "Wood":
				this.PlayFootStepSound(this.soundHolderFootSteps.landBridgeSounds, 0.6f, 1f);
				break;
			case "Grass":
				this.PlayFootStepSound(this.soundHolderFootSteps.landGrassSounds, 0.6f, 1f);
				break;
			case "Slime":
				this.PlayFootStepSound(this.soundHolderFootSteps.landGunkSounds, 0.6f, 1f);
				break;
			}
		}
	}

	protected virtual void PlayJumpSound()
	{
		if (this.soundHolderFootSteps != null && this.currentFootStepGroundType.Length > 1)
		{
			string text = this.currentFootStepGroundType;
			switch (text)
			{
			case "Dirt":
				this.PlayFootStepSound(this.soundHolderFootSteps.jumpDirtSounds, 0.65f, 1f);
				break;
			case "Stone":
				this.PlayFootStepSound(this.soundHolderFootSteps.jumpConcreteSounds, 0.65f, 1f);
				break;
			case "Metal":
				this.PlayFootStepSound(this.soundHolderFootSteps.jumpMetalSounds, 0.65f, 1f);
				break;
			case "Wood":
				this.PlayFootStepSound(this.soundHolderFootSteps.jumpBridgeSounds, 0.65f, 1f);
				break;
			case "Grass":
				this.PlayFootStepSound(this.soundHolderFootSteps.jumpGrassSounds, 0.65f, 1f);
				break;
			case "Slime":
				this.PlayFootStepSound(this.soundHolderFootSteps.jumpGunkSounds, 0.65f, 1f);
				break;
			}
		}
	}

	protected virtual void PlayFootStepSound()
	{
		this.PlayFootStepSound(0.4f, 1f);
	}

	protected virtual void PlayFootStepSound(float v, float p)
	{
		if (this.soundHolderFootSteps != null && this.currentFootStepGroundType.Length > 1)
		{
			string text = this.currentFootStepGroundType;
			switch (text)
			{
			case "Dirt":
				this.PlayFootStepSound(this.soundHolderFootSteps.footStepDirtSounds, v, p);
				break;
			case "Stone":
				this.PlayFootStepSound(this.soundHolderFootSteps.footStepConcreteSounds, v, p);
				break;
			case "Metal":
				this.PlayFootStepSound(this.soundHolderFootSteps.footStepMetalSounds, v, p);
				break;
			case "Wood":
				this.PlayFootStepSound(this.soundHolderFootSteps.footStepBridgeSounds, v, p);
				break;
			case "Grass":
				this.PlayFootStepSound(this.soundHolderFootSteps.footStepGrassSounds, v, p);
				break;
			case "Slime":
				this.PlayFootStepSound(this.soundHolderFootSteps.footStepGunkSounds, v * 0.7f, p);
				break;
			}
		}
	}

	protected virtual void IncreaseFrame()
	{
		this.frame++;
	}

	protected virtual void ChangeFrame()
	{
		if (this.chimneyFlipFrames > 0)
		{
			this.chimneyFlipFrames--;
		}
		if (this.rollingFrames > 0)
		{
			this.rollingFrames--;
		}
		this.SetSpriteOffset(0f, 0f);
		this.hangingOneArmed = false;
		if (this.health > 0 && !this.usingSpecial && !this.doingMelee)
		{
			this.ActivateGun();
		}
		if (this.impaledTransform != null && this.useImpaledFrames)
		{
			this.AnimateImpaled();
		}
		else if (this.actionState == ActionState.Idle)
		{
			this.AnimateIdle();
		}
		else if (this.actionState == ActionState.Running)
		{
			this.AnimateRunning();
		}
		else if (this.actionState == ActionState.Panicking)
		{
			this.frameRate = 0.044455f;
			int num = 15 + this.frame % 4;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)this.spritePixelHeight);
		}
		else if (this.actionState == ActionState.ClimbingLadder)
		{
			if (this.throwingGrenade)
			{
				this.AnimateThrowingGrenade();
			}
			else if (this.doingMelee && !this.canDoIndependentMeleeAnimation)
			{
				this.AnimateMelee();
			}
			else if (this.usingSpecial)
			{
				this.AnimateSpecial();
			}
			else if (this.usingSpecial2)
			{
				this.AnimateSpecial2();
			}
			else if (this.usingSpecial3)
			{
				this.AnimateSpecial3();
			}
			else if (this.usingSpecial4)
			{
				this.AnimateSpecial4();
			}
			else
			{
				this.AnimateClimbingLadder();
			}
		}
		else if (this.actionState == ActionState.Hanging)
		{
			this.AnimateHanging();
		}
		else if (this.actionState == ActionState.Melee && !this.canDoIndependentMeleeAnimation)
		{
			this.AnimateMelee();
		}
		else if (this.actionState == ActionState.Jumping)
		{
			if (this.throwingGrenade)
			{
				this.AnimateThrowingGrenade();
			}
			else if (this.usingSpecial)
			{
				this.AnimateSpecial();
			}
			else if (this.usingSpecial2)
			{
				this.AnimateSpecial2();
			}
			else if (this.usingSpecial3)
			{
				this.AnimateSpecial3();
			}
			else if (this.usingSpecial4)
			{
				this.AnimateSpecial4();
			}
			else if (this.ledgeGrapple)
			{
				this.AnimateRunning();
			}
			else if (this.chimneyFlip)
			{
				this.AnimateChimneyFlip();
			}
			else if (this.wallClimbing && !this.fire)
			{
				this.RunStepOnWalls();
				this.AnimateWallClimb();
			}
			else if (this.WallDrag)
			{
				this.RunStepOnWalls();
				this.AnimateWallDrag();
			}
			else if (!this.fire && this.useNewKnifeClimbingFrames && (this.buttonJump || (this.yI > -30f && Time.time - this.lastButtonJumpTime < 1f)) && this.jumpTime <= 0f && ((this.left && this.canTouchLeftWalls) || (this.right && this.canTouchRightWalls)))
			{
				this.AnimateWallAnticipation();
			}
			else if (this.rollingTime > 0f)
			{
				this.frameRate = 0.0334f;
				int num2 = 16 + this.frame % 8;
				this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)this.spritePixelHeight);
			}
			else if (this.canAirdash && this.airdashTime > 0f)
			{
				this.AnimateAirdash();
			}
			else
			{
				this.AnimateJumping();
			}
		}
		else if (this.actionState == ActionState.Dead)
		{
			this.AnimateDeath();
		}
		else if (this.actionState == ActionState.Fallen)
		{
			this.AnimateFallen();
		}
	}

	protected virtual void AnimateClimbingLadderTransition(bool intoClimbing)
	{
		this.DeactivateGun();
		int num = 0 + Mathf.Clamp(this.ladderClimbingTransitionFrames, 0, 5);
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 7));
		this.frame = 0;
		if (intoClimbing)
		{
			this.frameRate = 0.025f;
			this.ladderClimbingTransitionFrames++;
		}
		else
		{
			this.frameRate = 0.033f;
			this.ladderClimbingTransitionFrames--;
		}
		if (this.ladderClimbingTransitionFrames > 5)
		{
			this.ladderClimbingTransitionFrames = 5;
		}
		if (this.ladderClimbingTransitionFrames < 0)
		{
			this.ladderClimbingTransitionFrames = 0;
		}
	}

	protected virtual void AnimateClimbingLadder()
	{
		if (!this.IsNearbyLadder(base.transform.localScale.x * 6f, 22f))
		{
			if (this.useLadderClimbingTransition && this.ladderClimbingTransitionFrames > 0)
			{
				this.AnimateClimbingLadderTransition(false);
			}
			else if (!this.up && !this.down && !this.left && !this.right)
			{
				this.AnimateActualIdleFrames();
			}
			else
			{
				this.AnimateActualNewRunningFrames();
			}
		}
		else if (!this.up && !this.down && !this.left && !this.right)
		{
			if (this.useLadderClimbingTransition && this.ladderClimbingTransitionFrames > 0)
			{
				this.AnimateClimbingLadderTransition(false);
			}
			else if (this.useNewLadderClimbingFrames)
			{
				this.hangingOneArmed = true;
				this.ActivateGun();
				this.SetGunPosition(0f, 0f);
				int num = 11 + this.frame % 3;
				this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 6));
				if (this.gunFrame <= 0 && !this.fire && !this.doingMelee)
				{
					this.SetGunSprite(0, 0);
				}
			}
			else
			{
				this.ActivateGun();
				this.SetGunPosition(0f, 0f);
				this.sprite.SetLowerLeftPixel((float)(0 * this.spritePixelWidth), (float)(this.spritePixelHeight * 1));
			}
		}
		else if (!this.useNewLadderClimbingFrames || this.fire || (this.right && !this.IsWallInFront(this.halfWidth + 3f)) || (this.left && !this.IsWallInFront(this.halfWidth + 3f)))
		{
			if (this.useLadderClimbingTransition && this.ladderClimbingTransitionFrames > 0)
			{
				this.AnimateClimbingLadderTransition(false);
			}
			else
			{
				this.ActivateGun();
				this.SetGunPosition(0f, 0f);
				this.SetSpriteOffset(0f, 0f);
				this.frameRate = 0.044455f;
				int num2 = 0 + this.frame % 4;
				this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)this.spritePixelHeight);
				if (this.gunFrame <= 0 && !this.fire && !this.doingMelee)
				{
					this.SetGunSprite(0, 0);
				}
			}
		}
		else if (this.down)
		{
			if (this.useLadderClimbingTransition && this.ladderClimbingTransitionFrames < 5)
			{
				this.AnimateClimbingLadderTransition(true);
			}
			else
			{
				this.DeactivateGun();
				this.frameRate = 0.033f;
				this.SetSpriteOffset(0f, 0f);
				if (this.yI < -240f && Physics.OverlapSphere(base.transform.position + Vector3.up * 12f, 0.1f, this.ladderLayer).Length > 0)
				{
					EffectsController.CreateFootPoofEffect(this.x + base.transform.localScale.x * 5f, this.y + 18f, 0f, Vector3.zero);
					EffectsController.CreateFootPoofEffect(this.x - base.transform.localScale.x * 5f, this.y + 18f, 0f, Vector3.zero);
				}
				int num3 = 8 + this.frame % 3;
				this.sprite.SetLowerLeftPixel((float)(num3 * this.spritePixelWidth), (float)(this.spritePixelHeight * 6));
			}
		}
		else if (this.useLadderClimbingTransition && this.ladderClimbingTransitionFrames < 5)
		{
			this.AnimateClimbingLadderTransition(true);
		}
		else
		{
			this.DeactivateGun();
			this.frameRate = 0.02f;
			this.SetSpriteOffset(0f, 0f);
			int num4 = 0 + this.frame % 8;
			this.sprite.SetLowerLeftPixel((float)(num4 * this.spritePixelWidth), (float)(this.spritePixelHeight * 6));
		}
	}

	protected virtual void AnimateWallDrag()
	{
		this.wallClimbAnticipation = false;
		if (this.useNewKnifeClimbingFrames)
		{
			if (this.yI > 100f)
			{
				if (this.knifeHand % 2 == 0)
				{
					if (this.frame > 1)
					{
						this.frame = 1;
					}
					int num = 12 + Mathf.Clamp(this.frame, 0, 1);
					this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
				}
				else
				{
					if (this.frame > 1)
					{
						this.frame = 1;
					}
					int num2 = 22 + Mathf.Clamp(this.frame, 0, 1);
					this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
				}
			}
			else
			{
				if (this.frame == 2)
				{
					this.PlayKnifeClimbSound();
				}
				if (this.knifeHand % 2 == 0)
				{
					int num3 = 12 + Mathf.Clamp(this.frame, 0, 4);
					if (!FluidController.IsSubmerged(this))
					{
						EffectsController.CreateFootPoofEffect(this.x + base.transform.localScale.x * 6f, this.y + 16f, 0f, Vector3.up * 1f);
					}
					this.sprite.SetLowerLeftPixel((float)(num3 * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
				}
				else
				{
					int num4 = 22 + Mathf.Clamp(this.frame, 0, 4);
					if (!FluidController.IsSubmerged(this))
					{
						EffectsController.CreateFootPoofEffect(this.x + base.transform.localScale.x * 6f, this.y + 16f, 0f, Vector3.up * 1f);
					}
					this.sprite.SetLowerLeftPixel((float)(num4 * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
				}
			}
		}
		else if (this.knifeHand % 2 == 0)
		{
			int num5 = 11 + Mathf.Clamp(this.frame, 0, 2);
			if (!FluidController.IsSubmerged(this))
			{
				EffectsController.CreateFootPoofEffect(this.x + base.transform.localScale.x * 6f, this.y + 12f, 0f, Vector3.up * 1f);
			}
			this.sprite.SetLowerLeftPixel((float)(num5 * this.spritePixelWidth), (float)this.spritePixelHeight);
		}
		else
		{
			int num6 = 14 + Mathf.Clamp(this.frame, 0, 2);
			if (!FluidController.IsSubmerged(this))
			{
				EffectsController.CreateFootPoofEffect(this.x + base.transform.localScale.x * 6f, this.y + 12f, 0f, Vector3.up * 1f);
			}
			this.sprite.SetLowerLeftPixel((float)(num6 * this.spritePixelWidth), (float)this.spritePixelHeight);
		}
	}

	protected virtual void ApplyWallClimbingGravity()
	{
		if (!this.useNewKnifeClimbingFrames || !this.WallDrag)
		{
			this.ApplyFallingGravity();
		}
		else if (this.frame <= 1)
		{
			this.ApplyFallingGravity();
		}
		else if (this.frame <= 7)
		{
			this.yI -= 1100f * this.t;
			if (this.yI < 0f)
			{
				this.yI = 0f;
			}
		}
		else if (this.wallClimbing)
		{
			this.yI -= 1100f * this.t * 0.66f;
			if (this.yI < 30f)
			{
				this.yI = 30f;
			}
		}
		else
		{
			this.ApplyFallingGravity();
		}
	}

	protected virtual void AnimateWallAnticipation()
	{
		this.DeactivateGun();
		this.wallClimbAnticipation = true;
		this.frameRate = 0.025f;
		if (this.yI > -50f && this.frame > 1)
		{
			this.frame = 1;
		}
		float num = this.y + this.knifeClimbStabHeight - this.lastKnifeClimbStabY;
		if (num < 16f && num > -1f)
		{
			int value = (int)Mathf.Clamp(num / 3f, 0f, 5f);
			if (this.knifeHand % 2 == 0)
			{
				int num2 = 16 + Mathf.Clamp(value, 0, 5);
				this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
			}
			else
			{
				int num3 = 26 + Mathf.Clamp(value, 0, 5);
				this.sprite.SetLowerLeftPixel((float)(num3 * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
			}
			this.frame = -1;
			this.armUpInAnticipationWallClimb = false;
		}
		else
		{
			if (!this.armUpInAnticipationWallClimb)
			{
				this.armUpInAnticipationWallClimb = true;
				this.knifeHand++;
			}
			if (this.knifeHand % 2 == 0)
			{
				int num4 = 12 + Mathf.Clamp(this.frame, 0, 1);
				this.sprite.SetLowerLeftPixel((float)(num4 * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
			}
			else
			{
				int num5 = 22 + Mathf.Clamp(this.frame, 0, 1);
				this.sprite.SetLowerLeftPixel((float)(num5 * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
			}
		}
	}

	protected virtual void AnimateWallClimb()
	{
		this.wallClimbAnticipation = false;
		if (this.useNewKnifeClimbingFrames)
		{
			this.DeactivateGun();
			if (this.frame == 8)
			{
				this.yI = 190f;
				this.frameRate = 0.02f;
				this.lastKnifeClimbStabY = this.y + this.knifeClimbStabHeight;
			}
			if (this.frame < 8)
			{
				if (this.knifeHand % 2 == 0)
				{
					int num = 12 + Mathf.Clamp(this.frame, 0, 4);
					this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
				}
				else
				{
					int num2 = 22 + Mathf.Clamp(this.frame, 0, 4);
					this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
				}
				if (this.frame == 2)
				{
					this.PlayKnifeClimbSound();
				}
				if (this.frame == 1)
				{
					this.frameRate = 0.0222f;
					if (this.yI > 16f)
					{
						this.frame--;
					}
				}
				else
				{
					this.frameRate = 0.033f;
				}
			}
			else
			{
				float num3 = this.y + this.knifeClimbStabHeight - this.lastKnifeClimbStabY;
				int value = (int)Mathf.Clamp(num3 / 2f, 0f, 5f);
				if (this.knifeHand % 2 == 0)
				{
					int num4 = 16 + Mathf.Clamp(value, 0, 5);
					this.sprite.SetLowerLeftPixel((float)(num4 * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
				}
				else
				{
					int num5 = 26 + Mathf.Clamp(value, 0, 5);
					this.sprite.SetLowerLeftPixel((float)(num5 * this.spritePixelWidth), (float)(this.spritePixelHeight * 3));
				}
				if (this.y + this.knifeClimbStabHeight - this.lastKnifeClimbStabY > 12f)
				{
					this.frame = -1;
					this.knifeHand++;
				}
			}
		}
		else if (this.knifeHand % 2 == 0)
		{
			int num6 = 10 + Mathf.Clamp(this.frame, 0, 3);
			this.sprite.SetLowerLeftPixel((float)(num6 * this.spritePixelWidth), (float)this.spritePixelHeight);
			if (this.frame == 1)
			{
				this.yI = 190f;
				this.PlayKnifeClimbSound();
			}
			if (this.frame >= 5)
			{
				this.frame = 0;
				this.knifeHand++;
				this.RunStepOnWalls();
			}
			else if (this.frame > 1 && !FluidController.IsSubmerged(this))
			{
				EffectsController.CreateFootPoofEffect(this.x + base.transform.localScale.x * 6f, this.y + 12f, 0f, Vector3.up * 1f);
			}
		}
		else
		{
			int num7 = 13 + Mathf.Clamp(this.frame, 0, 3);
			this.sprite.SetLowerLeftPixel((float)(num7 * this.spritePixelWidth), (float)this.spritePixelHeight);
			if (this.frame == 1)
			{
				this.yI = 190f;
				this.PlayKnifeClimbSound();
			}
			if (this.frame >= 5)
			{
				this.frame = 0;
				this.knifeHand++;
			}
			else if (this.frame > 1 && !FluidController.IsSubmerged(this))
			{
				EffectsController.CreateFootPoofEffect(this.x + base.transform.localScale.x * 6f, this.y + 12f, 0f, Vector3.up * 1f);
			}
		}
		RaycastHit raycastHit;
		if (this.left && Physics.Raycast(new Vector3(this.x, this.y + this.headHeight, 0f), Vector3.left, out raycastHit, this.halfWidth + 2f, this.groundLayer))
		{
			raycastHit.collider.SendMessage("StepOn", this, SendMessageOptions.DontRequireReceiver);
		}
		else if (this.right && Physics.Raycast(new Vector3(this.x, this.y + this.headHeight, 0f), Vector3.left, out raycastHit, this.halfWidth + 2f, this.groundLayer))
		{
			raycastHit.collider.SendMessage("StepOn", this, SendMessageOptions.DontRequireReceiver);
		}
	}

	protected virtual void RunStepOnWalls()
	{
		RaycastHit raycastHit;
		if (this.left && Physics.Raycast(new Vector3(this.x, this.y + this.headHeight, 0f), Vector3.left, out raycastHit, 10f, this.groundLayer))
		{
			raycastHit.collider.SendMessage("StepOn", this, SendMessageOptions.DontRequireReceiver);
		}
		else if (this.right && Physics.Raycast(new Vector3(this.x, this.y + this.headHeight, 0f), Vector3.left, out raycastHit, 10f, this.groundLayer))
		{
			raycastHit.collider.SendMessage("StepOn", this, SendMessageOptions.DontRequireReceiver);
		}
	}

	protected virtual void AnimateFallen()
	{
	}

	protected virtual void AnimateDeath()
	{
		if (this.y > this.groundHeight + 0.2f && this.impaledOnSpikes == null)
		{
			int num = 4;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)this.spritePixelHeight);
		}
		else
		{
			int num2 = 5;
			this.sprite.SetLowerLeftPixel((float)(num2 * this.spritePixelWidth), (float)this.spritePixelHeight);
		}
	}

	protected virtual void StartDashing()
	{
		if (this.canDash)
		{
			if (this.actionState == ActionState.Jumping)
			{
				this.hasDashedInAir = true;
			}
			if (this.hasDashedInAir)
			{
				this.dashSpeedM = this.lastDashSpeedM;
			}
			else
			{
				this.dashSpeedM -= 0.33f;
				if (this.dashSpeedM < 1f)
				{
					this.dashSpeedM = 1f;
				}
			}
			if (this.actionState != ActionState.Jumping)
			{
				if (!this.dashing)
				{
					this.PlayDashSound(0.3f);
				}
				this.dashing = true;
				this.dashSpeedM = 1.33f;
				this.delayedDashing = false;
				if (!FluidController.IsSubmerged(this))
				{
					EffectsController.CreateDashPoofEffect_Networked(this.x, this.y, (Mathf.Abs(this.xI) >= 1f) ? ((int)base.transform.localScale.x) : 0);
				}
			}
			else
			{
				this.delayedDashing = true;
			}
		}
	}

	protected void CheckAirDash()
	{
		if (this.canAirdash && ((this.actionState == ActionState.Jumping && (this.airdashLeftAvailable || this.airdashRightAvailable)) || this.CanTouchGround(base.transform.localScale.x * -4f)) && !this.wallClimbing && !this.WallDrag)
		{
			this.Airdash(false);
		}
		else if (this.down && !this.wasDown && this.airdashDownAvailable && !this.right && !this.left)
		{
			this.Airdash(false);
		}
	}

	protected void StopDashing()
	{
		if (this.actionState != ActionState.Jumping)
		{
			this.lastDashSpeedM = 1f;
			this.dashSpeedM = 1f;
		}
		else
		{
			this.lastDashSpeedM = this.dashSpeedM;
		}
	}

	protected virtual bool CanAirDash(DirectionEnum direction)
	{
		if (!this.canAirdash || this.health <= 0)
		{
			return false;
		}
		if (this.CanTouchGround(0f))
		{
			return true;
		}
		switch (direction)
		{
		case DirectionEnum.Up:
			return this.airdashUpAvailable;
		case DirectionEnum.Down:
			return this.airdashDownAvailable;
		case DirectionEnum.Left:
			return this.airdashLeftAvailable;
		case DirectionEnum.Right:
			return this.airdashRightAvailable;
		default:
			return false;
		}
	}

	protected virtual void CalculateMovement()
	{
		if (this.health <= 0)
		{
			return;
		}
		if (!this.usingSpecial)
		{
			if (this.wasSpecial)
			{
			}
		}
		if (this.fire)
		{
			if (!this.wasFire)
			{
				this.StartFiring();
			}
		}
		else if (this.wasFire)
		{
			this.StopFiring();
		}
		this.CheckDashing();
		if (!this.right && !this.left && this.actionState == ActionState.Running)
		{
			if (this.actionState != ActionState.ClimbingLadder && this.actionState != ActionState.Hanging && this.actionState != ActionState.Jumping)
			{
				this.actionState = ActionState.Idle;
			}
			this.dashing = false;
		}
		if (!this.dashing)
		{
			this.StopDashing();
		}
		if (this.left)
		{
			if (!this.wasLeft)
			{
				if (!this.dashing && !this.right && Time.time - this.leftTapTime < this.minDashTapTime)
				{
					if (!this.dashing)
					{
						this.StartDashing();
					}
					this.dashTime = Time.time;
				}
				if (this.holdingHighFive && this.CanAirDash(DirectionEnum.Left))
				{
					this.Airdash(true);
				}
				else
				{
					this.leftTapTime = Time.time;
					this.ClampSpeedPressingLeft();
					if (this.actionState == ActionState.Idle)
					{
						this.actionState = ActionState.Running;
						this.AnimateRunning();
					}
				}
			}
			if (!this.right)
			{
				this.AddSpeedLeft();
			}
		}
		if (this.right)
		{
			if (!this.wasRight)
			{
				if (!this.left && Time.time - this.rightTapTime < this.minDashTapTime)
				{
					if (!this.dashing)
					{
						this.StartDashing();
					}
					this.dashTime = Time.time;
				}
				if (this.holdingHighFive && this.CanAirDash(DirectionEnum.Right))
				{
					this.Airdash(true);
				}
				else
				{
					this.rightTapTime = Time.time;
					this.ClampSpeedPressingRight();
					if (this.actionState == ActionState.Idle)
					{
						this.actionState = ActionState.Running;
						this.AnimateRunning();
					}
				}
			}
			if (!this.left)
			{
				this.AddSpeedRight();
			}
		}
		else if (this.wasRight)
		{
			if (!this.left && this.actionState != ActionState.ClimbingLadder && this.actionState != ActionState.Hanging && this.actionState != ActionState.Jumping)
			{
				this.actionState = ActionState.Idle;
			}
			this.dashing = false;
		}
		if (this.down && !this.wasDown)
		{
			this.downTapTime = Time.time;
		}
		if (this.buttonJump)
		{
			this.lastButtonJumpTime = Time.time;
			if (this.playerNum < 0)
			{
			}
			if (this.actionState == ActionState.Jumping && (this.jumpTime > 0f || (!this.wasButtonJump && FluidController.IsSubmerged(this))) && this.yI < this.jumpForce)
			{
				this.yI = this.jumpForce;
			}
			if (!this.wasButtonJump || this.jumpGrace > 0f)
			{
				if ((!this.ducking || !Map.IsBlockSolid(this.collumn, this.row + 1)) && Time.time - this.lastJumpTime > 0.08f && this.CanTouchGround((float)(((!this.right || this.canTouchLeftWalls || Physics.Raycast(new Vector3(this.x, this.y + 5f, 0f), Vector3.left, out this.raycastHitWalls, 13.5f, this.groundLayer)) ? 0 : -13) + ((!this.left || this.canTouchRightWalls || Physics.Raycast(new Vector3(this.x, this.y + 5f, 0f), Vector3.right, out this.raycastHitWalls, 13.5f, this.groundLayer)) ? 0 : 13))))
				{
					if (this.yI < 0f)
					{
						this.Land();
					}
					this.Jump(false);
				}
				else if (this.WallDrag && this.yI < 25f)
				{
					if (this.right || this.left)
					{
						this.Jump(true);
					}
				}
				else if (this.canTouchLeftWalls && this.right && !this.wasButtonJump)
				{
					this.Jump(true);
				}
				else if (this.canTouchRightWalls && this.left && !this.wasButtonJump)
				{
					this.Jump(true);
				}
				else if (!this.ducking && this.actionState == ActionState.Jumping && this.left && Time.time - this.lastJumpTime > 0.3f && Physics.Raycast(new Vector3(this.x - 8f, this.y + 10f, 0f), Vector3.up, out this.raycastHit, this.headHeight, this.groundLayer))
				{
					this.Jump(true);
				}
				else if (!this.ducking && this.actionState == ActionState.Jumping && this.right && Time.time - this.lastJumpTime > 0.3f && Physics.Raycast(new Vector3(this.x + 8f, this.y + 10f, 0f), Vector3.up, out this.raycastHit, this.headHeight, this.groundLayer))
				{
					this.Jump(true);
				}
				else if (!this.wasButtonJump)
				{
					this.jumpGrace = 0.2f;
				}
			}
			else if (this.WallDrag)
			{
				if (!this.wallClimbing)
				{
					if (!this.useNewKnifeClimbingFrames)
					{
						this.PlayKnifeClimbSound();
					}
					if (this.useNewKnifeClimbingFrames && !this.wallClimbAnticipation && !this.chimneyFlip)
					{
						this.frame = 0;
						this.lastKnifeClimbStabY = this.y + this.knifeClimbStabHeight;
						this.AnimateWallClimb();
					}
				}
				this.wallClimbing = true;
				if (this.yI < 5f)
				{
					this.yI = 5f;
				}
			}
			else
			{
				this.wallClimbing = false;
			}
		}
		else
		{
			this.wallClimbing = false;
		}
	}

	protected virtual void ClampSpeedPressingLeft()
	{
		if (!this.right && this.xI > -10f)
		{
			this.xI = -10f;
		}
	}

	protected virtual void ClampSpeedPressingRight()
	{
		if (!this.left && this.xI < 10f)
		{
			this.xI = 10f;
		}
	}

	protected virtual void HitRightWall()
	{
	}

	protected virtual void HitLeftWall()
	{
	}

	protected void CreateFaderTrailInstance()
	{
		SpriteSM spriteSM = UnityEngine.Object.Instantiate(this.faderSpritePrefab, base.transform.position, base.transform.rotation) as SpriteSM;
		if (spriteSM != null)
		{
			FaderSprite component = spriteSM.GetComponent<FaderSprite>();
			if (component != null)
			{
				component.transform.localScale = base.transform.localScale;
				component.SetMaterial(base.GetComponent<Renderer>().material, this.sprite.lowerLeftPixel, this.sprite.pixelDimensions, this.sprite.offset);
			}
		}
		else
		{
			UnityEngine.Debug.Log("Fader null?");
		}
	}

	protected void SetAirdashAvailable()
	{
		this.airdashUpAvailable = true;
		this.airdashRightAvailable = true;
		this.airdashLeftAvailable = true;
		this.airdashDownAvailable = true;
	}

	protected virtual void AirDashRight()
	{
		UnityEngine.Debug.Log("Air Dash Right !");
		this.airdashRightAvailable = false;
		this.airdashDirection = DirectionEnum.Right;
		if (this.airdashTime <= 0f)
		{
			this.yI = 0f;
			this.xI = 0f;
			this.PlayAidDashChargeUpSound();
			this.airDashDelay = 0.15f;
			this.airdashTime = this.airdashMaxTime + this.airDashDelay;
		}
		else if (this.airDashDelay <= 0f)
		{
			this.PlayAidDashSound();
			EffectsController.CreateAirDashPoofEffect(this.x, this.y + 8f, new Vector3(-100f, 0f, 0f));
			this.airdashTime = this.airdashMaxTime;
		}
		this.airdashDownAvailable = true;
		this.actionState = ActionState.Jumping;
		this.ChangeFrame();
		this.SetInvulnerable(this.airDashDelay + 0.02f, false);
	}

	protected virtual void AirDashLeft()
	{
		UnityEngine.Debug.Log("Air Dash Left !");
		this.airdashLeftAvailable = false;
		this.airdashDirection = DirectionEnum.Left;
		if (this.airdashTime <= 0f)
		{
			this.yI = 0f;
			this.xI = 0f;
			this.PlayAidDashChargeUpSound();
			this.airDashDelay = 0.15f;
			this.airdashTime = this.airdashMaxTime + this.airDashDelay;
		}
		else if (this.airDashDelay <= 0f)
		{
			this.PlayAidDashSound();
			EffectsController.CreateAirDashPoofEffect(this.x, this.y + 8f, new Vector3(100f, 0f, 0f));
			this.airdashTime = this.airdashMaxTime;
		}
		this.airdashDownAvailable = true;
		this.actionState = ActionState.Jumping;
		this.ChangeFrame();
		this.SetInvulnerable(this.airDashDelay + 0.02f, false);
	}

	protected virtual void AirDashUp()
	{
		this.airdashUpAvailable = false;
		this.airdashTime = this.airdashMaxTime * 0.8f;
		this.airdashDirection = DirectionEnum.Up;
		this.yI = this.jumpForce * 1.5f;
		this.xI = 0f;
		this.actionState = ActionState.Jumping;
		EffectsController.CreateAirDashPoofEffect(this.x, this.y + 8f, new Vector3(0f, -100f, 0f));
		this.PlayAidDashSound();
	}

	protected virtual void AirDashDown()
	{
		this.airdashTime = this.airdashMaxTime * 5f;
		this.airdashDirection = DirectionEnum.Down;
		this.yI = this.jumpForce * 0.8f;
		this.xI = 0f;
		this.airdashDownAvailable = false;
		this.actionState = ActionState.Jumping;
		this.PlayAidDashChargeUpSound();
	}

	protected void Airdash(bool highFived)
	{
		if (this.right && (!this.wasRight || highFived) && this.CanAirDash(DirectionEnum.Right))
		{
			this.AirDashRight();
		}
		else if (this.left && (!this.wasLeft || highFived) && this.CanAirDash(DirectionEnum.Left))
		{
			this.AirDashLeft();
		}
		else if (this.down && this.airdashDownAvailable && this.CanAirDash(DirectionEnum.Down))
		{
			this.AirDashDown();
		}
		else if (this.up && this.airdashUpAvailable && this.CanAirDash(DirectionEnum.Up))
		{
			this.AirDashUp();
		}
	}

	protected virtual void PlayAidDashChargeUpSound()
	{
	}

	protected virtual void PlayAidDashSound()
	{
	}

	private void CheckDashing()
	{
		if (this.playerNum < 0 || this.playerNum > 3)
		{
			return;
		}
		this.wasdashButton = this.dashButton;
		if (HeroController.players[this.playerNum] != null && InputReader.GetDashStart(HeroController.players[this.playerNum].controllerNum))
		{
			this.dashButton = true;
		}
		else
		{
			this.dashButton = false;
		}
		if (this.dashButton && !this.wasdashButton)
		{
			this.PressDashButton();
		}
		else if (this.wasdashButton && !this.dashButton)
		{
			this.ReleaseDashButton();
		}
	}

	protected virtual void PressDashButton()
	{
		if (this.canAirdash)
		{
			this.Airdash(true);
		}
		if (!this.dashing && (this.left || this.right))
		{
			this.StartDashing();
		}
	}

	protected virtual void ReleaseDashButton()
	{
		if (this.dashing)
		{
			this.dashing = false;
			this.StopDashing();
		}
	}

	public virtual void Tumble()
	{
	}

	protected virtual void Jump(bool wallJump)
	{
		if (this.canAirdash && (this.canTouchLeftWalls || this.canTouchRightWalls || !wallJump))
		{
			this.SetAirdashAvailable();
		}
		this.lastJumpTime = Time.time;
		this.actionState = ActionState.Jumping;
		this.yI = this.jumpForce;
		this.xIBlast += this.parentedDiff.x / this.t;
		this.yI += this.parentedDiff.y / this.t;
		this.doubleJumpsLeft = 0;
		this.wallClimbAnticipation = false;
		if (wallJump)
		{
			this.jumpTime = 0f;
			this.xI = 0f;
			if (this.useNewKnifeClimbingFrames)
			{
				this.frame = 0;
				this.lastKnifeClimbStabY = this.y + this.knifeClimbStabHeight;
			}
			else
			{
				this.knifeHand++;
			}
			RaycastHit raycastHit;
			if (this.left && Physics.Raycast(new Vector3(this.x, this.y + this.headHeight, 0f), Vector3.left, out raycastHit, 10f, this.groundLayer))
			{
				raycastHit.collider.SendMessage("StepOn", this, SendMessageOptions.DontRequireReceiver);
				this.SetCurrentFootstepSound(raycastHit.collider);
				if (this.useNewKnifeClimbingFrames)
				{
					this.AnimateWallAnticipation();
				}
			}
			else if (this.right && Physics.Raycast(new Vector3(this.x, this.y + this.headHeight, 0f), Vector3.right, out raycastHit, 10f, this.groundLayer))
			{
				raycastHit.collider.SendMessage("StepOn", this, SendMessageOptions.DontRequireReceiver);
				this.SetCurrentFootstepSound(raycastHit.collider);
				if (this.useNewKnifeClimbingFrames)
				{
					this.AnimateWallAnticipation();
				}
			}
		}
		else
		{
			this.jumpTime = 0.123f;
			this.ChangeFrame();
		}
		this.PlayJumpSound();
		if (!wallJump && this.groundHeight - this.y > -2f)
		{
			if (!FluidController.IsSubmerged(this))
			{
				EffectsController.CreateJumpPoofEffect(this.x, this.y, (Mathf.Abs(this.xI) >= 30f) ? (-(int)base.transform.localScale.x) : 0);
			}
			else
			{
				EffectsController.CreateBubbles(this.x, this.y, base.transform.position.z + 1f, UnityEngine.Random.Range(8, 13), 10f, 1f);
			}
		}
	}

	protected virtual void AddSpeedLeft()
	{
		if (this.xI > -25f)
		{
			this.xI = -25f;
		}
		this.xI -= this.speed * ((!this.dashing || this.ducking) ? 2f : 4f) * this.t;
		if (this.xI < -((!this.dashing || this.ducking) ? this.speed : (this.speed * this.dashSpeedM)))
		{
			this.xI = -((!this.dashing || this.ducking) ? this.speed : (this.speed * this.dashSpeedM));
		}
		else if (this.xI > -50f)
		{
			this.xI -= this.speed * 2.6f * this.t;
		}
	}

	protected virtual void AddSpeedRight()
	{
		if (this.xI < 25f)
		{
			this.xI = 25f;
		}
		this.xI += this.speed * ((!this.dashing || this.ducking) ? 2f : 4f) * this.t;
		if (this.xI > ((!this.dashing || this.ducking) ? this.speed : (this.speed * this.dashSpeedM)))
		{
			this.xI = ((!this.dashing || this.ducking) ? this.speed : (this.speed * this.dashSpeedM));
		}
		else if (this.xI < 50f)
		{
			this.xI += this.speed * 2.6f * this.t;
		}
	}

	protected virtual void StopFiring()
	{
	}

	protected virtual void StartFiring()
	{
		if (this.fireDelay <= 0f)
		{
			if (this.fireRate < 0.3f)
			{
				this.fireCounter = this.fireRate;
			}
			else
			{
				this.fireCounter = 0f;
			}
		}
	}

	protected void SetCurrentFootstepSound(Collider collider)
	{
		if (this.soundHolderFootSteps != null && !collider.CompareTag(string.Empty))
		{
			this.currentFootStepGroundType = collider.tag;
		}
	}

	protected bool CanTouchGround(float xOffset)
	{
		LayerMask mask = this.GetGroundLayer() | this.fragileLayer;
		RaycastHit raycastHit;
		if (Physics.Raycast(new Vector3(this.x, this.y + 14f, 0f), Vector3.down, out raycastHit, 16f, mask))
		{
			this.SetCurrentFootstepSound(raycastHit.collider);
			return true;
		}
		if (Physics.Raycast(new Vector3(this.x - 3f, this.y + 14f, 0f), Vector3.down, out raycastHit, 16f, mask))
		{
			this.SetCurrentFootstepSound(raycastHit.collider);
			return true;
		}
		if (Physics.Raycast(new Vector3(this.x + 3f, this.y + 14f, 0f), Vector3.down, out raycastHit, 16f, mask))
		{
			this.SetCurrentFootstepSound(raycastHit.collider);
			return true;
		}
		if (xOffset != 0f && Physics.Raycast(new Vector3(this.x + xOffset, this.y + 12f, 0f), Vector3.down, out raycastHit, 15f, mask))
		{
			this.SetCurrentFootstepSound(raycastHit.collider);
			return true;
		}
		if (!Map.IsBlockLadder(this.x, this.y) && !this.down)
		{
			if (Physics.Raycast(new Vector3(this.x, this.y + 14f, 0f), Vector3.down, out raycastHit, 16f, this.ladderLayer))
			{
				this.SetCurrentFootstepSound(raycastHit.collider);
				return true;
			}
			if (!Map.IsBlockLadder(this.x - 3f, this.y) && !this.down && Physics.Raycast(new Vector3(this.x - 3f, this.y + 14f, 0f), Vector3.down, out raycastHit, 16f, this.ladderLayer))
			{
				this.SetCurrentFootstepSound(raycastHit.collider);
				return true;
			}
			if (!Map.IsBlockLadder(this.x, this.y) && !this.down && Physics.Raycast(new Vector3(this.x - 3f, this.y + 14f, 0f), Vector3.down, out raycastHit, 16f, this.ladderLayer))
			{
				this.SetCurrentFootstepSound(raycastHit.collider);
				return true;
			}
			if (xOffset != 0f && !Map.IsBlockLadder(this.x + xOffset, this.y) && !this.down && Physics.Raycast(new Vector3(this.x + xOffset, this.y + 12f, 0f), Vector3.down, out raycastHit, 15f, this.ladderLayer))
			{
				this.SetCurrentFootstepSound(raycastHit.collider);
				return true;
			}
		}
		return false;
	}

	public virtual LayerMask GetGroundLayer()
	{
		if (!this.down)
		{
			return this.groundLayer | this.platformLayer;
		}
		return this.groundLayer;
	}

	public override bool IsOnGround()
	{
		return this.y < this.groundHeight + 0.5f && this.yI <= 0f;
	}

	public virtual float GetGroundHeightGround()
	{
		this.currentFootStepGroundType = string.Empty;
		LayerMask mask = this.GetGroundLayer() | this.fragileLayer;
		float num = -200f;
		if (this.xI > 0f)
		{
			if (Physics.Raycast(new Vector3(this.x + this.feetWidth, this.y + 4f, 0f), Vector3.down, out this.raycastHit, 540f, mask) && this.raycastHit.point.y > num)
			{
				num = this.raycastHit.point.y;
				if (this.soundHolderFootSteps != null && !this.raycastHit.collider.CompareTag(string.Empty))
				{
					this.currentFootStepGroundType = this.raycastHit.collider.tag;
				}
				this.AssignGroundTransform(this.raycastHit.collider.transform);
			}
			if (Physics.Raycast(new Vector3(this.x - this.feetWidth * 0.75f, this.y + 4f, 0f), Vector3.down, out this.raycastHit, 540f, mask) && this.raycastHit.point.y > num)
			{
				num = this.raycastHit.point.y;
				if (this.soundHolderFootSteps != null && !this.raycastHit.collider.CompareTag(string.Empty))
				{
					this.currentFootStepGroundType = this.raycastHit.collider.tag;
				}
				this.AssignGroundTransform(this.raycastHit.collider.transform);
			}
		}
		if (this.xI < 0f)
		{
			if (Physics.Raycast(new Vector3(this.x - this.feetWidth, this.y + 4f, 0f), Vector3.down, out this.raycastHit, 540f, mask) && this.raycastHit.point.y > num)
			{
				num = this.raycastHit.point.y;
				if (this.soundHolderFootSteps != null && !this.raycastHit.collider.CompareTag(string.Empty))
				{
					this.currentFootStepGroundType = this.raycastHit.collider.tag;
				}
				this.AssignGroundTransform(this.raycastHit.collider.transform);
			}
			if (Physics.Raycast(new Vector3(this.x + this.feetWidth * 0.75f, this.y + 4f, 0f), Vector3.down, out this.raycastHit, 540f, mask) && this.raycastHit.point.y > num)
			{
				num = this.raycastHit.point.y;
				if (this.soundHolderFootSteps != null && !this.raycastHit.collider.CompareTag(string.Empty))
				{
					this.currentFootStepGroundType = this.raycastHit.collider.tag;
				}
				this.AssignGroundTransform(this.raycastHit.collider.transform);
			}
		}
		if (Physics.Raycast(new Vector3(this.x, this.y + 4f, 0f), Vector3.down, out this.raycastHit, 540f, mask) && this.raycastHit.point.y > num)
		{
			num = this.raycastHit.point.y;
			if (this.soundHolderFootSteps != null && !this.raycastHit.collider.CompareTag(string.Empty))
			{
				this.currentFootStepGroundType = this.raycastHit.collider.tag;
			}
			this.AssignGroundTransform(this.raycastHit.collider.transform);
		}
		if (this.feetWidth > 7f)
		{
			if (Physics.Raycast(new Vector3(this.x + this.feetWidth, this.y + 4f, 0f), Vector3.down, out this.raycastHit, 540f, mask) && this.raycastHit.point.y > num)
			{
				num = this.raycastHit.point.y;
				if (this.soundHolderFootSteps != null && !this.raycastHit.collider.CompareTag(string.Empty))
				{
					this.currentFootStepGroundType = this.raycastHit.collider.tag;
				}
				this.AssignGroundTransform(this.raycastHit.collider.transform);
			}
			if (Physics.Raycast(new Vector3(this.x - this.feetWidth, this.y + 4f, 0f), Vector3.down, out this.raycastHit, 540f, mask) && this.raycastHit.point.y > num)
			{
				num = this.raycastHit.point.y;
				if (this.soundHolderFootSteps != null && !this.raycastHit.collider.CompareTag(string.Empty))
				{
					this.currentFootStepGroundType = this.raycastHit.collider.tag;
				}
				this.AssignGroundTransform(this.raycastHit.collider.transform);
			}
		}
		if (!Map.IsBlockLadder(this.x, this.y + 8f) && !this.down && this.health > 0)
		{
			if (this.xI > 0f)
			{
				if (!Map.IsBlockLadder(this.x + this.feetWidth, this.y + 4f) && !this.down && Physics.Raycast(new Vector3(this.x + 4f, this.y + 8f, 0f), Vector3.down, out this.raycastHit, 540f, this.ladderLayer) && this.raycastHit.point.y > num)
				{
					num = this.raycastHit.point.y;
					if (this.soundHolderFootSteps != null && !this.raycastHit.collider.CompareTag(string.Empty))
					{
						this.currentFootStepGroundType = this.raycastHit.collider.tag;
					}
					this.AssignGroundTransform(this.raycastHit.collider.transform);
				}
				if (!Map.IsBlockLadder(this.x - this.feetWidth / 2f, this.y + 4f) && !this.down && Physics.Raycast(new Vector3(this.x - 2f, this.y + 8f, 0f), Vector3.down, out this.raycastHit, 540f, this.ladderLayer) && this.raycastHit.point.y > num)
				{
					num = this.raycastHit.point.y;
					if (this.soundHolderFootSteps != null && !this.raycastHit.collider.CompareTag(string.Empty))
					{
						this.currentFootStepGroundType = this.raycastHit.collider.tag;
					}
					this.AssignGroundTransform(this.raycastHit.collider.transform);
				}
			}
			if (this.xI < 0f)
			{
				if (!Map.IsBlockLadder(this.x - this.feetWidth, this.y + 4f) && !this.down && Physics.Raycast(new Vector3(this.x - 4f, this.y + 8f, 0f), Vector3.down, out this.raycastHit, 540f, this.ladderLayer) && this.raycastHit.point.y > num)
				{
					num = this.raycastHit.point.y;
					if (this.soundHolderFootSteps != null && !this.raycastHit.collider.CompareTag(string.Empty))
					{
						this.currentFootStepGroundType = this.raycastHit.collider.tag;
					}
					this.AssignGroundTransform(this.raycastHit.collider.transform);
				}
				if (!Map.IsBlockLadder(this.x + this.feetWidth / 2f, this.y + 4f) && !this.down && Physics.Raycast(new Vector3(this.x + 2f, this.y + 8f, 0f), Vector3.down, out this.raycastHit, 540f, this.ladderLayer) && this.raycastHit.point.y > num)
				{
					num = this.raycastHit.point.y;
					if (this.soundHolderFootSteps != null && !this.raycastHit.collider.CompareTag(string.Empty))
					{
						this.currentFootStepGroundType = this.raycastHit.collider.tag;
					}
					this.AssignGroundTransform(this.raycastHit.collider.transform);
				}
			}
			if (Physics.Raycast(new Vector3(this.x, this.y + 8f, 0f), Vector3.down, out this.raycastHit, 540f, this.ladderLayer) && this.raycastHit.point.y > num)
			{
				num = this.raycastHit.point.y;
				if (this.soundHolderFootSteps != null && !this.raycastHit.collider.CompareTag(string.Empty))
				{
					this.currentFootStepGroundType = this.raycastHit.collider.tag;
				}
				this.AssignGroundTransform(this.raycastHit.collider.transform);
			}
		}
		return num;
	}

	protected void AssignGroundTransform(Transform gTransform)
	{
		this.groundTransform = gTransform;
		if (this.groundTransform.GetComponent<Tank>() != null)
		{
			this.groundTransformLocalPos = gTransform.InverseTransformPoint(new Vector3(this.x, this.y, 0f));
		}
		else
		{
			this.groundTransformLocalPos = base.transform.position - gTransform.position;
		}
	}

	protected void AssignWallTransform(Transform wTransform)
	{
		this.wallClimbingWallTransform = wTransform;
		this.wallClimbingWallTransformLocalPos = wTransform.InverseTransformPoint(new Vector3(this.x, this.y, 0f));
	}

	protected virtual bool IsOverLadder(ref float ladderXPos)
	{
		return this.IsOverLadder(0f, ref ladderXPos);
	}

	protected virtual bool IsOverLadder(float xOffset, ref float ladderXPos)
	{
		Collider[] array = Physics.OverlapSphere(new Vector3(this.x + xOffset, this.y + 6f + (float)((!this.down) ? 0 : -2), 0f), 4f, this.ladderLayer);
		if (array.Length > 0)
		{
			this.jumpTime = 0.07f;
			this.doubleJumpsLeft = 0;
			ladderXPos = array[0].transform.position.x;
			return true;
		}
		return false;
	}

	protected bool IsAboveLadder()
	{
		Collider[] array = Physics.OverlapSphere(new Vector3(this.x, this.y - 1f, 0f), 4f, this.ladderLayer);
		return array.Length > 0;
	}

	protected bool IsNearbyLadder(float xOffset, float yOffset)
	{
		Collider[] array = Physics.OverlapSphere(new Vector3(this.x + xOffset, this.y + yOffset, 0f), 4f, this.ladderLayer);
		return array.Length > 0;
	}

	protected bool IsWallInFront(float distance)
	{
		return Physics.Raycast(new Vector3(this.x, this.y + this.waistHeight, 0f), (base.transform.localScale.x <= 0f) ? Vector3.left : Vector3.right, distance, this.groundLayer);
	}

	protected bool IsOverFinish(ref float ladderXPos)
	{
		if (!base.IsMine || !base.enabled || this.IsHeavy())
		{
			return false;
		}
		if (this.playerNum >= 0 && this.playerNum < 5)
		{
			Collider[] array = Physics.OverlapSphere(new Vector3(this.x, this.y, 0f), 4f, this.victoryLayer);
			if (array.Length > 0)
			{
				Helicopter component = array[0].transform.parent.GetComponent<Helicopter>();
				component.attachedHeroes.Add(this);
				MonoBehaviour.print("ATTACHED: " + this);
				this.jumpTime = 0.07f;
				this.doubleJumpsLeft = 0;
				ladderXPos = component.transform.position.x + 13f;
				component.Leave();
				this.invulnerable = true;
				if (base.GetComponent<AudioSource>() != null)
				{
					base.GetComponent<AudioSource>().Stop();
				}
				base.transform.parent = component.transform;
				float x = component.transform.position.x;
				if (this.x > x)
				{
					base.transform.localScale = new Vector3(-1f, 1f, 1f);
					this.x = x + 5f;
				}
				else
				{
					base.transform.localScale = new Vector3(1f, 1f, 1f);
					this.x = x - 5f;
				}
				base.transform.position = new Vector3(Mathf.Round(this.x), Mathf.Round(this.y), -50f);
				base.enabled = false;
				Networking.RPC<Vector3, float, TestVanDammeAnim, Helicopter>(PID.TargetAll, new RpcSignature<Vector3, float, TestVanDammeAnim, Helicopter>(HeroController.Instance.AttachHeroToHelicopter), base.transform.localPosition, base.transform.localScale.x, this, component, false);
				GameModeController.LevelFinish(LevelResult.Success);
				Map.StartLevelEndExplosionsOverNetwork();
				this.isOnHelicopter = true;
				this.playerNum = 5;
				return true;
			}
		}
		return false;
	}

	private void GrabHelicopterLadder()
	{
	}

	public void SetInvulnerable(float time, bool restartBubble = true)
	{
		this.firedWhileInvulnerable = false;
		this.invulnerableTime = Mathf.Max(this.invulnerableTime, time);
		this.invulnerable = true;
		if (restartBubble && HeroController.mustShowHUDS)
		{
			this.RestartBubble(time);
		}
	}

	protected virtual void RestartBubble(float time)
	{
		if (GameModeController.GameMode == GameMode.Campaign && HeroController.mustShowHUDS)
		{
			if (HeroController.GetPlayersPlayingCount() > 0)
			{
			}
			if (this.playerNum == 0)
			{
				this.player1Bubble.RestartBubble(time);
			}
			if (this.playerNum == 1)
			{
				this.player2Bubble.RestartBubble(time);
			}
			if (this.playerNum == 2)
			{
				this.player3Bubble.RestartBubble(time);
			}
			if (this.playerNum == 3)
			{
				this.player4Bubble.RestartBubble(time);
			}
		}
	}

	protected virtual void RestartBubble()
	{
		if (GameModeController.ShowStandardHUDS() && HeroController.mustShowHUDS && this.playerBubble != null)
		{
			this.playerBubble.RestartBubbleWithBoolParam(false);
		}
	}

	protected virtual void StopBubble()
	{
		if (GameModeController.ShowStandardHUDS() && HeroController.mustShowHUDS)
		{
			if (this.playerNum == 0)
			{
				this.player1Bubble.StopBubble();
			}
			if (this.playerNum == 1)
			{
				this.player2Bubble.StopBubble();
			}
			if (this.playerNum == 2)
			{
				this.player3Bubble.StopBubble();
			}
			if (this.playerNum == 3)
			{
				this.player4Bubble.StopBubble();
			}
		}
	}

	protected virtual void RunAvatarFiring()
	{
		if (this.health > 0)
		{
			if (this.avatarGunFireTime > 0f)
			{
				this.avatarGunFireTime -= this.t;
				if (this.avatarGunFireTime <= 0f)
				{
					if (this.avatarAngryTime > 0f)
					{
						HeroController.SetAvatarAngry(this.playerNum, this.usePrimaryAvatar);
					}
					else
					{
						HeroController.SetAvatarCalm(this.playerNum, this.usePrimaryAvatar);
					}
				}
			}
			if (this.fire)
			{
				if (!this.wasFire && this.avatarGunFireTime <= 0f)
				{
					HeroController.SetAvatarAngry(this.playerNum, this.usePrimaryAvatar);
				}
				this.avatarAngryTime = 0.1f;
			}
			else if (this.avatarAngryTime > 0f)
			{
				this.avatarAngryTime -= this.t;
				if (this.avatarAngryTime <= 0f)
				{
					HeroController.SetAvatarCalm(this.playerNum, this.usePrimaryAvatar);
				}
			}
		}
	}

	protected virtual void RunAvatarRunning()
	{
		if (this.actionState == ActionState.Running)
		{
			if (this.frame / 2 % 2 == 1)
			{
				HeroController.SetAvatarBounceDown(this.playerNum, this.usePrimaryAvatar);
			}
			else
			{
				HeroController.SetAvatarBounceUp(this.playerNum, this.usePrimaryAvatar);
			}
		}
		else
		{
			HeroController.SetAvatarBounceUp(this.playerNum, this.usePrimaryAvatar);
		}
	}

	protected void FireFlashAvatar()
	{
		this.avatarGunFireTime = 0.04f;
		HeroController.SetAvatarFire(this.playerNum, this.usePrimaryAvatar);
	}

	protected virtual void RunFiring()
	{
		this.fireDelay -= this.t;
		if (this.fire)
		{
			this.rollingFrames = 0;
		}
		if (this.fire && this.fireDelay <= 0f)
		{
			this.fireCounter += this.t;
			if (this.fireCounter >= this.fireRate)
			{
				this.fireCounter -= this.fireRate;
				this.UseFire();
				this.FireFlashAvatar();
			}
		}
	}

	protected virtual void RunImpaledMovement()
	{
		this.yIT = 0f;
		this.xIBlast = 0f;
		this.xI = -0.01f * (float)this.impaledDirection;
		this.groundHeight = this.GetGroundHeightGround();
		this.ConstrainToCeiling(ref this.yIT);
		this.xIT = 0f;
		this.ConstrainToWalls(ref this.yIT, ref this.xIT);
		this.SetPosition();
	}

	protected virtual void RunMovement()
	{
		this.rollingTime -= this.t;
		this.groundHeight = this.GetGroundHeightGround();
		if (this.actionState == ActionState.Dead)
		{
			if (this.impaledOnSpikes == null)
			{
				if (this.y > this.groundHeight)
				{
					this.yI -= 1100f * this.t;
					if (this.yI < -50f)
					{
						this.RunFalling();
					}
				}
				else
				{
					this.xI *= 1f - Mathf.Clamp01(this.t * this.frictionM);
				}
			}
			else
			{
				this.xI = 0f;
				this.xIBlast = 0f;
				this.yIBlast = 0f;
				if (this.y >= this.groundHeight)
				{
					this.yI = -0.5f;
				}
				else
				{
					this.yI = 0f;
				}
				if (!this.impaledOnSpikes.gameObject.activeSelf)
				{
					this.impaledOnSpikes = null;
				}
			}
		}
		else if (this.IsOverFinish(ref this.ladderX))
		{
			this.actionState = ActionState.ClimbingLadder;
			this.yI = 0f;
			this.StopAirDashing();
		}
		else if (this.attachedToZipline != null)
		{
			if (this.down && !this.wasDown)
			{
				this.attachedToZipline.DetachUnit(this);
			}
			if (this.buttonJump && !this.wasButtonJump)
			{
				this.attachedToZipline.DetachUnit(this);
				this.yI = this.jumpForce;
			}
		}
		else if (this.actionState == ActionState.Melee)
		{
			this.RunMelee();
		}
		else if (this.actionState != ActionState.ClimbingLadder && (this.up || this.down) && (!this.canDash || this.airdashTime <= 0f) && this.IsOverLadder(ref this.ladderX))
		{
			this.actionState = ActionState.ClimbingLadder;
			this.yI = 0f;
			this.StopAirDashing();
		}
		else if (this.actionState == ActionState.ClimbingLadder)
		{
			this.RunClimbingLadder();
		}
		else if (this.actionState == ActionState.Hanging)
		{
			this.RunHanging();
		}
		else if (this.canAirdash && this.airdashTime > 0f)
		{
			this.RunAirDashing();
		}
		else if (this.actionState == ActionState.Jumping)
		{
			if (this.jumpTime > 0f)
			{
				this.jumpTime -= this.t;
				if (!this.buttonJump)
				{
					this.jumpTime = 0f;
				}
			}
			if (this.wallClimbing)
			{
				this.ApplyWallClimbingGravity();
			}
			else if (this.yI > 40f)
			{
				this.ApplyFallingGravity();
			}
			else
			{
				this.ApplyFallingGravity();
			}
			if (this.yI < this.maxFallSpeed)
			{
				this.yI = this.maxFallSpeed;
			}
			if (this.yI < -50f)
			{
				this.RunFalling();
			}
			if (this.canCeilingHang && this.hangGrace > 0f)
			{
				this.RunCheckHanging();
			}
		}
		else
		{
			if (this.actionState == ActionState.Fallen)
			{
				this.RunFallen();
			}
			if (this.groundHeight > -100000f && this.y > this.groundHeight + 0.1f)
			{
				if (this.health > 0)
				{
					this.actionState = ActionState.Jumping;
				}
			}
		}
		this.yIT = this.yI * this.t;
		if (FluidController.IsSubmerged(this.x, this.y))
		{
			this.yIT *= this.waterDampingY;
		}
		if (this.actionState != ActionState.Recalling)
		{
			if (this.health > 0 && this.playerNum >= 0 && this.playerNum <= 3)
			{
				this.ConstrainSpeedToSidesOfScreen();
			}
			this.ConstrainToCeiling(ref this.yIT);
			if (FluidController.IsSubmerged(this.x, this.y))
			{
				this.xI *= this.waterDampingX;
				this.xIBlast *= this.waterDampingX;
			}
			this.xIT = (this.xI + this.xIBlast + this.xIAttackExtra) * this.t;
			this.ConstrainToWalls(ref this.yIT, ref this.xIT);
			this.x += this.xIT;
			this.CheckClimbAlongCeiling();
			this.CheckForTraps();
			if (this.yI <= 0f)
			{
				this.ConstrainToFloor(ref this.yIT);
			}
		}
		else
		{
			this.invulnerable = true;
			this.yI = 0f;
			this.yIT = this.yI * this.t;
			this.xI = 0f;
		}
		if (this.WallDrag && this.parentHasMovedTime > 0f)
		{
			this.wallDragTime = 0.25f;
		}
		this.y += this.yIT;
		if (this.y < -44f || ((GameModeController.IsDeathMatchMode || GameModeController.GameMode == GameMode.BroDown) && this.y < this.screenMinY - 55f && this.playerNum >= 0))
		{
			if (Map.isEditing)
			{
				this.y = -20f;
				this.yI = -this.yI * 1.5f;
			}
			else if (base.IsHero && Map.lastYLoadOffset > 0 && Map.FindLadderNearPosition((this.screenMaxX + this.screenMinX) / 2f, this.screenMinY, 16, ref this.x, ref this.y))
			{
				this.holdUpTime = 0.3f;
				this.yI = 150f;
				this.xI = 0f;
				this.ShowStartBubble();
			}
			else if (!base.IsHero || base.IsMine)
			{
				this.Gib(DamageType.OutOfBounds, this.xI, 840f);
			}
		}
		if (this.actionState == ActionState.Idle)
		{
			this.xI *= 1f - this.t * 25f;
		}
		if (float.IsNaN(this.x))
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				this.xI,
				", ",
				this.yI,
				", ",
				this.xIBlast,
				", ",
				this.yIBlast
			}));
			UnityEngine.Debug.Break();
		}
		this.SetPosition();
	}

	protected virtual void RunFalling()
	{
	}

	protected virtual void RunFallen()
	{
	}

	protected virtual void RunMelee()
	{
		if (!this.useNewKnifingFrames)
		{
			if (this.y > this.groundHeight + 1f)
			{
				this.ApplyFallingGravity();
			}
		}
		else if (this.jumpingMelee)
		{
			this.ApplyFallingGravity();
			if (this.yI < this.maxFallSpeed)
			{
				this.yI = this.maxFallSpeed;
			}
		}
		else if (this.dashingMelee)
		{
			if (this.frame <= 0)
			{
				this.xI = 0f;
				this.yI = 0f;
			}
			else if (this.frame <= 3)
			{
				if (this.meleeChosenUnit == null)
				{
					this.xI = this.speed * 1f * base.transform.localScale.x;
					this.yI = 0f;
				}
				else
				{
					this.xI = this.speed * 0.5f * base.transform.localScale.x + (this.meleeChosenUnit.x - this.x) * 6f;
				}
			}
			else if (this.frame <= 5)
			{
				this.xI = this.speed * 0.3f * base.transform.localScale.x;
				this.ApplyFallingGravity();
			}
			else
			{
				this.ApplyFallingGravity();
			}
		}
		else if (this.y > this.groundHeight + 1f)
		{
			this.CancelMelee();
		}
	}

	protected virtual void CancelMelee()
	{
		this.doingMelee = false;
		this.jumpingMelee = false;
		this.dashingMelee = false;
		this.standingMelee = false;
		if (this.y > this.groundHeight)
		{
			this.actionState = ActionState.Jumping;
		}
		else if (this.left || this.right)
		{
			this.actionState = ActionState.Running;
		}
		else
		{
			this.actionState = ActionState.Idle;
		}
	}

	protected virtual void RunAirDashing()
	{
		switch (this.airdashDirection)
		{
		case DirectionEnum.Up:
			this.RunUpwardDash();
			break;
		case DirectionEnum.Down:
			this.RunDownwardDash();
			break;
		case DirectionEnum.Left:
			this.RunLeftAirDash();
			break;
		case DirectionEnum.Right:
			this.RunRightAirDash();
			break;
		}
		this.airdashTime -= this.t;
		if (this.airdashTime <= 0f)
		{
			this.xI = Mathf.Clamp(this.xI, -this.speed, this.speed);
			this.yI = Mathf.Clamp(this.yI, this.maxFallSpeed, -this.maxFallSpeed);
			if (this.airdashDirection == DirectionEnum.Up && this.yI > this.jumpForce * 0.8f)
			{
				this.yI = this.jumpForce * 0.8f;
			}
			this.airDashDelay = 0f;
			if (this.right && !this.left)
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else if (this.left && !this.right)
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
		}
	}

	protected virtual void RunLeftAirDash()
	{
		if (this.airDashDelay > 0f)
		{
			this.airDashDelay -= this.t;
			this.yI = 0f;
			this.xI = 50f;
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
			if (this.airDashDelay <= 0f)
			{
				this.ChangeFrame();
				this.PlayAidDashSound();
				EffectsController.CreateAirDashPoofEffect(this.x, this.y + 8f, new Vector3(100f, 0f, 0f));
			}
		}
		else
		{
			this.yI = 0f;
			this.xI = this.speed * -2.3f;
		}
	}

	protected virtual void RunRightAirDash()
	{
		if (this.airDashDelay > 0f)
		{
			this.airDashDelay -= this.t;
			this.yI = 0f;
			this.xI = -50f;
			base.transform.localScale = new Vector3(1f, 1f, 1f);
			if (this.airDashDelay <= 0f)
			{
				this.ChangeFrame();
				this.PlayAidDashSound();
				EffectsController.CreateAirDashPoofEffect(this.x, this.y + 8f, new Vector3(100f, 0f, 0f));
			}
		}
		else
		{
			this.yI = 0f;
			this.xI = this.speed * 2.3f;
		}
	}

	protected virtual void RunUpwardDash()
	{
		this.jumpTime = 0f;
		this.yI = this.jumpForce * 1.5f;
		this.xI = 0f;
	}

	protected virtual void RunDownwardDash()
	{
		if (this.yI > -50f)
		{
			this.yI = Mathf.Clamp(this.yI - 1500f * this.t + Mathf.Clamp(this.yI, -1000f, 18f) * 20f * this.t, this.maxFallSpeed * 1.25f, 300f);
			if (this.yI <= -50f)
			{
				this.PlayAidDashSound();
				EffectsController.CreateAirDashPoofEffect(this.x, this.y + 8f, new Vector3(0f, 100f, 0f));
			}
		}
		else
		{
			this.yI = Mathf.Clamp(this.yI - 1500f * this.t + Mathf.Clamp(this.yI, -1000f, 18f) * 20f * this.t, this.maxFallSpeed * 1.25f, 300f);
		}
		this.xI = 0f;
	}

	protected virtual bool RunCheckHanging()
	{
		bool result = false;
		if (!this.down)
		{
			if (this.left && Physics.Raycast(new Vector3(this.x - this.halfWidth - 1f, this.y + 5f, 0f), Vector3.up, out this.raycastHit, this.headHeight + 5f, this.groundLayer))
			{
				result = true;
				if (Physics.Raycast(new Vector3(this.x - 4f, this.y + 5f, 0f), Vector3.up, out this.raycastHit, this.headHeight + 5f, this.groundLayer))
				{
					UnityEngine.Debug.Log("Check Hanging set speed ");
					if (this.yI < 150f)
					{
						this.yI = 150f;
					}
				}
				else if (this.yI < -50f)
				{
					this.yI = -50f;
				}
			}
			if (this.right && Physics.Raycast(new Vector3(this.x + this.halfWidth + 1f, this.y + 5f, 0f), Vector3.up, out this.raycastHit, this.headHeight + 5f, this.groundLayer))
			{
				result = true;
				if (Physics.Raycast(new Vector3(this.x + 4f, this.y + 5f, 0f), Vector3.up, out this.raycastHit, this.headHeight + 5f, this.groundLayer))
				{
					UnityEngine.Debug.Log("Check Hanging set speed ");
					if (this.yI < 150f)
					{
						this.yI = 150f;
					}
				}
				else if (this.yI < -50f)
				{
					this.yI = -50f;
				}
			}
		}
		return result;
	}

	protected virtual void RunHanging()
	{
		this.yI = 0f;
		if (this.frame % 6 == 0)
		{
			this.xI = Mathf.Clamp(this.xI, -this.speed * 0f, this.speed * 0f);
		}
		else if (this.frame % 6 == 1)
		{
			this.xI = Mathf.Clamp(this.xI, -this.speed * 0.8f, this.speed * 0.8f);
		}
		else
		{
			this.xI = Mathf.Clamp(this.xI, -this.speed * 0.6f, this.speed * 0.6f);
		}
		if (!this.right && !this.left)
		{
			this.xI *= 1f - this.t * 17f;
		}
		else
		{
			this.xI *= 1f - this.t * 8f;
		}
		if (this.up || this.buttonJump)
		{
			this.hangGrace = 0f;
		}
		if ((!this.up && !this.buttonJump && this.hangGrace <= 0f) || this.down)
		{
			this.hangGrace = 0f;
			this.StopHanging();
		}
		else if (!Physics.Raycast(new Vector3(this.x + 4f, this.y + 5f, 0f), Vector3.up, out this.raycastHit, this.headHeight, this.groundLayer))
		{
			if (!Physics.Raycast(new Vector3(this.x - 4f, this.y + 5f, 0f), Vector3.up, out this.raycastHit, this.headHeight, this.groundLayer))
			{
				this.StopHanging();
				this.currentFootStepGroundType = "Stone";
				this.PlayJumpSound();
				this.yI = 200f;
			}
		}
	}

	protected virtual void StopHanging()
	{
		this.actionState = ActionState.Jumping;
	}

	protected virtual void RunClimbingLadder()
	{
		if (this.up && !this.down)
		{
			if (this.yI < this.speed * 0.45f)
			{
				this.yI = this.speed * 0.45f;
			}
			if (this.yI < this.speed * 1.454f)
			{
				this.yI += this.t * 300f;
			}
			else
			{
				this.yI -= 200f * this.t;
			}
			if (this.yI > this.speed * 2.272f)
			{
				this.yI = this.speed * 2.272f;
			}
		}
		else if (this.down && !this.up)
		{
			if (this.yI > -this.speed * 1.272f)
			{
				this.yI = -this.speed * 1.272f;
			}
			this.yI -= this.t * 600f;
			if (this.yI < -this.speed * 5f)
			{
				this.yI = -this.speed * 5f;
			}
		}
		else
		{
			this.yI = 0f;
		}
		if (!this.IsOverLadder(ref this.ladderX))
		{
			this.actionState = ActionState.Jumping;
			this.PlayJumpSound();
		}
		else if (!this.right && !this.left)
		{
			this.xI = 0f;
			if (this.up || this.down)
			{
				this.x += (this.ladderX - this.x) * this.t * 20f;
			}
		}
		else
		{
			this.xI *= 1f - this.t * 5f;
		}
	}

	protected virtual void ApplyFallingGravity()
	{
		if (this.chimneyFlip && this.chimneyFlipConstrained)
		{
			return;
		}
		float num = 1100f * this.t;
		if (FluidController.IsSubmerged(this))
		{
			num *= 0.5f;
		}
		if (this.highFiveBoost)
		{
			num /= this.highFiveBoostM;
		}
		this.yI -= num;
	}

	protected virtual bool ConstrainToFloor(ref float yIT)
	{
		if (this.y + yIT <= this.groundHeight)
		{
			if (this.airdashTime <= 0f || (this.airdashDirection != DirectionEnum.Right && this.airdashDirection != DirectionEnum.Left) || this.y + yIT < this.groundHeight)
			{
				if (this.actionState == ActionState.Jumping || this.actionState == ActionState.Dead || this.jumpingMelee)
				{
					this.Land();
				}
			}
			this.yI = 0f;
			yIT = this.groundHeight - this.y;
			return true;
		}
		if (yIT == 0f && this.y > this.groundHeight && this.y - this.groundHeight < 2f)
		{
			this.y = this.groundHeight;
		}
		return false;
	}

	protected virtual bool ConstrainToCeiling(ref float yIT)
	{
		this.chimneyFlipConstrained = false;
		if (this.yI >= 0f)
		{
			if (((!this.right && base.transform.localScale.x < 0f && Physics.Raycast(new Vector3(this.x + 4f, this.y + 1f, 0f), Vector3.up, out this.raycastHit, this.headHeight + 15f, this.groundLayer)) || (!this.left && base.transform.localScale.x > 0f && Physics.Raycast(new Vector3(this.x + -4f, this.y + 1f, 0f), Vector3.up, out this.raycastHit, this.headHeight + 15f, this.groundLayer))) && this.raycastHit.point.y < this.y + this.headHeight + yIT)
			{
				this.lastKnifeClimbStabY -= this.t * 16f;
				if ((this.up || this.buttonJump) && this.chimneyFlip)
				{
					this.yI = 100f;
					this.jumpTime = 0.03f;
					yIT = this.raycastHit.point.y - this.headHeight - this.y;
					this.chimneyFlipConstrained = true;
					if (this.chimneyFlipFrames > 8 && !Physics.Raycast(new Vector3(this.x + (float)(this.chimneyFlipDirection * 1), this.y + 3f, 0f), Vector3.up, out this.newRaycastHit, this.headHeight + 16f + yIT, this.groundLayer))
					{
						this.xI = (float)(this.chimneyFlipDirection * 0);
					}
					else
					{
						this.xI = (float)(this.chimneyFlipDirection * 100);
					}
				}
				else if (this.canChimneyFlip && (this.up || this.buttonJump) && this.left && !Physics.Raycast(new Vector3(this.x - 12f, this.y - (17f - this.headHeight), 0f), Vector3.up, out this.newRaycastHit, this.headHeight + 16f + yIT, this.groundLayer) && !this.IsNearbyLadder(base.transform.localScale.x * 10f, 2f))
				{
					if (!this.chimneyFlip)
					{
						this.chimneyFlip = true;
						this.chimneyFlipFrames = 11;
						this.chimneyFlipDirection = -1;
						this.AnimateChimneyFlip();
						this.counter = 0f;
						this.chimneyFlipConstrained = true;
					}
					this.yI = 100f;
					this.jumpTime = 0.05f;
					yIT = this.raycastHit.point.y - this.headHeight - this.y;
					if (!Physics.Raycast(new Vector3(this.x + (float)this.chimneyFlipDirection, this.y + 3f, 0f), Vector3.up, out this.newRaycastHit, this.headHeight + 16f + yIT, this.groundLayer))
					{
						this.xI = (float)(this.chimneyFlipDirection * 0);
					}
					else
					{
						this.xI = (float)(this.chimneyFlipDirection * 100);
					}
				}
				else if (this.canChimneyFlip && (this.up || this.buttonJump) && this.right && !Physics.Raycast(new Vector3(this.x + 12f, this.y - (17f - this.headHeight), 0f), Vector3.up, out this.newRaycastHit, this.headHeight + 16f + yIT, this.groundLayer) && !this.IsNearbyLadder(base.transform.localScale.x * 10f, 2f) && this.collumn < Map.Width - 1)
				{
					if (!this.chimneyFlip)
					{
						this.chimneyFlip = true;
						this.chimneyFlipFrames = 11;
						this.chimneyFlipDirection = 1;
						this.AnimateChimneyFlip();
						this.counter = 0f;
						this.chimneyFlipConstrained = true;
					}
					this.yI = 100f;
					this.jumpTime = 0.05f;
					yIT = this.raycastHit.point.y - this.headHeight - this.y;
					if (!Physics.Raycast(new Vector3(this.x + (float)this.chimneyFlipDirection, this.y + 3f, 0f), Vector3.up, out this.newRaycastHit, this.headHeight + 16f + yIT, this.groundLayer))
					{
						this.xI = (float)(this.chimneyFlipDirection * 0);
					}
					else
					{
						this.xI = (float)(this.chimneyFlipDirection * 100);
					}
				}
				else if (this.canChimneyFlip && (this.up || this.buttonJump) && !this.right && !this.left && !Physics.Raycast(new Vector3(this.x + 5f, this.y - (17f - this.headHeight), 0f), Vector3.up, out this.newRaycastHit, this.headHeight + 16f + yIT, this.groundLayer) && !this.IsNearbyLadder(base.transform.localScale.x * 5f, 2f))
				{
					if (!this.chimneyFlip)
					{
						this.chimneyFlip = true;
						this.chimneyFlipFrames = 11;
						this.chimneyFlipDirection = 1;
						this.AnimateChimneyFlip();
						this.counter = 0f;
						this.chimneyFlipConstrained = true;
					}
					this.yI = 100f;
					this.jumpTime = 0.05f;
					yIT = this.raycastHit.point.y - this.headHeight - this.y;
					if (!Physics.Raycast(new Vector3(this.x + (float)this.chimneyFlipDirection, this.y + 3f, 0f), Vector3.up, out this.newRaycastHit, this.headHeight + 16f + yIT, this.groundLayer))
					{
						this.xI = (float)(this.chimneyFlipDirection * 0);
					}
					else
					{
						this.xI = (float)(this.chimneyFlipDirection * 100);
					}
				}
				else if (this.canChimneyFlip && (this.up || this.buttonJump) && !this.right && !this.left && !Physics.Raycast(new Vector3(this.x - 5f, this.y - (17f - this.headHeight), 0f), Vector3.up, out this.newRaycastHit, this.headHeight + 16f + yIT, this.groundLayer) && !this.IsNearbyLadder(base.transform.localScale.x * 5f, 2f))
				{
					if (!this.chimneyFlip)
					{
						this.chimneyFlip = true;
						this.chimneyFlipFrames = 11;
						this.chimneyFlipDirection = -1;
						this.AnimateChimneyFlip();
						this.counter = 0f;
						this.chimneyFlipConstrained = true;
					}
					this.yI = 100f;
					this.jumpTime = 0.05f;
					yIT = this.raycastHit.point.y - this.headHeight - this.y;
					if (!Physics.Raycast(new Vector3(this.x + (float)this.chimneyFlipDirection, this.y + 3f, 0f), Vector3.up, out this.newRaycastHit, this.headHeight + 16f + yIT, this.groundLayer))
					{
						this.xI = (float)(this.chimneyFlipDirection * 0);
					}
					else
					{
						this.xI = (float)(this.chimneyFlipDirection * 100);
					}
				}
			}
			if (Physics.Raycast(new Vector3(this.x + 4f, this.y + 1f, 0f), Vector3.up, out this.raycastHit, this.headHeight + 15f, this.groundLayer) && this.raycastHit.point.y < this.y + this.headHeight + yIT)
			{
				this.lastKnifeClimbStabY -= this.t * 16f;
				if (this.chimneyFlip)
				{
					this.chimneyFlipConstrained = true;
				}
				this.HitCeiling(this.raycastHit);
			}
			if (Physics.Raycast(new Vector3(this.x - 4f, this.y + 1f, 0f), Vector3.up, out this.raycastHit, this.headHeight + 15f, this.groundLayer) && this.raycastHit.point.y < this.y + this.headHeight + yIT)
			{
				this.lastKnifeClimbStabY -= this.t * 16f;
				if (this.chimneyFlip)
				{
					this.chimneyFlipConstrained = true;
				}
				this.HitCeiling(this.raycastHit);
			}
		}
		if ((!this.chimneyFlipConstrained || this.up || this.buttonJump) && this.chimneyFlip && this.chimneyFlipFrames > 5)
		{
			this.xI = (float)(this.chimneyFlipDirection * 100);
			if (this.up || this.buttonJump)
			{
				this.jumpTime = this.t + 0.001f;
				this.yI = 100f;
			}
		}
		return false;
	}

	protected virtual void HitCeiling(RaycastHit ceilingHit)
	{
		if (this.up || this.buttonJump)
		{
			ceilingHit.collider.SendMessage("StepOn", this, SendMessageOptions.DontRequireReceiver);
		}
		this.yIT = ceilingHit.point.y - this.headHeight - this.y;
		if (!this.chimneyFlip && this.yI > 100f && ceilingHit.collider != null)
		{
			this.currentFootStepGroundType = ceilingHit.collider.tag;
			this.PlayFootStepSound(0.2f, 0.6f);
		}
		this.yI = 0f;
		this.jumpTime = 0f;
		if ((this.canCeilingHang && (this.up || this.buttonJump)) || this.hangGrace > 0f)
		{
			this.StartHanging();
		}
	}

	protected virtual void StartHanging()
	{
		if (!this.ducking)
		{
			this.actionState = ActionState.Hanging;
		}
	}

	protected virtual void Land()
	{
		if (this.health > 0 && this.playerNum >= 0 && this.yI < -150f)
		{
			Map.BotherNearbyMooks(this.x, this.y, 24f, 16f, this.playerNum);
		}
		this.FallDamage(this.yI);
		this.StopAirDashing();
		if (this.jumpingMelee)
		{
			this.jumpingMelee = false;
			this.standingMelee = true;
		}
		if (this.yI < 0f && this.health > 0 && this.groundHeight > this.y - 2f + this.yIT)
		{
			if (!FluidController.IsSubmerged(this))
			{
				EffectsController.CreateLandPoofEffect(this.x, this.groundHeight, (Mathf.Abs(this.xI) >= 30f) ? (-(int)base.transform.localScale.x) : 0);
			}
			else
			{
				EffectsController.CreateBubbles(this.x, this.y, base.transform.position.z + 1f, UnityEngine.Random.Range(8, 13), 10f, 1f);
			}
		}
		if (this.health > 0)
		{
			if ((this.left || this.right) && (!this.left || !this.right))
			{
				this.rollingTime = 0f;
				if (this.xI > 0f)
				{
					this.xI += 100f;
				}
				if (this.xI < 0f)
				{
					this.xI -= 100f;
				}
				if (this.actionState == ActionState.Jumping)
				{
					this.lastLandTime = Time.time;
				}
				this.actionState = ActionState.Running;
				if (this.delayedDashing || (this.dashing && Time.time - this.leftTapTime > this.minDashTapTime && Time.time - this.rightTapTime > this.minDashTapTime))
				{
					this.StartDashing();
				}
				this.hasDashedInAir = false;
				if (this.useNewFrames)
				{
					if (this.doRollOnLand && this.yI < -350f)
					{
						this.rollingFrames = 13;
					}
					this.counter = 0f;
					this.AnimateRunning();
					if (!FluidController.IsSubmerged(this))
					{
						EffectsController.CreateFootPoofEffect(this.x, this.groundHeight + 1f, 0f, Vector3.up * 1f);
					}
				}
			}
			else
			{
				this.rollingFrames = 0;
				this.actionState = ActionState.Idle;
			}
			if (this.yI < -100f)
			{
				this.PlayLandSound();
			}
		}
		if (this.yI < -50f)
		{
			if (Physics.Raycast(new Vector3(this.x, this.y + 5f, 0f), Vector3.down, out this.raycastHit, 12f, this.groundLayer))
			{
				this.raycastHit.collider.SendMessage("StepOn", this, SendMessageOptions.DontRequireReceiver);
			}
			if (Physics.Raycast(new Vector3(this.x + 6f, this.y + 5f, 0f), Vector3.down, out this.raycastHit, 12f, this.groundLayer))
			{
				this.raycastHit.collider.SendMessage("StepOn", this, SendMessageOptions.DontRequireReceiver);
			}
			if (Physics.Raycast(new Vector3(this.x - 6f, this.y + 5f, 0f), Vector3.down, out this.raycastHit, 12f, this.groundLayer))
			{
				this.raycastHit.collider.SendMessage("StepOn", this, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	protected void CheckConstrainToScreenTop()
	{
		if (GameModeController.IsDeathMatchMode && this.playerNum >= 0)
		{
			if (this.y > this.screenMaxY - 24f)
			{
				this.down = true;
				this.up = false;
			}
			if (this.y > this.screenMaxY - 6f)
			{
				this.y = this.screenMaxY - 6f;
				if (this.yI > 0f)
				{
					this.yI = 0f;
				}
			}
		}
	}

	protected void ConstrainSpeedToSidesOfScreen()
	{
		if (this.x >= this.screenMaxX - 8f && (this.xI > 0f || this.xIBlast > 0f))
		{
			this.xIBlast = 0f; this.xI = (this.xIBlast );
		}
		if (this.x <= this.screenMinX + 8f && (this.xI < 0f || this.xIBlast < 0f))
		{
			this.xIBlast = 0f; this.xI = (this.xIBlast );
		}
		if (this.x < this.screenMinX - 30f && TriggerManager.destroyOffscreenPlayers && base.IsMine)
		{
			this.Gib(DamageType.OutOfBounds, 840f, this.yI + 50f);
		}
		if (this.y < this.screenMinY - 30f)
		{
			if (TriggerManager.destroyOffscreenPlayers && base.IsMine)
			{
				this.Gib(DamageType.OutOfBounds, this.xI, 840f);
			}
			this.belowScreenCounter += this.t;
			if (this.belowScreenCounter > 2f && HeroController.CanLookForReposition())
			{
				if (Map.FindLadderNearPosition((this.screenMaxX + this.screenMinX) / 2f, this.screenMinY, ref this.x, ref this.y))
				{
					this.holdUpTime = 0.3f;
					this.yI = 150f;
					this.xI = 0f;
					this.ShowStartBubble();
					if (!GameModeController.IsDeathMatchMode && GameModeController.GameMode != GameMode.BroDown)
					{
						this.SetInvulnerable(2f, false);
					}
				}
				int num = 1;
				if (Map.FindHoleToJumpThroughAndAppear((this.screenMaxX + this.screenMinX) / 2f, this.screenMinY, ref this.x, ref this.y, ref num))
				{
					if (num > 0)
					{
						this.holdRightTime = 0.3f;
					}
					else
					{
						this.holdLeftTime = 0.3f;
					}
					this.yI = 240f;
					this.xI = (float)(num * 70);
					this.ShowStartBubble();
					if (!GameModeController.IsDeathMatchMode && GameModeController.GameMode != GameMode.BroDown)
					{
						this.SetInvulnerable(2f, false);
					}
				}
				this.belowScreenCounter -= 0.5f;
			}
		}
		else
		{
			this.belowScreenCounter = 0f;
		}
	}

	protected Transform GetParentedToTransform()
	{
		if (this.impaledTransform != null)
		{
			return this.impaledTransform;
		}
		if (this.WallDrag || this.wallClimbing)
		{
			return this.wallClimbingWallTransform;
		}
		if (this.actionState != ActionState.Jumping && this.actionState != ActionState.ClimbingLadder)
		{
			return this.groundTransform;
		}
		return null;
	}

	protected Vector3 GetParentedToPos()
	{
		if (this.impaledTransform != null)
		{
			if (this.impaledDirection > 0)
			{
				return new Vector3(Mathf.Max(this.impaledTransform.position.x + this.impaleXOffset, this.impaledPosition.x - 3f), this.impaledTransform.position.y, this.impaledTransform.position.z);
			}
			return new Vector3(Mathf.Min(this.impaledTransform.position.x + this.impaleXOffset, this.impaledPosition.x + 3f), this.impaledTransform.position.y, this.impaledTransform.position.z);
		}
		else
		{
			if (this.WallDrag || this.wallClimbing)
			{
				return this.wallClimbingWallTransformLocalPos;
			}
			if (this.actionState != ActionState.Jumping)
			{
				return this.groundTransformLocalPos;
			}
			UnityEngine.Debug.LogError("How is there a transform and no position??");
			return base.transform.position;
		}
	}

	public override void Impale(Transform impaleTransform, int damage, float xI, float yI, float xOffset)
	{
		this.impaledTransform = impaleTransform;
		this.impaledDamage = damage;
		this.impaledDirection = (int)Mathf.Sign(xI);
		this.impaledPosition = base.transform.position;
		this.impaleXOffset = xOffset;
		this.xI = xI * 0.01f;
		this.yI = yI * 0f;
		this.frame = 0;
		this.counter = 0f;
		this.AnimateImpaled();
		this.parentVelocityInheritM = 1f;
		this.AssignParentedPos();
	}

	public override void Unimpale(int damage, DamageType damageType, float xI, float yI)
	{
		this.impaledDamage = damage;
		this.xI = xI * this.parentVelocityInheritM;
		this.yI = yI * this.parentVelocityInheritM;
		this.impaledTransform = null;
		this.parentVelocityInheritM = 1f;
		this.Damage(this.impaledDamage, damageType, xI * this.parentVelocityInheritM * 0.5f, yI * this.parentVelocityInheritM * 0.5f, (int)Mathf.Sign(xI), this, this.x, this.y + 8f);
	}

	protected void ShiftUnitWithParent()
	{
		Transform parentedToTransform = this.GetParentedToTransform();
		if (parentedToTransform == this.lastParentedToTransform && parentedToTransform != null)
		{
			Vector3 parentedToPos = this.GetParentedToPos();
			if (parentedToTransform.GetComponent<Tank>() != null)
			{
				Vector3 a = parentedToTransform.TransformPoint(parentedToPos);
				this.parentedDiff = a - this.lastParentWorldPos;
			}
			else if (this.impaledTransform != null)
			{
				this.parentedDiff = parentedToPos - this.lastParentWorldPos;
			}
			else
			{
				this.parentedDiff = parentedToTransform.position - this.lastParentWorldPos;
			}
			this.AddParentedDiff(this.parentedDiff.x * this.parentVelocityInheritM, this.parentedDiff.y * this.parentVelocityInheritM);
			if (this.parentedDiff.x != 0f || this.parentedDiff.y != 0f)
			{
				this.parentHasMovedTime = 0.5f;
			}
		}
		else
		{
			if (this.lastParentedToTransform != parentedToTransform && this.lastParentedToTransform != null && parentedToTransform == null && this.lastT != 0f && this.parentedDiff.x != 0f)
			{
				this.xIBlast += this.parentedDiff.x / this.lastT;
				this.yI += this.parentedDiff.y / this.lastT;
			}
			this.parentedDiff = Vector3.zero;
		}
		this.lastParentedToTransform = parentedToTransform;
		this.wallClimbingWallTransform = null;
		this.groundTransform = null;
	}

	protected virtual void AddParentedDiff(float xDiff, float yDiff)
	{
		this.x += xDiff;
		this.y += yDiff;
	}

	protected void AssignParentedPos()
	{
		Transform parentedToTransform = this.GetParentedToTransform();
		if (parentedToTransform != null)
		{
			Vector3 parentedToPos = this.GetParentedToPos();
			if (parentedToTransform.GetComponent<Tank>() != null)
			{
				this.lastParentWorldPos = parentedToTransform.TransformPoint(parentedToPos);
			}
			else if (this.impaledTransform != null)
			{
				this.lastParentWorldPos = parentedToPos;
			}
			else
			{
				this.lastParentWorldPos = parentedToTransform.transform.position;
			}
		}
	}

	protected void LedgeGrapple(bool left, bool right, float radius, float heightOpenOffset)
	{
		if (!left || !right)
		{
			this.yI = 150f;
			RaycastHit raycastHit;
			if (Physics.Raycast(new Vector3(this.x + ((!right) ? 0f : (radius + 3f)) + ((!left) ? 0f : (-radius - 3f)), this.y + heightOpenOffset, 0f), Vector3.down, out raycastHit, 23f, this.groundLayer))
			{
				this.ledgeGrapple = true;
				if (!this.wasLedgeGrapple && !this.fire)
				{
					this.ChangeFrame();
				}
				this.ledgeOffsetY = raycastHit.point.y - this.y;
			}
		}
	}

	public float HalfWidth
	{
		get
		{
			return this.halfWidth;
		}
	}

	protected bool ConstrainToWalls(ref float yIT, ref float xIT)
	{
		if (!this.dashing || (this.left && this.xIBlast > 0f) || (this.right && this.xIBlast < 0f) || (!this.left && !this.right && Mathf.Abs(this.xIBlast) > 0f))
		{
			this.xIBlast *= 1f - this.t * 4f;
		}
		this.pushingTime -= this.t;
		this.wasWallDragging = this.wallDrag;
		bool flag = false;
		this.canTouchRightWalls = false;
		this.canTouchLeftWalls = false;
		this.wallDragTime -= this.t;
		this.wasConstrainedLeft = this.constrainedLeft;
		this.wasConstrainedRight = this.constrainedRight;
		this.constrainedLeft = false;
		this.constrainedRight = false;
		this.ConstrainToFragileBarriers(ref xIT, this.halfWidth);
		this.ConstrainToMookBarriers(ref xIT, this.halfWidth);
		this.row = (int)((this.y + 16f) / 16f);
		this.collumn = (int)((this.x + 8f) / 16f);
		this.wasLedgeGrapple = this.ledgeGrapple;
		this.ledgeGrapple = false;
		if (Physics.Raycast(new Vector3(this.x + 2f, this.y + this.waistHeight, 0f), Vector3.left, out this.raycastHitWalls, 23f, this.groundLayer))
		{
			if (this.raycastHitWalls.point.x > this.x - this.halfWidth - 4f + xIT)
			{
				this.canTouchLeftWalls = true;
			}
			if (this.raycastHitWalls.point.x > this.x - this.halfWidth + xIT)
			{
				this.constrainedLeft = true;
				this.HitLeftWall();
				if (this.airdashDirection == DirectionEnum.Left)
				{
					this.StopAirDashing();
				}
				if (this.actionState == ActionState.Jumping && this.left && !Physics.Raycast(new Vector3(this.x + 2f, this.y + this.headHeight - 6f, 0f), Vector3.left, 23f, this.groundLayer))
				{
					if (!this.down && this.canLedgeGrapple)
					{
						this.LedgeGrapple(this.left, this.right, this.halfWidth, this.headHeight);
					}
					if (Map.IsBlockSolid(this.collumn - 1, this.row - 1) && Map.IsBlockSolid(this.collumn - 1, this.row + 1))
					{
						this.StartDucking();
					}
					this.rollingTime = 0f;
				}
				else if (this.actionState == ActionState.Jumping && this.left && (this.yI <= 100f || this.wallClimbing) && this.canWallClimb && this.y - this.groundHeight > 6f)
				{
					this.AssignWallTransform(this.raycastHitWalls.collider.transform);
					flag = true;
					this.SetAirdashAvailable();
					if (!this.wasWallDragging)
					{
						if (this.yI < 0f)
						{
							this.yI = 0f;
							yIT = 0f;
						}
						this.raycastHitWalls.collider.SendMessage("StepOn", this, SendMessageOptions.DontRequireReceiver);
						if (!this.wallClimbAnticipation)
						{
							this.frame = 0;
						}
						if (!this.useNewKnifeClimbingFrames)
						{
							this.knifeHand++;
						}
						this.ChangeFrame();
					}
					this.DeactivateGun();
					if (!this.wasWallDragging || this.up || this.buttonJump)
					{
						this.wallDragTime = 0.2f;
					}
					this.rollingTime = 0f;
					this.rollingFrames = 0;
					this.doubleJumpsLeft = 0;
					this.ClampWallDragYI(ref yIT);
				}
				if (this.canPushBlocks && (this.actionState == ActionState.Running || this.actionState == ActionState.ClimbingLadder))
				{
					if (base.IsMine)
					{
						Map.PushBlock(this.collumn - 1, this.row, Mathf.Sign(this.xI));
					}
					this.AssignPushingTime();
				}
				this.xI = 0f;
				if (!this.left)
				{
					this.xIBlast = Mathf.Abs(this.xIBlast * 0.4f);
				}
				else
				{
					this.xIBlast = 0f;
				}
				xIT = this.raycastHitWalls.point.x - (this.x - this.halfWidth);
				this.WallDrag = flag;
				return true;
			}
		}
		if (Physics.Raycast(new Vector3(this.x + 2f, this.y + this.toeHeight, 0f), Vector3.left, out this.raycastHitWalls, 23f, this.groundLayer))
		{
			if (this.raycastHitWalls.point.x > this.x - this.halfWidth - 4f + xIT)
			{
				this.canTouchLeftWalls = true;
			}
			if (this.raycastHitWalls.point.x > this.x - this.halfWidth + xIT)
			{
				this.constrainedLeft = true;
				this.HitLeftWall();
				if (this.airdashDirection == DirectionEnum.Left)
				{
					this.StopAirDashing();
				}
				xIT = this.raycastHitWalls.point.x - (this.x - this.halfWidth);
				if (this.actionState == ActionState.Jumping && this.left)
				{
					if (!this.down && this.canLedgeGrapple)
					{
						this.LedgeGrapple(this.left, this.right, this.halfWidth, this.headHeight);
					}
					if (Map.IsBlockSolid(this.collumn - 1, this.row - 1) && Map.IsBlockSolid(this.collumn - 1, this.row + 1))
					{
						this.StartDucking();
					}
					this.rollingTime = 0f;
				}
				else if (this.canDuck && this.actionState == ActionState.Running && this.left && !Map.IsBlockSolid(this.collumn - 1, this.row) && Map.IsBlockSolid(this.collumn - 1, this.row - 1) && Map.IsBlockSolid(this.collumn - 1, this.row + 1))
				{
					this.StartDucking();
				}
				else if (this.actionState == ActionState.Jumping && this.left && (this.yI <= 100f || this.wallClimbing) && this.canWallClimb && this.y - this.groundHeight > 6f)
				{
					this.AssignWallTransform(this.raycastHitWalls.collider.transform);
					flag = true;
					this.SetAirdashAvailable();
					if (!this.wasWallDragging)
					{
						if (this.yI < 0f)
						{
							this.yI = 0f;
							yIT = 0f;
						}
						this.raycastHitWalls.collider.SendMessage("StepOn", this, SendMessageOptions.DontRequireReceiver);
						if (!this.wallClimbAnticipation)
						{
							this.frame = 0;
						}
						if (!this.useNewKnifeClimbingFrames)
						{
							this.knifeHand++;
						}
						this.ChangeFrame();
					}
					this.DeactivateGun();
					if (!this.wasWallDragging || this.up || this.buttonJump)
					{
						this.wallDragTime = 0.2f;
					}
					this.rollingTime = 0f;
					this.rollingFrames = 0;
					this.doubleJumpsLeft = 0;
					this.ClampWallDragYI(ref yIT);
				}
				if (this.canPushBlocks && (this.actionState == ActionState.Running || this.actionState == ActionState.ClimbingLadder))
				{
					if (base.IsMine)
					{
						Map.PushBlock(this.collumn - 1, this.row, Mathf.Sign(this.xI));
					}
					this.AssignPushingTime();
				}
				this.xI = 0f;
				if (!this.left)
				{
					this.xIBlast = Mathf.Abs(this.xIBlast * 0.4f);
				}
				else
				{
					this.xIBlast = 0f;
				}
				xIT = this.raycastHitWalls.point.x - (this.x - this.halfWidth);
				this.WallDrag = flag;
				return true;
			}
		}
		if (Physics.Raycast(new Vector3(this.x + 2f, this.y + this.headHeight - 3f, 0f), Vector3.left, out this.raycastHitWalls, 23f, this.groundLayer) && this.raycastHitWalls.point.x > this.x - this.halfWidth + xIT)
		{
			this.constrainedLeft = true;
			if (this.canDuck && Map.IsBlockSolid(this.collumn - 1, this.row - 1) && Map.IsBlockSolid(this.collumn - 1, this.row + 1))
			{
				this.StartDucking();
			}
			if (this.canPushBlocks && (this.actionState == ActionState.Running || this.actionState == ActionState.ClimbingLadder) && base.IsMine)
			{
				Map.PushBlock(this.collumn - 1, this.row, Mathf.Sign(this.xI));
			}
			this.xI = 0f;
			if (!this.left)
			{
				this.xIBlast = Mathf.Abs(this.xIBlast * 0.4f);
			}
			else
			{
				this.xIBlast = 0f;
			}
			xIT = this.raycastHitWalls.point.x - (this.x - this.halfWidth);
			this.WallDrag = flag;
			return true;
		}
		if (Physics.Raycast(new Vector3(this.x - 2f, this.y + this.waistHeight, 0f), Vector3.right, out this.raycastHitWalls, 23f, this.groundLayer))
		{
			if (this.raycastHitWalls.point.x < this.x + this.halfWidth + 4f + xIT)
			{
				this.canTouchRightWalls = true;
			}
			if (this.raycastHitWalls.point.x < this.x + this.halfWidth + xIT)
			{
				this.constrainedRight = true;
				this.HitRightWall();
				if (this.airdashDirection == DirectionEnum.Right)
				{
					this.StopAirDashing();
				}
				if (this.actionState == ActionState.Jumping && this.right && !Physics.Raycast(new Vector3(this.x - 2f, this.y + this.headHeight - 6f, 0f), Vector3.right, 23f, this.groundLayer))
				{
					if (!this.down && this.canLedgeGrapple)
					{
						this.LedgeGrapple(this.left, this.right, this.halfWidth, this.headHeight);
					}
					if (Map.IsBlockSolid(this.collumn + 1, this.row - 1) && Map.IsBlockSolid(this.collumn + 1, this.row + 1))
					{
						this.StartDucking();
					}
					this.rollingTime = 0f;
				}
				else if (this.actionState == ActionState.Jumping && (this.yI <= 100f || this.wallClimbing) && this.right && this.canWallClimb && this.y - this.groundHeight > 6f)
				{
					this.AssignWallTransform(this.raycastHitWalls.collider.transform);
					flag = true;
					this.SetAirdashAvailable();
					if (!this.wasWallDragging)
					{
						if (this.yI < 0f)
						{
							this.yI = 0f;
							yIT = 0f;
						}
						this.raycastHitWalls.collider.SendMessage("StepOn", this, SendMessageOptions.DontRequireReceiver);
						if (!this.wallClimbAnticipation)
						{
							this.frame = 0;
						}
						if (!this.useNewKnifeClimbingFrames)
						{
							this.knifeHand++;
						}
						this.ChangeFrame();
					}
					this.DeactivateGun();
					if (!this.wasWallDragging || this.up || this.buttonJump)
					{
						this.wallDragTime = 0.2f;
					}
					this.rollingTime = 0f;
					this.rollingFrames = 0;
					this.doubleJumpsLeft = 0;
					this.ClampWallDragYI(ref yIT);
				}
				if (this.canPushBlocks && (this.actionState == ActionState.Running || this.actionState == ActionState.ClimbingLadder))
				{
					if (base.IsMine)
					{
						Map.PushBlock(this.collumn + 1, this.row, Mathf.Sign(this.xI));
					}
					this.AssignPushingTime();
				}
				this.xI = 0f;
				if (!this.right)
				{
					this.xIBlast = -Mathf.Abs(this.xIBlast * 0.4f);
				}
				else
				{
					this.xIBlast = 0f;
				}
				xIT = this.raycastHitWalls.point.x - (this.x + this.halfWidth);
				this.WallDrag = flag;
				return true;
			}
		}
		if (Physics.Raycast(new Vector3(this.x - 2f, this.y + this.toeHeight, 0f), Vector3.right, out this.raycastHitWalls, 23f, this.groundLayer))
		{
			if (this.raycastHitWalls.point.x < this.x + this.halfWidth + 4f + xIT)
			{
				this.canTouchRightWalls = true;
			}
			if (this.raycastHitWalls.point.x < this.x + this.halfWidth + xIT)
			{
				this.constrainedRight = true;
				this.HitRightWall();
				if (this.airdashDirection == DirectionEnum.Right)
				{
					this.StopAirDashing();
				}
				if (this.actionState == ActionState.Jumping && this.right)
				{
					if (!this.down && this.canLedgeGrapple)
					{
						this.LedgeGrapple(this.left, this.right, this.halfWidth, this.headHeight);
					}
					if (Map.IsBlockSolid(this.collumn + 1, this.row - 1) && Map.IsBlockSolid(this.collumn + 1, this.row + 1))
					{
						this.StartDucking();
					}
					this.rollingTime = 0f;
				}
				else if (this.canDuck && this.actionState == ActionState.Running && this.right && !Map.IsBlockSolid(this.collumn + 1, this.row) && Map.IsBlockSolid(this.collumn + 1, this.row - 1) && Map.IsBlockSolid(this.collumn + 1, this.row + 1))
				{
					this.StartDucking();
				}
				else if (this.actionState == ActionState.Jumping && this.right && (this.yI <= 100f || this.wallClimbing) && this.canWallClimb && this.y - this.groundHeight > 6f)
				{
					this.AssignWallTransform(this.raycastHitWalls.collider.transform);
					flag = true;
					this.SetAirdashAvailable();
					if (!this.wasWallDragging)
					{
						if (this.yI < 0f)
						{
							this.yI = 0f;
							yIT = 0f;
						}
						this.raycastHitWalls.collider.SendMessage("StepOn", this, SendMessageOptions.DontRequireReceiver);
						if (!this.wallClimbAnticipation)
						{
							this.frame = 0;
						}
						if (!this.useNewKnifeClimbingFrames)
						{
							this.knifeHand++;
						}
						this.ChangeFrame();
					}
					this.DeactivateGun();
					if (!this.wasWallDragging || this.up || this.buttonJump)
					{
						this.wallDragTime = 0.2f;
					}
					this.rollingTime = 0f;
					this.rollingFrames = 0;
					this.doubleJumpsLeft = 0;
					this.ClampWallDragYI(ref yIT);
				}
				xIT = this.raycastHitWalls.point.x - (this.x + this.halfWidth);
				if (this.canPushBlocks && (this.actionState == ActionState.Running || this.actionState == ActionState.ClimbingLadder))
				{
					if (base.IsMine)
					{
						Map.PushBlock(this.collumn + 1, this.row, Mathf.Sign(this.xI));
					}
					this.AssignPushingTime();
				}
				this.xI = 0f;
				if (!this.right)
				{
					this.xIBlast = -Mathf.Abs(this.xIBlast * 0.4f);
				}
				else
				{
					this.xIBlast = 0f;
				}
				this.WallDrag = flag;
				return true;
			}
		}
		if (Physics.Raycast(new Vector3(this.x - 2f, this.y + this.headHeight - 3f, 0f), Vector3.right, out this.raycastHitWalls, 23f, this.groundLayer) && this.raycastHitWalls.point.x < this.x + this.halfWidth + xIT)
		{
			this.constrainedRight = true;
			if (this.canDuck && Map.IsBlockSolid(this.collumn + 1, this.row - 1) && Map.IsBlockSolid(this.collumn + 1, this.row + 1))
			{
				this.StartDucking();
			}
			if (this.canPushBlocks && (this.actionState == ActionState.Running || this.actionState == ActionState.ClimbingLadder))
			{
				if (base.IsMine)
				{
					Map.PushBlock(this.collumn + 1, this.row, Mathf.Sign(this.xI));
				}
				this.AssignPushingTime();
			}
			this.xI = 0f;
			if (!this.right)
			{
				this.xIBlast = -Mathf.Abs(this.xIBlast * 0.4f);
			}
			else
			{
				this.xIBlast = 0f;
			}
			xIT = this.raycastHitWalls.point.x - (this.x + this.halfWidth);
			this.WallDrag = flag;
			return true;
		}
		this.WallDrag = flag;
		return false;
	}

	protected void CheckClimbAlongCeiling()
	{
		if (this.canCeilingHang && !this.ducking && !this.down && this.actionState == ActionState.Jumping && ((this.wasConstrainedLeft && !this.constrainedLeft) || (this.wasConstrainedRight && !this.constrainedRight)))
		{
			if ((this.up || this.buttonJump) && Physics.Raycast(new Vector3(this.x, this.y + 5f, 0f), Vector3.up, out this.raycastHitWalls, this.headHeight + 16f, this.groundLayer))
			{
				this.hangGrace = 0.4f;
			}
			else if (this.right && Physics.Raycast(new Vector3(this.x + this.halfWidth + 4f, this.y + 5f, 0f), Vector3.up, out this.raycastHitWalls, this.headHeight + 14f, this.groundLayer))
			{
				this.hangGrace = 0.3f;
			}
			else if (this.left && Physics.Raycast(new Vector3(this.x - this.halfWidth - 4f, this.y + 5f, 0f), Vector3.up, out this.raycastHitWalls, this.headHeight + 14f, this.groundLayer))
			{
				this.hangGrace = 0.3f;
			}
		}
	}

	protected virtual void AssignPushingTime()
	{
		this.pushingTime = 0.06f;
		this.rollingFrames = 0;
		this.rollingTime = 0f;
	}

	protected virtual void StopAirDashing()
	{
		this.chimneyFlipFrames = 0;
		this.chimneyFlip = false;
		this.airdashTime = 0f;
		this.airDashDelay = 0f;
	}

	protected virtual void ClampWallDragYI(ref float yIT)
	{
		if (this.wallDragTime > 0f)
		{
			if (yIT < 0f)
			{
				yIT = 0f;
			}
			if (this.yI < 0f)
			{
				this.yI = 0f;
			}
		}
		if (this.yI < -40f)
		{
			this.yI = -40f;
		}
	}

	protected virtual void ConstrainToFragileBarriers(ref float xIT, float radius)
	{
		if (!Map.isEditing)
		{
			if (xIT < 0f && Physics.Raycast(new Vector3(this.x + 2f, this.y + this.waistHeight, 0f), Vector3.left, out this.raycastHit, radius + Mathf.Abs(xIT), this.fragileLayer) && this.raycastHit.collider.gameObject.GetComponent<Parachute>() == null)
			{
				EffectsController.CreateProjectilePuff(this.raycastHit.point.x, this.raycastHit.point.y);
				this.raycastHit.collider.gameObject.SendMessage("Damage", new DamageObject(1, DamageType.Melee, this.xI, this.yI, this));
			}
			if (xIT > 0f && Physics.Raycast(new Vector3(this.x - 2f, this.y + this.waistHeight, 0f), Vector3.right, out this.raycastHit, radius + xIT, this.fragileLayer) && this.raycastHit.collider.gameObject.GetComponent<Parachute>() == null)
			{
				EffectsController.CreateProjectilePuff(this.raycastHit.point.x, this.raycastHit.point.y);
				this.raycastHit.collider.gameObject.SendMessage("Damage", new DamageObject(1, DamageType.Melee, this.xI, this.yI, this));
			}
			if (this.yI < -25f)
			{
				if (Physics.Raycast(new Vector3(this.x, this.y + this.waistHeight, 0f), Vector3.down, out this.raycastHit, this.waistHeight - this.yI * this.t, this.fragileLayer) && (this.raycastHit.collider.gameObject.GetComponent<Parachute>() == null || this.playerNum < 0))
				{
					EffectsController.CreateProjectilePuff(this.raycastHit.point.x, this.raycastHit.point.y);
					this.raycastHit.collider.gameObject.SendMessage("Damage", new DamageObject(1, DamageType.Melee, this.xI, this.yI, this));
				}
			}
			else if (this.yI > 0f)
			{
				if (Physics.Raycast(new Vector3(this.x, this.y + 2f, 0f), Vector3.up, out this.raycastHit, this.headHeight - 2f + this.yI * this.t, this.fragileLayer) && this.raycastHit.collider.gameObject.GetComponent<Parachute>() == null)
				{
					EffectsController.CreateProjectilePuff(this.raycastHit.point.x, this.raycastHit.point.y);
					this.raycastHit.collider.gameObject.SendMessage("Damage", new DamageObject(1, DamageType.Melee, this.xI, this.yI, this));
				}
			}
			else if (this.playerNum >= 0 && Physics.Raycast(new Vector3(this.x, this.y + 4f, 0f), Vector3.down, out this.raycastHit, 5f + Mathf.Abs(this.yI) * this.t, this.fragileLayer))
			{
				Parachute component = this.raycastHit.collider.gameObject.GetComponent<Parachute>();
				if (component != null)
				{
					component.Deform();
				}
			}
			if (this.health > 0)
			{
				if (this.xI < -25f && Physics.Raycast(new Vector3(this.x + 12f - xIT, this.y + this.waistHeight, 0f), Vector3.left, out this.raycastHit, 5.0001f + Mathf.Abs(xIT), this.openDoorsLayer) && this.raycastHit.collider.gameObject.GetComponent<Parachute>() == null)
				{
					this.raycastHit.collider.gameObject.SendMessage("Use", new DamageObject(1, DamageType.Melee, -10f, this.yI, this));
				}
				if (this.xI > 25f && Physics.Raycast(new Vector3(this.x - 12f - xIT, this.y + this.waistHeight, 0f), Vector3.right, out this.raycastHit, 5.0001f + xIT, this.openDoorsLayer) && this.raycastHit.collider.gameObject.GetComponent<Parachute>() == null)
				{
					this.raycastHit.collider.gameObject.SendMessage("Use", new DamageObject(1, DamageType.Melee, 10f, this.yI, this));
				}
			}
		}
	}

	protected virtual void ConstrainToMookBarriers(ref float xIT, float radius)
	{
		if (xIT >= 0f || !Physics.Raycast(new Vector3(this.x + 2f, this.y + this.waistHeight, 0f), Vector3.left, out this.raycastHit, 23f, this.barrierLayer) || this.raycastHit.point.x <= this.x - radius + xIT)
		{
			if (xIT > 0f && Physics.Raycast(new Vector3(this.x - 2f, this.y + this.waistHeight, 0f), Vector3.right, out this.raycastHit, 23f, this.barrierLayer))
			{
				if (this.playerNum >= 0 && this.raycastHit.collider.GetComponent<Unit>() != null && this.raycastHit.collider.GetComponent<Unit>().playerNum >= 0)
				{
					return;
				}
				if (this.raycastHit.point.x < this.x + radius + xIT)
				{
					this.xI = 0f;
					xIT = this.raycastHit.point.x - (this.x + radius);
					return;
				}
			}
			return;
		}
		if (this.playerNum >= 0 && this.raycastHit.collider.GetComponent<Unit>() != null && this.raycastHit.collider.GetComponent<Unit>().playerNum >= 0)
		{
			return;
		}
		this.xI = 0f;
		xIT = this.raycastHit.point.x - (this.x - radius);
	}

	protected void StartDucking()
	{
		if (this.canDuck)
		{
			this.ducking = true;
			this.headHeight = this.duckingHeadHeight;
			this.waistHeight = this.duckingWaistHeight;
			this.toeHeight = this.duckingToeHeight;
			if (this.actionState == ActionState.Hanging)
			{
				this.actionState = ActionState.Jumping;
			}
			this.hangGrace = 0f;
			this.ChangeFrame();
			this.StopAirDashing();
		}
	}

	protected void StopDucking()
	{
		bool flag = this.down && this.IsStandingStill;
		if (this.ducking && !flag)
		{
			this.ducking = false;
			this.headHeight = this.standingHeadHeight;
			this.waistHeight = this.standingWaistHeight;
			this.toeHeight = this.standingToeHeight;
		}
	}

	private bool RaycastGroundSingleSquare(Vector3 direction)
	{
		return Physics.Raycast(new Vector3(this.x, this.y + 8f, 0f), direction, 12f, this.groundLayer);
	}

	protected void CheckDucking()
	{
		Vector3 vector = new Vector3(this.x, this.y + 8f, 0f);
		if (this.ducking)
		{
			if (this.RaycastGroundSingleSquare(Vector3.up) && this.RaycastGroundSingleSquare(Vector3.down))
			{
				return;
			}
			if (base.transform.localScale.x < 0f)
			{
				bool flag = false;
				bool flag2 = false;
				if (this.RaycastGroundSingleSquare(Vector3.left) && this.RaycastGroundSingleSquare(Vector3.up))
				{
					flag = true;
				}
				else if (!this.RaycastGroundSingleSquare(Vector3.left) && (this.RaycastGroundSingleSquare(Vector3.up) || this.RaycastGroundSingleSquare(Vector3.up + Vector3.left)))
				{
					flag2 = true;
				}
				if (!flag && !flag2)
				{
					this.StopDucking();
				}
			}
			if (base.transform.localScale.x > 0f)
			{
				bool flag3 = false;
				bool flag4 = false;
				if (this.RaycastGroundSingleSquare(Vector3.up) && this.RaycastGroundSingleSquare(Vector3.down))
				{
					flag3 = true;
				}
				else if (!this.RaycastGroundSingleSquare(Vector3.right) && (this.RaycastGroundSingleSquare(Vector3.up) || this.RaycastGroundSingleSquare(Vector3.up + Vector3.right)))
				{
					flag4 = true;
				}
				if (!flag3 && !flag4)
				{
					this.StopDucking();
				}
			}
		}
		else
		{
			this.row = (int)(this.y + 16f) / 16;
			this.collumn = (int)(this.x + 8f) / 16;
			if (this.RaycastGroundSingleSquare(Vector3.up) && this.RaycastGroundSingleSquare(Vector3.down))
			{
				this.StartDucking();
			}
			else if (!Map.IsBlockSolid(this.collumn, this.row - 1) || Map.IsBlockSolid(this.collumn, this.row + 1))
			{
			}
			if (base.transform.localScale.x > 0f && this.right)
			{
				if (this.RaycastGroundSingleSquare(Vector3.right + Vector3.down) && !this.RaycastGroundSingleSquare(Vector3.right) && (this.RaycastGroundSingleSquare(Vector3.up + Vector3.right) || this.RaycastGroundSingleSquare(Vector3.up)))
				{
					this.StartDucking();
				}
				else if (this.actionState == ActionState.Running && !this.RaycastGroundSingleSquare(Vector3.right) && (this.RaycastGroundSingleSquare(Vector3.up) || this.RaycastGroundSingleSquare(Vector3.up + Vector3.right)))
				{
					this.StartDucking();
				}
			}
			if (base.transform.localScale.x < 0f && this.left)
			{
				if (this.RaycastGroundSingleSquare(Vector3.left + Vector3.down) && !this.RaycastGroundSingleSquare(Vector3.left) && (this.RaycastGroundSingleSquare(Vector3.up + Vector3.left) || this.RaycastGroundSingleSquare(Vector3.up)))
				{
					this.StartDucking();
				}
				else if (this.actionState == ActionState.Running && !this.RaycastGroundSingleSquare(Vector3.left) && (this.RaycastGroundSingleSquare(Vector3.up) || this.RaycastGroundSingleSquare(Vector3.up + Vector3.left)))
				{
					this.StartDucking();
				}
			}
		}
	}

	protected virtual void CheckTriggerActionInput()
	{
		this.wasUp = this.up;
		this.wasButtonJump = this.buttonJump;
		this.wasDown = this.down;
		this.wasLeft = this.left;
		this.wasRight = this.right;
		this.wasFire = this.fire;
		this.wasSpecial = this.special;
		this.wasHighFive = this.highFive;
		this.wasButtonTaunt = this.buttonTaunt;
		this.wasUsingSpecial2 = this.usingSpecial2;
		this.wasUsingSpecial3 = this.usingSpecial3;
		this.wasUsingSpecial4 = this.usingSpecial4;
		if (this.health <= 0)
		{
			return;
		}
		CharacterCommand currentCommand = this.controllingTriggerAction.GetCurrentCommand(this);
		if (currentCommand != null)
		{
			if (currentCommand.type == CharacterCommandType.Move)
			{
				this.special = false; this.fire = (this.special );
				if (base.GetComponent<PathAgent>() != null)
				{
					if ((base.GetComponent<PathAgent>().CurrentPath == null || base.GetComponent<PathAgent>().CurrentPath.TargetPoint != currentCommand.target) && Time.time - this.lastPathingTime > 0.1f)
					{
						base.GetComponent<PathAgent>().GetPath(currentCommand.target.CollumnAdjusted, currentCommand.target.RowAdjusted, 15f);
						this.lastPathingTime = Time.time;
					}
					base.GetComponent<PathAgent>().GetMove(ref this.left, ref this.right, ref this.up, ref this.down, ref this.buttonJump);
					if (this.collumn == currentCommand.target.CollumnAdjusted && this.row == currentCommand.target.RowAdjusted)
					{
						this.controllingTriggerAction.CompleteCurrentCommand(this);
					}
				}
			}
			else if (currentCommand.type == CharacterCommandType.AICommand)
			{
				if (base.GetComponent<PolymorphicAI>() != null)
				{
					this.CheckInput();
				}
				else
				{
					this.controllingTriggerAction.CompleteCurrentCommand(this);
				}
			}
		}
	}

	protected virtual void CheckInput()
	{
		this.wasUp = this.up;
		this.wasButtonJump = this.buttonJump;
		this.wasDown = this.down;
		this.wasLeft = this.left;
		this.wasRight = this.right;
		this.wasFire = this.fire;
		this.wasSpecial = this.special;
		this.wasHighFive = this.highFive;
		this.wasButtonTaunt = this.buttonTaunt;
		this.wasUsingSpecial2 = this.usingSpecial2;
		this.wasUsingSpecial3 = this.usingSpecial3;
		this.wasUsingSpecial4 = this.usingSpecial4;
		if (this.isZombie && this.reviveSource != null)
		{
			this.CalculateZombieInput();
		}
		else if (base.IsHero && this.playerNum >= 0 && this.playerNum < 4)
		{
			if (!base.IsMine)
			{
				base.RunSync();
			}
			else if (LevelEditorGUI.levelEditorActive)
			{
				InputReader.GetCombinedInput(ref this.up, ref this.down, ref this.left, ref this.right, ref this.fire, ref this.buttonJump, ref this.special, ref this.highFive);
			}
			else
			{
				HeroController.players[this.playerNum].GetInput(ref this.up, ref this.down, ref this.left, ref this.right, ref this.fire, ref this.buttonJump, ref this.special, ref this.highFive);
			}
			if (GameModeController.IsDeathMatchMode && GameModeController.InRewardPhase() && this.playerNum != GameModeController.GetWinnerNum())
			{
				this.fire = false;
				this.special = false;
			}
			float num = 10f;
			float num2 = 15f;
			if (HeroController.GetPlayersPlayingCount() > 2)
			{
				num = 1f;
			}
			if (this.up || this.down || this.left || this.right || this.fire || this.buttonJump || this.special || this.highFive || this.health <= 0)
			{
				this.idleTime = 0f;
				this.StopBubble();
				this.ElgilbleToBeKicked = false;
				this.StopKickBubble();
			}
			else
			{
				this.idleTime += Time.deltaTime;
				if (HeroController.GetPlayerAliveNum() > 1 && this.idleTime >= num2 && this.playerBubble.IsHidden && !Connect.IsOffline)
				{
					this.ElgilbleToBeKicked = true;
				}
				if (this.idleTime >= num && this.playerBubble.IsHidden && !this.ElgilbleToBeKicked)
				{
					this.ShowStartBubble();
				}
			}
			if (this.left && this.right)
			{
				this.left = false;
				this.right = false;
			}
			if (Application.isEditor || PlaytomicController.isExhibitionBuild)
			{
				if (Input.GetKeyDown(KeyCode.F4))
				{
					Satan satan = UnityEngine.Object.FindObjectOfType(typeof(Satan)) as Satan;
					if (satan != null && satan.health > 0)
					{
						this.x = satan.x + 8f + (float)(this.playerNum * 8);
						this.y = satan.y + 12f;
					}
				}
				if (Input.GetKeyDown(KeyCode.F2))
				{
				}
			}
			if (Input.GetKeyDown(KeyCode.F4) && GameModeController.GameMode == GameMode.ExplosionRun)
			{
				if (Input.GetKeyDown(KeyCode.F4))
				{
					Satan satan2 = UnityEngine.Object.FindObjectOfType(typeof(Satan)) as Satan;
					if (satan2 != null && satan2.health > 0)
					{
						this.x = satan2.x + 8f + (float)(this.playerNum * 8);
						this.y = satan2.y + 12f;
					}
				}
				Map.nextLevelToLoad++;
			}
		}
		if (this.remoteProjectile != null && this.remoteProjectile.gameObject.activeInHierarchy)
		{
			if (this.up)
			{
				this.remoteProjectile.SetSpeed(0f, this.remoteProjectile.GetSuggestedSpeed());
			}
			if (this.down)
			{
				this.remoteProjectile.SetSpeed(0f, -this.remoteProjectile.GetSuggestedSpeed());
			}
			if (this.left)
			{
				this.remoteProjectile.SetSpeed(-this.remoteProjectile.GetSuggestedSpeed(), 0f);
			}
			if (this.right)
			{
				this.remoteProjectile.SetSpeed(this.remoteProjectile.GetSuggestedSpeed(), 0f);
			}
			this.up = false;
			this.left = false;
			this.right = false;
			this.down = false;
			this.buttonJump = false;
			if (Time.time - this.projectileTime > 0.56f && ((this.usingSpecial && !this.wasSpecial) || (this.fire && !this.wasFire)))
			{
				this.controllingProjectile = false;
				this.usingSpecial = false;
				this.remoteProjectile.Death();
			}
			this.usingSpecial = false;
			this.fire = false;
			this.controllingProjectileDelay = 0.25f;
		}
		else
		{
			if (this.controllingProjectileDelay > 0f)
			{
				this.controllingProjectileDelay -= this.t;
				this.up = false;
				this.left = false;
				this.right = false;
				this.down = false;
				this.usingSpecial = false;
				this.fire = false;
			}
			this.controllingProjectile = false;
		}
		if (Map.isEditing)
		{
			this.wasHighFive = false; this.fire = (this.special = (this.wasFire = (this.wasSpecial = (this.specialDown = (this.wasSpecialDown = (this.highFive = (this.wasHighFive )))))));
		}
		if ((this.playerNum < 0 || this.playerNum > 3 || this.IsEvil()) && this.health > 0)
		{
			this.GetEnemyMovement();
		}
		if (base.GetComponent<CongaBrain>() != null && base.GetComponent<CongaBrain>().leadingBro != null)
		{
			base.GetComponent<CongaBrain>().GetInput(ref this.left, ref this.right, ref this.up, ref this.down, ref this.buttonJump, ref this.fire);
		}
		if (this.buttonTaunt && !this.wasButtonTaunt)
		{
			this.wasButtonTaunt = false;
		}
		if (this.special && !this.wasSpecial && this.ignoreHighFivePressTime <= 0f)
		{
			this.PressSpecial();
		}
		if (!this.special && this.wasSpecial)
		{
			this.ReleaseSpecial();
		}
		if (this.highFive && !this.wasHighFive && this.ignoreHighFivePressTime <= 0f)
		{
			this.PressHighFiveMelee(false);
		}
		if (!this.highFive && this.wasHighFive)
		{
			this.ReleaseHighFive();
		}
		if (!this.fire && this.wasFire)
		{
			this.ReleaseFire();
		}
		if (this.doingMelee && !this.canDoIndependentMeleeAnimation)
		{
			this.special = false; this.up = (this.down = (this.fire = (this.buttonJump = (this.special ))));
		}
		if (!this.IsAlive())
		{
			this.ClearFireInput();
		}
		if (this.dashingMelee && this.frame < 4 && !this.meleeFollowUp)
		{
			if (base.transform.localScale.x > 0f)
			{
				this.left = false;
				this.right = true;
			}
			else
			{
				this.left = true;
				this.right = false;
			}
		}
		if (this.holdUpTime > 0f)
		{
			this.holdUpTime -= this.t;
			if (this.holdUpTime <= 0f)
			{
				this.up = false;
			}
			else
			{
				this.up = true;
				this.down = false;
				this.left = false;
				this.right = false;
			}
		}
		if (this.holdLeftTime > 0f)
		{
			this.holdLeftTime -= this.t;
			if (this.holdLeftTime <= 0f)
			{
				this.left = false;
				this.up = false;
			}
			else
			{
				this.left = true;
				this.down = false;
				this.up = true;
				this.right = false;
			}
		}
		if (this.holdRightTime > 0f)
		{
			this.holdRightTime -= this.t;
			if (this.holdRightTime <= 0f)
			{
				this.right = false;
				this.up = false;
			}
			else
			{
				this.right = true;
				this.down = false;
				this.up = true;
				this.left = false;
			}
		}
		if (this.usingSpecial2 && !this.wasUsingSpecial2)
		{
			this.frame = 0;
		}
		if (this.usingSpecial3 && !this.wasUsingSpecial3)
		{
			this.frame = 0;
		}
		if (this.usingSpecial4 && !this.wasUsingSpecial4)
		{
			this.frame = 0;
		}
	}

	protected virtual void GetEnemyMovement()
	{
		if (this.enemyAI != null)
		{
			this.enemyAI.GetInput(ref this.left, ref this.right, ref this.up, ref this.down, ref this.buttonJump, ref this.fire, ref this.usingSpecial, ref this.usingSpecial2, ref this.usingSpecial3, ref this.usingSpecial4);
		}
		else if (base.GetComponent<EnemyAI>() != null)
		{
			base.GetComponent<EnemyAI>().GetMovement(ref this.left, ref this.right, ref this.up, ref this.down, ref this.buttonJump, ref this.fire, ref this.usingSpecial, ref this.usingSpecial2, ref this.usingSpecial3, ref this.usingSpecial4);
		}
	}

	public override bool IsPressingDown()
	{
		return this.down;
	}

	protected virtual void PressSpecial()
	{
		if (this.actionState != ActionState.Melee)
		{
			this.usingSpecial = true;
			this.frame = 0;
		}
	}

	protected virtual void ReleaseSpecial()
	{
	}

	protected virtual void StartHighFive()
	{
		this.showHighFiveAfterMeleeTimer = 0f;
		this.highfiveHoldTime = 0f;
		this.holdingHighFive = true;
		this.frame = 0;
	}

	protected virtual void StartMelee()
	{
		this.showHighFiveAfterMeleeTimer = 0f;
		this.DeactivateGun();
		this.SetMeleeType();
		this.doingMelee = true;
		this.meleeHasHit = false;
		this.actionState = ActionState.Melee;
		if (!this.doingMelee || this.frame > 3)
		{
			this.frame = 0;
			this.counter = -0.05f;
			this.AnimateMelee();
		}
		else
		{
			this.meleeFollowUp = true;
		}
	}

	protected virtual void SetMeleeType()
	{
		if (!this.useNewKnifingFrames)
		{
			this.standingMelee = true;
			this.jumpingMelee = false;
			this.dashingMelee = false;
		}
		else if (this.actionState == ActionState.Jumping || this.y > this.groundHeight + 1f)
		{
			this.standingMelee = false;
			this.jumpingMelee = true;
			this.dashingMelee = false;
		}
		else if (this.right || this.left)
		{
			this.standingMelee = false;
			this.jumpingMelee = false;
			this.dashingMelee = true;
		}
		else if (this.actionState == ActionState.ClimbingLadder)
		{
			this.standingMelee = false;
			this.jumpingMelee = true;
			this.dashingMelee = false;
		}
		else
		{
			this.standingMelee = true;
			this.jumpingMelee = false;
			this.dashingMelee = false;
		}
	}

	protected virtual void PressHighFiveMelee(bool forceHighFive = false)
	{
		if (this.health <= 0)
		{
			return;
		}
		if (this.heldGrenade != null || this.usingSpecial || this.usingSpecial2 || this.usingSpecial3 || this.usingSpecial4)
		{
			return;
		}
		Grenade nearbyGrenade = Map.GetNearbyGrenade(this.playerNum, 20f, this.x, this.y + this.waistHeight);
		Switch nearbySwitch = Map.GetNearbySwitch(this.x, this.y);
		if (GameModeController.IsDeathMatchMode || GameModeController.GameMode == GameMode.BroDown)
		{
			for (int i = -1; i < 4; i++)
			{
				if (i != this.playerNum && Map.IsUnitNearby(i, this.x + base.transform.localScale.x * 16f, this.y + 8f, 28f, 14f, true, out this.meleeChosenUnit))
				{
					this.StartMelee();
				}
			}
		}
		if (nearbyGrenade != null)
		{
			this.dashingMelee = false; this.doingMelee = (this.dashingMelee );
			this.ThrowBackGrenade(nearbyGrenade);
		}
		else if (!GameModeController.IsDeathMatchMode || !this.doingMelee)
		{
			if (forceHighFive && !this.doingMelee)
			{
				this.StartHighFive();
			}
			else if (Map.IsUnitNearby(-1, this.x + base.transform.localScale.x * 16f, this.y + 8f, 28f, 14f, false, out this.meleeChosenUnit))
			{
				this.StartMelee();
			}
			else if (nearbySwitch != null)
			{
				nearbySwitch.Activate(this);
			}
			else if (this.CheckBustCage())
			{
				this.StartMelee();
			}
			else if (HeroController.IsAnotherPlayerNearby(this.playerNum, this.x, this.y, 32f, 32f))
			{
				if (!this.doingMelee)
				{
					this.StartHighFive();
				}
			}
			else
			{
				this.StartMelee();
			}
		}
	}

	protected virtual void ThrowBackGrenade(Grenade grenade)
	{
		this.heldGrenade = grenade;
		this.heldGrenade.enabled = false;
		this.heldGrenade.transform.parent = base.transform;
		this.heldGrenade.transform.localScale = Vector3.one;
		this.throwingGrenade = true;
		this.PlayThrowSound(0.4f);
		this.frame = 0;
		this.counter = 0f;
		this.AnimateThrowingGrenade();
	}

	protected virtual bool CheckBustCage()
	{
		if (Physics.Raycast(new Vector3(this.x - base.transform.localScale.x * 4f, this.y + 4f, 0f), new Vector3(base.transform.localScale.x, 0f, 0f), out this.raycastHit, 16f, this.groundLayer))
		{
			if (this.raycastHit.collider.GetComponent<Cage>() != null)
			{
				return true;
			}
			if (this.raycastHit.collider.transform.parent != null && this.raycastHit.collider.transform.parent.GetComponent<Cage>() != null)
			{
				return true;
			}
		}
		return false;
	}

	protected virtual bool TryBustCage()
	{
		if (Physics.Raycast(new Vector3(this.x - base.transform.localScale.x * 4f, this.y + 4f, 0f), new Vector3(base.transform.localScale.x, 0f, 0f), out this.raycastHit, 16f, this.groundLayer))
		{
			Cage component = this.raycastHit.collider.GetComponent<Cage>();
			if (component == null && this.raycastHit.collider.transform.parent != null)
			{
				component = this.raycastHit.collider.transform.parent.GetComponent<Cage>();
			}
			if (component != null)
			{
				MapController.Damage_Networked(this, this.raycastHit.collider.gameObject, component.health, DamageType.Melee, 0f, 40f);
				return true;
			}
		}
		return false;
	}

	protected virtual void ReleaseFire()
	{
	}

	protected virtual void ReleaseHighFive()
	{
		if (this.holdingHighFive)
		{
			this.holdingHighFive = false;
			this.releasingHighFive = true;
		}
	}

	protected virtual void ClearFireInput()
	{
		this.fire = false;
		this.usingSpecial = false;
		this.special = false;
		this.wasSpecial = false;
		this.highFive = false;
		this.wasHighFive = false;
	}

	protected virtual void ClearAllInput()
	{
		this.up = false; this.wasUp = (this.up );
		this.buttonJump = false; this.wasButtonJump = (this.buttonJump );
		this.down = false; this.wasDown = (this.down );
		this.left = false; this.wasLeft = (this.left );
		this.right = false; this.wasRight = (this.right );
		this.fire = false; this.wasFire = (this.fire );
		this.special = false; this.wasSpecial = (this.special );
		this.highFive = false; this.wasHighFive = (this.highFive );
		this.buttonTaunt = false; this.wasButtonTaunt = (this.buttonTaunt );
		this.wasButtonJump = false; this.buttonJump = (this.wasButtonJump );
		this.buttonHighFive = false; this.wasButtonHighFive = (this.buttonHighFive );
	}

	protected virtual void RunIndependentMeleeFrames()
	{
	}

	protected virtual void RunGun()
	{
		if (!this.WallDrag && this.gunFrame > 0)
		{
			this.gunCounter += this.t;
			if (this.gunCounter > 0.0334f)
			{
				this.gunCounter -= 0.0334f;
				this.gunFrame--;
				this.SetGunSprite(this.gunFrame, 0);
			}
		}
	}

	protected virtual void SetGunSprite(int spriteFrame, int spriteRow)
	{
		if (this.actionState == ActionState.ClimbingLadder && this.hangingOneArmed)
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * (6 + spriteFrame)), (float)(this.gunSpritePixelHeight * (1 + spriteRow)));
		}
		else if (this.attachedToZipline != null && this.actionState == ActionState.Jumping)
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * (6 + spriteFrame)), (float)(this.gunSpritePixelHeight * (1 + spriteRow)));
		}
		else
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * spriteFrame), (float)(this.gunSpritePixelHeight * (1 + spriteRow)));
		}
	}

	protected virtual void FallDamage(float yI)
	{
	}

	public override void Stun(float time)
	{
		this.Stop();
		this.stunTime = time;
		if (this.blindCounter < 0f)
		{
			this.blindCounter = 0f;
		}
	}

	public override void Blind(float time)
	{
		this.Stop();
		this.stunTime = time;
	}

	public override void Blind()
	{
		this.Stop();
		this.stunTime = 3f;
	}

	protected virtual void DealWithBounce(ref DamageType damageType, ref int damage)
	{
		if (this.playerNum >= 0)
		{
			damage = 0;
			this.Blind(0.25f);
		}
		else
		{
			damageType = DamageType.Crush;
		}
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		bool flag = this.health > 0;
		if (damageType == DamageType.Bounce)
		{
			this.DealWithBounce(ref damageType, ref damage);
		}
		if ((base.IsMine && base.IsHero) || !base.IsHero)
		{
			if (this.playerNum >= 0 && this.actionState == ActionState.Jumping && yI > 0f && this.yI > 0f && this.up && (damageType == DamageType.Explosion || damageType == DamageType.Fire) && !GameModeController.IsDeathMatchMode && GameModeController.GameMode != GameMode.BroDown)
			{
				this.yI += yI * 0.25f;
			}
			else
			{
				this.health -= damage;
			}
		}
		if (damageType == DamageType.Knock && this.health < -2)
		{
			this.health = -2;
			damage = 1;
		}
		if (this.health <= 0 && this.health + damage > 0 && this.playerNum >= 0 && this.playerNum < 5 && damageSender != null)
		{
			Unit component = damageSender.GetComponent<Unit>();
			if (component != null && component.playerNum >= 0 && component.playerNum < 5 && ((GameModeController.IsDeathMatchMode && HeroController.GetPlayerLives(this.playerNum) <= 1) || GameModeController.GameMode == GameMode.BroDown))
			{
				TimeController.StopTime(0.3f, 0.2f, 0.4f, false, false);
			}
		}
		if (this.health < -10 && damageType != DamageType.Spikes && this.canGib)
		{
			this.Gib(damageType, this.xI + xI + this.xIBlast, this.yI + yI + this.yIBlast);
		}
		else if (this.health <= 0 && this.health + damage > 0)
		{
			this.PlayDeathSound();
		}
		else if (damageType == DamageType.Fall || damageType == DamageType.Knock)
		{
			this.PlayFallDamageSound();
		}
		else if (damageType == DamageType.Fire)
		{
			this.PlayBurnSound();
		}
		else if (damageType != DamageType.Melee && damageType != DamageType.Knifed && damageType != DamageType.Bite && damageType != DamageType.Plasma)
		{
			this.PlayHitSound();
		}
		if (damageType == DamageType.Bullet)
		{
			float y = Mathf.Clamp(hitY, this.y + 3f, this.y + this.headHeight - 2f);
			EffectsController.CreateBloodGushEffect(this.bloodColor, this.x, y, xI * 0.2f, yI * 0.5f + 24f);
		}
		if (damageType == DamageType.Melee || damageType == DamageType.Knifed)
		{
			EffectsController.CreateBloodParticles(this.bloodColor, this.x, this.y + 6f, 4, 4f, 5f, 60f, this.xI * 0.2f, this.yI * 0.5f + 40f);
			EffectsController.CreateMeleeStrikeEffect(this.x, this.y + 8f, xI * 0.2f, yI * 0.5f + 24f);
		}
		if (this.health <= 0)
		{
			if (this.deathType == DeathType.Unassigned)
			{
				this.deathTime = Time.time;
				this.SetDeathType(damageType, this.health);
			}
			this.Death(xI, yI, new DamageObject(damage, damageType, xI, yI, damageSender));
		}
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		base.SetSyncingInternal(false);
		if (this.health > 0)
		{
			this.health = 0;
		}
		this.reviveSource = null;
		this.StopPlayerBubbles();
		if (this.actionState != ActionState.Dead && !this.isZombie)
		{
			HeroController.SetAvatarDead(this.playerNum, this.usePrimaryAvatar);
			this.ReduceLives(false);
		}
		this.xI = xI * 0.3f;
		if (yI < 0f)
		{
			yI = 0f;
		}
		this.yI = yI * 0.3f;
		this.DeactivateGun();
		this.yI += 20f;
		if (damage != null && damage.damageType != DamageType.Chainsaw)
		{
			this.y += 5f;
		}
		if (damage != null && damage.damageType != DamageType.SelfEsteem)
		{
			EffectsController.CreateBloodParticles(this.bloodColor, this.x, this.y + 6f, 10, 4f, 5f, 60f, this.xI * 0.5f, this.yI * 0.5f);
		}
		this.ReleaseGrenade(false);
		base.Death(xI, yI, damage);
	}

	protected void StopPlayerBubbles()
	{
		if (this.playerNum > -1 && this.player1Bubble != null)
		{
			this.player1Bubble.StopBubble();
		}
	}

	protected virtual void DeactivateGun()
	{
		if (this.gunSprite != null)
		{
			this.gunSprite.gameObject.SetActive(false);
		}
	}

	protected virtual void ActivateGun()
	{
		if (this.gunSprite != null)
		{
			this.gunSprite.gameObject.SetActive(true);
		}
	}

	public override void SetVelocity(DamageType damageType, float xI, float xIBlast, float yIBlast)
	{
		this.xI = Mathf.Clamp(this.xI + xI, -200f, 200f);
		this.xIBlast = Mathf.Clamp(xIBlast, -200f, 200f);
		this.yI = Mathf.Clamp(this.yI + yIBlast, -20000f, 300f);
	}

	public override void Knock(DamageType damageType, float xI, float yI, bool forceTumble)
	{
		this.xI = Mathf.Clamp(this.xI + xI / 2f, -200f, 200f);
		this.xIBlast = Mathf.Clamp(this.xIBlast + xI / 2f, -200f, 200f);
		this.yI = Mathf.Clamp(this.yI + yI, -20000f, 300f);
	}

	public override void KnockSimple(DamageObject damageObject)
	{
		this.xIBlast = Mathf.Clamp(this.xIBlast + damageObject.xForce, -200f, 200f);
		this.yI = Mathf.Clamp(this.yI + damageObject.yForce, -20000f, 300f);
	}

	protected virtual void NotifyDeathType()
	{
		if (!this.hasNotifiedDeathType)
		{
			this.hasNotifiedDeathType = true;
			StatisticsController.NotifyMookDeathType(this, this.deathType);
		}
	}

	public virtual bool CheckPlasmaDeath(ref float xGibI, ref float yGibI)
	{
		if (this.plasmaDamage >= 1)
		{
			yGibI += 100f;
			EffectsController.CreateWhiteFlashPop(this.x, this.y + 8f);
			MapController.DamageGround(this, 15, DamageType.Plasma, 28f, this.x, this.y, null);
			Map.ExplodeUnits(this, 15, DamageType.Plasma, 44f, 32f, this.x, this.y, 150f, 90f, 15, false, true);
			Vector3 a = Vector3.up * 0.1f + UnityEngine.Random.insideUnitSphere;
			EffectsController.CreateSmoke(this.x, this.y + 5f, 0.3f * UnityEngine.Random.value, a * 60f);
			a = Vector3.up * 0.1f + UnityEngine.Random.insideUnitSphere;
			EffectsController.CreateSmoke(this.x + a.x * 8f, this.y + 5f + a.y * 4f, 0.3f * UnityEngine.Random.value, a * 120f);
			a = Vector3.up * 0.1f + UnityEngine.Random.insideUnitSphere;
			EffectsController.CreateSmoke(this.x + a.x * 8f, this.y + 5f + a.y * 4f, 0.3f * UnityEngine.Random.value, a * 120f);
			a = Vector3.up * 0.1f + UnityEngine.Random.insideUnitSphere;
			EffectsController.CreateSmoke(this.x + a.x * 8f, this.y + 5f + a.y * 4f, 0.3f * UnityEngine.Random.value, a * 120f);
			a = Vector3.up * 0.1f + UnityEngine.Random.insideUnitSphere;
			EffectsController.CreateSmoke(this.x + a.x * 8f, this.y + 5f + a.y * 4f, 0.3f * UnityEngine.Random.value, a * 120f);
			SortOfFollow.Shake(0.4f, 4f, base.transform.position);
			Map.DisturbWildLife(this.x, this.y, 90f, 15);
			Map.ShakeTrees(this.x, this.y, 60f, 32f, 80f);
			return true;
		}
		return false;
	}

	protected virtual void Gib(DamageType damageType, float xI, float yI)
	{
		if (!this.destroyed && (this.canGib || damageType == DamageType.OutOfBounds) && (this.playerNum < 0 || !this.broMustFailToWin || damageType == DamageType.OutOfBounds))
		{
			this.ReleaseGrenade(false);
			if (this.deathType == DeathType.Unassigned || Time.time - this.deathTime < 0.33f)
			{
				this.SetDeathType(damageType, -100);
				this.NotifyDeathType();
			}
			this.CreateGibEffects(damageType, xI, yI);
			if (base.IsMine || base.IsEnemy || this is RescueBro)
			{
				Networking.RPC<DamageType, float, float>(PID.TargetOthers, new RpcSignature<DamageType, float, float>(this.Gib), damageType, xI, yI, false);
			}
			this.destroyed = true;
			this.CheckPlasmaDeath(ref xI, ref yI);
		}
	}

	public override void CreateGibEffects(DamageType damageType, float xI, float yI)
	{
		if (!this.destroyed)
		{
			if (damageType == DamageType.Crush)
			{
				int num = UnityEngine.Random.Range(0, 2) * 2 - 1;
				EffectsController.CreateGibs(this.gibs, base.GetComponent<Renderer>().sharedMaterial, this.x, this.y, 180f, 40f, xI * 0.5f, 100f);
				EffectsController.CreateBloodParticles(this.bloodColor, this.x, this.y + 6f, 20, 5f, 5f, 45f, xI * 0.5f + (float)(num * 200), 25f);
				num *= -1;
				EffectsController.CreateBloodParticles(this.bloodColor, this.x, this.y + 6f, 20, 5f, 5f, 45f, xI * 0.5f + (float)(num * 200), 25f);
				num *= -1;
				EffectsController.CreateBloodParticles(this.bloodColor, this.x, this.y + 6f, 20, 5f, 5f, 25f, xI * 0.5f + (float)num * (100f + UnityEngine.Random.value * 100f), 25f + UnityEngine.Random.value * 200f);
				num *= -1;
				EffectsController.CreateBloodParticles(this.bloodColor, this.x, this.y + 6f, 20, 5f, 5f, 25f, xI * 0.5f + (float)num * (100f + UnityEngine.Random.value * 100f), 25f + UnityEngine.Random.value * 200f);
			}
			else if (damageType == DamageType.OutOfBounds)
			{
				int num2 = UnityEngine.Random.Range(0, 2) * 2 - 1;
				EffectsController.CreateGibs(this.gibs, base.GetComponent<Renderer>().sharedMaterial, this.x, this.y, 140f, 140f, xI * 0.5f, 300f);
				EffectsController.CreateBloodParticles(this.bloodColor, this.x, this.y + 6f, 20, 5f, 5f, 66f, xI * 0.5f + (float)(num2 * 150), 75f);
				num2 *= -1;
				EffectsController.CreateBloodParticles(this.bloodColor, this.x, this.y + 6f, 20, 5f, 5f, 66f, xI * 0.5f + (float)(num2 * 150), 75f);
				num2 *= -1;
				EffectsController.CreateBloodParticles(this.bloodColor, this.x, this.y + 6f, 20, 5f, 5f, 55f, xI * 0.5f + (float)(num2 * 100), 125f + UnityEngine.Random.value * 150f);
				num2 *= -1;
				EffectsController.CreateBloodParticles(this.bloodColor, this.x, this.y + 6f, 20, 5f, 5f, 55f, xI * 0.5f + (float)(num2 * 100), 125f + UnityEngine.Random.value * 150f);
			}
			else
			{
				this.CreateGibs(xI, yI);
			}
			this.PlayGibSound();
			this.SprayBlood(40f);
			this.SprayBlood(40f);
			this.SprayBlood(40f);
			this.SprayBlood(40f);
		}
	}

	protected virtual void CreateGibs(float xI, float yI)
	{
		EffectsController.CreateGibs(this.gibs, base.GetComponent<Renderer>().sharedMaterial, this.x, this.y, 100f, 100f, xI * 0.25f, yI * 0.25f + 60f);
	}

	protected virtual void PlayBurnSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.burnSounds, 0.16f, base.transform.position, 0.6f + UnityEngine.Random.value * 0.4f);
		}
	}

	protected virtual void PlayHitSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.effortSounds, 0.4f, base.transform.position);
		}
	}

	protected virtual void PlayFallDamageSound()
	{
		this.PlayFallDamageSound(0.4f);
	}

	protected virtual void PlayFallDamageSound(float v)
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.fallHitSound, v, base.transform.position);
		}
	}

	protected virtual void PlayDecapitateSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.dismemberSounds, 0.5f, base.transform.position);
		}
	}

	protected virtual void PlayDeathGargleSound()
	{
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.deathGargleSounds, 0.45f, base.transform.position);
	}

	protected virtual void PlayBleedSound()
	{
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.bleedSounds, 0.45f, base.transform.position);
	}

	protected virtual void PlayGibSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.deathSounds, 0.4f, base.transform.position);
		}
	}

	protected virtual void PlayDeathSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			if (this.soundHolder.hitSounds.Length <= 0)
			{
				this.sound.PlaySoundEffectAt(this.soundHolder.hitSounds, this.deathSoundVolume * 0.5f, base.transform.position);
			}
			else
			{
				this.sound.PlaySoundEffectAt(this.soundHolder.hitSounds, this.deathSoundVolume, base.transform.position);
			}
		}
	}

	protected virtual void SprayBlood(float range)
	{
		if (Physics.Raycast(new Vector3(this.x, this.y + 4f, 0f), global::Math.RandomPointOnCircle(), out this.raycastHit, range, this.groundLayer))
		{
			Block component = this.raycastHit.collider.GetComponent<Block>();
			if (component != null)
			{
				component.Bloody(this.raycastHit.normal, BloodColor.Red);
			}
		}
		Map.BloodyDoodads(this.x, this.y, range);
	}

	public virtual void Stop()
	{
		this.up = false;
		this.down = false;
		this.specialDown = false;
		this.left = false;
		this.right = false;
		this.buttonJump = false;
		if (this.actionState != ActionState.Dead)
		{
			this.actionState = ActionState.Idle;
		}
		this.xI = 0f;
	}

	public virtual void RecallBro()
	{
		if (!this.recalling)
		{
		}
		this.recalling = true;
		if (this.y > this.groundHeight)
		{
			this.actionState = ActionState.Jumping;
		}
		else
		{
			this.actionState = ActionState.Idle;
		}
		Shader shader = Shader.Find("Particles/Alpha Blended");
		if (shader != null)
		{
			base.GetComponent<Renderer>().material.shader = shader;
			this.gunSprite.GetComponent<Renderer>().material.shader = shader;
			base.gameObject.layer = LayerMask.NameToLayer("Effects");
			this.gunSprite.gameObject.layer = LayerMask.NameToLayer("Effects");
		}
		this.actionState = ActionState.Recalling;
		this.health = 0;
		Map.RemoveUnit(this);
		this.up = false;
		this.down = false;
		this.specialDown = false;
		this.left = false;
		this.right = false;
		this.buttonJump = false;
		this.xI = 0f;
		this.playerNum = 5;
	}

	protected virtual void ReduceLives(bool destroyed)
	{
		if (!base.IsMine)
		{
			return;
		}
		if (!this.reducedLife)
		{
			this.reducedLife = true;
			if (this.playerNum >= 0 && this.playerNum < 5)
			{
				HeroController.PlayerHasDied(this.playerNum);
				HeroController.PlayerDelay(this.playerNum);
			}
		}
	}

	public override void RollOnto(int direction)
	{
		this.Knock(DamageType.Explosion, (float)(direction * 200), 9f, false);
	}

	public virtual bool IsExitingDoor()
	{
		return false;
	}

	public virtual void RunTrail()
	{
		if (this.heroTrail != null)
		{
			if (this.health > 0)
			{
				if (Mathf.Abs(this.xI + this.xIBlast) > this.speed * 0.5f)
				{
					if (!this.heroTrail.enabled)
					{
						this.heroTrail.Enable();
					}
					this.heroTrail.time = Mathf.Clamp(this.heroTrail.time + this.t * 8f, 0.1f, 0.4f);
					if (this.yI == 0f)
					{
						this.heroTrail.transform.localEulerAngles = Vector3.zero;
					}
				}
				else if (this.heroTrail.enabled)
				{
					if (this.heroTrail.time <= 0f)
					{
						this.heroTrail.Disable();
					}
					this.heroTrail.time -= this.t;
				}
			}
			else
			{
				this.heroTrail.Disable();
			}
		}
	}

	public void HideCharacter()
	{
		this.gunSprite.gameObject.SetActive(false);
		base.GetComponent<Renderer>().enabled = false;
		base.enabled = false;
		this.HidingPlayer = true;
	}

	public void ShowCharacter()
	{
		if (base.IsMine)
		{
			this.ShowCharacterInternal();
			if (base.Syncronize)
			{
				Networking.RPC(PID.TargetOthers, new RpcSignature(this.ShowCharacterInternal), false);
			}
		}
	}

	public void ShowCharacterInternal()
	{
		if (this != null)
		{
			this.gunSprite.gameObject.SetActive(true);
			base.GetComponent<Renderer>().enabled = true;
		}
		base.enabled = true;
		this.HidingPlayer = false;
	}

	[Syncronize]
	public short InputBits
	{
		get
		{
			return TypeSerializer.GetShortFromBoolArray(new bool[]
			{
				this.up,
				this.down,
				this.left,
				this.right,
				this.fire,
				this.buttonJump,
				this.highFive,
				this.special,
				this.dashing,
				this.holdingHighFive,
				this.releasingHighFive
			});
		}
		set
		{
			bool[] boolArrayFromShort = TypeSerializer.GetBoolArrayFromShort(value);
			this.up = boolArrayFromShort[0];
			this.down = boolArrayFromShort[1];
			this.left = boolArrayFromShort[2];
			this.right = boolArrayFromShort[3];
			this.fire = boolArrayFromShort[4];
			this.buttonJump = boolArrayFromShort[5];
			this.highFive = boolArrayFromShort[6];
			this.special = boolArrayFromShort[7];
			this.dashing = boolArrayFromShort[8];
			this.holdingHighFive = boolArrayFromShort[9];
			this.releasingHighFive = boolArrayFromShort[10];
		}
	}

	[Syncronize]
	public Transform SyncParent
	{
		get
		{
			return this.lastParentedToTransform;
		}
		set
		{
			this.lastParentedToTransform = value;
		}
	}

	[Syncronize]
	public ParentedPosition SyncParentedPosition
	{
		get
		{
			return new ParentedPosition(this.x, this.y, this.lastParentedToTransform);
		}
		set
		{
			this.x = value.WorldX;
			this.y = value.WorldY;
			this.lastParentedToTransform = value.parent;
		}
	}

	public override Vector2 XY
	{
		get
		{
			return base.XY;
		}
		set
		{
			MonoBehaviour.print("Set XY");
			base.XY = value;
		}
	}

	public override UnityStream PackState(UnityStream stream)
	{
		stream.Serialize<byte>((byte)this.actionState);
		stream.Serialize<int>(this.health);
		if (base.IsHero)
		{
			stream.Serialize<int>(this.playerNum);
			stream.Serialize<HeroType>(this.heroType);
			stream.Serialize<bool>(base.Syncronize);
		}
		return base.PackState(stream);
	}

	public override UnityStream UnpackState(UnityStream stream)
	{
		this.actionState = (ActionState)((byte)stream.DeserializeNext());
		this.health = (int)stream.DeserializeNext();
		if (base.IsHero)
		{
			int playerNum = (int)stream.DeserializeNext();
			HeroType heroTypeEnum = (HeroType)((int)stream.DeserializeNext());
			bool syncingInternal = (bool)stream.DeserializeNext();
			base.SetSyncingInternal(syncingInternal);
			this.SetUpHero(playerNum, heroTypeEnum);
		}
		return base.UnpackState(stream);
	}

	public virtual bool IsAlive()
	{
		return this.health > 0;
	}

	protected virtual void Unrevive()
	{
		this.Damage(this.health + 1, DamageType.Crush, -base.transform.localScale.x * 50f, 30f, (int)(-(int)base.transform.localScale.x), this, this.x, this.y + 3f);
	}

	public override bool Revive(int playerNum, bool isUnderPlayerControl, TestVanDammeAnim reviveSource)
	{
		if (this.health <= 0)
		{
			EffectsController.CreateReviveZombieEffect(this.x, this.y, null);
			this.deathNotificationSent = false;
			this.reviveZombieTime = 0.4f + UnityEngine.Random.value * 0.7f;
			this.yI = 120f;
			this.actionState = ActionState.Jumping;
			if (this.enemyAI != null)
			{
				this.enemyAI.StopPanicking();
			}
			if (this.playerNum >= 0 && this.playerNum < 4 && HeroController.IsPlaying(this.playerNum) && !HeroController.PlayerIsAlive(this.playerNum))
			{
				this.ShowStartBubble();
				this.reviveSource = null;
				this.isZombie = false;
				HeroController.AssignPlayerCharacter(this.playerNum, this);
				if (this.reducedLife)
				{
					HeroController.AddLife(this.playerNum);
				}
				this.reducedLife = false;
				HeroController.SetAvatarCalm(this.playerNum, true);
				base.SetSyncingInternal(true);
				this.SetInvulnerable(0.5f, true);
			}
			else
			{
				base.gameObject.SetOwnerLocal(reviveSource.Owner);
				this.reviveSource = reviveSource;
				this.isZombie = true;
				if (playerNum != this.playerNum)
				{
					this.isHero = false;
				}
				this.playerNum = playerNum;
			}
			this.health = this.maxHealth;
			return true;
		}
		return false;
	}

	protected virtual void CalculateZombieInput()
	{
		this.zombieDelay -= this.t;
		this.reviveSource.CopyInput(this, ref this.zombieDelay, ref this.up, ref this.down, ref this.left, ref this.right, ref this.fire, ref this.buttonJump, ref this.special, ref this.highFive);
		this.special = false;
		this.highFive = false;
	}

	protected virtual void CopyInput(TestVanDammeAnim zombie, ref float zombieDelay, ref bool up, ref bool down, ref bool left, ref bool right, ref bool fire, ref bool buttonJump, ref bool special, ref bool highFive)
	{
		if (this.health > 0)
		{
			up = this.up;
			down = this.down;
			left = this.left;
			right = this.right;
			fire = this.fire;
			buttonJump = this.buttonJump;
			special = this.special;
			highFive = this.highFive;
		}
		else
		{
			down = false;
			left = false;
			right = false;
			fire = false;
			buttonJump = false;
			special = false;
			highFive = false;
		}
	}

	public virtual void SetRemoteProjectile(Projectile p)
	{
		this.projectileTime = Time.time;
		this.remoteProjectile = p;
	}

	public virtual void BrosMustFailToWin()
	{
		UnityEngine.Debug.Log("Bro Must Fail To Win ");
		if (this.playerNum >= 0)
		{
			this.broMustFailToWin = true;
			this.canGib = false;
		}
	}

	public virtual bool MustBroFailToWin()
	{
		UnityEngine.Debug.LogError("THIS SHOULD BE CALCULATED FROM A ON LEVEL FAILURE TRIGGER ");
		return this.broMustFailToWin;
	}

	[HideInInspector]
	public const float GRAVITY = 1100f;

	public const float JUMP_TIME = 0.123f;

	public const float gravityWaterDanmping = 0.5f;

	protected SpriteSM sprite;

	protected Vector3 spriteOffset;

	protected int spritePixelWidth = 32;

	protected int spritePixelHeight = 32;

	public bool useNewFrames;

	public bool useNewDuckingFrames;

	public bool useNewThrowingFrames;

	public bool useNewKnifingFrames;

	public bool useNewKnifeClimbingFrames;

	protected bool wallClimbAnticipation;

	protected bool armUpInAnticipationWallClimb;

	protected float lastKnifeClimbStabY;

	protected float knifeClimbStabHeight = 18f;

	public bool useNewLadderClimbingFrames;

	protected int ladderClimbingTransitionFrames;

	public bool useLadderClimbingTransition;

	protected bool hangingOneArmed;

	public bool doRollOnLand;

	public bool useNewLedgeGrappleFrames;

	public bool canLedgeGrapple = true;

	public bool canChimneyFlip;

	public bool useDashFrames;

	public bool useImpaledFrames;

	public bool canGib = true;

	private GameObject ellipsis;

	public Projectile remoteProjectile;

	protected float projectileTime;

	public bool isOnHelicopter;

	protected bool broMustFailToWin;

	protected float stunTime;

	protected float blindCounter;

	public HeroType heroType;

	protected float idleTime;

	public Material disarmedGunMaterial;

	public SpriteSM faderSpritePrefab;

	protected bool usePrimaryAvatar = true;

	protected float showHighFiveAfterMeleeTimer;

	[HideInInspector]
	public Unit pilottedUnit;

	protected TestVanDammeAnim reviveSource;

	protected float reviveZombieTime = 2f;

	protected float reviveZombieCounter;

	protected float zombieDelay;

	[HideInInspector]
	public float zombieOffset;

	[HideInInspector]
	public int zombieTimerOffset;

	protected float invulnerableTime;

	protected bool firedWhileInvulnerable;

	private float rollingTime;

	protected int rollingFrames;

	protected float t = 0.1f;

	protected float lastT = 0.1f;

	protected float frameRate = 0.0667f;

	protected int frame;

	protected float counter;

	public float speed = 110f;

	protected float dashSpeedM = 1f;

	protected float lastDashSpeedM = 1f;

	protected float lastLandTime;

	public float maxFallSpeed = -400f;

	public GibHolder gibs;

	public ReactionBubble player1Bubble;

	public ReactionBubble player2Bubble;

	public ReactionBubble player3Bubble;

	public ReactionBubble player4Bubble;

	public ReactionBubble high5Bubble;

	[HideInInspector]
	public bool buttonJump;

	[HideInInspector]
	public bool buttonHighFive;

	[HideInInspector]
	public bool buttonTaunt;

	[HideInInspector]
	public bool up;

	[HideInInspector]
	public bool down;

	[HideInInspector]
	public bool left;

	[HideInInspector]
	public bool right;

	[HideInInspector]
	public bool wasButtonJump;

	[HideInInspector]
	public bool wasButtonHighFive;

	[HideInInspector]
	public bool wasButtonTaunt;

	[HideInInspector]
	public bool wasUp;

	[HideInInspector]
	public bool wasDown;

	[HideInInspector]
	public bool wasLeft;

	[HideInInspector]
	public bool wasRight;

	[HideInInspector]
	public bool dashButton;

	[HideInInspector]
	public bool wasdashButton;

	protected float lastButtonJumpTime;

	protected float jumpGrace;

	protected float hangGrace;

	protected bool chimneyFlip;

	protected bool chimneyFlipConstrained;

	protected int chimneyFlipFrames;

	protected int chimneyFlipDirection;

	protected float dashTime;

	protected bool hasDashedInAir;

	protected float minDashTapTime = 0.33f;

	protected float rightTapTime;

	protected float leftTapTime;

	protected float downTapTime;

	[HideInInspector]
	public bool dashing;

	protected bool delayedDashing;

	protected bool meleeFollowUp;

	protected bool standingMelee;

	protected bool meleeHasHit;

	protected bool jumpingMelee;

	protected bool dashingMelee;

	[HideInInspector]
	public bool special;

	[HideInInspector]
	public bool wasSpecial;

	[HideInInspector]
	public bool fire;

	[HideInInspector]
	public bool wasFire;

	protected bool usingSpecial;

	protected bool holdingHighFive;

	protected bool releasingHighFive;

	protected bool highFive;

	protected bool doingMelee;

	protected bool wasHighFive;

	protected bool usingSpecial2;

	protected bool usingSpecial3;

	protected bool usingSpecial4;

	protected bool wasUsingSpecial2;

	protected bool wasUsingSpecial3;

	protected bool wasUsingSpecial4;

	protected float highfiveHoldTime;

	protected Unit meleeChosenUnit;

	protected float jumpTime;

	protected float ignoreHighFivePressTime;

	protected int doubleJumpsLeft;

	protected bool wallDrag;

	protected bool ledgeGrapple;

	protected bool wasLedgeGrapple;

	protected float ledgeOffsetY;

	public float fireRate = 0.0334f;

	public float fireDelay;

	protected float fireCounter = 0.0667f;

	protected float specialCounter = 0.0667f;

	private int specialAmmo = 3;

	public int originalSpecialAmmo = 3;

	protected bool highFiveBoost;

	protected float highFiveBoostTime;

	protected float highFiveBoostM = 1.4f;

	protected float timeBroBoostTime;

	public SoundHolder soundHolder;

	public SoundHolderFootstep soundHolderFootSteps;

	public Grenade specialGrenade;

	public SpriteSM gunSprite;

	protected int gunSpritePixelWidth = 32;

	protected int gunSpritePixelHeight = 32;

	protected float gunCounter;

	protected int gunFrame;

	public Spikes impaledOnSpikes;

	public Shrapnel[] blood;

	public bool canWallClimb = true;

	public bool canPushBlocks = true;

	public bool useNewPushingFrames;

	protected float pushingTime;

	public bool canDash = true;

	public float jumpForce = 260f;

	public bool canCeilingHang;

	public bool canAirdash;

	public bool canDoIndependentMeleeAnimation;

	protected bool airdashUpAvailable;

	protected bool airdashDownAvailable;

	protected bool airdashLeftAvailable;

	protected bool airdashRightAvailable;

	protected float airdashTime;

	protected float airDashDelay;

	protected DirectionEnum airdashDirection = DirectionEnum.Any;

	public float airdashMaxTime = 0.5f;

	protected bool recalling;

	protected float recallCounter;

	protected float avatarGunFireTime;

	protected float avatarAngryTime;

	protected bool ducking;

	protected bool xBoxControlled;

	protected bool controllingProjectile;

	protected float controllingProjectileDelay;

	protected float lastAlertTime;

	protected bool firstFrame = true;

	private float noclipSpeed = 200f;

	protected AudioSource wallDragAudio;

	private ReactionBubble kickPlayerBubble;

	private bool ElgilbleToBeKicked;

	private bool hasBeenKicked;

	private float bubbleTimer;

	private float bubbleInterval = 1.2f;

	protected Sound sound;

	protected static float lastHighFiveTime;

	protected static float lastBassTime;

	protected bool wasHangingMoving;

	protected int knifeHand;

	public float runningFrameRate = 0.025f;

	protected float footstepDelay = 0.5f;

	protected GroundType lastFootStepGroundType = GroundType.BrickBehind;

	protected string currentFootStepGroundType = string.Empty;

	protected int knifeSoundCount;

	protected float lastJumpTime;

	public bool canTouchLeftWalls;

	public bool canTouchRightWalls;

	protected LayerMask groundLayer;

	protected LayerMask platformLayer;

	protected LayerMask barrierLayer;

	protected LayerMask fragileLayer;

	protected LayerMask openDoorsLayer;

	protected LayerMask ladderLayer;

	protected LayerMask victoryLayer;

	protected RaycastHit raycastHit;

	protected LayerMask switchesLayer;

	protected float ladderX;

	protected float groundHeight;

	protected float frictionM = 10f;

	protected float yIT;

	protected float xIT;

	protected RaycastHit newRaycastHit;

	protected bool wallClimbing;

	protected bool wasWallDragging;

	protected float wallDragTime;

	public float xIBlast;

	public float xIAttackExtra;

	public float yIBlast;

	protected bool constrainedRight;

	protected bool wasConstrainedRight;

	protected bool constrainedLeft;

	protected bool wasConstrainedLeft;

	private int lastRow;

	private int lastCollumn;

	protected float screenMaxX;

	protected float screenMinX;

	protected float screenMaxY;

	protected float screenMinY;

	protected float belowScreenCounter;

	protected float holdUpTime;

	protected float holdRightTime;

	protected float holdLeftTime;

	protected float zOffset;

	protected Transform impaledTransform;

	protected float impaleXOffset;

	protected int impaledDamage;

	protected int impaledDirection;

	protected Vector3 impaledPosition;

	protected Vector3 impaledLocalPos;

	protected float parentVelocityInheritM = 1f;

	protected Transform lastParentedToTransform;

	protected Vector3 lastParentPosition;

	protected Vector3 parentedDiff;

	protected float parentHasMovedTime;

	protected Transform wallClimbingWallTransform;

	protected Vector3 wallClimbingWallTransformLocalPos;

	protected Transform groundTransform;

	protected Vector3 groundTransformLocalPos;

	protected Vector3 lastParentWorldPos;

	protected bool wasWallClimbing;

	protected RaycastHit raycastHitWalls;

	private float waterDampingX = 0.95f;

	private float waterDampingY = 0.65f;

	protected float halfWidth = 6f;

	protected float feetWidth = 4f;

	protected float headHeight = 15f;

	protected float standingHeadHeight = 18f;

	protected float duckingHeadHeight = 13f;

	protected float waistHeight = 10f;

	protected float standingWaistHeight = 10f;

	protected float duckingWaistHeight = 6f;

	protected float toeHeight = 2f;

	protected float standingToeHeight = 2f;

	protected float duckingToeHeight = 5f;

	public bool canDuck = true;

	private float lastPathingTime;

	protected bool specialDown;

	protected bool wasSpecialDown;

	protected bool wasXBoxFireDown;

	public Grenade heldGrenade;

	protected bool throwingGrenade;

	public float deathSoundVolume = 0.7f;

	protected bool reducedLife;

	public CustomLineRenderer heroTrailPrefab;

	protected CustomLineRenderer heroTrail;

	public bool HidingPlayer;
}
