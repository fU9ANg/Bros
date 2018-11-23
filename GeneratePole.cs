// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class GeneratePole : MonoBehaviour
{
	private void Awake()
	{
		int row = Map.GetRow(base.transform.position.y);
		int row2 = Map.GetRow(base.transform.position.x);
		for (int i = 1; i < row; i++)
		{
			if (Map.GetBlock(row2, row - i) != null)
			{
				break;
			}
			if (Map.GetBlock(row2, row - i) == null)
			{
				if (Map.GetBlock(row2, row - i - 1) != null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(this.poleBase) as GameObject;
					float num = (float)(i * 16);
					gameObject.transform.parent = base.transform;
					gameObject.transform.localPosition = new Vector3(0f, -num, 0f);
					break;
				}
				GameObject gameObject2 = UnityEngine.Object.Instantiate(this.poleMid) as GameObject;
				float num2 = (float)(i * 16);
				gameObject2.transform.parent = base.transform;
				gameObject2.transform.localPosition = new Vector3(0f, -num2, 0f);
			}
		}
		this.poleMid.SetActive(false);
		this.poleBase.SetActive(false);
	}

	private void Update()
	{
	}

	public GameObject poleMid;

	public GameObject poleBase;
}
