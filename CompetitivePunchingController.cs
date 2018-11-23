// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CompetitivePunchingController : IntermissionScreen
{
	protected override void Awake()
	{
		base.Awake();
		foreach (CutscenePlayer cutscenePlayer in this.players)
		{
			cutscenePlayer.Setup(cutscenePlayer.playerNum, this);
		}
		this.playerHits = new int[4];
	}

	protected void Start()
	{
		Vector3 vector = this.intermissionCamera.ScreenToWorldPoint(new Vector3(0f, 0f, 10f));
		Vector3 vector2 = this.intermissionCamera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 10f));
		this.minX = vector.x;
		this.maxX = vector2.x;
		this.minY = vector.y;
		this.maxY = vector2.y;
		this.screenWorldWidth = this.maxX - this.minX + 0.5f;
		this.progressSprite.transform.position = new Vector3(this.minX - 0.25f, 0f, this.progressSprite.transform.position.z);
		this.progressGreyedSprite.transform.position = new Vector3(this.maxX + 0.25f, 0f, this.progressGreyedSprite.transform.position.z);
		CutsceneSound.PlaySoundEffectAt(this.announcer.interrogation.RandomElement<AudioClip>(), 0.5f + UnityEngine.Random.value * 0.2f, base.transform.position, 0.9f + UnityEngine.Random.value * 0.2f);
	}

	protected void Update()
	{
		Vector3 vector = this.intermissionCamera.ScreenToWorldPoint(new Vector3(0f, 0f, 10f));
		Vector3 vector2 = this.intermissionCamera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 10f));
		this.minX = vector.x;
		this.maxX = vector2.x;
		this.minY = vector.y;
		this.maxY = vector2.y;
		this.screenWorldWidth = this.maxX - this.minX + 0.5f;
		this.progressSprite.transform.localPosition = new Vector3(this.minX - 0.25f, 0f, this.progressSprite.transform.localPosition.z);
		this.progressGreyedSprite.transform.localPosition = new Vector3(this.maxX + 0.25f, 0f, this.progressGreyedSprite.transform.localPosition.z);
		float deltaTime = Time.deltaTime;
		if (this.timingOut)
		{
			this.timeOutDelay -= deltaTime;
			if (this.timeOutDelay < 0f)
			{
				this.GetComponentInHeirarchy<Cutscene>().isFinished = true;
			}
		}
		if (Application.isEditor)
		{
			if (Input.GetKeyDown(KeyCode.F1))
			{
				if (Time.timeScale < 1f)
				{
					Time.timeScale = 1f;
				}
				else
				{
					UnityEngine.Debug.Log("Slow time");
					Time.timeScale = 0.1f;
				}
			}
			if (Input.GetKeyDown(KeyCode.F6))
			{
				Application.LoadLevel(Application.loadedLevel);
			}
		}
		bool isDead = this.face.IsDead;
		if (!this.InterogationComplete && this.face.FaceSmashedOff)
		{
			CutsceneSound.PlaySoundEffectAt(this.announcer.interrogationComplete, 0.5f + UnityEngine.Random.value * 0.2f, base.transform.position, 0.9f + UnityEngine.Random.value * 0.2f);
		}
		this.InterogationComplete = this.face.FaceSmashedOff;
		float progress = this.face.GetProgress();
		if (progress < 1f)
		{
			int num = (int)(progress * (float)this.progressSpritePixelsWidth);
			float num2 = (float)num / (float)this.progressSpritePixelsWidth;
			float num3 = this.screenWorldWidth * num2;
			this.progressSprite.SetPixelDimensions(num, (int)this.progressSprite.pixelDimensions.y);
			this.progressSprite.SetLowerLeftPixel(0f, (float)((int)this.progressSprite.lowerLeftPixel.y));
			this.progressSprite.SetSize(num3, this.progressSprite.height);
			this.progressGreyedSprite.SetPixelDimensions(this.progressSpritePixelsWidth - num, (int)this.progressGreyedSprite.pixelDimensions.y);
			this.progressGreyedSprite.SetLowerLeftPixel((float)num, (float)((int)this.progressGreyedSprite.lowerLeftPixel.y));
			this.progressGreyedSprite.SetSize(this.screenWorldWidth - num3, this.progressGreyedSprite.height);
			this.ShimmerMaterial(deltaTime, this.progressSprite, this.progressMaterial1, this.progressMaterial2);
		}
		else
		{
			this.scrollCounter += deltaTime;
			this.progressGreyedSprite.gameObject.SetActive(false);
			this.progressSprite.SetPixelDimensions(this.progressSpritePixelsWidth / 2, (int)this.progressSprite.pixelDimensions.y);
			this.progressSprite.SetLowerLeftPixel((float)((int)(this.scrollCounter * 128f)), (float)((int)this.progressSprite.lowerLeftPixel.y));
			this.progressSprite.SetSize(this.screenWorldWidth, this.progressSprite.height);
			this.ShimmerMaterial(deltaTime, this.progressSprite, this.progressCompletedMaterial1, this.progressCompletedMaterial2);
		}
		if (isDead)
		{
			if (!this.timingOut)
			{
				this.timingOut = true;
				this.timeOutDelay = 3f;
			}
			if (!this.hasHeadExploded)
			{
				base.GetComponentInChildren<Shake>().AddShake2(10f, 10f, 1000f);
				this.hasHeadExploded = true;
				this.timeOutDelay = 3f;
			}
		}
	}

	protected void ShimmerMaterial(float t, SpriteSM sprite, Material material1, Material material2)
	{
		this.progressCounter += t;
		if (this.progressCounter > 0.0667f)
		{
			this.progressCounter -= 0.0667f;
			this.progressFlashCount++;
			if (this.progressFlashCount % 2 == 0)
			{
				sprite.GetComponent<Renderer>().sharedMaterial = material1;
				sprite.RecalcTexture();
			}
			else
			{
				sprite.GetComponent<Renderer>().sharedMaterial = material2;
				sprite.RecalcTexture();
			}
			sprite.SetLowerLeftPixel(sprite.lowerLeftPixel);
		}
	}

	public bool CheckHit(ref float x, ref float y, DirectionEnum direction)
	{
		float num = 0f;
		if (this.face.HeadExplodesOnNextBlow())
		{
			num = 100f;
		}
		switch (direction)
		{
		case DirectionEnum.Up:
			if (y > base.transform.position.y - 25f + num)
			{
				y = base.transform.position.y - 25f + num;
				return true;
			}
			break;
		case DirectionEnum.Down:
			if (y < base.transform.position.y + 45f - num)
			{
				y = base.transform.position.y + 45f - num;
				return true;
			}
			break;
		case DirectionEnum.Left:
			if (x < base.transform.position.x + 10f - num)
			{
				x = base.transform.position.x + 10f - num;
				return true;
			}
			break;
		case DirectionEnum.Right:
			if (x > base.transform.position.x - 10f + num)
			{
				x = base.transform.position.x - 10f + num;
				return true;
			}
			break;
		}
		return false;
	}

	public void Punch(DirectionEnum direction, float x, float y, int playerNum)
	{
		this.face.Punch(direction, x, y);
		this.playerHits[playerNum]++;
		this.Shake(direction);
	}

	private void Shake(DirectionEnum direction)
	{
		if (direction == DirectionEnum.Down || direction == DirectionEnum.Up)
		{
			base.GetComponentInChildren<Shake>().AddShake2(1f, 3f, 50f);
		}
		if (direction == DirectionEnum.Left || direction == DirectionEnum.Right)
		{
			base.GetComponentInChildren<Shake>().AddShake2(3f, 1f, 50f);
		}
	}

	public CompetitivePunchingFace face;

	public CutscenePlayer[] players;

	public SpriteSM progressSprite;

	public SpriteSM progressGreyedSprite;

	public Material progressCompletedMaterial1;

	public Material progressCompletedMaterial2;

	public Material progressFailedMaterial1;

	public Material progressFailedMaterial2;

	public Material progressMaterial1;

	public Material progressMaterial2;

	protected float progressCounter;

	protected int progressFlashCount;

	protected int progressSpriteWidth = 512;

	protected int progressSpritePixelsWidth = 512;

	protected int[] playerHits = new int[4];

	protected float scrollCounter;

	protected float screenWorldWidth = 128f;

	protected bool timingOut;

	protected float timeOutDelay = 3f;

	public bool hasHeadExploded;

	private bool InterogationComplete;

	public SoundHolderAnnouncer announcer;
}
