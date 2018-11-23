// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class BoondockBro : TestVanDammeAnim
{
	public virtual BroFollowMode FollowMode
	{
		get
		{
			return this.followMode;
		}
		set
		{
			this.followMode = value;
			if (this.followMode == BroFollowMode.CopyCat)
			{
				base.StopDashing();
			}
			else if (this.followMode == BroFollowMode.CatchUp)
			{
				this.ForceStartDash();
			}
			else if (this.followMode == BroFollowMode.Calibrate)
			{
				this.calibrateTimer = 0.1f;
			}
		}
	}

	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
		this.inputChanges = new List<InputStateChange>();
		this.fireInputChanges = new List<InputStateChange>();
		this.defaultSpeed = this.speed;
		this.defaultFireRate = this.fireRate;
	}

	protected override void Start()
	{
		base.Start();
		if (this.isLeadBro)
		{
			this.usePrimaryAvatar = true;
			this.SpawnTrailingBro();
			HeroController.players[this.playerNum].hud.GoToDoubleAvatarMode(this.avatar1, this.avatar2);
		}
		this.SetEnraged(false);
	}

	protected override void UseFire()
	{
		this.frontBullet = !this.frontBullet;
		if (this.attachedToZipline != null)
		{
			this.frontBullet = false;
		}
		this.FireWeapon(this.x + base.transform.localScale.x * (float)((!this.frontBullet) ? 2 : 12), this.y + 8f, base.transform.localScale.x * 400f, (float)UnityEngine.Random.Range(-20, 20));
		this.PlayAttackSound();
		Map.DisturbWildLife(this.x, this.y, 60f, this.playerNum);
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		if (this.attachedToZipline == null)
		{
			this.gunFrame = ((!this.frontBullet) ? 6 : 3);
		}
		else
		{
			this.gunFrame = 3;
		}
		this.SetGunSprite(this.gunFrame, 0);
		EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, base.transform);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed, ySpeed, this.playerNum);
	}

	protected override void RunGun()
	{
		if (!this.WallDrag && this.gunFrame > 0)
		{
			this.gunCounter += this.t;
			if (this.gunCounter > 0.0334f)
			{
				this.gunCounter -= 0.0334f;
				this.gunFrame--;
				if (this.gunFrame == 3)
				{
					this.gunFrame = 0;
				}
				this.SetGunSprite(this.gunFrame, 0);
			}
		}
	}

	protected override void RunFiring()
	{
		this.fireDelay -= this.t;
		if (this.fire)
		{
			this.rollingFrames = 0;
		}
		if (this.fire && (this.fireDelay <= 0f || !this.wasFire))
		{
			this.fireDelay = this.fireRate;
			this.UseFire();
			base.FireFlashAvatar();
		}
	}

	protected override void SetGunSprite(int spriteFrame, int spriteRow)
	{
		if (this.actionState == ActionState.ClimbingLadder && this.hangingOneArmed)
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * (9 + spriteFrame)), (float)(this.gunSpritePixelHeight * (1 + spriteRow)));
		}
		else if (this.attachedToZipline != null && this.actionState == ActionState.Jumping)
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * (9 + spriteFrame)), (float)(this.gunSpritePixelHeight * (1 + spriteRow)));
		}
		else
		{
			this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * spriteFrame), (float)(this.gunSpritePixelHeight * (1 + spriteRow)));
		}
	}

	protected override void CheckInput()
	{
		if (this.health <= 0)
		{
			this.ClearAllInput();
			return;
		}
		if (this.isLeadBro)
		{
			base.CheckInput();
		}
		else
		{
			if (this.actionState == ActionState.Dead)
			{
				return;
			}
			if (base.IsMine && this.leadingBro != null && this.leadingBro.actionState != ActionState.Jumping && (this.lastTargetCol != this.leadingBro.collumn || this.lastTargetRow != this.leadingBro.row))
			{
				this.lastTargetCol = this.leadingBro.collumn;
				this.lastTargetRow = this.leadingBro.row;
				if (this.actionState != ActionState.Jumping)
				{
					this.needPathRecompute = true;
				}
			}
			this.wasUp = this.up;
			this.wasButtonJump = this.buttonJump;
			this.wasDown = this.down;
			this.wasLeft = this.left;
			this.wasRight = this.right;
			this.wasFire = this.fire;
			this.wasSpecial = this.special;
			this.wasHighFive = this.highFive;
			this.wasButtonTaunt = this.buttonTaunt;
			this.buttonJump = false; this.left = (this.right = (this.up = (this.down = (this.buttonJump ))));
			this.buttonHighFive = false; this.fire = (this.special = (this.buttonHighFive ));
			if (this.leadingBro != null && base.IsMine && !this.leadingBro.isOnHelicopter)
			{
				this.special = this.leadingBro.special;
				this.highFive = this.leadingBro.highFive;
				if (this.highFive && !this.wasHighFive)
				{
					this.PressHighFiveMelee(false);
				}
				this.TrackInputStateChanges();
				this.CopyInputStateChanges(this.inputChanges);
				this.CopyInputStateChanges(this.fireInputChanges);
				this.fire = this.trackedInputState.fire;
				if (this.FollowMode == BroFollowMode.CopyCat)
				{
					this.GetCopyCatInput();
				}
				else if (this.FollowMode == BroFollowMode.Calibrate)
				{
					this.GetCalibrateInput();
				}
				else if (this.FollowMode == BroFollowMode.CatchUp)
				{
					this.GetCatchupInput();
				}
			}
		}
		if (!base.IsMine)
		{
			base.RunSync();
		}
	}

	private void DebugShowInputStateChanges(List<InputStateChange> inputChanges)
	{
		string text = this.FollowMode.ToString() + "\n";
		foreach (InputStateChange inputStateChange in inputChanges)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				inputStateChange.key.ToString(),
				", ",
				inputStateChange.newState,
				", ",
				inputStateChange.timeLeft.ToString("0.0000"),
				"\n"
			});
		}
		LevelEditorGUI.DebugText = text;
	}

	protected override void ThrowBackGrenade(Grenade grenade)
	{
		if (this.isLeadBro)
		{
			base.ThrowBackGrenade(grenade);
		}
	}

	private void GetCopyCatInput()
	{
		this.up = this.trackedInputState.up;
		this.down = this.trackedInputState.down;
		this.left = this.trackedInputState.left;
		this.right = this.trackedInputState.right;
		this.buttonJump = this.trackedInputState.jump;
		if (this.leadingBro != null)
		{
			this.buttonHighFive = this.leadingBro.buttonHighFive;
		}
		if (this.trackedInputState.dashing && !this.dashing)
		{
			this.ForceStartDash();
		}
		else if (!this.trackedInputState.dashing && this.dashing)
		{
			base.StopDashing();
		}
		if (!this.up && !this.down && !this.left && !this.right && !this.buttonJump && this.actionState != ActionState.Jumping && !this.trackedInputState.up && !this.trackedInputState.down && !this.trackedInputState.left && !this.trackedInputState.right && !this.trackedInputState.jump)
		{
			if (this.inputChanges.Count == 0)
			{
				if ((this.repositionDelay -= this.t) < 0f && this.inputChanges.Count == 0 && Mathf.Abs(this.leadingBro.xI * this.leadingBro.yI) < 20f)
				{
					if (this.leadingBro.transform.localScale.x > 0f)
					{
						if (base.transform.position.x > this.leadingBro.transform.position.x - (4f * (float)this.position + 2f) && (this.repositionTimeLeft -= this.t) > 0f)
						{
							this.left = true;
						}
						else
						{
							base.transform.localScale = Vector3.one;
							this.xI = 0.0001f;
						}
					}
					else if (base.transform.position.x < this.leadingBro.transform.position.x + (4f * (float)this.position + 2f) && (this.repositionTimeLeft -= this.t) > 0f)
					{
						this.right = true;
					}
					else
					{
						base.transform.localScale = new Vector3(-1f, 1f, 1f);
						this.xI = -0.0001f;
					}
				}
			}
			else if (this.repositionDelay < 0f)
			{
				this.FollowMode = BroFollowMode.Calibrate;
			}
		}
	}

	protected void GetCalibrateInput()
	{
		if (this.inputChanges.Count > 0)
		{
			Vector3 pos = this.inputChanges[0].pos;
			if (Vector3.Distance(base.transform.position, pos) > 4f)
			{
				this.calibrateTimer = 0f;
			}
			if ((this.calibrateTimer -= this.t) > 0f)
			{
				return;
			}
			this.repositionTimeLeft = 0.5f;
			if (this.inputChanges[0].pos.x - base.transform.position.x < 1f)
			{
				this.left = true;
			}
			else if (this.inputChanges[0].pos.x - base.transform.position.x > -1f)
			{
				this.right = true;
			}
			if (this.inputChanges[0].pos.y - base.transform.position.y < 1f)
			{
				this.down = true;
			}
			else if (this.inputChanges[0].pos.y - base.transform.position.y > -1f)
			{
				this.buttonJump = true;
				this.up = true;
			}
			if (Mathf.Abs(base.transform.position.x - pos.x) < 2f || Mathf.Abs(base.transform.position.x + this.xI * this.t - pos.x) < 2f)
			{
				this.FollowMode = BroFollowMode.CopyCat;
				this.inputDelay -= this.inputChanges[0].timeLeft;
				for (int i = 1; i < this.inputChanges.Count; i++)
				{
					this.inputChanges[i].timeLeft -= this.inputChanges[0].timeLeft;
				}
				this.inputChanges[0].timeLeft = 0f;
			}
			if (Vector3.Distance(base.transform.position, pos) > 32f)
			{
				this.DoCatchup();
			}
		}
		else
		{
			this.FollowMode = BroFollowMode.CopyCat;
		}
	}

	protected void GetCatchupInput()
	{
		if (base.GetComponent<PathAgent>().CurrentPath == null || this.needPathRecompute)
		{
			this.needPathRecompute = false;
			base.GetComponent<PathAgent>().GetPath(this.lastTargetCol, this.lastTargetRow, 15f);
		}
		if (base.GetComponent<PathAgent>().CurrentPath != null)
		{
			base.GetComponent<PathAgent>().GetMove(ref this.left, ref this.right, ref this.up, ref this.down, ref this.buttonJump);
		}
		if (Vector3.SqrMagnitude(base.transform.position - this.leadingBro.transform.position) < this.copyCatDistanceSquared * 0.5f)
		{
			this.FollowMode = BroFollowMode.CopyCat;
			base.GetComponent<PathAgent>().CurrentPath = null;
		}
	}

	private void DoCatchup()
	{
		if (this is BillyConnolly)
		{
			this.followMode = BroFollowMode.CatchUp;
		}
		else if (this.inputChanges.Count > 0)
		{
			this.x = (this.inputChanges[0].pos.x + this.x) / 2f;
			this.y = (this.inputChanges[0].pos.y + this.y) / 2f;
			this.xI = this.inputChanges[0].xI;
			this.yI = this.inputChanges[0].yI;
			float timeLeft = this.inputChanges[0].timeLeft;
			this.inputChanges[0].timeLeft = 0f;
			for (int i = 1; i < this.inputChanges.Count; i++)
			{
				this.inputChanges[i].timeLeft -= timeLeft;
			}
		}
	}

	protected void TrackInputStateChanges()
	{
		if (this.leadingBro == null || this.leadingBro.isOnHelicopter)
		{
			return;
		}
		if ((this.leadingBro.up || this.leadingBro.down || this.leadingBro.left || this.leadingBro.right || this.leadingBro.buttonJump) && (this.arbitraryTrackingDelay -= this.t) < 0f)
		{
			this.arbitraryTrackingDelay = 0.02f;
			this.inputChanges.Add(new InputStateChange
			{
				newState = this.leadingBro.up,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (this.leadingBro.up && !this.leadingBro.wasUp)
		{
			this.inputChanges.Add(new InputStateChange
			{
				newState = true,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (this.leadingBro.down && !this.leadingBro.wasDown)
		{
			this.inputChanges.Add(new InputStateChange
			{
				key = InputKey.down,
				newState = true,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (this.leadingBro.left && !this.leadingBro.wasLeft)
		{
			this.inputChanges.Add(new InputStateChange
			{
				key = InputKey.left,
				newState = true,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (this.leadingBro.right && !this.leadingBro.wasRight)
		{
			this.inputChanges.Add(new InputStateChange
			{
				key = InputKey.right,
				newState = true,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (this.leadingBro.buttonJump && !this.leadingBro.wasButtonJump)
		{
			this.inputChanges.Add(new InputStateChange
			{
				key = InputKey.jump,
				newState = true,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (this.leadingBro.fire && !this.leadingBro.wasFire)
		{
			this.fireInputChanges.Add(new InputStateChange
			{
				key = InputKey.fire,
				newState = true,
				timeLeft = this.fireRate * 0.4f + 0.1f * (float)this.position * UnityEngine.Random.value,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (this.leadingBro.dashing && !this.leadBroWasDashing)
		{
			this.fireInputChanges.Add(new InputStateChange
			{
				key = InputKey.dash,
				newState = true,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
			this.leadBroWasDashing = true;
		}
		if (!this.leadingBro.up && this.leadingBro.wasUp)
		{
			this.inputChanges.Add(new InputStateChange
			{
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (!this.leadingBro.down && this.leadingBro.wasDown)
		{
			this.inputChanges.Add(new InputStateChange
			{
				key = InputKey.down,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (!this.leadingBro.left && this.leadingBro.wasLeft)
		{
			this.inputChanges.Add(new InputStateChange
			{
				key = InputKey.left,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (!this.leadingBro.right && this.leadingBro.wasRight)
		{
			this.inputChanges.Add(new InputStateChange
			{
				key = InputKey.right,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (!this.leadingBro.buttonJump && this.leadingBro.wasButtonJump)
		{
			this.inputChanges.Add(new InputStateChange
			{
				key = InputKey.jump,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (!this.leadingBro.fire && this.leadingBro.wasFire)
		{
			this.fireInputChanges.Add(new InputStateChange
			{
				key = InputKey.fire,
				timeLeft = this.fireRate * 0.4f + 0.1f * (float)this.position * UnityEngine.Random.value,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (!this.leadingBro.dashing && this.leadBroWasDashing)
		{
			this.fireInputChanges.Add(new InputStateChange
			{
				key = InputKey.dash,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
			this.leadBroWasDashing = false;
		}
	}

	protected override void UseSpecial()
	{
		if (this.isLeadBro && base.SpecialAmmo > 0)
		{
			if (this.trailingBro != null && this.connollyBro == null)
			{
				base.SpecialAmmo--;
				HeroController.SetSpecialAmmo(this.playerNum, base.SpecialAmmo);
				this.connollyBro = Networking.InstantiateBuffered<BillyConnolly>(this.billyConnollyPrefab, base.transform.position, base.transform.rotation, false);
				Networking.RPC<BoondockBro, BoondockBro, int>(PID.TargetAll, new RpcSignature<BoondockBro, BoondockBro, int>(BoondockBro.SetUpConnollyBro), this.connollyBro, this, this.playerNum, false);
				this.trailingBro.connollyBro = this.connollyBro;
			}
			else if (this.trailingBro == null)
			{
				base.SpecialAmmo--;
				HeroController.SetSpecialAmmo(this.playerNum, base.SpecialAmmo);
				this.SpawnTrailingBro();
			}
		}
	}

	private static void SetUpConnollyBro(BoondockBro ConnollyBro, BoondockBro LeadingBro, int PlayerNum)
	{
		ConnollyBro.playerNum = PlayerNum;
		ConnollyBro.isLeadBro = false;
		ConnollyBro.position = 2;
		ConnollyBro.leadingBro = LeadingBro;
		MonoBehaviour.print("SET UP CONNOLY BRO");
	}

	private void SpawnTrailingBro()
	{
		if (base.IsMine)
		{
			this.position = 0;
			this.leadingBro = this;
			this.isLeadBro = true;
			this.trailingBro = Networking.InstantiateBuffered<BoondockBro>(HeroController.Instance.boondockBroPrefab, base.transform.position, base.transform.rotation, new object[0], false);
			Networking.RPC<BoondockBro, int, float, float, bool>(PID.TargetAll, new RpcSignature<BoondockBro, int, float, float, bool>(this.trailingBro.SetUpTrailingBro), this, this.playerNum, this.defaultSpeed, this.defaultFireRate, !this.usePrimaryAvatar, false);
			this.trailingBro.xI = this.xI;
			this.trailingBro.yI = this.yI;
			if (this.connollyBro != null)
			{
				Networking.RPC<BoondockBro>(PID.TargetAll, new RpcSignature<BoondockBro>(this.trailingBro.SetConnollyBro), this.connollyBro, false);
			}
		}
	}

	private void SetUpTrailingBro(BoondockBro LeadingBro, int PlayerNum, float DefaultSpeed, float DefaultFireRate, bool UsePrimaryAvatar)
	{
		base.GetComponent<Renderer>().material = this.secondBroMaterial;
		this.playerNum = PlayerNum;
		this.leadingBro = LeadingBro;
		this.isLeadBro = false;
		this.position = 1;
		this.speed = DefaultSpeed;
		this.fireRate = DefaultFireRate;
		this.defaultSpeed = DefaultSpeed;
		this.defaultFireRate = DefaultFireRate;
		this.usePrimaryAvatar = UsePrimaryAvatar;
	}

	private void SetConnollyBro(BoondockBro ConnollyBro)
	{
		this.connollyBro = ConnollyBro;
	}

	private void CopyInputStateChanges(List<InputStateChange> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			list[i].timeLeft -= this.t;
		}
		if (list.Count > 0 && this.followMode != BroFollowMode.Calibrate)
		{
			while (list.Count > 0 && list[0].timeLeft <= 0f)
			{
				switch (list[0].key)
				{
				case InputKey.up:
					this.trackedInputState.up = list[0].newState;
					break;
				case InputKey.down:
					this.trackedInputState.down = list[0].newState;
					break;
				case InputKey.left:
					this.trackedInputState.left = list[0].newState;
					break;
				case InputKey.right:
					this.trackedInputState.right = list[0].newState;
					break;
				case InputKey.jump:
					this.trackedInputState.jump = list[0].newState;
					break;
				case InputKey.fire:
					this.trackedInputState.fire = list[0].newState;
					break;
				case InputKey.dash:
					this.trackedInputState.dashing = list[0].newState;
					break;
				}
				if (list[0].key != InputKey.fire)
				{
					if (this.FollowMode != BroFollowMode.Calibrate)
					{
						if (Vector3.SqrMagnitude(base.transform.position - list[0].pos) > this.copyCatDistanceSquared)
						{
							this.DoCatchup();
							base.GetComponent<PathAgent>().CurrentPath = null;
						}
						else if (this.FollowMode != BroFollowMode.CopyCat || this.actionState != ActionState.Jumping)
						{
						}
						list.RemoveAt(0);
						this.repositionDelay = 0.2f;
					}
				}
				else
				{
					list.RemoveAt(0);
				}
			}
		}
	}

	protected override void CheckRescues()
	{
		if (this.isLeadBro)
		{
			base.CheckRescues();
		}
	}

	protected override void ReduceLives(bool destroyed)
	{
		if (this.isLeadBro)
		{
			if (this.trailingBro == null)
			{
				if (this.connollyBro != null)
				{
					this.connollyBro.RecallBro();
				}
				base.ReduceLives(destroyed);
			}
			else if (base.IsMine)
			{
				Networking.RPC(PID.TargetAll, new RpcSignature(this.SwitchLeadingBro), false);
			}
		}
		else if (this.leadingBro != null)
		{
			this.leadingBro.trailingBro = null;
			this.leadingBro.SetEnraged(true);
			if (this.connollyBro != null)
			{
				this.connollyBro.SetEnraged(true);
			}
		}
	}

	private void SwitchLeadingBro()
	{
		this.trailingBro.SetEnraged(true);
		this.trailingBro.SwitchToLeadBro();
		this.trailingBro.SpecialAmmo = base.SpecialAmmo;
		if (this.connollyBro != null)
		{
			this.connollyBro.SetEnraged(true);
			this.connollyBro.leadingBro = this.trailingBro;
		}
	}

	protected override void Update()
	{
		if (this.enraged)
		{
			this.enragedTimer -= this.t;
			if (this.enragedTimer <= 0f)
			{
				this.SetEnraged(false);
			}
			if (!GameModeController.LevelFinished)
			{
				float r = 0.5f + Mathf.Sin(Time.time * 25f) * 0.33f;
				Color color = new Color(r, 0.1f, 0.1f, 1f);
				base.GetComponent<SpriteSM>().GetComponent<Renderer>().material.SetColor("_TintColor", color);
				this.gunSprite.GetComponent<Renderer>().material.SetColor("_TintColor", color);
			}
			else
			{
				base.GetComponent<SpriteSM>().GetComponent<Renderer>().material.SetColor("_TintColor", Color.gray);
				this.gunSprite.GetComponent<Renderer>().material.SetColor("_TintColor", Color.gray);
			}
			if (this.enragedTimer > 5f)
			{
				Vector3 b = UnityEngine.Random.onUnitSphere * 1f;
				b.z = 0f;
				base.transform.position = this.enrageStartPos + b;
				return;
			}
		}
		if (this.health > 0 && !this.isLeadBro && this.leadingBro != null && base.GetComponent<BillyConnolly>() == null && !SortOfFollow.IsItSortOfVisible(base.transform.position))
		{
			base.transform.position = this.leadingBro.transform.position;
			this.x = this.leadingBro.x;
			this.y = this.leadingBro.y;
			this.invulnerable = true;
			this.invulnerableTime = 1f;
		}
		base.Update();
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		base.Death(xI, yI, damage);
	}

	public void SetEnraged(bool enraged)
	{
		this.enraged = enraged;
		if (enraged)
		{
			this.enragedTimer = 5.6f;
			this.speed = this.defaultSpeed * 1.5f;
			this.fireRate = this.defaultFireRate / 4f;
			if (GameModeController.IsDeathMatchMode)
			{
				this.invulnerable = true;
				this.invulnerableTime = 0.01f;
			}
			else
			{
				this.invulnerable = true;
				this.invulnerableTime = 1f;
			}
			this.enrageStartPos = base.transform.position;
			base.GetComponent<InvulnerabilityFlash>().enabled = false;
		}
		else
		{
			this.speed = this.defaultSpeed;
			this.fireRate = this.defaultFireRate;
			base.GetComponent<SpriteSM>().GetComponent<Renderer>().material.SetColor("_TintColor", Color.gray);
			this.gunSprite.GetComponent<Renderer>().material.SetColor("_TintColor", Color.gray);
			base.GetComponent<InvulnerabilityFlash>().enabled = true;
		}
	}

	public void SwitchToLeadBro()
	{
		this.isLeadBro = true;
		this.leadingBro.isLeadBro = false;
		BoondockBro.SetPlayerCharacter(HeroController.players[this.playerNum], this);
		if (base.IsMine)
		{
			Networking.RPC<Player, BoondockBro>(PID.TargetOthers, new RpcSignature<Player, BoondockBro>(BoondockBro.SetPlayerCharacter), HeroController.players[this.playerNum], this, false);
		}
		if (!GameModeController.ShowStandardHUDS())
		{
			HeroController.players[this.playerNum].deathMatchHUD.transform.parent = base.transform;
			HeroController.players[this.playerNum].deathMatchHUD.transform.localPosition = Vector3.zero;
			HeroController.players[this.playerNum].deathMatchHUD.Setup(HeroController.players[this.playerNum].Lives, this.playerNum, this);
		}
	}

	public static void SetPlayerCharacter(Player player, BoondockBro bro)
	{
		player.character = bro;
	}

	public override void RecallBro()
	{
		if (this.isLeadBro)
		{
			if (this.trailingBro != null)
			{
				this.trailingBro.RecallBro();
				this.isLeadBro = false;
				this.trailingBro = null;
			}
			if (this.connollyBro != null)
			{
				this.connollyBro.RecallBro();
			}
		}
		base.RecallBro();
	}

	protected override void StartDashing()
	{
		if (this.isLeadBro)
		{
			base.StartDashing();
		}
	}

	protected void ForceStartDash()
	{
		base.StartDashing();
	}

	protected override void RestartBubble()
	{
		if (this.isLeadBro)
		{
			base.RestartBubble();
		}
	}

	protected override void RestartBubble(float time)
	{
		if (this.isLeadBro)
		{
			base.RestartBubble(time);
		}
	}

	public BoondockBro leadingBro;

	public BoondockBro trailingBro;

	public BoondockBro connollyBro;

	public BillyConnolly billyConnollyPrefab;

	public Material secondBroMaterial;

	public Material avatar1;

	public Material avatar2;

	private InputState trackedInputState = new InputState();

	private bool needPathRecompute;

	private float inputDelay = 0.15f;

	private int position;

	protected float calibrateTimer;

	protected bool enraged;

	protected float enragedTimer;

	public bool isLeadBro = true;

	protected int lastTargetCol;

	protected int lastTargetRow;

	protected BroFollowMode followMode;

	private float copyCatDistanceSquared = 36f;

	protected float defaultSpeed;

	protected float defaultFireRate;

	private List<InputStateChange> inputChanges;

	private List<InputStateChange> fireInputChanges;

	private float repositionDelay = 0.2f;

	private float repositionTimeLeft = 0.4f;

	private bool frontBullet;

	private bool leadBroWasDashing;

	private float arbitraryTrackingDelay = 0.1f;

	private Vector3 enrageStartPos;
}
