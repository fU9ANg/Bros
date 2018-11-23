// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Elevator : Doodad
{
	protected override void Start()
	{
		base.Start();
		if (base.GetComponent<AudioSource>() == null)
		{
			base.gameObject.AddComponent<AudioSource>();
			base.GetComponent<AudioSource>().playOnAwake = false;
			base.GetComponent<AudioSource>().dopplerLevel = 0.1f;
			base.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
			base.GetComponent<AudioSource>().maxDistance = 320f;
			base.GetComponent<AudioSource>().loop = true;
			base.GetComponent<AudioSource>().volume = 0.2f;
			base.GetComponent<AudioSource>().clip = this.elevatorMoving;
			base.GetComponent<AudioSource>().Stop();
		}
		this.y = base.transform.position.y; this.targetY = (this.y );
		this.x = base.transform.position.x;
		if (!Map.isEditing)
		{
			this.CheckPosition();
		}
	}

	protected void CheckPosition()
	{
		this.checkedPosition = true;
		float y = this.y + this.elevatorTop;
		if (Physics.Raycast(new Vector3(this.x - 17f, y, 0f), Vector3.up, out this.groundHit, 1500f, this.groundLayer))
		{
			this.groundHit.collider.gameObject.SendMessage("AttachMe", this);
			if (this.groundHit.point.y < this.ceilingY)
			{
				this.ceilingY = this.groundHit.point.y;
				this.SetElevatorWires(this.ceilingY - this.elevatorWires.transform.position.y);
			}
		}
		if (Physics.Raycast(new Vector3(this.x + 17f, y, 0f), Vector3.up, out this.groundHit, 1500f, this.groundLayer))
		{
			this.groundHit.collider.gameObject.SendMessage("AttachMe", this);
			if (this.groundHit.point.y > this.ceilingY)
			{
				this.ceilingY = this.groundHit.point.y;
				this.SetElevatorWires(this.ceilingY - this.elevatorWires.transform.position.y);
			}
		}
		float y2 = this.y + this.elevatorBottom;
		if (Physics.Raycast(new Vector3(this.x - 17f, y2, 0f), Vector3.down, out this.groundHit, 500f, this.groundLayer) && this.groundHit.point.y > this.floorY)
		{
			this.floorY = this.groundHit.point.y;
		}
		if (Physics.Raycast(new Vector3(this.x + 17f, y2, 0f), Vector3.down, out this.groundHit, 500f, this.groundLayer) && this.groundHit.point.y > this.floorY)
		{
			this.floorY = this.groundHit.point.y;
		}
	}

	protected void SetElevatorWires(float height)
	{
		this.elevatorWires.SetPixelDimensions((int)this.elevatorWires.pixelDimensions.x, (int)height);
		this.elevatorWires.SetSize(this.elevatorWires.width, height);
		this.elevatorWires.UpdateUVs();
	}

	protected override void AttachDoodad()
	{
	}

	public void GoUp()
	{
		Networking.RPC<float, float>(PID.TargetAll, new RpcSignature<float, float>(this.GoUpRPC), this.x, this.y, false);
	}

	public void GoUpRPC(float X, float Y)
	{
		if (this.moving)
		{
			return;
		}
		this.x = X;
		this.y = Y;
		int num = Map.GetRow(this.y);
		while (this.targetY + 16f <= this.ceilingY - 32f)
		{
			this.targetY += 16f;
			num++;
			if ((Map.WasBlockOriginallySolid(this.collumn - 2, num - 1) && !Map.WasBlockOriginallySolid(this.collumn - 2, num)) || (Map.WasBlockOriginallySolid(this.collumn + 2, num - 1) && !Map.WasBlockOriginallySolid(this.collumn + 2, num)))
			{
				break;
			}
		}
		if (this.targetY != this.y)
		{
			if (!this.moving)
			{
				this.yI = this.speed * 0.25f;
			}
			this.StartMoving();
		}
	}

	public void GoDown()
	{
		if (this.moving)
		{
			return;
		}
		int num = Map.GetRow(this.y);
		while (this.targetY - 32f >= this.floorY + 8f)
		{
			this.targetY -= 16f;
			num--;
			if ((Map.WasBlockOriginallySolid(this.collumn - 2, num - 1) && !Map.WasBlockOriginallySolid(this.collumn - 2, num)) || (Map.WasBlockOriginallySolid(this.collumn + 2, num - 1) && !Map.WasBlockOriginallySolid(this.collumn + 2, num)))
			{
				break;
			}
		}
		if (this.targetY != this.y)
		{
			if (!this.moving)
			{
				this.yI = -this.speed * 0.25f;
			}
			this.StartMoving();
		}
	}

	protected void StartMoving()
	{
		if (!this.moving)
		{
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.effortSounds, 0.25f, base.transform.position);
			SortOfFollow.Shake(0.25f, 2f);
			this.moving = true;
			base.GetComponent<AudioSource>().Play();
		}
	}

	public override void Collapse()
	{
		this.collapsed = true;
		this.falling = true;
	}

	protected virtual void Update()
	{
		if (!this.checkedPosition && !Map.isEditing)
		{
			this.CheckPosition();
		}
		if (this.falling)
		{
			this.elevatorWires.gameObject.SetActive(false);
			if (this.shakeCounter > 0f)
			{
				this.SetPosition(global::Math.Sin(this.shakeCounter * this.shakeCounter * 120f) * 2f);
				this.shakeCounter -= Time.deltaTime;
			}
			else
			{
				this.yI -= 700f * Time.deltaTime;
				if (this.yI < this.terminalVelocity)
				{
					this.yI = this.terminalVelocity;
				}
				float num = this.yI * Time.deltaTime;
				float num2 = this.y + this.elevatorBottom;
				float groundHeight = this.GetGroundHeight(num2);
				if (num2 + num <= groundHeight)
				{
					EffectsController.CreateExplosion(this.x, this.y, 8f, 8f, 160f, 0f, 10f, 0.7f, 0.6f, false);
					EffectsController.CreateExplosion(this.x, this.y, 16f, 16f, 160f, 1f, 10f, 0f, 0f, false);
					EffectsController.CreateExplosion(this.x, this.y, 16f, 16f, 160f, 1f, 10f, 0f, 0f, false);
					EffectsController.CreateGlassShards(this.x, this.y, 30, 16f, 16f, 0f, 250f, 0f, 125f, 0f, 0f, 0.4f);
					this.CrushGround(num2);
					UnityEngine.Object.Destroy(base.gameObject);
				}
				else
				{
					this.y += num;
					this.SetPosition();
				}
			}
		}
		else if (this.moving)
		{
			this.HitUnits();
			if (this.targetY > this.y)
			{
				base.GetComponent<AudioSource>().pitch = Mathf.Lerp(base.GetComponent<AudioSource>().pitch, 6f, Time.deltaTime * 5f);
				this.yI = Mathf.Lerp(this.yI, this.speed, Time.deltaTime * 15f);
				this.y += this.yI * Time.deltaTime;
				if (this.y >= this.targetY)
				{
					this.StopElevator();
				}
			}
			else if (this.targetY < this.y)
			{
				base.GetComponent<AudioSource>().pitch = Mathf.Lerp(base.GetComponent<AudioSource>().pitch, 4f, Time.deltaTime * 5f);
				this.yI = Mathf.Lerp(this.yI, -this.speed, Time.deltaTime * 15f);
				this.y += this.yI * Time.deltaTime;
				if (this.y <= this.targetY)
				{
					this.StopElevator();
				}
			}
			this.SetElevatorWires(this.ceilingY - this.elevatorWires.transform.position.y);
			this.SetPosition();
		}
	}

	protected void CrushGround(float elevatorBottomY)
	{
		if (Physics.Raycast(new Vector3(this.x - 16f, elevatorBottomY, 0f), Vector3.down, out this.groundHit, 13f, this.groundLayer))
		{
			this.groundHit.collider.SendMessage("Damage", new DamageObject(20, DamageType.Crush, 0f, 0f, null));
		}
		if (Physics.Raycast(new Vector3(this.x + 16f, elevatorBottomY, 0f), Vector3.down, out this.groundHit, 13f, this.groundLayer))
		{
			this.groundHit.collider.SendMessage("Damage", new DamageObject(20, DamageType.Crush, 0f, 0f, null));
		}
		if (Physics.Raycast(new Vector3(this.x, elevatorBottomY, 0f), Vector3.down, out this.groundHit, 13f, this.groundLayer))
		{
			this.groundHit.collider.SendMessage("Damage", new DamageObject(20, DamageType.Crush, 0f, 0f, null));
		}
	}

	protected float GetGroundHeight(float elevatorBottomY)
	{
		float num = -50f;
		if (Physics.Raycast(new Vector3(this.x - 16f, elevatorBottomY, 0f), Vector3.down, out this.groundHit, 500f, this.groundLayer))
		{
			if (this.groundHit.point.y > num)
			{
				num = this.groundHit.point.y;
			}
			if (this.groundHit.point.y > elevatorBottomY - 18f)
			{
				UnityEngine.Debug.DrawLine(new Vector3(this.x - 16f, elevatorBottomY, 0f), new Vector3(this.x - 16f, elevatorBottomY - 16f, 0f), Color.red);
				if (Map.HitUnits(this, 20, DamageType.Crush, 8f, 2f, this.x - 11f, elevatorBottomY - 2f, 0f, this.yI, true, false))
				{
					UnityEngine.Debug.Log("Crush Bottom Left  ********************************  ");
				}
			}
		}
		if (Physics.Raycast(new Vector3(this.x + 16f, elevatorBottomY, 0f), Vector3.down, out this.groundHit, 500f, this.groundLayer))
		{
			if (this.groundHit.point.y > num)
			{
				num = this.groundHit.point.y;
			}
			if (this.groundHit.point.y > elevatorBottomY - 18f)
			{
				UnityEngine.Debug.DrawLine(new Vector3(this.x + 16f, elevatorBottomY, 0f), new Vector3(this.x + 16f, elevatorBottomY - 16f, 0f), Color.red);
				if (Map.HitUnits(this, 20, DamageType.Crush, 8f, 2f, this.x + 11f, elevatorBottomY - 2f, 0f, this.yI, true, false))
				{
					UnityEngine.Debug.Log("Crush Bottom Right ********************************  ");
				}
			}
		}
		if (Physics.Raycast(new Vector3(this.x, elevatorBottomY, 0f), Vector3.down, out this.groundHit, 500f, this.groundLayer))
		{
			if (this.groundHit.point.y > num)
			{
				num = this.groundHit.point.y;
			}
			if (this.groundHit.point.y > elevatorBottomY - 18f)
			{
				UnityEngine.Debug.DrawLine(new Vector3(this.x, elevatorBottomY, 0f), new Vector3(this.x + 16f, elevatorBottomY - 16f, 0f), Color.red);
				if (Map.HitUnits(this, 20, DamageType.Crush, 8f, 2f, this.x, elevatorBottomY - 2f, 0f, this.yI, true, false))
				{
					UnityEngine.Debug.Log("Crush Bottom Centre ********************************  ");
				}
			}
		}
		return num;
	}

	protected virtual void HitUnits()
	{
		if (this.targetY > this.y)
		{
			float num = this.y + this.elevatorTop;
			if (Physics.Raycast(new Vector3(this.x - 16f, num, 0f), Vector3.up, out this.groundHit, 500f, this.groundLayer) && this.groundHit.point.y < num + 18f)
			{
				UnityEngine.Debug.DrawLine(new Vector3(this.x - 16f, num, 0f), new Vector3(this.x - 16f, num + 16f, 0f), Color.red);
				if (Map.HitUnits(this, 20, DamageType.Crush, 11f, 2f, this.x - 12f, num, 0f, this.yI, true, false))
				{
				}
			}
			if (Physics.Raycast(new Vector3(this.x + 16f, num, 0f), Vector3.up, out this.groundHit, 500f, this.groundLayer) && this.groundHit.point.y < num + 18f)
			{
				UnityEngine.Debug.DrawLine(new Vector3(this.x + 16f, num, 0f), new Vector3(this.x + 16f, num + 16f, 0f), Color.red);
				if (Map.HitUnits(this, 20, DamageType.Crush, 11f, 2f, this.x + 12f, num, 0f, this.yI, true, false))
				{
				}
			}
		}
		else if (this.targetY < this.y)
		{
			float num2 = this.y + this.elevatorBottom;
			if (Physics.Raycast(new Vector3(this.x - 16f, num2, 0f), Vector3.down, out this.groundHit, 500f, this.groundLayer) && this.groundHit.point.y > num2 - 18f)
			{
				UnityEngine.Debug.DrawLine(new Vector3(this.x - 16f, num2, 0f), new Vector3(this.x - 16f, num2 - 16f, 0f), Color.red);
				if (Map.HitUnits(this, 20, DamageType.Crush, 11f, 2f, this.x - 11f, num2 - 2f, 0f, this.yI, true, false))
				{
					UnityEngine.Debug.Log("Crush Bottom Left  ********************************  ");
				}
			}
			if (Physics.Raycast(new Vector3(this.x + 16f, num2, 0f), Vector3.down, out this.groundHit, 500f, this.groundLayer) && this.groundHit.point.y > num2 - 18f)
			{
				UnityEngine.Debug.DrawLine(new Vector3(this.x + 16f, num2, 0f), new Vector3(this.x + 16f, num2 - 16f, 0f), Color.red);
				if (Map.HitUnits(this, 20, DamageType.Crush, 11f, 2f, this.x + 11f, num2 - 2f, 0f, this.yI, true, false))
				{
					UnityEngine.Debug.Log("Crush Bottom Right ********************************  ");
				}
			}
		}
	}

	protected void StopElevator()
	{
		this.y = this.targetY;
		this.yI = 0f;
		this.moving = false;
		SortOfFollow.Shake(0.3f, 2f);
		base.GetComponent<AudioSource>().Stop();
		base.GetComponent<AudioSource>().pitch = 0.01f;
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.hitSounds, 0.2f, base.transform.position);
	}

	public virtual void SetPosition()
	{
		base.transform.position = new Vector3(this.x, this.y, 0f);
	}

	public void SetPosition(float xOffset)
	{
		base.transform.position = new Vector3(this.x + xOffset, this.y, 0f);
	}

	public float elevatorTop = 33f;

	public float elevatorBottom = -11f;

	protected float ceilingY = 10000f;

	protected float floorY = -10f;

	public SpriteSM elevatorWires;

	public float speed = 120f;

	public AudioClip elevatorMoving;

	public float terminalVelocity = -400f;

	public float targetY;

	protected bool moving;

	protected bool falling;

	protected bool checkedPosition;

	protected float shakeCounter = 0.22f;
}
