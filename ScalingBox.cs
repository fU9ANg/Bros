// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[ExecuteInEditMode]
public class ScalingBox : MonoBehaviour
{
	private void Awake()
	{
	}

	public void Activate()
	{
		base.gameObject.SetActive(true);
		this.top.gameObject.SetActive(true);
		this.bot.gameObject.SetActive(true);
		this.left.gameObject.SetActive(true);
		this.right.gameObject.SetActive(true);
		this.botLeft.gameObject.SetActive(true);
		this.botRight.gameObject.SetActive(true);
		this.topLeft.gameObject.SetActive(true);
		this.topRight.gameObject.SetActive(true);
		if (this.middle != null)
		{
			this.middle.gameObject.SetActive(true);
		}
	}

	public void SetMinimumSize(Vector2 newSize)
	{
		this.minimumSize = newSize;
	}

	public void SetToMinimumSize()
	{
		this.lastSize = this.minimumSize;
		this.left.transform.localPosition = new Vector3(-this.minimumSize.x / 2f, 0f, 0f);
		this.right.transform.localPosition = new Vector3(this.minimumSize.x / 2f, 0f, 0f);
		this.top.transform.localPosition = new Vector3(0f, this.minimumSize.y / 2f, 0f);
		this.bot.transform.localPosition = new Vector3(0f, -this.minimumSize.y / 2f, 0f);
		this.topLeft.transform.localPosition = new Vector3(-this.minimumSize.x / 2f, this.minimumSize.y / 2f);
		this.topRight.transform.localPosition = new Vector3(this.minimumSize.x / 2f, this.minimumSize.y / 2f, 0f);
		this.botLeft.transform.localPosition = new Vector3(-this.minimumSize.x / 2f, -this.minimumSize.y / 2f);
		this.botRight.transform.localPosition = new Vector3(this.minimumSize.x / 2f, -this.minimumSize.y / 2f, 0f);
		this.top.SetSize(this.minimumSize.x - this.topLeft.width - this.topRight.width, this.top.height);
		this.bot.SetSize(this.minimumSize.x - this.botLeft.width - this.botRight.width, this.bot.height);
		this.left.SetSize(this.left.width, this.minimumSize.y - this.topLeft.height - this.botLeft.height);
		this.right.SetSize(this.right.width, this.minimumSize.y - this.topRight.width - this.botRight.height);
		if (this.middle != null)
		{
			this.middle.SetSize(this.minimumSize.x - 8f, this.minimumSize.y - 8f);
		}
	}

	public void SetToDesiredSize()
	{
		this.left.transform.localPosition = new Vector3(-this.desiredSize.x / 2f, 0f, 0f);
		this.right.transform.localPosition = new Vector3(this.desiredSize.x / 2f, 0f, 0f);
		this.top.transform.localPosition = new Vector3(0f, this.desiredSize.y / 2f, 0f);
		this.bot.transform.localPosition = new Vector3(0f, -this.desiredSize.y / 2f, 0f);
		this.topLeft.transform.localPosition = new Vector3(-this.desiredSize.x / 2f, this.desiredSize.y / 2f);
		this.topRight.transform.localPosition = new Vector3(this.desiredSize.x / 2f, this.desiredSize.y / 2f, 0f);
		this.botLeft.transform.localPosition = new Vector3(-this.desiredSize.x / 2f, -this.desiredSize.y / 2f);
		this.botRight.transform.localPosition = new Vector3(this.desiredSize.x / 2f, -this.desiredSize.y / 2f, 0f);
		this.top.SetSize(this.desiredSize.x - this.topLeft.width - this.topRight.width, this.top.height);
		this.bot.SetSize(this.desiredSize.x - this.botLeft.width - this.botRight.width, this.bot.height);
		this.left.SetSize(this.left.width, this.desiredSize.y - this.topLeft.height - this.botLeft.height);
		this.right.SetSize(this.right.width, this.desiredSize.y - this.topRight.width - this.botRight.height);
		if (this.middle != null)
		{
			this.middle.SetSize(this.desiredSize.x - 8f, this.desiredSize.y - 8f);
		}
	}

	private void Start()
	{
		this.lastSize = this.minimumSize;
		if (this.growSpeed < 0.1f)
		{
			this.growSpeed = 0.1f;
		}
		this.SetToMinimumSize();
	}

	private void Update()
	{
		Vector2 vector = Vector2.Lerp(this.lastSize, this.desiredSize, Time.deltaTime * this.growSpeed);
		this.left.transform.localPosition = new Vector3(-vector.x / 2f, 0f, 0f);
		this.right.transform.localPosition = new Vector3(vector.x / 2f, 0f, 0f);
		this.top.transform.localPosition = new Vector3(0f, vector.y / 2f, 0f);
		this.bot.transform.localPosition = new Vector3(0f, -vector.y / 2f, 0f);
		this.topLeft.transform.localPosition = new Vector3(-vector.x / 2f, vector.y / 2f);
		this.topRight.transform.localPosition = new Vector3(vector.x / 2f, vector.y / 2f, 0f);
		this.botLeft.transform.localPosition = new Vector3(-vector.x / 2f, -vector.y / 2f);
		this.botRight.transform.localPosition = new Vector3(vector.x / 2f, -vector.y / 2f, 0f);
		this.top.SetSize(vector.x - this.topLeft.width - this.topRight.width, this.top.height);
		this.bot.SetSize(vector.x - this.botLeft.width - this.botRight.width, this.bot.height);
		this.left.SetSize(this.left.width, vector.y - this.topLeft.height - this.botLeft.height);
		this.right.SetSize(this.right.width, vector.y - this.topRight.width - this.botRight.height);
		if (this.middle != null)
		{
			this.middle.SetSize(vector.x - 8f, vector.y - 8f);
		}
		this.lastSize = vector;
		if (!Application.isPlaying)
		{
			this.SetToDesiredSize();
		}
	}

	public bool IsBusyGrowing()
	{
		return Vector2.SqrMagnitude(this.lastSize - this.desiredSize) > 5f;
	}

	public void FinishGrowing()
	{
		this.lastSize = this.desiredSize;
	}

	public void SetColor(Color col)
	{
		this.top.SetColor(col);
		this.bot.SetColor(col);
		this.left.SetColor(col);
		this.right.SetColor(col);
		this.botLeft.SetColor(col);
		this.botRight.SetColor(col);
		this.topLeft.SetColor(col);
		this.topRight.SetColor(col);
	}

	public SpriteSM top;

	public SpriteSM bot;

	public SpriteSM left;

	public SpriteSM right;

	public SpriteSM botLeft;

	public SpriteSM botRight;

	public SpriteSM topLeft;

	public SpriteSM topRight;

	public SpriteSM middle;

	protected Vector2 minimumSize = new Vector2(12f, 12f);

	protected Vector2 lastSize;

	public Vector2 desiredSize;

	public bool preview;

	public float growSpeed;
}
