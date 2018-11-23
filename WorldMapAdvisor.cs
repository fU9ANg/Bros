// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldMapAdvisor : MonoBehaviour
{
	private void Awake()
	{
		WorldMapAdvisor.instance = this;
	}

	public static void Activate()
	{
		WorldMapAdvisor.instance.animatedTexture.PlayAnimation("Salute");
	}

	public static void TalkAtEase()
	{
		WorldMapAdvisor.instance.animatedTexture.PlayAnimation("TalkAtEase");
	}

	public static void TalkAtSalute()
	{
		WorldMapAdvisor.instance.animatedTexture.PlayAnimation("TalkAtSalute");
	}

	public static void Salute()
	{
		WorldMapAdvisor.instance.animatedTexture.PlayAnimation("Salute");
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		this.counter += num;
	}

	public AnimatedTextureUltra animatedTexture;

	protected float counter;

	protected static WorldMapAdvisor instance;
}
