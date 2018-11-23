// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CutsceneSound : MonoBehaviour
{
	private void Awake()
	{
		CutsceneSound.instance = this;
		this.music = (base.gameObject.AddComponent(typeof(AudioSource)) as AudioSource);
		this.music.clip = this.musicClip;
		this.music.volume = Mathf.Clamp(PlayerOptions.Instance.musicVolume * this.musicVolumeM, 0f, 0.5f);
		this.music.rolloffMode = AudioRolloffMode.Linear;
		this.music.maxDistance = 250f;
		this.music.loop = true;
		this.music.playOnAwake = false;
		this.music.Stop();
		this.effects = (base.gameObject.AddComponent(typeof(AudioSource)) as AudioSource);
		this.effects.rolloffMode = AudioRolloffMode.Linear;
		this.effects.minDistance = 250f;
		this.effects.maxDistance = 250f;
		this.effects.loop = false;
		this.effects.playOnAwake = false;
	}

	private void Update()
	{
		if (this.RandomlyPlayTheseSoundEffects.Length > 0 && (this.randomSfxDelay -= Time.deltaTime) < 0f)
		{
			CutsceneSound.PlaySoundEffectAt(this.RandomlyPlayTheseSoundEffects, UnityEngine.Random.Range(0.4f, 0.8f), base.transform.position + UnityEngine.Random.insideUnitSphere * 50f);
			this.randomSfxDelay = UnityEngine.Random.value * 0.3f * this.randomSfxDelayIncrease;
			this.randomSfxDelayIncrease *= 1.2f;
		}
	}

	protected void PlayMusic()
	{
		MonoBehaviour.print("fucking play wtf");
		this.music.Play();
	}

	private void Start()
	{
		if (this.music.clip != null)
		{
			this.PlayMusic();
		}
	}

	public static void PlaySoundEffectAt(AudioClip[] clips, float v, Vector3 pos, float pitch)
	{
		if (clips.Length == 0)
		{
			return;
		}
		CutsceneSound.PlaySoundEffectAt(clips[UnityEngine.Random.Range(0, clips.Length)], v, pos, pitch);
	}

	public static void PlaySoundEffectAt(AudioClip[] clips, float v, Vector3 pos)
	{
		if (clips.Length == 0)
		{
			return;
		}
		CutsceneSound.PlaySoundEffectAt(clips[UnityEngine.Random.Range(0, clips.Length)], v, pos, 1f);
	}

	public void PlaySoundEffectAt(AudioClip clip, float v, Vector3 pos)
	{
		CutsceneSound.PlaySoundEffectAt(clip, v, pos, 1f);
	}

	public static void PlaySoundEffectAt(AudioClip clip, float v, Vector3 pos, float pitch)
	{
		if (clip == null)
		{
			return;
		}
		if (CutsceneSound.instance != null)
		{
			CutsceneSound.instance.PlayAudioClip(clip, pos, v, pitch);
		}
	}

	public AudioSource PlayAudioClip(AudioClip clip, Vector3 position, float volume)
	{
		return this.PlayAudioClip(clip, position, volume, 1f);
	}

	public AudioSource PlayAudioClip(AudioClip clip, Vector3 position, float volume, float pitch)
	{
		GameObject gameObject = new GameObject("One shot audio");
		gameObject.transform.position = position;
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.volume = volume;
		audioSource.pitch = pitch;
		audioSource.Play();
		audioSource.rolloffMode = AudioRolloffMode.Linear;
		audioSource.maxDistance = 286f;
		audioSource.minDistance = 64f;
		audioSource.dopplerLevel = 0.1f;
		UnityEngine.Object.Destroy(gameObject, clip.length);
		return audioSource;
	}

	public void PlaySoundEffect(AudioClip[] clips, float v)
	{
		if (clips.Length == 0)
		{
			return;
		}
		this.PlaySoundEffect(clips[UnityEngine.Random.Range(0, clips.Length)], v);
	}

	public void PlaySoundEffect(AudioClip clip, float v)
	{
		if (clip == null)
		{
			return;
		}
		this.effects.PlayOneShot(clip, v * Sound.volumeEffects);
	}

	public static void PlayVictorySting()
	{
		MonoBehaviour.print("Play victory sting");
		CutsceneSound.instance.music.Stop();
		CutsceneSound.instance.music.clip = CutsceneSound.instance.victorySting;
		CutsceneSound.instance.music.Play();
	}

	internal static void PlayPecshineSound()
	{
		CutsceneSound.instance.PlaySoundEffect(CutsceneSound.instance.pecShineSound, 0.6f);
	}

	internal static void PlayWhipSound()
	{
		CutsceneSound.instance.PlaySoundEffect(CutsceneSound.instance.whipSound, 0.6f);
	}

	public AudioSource music;

	public AudioSource effects;

	protected static CutsceneSound instance;

	public AudioClip musicClip;

	public AudioClip victorySting;

	public float musicVolumeM = 1f;

	public AudioClip[] pecShineSound;

	public AudioClip[] whipSound;

	public AudioClip[] RandomlyPlayTheseSoundEffects;

	private float randomSfxDelay = 0.1f;

	private float randomSfxDelayIncrease = 1f;
}
