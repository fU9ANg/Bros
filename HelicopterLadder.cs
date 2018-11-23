// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class HelicopterLadder : MonoBehaviour
{
	public void Leave()
	{
		base.transform.parent.SendMessage("Leave");
	}
}
