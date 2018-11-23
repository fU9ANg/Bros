// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScreen : MonoBehaviour
{
	protected void Awake()
	{
		ScoreScreen.instance = this;
		this.textMeshLocalPos = this.textMesh.transform.localPosition;
		this.textMesh.transform.localPosition += Vector3.right * 1000f;
		base.gameObject.SetActive(false);
	}

	protected void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.growing)
		{
			this.growCounter += this.t;
			float num = Mathf.Clamp(this.growCounter / this.tweenTime, 0f, 1f);
			this.bar.SetSize(1000f, 1f + this.barSize * num);
		}
		else if (this.fading)
		{
			this.fadeCounter += this.t;
			float num2 = Mathf.Clamp(this.fadeCounter / this.tweenTime, 0f, 1f);
			this.bar.SetSize(1000f, 1f + this.barSize * (1f - num2));
		}
		else
		{
			this.bar.SetSize(1000f, 1f + this.barSize);
		}
		if (this.growing)
		{
			this.particleCounter += this.t;
			if (this.particleCounter > 0.0667f)
			{
				this.particleCounter -= 0.0667f;
			}
			if (this.growing)
			{
				this.RunParticles(this.t * this.particleSpeedM);
			}
			this.particleSpeedM = Mathf.Lerp(this.particleSpeedM, 1f, this.t * 8f);
		}
		else if (this.fading)
		{
			this.RunParticles(this.t * this.particleSpeedM);
			this.particleSpeedM = Mathf.Lerp(this.particleSpeedM, 1f, this.t * 15f);
		}
		else
		{
			this.RunParticles(this.t * this.particleSpeedM);
			this.particleSpeedM = Mathf.Lerp(this.particleSpeedM, 0.1f, this.t * 15f);
		}
		if (this.textFadingIn)
		{
			this.textCounter += this.t;
			float a = Mathf.Clamp(this.textCounter * 2f / this.tweenTime, 0f, 1f);
			if (this.textCounter > this.tweenTime)
			{
				this.textFadingIn = false;
				this.textCounter = 0f;
				a = 1f;
				this.textMesh.GetComponent<Renderer>().material = this.textMaterialNormal;
			}
			this.textMesh.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, a));
			this.textMesh.transform.localPosition = Vector3.Lerp(this.textMesh.transform.localPosition, this.textMeshLocalPos, this.t * 18f);
		}
		else if (this.textFadingOut)
		{
			this.textMesh.transform.localPosition = Vector3.Lerp(this.textMesh.transform.localPosition, this.textMeshLocalPos + Vector3.right * 100f, this.t * 8f);
			this.textCounter += this.t;
			float num3 = Mathf.Clamp(this.textCounter * 2f / this.tweenTime, 0f, 1f);
			this.textMesh.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 1f - num3));
			this.raysOfLight.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 1f - num3));
		}
		else
		{
			this.textMesh.transform.localPosition = this.textMeshLocalPos;
		}
		if (this.timeCounter < this.tweenTime)
		{
			this.timeCounter += this.t;
			if (this.timeCounter >= this.tweenTime)
			{
				this.growing = false;
				this.fadeCounter = 0f;
			}
		}
		else if (this.timeCounter < this.tweenTime + this.textTime)
		{
			this.timeCounter += this.t;
			if (this.timeCounter >= this.tweenTime + this.textTime)
			{
				this.fading = true;
				if (this.textTime < 1f)
				{
					this.particleSpeedM = 1f;
				}
				this.growCounter = 0f;
				this.textMesh.GetComponent<Renderer>().material = this.textMaterialItalic;
				this.textFadingOut = true;
				this.textFadingIn = false;
			}
		}
		else if (this.timeCounter < this.tweenTime * 2f + this.textTime)
		{
			this.timeCounter += this.t;
			if (this.timeCounter >= this.tweenTime * 2f + this.textTime)
			{
				this.Disappear();
			}
		}
		if (this.disappearing)
		{
			this.disappearCounter += this.t;
			if (this.disappearCounter > 0.2f)
			{
				this.raysOfLight.ClearParticles();
				base.gameObject.SetActive(false);
			}
		}
	}

	protected void RunParticles(float t)
	{
		this.particles = this.raysOfLight.particles;
		for (int i = 0; i < this.particles.Length; i++)
		{
			Particle[] array = this.particles;
			int num = i;
			array[num].position = array[num].position + this.particles[i].velocity * t;
		}
		this.raysOfLight.particles = this.particles;
	}

	protected void AppearInternal(float time, string text, Color c, float scale, bool showScores, bool showWinnerText)
	{
		base.gameObject.SetActive(true);
		for (int i = 0; i < 4; i++)
		{
			this.scoredisplays[i].gameObject.SetActive(false);
		}
		if (showScores)
		{
			base.StartCoroutine(this.SetScores((!GameModeController.InRewardPhase()) ? 1000f : 2f));
		}
		if (showWinnerText)
		{
			if (GameModeController.InRewardPhase())
			{
				this.textMesh.transform.localPosition = new Vector3(0f, -20f, 5f);
				this.textMeshLocalPos = this.textMesh.transform.localPosition;
			}
			base.StartCoroutine(this.ShowSlideInText((!showScores) ? 0f : 2.5f, time, text, c, scale));
		}
		else
		{
			this.textMesh.gameObject.SetActive(false);
		}
	}

	protected IEnumerator ShowSlideInText(float delay, float time, string text, Color c, float scale)
	{
		this.textMesh.gameObject.SetActive(false);
		yield return new WaitForSeconds(delay);
		this.textMesh.gameObject.SetActive(true);
		this.textTime = time;
		this.color = c;
		this.textMesh.text = text;
		this.raysOfLight.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 1f));
		this.textMesh.GetComponent<Renderer>().material = this.textMaterialItalic;
		this.textMesh.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0f));
		this.bar.SetColor(this.color);
		float count = 6f + this.textTime * 5f;
		float dist = (300f + 330f * this.textTime) / count;
		int i = 0;
		while ((float)i < 1f + this.textTime * 8f)
		{
			this.particleCount++;
			float size = 1.5f + UnityEngine.Random.value * 1f;
			Vector3 pos = new Vector3(-60f - this.textTime * 100f - UnityEngine.Random.value * 50f - (10f + 40f * this.textTime) * size - dist * (float)i, (float)(8 - this.particleCount % 5 * 4) - 1.5f + UnityEngine.Random.value * 3f, 2f);
			this.raysOfLight.Emit(pos, Vector3.right * 600f * size, 1f, 2f + UnityEngine.Random.value * this.tweenTime * 0.5f, new Color(this.color.r * 1.2f + 0.1f, this.color.g * 1.2f + 0.1f, this.color.b * 1.2f + 0.1f, 0.3f));
			i++;
		}
		this.growing = true;
		this.fading = false;
		this.textMesh.transform.localScale = Vector3.one * scale;
		this.timeCounter = 0f;
		this.textCounter = 0f;
		this.textFadingIn = true;
		this.textFadingOut = false;
		this.particleSpeedM = 0.5f;
		this.disappearing = false;
		this.tweenTime = Mathf.Min(0.5f, this.textTime / 2f);
		this.textMeshLocalPos = this.textMesh.transform.localPosition;
		this.textMesh.transform.localPosition += Vector3.right * -300f;
		yield return null;
		yield break;
	}

	public static void Appear(float time, string text, bool showScores, bool showWinnerText, int winningPlayer)
	{
		if (ScoreScreen.instance != null)
		{
			ScoreScreen.instance.lastPlayerToWin = winningPlayer;
			ScoreScreen.instance.AppearInternal(time, text, Color.white, 1.25f, showScores, showWinnerText);
		}
	}

	public IEnumerator SetScores(float timeAppearing)
	{
		for (int i = 0; i < 4; i++)
		{
			if (HeroController.IsPlaying(i))
			{
				base.StartCoroutine(this.ShowPlayerScore(i));
				timeAppearing -= 0.3f;
				yield return new WaitForSeconds(0.3f);
			}
		}
		yield return new WaitForSeconds(timeAppearing);
		for (int j = 0; j < 4; j++)
		{
			if (this.scoredisplays[j].gameObject.activeSelf)
			{
				this.scoredisplays[j].SlideAway();
				yield return new WaitForSeconds(0.2f);
			}
		}
		yield break;
	}

	public IEnumerator ShowPlayerScore(int player)
	{
		this.scoredisplays[player].gameObject.SetActive(HeroController.IsPlaying(player));
		HeroController.SwitchAvatarMaterial(this.scoredisplays[player].Avatar, GameModeController.deathmatchHero[player]);
		this.scoredisplays[player].SetColor(HeroController.GetHeroColor(player));
		this.scoredisplays[player].avatarBox.Activate();
		yield return new WaitForSeconds(0.1f);
		this.scoredisplays[player].textBox.Activate();
		yield return new WaitForSeconds(0.1f);
		this.scoredisplays[player].badgeBox.Activate();
		yield return new WaitForSeconds(0.1f);
		while (this.scoredisplays[player].avatarBox.IsBusyGrowing())
		{
			yield return null;
		}
		this.scoredisplays[player].avatarBox.gameObject.SetActive(true);
		while (this.scoredisplays[player].textBox.IsBusyGrowing())
		{
			yield return null;
		}
		this.scoredisplays[player].textBox.gameObject.SetActive(true);
		while (this.scoredisplays[player].badgeBox.IsBusyGrowing())
		{
			yield return null;
		}
		this.scoredisplays[player].badgeBox.gameObject.SetActive(true);
		int score = GameModeController.GetPlayerRoundWins(player);
		if (player == this.lastPlayerToWin)
		{
			score--;
		}
		for (int i = 0; i < score; i++)
		{
			SpriteSM badge = UnityEngine.Object.Instantiate(this.winBadgePrefab) as SpriteSM;
			badge.gameObject.layer = LayerMask.NameToLayer("UI");
			badge.transform.position = this.scoredisplays[player].badgeHolder.transform.position + Vector3.right * ((float)i * badge.width * 0.5f) - Vector3.forward * (float)i;
			badge.transform.parent = this.scoredisplays[player].badgeHolder.transform;
		}
		yield return new WaitForSeconds(1f);
		if (player == this.lastPlayerToWin)
		{
			Puff explosion = UnityEngine.Object.Instantiate(this.explosionPrefab) as Puff;
			explosion.gameObject.layer = LayerMask.NameToLayer("UI");
			explosion.transform.position = this.scoredisplays[player].badgeHolder.transform.position + Vector3.right * ((float)score * this.winBadgePrefab.width * 0.5f) - Vector3.forward * (float)score * 2f;
			SpriteSM badge2 = UnityEngine.Object.Instantiate(this.winBadgePrefab) as SpriteSM;
			badge2.gameObject.layer = LayerMask.NameToLayer("UI");
			badge2.transform.position = this.scoredisplays[player].badgeHolder.transform.position + Vector3.right * ((float)score * badge2.width * 0.5f) - Vector3.forward * (float)score;
			badge2.transform.parent = this.scoredisplays[player].badgeHolder.transform;
			Sound.GetInstance().PlaySoundEffect(this.soundHolder.attackSounds, 0.5f);
			ShakeM sm = new ShakeM();
			sm.m = 1f;
			base.StartCoroutine(this.ShakeTransform(this.scoredisplays[player].transform, sm));
		}
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
		while (shakeM.m > 0f)
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

	protected void Disappear()
	{
		this.disappearing = true;
		this.disappearCounter = 0.25f;
	}

	protected float growCounter;

	protected float timeCounter;

	protected float fadeCounter;

	protected float textTime = 15f;

	protected float tweenTime = 0.5f;

	protected bool growing;

	protected bool fading;

	protected float textCounter;

	protected bool textFadingIn;

	protected bool textFadingOut;

	public Material textMaterialNormal;

	public Material textMaterialItalic;

	protected float particleCounter;

	protected bool disappearing;

	protected float particleSpeedM = 1f;

	protected float disappearCounter;

	private int lastPlayerToWin;

	public float barSize = 19f;

	protected static ScoreScreen instance;

	public TextMesh textMesh;

	private Vector3 textMeshLocalPos;

	public PlayerScoreDisplay[] scoredisplays;

	public SpriteSM winBadgePrefab;

	public Puff explosionPrefab;

	public SoundHolder soundHolder;

	public SpriteSM bar;

	public ParticleEmitter raysOfLight;

	protected Color color;

	protected int particleCount;

	protected float t = 0.01f;

	protected List<Particle> particlesList;

	protected Particle[] particles;
}
