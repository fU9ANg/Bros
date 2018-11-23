// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FullScreenRightClick : MonoBehaviour
{
	private void Start()
	{
		this.windowHeight = Screen.height;
		this.windowWidth = Screen.width;
		this.buttonObject.SetActive(false);
		this.buttonObjectHighlight.SetActive(false);
		if (Screen.fullScreen)
		{
			this.fullscreenText.text = "EXIT FULLSCREEN";
			this.fullscreenTextHilight.text = "EXIT FULLSCREEN";
		}
		else
		{
			this.fullscreenText.text = "GO FULLSCREEN";
			this.fullscreenTextHilight.text = "GO FULLSCREEN";
		}
	}

	private void Update()
	{
		if (Application.isWebPlayer)
		{
			if (Input.GetMouseButtonDown(1))
			{
				UnityEngine.Debug.Log("Right Click");
				this.buttonObject.SetActive(true);
				this.buttonActive = true;
				base.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 3f));
			}
			if (this.buttonActive)
			{
				Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 3f));
				if (base.GetComponent<Collider>().bounds.Contains(point))
				{
					if (Input.GetMouseButtonDown(0))
					{
						this.ToggleFullscreen();
					}
					if (Screen.fullScreen)
					{
						this.fullscreenText.text = "EXIT FULLSCREEN";
						this.fullscreenTextHilight.text = "EXIT FULLSCREEN";
					}
					else
					{
						this.fullscreenText.text = "GO FULLSCREEN";
						this.fullscreenTextHilight.text = "GO FULLSCREEN";
					}
					this.buttonObject.SetActive(false);
					this.buttonObjectHighlight.SetActive(true);
				}
				else
				{
					this.buttonObject.SetActive(true);
					this.buttonObjectHighlight.SetActive(false);
				}
				if (Input.GetMouseButtonDown(0))
				{
					this.buttonObject.SetActive(false);
					this.buttonObjectHighlight.SetActive(false);
					this.buttonActive = false;
				}
			}
		}
	}

	private void ToggleFullscreen()
	{
		if (!Screen.fullScreen)
		{
			this.windowHeight = Screen.height;
			this.windowWidth = Screen.width;
			Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
		}
		else if (this.windowWidth * this.windowHeight > 0)
		{
			Screen.SetResolution(this.windowWidth, this.windowHeight, false);
		}
	}

	public GameObject buttonObject;

	public GameObject buttonObjectHighlight;

	protected bool buttonActive;

	public TextMesh fullscreenText;

	public TextMesh fullscreenTextHilight;

	protected int windowHeight;

	protected int windowWidth;
}
