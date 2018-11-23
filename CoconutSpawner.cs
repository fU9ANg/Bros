// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CoconutSpawner : MonoBehaviour
{
	private void Awake()
	{
		bool flag = UnityEngine.Random.value < 0.2f + Map.MapData.coconutProbability;
		for (int i = 0; i < this.cocunutDummies.Length; i++)
		{
			if (this.cocunutDummies[i] != null)
			{
				if (flag && UnityEngine.Random.value < Map.MapData.coconutProbability)
				{
					Coconut coconut = UnityEngine.Object.Instantiate(this.coconutPrefab, this.cocunutDummies[i].transform.position, this.cocunutDummies[i].transform.rotation) as Coconut;
					coconut.transform.parent = base.transform;
					coconut.SetToDisable(true);
					if (this.treeFoliage != null)
					{
						this.treeFoliage.RegisterCoconut(coconut);
					}
				}
				UnityEngine.Object.Destroy(this.cocunutDummies[i]);
			}
		}
	}

	public Coconut coconutPrefab;

	public GameObject[] cocunutDummies;

	public TreeFoliage treeFoliage;
}
