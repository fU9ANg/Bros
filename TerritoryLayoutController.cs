// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TerritoryLayoutController : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void BuildSprites()
	{
		this.prefab.GetComponent<Renderer>().sharedMaterial = this.vietnamMaterial;
		for (int i = 0; i < 43; i++)
		{
			for (int j = 0; j < 36; j++)
			{
				WorldMapTerritoryCollider worldMapTerritoryCollider = UnityEngine.Object.Instantiate(this.prefab, new Vector3((float)(i - 21) * 0.4f, 0f, (float)(16 - j) * 0.4f), Quaternion.identity) as WorldMapTerritoryCollider;
				worldMapTerritoryCollider.transform.parent = base.transform;
				worldMapTerritoryCollider.lowerLeftPixel = new Vector2((float)(i * 16), (float)((j + 1) * 16));
				worldMapTerritoryCollider.GetComponent<Renderer>().sharedMaterial = this.vietnamMaterial;
				SpriteSM component = worldMapTerritoryCollider.GetComponent<SpriteSM>();
				component.SetLowerLeftPixel(worldMapTerritoryCollider.lowerLeftPixel);
			}
		}
	}

	public void SetRowCollumns()
	{
		bool flag = true;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			WorldMapTerritoryCollider component = base.transform.GetChild(i).GetComponent<WorldMapTerritoryCollider>();
			if (component != null)
			{
				component.layoutController = this;
				component.collumn = (int)Mathf.Floor(component.transform.position.x / 0.4f + 22f) - 1;
				component.row = (int)Mathf.Floor(19f + component.transform.position.z / 0.4f);
				if (component.collumn < 0 || component.row < 0)
				{
					flag = false;
				}
				else
				{
					if (component.collumn > num2)
					{
						num2 = component.collumn;
					}
					if (component.row > num)
					{
						num = component.row;
					}
				}
			}
		}
		if (flag)
		{
			UnityEngine.Debug.Log("Make Grid !");
			int num3 = 0;
			this.worldColliders = new WorldMapTerritoryCollider[num2 + 1, num + 1];
			for (int j = 0; j < base.transform.childCount; j++)
			{
				WorldMapTerritoryCollider component2 = base.transform.GetChild(j).GetComponent<WorldMapTerritoryCollider>();
				if (component2 != null && this.worldColliders[component2.collumn, component2.row] == null)
				{
					this.worldColliders[component2.collumn, component2.row] = component2;
					num3++;
				}
			}
			UnityEngine.Debug.Log("AssignChild Grids ! " + num3);
		}
		else
		{
			UnityEngine.Debug.LogError("Grid doesn't work");
		}
	}

	public WorldMapTerritoryCollider prefab;

	public Material vietnamMaterial;

	public WorldMapTerritoryCollider[,] worldColliders;
}
