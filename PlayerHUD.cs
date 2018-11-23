// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
	protected void Awake()
	{
		if (this.fuelSprite != null)
		{
			this.fuelSpriteWidth = this.fuelSprite.width;
			this.fuelSpriteHeight = this.fuelSprite.height;
		}
	}

	protected void Start()
	{
		this.SetupFuelGuage();
	}

	private void debug()
	{
		foreach (PlayerHUD playerHUD in UnityEngine.Object.FindObjectsOfType(typeof(PlayerHUD)) as PlayerHUD[])
		{
			if (playerHUD.playerNum == this.playerNum)
			{
				UnityEngine.Debug.LogError(" Thats strange, a hud with this player num already exists " + this.playerNum);
				UnityEngine.Debug.Log(" Thats strange, a hud with this player num already exists " + this.playerNum);
			}
		}
	}

	public void Setup(int lives, int playerNum)
	{
		this.playerNum = playerNum;
		this.SetPosition();
		this.SetSpacing(false);
		switch (playerNum)
		{
		case 0:
			this.freeLifeText.SetupHudPosition(36f, 1);
			break;
		case 1:
			this.freeLifeText.SetupHudPosition(-36f, 1);
			break;
		case 2:
			this.freeLifeText.SetupHudPosition(36f, -1);
			break;
		case 3:
			this.freeLifeText.SetupHudPosition(-36f, 1);
			break;
		}
		this.avatarFacingDirection = this.direction;
		this.avatarWidth = this.avatar.width;
		this.avatar.SetSize((float)this.avatarFacingDirection * this.avatarWidth, this.avatar.height);
		this.plusText.Setup();
		this.SetupIcons(this.grenadeIcons, this.direction, false);
		switch (GameModeController.GameMode)
		{
		case GameMode.Campaign:
			this.SetLives(lives);
			break;
		case GameMode.ExplosionRun:
		case GameMode.DeathMatch:
		case GameMode.BroDown:
		case GameMode.SuicideHorde:
		case GameMode.Race:
		case GameMode.TeamDeathMatch:
			this.SetWins(GameModeController.GetPlayerRoundWins(playerNum));
			break;
		}
		base.transform.parent = SortOfFollow.GetInstance().UICamera.transform;
		this.secondAvatar.gameObject.SetActive(false);
	}

	protected void LateUpdate()
	{
		if ((this.playerNum >= 0 && Screen.width != this.currentScreenWidth) || (Screen.height != this.currentScreenHeight || this.currentCameraOrthographicSize != SortOfFollow.GetInstance().UICamera.orthographicSize) || ((this.playerNum == 0 || this.playerNum == 1) && LevelTitle.IsMoving()))
		{
			this.SetPosition();
			this.wasMoving = true;
		}
		else if (this.wasMoving)
		{
			this.SetPosition();
			this.wasMoving = false;
		}
		if (Input.GetKeyDown(KeyCode.F12) || !HeroController.MustShowHuds())
		{
			base.gameObject.SetActive(false);
			this.hidden = true;
		}
		if (HeroController.MustShowHuds() && this.hidden)
		{
			this.hidden = false;
			base.gameObject.SetActive(true);
		}
	}

	public void FlashSpecialIcons()
	{
		base.Invoke("FlashSpecialIconsRed", 0.001f);
		base.Invoke("FlashSpecialIconsNormal", 0.2f);
		base.Invoke("FlashSpecialIconsRed", 0.4f);
		base.Invoke("FlashSpecialIconsNormal", 0.6f);
		base.Invoke("FlashSpecialIconsRed", 0.8f);
		base.Invoke("FlashSpecialIconsNormal", 1f);
		base.Invoke("FlashSpecialIconsNormal", 1.1f);
	}

	protected void FlashSpecialIconsRed()
	{
		for (int i = 0; i < this.grenadeIcons.Length; i++)
		{
			if (i < this.grenadeOriginalCount)
			{
				this.grenadeIcons[i].gameObject.SetActive(true);
				this.grenadeIcons[i].SetColor(Color.red);
			}
			else
			{
				this.grenadeIcons[i].gameObject.SetActive(false);
				this.grenadeIcons[i].SetColor(Color.white);
			}
		}
	}

	protected void FlashSpecialIconsNormal()
	{
		for (int i = 0; i < this.grenadeIcons.Length; i++)
		{
			if (i + 1 <= this.grenadeCount)
			{
				this.grenadeIcons[i].gameObject.SetActive(true);
				this.grenadeIcons[i].SetColor(Color.white);
			}
			else
			{
				this.grenadeIcons[i].gameObject.SetActive(false);
				this.grenadeIcons[i].SetColor(Color.white);
			}
		}
	}

	public void FlashAvatar(float flashTime, bool primaryAvatar)
	{
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.FlashAvatarCoroutine(flashTime, primaryAvatar));
		}
	}

	private IEnumerator FlashAvatarCoroutine(float flashTime, bool primaryAvatar)
	{
		Renderer renderer = (!primaryAvatar) ? this.secondAvatar.GetComponent<Renderer>() : base.GetComponent<Renderer>();
		while (flashTime > 0f)
		{
			renderer.material.SetColor("_TintColor", new Color(0.33f, 0.33f, 0.33f, 1f));
			yield return new WaitForSeconds(0.033f);
			flashTime -= 0.033f;
			renderer.material.SetColor("_TintColor", Color.grey);
			yield return new WaitForSeconds(0.033f);
			flashTime -= 0.033f;
			renderer.material.SetColor("_TintColor", new Color(0.7f, 0.7f, 0.7f, 1f));
			yield return new WaitForSeconds(0.033f);
			flashTime -= 0.033f;
			renderer.material.SetColor("_TintColor", Color.grey);
			yield return new WaitForSeconds(0.033f);
			flashTime -= 0.033f;
		}
		yield break;
	}

	protected void SetPosition()
	{
		this.currentScreenHeight = Screen.height;
		this.currentScreenWidth = Screen.width;
		this.currentCameraOrthographicSize = SortOfFollow.GetInstance().UICamera.orthographicSize;
		float num = (float)Screen.height * Camera.main.rect.height;
		switch (this.playerNum)
		{
		case 0:
		{
			Vector3 position = SortOfFollow.GetInstance().UICamera.ScreenToWorldPoint(new Vector3(num / 16f, num / 16f + (float)Screen.height * ((1f - SortOfFollow.GetInstance().UICamera.rect.height) / 2f), 5f)) + LevelTitle.GetOffset() * Vector3.up;
			base.transform.position = position;
			this.direction = 1;
			break;
		}
		case 1:
		{
			Vector3 position = SortOfFollow.GetInstance().UICamera.ScreenToWorldPoint(new Vector3((float)Screen.width - num / 16f, num / 16f + (float)Screen.height * ((1f - SortOfFollow.GetInstance().UICamera.rect.height) / 2f), 5f)) + LevelTitle.GetOffset() * Vector3.up;
			base.transform.position = position;
			this.direction = -1;
			break;
		}
		case 2:
		{
			Vector3 position = SortOfFollow.GetInstance().UICamera.ScreenToWorldPoint(new Vector3((float)Screen.height / 16f, (float)Screen.height - num / 16f + 8f - (float)Screen.height * ((1f - SortOfFollow.GetInstance().UICamera.rect.height) / 2f), 5f));
			base.transform.position = position;
			this.direction = 1;
			break;
		}
		case 3:
		{
			Vector3 position = SortOfFollow.GetInstance().UICamera.ScreenToWorldPoint(new Vector3((float)Screen.width - num / 16f, (float)Screen.height - num / 16f + 8f - (float)Screen.height * ((1f - SortOfFollow.GetInstance().UICamera.rect.height) / 2f), 5f));
			base.transform.position = position;
			this.direction = -1;
			break;
		}
		default:
			UnityEngine.Debug.LogError("Not a player num");
			break;
		}
	}

	public void SetLives(int lives)
	{
		this.livesText.text = "LIVES " + lives;
	}

	public void SetWins(int wins)
	{
		this.livesText.text = "WINS " + wins;
	}

	public void ShowFreeLife()
	{
		UnityEngine.Debug.Log("Show Free Life");
		this.freeLifeText.RestartBubble();
		this.FlashAvatar(1f, true);
	}

	public void SetAvatar(Material avatarMaterial)
	{
		this.avatar.GetComponent<Renderer>().sharedMaterial = avatarMaterial;
		this.avatar.RecalcTexture();
		this.avatar.SetLowerLeftPixel(this.avatar.lowerLeftPixel);
		this.avatar.SetSize((float)this.avatarFacingDirection * this.avatarWidth, this.avatar.height);
		this.avatar.CalcUVs();
		this.avatar.UpdateUVs();
		this.FlashAvatar(1f, true);
		this.secondAvatar.gameObject.SetActive(false);
	}

	public void SetGrenadesOriginalCount(int grenades)
	{
		this.grenadeOriginalCount = grenades;
	}

	public void SetGrenades(int grenades)
	{
		if (this.grenadeCount != grenades)
		{
			this.grenadeCount = grenades;
			if (this.hidden)
			{
				return;
			}
			this.FlashSpecialIconsNormal();
		}
		this.fuelSprite.gameObject.SetActive(false);
		this.fuelM = -1f;
	}

	public void SetFuel(float fuelM, bool red)
	{
		this.grenadeCount = 0;
		if (this.fuelM != fuelM)
		{
			this.fuelM = fuelM;
			for (int i = 0; i < this.grenadeIcons.Length; i++)
			{
				this.grenadeIcons[i].gameObject.SetActive(false);
			}
			this.fuelSprite.SetSize(this.fuelSpriteWidth * fuelM * (float)this.avatarFacingDirection, this.fuelSpriteHeight);
		}
		if (red)
		{
			this.fuelWarningTime = Time.time;
		}
		if (this.fuelSprite.color.g > 0.2f && Time.time - this.fuelWarningTime < 0.25f)
		{
			this.fuelSprite.SetColor(Color.red);
		}
		else if (this.fuelSprite.color.g < 0.2f && Time.time - this.fuelWarningTime >= 0.25f)
		{
			this.fuelSprite.SetColor(Color.white);
		}
		this.fuelSprite.gameObject.SetActive(true);
	}

	public void Hide()
	{
		this.hidden = true;
		base.gameObject.SetActive(false);
	}

	public void Show()
	{
		this.hidden = false;
		base.gameObject.SetActive(true);
	}

	protected void SetupIcons(SpriteSM[] icons, int direction, bool doubleAvatar)
	{
		for (int i = 0; i < icons.Length; i++)
		{
			icons[i].transform.localPosition = new Vector3((float)(((!doubleAvatar) ? 0 : (direction * 6)) + direction * 9 * (i + 2)), -0.1f, 2f);
		}
	}

	protected void SetupFuelGuage()
	{
		if (this.direction == -1)
		{
			this.fuelSprite.SetAnchor(SpriteBase.ANCHOR_METHOD.MIDDLE_RIGHT);
		}
		else
		{
			this.fuelSprite.SetAnchor(SpriteBase.ANCHOR_METHOD.MIDDLE_LEFT);
		}
		this.fuelSprite.SetSize(this.fuelSpriteWidth, this.fuelSpriteHeight);
		this.fuelSprite.gameObject.SetActive(false);
	}

	public void SwitchAvatarMaterial(HeroType type)
	{
		this.secondAvatar.gameObject.SetActive(false);
		this.heroType = type;
		HeroController.SwitchAvatarMaterial(this.avatar, type);
		this.avatar.RecalcTexture();
		this.avatar.SetSize((float)this.avatarFacingDirection * this.avatarWidth, this.avatar.height);
		this.avatar.CalcUVs();
		this.avatar.UpdateUVs();
		this.SetSpacing(false);
	}

	public void GoToDoubleAvatarMode(Material avatar1, Material avatar2)
	{
		this.avatar.GetComponent<Renderer>().material = avatar1;
		this.secondAvatar.gameObject.SetActive(true);
		this.secondAvatar.transform.position = new Vector3(this.avatar.transform.position.x + (float)this.direction * 12f, this.avatar.transform.position.y - 2f, this.avatar.transform.position.z + 4f);
		this.secondAvatar.GetComponent<Renderer>().material = avatar2;
		this.secondAvatar.SetSize((float)this.avatarFacingDirection * this.avatarWidth, this.avatar.height);
		this.SetSpacing(true);
	}

	private void SetSpacing(bool doubleAvatar)
	{
		if (this.direction > 0)
		{
			this.textBackground.SetSize(this.textBackground.width, this.textBackground.height);
			this.textBackground.transform.localPosition = new Vector3(32f, 0f, 0f);
			this.livesText.transform.localPosition = new Vector3((float)((!doubleAvatar) ? 15 : 22), -11f, -2f);
			this.livesText.anchor = TextAnchor.MiddleLeft;
			this.livesText.alignment = TextAlignment.Left;
			this.plusText.transform.localPosition = new Vector3(36f, 12.7f, -4f);
		}
		else
		{
			this.textBackground.SetSize(-this.textBackground.width, this.textBackground.height);
			this.textBackground.transform.localPosition = new Vector3(-32f, 0f, 0f);
			this.livesText.transform.localPosition = new Vector3((float)((!doubleAvatar) ? -16 : -23), -11f, -2f);
			this.livesText.anchor = TextAnchor.MiddleRight;
			this.livesText.alignment = TextAlignment.Right;
			this.plusText.transform.localPosition = new Vector3(-13f, 12.7f, -4f);
		}
		this.SetupIcons(this.grenadeIcons, this.direction, doubleAvatar);
	}

	public void SetAvatarCalm(bool useFirstAvatar)
	{
		SpriteSM spriteSM = (!useFirstAvatar) ? this.secondAvatar : this.avatar;
		spriteSM.SetLowerLeftPixel(new Vector2(0f, spriteSM.lowerLeftPixel.y));
	}

	public void SetAvatarAngry(bool useFirstAvatar)
	{
		SpriteSM spriteSM = (!useFirstAvatar) ? this.secondAvatar : this.avatar;
		spriteSM.SetLowerLeftPixel(new Vector2(32f, spriteSM.lowerLeftPixel.y));
	}

	public void SetAvatarFire(bool useFirstAvatar)
	{
		SpriteSM spriteSM = (!useFirstAvatar) ? this.secondAvatar : this.avatar;
		spriteSM.SetLowerLeftPixel(new Vector2(64f, spriteSM.lowerLeftPixel.y));
	}

	public void SetAvatarDead(bool useFirstAvatar)
	{
		SpriteSM spriteSM = (!useFirstAvatar) ? this.secondAvatar : this.avatar;
		spriteSM.SetLowerLeftPixel(new Vector2(96f, spriteSM.lowerLeftPixel.y));
	}

	public void SetAvatarBounceDown(bool useFirstAvatar)
	{
		SpriteSM spriteSM = (!useFirstAvatar) ? this.secondAvatar : this.avatar;
		spriteSM.SetLowerLeftPixel(new Vector2(spriteSM.lowerLeftPixel.x, 63f));
	}

	public void SetAvatarBounceUp(bool useFirstAvatar)
	{
		SpriteSM spriteSM = (!useFirstAvatar) ? this.secondAvatar : this.avatar;
		spriteSM.SetLowerLeftPixel(new Vector2(spriteSM.lowerLeftPixel.x, 64f));
	}

	public int direction = 1;

	public TextMesh livesText;

	public Plus1Text plusText;

	public ReactionBubble freeLifeText;

	public SpriteSM avatar;

	public SpriteSM secondAvatar;

	public HeroType heroType;

	public SpriteSM textBackground;

	public SpriteSM[] grenadeIcons;

	protected int grenadeCount;

	protected int grenadeOriginalCount = 3;

	protected float fuelM;

	public SpriteSM fuelSprite;

	protected float fuelWarningTime;

	protected float fuelSpriteWidth;

	protected float fuelSpriteHeight = 10f;

	protected int currentScreenWidth;

	protected int currentScreenHeight;

	protected float currentCameraOrthographicSize = 128f;

	protected int playerNum = -1;

	protected int avatarFacingDirection = 1;

	protected float avatarWidth = 32.1f;

	protected bool wasMoving;

	private bool hidden;
}
