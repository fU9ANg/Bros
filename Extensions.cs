// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public static class Extensions
{
    public static Vector3 Vec2toVec3(Vector2 v)
    {
        return new Vector3(v.x, v.y, 0f);
    }

    /*
	public static void SetChildrenActive(this GameObject go, bool active)
	{
		foreach (object obj in go.transform)
		{
			Transform transform = (Transform)obj;
			transform.gameObject.SetActive(active);
		}
	}
    */

    /*
	public static void BroadcastMessageToAllMonoBehaviours(this MonoBehaviour mono, string message)
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject gameObject in array)
		{
			gameObject.SendMessage(message, SendMessageOptions.DontRequireReceiver);
		}
	}
    */

    /*
	public static T GetComponentInHeirarchy<T>(this MonoBehaviour mono) where T : Component
	{
		Transform transform = mono.transform;
		T component;
		for (;;)
		{
			component = transform.GetComponent<T>();
			if (component != null)
			{
				break;
			}
			transform = transform.parent;
			if (!(transform != null))
			{
				goto Block_2;
			}
		}
		return component;
		Block_2:
		return (T)((object)null);
	}
    */

    /*
	public static T GetComponentInHeirarchy<T>(this GameObject go) where T : Component
	{
		Transform transform = go.transform;
		while (transform.parent != null)
		{
			transform = transform.parent;
		}
		return transform.GetComponentInChildren<T>();
	}
    */

    /*
	public static T Clone<T>(this T monobehaviour) where T : MonoBehaviour
	{
		return UnityEngine.Object.Instantiate(monobehaviour) as T;
	}
    */

	public static GameObject Clone(this GameObject monobehaviour)
	{
		return UnityEngine.Object.Instantiate(monobehaviour) as GameObject;
	}

	public static float Distance(this Vector3 from, Vector3 to)
	{
		return Vector3.Distance(from, to);
	}

	public static float Distance(this Vector2 from, Vector2 to)
	{
		return Vector2.Distance(from, to);
	}

	public static float ManhattanDistance(this Vector3 from, Vector3 to)
	{
		return Mathf.Abs(from.x - to.x) + Mathf.Abs(from.y - to.y) + Mathf.Abs(from.z - to.z);
	}

    /*
	public static float ManhattanDistance(this Vector2 from, Vector2 to)
	{
		return Mathf.Abs(from.x - to.x) + Mathf.Abs(from.y - to.y);
	}
    */

	public static void SetOwnerNetworked(this GameObject go, PID newOwner)
	{
		Networking.RPC<GameObject, PID>(PID.TargetAll, new RpcSignature<GameObject, PID>(Extensions.SetOwnerLocal), go, newOwner, false);
	}

	public static void SetOwnerLocal(this GameObject go, PID newOwner)
	{
		NetworkObject[] componentsInChildren = go.GetComponentsInChildren<NetworkObject>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetOwner(newOwner);
		}
	}

    /*
	public static T RandomElement<T>(this T[] array)
	{
		return array[UnityEngine.Random.Range(0, array.Length)];
	}
    */

    /*
	public static Vector3 BottomLeftWorldPos(this Camera cam)
	{
		return cam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
	}
    */

	public static Vector3 BottomRightWorldPos(this Camera cam)
	{
		return cam.ScreenToWorldPoint(new Vector3((float)Screen.width, 0f, 0f));
	}

    /*
	public static int RandomIndex<T>(this T[] array)
	{
		return UnityEngine.Random.Range(0, array.Length);
	}
    */

	public static void InitializeValues<T>(this T[] array, T value)
	{
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = value;
		}
	}

    /*
	public static void StripNullValues(this IDictionary original)
	{
		object[] array = new object[original.Count];
		original.Keys.CopyTo(array, 0);
		foreach (object obj in array)
		{
			if (obj == null || original[obj] == null)
			{
				original.Remove(obj);
			}
		}
	}
    */

    /*
	public static bool Contains<T>(this Array array, T element)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (element.Equals((T)((object)array.GetValue(i))))
			{
				return true;
			}
		}
		return false;
	}
    */

    /*
	public static GameObject GetGameObject(this UnityEngine.Object obj)
	{
		Component component = obj as Component;
		if (component != null)
		{
			return component.gameObject;
		}
		return obj as GameObject;
	}
    */

    /*
	public static void SetX(this Transform t, float value)
	{
		Vector3 position = t.position;
		position.x = value;
		t.position = position;
	}
    */

	public static void SetY(this Transform t, float value)
	{
		Vector3 position = t.position;
		position.y = value;
		t.position = position;
	}

	public static void SetZ(this Transform t, float value)
	{
		Vector3 position = t.position;
		position.z = value;
		t.position = position;
	}

    /*
	public static void SetLocalX(this Transform t, float value)
	{
		Vector3 localPosition = t.localPosition;
		localPosition.x = value;
		t.localPosition = localPosition;
	}
    */

    /*
	public static void SetLocalY(this Transform t, float value)
	{
		Vector3 localPosition = t.localPosition;
		localPosition.y = value;
		t.localPosition = localPosition;
	}
    */

	public static void SetLocalZ(this Transform t, float value)
	{
		Vector3 localPosition = t.localPosition;
		localPosition.z = value;
		t.localPosition = localPosition;
	}

	public static void SetX(ref Vector3 v, float value)
	{
		v.x = value;
	}

	public static void SetY(ref Vector3 v, float value)
	{
		v.y = value;
	}

	public static void SetZ(ref Vector3 v, float value)
	{
		v.z = value;
	}

	public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hit, float distance, int layerMask, bool debugDraw, Color color, float debugDuration = 0f)
	{
		if (debugDraw)
		{
			UnityEngine.Debug.DrawRay(origin, direction * distance, color, debugDuration);
		}
		return Physics.Raycast(origin, direction, out hit, distance, layerMask);
	}

	public static void Invoke(this MonoBehaviour mono, Extensions.MethodSignature method, float time)
	{
		mono.Invoke(method.Method.Name, time);
	}

	public static void DrawRect(Vector3 origin, float width, float height, Color color, float time)
	{
		Vector3 vector = origin + new Vector3(-width, height, 0f) / 2f;
		Vector3 vector2 = origin + new Vector3(width, height, 0f) / 2f;
		Vector3 vector3 = origin + new Vector3(width, -height, 0f) / 2f;
		Vector3 vector4 = origin + new Vector3(-width, -height, 0f) / 2f;
		UnityEngine.Debug.DrawLine(vector, vector2, color, time);
		UnityEngine.Debug.DrawLine(vector2, vector3, color, time);
		UnityEngine.Debug.DrawLine(vector3, vector4, color, time);
		UnityEngine.Debug.DrawLine(vector4, vector, color, time);
	}

	public static void DrawCircle(Vector3 origin, float radius, Color color, float duration)
	{
		Vector3 start = Vector3.up * radius + origin;
		int num = 64;
		for (int i = 1; i < num + 1; i++)
		{
			float num2 = (float)i / (float)num;
			num2 *= 360f;
			Vector3 vector = Quaternion.AngleAxis(num2, Vector3.forward) * Vector3.up * radius + origin;
			UnityEngine.Debug.DrawLine(start, vector, color, duration);
			start = vector;
		}
	}

	public static void OffsetLowerLeftPixel(this SpriteSM sprite, Vector2 offset)
	{
		sprite.SetLowerLeftPixel(sprite.lowerLeftPixel + offset);
	}

	public static void OffsetLowerLeftPixel(this SpriteSM sprite, float x, float y)
	{
		sprite.SetLowerLeftPixel(sprite.lowerLeftPixel + new Vector2(x, y));
	}

	public static void SetLowerLeftPixel_X(this SpriteSM sprite, float x)
	{
		sprite.SetLowerLeftPixel(x, sprite.lowerLeftPixel.y);
	}

	public static void SetLowerLeftPixel_Y(this SpriteSM sprite, float y)
	{
		sprite.SetLowerLeftPixel(sprite.lowerLeftPixel.x, y);
	}

    /*
	public static void SetAlpha(this Material mat, float alpha)
	{
		Color color = mat.GetColor("_TintColor");
		color.a = alpha;
		mat.SetColor("_TintColor", color);
	}
    */

	public static void SetColor(this Material mat, Color col)
	{
		mat.SetColor("_TintColor", col);
	}

    /*
	public static void SetParticleColor(this ParticleSystem particleSystem, Color col)
	{
		ParticleSystem.Particle[] array = new ParticleSystem.Particle[particleSystem.particleCount];
		particleSystem.GetParticles(array);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].color = col;
		}
		particleSystem.SetParticles(array, array.Length);
		particleSystem.startColor = col;
	}
    */

	public delegate void MethodSignature();

	public delegate void MethodSignature<T1>();
}
