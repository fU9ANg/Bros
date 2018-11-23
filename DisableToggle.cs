// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DisableToggle : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			this.is0Enabled = !this.is0Enabled;
			foreach (GameObject gameObject in this.objects0)
			{
				if (gameObject.activeSelf != this.is0Enabled)
				{
					gameObject.SetActive(this.is0Enabled);
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			this.is9Enabled = !this.is9Enabled;
			foreach (GameObject gameObject2 in this.objects9)
			{
				if (gameObject2.activeSelf != this.is9Enabled)
				{
					gameObject2.SetActive(this.is9Enabled);
				}
			}
		}
	}

	public GameObject[] objects0;

	public GameObject[] objects9;

	private bool is0Enabled = true;

	private bool is9Enabled = true;
}
