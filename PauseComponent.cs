// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class PauseComponent : MonoBehaviour
{
	public event PauseComponent.PauseEventHandler gamePausedChangedEvent;

	public void NotifyPause(bool paused)
	{
		if (this.gamePausedChangedEvent != null)
		{
			this.gamePausedChangedEvent(paused);
		}
	}

	public bool PauseOnCutscene = true;

	public bool PauseOnMenu = true;

	public delegate void PauseEventHandler(bool paused);
}
