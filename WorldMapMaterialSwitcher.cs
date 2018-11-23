// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldMapMaterialSwitcher : MonoBehaviour
{
	private void Start()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position + Vector3.forward * 0.15f + Vector3.up, Vector3.down, out raycastHit, 3f, 1 << LayerMask.NameToLayer("Ground")))
		{
			WorldMapTerritoryCollider component = raycastHit.collider.GetComponent<WorldMapTerritoryCollider>();
			if (component != null)
			{
				this.territory = component.territory;
			}
		}
		WorldMapMaterialController.RegisterMaterialSwitcher(this);
	}

	public void SwitchMaterialBasedOnTerritory()
	{
		if (this.territory != null && this.territory.properties.state == TerritoryState.TerroristBurning)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.burningMaterial;
			WorldMapWavyGrass component = base.gameObject.GetComponent<WorldMapWavyGrass>();
			if (component != null)
			{
				component.emitFire = true;
			}
		}
	}

	private void Update()
	{
		if (this.firstFrame)
		{
			this.firstFrame = false;
			this.SwitchMaterialBasedOnTerritory();
		}
	}

	public Material normalMaterial;

	public Material burningMaterial;

	public WorldTerritory3D territory;

	protected bool firstFrame = true;
}
