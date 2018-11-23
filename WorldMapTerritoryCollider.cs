// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapTerritoryCollider : MonoBehaviour
{
	private void Awake()
	{
		if (this.territory != null)
		{
			this.CalculateBorders();
			this.CreateBorders();
		}
		if (this.isOcean)
		{
			this.CalculateAndCreateShores();
		}
	}

	private void Start()
	{
		if (this.territory != null)
		{
			this.territory.terrainColliders.Add(this);
		}
		if (this.territory != null)
		{
			this.CheckBorders();
		}
		SpriteSM component = base.GetComponent<SpriteSM>();
		if (component != null && (this.lowerLeftPixel.x > 0f || this.lowerLeftPixel.y > 0f))
		{
			component.SetLowerLeftPixel(this.lowerLeftPixel);
		}
		this.HideBorder();
	}

	protected void CreateBorders()
	{
		if (!this.leftTerritorySame)
		{
			SpriteSM spriteSM = UnityEngine.Object.Instantiate(this.borderSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
			spriteSM.transform.parent = base.transform;
			this.borderSprites.Add(spriteSM);
			spriteSM.transform.eulerAngles = new Vector3(0f, -90f, 0f);
			spriteSM.name = "Border Left";
		}
		if (!this.rightTerritorySame)
		{
			SpriteSM spriteSM2 = UnityEngine.Object.Instantiate(this.borderSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
			spriteSM2.transform.parent = base.transform;
			this.borderSprites.Add(spriteSM2);
			spriteSM2.transform.eulerAngles = new Vector3(0f, 90f, 0f);
			spriteSM2.name = "Border Right";
		}
		if (!this.upTerritorySame)
		{
			SpriteSM spriteSM3 = UnityEngine.Object.Instantiate(this.borderSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
			spriteSM3.transform.parent = base.transform;
			this.borderSprites.Add(spriteSM3);
			spriteSM3.transform.eulerAngles = new Vector3(0f, 0f, 0f);
			spriteSM3.name = "Border Up";
		}
		if (!this.downTerritorySame)
		{
			SpriteSM spriteSM4 = UnityEngine.Object.Instantiate(this.borderSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
			spriteSM4.transform.parent = base.transform;
			this.borderSprites.Add(spriteSM4);
			spriteSM4.transform.eulerAngles = new Vector3(0f, 180f, 0f);
			spriteSM4.name = "Border Down";
		}
		if (!this.upperLeftTerritorySame)
		{
			SpriteSM spriteSM5 = UnityEngine.Object.Instantiate(this.borderCornerSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
			spriteSM5.transform.parent = base.transform;
			this.borderSprites.Add(spriteSM5);
			spriteSM5.transform.eulerAngles = new Vector3(0f, 0f, 0f);
			spriteSM5.name = "Border UpperLeft";
		}
		if (!this.upperRightTerritorySame)
		{
			SpriteSM spriteSM6 = UnityEngine.Object.Instantiate(this.borderCornerSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
			spriteSM6.transform.parent = base.transform;
			this.borderSprites.Add(spriteSM6);
			spriteSM6.transform.eulerAngles = new Vector3(0f, 90f, 0f);
			spriteSM6.name = "Border UpperRight";
		}
		if (!this.lowerLeftTerritorySame)
		{
			SpriteSM spriteSM7 = UnityEngine.Object.Instantiate(this.borderCornerSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
			spriteSM7.transform.parent = base.transform;
			this.borderSprites.Add(spriteSM7);
			spriteSM7.transform.eulerAngles = new Vector3(0f, -90f, 0f);
			spriteSM7.name = "Border LowerLeft";
		}
		if (!this.lowerRightTerritorySame)
		{
			SpriteSM spriteSM8 = UnityEngine.Object.Instantiate(this.borderCornerSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
			spriteSM8.transform.parent = base.transform;
			this.borderSprites.Add(spriteSM8);
			spriteSM8.transform.eulerAngles = new Vector3(0f, 180f, 0f);
			spriteSM8.name = "Border LowerRight";
		}
	}

	public void SwitchMaterialsDelay()
	{
		if (this.setToMaterials != null)
		{
			base.GetComponent<Renderer>().sharedMaterials = this.setToMaterials;
		}
	}

	protected void CalculateBorders()
	{
		this.leftTerritorySame = (this.GetTerritory(-1, 0) == this.territory);
		this.rightTerritorySame = (this.GetTerritory(1, 0) == this.territory);
		this.upTerritorySame = (this.GetTerritory(0, 1) == this.territory);
		this.downTerritorySame = (this.GetTerritory(0, -1) == this.territory);
		if (this.leftTerritorySame && this.upTerritorySame)
		{
			this.upperLeftTerritorySame = (this.GetTerritory(-1, 1) == this.territory);
		}
		else
		{
			this.upperLeftTerritorySame = true;
		}
		if (this.leftTerritorySame && this.downTerritorySame)
		{
			this.lowerLeftTerritorySame = (this.GetTerritory(-1, -1) == this.territory);
		}
		else
		{
			this.lowerLeftTerritorySame = true;
		}
		if (this.rightTerritorySame && this.upTerritorySame)
		{
			this.upperRightTerritorySame = (this.GetTerritory(1, 1) == this.territory);
		}
		else
		{
			this.upperRightTerritorySame = true;
		}
		if (this.rightTerritorySame && this.downTerritorySame)
		{
			this.lowerRightTerritorySame = (this.GetTerritory(1, -1) == this.territory);
		}
		else
		{
			this.lowerRightTerritorySame = true;
		}
	}

	protected void CalculateAndCreateShores()
	{
		bool ocean = this.GetOcean(-1, 0);
		bool ocean2 = this.GetOcean(1, 0);
		bool ocean3 = this.GetOcean(0, 1);
		bool ocean4 = this.GetOcean(0, -1);
		bool flag = true;
		if (ocean && ocean3)
		{
			flag = this.GetOcean(-1, 1);
		}
		bool flag2 = true;
		if (ocean && ocean4)
		{
			flag2 = this.GetOcean(-1, -1);
		}
		bool flag3 = true;
		if (ocean2 && ocean3)
		{
			flag3 = this.GetOcean(1, 1);
		}
		bool flag4 = true;
		if (ocean2 && ocean4)
		{
			flag4 = this.GetOcean(1, -1);
		}
		if (!ocean)
		{
			if (ocean3 && ocean4)
			{
				SpriteSM spriteSM = UnityEngine.Object.Instantiate(this.shoreWavesBorderSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
				spriteSM.transform.parent = base.transform;
				spriteSM.transform.eulerAngles = new Vector3(0f, -90f, 0f);
				spriteSM.name = "Shore Left";
			}
			else if (ocean3 && !ocean4 && ocean2)
			{
				SpriteSM spriteSM2 = UnityEngine.Object.Instantiate(this.shoreWavesInnerCornerSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
				spriteSM2.transform.parent = base.transform;
				spriteSM2.transform.eulerAngles = new Vector3(0f, -90f, 0f);
				spriteSM2.name = "Shore Left and Down";
			}
			else if (!ocean3 && ocean4 && ocean2)
			{
				SpriteSM spriteSM3 = UnityEngine.Object.Instantiate(this.shoreWavesInnerCornerSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
				spriteSM3.transform.parent = base.transform;
				spriteSM3.transform.eulerAngles = new Vector3(0f, 0f, 0f);
				spriteSM3.name = "Shore Left and Up";
			}
		}
		if (!ocean2)
		{
			if (ocean3 && ocean4)
			{
				SpriteSM spriteSM4 = UnityEngine.Object.Instantiate(this.shoreWavesBorderSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
				spriteSM4.transform.parent = base.transform;
				spriteSM4.transform.eulerAngles = new Vector3(0f, 90f, 0f);
				spriteSM4.name = "Shore Right";
			}
			else if (ocean3 && !ocean4 && ocean)
			{
				SpriteSM spriteSM5 = UnityEngine.Object.Instantiate(this.shoreWavesInnerCornerSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
				spriteSM5.transform.parent = base.transform;
				spriteSM5.transform.eulerAngles = new Vector3(0f, 180f, 0f);
				spriteSM5.name = "Shore Right and Down";
			}
			else if (!ocean3 && ocean4 && ocean)
			{
				SpriteSM spriteSM6 = UnityEngine.Object.Instantiate(this.shoreWavesInnerCornerSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
				spriteSM6.transform.parent = base.transform;
				spriteSM6.transform.eulerAngles = new Vector3(0f, 90f, 0f);
				spriteSM6.name = "Shore Right and Up";
			}
		}
		if (!ocean3 && ocean && ocean2)
		{
			SpriteSM spriteSM7 = UnityEngine.Object.Instantiate(this.shoreWavesBorderSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
			spriteSM7.transform.parent = base.transform;
			spriteSM7.transform.eulerAngles = new Vector3(0f, 0f, 0f);
			spriteSM7.name = "Shore Up";
		}
		if (!ocean4 && ocean && ocean2)
		{
			SpriteSM spriteSM8 = UnityEngine.Object.Instantiate(this.shoreWavesBorderSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
			spriteSM8.transform.parent = base.transform;
			spriteSM8.transform.eulerAngles = new Vector3(0f, 180f, 0f);
			spriteSM8.name = "Shore Down";
		}
		if (!flag)
		{
			SpriteSM spriteSM9 = UnityEngine.Object.Instantiate(this.shoreWavesCornerSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
			spriteSM9.transform.parent = base.transform;
			spriteSM9.transform.eulerAngles = new Vector3(0f, 0f, 0f);
			spriteSM9.name = "Shore Corner UpperLeft";
		}
		if (!flag3)
		{
			SpriteSM spriteSM10 = UnityEngine.Object.Instantiate(this.shoreWavesCornerSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
			spriteSM10.transform.parent = base.transform;
			spriteSM10.transform.eulerAngles = new Vector3(0f, 90f, 0f);
			spriteSM10.name = "Shore Corner UpperRight";
		}
		if (!flag2)
		{
			SpriteSM spriteSM11 = UnityEngine.Object.Instantiate(this.shoreWavesCornerSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
			spriteSM11.transform.parent = base.transform;
			spriteSM11.transform.eulerAngles = new Vector3(0f, -90f, 0f);
			spriteSM11.name = "Shore Corner LowerLeft";
		}
		if (!flag4)
		{
			SpriteSM spriteSM12 = UnityEngine.Object.Instantiate(this.shoreWavesCornerSpritePrefab, base.transform.position + Vector3.up * 0.01f, Quaternion.identity) as SpriteSM;
			spriteSM12.transform.parent = base.transform;
			spriteSM12.transform.eulerAngles = new Vector3(0f, 180f, 0f);
			spriteSM12.name = "Shore Corner LowerRight";
		}
	}

	protected Vector2 GetBorderLowerLeftPixel()
	{
		return new Vector2(0f, 0f);
	}

	public WorldTerritory3D GetTerritory(int xDirection, int zDirection)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position + new Vector3((float)xDirection * 0.4f, 1f, (float)zDirection * 0.4f), Vector3.down, out raycastHit, 3f, 1 << LayerMask.NameToLayer("Ground")))
		{
			WorldMapTerritoryCollider component = raycastHit.collider.GetComponent<WorldMapTerritoryCollider>();
			if (component != null)
			{
				return component.territory;
			}
		}
		return null;
	}

	public bool GetOcean(int xDirection, int zDirection)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position + new Vector3((float)xDirection * 0.4f, 1f, (float)zDirection * 0.4f), Vector3.down, out raycastHit, 3f, 1 << LayerMask.NameToLayer("Ground")))
		{
			WorldMapTerritoryCollider component = raycastHit.collider.GetComponent<WorldMapTerritoryCollider>();
			if (component != null)
			{
				return component.isOcean;
			}
		}
		return true;
	}

	public void CheckBorders()
	{
	}

	public void ShowBorder()
	{
		foreach (SpriteSM spriteSM in this.borderSprites)
		{
			spriteSM.gameObject.SetActive(true);
		}
	}

	public void HideBorder()
	{
		foreach (SpriteSM spriteSM in this.borderSprites)
		{
			spriteSM.gameObject.SetActive(false);
		}
	}

	public void Raycast()
	{
		if (this.territory != null && !this.territory.IsSelected())
		{
			WorldTerritorySelector.SelectTerritoryStatic(this.territory);
		}
		else if (this.territory == null)
		{
			WorldTerritorySelector.SelectTerritoryStatic(null);
		}
	}

	private void Update()
	{
	}

	public Vector2 lowerLeftPixel = -Vector2.one;

	public TerritoryLayoutController layoutController;

	public WorldTerritory3D territory;

	public SpriteSM borderSpritePrefab;

	public SpriteSM borderCornerSpritePrefab;

	public SpriteSM shoreWavesBorderSpritePrefab;

	public SpriteSM shoreWavesCornerSpritePrefab;

	public SpriteSM shoreWavesInnerCornerSpritePrefab;

	public int row = -1;

	public int collumn = -1;

	public bool isOcean;

	public bool leftTerritorySame;

	public bool rightTerritorySame;

	public bool upTerritorySame;

	public bool downTerritorySame;

	public bool upperLeftTerritorySame;

	public bool upperRightTerritorySame;

	public bool lowerLeftTerritorySame;

	public bool lowerRightTerritorySame;

	[HideInInspector]
	public Material[] setToMaterials;

	protected List<SpriteSM> borderSprites = new List<SpriteSM>();
}
