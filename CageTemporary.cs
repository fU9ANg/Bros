// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CageTemporary : Cage
{
	protected override void Update()
	{
		base.Update();
		if (GameModeController.GameMode == GameMode.Campaign && (this.checkDelay -= Time.deltaTime) < 0f)
		{
			this.checkDelay = 0.1f;
			if (!this.destroyed && !SortOfFollow.IsItSortOfVisible(base.transform.position))
			{
				if (HeroController.GetPlayersPlayingCount() > 1 && HeroController.IsAnyPlayerDead())
				{
					this.rescueBro.gameObject.SetActive(true);
				}
				else
				{
					this.rescueBro.gameObject.SetActive(false);
				}
			}
		}
		if (this.invulnerable && (HeroController.isCountdownFinished || GameModeController.GameMode == GameMode.Campaign))
		{
			this.destroyed = false;
			if (this.health <= 0)
			{
				this.Collapse(0f, 0f, 1f);
			}
			this.invulnerable = false;
		}
	}

	protected override void CheckDestroyed()
	{
		this.destroyed = true;
	}

	public void SetColor(Color col)
	{
		base.GetComponent<Renderer>().material.SetColor("_TintColor", col);
	}

	public void SetPlayerColor(int playerNum)
	{
		switch (playerNum)
		{
		case 0:
			base.GetComponent<Renderer>().material.mainTexture = this.textureCageBlue;
			break;
		case 1:
			base.GetComponent<Renderer>().material.mainTexture = this.textureCageRed;
			break;
		case 2:
			base.GetComponent<Renderer>().material.mainTexture = this.textureCageOrange;
			break;
		case 3:
			base.GetComponent<Renderer>().material.mainTexture = this.textureCagePurple;
			break;
		}
		this.sprite.RecalcTexture();
		this.sprite.SetSize(this.sprite.width, this.sprite.height);
	}

	public override void DamageInternal(int damage, float xI, float yI)
	{
		base.DamageInternal(damage, xI, yI);
		MonoBehaviour.print("Damage! " + this.health);
	}

	public bool invulnerable = true;

	public Texture textureCageBlue;

	public Texture textureCageRed;

	public Texture textureCageOrange;

	public Texture textureCagePurple;

	private float checkDelay = 0.1f;
}
