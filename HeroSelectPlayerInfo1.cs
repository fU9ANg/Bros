// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class HeroSelectPlayerInfo1 : MonoBehaviour
{
	public Color color
	{
		get
		{
			return HeroController.GetHeroColor(this.PlayerNumber);
		}
	}

	private IEnumerator Start()
	{
		this.playerName.color = this.color;
		for (;;)
		{
			float lerp = Mathf.Lerp(base.transform.position.x, this.GetTargetX(), Time.deltaTime * 20f);
			base.transform.SetX(lerp);
			yield return null;
		}
		yield break;
	}

	private float GetTargetX()
	{
		float num = 30f;
		int num2 = SingletonMono<NewHeroSelectController1>.Instance.PlayerOrder[this.PlayerNumber];
		float num3 = (float)num2 * num - 1.5f * num;
		return -num3;
	}

	private void Awake()
	{
		base.transform.SetX(this.GetTargetX());
	}

	public void SetCharacter(CharacterHolder1 holder)
	{
		this.currentCharacter = holder;
		if (this.currentCharacter != null)
		{
			this.avatar.sharedMaterial = HeroController.GetAvatarMaterial(this.currentCharacter.heroType);
		}
		else
		{
			this.avatar.sharedMaterial = HeroController.GetAvatarMaterial(HeroType.None);
		}
	}

	public void SetSelection()
	{
		this.isChoosingHero = false;
		this.currentCharacter.hasBeenSelected = true;
	}

	public SpriteSM[] fists;

	public int PlayerNumber;

	public CharacterHolder1 currentCharacter;

	public TextMesh playerName;

	public bool isChoosingHero;

	public Renderer avatar;
}
