// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
	public static bool Paused
	{
		get
		{
			return PauseController.pauseStatus != PauseStatus.UnPaused;
		}
	}

	private void Awake()
	{
		PauseController.instance = this;
		PauseController.pauseStatus = PauseStatus.UnPaused;
	}

	private void Update()
	{
		if (InputReader.GetKeyDown(KeyCode.Pause) || InputReader.GetKeyDown(KeyCode.Escape) || InputReader.GetControllerPressingStart() >= 0)
		{
			PauseController.pausedByController = InputReader.GetControllerPressingStart();
			MonoBehaviour.print("TOGGLE PAUSE: " + InputReader.GetControllerPressingStart());
			this.TogglePause();
		}
		if (PauseController.pauseStatus == PauseStatus.MenuPause)
		{
			this.AnimatePauseScreenshot();
		}
	}

	private static void PauseGameObjects()
	{
		PauseController.pausedObjects = new List<GameObject>();
		PauseController.pausedComponents = new List<MonoBehaviour>();
		StatisticsController.NotifyPause(true);
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject gameObject in array)
		{
			if (gameObject.activeInHierarchy && !gameObject.CompareTag("Unpausable"))
			{
				if (gameObject.GetComponent<PauseComponent>() != null)
				{
					gameObject.GetComponent<PauseComponent>().NotifyPause(true);
				}
				else
				{
					gameObject.SetActive(false);
				}
				PauseController.pausedObjects.Add(gameObject);
			}
		}
		PauseController.instance.gameObject.SetActive(true);
	}

	public static void SetIntermissionPause(bool pause)
	{
		if (pause)
		{
			PauseController.PauseGameObjects();
		}
		else
		{
			PauseController.UnpauseGameObjects();
		}
	}

	private static void UnpauseGameObjects()
	{
		StatisticsController.NotifyPause(false);
		foreach (GameObject gameObject in PauseController.pausedObjects)
		{
			if (gameObject != null)
			{
				if (gameObject.GetComponent<PauseComponent>() != null)
				{
					gameObject.GetComponent<PauseComponent>().NotifyPause(false);
					if (gameObject.GetComponent<PauseComponent>().PauseOnMenu)
					{
						gameObject.SetActive(false);
					}
				}
				else
				{
					gameObject.SetActive(true);
				}
			}
		}
		PauseController.pausedComponents.Clear();
		PauseController.pausedObjects.Clear();
	}

	public void TogglePause()
	{
		if (PauseController.pauseStatus == PauseStatus.UnPaused && PauseController.PauseIsAllowed())
		{
			PauseController.PauseGameObjects();
			PauseController.pauseStatus = PauseStatus.MenuPause;
			this.pauseCam.gameObject.SetActive(true);
		}
		else if (PauseController.pauseStatus == PauseStatus.MenuPause)
		{
			PauseController.UnpauseGameObjects();
			PauseController.pauseStatus = PauseStatus.UnPaused;
			this.pauseCam.gameObject.SetActive(false);
			PauseController.pausedByController = -1;
		}
	}

	public static bool PauseIsAllowed()
	{
		return !GameModeController.IsLevelFinished() && !CutsceneController.isInCutscene;
	}

	private static Texture2D TakePauseScreenShot()
	{
		Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
		RenderTexture.active = renderTexture;
		if (PauseController.instance.parralaxCam != null)
		{
			PauseController.instance.parralaxCam.targetTexture = renderTexture;
			PauseController.instance.parralaxCam.Render();
			PauseController.instance.parralaxCam.targetTexture = null;
		}
		else
		{
			UnityEngine.Debug.LogWarning("This should probably be set");
		}
		if (PauseController.instance.backgroundCam != null)
		{
			PauseController.instance.backgroundCam.targetTexture = renderTexture;
			PauseController.instance.backgroundCam.Render();
			PauseController.instance.backgroundCam.targetTexture = null;
		}
		Camera.main.targetTexture = renderTexture;
		Camera.main.Render();
		Camera.main.targetTexture = null;
		texture2D.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0);
		RenderTexture.active = null;
		UnityEngine.Object.Destroy(renderTexture);
		Color[] pixels = texture2D.GetPixels(0);
		for (int i = 0; i < pixels.Length; i++)
		{
			if (pixels[i].r > 0.3f && pixels[i].b < 0.1f)
			{
				if (pixels[i].g >= pixels[i].r)
				{
					pixels[i].g = pixels[i].r;
				}
				pixels[i].b = 0f;
			}
			else
			{
				pixels[i].r = pixels[i].grayscale / 2f;
				pixels[i].g = pixels[i].grayscale / 2f;
				pixels[i].b = pixels[i].grayscale / 2f;
			}
			pixels[i].a = 1f;
		}
		texture2D.SetPixels(pixels, 0);
		texture2D.Apply();
		return texture2D;
	}

	public static void SetPause(PauseStatus newPauseStatus)
	{
		if (newPauseStatus != PauseController.pauseStatus)
		{
			PauseController.instance.TogglePause();
		}
	}

	private void AnimatePauseScreenshot()
	{
		this.pauseTime += Time.deltaTime;
	}

	private void SetPauseScreenShot(Texture2D tex)
	{
		this.pauseSprite.GetComponent<Renderer>().material.mainTexture = tex;
		this.pauseSprite.transform.position = this.pauseCam.ScreenToWorldPoint(Vector3.zero + Vector3.forward * 100f);
		Vector3 vector = default(Vector3);
		vector = this.pauseCam.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0f)) - this.pauseCam.ScreenToWorldPoint(Vector3.zero);
		this.pauseSprite.SetSize(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
		this.pauseSprite.SetLowerLeftPixel(0f, (float)tex.height);
		this.pauseSprite.SetPixelDimensions(tex.width, tex.height);
		this.pauseSprite.RecalcTexture();
		this.pauseSprite.CalcUVs();
		this.pauseSprite.UpdateUVs();
		this.pauseSprite.gameObject.SetActive(true);
	}

	private static List<GameObject> pausedObjects;

	private static List<MonoBehaviour> pausedComponents;

	public static PauseStatus pauseStatus;

	public Camera pauseCam;

	public Camera parralaxCam;

	public Camera backgroundCam;

	public SpriteSM pauseSprite;

	public static int pausedByController = -1;

	private Texture2D pauseScreenShot;

	private static PauseController instance;

	private float lastPauseTime;

	private float pauseTime;
}
