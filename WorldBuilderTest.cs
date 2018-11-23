// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldBuilderTest : MonoBehaviour
{
	private void Start()
	{
		this.worldBuilder = base.GetComponent<WorldBuilder>();
	}

	private void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			UnityEngine.Debug.Log("Input Mouse 1");
			RaycastHit raycastHit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 100f, 1 << LayerMask.NameToLayer("Ground")))
			{
				UnityEngine.Debug.Log("Click " + raycastHit.collider.transform.InverseTransformPoint(raycastHit.point));
				raycastHit.collider.SendMessage("Raycast");
			}
		}
		RaycastHit raycastHit2;
		if (Input.GetButtonDown("Fire2") && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit2, 100f, 1 << LayerMask.NameToLayer("Ground")))
		{
			UnityEngine.Debug.Log("Right Click " + raycastHit2.collider.transform.InverseTransformPoint(raycastHit2.point));
			this.worldBuilder.Click(raycastHit2.collider.transform.InverseTransformPoint(raycastHit2.point), WorldMapTerrainType.Ocean);
		}
	}

	protected WorldBuilder worldBuilder;
}
