// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class FlexController : MonoBehaviour
{
	public int playerCount
	{
		get
		{
			return this.PlayerCount;
		}
	}

	public void PlayFlexSound(int playerNum)
	{
		int num = this.effortSoundCoolDowns.RandomIndex<float>();
		num = playerNum % this.effortSounds.Length;
		if (this.effortSoundCoolDowns[num] <= 0f)
		{
			this.effortSoundCoolDowns[num] = UnityEngine.Random.Range(0.3f, 0.7f);
			CutsceneSound.PlaySoundEffectAt(this.effortSounds[num], 0.5f + UnityEngine.Random.value * 0.2f, base.transform.position, 0.9f + UnityEngine.Random.value * 0.1f);
		}
	}

	private void Awake()
	{
		FlexController.instance = this;
		this.PlayerCount = HeroController.GetPlayersPlayingCount();
		bool flag = false;
		if (this.playerCount == 0)
		{
			flag = true;
			this.PlayerCount = 4;
		}
		for (int i = 0; i < this.PlayerCount; i++)
		{
			FlexBackdrop flexBackdrop = UnityEngine.Object.Instantiate(this.flexTextsPrefab) as FlexBackdrop;
			flexBackdrop.transform.parent = this.flexTextsPrefab.transform.parent;
			flexBackdrop.transform.position = this.flexTextsPrefab.transform.position;
			flexBackdrop.Setup(i);
		}
		for (int j = 0; j < this.PlayerCount; j++)
		{
			FlexBackdrop flexBackdrop2 = UnityEngine.Object.Instantiate(this.lightFlickersPerfab) as FlexBackdrop;
			flexBackdrop2.gameObject.SetActive(true);
			flexBackdrop2.transform.parent = this.lightFlickersPerfab.transform.parent;
			flexBackdrop2.transform.position = this.lightFlickersPerfab.transform.position;
			flexBackdrop2.Setup(j);
		}
		for (int k = 0; k < this.playerCount; k++)
		{
			if (HeroController.playersPlaying[k] || flag)
			{
				PlayerFlex playerFlex = UnityEngine.Object.Instantiate(this.avatarPrefab) as PlayerFlex;
				playerFlex.transform.parent = base.transform;
				playerFlex.transform.position = this.avatarPrefab.transform.position;
				playerFlex.Setup(k);
				playerFlex.transform.localScale -= Vector3.one * (float)(this.playerCount - 1) / 3f * 0.25f;
				float num = (this.cam.ScreenToWorldPoint(Vector3.right * (float)Screen.width) - this.cam.transform.position).x * (1f / (float)this.PlayerCount) * 2f;
				playerFlex.transform.SetX(num * (float)k - num * (0.5f * (float)(this.playerCount - 1)));
				playerFlex.playerNum = k;
			}
		}
		UnityEngine.Object.Destroy(this.avatarPrefab.gameObject);
		FlexController.instance.DimBackground.SetActive(false);
		FlexController.instance.particles.gameObject.SetActive(false);
		UnityEngine.Object.Destroy(this.flexTextsPrefab.gameObject);
		UnityEngine.Object.Destroy(this.lightFlickersPerfab.gameObject);
		this.effortSoundCoolDowns = new float[this.effortSounds.Length];
		this.effortSoundCoolDowns.Initialize();
		this.ropeStrainSoundsCooldowns = new float[this.ropeStrainSounds.Length];
		this.ropeStrainSoundsCooldowns.Initialize();
		this.chainStrainSoundsCooldowns = new float[this.ChainStrainSounds.Length];
		this.chainStrainSoundsCooldowns.Initialize();
	}

	public void SwitchOnLights()
	{
		if (!this.lightsOn)
		{
			MonoBehaviour.print("switch on lights");
			CutsceneSound.PlaySoundEffectAt(this.lightSwitchAudio, 0.5f + UnityEngine.Random.value * 0.2f, base.transform.position, 0.9f + UnityEngine.Random.value * 0.2f);
			CutsceneSound.PlaySoundEffectAt(this.lightsTurningOnAudio, 0.5f + UnityEngine.Random.value * 0.2f, base.transform.position, 0.9f + UnityEngine.Random.value * 0.2f);
			this.lightsOn = true;
			base.StartCoroutine(this.AnnounceFlex());
		}
	}

	private IEnumerator AnnounceFlex()
	{
		CutsceneSound.PlaySoundEffectAt(this.announcer.start3, 0.5f + UnityEngine.Random.value * 0.2f, FlexController.instance.transform.position, 0.9f + UnityEngine.Random.value * 0.2f);
		yield return new WaitForSeconds(0.8f);
		CutsceneSound.PlaySoundEffectAt(this.announcer.start2, 0.5f + UnityEngine.Random.value * 0.2f, FlexController.instance.transform.position, 0.9f + UnityEngine.Random.value * 0.2f);
		yield return new WaitForSeconds(0.6f);
		CutsceneSound.PlaySoundEffectAt(this.announcer.start1, 0.5f + UnityEngine.Random.value * 0.2f, FlexController.instance.transform.position, 0.9f + UnityEngine.Random.value * 0.2f);
		yield return new WaitForSeconds(0.6f);
		this.FlexAnnounced = true;
		CutsceneSound.PlaySoundEffectAt(FlexController.instance.flexAnouncement, 0.5f + UnityEngine.Random.value * 0.2f, FlexController.instance.transform.position, 0.9f + UnityEngine.Random.value * 0.2f);
		yield break;
	}

	public static void PlayRopeSnapSound()
	{
		CutsceneSound.PlaySoundEffectAt(FlexController.instance.ropeSnapSounds.RandomElement<AudioClip>(), 0.4f + UnityEngine.Random.value * 0.2f, FlexController.instance.transform.position, 0.9f + UnityEngine.Random.value * 0.2f);
	}

	public static void PlayChainSnapSound()
	{
		CutsceneSound.PlaySoundEffectAt(FlexController.instance.chainSnapSounds.RandomElement<AudioClip>(), 0.4f + UnityEngine.Random.value * 0.2f, FlexController.instance.transform.position, 0.9f + UnityEngine.Random.value * 0.2f);
	}

	public void PlayRopeStainSound()
	{
		for (int i = 0; i < 20; i++)
		{
			int num = this.ropeStrainSoundsCooldowns.RandomIndex<float>();
			if (this.ropeStrainSoundsCooldowns[num] <= 0f)
			{
				this.ropeStrainSoundsCooldowns[num] = UnityEngine.Random.Range(0.4f, 0.6f);
				CutsceneSound.PlaySoundEffectAt(this.ropeStrainSounds[num], 0.2f + UnityEngine.Random.value * 0.2f, base.transform.position, 0.9f + UnityEngine.Random.value * 0.2f);
				break;
			}
		}
	}

	public void PlayChainStrainSound()
	{
		for (int i = 0; i < 20; i++)
		{
			int num = this.chainStrainSoundsCooldowns.RandomIndex<float>();
			if (this.chainStrainSoundsCooldowns[num] <= 0f)
			{
				this.chainStrainSoundsCooldowns[num] = UnityEngine.Random.Range(0.4f, 0.6f);
				CutsceneSound.PlaySoundEffectAt(this.ChainStrainSounds[num], 0.2f + UnityEngine.Random.value * 0.2f, base.transform.position, 0.9f + UnityEngine.Random.value * 0.2f);
				break;
			}
		}
	}

	private void Update()
	{
		for (int i = 0; i < this.effortSoundCoolDowns.Length; i++)
		{
			this.effortSoundCoolDowns[i] -= Time.deltaTime;
		}
		for (int j = 0; j < this.effortSoundCoolDowns.Length; j++)
		{
			this.ropeStrainSoundsCooldowns[j] -= Time.deltaTime;
		}
		for (int k = 0; k < this.chainStrainSoundsCooldowns.Length; k++)
		{
			this.chainStrainSoundsCooldowns[k] -= Time.deltaTime;
		}
	}

	public static void BreakFree(PlayerFlex WinningPlayer)
	{
		CutsceneSound.PlaySoundEffectAt(FlexController.instance.explosion, 0.5f + UnityEngine.Random.value * 0.2f, FlexController.instance.transform.position, 0.9f + UnityEngine.Random.value * 0.2f);
		FlexController.instance.winningPlayer = WinningPlayer;
		FlexController.instance.playerHasBrokenFree = true;
		MonoBehaviour.print(FlexController.instance);
		MonoBehaviour.print(FlexController.instance.DimBackground);
		CutsceneSound.PlayVictorySting();
		FlexController.instance.DimBackground.SetActive(true);
		FlexController.instance.Invoke("FlexCompleteAnnouncement", 2f);
	}

	private void FlexCompleteAnnouncement()
	{
		if (!this.completeAnnounced)
		{
			this.completeAnnounced = true;
			CutsceneSound.PlaySoundEffectAt(FlexController.instance.flexCompleteAnnouncement, 0.5f + UnityEngine.Random.value * 0.2f, FlexController.instance.transform.position, 0.9f + UnityEngine.Random.value * 0.2f);
		}
	}

	public void ActivateBackground(Color color)
	{
		this.particles.gameObject.SetActive(true);
		this.particles.SetParticleColor(color - Color.white * 0.4f);
		this.WinningBackground.GetComponent<SpriteSM>().SetColor(color);
	}

	public GameObject DimBackground;

	public GameObject WinningBackground;

	public PlayerFlex avatarPrefab;

	private int PlayerCount = 1;

	public Shake cutSceneCamShake;

	public bool playerHasBrokenFree;

	public PlayerFlex winningPlayer;

	public ParticleSystem particles;

	public static FlexController instance;

	public FlexBackdrop flexTextsPrefab;

	public FlexBackdrop lightFlickersPerfab;

	public Camera cam;

	public SoundHolderAnnouncer announcer;

	public AudioClip flexAnouncement;

	public AudioClip flexCompleteAnnouncement;

	public AudioClip lightSwitchAudio;

	public AudioClip lightsTurningOnAudio;

	public AudioClip[] effortSounds;

	private float[] effortSoundCoolDowns;

	public AudioClip[] ropeSnapSounds;

	public AudioClip[] chainSnapSounds;

	public AudioClip[] ropeStrainSounds;

	public AudioClip[] ChainStrainSounds;

	private float[] ropeStrainSoundsCooldowns;

	private float[] chainStrainSoundsCooldowns;

	public AudioClip pecShineSound;

	public AudioClip whipSound;

	public AudioClip explosion;

	public bool lightsOn;

	public bool FlexAnnounced;

	private bool completeAnnounced;
}
