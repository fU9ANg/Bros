// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AssMouthBlock : BrickBlock
{
	public override void ShowForeground(bool isEarth, bool isBrick, bool onlyEdges)
	{
		base.ShowForeground(isEarth, isBrick, onlyEdges);
		if (this.orificeInstance != null)
		{
			UnityEngine.Object.Destroy(this.orificeInstance.gameObject);
		}
		if (this.IsEndCorner())
		{
			AssMouthBlock assMouthBlock = base.RightBlock as AssMouthBlock;
			if (assMouthBlock != null && assMouthBlock.IsEndCorner())
			{
				if (!base.IsAboveTheSame && !assMouthBlock.IsAboveTheSame)
				{
					this.orificeInstance = (UnityEngine.Object.Instantiate(this.MouthPrefab) as AssMouthOrifice);
					this.orificeInstance.transform.position = base.transform.position;
					this.orificeInstance.transform.position += Vector3.up * 16f;
					this.orificeInstance.transform.position += Vector3.right * 8f;
					this.orificeInstance.transform.position -= Vector3.forward * 8f;
					this.orificeInstance.rootA = this;
					this.orificeInstance.rootB = assMouthBlock;
				}
				if (!base.IsBelowTheSame && !assMouthBlock.IsBelowTheSame)
				{
					this.orificeInstance = (UnityEngine.Object.Instantiate(this.AssPrefab) as AssMouthOrifice);
					this.orificeInstance.transform.position = base.transform.position;
					this.orificeInstance.transform.position -= Vector3.up * 16f;
					this.orificeInstance.transform.position += Vector3.right * 8f;
					this.orificeInstance.transform.position -= Vector3.forward * 8f;
					this.orificeInstance.rootA = this;
					this.orificeInstance.rootB = assMouthBlock;
				}
			}
			AssMouthBlock assMouthBlock2 = this.belowBlock as AssMouthBlock;
			if (assMouthBlock2 != null && assMouthBlock2.IsEndCorner())
			{
				if (!base.IsLeftTheSame && !assMouthBlock2.IsLeftTheSame)
				{
					this.orificeInstance = (UnityEngine.Object.Instantiate(this.MouthPrefab) as AssMouthOrifice);
					this.orificeInstance.transform.position = base.transform.position;
					this.orificeInstance.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
					this.orificeInstance.transform.position += Vector3.left * 16f;
					this.orificeInstance.transform.position += Vector3.down * 8f;
					this.orificeInstance.transform.position -= Vector3.forward * 8f;
					this.orificeInstance.rootA = this;
					this.orificeInstance.rootB = assMouthBlock2;
				}
				if (!base.IsRightTheSame && !assMouthBlock2.IsRightTheSame)
				{
					this.orificeInstance = (UnityEngine.Object.Instantiate(this.MouthPrefab) as AssMouthOrifice);
					this.orificeInstance.transform.position = base.transform.position;
					this.orificeInstance.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
					this.orificeInstance.transform.position -= Vector3.left * 16f;
					this.orificeInstance.transform.position += Vector3.down * 8f;
					this.orificeInstance.transform.localScale = new Vector3(-1f, 1f, 1f);
					this.orificeInstance.transform.position -= Vector3.forward * 8f;
					this.orificeInstance.rootA = this;
					this.orificeInstance.rootB = assMouthBlock2;
				}
			}
		}
	}

	private bool IsEndCap()
	{
		int num = 0;
		if (base.IsAboveTheSame)
		{
			num++;
		}
		if (base.IsBelowTheSame)
		{
			num++;
		}
		if (base.IsLeftTheSame)
		{
			num++;
		}
		if (base.IsRightTheSame)
		{
			num++;
		}
		MonoBehaviour.print(num);
		return num == 1;
	}

	public bool IsEndCorner()
	{
		int num = 0;
		if (base.IsAboveTheSame)
		{
			num++;
		}
		if (base.IsBelowTheSame)
		{
			num++;
		}
		if (base.IsLeftTheSame)
		{
			num++;
		}
		if (base.IsRightTheSame)
		{
			num++;
		}
		return num == 2 && (!base.IsAboveTheSame || !base.IsBelowTheSame) && (!base.IsLeftTheSame || !base.IsRightTheSame);
	}

	private new void OnDestroy()
	{
		if (this.orificeInstance != null)
		{
			UnityEngine.Object.Destroy(this.orificeInstance.gameObject);
		}
	}

	public AssMouthOrifice AssPrefab;

	public AssMouthOrifice MouthPrefab;

	public AssMouthOrifice orificeInstance;
}
