// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class GameModeController : MonoBehaviour
{
	public static bool IsDeathMatchMode
	{
		get
		{
			return GameModeController.GameMode == GameMode.TeamDeathMatch || GameModeController.GameMode == GameMode.DeathMatch;
		}
	}

	public static bool IsLevelFinished()
	{
		return !(GameModeController.instance == null) && GameModeController.instance.levelFinished;
	}

	public static bool IsHelicopterVictory()
	{
		return !(GameModeController.instance == null) && GameModeController.instance.levelFinished && HeroController.GetPlayersOnHelicopterAmount() > 0;
	}

	public static LevelResult GetLevelResult()
	{
		if (GameModeController.instance == null)
		{
			return LevelResult.Unknown;
		}
		return GameModeController.instance.levelResult;
	}

	public static GameModeController Instance
	{
		get
		{
			if (GameModeController.instance == null)
			{
				GameModeController.instance = (UnityEngine.Object.FindObjectOfType(typeof(GameModeController)) as GameModeController);
			}
			return GameModeController.instance;
		}
	}

	public static GameMode GameMode
	{
		get
		{
			return GameModeController.staticGameMode;
		}
		set
		{
			GameModeController.staticGameMode = value;
		}
	}

	public static bool HasEveryBodyVotedToSkip()
	{
		bool flag = false;
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.playersPlaying[i])
			{
				flag = true;
			}
		}
		if (!flag)
		{
			return false;
		}
		for (int j = 0; j < 4; j++)
		{
			if (HeroController.playersPlaying[j] && !GameModeController.playersVotedToSkip[j])
			{
				return false;
			}
		}
		return true;
	}

	private void VoteToSkipRPC(PID sender)
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.PIDS[i] == sender)
			{
				GameModeController.playersVotedToSkip[i] = true;
				LevelOverScreen.instance.VoteToSkip(i);
			}
		}
	}

	public static void SendReady()
	{
		Networking.RPC<PID>(PID.TargetAll, new RpcSignature<PID>(GameModeController.instance.VoteToSkipRPC), PID.MyID, false);
	}

	private void Awake()
	{
		GameModeController.instance = this;
		this.levelResult = LevelResult.Unknown;
		if (LevelSelectionController.currentCampaign != null)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Level Start ",
				LevelSelectionController.CurrentLevelNum,
				"  campaign ",
				LevelSelectionController.campaignToLoad,
				" Campaign length ",
				LevelSelectionController.currentCampaign.levels.Length
			}));
		}
		if (MainMenu.wasShown)
		{
			if (LevelEditorGUI.levelEditorActive && !this.forcePlayLevel)
			{
				Map.isEditing = true;
				this.levelEditor.gameObject.SetActive(true);
			}
			else if (this.levelEditor != null)
			{
				Map.isEditing = false;
				this.levelEditor.gameObject.SetActive(false);
			}
		}
		GameModeController.playersVotedToSkip = new bool[4];
		if (GameModeController.playerRoundWins == null)
		{
			GameModeController.playerRoundWins = new int[4];
			for (int i = 0; i < GameModeController.playerRoundWins.Length; i++)
			{
				GameModeController.playerRoundWins[i] = 0;
			}
			GameModeController.playerMatchWins = new int[4];
			for (int j = 0; j < GameModeController.playerMatchWins.Length; j++)
			{
				GameModeController.playerMatchWins[j] = 0;
			}
		}
		if (GameModeController.GameMode == GameMode.NotSet)
		{
			GameModeController.GameMode = this.gameMode;
		}
		this.gameMode = GameModeController.GameMode;
		if (GameModeController.IsDeathMatchMode)
		{
			HeroController.ResetAmmo();
		}
		if (GameModeController.GameMode == GameMode.ExplosionRun && GameModeController.explosionRunTotalAttempts > 4f)
		{
			GameModeController.explosionRunTotalAttempts *= 0.8f;
			GameModeController.explosionRunFailTotalFails *= 0.8f;
		}
		if (this.gameMode == GameMode.SuicideHorde && HeroController.GetPlayersPlayingCount() > 0)
		{
			do
			{
				GameModeController.broPlayer = (GameModeController.broPlayer + 1) % 4;
			}
			while (!HeroController.IsPlaying(GameModeController.broPlayer));
		}
	}

	private void Start()
	{
		if (GameModeController.GameMode == GameMode.Race)
		{
			base.gameObject.AddComponent<RaceModeController>();
		}
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		this.levelStartCounter -= num;
		int num2 = 0;
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.playersPlaying[i] && HeroController.players[i] != null && !HeroController.players[i].firstDeployment)
			{
				num2++;
			}
			if (this.gameMode == GameMode.BroDown && GameModeController.isPlayerDoingBrodown[i])
			{
				num2++;
			}
		}
		if (GameModeController.IsDeathMatchMode && Time.time - this.lastDebugLivesTime > 2f)
		{
			int num3 = 0;
			for (int j = 0; j < 4; j++)
			{
				if (HeroController.players[j] != null && HeroController.players[j].Lives > 0)
				{
					num3++;
				}
			}
			this.lastDebugLivesTime = Time.time;
		}
		if (this.gameMode == GameMode.SuicideHorde && HeroController.isCountdownFinished && (HeroController.players[GameModeController.broPlayer].character == null || !HeroController.players[GameModeController.broPlayer].IsAlive()))
		{
			GameModeController.LevelFinish(LevelResult.Unknown);
		}
		if (this.levelStartCounter <= 0f)
		{
			if ((GameModeController.IsDeathMatchMode || this.gameMode == GameMode.BroDown) && num2 > 1 && HeroController.GetPlayersAliveCount() <= 1)
			{
				int num4 = 0;
				for (int k = 0; k < 4; k++)
				{
					if (HeroController.players[k] != null && HeroController.players[k].Lives > 0)
					{
						num4++;
					}
				}
				if (num4 <= 1)
				{
					UnityEngine.Debug.Log(" only one player alive");
					GameModeController.LevelFinish(LevelResult.Unknown);
				}
				else if (this.gameMode == GameMode.BroDown)
				{
					GameModeController.LevelFinish(LevelResult.Unknown);
				}
				else if (GameModeController.GameMode == GameMode.TeamDeathMatch)
				{
					if (HeroController.players[0].Lives + HeroController.players[1].Lives <= 0)
					{
						GameModeController.LevelFinish(LevelResult.Unknown);
					}
					if (HeroController.players[2].Lives + HeroController.players[3].Lives <= 0)
					{
						GameModeController.LevelFinish(LevelResult.Unknown);
					}
				}
			}
			if (this.gameMode == GameMode.Race && num2 > 1 && !GameModeController.IsLevelFinished() && HeroController.GetPlayersAliveCount() <= 1)
			{
				GameModeController.LevelFinish(LevelResult.Unknown);
				this.winTimer = 2f;
				UnityEngine.Debug.Log("Level finished Race End ");
			}
		}
		if (this.deathMatchRewarded)
		{
			if (!HeroController.PlayerIsAlive(this.deathMatchWinnerPlayerNum))
			{
				this.switchLevelDelay = Mathf.Clamp(this.switchLevelDelay - 15f, 1.3f, 100f);
				this.deathMatchRewarded = false;
			}
			this.deathMatchRewardCounter += num;
			if (this.deathMatchRewardCounter > 2f)
			{
				this.deathMatchRewardCounter -= 3.5f;
				Pickupable pickupable = PickupableController.CreateAmmoBox(Map.GetBlocksX(UnityEngine.Random.Range(5, 25)), SortOfFollow.GetScreenMaxY() + 8f);
				if (pickupable != null)
				{
					pickupable.Launch(Map.GetBlocksX(UnityEngine.Random.Range(5, 25)), SortOfFollow.GetScreenMaxY() + 8f, 0f, 0f);
				}
			}
		}
		if (this.levelFinished)
		{
			this.winTimer -= num;
			if (this.winTimer <= 0f)
			{
				if (TriggerManager.CheckAndTriggerLevelEndTriggers())
				{
					this.winTimer = 999999f;
				}
				else
				{
					this.DetermineLevelOutcome();
					this.winTimer = 1000f;
				}
			}
		}
		if (this.switchingLevel && (this.switchLevelDelay -= num) <= 0f)
		{
			if (this.resetWinsOnLevelSwitch)
			{
				GameModeController.ResetPlayerRoundWins();
			}
			this.SwitchLevel();
		}
		if (Input.GetKeyDown(KeyCode.F11) && Input.GetKey(KeyCode.LeftControl))
		{
			WorldMapProgressController.FinishCampaign();
			this.nextScene = "NewMap";
			Fader.nextNextScene = this.nextScene; Fader.nextScene = (Fader.nextNextScene );
			Fader.FadeSolid(1f, true);
			UnityEngine.Debug.Log("FORCE CAMPAIGN Finish " + LevelSelectionController.CurrentLevelNum + "  CTRL +  F 11  ");
		}
	}

	public static bool ShowStandardHUDS()
	{
		return !GameModeController.IsDeathMatchMode && GameModeController.GameMode != GameMode.BroDown && GameModeController.GameMode != GameMode.ExplosionRun && GameModeController.GameMode != GameMode.Race && !StatisticsController.ShowBrotalityScore();
	}

	public static int GetRequiredRoundWins()
	{
		return 3;
	}

	public static int GetPlayerRoundWins(int playerNum)
	{
		if (GameModeController.playerRoundWins != null && playerNum >= 0 && playerNum < GameModeController.playerRoundWins.Length)
		{
			return GameModeController.playerRoundWins[playerNum];
		}
		return 0;
	}

	public static void ResetPlayerRoundWins()
	{
		if (GameModeController.playerRoundWins == null)
		{
			return;
		}
		for (int i = 0; i < GameModeController.playerRoundWins.Length; i++)
		{
			GameModeController.playerRoundWins[i] = 0;
		}
	}

	public static void ResetPlayerRoundWins(int playerNum)
	{
		if (GameModeController.playerRoundWins == null)
		{
			return;
		}
		GameModeController.playerRoundWins[playerNum] = 0;
	}

	public static bool IsMatchLeader(int playerNum)
	{
		bool result = false;
		if (GameModeController.playerMatchWins != null)
		{
			int num = 1;
			if (playerNum >= 0 && playerNum < GameModeController.playerMatchWins.Length)
			{
				for (int i = 0; i < GameModeController.playerMatchWins.Length; i++)
				{
					if (GameModeController.playerMatchWins[i] >= num)
					{
						num = GameModeController.playerMatchWins[0];
						if (i == playerNum)
						{
							result = true;
						}
					}
				}
			}
		}
		return result;
	}

	public static int GetPlayerMatchWins(int playerNum)
	{
		if (GameModeController.playerMatchWins != null && playerNum >= 0 && playerNum < GameModeController.playerMatchWins.Length)
		{
			return GameModeController.playerMatchWins[playerNum];
		}
		return 0;
	}

	public static void ResetPlayerMatchWins()
	{
		if (GameModeController.playerMatchWins == null)
		{
			return;
		}
		for (int i = 0; i < GameModeController.playerMatchWins.Length; i++)
		{
			GameModeController.playerMatchWins[i] = 0;
		}
	}

	public static void MakeFinishInstant()
	{
		GameModeController.instance.winTimer = 0.0001f;
	}

	public static void LevelFinish(LevelResult result)
	{
		if (!GameModeController.instance.levelFinished)
		{
			if (Connect.IsHost)
			{
				StatisticsController.CalculateTotalTime();
				GameModeController.instance.SyncStats();
			}
			Networking.RPC<int, LevelResult>(PID.TargetAll, new RpcSignature<int, LevelResult>(GameModeController.instance.LevelFinishRPC), LevelSelectionController.CurrentLevelNum, result, false);
		}
	}

	[RPC]
	public void LevelFinishRPC(int finishedLevelNum, LevelResult result)
	{
		if (LevelSelectionController.CurrentLevelNum != finishedLevelNum)
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Recieved LevelFinishRPC for different level, that cant be right. CurrentLevelNum: ",
				Map.LevelFileName,
				". finishedLevelNum: ",
				finishedLevelNum
			}));
		}
		else
		{
			GameModeController.instance.levelFinished = true;
			StatisticsController.NotifyLevelFinished(result);
			Brotalitometer.Reset();
			GameModeController.instance.winTimer = 2f;
			UnityEngine.Debug.Log("Level Finish " + LevelSelectionController.CurrentLevelNum);
			GameModeController.instance.levelResult = result;
		}
	}

	public static void RestartLevel()
	{
		if (GameModeController.instance != null)
		{
			if (Connect.IsHost)
			{
				MonoBehaviour.print("> Request Host Restart");
				GameModeController.instance.nextScene = Application.loadedLevelName;
				GameModeController.instance.SwitchLevel();
			}
			else
			{
				Networking.RPC(PID.TargetServer, new RpcSignature(GameModeController.instance.RequestHostRestartLevel), true);
				MonoBehaviour.print("> Request Host Restart");
				ChatSystem.AddMessage("Requesting Host Restart", PID.NoID, Connect.Timef);
			}
		}
	}

	public void RequestHostRestartLevel()
	{
		MonoBehaviour.print("> Requested to Restart");
		this.nextScene = Application.loadedLevelName;
		this.SwitchLevel();
	}

	public void SwitchLevel()
	{
		if (Connect.IsHost)
		{
			int arg = UnityEngine.Random.Range(-10000, 10000);
			SID incrementedSessionID = Connect.GetIncrementedSessionID();
			string text = LevelSelectionController.campaignToLoad;
			if (string.IsNullOrEmpty(text) && LevelSelectionController.currentCampaign != null)
			{
				text = LevelSelectionController.currentCampaign.name;
			}
			if (text == string.Empty)
			{
				UnityEngine.Debug.LogError("Dont know what campaign to tell the player to load");
			}
			MonoBehaviour.print(string.Concat(new object[]
			{
				"1) Map.nextLevelToLoad[",
				Map.nextLevelToLoad,
				"] CurrentLevelNum[",
				LevelSelectionController.CurrentLevelNum,
				"]"
			}));
			MonoBehaviour.print(string.Concat(new object[]
			{
				"CurrentLevelNum ",
				LevelSelectionController.CurrentLevelNum,
				" campaignToLoad ",
				LevelSelectionController.campaignToLoad
			}));
			Networking.RPC<SID, string, string, int, int, int, int, int, int, bool, GridPos>(PID.TargetAll, true, true, new RpcSignature<SID, string, string, int, int, int, int, int, int, bool, GridPos>(this.SwitchLevelInternal), incrementedSessionID, text, this.nextScene, arg, LevelSelectionController.CurrentLevelNum, Map.lastXLoadOffset, Map.lastYLoadOffset, Map.nextXLoadOffset, Map.nextYLoadOffset, Map.startFromSuperCheckPoint, Map.superCheckpointStartPos);
			MonoBehaviour.print("Flush");
		}
	}

	public static bool LevelFinished
	{
		get
		{
			return GameModeController.instance.levelFinished;
		}
		set
		{
			GameModeController.instance.levelFinished = value;
		}
	}

	[RPC]
	public void SwitchLevelInternal(SID newSessionID, string campaignToLoad, string NextScene, int nextSeed, int nextLevel, int lastXLoadOffset, int lastYLoadOffset, int nextXLoadOffset, int nextYLoadOffset, bool startFromSuperCheckPoint, GridPos superCheckpointStartPos)
	{
		Map.MapData = null;
		this.nextScene = NextScene;
		if (this.nextScene.ToUpper() == LevelSelectionController.VictoryScene.ToUpper() || this.nextScene.ToUpper().Equals("VICTORYCUSTOMCAMPAIGN"))
		{
			GameModeController.instance.gameMode = GameMode.Cutscene; GameModeController.GameMode = (GameModeController.instance.gameMode );
			Connect.Disconnect();
		}
		Networking.SetSeed(nextSeed);
		Connect.SetSessionID((byte)newSessionID);
		MonoBehaviour.print(string.Concat(new object[]
		{
			"> SwitchLevelInternal:   Seed: ",
			Networking.RandomSeed,
			"  | GameID ",
			newSessionID,
			" |  nextScene: ",
			this.nextScene,
			" | nextLevel: ",
			nextLevel
		}));
		if (!LevelSelectionController.isOnlineCampaign)
		{
			LevelSelectionController.campaignToLoad = campaignToLoad;
		}
		Map.nextLevelToLoad = nextLevel;
		Networking.StreamIsPaused = true;
		Time.timeScale = 1f;
		MonoBehaviour.print(string.Concat(new object[]
		{
			"3) Map.nextLevelToLoad[",
			Map.nextLevelToLoad,
			"] CurrentLevelNum[",
			LevelSelectionController.CurrentLevelNum,
			"] LevelSelectionController.campaignToLoad ",
			LevelSelectionController.campaignToLoad
		}));
		Map.lastXLoadOffset = lastXLoadOffset;
		Map.lastYLoadOffset = lastYLoadOffset;
		Map.nextXLoadOffset = nextXLoadOffset;
		Map.nextYLoadOffset = nextYLoadOffset;
		Map.superCheckpointStartPos = superCheckpointStartPos;
		Map.startFromSuperCheckPoint = startFromSuperCheckPoint;
		Application.LoadLevel(this.nextScene);
	}

	public void SyncStats()
	{
		byte[] arg = SingletonNetObj<StatisticsController>.Instance.SerializeStats();
		Networking.RPC<byte[]>(PID.TargetOthers, new RpcSignature<byte[]>(this.ReceiveStatsFromMaster), arg, false);
	}

	[RPC]
	public void ReceiveStatsFromMaster(byte[] stats)
	{
		MonoBehaviour.print("ReceiveStatsFromMaster");
		SingletonNetObj<StatisticsController>.Instance.DeserializeStats(stats);
	}

	private void DetermineLevelOutcome()
	{
		this.switchingLevel = true;
		this.switchLevelDelay = 3f;
		HeroController.DisableHud();
		if (LevelEditorGUI.levelEditorActive)
		{
			LevelEditorGUI.lastCameraPos = SortOfFollow.instance.transform.position;
			LevelSelectionController.loadMode = MapLoadMode.LoadFromMapdata;
			LevelSelectionController.MapDataToLoad = Map.MapData;
			Application.LoadLevel(Application.loadedLevel);
		}
		switch (GameModeController.GameMode)
		{
		case GameMode.Campaign:
			this.nextScene = LevelSelectionController.CampaignScene;
			if (this.levelResult == LevelResult.Fail && HeroController.GetPlayersAliveCount() > 0)
			{
				this.levelFinished = false;
				this.winTimer = 2f;
				this.switchingLevel = false;
				this.switchLevelDelay = 3f;
				HeroController.EnableHud();
				this.levelResult = LevelResult.Unknown;
				MonoBehaviour.print("levelFinished = false;");
				GameModeController.campaignLevelFailCount++;
			}
			else if ((this.levelResult != LevelResult.Fail && this.levelResult != LevelResult.ForcedFail && (HeroController.GetPlayersAliveCount() > 0 || HeroController.AtLeastOnePlayerStillHasALife())) || this.levelResult == LevelResult.Success)
			{
				Map.ClearSuperCheckpointStatus();
				GameModeController.campaignLevelFailCount = 0;
				if (HeroController.GetPlayersOnHelicopterAmount() > 0 || this.levelResult == LevelResult.Success)
				{
					CutsceneCharacterController.PlayedToVictory();
					PlayerProgress.Save();
					LevelSelectionController.CompleteCurrentLevel();
					if (!LevelSelectionController.IsCustomCampaign())
					{
						PlayerProgress.Instance.SetLastFinishedLevel(LevelSelectionController.CurrentLevelNum);
						PlayerProgress.Save();
						if (LevelSelectionController.CurrentLevelNum != 11)
						{
							if (LevelSelectionController.CurrentLevelNum == 16)
							{
								MissionScreenController.SetVariables(string.Empty, WeatherType.NoChange, RainType.NoChange);
								this.nextScene = "MissionScreenVietnam";
								Fader.nextScene = LevelSelectionController.CampaignScene; Fader.nextNextScene = (Fader.nextScene );
							}
							else if (LevelSelectionController.CurrentLevelNum == 18)
							{
								this.nextScene = "MissionScreenVietnam";
								MissionScreenController.SetVariables(string.Empty, WeatherType.Burning, RainType.NoChange);
								Fader.nextScene = LevelSelectionController.CampaignScene; Fader.nextNextScene = (Fader.nextScene );
							}
						}
					}
					if (this.switchSilently)
					{
						this.switchLevelDelay = 2f;
					}
					else
					{
						LevelOverScreen.Show(true);
						this.switchLevelDelay = 100f;
					}
					if (LevelSelectionController.CurrentLevelNum >= LevelSelectionController.GetCurrentCampaignLength())
					{
						if (LevelSelectionController.returnToWorldMap)
						{
							WorldMapProgressController.FinishCampaign();
							this.nextScene = "NewMap";
							string @string = PlayerPrefs.GetString("LastCampaignLoaded");
							if (!string.IsNullOrEmpty(@string))
							{
								UnityEngine.Debug.Log("Completed Campaign " + @string);
							}
							else
							{
								UnityEngine.Debug.LogError("No Last Campaign Loaded");
							}
							PlayerPrefs.SetString("LastCampaignLoaded", string.Empty);
						}
						else if (LevelSelectionController.loadCustomCampaign && LevelSelectionController.currentCampaign.header.isPublished)
						{
							this.nextScene = LevelSelectionController.CustomCampaignVictoryScene;
							if (LevelSelectionController.isOnlineCampaign)
							{
								PlayerProgress.Instance.lastOnlineLevelId = null;
								PlayerProgress.Instance.lastOnlineLevelProgress = 0;
							}
							StatisticsController.CalcAndSubmitCampaignScore();
						}
						else if (GameModeController.publishRun)
						{
							GameModeController.publishRun = false;
							LevelEditorGUI.levelEditorActive = true;
							LevelEditorGUI.publishRunSuccessful = true;
							LevelSelectionController.CurrentLevelNum = 0;
						}
						else
						{
							this.nextScene = LevelSelectionController.VictoryScene;
							PlayerProgress.Instance.SetLastFinishedLevel(-1);
							PlayerProgress.Save();
						}
					}
					UnityEngine.Debug.Log("Level Result Success   " + Fader.nextScene);
				}
			}
			else
			{
				GameModeController.campaignLevelFailCount++;
				Announcer.AnnounceFailure(0.12f, 1f);
				LevelOverScreen.Show(false);
			}
			break;
		case GameMode.ExplosionRun:
		case GameMode.Race:
			this.switchLevelDelay = 2.5f;
			GameModeController.explosionRunTotalAttempts += 1f;
			if (HeroController.GetPlayersOnHelicopterAmount() == 0)
			{
				if (GameModeController.GameMode == GameMode.ExplosionRun || HeroController.GetPlayersAliveCount() <= 0)
				{
					this.switchLevelDelay = 1.5f;
					if (GameModeController.GameMode == GameMode.Race)
					{
						ScoreScreen.Appear(4f, "TIED!", false, true, -1);
						this.nextScene = LevelSelectionController.RaceRunSceneName;
						UnityEngine.Debug.Log("RACE outcome  is TIED  ");
					}
					else
					{
						ScoreScreen.Appear(4f, "BRO TEAM FAIL!", false, true, -1);
						this.nextScene = LevelSelectionController.ExplosionRunSceneName;
					}
					GameModeController.explosionRunFailCount++;
					GameModeController.explosionRunFailTotalFails += 1f;
					GameModeController.explosionRunWinsInARow = 0;
				}
				else if (GameModeController.GameMode == GameMode.Race)
				{
					this.switchLevelDelay = 2f;
					int firstHeroAlive = HeroController.GetFirstHeroAlive();
					if (HeroController.GetPlayersAliveCount() > 1)
					{
						UnityEngine.Debug.LogError("Only one should be alive!!!");
					}
					GameModeController.playerRoundWins[firstHeroAlive]++;
					this.nextScene = LevelSelectionController.RaceRunSceneName;
					LevelSelectionController.CompleteCurrentLevel();
					UnityEngine.Debug.Log("RACE outcome " + firstHeroAlive + "  wins ");
					ScoreScreen.Appear(4f, "PLAYER " + (firstHeroAlive + 1) + " WINS THE RACE!!", true, true, firstHeroAlive);
				}
			}
			else if (HeroController.GetPlayersOnHelicopterAmount() == 1)
			{
				this.nextScene = LevelSelectionController.ExplosionRunSceneName;
				GameModeController.explosionRunWinsInARow++;
				GameModeController.explosionRunFailCount = 0;
				for (int i = 0; i < 4; i++)
				{
					if (HeroController.PlayerIsOnHelicopter(i))
					{
						GameModeController.playerRoundWins[i]++;
						LevelSelectionController.CompleteCurrentLevel();
						if (GameModeController.GetExplosionRunWinsInARow() > 2f)
						{
							ScoreScreen.Appear(4f, string.Empty + GameModeController.GetExplosionRunWinsInARow() + " WINS IN A ROW!", true, true, i);
						}
						else if (GameModeController.playerRoundWins[i] >= 3)
						{
							ScoreScreen.Appear(4f, "PLAYER " + (i + 1) + " WINS THE MATCH!!", true, true, i);
						}
						else
						{
							ScoreScreen.Appear(4f, "PLAYER " + (i + 1) + " WINS THE ROUND!", true, true, i);
						}
					}
				}
			}
			else if (HeroController.GetPlayersOnHelicopterAmount() > 1)
			{
				UnityEngine.Debug.Log("SET TO BRODOWN!!!");
				GameModeController.explosionRunWinsInARow++;
				GameModeController.explosionRunFailCount = 0;
				ScoreScreen.Appear(4f, "TIED! PREPARE FOR BRODOWN!", false, true, -1);
				this.nextScene = LevelSelectionController.BrodownSceneName;
				GameModeController.gameModeBeforeBrodown = GameModeController.GameMode;
				GameModeController.GameMode = GameMode.BroDown;
				LevelSelectionController.CompleteCurrentLevel();
				LevelSelectionController.loadMode = MapLoadMode.Campaign;
				LevelSelectionController.loadCustomCampaign = false;
				LevelSelectionController.campaignToLoad = "DefaultBrodown";
				for (int j = 0; j < 4; j++)
				{
					GameModeController.isPlayerDoingBrodown[j] = false;
					if (HeroController.PlayerIsOnHelicopter(j))
					{
						GameModeController.isPlayerDoingBrodown[j] = true;
						GameModeController.deathmatchHero[j] = HeroController.GetCurrentHeroType(j);
					}
				}
			}
			break;
		case GameMode.DeathMatch:
		case GameMode.TeamDeathMatch:
			this.nextScene = LevelSelectionController.DeathmatchSceneName;
			LevelSelectionController.CompleteCurrentLevel();
			if (HeroController.GetPlayersAliveCount() == 1 || HeroController.GetTotalLives() > 0)
			{
				for (int k = 0; k < 4; k++)
				{
					if (HeroController.PlayerIsAlive(k) || HeroController.PlayerHasALife(k))
					{
						GameModeController.playerRoundWins[k]++;
						if (GameModeController.playerRoundWins[k] >= 3)
						{
							this.resetWinsOnLevelSwitch = true;
							GameModeController.playerMatchWins[k]++;
							UnityEngine.Debug.Log("Player " + k + " + wins!! ");
							this.deathMatchRewardCounter = 0f;
							this.deathMatchRewarded = true;
							this.deathMatchWinnerPlayerNum = k;
							this.switchLevelDelay = 22f;
							HeroController.SetHeroInvulnerable(k, 0.4f);
							Sound.GetInstance().PlayVictorySting();
							ScoreScreen.Appear(20f, "PLAYER " + HeroController.GetHeroColorName(k) + " WINS!", true, true, k);
							this.nextScene = "HeroSelect";
						}
						else
						{
							this.nextScene = "HeroSelect";
							ScoreScreen.Appear(4f, "PLAYER " + HeroController.GetHeroColorName(k) + " WINS!", true, false, k);
						}
					}
				}
			}
			else
			{
				ScoreScreen.Appear(4f, "DRAW!", false, true, -1);
			}
			break;
		case GameMode.BroDown:
		{
			GameMode gameMode = GameModeController.gameModeBeforeBrodown;
			if (gameMode != GameMode.ExplosionRun)
			{
				if (gameMode != GameMode.Race)
				{
					UnityEngine.Debug.LogError("What mode is this!!");
				}
				else
				{
					this.nextScene = LevelSelectionController.RaceRunSceneName;
					LevelSelectionController.campaignToLoad = "DefaultRace";
				}
			}
			else
			{
				this.nextScene = LevelSelectionController.ExplosionRunSceneName;
				LevelSelectionController.campaignToLoad = "DefaultExplosionRun";
			}
			LevelSelectionController.loadMode = MapLoadMode.Campaign;
			LevelSelectionController.loadCustomCampaign = false;
			UnityEngine.Debug.Log("Next Scene " + this.nextScene);
			GameModeController.GameMode = GameModeController.gameModeBeforeBrodown;
			if (HeroController.GetPlayersAliveCount() == 0)
			{
				ScoreScreen.Appear(4f, "DRAW!", false, true, -1);
			}
			else
			{
				for (int l = 0; l < 4; l++)
				{
					if (HeroController.PlayerIsAlive(l))
					{
						GameModeController.playerRoundWins[l]++;
						ScoreScreen.Appear(4f, "PLAYER " + (l + 1) + " WINS!", true, true, l);
					}
				}
			}
			break;
		}
		case GameMode.SuicideHorde:
			this.nextScene = Application.loadedLevelName;
			LevelSelectionController.CompleteCurrentLevel();
			LevelSelectionController.CurrentLevelNum = UnityEngine.Random.Range(0, 3);
			break;
		}
	}

	public static int GetBrodownBroCount()
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			if (GameModeController.isPlayerDoingBrodown[i])
			{
				num++;
			}
		}
		return num;
	}

	public static float GetExplosionRunWinsInARow()
	{
		return (float)GameModeController.explosionRunWinsInARow;
	}

	public static float GetExplosionRunFailM()
	{
		if (GameModeController.explosionRunTotalAttempts > 0f)
		{
			return GameModeController.explosionRunFailTotalFails / GameModeController.explosionRunTotalAttempts;
		}
		return 0f;
	}

	public static float GetExplosionRunLevelFails()
	{
		return (float)GameModeController.explosionRunFailCount;
	}

	public static int GetExplosionRunTotalAttempts()
	{
		return (int)GameModeController.explosionRunTotalAttempts;
	}

	public static void SetSwitchDelay(float d)
	{
		GameModeController.instance.switchLevelDelay = d;
	}

	public static bool InSwitchingScenesPhase()
	{
		return GameModeController.instance.switchingLevel;
	}

	public static bool InRewardPhase()
	{
		return GameModeController.instance.deathMatchRewarded;
	}

	public static int GetWinnerNum()
	{
		return GameModeController.instance.deathMatchWinnerPlayerNum;
	}

	public static bool DoesPlayerNumDamage(int fromNum, int toNum)
	{
		switch (GameModeController.instance.gameMode)
		{
		case GameMode.Campaign:
		case GameMode.ExplosionRun:
		case GameMode.Race:
			return (fromNum < 0 && toNum >= 0) || (fromNum >= 0 && toNum < 0) || fromNum < -5;
		case GameMode.DeathMatch:
		case GameMode.BroDown:
		case GameMode.SuicideHorde:
			return (fromNum <= 5 || toNum < 0) && (fromNum != toNum || (fromNum > 5 && toNum >= 0));
		case GameMode.Cutscene:
			return toNum < -5;
		case GameMode.TeamDeathMatch:
			return (fromNum <= 5 || toNum < 0) && fromNum + toNum != 1 && fromNum + toNum != 5 && fromNum != toNum;
		}
		UnityEngine.Debug.LogError(string.Concat(new object[]
		{
			"Could not determine damage ",
			fromNum,
			" to ",
			toNum
		}));
		return true;
	}

	public static void AddPoint(int playernum)
	{
		GameModeController.playerRoundWins[playernum]++;
	}

	public static void SetupIntroActions()
	{
		if (GameModeController.GameMode == GameMode.ExplosionRun || GameModeController.IsDeathMatchMode || GameModeController.GameMode == GameMode.Race || GameModeController.GameMode == GameMode.BroDown)
		{
			float offset = 0f;
			if (GameModeController.GameMode == GameMode.ExplosionRun && !LevelEditorGUI.levelEditorActive)
			{
				offset = 2f;
			}
			TriggerManager.AddCountdownAction(offset);
		}
		else if (GameModeController.GameMode == GameMode.SuicideHorde)
		{
			TriggerManager.AddCountdownAction(0f);
		}
	}

	public static bool SpawnBeforeCountdown
	{
		get
		{
			switch (GameModeController.instance.gameMode)
			{
			case GameMode.Campaign:
			case GameMode.Race:
				return false;
			case GameMode.ExplosionRun:
			case GameMode.DeathMatch:
			case GameMode.BroDown:
			case GameMode.SuicideHorde:
			case GameMode.TeamDeathMatch:
				return true;
			}
			UnityEngine.Debug.LogError("Need to set whether bro's are spawned before or after countdown for this gamemode!");
			return true;
		}
	}

	private void OnDisable()
	{
	}

	public static bool AllowPlayerDropIn
	{
		get
		{
			return GameModeController.GameMode == GameMode.Campaign;
		}
	}

	public const int deathmatchRoundsPerMatch = 3;

	private static GameModeController instance;

	public GameMode gameMode;

	private bool switchingLevel;

	private float switchLevelDelay = 1.5f;

	public static bool[] isPlayerDoingBrodown = new bool[4];

	public static HeroType[] deathmatchHero = new HeroType[]
	{
		HeroType.None,
		HeroType.None,
		HeroType.None,
		HeroType.None
	};

	public static int deathMatchLives = 5;

	public bool forcePlayLevel;

	[HideInInspector]
	public static bool[] playersVotedToSkip = new bool[4];

	public static int broPlayer = -1;

	private string nextScene = string.Empty;

	public LevelEditorGUI levelEditor;

	private static GameMode gameModeBeforeBrodown = GameMode.NotSet;

	private static GameMode staticGameMode = GameMode.NotSet;

	protected static int[] playerRoundWins;

	protected static int[] playerMatchWins = new int[4];

	protected bool levelFinished;

	protected LevelResult levelResult;

	public static int campaignLevelFailCount = 0;

	protected float winTimer = 2f;

	protected float levelStartCounter = 2f;

	protected float deathMatchRewardCounter;

	protected bool deathMatchRewarded;

	protected int deathMatchWinnerPlayerNum = -1;

	protected float lastDebugLivesTime;

	private static int explosionRunFailCount = 0;

	private static float explosionRunTotalAttempts = 0f;

	private static float explosionRunFailTotalFails = 0f;

	private static int explosionRunWinsInARow = 0;

	private bool resetWinsOnLevelSwitch;

	public static bool publishRun;

	public bool switchSilently;
}
