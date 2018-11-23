// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BloodDebug : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit;
			Physics.Raycast(ray, out raycastHit);
			if (raycastHit.collider != null)
			{
				Block component = raycastHit.collider.GetComponent<Block>();
				if (component != null)
				{
					component.Bloody(Vector3.up, BloodColor.Red);
					component.Bloody(Vector3.left, BloodColor.Red);
					component.Bloody(Vector3.right, BloodColor.Red);
					component.Bloody(Vector3.down, BloodColor.Red);
				}
			}
		}
	}
}
