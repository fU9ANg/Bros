// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	private void Awake()
	{
		Map.RegisterSpawnPoint(this);
	}

	private void Update()
	{
		if (!Map.isEditing)
		{
			base.enabled = false;
			if (base.GetComponent<Renderer>() != null)
			{
				base.GetComponent<Renderer>().enabled = false;
			}
		}
	}

	private void OnDestroy()
	{
		if (this.cage != null)
		{
			UnityEngine.Object.Destroy(this.cage.gameObject);
		}
	}

	public CageTemporary cage;
}
