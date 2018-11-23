// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldTransport : MonoBehaviour
{
	private void Awake()
	{
		WorldTransport.instance = this;
	}

	private void Start()
	{
		if (this.targetTransform != null)
		{
			this.currentPosition = this.targetTransform.position;
		}
		else
		{
			this.currentPosition = base.transform.position;
		}
		this.SetPosition();
	}

	public static void ShowText(string t)
	{
		if (WorldTransport.instance != null)
		{
			WorldTransport.instance.textFloat.text = t;
			WorldTransport.instance.textFloat.gameObject.SetActive(true);
		}
	}

	protected void SetPosition()
	{
		base.transform.position = new Vector3(this.currentPosition.x, this.currentPosition.y, -20f);
	}

	public bool IsMoving()
	{
		return this.moving;
	}

	public void GoTo(WorldLocation location)
	{
		this.targetTransform = location.transform;
		this.moving = true;
	}

	private void Update()
	{
		if (this.moving)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.currentPosition = this.targetTransform.position;
			}
			float deltaTime = Time.deltaTime;
			Vector3 a = this.targetTransform.position - this.currentPosition;
			float magnitude = a.magnitude;
			if (magnitude < this.speed * deltaTime * 1.5f)
			{
				this.moving = false;
				this.currentPosition = this.targetTransform.position;
				this.targetTransform.SendMessage("Activate");
			}
			else
			{
				Vector3 a2 = a / magnitude;
				this.currentPosition += a2 * this.speed * deltaTime;
			}
			this.SetPosition();
		}
	}

	public float speed = 20f;

	public Transform targetTransform;

	protected Vector3 currentPosition;

	protected bool moving;

	public TextMesh textFloat;

	protected static WorldTransport instance;
}
