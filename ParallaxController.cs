// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
	public static void ResetParallaxGlobalValues(float setParallaxSpeed)
	{
		ParallaxController.parallaxXOffset = 0f;
		ParallaxController.parallaxXSpeed = setParallaxSpeed;
	}

	public static float ParallaxXSpeed
	{
		get
		{
			return ParallaxController.parallaxXSpeed;
		}
	}

	public static float ParallaxXOffset
	{
		get
		{
			return ParallaxController.parallaxXOffset;
		}
	}

	private void Awake()
	{
		ParallaxController.parallaxXOffset = 0f;
	}

	private void Start()
	{
		ParallaxController.parallaxXSpeed = this.setParallaxXSpeed;
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.03334f);
		ParallaxController.parallaxXOffset -= ParallaxController.ParallaxXSpeed * num;
	}

	public float setParallaxXSpeed;

	protected static float parallaxXSpeed;

	protected static float parallaxXOffset;
}
