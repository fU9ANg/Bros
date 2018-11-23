// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class HeroSelectController : MonoBehaviour
{
	private void Awake()
	{
		if (this.heroNameText != null)
		{
			this.heroNamePing = this.heroNameText.gameObject.GetComponent<GlowPingText>();
		}
	}

	private void Start()
	{
		this.hasBeenSelected = new bool[this.selectionPortraits.Length];
		this.heroes = new HeroType[this.selectionPortraits.Length];
		for (int i = 0; i < GameModeController.deathmatchHero.Length; i++)
		{
			GameModeController.deathmatchHero[i] = HeroType.None;
		}
		this.SetupHeroes();
		if (HeroController.GetPlayersPlayingCount() <= 0)
		{
			GameModeController.GameMode = GameMode.DeathMatch;
			LevelSelectionController.campaignToLoad = "DefaultDeathmatch";
			LevelSelectionController.loadCustomCampaign = false;
			LevelSelectionController.loadMode = MapLoadMode.Campaign;
			LevelEditorGUI.levelEditorActive = false;
			HeroController.playersPlaying[0] = true;
			HeroController.playerControllerIDs[0] = 0;
			HeroController.PIDS[0] = PID.MyID;
			string path = "Player/PlayerPrefab";
			object[] instantiationData = new object[]
			{
				0,
				0
			};
			GameObject gameObject = Resources.Load(path) as GameObject;
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				" Is null prefab? ",
				gameObject != null,
				" Is null players ",
				HeroController.players != null
			}));
			HeroController.players[0] = Networking.InstantiateBuffered<GameObject>(gameObject, Vector3.zero, Quaternion.identity, instantiationData, false).GetComponent<Player>();
			HeroController.playersPlaying[1] = true;
			HeroController.playerControllerIDs[1] = 1;
			HeroController.PIDS[1] = PID.MyID;
			object[] instantiationData2 = new object[]
			{
				1,
				1
			};
			HeroController.players[1] = Networking.InstantiateBuffered<GameObject>(gameObject, Vector3.zero, Quaternion.identity, instantiationData2, false).GetComponent<Player>();
		}
		this.SetupPlayerCards();
		if (HeroSelectController.selectingPlayer == -1)
		{
			this.MoveToNextPlayer();
		}
		else
		{
			HeroSelectController.selectingPlayer--;
			this.MoveToNextPlayer();
		}
		UnityEngine.Debug.Log("Start Hero select");
		Announcer.AnnounceBroSelect(0.4f, 1f, 0.5f);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F6))
		{
			Application.LoadLevel(Application.loadedLevel);
		}
		this.delay -= Time.deltaTime;
		if (!this.allPlayersHaveSelected)
		{
			this.CheckInput();
			this.UpdateSelectionBox();
			this.mainText.GetComponent<Renderer>().material.SetColor("_TintColor", Color.Lerp(Color.white, HeroController.GetHeroColor(HeroSelectController.selectingPlayer), Mathf.PingPong(Time.time * 4f, 1f)));
		}
		else
		{
			this.mainText.GetComponent<Renderer>().material.SetColor("_TintColor", Color.white);
			this.switchDelay -= Time.deltaTime * 1.25f;
			if (this.switchDelay > 2f)
			{
				this.mainText.text = "3";
			}
			else if (this.switchDelay > 1f)
			{
				this.mainText.text = "2";
			}
			else if (this.switchDelay > 0f)
			{
				this.mainText.text = "1";
			}
			else
			{
				Application.LoadLevel(LevelSelectionController.CurrentGameModeScene);
			}
		}
	}

	private void UpdateSelectionBox()
	{
		this.heroSelectBox.transform.position = Vector3.Lerp(this.heroSelectBox.transform.position, this.selectionPortraits[this.selectionIndex].transform.position, Time.deltaTime * 20f);
	}

	private void SetupHeroes()
	{
		for (int i = 0; i < this.heroes.Length; i++)
		{
			bool flag = false;
			while (!flag)
			{
				flag = true;
				this.heroes[i] = this.availableHeroes[UnityEngine.Random.Range(0, this.availableHeroes.Length)];
				for (int j = 0; j < i; j++)
				{
					if (this.heroes[i] == this.heroes[j])
					{
						flag = false;
					}
				}
			}
			this.SetPortrait(i, this.heroes[i]);
		}
	}

	private void SetupPlayerCards()
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.IsPlaying(i))
			{
				this.playerCards[i].box.SetColor(HeroController.GetHeroColor(i));
				this.playerCards[i].badgeHolder.gameObject.SetActive(true);
				float num = 1f;
				for (int j = 0; j < GameModeController.GetRequiredRoundWins(); j++)
				{
					SpriteSM spriteSM = UnityEngine.Object.Instantiate(this.matchWinBGPrefab, this.playerCards[i].badgeHolder.transform.position + (float)j * num * Vector3.up * this.matchWinBadgePrefab.height / 1.4f + Vector3.forward * (float)j, Quaternion.identity) as SpriteSM;
					spriteSM.transform.parent = this.playerCards[i].badgeHolder.transform;
				}
				for (int k = 0; k < GameModeController.GetPlayerRoundWins(i); k++)
				{
					SpriteSM spriteSM2 = UnityEngine.Object.Instantiate(this.matchWinBadgePrefab, this.playerCards[i].badgeHolder.transform.position + (float)k * num * Vector3.up * this.matchWinBadgePrefab.height / 1.4f + Vector3.forward * (float)k, Quaternion.identity) as SpriteSM;
					spriteSM2.transform.parent = this.playerCards[i].badgeHolder.transform;
				}
			}
			else
			{
				this.playerCards[i].box.SetColor(new Color(0.3f, 0.3f, 0.3f, 0.3f));
				this.playerCards[i].badgeHolder.gameObject.SetActive(false);
			}
		}
	}

	private void SetPortrait(int portrait, HeroType hero)
	{
		this.selectionPortraits[portrait].GetComponent<Renderer>().material = HeroController.GetAvatarMaterial(hero);
	}

	private void CheckInput()
	{
		int controllerNum = HeroController.playerControllerIDs[HeroSelectController.selectingPlayer];
		if (Input.GetKeyDown(KeyCode.F1))
		{
			if (Time.timeScale < 0.5f)
			{
				Time.timeScale = 1f;
			}
			else
			{
				UnityEngine.Debug.Log("Slow time");
				Time.timeScale = 0.1f;
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.LoadLevel(LevelSelectionController.MainMenuScene);
		}
		bool flag = true;
		foreach (bool flag2 in this.hasBeenSelected)
		{
			flag = (flag && flag2);
		}
		if (flag)
		{
			return;
		}
		int num = InputReader.GetControllerPressingFire();
		if (num >= 0 && !InputReader.IsControllerInUse(num))
		{
			this.TryAddPlayer(num);
		}
		num = InputReader.GetControllerPressingSpecial();
		if (num != -1 && num >= 0 && InputReader.IsControllerInUse(num) && HeroController.GetPlayersPlayingCount() > 2)
		{
			int num2 = -1;
			for (int j = 0; j < 4; j++)
			{
				if (HeroController.IsPlaying(j) && HeroController.playerControllerIDs[j] == num)
				{
					num2 = j;
				}
			}
			if (num2 >= 0)
			{
				this.RemovePlayer(num2);
			}
		}
		InputReader.GetInput(controllerNum, ref this.up, ref this.down, ref this.left, ref this.right, ref this.fire, ref this.jump, ref this.special, ref this.highfive);
		if (this.delay > 0f)
		{
			this.special = false; this.fire = (this.special );
		}
		if (this.left && !this.wasLeft)
		{
			this.MoveSelectionBoxLeft();
		}
		if (this.right && !this.wasRight)
		{
			this.MoveSelectionBoxRight();
		}
		this.wasLeft = this.left;
		this.wasRight = this.right;
		if (this.fire || this.jump)
		{
			GameModeController.deathmatchHero[HeroSelectController.selectingPlayer] = this.heroes[this.selectionIndex];
			this.heroSelectBox.transform.position = this.selectionPortraits[this.selectionIndex].transform.position;
			this.heroSelectBox.SetLowerLeftPixel(32f, 32f);
			this.selectionPortraits[this.selectionIndex].SetLowerLeftPixel(32f, (float)((int)this.selectionPortraits[this.selectionIndex].lowerLeftPixel.y));
			base.StartCoroutine(this.ShakeTransform(this.selectionPortraits[this.selectionIndex].transform, new ShakeM(1f, 1f)));
			base.StartCoroutine(this.ShakeTransform(this.heroSelectBox.transform, new ShakeM(1.5f, 0.75f)));
			if (!Announcer.PlayHeroName(GameModeController.deathmatchHero[HeroSelectController.selectingPlayer], 0.5f, true))
			{
				Sound.GetInstance().PlaySoundEffect(this.soundHolder.special2Sounds, 0.5f);
				base.Invoke("YeahSound", 0.15f);
			}
			this.hasBeenSelected[this.selectionIndex] = true;
			Puff puff = UnityEngine.Object.Instantiate(this.explosionPuff) as Puff;
			puff.transform.position = Vector3.down * 12f + this.heroSelectBox.transform.position - Vector3.forward * 2f;
			puff.transform.localScale = Vector3.one * 1.25f;
			this.heroSelectBox.SetLowerLeftPixel(64f, 64f);
			this.MoveToNextPlayer();
		}
	}

	private void RemovePlayer(int playerNum)
	{
		HeroController.Dropout(playerNum, true);
		GameModeController.ResetPlayerRoundWins(playerNum);
		this.SetupPlayerCards();
		if (HeroSelectController.selectingPlayer == playerNum)
		{
			UnityEngine.Object.Destroy(this.heroSelectBox.gameObject);
			this.MoveToNextPlayer();
		}
	}

	private void TryAddPlayer(int controllerNum)
	{
		if (HeroController.GetPlayersPlayingCount() < 4)
		{
			for (int i = 0; i < 4; i++)
			{
				if (!HeroController.IsPlaying(i))
				{
					HeroController.playersPlaying[i] = true;
					HeroController.playerControllerIDs[i] = controllerNum;
					HeroController.PIDS[i] = PID.MyID;
					this.SetupPlayerCards();
					Sound.GetInstance().PlaySoundEffect(this.soundHolder.attackSounds[0], 0.3f);
					base.StartCoroutine(this.ShakeTransform(this.playerCards[i].transform, new ShakeM(1.5f, 0.75f)));
					return;
				}
			}
		}
	}

	private void YeahSound()
	{
		Sound.GetInstance().PlaySoundEffect(this.soundHolder.specialSounds, 0.5f);
	}

	private void MoveSelectionBoxLeft()
	{
		do
		{
			this.selectionIndex--;
			if (this.selectionIndex < 0)
			{
				this.selectionIndex = this.selectionPortraits.Length - 1;
			}
		}
		while (this.hasBeenSelected[this.selectionIndex]);
		Sound.GetInstance().PlaySoundEffect(this.soundHolder.attackSounds[0], 0.3f);
		this.heroNameText.text = HeroController.GetHeroName(this.heroes[this.selectionIndex]).ToLower();
		this.heroNamePing.Ping(HeroController.GetHeroColor(HeroSelectController.selectingPlayer));
	}

	private void MoveSelectionBoxRight()
	{
		do
		{
			this.selectionIndex++;
			if (this.selectionIndex >= this.selectionPortraits.Length)
			{
				this.selectionIndex = 0;
			}
		}
		while (this.hasBeenSelected[this.selectionIndex]);
		Sound.GetInstance().PlaySoundEffect(this.soundHolder.attackSounds[0], 0.3f);
		this.heroNameText.text = HeroController.GetHeroName(this.heroes[this.selectionIndex]).ToLower();
		this.heroNamePing.Ping(HeroController.GetHeroColor(HeroSelectController.selectingPlayer));
	}

	private void MoveToNextPlayer()
	{
		int num = HeroSelectController.selectingPlayer;
		bool flag = false;
		do
		{
			HeroSelectController.selectingPlayer++;
			if (HeroSelectController.selectingPlayer >= 4)
			{
				HeroSelectController.selectingPlayer = 0;
			}
			if (HeroController.IsPlaying(HeroSelectController.selectingPlayer) && GameModeController.deathmatchHero[HeroSelectController.selectingPlayer] == HeroType.None)
			{
				flag = true;
			}
		}
		while (HeroSelectController.selectingPlayer != num && !flag);
		if (!flag)
		{
			this.allPlayersHaveSelected = true;
		}
		else
		{
			this.heroSelectBox = (UnityEngine.Object.Instantiate(this.heroSelectBoxPrefab) as SpriteSM);
			this.heroSelectBox.SetColor(HeroController.GetHeroColor(HeroSelectController.selectingPlayer));
			this.mainText.text = "P" + (HeroSelectController.selectingPlayer + 1) + " SELECT YOUR BRO!";
			while (this.hasBeenSelected[this.selectionIndex])
			{
				this.selectionIndex++;
				if (this.selectionIndex >= this.selectionPortraits.Length)
				{
					this.selectionIndex = 0;
				}
			}
			this.heroNameText.text = HeroController.GetHeroName(this.heroes[this.selectionIndex]).ToLower();
			this.heroNamePing.Ping(HeroController.GetHeroColor(HeroSelectController.selectingPlayer));
		}
	}

	private IEnumerator ShakeTransform(Transform trans, ShakeM shakeM)
	{
		Vector3 pos = trans.position;
		float sinXCounter = 0f;
		float sinYCounter = 1f;
		float sinXRate = 52f + UnityEngine.Random.value * 64f;
		float sinYRate = 30f + UnityEngine.Random.value * 64f;
		float lastM = 0f;
		for (;;)
		{
			float t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
			if (shakeM.m > lastM)
			{
				sinXRate = 52f + UnityEngine.Random.value * 64f;
				sinYRate = 30f + UnityEngine.Random.value * 64f;
			}
			sinXCounter += sinXRate * t;
			sinYCounter += sinYRate * t;
			sinXRate *= 1f - t * 8f;
			sinYRate *= 1f - t * 8f;
			if (shakeM.m > 0f)
			{
				shakeM.m -= t * 1.6f;
				if (shakeM.m <= 0f)
				{
					shakeM.m = 0f;
				}
				else if (shakeM.m > 2f)
				{
					shakeM.m = 2f;
				}
			}
			trans.position = pos + new Vector3(Mathf.Sin(sinXCounter) * shakeM.m * 4f, Mathf.Sin(sinYCounter) * shakeM.m * 2f, 0f);
			yield return null;
		}
		yield break;
	}

	public SpriteSM[] selectionPortraits;

	public TextMesh heroNameText;

	protected GlowPingText heroNamePing;

	public SpriteSM heroSelectBoxPrefab;

	public SpriteSM matchWinBadgePrefab;

	public SpriteSM matchWinBGPrefab;

	protected SpriteSM heroSelectBox;

	public SoundHolder soundHolder;

	public Puff explosionPuff;

	public HeroType[] availableHeroes;

	public PlayerCard[] playerCards;

	private HeroType[] heroes;

	private static int selectingPlayer = -1;

	private int selectionIndex;

	private bool[] hasBeenSelected;

	private bool up;

	private bool down;

	private bool left;

	private bool right;

	private bool fire;

	private bool jump;

	private bool special;

	private bool highfive;

	private bool wasLeft;

	private bool wasRight;

	private bool allPlayersHaveSelected;

	public TextMesh mainText;

	protected float delay = 0.75f;

	private float switchDelay = 3f;
}
