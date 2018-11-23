// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Helicopter : BroforceObject
{
	protected virtual void Awake()
	{
		global::Math.SetupLookupTables();
		this.audioSource = base.gameObject.AddComponent<AudioSource>();
		this.audioSource.volume = this.helicopterVolume;
		this.audioSource.loop = true;
		this.audioSource.rolloffMode = AudioRolloffMode.Linear;
		this.audioSource.maxDistance = 320f;
		this.audioSource.loop = false;
		this.audioSource.dopplerLevel = 0.2f;
		this.audioSource.clip = this.helicopterSound;
		this.audioSource.Stop();
		this.ani = base.GetComponent<AnimatedTexture>();
		base.gameObject.SetActive(false);
		if (base.InstantiationData.Length > 0)
		{
			this.IsEndLevelHeli = (bool)base.InstantiationData[0];
			if (this.IsEndLevelHeli)
			{
				Map.RegisterHelicopter(this);
			}
		}
	}

	private void Start()
	{
		this.StartIdleAnim();
		this.SetLadderTransistion(1f);
		base.transform.parent = SingletonMono<MapController>.Instance.transform;
	}

	private float GetDirection()
	{
		if (this.idleFrame == 6)
		{
			return -1f;
		}
		return 1f;
	}

	private void StartIdleAnim()
	{
		Vector3 localPosition = this.sideRotor.transform.localPosition;
		localPosition.x = -Mathf.Abs(localPosition.x) * this.GetDirection();
		localPosition.z = -Mathf.Abs(localPosition.z) * this.GetDirection();
		this.sideRotor.transform.localPosition = localPosition;
		this.sideRotor.gameObject.SetActive(true);
		this.ani.enabled = false;
		this.ani.frame = this.idleFrame;
		this.isTurning = false;
	}

	private void StartCrashAnim()
	{
		this.ani.counter = 0f;
		this.ani.enabled = true;
		this.isTurning = false;
		this.prevFrame = this.ani.frame;
		foreach (GameObject gameObject in this.disableObjectsOnDeath)
		{
			gameObject.SetActive(false);
		}
	}

	private void StartTurningAnim()
	{
		if (!this.isTurning)
		{
			this.ani.counter = 0f;
			this.ani.enabled = true;
			this.ani.frame++;
			this.ani.frame %= this.ani.frames;
			this.isTurning = true;
			this.prevFrame = this.ani.frame;
			this.sideRotor.gameObject.SetActive(false);
		}
	}

	private void CheckIfTurningMustStop()
	{
		if (this.isTurning)
		{
			if (this.prevFrame < 6 && this.ani.frame >= 6)
			{
				this.idleFrame = 6;
				this.StartIdleAnim();
			}
			else if (this.prevFrame > this.ani.frame)
			{
				this.idleFrame = 0;
				this.StartIdleAnim();
			}
		}
		this.prevFrame = this.ani.frame;
	}

	public void SetDropStartPosition(Vector3 dropOffTarget)
	{
		if (!GameModeController.IsDeathMatchMode)
		{
			UnityEngine.Debug.DrawRay(dropOffTarget, UnityEngine.Random.onUnitSphere * 20f, Color.yellow, 10f);
			UnityEngine.Debug.DrawRay(dropOffTarget, UnityEngine.Random.onUnitSphere * 20f, Color.yellow, 10f);
			Vector3 a = this.FindBestWayIn(dropOffTarget);
			dropOffTarget += a * 200f;
			UnityEngine.Debug.DrawRay(dropOffTarget, a * 200f, Color.black, 10f);
			this.x = dropOffTarget.x;
			this.y = dropOffTarget.y;
			UnityEngine.Debug.DrawRay(dropOffTarget, UnityEngine.Random.onUnitSphere * 20f, Color.yellow, 10f);
			UnityEngine.Debug.DrawRay(dropOffTarget, UnityEngine.Random.onUnitSphere * 20f, Color.yellow, 10f);
		}
		else
		{
			if (dropOffTarget.x < Map.GetBlocksX(Map.Width / 2))
			{
				this.deathmatchFlybyDirection = DirectionEnum.Right;
				this.x = dropOffTarget.x - 128f;
				this.y = SortOfFollow.maxY + 96f;
			}
			else
			{
				this.deathmatchFlybyDirection = DirectionEnum.Left;
				this.x = dropOffTarget.x + 128f;
				this.y = SortOfFollow.maxY + 96f;
			}
			this.SetDeathmatchContainedBrosPositions();
		}
		this.SetPosition();
	}

	protected void SetDeathmatchContainedBrosPositions()
	{
		foreach (TestVanDammeAnim testVanDammeAnim in this.containedBros)
		{
			UnityEngine.Debug.Log("Set Deathmatch Helicopter Position");
			if (this.deathmatchFlybyDirection == DirectionEnum.Right)
			{
				testVanDammeAnim.transform.localPosition = new Vector3(2f, -30f, 0f);
			}
			else
			{
				testVanDammeAnim.transform.localPosition = new Vector3(-2f, -30f, 0f);
				testVanDammeAnim.transform.localScale = new Vector3(-1f, 1f, 1f);
				UnityEngine.Debug.Log("Go Left ! " + testVanDammeAnim.transform.localScale);
			}
		}
	}

	public void SetToDroppingHerosOff()
	{
		this.ladderVictoryCollider.enabled = false;
		this.DroppingHeroesOff = true;
		Helicopter.DropOffHeliInstance = this;
	}

	public void Enter(Vector2 Target, float delay)
	{
		if (this.hasBeenCalledDown)
		{
			return;
		}
		this.pickupTarget = Target;
		Target.y += 80f;
		UnityEngine.Debug.DrawRay(Target, UnityEngine.Random.onUnitSphere * 100f, Color.red, 10f);
		UnityEngine.Debug.DrawRay(Target, UnityEngine.Random.onUnitSphere * 100f, Color.red, 10f);
		UnityEngine.Debug.DrawRay(Target, UnityEngine.Random.onUnitSphere * 100f, Color.red, 10f);
		this.SetDropStartPosition(Target);
		base.gameObject.SetActive(true);
		this.targetY = Target.y;
		this.targetX = Target.x;
		float num = this.targetX - this.x;
		float num2 = this.targetY - this.y;
		this.xI = num * 0.2f;
		this.yI = num2 * 0.3f;
		this.hasBeenCalledDown = true;
		if (delay <= 0f)
		{
			this.SetPosition();
			this.StartAudio();
		}
		else
		{
			base.transform.position = Vector3.down * 100f;
		}
		this.arriveDelay = delay;
	}

	protected void StartAudio()
	{
		this.audioSource.Play();
		this.audioSource.loop = true;
	}

	private Vector3 FindBestWayOut(Vector3 from)
	{
		Vector3 up = Vector3.up;
		if (this.CheckDirection(45f, from, out up))
		{
			return up;
		}
		if (this.CheckDirection(75f, from, out up))
		{
			return up;
		}
		if (this.CheckDirection(25f, from, out up))
		{
			return up;
		}
		if (this.CheckDirection(90f, from, out up))
		{
			return up;
		}
		if (this.CheckDirection(1f, from, out up))
		{
			return up;
		}
		if (this.CheckDirection(-45f, from, out up))
		{
			return up;
		}
		if (this.CheckDirection(-75f, from, out up))
		{
			return up;
		}
		if (this.CheckDirection(-25f, from, out up))
		{
			return up;
		}
		if (this.CheckDirection(-90f, from, out up))
		{
			return up;
		}
		return Vector3.up;
	}

	private Vector3 FindBestWayIn(Vector3 target)
	{
		Vector3 up = Vector3.up;
		if (this.CheckDirection(-45f, target, out up))
		{
			return up;
		}
		if (this.CheckDirection(-75f, target, out up))
		{
			return up;
		}
		if (this.CheckDirection(-25f, target, out up))
		{
			return up;
		}
		if (this.CheckDirection(-90f, target, out up))
		{
			return up;
		}
		if (this.CheckDirection(1f, target, out up))
		{
			return up;
		}
		if (this.CheckDirection(45f, target, out up))
		{
			return up;
		}
		if (this.CheckDirection(75f, target, out up))
		{
			return up;
		}
		if (this.CheckDirection(25f, target, out up))
		{
			return up;
		}
		if (this.CheckDirection(90f, target, out up))
		{
			return up;
		}
		return Vector3.up;
	}

	private bool CheckDirection(float angle, Vector3 position, out Vector3 direction)
	{
		direction = Quaternion.AngleAxis(angle, -Vector3.forward) * Vector3.up;
		UnityEngine.Debug.DrawRay(position, direction * 1000f, Color.magenta, 10f);
		RaycastHit raycastHit;
		return !Physics.SphereCast(position, 50f, direction, out raycastHit, float.PositiveInfinity, Map.groundLayer);
	}

	public void Leave()
	{
		if (this.state != Helicopter.State.TakingOff)
		{
			Vector3 a = this.FindBestWayOut(base.transform.position);
			Vector3 vector = base.transform.position + a * 500f;
			UnityEngine.Debug.DrawRay(base.transform.position, a * 500f, Color.black, 20f);
			this.targetY = vector.y;
			this.targetX = vector.x;
			this.movementM = this.leaveMovementM;
			this.state = Helicopter.State.TakingOff;
		}
	}

	public virtual void SetPosition()
	{
		base.transform.position = new Vector3(this.x, this.y, 0f);
		if (this.containedBros != null && this.containedBros.Count > 0)
		{
			this.SetBrosPositions();
		}
	}

	private void SetLadderTransistion(float t)
	{
		t = Mathf.Clamp01(t);
		float x = 8f - 16f * t;
		Vector3 localPosition = new Vector3(x, -1f, 0f);
		this.ladderHolder.transform.localPosition = localPosition;
	}

	public static bool CanAddBro()
	{
		return Helicopter.DropOffHeliInstance == null || !Helicopter.DropOffHeliInstance.hasBeenCalledDown;
	}

	public bool IsLanding()
	{
		return this.state == Helicopter.State.Landing;
	}

	protected virtual void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (!this.isTurning && this.state != Helicopter.State.Crashing)
		{
			float num = 60f;
			if ((this.xI > num && this.idleFrame == 6) || (this.xI < -num && this.idleFrame == 0))
			{
				this.StartTurningAnim();
			}
		}
		this.CheckIfTurningMustStop();
		float num2 = (float)(this.ani.frames - this.ani.frame);
		if (this.idleFrame == 0)
		{
			float num3 = (float)(6 - this.ani.frame);
			num2 = num3 / 6f;
		}
		else
		{
			float num3 = (float)(14 - this.ani.frame);
			num2 = 1f - num3 / 8f;
		}
		this.SetLadderTransistion(1f - num2);
		Map.HitUnits(this, 20, DamageType.Melee, 23f, 5f, base.transform.position.x, base.transform.position.y + 34f, UnityEngine.Random.Range(-300f, 300f), 250f, false, true);
		if (this.arriveDelay > 0f)
		{
			this.arriveDelay -= this.t;
			if (this.arriveDelay <= 0f)
			{
				this.StartAudio();
			}
		}
		else
		{
			if (this.state == Helicopter.State.Crashing)
			{
				this.crashTime += this.t;
				if (this.xI < 25f - this.crashTime * 5f)
				{
					this.xI = 25f - this.crashTime * 5f;
				}
				this.xI *= 1f - this.t;
				this.yI -= 120f * this.t;
				this.smokeCounter += this.t;
				if (this.smokeCounter > 0.0334f)
				{
					this.smokeCounter -= 0.0334f;
					EffectsController.CreateBlackPlumeParticle(this.x + (float)UnityEngine.Random.Range(-5, 3), this.y + 24f, 16f, 0f, 45f, 1.5f, 1f);
					EffectsController.CreateBlackPlumeParticle(this.x + (float)UnityEngine.Random.Range(1, 9), this.y + 24f, 16f, 0f, 45f, 1.5f, 1f);
					EffectsController.CreateFireSparks(this.x + (float)UnityEngine.Random.Range(-5, 5), this.y, 1, 10f, 7f, 0f, 15f, 0.65f);
					EffectsController.CreateFireSparks(this.x + (float)UnityEngine.Random.Range(-1, 9), this.y, 1, 10f, 7f, 0f, 15f, 0.65f);
				}
				Collider[] array = Physics.OverlapSphere(base.transform.position + Vector3.up * 24f, 24f, Map.groundLayer);
				if (array.Length > 0)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != base.GetComponent<Collider>())
						{
							this.Death();
							break;
						}
					}
				}
			}
			else if (this.state == Helicopter.State.Landing)
			{
				if (this.audioSource.maxDistance < 500f)
				{
					this.audioSource.maxDistance += this.t * 80f;
				}
				float num4 = this.targetX - this.x;
				float num5 = this.targetY - this.y;
				this.xI += num4 * this.t * this.movementM;
				this.yI += num5 * this.t * this.movementM;
				this.xI *= 1f - this.t * 4f;
				this.yI *= 1f - this.t * 4f;
				this.sinCounter += this.t;
				this.yI += global::Math.Sin(this.sinCounter * 6f) * 60f * this.t;
				if (this.movementM < 10f)
				{
					this.movementM += this.t;
				}
				if (Mathf.Abs(num4) < 5f && Mathf.Abs(num5) < 5f && this.DroppingHeroesOff && (!GameModeController.IsDeathMatchMode || (this.dropOffDelay -= Time.deltaTime) < 0f))
				{
					this.ReleaseBros();
					this.Leave();
				}
				if (GameModeController.IsDeathMatchMode && Mathf.Abs(num5) < 180f)
				{
					if (Mathf.Abs(num5) < 16f && this.containedBros.Count > 0)
					{
						this.yI *= 1f - this.t * 3f;
					}
					if (Mathf.Abs(num5) < 32f)
					{
						if (this.deathmatchFlybyDirection == DirectionEnum.Right)
						{
							if (this.targetX < SortOfFollow.maxX - 80f)
							{
								this.targetX = this.x + 48f;
							}
						}
						else if (this.targetX > SortOfFollow.minX + 80f)
						{
							this.targetX = this.x - 48f;
						}
					}
					int num6 = -1;
					for (int j = 0; j < this.containedBros.Count; j++)
					{
						if ((SortOfFollow.IsItSortOfVisible(this.containedBros[j].transform.position, -4f, -4f) || (Mathf.Abs(num5) < 32f && Mathf.Abs(num4) < 32f)) && (InputReader.IsControllerPressingJump(HeroController.players[this.containedBros[j].playerNum].controllerNum) || InputReader.IsControllerPressingFire(HeroController.players[this.containedBros[j].playerNum].controllerNum)))
						{
							num6 = this.containedBros[j].playerNum;
						}
					}
					if (num6 != -1)
					{
						this.ReleaseBro(num6);
						if (this.containedBros.Count == 0)
						{
							this.Leave();
						}
					}
				}
			}
			else if (this.state == Helicopter.State.TakingOff)
			{
				float num7 = this.targetX - this.x;
				float num8 = this.targetY - this.y;
				this.xI += num7 * this.t * this.movementM;
				this.yI += num8 * this.t * this.movementM;
				this.xI *= 1f - this.t * 6f;
				this.yI *= 1f - this.t * 6f;
				if (this.movementM < 10f)
				{
					this.movementM += this.t;
				}
				if (this.DroppingHeroesOff && (GameModeController.GameMode == GameMode.DeathMatch || GameModeController.GameMode == GameMode.TeamDeathMatch) && !SortOfFollow.IsItSortOfVisible(this.x, this.y - 48f, 12f, 0f))
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
				else if (Mathf.Abs(num7) < 100f && Mathf.Abs(num8) < 100f)
				{
					if (this.DroppingHeroesOff)
					{
						UnityEngine.Object.Destroy(base.gameObject);
					}
					else
					{
						this.state = Helicopter.State.FlyingStraight;
					}
				}
				if (GameModeController.IsLevelFinished())
				{
					if (Sound.IsVictoryStingPlaying())
					{
						this.audioSource.volume *= 1f - this.t;
					}
					else
					{
						this.audioSource.volume = Mathf.Lerp(this.audioSource.volume, this.helicopterVolume, this.t * 2f);
					}
				}
			}
			else if (this.state == Helicopter.State.FlyingStraight)
			{
				this.yI = Mathf.Lerp(this.yI, Mathf.Sin(Time.time) * 30f, this.t);
				this.xI = Mathf.Lerp(this.xI, 350f, this.t);
			}
			this.x += this.xI * this.t;
			this.y += this.yI * this.t;
			this.SetPosition();
		}
	}

	protected void SetBrosPositions()
	{
		for (int i = 0; i < this.containedBros.Count; i++)
		{
			this.containedBros[i].x = this.containedBros[i].transform.position.x;
			this.containedBros[i].y = this.containedBros[i].transform.position.y;
			this.containedBros[i].collumn = Map.GetRow(this.containedBros[i].x);
			this.containedBros[i].row = Map.GetRow(this.containedBros[i].y);
		}
	}

	public void Death()
	{
		EffectsController.CreateExplosion(this.x + 24f, this.y + 24f, 24f, 16f, 120f, 1f, 24f, 1f, 0.7f, false);
		EffectsController.CreateExplosion(this.x + 24f, this.y + 24f, 24f, 16f, 120f, 1f, 24f, 1f, 0.7f, false);
		EffectsController.CreateExplosion(this.x - 24f, this.y + 24f, 24f, 16f, 120f, 2f, 24f, 1f, 0.3f, false);
		EffectsController.CreateExplosion(this.x - 24f, this.y + 24f, 24f, 16f, 120f, 2f, 24f, 1f, 0.3f, false);
		MapController.DamageGround(this, 15, DamageType.Explosion, 80f, this.x, this.y + 24f, null);
		Map.ExplodeUnits(this, 15, DamageType.Explosion, 128f, 48f, this.x, this.y + 24f, 200f, 10f, -1, false, false);
		Map.ShakeTrees(this.x, this.y, 256f, 64f, 128f);
		EffectsController.CreateGibs(this.helicopterGibs, this.helicopterGibs.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial, this.x, this.y, 100f, 100f, 0f, 100f);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void Damage(DamageObject damageObject)
	{
		this.health -= damageObject.damage;
		if (this.health <= 0 && this.state != Helicopter.State.Crashing)
		{
			if (Connect.IsHost && !this.DroppingHeroesOff)
			{
				Networking.InstantiateBuffered<GameObject>(Map.Instance.activeTheme.helicopterPrefab.gameObject, new object[]
				{
					true
				}, false);
				Networking.RPC<Vector2, float>(PID.TargetAll, new RpcSignature<Vector2, float>(Map.newestHelicopter.Enter), this.pickupTarget, 4f, false);
			}
			this.state = Helicopter.State.Crashing;
			this.ladderVictoryCollider.enabled = false;
			foreach (TestVanDammeAnim hero in this.attachedHeroes)
			{
				HeroController.DetachHeroFromHelicopter(hero);
			}
			this.ReleaseBros();
			if (GameModeController.LevelFinished)
			{
				SortOfFollow.instance.followMode = CameraFollowMode.PanUpward;
			}
			this.StartCrashAnim();
		}
	}

	public void AddBroToTransport(TestVanDammeAnim bro)
	{
		if (!this.containedBros.Contains(bro))
		{
			this.containedBros.Add(bro);
			bro.enabled = false;
			bro.invulnerable = true;
			bro.transform.parent = base.transform;
			if (!GameModeController.IsDeathMatchMode)
			{
				bro.transform.localPosition = new Vector3(2f, (float)UnityEngine.Random.Range(-60, -25), 0f);
				bro.SetPosition(bro.transform.position);
				MonoBehaviour.print(bro + "   " + bro.transform.position);
			}
			else
			{
				bro.transform.localPosition = new Vector3(2f, -40f, 0f);
				this.lowestLadderPart.gameObject.SetActive(false);
				this.lowestLadderPart = this.secondLowestLadderPart;
				this.SetDeathmatchContainedBrosPositions();
			}
		}
	}

	protected void ReleaseBros()
	{
		HeroController.Instance.brosHaveBeenReleased = true;
		this.hasReleasedHeros = true;
		for (int i = 0; i < this.containedBros.Count; i++)
		{
			this.containedBros[i].enabled = true;
			this.containedBros[i].transform.parent = null;
			this.containedBros[i].x = this.containedBros[i].transform.position.x;
			this.containedBros[i].y = this.containedBros[i].transform.position.y;
			this.containedBros[i].SetPosition();
			this.containedBros[i].xI = this.xI + (float)UnityEngine.Random.Range(0, 50);
			this.containedBros[i].yI = this.yI;
			this.containedBros[i].actionState = ActionState.Jumping;
			if (GameModeController.IsDeathMatchMode)
			{
				this.containedBros[i].SetInvulnerable(0.2f, true);
			}
			else
			{
				this.containedBros[i].SetInvulnerable(2.5f, true);
			}
		}
		this.containedBros.Clear();
	}

	public void ReleaseBro(int playernum)
	{
		for (int i = 0; i < this.containedBros.Count; i++)
		{
			if (this.containedBros[i].playerNum == playernum)
			{
				this.containedBros[i].enabled = true;
				this.containedBros[i].transform.parent = null;
				this.containedBros[i].x = this.containedBros[i].transform.position.x;
				this.containedBros[i].y = this.containedBros[i].transform.position.y;
				this.containedBros[i].SetPosition();
				this.containedBros[i].xI = this.xI + (float)UnityEngine.Random.Range(0, 50);
				this.containedBros[i].yI = this.containedBros[i].jumpForce;
				this.containedBros[i].actionState = ActionState.Jumping;
				if (!GameModeController.IsDeathMatchMode)
				{
					this.containedBros[i].SetInvulnerable(0.2f, true);
				}
				else
				{
					this.containedBros[i].SetInvulnerable(0.06f, true);
				}
			}
		}
		this.containedBros.Remove(this.containedBros.First((TestVanDammeAnim b) => b.playerNum == playernum));
		if (this.containedBros.Count == 0)
		{
			this.hasReleasedHeros = true;
		}
	}

	internal static void CreateDropoffInstance()
	{
		if (Helicopter.DropOffHeliInstance != null)
		{
			UnityEngine.Object.Destroy(Helicopter.DropOffHeliInstance.gameObject);
		}
		Helicopter.DropOffHeliInstance = (UnityEngine.Object.Instantiate(Map.Instance.activeTheme.helicopterPrefab) as Helicopter);
		NID nid = Registry.AllocatePlayerUniqueID();
		Registry.RegisterGameObject(ref nid, Helicopter.DropOffHeliInstance.gameObject);
	}

	private const int faceRightFrame = 0;

	private const int faceLeftFrame = 6;

	public static Helicopter DropOffHeliInstance;

	protected float targetX;

	protected float targetY;

	public Vector2 pickupTarget;

	protected bool arriving = true;

	protected float t = 0.01f;

	protected float sinCounter;

	protected float movementM = 10f;

	protected AudioSource audioSource;

	public AudioClip helicopterSound;

	protected float dustCounter;

	public float leaveMovementM = 2f;

	protected float crashTime;

	public Collider ladderVictoryCollider;

	protected float arriveDelay;

	public GameObject[] disableObjectsOnDeath;

	public List<TestVanDammeAnim> attachedHeroes = new List<TestVanDammeAnim>();

	public bool hasBeenCalledDown;

	public GameObject lowestLadderPart;

	public GameObject secondLowestLadderPart;

	public GibHolder helicopterGibs;

	protected List<TestVanDammeAnim> containedBros = new List<TestVanDammeAnim>();

	public bool hasReleasedHeros;

	private Helicopter.State state;

	public SpriteSM sideRotor;

	protected AnimatedTexture ani;

	protected float smokeCounter;

	public bool DroppingHeroesOff;

	public Transform ladderHolder;

	private int idleFrame;

	private int prevFrame;

	private int lastFrame;

	protected float helicopterVolume = 0.6f;

	private bool IsEndLevelHeli;

	private bool isTurning;

	private DirectionEnum deathmatchFlybyDirection = DirectionEnum.Right;

	private float dropOffDelay = 0.5f;

	private enum State
	{
		Landing,
		TakingOff,
		FlyingStraight,
		Crashing
	}
}
