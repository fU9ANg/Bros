// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MechAnimationListener : MonoBehaviour
{
	private void Start()
	{
		base.GetComponent<Animation>().Play("Idle", PlayMode.StopAll);
	}

	public void ExecuteJump()
	{
		UnityEngine.Debug.Log("EXecture Jum,p ~!");
		this.mech.ExecuteJump();
	}

	private void LateUpdate()
	{
		if (!base.GetComponent<Animation>().isPlaying && base.GetComponent<Animation>().clip != base.GetComponent<Animation>()["Idle"])
		{
			UnityEngine.Debug.Log("Play Idle ! " + ((!(base.GetComponent<Animation>().clip != null)) ? "Null" : base.GetComponent<Animation>().clip.name));
			base.GetComponent<Animation>().Play("Idle", PlayMode.StopAll);
		}
	}

	public Mech mech;

	public bool forceIdle = true;
}
