// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter2D : MonoBehaviour
{
	private void Awake()
	{
		Helicopter2D.instance = this;
		global::Math.SetupLookupTables();
	}

	private void Start()
	{
		WorldTerritory3D startingTerritory = WorldMapProgressController.GetStartingTerritory();
		if (startingTerritory != null)
		{
			base.transform.position = startingTerritory.GetCentreWorldLocal() + Vector3.up * this.heightOffGround;
			this.TurnToFace(Helicopter2D.facingDirection, 2f);
			this.worldCamera.SetZoom(2f);
			this.landingCounter = 1f;
			this.liftingOff = true;
			base.GetComponent<AudioSource>().pitch = 0.5f;
		}
		if (WorldMapProgressController.GetTurn() <= 0)
		{
			this.entering = true;
			base.transform.position = new Vector3(base.transform.position.x, this.heightOffGround, base.transform.position.z);
			this.TurnToFace(-1, 2f);
			this.worldCamera.SetZoom(3f);
		}
		this.helicopterPathPoints.Add(base.transform.localPosition.normalized);
		this.yAngle = global::Math.GetAngle(base.transform.position.x, base.transform.position.z) / 3.14159274f * 180f + 90f;
		this.xAngle = global::Math.GetAngle(base.transform.position.y, 1f) / 3.14159274f * 180f - 90f;
		this.shadowSprite.transform.parent = base.transform.parent;
	}

	public static void ShowText(string t)
	{
		if (Helicopter2D.instance != null)
		{
			Helicopter2D.instance.textFloat.text = t;
			Helicopter2D.instance.textFloat.gameObject.SetActive(true);
		}
	}

	public void GoToPosition(WorldTerritory3D territory)
	{
		this.targetPos = territory.GetCentreWorldLocal() + Vector3.up * this.heightOffGround;
		this.targetPos = new Vector3(this.targetPos.x, this.heightOffGround, this.targetPos.z);
		this.targetTerritory = territory;
		this.travelling = true;
		if (this.targetPos.x < base.transform.position.x)
		{
			Helicopter2D.facingDirection = -1;
		}
		else
		{
			Helicopter2D.facingDirection = 1;
		}
		WorldMapAdvisor.Salute();
		WorldMapInterfaceTerritoryDetails.SayGoodbye();
	}

	protected void GetInput()
	{
		InputReader.GetCombinedInput(ref this.up, ref this.down, ref this.left, ref this.right, ref this.fire, ref this.buttonJump, ref this.special, ref this.highFive);
		this.inputJitterCounter += Time.deltaTime * 2f + Time.deltaTime * Mathf.Sin(this.inputJitterCounter * 0.3f) * 1f;
		if (this.left && this.right)
		{
			if (Mathf.Repeat(this.inputJitterCounter, 0.6f) < 0.1f)
			{
				this.right = false; this.left = (this.right );
			}
			else if (Mathf.Repeat(this.inputJitterCounter, 0.6f) < 0.3f)
			{
				this.left = false;
			}
			else if (Mathf.Repeat(this.inputJitterCounter, 0.6f) < 0.4f)
			{
				this.right = false; this.left = (this.right );
			}
			else
			{
				this.right = false;
			}
		}
		if (this.up && this.down)
		{
			if (Mathf.Repeat(this.inputJitterCounter * 1.3f, 0.6f) < 0.1f)
			{
				this.down = false; this.up = (this.down );
			}
			else if (Mathf.Repeat(this.inputJitterCounter * 1.3f, 0.6f) < 0.3f)
			{
				this.up = false;
			}
			else if (Mathf.Repeat(this.inputJitterCounter * 1.3f, 0.6f) < 0.4f)
			{
				this.down = false; this.up = (this.down );
			}
			else
			{
				this.down = false;
			}
		}
	}

	protected void RunInput()
	{
		Vector3 a = Vector3.zero;
		Vector3 vector = base.transform.position;
		if (this.up)
		{
			a += new Vector3(0f, 0f, this.moveSpeed);
		}
		if (this.down)
		{
			a += new Vector3(0f, 0f, -this.moveSpeed);
		}
		if (this.left)
		{
			a += new Vector3(-this.moveSpeed, 0f, 0f);
		}
		if (this.right)
		{
			a += new Vector3(this.moveSpeed, 0f, 0f);
		}
		a = a.normalized;
		base.transform.Translate(a.x * (this.moveSpeed * (0.5f + Mathf.Abs(this.bankingForwardAmount) * 0.5f)) * Time.deltaTime, a.y * (this.moveSpeed * (0.5f + Mathf.Abs(this.bankingForwardAmount) * 0.5f)) * Time.deltaTime, a.z * (this.moveSpeed * (0.5f + Mathf.Abs(this.bankingForwardAmount) * 0.5f)) * Time.deltaTime, Space.World);
		if (vector != base.transform.position)
		{
			Vector3 forward = base.transform.forward;
			this.dummyTransform.LookAt(base.transform.position + (base.transform.position - vector), Vector3.up);
			Vector3 b = Vector3.RotateTowards(base.transform.forward, this.dummyTransform.forward, Time.deltaTime * (2f + Mathf.Abs(this.bankingAmount) * 6f + this.trickBankSpeed * 6f), Time.deltaTime * 8f);
			base.transform.LookAt(base.transform.position + b, Vector3.up);
			this.movingTime += Time.deltaTime;
			if (this.movingTime > 2.5f)
			{
				this.movingTime = 2.5f;
			}
			this.worldCamera.SetZoom(Mathf.Lerp(this.worldCamera.currentZoom, 1f - Mathf.Clamp(this.movingTime, 0f, 2f) * 0.1f, Time.deltaTime * 8f));
			Vector3 normalized = a.normalized;
			Vector3 vector2 = new Vector3(base.transform.forward.x, 0f, base.transform.forward.z);
			this.RunBanking(normalized, vector2.normalized);
		}
		else if (this.movingTime > 0f)
		{
			this.movingTime = Mathf.Clamp(this.movingTime - Time.deltaTime, -1.5f, 100f);
			this.worldCamera.SetZoom(Mathf.Lerp(this.worldCamera.currentZoom, 1f - Mathf.Clamp(this.movingTime, 0f, 2f) * 0.1f, Time.deltaTime * 8f));
			this.RunBanking(base.transform.forward, base.transform.forward);
		}
		this.oldInputDirection = a;
	}

	protected void RunBanking(Vector3 oldForward, Vector3 newForward)
	{
		float angle = global::Math.GetAngle(oldForward.x, oldForward.z);
		float num = global::Math.GetAngle(newForward.x, newForward.z);
		if (num - angle < -3.14159274f)
		{
			num += 6.28318548f;
		}
		if (num - angle > 3.14159274f)
		{
			num -= 6.28318548f;
		}
		if (Mathf.Abs(num - angle) > 0.314159274f)
		{
			this.bankingForwardAmount = Mathf.Lerp(this.bankingForwardAmount, 0.5f, Time.deltaTime * 5f);
			this.bankingAmount = Mathf.Lerp(this.bankingAmount, (float)((num >= angle) ? -1 : 1), Time.deltaTime * 7f);
			this.trickBankSpeed = Mathf.Lerp(this.trickBankSpeed, 1f, Time.deltaTime * 0.5f);
		}
		else
		{
			if (this.up || this.down || this.left || this.right)
			{
				this.bankingForwardAmount = Mathf.Lerp(this.bankingForwardAmount, 1f, Time.deltaTime * 7f);
			}
			else
			{
				this.bankingForwardAmount = Mathf.Lerp(this.bankingForwardAmount, 0f, Time.deltaTime * 5f);
			}
			this.bankingAmount = Mathf.Lerp(this.bankingAmount, 0f, Time.deltaTime * 5f);
			this.trickBankSpeed = Mathf.Lerp(this.trickBankSpeed, 0f, Time.deltaTime * 5f);
		}
		this.helicopterActualTransform.localEulerAngles = new Vector3(this.bankingAmount * this.bankDegrees, this.helicopterActualTransform.localEulerAngles.y, this.bankingForwardAmount * this.bankDegrees * -0.4f);
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		Vector3 forward = base.transform.forward;
		if (this.entering)
		{
			this.enterTime -= num;
			if (this.enterTime <= 0f)
			{
				this.enterTime = 0f;
				this.entering = false;
			}
			if (this.enterTime < 1.5f)
			{
				float num2 = this.enterTime / 1.5f;
				this.worldCamera.SetZoom(1f + num2 * 2f);
				base.transform.Translate(-this.moveSpeed * num * num2, 0f, 0f, Space.World);
			}
			else
			{
				this.worldCamera.SetZoom(3f);
				base.transform.Translate(-this.moveSpeed * num, 0f, 0f, Space.World);
			}
			if (this.enterTime < 3f)
			{
				this.GetInput();
				this.RunInput();
				if (this.up || this.down || this.left || this.right || this.fire)
				{
					this.entering = false;
				}
			}
		}
		else if (!this.liftingOff && !this.landing)
		{
			int num3 = 0;
			Vector3 vector = base.transform.localPosition.normalized - this.helicopterPathPoints[this.helicopterPathPoints.Count - 1];
			while (vector.magnitude > 0.01f && num3 < 20)
			{
				num3++;
				this.helicopterPathPoints.Add(this.helicopterPathPoints[this.helicopterPathPoints.Count - 1] + vector.normalized * 0.01f);
				if (this.helicopterPathPoints.Count > this.maxPathPoints)
				{
					this.helicopterPathPoints.RemoveAt(0);
					this.helicopterPathTextureOffest += 0.01f;
					this.helicopterPath.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(this.helicopterPathTextureOffest * this.helicopterPathScaleM, 0f));
					this.helicopterPathTextureScale -= 0.01f;
				}
				this.helicopterPath.SetVertexCount(this.helicopterPathPoints.Count);
				for (int i = 0; i < this.helicopterPathPoints.Count; i++)
				{
					this.helicopterPath.SetPosition(i, this.helicopterPathPoints[i] * this.helicopterPathHeight);
				}
				this.helicopterPathTextureScale += 0.01f;
				this.helicopterPath.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(this.helicopterPathTextureScale * this.helicopterPathScaleM, 1f));
				vector = base.transform.localPosition.normalized - this.helicopterPathPoints[this.helicopterPathPoints.Count - 1];
			}
		}
		if (this.liftingOff)
		{
			base.GetComponent<AudioSource>().pitch = Mathf.Lerp(base.GetComponent<AudioSource>().pitch, 1f, Time.deltaTime * 2f);
			this.down = false; this.left = (this.right = (this.up = (this.down )));
			this.landingCounter -= Time.deltaTime * 0.6f;
			if (this.landingCounter <= 0f)
			{
				this.landingCounter = 0f;
				UnityEngine.Debug.Log("Lifted Off! " + this.targetTerritory);
				this.landing = false;
				this.liftingOff = false;
			}
			base.transform.position = new Vector3(base.transform.position.x, this.heightOffGround * (1f - this.landingCounter * this.landingCounter * 0.6f), base.transform.position.z);
			this.worldCamera.SetZoom(Mathf.Lerp(this.worldCamera.currentZoom, 1f, num * 4f));
		}
		else if (!this.travelling && !this.landing)
		{
			this.GetInput();
			this.RunInput();
			WorldTerritory3D y = null;
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position + Vector3.right * 0.15f, Vector3.down, out raycastHit, 3f, 1 << LayerMask.NameToLayer("Ground")))
			{
				WorldMapTerritoryCollider component = raycastHit.collider.GetComponent<WorldMapTerritoryCollider>();
				if (component != null)
				{
					y = component.territory;
				}
			}
			WorldTerritory3D y2 = null;
			if (Physics.Raycast(base.transform.position + Vector3.left * 0.15f, Vector3.down, out raycastHit, 3f, 1 << LayerMask.NameToLayer("Ground")))
			{
				WorldMapTerritoryCollider component2 = raycastHit.collider.GetComponent<WorldMapTerritoryCollider>();
				if (component2 != null)
				{
					y2 = component2.territory;
				}
			}
			WorldTerritory3D y3 = null;
			if (Physics.Raycast(base.transform.position + Vector3.forward * 0.15f, Vector3.down, out raycastHit, 3f, 1 << LayerMask.NameToLayer("Ground")))
			{
				WorldMapTerritoryCollider component3 = raycastHit.collider.GetComponent<WorldMapTerritoryCollider>();
				if (component3 != null)
				{
					y3 = component3.territory;
				}
			}
			WorldTerritory3D y4 = null;
			if (Physics.Raycast(base.transform.position + Vector3.back * 0.15f, Vector3.down, out raycastHit, 3f, 1 << LayerMask.NameToLayer("Ground")))
			{
				WorldMapTerritoryCollider component4 = raycastHit.collider.GetComponent<WorldMapTerritoryCollider>();
				if (component4 != null)
				{
					y4 = component4.territory;
				}
			}
			if (Physics.Raycast(base.transform.position, Vector3.down, out raycastHit, 3f, 1 << LayerMask.NameToLayer("Ground")))
			{
				WorldMapTerritoryCollider component5 = raycastHit.collider.GetComponent<WorldMapTerritoryCollider>();
				if (component5 != null && ((component5.territory == y && component5.territory == y2 && component5.territory == y3 && component5.territory == y4) || (this.currentTerritory != component5.territory && this.currentTerritory != y && this.currentTerritory != y2 && this.currentTerritory != y3 && this.currentTerritory != y4)))
				{
					component5.Raycast();
					if (component5.territory != null)
					{
						this.currentTerritory = component5.territory;
					}
				}
				this.dustCounter += num;
				if (this.dustCounter > 0.033f)
				{
					if (component5.isOcean)
					{
						this.dustCounter -= 0.025f;
						WorldMapEffectsController.EmitWaves(1, new Vector3(base.transform.position.x, 0.01f, base.transform.position.z), Color.white, 0.2f, Vector3.zero, 0.4f);
					}
					else
					{
						this.dustCounter -= 0.0334f;
						WorldMapEffectsController.EmitDust(1, new Vector3(base.transform.position.x, 0.01f, base.transform.position.z), Color.white, 0.3f, Vector3.zero, 0.4f);
					}
				}
			}
			if (this.fire && !this.travelling)
			{
				UnityEngine.Debug.Log("Go To Position + " + ((!(this.currentTerritory != null)) ? " NULL " : this.currentTerritory.name));
				if (this.currentTerritory != null)
				{
					this.GoToPosition(this.currentTerritory);
				}
			}
			base.GetComponent<AudioSource>().pitch = Mathf.Lerp(base.GetComponent<AudioSource>().pitch, 1f, Time.deltaTime * 2f);
		}
		else if (this.travelling)
		{
			this.down = false; this.left = (this.right = (this.up = (this.down )));
			Vector3 vector2 = new Vector3(this.targetPos.x, this.heightOffGround, this.targetPos.z) - base.transform.position;
			Vector3 localPosition = base.transform.position + vector2.normalized * this.moveSpeed * Time.deltaTime;
			if ((new Vector3(this.targetPos.x, this.heightOffGround, this.targetPos.z) - base.transform.position).magnitude <= this.moveSpeed * Time.deltaTime * 1.2f)
			{
				this.travelling = false;
				this.landing = true;
			}
			else
			{
				this.dummyTransform.LookAt(this.targetPos, Vector3.up);
				Vector3 b = Vector3.RotateTowards(base.transform.forward, this.dummyTransform.forward, Time.deltaTime * 5f, Time.deltaTime * 8f);
				base.transform.LookAt(base.transform.position + b, Vector3.up);
			}
			base.transform.localPosition = localPosition;
			this.worldCamera.SetZoom(Mathf.Lerp(this.worldCamera.currentZoom, 1f, num));
		}
		else if (this.landing)
		{
			this.down = false; this.left = (this.right = (this.up = (this.down )));
			this.landingCounter += Time.deltaTime;
			base.GetComponent<AudioSource>().pitch = Mathf.Lerp(base.GetComponent<AudioSource>().pitch, 0.4f, Time.deltaTime * 2f);
			if (this.landingCounter >= 1f)
			{
				this.landingCounter = 1f;
				UnityEngine.Debug.Log("Arrived! " + this.targetTerritory);
				WorldMapController.TransportArriveAt(this.targetTerritory);
				this.landing = false;
				this.liftingOff = true;
			}
			this.TurnToFace(Helicopter2D.facingDirection, Time.deltaTime * 5f);
			base.transform.position = new Vector3(this.targetPos.x, this.heightOffGround * (1f - this.landingCounter * this.landingCounter * 0.6f), this.targetPos.z);
			this.worldCamera.SetZoom(Mathf.Lerp(this.worldCamera.currentZoom, 2f, num));
			this.RunBanking(base.transform.forward, base.transform.forward);
		}
	}

	protected void LateUpdate()
	{
		this.shadowSprite.transform.position = new Vector3(base.transform.position.x, 0.01f, base.transform.position.z);
		this.shadowSprite.SetColor(new Color(0f, 0f, 0f, 0.2f + (1f - base.transform.position.y / this.heightOffGround) * 0.5f));
	}

	protected void TurnToFace(int d, float lerpM)
	{
		if (d < 0)
		{
			this.dummyTransform.LookAt(Vector3.left * 1000f + Vector3.up * this.heightOffGround, Vector3.up);
		}
		else
		{
			this.dummyTransform.LookAt(Vector3.right * 1000f + Vector3.up * this.heightOffGround, Vector3.up);
		}
		base.transform.LookAt(base.transform.position + Vector3.RotateTowards(base.transform.forward, this.dummyTransform.forward, lerpM, lerpM), Vector3.up);
	}

	protected Vector3 position = Vector3.back;

	protected Vector3 targetPos = Vector3.back;

	public float heightOffGround = 5.3f;

	protected bool travelling;

	public float speedRadias = 1f;

	protected WorldTerritory3D targetTerritory;

	protected static Helicopter2D instance;

	public TextMesh textFloat;

	protected bool left;

	protected bool right;

	protected bool up;

	protected bool down;

	protected bool special;

	protected bool fire;

	protected bool highFive;

	protected bool buttonJump;

	protected float yAngle;

	protected float xAngle;

	public float moveSpeed = 1f;

	public Transform dummyTransform;

	public WorldTerritorySelector territorySelector;

	protected WorldTerritory3D currentTerritory;

	public LineRenderer helicopterPath;

	protected List<Vector3> helicopterPathPoints = new List<Vector3>();

	protected float helicopterPathTextureScale;

	protected float helicopterPathTextureOffest;

	public float helicopterPathScaleM = 6f;

	public float helicopterPathHeight = 5f;

	protected bool landing;

	protected bool liftingOff;

	protected bool entering;

	protected float landingCounter;

	public WorldCamera worldCamera;

	public Transform helicopterActualTransform;

	public int maxPathPoints = 500;

	protected static int facingDirection;

	protected float enterTime = 4f;

	protected float movingTime;

	public SpriteSM shadowSprite;

	protected float dustCounter;

	protected float inputJitterCounter;

	protected Vector3 oldInputDirection;

	protected float trickBankSpeed;

	public float bankDegrees = 10f;

	protected float bankingAmount;

	protected float bankingForwardAmount;
}
