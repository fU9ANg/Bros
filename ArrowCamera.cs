// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ArrowCamera : MonoBehaviour
{
	public static void ShowArrow(int playerNum, Vector3 playerPos, Vector3 centre)
	{
		if (ArrowCamera.instance != null)
		{
			if (playerNum == 0)
			{
				ArrowCamera.player1Pos = playerPos;
				ArrowCamera.showArrowPlayer1 = true;
			}
			ArrowCamera.cameraCentre = centre;
		}
		else
		{
			UnityEngine.Debug.Log("What foul sorcery is this ?");
		}
	}

	private bool FindIntersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, ref Vector2 outVector)
	{
		bool result = false;
		float num = (b2.x - b1.x) * (a1.y - b1.y) - (b2.y - b1.y) * (a1.x - b1.x);
		float num2 = (a2.x - a1.x) * (a1.y - b1.y) - (a2.y - a1.y) * (a1.x - b1.x);
		float num3 = (b2.y - b1.y) * (a2.x - a1.x) - (b2.x - b1.x) * (a2.y - a1.y);
		if (num3 == 0f)
		{
			return result;
		}
		float num4 = num / num3;
		float num5 = num2 / num3;
		if (0f <= num4 && num4 <= 1f && 0f <= num5 && num5 <= 1f)
		{
			outVector = new Vector2(a1.x + num4 * (a2.x - a1.x), a1.y + num4 * (a2.y - a1.y));
			return true;
		}
		return false;
	}

	protected void ActuallyShowArrow(Transform arrow, Vector3 playerPos)
	{
		float num = playerPos.x - ArrowCamera.cameraCentre.x;
		float num2 = playerPos.y - ArrowCamera.cameraCentre.y;
		Vector2 zero = Vector2.zero;
		Vector2 zero2 = Vector2.zero;
		arrow.gameObject.SetActive(true);
		if (num > 0f)
		{
			this.FindIntersect(new Vector2(0f, 0f), new Vector2(0f + num * 100000f, 0f + num2 * 100000f), new Vector3((float)(Screen.width / 2) - this.offsetFromScreenEdge, -10000f), new Vector3((float)(Screen.width / 2) - this.offsetFromScreenEdge, 10000f), ref zero2);
		}
		else
		{
			this.FindIntersect(new Vector2(0f, 0f), new Vector2(0f + num * 100000f, 0f + num2 * 100000f), new Vector3((float)(-(float)Screen.width / 2) + this.offsetFromScreenEdge, -10000f), new Vector3((float)(-(float)Screen.width / 2) + this.offsetFromScreenEdge, 10000f), ref zero2);
		}
		if (num2 > 0f)
		{
			this.FindIntersect(new Vector2(0f, 0f), new Vector2(0f + num * 100000f, 0f + num2 * 100000f), new Vector3(-10000f, (float)(Screen.height / 2) - this.offsetFromScreenEdge), new Vector3(10000f, (float)(Screen.height / 2) - this.offsetFromScreenEdge), ref zero);
		}
		else
		{
			this.FindIntersect(new Vector2(0f, 0f), new Vector2(0f + num * 100000f, 0f + num2 * 100000f), new Vector3(-10000f, (float)(-(float)Screen.height / 2) + this.offsetFromScreenEdge), new Vector3(10000f, (float)(-(float)Screen.height / 2) + this.offsetFromScreenEdge), ref zero);
		}
		float magnitude = zero2.magnitude;
		float magnitude2 = zero.magnitude;
		if (magnitude > 0f && (magnitude < magnitude2 || magnitude2 == 0f))
		{
			arrow.transform.position = this.arrowCamera.ScreenToWorldPoint(new Vector3((float)(Screen.width / 2) + zero2.x, (float)(Screen.height / 2) + zero2.y, 3f));
		}
		else if (zero.magnitude > 0f)
		{
			if (zero.y > 0f && Mathf.Abs(zero.x) < 400f)
			{
				arrow.transform.position = this.arrowCamera.ScreenToWorldPoint(new Vector3((float)(Screen.width / 2) + zero.x, (float)(Screen.height / 2) + zero.y, 3f));
			}
			else
			{
				arrow.transform.position = this.arrowCamera.ScreenToWorldPoint(new Vector3((float)(Screen.width / 2) + zero.x, (float)(Screen.height / 2) + zero.y, 3f));
			}
		}
		else
		{
			arrow.gameObject.SetActive(false);
		}
		arrow.localScale = Vector3.one;
		float num3 = Mathf.Atan2(num2, -num);
		arrow.GetChild(0).eulerAngles = new Vector3(0f, 0f, -num3 * 180f / 3.14159274f - 180f - 45f);
	}

	private void Awake()
	{
		ArrowCamera.instance = this;
		ArrowCamera.showArrowPlayer1 = false;
	}

	private void Update()
	{
		Vector3 centre = Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f));
		if (GameModeController.GameMode == GameMode.Campaign)
		{
			if (this.MustShowArrow())
			{
				if ((this.arrowShowDelay -= Time.deltaTime) < 0f)
				{
					RescueBro nearestRescueBro = Map.GetNearestRescueBro(centre.x, centre.y);
					if (nearestRescueBro != null)
					{
						if (nearestRescueBro.transform != this.lastArrowTarget)
						{
							if ((this.arrowSwitchDelay -= Time.deltaTime) < 0f)
							{
								this.lastArrowTarget = nearestRescueBro.transform;
							}
						}
						else
						{
							this.arrowSwitchDelay = 0.5f;
						}
					}
					if (this.lastArrowTarget != null && !SortOfFollow.IsItSortOfVisible(this.lastArrowTarget.transform.position))
					{
						ArrowCamera.ShowArrow(0, this.lastArrowTarget.position, centre);
					}
				}
			}
			else
			{
				this.arrowShowDelay = 2f;
			}
		}
		this.arrowPlayer1.gameObject.SetActive(ArrowCamera.showArrowPlayer1);
		if (ArrowCamera.showArrowPlayer1)
		{
			this.ActuallyShowArrow(this.arrowPlayer1, ArrowCamera.player1Pos);
			ArrowCamera.showArrowPlayer1 = false;
		}
	}

	private bool MustShowArrow()
	{
		if (!HeroController.mustShowHUDS)
		{
			return false;
		}
		if (GameModeController.IsLevelFinished())
		{
			return false;
		}
		if (HeroController.GetPlayersPlayingCount() > 1)
		{
			for (int i = 0; i < 4; i++)
			{
				if (HeroController.IsPlaying(i) && HeroController.players[i] != null && (HeroController.players[i].character == null || HeroController.players[i].character.health <= 0))
				{
					return true;
				}
			}
		}
		else if (HeroController.GetPlayersPlayingCount() == 1)
		{
			for (int j = 0; j < 4; j++)
			{
				if (HeroController.IsPlaying(j) && HeroController.players[j] != null && HeroController.players[j].Lives <= 1 && Time.timeSinceLevelLoad > 30f)
				{
					return true;
				}
			}
		}
		return false;
	}

	public Camera arrowCamera;

	public Transform arrowPlayer1;

	protected static Vector3 player1Pos;

	protected static Vector3 cameraCentre;

	protected static ArrowCamera instance;

	protected static bool showArrowPlayer1;

	private float offsetFromScreenEdge = 40f;

	private float arrowShowDelay;

	private float arrowSwitchDelay;

	private Transform lastArrowTarget;
}
