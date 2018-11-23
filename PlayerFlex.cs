// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlex : MonoBehaviour
{
	private void Awake()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		this.shake = base.GetComponentInChildren<Shake>();
		this.BarMax = this.forceBar.transform.localScale.x;
	}

	public void Setup(int PlayerNum)
	{
		this.playerNum = PlayerNum;
		Player player = HeroController.players[this.playerNum];
		if (player != null && player.character != null)
		{
			this.hero = player.character.heroType;
			MonoBehaviour.print("Bro found " + this.hero);
		}
		if (this.hero == HeroType.None)
		{
			PlayerFlex[] array = UnityEngine.Object.FindObjectsOfType<PlayerFlex>();
			List<HeroType> list = new List<HeroType>();
			foreach (PlayerFlex playerFlex in array)
			{
				list.Add(playerFlex.hero);
			}
			if (!list.Contains(HeroType.BroneyRoss))
			{
				this.hero = HeroType.BroneyRoss;
			}
			else if (!list.Contains(HeroType.LeeBroxmas))
			{
				this.hero = HeroType.LeeBroxmas;
			}
			else if (!list.Contains(HeroType.HaleTheBro))
			{
				this.hero = HeroType.HaleTheBro;
			}
			else if (!list.Contains(HeroType.BronnarJensen))
			{
				this.hero = HeroType.BronnarJensen;
			}
		}
		switch (this.hero)
		{
		case HeroType.BroneyRoss:
			base.GetComponent<Renderer>().material = this.Stallone;
			break;
		case HeroType.LeeBroxmas:
			base.GetComponent<Renderer>().material = this.Statham;
			break;
		case HeroType.BronnarJensen:
			base.GetComponent<Renderer>().material = this.Lungren;
			break;
		case HeroType.HaleTheBro:
			base.GetComponent<Renderer>().material = this.Crews;
			break;
		}
	}

	private void Start()
	{
		this.ButtonMashInstance = (UnityEngine.Object.Instantiate(this.ButtonMashPrefab) as ButtonMash);
		this.ButtonMashInstance.transform.parent = base.transform;
		this.ButtonMashInstance.transform.localPosition = this.ButtonMashPrefab.transform.position;
		this.ButtonMashInstance.transform.SetLocalX(45f);
		this.ButtonMashInstance.Setup(this.playerNum);
		this.ButtonMashInstance.gameObject.SetActive(false);
		this.ButtonMashInstance.transform.parent = null;
	}

	private void Update()
	{
		if (!FlexController.instance.FlexAnnounced)
		{
			return;
		}
		if (FlexController.instance.winningPlayer != null)
		{
			this.ButtonMashInstance.gameObject.SetActive(false);
			this.ButtonMashInstance.gameObject.SetActive(false);
			if (FlexController.instance.winningPlayer != this)
			{
				this.shake.frequencyX = 0f;
				this.shake.frequencyY = 0f;
				this.shake.amplitudeX = 0f;
				this.shake.amplitudeY = 0f;
				return;
			}
		}
		else
		{
			this.ButtonMashInstance.gameObject.SetActive(true);
		}
		this.timerBar.localScale = this.lastPress * this.BarMax * Vector3.right + Vector3.up * 5f;
		this.forceBar.localScale = this.force * this.BarMax * Vector3.right + Vector3.up * 5f;
		this.shake.frequencyX = this.force * 90f;
		this.shake.frequencyY = this.shake.frequencyX * 0.79f;
		this.shake.amplitudeX = this.force * 2.5f;
		this.shake.amplitudeY = this.force * 1f;
		if (Input.GetKey(KeyCode.F6))
		{
			Application.LoadLevel(Application.loadedLevelName);
		}
		if (Input.GetKey(KeyCode.F1))
		{
			if (Time.timeScale < 1f)
			{
				Time.timeScale = 1f;
			}
			else
			{
				Time.timeScale = 0.1f;
			}
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		int controllerNum = HeroController.playerControllerIDs[this.playerNum];
		InputReader.GetInputIgnoreBlock(controllerNum, ref flag, ref flag, ref flag, ref flag, ref flag2, ref flag5, ref flag4, ref flag3);
		bool flag6 = flag2 || flag3 || flag5 || flag4;
		if (Application.isEditor)
		{
			switch (this.playerNum)
			{
			case 0:
				flag6 = (flag6 || Input.GetKey(KeyCode.V));
				break;
			case 1:
				flag6 = (flag6 || Input.GetKey(KeyCode.B));
				break;
			case 2:
				flag6 = (flag6 || Input.GetKey(KeyCode.N));
				break;
			case 3:
				flag6 = (flag6 || Input.GetKey(KeyCode.M));
				break;
			}
		}
		if (flag6 && !this.wasKeyDown)
		{
			this.lastPress = 0.5f;
			this.timer = 0.2f;
			if (!this.breakingRopes)
			{
				float num = 0.3f;
				num *= 1f - this.force;
				this.force += num;
				this.PlayStrainSound();
				FlexController.instance.PlayFlexSound(this.playerNum);
			}
			this.ButtonMashInstance.Mash();
		}
		this.wasKeyDown = flag6;
		this.lastPress -= this.drainRate * Time.deltaTime;
		this.lastPress = Mathf.Max(0f, this.lastPress);
		this.force = Mathf.Clamp01(this.force);
		float num2 = this.lastPress * 1f;
		if (this.lastPress <= 0f)
		{
			this.force = 0f;
		}
		if (this.state != PlayerFlex.BreakState.AllRopesBroken)
		{
			this.cooldown -= Time.deltaTime;
			this.timer -= Time.deltaTime;
			if (this.lastPress > 0f)
			{
				if (this.cooldown <= 0f)
				{
					this.cooldown = this.cooldownDuration;
				}
				if (this.cooldown >= this.cooldownDuration / 2f)
				{
					this.sprite.SetLowerLeftPixel_X(this.sprite.width * 1f);
				}
				else
				{
					this.sprite.SetLowerLeftPixel_X(0f);
				}
			}
			else
			{
				this.sprite.SetLowerLeftPixel_X(0f);
			}
			if (this.force >= 0.95f)
			{
				base.StartCoroutine(this.BreakRopes());
			}
		}
	}

	private void PlayStrainSound()
	{
		if (this.hero == HeroType.HaleTheBro)
		{
			FlexController.instance.PlayChainStrainSound();
		}
		else
		{
			FlexController.instance.PlayRopeStainSound();
		}
	}

	private void CreateRopeGibs(float force)
	{
		GibHolder original = this.ropeGibsHolder;
		if (this.hero == HeroType.HaleTheBro)
		{
			original = this.chainGibsHolder;
		}
		GibHolder gibHolder = UnityEngine.Object.Instantiate(original) as GibHolder;
		gibHolder.transform.position = base.transform.position;
		float num = UnityEngine.Random.Range(0.8f, 1.2f);
		float num2 = UnityEngine.Random.Range(0.8f, 1.2f);
		num *= force;
		EffectsController.CreateGibs(gibHolder, gibHolder.transform.position.x, gibHolder.transform.position.y, 30f * num, 15f * num2, 0f, 0f);
	}

	private void PlaySnapSound()
	{
		if (this.hero == HeroType.HaleTheBro)
		{
			FlexController.PlayChainSnapSound();
		}
		else
		{
			FlexController.PlayRopeSnapSound();
		}
	}

	private IEnumerator BreakRopes()
	{
		this.PlayStrainSound();
		if (this.breakingRopes)
		{
			yield break;
		}
		this.breakingRopes = true;
		float playbackSpeed = 0.06666667f;
		int i = (int)this.state;
		i++;
		i = Mathf.Clamp(i, 0, 4);
		this.state = (PlayerFlex.BreakState)i;
		if (this.state != PlayerFlex.BreakState.AllRopesBroken)
		{
			this.sprite.SetLowerLeftPixel_X(this.sprite.width * 0f);
			yield return new WaitForSeconds(0.5f);
			this.PlaySnapSound();
			this.lastPress = 0f;
			this.force = 0f;
			this.CreateRopeGibs(1f);
			this.CreateRopeGibs(2f);
			this.sprite.SetLowerLeftPixel_X(this.sprite.width * 2f);
			yield return new WaitForSeconds(playbackSpeed);
			this.sprite.SetLowerLeftPixel_X(this.sprite.width * 3f);
			yield return new WaitForSeconds(playbackSpeed);
			this.sprite.SetLowerLeftPixel_X(this.sprite.width * 4f);
			yield return new WaitForSeconds(playbackSpeed);
			this.sprite.SetLowerLeftPixel_X(this.sprite.width * 5f);
			yield return new WaitForSeconds(playbackSpeed);
		}
		else
		{
			this.sprite.SetLowerLeftPixel_X(this.sprite.width * 4f);
			yield return new WaitForSeconds(1f);
			this.lastPress = 0f;
			this.force = 0f;
		}
		this.sprite.SetLowerLeftPixel_Y(this.sprite.height * (float)(i + 1));
		if (this.state != PlayerFlex.BreakState.AllRopesBroken)
		{
			this.sprite.SetLowerLeftPixel_X(this.sprite.width);
		}
		else
		{
			this.PlaySnapSound();
			this.CreateRopeGibs(7f);
			this.CreateRopeGibs(6f);
			this.CreateRopeGibs(5f);
			this.CreateRopeGibs(4f);
			this.CreateRopeGibs(3f);
			this.CreateRopeGibs(2f);
			this.CreateRopeGibs(1f);
			this.sprite.SetLowerLeftPixel_X(this.sprite.width * 3f);
			yield return new WaitForSeconds(playbackSpeed);
			this.sprite.SetLowerLeftPixel_X(this.sprite.width * 4f);
			yield return new WaitForSeconds(playbackSpeed);
			FlexController.instance.cutSceneCamShake.AddShake2(15f, 5f, 60f);
			FlexController.BreakFree(this);
			base.transform.localPosition -= Vector3.forward * 1f;
			base.StartCoroutine(this.CratePeckShineRoutine());
			this.sprite.SetLowerLeftPixel_X(this.sprite.width * 5f);
			yield return new WaitForSeconds(1.5f);
			Vector3 offset = Vector3.zero;
			float lerp = 0f;
			float originalX = base.transform.localPosition.x;
			int dist = -800;
			CutsceneSound.PlayWhipSound();
			while (lerp < 1f)
			{
				lerp += Time.deltaTime * 5f;
				base.transform.localPosition -= offset;
				offset = Vector3.right * (float)dist * lerp;
				base.transform.localPosition += offset;
				yield return null;
			}
			this.sprite.SetLowerLeftPixel(0f, this.sprite.height * 5f);
			base.transform.localPosition -= Vector3.forward * 50f;
			base.transform.position -= Vector3.up * 30f;
			if (FlexController.instance.playerCount == 2)
			{
				base.transform.localScale *= 1.1f;
			}
			if (FlexController.instance.playerCount == 3)
			{
				base.transform.localScale *= 1.15f;
			}
			if (FlexController.instance.playerCount == 4)
			{
				base.transform.localScale *= 1.6f;
			}
			bool camHasShaken = false;
			yield return new WaitForSeconds(0.4f);
			base.StartCoroutine(this.NotifyComplete());
			lerp = base.transform.localPosition.x;
			CutsceneSound.PlayWhipSound();
			while (lerp != 0f)
			{
				lerp = Mathf.Lerp(lerp, originalX, Time.deltaTime * 12f);
				base.transform.SetLocalX(lerp);
				if (lerp / (float)dist < 0.5f && !camHasShaken)
				{
					camHasShaken = true;
					FlexController.instance.cutSceneCamShake.AddShake2(5f, 2f, 30f);
					FlexController.instance.ActivateBackground(HeroController.GetHeroColor(this.playerNum));
				}
				yield return null;
			}
		}
		this.breakingRopes = false;
		yield break;
	}

	private IEnumerator NotifyComplete()
	{
		base.gameObject.GetComponentInHeirarchy<Cutscene>().gameObject.AddComponent<SkippableCutsceneControl>().delayBeforeSkippable = 2f;
		yield return new WaitForSeconds(5f);
		base.gameObject.GetComponentInHeirarchy<Cutscene>().isFinished = true;
		yield break;
	}

	private IEnumerator CratePeckShineRoutine()
	{
		yield return new WaitForSeconds(0.3f);
		int randomindex = UnityEngine.Random.Range(0, this.peckShinePoints.Count);
		Vector3 pos = this.peckShinePoints[randomindex].transform.position;
		this.peckShinePoints.RemoveAt(randomindex);
		GameObject peckShine = UnityEngine.Object.Instantiate(this.peckShinePrefab, pos, Quaternion.identity) as GameObject;
		CutsceneSound.PlayPecshineSound();
		yield return new WaitForSeconds(0.4f);
		randomindex = UnityEngine.Random.Range(0, this.peckShinePoints.Count);
		pos = this.peckShinePoints[randomindex].transform.position;
		this.peckShinePoints.RemoveAt(randomindex);
		peckShine = (UnityEngine.Object.Instantiate(this.peckShinePrefab, pos, Quaternion.identity) as GameObject);
		yield return new WaitForSeconds(0.2f);
		yield break;
	}

	public Material Stallone;

	public Material Statham;

	public Material Lungren;

	public Material Crews;

	[HideInInspector]
	public SpriteSM sprite;

	private PlayerFlex.BreakState state;

	private float force;

	private float lastPress;

	private float drainRate = 1f;

	public Transform timerBar;

	public Transform forceBar;

	private float BarMax;

	private float cooldown;

	private float cooldownDuration = 0.2f;

	private float timer;

	private bool breakingRopes;

	public GibHolder ropeGibsHolder;

	public GibHolder chainGibsHolder;

	private Shake shake;

	public int playerNum;

	public List<Transform> peckShinePoints = new List<Transform>();

	public GameObject peckShinePrefab;

	public ButtonMash ButtonMashPrefab;

	private ButtonMash ButtonMashInstance;

	private bool wasKeyDown;

	public HeroType hero = HeroType.None;

	private enum BreakState
	{
		NoRopesBrokes,
		OneRopeBroken,
		TwoRopesBroken,
		AllRopesBroken
	}
}
