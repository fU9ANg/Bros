// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Brotalitometer : MonoBehaviour
{
	private void Awake()
	{
		Brotalitometer.instance = this;
	}

	public static void Reset()
	{
		if (Brotalitometer.instance != null)
		{
			if (GameModeController.LevelFinished || Map.isEditing || !StatisticsController.ShowBrotalityScore())
			{
				Brotalitometer.instance.gameObject.SetActive(false);
			}
			else
			{
				Brotalitometer.instance.gameObject.SetActive(true);
			}
		}
	}

	private void Start()
	{
		if (Map.isEditing || !StatisticsController.ShowBrotalityScore())
		{
			base.gameObject.SetActive(false);
		}
		Vector3 vector = this.uiCamera.ScreenToWorldPoint(new Vector3((float)Screen.width * this.padding, 0f, 6f));
		this.width = this.uiCamera.ScreenToWorldPoint(new Vector3((float)Screen.width - (float)Screen.width * this.padding, (float)Screen.height, 6f)).x - vector.x;
		Vector3 vector2 = this.uiCamera.ScreenToWorldPoint(new Vector3((float)Screen.width * this.padding, (float)Screen.height * this.bootomPadding, 25f));
		this.backgroundBar.transform.position = vector2;
		this.backgroundBar.SetSize(this.width, 5f);
		this.highlightBar.SetSize(this.width + 2f, 7f);
		this.highlightBar.SetColor(new Color(1f, 1f, 1f, 0f));
		this.highlightBar.transform.position = vector2 + Vector3.forward - Vector3.right;
		this.firstBar.Setup(vector2 - Vector3.forward, this.width);
		this.secondBar.Setup(vector2 - Vector3.forward * 2f, this.width);
		this.thirdBar.Setup(vector2 - Vector3.forward * 3f, this.width);
		this.fourthBar.Setup(vector2 - Vector3.forward * 4f, this.width);
		this.fifthBar.Setup(vector2 - Vector3.forward * 5f, this.width);
	}

	protected float GetFirstBarDegree(float brotality)
	{
		return Mathf.Clamp01(brotality / 12f);
	}

	protected float GetSecondBarDegree(float brotality)
	{
		return Mathf.Clamp01((brotality - 12f) / 25f);
	}

	protected float GetThirdBarDegree(float brotality)
	{
		return Mathf.Clamp01((brotality - 12f - 25f) / 50f);
	}

	protected float GetFourthBarDegree(float brotality)
	{
		return Mathf.Clamp01((brotality - 12f - 25f - 50f) / 75f);
	}

	protected float GetFifthBarDegree(float brotality)
	{
		return Mathf.Clamp01((brotality - 12f - 25f - 50f - 75f) / 200f);
	}

	private void Update()
	{
		float brotalometerValue = StatisticsController.GetBrotalometerValue();
		if (this.lastBrotalityValue + 0.5f < brotalometerValue)
		{
			this.decreasing = false;
			this.hilightM = 1f;
			this.redM = 0f;
		}
		else if (this.lastBrotalityValue > brotalometerValue)
		{
			if (!this.decreasing)
			{
				this.hilightM = 1f;
				this.redM = 1f;
				this.decreasing = true;
			}
			else
			{
				this.redM *= 1f - Time.deltaTime * 8f;
				this.hilightM *= 1f - Time.deltaTime * 8f;
			}
		}
		else
		{
			this.decreasing = false;
			this.redM *= 1f - Time.deltaTime * 15f;
			this.hilightM *= 1f - Time.deltaTime * 15f;
		}
		this.highlightBar.SetColor(new Color(1f, 1f - this.redM, 1f - this.redM, this.hilightM));
		this.firstBar.SetSize(this.GetFirstBarDegree(brotalometerValue), this.GetSecondBarDegree(brotalometerValue));
		this.secondBar.SetSize(this.GetSecondBarDegree(brotalometerValue), this.GetThirdBarDegree(brotalometerValue));
		this.thirdBar.SetSize(this.GetThirdBarDegree(brotalometerValue), this.GetFourthBarDegree(brotalometerValue));
		this.fourthBar.SetSize(this.GetFourthBarDegree(brotalometerValue), this.GetFifthBarDegree(brotalometerValue));
		this.fifthBar.SetSize(this.GetFifthBarDegree(brotalometerValue), 0f);
		if (brotalometerValue <= 12f)
		{
			this.fist.transform.position = new Vector3(this.backgroundBar.transform.position.x + this.width * this.GetFirstBarDegree(brotalometerValue), this.backgroundBar.transform.position.y, this.backgroundBar.transform.position.z - 6f);
		}
		else if (brotalometerValue <= 37f)
		{
			this.fist.transform.position = new Vector3(this.backgroundBar.transform.position.x + this.width * this.GetSecondBarDegree(brotalometerValue), this.backgroundBar.transform.position.y, this.backgroundBar.transform.position.z - 6f);
		}
		else if (brotalometerValue <= 87f)
		{
			this.fist.transform.position = new Vector3(this.backgroundBar.transform.position.x + this.width * this.GetThirdBarDegree(brotalometerValue), this.backgroundBar.transform.position.y, this.backgroundBar.transform.position.z - 6f);
		}
		else if (brotalometerValue <= 162f)
		{
			this.fist.transform.position = new Vector3(this.backgroundBar.transform.position.x + this.width * this.GetFourthBarDegree(brotalometerValue), this.backgroundBar.transform.position.y, this.backgroundBar.transform.position.z - 6f);
		}
		else if (brotalometerValue <= 362f)
		{
			this.fist.transform.position = new Vector3(this.backgroundBar.transform.position.x + this.width * this.GetFifthBarDegree(brotalometerValue), this.backgroundBar.transform.position.y, this.backgroundBar.transform.position.z - 6f);
		}
		else
		{
			this.fist.transform.position = new Vector3(this.backgroundBar.transform.position.x + this.width * this.GetFifthBarDegree(brotalometerValue), this.backgroundBar.transform.position.y, this.backgroundBar.transform.position.z - 6f);
		}
		this.lastBrotalityValue = brotalometerValue;
	}

	public BrotalityBar firstBar;

	public SpriteSM backgroundBar;

	public SpriteSM highlightBar;

	public BrotalityBar secondBar;

	public BrotalityBar thirdBar;

	public BrotalityBar fourthBar;

	public BrotalityBar fifthBar;

	public Transform fist;

	public Camera uiCamera;

	protected float width = 200f;

	public float padding = 0.13f;

	public float bootomPadding = 0.05f;

	protected float lastBrotalityValue;

	protected float hilightM;

	protected float redM;

	protected bool decreasing;

	protected static Brotalitometer instance;
}
