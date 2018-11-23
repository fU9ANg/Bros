// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CutsceneCharacterController : SingletonMono<CutsceneCharacterController>
{
	public static void PlayedToVictory()
	{
		CutsceneCharacterController.hasHadVictory = true;
		PlayerPrefs.SetInt("HasPlayedToVictory", 1);
	}

	public static bool HasSeenVictory()
	{
		return CutsceneCharacterController.hasSeenVictory;
	}

	private void Awake()
	{
		CutsceneCharacterController.hasSeenVictory = true;
		CutsceneCharacterController.hasHadVictory = (PlayerPrefs.GetInt("HasPlayedToVictory", -1) >= 0);
		if (!CutsceneCharacterController.hasHadVictory)
		{
			this.rambro.transform.localPosition = new Vector3(1f, 12f, 0f);
			this.satan.transform.localPosition = new Vector3(-260f, 12f, 0f);
			TestVanDammeAnim component = this.satan.GetComponent<TestVanDammeAnim>();
			if (component != null)
			{
				component.speed = 130f;
			}
			this.confetti.SetActive(false);
		}
	}

	private void Start()
	{
		if (CutsceneCharacterController.hasHadVictory)
		{
			this.satan.Special();
		}
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.firstFrame)
		{
			this.firstFrame = false;
			this.satan.gameObject.SetActive(true);
			this.rambro.gameObject.SetActive(true);
			this.bromando.gameObject.SetActive(true);
			this.coljamesbroddock.gameObject.SetActive(true);
			this.brohard.gameObject.SetActive(true);
			this.brodredd.gameObject.SetActive(true);
			this.mcbrover.gameObject.SetActive(true);
			this.brade.gameObject.SetActive(true);
			this.broinblack.gameObject.SetActive(true);
			this.snakebroskin.gameObject.SetActive(true);
			this.brobocop.gameObject.SetActive(true);
			this.mookCaptain.gameObject.SetActive(true);
			this.mookCaptain.TurnAround_Networked(this.mookCaptain.transform.position.x, this.mookCaptain.transform.position.y, -1);
			this.mookCaptain.Special(100f);
		}
		if (CutsceneCharacterController.hasHadVictory)
		{
			if (this.delayStart > 0f)
			{
				this.delayStart -= num;
				if (this.delayStart <= 0f)
				{
					this.satan.Exclaim(-1);
				}
			}
			else if (this.delayRun > 0f)
			{
				this.delayRun -= num;
				if (this.delayRun <= 0f)
				{
					this.satan.PanicRun(1, Camera.main.transform.position.x + 300f);
					this.StartBrosRunning(this.currentDirection);
					this.SetupNextWave();
				}
			}
			else
			{
				if (this.newRunCounter > 0f)
				{
					this.newRunCounter -= num;
					if (this.newRunCounter <= 0f)
					{
						this.satan.PanicRun(this.currentDirection, Camera.main.transform.position.x + (float)(300 * this.currentDirection), SetResolutionCamera.GetXEdge(-this.currentDirection) - (float)(this.currentDirection * 16));
					}
				}
				if (this.newRunAlliesCounter > 0f)
				{
					this.newRunAlliesCounter -= num;
					if (this.newRunAlliesCounter <= 0f)
					{
						this.StartBrosRunning(this.currentDirection);
						this.SetupNextWave();
					}
				}
				if (this.actionCounter < 1f)
				{
					this.actionCounter += num;
				}
				else
				{
					this.actionCounter -= 0.23f + UnityEngine.Random.value * 0.3f;
					this.DoRandomAction();
				}
			}
		}
		else if (this.delayStart > 1.4f)
		{
			this.delayStart -= num;
			if (this.delayStart <= 1.4f)
			{
				this.rambro.Exclaim(-1);
			}
		}
		else if (this.delayRun > 0f)
		{
			this.delayRun -= num;
			if (this.delayRun <= 0f)
			{
				this.rambro.PanicRun(1, Camera.main.transform.position.x + 300f);
				this.satan.Walk(this.currentDirection, Camera.main.transform.position.x + (float)(300 * this.currentDirection), SetResolutionCamera.GetXEdge(-this.currentDirection) - (float)(this.currentDirection * 16));
				this.SetupNextWave();
			}
		}
		else
		{
			if (this.newRunCounter > 1f)
			{
				this.newRunCounter -= num;
				if (this.newRunCounter <= 1f)
				{
					this.rambro.PanicRun(this.currentDirection, Camera.main.transform.position.x + (float)(300 * this.currentDirection), SetResolutionCamera.GetXEdge(-this.currentDirection) - (float)(this.currentDirection * 16));
				}
			}
			if (this.newRunAlliesCounter > 2f)
			{
				this.newRunAlliesCounter -= num;
				if (this.newRunAlliesCounter <= 2f)
				{
					this.satan.Walk(this.currentDirection, Camera.main.transform.position.x + (float)(300 * this.currentDirection), SetResolutionCamera.GetXEdge(-this.currentDirection) - (float)(this.currentDirection * 16));
					this.SetupNextWave();
				}
			}
		}
	}

	protected void SetupNextWave()
	{
		this.newRunCounter = 6f;
		this.newRunAlliesCounter = 9f;
		this.currentDirection *= -1;
		this.actionCounter = -0.5f;
	}

	protected void StartBrosRunning(int direction)
	{
		this.rambro.Walk(direction, Camera.main.transform.position.x + 300f, SetResolutionCamera.GetXEdge(-direction) - (float)(direction * 16));
		this.bromando.Walk(direction, Camera.main.transform.position.x + 300f, SetResolutionCamera.GetXEdge(-direction) - (float)(direction * 32));
		this.coljamesbroddock.Walk(direction, Camera.main.transform.position.x + 300f, SetResolutionCamera.GetXEdge(-direction) - (float)(direction * 64));
		this.brohard.Walk(direction, Camera.main.transform.position.x + 300f, SetResolutionCamera.GetXEdge(-direction) - (float)(direction * 80));
		this.brobocop.Walk(direction, Camera.main.transform.position.x + 300f, SetResolutionCamera.GetXEdge(-direction) - (float)(direction * 192));
		this.broinblack.Walk(direction, Camera.main.transform.position.x + 300f, SetResolutionCamera.GetXEdge(-direction) - (float)(direction * 112));
		this.mcbrover.Walk(direction, Camera.main.transform.position.x + 300f, SetResolutionCamera.GetXEdge(-direction) - (float)(direction * 128));
		this.brade.Walk(direction, Camera.main.transform.position.x + 300f, SetResolutionCamera.GetXEdge(-direction) - (float)(direction * 144));
		this.brodredd.Walk(direction, Camera.main.transform.position.x + 300f, SetResolutionCamera.GetXEdge(-direction) - (float)(direction * 160));
		this.snakebroskin.Walk(direction, 300f, SetResolutionCamera.GetXEdge(-direction) - (float)(direction * 176));
	}

	protected void DoRandomAction()
	{
		switch (UnityEngine.Random.Range(0, 12))
		{
		case 0:
			this.DoRandomAction(this.rambro);
			break;
		case 1:
			this.DoRandomAction(this.rambro);
			break;
		case 2:
			this.DoRandomAction(this.bromando);
			break;
		case 3:
			this.DoRandomAction(this.babroracus);
			break;
		case 4:
			this.DoRandomAction(this.coljamesbroddock);
			break;
		case 5:
			this.DoRandomAction(this.brohard);
			break;
		case 6:
			this.DoRandomAction(this.brobocop);
			break;
		case 7:
			this.DoRandomAction(this.mcbrover);
			break;
		case 8:
			this.DoRandomAction(this.brade);
			break;
		case 9:
			this.DoRandomAction(this.brodredd);
			break;
		case 10:
			this.DoRandomAction(this.broinblack);
			break;
		default:
			this.DoRandomAction(this.snakebroskin);
			break;
		}
	}

	protected void DoRandomAction(CutsceneAI character)
	{
		if (UnityEngine.Random.value > 0.87f)
		{
			character.Jump();
		}
		else if (UnityEngine.Random.value > 0.88f)
		{
			character.Shoot();
		}
		else
		{
			character.Shoot();
			character.Jump();
		}
	}

	protected static bool hasHadVictory;

	protected static bool hasSeenVictory;

	public CutsceneAI satan;

	public CutsceneAI mookCaptain;

	public CutsceneAI rambro;

	public CutsceneAI bromando;

	public CutsceneAI babroracus;

	public CutsceneAI coljamesbroddock;

	public CutsceneAI brohard;

	public CutsceneAI mcbrover;

	public CutsceneAI brade;

	public CutsceneAI brodredd;

	public CutsceneAI broinblack;

	public CutsceneAI snakebroskin;

	public CutsceneAI brobocop;

	public GameObject confetti;

	private float delayStart = 3.3f;

	private float delayRun = 1f;

	private float newRunCounter;

	private float newRunAlliesCounter;

	protected int currentDirection = 1;

	protected float actionCounter;

	protected bool firstFrame = true;
}
