// dnSpy decompiler from Assembly-CSharp.dll
using System;
using EaserCore;
using UnityEngine;

public class Easer
{
	public static void Initialize()
	{
		if (Easer._initialized)
		{
			return;
		}
		EaserDataManager.Init();
		Easer._initialized = true;
	}

	public static float Ease(EaserEase easeType, float from, float to, float t)
	{
		if (!Easer._initialized)
		{
			Easer.Initialize();
		}
		AnimationCurve curve = EaserDataManager.GetCurve(easeType);
		float num;
		if (curve != null)
		{
			num = curve.Evaluate(t);
		}
		else
		{
			num = Mathf.Lerp(0f, 1f, t);
		}
		return from + (to - from) * num;
	}

	public static float PingPong(EaserEase easeType, float from, float to, float t)
	{
		return Easer.Ease(easeType, from, to, Mathf.PingPong(t, 1f));
	}

	public static Vector2 EaseVector2(EaserEase easeType, Vector2 from, Vector2 to, float t)
	{
		return new Vector2(Easer.Ease(easeType, from.x, to.x, t), Easer.Ease(easeType, from.y, to.y, t));
	}

	public static Vector3 EaseVector3(EaserEase easeType, Vector3 from, Vector3 to, float t)
	{
		return new Vector3(Easer.Ease(easeType, from.x, to.x, t), Easer.Ease(easeType, from.y, to.y, t), Easer.Ease(easeType, from.z, to.z, t));
	}

	public static Vector3 EaseVector4(EaserEase easeType, Vector4 from, Vector4 to, float t)
	{
		return new Vector4(Easer.Ease(easeType, from.x, to.x, t), Easer.Ease(easeType, from.y, to.y, t), Easer.Ease(easeType, from.z, to.z, t), Easer.Ease(easeType, from.w, to.w, t));
	}

	public static Quaternion EaseQuaternion(EaserEase easeType, Quaternion from, Quaternion to, float t)
	{
		float t2 = Easer.Ease(easeType, 0f, 1f, t);
		return Quaternion.Lerp(from, to, t2);
	}

	public static Color EaseColor(EaserEase easeType, Color from, Color to, float t)
	{
		return new Color(Easer.Ease(easeType, from.r, to.r, t), Easer.Ease(easeType, from.g, to.g, t), Easer.Ease(easeType, from.b, to.b, t), Easer.Ease(easeType, from.a, to.a, t));
	}

	public const string DATA_PATH = "EaserOutput/Resources";

	public const string DATA_FILENAME = "easer_data.asset";

	public const string ENUM_PATH = "EaserOutput/Enums";

	private static bool _initialized;
}
