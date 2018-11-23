// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DisplayVersionNumber : MonoBehaviour
{
	private void Start()
	{
		base.GetComponent<TextMesh>().text = "V: " + VersionNumber.version;
	}

	private void Update()
	{
	}
}
