// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroTransport : NetworkObject
{
	private void Awake()
	{
		HeroTransport.instance = this;
		this.sprite = base.GetComponent<SpriteSM>();
		this.hasEnteredLevel = false;
		this.containedBros = new List<TestVanDammeAnim>();
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
		base.gameObject.SetActive(false);
	}

	public static Vector3 GetPosition()
	{
		if (HeroTransport.instance != null)
		{
			if (!HeroTransport.instance.enteringLevel)
			{
				UnityEngine.Debug.LogError("Balls! How did the code get here?");
			}
			return HeroTransport.instance.transform.position;
		}
		UnityEngine.Debug.LogError("How did the code get here?");
		return Vector3.zero;
	}

	public static void EnterLevel(Vector3 target)
	{
		if (HeroTransport.instance != null && !HeroTransport.instance.enteringLevel && !HeroTransport.instance.hasEnteredLevel)
		{
			if (HeroTransport.instance != null)
			{
				HeroTransport.instance.gameObject.SetActive(true);
				HeroTransport.instance.enteringLevel = true;
				HeroTransport.instance.SetStartPosition(target);
				HeroTransport.instance.SetGroundHeightGround();
			}
			else
			{
				UnityEngine.Debug.LogError("Cannot Enter Level");
			}
		}
	}

	public virtual void SetGroundHeightGround()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(new Vector3(8f, 1000f, 0f), Vector3.down, out raycastHit, 1540f, this.groundLayer))
		{
			this.groundHeight = raycastHit.point.y; this.y = (this.groundHeight );
		}
		else
		{
			UnityEngine.Debug.LogError("This Level has no start");
			this.groundHeight = 128f; this.y = (this.groundHeight );
		}
	}

	public static bool Exists()
	{
		return HeroTransport.instance != null;
	}

	public static bool IsTransportingBros()
	{
		return !(HeroTransport.instance != null) || HeroTransport.instance.enteringLevel;
	}

	public static void AddBroToTransport(TestVanDammeAnim bro)
	{
		if (HeroTransport.instance != null && !HeroTransport.instance.hasReleasedHeros)
		{
			if (!HeroTransport.instance.containedBros.Contains(bro))
			{
				HeroTransport.instance.containedBros.Add(bro);
				bro.HideCharacter();
				bro.transform.position = HeroTransport.instance.transform.position;
				bro.invulnerable = true;
			}
		}
		else
		{
			UnityEngine.Debug.LogError("NO INSTANCE to ADD BRO");
		}
	}

	public void SetStartPosition(Vector3 target)
	{
		this.targetPosition = target;
		this.destinationX = target.x;
		Vector3 vector = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
		Vector3 start = vector;
		start.y = target.y;
		start.x -= 24f;
		this.x = start.x;
		this.y = start.y;
		this.SetPosition(0f);
		UnityEngine.Debug.DrawRay(start, UnityEngine.Random.onUnitSphere * 20f, Color.magenta, 30f);
		UnityEngine.Debug.DrawRay(start, UnityEngine.Random.onUnitSphere * 20f, Color.magenta, 30f);
	}

	public void SetPosition(float xOffset)
	{
		base.transform.position = new Vector3(this.x + xOffset, this.y, 5f);
	}

	protected virtual void PlayFrame(int frame)
	{
		if (frame % 2 == 0)
		{
			this.sprite.SetLowerLeftPixel(0f, 128f);
		}
		else
		{
			this.sprite.SetLowerLeftPixel(0f, 64f);
		}
	}

	private void Update()
	{
		if (!Map.Instance.HasBeenSetup)
		{
			return;
		}
		if (Map.isEditing)
		{
			if (this.containedBros.Count > 0)
			{
				this.x += 100f;
				this.ReleaseBros();
				this.x -= 100f;
			}
			return;
		}
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.enteringLevel)
		{
			this.frameCounter += this.t;
			if (this.frameCounter > 0.0667f)
			{
				this.frameCounter -= 0.0667f;
				this.frame++;
				this.PlayFrame(this.frame);
			}
			if (this.x > 16f)
			{
				this.GetGroundHeight();
			}
			if (this.y <= this.groundHeight)
			{
				this.xI = Mathf.Lerp(this.xI, 130f, this.t * 10f);
				this.x += this.xI * this.t;
				if (this.x > this.destinationX)
				{
					HeroController.SetCheckPoint(new Vector2(this.x + 32f, this.y + 16f));
					this.x = this.destinationX;
					this.enteringLevel = false;
					this.PlayFrame(0);
					this.hasEnteredLevel = true;
					AudioSource component = base.GetComponent<AudioSource>();
					if (component != null)
					{
						component.clip = this.stopSound;
						component.time = 0f;
						component.Play();
					}
					this.ReleaseBros();
				}
				else
				{
					foreach (TestVanDammeAnim testVanDammeAnim in this.containedBros)
					{
						testVanDammeAnim.x = this.x;
						testVanDammeAnim.y = this.y;
					}
				}
				this.SetPosition(0f);
				Map.DisturbWildLife(this.x + 16f, this.y, 120f, 5);
				Map.HitLivingUnits(this, 5, 3, DamageType.Bullet, 24f, this.x + 16f, this.y + 8f, 60f, 40f, true, true);
				MapController.DamageGround(this, 10, DamageType.Crush, 24f, this.x + 24f, this.y + 24f, null);
				MapController.DamageGround(this, 10, DamageType.Crush, 24f, this.x + 24f, this.y + 48f, null);
			}
			else
			{
				this.xI = 0f;
				this.RunFalling();
			}
		}
		else if (this.x > 16f)
		{
			this.GetGroundHeight();
			if (this.y > this.groundHeight)
			{
				this.RunFalling();
			}
		}
	}

	protected void RunFalling()
	{
		if (this.settled)
		{
			this.shakeCounter = 0.3f;
			this.settled = false;
		}
		if (this.shakeCounter > 0f)
		{
			this.shakeCounter -= this.t;
			this.SetPosition(Mathf.Sin(this.shakeCounter * 40f) * 2f);
		}
		else
		{
			this.yI -= 700f * this.t;
			float num = this.yI * this.t;
			if (this.y + num <= this.groundHeight)
			{
				this.y = this.groundHeight;
				this.yI = 0f;
				this.settled = true;
				Sound.GetInstance().PlaySoundEffectAt(this.landGroundSound, 0.6f, base.transform.position);
			}
			else
			{
				if (this.yI < -70f)
				{
					this.HitUnits();
				}
				this.y += num;
			}
			this.SetPosition(0f);
			if (this.enteringLevel && this.groundHeight < 0f && this.x > 4f)
			{
				HeroController.SetCheckPoint(new Vector2(this.x + 32f, this.y + 16f));
				this.ReleaseBros();
				this.enteringLevel = false;
				this.hasEnteredLevel = true;
			}
		}
	}

	protected virtual void HitUnits()
	{
		if (Map.HitUnits(this, 20, DamageType.Crush, 21f, 2f, this.x - 8f, this.y - 8f, 0f, this.yI, true, false))
		{
		}
	}

	protected virtual void GetGroundHeight()
	{
		this.groundHeight = -200f;
		this.leftGround = false;
		this.rightGround = false;
		this.midLeftGround = false;
		this.midRightGround = false;
		this.midGround = false;
		if (Physics.Raycast(new Vector3(this.x, this.y + 6f, 0f), Vector3.down, out this.raycastHitMid, 64f, this.groundLayer))
		{
			if (this.raycastHitMid.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitMid.point.y;
			}
			if (this.raycastHitMid.point.y > this.y - 9f)
			{
				this.midGround = true;
			}
		}
		if (Physics.Raycast(new Vector3(this.x - 16f, this.y + 6f, 0f), Vector3.down, out this.raycastHitMidLeft, 64f, this.groundLayer))
		{
			if (this.raycastHitMidLeft.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitMidLeft.point.y;
			}
			if (this.raycastHitMidLeft.point.y > this.y - 9f)
			{
				this.midLeftGround = true;
			}
		}
		if (Physics.Raycast(new Vector3(this.x + 16f, this.y + 6f, 0f), Vector3.down, out this.raycastHitMidRight, 64f, this.groundLayer))
		{
			if (this.raycastHitMidRight.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitMidRight.point.y;
			}
			if (this.raycastHitMidRight.point.y > this.y - 9f)
			{
				this.midRightGround = true;
			}
		}
		if (Physics.Raycast(new Vector3(this.x + 32f, this.y + 6f, 0f), Vector3.down, out this.raycastHitRight, 64f, this.groundLayer))
		{
			if (this.raycastHitRight.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitRight.point.y;
			}
			if (this.raycastHitRight.point.y > this.y - 9f)
			{
				this.rightGround = true;
			}
		}
		if (Physics.Raycast(new Vector3(this.x - 32f, this.y + 6f, 0f), Vector3.down, out this.raycastHitLeft, 64f, this.groundLayer))
		{
			if (this.raycastHitLeft.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitLeft.point.y;
			}
			if (this.raycastHitLeft.point.y > this.y - 9f)
			{
				this.leftGround = true;
			}
		}
		if ((!this.leftGround && !this.midLeftGround) || (!this.rightGround && !this.midRightGround) || (!this.midLeftGround && !this.midRightGround))
		{
			this.DamageGroundBelow(true);
		}
	}

	protected void DamageGroundBelow(bool forced)
	{
		if ((!this.leftGround && !this.midLeftGround) || (!this.rightGround && !this.midRightGround))
		{
			forced = true;
		}
		if (this.leftGround || this.midLeftGround || this.midRightGround || this.rightGround || this.midGround)
		{
			this.yI = 0f;
		}
		if ((!this.midLeftGround || forced) && this.midRightGround && this.raycastHitRight.collider != null)
		{
			this.raycastHitMidRight.collider.gameObject.SendMessage("Damage", new DamageObject(5, DamageType.Crush, 0f, 0f, null), SendMessageOptions.DontRequireReceiver);
		}
		if ((!this.midRightGround || forced) && this.midLeftGround && this.raycastHitLeft.collider != null)
		{
			this.raycastHitMidLeft.collider.gameObject.SendMessage("Damage", new DamageObject(5, DamageType.Crush, 0f, 0f, null), SendMessageOptions.DontRequireReceiver);
		}
		if (forced)
		{
			if (this.leftGround && this.raycastHitLeft.collider != null)
			{
				this.raycastHitLeft.collider.gameObject.SendMessage("Damage", new DamageObject(5, DamageType.Crush, 0f, 0f, null), SendMessageOptions.DontRequireReceiver);
			}
			if (this.rightGround && this.raycastHitRight.collider != null)
			{
				this.raycastHitRight.collider.gameObject.SendMessage("Damage", new DamageObject(5, DamageType.Crush, 0f, 0f, null), SendMessageOptions.DontRequireReceiver);
			}
			if (this.midGround && this.raycastHitMid.collider != null)
			{
				this.raycastHitMid.collider.gameObject.SendMessage("Damage", new DamageObject(5, DamageType.Crush, 0f, 0f, null), SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	protected void ReleaseBros()
	{
		HeroController.Instance.brosHaveBeenReleased = true;
		this.hasReleasedHeros = true;
		for (int i = 0; i < this.containedBros.Count; i++)
		{
			this.containedBros[i].gunSprite.gameObject.SetActive(true);
			this.containedBros[i].GetComponent<Renderer>().enabled = true;
			this.containedBros[i].enabled = true;
			this.containedBros[i].EnableSyncing(true, false);
			this.containedBros[i].x = this.x + 16f + (float)(i * 8);
			if (this.y >= 0f)
			{
				this.containedBros[i].y = this.y + 24f;
			}
			else
			{
				this.containedBros[i].y = 64f;
			}
			this.containedBros[i].SetPosition();
			this.containedBros[i].xI = 60f;
			this.containedBros[i].yI = 360f;
			this.containedBros[i].actionState = ActionState.Jumping;
			this.containedBros[i].SetInvulnerable(1f, true);
		}
		this.containedBros.Clear();
	}

	public override UnityStream PackState(UnityStream stream)
	{
		stream.Serialize<bool>(this.enteringLevel);
		stream.Serialize<bool>(this.hasEnteredLevel);
		stream.Serialize<bool>(this.hasReleasedHeros);
		stream.Serialize<Vector3>(this.targetPosition);
		stream.Serialize<float>(this.x);
		stream.Serialize<float>(this.y);
		return base.PackState(stream);
	}

	public override UnityStream UnpackState(UnityStream stream)
	{
		bool flag = (bool)stream.DeserializeNext();
		bool flag2 = (bool)stream.DeserializeNext();
		this.hasReleasedHeros = (bool)stream.DeserializeNext();
		this.targetPosition = (Vector3)stream.DeserializeNext();
		float num = (float)stream.DeserializeNext();
		float num2 = (float)stream.DeserializeNext();
		if (flag || flag2)
		{
			HeroTransport.instance.gameObject.SetActive(true);
		}
		if (flag)
		{
			HeroTransport.EnterLevel(this.targetPosition);
		}
		if (flag2)
		{
			this.x = num;
			this.y = num2;
			this.SetPosition(0f);
		}
		this.enteringLevel = flag;
		this.hasEnteredLevel = flag2;
		return base.UnpackState(stream);
	}

	public static HeroTransport instance;

	public float x = 96f;

	public float y = 196f;

	public float destinationX = 80f;

	public bool enteringLevel;

	public bool hasEnteredLevel;

	public bool hasReleasedHeros;

	private Vector3 targetPosition = Vector3.zero;

	protected bool settled = true;

	protected float frameCounter;

	protected int frame;

	protected SpriteSM sprite;

	protected float xI;

	protected float yI;

	protected float t = 0.01f;

	protected float shakeCounter;

	protected LayerMask groundLayer;

	public AudioClip stopSound;

	public AudioClip landGroundSound;

	protected List<TestVanDammeAnim> containedBros = new List<TestVanDammeAnim>();

	protected float groundHeight;

	protected RaycastHit raycastHitMidLeft;

	protected RaycastHit raycastHitMidRight;

	protected RaycastHit raycastHitLeft;

	protected RaycastHit raycastHitMid;

	protected RaycastHit raycastHitRight;

	protected RaycastHit raycastHit;

	protected bool leftGround;

	protected bool rightGround;

	protected bool midLeftGround;

	protected bool midGround;

	protected bool midRightGround;
}
