// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Fader : MonoBehaviour
{
	private void Awake()
	{
		Fader.instance = this;
		base.transform.localPosition = new Vector3(0f, 0f, this.zDepth);
		base.GetComponent<Renderer>().material.mainTexture = this.fadeTexture;
	}

	private void Start()
	{
		if (Fader.nextScene == string.Empty)
		{
			Fader.nextScene = Application.loadedLevelName;
		}
		UnityEngine.Debug.Log("Fader Start   " + Fader.nextScene);
	}

	public static void FadeHorizontalSwipe(string nextSceneName)
	{
		Fader.nextScene = nextSceneName;
		if (Fader.instance != null)
		{
			Fader.instance.fading = false;
			Fader.instance.fadingBlack = false;
			Fader.instance.fadingHorizontalSwipe = true;
			Fader.instance.gameObject.SetActive(true);
			Fader.instance.horizontalSwipeTransform.gameObject.SetActive(true);
			Fader.instance.horizontalSwipeTransform.transform.position = Fader.instance.uiCamera.ScreenToWorldPoint(new Vector3(-1f, (float)(Screen.height / 2), 1f));
		}
	}

	public static bool FadeSolid()
	{
		if (Fader.instance != null)
		{
			Fader.instance.fading = false;
			Fader.instance.fadingBlack = true;
			Fader.instance.counter = 0f;
			Fader.instance.gameObject.SetActive(true);
			Fader.instance.transform.localScale = Vector3.one * 100f;
			Fader.instance.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0f, 0f, 0f, 0f));
			return true;
		}
		return false;
	}

	public static bool FadeSolid(float fadeSpeed)
	{
		if (Fader.instance != null)
		{
			Fader.instance.fadeSpeed = fadeSpeed;
		}
		return Fader.FadeSolid();
	}

	public static bool FadeSolid(float fadeSpeed, bool switchLevel)
	{
		Fader.instance.switchLevel = switchLevel;
		return Fader.FadeSolid(fadeSpeed);
	}

	public static void StopFading()
	{
		if (Fader.instance != null)
		{
			Fader.instance.fading = false;
			Fader.instance.fadingBlack = false;
			Fader.instance.gameObject.SetActive(false);
			Fader.instance.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0f, 0f, 0f, 0f));
			Fader.instance.counter = 0f;
		}
	}

	public static void Fade()
	{
		Fader.instance.fading = true;
		Fader.instance.counter = 0f;
		Fader.instance.gameObject.SetActive(true);
		Fader.instance.transform.localScale = Vector3.one * 100f;
		Fader.instance.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0f, 0f, 0f, 1f));
	}

	public static void Fade(float fadeSpeed)
	{
		Fader.instance.fadeSpeed = fadeSpeed;
		Fader.Fade();
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.033f);
		if (this.fading)
		{
			this.counter += num * this.fadeSpeed * 1.5f;
			if (base.transform.localScale.x >= 1.2f)
			{
				base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.one, Mathf.Clamp(this.counter * num, 0f, 1f));
				if (base.transform.localScale.x < 1.2f)
				{
					base.GetComponent<Renderer>().material.mainTexture = this.fadeTexture;
				}
			}
			else
			{
				Map.ExitLevel();
				if (this.switchLevel)
				{
					if (Fader.nextScene != "Quit")
					{
						if (Fader.nextScene.Length < 2)
						{
							UnityEngine.Debug.LogError("Load same level, no level name");
							Application.LoadLevel(Application.loadedLevel);
						}
						else
						{
							UnityEngine.Debug.LogError("Load  level " + Fader.nextScene);
							Application.LoadLevel(Fader.nextScene);
							if (Fader.nextNextScene.Length > 0)
							{
								Fader.nextScene = Fader.nextNextScene;
								Fader.nextNextScene = string.Empty;
							}
						}
					}
					else
					{
						UnityEngine.Debug.Log("Quit");
						Application.Quit();
					}
				}
			}
		}
		else if (this.fadingBlack)
		{
			if (this.delay > 0f)
			{
				this.delay -= num;
			}
			else
			{
				this.counter += num * this.fadeSpeed;
			}
			if (this.counter >= 1f)
			{
				if (this.switchLevel)
				{
					if (Fader.nextScene != "Quit")
					{
						Application.LoadLevel(Fader.nextScene);
					}
					else
					{
						UnityEngine.Debug.Log("Quit");
						Application.Quit();
					}
				}
				base.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0f, 0f, 0f, this.counter));
			}
			else
			{
				base.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0f, 0f, 0f, this.counter));
			}
		}
		else if (this.fadingHorizontalSwipe)
		{
			this.horizontalSwipeTransform.Translate(this.horizontalSwipeSpeed * num, 0f, 0f, Space.World);
			if (this.horizontalSwipeTransform.position.x > this.uiCamera.ScreenToWorldPoint(new Vector3((float)Screen.width, 0f, 1f)).x + 18f)
			{
				Application.LoadLevel(Fader.nextScene);
				UnityEngine.Debug.Log("Next Scene " + Fader.nextScene);
			}
		}
		else
		{
			if (this.delay > 0f)
			{
				this.delay -= num;
			}
			else
			{
				this.counter += num * 3f;
			}
			if (this.counter >= 1f)
			{
				base.gameObject.SetActive(false);
				base.GetComponent<Renderer>().material.mainTexture = this.circleTexture;
			}
			else
			{
				base.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0f, 0f, 0f, 1f - this.counter));
			}
		}
	}

	public Texture fadeTexture;

	public Texture circleTexture;

	protected float counter;

	public static Fader instance;

	public static string nextScene = string.Empty;

	public static string nextNextScene = string.Empty;

	public bool fading;

	public bool fadingBlack;

	public bool fadingHorizontalSwipe;

	public float delay;

	protected bool switchLevel = true;

	public float zDepth = 28f;

	public Transform horizontalSwipeTransform;

	public float horizontalSwipeSpeed = 2000f;

	public Camera uiCamera;

	protected float fadeSpeed = 2f;
}
