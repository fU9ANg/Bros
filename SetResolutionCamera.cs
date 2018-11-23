// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SetResolutionCamera : MonoBehaviour
{
	private void Start()
	{
		SetResolutionCamera.mainCamera = base.GetComponent<Camera>();
		if (Application.platform == RuntimePlatform.WindowsPlayer && SetResolutionCamera.mainCamera != null)
		{
			SetResolutionCamera.mainCamera.aspect = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
		}
	}

	public static bool AreBothVisible(Vector3 worldPos1, Vector3 worldPos2, ref bool failedWidth, ref bool failedHeight)
	{
		Vector3 vector = SetResolutionCamera.mainCamera.WorldToScreenPoint(worldPos1);
		Vector3 vector2 = SetResolutionCamera.mainCamera.WorldToScreenPoint(worldPos2);
		float f = vector.x - vector2.x;
		if (Mathf.Abs(f) > (float)(Screen.width + 4))
		{
			failedWidth = true;
			return false;
		}
		failedWidth = false;
		float f2 = vector.y - vector2.y;
		if (Mathf.Abs(f2) > (float)(Screen.height + 4))
		{
			failedHeight = true;
			return false;
		}
		failedHeight = false;
		return true;
	}

	public static bool IsItVisible(Vector3 worldPos)
	{
		Vector3 vector = SetResolutionCamera.mainCamera.WorldToScreenPoint(worldPos);
		if (vector.x > -4f && vector.x < (float)(Screen.width + 4))
		{
			if (vector.y > 0f && vector.y < (float)Screen.height)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsBelowScreen(Vector3 worldPos)
	{
		return SetResolutionCamera.mainCamera.WorldToScreenPoint(worldPos).y < 0f;
	}

	public static float GetXEdge(int direction)
	{
		if (direction > 0)
		{
			return SetResolutionCamera.mainCamera.ScreenToWorldPoint(new Vector3((float)Screen.width, 10f, 10f)).x;
		}
		if (direction < 0)
		{
			return SetResolutionCamera.mainCamera.ScreenToWorldPoint(new Vector3(0f, 10f, 10f)).x;
		}
		return 0f;
	}

	public static void GetScreenExtents(ref float minX, ref float maxX, ref float minY, ref float maxY)
	{
		Vector3 vector = SetResolutionCamera.mainCamera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 10f));
		maxX = vector.x;
		maxY = vector.y;
		vector = SetResolutionCamera.mainCamera.ScreenToWorldPoint(new Vector3(0f, 0f, 10f));
		minX = vector.x;
		minY = vector.y;
	}

	public static bool IsItSortOfVisible(Vector3 worldPos)
	{
		return SetResolutionCamera.IsItSortOfVisible(worldPos, 64f, 64f);
	}

	public static bool IsItSortOfVisible(Vector3 worldPos, float xBuffer, float yBuffer)
	{
		Vector3 vector = SetResolutionCamera.mainCamera.WorldToScreenPoint(worldPos);
		return vector.x > -xBuffer && vector.x < (float)Screen.width + xBuffer && vector.y > -yBuffer && vector.y < (float)Screen.height + yBuffer;
	}

	private void Update()
	{
		bool flag = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
		if (flag && Input.GetKeyDown(KeyCode.F5))
		{
			if (Screen.fullScreen)
			{
				Screen.SetResolution((int)((float)Screen.width / 1.5f), (int)((float)Screen.height / 1.5f), false);
			}
			else
			{
				Screen.SetResolution(Screen.width, Screen.height, true);
			}
		}
	}

	private static Camera mainCamera;
}
