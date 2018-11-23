// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class HUDHeadGear : MonoBehaviour
{
	public void Setup(int lives, int playerNum, TestVanDammeAnim character)
	{
		if (GameModeController.GameMode == GameMode.ExplosionRun || GameModeController.GameMode == GameMode.BroDown || GameModeController.GameMode == GameMode.Race)
		{
			this.livesShowTime = 0.02f;
		}
		this.livesCount = lives;
		this.playerColor = HeroController.GetHeroColor(playerNum);
		this.SetLives(lives);
		this.livesText.color = this.playerColor;
		this.character = character;
		for (int i = 0; i < this.grenadeSprites.Length; i++)
		{
			this.grenadeSprites[i].SetColor(this.playerColor);
			this.grenadeSprites[i].gameObject.SetActive(false);
		}
		this.grenadesHolder.SetActive(false);
		new Vector3(0f, this.grenadeSprites[0].transform.localPosition.y + 8f, -20f);
		if (GameModeController.IsMatchLeader(playerNum))
		{
			this.kingSprite.gameObject.SetActive(true);
		}
		else
		{
			this.kingSprite.gameObject.SetActive(false);
		}
	}

	protected void SetLives(int lives)
	{
		this.livesText.gameObject.SetActive(false);
		for (int i = 1; i < this.livesNumbers.Length; i++)
		{
			this.livesNumbers[i].gameObject.SetActive(i == lives);
			this.livesNumbers[i].SetColor(this.playerColor);
		}
		if (lives <= 0)
		{
			this.livesNumbers[0].gameObject.SetActive(true);
			this.livesNumbers[0].SetColor(this.playerColor);
		}
		else
		{
			this.livesNumbers[0].gameObject.SetActive(false);
		}
	}

	protected void HideLives()
	{
		for (int i = 0; i < this.livesNumbers.Length; i++)
		{
			this.livesNumbers[i].gameObject.SetActive(false);
		}
	}

	public void SetGrenades(int grenades)
	{
		this.grenadeCount = grenades;
		for (int i = 0; i < this.grenadeSprites.Length; i++)
		{
			if (grenades > this.grenadeSprites.Length)
			{
				grenades = this.grenadeSprites.Length;
			}
			if (i < grenades)
			{
				this.grenadeSprites[i].transform.localPosition = new Vector3(((float)i - (float)(grenades - 1) / 2f) * 5f, this.grenadeSprites[i].transform.localPosition.y, -10f);
				this.grenadeSprites[i].gameObject.SetActive(true);
			}
			else
			{
				this.grenadeSprites[i].gameObject.SetActive(false);
			}
		}
		if (grenades == 0)
		{
			this.kingSprite.transform.localPosition = new Vector3(0f, this.grenadeSprites[0].transform.localPosition.y + 6f, -20f);
		}
		else if (this.livesShowTime <= 0f)
		{
			this.kingSprite.transform.localPosition = new Vector3(0f, this.grenadeSprites[0].transform.localPosition.y + 6f, -20f);
		}
		else
		{
			this.kingSprite.transform.localPosition = new Vector3(0f, this.grenadeSprites[0].transform.localPosition.y + 8f, -20f);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.character == null || this.character.health <= 0)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			if (this.grenadeCount > 0 && this.livesShowTime > 0f)
			{
				this.livesShowTime -= Time.deltaTime;
				if (this.livesShowTime <= 0f)
				{
					this.HideLives();
					this.grenadesHolder.SetActive(true);
					this.livesText.gameObject.SetActive(false);
					this.kingSprite.transform.localPosition = new Vector3(0f, this.grenadeSprites[0].transform.localPosition.y + 6f, -20f);
				}
			}
			if (this.grenadeCount <= 0 && this.livesShowTime <= 0f)
			{
				this.SetLives(this.livesCount);
				this.livesShowTime = 0.3f;
			}
		}
	}

	private void LateUpdate()
	{
		if (base.transform.lossyScale.x < 0f)
		{
			base.transform.localScale = new Vector3(base.transform.localScale.x * -1f, 1f, 1f);
		}
	}

	public SpriteSM[] grenadeSprites;

	protected Color playerColor;

	public TextMesh livesText;

	public GameObject grenadesHolder;

	protected float livesShowTime = 5f;

	public SpriteSM[] livesNumbers;

	protected TestVanDammeAnim character;

	public SpriteSM kingSprite;

	protected int grenadeCount;

	protected int livesCount;
}
