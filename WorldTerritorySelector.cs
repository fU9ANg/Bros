// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldTerritorySelector : MonoBehaviour
{
	private void Awake()
	{
		WorldTerritorySelector.instance = this;
	}

	private void Start()
	{
		this.territories = (UnityEngine.Object.FindObjectsOfType(typeof(WorldTerritory3D)) as WorldTerritory3D[]);
		this.currentTerritory = WorldMapProgressController.GetStartingTerritory();
		if (this.currentTerritory != null)
		{
			this.startTransportVector = this.currentTerritory.GetCentre();
		}
		else
		{
			UnityEngine.Debug.LogError("No Current Territory!!");
		}
	}

	public static void DeselectOtherTerritories(WorldTerritory3D current)
	{
		foreach (WorldTerritory3D worldTerritory3D in WorldTerritorySelector.instance.territories)
		{
			if (worldTerritory3D != current)
			{
				worldTerritory3D.Select(false);
			}
		}
	}

	public static void SelectTerritoryStatic(WorldTerritory3D territory)
	{
		WorldTerritorySelector.instance.SelectTerritory(territory);
	}

	public void SelectTerritory(WorldTerritory3D territory)
	{
		WorldMapInterfaceTerritoryDetails.Disappear();
		if (territory == null)
		{
			WorldTerritorySelector.DeselectOtherTerritories(territory);
			return;
		}
		if (territory.properties.state != TerritoryState.Empty && territory.properties.state != TerritoryState.Liberated)
		{
			string actionText = "LIBERATE!";
			if (territory.properties.state == TerritoryState.Brofort || territory.properties.state == TerritoryState.ForwardBase)
			{
				actionText = "RECUPERATE!";
			}
			WorldMapInterfaceTerritoryDetails.Appear(territory.properties.territoryName, actionText, territory.properties.threatLevel, territory.properties.threatName, territory.properties.threatColor);
		}
		if (territory.properties.state == TerritoryState.Liberated && territory.HasHospital())
		{
			WorldMapInterfaceTerritoryDetails.Appear(territory.properties.territoryName, "RECUPERATE", territory.properties.threatLevel, territory.properties.threatName, territory.properties.threatColor);
		}
		this.selectedTerritory = territory;
		territory.Select(true);
		WorldTerritorySelector.DeselectOtherTerritories(territory);
	}

	public void DeselectTerritories()
	{
		WorldMapInterfaceTerritoryDetails.Disappear();
		foreach (WorldTerritory3D worldTerritory3D in WorldTerritorySelector.instance.territories)
		{
			worldTerritory3D.Select(false);
		}
	}

	protected bool ButtonPressedUp()
	{
		return Input.GetKeyDown(KeyCode.UpArrow);
	}

	protected bool ButtonPressedDown()
	{
		return Input.GetKeyDown(KeyCode.DownArrow);
	}

	protected bool ButtonPressedLeft()
	{
		return Input.GetKeyDown(KeyCode.LeftArrow);
	}

	protected bool ButtonPressedRight()
	{
		return Input.GetKeyDown(KeyCode.RightArrow);
	}

	protected bool ButtonPressedFire()
	{
		return Input.GetKeyDown(KeyCode.Space);
	}

	protected bool ButtonReleaseUp()
	{
		return Input.GetKeyUp(KeyCode.UpArrow);
	}

	protected bool ButtonReleaseDown()
	{
		return Input.GetKeyUp(KeyCode.DownArrow);
	}

	protected bool ButtonReleaseLeft()
	{
		return Input.GetKeyUp(KeyCode.LeftArrow);
	}

	protected bool ButtonReleaseRight()
	{
		return Input.GetKeyUp(KeyCode.RightArrow);
	}

	protected bool ButtonReleaseFire()
	{
		return Input.GetKeyUp(KeyCode.Space);
	}

	protected void GetInput()
	{
		this.wasFire = this.fire;
		if (this.ButtonPressedUp())
		{
			this.up = true;
			this.recalcKeys = true;
		}
		if (this.ButtonReleaseUp())
		{
			this.up = false;
		}
		if (this.ButtonPressedDown())
		{
			this.down = true;
			this.recalcKeys = true;
		}
		if (this.ButtonReleaseDown())
		{
			this.down = false;
		}
		if (this.ButtonPressedLeft())
		{
			this.left = true;
			this.recalcKeys = true;
		}
		if (this.ButtonReleaseLeft())
		{
			this.left = false;
		}
		if (this.ButtonPressedRight())
		{
			this.right = true;
			this.recalcKeys = true;
		}
		if (this.ButtonReleaseRight())
		{
			this.right = false;
		}
		if (this.ButtonPressedFire())
		{
			UnityEngine.Debug.Log(" Fire! " + this.wasFire);
			this.fire = true;
		}
		if (this.ButtonReleaseFire())
		{
			UnityEngine.Debug.Log("Un Fire! " + this.wasFire);
			this.fire = false;
		}
	}

	private void SelectTerritory(bool up, bool down, bool left, bool right)
	{
		if (!this.recalcKeys)
		{
			return;
		}
		this.recalcKeys = false;
		WorldTerritory3D worldTerritory3D = null;
		if (up)
		{
			if (right)
			{
				worldTerritory3D = this.currentTerritory.GetUpperRightTerritory();
			}
			else if (left)
			{
				worldTerritory3D = this.currentTerritory.GetUpperLeftTerritory();
			}
			else
			{
				worldTerritory3D = this.currentTerritory.GetUpperTerritory();
			}
		}
		else if (down)
		{
			if (right)
			{
				worldTerritory3D = this.currentTerritory.GetLowerRightTerritory();
			}
			else if (left)
			{
				worldTerritory3D = this.currentTerritory.GetLowerLeftTerritory();
			}
			else
			{
				worldTerritory3D = this.currentTerritory.GetLowerTerritory();
			}
		}
		else if (right)
		{
			worldTerritory3D = this.currentTerritory.GetRightTerritory();
		}
		else if (left)
		{
			worldTerritory3D = this.currentTerritory.GetLeftTerritory();
		}
		if (worldTerritory3D != null && !worldTerritory3D.selected)
		{
			this.SelectTerritory(worldTerritory3D);
		}
	}

	protected void CheckGotoTerritory()
	{
		if (this.fire && !this.wasFire)
		{
			UnityEngine.Debug.Log(" Go!  " + this.selectedTerritory + "   was fire ");
			if (this.selectedTerritory != null)
			{
				this.testHelicopter.GoToPosition(this.selectedTerritory);
			}
		}
	}

	public static void SetCurrentTerritory(WorldTerritory3D territory)
	{
		WorldTerritorySelector.instance.startTransportVector = territory.GetCentreWorldLocal();
		WorldTerritorySelector.instance.currentTerritory = territory;
	}

	private void Update()
	{
		this.SelectTerritory(this.up, this.down, this.left, this.right);
		this.CheckGotoTerritory();
		RaycastHit raycastHit;
		if (Input.GetMouseButtonDown(1) && Physics.Raycast(this.worldCamera.ScreenPointToRay(Input.mousePosition), out raycastHit, 1000f, 1 << LayerMask.NameToLayer("Ground")))
		{
			WorldTerritory3D component = raycastHit.collider.GetComponent<WorldTerritory3D>();
			if (component != null)
			{
				if (!component.IsSelected())
				{
					this.SelectTerritory(component);
				}
				else
				{
					this.startTransportVector = component.GetCentreWorldLocal();
					this.testHelicopter.GoToPosition(component);
				}
			}
		}
	}

	protected static WorldTerritorySelector instance;

	public Camera worldCamera;

	public Transform worldActualTransform;

	public float transportHeight = 6f;

	protected Vector3 startTransportVector = Vector3.back;

	public Helicopter2D testHelicopter;

	protected WorldTerritory3D[] territories;

	public WorldLine3d worldLine;

	protected WorldTerritory3D currentTerritory;

	protected WorldTerritory3D selectedTerritory;

	protected bool left;

	protected bool right;

	protected bool up;

	protected bool down;

	protected bool fire;

	protected bool wasFire;

	protected bool recalcKeys;
}
