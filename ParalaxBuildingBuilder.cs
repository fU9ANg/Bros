// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ParalaxBuildingBuilder : MonoBehaviour
{
	private void Start()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		float num = this.sprite.pixelDimensions.y;
		int num2 = 1 + UnityEngine.Random.Range(0, 7);
		for (int i = 0; i < num2; i++)
		{
			SpriteSM spriteSM = UnityEngine.Object.Instantiate(this.buildingMiddles[UnityEngine.Random.Range(0, this.buildingMiddles.Length)], base.transform.position + Vector3.up * num, Quaternion.identity) as SpriteSM;
			spriteSM.transform.parent = base.transform;
			num += spriteSM.pixelDimensions.y;
		}
		SpriteSM spriteSM2 = UnityEngine.Object.Instantiate(this.buildingTops[UnityEngine.Random.Range(0, this.buildingTops.Length)], base.transform.position + Vector3.up * num, Quaternion.identity) as SpriteSM;
		spriteSM2.transform.parent = base.transform;
	}

	private void Update()
	{
	}

	public SpriteSM[] buildingMiddles;

	public SpriteSM[] buildingTops;

	protected SpriteSM sprite;
}
