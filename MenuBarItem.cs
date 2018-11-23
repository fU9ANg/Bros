// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[Serializable]
public class MenuBarItem
{
	public MenuBarItem()
	{
	}

	public MenuBarItem(MenuBarItem toCopy)
	{
		this.name = toCopy.name;
		this.invokeMethod = toCopy.invokeMethod;
		this.size = toCopy.size;
		this.color = toCopy.color;
		this.storage = toCopy.storage;
	}

	public string name;

	public string invokeMethod;

	public float size = 3f;

	public Color color = Color.white;

	public object storage;

	public bool isBetaAccess;

	public bool isAlphaAccess;

	public float alphaBetaTextXOffset = 80f;
}
