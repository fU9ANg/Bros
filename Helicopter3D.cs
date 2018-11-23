// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter3D : MonoBehaviour
{
	private void Awake()
	{
		Helicopter3D.instance = this;
		global::Math.SetupLookupTables();
	}

	private void Start()
	{
		WorldTerritory3D startingTerritory = WorldMapProgressController.GetStartingTerritory();
		if (startingTerritory != null)
		{
			base.transform.position = startingTerritory.GetCentreWorldLocal().normalized * this.heightOffGround;
		}
		else
		{
			base.transform.position = this.position.normalized * this.heightOffGround;
		}
		this.helicopterPathPoints.Add(base.transform.localPosition.normalized);
		this.yAngle = global::Math.GetAngle(base.transform.position.x, base.transform.position.z) / 3.14159274f * 180f + 90f;
		this.xAngle = global::Math.GetAngle(base.transform.position.y, 1f) / 3.14159274f * 180f - 90f;
	}

	public static void ShowText(string t)
	{
		if (Helicopter3D.instance != null)
		{
			Helicopter3D.instance.textFloat.text = t;
			Helicopter3D.instance.textFloat.gameObject.SetActive(true);
		}
	}

	public void GoToPosition(WorldTerritory3D territory)
	{
		this.targetPos = territory.GetCentreWorldLocal().normalized * this.heightOffGround;
		this.targetTerritory = territory;
		this.travelling = true;
	}

	protected void GetInput()
	{
		this.left = Input.GetKey(KeyCode.LeftArrow);
		this.right = Input.GetKey(KeyCode.RightArrow);
		this.up = Input.GetKey(KeyCode.UpArrow);
		this.down = Input.GetKey(KeyCode.DownArrow);
		if (this.left && this.right)
		{
			this.right = false; this.left = (this.right );
		}
		if (this.up && this.down)
		{
			this.down = false; this.up = (this.down );
		}
	}

	protected void RunInput()
	{
		Vector3 vector = base.transform.position;
		if (this.up)
		{
			this.xAngle -= Time.deltaTime * this.angularMoveSpeed;
			base.transform.RotateAround(Vector3.zero, Vector3.right, Time.deltaTime * this.angularMoveSpeed);
		}
		if (this.down)
		{
			this.xAngle += Time.deltaTime * this.angularMoveSpeed;
			base.transform.RotateAround(Vector3.zero, Vector3.right, -Time.deltaTime * this.angularMoveSpeed);
		}
		if (this.left)
		{
			this.yAngle -= Time.deltaTime * this.angularMoveSpeed;
			base.transform.RotateAround(Vector3.zero, Vector3.up, Time.deltaTime * this.angularMoveSpeed);
		}
		if (this.right)
		{
			this.yAngle += Time.deltaTime * this.angularMoveSpeed;
			base.transform.RotateAround(Vector3.zero, Vector3.up, -Time.deltaTime * this.angularMoveSpeed);
		}
		if (vector != base.transform.position)
		{
			this.dummyTransform.LookAt(base.transform.position + (base.transform.position - vector), base.transform.position);
			Vector3 b = Vector3.RotateTowards(base.transform.forward, this.dummyTransform.forward, Time.deltaTime * 5f, Time.deltaTime * 8f);
			base.transform.LookAt(base.transform.position + b, base.transform.position);
		}
	}

	private void Update()
	{
		if (!this.liftingOff && !this.landing)
		{
			int num = 0;
			Vector3 vector = base.transform.localPosition.normalized - this.helicopterPathPoints[this.helicopterPathPoints.Count - 1];
			while (vector.magnitude > 0.01f && num < 20)
			{
				num++;
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
			this.down = false; this.left = (this.right = (this.up = (this.down )));
			this.landingCounter -= Time.deltaTime * 0.6f;
			if (this.landingCounter <= 0f)
			{
				this.landingCounter = 0f;
				UnityEngine.Debug.Log("Lifted Off! " + this.targetTerritory);
				this.landing = false;
				this.liftingOff = false;
			}
			this.dummyTransform.LookAt(Vector3.RotateTowards(this.dummyTransform.position, Vector3.up * this.dummyTransform.position.magnitude, 0.02f, 1f), base.transform.position);
			Vector3 vector2 = Vector3.RotateTowards(base.transform.forward, this.dummyTransform.forward, Time.deltaTime * 5f, Time.deltaTime * 8f);
			base.transform.LookAt(base.transform.position + vector2, base.transform.position);
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Look At ! ",
				vector2,
				" dummyTransform.position ",
				this.dummyTransform.position
			}));
			base.transform.localPosition = base.transform.localPosition.normalized * (this.heightOffGround - this.landingCounter * this.landingCounter);
		}
		else if (!this.travelling && !this.landing)
		{
			this.GetInput();
			this.RunInput();
			RaycastHit[] array = Physics.RaycastAll(base.transform.position, -base.transform.position, 1f, 1 << LayerMask.NameToLayer("Ground"));
			if (array.Length == 1)
			{
			}
			UnityEngine.Debug.Log("Trying to select territory");
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position, Vector3.down, out raycastHit, 3f, 1 << LayerMask.NameToLayer("Ground")))
			{
				WorldMapTerritoryCollider component = raycastHit.collider.GetComponent<WorldMapTerritoryCollider>();
				UnityEngine.Debug.Log("Select territory " + (component != null));
				if (component != null)
				{
					component.Raycast();
				}
			}
			else if (array.Length == 0)
			{
				this.territorySelector.DeselectTerritories();
				this.currentTerritory = null;
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.GoToPosition(this.currentTerritory);
			}
		}
		else if (this.travelling)
		{
			this.down = false; this.left = (this.right = (this.up = (this.down )));
			Vector3 vector3 = Vector3.RotateTowards(base.transform.localPosition, this.targetPos, Time.deltaTime * this.speedRadias, Time.deltaTime);
			if ((vector3 - base.transform.localPosition).magnitude > 0.0001f)
			{
				base.transform.LookAt(base.transform.parent.TransformPoint(vector3), base.transform.position);
			}
			if ((this.targetPos - base.transform.localPosition).magnitude < 0.001f)
			{
				this.travelling = false;
				this.landing = true;
			}
			base.transform.localPosition = vector3;
		}
		else if (this.landing)
		{
			this.down = false; this.left = (this.right = (this.up = (this.down )));
			this.landingCounter += Time.deltaTime;
			this.dummyTransform.LookAt(Vector3.RotateTowards(this.dummyTransform.position, Vector3.up * this.dummyTransform.position.magnitude, 0.02f, 1f), base.transform.position);
			Vector3 b = Vector3.RotateTowards(base.transform.forward, this.dummyTransform.forward, Time.deltaTime * 5f, Time.deltaTime * 8f);
			base.transform.LookAt(base.transform.position + b, base.transform.position);
			base.transform.localPosition = base.transform.localPosition.normalized * (this.heightOffGround - this.landingCounter * this.landingCounter);
			if (this.landingCounter > 0.7f)
			{
				UnityEngine.Debug.Log("Arrived! " + this.targetTerritory);
				WorldMapController.TransportArriveAt(this.targetTerritory);
				this.landing = false;
				this.liftingOff = true;
			}
		}
	}

	protected Vector3 position = Vector3.back;

	protected Vector3 targetPos = Vector3.back;

	public float heightOffGround = 5.3f;

	protected bool travelling;

	public float speedRadias = 1f;

	protected WorldTerritory3D targetTerritory;

	protected static Helicopter3D instance;

	public TextMesh textFloat;

	protected bool left;

	protected bool right;

	protected bool up;

	protected bool down;

	protected float yAngle;

	protected float xAngle;

	public float angularMoveSpeed = 1f;

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

	protected float landingCounter;

	public int maxPathPoints = 500;
}
