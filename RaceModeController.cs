// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class RaceModeController : MonoBehaviour
{
	private void Start()
	{
		UnityEngine.Debug.Log("Added Race Mode Controller ");
	}

	private void Update()
	{
		if (SortOfFollow.GetInstance() == null)
		{
			return;
		}
		switch (SortOfFollow.GetFollowMode())
		{
		case CameraFollowMode.Normal:
			UnityEngine.Debug.LogError("Normal Follow Mode no Implemented ! ");
			base.enabled = false;
			break;
		case CameraFollowMode.Vertical:
			if (Map.HitLivingUnits(this, -1, 1, DamageType.Crush, 400f, 400f, SortOfFollow.GetScreenMinX() + SortOfFollow.GetWorldScreenWidth() * 0.5f, SortOfFollow.GetScreenMinY() - 400f, 0f, 100f, true, true))
			{
				UnityEngine.Debug.Log("Hit bottom players");
			}
			break;
		case CameraFollowMode.Horizontal:
			if (Map.HitLivingUnits(this, -1, 1, DamageType.Crush, 400f, 400f, SortOfFollow.GetScreenMinX() - 400f, SortOfFollow.GetScreenMinY() + SortOfFollow.GetWorldScreenHeight() * 0.5f, 200f, 100f, true, true))
			{
				UnityEngine.Debug.Log("Hit left players");
			}
			break;
		default:
			UnityEngine.Debug.LogError("This Follow Mode no Implemented ! " + SortOfFollow.GetFollowMode());
			base.enabled = false;
			break;
		}
	}
}
