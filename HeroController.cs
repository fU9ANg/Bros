// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : NetworkObject
{
	public static HeroController Instance
	{
		get
		{
			if (HeroController.inst == null)
			{
				HeroController.inst = (UnityEngine.Object.FindObjectOfType(typeof(HeroController)) as HeroController);
			}
			return HeroController.inst;
		}
	}

	public static void SetTestInfo(HeroType alwaysChoose)
	{
		HeroController.alwaysChooseHero = alwaysChoose;
	}

	public static void StartCountdown()
	{
		Announcer.CancelCurrentDelayedGo();
		HeroController.Instance.StartCoroutine(HeroController.Instance.DoCountDown());
	}

	private void Start()
	{
		HeroController.groundLayer = 1 << LayerMask.NameToLayer("Ground");
		HeroController.fragileLayer = 1 << LayerMask.NameToLayer("DirtyHippie");
		InputReader.LoadKeys();
		HeroController.checkPointStart = Map.FindStartLocation();
		this.SpawnJoinedPlayers();
		base.StartCoroutine(this.BeginGame());
	}

	private void OnSceneSyncronised()
	{
	}

	private IEnumerator BeginGame()
	{
		if (GameModeController.SpawnBeforeCountdown)
		{
			UnityEngine.Debug.Log("ANNOUCEN! " + Announcer.HasInstance());
			Announcer.AnnounceGo(0.12f, 1f, 0.4f);
			GameModeController.SetupIntroActions();
		}
		else
		{
			HeroController.StartCountdown();
		}
		yield return null;
		yield break;
	}

	private void SpawnJoinedPlayers()
	{
		for (int i = 0; i < HeroController.players.Length; i++)
		{
			if (HeroController.playersPlaying[i] && HeroController.players[i] == null && HeroController.PIDS[i].IsMine)
			{
				if (GameModeController.GameMode == GameMode.BroDown)
				{
					if (GameModeController.isPlayerDoingBrodown[i])
					{
						Networking.RPC<int, int, PID>(PID.TargetAll, new RpcSignature<int, int, PID>(this.AddPlayer), i, HeroController.playerControllerIDs[i], HeroController.PIDS[i], true);
					}
				}
				else if (GameModeController.GameMode == GameMode.SuicideHorde)
				{
					Networking.RPC<int, int, PID>(PID.TargetAll, new RpcSignature<int, int, PID>(this.AddPlayer), i, HeroController.playerControllerIDs[i], HeroController.PIDS[i], true);
				}
				else
				{
					Networking.RPC<int, int, PID>(PID.TargetAll, new RpcSignature<int, int, PID>(this.AddPlayer), i, HeroController.playerControllerIDs[i], HeroController.PIDS[i], true);
				}
			}
		}
	}

	public static void HighFiveBoostNetworked(int playerNum)
	{
	}

	public static void AddTemporaryPlayerTarget(int playerNum, Transform target)
	{
		if (playerNum < 0 || playerNum > 4)
		{
			UnityEngine.Debug.LogError("In valid Player num");
		}
		HeroController.players[playerNum].AddPlayerTarget(target);
	}

	public static void RemoveTemporaryPlayerTarget(int playerNum)
	{
		if (playerNum < 0 || playerNum > 4)
		{
			UnityEngine.Debug.LogError("In valid Player num");
		}
		HeroController.players[playerNum].RemovePlayerTarget();
	}

	public static void SetNextHeroType(HeroType unlockHeroType)
	{
	}

	public static void TryFollow(Transform trans)
	{
		if (!HeroController.extraFollowPositions.Contains(trans))
		{
			HeroController.extraFollowPositions.Add(trans);
		}
	}

	public static void StopFollowing(Transform trans)
	{
		HeroController.extraFollowPositions.Remove(trans);
	}

	public static bool CanFollow(Vector3 otherPosition, float minX, float maxX, float minY, float maxY, float requiredBuffer = 24f)
	{
		bool flag = (otherPosition.x >= minX && otherPosition.x <= maxX) || (otherPosition.x > maxX && otherPosition.x - minX < SortOfFollow.GetWorldScreenWidth() - requiredBuffer) || (otherPosition.x < minX && maxX - otherPosition.x < SortOfFollow.GetWorldScreenWidth() - requiredBuffer);
		bool flag2 = (otherPosition.y >= minY && otherPosition.y <= maxY) || (otherPosition.y > maxY && otherPosition.y - minY < SortOfFollow.GetWorldScreenHeight() - requiredBuffer) || (otherPosition.y < minY && maxY - otherPosition.y < SortOfFollow.GetWorldScreenHeight() - requiredBuffer);
		return flag && flag2;
	}

	public static bool GetGetFollowPosition(ref Vector3 pos)
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0333334f);
		if (!GameModeController.LevelFinished)
		{
			bool flag = false;
			float num2 = 100000f;
			float num3 = 0f;
			float num4 = 100000f;
			float num5 = 0f;
			float num6 = -10000f;
			int num7 = 0;
			for (int i = 0; i < 4; i++)
			{
				if (HeroController.IsPlaying(i) && HeroController.players[i] != null && HeroController.players[i].HasFollowPosition() && (GameModeController.GameMode != GameMode.SuicideHorde || i == GameModeController.broPlayer))
				{
					Vector3 followPosition = HeroController.players[i].GetFollowPosition();
					HeroController.AddToExtents(followPosition, ref num2, ref num3, ref num4, ref num5);
					if (followPosition.y > num6)
					{
						num6 = followPosition.y;
					}
					flag = true;
					num7++;
				}
			}
			bool flag2 = false;
			foreach (Transform transform in HeroController.extraFollowPositions)
			{
				if (transform != null && transform.gameObject.activeSelf)
				{
					Vector3 position = transform.position;
					if (HeroController.CanFollow(position, num2, num3, num4, num5, 64f))
					{
						HeroController.AddToExtents(position, ref num2, ref num3, ref num4, ref num5);
						flag = true;
						num7++;
						if (!HeroController.wasFollowingExtra)
						{
							SortOfFollow.ForceSlowSnapBack(1f);
						}
						flag2 = true;
					}
				}
			}
			if (flag2 && !HeroController.wasFollowingExtra)
			{
				SortOfFollow.ForceSlowSnapBack(1f);
			}
			else if (!flag2 && HeroController.wasFollowingExtra)
			{
				SortOfFollow.ForceSlowSnapBack(1f);
			}
			HeroController.wasFollowingExtra = flag2;
			if (flag)
			{
				pos = new Vector3((num3 + num2) / 2f, (num5 + num4) / 2f, 20f);
				if (pos.y < num6 - 100f)
				{
					pos.y = num6 - 100f;
				}
				HeroController.lastCameraFollowPos = pos;
			}
			if (GameModeController.GameMode == GameMode.Race)
			{
				pos += SortOfFollow.GetRaceOffset();
				HeroController.lastCameraFollowPos = pos;
			}
			return flag;
		}
		if (HeroController.GetPlayersOnHelicopterAmount() > 0)
		{
			Vector3 vector = HeroController.lastCameraFollowPos;
			for (int j = 0; j < 4; j++)
			{
				if (HeroController.players[j] != null && HeroController.PlayerIsOnHelicopter(j) && HeroController.players[j].IsAlive())
				{
					vector = HeroController.players[j].character.transform.position;
				}
			}
			vector += Vector3.right * 12f * 16f;
			HeroController.timeSinceFinish += Time.deltaTime;
			if (HeroController.timeSinceFinish < 0.5f)
			{
				pos += Vector3.up * 24f * Time.deltaTime;
			}
			else if (HeroController.timeSinceFinish < 2f)
			{
				pos = Vector3.Lerp(HeroController.lastCameraFollowPos, vector, num * 2f);
			}
			else
			{
				pos = Vector3.Lerp(HeroController.lastCameraFollowPos, vector, num * 15f);
			}
		}
		else
		{
			pos = HeroController.lastCameraFollowPos;
		}
		HeroController.lastCameraFollowPos = pos;
		return true;
	}

	public static bool CanLookForReposition()
	{
		if (SortOfFollow.ControlledByTriggerAction || GameModeController.GameMode == GameMode.ExplosionRun)
		{
			HeroController.repositionDelay = 0.6f;
			return false;
		}
		if (HeroController.repositionDelay > 0f)
		{
			return false;
		}
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i].Exists() && HeroController.players[i].HasTemporaryTarget())
			{
				HeroController.repositionDelay = 0.6f;
				return false;
			}
		}
		return true;
	}

	protected static void AddToExtents(Vector3 pos, ref float minX, ref float maxX, ref float minY, ref float maxY)
	{
		if (pos.x < minX)
		{
			minX = pos.x;
		}
		if (pos.x > maxX)
		{
			maxX = pos.x;
		}
		if (pos.y < minY)
		{
			minY = pos.y;
		}
		if (pos.y > maxY)
		{
			maxY = pos.y;
		}
	}

	public static int GetPlayersPlayingCount()
	{
		int num = 0;
		for (int i = 0; i < HeroController.playersPlaying.Length; i++)
		{
			if (HeroController.playersPlaying[i])
			{
				num++;
			}
		}
		return num;
	}

	public static int GetTotalLives()
	{
		int num = 0;
		for (int i = 0; i < HeroController.playersPlaying.Length; i++)
		{
			if (HeroController.playersPlaying[i] && HeroController.players[i] != null)
			{
				num += HeroController.players[i].Lives;
			}
		}
		return num;
	}

	public static int GetPlayersAliveCount()
	{
		int num = 0;
		for (int i = 0; i < HeroController.playersPlaying.Length; i++)
		{
			if (HeroController.playersPlaying[i] && HeroController.players[i] != null && HeroController.players[i].IsAlive())
			{
				num++;
			}
		}
		return num;
	}

	public static int GetPlayersOnHelicopterAmount()
	{
		int num = 0;
		for (int i = 0; i < HeroController.playersPlaying.Length; i++)
		{
			if (HeroController.playersPlaying[i] && HeroController.players[i] != null && HeroController.players[i].IsAlive() && HeroController.players[i].character != null && HeroController.players[i].character.isOnHelicopter)
			{
				num++;
			}
		}
		return num;
	}

	public static int GetPlayerAliveNum()
	{
		int num = 0;
		for (int i = 0; i < HeroController.playersPlaying.Length; i++)
		{
			if (HeroController.playersPlaying[i] && HeroController.players[i] != null && HeroController.players[i].IsAlive())
			{
				num++;
			}
		}
		return num;
	}

	public static void RegisterHiddenExplosives(HiddenExplosives hiddenExplosive)
	{
		if (HeroController.hiddenExplosives == null)
		{
			HeroController.hiddenExplosives = new List<HiddenExplosives>();
		}
		HeroController.hiddenExplosives.Add(hiddenExplosive);
	}

	public static void RemoveHiddenExplosives(HiddenExplosives hiddenExplosive)
	{
		HeroController.hiddenExplosives.Remove(hiddenExplosive);
	}

	public static bool AtLeastOnePlayerStillHasALife()
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i].Lives > 0)
			{
				return true;
			}
		}
		return false;
	}

	private void MayIRescueThisBro(int playerNum, RescueBro rescueBro)
	{
		MonoBehaviour.print("MayIRescueThisBro");
		if (!Connect.IsHost)
		{
			MonoBehaviour.print("Only master client should recieve this RPC MayIRescueThisBro()");
		}
		if (rescueBro == null || rescueBro.isBeingRescued)
		{
			MonoBehaviour.print("Cannot Rescue Bro " + rescueBro);
			return;
		}
		int num = -1;
		List<int> list = new List<int>();
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.playersPlaying[i] && !HeroController.players[i].IsAlive() && !HeroController.players[i].firstDeployment)
			{
				list.Add(i);
			}
		}
		if (this.playerDeathOrder.Count == 0 && list.Count != 0)
		{
			num = list[0];
			UnityEngine.Debug.LogError("death count eqauls zero but player is dead ");
		}
		while (this.playerDeathOrder.Count != 0 && list.Count != 0)
		{
			int num2 = this.playerDeathOrder[0];
			this.playerDeathOrder.RemoveAt(0);
			if (HeroController.players[num2] != null && HeroController.playersPlaying[num2] && !HeroController.players[num2].IsAlive())
			{
				num = num2;
				break;
			}
		}
		if (num == -1)
		{
			Networking.RPC<int, RescueBro, int[]>(PID.TargetAll, new RpcSignature<int, RescueBro, int[]>(this.SwapBro), playerNum, rescueBro, HeroController.Instance.playerDeathOrder.ToArray(), true);
		}
		else
		{
			this.RespawnBro(num, rescueBro, HeroController.Instance.playerDeathOrder.ToArray());
			Networking.RPC<int, RescueBro, int[]>(PID.TargetOthers, new RpcSignature<int, RescueBro, int[]>(this.RespawnBro), num, rescueBro, HeroController.Instance.playerDeathOrder.ToArray(), true);
		}
	}

	private void SwapBro(int playerNum, RescueBro rescueBro, int[] latestDeathOrder)
	{
		HeroController.Instance.playerDeathOrder = new List<int>(latestDeathOrder);
		HeroController.ResetLossCounter();
		HeroUnlockController.FreeBro();
		rescueBro.isBeingRescued = true;
		rescueBro.BeginRescueAnim();
		Player player = HeroController.players[playerNum];
		player.RescueInProgress = true;
		player.rescuingThisBro = rescueBro;
		if (HeroController.PIDS[playerNum].IsMine)
		{
			TestVanDammeAnim character = HeroController.players[playerNum].character;
			character.SetInvulnerable(float.PositiveInfinity, false);
			HeroController.players[playerNum].RescueInProgress = true;
			HeroController.players[playerNum].AddLife();
			HeroController.players[playerNum].RequestNewHero(new Vector2(rescueBro.x, rescueBro.y));
		}
	}

	private void RespawnBro(int rescuedPlayer, RescueBro rescueBro, int[] latestDeathOrder)
	{
		HeroController.Instance.playerDeathOrder = new List<int>(latestDeathOrder);
		Player player = HeroController.players[rescuedPlayer];
		player.RescueInProgress = true;
		HeroController.ResetLossCounter();
		HeroUnlockController.FreeBro();
		rescueBro.isBeingRescued = true;
		rescueBro.BeginRescueAnim();
		player.rescuingThisBro = rescueBro;
		if (HeroController.PIDS[rescuedPlayer].IsMine)
		{
			HeroController.players[rescuedPlayer].RescueInProgress = true;
			HeroController.players[rescuedPlayer].AddLife();
			HeroController.players[rescuedPlayer].RequestNewHero(new Vector2(rescueBro.x, rescueBro.y));
		}
		MonoBehaviour.print("Need to add master confirmation here");
	}

	public static bool CheckRescueBros(int playerNum, float x, float y, float range)
	{
		List<RescueBro> list = new List<RescueBro>(HeroController.Instance.rescueBros);
		bool result = false;
		foreach (RescueBro rescueBro in list)
		{
			if (rescueBro != null && rescueBro.gameObject.activeSelf && rescueBro.freed && Mathf.Abs(rescueBro.x - x) <= range && Mathf.Abs(rescueBro.y - y) <= range && rescueBro.rescueState == RescueBro.RescueState.Idle)
			{
				rescueBro.BeginRescueAnim();
				Networking.RPC<int, RescueBro>(PID.TargetServer, new RpcSignature<int, RescueBro>(HeroController.Instance.MayIRescueThisBro), playerNum, rescueBro, true);
				result = true;
			}
		}
		return result;
	}

	public void DestroyRescueBroRPC(RescueBro rescueBro)
	{
		UnityEngine.Object.Destroy(rescueBro.gameObject);
		if (HeroController.Instance.rescueBros.Contains(rescueBro))
		{
			HeroController.Instance.rescueBros.Remove(rescueBro);
		}
	}

	public static int GetPlayerNum(PID playerID)
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.PIDS[i] == playerID)
			{
				return i;
			}
		}
		return -1;
	}

	public static bool IsPlayerNearby(float x, float y, float xRange, float yRange, ref float playerX, ref float playerY, ref int seenPlayer)
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i].Exists())
			{
				Vector3 characterPosition = HeroController.players[i].GetCharacterPosition();
				float f = characterPosition.x - x;
				float f2 = characterPosition.y - y;
				if (Mathf.Abs(f) < xRange && Mathf.Abs(f2) < yRange)
				{
					playerX = characterPosition.x;
					playerY = characterPosition.y;
					seenPlayer = i;
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsPlayerNearby(float x, float y, int xDirection, float xRange, float yRange, ref float playerX, ref float playerY)
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i].Exists())
			{
				Vector3 characterPosition = HeroController.players[i].GetCharacterPosition();
				float f = characterPosition.x - x;
				float f2 = characterPosition.y - y;
				if (Mathf.Sign(f) == Mathf.Sign((float)xDirection) && Mathf.Abs(f) < xRange && Mathf.Abs(f2) < yRange)
				{
					playerX = characterPosition.x;
					playerY = characterPosition.y;
					return true;
				}
			}
		}
		return false;
	}

	public static int GetNearestPlayer(float x, float y, float xRange, float yRange)
	{
		int result = -1;
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i].Exists())
			{
				Vector3 characterPosition = HeroController.players[i].GetCharacterPosition();
				float f = characterPosition.x - x;
				float f2 = characterPosition.y - y;
				if (Mathf.Abs(f) < xRange && Mathf.Abs(f2) < yRange)
				{
					result = i;
				}
			}
		}
		return result;
	}

	public static Vector3 GetNearestPlayerPos(float x, float y)
	{
		float num = 10000f;
		Vector3 result = Vector3.zero;
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i].Exists())
			{
				Vector3 characterPosition = HeroController.players[i].GetCharacterPosition();
				float num2 = characterPosition.x - x;
				float num3 = characterPosition.y - y;
				float num4 = Mathf.Sqrt(num2 * num2 + num3 * num3);
				if (num4 < num)
				{
					num = num4;
					result = characterPosition;
				}
			}
		}
		return result;
	}

	public static bool GetNearestPlayer(float x, float y, float xRange, float yRange, ref int currentNearest)
	{
		float num = 1000f;
		bool result = false;
		if (currentNearest >= 0 && currentNearest < 5 && HeroController.players[currentNearest] != null && HeroController.players[currentNearest].Exists())
		{
			Vector3 characterPosition = HeroController.players[currentNearest].GetCharacterPosition();
			float f = characterPosition.x - x;
			float f2 = characterPosition.y - y;
			num = Mathf.Abs(f) + Mathf.Abs(f2);
		}
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i].Exists())
			{
				Vector3 characterPosition2 = HeroController.players[i].GetCharacterPosition();
				float f3 = characterPosition2.x - x;
				float f4 = characterPosition2.y - y;
				if (Mathf.Abs(f3) < xRange && Mathf.Abs(f4) < yRange)
				{
					float num2 = Mathf.Abs(f3) + Mathf.Abs(f4);
					if (num2 < num)
					{
						currentNearest = i;
						num = num2;
						result = true;
					}
				}
			}
		}
		return result;
	}

	public static bool IsAnotherPlayerNearby(int currentPlayerNum, float x, float y, float xRange, float yRange)
	{
		for (int i = 0; i < 4; i++)
		{
			if (i != currentPlayerNum && HeroController.players[i] != null && HeroController.players[i].Exists())
			{
				Vector3 characterPosition = HeroController.players[i].GetCharacterPosition();
				float f = characterPosition.x - x;
				if (Mathf.Abs(f) < xRange)
				{
					float f2 = characterPosition.y - y;
					if (Mathf.Abs(f2) < yRange)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool IsPlayerNearby(float x, float y, float xRange, float yRange)
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i].Exists() && HeroController.players[i].character != null && HeroController.players[i].character.enabled)
			{
				Vector3 characterPosition = HeroController.players[i].GetCharacterPosition();
				float f = characterPosition.x - x;
				if (Mathf.Abs(f) < xRange)
				{
					float f2 = characterPosition.y - y;
					if (Mathf.Abs(f2) < yRange)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool IsPlayerNearby(float x, float y, int xDirection, float xRange, float yRange)
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i].Exists())
			{
				Vector3 characterPosition = HeroController.players[i].GetCharacterPosition();
				float f = characterPosition.x - x;
				float f2 = characterPosition.y - y;
				if (Mathf.Sign(f) == Mathf.Sign((float)xDirection) && Mathf.Abs(f) < xRange && Mathf.Abs(f2) < yRange)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsPlayerNearby(float x, float y, int xDirection, float xRange, float yRange, ref int seenPlayerNum)
	{
		float num = 0f;
		float num2 = 0f;
		if (seenPlayerNum >= 0 && HeroController.IsPlayerNearby(seenPlayerNum, x, y, xDirection, ref xRange, yRange, ref num, ref num2))
		{
			return true;
		}
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i].Exists())
			{
				Vector3 characterPosition = HeroController.players[i].GetCharacterPosition();
				float f = characterPosition.x - x;
				float f2 = characterPosition.y - y;
				if (Mathf.Sign(f) == Mathf.Sign((float)xDirection) && Mathf.Abs(f) < xRange && Mathf.Abs(f2) < yRange)
				{
					seenPlayerNum = i;
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsPlayerNearby(int playerNum, float x, float y, int xDirection, ref float xRange, float yRange, ref float targetX, ref float targetY)
	{
		if (HeroController.players[playerNum] != null && HeroController.players[playerNum].Exists() && HeroController.players[playerNum].character != null)
		{
			Vector3 characterPosition = HeroController.players[playerNum].GetCharacterPosition();
			targetX = characterPosition.x;
			targetY = characterPosition.y;
			float num = targetX - x;
			float f = targetY - y;
			if (Mathf.Sign(num) == Mathf.Sign((float)xDirection) && Mathf.Abs(num) < xRange && Mathf.Abs(f) < yRange)
			{
				xRange = num;
				return true;
			}
		}
		return false;
	}

	public static bool IsPlayerNearby(int playerNum, float x, float y, ref float xRange, float yRange, ref float targetX, ref float targetY)
	{
		if (HeroController.players[playerNum] != null && HeroController.players[playerNum].Exists() && HeroController.players[playerNum].character != null)
		{
			Vector3 characterPosition = HeroController.players[playerNum].GetCharacterPosition();
			targetX = characterPosition.x;
			targetY = characterPosition.y;
			float num = targetX - x;
			float f = targetY - y;
			if (Mathf.Abs(num) < xRange && Mathf.Abs(f) < yRange)
			{
				xRange = num;
				return true;
			}
		}
		return false;
	}

	public static void GetPlayerPos(int playerNum, ref float xPos, ref float yPos)
	{
		if (playerNum >= 0 && playerNum < 4 && HeroController.PlayerIsAlive(playerNum))
		{
			xPos = HeroController.players[playerNum].character.x;
			yPos = HeroController.players[playerNum].character.y;
		}
	}

	public static Vector3 GetPlayerPos(int playerNum)
	{
		if (playerNum >= 0 && playerNum < 4 && HeroController.PlayerIsAlive(playerNum))
		{
			return new Vector3(HeroController.players[playerNum].character.x, HeroController.players[playerNum].character.y, 0f);
		}
		return Vector3.one * -1000f;
	}

	public static bool GetRandomPlayerPos(ref float xPos, ref float yPos, ref int playerNum)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < HeroController.players.Length; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i].IsAlive())
			{
				list.Add(i);
			}
		}
		if (list.Count > 0)
		{
			playerNum = list[UnityEngine.Random.Range(0, list.Count)];
			xPos = HeroController.players[playerNum].character.x;
			yPos = HeroController.players[playerNum].character.y;
			return true;
		}
		return false;
	}

	public static bool CanSeePlayer(float x, float y, int xDirection, float xRange, float yRange, ref int seenPlayerNum)
	{
		float num = xRange;
		float x2 = 0f;
		float num2 = 0f;
		int num3 = -1;
		float num4 = float.PositiveInfinity;
		int i = 0;
		while (i < 4)
		{
			if (!HeroController.IsPlayerNearby(i, x - (float)(xDirection * 5), y + 10f, xDirection, ref num, yRange, ref x2, ref num2))
			{
				goto IL_AC;
			}
			Vector3 vector = new Vector3(x, y + 6f, 0f);
			Vector3 direction = new Vector3(x2, num2 + 6f, 0f) - vector;
			if (!Physics.Raycast(vector, direction, Mathf.Abs(num), HeroController.groundLayer | HeroController.fragileLayer))
			{
				if (num4 > num)
				{
					num3 = i;
					num4 = num;
					goto IL_AC;
				}
				goto IL_AC;
			}
			IL_1EC:
			i++;
			continue;
			IL_AC:
			if (HeroController.players[i] != null)
			{
				foreach (Unit unit in HeroController.players[i].minions)
				{
					if (unit.health > 0 && unit.actionState != ActionState.Dead)
					{
						if (Mathf.Abs(x - unit.x) < xRange && Mathf.Abs(y - unit.y) < yRange && Mathf.Sign(unit.x - x) == Mathf.Sign((float)xDirection))
						{
							Vector3 vector2 = new Vector3(x, y + 6f, 0f);
							Vector3 direction2 = new Vector3(unit.x, unit.y + 6f, 0f) - vector2;
							if (!Physics.Raycast(vector2, direction2, Mathf.Abs(x - unit.x), HeroController.groundLayer | HeroController.fragileLayer))
							{
								if (num4 > Mathf.Abs(x - unit.x))
								{
									num3 = 1;
									num4 = num;
								}
							}
						}
					}
				}
				goto IL_1EC;
			}
			goto IL_1EC;
		}
		if (num3 != -1)
		{
			seenPlayerNum = num3;
			return true;
		}
		return false;
	}

	public static bool CanSeePlayer(float x, float y, float xRange, float yRange, ref int seenPlayerNum)
	{
		float num = xRange;
		float x2 = 0f;
		float num2 = 0f;
		int num3 = -1;
		float num4 = float.PositiveInfinity;
		int i = 0;
		while (i < 4)
		{
			if (!HeroController.IsPlayerNearby(i, x, y + 10f, ref num, yRange, ref x2, ref num2))
			{
				goto IL_AC;
			}
			Vector3 vector = new Vector3(x, y + 6f, 0f);
			Vector3 direction = new Vector3(x2, num2 + 6f, 0f) - vector;
			if (!Physics.Raycast(vector, direction, direction.magnitude - 6f, HeroController.groundLayer | HeroController.fragileLayer))
			{
				if (num4 > num)
				{
					num3 = i;
					num4 = num;
					goto IL_AC;
				}
				goto IL_AC;
			}
			IL_1D0:
			i++;
			continue;
			IL_AC:
			if (HeroController.players[i] != null)
			{
				foreach (Unit unit in HeroController.players[i].minions)
				{
					if (unit.health > 0 && unit.actionState != ActionState.Dead)
					{
						if (Mathf.Abs(x - unit.x) < xRange && Mathf.Abs(y - unit.y) < yRange)
						{
							Vector3 vector2 = new Vector3(x, y + 6f, 0f);
							Vector3 direction2 = new Vector3(unit.x, unit.y + 6f, 0f) - vector2;
							if (!Physics.Raycast(vector2, direction2, direction2.magnitude - 6f, HeroController.groundLayer | HeroController.fragileLayer))
							{
								if (num4 > Mathf.Abs(x - unit.x))
								{
									num3 = 1;
									num4 = num;
								}
							}
						}
					}
				}
				goto IL_1D0;
			}
			goto IL_1D0;
		}
		if (num3 != -1)
		{
			seenPlayerNum = num3;
			return true;
		}
		return false;
	}

	public static bool PlayerIsInvulnerable(int playerNum)
	{
		return HeroController.players[playerNum] != null && HeroController.players[playerNum].IsAlive() && HeroController.players[playerNum].IsInvulnerable();
	}

	public static bool PlayerIsOnHelicopter(int playerNum)
	{
		return HeroController.players[playerNum] != null && HeroController.players[playerNum].IsAlive() && HeroController.players[playerNum].character.isOnHelicopter;
	}

	public static void AssignPlayerCharacter(int playerNum, TestVanDammeAnim character)
	{
		UnityEngine.Debug.Log("Try Assign Character ! ");
		if (playerNum < 0)
		{
			return;
		}
		if (HeroController.players[playerNum] != null && !HeroController.players[playerNum].IsAlive())
		{
			UnityEngine.Debug.Log("Assign Character ! ");
			HeroController.players[playerNum].AssignCharacter(character);
		}
		else
		{
			UnityEngine.Debug.LogError("There is no player " + playerNum + "... or character is already Alive ");
		}
	}

	public static bool PlayerIsBeingRescuedOrRespawned(int playerNum)
	{
		return playerNum >= 0 && (HeroController.players[playerNum] != null && HeroController.players[playerNum].IsPendingRespawnOrResuingInProgress());
	}

	public static bool PlayerIsAlive(int playerNum)
	{
		return playerNum >= 0 && (HeroController.players[playerNum] != null && HeroController.players[playerNum].IsAlive());
	}

	public static bool PlayerHasALife(int playerNum)
	{
		return playerNum >= 0 && (HeroController.players[playerNum] != null && HeroController.players[playerNum].Lives > 0);
	}

	public static int GetPlayerLives(int playerNum)
	{
		if (playerNum < 0 || playerNum > 3)
		{
			return 0;
		}
		if (HeroController.players[playerNum] != null)
		{
			return HeroController.players[playerNum].Lives;
		}
		return 0;
	}

	public static Vector3 GetPlayerPosition(int playerNum)
	{
		if (playerNum < 0)
		{
			playerNum = HeroController.GetFirstHeroAlive();
		}
		if (HeroController.players[playerNum] != null && HeroController.players[playerNum].character != null)
		{
			return HeroController.players[playerNum].character.transform.position;
		}
		return Vector3.zero;
	}

	public static bool IsPlayerThisWay(int playerNum, float x, float y, int xDirection)
	{
		return HeroController.players[playerNum] != null && HeroController.players[playerNum].IsThisWay(x, y, xDirection);
	}

	public static bool IsPlayerThisWay(float x, float y, int xDirection)
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i].IsThisWay(x, y, xDirection))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsPlayerThisWay(float x, float y, int xDirection, ref int seenPlayerNum)
	{
		float num = 100000f;
		int num2 = -1;
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i].IsThisWay(x, y, xDirection))
			{
				float sqrMagnitude = (HeroController.GetPlayerPos(i) - new Vector3(x, y, 0f)).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num2 = i;
					num = sqrMagnitude;
				}
			}
		}
		if (num2 >= 0)
		{
			seenPlayerNum = num2;
			return true;
		}
		return false;
	}

	private void Awake()
	{
		HeroController.isCountdownFinished = false;
		HeroController.hiddenExplosives = new List<HiddenExplosives>();
		HeroController.AllPlayersHaveJoined = false;
		HeroController.lastFollowTimeSwitch = Time.time - 10f;
		HeroController.lastFollowTimeSwitchBack = Time.time;
		if (HeroController.heroAlreadyChosenTypes == null)
		{
			HeroController.heroAlreadyChosenTypes = new List<HeroType>();
		}
		HeroUnlockController.Initialize();
		HeroController.timeSinceFinish = 0f;
		HeroController.wasFollowingExtra = false;
	}

	private static bool IsTypeInUse(HeroType typeTocheck)
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.GetCurrentHeroType(i) == typeTocheck)
			{
				return true;
			}
		}
		return false;
	}

	public static void RequestHeroTypeFromMaster(Vector2 spawnPos, int PlayerNum)
	{
		List<HeroType> unlockedHeroes = HeroUnlockController.GetUnlockedHeroes();
		HeroType[] arg = unlockedHeroes.ToArray();
		Networking.RPC<PID, Vector2, int, HeroType[], HeroType, Ack>(PID.TargetServer, new RpcSignature<PID, Vector2, int, HeroType[], HeroType, Ack>(HeroController.Instance.RequestHeroTypeFromMasterRPC), PID.MyID, spawnPos, PlayerNum, arg, HeroController.nextHeroType, Ack.Allocate(), true);
		HeroController.nextHeroType = HeroType.None;
	}

	private void RequestHeroTypeFromMasterRPC(PID Requestee, Vector2 spawnPos, int PlayerNum, HeroType[] unlockedTypes, HeroType preferedNextHero, Ack ack)
	{
		UnityEngine.Debug.Log("> RequestHeroTypeFromMasterRPC " + PlayerNum);
		if (!Connect.IsHost)
		{
			MonoBehaviour.print("only master client should recieve this");
		}
		List<HeroType> unlockedBros = new List<HeroType>(unlockedTypes);
		HeroType heroType = HeroController.GetHeroType(PlayerNum, unlockedBros, preferedNextHero);
		Networking.RPC<Vector2, int, int, HeroType[], Ack>(Requestee, new RpcSignature<Vector2, int, int, HeroType[], Ack>(this.RecieveHeroTypeFromMaster), spawnPos, (int)heroType, PlayerNum, HeroController.heroAlreadyChosenTypes.ToArray(), ack, true);
	}

	private void RecieveHeroTypeFromMaster(Vector2 pos, int nextHeroTypeRecieved, int PlayerNum, HeroType[] updatedHeroAlreadyChosenTypes, Ack ack)
	{
		HeroController.heroAlreadyChosenTypes = new List<HeroType>(updatedHeroAlreadyChosenTypes);
		HeroController.players[PlayerNum].SpawnHero(pos, (HeroType)nextHeroTypeRecieved);
		if (GameModeController.GameMode == GameMode.ExplosionRun || GameModeController.GameMode == GameMode.Race)
		{
			GameModeController.deathmatchHero[PlayerNum] = (HeroType)nextHeroTypeRecieved;
		}
		if (HeroController.nextHeroType == (HeroType)nextHeroTypeRecieved)
		{
			HeroController.nextHeroType = HeroType.None;
		}
	}

	public static HeroType GetCurrentHeroType(int playerNum)
	{
		if (!(HeroController.players[playerNum] != null) || !HeroController.players[playerNum].IsAlive())
		{
			return HeroType.None;
		}
		if (HeroController.players[playerNum].character != null)
		{
			return HeroController.players[playerNum].character.heroType;
		}
		return HeroController.players[playerNum].heroType;
	}

	public static HeroType GetHeroType(int playerNum, List<HeroType> unlockedBros, HeroType preferredHero)
	{
		if (GameModeController.GameMode == GameMode.BroDown || GameModeController.IsDeathMatchMode)
		{
			return GameModeController.deathmatchHero[playerNum];
		}
		if (GameModeController.GameMode == GameMode.SuicideHorde)
		{
			if (playerNum != GameModeController.broPlayer)
			{
				return HeroType.SuicideBro;
			}
		}
		else if (GameModeController.GameMode == GameMode.Campaign && Map.MapData.forcedBros != null && Map.MapData.forcedBros.Count > 0)
		{
			unlockedBros = new List<HeroType>(Map.MapData.forcedBros.ToArray());
		}
		else if (GameModeController.GameMode == GameMode.Campaign && Map.MapData.forcedBro != HeroType.Random)
		{
			return Map.MapData.forcedBro;
		}
		if (HeroController.alwaysChooseHero != HeroType.None && HeroController.alwaysChooseHero != HeroType.Random)
		{
			return HeroController.alwaysChooseHero;
		}
		if (preferredHero != HeroType.None && !HeroController.IsTypeInUse(preferredHero))
		{
			HeroController.AddHeroToAlreadyChosenHeroes(preferredHero);
			return preferredHero;
		}
		HeroType heroType = HeroController.GetCurrentHeroType(0);
		if (unlockedBros.Count > HeroController.GetPlayersPlayingCount())
		{
			unlockedBros.RemoveAll((HeroType t) => HeroController.IsTypeInUse(t));
		}
		if (unlockedBros.Count > HeroController.heroAlreadyChosenTypes.Count)
		{
			unlockedBros.RemoveAll((HeroType t) => HeroController.heroAlreadyChosenTypes.Contains(t));
		}
		else
		{
			int num = HeroController.heroAlreadyChosenTypes.Count - 1;
			while (unlockedBros.Count > 1 && num > 0)
			{
				if (unlockedBros.Contains(HeroController.heroAlreadyChosenTypes[num]))
				{
					unlockedBros.Remove(HeroController.heroAlreadyChosenTypes[num]);
				}
				num--;
			}
		}
		heroType = unlockedBros[UnityEngine.Random.Range(0, unlockedBros.Count)];
		HeroController.AddHeroToAlreadyChosenHeroes(heroType);
		return heroType;
	}

	protected static void AddHeroToAlreadyChosenHeroes(HeroType currentType)
	{
		int count = HeroUnlockController.GetUnlockedHeroes().Count;
		if (count > HeroController.GetPlayersPlayingCount() + 1)
		{
			int num = count - HeroController.GetPlayersPlayingCount();
			if (HeroController.heroAlreadyChosenTypes.Count > 0 && (float)(HeroController.heroAlreadyChosenTypes.Count + 1) >= (float)num * 0.8f)
			{
				HeroController.heroAlreadyChosenTypes.RemoveAt(0);
			}
		}
		else if (HeroController.heroAlreadyChosenTypes.Count > 0 && HeroController.heroAlreadyChosenTypes.Count + 1 >= HeroController.GetPlayersPlayingCount())
		{
			HeroController.heroAlreadyChosenTypes.RemoveAt(0);
		}
		HeroController.heroAlreadyChosenTypes.Add(currentType);
	}

	public static int[] ConvertHeroArrayToIntArray(HeroType[] heroArray)
	{
		return Array.ConvertAll<HeroType, int>(heroArray, (HeroType item) => (int)item);
	}

	public static HeroType[] ConvertIntArrayToHeroArray(int[] intArray)
	{
		return Array.ConvertAll<int, HeroType>(intArray, (int item) => (HeroType)item);
	}

	public static bool HasJustStarted()
	{
		return HeroController.Instance != null && HeroController.Instance.inputDelay > 0f;
	}

	public static TestVanDammeAnim GetHeroPrefab(HeroType heroType)
	{
		if (!(HeroController.Instance != null))
		{
			UnityEngine.Debug.LogError("No Hero Controller Instance");
			return null;
		}
		if (heroType == HeroType.None && GameModeController.IsDeathMatchMode)
		{
			heroType = (HeroType)UnityEngine.Random.Range(0, 31);
		}
		HeroType heroType2 = heroType;
		switch (heroType2)
		{
		case HeroType.Rambro:
			return HeroController.Instance.rambo;
		case HeroType.Brommando:
			return HeroController.Instance.brommander;
		case HeroType.BaBroracus:
			return HeroController.Instance.baBroracus;
		case HeroType.BrodellWalker:
			return HeroController.Instance.BrodellWalker;
		case HeroType.Blade:
			return HeroController.Instance.Blade;
		case HeroType.McBrover:
			return HeroController.Instance.McBrover;
		case HeroType.Brononymous:
			return HeroController.Instance.Brononymous;
		case HeroType.Brobocop:
			return HeroController.Instance.BroboCop;
		case HeroType.BroDredd:
			return HeroController.Instance.BroDredd;
		case HeroType.BroHard:
			return HeroController.Instance.BroHard;
		case HeroType.MadMaxBrotansky:
			return HeroController.Instance.MadMaxBrotansky;
		case HeroType.SnakeBroSkin:
			return HeroController.Instance.SnakeBroskin;
		case HeroType.Brominator:
			return HeroController.Instance.Brominator;
		case HeroType.IndianaBrones:
			return HeroController.Instance.IndianaBrones;
		case HeroType.AshBrolliams:
			return HeroController.Instance.AshBrolliams;
		case HeroType.Nebro:
			return HeroController.Instance.Nebro;
		case HeroType.BoondockBros:
			return HeroController.Instance.BoondockBros;
		case HeroType.Brochete:
			return HeroController.Instance.Brochete;
		case HeroType.BronanTheBrobarian:
			return HeroController.Instance.BronanTheBrobarian;
		case HeroType.EllenRipbro:
			return HeroController.Instance.EllenRipbro;
		case HeroType.CherryBroling:
			return HeroController.Instance.CherryBroling;
		case HeroType.TimeBroVanDamme:
			return HeroController.Instance.TimeBroVanDamme;
		case HeroType.ColJamesBroddock:
			return HeroController.Instance.ColJamesBroddock;
		case HeroType.BroniversalSoldier:
			return HeroController.Instance.BroniversalSoldier;
		case HeroType.BroneyRoss:
			return HeroController.Instance.BroneyRoss;
		case HeroType.LeeBroxmas:
			return HeroController.Instance.LeeBroxmas;
		case HeroType.BronnarJensen:
			return HeroController.Instance.BronnarJensen;
		case HeroType.HaleTheBro:
			return HeroController.Instance.HaleTheBro;
		case HeroType.TrentBroser:
			return HeroController.Instance.TrentBroser;
		case HeroType.Broc:
			return HeroController.Instance.Broc;
		case HeroType.TollBroad:
			return HeroController.Instance.TollBroad;
		default:
			if (heroType2 != HeroType.SuicideBro)
			{
				UnityEngine.Debug.LogError("heroType is Random " + heroType);
				return HeroController.Instance.rambo;
			}
			return HeroController.Instance.SuicideBro;
		}
	}

	public static bool IsPlaying(int playerNum)
	{
		return HeroController.playersPlaying[playerNum] || HeroController.players[playerNum] != null;
	}

	public static void ResetPlayersPlaying()
	{
		for (int i = 0; i < HeroController.players.Length; i++)
		{
			HeroController.playersPlaying[i] = false;
			HeroController.playerControllerIDs[i] = -1;
			HeroController.PIDS[i] = PID.NoID;
		}
	}

	public static void ResetAmmo()
	{
		for (int i = 0; i < HeroController.ammoCounts.Length; i++)
		{
			HeroController.ammoCounts[i] = -1;
		}
	}

	public static void SetSpecialAmmo(int playerNum, int specialAmmo)
	{
		if (playerNum >= 0 && playerNum < HeroController.ammoCounts.Length)
		{
			if (specialAmmo < 0)
			{
				specialAmmo = 0;
			}
			HeroController.ammoCounts[playerNum] = specialAmmo;
		}
	}

	public static int GetSpecialAmmo(int playerNum, int currentSpecialAmmo)
	{
		if (playerNum < 0 || playerNum >= HeroController.ammoCounts.Length)
		{
			return currentSpecialAmmo;
		}
		if (HeroController.ammoCounts[playerNum] < 0)
		{
			return currentSpecialAmmo;
		}
		return HeroController.ammoCounts[playerNum];
	}

	public static void SetOriginalSpecialAmmoCount(int playerNum, int count)
	{
		if (!GameModeController.IsDeathMatchMode && playerNum >= 0 && playerNum < HeroController.players.Length && HeroController.players[playerNum] != null)
		{
			HeroController.players[playerNum].hud.SetGrenadesOriginalCount(count);
		}
	}

	public static void FlashSpecialAmmo(int playerNum)
	{
		if (GameModeController.ShowStandardHUDS() && playerNum >= 0 && playerNum < HeroController.players.Length && HeroController.players[playerNum] != null)
		{
			HeroController.players[playerNum].FlashSpecialAmmo();
		}
	}

	public static void SetAvatarCalm(int playerNum, bool primaryAvatar)
	{
		if (playerNum >= 0 && playerNum < HeroController.players.Length && HeroController.players[playerNum] != null)
		{
			HeroController.players[playerNum].hud.SetAvatarCalm(primaryAvatar);
		}
	}

	public static void SetAvatarAngry(int playerNum, bool primaryAvatar)
	{
		if (playerNum >= 0 && playerNum < HeroController.players.Length && HeroController.players[playerNum] != null)
		{
			HeroController.players[playerNum].hud.SetAvatarAngry(primaryAvatar);
		}
	}

	public static void FlashAvatar(int playerNum, float time, bool primaryAvatar)
	{
		if (playerNum >= 0 && playerNum < HeroController.players.Length && HeroController.players[playerNum] != null)
		{
			HeroController.players[playerNum].hud.FlashAvatar(time, primaryAvatar);
		}
	}

	public static void SetAvatarFire(int playerNum, bool primaryAvatar)
	{
		if (playerNum >= 0 && playerNum < HeroController.players.Length && HeroController.players[playerNum] != null)
		{
			HeroController.players[playerNum].hud.SetAvatarFire(primaryAvatar);
		}
	}

	public static void SetAvatarDead(int playerNum, bool primaryAvatar)
	{
		if (playerNum >= 0 && playerNum < HeroController.players.Length && HeroController.players[playerNum] != null)
		{
			HeroController.players[playerNum].hud.SetAvatarDead(primaryAvatar);
		}
	}

	public static void SetAvatarBounceDown(int playerNum, bool primaryAvatar)
	{
		if (playerNum >= 0 && playerNum < HeroController.players.Length && HeroController.players[playerNum] != null)
		{
			HeroController.players[playerNum].hud.SetAvatarBounceDown(primaryAvatar);
		}
	}

	public static void SetAvatarBounceUp(int playerNum, bool primaryAvatar)
	{
		if (playerNum >= 0 && playerNum < HeroController.players.Length && HeroController.players[playerNum] != null)
		{
			HeroController.players[playerNum].hud.SetAvatarBounceUp(primaryAvatar);
		}
	}

	public static void SetAvatarMaterial(int playerNum, Material material)
	{
		if (playerNum >= 0 && playerNum < HeroController.players.Length && HeroController.players[playerNum] != null)
		{
			HeroController.players[playerNum].hud.SetAvatar(material);
		}
	}

	public static void SwitchAvatarMaterial(SpriteSM sprite, HeroType heroType)
	{
		switch (heroType)
		{
		case HeroType.Rambro:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialRambro;
			break;
		case HeroType.Brommando:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialBromando;
			break;
		case HeroType.BaBroracus:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialBabroracus;
			break;
		case HeroType.BrodellWalker:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialBrodellWalker;
			break;
		case HeroType.Blade:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialBlade;
			break;
		case HeroType.McBrover:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialMcBrover;
			break;
		case HeroType.Brononymous:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialBrononymous;
			break;
		case HeroType.Brobocop:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialBroboCop;
			break;
		case HeroType.BroDredd:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialBroDredd;
			break;
		case HeroType.BroHard:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialBroHard;
			break;
		case HeroType.MadMaxBrotansky:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialMadMaxBrotansky;
			break;
		case HeroType.SnakeBroSkin:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialSnakeBroskin;
			break;
		case HeroType.Brominator:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialBrominator;
			break;
		case HeroType.IndianaBrones:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialIndianaBrones;
			break;
		case HeroType.AshBrolliams:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialaAshBrolliams;
			break;
		case HeroType.Nebro:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialNebro;
			break;
		case HeroType.BoondockBros:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialBoondockBros;
			break;
		case HeroType.Brochete:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialBrochete;
			break;
		case HeroType.BronanTheBrobarian:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialBronanTheBrobarian;
			break;
		case HeroType.EllenRipbro:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialEllenRipbro;
			break;
		case HeroType.CherryBroling:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialCherryBroling;
			break;
		case HeroType.TimeBroVanDamme:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialTimeBroVanneDamme;
			break;
		case HeroType.ColJamesBroddock:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialColJamesBroddick;
			break;
		case HeroType.BroniversalSoldier:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialBroniversalSoldier;
			break;
		case HeroType.BroneyRoss:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialBroneyRoss;
			break;
		case HeroType.LeeBroxmas:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialLeeBroxmas;
			break;
		case HeroType.BronnarJensen:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialBronnarJensen;
			break;
		case HeroType.HaleTheBro:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialHaleTheBro;
			break;
		case HeroType.TrentBroser:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialTrentBroser;
			break;
		case HeroType.Broc:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialBroc;
			break;
		case HeroType.TollBroad:
			sprite.GetComponent<Renderer>().material = HeroController.Instance.materialTollBroad;
			break;
		}
	}

	public static Material GetAvatarMaterial(HeroType heroType)
	{
		switch (heroType)
		{
		case HeroType.Rambro:
			return (Material)Resources.Load("RambroAvatar", typeof(Material));
		case HeroType.Brommando:
			return (Material)Resources.Load("BrommandoAvatar", typeof(Material));
		case HeroType.BaBroracus:
			return (Material)Resources.Load("BroracusAvatar", typeof(Material));
		case HeroType.BrodellWalker:
			return (Material)Resources.Load("BrodellAvatar", typeof(Material));
		case HeroType.Blade:
			return (Material)Resources.Load("BladeAvatar", typeof(Material));
		case HeroType.McBrover:
			return (Material)Resources.Load("McBroverAvatar", typeof(Material));
		case HeroType.Brononymous:
			return (Material)Resources.Load("BroInBlackAvatar", typeof(Material));
		case HeroType.Brobocop:
			return (Material)Resources.Load("RobrocopAvatar", typeof(Material));
		case HeroType.BroDredd:
			return (Material)Resources.Load("BroDreddAvatar", typeof(Material));
		case HeroType.BroHard:
			return (Material)Resources.Load("BrohardAvatar", typeof(Material));
		case HeroType.MadMaxBrotansky:
			return (Material)Resources.Load("RambroAvatar", typeof(Material));
		case HeroType.SnakeBroSkin:
			return (Material)Resources.Load("SnakeBroskinAvatar", typeof(Material));
		case HeroType.Brominator:
			return (Material)Resources.Load("BrominatorAvatar", typeof(Material));
		case HeroType.IndianaBrones:
			return (Material)Resources.Load("IndiAvatar", typeof(Material));
		case HeroType.AshBrolliams:
			return (Material)Resources.Load("AshAvatar", typeof(Material));
		case HeroType.Nebro:
			return (Material)Resources.Load("NebroAvatar", typeof(Material));
		case HeroType.BoondockBros:
			return (Material)Resources.Load("BoondockBrosAvatar", typeof(Material));
		case HeroType.Brochete:
			return (Material)Resources.Load("BrocheteAvatar", typeof(Material));
		case HeroType.BronanTheBrobarian:
			return (Material)Resources.Load("BronanTheBrobarianAvatar", typeof(Material));
		case HeroType.EllenRipbro:
			return (Material)Resources.Load("EllenRipbroAvatar", typeof(Material));
		case HeroType.CherryBroling:
			return (Material)Resources.Load("CherryBrolingAvatar", typeof(Material));
		case HeroType.TimeBroVanDamme:
			return (Material)Resources.Load("TimeBroVanDammeAvatar", typeof(Material));
		case HeroType.ColJamesBroddock:
			return (Material)Resources.Load("ColJamesBroddockAvatar", typeof(Material));
		case HeroType.BroniversalSoldier:
			return (Material)Resources.Load("BroniversalSoldierAvatar", typeof(Material));
		case HeroType.BroneyRoss:
			return (Material)Resources.Load("BroneyRossAvatar", typeof(Material));
		case HeroType.LeeBroxmas:
			return (Material)Resources.Load("LeeBroxmasAvatar", typeof(Material));
		case HeroType.BronnarJensen:
			return (Material)Resources.Load("BronnarJensenAvatar", typeof(Material));
		case HeroType.HaleTheBro:
			return (Material)Resources.Load("HaleTheBroAvatar", typeof(Material));
		case HeroType.TrentBroser:
			return (Material)Resources.Load("TrentBroserAvatar", typeof(Material));
		case HeroType.Broc:
			return (Material)Resources.Load("BrocAvatar", typeof(Material));
		case HeroType.TollBroad:
			return (Material)Resources.Load("TollBroadAvatar", typeof(Material));
		default:
			return (Material)Resources.Load("BlankAvatar", typeof(Material));
		}
	}

	protected static bool IsDead(int playerNum)
	{
		return HeroController.players[playerNum] != null && !HeroController.players[playerNum].IsAlive();
	}

	[RPC]
	public void PlayerHasDiedRPC(int playerNum)
	{
		HeroController.Instance.playerDeathOrder.Add(playerNum);
		HeroController.players[playerNum].RemoveLife();
		if (GameModeController.GameMode == GameMode.ExplosionRun && HeroController.GetPlayerAliveNum() > 0)
		{
			SortOfFollow.SpeedUp();
		}
		Map.ForgetPlayer(playerNum);
		if (Connect.IsHost)
		{
			HeroController.Instance.UpdateDeathOrder(this.playerDeathOrder.ToArray());
			Networking.RPC<int[]>(PID.TargetOthers, new RpcSignature<int[]>(this.UpdateDeathOrder), HeroController.Instance.playerDeathOrder.ToArray(), false);
		}
	}

	[RPC]
	private void UpdateDeathOrder(int[] latestDeathOrder)
	{
		this.playerDeathOrder = new List<int>(latestDeathOrder);
	}

	public static void PlayerHasDied(int playerNum)
	{
		Networking.RPC<int>(PID.TargetAll, new RpcSignature<int>(HeroController.Instance.PlayerHasDiedRPC), playerNum, false);
	}

	[RPC]
	public void AttachHeroToHelicopter(Vector3 localPosition, float direction, TestVanDammeAnim hero, Helicopter heli)
	{
		if (heli == null)
		{
			MonoBehaviour.print("heli is null");
			return;
		}
		if (hero == null)
		{
			MonoBehaviour.print("heli is null");
			return;
		}
		heli.Leave();
		hero.transform.parent = heli.transform;
		hero.transform.localPosition = localPosition;
		hero.transform.localScale = new Vector3(direction, 1f, 1f);
		hero.isOnHelicopter = true;
		hero.SetInvulnerable(float.PositiveInfinity, false);
		hero.enabled = false;
		heli.attachedHeroes.Add(hero);
		Map.StartLevelEndExplosionsOverNetwork();
	}

	public static void DetachHeroFromHelicopter(TestVanDammeAnim hero)
	{
		hero.transform.parent = null;
		hero.isOnHelicopter = false;
		if (GameModeController.IsDeathMatchMode)
		{
			hero.invulnerable = false;
		}
		else
		{
			hero.SetInvulnerable(0.5f, false);
		}
		MonoBehaviour.print("DetachHeroFromHelicopter");
		hero.enabled = true;
	}

	public static void HighFiveBoost(float time)
	{
		HeroController.timeBoostTime = time;
		Time.timeScale = 0.5f;
		Sound.SetPitch(0.7f);
	}

	public static void TimeBroBoost(float time)
	{
		HeroController.timeBoostTime = time;
		Time.timeScale = 0.25f;
		Sound.SetPitch(0.5f);
	}

	public static void CancelTimeBroBoost()
	{
		HeroController.timeBoostTime = 0.01f;
	}

	public static void BoostHeroes(float time)
	{
		for (int i = 0; i < HeroController.players.Length; i++)
		{
			if (HeroController.IsPlaying(i) && HeroController.PlayerIsAlive(i))
			{
				HeroController.players[i].BoostHero(time);
			}
		}
	}

	public static void TimeBroBoostHeroes(float time)
	{
		for (int i = 0; i < HeroController.players.Length; i++)
		{
			if (HeroController.IsPlaying(i) && HeroController.PlayerIsAlive(i))
			{
				HeroController.players[i].TimeBroBoostHero(time);
			}
		}
	}

	public static bool HaveAllPlayersJoined()
	{
		HeroController.AllPlayersHaveJoined = true;
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.playersPlaying[i] && ((GameModeController.GameMode != GameMode.BroDown && GameModeController.GameMode != GameMode.SuicideHorde) || (GameModeController.GameMode == GameMode.BroDown && GameModeController.isPlayerDoingBrodown[i]) || (GameModeController.GameMode == GameMode.SuicideHorde && i == GameModeController.broPlayer)) && HeroController.players[i] == null)
			{
				return false;
			}
		}
		return true;
	}

	public static void KickPlayersIfHeHasNotJoined(PID playerPid)
	{
		HeroController.Instance.StartCoroutine(HeroController.KickPlayersIfHeHasNotJoinedRoutine(playerPid));
	}

	public static IEnumerator KickPlayersIfHeHasNotJoinedRoutine(PID playerPid)
	{
		yield return new WaitForSeconds(2f);
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.PIDS[i] == playerPid)
			{
				MonoBehaviour.print(HeroController.PIDS[i]);
				yield break;
			}
		}
		Connect.SendKick(playerPid);
		yield break;
	}

	public static void Dropout(int playerNum, bool sendRPC)
	{
		if (sendRPC)
		{
			Networking.RPC<int>(PID.TargetAll, true, true, new RpcSignature<int>(HeroController.DropoutRPC), playerNum);
		}
		else
		{
			HeroController.DropoutRPC(playerNum);
		}
	}

	private static void DropoutRPC(int playerNum)
	{
		if (playerNum < 0 || playerNum >= 4)
		{
			return;
		}
		if (HeroController.players[playerNum] != null)
		{
			UnityEngine.Object.Destroy(HeroController.players[playerNum].gameObject);
		}
		HeroController.playersPlaying[playerNum] = false;
		HeroController.playerControllerIDs[playerNum] = -1;
		HeroController.PIDS[playerNum] = PID.NoID;
		HeroController.players[playerNum] = null;
		if (HeroController.Instance != null)
		{
			for (int i = HeroController.Instance.playerDeathOrder.Count - 1; i >= 0; i--)
			{
				if (HeroController.Instance.playerDeathOrder[i] == playerNum)
				{
					HeroController.Instance.playerDeathOrder.RemoveAt(i);
				}
			}
		}
	}

	public static void DeregisterPlayer(PID pid)
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.PIDS[i] == pid)
			{
				HeroController.DropoutRPC(i);
			}
		}
	}

	protected void Update()
	{
		if (!Map.Instance.HasBeenSetup)
		{
			return;
		}
		if (!HeroController.AllPlayersHaveJoined)
		{
			HeroController.AllPlayersHaveJoined = HeroController.HaveAllPlayersJoined();
		}
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		this.inputDelay -= num;
		if (HeroController.timeBoostTime > 0f)
		{
			HeroController.timeBoostTime -= num;
			if (HeroController.timeBoostTime <= 0.2f && HeroController.timeBoostTime > 0f)
			{
				Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, num * 3f);
			}
			else if (HeroController.timeBoostTime <= 0f)
			{
				Time.timeScale = 1f;
				Sound.SetPitch(1f);
			}
		}
		if (HeroController.repositionDelay > 0f)
		{
			HeroController.repositionDelay -= num;
		}
		if (Input.GetKeyUp(KeyCode.F12) && Info.IsDevBuild)
		{
			HeroController.mustShowHUDS = !HeroController.mustShowHUDS;
			if (HeroController.mustShowHUDS)
			{
				foreach (Player player in HeroController.players)
				{
					if (player != null && player.hud != null)
					{
						player.hud.Show();
					}
				}
			}
		}
		if (!GameModeController.LevelFinished || GameModeController.InRewardPhase())
		{
			for (int j = 0; j < HeroController.players.Length; j++)
			{
				if (HeroController.players[j] == null && this.inputDelay <= 0f && GameModeController.AllowPlayerDropIn)
				{
					this.MonitorPlayerDropin(j);
				}
				this.RunHeroRespawnLogic(j);
			}
		}
		this.allDead = true;
		for (int k = 0; k < 4; k++)
		{
			if (HeroController.IsPlaying(k) && (!HeroController.IsDead(k) || HeroController.players[k].Lives > 0))
			{
				this.allDead = false;
				break;
			}
		}
		if (Application.isEditor && Input.GetKey(KeyCode.LeftControl))
		{
			Time.timeScale = 5f;
		}
		else if (Application.isEditor && Time.timeScale > 1.5f && Input.GetKeyUp(KeyCode.LeftControl))
		{
			Time.timeScale = 1f;
		}
		if (Info.IsDevBuild)
		{
			if (Input.GetKeyUp(KeyCode.Backspace))
			{
				PlayerPrefs.DeleteAll();
				UnityEngine.Debug.Log("DeleteAll");
			}
			if (Input.GetKeyDown(KeyCode.F6))
			{
				GameModeController.RestartLevel();
			}
			if (Input.GetKeyDown(KeyCode.F1))
			{
				if (Time.timeScale > 0.9f)
				{
					Time.timeScale = 0.1f;
				}
				else if (Application.isEditor && Input.GetKey(KeyCode.LeftControl))
				{
					Time.timeScale = 2f;
				}
				else
				{
					Time.timeScale = 1f;
				}
			}
		}
		if (Connect.IsHost)
		{
			if (!GameModeController.LevelFinished && (HeroController.players[0] != null || HeroController.players[1] != null || HeroController.players[2] != null || HeroController.players[3] != null) && !HeroController.AtLeastOnePlayerStillHasALife())
			{
				bool flag = false;
				for (int l = 0; l < HeroController.players.Length; l++)
				{
					if (HeroController.players[l] != null && HeroController.players[l].character != null && HeroController.players[l].character.MustBroFailToWin())
					{
						flag = true;
					}
				}
				if (flag && !GameModeController.LevelFinished)
				{
					UnityEngine.Debug.Log("***************  WIN ANYWAY ********************** " + LevelSelectionController.CurrentLevelNum);
					LevelSelectionController.CurrentLevelNum++;
					Map.ClearSuperCheckpointStatus();
				}
				GameModeController.LevelFinish(LevelResult.Fail);
			}
			if (GameModeController.LevelFinished && this.finishedCounter > 2f && !this.faded)
			{
				this.faded = true;
			}
		}
	}

	private void RunHeroRespawnLogic(int playerNum)
	{
		if (!HeroController.IsPlaying(playerNum) || HeroController.players[playerNum] == null || !HeroController.players[playerNum].IsMine || HeroController.players[playerNum].pendingRespawn)
		{
			return;
		}
		if (HeroController.players[playerNum].firstDeployment)
		{
			this.RunFirstDeployment(playerNum);
			return;
		}
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.033334f);
		if (!this.AreAllHerosDead())
		{
			this.broRespawnDelay = 0.4f;
		}
		else
		{
			this.broRespawnDelay -= num;
		}
		GameMode gameMode = GameModeController.GameMode;
		switch (gameMode)
		{
		case GameMode.Campaign:
			if (HeroController.players[playerNum].Lives > 0)
			{
				if (this.broRespawnDelay < 0f && this.AreAllHerosDead() && HeroController.AtLeastOnePlayerStillHasALife() && HeroController.players[playerNum].IsMine)
				{
					Vector3 v = HeroController.GetCheckPointPosition(playerNum);
					if (Map.isEditing && LevelEditorGUI.levelEditorActive && LevelEditorGUI.lastCameraPos.x > 0f)
					{
						v = LevelEditorGUI.lastCameraPos;
						LevelEditorGUI.lastCameraPos = -Vector3.one;
					}
					HeroController.players[playerNum].RequestNewHero(v);
				}
				else if (!HeroController.players[playerNum].IsAlive())
				{
					Vector2 pos = HeroController.GetCheckPointPosition(playerNum);
					if (this.IsPlayerNearbyCheckPoint(ref pos))
					{
						HeroController.players[playerNum].RequestNewHero(pos);
					}
				}
			}
			return;
		default:
			if (gameMode != GameMode.TeamDeathMatch)
			{
				return;
			}
			break;
		case GameMode.DeathMatch:
			break;
		case GameMode.SuicideHorde:
			if (HeroController.isCountdownFinished && playerNum != GameModeController.broPlayer && HeroController.players[playerNum].character == null)
			{
				if (this.nextSpawnDoor[playerNum] == null)
				{
					this.FindAndFlashNextSpawnDoor(playerNum);
				}
				else if ((this.suicideBroSpawnDelay[playerNum] -= num) < 0f)
				{
					this.SpawnSuicideBro(playerNum);
					this.suicideBroSpawnDelay[playerNum] = 0.6f;
				}
			}
			return;
		}
		if (GameModeController.InRewardPhase() && !HeroController.players[playerNum].IsAlive() && playerNum != GameModeController.GetWinnerNum())
		{
			Vector3 vector = new Vector3((float)(UnityEngine.Random.Range(5, 25) * 16), SortOfFollow.GetScreenMaxY(), 0f);
			HeroController.players[playerNum].RequestNewHero(new Vector2(vector.x, vector.y + 8f));
		}
		if (!HeroController.players[playerNum].IsAlive() && HeroController.players[playerNum].Lives > 0 && !CollapsingDeathmatchLevel.IsCollapsing())
		{
			if (Helicopter.CanAddBro())
			{
				if (this.deathmatchSpawnQueue.Count == 0)
				{
					HeroController.players[playerNum].RespawnOnHeli();
				}
				else if (this.deathmatchSpawnQueue[0] == playerNum)
				{
					this.deathmatchSpawnQueue.RemoveAt(0);
					HeroController.players[playerNum].RespawnOnHeli();
				}
				else if (!this.deathmatchSpawnQueue.Contains(playerNum))
				{
					this.deathmatchSpawnQueue.Add(playerNum);
				}
			}
			else if (!this.deathmatchSpawnQueue.Contains(playerNum))
			{
				this.deathmatchSpawnQueue.Add(playerNum);
			}
		}
	}

	private void RunFirstDeployment(int playernum)
	{
		if (HeroController.players[playernum] == null || !HeroController.players[playernum].IsMine)
		{
			UnityEngine.Debug.LogWarning("Cannot deploy remote player");
			return;
		}
		if (GameModeController.GameMode != GameMode.SuicideHorde || playernum == GameModeController.broPlayer)
		{
			SpawnPoint spawnPoint = Map.GetSpawnPoint(playernum);
			Vector3 position = new Vector3(-1f, -1f, -1f);
			if (spawnPoint != null)
			{
				position = spawnPoint.transform.position;
				if (spawnPoint.cage != null)
				{
					spawnPoint.cage.SetPlayerColor(playernum);
				}
			}
			Vector2 pos;
			if (HeroController.Instance.brosHaveBeenReleased)
			{
				pos = HeroController.GetFirstPlayerPosition() - Vector2.right * 16f;
			}
			else
			{
				pos = position;
				if (Map.isEditing && LevelEditorGUI.levelEditorActive && LevelEditorGUI.lastCameraPos.x > 0f)
				{
					pos = LevelEditorGUI.lastCameraPos;
					LevelEditorGUI.lastCameraPos = -Vector3.one;
				}
			}
			HeroController.players[playernum].RequestNewHero(pos);
		}
		if (GameModeController.GameMode == GameMode.SuicideHorde)
		{
			HeroController.players[playernum].firstDeployment = false;
		}
	}

	private void MonitorPlayerDropin(int playernum)
	{
		int num = -1;
		this.GetFireKeyDown(ref num);
		bool flag = num != -1 && !HeroController.IsControllerIDUsed(num) && (!LevelEditorGUI.levelEditorActive || playernum == 0);
		if (flag && !HeroController.playersPlaying[playernum])
		{
			Networking.RPC<int, PID>(PID.TargetServer, new RpcSignature<int, PID>(this.RequestJoinGame), num, PID.MyID, true);
			HeroController.isCountdownFinished = true;
		}
	}

	private bool AreAllHerosDead()
	{
		foreach (Player player in HeroController.players)
		{
			if (player != null && player.IsAlive())
			{
				return false;
			}
		}
		return true;
	}

	public static void SetAllHeroesInvulnerable(float duration)
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i].character != null)
			{
				HeroController.players[i].character.SetInvulnerable(duration, false);
			}
		}
	}

	public static void SetHeroInvulnerable(int playerNum, float duration)
	{
		if (playerNum >= 0 && playerNum < HeroController.playersPlaying.Length && HeroController.players[playerNum] != null && HeroController.players[playerNum].character != null)
		{
			HeroController.players[playerNum].character.SetInvulnerable(duration, false);
		}
	}

	public static void SetHerosInvulnerable(float x, float y, float range, float duration)
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i].character != null)
			{
				float num = HeroController.players[i].character.x - x;
				if (num < range)
				{
					float num2 = HeroController.players[i].character.y - y;
					if (num2 < range)
					{
						HeroController.players[i].character.SetInvulnerable(duration, false);
					}
				}
			}
		}
	}

	protected static bool IsControllerIDUsed(int controllerID)
	{
		for (int i = 0; i < HeroController.playerControllerIDs.Length; i++)
		{
			if (HeroController.playerControllerIDs[i] == controllerID && HeroController.PIDS[i].IsMine)
			{
				return true;
			}
		}
		return false;
	}

	protected bool GetFireKeyDown(ref int controllerNum)
	{
		int controllerPressingFire = InputReader.GetControllerPressingFire();
		if (controllerPressingFire >= 0)
		{
			controllerNum = controllerPressingFire;
			return true;
		}
		return false;
	}

	protected bool PlayerExists(int playerNum)
	{
		return HeroController.players[playerNum] != null && HeroController.players[playerNum].Exists();
	}

	public static void RegisterHeroToPlayer(TestVanDammeAnim hero, int playerNum, HeroType heroTypeEnum)
	{
		Player player = HeroController.players[playerNum];
		player.character = hero;
		player.ActivateHUD();
		player.SetHeroType(heroTypeEnum);
		if (player.rescuingThisBro != null)
		{
			player.rescuingThisBro.DestroyWhenReady = true;
			player.rescuingThisBro.showHeroOnDestroy = hero;
			hero.HideCharacter();
		}
		else
		{
			hero.ShowCharacter();
		}
	}

	[RPC]
	private void RequestJoinGame(int controllerNum, PID requesteeID)
	{
		MonoBehaviour.print(string.Concat(new object[]
		{
			"> RequestJoinGame from ",
			requesteeID,
			" with controller ",
			controllerNum
		}));
		if (GameModeController.LevelFinished)
		{
			UnityEngine.Debug.Log("RequestJoinGame(): New players cannot join as the level is finished");
			return;
		}
		if (!Connect.IsHost)
		{
			UnityEngine.Debug.Log("Only master client may recieve this RPC!. RequestJoinGame()");
			return;
		}
		if (this.IsControIdRegisteredToPID(controllerNum, requesteeID))
		{
			return;
		}
		int nextUnusedPlayerNumber = this.GetNextUnusedPlayerNumber();
		MonoBehaviour.print("GetNextUnusedPlayerNumber " + nextUnusedPlayerNumber);
		if (nextUnusedPlayerNumber != -1)
		{
			MonoBehaviour.print(string.Concat(new object[]
			{
				">> Open Player num found ",
				nextUnusedPlayerNumber,
				" for pid:",
				requesteeID,
				" with controller: ",
				controllerNum
			}));
			Networking.RPC<int, int, PID>(PID.TargetAll, new RpcSignature<int, int, PID>(this.AddPlayer), nextUnusedPlayerNumber, controllerNum, requesteeID, true);
		}
		else
		{
			UnityEngine.Debug.Log(">> MayPlayerJoin: Cannot add new player as there is already 4 players");
		}
	}

	private bool IsControIdRegisteredToPID(int controlId, PID pid)
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.playerControllerIDs[i] == controlId && HeroController.PIDS[i].AsByte == pid.AsByte)
			{
				return true;
			}
		}
		return false;
	}

	private int GetNextUnusedPlayerNumber()
	{
		for (int i = 0; i < 4; i++)
		{
			if (!HeroController.playersPlaying[i])
			{
				return i;
			}
		}
		return -1;
	}

	[RPC]
	protected void AddPlayer(int playerNum, int controllerNum, PID playerPID)
	{
		MonoBehaviour.print(string.Concat(new object[]
		{
			"> AddPlayer ",
			playerNum,
			" with pid:",
			playerPID,
			" with controller: ",
			controllerNum,
			" Is Mine ",
			playerPID.IsMine,
			" name ",
			playerPID.PlayerName
		}));
		if (playerPID.IsMine)
		{
			Networking.InstantiateBuffered<GameObject>(this.playerPrefab, new object[]
			{
				playerNum,
				controllerNum,
				playerPID
			}, true);
		}
		HeroUnlockController.MakeSureTheresEnoughUnlockedBrosForAllTheJoinedPlayers();
	}

	public static int NumberOfPlayersOnThisPC()
	{
		int num = 0;
		foreach (PID pid in HeroController.PIDS)
		{
			if (pid.IsMine)
			{
				num++;
			}
		}
		return num;
	}

	public static int GetFirstHeroAlive()
	{
		for (int i = 0; i < HeroController.players.Length; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i].IsAlive())
			{
				return i;
			}
		}
		return 0;
	}

	public static Vector2 GetFirstPlayerPosition()
	{
		for (int i = 0; i < HeroController.players.Length; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i].IsAlive())
			{
				return HeroController.players[i].GetCharacterPosition();
			}
		}
		return HeroController.GetCheckPointPosition(0);
	}

	protected static Vector2 GetCheckPointOffset(int playerNum)
	{
		switch (playerNum)
		{
		case 0:
			return Vector2.right * 8f;
		case 1:
			return Vector2.right * 8f;
		case 2:
			return Vector2.right * 12f;
		case 3:
			return Vector2.right * 12f;
		default:
			return Vector2.right * 1f;
		}
	}

	public static Vector3 GetCheckPointPosition(int playerNum)
	{
		Vector3 spawnPointPosition = Map.GetSpawnPointPosition(playerNum);
		if (!LevelEditorGUI.IsActive && HeroController.players[playerNum].firstDeployment)
		{
			return spawnPointPosition;
		}
		return HeroController.checkPointStart + HeroController.GetCheckPointOffset(playerNum);
	}

	protected bool CanSpawn(int playerNum)
	{
		return HeroController.players[playerNum] != null && HeroController.players[playerNum].Lives > 0 && !HeroController.players[playerNum].IsAlive();
	}

	protected bool IsPlayerNearbyCheckPoint(ref Vector2 pos)
	{
		for (int i = 0; i < HeroController.players.Length; i++)
		{
			if (HeroController.playersPlaying[i] && HeroController.players[i] != null && HeroController.players[i].IsNearbyCheckPoint(ref pos))
			{
				MonoBehaviour.print("IsPlayerNearbyCheckPoint");
				return true;
			}
		}
		return false;
	}

	public static void DisableHud()
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null)
			{
				HeroController.players[i].DisableHud();
			}
		}
	}

	public static void EnableHud()
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null)
			{
				HeroController.players[i].EnableHud();
			}
		}
	}

	public static void ResetLossCounter()
	{
		if (HeroController.Instance != null && HeroController.Instance.allDead && (!HeroController.playersPlaying[0] || !HeroController.players[0].IsAlive()) && (!HeroController.playersPlaying[1] || !HeroController.players[1].IsAlive()) && (!HeroController.playersPlaying[2] || !HeroController.players[2].IsAlive()) && (!HeroController.playersPlaying[3] || !HeroController.players[3].IsAlive()) && HeroController.Instance.lossTimer < 0.5f)
		{
			HeroController.Instance.lossTimer = 0.5f;
			Fader.StopFading();
		}
	}

	public static void PlayerDelay(int playerNum)
	{
		HeroController.players[playerNum].playerDelay = 0.5f;
	}

	public static void SetCheckPoint(Vector2 checkPointPos)
	{
		Networking.RPC<Vector2>(PID.TargetAll, new RpcSignature<Vector2>(HeroController.Instance.SetCheckPointInternal), checkPointPos, false);
	}

	[RPC]
	public void SetCheckPointInternal(Vector2 checkPointPos)
	{
		HeroController.checkPointStart = checkPointPos;
	}

	private IEnumerator DoCountDown()
	{
		if (HeroController.isCountdownFinished)
		{
			yield break;
		}
		if (!Map.MapData.suppressAnnouncer)
		{
			yield return new WaitForSeconds(0.2f + ((GameModeController.campaignLevelFailCount <= 2) ? 0f : 0.1f));
			InfoBar.Appear(0.5f, "3", HeroController.Instance.infoBarColor, 1f);
			Announcer.Announce3(0.11f, 1f);
			yield return new WaitForSeconds(0.55f + ((GameModeController.campaignLevelFailCount <= 2) ? 0f : 0.2f));
			InfoBar.Appear(0.5f, "2", HeroController.Instance.infoBarColor, 1f);
			Announcer.Announce2(0.11f, 1f);
			yield return new WaitForSeconds(0.35f + ((GameModeController.campaignLevelFailCount <= 2) ? 0f : 0.2f));
			InfoBar.Appear(0.5f, "1", HeroController.Instance.infoBarColor, 1f);
			Announcer.Announce1(0.11f, 1f);
			yield return new WaitForSeconds(0.5f + ((GameModeController.campaignLevelFailCount <= 2) ? 0f : 0.2f));
			if (Map.isEditing)
			{
				InfoBar.Appear(0.4f, "EDIT!", this.infoBarColor, 1f);
			}
			else if (GameModeController.publishRun)
			{
				InfoBar.Appear(0.4f, "TEST!", this.infoBarColor, 1f);
				LevelTitle.ShowText("PUBLISHING TEST RUN!", 0f);
			}
			else
			{
				Announcer.AnnounceGo(0.11f, 1f, 0.3f);
				InfoBar.Appear(0.4f, Announcer.currentAnouncement, this.infoBarColor, 1f);
			}
		}
		yield return new WaitForSeconds(0.5f);
		HeroController.isCountdownFinished = true;
		if (!GameModeController.SpawnBeforeCountdown)
		{
			this.SpawnJoinedPlayers();
		}
		yield break;
	}

	public static Color GetHeroColor(int playerNum)
	{
		if (GameModeController.GameMode == GameMode.TeamDeathMatch)
		{
			switch (playerNum)
			{
			case 0:
			case 1:
				return new Color(1f, 0f, 0f, 1f);
			case 2:
			case 3:
				return new Color(0f, 0.5f, 1f, 1f);
			}
		}
		switch (playerNum)
		{
		case 0:
			return new Color(0f, 0.5f, 1f, 1f);
		case 1:
			return new Color(1f, 0f, 0f, 1f);
		case 2:
			return new Color(1f, 0.45f, 0f, 1f);
		case 3:
			return new Color(0.55f, 0f, 1f, 1f);
		case 4:
			return new Color(0f, 0.8f, 0.3f, 1f);
		case 5:
			return new Color(0.6f, 1f, 0f, 1f);
		case 6:
			return new Color(1f, 0.8f, 0f, 1f);
		case 7:
			return new Color(1f, 0f, 0.6f, 1f);
		default:
			return Color.white;
		}
	}

	public static string GetHeroColorName(int playerNum)
	{
		switch (playerNum)
		{
		case 0:
			return "BLUE";
		case 1:
			return "RED";
		case 2:
			return "ORANGE";
		case 3:
			return "PURPLE";
		case 4:
			return "GREEN";
		case 5:
			return "LEMON";
		case 6:
			return "YELLOW";
		case 7:
			return "PINK";
		default:
			return "WHITE";
		}
	}

	public static bool MustShowHuds()
	{
		return HeroController.mustShowHUDS;
	}

	public static Player GetPlayerUsingController(int controllerID)
	{
		for (int i = 0; i < HeroController.players.Length; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i].controllerNum == controllerID)
			{
				return HeroController.players[i];
			}
		}
		return null;
	}

	public static string GetHeroName(HeroType type)
	{
		switch (type)
		{
		case HeroType.BaBroracus:
			return "B.A. Broracus";
		case HeroType.BrodellWalker:
			return "Brodell Walker";
		case HeroType.Blade:
			return "Brade";
		case HeroType.McBrover:
			return "MacBrover";
		case HeroType.Brononymous:
			return "Bro In Black";
		case HeroType.BroDredd:
			return "Bro Dredd";
		case HeroType.BroHard:
			return "Bro Hard";
		case HeroType.SnakeBroSkin:
			return "Snake Broskin";
		case HeroType.IndianaBrones:
			return "Indiana Brones";
		case HeroType.AshBrolliams:
			return "Ash Brolliams";
		case HeroType.Nebro:
			return "Mr Anderbro";
		case HeroType.BoondockBros:
			return "Boondock Bros";
		case HeroType.BronanTheBrobarian:
			return "Bronan The Brobarian";
		case HeroType.EllenRipbro:
			return "Ellen Ripbro";
		case HeroType.CherryBroling:
			return "Cherry Broling";
		case HeroType.TimeBroVanDamme:
			return "Time Bro";
		case HeroType.ColJamesBroddock:
			return "Col. James Broddock";
		case HeroType.BroniversalSoldier:
			return "Broniversal Soldier";
		case HeroType.BroneyRoss:
			return "Broney Ross";
		case HeroType.LeeBroxmas:
			return "Lee Broxmas";
		case HeroType.BronnarJensen:
			return "Bronnar Jensen";
		case HeroType.HaleTheBro:
			return "Bro Caesar";
		case HeroType.TrentBroser:
			return "Trent Broser";
		case HeroType.Broc:
			return "Broctor Death";
		case HeroType.TollBroad:
			return "Toll Broad";
		}
		return type.ToString();
	}

	public void SpawnSuicideBro(int playerNum)
	{
		HeroController.players[playerNum].RequestNewHero(new Vector2(this.nextSpawnDoor[playerNum].x, this.nextSpawnDoor[playerNum].y));
		this.nextSpawnDoor[playerNum] = null;
	}

	public void FindAndFlashNextSpawnDoor(int playerNum)
	{
		for (int i = 0; i < 50; i++)
		{
			MookDoor mookDoor = Map.mookDoors[UnityEngine.Random.Range(0, Map.mookDoors.Count)];
			if (this.nextSpawnDoor[0] != mookDoor && this.nextSpawnDoor[1] != mookDoor && this.nextSpawnDoor[2] != mookDoor && this.nextSpawnDoor[3] != mookDoor && SortOfFollow.IsItSortOfVisible(mookDoor.x, mookDoor.y, -28f, 0f) && !mookDoor.isDestroyed)
			{
				this.nextSpawnDoor[playerNum] = mookDoor;
				mookDoor.Flash(HeroController.GetHeroColor(playerNum));
				return;
			}
		}
	}

	public override UnityStream PackState(UnityStream stream)
	{
		stream.Serialize<Vector2>(HeroController.checkPointStart);
		stream.Serialize<bool>(this.brosHaveBeenReleased);
		return base.PackState(stream);
	}

	public override UnityStream UnpackState(UnityStream stream)
	{
		MonoBehaviour.print(string.Concat(new object[]
		{
			"> Unpack state players playing ",
			HeroController.playersPlaying[0],
			" ",
			HeroController.playersPlaying[1],
			" ",
			HeroController.playersPlaying[2],
			" ",
			HeroController.playersPlaying[3]
		}));
		HeroController.checkPointStart = (Vector2)stream.DeserializeNext();
		bool flag = (bool)stream.DeserializeNext();
		if (flag)
		{
			this.brosHaveBeenReleased = flag;
		}
		return base.UnpackState(stream);
	}

	public void DebugDraw()
	{
		GUILayout.Label("lastCameraFollowPos " + HeroController.lastCameraFollowPos, new GUILayoutOption[0]);
		GUILayout.Label("followPos " + SortOfFollow.followPos, new GUILayoutOption[0]);
		if (SortOfFollow.instance != null)
		{
			GUILayout.Label("followPosLerp " + SortOfFollow.instance.followPosLerp, new GUILayoutOption[0]);
			GUILayout.Label("followMode " + SortOfFollow.instance.followMode, new GUILayoutOption[0]);
			GUILayout.Label("Cam position " + SortOfFollow.instance.transform.position, new GUILayoutOption[0]);
		}
		GUILayout.Label(string.Empty, new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("CONTROLLER|", new GUILayoutOption[0]);
		for (int i = 0; i < 4; i++)
		{
			GUILayout.Label(string.Empty + HeroController.playerControllerIDs[i], new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("PIDS|", new GUILayoutOption[0]);
		for (int j = 0; j < 4; j++)
		{
			GUILayout.Label(string.Empty + HeroController.PIDS[j], new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("IS PLAYING|", new GUILayoutOption[0]);
		for (int k = 0; k < 4; k++)
		{
			GUILayout.Label(string.Empty + HeroController.playersPlaying[k], new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("INSTANTIATED|", new GUILayoutOption[0]);
		for (int l = 0; l < 4; l++)
		{
			GUILayout.Label(string.Empty + (HeroController.players[l] == null), new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("HERO|", new GUILayoutOption[0]);
		for (int m = 0; m < 4; m++)
		{
			if (HeroController.players[m] != null && HeroController.players[m].character != null)
			{
				GUILayout.Label(string.Empty + HeroController.players[m].character, new GUILayoutOption[0]);
			}
			else
			{
				GUILayout.Label("null", new GUILayoutOption[0]);
			}
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("-HERO ALIVE-", new GUILayoutOption[0]);
		for (int n = 0; n < 4; n++)
		{
			GUILayout.Label(string.Empty + (HeroController.players[n] != null && HeroController.players[n].IsAlive()), new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("HERO ACTIVE|", new GUILayoutOption[0]);
		for (int num = 0; num < 4; num++)
		{
			GUILayout.Label(string.Empty + (HeroController.players[num] != null && HeroController.players[num].character != null && HeroController.players[num].character.gameObject.activeInHierarchy), new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("HIDING|", new GUILayoutOption[0]);
		for (int num2 = 0; num2 < 4; num2++)
		{
			GUILayout.Label(string.Empty + (HeroController.players[num2] != null && HeroController.players[num2].character != null && HeroController.players[num2].character.HidingPlayer), new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("HERO SYNC|", new GUILayoutOption[0]);
		for (int num3 = 0; num3 < 4; num3++)
		{
			GUILayout.Label(string.Empty + (HeroController.players[num3] != null && HeroController.players[num3].character != null && HeroController.players[num3].character.Syncronize), new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("Is Mine|", new GUILayoutOption[0]);
		for (int num4 = 0; num4 < 4; num4++)
		{
			GUILayout.Label(string.Empty + (HeroController.players[num4] != null && HeroController.players[num4].IsMine), new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("pendingRespawn|", new GUILayoutOption[0]);
		for (int num5 = 0; num5 < 4; num5++)
		{
			GUILayout.Label(string.Empty + (HeroController.players[num5] != null && HeroController.players[num5].pendingRespawn), new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("pendingRespawn|", new GUILayoutOption[0]);
		for (int num6 = 0; num6 < 4; num6++)
		{
			GUILayout.Label(string.Empty + (HeroController.players[num6] != null && HeroController.players[num6].RescueInProgress), new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("HERO POS|", new GUILayoutOption[0]);
		for (int num7 = 0; num7 < 4; num7++)
		{
			if (HeroController.players[num7] != null && HeroController.players[num7].character != null)
			{
				GUILayout.Label(string.Empty + HeroController.players[num7].character.transform.position, new GUILayoutOption[0]);
			}
			else
			{
				GUILayout.Label("N/A", new GUILayoutOption[0]);
			}
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("Bot Brain", new GUILayoutOption[0]);
		for (int num8 = 0; num8 < 4; num8++)
		{
			if (HeroController.players[num8] != null)
			{
				bool flag = GUILayout.Toggle(HeroController.players[num8].UsingBotBrain, new GUIContent(), new GUILayoutOption[0]);
				if (HeroController.players[num8].UsingBotBrain != flag)
				{
					int num9 = this.FindRemotePlayerToFollow(HeroController.players[num8]);
					if (flag)
					{
						HeroController.players[num8].EnableBotBrain();
					}
					else
					{
						HeroController.players[num8].DisableBotBrain();
					}
					if (num9 != -1)
					{
						HeroController.players[num8].SetBotbrainLeader(num9);
					}
				}
			}
			else
			{
				GUILayout.Label("N/A", new GUILayoutOption[0]);
			}
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("Following", new GUILayoutOption[0]);
		for (int num10 = 0; num10 < 4; num10++)
		{
			if (HeroController.players[num10] != null)
			{
				try
				{
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					GUILayout.Label(HeroController.players[num10].BotBrainLeader + string.Empty, new GUILayoutOption[]
					{
						GUILayout.Width(30f)
					});
					if (GUILayout.Button("-", new GUILayoutOption[]
					{
						GUILayout.Width(30f)
					}))
					{
						HeroController.players[num10].BotBrainLeader--;
					}
					if (GUILayout.Button("+", new GUILayoutOption[]
					{
						GUILayout.Width(30f)
					}))
					{
						HeroController.players[num10].BotBrainLeader++;
					}
					HeroController.players[num10].BotBrainLeader = Mathf.Clamp(HeroController.players[num10].BotBrainLeader, 0, 3);
					GUILayout.EndHorizontal();
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception, this);
				}
			}
			else
			{
				GUILayout.Label("N/A", new GUILayoutOption[0]);
			}
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	private int FindRemotePlayerToFollow(Player player)
	{
		int result = -1;
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.players[i] != player)
			{
				if (!HeroController.players[i].IsMine)
				{
					return i;
				}
				result = i;
			}
		}
		return result;
	}

	public static void AddLife(int playerNum)
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Add Life ",
			HeroController.players[playerNum] != null,
			" IsPlaying(playerNum) ",
			HeroController.IsPlaying(playerNum)
		}));
		if (HeroController.players[playerNum] != null && HeroController.IsPlaying(playerNum))
		{
			HeroController.players[playerNum].AddLife();
		}
	}

	internal static void GiveAllLifelessPlayersALife()
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.IsPlaying(i) && HeroController.players[i].Lives == 0)
			{
				HeroController.players[i].AddLife();
			}
		}
	}

	internal static bool IsAnyPlayerDead()
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.players[i] != null && HeroController.IsPlaying(i) && !HeroController.players[i].IsAlive())
			{
				return true;
			}
		}
		return false;
	}

	public GameObject playerPrefab;

	public static bool AllPlayersHaveJoined = false;

	public static Player[] players = new Player[4];

	public static int[] ammoCounts = new int[]
	{
		-1,
		-1,
		-1,
		-1
	};

	public static bool[] playersPlaying = new bool[4];

	public static PID[] PIDS = new PID[]
	{
		PID.NoID,
		PID.NoID,
		PID.NoID,
		PID.NoID
	};

	public static int[] playerControllerIDs = new int[]
	{
		-1,
		-1,
		-1,
		-1
	};

	public static bool[] UseBotBrain = new bool[4];

	public static int[] BotBrainLeader = new int[]
	{
		-1,
		-1,
		-1,
		-1
	};

	public TestVanDammeAnim rambo;

	public TestVanDammeAnim brommander;

	public TestVanDammeAnim baBroracus;

	public TestVanDammeAnim BrodellWalker;

	public TestVanDammeAnim Blade;

	public TestVanDammeAnim McBrover;

	public TestVanDammeAnim Brononymous;

	public TestVanDammeAnim BroHard;

	public TestVanDammeAnim BroDredd;

	public TestVanDammeAnim MadMaxBrotansky;

	public TestVanDammeAnim SnakeBroskin;

	public TestVanDammeAnim BroboCop;

	public TestVanDammeAnim Brominator;

	public TestVanDammeAnim IndianaBrones;

	public TestVanDammeAnim AshBrolliams;

	public TestVanDammeAnim Nebro;

	public TestVanDammeAnim BoondockBros;

	public TestVanDammeAnim SuicideBro;

	public TestVanDammeAnim Brochete;

	public TestVanDammeAnim BronanTheBrobarian;

	public TestVanDammeAnim EllenRipbro;

	public TestVanDammeAnim CherryBroling;

	public TestVanDammeAnim TimeBroVanDamme;

	public TestVanDammeAnim ColJamesBroddock;

	public TestVanDammeAnim BroniversalSoldier;

	public TestVanDammeAnim BroneyRoss;

	public TestVanDammeAnim LeeBroxmas;

	public TestVanDammeAnim BronnarJensen;

	public TestVanDammeAnim HaleTheBro;

	public TestVanDammeAnim TrentBroser;

	public TestVanDammeAnim Broc;

	public TestVanDammeAnim TollBroad;

	public BoondockBro boondockBroPrefab;

	[HideInInspector]
	public static HeroType nextHeroType = HeroType.None;

	public Material materialBromando;

	public Material materialRambro;

	public Material materialBrodellWalker;

	public Material materialBabroracus;

	public Material materialColJamesBroddock;

	public Material materialBlade;

	public Material materialMcBrover;

	public Material materialBrononymous;

	public Material materialBroHard;

	public Material materialBroDredd;

	public Material materialMadMaxBrotansky;

	public Material materialSnakeBroskin;

	public Material materialBroboCop;

	public Material materialIndianaBrones;

	public Material materialaAshBrolliams;

	public Material materialBrominator;

	public Material materialNebro;

	public Material materialBrochete;

	public Material materialBronanTheBrobarian;

	public Material materialEllenRipbro;

	public Material materialCherryBroling;

	public Material materialTimeBroVanneDamme;

	public Material materialColJamesBroddick;

	public Material materialBroniversalSoldier;

	public Material materialBoondockBros;

	public Material materialBroneyRoss;

	public Material materialLeeBroxmas;

	public Material materialBronnarJensen;

	public Material materialHaleTheBro;

	public Material materialTrentBroser;

	public Material materialBroc;

	public Material materialTollBroad;

	public List<RescueBro> rescueBros = new List<RescueBro>();

	public static List<HiddenExplosives> hiddenExplosives;

	protected float explosivesCounter;

	public static bool isCountdownFinished = false;

	protected static List<HeroType> heroAlreadyChosenTypes = new List<HeroType>();

	public Dictionary<Player, RescueBro> RescueBrosAwaitingHeroSpawn = new Dictionary<Player, RescueBro>();

	public bool brosHaveBeenReleased;

	private static HeroController inst;

	protected static LayerMask groundLayer;

	protected static LayerMask fragileLayer;

	protected static RaycastHit rayCastHit;

	public static HeroType alwaysChooseHero = HeroType.None;

	private List<int> deathmatchSpawnQueue = new List<int>();

	protected bool allDead = true;

	protected float inputDelay = 0.25f;

	protected float broRespawnDelay = 0.4f;

	public static bool mustShowHUDS = true;

	private float[] suicideBroSpawnDelay = new float[]
	{
		-2f,
		-2.25f,
		-2.5f,
		-2.75f
	};

	public Color[] playerColors;

	protected float lossTimer = 0.5f;

	protected static int followingPreferredPlayer = -1;

	protected static float lastFollowTimeSwitch = 0f;

	protected static float lastFollowTimeSwitchBack = 0f;

	private static float timeSinceFinish = 0f;

	protected static List<Transform> extraFollowPositions = new List<Transform>();

	protected static bool wasFollowingExtra = false;

	protected static float repositionDelay = 0f;

	public List<int> playerDeathOrder = new List<int>();

	public Color infoBarColor = Color.black;

	protected static float timeBoostTime = 0f;

	protected bool faded;

	protected float finishedCounter;

	protected static Vector2 checkPointStart = new Vector2(128f, 196f);

	protected static Vector3 lastCameraFollowPos;

	private MookDoor[] nextSpawnDoor = new MookDoor[4];
}
