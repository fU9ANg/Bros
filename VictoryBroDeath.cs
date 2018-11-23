// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class VictoryBroDeath : VictoryMookDeath
{
	public override void Setup(DeathObject deathObject, float time, Transform parent, ShakeM shakeObject)
	{
		UnityEngine.Debug.Log(" BRO VICTORY DEATH!!:" + deathObject.heroType);
		base.Setup(deathObject, time, parent, shakeObject);
		this.deathCountDown += 0.7f;
		if (deathObject != null && deathObject.heroType != HeroType.None)
		{
			switch (deathObject.heroType)
			{
			case HeroType.Rambro:
				base.GetComponent<Renderer>().material = this.materialRambro;
				this.gunSprite.GetComponent<Renderer>().material = this.materialRambroGun;
				break;
			case HeroType.Brommando:
				base.GetComponent<Renderer>().material = this.materialRambro;
				this.gunSprite.GetComponent<Renderer>().material = this.materialRambroGun;
				break;
			case HeroType.BaBroracus:
				base.GetComponent<Renderer>().material = this.materialBABroracus;
				this.gunSprite.GetComponent<Renderer>().material = this.materialBABroracusGun;
				break;
			case HeroType.BrodellWalker:
				base.GetComponent<Renderer>().material = this.materialBrodelWalker;
				this.gunSprite.GetComponent<Renderer>().material = this.materialBrodelWalkerGun;
				break;
			case HeroType.Blade:
				base.GetComponent<Renderer>().material = this.materialBlade;
				this.gunSprite.GetComponent<Renderer>().material = this.materialBladeGun;
				break;
			case HeroType.McBrover:
				base.GetComponent<Renderer>().material = this.materialMcBrover;
				this.gunSprite.GetComponent<Renderer>().material = this.materialMcBroverGun;
				break;
			case HeroType.Brononymous:
				base.GetComponent<Renderer>().material = this.materialBroInBlack;
				this.gunSprite.GetComponent<Renderer>().material = this.materialBroInBlackGun;
				break;
			case HeroType.Brobocop:
				base.GetComponent<Renderer>().material = this.materialRobrocop;
				this.gunSprite.GetComponent<Renderer>().material = this.materialRobrocopGun;
				break;
			case HeroType.BroDredd:
				base.GetComponent<Renderer>().material = this.materialBroDredd;
				this.gunSprite.GetComponent<Renderer>().material = this.materialBroDreddGun;
				break;
			case HeroType.BroHard:
				base.GetComponent<Renderer>().material = this.materialBroHard;
				this.gunSprite.GetComponent<Renderer>().material = this.materialBroHardGun;
				break;
			case HeroType.SnakeBroSkin:
				base.GetComponent<Renderer>().material = this.materialSnakeBroSkin;
				this.gunSprite.GetComponent<Renderer>().material = this.materialSnakeBroSkinGun;
				break;
			case HeroType.Brominator:
				base.GetComponent<Renderer>().material = this.materialBrominator;
				this.gunSprite.GetComponent<Renderer>().material = this.materialBrominatorGun;
				break;
			case HeroType.IndianaBrones:
				base.GetComponent<Renderer>().material = this.materialIndianna;
				this.gunSprite.GetComponent<Renderer>().material = this.materialIndiannaGun;
				break;
			case HeroType.AshBrolliams:
				base.GetComponent<Renderer>().material = this.materialAshBrolliams;
				this.gunSprite.GetComponent<Renderer>().material = this.materialAshBrolliamsGun;
				break;
			case HeroType.Nebro:
				base.GetComponent<Renderer>().material = this.materialNebro;
				this.gunSprite.GetComponent<Renderer>().material = this.materialNebroGun;
				break;
			case HeroType.Brochete:
				base.GetComponent<Renderer>().material = this.materialBrochete;
				this.gunSprite.GetComponent<Renderer>().material = this.materialBrocheteGun;
				break;
			case HeroType.BronanTheBrobarian:
				base.GetComponent<Renderer>().material = this.materialBronanTheBrobarian;
				this.gunSprite.GetComponent<Renderer>().material = this.materialBronanTheBrobarianGun;
				break;
			case HeroType.EllenRipbro:
				base.GetComponent<Renderer>().material = this.materialEllenRipbro;
				this.gunSprite.GetComponent<Renderer>().material = this.materialEllenRipbroGun;
				break;
			case HeroType.CherryBroling:
				base.GetComponent<Renderer>().material = this.materialEllenRipbro;
				this.gunSprite.GetComponent<Renderer>().material = this.materialEllenRipbroGun;
				break;
			case HeroType.TimeBroVanDamme:
				base.GetComponent<Renderer>().material = this.materialTimeBroVanDamme;
				this.gunSprite.GetComponent<Renderer>().material = this.materialTimeBroVanDammeGun;
				break;
			case HeroType.ColJamesBroddock:
				base.GetComponent<Renderer>().material = this.materialColJamesBroddock;
				this.gunSprite.GetComponent<Renderer>().material = this.materialColJamesBroddockGun;
				break;
			case HeroType.BroniversalSoldier:
				base.GetComponent<Renderer>().material = this.materialBroniversalSoldier;
				this.gunSprite.GetComponent<Renderer>().material = this.materialBroniversalSoldierGun;
				break;
			case HeroType.BroneyRoss:
				base.GetComponent<Renderer>().material = this.materialBroneyRoss;
				this.gunSprite.GetComponent<Renderer>().material = this.materialBroneyRossGun;
				break;
			case HeroType.LeeBroxmas:
				base.GetComponent<Renderer>().material = this.materialLeeBroxmas;
				this.gunSprite.GetComponent<Renderer>().material = this.materialLeeBroxmasGun;
				break;
			case HeroType.BronnarJensen:
				base.GetComponent<Renderer>().material = this.materialBronnarJensen;
				this.gunSprite.GetComponent<Renderer>().material = this.materialBronnarJensenGun;
				break;
			case HeroType.HaleTheBro:
				base.GetComponent<Renderer>().material = this.materialHaleTheBro;
				this.gunSprite.GetComponent<Renderer>().material = this.materialHaleTheBroGun;
				break;
			case HeroType.TrentBroser:
				base.GetComponent<Renderer>().material = this.materialTrentBroser;
				this.gunSprite.GetComponent<Renderer>().material = this.materialTrentBroserGun;
				break;
			case HeroType.Broc:
				base.GetComponent<Renderer>().material = this.materialBroc;
				this.gunSprite.GetComponent<Renderer>().material = this.materialBrocGun;
				break;
			case HeroType.TollBroad:
				base.GetComponent<Renderer>().material = this.materialTollBroad;
				this.gunSprite.GetComponent<Renderer>().material = this.materialTollBroadGun;
				break;
			}
			this.sprite.RecalcTexture();
			this.sprite.SetSize(this.sprite.width, this.sprite.height);
			this.sprite.SetLowerLeftPixel(new Vector2(this.sprite.lowerLeftPixel.x, this.sprite.lowerLeftPixel.y));
			this.gunSprite.RecalcTexture();
		}
		else
		{
			UnityEngine.Debug.LogError("Bust HeroType. deathObject " + (deathObject != null));
		}
	}

	protected override void SetBloodSplat()
	{
		base.GetComponent<Renderer>().enabled = false;
		this.gunSprite.GetComponent<Renderer>().enabled = false;
	}

	public Material materialRambro;

	public Material materialRambroGun;

	public Material materialBrommando;

	public Material materialBrommandoGun;

	public Material materialBABroracus;

	public Material materialBABroracusGun;

	public Material materialBrodelWalker;

	public Material materialBrodelWalkerGun;

	public Material materialBroHard;

	public Material materialBroHardGun;

	public Material materialMcBrover;

	public Material materialMcBroverGun;

	public Material materialBroDredd;

	public Material materialBroDreddGun;

	public Material materialBroInBlack;

	public Material materialBroInBlackGun;

	public Material materialBlade;

	public Material materialBladeGun;

	public Material materialRobrocop;

	public Material materialRobrocopGun;

	public Material materialBrominator;

	public Material materialBrominatorGun;

	public Material materialSnakeBroSkin;

	public Material materialSnakeBroSkinGun;

	public Material materialIndianna;

	public Material materialIndiannaGun;

	public Material materialAshBrolliams;

	public Material materialAshBrolliamsGun;

	public Material materialNebro;

	public Material materialNebroGun;

	public Material materialBrochete;

	public Material materialBrocheteGun;

	public Material materialBronanTheBrobarian;

	public Material materialBronanTheBrobarianGun;

	public Material materialEllenRipbro;

	public Material materialEllenRipbroGun;

	public Material materialCherryBroling;

	public Material materialCherryBrolingGun;

	public Material materialTimeBroVanDamme;

	public Material materialTimeBroVanDammeGun;

	public Material materialColJamesBroddock;

	public Material materialColJamesBroddockGun;

	public Material materialBroniversalSoldier;

	public Material materialBroniversalSoldierGun;

	public Material materialBroneyRoss;

	public Material materialBroneyRossGun;

	public Material materialLeeBroxmas;

	public Material materialLeeBroxmasGun;

	public Material materialBronnarJensen;

	public Material materialBronnarJensenGun;

	public Material materialHaleTheBro;

	public Material materialHaleTheBroGun;

	public Material materialTrentBroser;

	public Material materialTrentBroserGun;

	public Material materialBroc;

	public Material materialBrocGun;

	public Material materialTollBroad;

	public Material materialTollBroadGun;
}
