// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class HeroControllerTestInfo : MonoBehaviour
{
	private void Awake()
	{
		HeroController.SetTestInfo(this.alwaysChooseHero);
		if (this.herosAreInvulnerable)
		{
			UnityEngine.Debug.LogWarning("HEROS ARE INVULNERABLE");
			HeroControllerTestInfo.HerosAreInvulnerable = this.herosAreInvulnerable;
		}
	}

	public HeroType alwaysChooseHero = HeroType.None;

	public bool herosAreInvulnerable;

	public static bool HerosAreInvulnerable;
}
