// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CameraMoveAction : TriggerAction
{
	public override TriggerActionInfo Info
	{
		get
		{
			return this.info;
		}
		set
		{
			this.info = (CameraActionInfo)value;
		}
	}

	public override void Update()
	{
		if (this.state == TriggerActionState.Done || TriggerManager.PauseCameraMovements)
		{
			if (this.info.holdPlayersInCutscene)
			{
				CutsceneController.HoldPlayersStill(true);
			}
			else if (this.info.killOffscreenPlayers)
			{
				TriggerManager.destroyOffscreenPlayers = true;
			}
			return;
		}
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.03334f);
		SortOfFollow.ControlledByTriggerAction = true;
		this.timeLeft -= num;
		this.lerpAmt = ((this.info.time != 0f) ? ((this.info.time - this.timeLeft) / this.info.time) : 1f);
		if (this.info.smootherCamMovement)
		{
			Vector3 to = Vector3.Lerp(this.startPos, this.targetPos, this.lerpAmt);
			SortOfFollow.followPos = Vector3.Lerp(SortOfFollow.followPos, to, Time.deltaTime * 2f);
		}
		else
		{
			SortOfFollow.followPos = Vector3.Lerp(this.startPos, this.targetPos, this.lerpAmt);
		}
		SortOfFollow.SetZoomLevel(Mathf.Lerp(this.startZoom, this.info.zoom, this.lerpAmt));
		if (this.timeLeft <= 0f)
		{
			this.currentPos++;
			if (this.info.posList != null && this.currentPos < this.info.posList.Count)
			{
				this.smoothLerp = 0f; this.lerpAmt = (this.smoothLerp );
				this.timeLeft = this.info.times[this.currentPos];
				this.startPos = this.targetPos;
				this.targetPos = new Vector3(Map.GetBlocksX(this.info.posList[this.currentPos].collumn - Map.lastXLoadOffset), Map.GetBlocksY(this.info.posList[this.currentPos].row - Map.lastYLoadOffset), this.startPos.z);
				this.startZoom = SortOfFollow.zoomLevel;
			}
			else
			{
				if (!this.info.smootherCamMovement)
				{
					SortOfFollow.followPos = this.targetPos;
				}
				SortOfFollow.SetZoomLevel(this.info.zoom);
				this.state = TriggerActionState.Done;
				SortOfFollow.ControlledByTriggerAction = false;
				SortOfFollow.SetZoomLevel(1f);
				if (this.info.holdPlayersInCutscene)
				{
					CutsceneController.HoldPlayersStill(false);
				}
				Time.timeScale = 1f;
			}
		}
		else
		{
			if (this.info.holdPlayersInCutscene)
			{
				CutsceneController.HoldPlayersStill(true);
				if (Time.time - this.timeStart > 0.5f)
				{
					if (InputReader.GetControllerHoldingFire() >= 0)
					{
						Time.timeScale = Mathf.Lerp(Time.timeScale, 3f, Time.deltaTime * 18f);
					}
					else
					{
						Time.timeScale = 1f;
					}
				}
			}
			if (this.info.killOffscreenPlayers)
			{
				TriggerManager.destroyOffscreenPlayers = true;
			}
		}
	}

	public override void Reset()
	{
		this.state = TriggerActionState.Waiting;
	}

	public override void Start()
	{
		this.timeStart = Time.time;
		this.startPos = Camera.main.transform.position;
		this.targetPos = new Vector3(Map.GetBlocksX(this.info.targetPoint.collumn - Map.lastXLoadOffset), Map.GetBlocksY(this.info.targetPoint.row - Map.lastYLoadOffset), this.startPos.z);
		this.timeLeft = this.info.time;
		this.startZoom = SortOfFollow.zoomLevel;
		this.state = TriggerActionState.Busy;
		SortOfFollow.ControlledByTriggerAction = true;
		if (this.info.letterbox)
		{
			if (this.info.letterboxAmount == 0f)
			{
				LetterBoxController.ClearLetterBox(Mathf.Clamp(this.info.time, 0f, 4f));
				HeroController.EnableHud();
			}
			else
			{
				LetterBoxController.ShowLetterBox(this.info.letterboxAmount, this.info.time);
				HeroController.DisableHud();
			}
		}
		if (this.info.holdPlayersInCutscene)
		{
			HeroController.SetAllHeroesInvulnerable(this.timeLeft + 0.5f);
		}
	}

	protected Vector3 startPos;

	protected Vector3 targetPos;

	private CameraActionInfo info;

	protected float startZoom;

	protected float timeLeft;

	protected float timeStart;

	private int currentPos = -1;

	private float lerpAmt;

	private float smoothLerp;
}
