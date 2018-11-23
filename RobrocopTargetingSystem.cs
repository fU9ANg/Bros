// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class RobrocopTargetingSystem : BroforceObject
{
	protected void Start()
	{
		this.startPos = base.transform.position;
	}

	public DirectionEnum TravelDirection
	{
		get
		{
			return this.travelDirection;
		}
		set
		{
			this.travelDirection = value;
		}
	}

	protected virtual void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0333334f);
		if (base.IsMine)
		{
			Vector3 a = Vector3.zero;
			switch (this.travelDirection)
			{
			case DirectionEnum.Up:
				a = Vector3.up;
				break;
			case DirectionEnum.Down:
				a = Vector3.down;
				break;
			case DirectionEnum.Left:
				a = Vector3.left;
				break;
			case DirectionEnum.Right:
				a = Vector3.right;
				break;
			}
			base.transform.position += a * num * this.speed;
		}
		if ((this.beepDelay -= num) < 0f)
		{
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.missSounds, 0.4f, base.transform.position);
			this.beepDelay = 0.333f;
			this.text.gameObject.SetActive(true);
		}
		else if (this.beepDelay < 0.1665f)
		{
			this.text.gameObject.SetActive(false);
		}
		if (this.ownerRef == null || this.ownerRef.health <= 0 || this.ownerRef.destroyed)
		{
			if (base.IsMine)
			{
				base.DestroyNetworked();
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	protected override void LateUpdate()
	{
		Vector3 position = Camera.main.ScreenToWorldPoint(Vector3.zero);
		Vector3 position2 = Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0f));
		position.x += 8f;
		position.y += 8f;
		position.z = -20f;
		position2.x -= 8f;
		position2.y -= 8f;
		position2.z = -20f;
		this.horizontalLine.SetPosition(0, new Vector3(position.x, base.transform.position.y, -20f));
		this.horizontalLine.SetPosition(1, new Vector3(position2.x, base.transform.position.y, -20f));
		this.verticalLine.SetPosition(0, new Vector3(base.transform.position.x, position.y, -20f));
		this.verticalLine.SetPosition(1, new Vector3(base.transform.position.x, position2.y, -20f));
		this.boxTop.SetPosition(0, position);
		this.boxTop.SetPosition(1, new Vector3(position2.x, position.y, -20f));
		this.boxRight.SetPosition(0, new Vector3(position2.x, position.y, -20f));
		this.boxRight.SetPosition(1, position2);
		this.boxBot.SetPosition(0, position2);
		this.boxBot.SetPosition(1, new Vector3(position.x, position2.y, -20f));
		this.boxLeft.SetPosition(0, new Vector3(position.x, position2.y, -20f));
		this.boxLeft.SetPosition(1, position);
		this.text.transform.position = new Vector3(position.x + 24f, position.y + 24f, -20f);
		if (base.transform.position.x < position.x + 4f)
		{
			this.travelDirection = DirectionEnum.None;
		}
		if (base.transform.position.x > position2.x - 4f)
		{
			this.travelDirection = DirectionEnum.None;
		}
		if (base.transform.position.y < position.y + 4f)
		{
			this.travelDirection = DirectionEnum.None;
		}
		if (base.transform.position.y < position.y + 4f)
		{
			this.travelDirection = DirectionEnum.None;
		}
		base.transform.position = new Vector3(Mathf.Clamp(base.transform.position.x, position.x + 4f, position2.x - 4f), Mathf.Clamp(base.transform.position.y, position.y + 4f, position2.y - 4f), base.transform.position.z);
		base.LateUpdate();
	}

	public void PlayHitSound()
	{
		this.beepDelay = 0.4f;
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.hitSounds, 0.4f, base.transform.position);
	}

	public void SetupTargetting(Unit hero, DirectionEnum direction)
	{
		this.TravelDirection = direction;
		this.ownerRef = hero;
		base.SetSyncingInternal(true);
	}

	[Syncronize]
	private Vector2 SyncPosition
	{
		get
		{
			return base.transform.position;
		}
		set
		{
			Vector3 position = value;
			position.z = base.transform.position.z;
			base.transform.position = position;
		}
	}

	public float speed = 120f;

	private DirectionEnum travelDirection;

	public SoundHolder soundHolder;

	private float beepDelay;

	[HideInInspector]
	public Vector3 startPos;

	private Vector3 lastTurnPos;

	public Unit ownerRef;

	public LineRenderer horizontalLine;

	public LineRenderer verticalLine;

	public LineRenderer boxTop;

	public LineRenderer boxBot;

	public LineRenderer boxLeft;

	public LineRenderer boxRight;

	public SpriteSM text;
}
