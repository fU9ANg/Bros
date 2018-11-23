// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class SpriteAnimationPump : MonoBehaviour
{
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void StartAnimationPump()
	{
		if (!SpriteAnimationPump.pumpIsRunning)
		{
			base.StartCoroutine(SpriteAnimationPump.AnimationPump());
		}
	}

	protected static IEnumerator AnimationPump()
	{
		float startTime = Time.realtimeSinceStartup;
		SpriteAnimationPump.pumpIsRunning = true;
		while (SpriteAnimationPump.pumpIsRunning)
		{
			yield return new WaitForSeconds(SpriteAnimationPump.animationPumpInterval);
			float time = Time.realtimeSinceStartup;
			float elapsed = time - startTime;
			startTime = time;
			for (int i = 0; i < SpriteAnimationPump.animList.Count; i++)
			{
				((SpriteBase)SpriteAnimationPump.animList[i]).StepAnim(elapsed);
			}
		}
		yield break;
	}

	public static SpriteAnimationPump Instance
	{
		get
		{
			if (SpriteAnimationPump.instance == null)
			{
				GameObject gameObject = new GameObject("SpriteAnimationPump");
				SpriteAnimationPump.instance = (SpriteAnimationPump)gameObject.AddComponent(typeof(SpriteAnimationPump));
			}
			return SpriteAnimationPump.instance;
		}
	}

	public static void Add(SpriteBase s)
	{
		SpriteAnimationPump.animList.Add(s);
	}

	public static void Remove(SpriteBase s)
	{
		SpriteAnimationPump.animList.Remove(s);
	}

	private static SpriteAnimationPump instance = null;

	protected static ArrayList animList = new ArrayList();

	public static bool pumpIsRunning = false;

	public static float animationPumpInterval = 0.03333f;
}
