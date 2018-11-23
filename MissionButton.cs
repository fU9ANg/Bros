// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MissionButton : SimpleButton
{
	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		this.buttonNormalMaterial = base.GetComponent<Renderer>().sharedMaterial;
	}

	protected override void PressButton()
	{
		UnityEngine.Debug.LogError("Obsolete!");
	}

	protected override void Update()
	{
		base.Update();
		if (!Application.isEditor || Input.GetKeyDown(KeyCode.F12))
		{
		}
	}

	public string campaignName;

	public int minPrestigeLevel;
}
