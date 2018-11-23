// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class LevelTitle : MonoBehaviour
{
	public static void ShowText(string s, float delay)
	{
		if (LevelTitle.instance != null)
		{
			LevelTitle.instance.titleCounter = 0f;
			LevelTitle.instance.text.text = s;
			LevelTitle.instance.delay = delay;
			LevelTitle.instance.SetOffscreen();
			LevelTitle.instance.gameObject.SetActive(true);
		}
		else
		{
			UnityEngine.Debug.LogError("No title text");
		}
	}

	public static float GetOffset()
	{
		if (LevelTitle.instance == null)
		{
			return 0f;
		}
		if (LevelTitle.instance.gameObject.activeSelf)
		{
			return LevelTitle.offset;
		}
		return 0f;
	}

	public static bool IsMoving()
	{
		return LevelTitle.isChanging;
	}

	private void Awake()
	{
		LevelTitle.instance = this;
	}

	private void Start()
	{
		if (Map.MapData == null)
		{
			LevelTitle.instance.gameObject.SetActive(false);
		}
		else if (Map.MapData.levelDescription != null && Map.MapData.levelDescription.Length > 0)
		{
			LevelTitle.instance.gameObject.SetActive(false);
		}
		else
		{
			LevelTitle.instance.gameObject.SetActive(false);
		}
	}

	private void SetOffscreen()
	{
		if (this.UICamera == null)
		{
			this.UICamera = Camera.main;
		}
		base.transform.localPosition = new Vector3(0f, -this.UICamera.orthographicSize - 24f, 5f);
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		if (this.delay > 0f)
		{
			this.delay -= deltaTime;
		}
		else
		{
			this.titleCounter += deltaTime;
			if (this.titleCounter < 0.5f)
			{
				LevelTitle.offset = 24f * this.titleCounter * 2f;
				base.transform.localPosition = new Vector3(0f, -this.UICamera.orthographicSize + LevelTitle.offset, 5f);
				LevelTitle.isChanging = true;
			}
			else if (this.titleCounter < 5f)
			{
				LevelTitle.offset = 24f;
				base.transform.localPosition = new Vector3(0f, -this.UICamera.orthographicSize + LevelTitle.offset, 5f);
				LevelTitle.isChanging = true;
			}
			else if (this.titleCounter < 5.5f)
			{
				LevelTitle.offset = 24f * (5.5f - this.titleCounter) * 2f;
				base.transform.localPosition = new Vector3(0f, -this.UICamera.orthographicSize + LevelTitle.offset, 5f);
				LevelTitle.isChanging = true;
			}
			else if (LevelTitle.isChanging)
			{
				LevelTitle.isChanging = false;
			}
			else
			{
				LevelTitle.instance.gameObject.SetActive(false);
			}
		}
	}

	private const float offscreenOffset = 24f;

	public TextMesh text;

	public Camera UICamera;

	protected float titleCounter;

	protected float delay;

	protected static float offset;

	protected static bool isChanging;

	public float letterBoxM = 1f;

	protected static LevelTitle instance;
}
