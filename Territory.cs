// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Territory : MonoBehaviour
{
	public bool IsCompleted()
	{
		UnityEngine.Debug.LogError("Obsolete!");
		return false;
	}

	public bool IsAvailable()
	{
		if (this.IsCompleted())
		{
			return true;
		}
		foreach (Territory territory in this.surroundingTerritories)
		{
			if (territory.IsCompleted())
			{
				return true;
			}
		}
		return false;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public Territory[] surroundingTerritories;

	public MissionButton[] missions;
}
