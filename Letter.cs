// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class Letter : MonoBehaviour
{
	private void Start()
	{
		foreach (LightFlare lightFlare in this.flarePrefabs)
		{
			lightFlare.gameObject.SetActive(false);
		}
		base.StartCoroutine(this.FlareSpawnRoutine());
	}

	private IEnumerator FlareSpawnRoutine()
	{
		LightFlare prefab = this.flarePrefabs[UnityEngine.Random.Range(0, this.flarePrefabs.Length)];
		LightFlare newFlare = UnityEngine.Object.Instantiate(prefab) as LightFlare;
		newFlare.transform.parent = base.transform;
		Vector3 pos = newFlare.transform.localPosition;
		pos.x = 0f;
		pos.y = (float)UnityEngine.Random.Range(-16, 16);
		pos.z = 5f;
		newFlare.transform.localPosition = pos;
		newFlare.speed *= UnityEngine.Random.Range(1.2f, 1.7f);
		newFlare.Direction = -1;
		newFlare.distance = newFlare.speed * 50f;
		newFlare.gameObject.SetActive(true);
		yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.3f));
		base.StartCoroutine(this.FlareSpawnRoutine());
		yield break;
	}

	public LightFlare[] flarePrefabs;
}
