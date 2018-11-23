// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class RescueBro : TestVanDammeAnim
{
	protected override void Awake()
	{
		base.Awake();
		this.SyncDestroy = true;
	}

	protected override void Start()
	{
		base.Start();
		if (HeroController.Instance != null && !HeroController.Instance.rescueBros.Contains(this))
		{
			HeroController.Instance.rescueBros.Add(this);
		}
		StatisticsController.RegisterRescueBro();
		this.invulnerable = true;
		this.invulnerableTime = 0f;
	}

	public override bool Revive(int playerNum, bool isUnderPlayerControl, TestVanDammeAnim reviveSource)
	{
		return false;
	}

	public void Free()
	{
		if (!this.freed)
		{
			StatisticsController.NotifyRescue();
		}
		this.freed = true;
		base.SetInvulnerable(0.75f, false);
	}

	public void BeginRescueAnim()
	{
		if (this.rescueState == RescueBro.RescueState.Idle)
		{
			this.rescueState = RescueBro.RescueState.BreakingConstraints;
			this.frame = 0;
			base.SetInvulnerable(float.PositiveInfinity, true);
			this.saveMeBubble.gameObject.SetActive(false);
		}
	}

	public override void DestroyUnit()
	{
		Networking.RPC<RescueBro>(PID.TargetAll, new RpcSignature<RescueBro>(HeroController.Instance.DestroyRescueBroRPC), this, false);
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		base.Death(xI, yI, damage);
	}

	protected override void Update()
	{
		if (Map.isEditing)
		{
			return;
		}
		this.t = Time.deltaTime;
		if (this.rescueState == RescueBro.RescueState.AwaitingSpawn)
		{
			if (this.flashTimer > 0f)
			{
				this.flashTimer -= Time.deltaTime;
			}
			else
			{
				this.ReadyToBeDestroyed = true;
			}
		}
		if (this.DestroyWhenReady && this.ReadyToBeDestroyed)
		{
			if (this.pauseTimer <= 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				this.pauseTimer -= Time.deltaTime;
			}
		}
		if (this.rescueState == RescueBro.RescueState.Idle)
		{
			if (this.freed)
			{
				this.saveMeTimer += this.t;
			}
			else
			{
				this.saveMeTimer += this.t * 0.33f;
			}
			if (this.saveMeTimer > 1.5f)
			{
				this.saveMeTimer -= 1.5f;
				this.saveMeBubble.RestartBubble(0.6f);
			}
		}
		base.Update();
	}

	protected override void RunMovement()
	{
		if (this.rescueState != RescueBro.RescueState.BreakingConstraints)
		{
			base.RunMovement();
		}
	}

	protected override void AnimateIdle()
	{
		switch (this.rescueState)
		{
		case RescueBro.RescueState.Idle:
			base.AnimateIdle();
			break;
		case RescueBro.RescueState.BreakingConstraints:
			this.RunBreakingConstraintsAnim();
			break;
		case RescueBro.RescueState.AwaitingSpawn:
			this.RunAwaitingSpawnAnim();
			break;
		}
	}

	protected override void AnimateDeath()
	{
		switch (this.rescueState)
		{
		case RescueBro.RescueState.Idle:
			base.AnimateDeath();
			break;
		case RescueBro.RescueState.BreakingConstraints:
			this.RunBreakingConstraintsAnim();
			break;
		case RescueBro.RescueState.AwaitingSpawn:
			this.RunAwaitingSpawnAnim();
			break;
		}
	}

	private void RunBreakingConstraintsAnim()
	{
		this.frameRate = 0.0667f;
		int num = 21 + Mathf.Clamp(this.frame, 0, 9);
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), 32f);
		if (this.frame >= 11)
		{
			this.rescueState = RescueBro.RescueState.AwaitingSpawn;
			this.frame = 0;
		}
	}

	private void RunAwaitingSpawnAnim()
	{
		this.frameRate = 0.03333f;
		int num = 30 + this.frame % 2;
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), 32f);
	}

	protected override void AnimateJumping()
	{
		switch (this.rescueState)
		{
		case RescueBro.RescueState.Idle:
			base.AnimateJumping();
			break;
		case RescueBro.RescueState.BreakingConstraints:
			this.RunBreakingConstraintsAnim();
			break;
		case RescueBro.RescueState.AwaitingSpawn:
			this.RunAwaitingSpawnAnim();
			break;
		}
	}

	protected override void OnDestroy()
	{
		if (this.showHeroOnDestroy != null)
		{
			this.showHeroOnDestroy.ShowCharacter();
		}
		base.OnDestroy();
	}

	[HideInInspector]
	public RescueBro.RescueState rescueState;

	[HideInInspector]
	public bool isBeingRescued;

	[HideInInspector]
	public bool DestroyWhenReady;

	[HideInInspector]
	public bool ReadyToBeDestroyed;

	public TestVanDammeAnim showHeroOnDestroy;

	public ReactionBubble saveMeBubble;

	protected float saveMeTimer;

	private float flashTimer = 0.35f;

	[HideInInspector]
	public bool freed;

	private float pauseTimer;

	public enum RescueState
	{
		Idle,
		BreakingConstraints,
		AwaitingSpawn
	}
}
