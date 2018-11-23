// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class LetterBoxController : MonoBehaviour
{
	public static bool IsShowingLetterBox()
	{
		return LetterBoxController.instance != null && LetterBoxController.instance.letterBoxAmount > 0f;
	}

	public static void ShowLetterBox(float amount, float tweenTime)
	{
		if (LetterBoxController.instance != null)
		{
			LetterBoxController.instance.ShowLetterBoxInternal(amount, tweenTime);
		}
	}

	protected void ShowLetterBoxInternal(float amount, float tweenTime)
	{
		this.letterBoxBottom.gameObject.SetActive(true);
		this.letterBoxTop.gameObject.SetActive(true);
		this.tweenStartTime = Time.time;
		this.tweenCounter = 0f;
		this.tweening = true;
		this.startLetterBoxAmount = this.letterBoxAmount;
		this.letterBoxAmount = amount;
		this.tweenTime = tweenTime;
		if (tweenTime <= 0f)
		{
			this.FinishTween();
		}
	}

	public static void ClearLetterBox(float tweenTime)
	{
		if (LetterBoxController.instance != null)
		{
			LetterBoxController.instance.ClearLetterBoxInternal(tweenTime);
		}
	}

	protected void ClearLetterBoxInternal(float tweenTime)
	{
		if (this.letterBoxAmount > 0f)
		{
			this.tweenStartTime = Time.time;
			this.tweenCounter = 0f;
			this.tweening = true;
			this.startLetterBoxAmount = this.letterBoxAmount;
			this.letterBoxAmount = 0f;
			this.tweenTime = tweenTime;
		}
		this.tweening = true;
	}

	protected void Awake()
	{
		LetterBoxController.instance = this;
	}

	private void Start()
	{
		this.letterBoxBottom.gameObject.SetActive(false);
		this.letterBoxTop.gameObject.SetActive(false);
		this.SetLetterBoxPosition(0f);
	}

	protected void FinishTween()
	{
		if (this.letterBoxAmount <= 0f)
		{
			this.letterBoxAmount = 0f;
			this.letterBoxBottom.gameObject.SetActive(false);
			this.letterBoxTop.gameObject.SetActive(false);
			this.SetLetterBoxPosition(0f);
		}
		else
		{
			this.SetLetterBoxPosition(this.fullLetterBoxWidth * this.letterBoxAmount);
		}
		this.tweening = false;
	}

	protected void SetLetterBoxPosition(float position)
	{
		this.letterBoxTop.transform.localPosition = new Vector3(0f, -position, 0f);
		this.letterBoxBottom.transform.localPosition = new Vector3(0f, position, 0f);
	}

	private void Update()
	{
		if (this.tweening)
		{
			float num = Time.time - this.tweenStartTime;
			if (this.tweenTime <= 0f || num > this.tweenTime)
			{
				this.FinishTween();
			}
			else
			{
				float num2 = Mathf.Clamp(num / this.tweenTime, 0f, 1f);
				float num3 = this.startLetterBoxAmount * (1f - num2) + this.letterBoxAmount * num2;
				this.SetLetterBoxPosition(this.fullLetterBoxWidth * num3);
			}
		}
	}

	public Transform letterBoxBottom;

	public Transform letterBoxTop;

	protected float letterBoxAmount;

	protected float startLetterBoxAmount;

	protected bool tweening;

	protected float tweenStartTime;

	protected float tweenCounter;

	protected float tweenTime = 1f;

	public float fullLetterBoxWidth = 32f;

	protected static LetterBoxController instance;
}
