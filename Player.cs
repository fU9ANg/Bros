// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkObject
{
	public int controllerNum
	{
		get
		{
			if (this.playerNum >= 0)
			{
				return HeroController.playerControllerIDs[this.playerNum];
			}
			return -1;
		}
		set
		{
			HeroController.playerControllerIDs[this.playerNum] = value;
		}
	}

	public bool UsingBotBrain
	{
		get
		{
			return HeroController.UseBotBrain[this.playerNum];
		}
		set
		{
			HeroController.UseBotBrain[this.playerNum] = value;
		}
	}

	public int BotBrainLeader
	{
		get
		{
			return HeroController.BotBrainLeader[this.playerNum];
		}
		set
		{
			HeroController.BotBrainLeader[this.playerNum] = value;
		}
	}

	public int Lives
	{
		get
		{
			return this.lives;
		}
		set
		{
			int num = value;
			if (num < 0)
			{
				MonoBehaviour.print("Lives cant be less than 0");
				num = 0;
			}
			if (base.IsMine)
			{
				this.SetLivesRPC(num);
				Networking.RPC<int>(PID.TargetOthers, new RpcSignature<int>(this.SetLivesRPC), num, false);
			}
		}
	}

	private void SetLivesRPC(int _lives)
	{
		this.lives = _lives;
		if (this.hud != null)
		{
			this.hud.SetLives(_lives);
		}
	}

	public virtual void Awake()
	{
		int num = (int)base.InstantiationData[0];
		int num2 = (int)base.InstantiationData[1];
		PID pid = (PID)base.InstantiationData[2];
		this.playerNum = num;
		this.controllerNum = this.controllerNum;
		HeroController.PIDS[num] = pid;
		HeroController.playersPlaying[num] = true;
		HeroController.playerControllerIDs[num] = num2;
		HeroController.players[num] = this;
		if (SortOfFollow.GetInstance() != null && this.hud == null)
		{
			this.hud = (UnityEngine.Object.Instantiate(this.playerHUDPrefab) as PlayerHUD);
			this.hud.Setup(this.Lives, this.playerNum);
			this.hud.gameObject.SetActive(false);
		}
	}

	private void Start()
	{
		if (GameModeController.IsDeathMatchMode)
		{
			this.Lives = GameModeController.deathMatchLives;
		}
		else
		{
			this.Lives = 1;
		}
	}

	private void Update()
	{
		if (Map.Instance == null || !Map.Instance.HasBeenSetup)
		{
			return;
		}
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.03334f);
		if (this.playerDelay > 0f)
		{
			this.playerDelay -= num;
		}
		if (this.IsAlive() && this.character != null)
		{
			if (this.character.GetPilottedUnit() != null && GameModeController.ShowStandardHUDS())
			{
				this.hud.SetFuel(this.character.GetPilottedUnit().GetFuel(), this.character.GetPilottedUnit().GetFuelWarning());
			}
			else if (GameModeController.ShowStandardHUDS())
			{
				this.hud.SetGrenades(this.character.SpecialAmmo);
			}
			else
			{
				this.deathMatchHUD.SetGrenades(this.character.SpecialAmmo);
			}
		}
		else
		{
			this.hud.SetGrenades(0);
		}
		if (this.UsingBotBrain)
		{
			if (base.GetComponent<BotBrain>() == null)
			{
				this.EnableBotBrain();
			}
			base.GetComponent<BotBrain>().SetUnit(this.character);
			if (this.BotBrainLeader > -1 && HeroController.players[this.BotBrainLeader] != null && HeroController.players[this.BotBrainLeader].character != null)
			{
				base.GetComponent<BotBrain>().SetUnitToFollow(HeroController.players[this.BotBrainLeader].character);
			}
		}
	}

	protected override void LateUpdate()
	{
		base.LateUpdate();
		this.UpdateSmoothFollowPos();
	}

	public void UpdateSmoothFollowPos()
	{
		if (this.character != null)
		{
			Vector3 vector = new Vector3(this.character.x, this.character.y, 0f);
			if (base.IsMine)
			{
				this.SmoothedFollowPosition = vector;
			}
			else if (this.SmoothedFollowPosition.x < 0f)
			{
				this.SmoothedFollowPosition = vector;
			}
			else
			{
				float t = Mathf.Clamp01(Time.deltaTime * 10f);
				this.SmoothedFollowPosition = Vector3.Lerp(this.SmoothedFollowPosition, vector, t);
			}
		}
	}

	public void FlashSpecialAmmo()
	{
		this.hud.FlashSpecialIcons();
	}

	public void ActivateHUD()
	{
		this.hud.gameObject.SetActive(true);
	}

	public void RequestNewHero(Vector2 pos)
	{
		if (this.pendingRespawn)
		{
			MonoBehaviour.print("Spawn already pending");
			return;
		}
		if (!base.IsMine)
		{
			MonoBehaviour.print("This can only be called on locally owned instance");
			return;
		}
		this.pendingRespawn = true;
		HeroController.RequestHeroTypeFromMaster(pos, this.playerNum);
	}

	public void RespawnOnHeli()
	{
		TestVanDammeAnim testVanDammeAnim = this.InstantiateHero(GameModeController.deathmatchHero[this.playerNum], new Vector3(0f, 0f, 0f), this.playerNum, this.controllerNum);
		Map.AddBroToHeroTransport(testVanDammeAnim);
		if (!GameModeController.ShowStandardHUDS())
		{
			this.hud.gameObject.SetActive(false);
			this.deathMatchHUD = (UnityEngine.Object.Instantiate(this.deathMatchHUDPrefab) as HUDHeadGear);
			this.deathMatchHUD.Setup(this.Lives, this.playerNum, testVanDammeAnim);
			this.deathMatchHUD.gameObject.SetActive(true);
			this.deathMatchHUD.transform.parent = testVanDammeAnim.transform;
			this.deathMatchHUD.transform.localPosition = Vector3.zero;
		}
	}

	public void SpawnHero(Vector2 pos, HeroType nextHeroType)
	{
		if (this.character != null && this.character.IsAlive())
		{
			Networking.RPC(PID.TargetAll, new RpcSignature(this.character.RecallBro), false);
		}
		TestVanDammeAnim testVanDammeAnim = this.InstantiateHero(nextHeroType, new Vector3(pos.x, pos.y, 0f), this.playerNum, this.controllerNum);
		if (!GameModeController.ShowStandardHUDS())
		{
			this.hud.gameObject.SetActive(false);
			this.deathMatchHUD = (UnityEngine.Object.Instantiate(this.deathMatchHUDPrefab) as HUDHeadGear);
			this.deathMatchHUD.Setup(this.Lives, this.playerNum, testVanDammeAnim);
			this.deathMatchHUD.gameObject.SetActive(true);
			this.deathMatchHUD.transform.parent = testVanDammeAnim.transform;
			this.deathMatchHUD.transform.localPosition = Vector3.zero;
		}
	}

	private TestVanDammeAnim InstantiateHero(HeroType heroTypeEnum, Vector3 position, int PlayerNum, int ControllerNum)
	{
		if (!base.IsMine)
		{
			UnityEngine.Debug.LogWarning("InstantiateHero should only be executed locally");
			return null;
		}
		bool arg = false;
		if ((GameModeController.GameMode == GameMode.Campaign || GameModeController.GameMode == GameMode.Race) && (position.x < 0f || position.y < 0f))
		{
			MonoBehaviour.print("HeroController.Instance.brosHaveBeenReleased " + HeroController.Instance.brosHaveBeenReleased);
			if (!HeroController.Instance.brosHaveBeenReleased)
			{
				arg = true;
			}
			else
			{
				UnityEngine.Debug.LogWarning("pos is still negative and bros have been released,  + probs active network timing thing");
				position = HeroController.GetFirstPlayerPosition();
				UnityEngine.Debug.LogWarning("Usinf GetFirstPlayerPosition instead " + position);
			}
		}
		TestVanDammeAnim heroPrefab = HeroController.GetHeroPrefab(heroTypeEnum);
		TestVanDammeAnim testVanDammeAnim = Networking.InstantiateBuffered<TestVanDammeAnim>(heroPrefab, position, Quaternion.identity, new object[0], false);
		Networking.RPC<int, HeroType>(PID.TargetAll, new RpcSignature<int, HeroType>(testVanDammeAnim.SetUpHero), PlayerNum, heroTypeEnum, false);
		Networking.RPC<TestVanDammeAnim, Vector3, bool>(PID.TargetAll, new RpcSignature<TestVanDammeAnim, Vector3, bool>(this.MoveHeroToSpawnPosition), testVanDammeAnim, position, arg, false);
		return testVanDammeAnim;
	}

	public void SetHeroType(HeroType heroTypeEnum)
	{
		this.heroType = heroTypeEnum;
		this.hud.SwitchAvatarMaterial(this.heroType);
	}

	private void MoveHeroToSpawnPosition(TestVanDammeAnim hero, Vector3 spawnPosition, bool addBroToTransport)
	{
		if (spawnPosition.y <= 0f && GameModeController.GameMode == GameMode.Campaign)
		{
			spawnPosition = HeroController.GetCheckPointPosition(this.playerNum);
		}
		if (spawnPosition.y <= 0f && GameModeController.GameMode == GameMode.Campaign)
		{
			MonoBehaviour.print("addBroToTransport " + addBroToTransport);
			spawnPosition = Map.FindStartLocation();
			MonoBehaviour.print(hero + " Cant find position, using start location " + spawnPosition);
		}
		hero.x = spawnPosition.x;
		hero.y = spawnPosition.y;
		hero.SetPosition();
		this.SmoothedFollowPosition = spawnPosition;
		this.lastPos = spawnPosition;
		if (hero != null)
		{
			if (base.IsMine)
			{
				if (!this.firstDeployment && Announcer.HasInstance())
				{
					hero.PlaySpecialSound(0.4f, 1f, false);
				}
				if (addBroToTransport && base.IsMine)
				{
					MonoBehaviour.print("AddBroToHeroTransport " + spawnPosition);
					Map.AddBroToHeroTransport(this.character);
				}
				if (base.IsMine && HeroController.Instance.brosHaveBeenReleased)
				{
					this.SetInvulnerable(0.5f);
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Hero is null!");
		}
		this.firstDeployment = false;
		this.RescueInProgress = false;
		this.pendingRespawn = false;
	}

	public bool IsThisWay(float x, float y, int xDirection)
	{
		if (this.character != null && this.character.IsAlive())
		{
			float f = this.character.transform.position.x - x;
			if (Mathf.Sign(f) == Mathf.Sign((float)xDirection))
			{
				return true;
			}
		}
		return false;
	}

	public void AddLife()
	{
		if (base.IsMine)
		{
			this.Lives++;
			this.hud.ShowFreeLife();
		}
	}

	public void DisableHud()
	{
		if (this.hud != null)
		{
			this.hud.Hide();
			this.hud.gameObject.SetActive(false);
			if (this.hud.livesText != null)
			{
				this.hud.livesText.gameObject.SetActive(false);
			}
		}
	}

	public void EnableHud()
	{
		if (this.hud != null && GameModeController.ShowStandardHUDS())
		{
			this.hud.Show();
			this.hud.gameObject.SetActive(true);
			if (this.hud.livesText != null)
			{
				this.hud.livesText.gameObject.SetActive(true);
			}
		}
	}

	public void RemoveLife()
	{
		if (GameModeController.GameMode == GameMode.Campaign)
		{
			this.Lives--;
		}
		else
		{
			this.Lives--;
			UnityEngine.Debug.Log("Not Adjusting Lives text " + GameModeController.GameMode);
		}
	}

	public bool IsNearbyCheckPoint(ref Vector2 pos)
	{
		return this.character != null && Map.IsNearCheckPoint(this.character.x, this.character.y, ref pos);
	}

	public bool Exists()
	{
		return this.character != null && this.character.gameObject.activeInHierarchy;
	}

	public void AssignCharacter(TestVanDammeAnim character)
	{
		this.character = character;
		this.pendingRespawn = false;
		this.RescueInProgress = false;
	}

	public bool IsPendingRespawnOrResuingInProgress()
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"RescueInProgress ",
			this.RescueInProgress,
			" pendingRespawn ",
			this.pendingRespawn
		}));
		return this.RescueInProgress || this.pendingRespawn;
	}

	public bool IsAlive()
	{
		return (this.character != null && this.character.IsAlive()) || (this.RescueInProgress || this.pendingRespawn);
	}

	public bool IsInvulnerable()
	{
		return this.character != null && this.character.invulnerable;
	}

	public bool HasFollowPosition()
	{
		if (this.character != null && this.character.IsAlive())
		{
			return true;
		}
		if (!this.firstDeployment)
		{
			this.timeSinceDeath += Time.deltaTime;
			if (this.timeSinceDeath < 0.7f)
			{
				return true;
			}
		}
		return false;
	}

	public Vector3 GetFollowPosition()
	{
		if (this.playerTemporaryTarget != null)
		{
			return this.playerTemporaryTarget.position;
		}
		if (this.character != null && this.character.IsAlive())
		{
			this.timeSinceDeath = 0f;
			this.lastPos = this.SmoothedFollowPosition;
			return this.lastPos;
		}
		return this.lastPos;
	}

	public Vector3 GetCharacterPosition()
	{
		if (!(this.character != null))
		{
			return Vector3.zero;
		}
		if (this.character.GetPilottedUnit() != null)
		{
			return this.character.GetPilottedUnit().transform.position;
		}
		return this.character.transform.position;
	}

	public void SetInvulnerable(float time)
	{
		this.character.SetInvulnerable(time, true);
	}

	public void AddPlayerTarget(Transform target)
	{
		this.playerTemporaryTarget = target;
	}

	public void RemovePlayerTarget()
	{
		this.playerTemporaryTarget = null;
	}

	public bool HasTemporaryTarget()
	{
		return this.playerTemporaryTarget != null;
	}

	protected override void OnDestroy()
	{
		if (this.hud != null)
		{
			this.hud.gameObject.transform.parent = null;
			UnityEngine.Object.Destroy(this.hud.gameObject);
		}
		if (this.character != null)
		{
			this.character.DestroyUnit();
		}
	}

	public void GetInput(ref bool up, ref bool down, ref bool left, ref bool right, ref bool fire, ref bool buttonJump, ref bool special, ref bool highFive)
	{
		if (this.UsingBotBrain)
		{
			base.GetComponent<BotBrain>().GetInput(ref up, ref down, ref left, ref right, ref fire, ref buttonJump, ref special, ref highFive);
		}
		else
		{
			InputReader.GetInput(this.controllerNum, ref up, ref down, ref left, ref right, ref fire, ref buttonJump, ref special, ref highFive);
		}
	}

	public void BoostHero(float time)
	{
		if (this.character != null)
		{
			this.character.Boost(time);
		}
	}

	public void TimeBroBoostHero(float time)
	{
		if (this.character != null)
		{
			this.character.TimeBroBoost(time);
		}
	}

	public void RegisterMinion(Unit minion)
	{
		if (!this.minions.Contains(minion))
		{
			this.minions.Add(minion);
		}
		else
		{
			UnityEngine.Debug.LogError("Tried to register already registered minion");
		}
	}

	public void DeRegisterMinion(Unit minion)
	{
		if (this.minions.Contains(minion))
		{
			this.minions.Remove(minion);
		}
		else
		{
			UnityEngine.Debug.LogError("Tried to deregister already deregistered minion " + minion);
		}
	}

	public override UnityStream PackState(UnityStream stream)
	{
		stream.Serialize<int>(this.Lives);
		stream.Serialize<TestVanDammeAnim>(this.character);
		return base.PackState(stream);
	}

	public override UnityStream UnpackState(UnityStream stream)
	{
		this.lives = (int)stream.DeserializeNext();
		this.character = (TestVanDammeAnim)stream.DeserializeNext();
		return base.UnpackState(stream);
	}

	public void EnableBotBrain()
	{
		if (base.GetComponent<BotBrain>() == null)
		{
			base.gameObject.AddComponent<BotBrain>();
		}
		if (this.character != null)
		{
			base.GetComponent<BotBrain>().SetUnit(this.character);
		}
		this.UsingBotBrain = true;
	}

	public void DisableBotBrain()
	{
		this.UsingBotBrain = false;
	}

	public void SetBotbrainLeader(int playerNum)
	{
		this.BotBrainLeader = playerNum;
	}

	public int playerNum = -1;

	public TestVanDammeAnim character;

	public PlayerHUD playerHUDPrefab;

	public PlayerHUD hud;

	public HUDHeadGear deathMatchHUDPrefab;

	public HUDHeadGear deathMatchHUD;

	public HeroType heroType = HeroType.None;

	public Transform playerTemporaryTarget;

	public bool firstDeployment = true;

	public float playerDelay;

	public double lastTimeStamp;

	public bool RescueInProgress;

	[HideInInspector]
	public bool pendingRespawn;

	private Vector3 SmoothedFollowPosition = -Vector3.one;

	public List<Unit> minions = new List<Unit>();

	public RescueBro rescuingThisBro;

	private int lives = 1;

	private Vector3 lastPos = Vector3.zero;

	private float timeSinceDeath;
}
