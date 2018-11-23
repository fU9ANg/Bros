// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DisableWhenOffCamera : MonoBehaviour
{
	private static Camera MainCamera
	{
		get
		{
			if (DisableWhenOffCamera.mainCamera == null)
			{
				DisableWhenOffCamera.mainCamera = Camera.main;
			}
			return DisableWhenOffCamera.mainCamera;
		}
	}

	private void Start()
	{
		this.mook = base.GetComponent<Unit>();
		this.mookAI = base.GetComponent<PolymorphicAI>();
	}

	private void Update()
	{
		this.timer -= Time.deltaTime;
		if (this.timer > 0f)
		{
			return;
		}
		this.timer = this.interval;
		Vector3 vector = DisableWhenOffCamera.MainCamera.WorldToScreenPoint(base.transform.position);
		float x = vector.x;
		float y = vector.y;
		Vector3 vector2 = DisableWhenOffCamera.MainCamera.WorldToScreenPoint(DisableWhenOffCamera.MainCamera.transform.position);
		float num = (float)Screen.height * 1f;
		float num2 = (float)Screen.width * 1f;
		if (this.mookAI == null)
		{
			return;
		}
		if (this.mook.beingControlledByTriggerAction)
		{
			this.Enable();
		}
		else if (!this.mookAI.IsMine && this.mookAI.Syncronize)
		{
			this.Disable();
		}
		else if (x > vector2.x + num2)
		{
			this.Disable();
		}
		else if (x < vector2.x - num2)
		{
			this.Disable();
		}
		else if (y > vector2.y + num)
		{
			this.Disable();
		}
		else if (y < vector2.y - num)
		{
			this.Disable();
		}
		else
		{
			this.Enable();
		}
	}

	private void Disable()
	{
		if (this.mook != null && this.mook.enabled && this.mook.actionState != ActionState.Jumping)
		{
			this.mook.enabled = false;
			if (this.mookAI != null)
			{
				this.mookAI.enabled = false;
			}
			if (this.mook.actionState == ActionState.Dead)
			{
				this.mookAI.enabled = false;
			}
		}
	}

	private void Enable()
	{
		if (this.mookAI.IsMine || !this.mookAI.Syncronize)
		{
			if (this.mook != null)
			{
				this.mook.enabled = true;
			}
			if (this.mookAI != null)
			{
				this.mookAI.enabled = true;
			}
			if (this.mook.actionState == ActionState.Dead)
			{
				this.mookAI.enabled = false;
			}
		}
	}

	private Unit mook;

	private PolymorphicAI mookAI;

	private bool hasBeenEnabled;

	private float timer;

	private float interval = 0.2f;

	private static Camera mainCamera;
}
