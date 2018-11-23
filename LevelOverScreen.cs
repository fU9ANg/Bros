// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class LevelOverScreen : MonoBehaviour
{
	private void Awake()
	{
		LevelOverScreen.instance = this;
		this.Hide();
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.hold)
		{
			return;
		}
		this.delay += Time.deltaTime;
		if (this.delay > this.delayInterval)
		{
			Vector3 localPosition = this.failScreen.transform.localPosition;
			Vector3 localPosition2 = this.victoryScreen.transform.localPosition;
			localPosition.x = 0f;
			localPosition2.x = 0f;
			if (InputReader.GetControllerPressingFire() >= 0)
			{
				this.failScreen.transform.localPosition = Vector3.Lerp(this.failScreen.transform.localPosition, localPosition, 1f);
				this.victoryScreen.transform.localPosition = Vector3.Lerp(this.victoryScreen.transform.localPosition, localPosition2, 1f);
			}
			else
			{
				this.failScreen.transform.localPosition = Vector3.Lerp(this.failScreen.transform.localPosition, localPosition, Time.deltaTime * 30f);
				this.victoryScreen.transform.localPosition = Vector3.Lerp(this.victoryScreen.transform.localPosition, localPosition2, Time.deltaTime * 10f);
			}
			if (this.victory && !this.hasStartedShowingScores && this.victoryScreen.transform.localPosition.x < 1f)
			{
				this.hasStartedShowingScores = true;
				if (GameModeController.GameMode == GameMode.Campaign)
				{
					LevelOverScreen.instance.StartCoroutine(LevelOverScreen.instance.ShowMookDeaths());
				}
				else
				{
					LevelOverScreen.instance.StartCoroutine(LevelOverScreen.instance.ShowBrotality());
				}
			}
		}
		float f = this.numberOfBrosTarget;
		if (this.numberOfBrosTarget > 0f)
		{
			string text = "Rescues";
			if (Mathf.Round(f) == 1f)
			{
				text = "Rescue";
			}
			this.rescueLeftText.text = string.Concat(new object[]
			{
				Mathf.Round(f),
				" More ",
				text,
				" until\nthe Next Unlock!"
			});
		}
		else if (this.victory)
		{
			if (PlaytomicController.isExpendabrosBuild)
			{
				this.rescueLeftText.text = "broforcegame.com \n theexpendables3film.com";
			}
			else if (PlaytomicController.IsThisBuildOutOfDate())
			{
				this.rescueLeftText.text = "BROFORCE IS OUT OF DATE!\nVISIT freelives.net FOR UPDATES";
			}
			else
			{
				this.rescueLeftText.text = "THANKS FOR PREORDERING! YOU RULE!";
			}
		}
		else if (PlaytomicController.IsThisBuildOutOfDate())
		{
			this.rescueLeftText.text = "BROFORCE IS OUT OF DATE!\nVISIT freelives.net FOR UPDATES";
		}
		else
		{
			this.rescueLeftText.text = string.Empty;
		}
		bool flag = false;
		if (InputReader.GetControllerPressingFire() >= 0 || HeroController.NumberOfPlayersOnThisPC() == 0)
		{
			flag = true;
		}
		if (this.waitingForVote && flag)
		{
			GameModeController.SendReady();
			this.waitingForVote = false;
		}
		this.waitForVotesTimer -= Time.deltaTime;
		if ((GameModeController.HasEveryBodyVotedToSkip() || HeroController.NumberOfPlayersOnThisPC() == 0 || this.waitForVotesTimer < 0f) && !this.TransitionHasBegun)
		{
			Fader.FadeSolid(0.9f, false);
			GameModeController.SetSwitchDelay(1f);
			this.TransitionHasBegun = true;
		}
	}

	private void Hide()
	{
		LevelOverScreen.instance.victoryScreen.gameObject.SetActive(false);
		LevelOverScreen.instance.failScreen.gameObject.SetActive(false);
	}

	public static void Show(bool victory)
	{
		LevelOverScreen.instance.victory = victory;
		if (victory)
		{
			LevelOverScreen.instance.textMesh.text = string.Empty;
			LevelOverScreen.instance.areaLiberatedSprite.gameObject.SetActive(true);
			LevelOverScreen.instance.victoryScreen.gameObject.SetActive(true);
		}
		else
		{
			if (Announcer.HadRecentAnnouncement())
			{
				LevelOverScreen.instance.textMesh.text = Announcer.currentAnouncement;
			}
			else
			{
				LevelOverScreen.instance.textMesh.text = "MISSION FAILED!";
			}
			LevelOverScreen.instance.textMesh.gameObject.SetActive(true);
			LevelOverScreen.instance.rescueLeftText.gameObject.SetActive(true);
			LevelOverScreen.instance.failScreen.gameObject.SetActive(true);
			LevelOverScreen.instance.rescueLeftText.transform.localPosition = new Vector3(-36f, LevelOverScreen.instance.rescueLeftText.transform.localPosition.y, LevelOverScreen.instance.rescueLeftText.transform.localPosition.z);
			LevelSelectionController.RestartCampaignScene();
			GameModeController.SetSwitchDelay(0.5f);
			LevelOverScreen.instance.delayInterval = 0f;
		}
		LevelOverScreen.instance.hold = false;
		LevelOverScreen.instance.numberOfBrosTarget = (float)HeroUnlockController.GetNumberOfRescuesToNextUnlock();
	}

	private void AddDeath(int deathNum, int totalDeaths, Transform parent, ShakeM shakeObject)
	{
		int num = 40 + totalDeaths / 6;
		int num2 = 1 + totalDeaths / num;
		int num3 = deathNum / num;
		int num4 = deathNum - num3 * num;
		float num5 = 190f / (float)Mathf.Min(num, totalDeaths);
		float num6 = (num2 > 1) ? (50f / ((float)num2 + 0.85f)) : 0f;
		if (num5 > 16f)
		{
			num5 = 16f;
		}
		float num7 = -num5 * (float)(Mathf.Min(num, totalDeaths) - 1) / 2f;
		float num8 = (num2 > 1) ? (num6 * ((float)(num2 - 1) / 2f) + 10f) : 10f;
		DeathObject deathObject = StatisticsController.GetDeathObject(deathNum);
		Vector3 position = this.mookDeathsHolder.transform.position + new Vector3(num7 + (float)num4 * num5, num8 - (float)num3 * num6, 400.1f + 35f * (float)deathNum / (float)totalDeaths - (float)(num3 * 45));
		if (deathObject != null)
		{
			if (deathObject.heroType != HeroType.None)
			{
				UnityEngine.Debug.LogError("Not an error... is a bro ");
			}
			VictoryMookDeath victoryMookDeath = null;
			if (deathObject.mookType == MookType.Trooper)
			{
				victoryMookDeath = this.mookDeathTrooperPrefab;
			}
			else if (deathObject.mookType == MookType.Suicide)
			{
				victoryMookDeath = this.mookDeathSuicidePrefab;
			}
			else if (deathObject.mookType == MookType.Devil)
			{
				victoryMookDeath = this.mookDeathSatanPrefab;
			}
			else if (deathObject.mookType == MookType.Dog)
			{
				victoryMookDeath = this.mookDeathDogPrefab;
			}
			else if (deathObject.mookType == MookType.BigGuy)
			{
				victoryMookDeath = this.mookDeathBigGuyPrefab;
			}
			else if (deathObject.mookType == MookType.RiotShield)
			{
				victoryMookDeath = this.mookDeathRiotShieldPrefab;
			}
			else if (deathObject.mookType == MookType.Scout)
			{
				victoryMookDeath = this.mookDeathScoutPrefab;
			}
			else if (deathObject.mookType == MookType.Villager)
			{
				victoryMookDeath = this.villagerDeathPrefab;
			}
			else if (deathObject.mookType == MookType.General)
			{
				victoryMookDeath = this.mookDeathGeneralPrefab;
			}
			else if (deathObject.mookType == MookType.Bazooka)
			{
				victoryMookDeath = this.mookDeathBazookaPrefab;
			}
			else if (deathObject.heroType != HeroType.None)
			{
				victoryMookDeath = this.broDeathGenericPrefab;
			}
			if (victoryMookDeath != null)
			{
				VictoryMookDeath victoryMookDeath2 = UnityEngine.Object.Instantiate(victoryMookDeath, position, Quaternion.identity) as VictoryMookDeath;
				victoryMookDeath2.Setup(deathObject, 0.2f, parent, shakeObject);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Huh!");
		}
	}

	private void AddBadge(int badgeNo, BadgeHolder[] holders, SpriteSM prefab, ShakeM shakeM)
	{
		int num = (badgeNo - 1) % holders.Length;
		int num2 = (badgeNo - 1) / holders.Length;
		if (num2 > 0 && num == 0)
		{
			SpriteSM[] array = new SpriteSM[holders.Length];
			for (int i = 0; i < holders.Length; i++)
			{
				array[i] = holders[i].badge;
				holders[i].badge = null;
			}
			base.StartCoroutine(this.CombineBadges(array, holders[(num2 - 1) % holders.Length], (num2 - 1) / holders.Length + 1, shakeM));
		}
		Puff puff = UnityEngine.Object.Instantiate(this.explosionPrefab, holders[num].transform.position, Quaternion.identity) as Puff;
		puff.gameObject.layer = LayerMask.NameToLayer("UI");
		puff.transform.parent = holders[num].transform;
		SpriteSM spriteSM = UnityEngine.Object.Instantiate(prefab, holders[num].transform.position, Quaternion.identity) as SpriteSM;
		spriteSM.gameObject.layer = LayerMask.NameToLayer("UI");
		spriteSM.transform.parent = holders[num].transform;
		holders[num].badge = spriteSM;
		Sound.GetInstance().PlaySoundEffect(this.soundHolder.attackSounds, 0.5f);
	}

	private IEnumerator CombineBadges(SpriteSM[] badges, BadgeHolder holder, int offset, ShakeM shakeM)
	{
		float lerp = 0f;
		Vector3 toPos = (Vector3.up * 8f + Vector3.forward) * (float)offset;
		foreach (SpriteSM badge in badges)
		{
			badge.transform.parent = holder.transform;
		}
		while (lerp < 1f)
		{
			if (lerp > 0.8f)
			{
				lerp = 1f;
			}
			foreach (SpriteSM badge2 in badges)
			{
				badge2.transform.localPosition = Vector3.Lerp(badge2.transform.localPosition, toPos, lerp);
			}
			lerp += Time.deltaTime;
			yield return null;
		}
		badges[0].SetLowerLeftPixel(32f, 64f);
		badges[0].UpdateUVs();
		for (int i = 1; i < badges.Length; i++)
		{
			UnityEngine.Object.Destroy(badges[i].gameObject);
		}
		for (int j = 0; j < 5; j++)
		{
			Puff exp = UnityEngine.Object.Instantiate(this.explosionPrefab, holder.transform.position + toPos + Extensions.Vec2toVec3(UnityEngine.Random.insideUnitCircle * 16f), Quaternion.identity) as Puff;
			exp.gameObject.layer = LayerMask.NameToLayer("UI");
			exp.transform.parent = holder.transform;
		}
		Sound.GetInstance().PlaySoundEffect(this.soundHolder.attackSounds, 0.8f);
		shakeM.m += 1f;
		yield break;
	}

	private IEnumerator ShakeTransform(Transform trans, ShakeM shakeM)
	{
		Vector3 pos = trans.position;
		float sinXCounter = 0f;
		float sinYCounter = 1f;
		float sinXRate = 52f + UnityEngine.Random.value * 64f;
		float sinYRate = 30f + UnityEngine.Random.value * 64f;
		float lastM = 0f;
		for (;;)
		{
			float t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
			if (shakeM.m > lastM)
			{
				sinXRate = 52f + UnityEngine.Random.value * 64f;
				sinYRate = 30f + UnityEngine.Random.value * 64f;
			}
			sinXCounter += sinXRate * t;
			sinYCounter += sinYRate * t;
			sinXRate *= 1f - t * 8f;
			sinYRate *= 1f - t * 8f;
			if (shakeM.m > 0f)
			{
				shakeM.m -= t * 1.6f;
				if (shakeM.m <= 0f)
				{
					shakeM.m = 0f;
				}
				else if (shakeM.m > 2f)
				{
					shakeM.m = 2f;
				}
			}
			trans.position = pos + new Vector3(Mathf.Sin(sinXCounter) * shakeM.m * 4f, Mathf.Sin(sinYCounter) * shakeM.m * 2f, 0f);
			yield return null;
		}
		yield break;
	}

	private IEnumerator ShowMookDeaths()
	{
		this.mookDeathsHolder.gameObject.SetActive(true);
		ShakeM shakeM = new ShakeM();
		base.StartCoroutine(this.ShakeTransform(this.mookDeathsHolder, shakeM));
		while (this.mookDeathsBox.IsBusyGrowing())
		{
			if (InputReader.GetControllerPressingFire() >= 0)
			{
				this.mookDeathsBox.FinishGrowing();
			}
			yield return null;
		}
		int totalMooks = StatisticsController.GetDeathsCount();
		float mookRate = 1.5f / (float)totalMooks + 0.7f / Mathf.Sqrt((float)totalMooks);
		int currentMook = 0;
		int mookInc = 0;
		float mookCounter = mookRate;
		while (currentMook < totalMooks)
		{
			mookCounter += Time.deltaTime;
			if (InputReader.GetControllerPressingFire() >= 0)
			{
				mookInc = totalMooks - currentMook;
			}
			else
			{
				mookInc = (int)(mookCounter / mookRate);
				mookCounter -= mookRate * (float)mookInc;
			}
			for (int i = 0; i < mookInc; i++)
			{
				if (currentMook < totalMooks)
				{
					this.AddDeath(currentMook, totalMooks, this.mookDeathsHolder, shakeM);
				}
				currentMook++;
			}
			yield return null;
		}
		base.StartCoroutine(this.ShowTime());
		yield break;
	}

	private IEnumerator ShowBrotality()
	{
		this.brotalityHolder.gameObject.SetActive(true);
		ShakeM shakeM = new ShakeM();
		base.StartCoroutine(this.ShakeTransform(this.brotalityHolder, shakeM));
		while (this.brotalityBox.IsBusyGrowing())
		{
			if (InputReader.GetControllerPressingFire() >= 0)
			{
				this.brotalityBox.FinishGrowing();
			}
			yield return null;
		}
		this.brotalityRankText.text = string.Empty;
		base.StartCoroutine(this.ShowStealth());
		while (StatisticsController.brotalityPercentile < 50f)
		{
			yield return null;
		}
		this.brotalityRankText.gameObject.SetActive(true);
		Sound.GetInstance().PlaySoundEffect(this.soundHolder.specialSounds, 0.8f);
		if (StatisticsController.brotalityRank < 11)
		{
			this.brotalityRankText.text = "#" + StatisticsController.brotalityRank + "!!";
		}
		else
		{
			this.brotalityRankText.text = "TOP " + (100f - StatisticsController.brotalityPercentile).ToString("#0") + "%";
		}
		int flashes = 15;
		for (;;)
		{
			int num;
			num = flashes; flashes = (num ) - 1;
			if (num <= 0)
			{
				break;
			}
			this.brotalityRankText.GetComponent<Renderer>().material.SetColor("_TintColor", (!(this.brotalityRankText.GetComponent<Renderer>().material.GetColor("_TintColor") == Color.red)) ? Color.red : Color.black);
			yield return new WaitForSeconds(0.1f);
		}
		this.brotalityRankText.GetComponent<Renderer>().material.SetColor("_TintColor", Color.red);
		yield break;
	}

	private IEnumerator ShowStealth()
	{
		ShakeM shakeM = new ShakeM();
		base.StartCoroutine(this.ShakeTransform(this.stealthHolder, shakeM));
		this.stealthHolder.gameObject.SetActive(true);
		Color col = new Color(0f, 0.2f, 1f, 1f);
		while (this.stealthBox.IsBusyGrowing())
		{
			if (InputReader.GetControllerPressingFire() >= 0)
			{
				this.stealthBox.FinishGrowing();
			}
			yield return null;
		}
		this.stealthText.GetComponent<Renderer>().material.SetColor("_TintColor", col);
		this.stealthText.text = "STEALTH";
		yield break;
	}

	private IEnumerator ShowTime()
	{
		float lerp = 0f;
		this.timeHolder.gameObject.SetActive(true);
		while (this.timeBox.IsBusyGrowing())
		{
			if (InputReader.GetControllerPressingFire() >= 0)
			{
				this.timeBox.FinishGrowing();
				lerp = 0.999f;
			}
			yield return null;
		}
		this.hourglassSprite.gameObject.SetActive(true);
		while (lerp < 1f)
		{
			if (InputReader.GetControllerPressingFire() >= 0)
			{
				lerp = 1f;
			}
			lerp = Mathf.Clamp(lerp += Time.deltaTime * 0.4f, 0f, 1f);
			this.timeText.text = string.Empty + StatisticsController.GetTimeString(Mathf.Lerp(0f, StatisticsController.GetTime(), lerp));
			yield return null;
		}
		this.waitingForVote = true;
		base.StartCoroutine(this.ShowRescuesLeft());
		while (StatisticsController.timePercentile < 50f)
		{
			yield return null;
		}
		Sound.GetInstance().PlaySoundEffect(this.soundHolder.specialSounds, 0.8f);
		this.timeRankText.gameObject.SetActive(true);
		this.timeRankText.text = string.Empty + ((StatisticsController.timeRank >= 11) ? (" FASTER THAN " + StatisticsController.timePercentile.ToString("#0.0") + "%") : (" #" + StatisticsController.timeRank + "!!"));
		int flashes = 15;
		for (;;)
		{
			int num;
			num = flashes; flashes = (num ) - 1;
			if (num <= 0)
			{
				break;
			}
			this.timeRankText.GetComponent<Renderer>().material.SetColor("_TintColor", (!(this.timeRankText.GetComponent<Renderer>().material.GetColor("_TintColor") == Color.white)) ? Color.white : Color.black);
			yield return new WaitForSeconds(0.1f);
		}
		this.timeRankText.GetComponent<Renderer>().material.SetColor("_TintColor", Color.white);
		yield break;
	}

	private IEnumerator ShowRescuesLeft()
	{
		this.rescuesLeftHolder.gameObject.SetActive(true);
		while (this.rescureBox.IsBusyGrowing())
		{
			yield return null;
		}
		this.rescueLeftText.gameObject.SetActive(true);
		float lerp = 0f;
		base.StartCoroutine(this.ShowSkip());
		for (;;)
		{
			Color toCol = Color.red;
			Color col = this.rescueLeftText.GetComponent<Renderer>().material.GetColor("_TintColor");
			while (lerp < 1f)
			{
				this.rescueLeftText.GetComponent<Renderer>().material.SetColor("_TintColor", Color.Lerp(col, toCol, lerp));
				lerp += Time.deltaTime * 2f;
				yield return null;
			}
			lerp = 0f;
			toCol = Color.white;
			col = this.rescueLeftText.GetComponent<Renderer>().material.GetColor("_TintColor");
			while (lerp < 1f)
			{
				this.rescueLeftText.GetComponent<Renderer>().material.SetColor("_TintColor", Color.Lerp(col, toCol, lerp));
				lerp += Time.deltaTime * 2f;
				yield return null;
			}
			lerp = 0f;
			toCol = Color.blue;
			col = this.rescueLeftText.GetComponent<Renderer>().material.GetColor("_TintColor");
			while (lerp < 1f)
			{
				this.rescueLeftText.GetComponent<Renderer>().material.SetColor("_TintColor", Color.Lerp(col, toCol, lerp));
				lerp += Time.deltaTime * 2f;
				yield return null;
			}
			lerp = 0f;
			toCol = Color.white;
			col = this.rescueLeftText.GetComponent<Renderer>().material.GetColor("_TintColor");
			while (lerp < 1f)
			{
				this.rescueLeftText.GetComponent<Renderer>().material.SetColor("_TintColor", Color.Lerp(col, toCol, lerp));
				lerp += Time.deltaTime * 2f;
				yield return null;
			}
			lerp = 0f;
		}
		yield break;
	}

	private IEnumerator ShowSkip()
	{
		if (this.skipStateRoot == null)
		{
			MonoBehaviour.print("readyStateRoot is not assigned");
			yield break;
		}
		if (!Connect.IsOffline)
		{
			this.skipStateRoot.gameObject.SetActive(true);
			for (int i = 0; i < 4; i++)
			{
				yield return new WaitForSeconds(0.1f);
				this.skipStates[i].gameObject.SetActive(true);
				if (HeroController.playersPlaying[i])
				{
					this.SetWaiting(i);
				}
			}
			yield return new WaitForSeconds(0.1f);
			this.fireToSkip.gameObject.SetActive(true);
		}
		yield return null;
		yield break;
	}

	public void SetWaiting(int playerNum)
	{
		if (this.skipStates.Length > playerNum)
		{
			if (!GameModeController.playersVotedToSkip[playerNum])
			{
				this.skipStates[playerNum].text = playerNum + 1 + " SKIP?";
			}
		}
		else
		{
			UnityEngine.Debug.Log("Ready States Not Set");
		}
	}

	public void VoteToSkip(int playerNum)
	{
		if (this.skipStates.Length > playerNum)
		{
			this.skipStates[playerNum].text = playerNum + 1 + " SKIP!";
		}
		else
		{
			UnityEngine.Debug.Log("Ready States Not Set");
		}
	}

	public Color winColor;

	public Color failColor;

	public BadgeHolder[] brotalityBadgeHolders;

	public BadgeHolder[] stealthBadgeHolders;

	public Puff explosionPrefab;

	public SpriteSM brotalityBadgePrefab;

	public SpriteSM stealthBadgePrefab;

	public SpriteSM hourglassSprite;

	public VictoryMookDeath mookDeathTrooperPrefab;

	public VictoryMookDeath mookDeathSuicidePrefab;

	public VictoryMookDeath mookDeathSatanPrefab;

	public VictoryMookDeath mookDeathRiotShieldPrefab;

	public VictoryMookDeath mookDeathDogPrefab;

	public VictoryMookDeath mookDeathBigGuyPrefab;

	public VictoryMookDeath mookDeathScoutPrefab;

	public VictoryMookDeath mookDeathGeneralPrefab;

	public VictoryMookDeath mookDeathBazookaPrefab;

	public VictoryMookDeath broDeathGenericPrefab;

	public VictoryMookDeath villagerDeathPrefab;

	public Transform victoryScreen;

	public Transform failScreen;

	public SpriteSM areaLiberatedSprite;

	public TextMesh textMesh;

	public TextMesh rescueLeftText;

	public TextMesh brotalityText;

	public TextMesh stealthText;

	public TextMesh timeText;

	public TextMesh brotalityRankText;

	public TextMesh stealthRankText;

	public TextMesh timeRankText;

	public Transform skipStateRoot;

	public TextMesh fireToSkip;

	public TextMesh[] skipStates;

	public Transform brotalityHolder;

	public Transform stealthHolder;

	public Transform mookDeathsHolder;

	public Transform timeHolder;

	public Transform rescuesLeftHolder;

	public ScalingBox brotalityBox;

	public ScalingBox stealthBox;

	public ScalingBox mookDeathsBox;

	public ScalingBox timeBox;

	public ScalingBox rescureBox;

	public SoundHolder soundHolder;

	private Vector3 bottomTargetLocal;

	private Vector3 topTargetLocal;

	private Vector3 nextUnlockTargetLocal;

	public static LevelOverScreen instance;

	private float delay;

	private float delayInterval = 0.4f;

	private float waitForVotesTimer = 10f;

	private float numberOfBrosAtMapStart;

	private float numberOfBrosTarget;

	private bool victory;

	private bool hasStartedShowingScores;

	private bool waitingForVote;

	private bool TransitionHasBegun;

	private bool hold = true;
}
