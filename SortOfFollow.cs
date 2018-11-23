// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class SortOfFollow : NetworkObject
{
	public static void ForceSlowSnapBack(float time, float zoom)
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Force slow snap back... from zoomlevel ",
			SortOfFollow.zoomLevel,
			" ... from snap back pos ",
			SortOfFollow.instance.transform.position
		}));
		if (SortOfFollow.instance.snapBackTime > 0f)
		{
			SortOfFollow.zoomLevel = Mathf.Lerp(SortOfFollow.instance.snapBackFromZoom, SortOfFollow.zoomLevel, (SortOfFollow.instance.snapBackDuration - SortOfFollow.instance.snapBackTime) / SortOfFollow.instance.snapBackDuration);
		}
		SortOfFollow.instance.snapBackTime = time;
		SortOfFollow.instance.snapBackDuration = time;
		SortOfFollow.instance.snapBackFromPos = SortOfFollow.instance.transform.position;
		SortOfFollow.instance.snapBackFromZoom = SortOfFollow.zoomLevel;
		SortOfFollow.zoomLevel = zoom;
	}

	public static void ForceSlowSnapBack(float time)
	{
		float num = SortOfFollow.zoomLevel;
		if (SortOfFollow.instance.snapBackTime > 0f)
		{
			num = Mathf.Lerp(SortOfFollow.instance.snapBackFromZoom, SortOfFollow.zoomLevel, (SortOfFollow.instance.snapBackDuration - SortOfFollow.instance.snapBackTime) / SortOfFollow.instance.snapBackDuration);
		}
		SortOfFollow.instance.snapBackTime = time;
		SortOfFollow.instance.snapBackDuration = time;
		SortOfFollow.instance.snapBackFromPos = SortOfFollow.instance.transform.position;
		SortOfFollow.instance.snapBackFromZoom = num;
	}

	public static bool ControlledByTriggerAction
	{
		get
		{
			return SortOfFollow.controlledByTriggerAction;
		}
		set
		{
			if (!value && SortOfFollow.controlledByTriggerAction)
			{
				if (SortOfFollow.instance.snapBackTime > 0f)
				{
					SortOfFollow.zoomLevel = Mathf.Lerp(SortOfFollow.instance.snapBackFromZoom, SortOfFollow.zoomLevel, (SortOfFollow.instance.snapBackDuration - SortOfFollow.instance.snapBackTime) / SortOfFollow.instance.snapBackDuration);
				}
				SortOfFollow.instance.snapBackTime = 1f;
				SortOfFollow.instance.snapBackDuration = 1f;
				SortOfFollow.instance.snapBackFromPos = SortOfFollow.instance.transform.position;
				SortOfFollow.instance.snapBackFromZoom = SortOfFollow.zoomLevel;
			}
			SortOfFollow.controlledByTriggerAction = value;
		}
	}

	public static void ReturnToNormal(float time)
	{
		if (SortOfFollow.instance.snapBackTime > 0f)
		{
			SortOfFollow.zoomLevel = Mathf.Lerp(SortOfFollow.instance.snapBackFromZoom, SortOfFollow.zoomLevel, (SortOfFollow.instance.snapBackDuration - SortOfFollow.instance.snapBackTime) / SortOfFollow.instance.snapBackDuration);
		}
		SortOfFollow.instance.snapBackTime = time;
		SortOfFollow.instance.snapBackDuration = 1f;
		SortOfFollow.instance.snapBackFromPos = SortOfFollow.instance.transform.position;
		SortOfFollow.instance.snapBackFromZoom = SortOfFollow.zoomLevel;
		SortOfFollow.controlledByTriggerAction = false;
		SortOfFollow.zoomLevel = 1f;
	}

	private new void OnDestroy()
	{
		SortOfFollow.ReturnToNormal(0f);
	}

	protected float currentOrthographicSize
	{
		get
		{
			return Camera.main.orthographicSize;
		}
		set
		{
			Camera.main.orthographicSize = value;
		}
	}

	public static void Shake(float m)
	{
		if (SortOfFollow.shakeM < m)
		{
			SortOfFollow.shakeM = m;
			SortOfFollow.sinXRate = 52f + UnityEngine.Random.value * 64f;
			SortOfFollow.sinYRate = 30f + UnityEngine.Random.value * 64f;
		}
	}

	public static void Shake(float m, Vector3 position)
	{
		float f = position.x - SortOfFollow.x;
		float f2 = position.y - SortOfFollow.y;
		float num = Mathf.Clamp01(Mathf.Clamp01(2f - Mathf.Abs(f) / 180f) * Mathf.Clamp01(2f - Mathf.Abs(f2) / 140f));
		SortOfFollow.Shake(m * num);
	}

	public static void Shake(float m, float speedM)
	{
		if (SortOfFollow.shakeM < m)
		{
			SortOfFollow.shakeM = m;
			SortOfFollow.sinXRate = (52f + UnityEngine.Random.value * 64f) * speedM;
			SortOfFollow.sinYRate = (30f + UnityEngine.Random.value * 64f) * speedM;
		}
	}

	public static void Shake(float m, float speedM, Vector3 position)
	{
		float f = position.x = SortOfFollow.x;
		float f2 = position.y - SortOfFollow.y;
		float num = Mathf.Clamp01(Mathf.Clamp01(2f - Mathf.Abs(f) / 250f) * Mathf.Clamp01(2f - Mathf.Abs(f2) / 160f));
		SortOfFollow.Shake(m * num, speedM);
	}

	public static SortOfFollow GetInstance()
	{
		return SortOfFollow.instance;
	}

	public static float GetWorldScreenWidth()
	{
		return SortOfFollow.worldScreenWidth;
	}

	public static float GetWorldScreenHeight()
	{
		return SortOfFollow.worldScreenHeight;
	}

	public static float GetScreenMinY()
	{
		return SortOfFollow.minY;
	}

	public static float GetScreenMaxY()
	{
		return SortOfFollow.maxY;
	}

	public static float GetScreenMinX()
	{
		return SortOfFollow.minX;
	}

	public static float GetScreenMaxX()
	{
		return SortOfFollow.maxX;
	}

	private void Awake()
	{
		SortOfFollow.controlledByTriggerAction = false;
		this.defaultPosition = new Vector3(256f, 196f, 0f);
		SortOfFollow.instance = this;
		global::Math.SetupLookupTables();
		SortOfFollow.followPos = this.defaultPosition;
		base.transform.position = new Vector3(SortOfFollow.followPos.x, SortOfFollow.followPos.y, base.transform.position.z);
		this.CalculateCameraWorldExtents();
		float explosionRunFailM = GameModeController.GetExplosionRunFailM();
		if (GameModeController.GetExplosionRunTotalAttempts() > 2)
		{
			this.moveSpeedSuccessM = 1.25f - 0.5f * explosionRunFailM;
		}
		else
		{
			this.moveSpeedSuccessM = 1f - 0.2f * explosionRunFailM;
		}
	}

	protected void ZoomToHorizontalGameplayExtents(float t)
	{
		if (this.snapBackTime > 0f)
		{
			base.GetComponent<Camera>().orthographicSize = 227.555557f / base.GetComponent<Camera>().aspect * Mathf.Lerp(this.snapBackFromZoom, SortOfFollow.zoomLevel, (this.snapBackDuration - this.snapBackTime) / this.snapBackDuration);
		}
		else
		{
			base.GetComponent<Camera>().orthographicSize = 227.555557f / base.GetComponent<Camera>().aspect;
		}
	}

	public void CalculateCameraWorldExtents()
	{
		SortOfFollow.minWorldPoint = base.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(0f, 0f, 1f));
		SortOfFollow.maxWorldPoint = base.GetComponent<Camera>().ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 1f));
	}

	protected void CheckScreenExtents()
	{
		this.CalculateCameraWorldExtents();
		if (base.GetComponent<Camera>().orthographicSize != this.currentOrthographicSize || SortOfFollow.minY < -60f)
		{
			this.currentOrthographicSize = base.GetComponent<Camera>().orthographicSize;
		}
		Vector3 vector = SortOfFollow.maxWorldPoint - SortOfFollow.minWorldPoint;
		SortOfFollow.worldScreenWidth = vector.x;
		SortOfFollow.worldScreenHeight = vector.y;
		this.centerX = vector.x / 2f + 32f;
		SortOfFollow.minY = SortOfFollow.minWorldPoint.y;
		SortOfFollow.maxY = SortOfFollow.maxWorldPoint.y;
		SortOfFollow.minX = SortOfFollow.minWorldPoint.x;
		SortOfFollow.maxX = SortOfFollow.maxWorldPoint.x;
	}

	public static bool IsItSortOfVisible(Vector3 worldPos)
	{
		return SortOfFollow.IsItSortOfVisible(worldPos, 28f, 28f);
	}

	public static bool IsItSortOfVisible(Vector3 worldPos, float xBuffer, float yBuffer)
	{
		return worldPos.x >= SortOfFollow.minWorldPoint.x - xBuffer && worldPos.x <= SortOfFollow.maxWorldPoint.x + xBuffer && worldPos.y >= SortOfFollow.minWorldPoint.y - yBuffer && worldPos.y <= SortOfFollow.maxWorldPoint.y + yBuffer;
	}

	public static bool IsItSortOfVisible(float x, float y)
	{
		return SortOfFollow.IsItSortOfVisible(x, y, 28f, 28f);
	}

	public static bool IsItSortOfVisible(float x, float y, float xBuffer, float yBuffer)
	{
		return x >= SortOfFollow.minWorldPoint.x - xBuffer && x <= SortOfFollow.maxWorldPoint.x + xBuffer && y >= SortOfFollow.minWorldPoint.y - yBuffer && y <= SortOfFollow.maxWorldPoint.y + yBuffer;
	}

	public static bool IsItSortOfVisible(Vector3 worldPos, float xBuffer, float yBuffer, ref float xOffSet, ref float yOffSet)
	{
		if (worldPos.x < SortOfFollow.minWorldPoint.x - xBuffer)
		{
			xOffSet = worldPos.x - SortOfFollow.minWorldPoint.x + xBuffer;
		}
		else if (worldPos.x > SortOfFollow.maxWorldPoint.x + xBuffer)
		{
			xOffSet = worldPos.x - SortOfFollow.maxWorldPoint.x - xBuffer;
		}
		else
		{
			xOffSet = 0f;
		}
		if (worldPos.y < SortOfFollow.minWorldPoint.y - yBuffer)
		{
			yOffSet = worldPos.y - SortOfFollow.minWorldPoint.y + yBuffer;
		}
		else if (worldPos.y > SortOfFollow.maxWorldPoint.y + yBuffer)
		{
			yOffSet = worldPos.y - SortOfFollow.maxWorldPoint.y - yBuffer;
		}
		else
		{
			yOffSet = 0f;
			if (xOffSet == 0f)
			{
				return true;
			}
		}
		return Mathf.Abs(xOffSet) < xBuffer && Mathf.Abs(yOffSet) < yBuffer;
	}

	public static void ForcePosition(Vector3 newPos)
	{
		if (SortOfFollow.instance == null)
		{
			MonoBehaviour.print("instance is null");
			return;
		}
		SortOfFollow.followPos = newPos;
		if (SortOfFollow.followPos.y < 128f)
		{
			SortOfFollow.followPos.y = 128f;
		}
		if (SortOfFollow.followPos.x < 256f)
		{
			SortOfFollow.followPos.x = 256f;
		}
		SortOfFollow.instance.followPosLerp = SortOfFollow.followPos;
		SortOfFollow.instance.SetRoundedPosition(SortOfFollow.instance.ClampPosition(new Vector3(SortOfFollow.followPos.x, SortOfFollow.followPos.y, SortOfFollow.instance.transform.position.z)));
		SortOfFollow.instance.SetPosition();
		SortOfFollow.x = SortOfFollow.instance.transform.position.x;
		SortOfFollow.y = SortOfFollow.instance.transform.position.y;
	}

	public static void ResetSpeed()
	{
		SortOfFollow.instance.moveSpeed = Map.MapData.cameraSpeed * SortOfFollow.instance.lastCheckPointSpeedM;
		SortOfFollow.instance.moveExtraSpeed = 0f;
		SortOfFollow.instance.moveSpeedBoostM = 0f;
	}

	public static void SlowTimeDown(float time)
	{
		SortOfFollow.instance.SlowTimeDownInternal(time);
	}

	protected void SetSpeed(float m)
	{
		this.lastCheckPointSpeedM = m;
		SortOfFollow.instance.moveSpeed = Map.MapData.cameraSpeed * m;
		SortOfFollow.instance.moveSpeedBoostM = 0f;
	}

	public static void RegisterExplosionRunCheckPoint(CheckPointExplosionRun checkPoint)
	{
		SortOfFollow.instance.explosionRunCheckPoints.Add(checkPoint);
	}

	public void CheckCheckPoints()
	{
		foreach (CheckPointExplosionRun checkPointExplosionRun in this.explosionRunCheckPoints)
		{
			if (checkPointExplosionRun != null && !checkPointExplosionRun.explosionRunDirecitonUsed && SortOfFollow.IsItSortOfVisible(checkPointExplosionRun.transform.position, -48f, -32f))
			{
				if (this.followMode == CameraFollowMode.ForcedHorizontal && checkPointExplosionRun.cameraFollowMode != this.followMode)
				{
					if (this.centerX + this.moveOffset > checkPointExplosionRun.transform.position.x)
					{
						this.centerX = checkPointExplosionRun.transform.position.x;
						this.centerY = SortOfFollow.y;
						this.moveOffset = 0f;
						this.followMode = checkPointExplosionRun.cameraFollowMode;
						checkPointExplosionRun.explosionRunDirecitonUsed = true;
						this.moveDelay = 0.25f;
						this.SetSpeed(checkPointExplosionRun.speedM);
						UnityEngine.Debug.Log("Set VERTICAL");
					}
				}
				else if (((this.followMode == CameraFollowMode.ForcedVertical && this.centerY + this.moveOffset > checkPointExplosionRun.transform.position.y) || (this.followMode == CameraFollowMode.ForcedDescent && this.centerY - this.moveOffset < checkPointExplosionRun.transform.position.y)) && checkPointExplosionRun.cameraFollowMode != this.followMode)
				{
					this.centerY = checkPointExplosionRun.transform.position.y;
					this.centerX = SortOfFollow.x;
					this.moveOffset = 0f;
					this.followMode = checkPointExplosionRun.cameraFollowMode;
					checkPointExplosionRun.explosionRunDirecitonUsed = true;
					this.moveDelay = 1f;
					this.SetSpeed(checkPointExplosionRun.speedM);
					UnityEngine.Debug.Log("Set HORIZONTAL");
				}
			}
		}
	}

	public float GetMinY()
	{
		return SortOfFollow.minY;
	}

	private void Start()
	{
		if (GameModeController.IsDeathMatchMode || GameModeController.GameMode == GameMode.BroDown)
		{
			this.followMode = CameraFollowMode.DescendInStages;
		}
		this.CalculateCameraWorldExtents();
		Vector3 vector = SortOfFollow.maxWorldPoint - SortOfFollow.minWorldPoint;
		SortOfFollow.worldScreenWidth = vector.x;
		this.centerX = vector.x / 2f + 32f;
		this.centerY = this.currentOrthographicSize;
		if (GameModeController.GameMode == GameMode.ExplosionRun && this.followMode == CameraFollowMode.ForcedVertical)
		{
			this.moveExtraSpeed = -this.moveSpeed * 0.8f;
		}
		if (this.totalSpeedText != null && this.speedSuccessMultiplierSpeedText != null && this.extraSpeedText != null && this.successWinRatioText != null && (GameModeController.GameMode != GameMode.ExplosionRun || !SortOfFollow.showingSpeed))
		{
			this.ShowSpeedTexts(false);
		}
	}

	protected void ShowSpeedTexts(bool show)
	{
		this.totalSpeedText.gameObject.SetActive(show);
		this.speedSuccessMultiplierSpeedText.gameObject.SetActive(show);
		this.extraSpeedText.gameObject.SetActive(show);
		this.successWinRatioText.gameObject.SetActive(show);
	}

	protected override void LateUpdate()
	{
		base.LateUpdate();
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		SortOfFollow.sinXCounter += SortOfFollow.sinXRate * this.t;
		SortOfFollow.sinYCounter += SortOfFollow.sinYRate * this.t;
		SortOfFollow.sinXRate *= 1f - this.t * 4f;
		SortOfFollow.sinYRate *= 1f - this.t * 4f;
		if (SortOfFollow.shakeM > 0f)
		{
			SortOfFollow.shakeM -= this.t;
			if (SortOfFollow.shakeM <= 0f)
			{
				SortOfFollow.shakeM = 0f;
			}
		}
		if (this.totalSpeedText != null && this.speedSuccessMultiplierSpeedText != null && this.extraSpeedText != null && this.successWinRatioText != null)
		{
			if (GameModeController.GameMode == GameMode.ExplosionRun)
			{
				if (SortOfFollow.showingSpeed)
				{
					this.totalSpeedText.text = string.Empty + (int)((this.moveSpeed * (1f + this.moveSpeedBoostM) + this.moveExtraSpeed) * this.moveSpeedSuccessM);
					this.extraSpeedText.text = string.Empty + (int)this.moveExtraSpeed;
					this.speedSuccessMultiplierSpeedText.text = string.Empty + Mathf.Round(this.moveSpeedSuccessM * 20f) / 20f;
					this.successWinRatioText.text = string.Empty + Mathf.Round((1f - GameModeController.GetExplosionRunFailM()) * 20f) / 20f;
				}
				else if (!SortOfFollow.showingSpeed && Input.GetKeyDown(KeyCode.F7))
				{
					SortOfFollow.showingSpeed = true;
					this.ShowSpeedTexts(true);
				}
			}
			else
			{
				this.ShowSpeedTexts(false);
			}
		}
		if (this.followMode == CameraFollowMode.DescendInStages && !Map.isEditing)
		{
			if (this.lastHighestGroundHeight != Map.GetHighestSolidBlock())
			{
				this.lastHighestGroundHeight = Map.GetHighestSolidBlock();
				this.descentOffsetOffset = Mathf.Clamp(this.descentOffsetOffset + 6f, 0f, 64f);
			}
			this.descentOffset = (float)((Map.GetHighestSolidBlock() + -13) * 16) + this.currentOrthographicSize + this.descentOffsetOffset;
			if (this.currentDescentOffset > this.descentOffset)
			{
				this.currentDescentOffset = Mathf.Clamp(this.currentDescentOffset - 100f * this.t, this.descentOffset, 1000000f);
			}
			else if (this.currentDescentOffset < this.descentOffset)
			{
				this.currentDescentOffset = Mathf.Clamp(this.currentDescentOffset + 200f * this.t, this.descentOffset, 1000000f);
			}
		}
		if (this.followMode == CameraFollowMode.ForcedHorizontal || this.followMode == CameraFollowMode.ForcedVertical || this.followMode == CameraFollowMode.ForcedDescent)
		{
			if (HeroController.GetPlayersOnHelicopterAmount() > 0 && GameModeController.LevelFinished && (GameModeController.GameMode == GameMode.SuicideHorde || GameModeController.GameMode == GameMode.ExplosionRun || GameModeController.GameMode == GameMode.Race))
			{
				this.followMode = CameraFollowMode.Normal;
				this.snapBackTime = 1f;
				this.snapBackDuration = 1f;
				this.snapBackFromPos = base.transform.position;
				this.snapBackFromZoom = SortOfFollow.zoomLevel;
			}
			if (this.moveDelay <= 0f)
			{
				this.moveExtraSpeed = Mathf.Clamp(this.moveExtraSpeed + this.moveSpeed / 2f * this.t, -this.moveSpeed * 0.99f, this.moveSpeed * 0.66f);
				if (this.moveSpeedBoostM > 0f)
				{
					this.moveSpeedBoostM = Mathf.Clamp(this.moveSpeedBoostM - this.t * 0.5f, 0f, 2f);
				}
				if (HeroController.mustShowHUDS)
				{
					CameraFollowMode cameraFollowMode = this.followMode;
					if (cameraFollowMode != CameraFollowMode.ForcedVertical)
					{
						if (cameraFollowMode != CameraFollowMode.ForcedHorizontal)
						{
							if (cameraFollowMode != CameraFollowMode.ForcedDescent)
							{
								this.arrowLeft.SetActive(false);
								this.arrowRight.SetActive(false);
								this.arrowUp.SetActive(false);
								this.arrowDown.SetActive(false);
							}
							else
							{
								this.arrowLeft.SetActive(false);
								this.arrowRight.SetActive(false);
								this.arrowUp.SetActive(false);
								this.arrowDown.SetActive(true);
							}
						}
						else
						{
							this.arrowLeft.SetActive(false);
							this.arrowRight.SetActive(true);
							this.arrowUp.SetActive(false);
							this.arrowDown.SetActive(false);
						}
					}
					else
					{
						this.arrowLeft.SetActive(false);
						this.arrowRight.SetActive(false);
						this.arrowUp.SetActive(true);
						this.arrowDown.SetActive(false);
					}
				}
				else
				{
					this.arrowLeft.SetActive(false);
					this.arrowRight.SetActive(false);
					this.arrowUp.SetActive(false);
					this.arrowDown.SetActive(false);
				}
			}
			this.CheckCheckPoints();
			if (Input.GetKeyDown(KeyCode.Minus))
			{
				this.moveSpeed *= 0.8f;
			}
			if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.Equals))
			{
				this.moveSpeed *= 1.25f;
			}
		}
		if (!Map.isEditing)
		{
			if (this.followMode == CameraFollowMode.Vertical || this.followMode == CameraFollowMode.ForcedVertical || this.followMode == CameraFollowMode.ForcedDescent || this.followMode == CameraFollowMode.SingleScreen || this.followMode == CameraFollowMode.DescendInStages)
			{
				if (!this.haveZoomed)
				{
					this.haveZoomed = true;
					this.ZoomToHorizontalGameplayExtents(1f);
				}
			}
			else if (this.snapBackTime > 0f)
			{
				CameraController.OrthographicSize = 128f * Mathf.Lerp(this.snapBackFromZoom, SortOfFollow.zoomLevel, (this.snapBackDuration - this.snapBackTime) / this.snapBackDuration);
			}
			else
			{
				CameraController.OrthographicSize = 128f * SortOfFollow.zoomLevel;
			}
		}
		this.CheckScreenExtents();
		if (SortOfFollow.controlledByTriggerAction || HeroController.GetGetFollowPosition(ref SortOfFollow.followPos))
		{
			if (!this.hasFollowed || SortOfFollow.controlledByTriggerAction)
			{
				if (SortOfFollow.followPos.x > 0f || SortOfFollow.followPos.y > 0f)
				{
					if (!SortOfFollow.controlledByTriggerAction)
					{
						if (SortOfFollow.followPos.y < 128f)
						{
							SortOfFollow.followPos.y = 128f;
						}
						if (SortOfFollow.followPos.x < 256f)
						{
							SortOfFollow.followPos.x = 256f;
						}
					}
					this.followPosLerp = SortOfFollow.followPos;
					base.transform.position = new Vector3(SortOfFollow.followPos.x, SortOfFollow.followPos.y, base.transform.position.z);
				}
			}
			else if (SortOfFollow.followPos.x > 0f || SortOfFollow.followPos.y > 0f)
			{
				if (!SortOfFollow.controlledByTriggerAction)
				{
					if (SortOfFollow.followPos.y < 128f)
					{
						SortOfFollow.followPos.y = 128f;
					}
					if (SortOfFollow.followPos.x < 256f)
					{
						SortOfFollow.followPos.x = 256f;
					}
				}
				this.followPosLerp = Vector3.Lerp(this.followPosLerp, SortOfFollow.followPos, this.t * 10f);
				this.SetPosition();
			}
			if (SortOfFollow.controlledByTriggerAction)
			{
				this.SetPosition();
			}
			this.hasFollowed = true;
		}
		else
		{
			this.followPosLerp = base.transform.position; SortOfFollow.followPos = (this.followPosLerp );
		}
		SortOfFollow.x = base.transform.position.x;
		SortOfFollow.y = base.transform.position.y;
	}

	protected void SetPosition()
	{
		if (Map.isEditing)
		{
			this.SetRoundedPosition(new Vector3(this.followPosLerp.x + Mathf.Sin(SortOfFollow.sinXCounter) * SortOfFollow.shakeM * 6f, this.followPosLerp.y + Mathf.Sin(SortOfFollow.sinYCounter) * SortOfFollow.shakeM * 6f, base.transform.position.z));
		}
		else if (SortOfFollow.controlledByTriggerAction)
		{
			this.SetRoundedPosition(this.ClampPosition(new Vector3(this.followPosLerp.x + Mathf.Sin(SortOfFollow.sinXCounter) * SortOfFollow.shakeM * 6f, this.followPosLerp.y + Mathf.Sin(SortOfFollow.sinYCounter) * SortOfFollow.shakeM * 6f, base.transform.position.z)));
		}
		else
		{
			switch (this.followMode)
			{
			case CameraFollowMode.Normal:
			case CameraFollowMode.MapWidth:
				this.SetRoundedPosition(this.ClampPosition(new Vector3(this.followPosLerp.x + Mathf.Sin(SortOfFollow.sinXCounter) * SortOfFollow.shakeM * 6f, this.followPosLerp.y + Mathf.Sin(SortOfFollow.sinYCounter) * SortOfFollow.shakeM * 6f, base.transform.position.z)));
				break;
			case CameraFollowMode.Vertical:
				this.SetRoundedPosition(this.ClampPosition(new Vector3(this.centerX + Mathf.Sin(SortOfFollow.sinXCounter) * SortOfFollow.shakeM * 6f, this.followPosLerp.y + Mathf.Sin(SortOfFollow.sinYCounter) * SortOfFollow.shakeM * 6f, base.transform.position.z)));
				break;
			case CameraFollowMode.Horizontal:
				this.SetRoundedPosition(this.ClampPosition(new Vector3(this.followPosLerp.x + Mathf.Sin(SortOfFollow.sinXCounter) * SortOfFollow.shakeM * 6f, this.centerY + Mathf.Sin(SortOfFollow.sinYCounter) * SortOfFollow.shakeM * 6f, base.transform.position.z)));
				break;
			case CameraFollowMode.ForcedVertical:
				if (this.moveDelay > 0f)
				{
					if (HeroController.isCountdownFinished)
					{
						this.moveDelay -= this.t;
					}
				}
				else
				{
					this.moveOffset += this.t * (this.moveSpeed * (1f + this.moveSpeedBoostM) + this.moveExtraSpeed) * this.moveSpeedSuccessM;
				}
				this.SetRoundedPosition(this.ClampPosition(new Vector3(this.centerX + Mathf.Sin(SortOfFollow.sinXCounter) * SortOfFollow.shakeM * 6f, this.centerY + this.moveOffset + Mathf.Sin(SortOfFollow.sinYCounter) * SortOfFollow.shakeM * 6f, base.transform.position.z)));
				break;
			case CameraFollowMode.ForcedHorizontal:
				if (this.moveDelay > 0f)
				{
					if (HeroController.isCountdownFinished)
					{
						this.moveDelay -= this.t;
					}
				}
				else
				{
					this.moveOffset += this.t * (this.moveSpeed * (1f + this.moveSpeedBoostM) + this.moveExtraSpeed) * this.moveSpeedSuccessM;
				}
				this.SetRoundedPosition(this.ClampPosition(new Vector3(this.centerX + this.moveOffset + Mathf.Sin(SortOfFollow.sinXCounter) * SortOfFollow.shakeM * 6f, this.centerY + Mathf.Sin(SortOfFollow.sinYCounter) * SortOfFollow.shakeM * 6f, base.transform.position.z)));
				break;
			case CameraFollowMode.SingleScreen:
				this.SetRoundedPosition(this.ClampPosition(new Vector3(this.centerX + Mathf.Sin(SortOfFollow.sinXCounter) * SortOfFollow.shakeM * 6f, this.currentOrthographicSize + Mathf.Sin(SortOfFollow.sinYCounter) * SortOfFollow.shakeM * 6f, base.transform.position.z)));
				break;
			case CameraFollowMode.PanUpward:
			{
				float num = base.transform.position.y + this.t * this.moveSpeed * 2f;
				this.SetRoundedPosition(this.ClampPosition(new Vector3(base.transform.position.x + Mathf.Sin(SortOfFollow.sinXCounter) * SortOfFollow.shakeM * 6f, num + Mathf.Sin(SortOfFollow.sinYCounter) * SortOfFollow.shakeM * 6f, base.transform.position.z)));
				break;
			}
			case CameraFollowMode.DescendInStages:
				this.SetRoundedPosition(this.ClampPosition(new Vector3(this.centerX + Mathf.Sin(SortOfFollow.sinXCounter) * SortOfFollow.shakeM * 6f, Mathf.Max(this.currentDescentOffset, this.currentOrthographicSize) + Mathf.Sin(SortOfFollow.sinYCounter) * SortOfFollow.shakeM * 6f, base.transform.position.z)));
				break;
			case CameraFollowMode.ForcedDescent:
				if (this.moveDelay > 0f)
				{
					if (HeroController.isCountdownFinished)
					{
						this.moveDelay -= this.t;
					}
				}
				else
				{
					this.moveOffset += this.t * (this.moveSpeed * (1f + this.moveSpeedBoostM) + this.moveExtraSpeed) * this.moveSpeedSuccessM;
				}
				this.SetRoundedPosition(this.ClampPosition(new Vector3(this.centerX + Mathf.Sin(SortOfFollow.sinXCounter) * SortOfFollow.shakeM * 6f, this.centerY - this.moveOffset + Mathf.Sin(SortOfFollow.sinYCounter) * SortOfFollow.shakeM * 6f, base.transform.position.z)));
				break;
			}
		}
		if ((this.snapBackTime -= this.t) > 0f)
		{
			base.transform.position = Vector3.Lerp(this.snapBackFromPos, base.transform.position, (this.snapBackDuration - this.snapBackTime) / this.snapBackDuration);
			base.transform.position = this.ClampPosition(new Vector3(base.transform.position.x + Mathf.Sin(SortOfFollow.sinXCounter) * SortOfFollow.shakeM * 6f, base.transform.position.y + Mathf.Sin(SortOfFollow.sinYCounter) * SortOfFollow.shakeM * 6f, base.transform.position.z));
		}
	}

	public Vector3 GetStartPosition()
	{
		return new Vector3(this.centerX, this.currentOrthographicSize);
	}

	protected void SetRoundedPosition(Vector3 pos)
	{
		if (pos.x > 0f && pos.y > 0f)
		{
			base.transform.position = new Vector3(pos.x, pos.y, pos.z);
		}
		else
		{
			UnityEngine.Debug.LogWarning("Should not set Camera to negative position " + pos);
		}
	}

	public Vector3 ClampPosition(Vector3 position)
	{
		if (SortOfFollow.ControlledByTriggerAction)
		{
			float num = this.currentOrthographicSize * base.GetComponent<Camera>().aspect;
			float max = Map.GetBlocksX(Map.Width) - num;
			position = new Vector3(Mathf.Clamp(position.x, num, max), Mathf.Clamp(position.y, this.currentOrthographicSize, 10000f), position.z);
		}
		else
		{
			CameraFollowMode cameraFollowMode = this.followMode;
			switch (cameraFollowMode)
			{
			case CameraFollowMode.ForcedVertical:
			case CameraFollowMode.SingleScreen:
			case CameraFollowMode.ForcedDescent:
				position = new Vector3(position.x, Mathf.Clamp(position.y, this.currentOrthographicSize, 10000f), position.z);
				break;
			default:
				if (cameraFollowMode == CameraFollowMode.Normal)
				{
					position = new Vector3(Mathf.Clamp(position.x, this.currentOrthographicSize * base.GetComponent<Camera>().aspect, (!GameModeController.IsHelicopterVictory()) ? (Map.GetBlocksX(Map.Width) - this.currentOrthographicSize * base.GetComponent<Camera>().aspect) : 8000f), Mathf.Clamp(position.y, this.currentOrthographicSize, 10000f), position.z);
				}
				break;
			case CameraFollowMode.MapWidth:
				position = new Vector3(Mathf.Clamp(position.x, this.currentOrthographicSize * base.GetComponent<Camera>().aspect, (float)((Map.MapData.Width - 1) * 16) - this.currentOrthographicSize * base.GetComponent<Camera>().aspect), Mathf.Clamp(position.y, this.currentOrthographicSize, 10000f), position.z);
				break;
			}
		}
		return position;
	}

	public static void SpeedUp()
	{
		SortOfFollow.instance.moveExtraSpeed = SortOfFollow.instance.moveSpeed * 0.35f;
		SortOfFollow.instance.moveSpeed *= 1.2f;
		SortOfFollow.instance.moveSpeedBoostM += 1.2f;
	}

	public static void SetZoomLevel(float zoom)
	{
		SortOfFollow.zoomLevel = zoom;
	}

	protected void PlayBassDropSoundSound()
	{
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.bassDrop, 0.6f, base.transform.position);
	}

	protected void SlowTimeDownInternal(float time)
	{
		this.PlayBassDropSoundSound();
		HeroController.HighFiveBoost(time);
		HeroController.BoostHeroes(time);
	}

	protected void PlaySpeedDownSound()
	{
		if (HeroController.mustShowHUDS)
		{
			Sound.GetInstance().PlaySoundEffectSpecial(this.soundHolder.greeting, 0.9f);
		}
	}

	protected void PlaySpeedUpSound()
	{
		if (HeroController.mustShowHUDS)
		{
			Sound.GetInstance().PlaySoundEffectSpecial(this.soundHolder.panic, 0.75f);
		}
	}

	public static Vector3 GetRaceOffset()
	{
		switch (SortOfFollow.instance.followMode)
		{
		case CameraFollowMode.Normal:
		case CameraFollowMode.Horizontal:
		case CameraFollowMode.ForcedHorizontal:
			return new Vector3(SortOfFollow.GetWorldScreenWidth() * 0.1f, 0f, 0f);
		case CameraFollowMode.Vertical:
		case CameraFollowMode.ForcedVertical:
			return new Vector3(SortOfFollow.GetWorldScreenHeight() * 0.1f, 0f, 0f);
		case CameraFollowMode.DescendInStages:
		case CameraFollowMode.ForcedDescent:
			return new Vector3(-SortOfFollow.GetWorldScreenHeight() * 0.1f, 0f, 0f);
		}
		UnityEngine.Debug.LogError("Unsupported Race Mode " + SortOfFollow.instance.followMode);
		return Vector3.zero;
	}

	public static CameraFollowMode GetFollowMode()
	{
		return SortOfFollow.instance.followMode;
	}

	public override UnityStream PackState(UnityStream stream)
	{
		stream.Serialize<Vector3>(this.followPosLerp);
		stream.Serialize<Vector3>(SortOfFollow.followPos);
		return base.PackState(stream);
	}

	public override UnityStream UnpackState(UnityStream stream)
	{
		this.followPosLerp = (Vector3)stream.DeserializeNext();
		SortOfFollow.followPos = (Vector3)stream.DeserializeNext();
		return base.UnpackState(stream);
	}

	public Transform followT;

	public Vector3 followPosLerp;

	public static Vector3 followPos;

	protected float snapBackTime;

	protected Vector3 snapBackFromPos;

	protected float snapBackFromZoom;

	protected float snapBackDuration = 1f;

	protected static float x;

	protected static float y;

	protected static Vector3 minWorldPoint;

	protected static Vector3 maxWorldPoint;

	public GameObject arrowUp;

	public GameObject arrowDown;

	public GameObject arrowLeft;

	public GameObject arrowRight;

	public SoundHolder soundHolder;

	protected List<CheckPointExplosionRun> explosionRunCheckPoints = new List<CheckPointExplosionRun>();

	public TextMesh totalSpeedText;

	public TextMesh extraSpeedText;

	public TextMesh speedSuccessMultiplierSpeedText;

	public TextMesh successWinRatioText;

	public Camera UICamera;

	private static bool controlledByTriggerAction;

	public static float zoomLevel = 1f;

	protected bool hasFollowed;

	protected static float sinXRate;

	protected static float sinYRate;

	protected static float sinXCounter;

	protected static float sinYCounter;

	protected static float shakeM;

	public float moveSpeed = 30f;

	protected float moveSpeedBoostM;

	protected float moveExtraSpeed;

	protected float moveSpeedSuccessM = 1f;

	public float moveOffset;

	[HideInInspector]
	public float moveDelay = 1f;

	protected float centerX = -1f;

	protected float centerY = 160f;

	public static float minY = -1000f;

	public static float maxY = 1000f;

	public static float minX = -1000f;

	public static float maxX = 1000f;

	protected float descentOffset = 100f;

	protected float currentDescentOffset = 100f;

	public Vector3 defaultPosition = new Vector3(256f, 196f, 0f);

	protected static float worldScreenWidth;

	protected static float worldScreenHeight;

	public CameraFollowMode followMode;

	public static SortOfFollow instance;

	protected float lastCheckPointSpeedM = 1f;

	private int lastHighestGroundHeight;

	private float descentOffsetOffset;

	private float t = 0.01f;

	private bool haveZoomed;

	protected static bool showingSpeed;
}
