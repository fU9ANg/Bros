// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MammothKopterAI : PolymorphicAI
{
	protected void Start()
	{
		this.mainCamera = Camera.main;
	}

	protected override void Think()
	{
		if (this.mamKopterThinkState == MammothKopterAI.ThinkState.WaitingForAnim)
		{
			if (!base.GetComponent<MammothKopter>().weapon.GetComponent<HelicopterBossChaingun>().playingIntroAnim)
			{
				if (base.IsMine)
				{
					Networking.RPC(PID.TargetAll, new RpcSignature(this.FinishedWaitingRPC), false);
				}
			}
		}
	}

	private void FinishedWaitingRPC()
	{
		this.mamKopterThinkState = MammothKopterAI.ThinkState.ShootAtBottom;
		TriggerManager.PauseCameraMovements = false;
		if (PlayerOptions.Instance.hardMode)
		{
			base.GetComponent<MammothKopter>().EnableGrenadeLauncher();
		}
	}

	public override void GetInput(ref bool left, ref bool right, ref bool up, ref bool down, ref bool jump, ref bool fire, ref bool special, ref bool special2, ref bool special3, ref bool special4)
	{
		special = false; up = (down = (left = (right = (fire = (special )))));
		if (this.mamKopterThinkState == MammothKopterAI.ThinkState.Approaching)
		{
			if (base.transform.position.y < this.targetY)
			{
				up = true;
			}
			else if (base.transform.position.y > this.targetY)
			{
				down = true;
			}
			if (base.transform.position.x < this.targetX - 8f)
			{
				right = true;
			}
			else if (base.transform.position.x > this.targetX + 8f)
			{
				left = true;
			}
			else
			{
				base.GetComponent<MammothKopter>().xI = Mathf.Lerp(base.GetComponent<MammothKopter>().xI, 0f, Time.deltaTime * 10f);
			}
			if (Mathf.Abs(base.transform.position.y - this.targetY) < 64f)
			{
				if (this.horizontalMode)
				{
					this.mamKopterThinkState = MammothKopterAI.ThinkState.Bomb;
				}
				else
				{
					base.GetComponent<MammothKopter>().weapon.GetComponent<HelicopterBossChaingun>().StartIntroAnim();
					TriggerManager.PauseCameraMovements = true;
					this.mamKopterThinkState = MammothKopterAI.ThinkState.WaitingForAnim;
				}
			}
		}
		else if (this.mamKopterThinkState == MammothKopterAI.ThinkState.ShootAtBottom)
		{
			if (base.transform.position.y < this.mainCamera.transform.position.y + 24f)
			{
				fire = true;
			}
			if ((double)base.transform.position.y < (double)this.mainCamera.transform.position.y - (double)this.mainCamera.orthographicSize * 0.7 + 24.0)
			{
				up = true;
			}
			else if (base.transform.position.y > this.mainCamera.transform.position.y)
			{
				down = true;
			}
		}
		else if (this.mamKopterThinkState == MammothKopterAI.ThinkState.Bomb)
		{
			special = true;
			if (!SortOfFollow.ControlledByTriggerAction || (double)base.transform.position.x < (double)this.mainCamera.transform.position.x - (double)(this.mainCamera.aspect * this.mainCamera.orthographicSize) * 0.25)
			{
				right = true;
			}
			if ((double)base.transform.position.y < (double)this.mainCamera.transform.position.y + (double)this.mainCamera.orthographicSize * 0.83)
			{
				up = true;
			}
			else if ((double)base.transform.position.y > (double)this.mainCamera.transform.position.y + (double)this.mainCamera.orthographicSize * 0.88)
			{
				down = true;
			}
		}
	}

	private void StartApproach()
	{
		base.GetComponent<MammothKopter>().TeleportTo(29, 1);
		this.mamKopterThinkState = MammothKopterAI.ThinkState.Approaching;
		Map.GetBlocksXY(ref this.targetX, ref this.targetY, 9, 8);
	}

	private void SwitchToHorizontal()
	{
		this.horizontalMode = true;
		this.mamKopterThinkState = MammothKopterAI.ThinkState.Approaching;
		base.GetComponent<MammothKopter>().TeleportTo(99, 16);
		Map.GetBlocksXY(ref this.targetX, ref this.targetY, 85, 43);
		base.GetComponent<MammothKopter>().tankSpeed = 150f;
		base.GetComponent<MammothKopter>().weapon.GetComponent<HelicopterBossChaingun>().StopShooting();
		base.GetComponent<MammothKopter>().weapon.GetComponent<HelicopterBossChaingun>().ExtendChaingunInstantly();
		if (!PlayerOptions.Instance.hardMode)
		{
			base.GetComponent<MammothKopter>().grenadeLauncherEnabled = false;
		}
	}

	private void DisableCamera()
	{
		TriggerManager.ClearActiveCameraActions();
	}

	private Camera mainCamera;

	private float targetX;

	private float targetY;

	public MammothKopterAI.ThinkState mamKopterThinkState;

	private bool horizontalMode;

	[HideInInspector]
	public float moveUpDelay;

	[HideInInspector]
	private bool movingUp;

	private float maxZipLineLength = 256f;

	private Vector3 zipAttachPoint;

	public enum ThinkState
	{
		Waiting,
		Approaching,
		MoveToMiddleAndShoot,
		ShootAtBottom,
		MovingToZiplinePoint,
		DeployMooks,
		Bomb,
		WaitingForAnim
	}
}
