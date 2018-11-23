// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class JoinScreen : SingletonNetObj<JoinScreen>
{
	private void Awake()
	{
		MonoBehaviour.print("JoinScreen ready at " + Time.realtimeSinceStartup);
	}

	private void Start()
	{
		Connect.AllDeterminsiticObjectsHaveBeenRegistered();
		MonoBehaviour.print("JoinScreen ready at " + Time.realtimeSinceStartup);
		HeroController.ResetPlayersPlaying();
		this.letterBoxBot.transform.position = Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width / 2f, 0f, 5f));
		this.playerReady = new bool[4];
		this.ResetDelay();
	}

	private void Update()
	{
		int num = InputReader.GetControllerPressingJump();
		if (num < 0)
		{
			num = InputReader.GetControllerPressingFire();
		}
		if (num < 0)
		{
			num = InputReader.GetControllerPressingStart();
		}
		int controllerPressingSpecial = InputReader.GetControllerPressingSpecial();
		bool flag = HeroController.playersPlaying[0] || HeroController.playersPlaying[1] || HeroController.playersPlaying[2] || HeroController.playersPlaying[3];
		if (num > -1)
		{
			int num2 = -1;
			for (int i = 0; i < 4; i++)
			{
				if (HeroController.playerControllerIDs[i] == num && HeroController.PIDS[i].IsMine)
				{
					num2 = i;
				}
			}
			if (num2 < 0)
			{
				this.requestingJoin.SetActive(true);
				Networking.RPC<int, PID, string>(PID.TargetServer, new RpcSignature<int, PID, string>(this.RequestJoin), num, PID.MyID, PlayerOptions.Instance.PlayerName, false);
			}
		}
		if (controllerPressingSpecial > -1)
		{
			int num3 = -2;
			for (int j = 0; j < 4; j++)
			{
				if (HeroController.playerControllerIDs[j] == controllerPressingSpecial)
				{
					num3 = j;
				}
			}
			if (num3 >= 0 && HeroController.PIDS[num3].IsMine)
			{
				Networking.RPC<int>(PID.TargetServer, new RpcSignature<int>(this.RequestUnJoin), num3, false);
			}
		}
		this.UpdatePlayerStatus();
		int num4 = 0;
		for (int k = 0; k < 4; k++)
		{
			if (HeroController.playersPlaying[k])
			{
				num4++;
			}
		}
		if (this.prevPlayerCount != num4)
		{
			this.Go123.transform.SetLocalX(400f);
			this.ResetDelay();
			if (this.startRoutine != null)
			{
				base.StopCoroutine(this.startRoutine);
			}
			if (num4 > 0)
			{
				this.startRoutine = this.StartGameRoutine();
				base.StartCoroutine(this.startRoutine);
				MonoBehaviour.print(this.startRoutine.Current + " < ___");
			}
		}
		this.prevPlayerCount = num4;
		if (Input.GetKeyDown(KeyCode.Escape) || (!flag && controllerPressingSpecial > -1))
		{
			Application.LoadLevel(LevelSelectionController.MainMenuScene);
		}
	}

	private IEnumerator StartGameRoutine()
	{
		float timer = 0f;
		Vector3 targetPos = this.Go123.transform.localPosition;
		timer = 1f;
		targetPos.x = 400f;
		while (timer > 0f)
		{
			timer -= Time.deltaTime;
			this.Go123.transform.localPosition = Vector3.Lerp(this.Go123.transform.localPosition, targetPos, Time.deltaTime * 10f);
			yield return null;
		}
		timer = 1f;
		targetPos.x = 0f;
		Sound.GetInstance().PlayAudioClip(this.announcer.start3.RandomElement<AudioClip>(), base.transform.position, 0.35f);
		this.PlaySound();
		while (timer > 0f)
		{
			timer -= Time.deltaTime;
			this.Go123.transform.localPosition = Vector3.Lerp(this.Go123.transform.localPosition, targetPos, Time.deltaTime * 10f);
			yield return null;
		}
		timer = 1f;
		targetPos.x = -400f;
		Sound.GetInstance().PlayAudioClip(this.announcer.start2.RandomElement<AudioClip>(), base.transform.position, 0.35f);
		this.PlaySound();
		while (timer > 0f)
		{
			timer -= Time.deltaTime;
			this.Go123.transform.localPosition = Vector3.Lerp(this.Go123.transform.localPosition, targetPos, Time.deltaTime * 10f);
			yield return null;
		}
		timer = 1f;
		targetPos.x = -800f;
		Sound.GetInstance().PlayAudioClip(this.announcer.start1.RandomElement<AudioClip>(), base.transform.position, 0.35f);
		this.PlaySound();
		while (timer > 0f)
		{
			timer -= Time.deltaTime;
			this.Go123.transform.localPosition = Vector3.Lerp(this.Go123.transform.localPosition, targetPos, Time.deltaTime * 10f);
			yield return null;
		}
		timer = 1f;
		targetPos.x = -1200f;
		Sound.GetInstance().PlayAudioClip(this.announcer.go.RandomElement<AudioClip>(), base.transform.position, 0.35f);
		this.PlaySound();
		while (timer > 0f)
		{
			timer -= Time.deltaTime;
			this.Go123.transform.localPosition = Vector3.Lerp(this.Go123.transform.localPosition, targetPos, Time.deltaTime * 10f);
			yield return null;
		}
		yield return new WaitForSeconds(0.3f);
		if (Connect.IsHost)
		{
			Networking.RPC(PID.TargetAll, new RpcSignature(this.StartGame), false);
		}
		this.startRoutine = null;
		yield break;
	}

	private void RequestJoin(int controllerID, PID requestee, string playerName)
	{
		MonoBehaviour.print(string.Concat(new object[]
		{
			"RequestJoin ",
			controllerID,
			"   ",
			requestee
		}));
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.playerControllerIDs[i] == controllerID && HeroController.PIDS[i] == requestee)
			{
				MonoBehaviour.print(" Control/Pid pair aleady in use");
				return;
			}
		}
		for (int j = 0; j < 4; j++)
		{
			if (HeroController.playerControllerIDs[j] == -1)
			{
				Networking.RPC<int, int, PID, string>(PID.TargetAll, new RpcSignature<int, int, PID, string>(this.Join), j, controllerID, requestee, playerName, false);
				break;
			}
		}
	}

	private void RequestUnJoin(int playerNum)
	{
		Networking.RPC<int>(PID.TargetAll, new RpcSignature<int>(this.UnJoin), playerNum, false);
	}

	private void UnJoin(int playerNumber)
	{
		MonoBehaviour.print("UnJoin  HeroController.playersPlaying[playerNumber]");
		this.playerReady[playerNumber] = false;
		HeroController.playersPlaying[playerNumber] = false;
		HeroController.playerControllerIDs[playerNumber] = -1;
		HeroController.PIDS[playerNumber] = PID.NoID;
		HeroController.players[playerNumber] = null;
		this.PlaySound();
		this.playerDisplay[playerNumber].SetPlayerName(string.Empty);
	}

	private void Join(int playerNumber, int controllerJoin, PID pid, string playerName)
	{
		HeroController.PIDS[playerNumber] = pid;
		HeroController.playersPlaying[playerNumber] = true;
		HeroController.playerControllerIDs[playerNumber] = controllerJoin;
		if (!Connect.IsOffline)
		{
			this.playerDisplay[playerNumber].SetPlayerName(playerName);
		}
		else
		{
			this.playerDisplay[playerNumber].SetPlayerName(string.Empty);
		}
		this.playerReady[playerNumber] = true;
		if (pid.IsMine)
		{
			this.requestingJoin.SetActive(false);
			this.pressFireToJoin.SetActive(true);
		}
		this.PlaySound();
	}

	private void PlaySound()
	{
		Sound instance = Sound.GetInstance();
		instance.PlaySoundEffect(this.drumSounds.attackSounds[this.drumSoundNum % 5], 0.25f);
		this.drumSoundNum++;
	}

	private void ResetDelay()
	{
		MonoBehaviour.print("Reset delay " + this.gameStartDelay);
		this.gameStartDelay = 5.8f;
	}

	private void UpdatePlayerStatus()
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.playersPlaying[i])
			{
				this.playerDisplay[i].joined = true;
			}
			else
			{
				this.playerDisplay[i].joined = false;
			}
		}
	}

	private void StartGame()
	{
		MonoBehaviour.print("Start Game");
		MonoBehaviour.print("Campaign to Load " + LevelSelectionController.campaignToLoad);
		GameMode gameMode = GameModeController.GameMode;
		switch (gameMode)
		{
		case GameMode.Campaign:
			if (LevelSelectionController.returnToWorldMap)
			{
				Application.LoadLevel("NewMap");
				UnityEngine.Debug.Log("GO TO WORLD MAP   ");
			}
			else
			{
				LevelSelectionController.GotoNextCampaignScene(true);
			}
			return;
		default:
			if (gameMode != GameMode.TeamDeathMatch)
			{
				Application.LoadLevel(LevelSelectionController.CurrentGameModeScene);
				return;
			}
			break;
		case GameMode.DeathMatch:
			break;
		}
		Application.LoadLevel("HeroSelect");
	}

	public override UnityStream PackState(UnityStream stream)
	{
		MonoBehaviour.print("PackState Join");
		stream.Serialize<bool[]>(this.playerReady);
		stream.Serialize<int[]>(HeroController.playerControllerIDs);
		stream.Serialize<PID[]>(HeroController.PIDS);
		stream.Serialize<bool[]>(HeroController.playersPlaying);
		return base.PackState(stream);
	}

	public override UnityStream UnpackState(UnityStream stream)
	{
		MonoBehaviour.print("UnpackState Join");
		this.playerReady = (bool[])stream.DeserializeNext();
		HeroController.playerControllerIDs = (int[])stream.DeserializeNext();
		HeroController.PIDS = (PID[])stream.DeserializeNext();
		HeroController.playersPlaying = (bool[])stream.DeserializeNext();
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.playersPlaying[i])
			{
				this.Join(i, HeroController.playerControllerIDs[i], HeroController.PIDS[i], HeroController.PIDS[i].PlayerName);
			}
		}
		return base.UnpackState(stream);
	}

	public SoundHolder drumSounds;

	private int drumSoundNum;

	public SpriteSM letterBoxBot;

	public TextMesh bottomText;

	public TextMesh B_text;

	public SpriteSM backGround;

	public SpriteSM[] playerSprites;

	public TextMesh[] playerTexts;

	public Player playerPrefab;

	public GameObject pressFireToJoin;

	public GameObject requestingJoin;

	protected bool[] playerReady = new bool[4];

	public Material XMaterial;

	public Material BMaterial;

	public Material AMaterial;

	public Transform Go123;

	public SoundHolderAnnouncer announcer;

	public JoinScreenPlayerEntry[] playerDisplay;

	private float gameStartDelay = 4.8f;

	private int prevPlayerCount;

	private IEnumerator startRoutine;
}
