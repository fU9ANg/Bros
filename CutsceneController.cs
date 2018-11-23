// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
	public static Vector3 GetPosition()
	{
		return CutsceneController.instance.transform.position;
	}

	public static bool HasInstance()
	{
		return CutsceneController.instance != null;
	}

	public static CutsceneController Instance
	{
		get
		{
			return CutsceneController.instance;
		}
	}

	public static bool PlayersCanMove()
	{
		return !CutsceneController.holdPlayersStill;
	}

	public static void HoldPlayersStill(bool hold)
	{
		CutsceneController.holdPlayersStill = hold;
	}

	public static float MinX
	{
		get
		{
			if (CutsceneController.instance != null)
			{
				return CutsceneController.instance.cutsceneCamera.ScreenToWorldPoint(Vector3.zero).x;
			}
			return Camera.main.ScreenToWorldPoint(Vector3.zero).x;
		}
	}

	public static float MaxX
	{
		get
		{
			if (CutsceneController.instance != null)
			{
				return CutsceneController.instance.cutsceneCamera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 1f)).x;
			}
			return Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 1f)).x;
		}
	}

	public static float MinY
	{
		get
		{
			if (CutsceneController.instance != null)
			{
				return CutsceneController.instance.cutsceneCamera.ScreenToWorldPoint(Vector3.zero).y;
			}
			return Camera.main.ScreenToWorldPoint(Vector3.zero).y;
		}
	}

	public static float MaxY
	{
		get
		{
			if (CutsceneController.instance != null)
			{
				return CutsceneController.instance.cutsceneCamera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 1f)).y;
			}
			return Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 1f)).y;
		}
	}

	public static void StartCutscene(CutsceneScreen[] cutscenes)
	{
		if (!(CutsceneController.instance != null))
		{
			UnityEngine.Debug.LogError("WTF, no cutscene controller");
		}
	}

	public static void LoadCutScene(string levelName)
	{
		MonoBehaviour.print("Loaded Cutscene  " + levelName);
		CutsceneController.instance.StartCoroutine(CutsceneController.instance.LoadCutSceneRoutine(levelName));
	}

	private IEnumerator LoadCutSceneRoutine(string levelName)
	{
		Renderer renderer = this.letterboxTop.GetComponent<Renderer>();
		bool enabled = false;
		this.letterboxBot.GetComponent<Renderer>().enabled = enabled;
		renderer.enabled = enabled;
		Application.LoadLevelAdditive(levelName);
		PauseController.SetIntermissionPause(true);
		yield return null;
		Cutscene cutscene = Cutscene.GetMostRecentlyLoadedCutscene();
		MonoBehaviour.print("Loaded Cutscene  " + cutscene);
		if (cutscene != null)
		{
			while (!cutscene.isFinished)
			{
				yield return null;
			}
			MonoBehaviour.print(" Destroy(cutscene.gameObject)");
			UnityEngine.Object.Destroy(cutscene.gameObject);
			PauseController.SetIntermissionPause(false);
		}
		Renderer renderer2 = this.letterboxTop.GetComponent<Renderer>();
		enabled = true;
		this.letterboxBot.GetComponent<Renderer>().enabled = enabled;
		renderer2.enabled = enabled;
		if (levelName == "0 PunchingFace")
		{
			CutsceneController.LoadCutScene("4 Final Cinematic 1");
		}
		else if (levelName == "4 Final Cinematic 1")
		{
			CutsceneController.LoadCutScene("5 Final Cinematic 2");
		}
		if (levelName == "6 Final Cinematic Heli Escape")
		{
			Application.LoadLevel("ExpendabrosVictory");
		}
		yield break;
	}

	protected virtual void Awake()
	{
		CutsceneController.instance = this;
		CutsceneController.holdPlayersStill = false;
	}

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
		if (this.activeCutsceneInfo != null)
		{
			this.screenTimeLeft -= Time.deltaTime;
			if (this.screenTimeLeft < 0f)
			{
				this.screenIndex++;
				if (this.screenIndex < this.activeCutsceneInfo.screens.Length)
				{
					this.SetupCurrentScreen();
				}
			}
		}
	}

	private void SetupCurrentScreen()
	{
		MonoBehaviour.print("Going to screen: " + this.screenIndex);
		if (this.screenIndex > this.activeCutsceneInfo.screens.Length)
		{
			UnityEngine.Debug.LogError("screen index out of range");
			return;
		}
		CutsceneScreen cutsceneScreen = this.activeCutsceneInfo.screens[this.screenIndex];
		if (this.currentForeground != null)
		{
			UnityEngine.Object.Destroy(this.currentForeground.gameObject);
		}
		if (this.currentBackground != null)
		{
			UnityEngine.Object.Destroy(this.currentBackground.gameObject);
		}
		this.currentForeground = (UnityEngine.Object.Instantiate(this.GetForegroundElementPrefab(cutsceneScreen.foreground)) as CutsceneForegroundElement);
		this.currentBackground = (UnityEngine.Object.Instantiate(this.GetBackgroundElementPrefab(cutsceneScreen.background)) as CutsceneBackgroundElement);
		this.screenTimeLeft = cutsceneScreen.displayTime;
	}

	private CutsceneForegroundElement GetForegroundElementPrefab(CutsceneForegroundType type)
	{
		foreach (CutsceneForegroundElement cutsceneForegroundElement in this.foregrounds)
		{
			if (cutsceneForegroundElement.type == type)
			{
				return cutsceneForegroundElement;
			}
		}
		UnityEngine.Debug.LogError("Cutscene foreground element " + type.ToString() + " not found");
		return this.foregrounds[0];
	}

	private CutsceneBackgroundElement GetBackgroundElementPrefab(CutsceneBackgroundType type)
	{
		foreach (CutsceneBackgroundElement cutsceneBackgroundElement in this.backgrounds)
		{
			if (cutsceneBackgroundElement.type == type)
			{
				return cutsceneBackgroundElement;
			}
		}
		UnityEngine.Debug.LogError("Cutscene background element " + type.ToString() + " not found");
		return this.backgrounds[0];
	}

	public void CreatePecShine(Vector3 pos)
	{
		UnityEngine.Object.Instantiate(this.pecShinePrefab, pos, Quaternion.identity);
	}

	public CutsceneBackgroundElement[] backgrounds;

	public CutsceneForegroundElement[] foregrounds;

	public SpriteSM letterboxBot;

	public SpriteSM letterboxTop;

	private CutsceneInfo activeCutsceneInfo;

	public ClashShing pecShinePrefab;

	public CutsceneInfo[] cutscenes;

	public TextMesh text;

	protected int screenIndex = -1;

	protected float screenTimer;

	public Camera cutsceneCamera;

	protected CutsceneScreen[] currentCutscenes;

	public static int cutsceneToPlay;

	public static string currentCutsceneString = "HELLO";

	public static float currentCutsceneTime = 4f;

	public static bool isInCutscene;

	public static bool holdPlayersStill;

	protected float cutsceneTime;

	protected static CutsceneController instance;

	private float screenTimeLeft;

	private CutsceneForegroundElement currentForeground;

	private CutsceneBackgroundElement currentBackground;
}
